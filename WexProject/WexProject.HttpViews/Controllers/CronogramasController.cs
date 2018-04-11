using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.HttpViews.Libs.ActionResults;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.HttpViews.Controllers
{
	public class CronogramasController : Controller
	{
		#region Recursos

		//
		// GET: /Cronogramas/
		[HttpGet]
		public ActionResult Index( Guid? id, string nomeCronograma = null )
		{
			if(id.HasValue)
			{
				return RetornarActionResult( CronogramaBo.ConsultarCronogramaPorOidDto( id.Value ), JsonRequestBehavior.AllowGet );
			}

			if(!string.IsNullOrWhiteSpace( nomeCronograma ))
			{
				using(WexDb contexto = ContextFactoryManager.CriarWexDb())
				{
					return RetornarActionResult( CronogramaBo.ConsultarCronogramaPorNomeDto( contexto, nomeCronograma ), JsonRequestBehavior.AllowGet );
				}
			}
			return RetornarActionResult( CronogramaBo.ConsultarCronogramasDto(), JsonRequestBehavior.AllowGet );
		}

		// PUT: /Cronogramas/
		[HttpPut]
		[ActionName( "Index" )]
		public ActionResult EditarCronograma( CronogramaDto cronograma )
		{
            return Json( CronogramaBo.EditarCronograma( cronograma ) );
		}

		// POST: /Cronogramas/
		[HttpPost]
		[ActionName( "Index" )]
		public ActionResult NovoCronograma()
		{
			return Json( CronogramaBo.DtoFactory( CronogramaBo.CriarCronogramaPadrao() ) );
		}

		// GET: /Cronogramas/Tarefas/{id do cronograma}
		[HttpGet, ActionName( "Tarefas" )]
		public ActionResult ListarTarefas( Guid? id, Guid? oidCronogramaTarefa )
		{
			if(id.HasValue)
			{
				return Json( CronogramaTarefaBo.ConsultarCronogramaTarefasPorOidCronogramaDto( id.Value ), JsonRequestBehavior.AllowGet );
			}

			if(oidCronogramaTarefa.HasValue)
			{
				return RetornarActionResult( CronogramaTarefaBo.ConsultarCronogramaTarefaPorOidDto( oidCronogramaTarefa.Value ), JsonRequestBehavior.AllowGet );
			}
			return new HttpStatusCodeResult( HttpStatusCode.BadRequest, "Deveria ser informado o Oid de identificação do cronograma ou da tarefa selecionada" ) { };
		}


		// Delete: /Cronogramas/?id={id}
		[HttpDelete]
		[ActionName( "Index" )]
		public ActionResult ExcluirCronograma( Guid? id )
		{
			bool respostaExclusao = false;
			using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				if(id.HasValue)
				{
					respostaExclusao = CronogramaBo.ExcluirCronograma( contexto, id.Value );
				}
				else
				{
					return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
				}
			}
			return Json( respostaExclusao );
		}

		/// <summary>
		/// Criar uma nova tarefa
		/// </summary>
		/// <param name="tarefa">dto com dados de criação de uma nova tarefa</param>
		/// <returns></returns>
		// POST : /Cronogramas/Tarefas
		[HttpPost, ActionName( "Tarefas" )]
		public ActionResult CriarTarefa( TarefaCriacaoDto tarefa )
		{
			CronogramaTarefa novaTarefa = null;
			List<CronogramaTarefa> tarefasImpactadas = null;
			DateTime dataHoraAcao = new DateTime();
			novaTarefa = CronogramaBo.CriarTarefa( tarefa.OidCronograma, tarefa.TxDescricaoTarefa, tarefa.OidSituacaoPlanejamentoTarefa, tarefa.DtInicio, tarefa.AutorCriacao,
				tarefa.TxObservacaoTarefa, tarefa.TxResponsaveis, out tarefasImpactadas, ref dataHoraAcao, tarefa.NbEstimativaInicial, tarefa.NbIdReferencia );
			return Json( TarefaBo.TarefaCriadaDtoFactory( novaTarefa, tarefasImpactadas, dataHoraAcao ) );
		}

		/// <summary>
		/// Editar uma tarefa
		/// </summary>
		/// <param name="tarefa">tarefa com dados alterados</param>
		/// <returns></returns>
		// PUT : /Cronogramas/Tarefas
		[HttpPut, ActionName( "Tarefas" )]
		public ActionResult EditarTarefa( CronogramaTarefaDto tarefa )
		{
			if(tarefa == null)
			{
				return Json( new { } );
			}

			Hashtable dadosEdicaoTarefa = null;
			dadosEdicaoTarefa = TarefaBo.EditarTarefa( tarefa.OidCronogramaTarefa.ToString(), tarefa.TxDescricaoTarefa, tarefa.OidSituacaoPlanejamentoTarefa.ToString(),
													   tarefa.TxAtualizadoPor, tarefa.TxObservacaoTarefa, tarefa.TxDescricaoColaborador, tarefa.NbEstimativaInicial,
													   tarefa.NbEstimativaRestante.ToTimeSpan(), tarefa.NbRealizado.ToTimeSpan(), tarefa.CsLinhaBaseSalva, tarefa.DtInicio );

			return new JsonNetResult( TarefaBo.EdicaoDtoFactory( dadosEdicaoTarefa ) );
		}

		#endregion

		#region Serviços

		/// <summary>
		/// Serviço para excluir múltiplas tarefas com exclusão lógica
		/// </summary>
		/// <param name="dto">dto de exclusão de tarefas</param>
		/// <returns>oid das tarefas excluidas</returns>
		[HttpPut, ActionName( "ExcluirTarefa" )]
		public ActionResult ExcluirTarefa( TarefaExclusaoDto dto )
		{
			try
			{
				if(!dto.OidCronograma.HasValue || dto.OidsCronogramaTarefas == null || dto.OidsCronogramaTarefas.Count < 1)
				{
					return new EmptyResult();
				}

				List<CronogramaTarefa> tarefasImpactadas = new List<CronogramaTarefa>();
				List<Guid> oidsTarefasNaoExcluidas = new List<Guid>();
				DateTime dataHoraAcao = new DateTime();
				List<CronogramaTarefa> tarefasExcluidas;
				tarefasExcluidas = CronogramaTarefaBo.ExcluirCronogramaTarefas( dto.OidsCronogramaTarefas, dto.OidCronograma.Value, ref tarefasImpactadas, ref oidsTarefasNaoExcluidas, ref dataHoraAcao );
				return Json( CronogramaTarefaBo.TarefasExcluidasDtoFactory( tarefasImpactadas, oidsTarefasNaoExcluidas, tarefasExcluidas, DateTime.Now, dto.OidCronograma.Value ) );
			}
			catch(Exception e)
			{
				Exception novaException = new Exception( String.Format( "Messagem: {0} /n \n - StackTrace: {1} /n \n -", e.Message, e.StackTrace ) );

				throw novaException;
			}
		}

		/// <summary>
		/// Selecionar o ultimo cronograma selecionado de um colaborador
		/// </summary>
		/// <param name="login">login do colaborador</param>
		/// <returns></returns>
		// GET: /Cronogramas/UltimoSelecionadoPor/?login=loginDoColaborador
		[HttpGet, ActionName( "UltimoSelecionadoPor" )]
		public ActionResult ConsultarUltimoCronogramaSelecionado( string login )
		{
			if(string.IsNullOrWhiteSpace( login ))
			{
				return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
			}
			using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
                var retorno = CronogramaUltimaSelecaoBO.ConsultarUltimoCronogramaSelecionadoDto(contexto, login);
				return Json( retorno, JsonRequestBehavior.AllowGet );
			}
		}

		/// <summary>
		/// Salvar o ultimo cronograma selecionado de um colaborador
		/// </summary>
		/// <param name="login"></param>
		/// <param name="oidCronograma"></param>
		/// <returns></returns>
		// PUT: /Cronogramas/UltimoSelecionadoPor/?login=loginDoColaborador&oidCronograma=xxxxx-xxxxxx-xxxxxx-xxxxx
		[HttpPut, ActionName( "UltimoSelecionadoPor" )]
		public ActionResult SalvarUltimoCronogramaSelecionado( string login, Guid? oidCronograma )
		{
			if(string.IsNullOrWhiteSpace( login ) || !oidCronograma.HasValue)
			{
				return Json( false, JsonRequestBehavior.AllowGet );
			}

			CronogramaUltimaSelecaoBO.SalvarUltimoCronogramaSelecionado( login, oidCronograma.Value );
			return Json( true, JsonRequestBehavior.AllowGet );
		}

		[HttpGet, ActionName( "LogTarefas" )]
		public ActionResult ConsultarLogTarefas( Guid? oidTarefa )
		{
		    if (!oidTarefa.HasValue) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    using(WexDb contexto = ContextFactoryManager.CriarWexDb())
		    {
		        List<TarefaLogAlteracaoDto> logs = TarefaLogAlteracaoBo.ConsultarAlteracoesTarefaPorOidDto( contexto, oidTarefa.Value );
		        if(logs == null || logs.Count < 1)
		            return new HttpStatusCodeResult( HttpStatusCode.NoContent );
		        return new JsonNetResult( logs );
		    }
		}

		/// <summary>
		/// Efetuar a consulta de tarefas dada uma lista de oids
		/// </summary>
		/// <param name="oids"></param>
		/// <returns></returns>
		[HttpPut, ActionName( "ConsultarTarefas" )]
		public ActionResult ListarTarefas( List<Guid> oids )
		{
			return RetornarActionResult( CronogramaTarefaBo.ConsultarCronogramasTarefaPorOidDto( oids ), JsonRequestBehavior.DenyGet );
		}

		/// <summary>
		/// Serviço para efetuar a movimentação de uma tarefa
		/// </summary>
		/// <param name="oidTarefaSelecionada">oid da tarefa movida</param>
		/// <param name="nbIdDestino">posição de destino</param>
		/// <returns>hash com informações sobre o retorno da movimentação</returns>
		// PUT: /Cronogramas/MoverTarefa
		[HttpPut, ActionName( "MoverTarefa" )]
		public ActionResult MoverTarefa( Guid? oidTarefaSelecionada, short nbIdDestino )
		{
			try
			{
				if(!oidTarefaSelecionada.HasValue)
				{
					throw new ArgumentException( "o oid da tarefa selecionada é obrigatório" );
				}
			    short nbIDAtualizadoTarefaMovida = 0;
				DateTime dataHoraAcao = new DateTime();
				Guid oidCronograma = new Guid();
				List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaBo.ReordenarTarefas( oidTarefaSelecionada.Value, nbIdDestino, ref nbIDAtualizadoTarefaMovida, ref dataHoraAcao, ref oidCronograma );
				return Json( CronogramaTarefaBo.TarefasMovidasDtoFactory( tarefasImpactadas, DateTime.Now, oidTarefaSelecionada.Value, nbIDAtualizadoTarefaMovida, oidCronograma ) );
			}
			catch(Exception e)
			{
				Exception novaException = new Exception( String.Format( "Messagem: {0} /n \n - StackTrace: {1} /n \n -", e.Message, e.StackTrace ) );
				throw novaException;
			}
		}

        [HttpGet]
        [ActionName( "ColaboradorConfig" )]
        public ActionResult ConsultarCronogramaColaboradorConfig( string login, Guid? oidCronograma )
        {
            if( !String.IsNullOrWhiteSpace( login ) && oidCronograma.HasValue )
                return Json( CronogramaColaboradorConfigBo.ConsultarCronogramaColaboradorConfigs( login, oidCronograma.Value ), JsonRequestBehavior.AllowGet );
     
            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }

        [HttpPut, ActionName( "ColaboradorConfig" )]
        public ActionResult SalvarCronogramaColaboradorConfig( CronogramaColaboradorConfigDto dto )
        {
            if (String.IsNullOrWhiteSpace(dto.Login) || dto.OidCronograma == new Guid())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string cor = CronogramaColaboradorConfigBo.EscolherCorColaborador( dto.Login, dto.OidCronograma );
            return Json( new {cor } );
        }

        [HttpGet, ActionName( "Burndown" )]
        public ActionResult GerarDadosGraficoBurndown( Guid? oidCronograma )
        {
            if(!oidCronograma.HasValue)
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
            return new JsonNetResult( GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( oidCronograma.Value ) );
        }


		#endregion

		#region Métodos Auxiliares

		/// <summary>
		/// Retorna a action result  de acordo com o tipo de informação
		/// </summary>
		/// <typeparam name="TResult">Tipo de retorno</typeparam>
		/// <param name="retorno">dados do retorno</param>
		/// <param name="behavior">comportamento do JsonResult</param>
		/// <returns>caso não seja nulo retorna o json do objeto, caso seja uma coleção retonar o Status code para No Content e caso não seja uma coleção retorna not found</returns>
		[NonAction]
		private ActionResult RetornarActionResult<TResult>( TResult retorno, JsonRequestBehavior behavior )
		{
		    if (retorno != null) return Json(retorno, behavior);
		    var tipo = typeof( TResult );
		    if(tipo.IsGenericType && ( tipo.GetGenericTypeDefinition() == typeof( IList<> ) || tipo.GetGenericTypeDefinition() == typeof( ICollection<> ) ))
		    {
		        return new HttpStatusCodeResult( HttpStatusCode.NoContent );
		    }
		    return HttpNotFound();
		}
		#endregion
	}
}
