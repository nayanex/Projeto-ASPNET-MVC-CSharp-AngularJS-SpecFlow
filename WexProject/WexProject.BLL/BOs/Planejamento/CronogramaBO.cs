using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using System.Data.Entity;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Contexto;
using WexProject.BLL.Extensions.Entities;
using System.Data;
using WexProject.Library.Libs.Comparacao;

namespace WexProject.BLL.BOs.Planejamento
{
	public class CronogramaBo
	{
		#region Atributos

		/// <summary>
		/// Semáforo para controlar a alteração do nome de uma cronograma.
		/// </summary>
		private static Semaphore semaforoAlteracaoNomeCronograma = new Semaphore( 1, 1 );

		#endregion

		#region Método de validações

		/// <summary>
		/// Método responsável por validar a descrição do cronograma
		/// Ex: verifica se existem espaços desnecessários e retira-os.
		/// </summary>
		/// <param name="novaDescricaoCronograma">Descrição passada pelo client</param>
		/// <returns>Descrição formatada e válida</returns>
		private static string ValidarDescricaoCronograma( string novaDescricaoCronograma )
		{
			string descricaoValida = String.Empty;

			string[] descricaoArray = novaDescricaoCronograma.Split( ' ' );

			for(int i = 0; i < descricaoArray.Length; i++)
			{
				if(descricaoArray[i].Length > 0)
				{
					if(!descricaoArray[i].Contains( " " ))
					{
						descricaoValida += descricaoArray[i] + ' ';
					}
				}
			}

			descricaoValida = descricaoValida.Trim();

			return descricaoValida;
		}

		#endregion

		/// <summary>
		/// Método responsável por criar um cronograma padrão.
		/// É usado para retornar uma instância padrão para a tela de cronograma
		/// </summary>
		/// <param name="contexto">contexto do banco</param>
		/// <returns>Cronograma criado</returns>
		public static Cronograma CriarCronogramaPadrao( WexDb contexto )
		{
			bool valido = false;

			//TODO: Quando associar ao projeto deverá ser mudada a pesquisa
			List<Cronograma> cronogramas = CronogramaDao.ConsultarCronogramas();
			int contador = cronogramas.Count;

			Cronograma cronograma = new Cronograma();

			if(cronogramas.Count < 10)
				cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{00:00}", ( cronogramas.Count + 1 ) );
			else
				cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{0}", ( cronogramas.Count + 1 ) );
			do
			{
				Cronograma cronogramaResultado = CronogramaDao.ConsultarCronogramaPorNome( contexto, cronograma.TxDescricao );

				if(cronogramaResultado == null)
					valido = true;
				else
				{
					//TODO: Quando associar ao projeto deverá ser mudada a pesquisa
					if(cronogramas.Count < 10)
						cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{00:00}", ( contador + 1 ) );

					else
						cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{0}", ( contador + 1 ) );

					valido = false;
				}

				contador++;

			} while(valido == false);

			cronograma.SituacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );
			cronograma.DtInicio = DateTime.Now;
			cronograma.DtFinal = DateTime.Now.AddDays(5);

			contexto.Cronograma.Add( cronograma );
			contexto.SaveChanges();

			return cronograma;
		}

		public static Cronograma CriarCronogramaPadrao()
		{
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				bool valido = false;

				//TODO: Quando associar ao projeto deverá ser mudada a pesquisa
				List<Cronograma> cronogramas = CronogramaDao.ConsultarCronogramas();
				int contador = cronogramas.Count;

				Cronograma cronograma = new Cronograma();

				if(cronogramas.Count < 10)
					cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{00:00}", ( cronogramas.Count + 1 ) );
				else
					cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{0}", ( cronogramas.Count + 1 ) );
				do
				{
					Cronograma cronogramaResultado = CronogramaDao.ConsultarCronogramaPorNome( contexto, cronograma.TxDescricao );

					if(cronogramaResultado == null)
						valido = true;
					else
					{
						//TODO: Quando associar ao projeto deverá ser mudada a pesquisa
						if(cronogramas.Count < 10)
							cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{00:00}", ( contador + 1 ) );

						else
							cronograma.TxDescricao = "Wex Cronograma " + String.Format( "{0}", ( contador + 1 ) );

						valido = false;
					}

					contador++;

				} while(valido == false);

                cronograma.SituacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );
				cronograma.DtInicio = DateTime.Now;
				cronograma.DtFinal = DateTime.Now;

				contexto.Cronograma.Add( cronograma );
				contexto.SaveChanges();

				return cronograma;
			}
		}

		/// <summary>
		/// Método responsável por efetuar alteração na descrição do cronograma
		/// </summary>
		/// <param name="oidCronograma"> oid do cronograma que terá o nome alterado</param>
		/// <param name="novaDescricaoCronograma">novo nome do cronograma</param>
		public static bool AlterarDescricaoCronograma( Guid oidCronograma, string novaDescricaoCronograma )
		{
			if(oidCronograma == new Guid())
				throw new ArgumentException( "Erro de argumento, a oidCronograma não pode ser nulo" );

			if(string.IsNullOrEmpty( novaDescricaoCronograma ))
				throw new ArgumentException( "Erro de argumento, a descrição da tarefa não pode ser nula" );

			Cronograma cronogramaSelecionado = CronogramaDao.ConsultarCronogramaPorOid( oidCronograma, o => o.SituacaoPlanejamento );
			if(cronogramaSelecionado == null)
				return false;

			semaforoAlteracaoNomeCronograma.WaitOne();

			string descricaoValida = ValidarDescricaoCronograma( novaDescricaoCronograma );

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				bool cronogramaComMesmoNome = contexto.Cronograma.Existe( o => o.TxDescricao == descricaoValida && !o.CsExcluido );

				if(cronogramaComMesmoNome)
				{
					semaforoAlteracaoNomeCronograma.Release();
					return false;
				}

                cronogramaSelecionado.TxDescricao = descricaoValida;
				contexto.Cronograma.Attach( cronogramaSelecionado );
				contexto.Entry( cronogramaSelecionado ).State = EntityState.Modified;
				contexto.Entry( cronogramaSelecionado.SituacaoPlanejamento ).State = EntityState.Unchanged;
				contexto.SaveChanges();
				semaforoAlteracaoNomeCronograma.Release();
				return true;
			}
		}

	    /// <summary>
		/// Método responsável por excluir um cronograma.
		/// Antes faz uma verificação para analisar se existem tarefas associadas ao cronograma
		/// caso não tenha, exclui as ultimas seleções dos usuários para aquele cronograma
		/// para então excluí-lo.
		/// </summary>
		/// <param name="session">Sessão Corrente</param>
		/// <param name="oidCronograma">Guid (ID) do cronograma a ser excluido</param>
		/// <returns>Boolean confirmando ou não a exclusão</returns>
		public static bool ExcluirCronograma( WexDb contexto, Guid oidCronograma )
		{
			if(contexto == null || oidCronograma == null)
				throw new ArgumentException( "Os parâmetros Session e OidCronograma não podem ser nulos." );

			Cronograma cronograma = CronogramaDao.ConsultarCronogramaCronogramaTarefasETarefasPodOidCronograma( contexto, oidCronograma );
			if(cronograma != null)
			{
				if(cronograma.CronogramaTarefas.Any())
				{
					List<CronogramaTarefa> cronogramaTarefasParaExcluir = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma.Oid, o => o.Tarefa.TarefaHistoricoTrabalhos );
					for(int i = 0; i < cronogramaTarefasParaExcluir.Count; i++)
					{
						if(cronogramaTarefasParaExcluir[i].Tarefa.TarefaHistoricoTrabalhos != null)
						{
							List<TarefaHistoricoTrabalho> historicosParaExcluir = cronogramaTarefasParaExcluir[i].Tarefa.TarefaHistoricoTrabalhos.ToList();

							for(int j = 0; j < historicosParaExcluir.Count; j++)
							{
								historicosParaExcluir[j].CsExcluido = true;
							}

							cronogramaTarefasParaExcluir[i].Tarefa.TarefaHistoricoTrabalhos = null;
						}
						cronogramaTarefasParaExcluir[i].CsExcluido = true;
						cronogramaTarefasParaExcluir[i].Tarefa.CsExcluido = true;
					}
				}
				CronogramaBo.RemoverSelecoesAssociadasAoCronograma( contexto, cronograma.Oid );
				cronograma.CsExcluido = true;
				contexto.SaveChanges();

				return true;
			}

			return false;
		}

		/// <summary>
		/// Método responsável por criar uma tarefa dependente de projeto e cronograma.
		/// É acessado pelo serviço de planejamento quando solicitado pela tela de cronograma.
		/// </summary>
		/// <param name="contexto">Contexto do banco</param>
		/// <param name="login">Login do usuario</param>
		/// <param name="oidCronograma"> oid cronograma</param>
		/// <param name="txDescricaoTarefa"> descrição da tarefa </param>
		/// <param name="txObservacaoTarefa">observações da tarefa</param>
		/// <param name="oidSituacao">oid situação da tarefa</param>
		/// <param name="responsaveis">responsáveis pela tarefa</param>
		/// <param name="nbEstimativaInicial"> horas estimadas inicialmente</param>
		/// <param name="dtInicio"> data de inicio da tarefa</param>
		/// <param name="oidTarefaSelecionada">oid da tarefa</param>
		/// <param name="salvar"> condição para indicar se irá salvar ou não a tarefa dentro da classe tarefa</param>
		/// <returns>hash contendo informações sobre a tarefa e lista das tarefas impactadas</returns>
		public static CronogramaTarefa CriarTarefa( Guid oidCronograma, string txDescricaoTarefa, string oidSituacao, DateTime dtInicio, string login, string txObservacaoTarefa, string responsaveis, out List<CronogramaTarefa> tarefasImpactadas, ref DateTime dataHoraAcao, Int16 nbEstimativaInicial = 0, short nbIDReferencia = 0 )
		{
			if(oidCronograma == null || oidCronograma == new Guid() || String.IsNullOrEmpty( oidSituacao ))
				throw new ArgumentException( "Os parametros session, oidCronograma sao obrigatorios." );

            SituacaoPlanejamento situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( Guid.Parse( oidSituacao ) );

			CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( oidCronograma, txDescricaoTarefa, situacaoPlanejamento, dtInicio, responsaveis, login, out tarefasImpactadas, ref dataHoraAcao, txObservacaoTarefa, nbEstimativaInicial, nbIDReferencia );

			return novaTarefa;
		}

		/// <summary>
		/// Método utilizado para remover todas as seleções dos usuários associados ao cronograma como ultima seleção.
		/// </summary>
		/// <param name="contexto">contexto do banco</param>
		/// <param name="oidCronograma">oid do cronograma a ser removidas as seleções</param>
		public static void RemoverSelecoesAssociadasAoCronograma( WexDb contexto, Guid oidCronograma )
		{
			List<CronogramaUltimaSelecao> colecao = contexto.CronogramaUltimaSelecao.Where( o => o.Cronograma.Oid == oidCronograma ).ToList();

			if(colecao != null && colecao.Count > 0)
			{
				int contador = colecao.Count;
				for(int i = 0; i < contador; i++)
				{
					CronogramaUltimaSelecao ultimaSelecao = colecao[i];
					contexto.CronogramaUltimaSelecao.Remove( ultimaSelecao );
					contexto.SaveChanges();
				}
			}
		}
		/// <summary>
		/// Método responsável por buscar todos os cronogramas
		/// É usado pela tela de cronograma
		/// </summary>
		/// <param name="session">Sessão Corrente</param>
		/// <returns>Lista de Objetos de CronogramaDTO</returns>
		public static List<CronogramaDto> ConsultarCronogramasDto()
		{
			List<CronogramaDto> cronogramasDTO = new List<CronogramaDto>();

			List<Cronograma> cronogramas = new List<Cronograma>();
			cronogramas = CronogramaDao.ConsultarCronogramas();

			if(cronogramas.Count > 0)
				foreach(Cronograma cronograma in cronogramas)
				{
					CronogramaDto cronoDTO = new CronogramaDto()
					{
						Oid = cronograma.Oid,
						TxDescricao = cronograma.TxDescricao,
						OidSituacaoPlanejamento = cronograma.SituacaoPlanejamento.Oid,
						TxDescricaoSituacaoPlanejamento = cronograma.TxDescricaoSituacaoPlanejamento,
						DtInicio = (DateTime)cronograma.DtInicio,
						DtFinal = (DateTime)cronograma.DtFinal
					};

					cronogramasDTO.Add( cronoDTO );
				}

			return cronogramasDTO;
		}

		/// <summary>
		/// Método responsável por buscar um cronograma pelo nome.
		/// </summary>
		/// <param name="session">Sessão Corrente</param>
		/// <param name="txDescricaoCronograma">Nome do Cronograma</param>
		/// <returns>Objeto cronograma</returns>
		public static CronogramaDto ConsultarCronogramaPorNomeDto( WexDb contexto, string txDescricaoCronograma )
		{
			CronogramaDto cronogramaDto = new CronogramaDto();

			Cronograma cronograma = CronogramaDao.ConsultarCronogramaPorNome( contexto, txDescricaoCronograma );

			if(cronograma != null)
			{
				cronogramaDto.Oid = cronograma.Oid;
				cronogramaDto.OidSituacaoPlanejamento = cronograma.SituacaoPlanejamento.Oid;
				cronogramaDto.TxDescricao = cronograma.TxDescricao;
				cronogramaDto.TxDescricaoSituacaoPlanejamento = cronograma.SituacaoPlanejamento.TxDescricao;
				cronogramaDto.DtInicio = (DateTime)cronograma.DtInicio;
				cronogramaDto.DtFinal = (DateTime)cronograma.DtFinal;

				return cronogramaDto;
			}

			return null;
		}

		/// <summary>
		/// Método responsável por buscar um cronograma pelo nome.
		/// </summary>
		/// <param name="session">Sessão Corrente</param>
		/// <param name="txDescricaoCronograma">Nome do Cronograma</param>
		/// <returns>Objeto cronograma</returns>
		public static CronogramaDto ConsultarCronogramaPorOidDto( Guid oidCronograma )
		{
			CronogramaDto cronogramaDto = new CronogramaDto();

			Cronograma cronograma = CronogramaDao.ConsultarCronogramaPorOid( oidCronograma , o=>o.SituacaoPlanejamento );

			if(cronograma != null)
			{
				cronogramaDto.Oid = cronograma.Oid;
				cronogramaDto.OidSituacaoPlanejamento = cronograma.OidSituacaoPlanejamento;
				cronogramaDto.TxDescricao = cronograma.TxDescricao;
				cronogramaDto.TxDescricaoSituacaoPlanejamento = cronograma.SituacaoPlanejamento.TxDescricao;
				cronogramaDto.DtInicio = (DateTime)cronograma.DtInicio;
				cronogramaDto.DtFinal = (DateTime)cronograma.DtFinal;

				return cronogramaDto;
			}

			return null;
		}

        /// <summary>
        /// Método responsável por editar os dados de um cronograma
        /// </summary>
        /// <param name="cronogramaDto">dados do cronograma editado</param>
        /// <returns>verdadeiro caso tenha sido editado e falso caso contrário</returns>
	    public static bool EditarCronograma(CronogramaDto cronogramaDto)
        {
            if(cronogramaDto == null || cronogramaDto.Oid == new Guid())
                return false;

            var cronograma = CronogramaDao.ConsultarCronogramaPorOid( cronogramaDto.Oid, o => o.SituacaoPlanejamento.Cronogramas );

            if(cronograma == null)
                return false;

            var dtoCronogramaBaseDados = CronogramaBo.DtoFactory( cronograma );

            if(ComparadorGenerico.HouveMudancaEm( dtoCronogramaBaseDados, cronogramaDto, o => o.TxDescricao ))
                return AlterarDescricaoCronograma( cronogramaDto.Oid, cronogramaDto.TxDescricao );

			if( !ValidarDatasCronograma(cronogramaDto) || !ComparadorGenerico.HouveMudancaEm( dtoCronogramaBaseDados , cronogramaDto , o => o.OidSituacaoPlanejamento , o => o.DtInicio , o => o.DtFinal ))
                return false;

            AtualizarDadosCronograma( cronograma, cronogramaDto );
            return CronogramaDao.SalvarCronograma( cronograma );
        }

        /// <summary>
        /// Métodor responsável por efetuar uma cópia dos dados do cronograma
        /// </summary>
        /// <param name="cronograma">entidade cronograma</param>
        /// <param name="cronogramaDto">dto com novos dados do cronograma</param>
        private static void AtualizarDadosCronograma( Cronograma cronograma, CronogramaDto cronogramaDto )
        {
            cronograma.OidSituacaoPlanejamento = cronogramaDto.OidSituacaoPlanejamento;
            cronograma.DtInicio = cronogramaDto.DtInicio;
            cronograma.DtFinal = cronogramaDto.DtFinal;
        }

		/// <summary>
		/// Método responsável por validar as datas do cronograma.
		/// </summary>
		/// <param name="cronogramaDto">Objeto que contem os dados a serem validados.</param>
		/// <returns>Se os dados são válidos ou não.</returns>
		private static bool ValidarDatasCronograma( CronogramaDto cronogramaDto )
		{
			if( cronogramaDto.DtInicio < cronogramaDto.DtFinal )
				return true;

			return false;
		}

	    #region Factories

		/// <summary>
		/// Método responsável por criar um instância de cronograma padrão e retornar para a tela de cronograma
		/// </summary>
		/// <returns>Objeto CronogramaDTO</returns>
		public static CronogramaDto DtoFactory( Cronograma cronograma )
		{
			CronogramaDto cronogramaDto = new CronogramaDto();

			if(cronograma != null)
			{
				cronogramaDto.Oid = cronograma.Oid;
				cronogramaDto.TxDescricao = cronograma.TxDescricao;
				cronogramaDto.OidSituacaoPlanejamento = (Guid)cronograma.OidSituacaoPlanejamento;
				cronogramaDto.TxDescricaoSituacaoPlanejamento = cronograma.SituacaoPlanejamento.TxDescricao;
				cronogramaDto.DtInicio = (DateTime)cronograma.DtInicio;
				cronogramaDto.DtFinal = (DateTime)cronograma.DtFinal;

				return cronogramaDto;
			}

			return cronogramaDto;
		}

		#endregion
	}
}
