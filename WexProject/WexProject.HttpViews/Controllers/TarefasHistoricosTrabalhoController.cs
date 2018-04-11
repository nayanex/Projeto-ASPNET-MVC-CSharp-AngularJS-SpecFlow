using System;
using System.Net;
using System.Web.Mvc;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.HttpViews.Libs.ActionResults;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.HttpViews.Controllers
{
	public class TarefasHistoricosTrabalhoController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult UltimoHistorico( string login, DateTime? data, Guid? oidTarefa )
		{
			if(!string.IsNullOrWhiteSpace( login ))
			{
				InicializadorEstimativaDto retorno = null;

				if(data.HasValue)
					retorno = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaColaboradorParaDataEspecifica( login, data.Value );
				else
					retorno = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( login, DateTime.Now.Date );

				if(retorno == null)
					return new HttpNotFoundResult();

				return new JsonNetResult( retorno );
			}

			if(oidTarefa.HasValue)
			{
				var resultado = TarefaHistoricoTrabalhoBo.ConsultarTarefaHistoricoAtualPorOidTarefaDto( oidTarefa.Value );
				if(resultado == null)
					return new HttpNotFoundResult();
				return Json( resultado, JsonRequestBehavior.AllowGet );
			}
			return new HttpStatusCodeResult( HttpStatusCode.BadRequest, "Parâmetro inválido." );
		}

		[HttpPost, ActionName( "Index" )]
		public ActionResult CriarHistorico( CriarHistoricoTarefaDto dto )
		{
			if(dto == null)
			{
				return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
			}
			TarefaHistoricoTrabalhoBo
				.CriarHistoricoTarefa( dto.OidTarefa, dto.Autor, dto.NbHoraRealizado.ToTimeSpan(),
				dto.DtRealizado, dto.NbHoraInicial.ToTimeSpan(), dto.NbHoraFinal.ToTimeSpan(), dto.Comentario,
				dto.NbHoraRestante.ToTimeSpan(), dto.OidSituacaoPlanejamento, dto.JustificativaReducao );
			return new HttpStatusCodeResult( HttpStatusCode.Created );
		}
	}
}
