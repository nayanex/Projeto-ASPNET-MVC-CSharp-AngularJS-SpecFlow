using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.HttpViews.Controllers
{
    public class SituacoesPlanejamentoController : Controller
    {
        [HttpGet]
        public ActionResult Index( bool? inativas, bool? padrao )
        {
            List<SituacaoPlanejamentoDTO> situacoes;

            if(inativas.HasValue)
                situacoes = SituacaoPlanejamentoBO.ConsultarSituacoesInativas();
            else if(padrao.HasValue)
                return RetornarActionResult<SituacaoPlanejamentoDTO>( SituacaoPlanejamentoBO.ConsultarSituacaoPadrao(), JsonRequestBehavior.AllowGet );
            else
                situacoes = SituacaoPlanejamentoBO.ConsultarSituacoesAtivas();

            return RetornarActionResult<List<SituacaoPlanejamentoDTO>>( situacoes, JsonRequestBehavior.AllowGet );
        }

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
            if(retorno == null)
            {
                Type tipo = typeof( TResult );
                if(tipo.IsGenericType && ( tipo.GetGenericTypeDefinition() == typeof( IList<> ) || tipo.GetGenericTypeDefinition() == typeof( ICollection<> ) ))
                {
                    return new HttpStatusCodeResult( HttpStatusCode.NoContent );
                }
                return HttpNotFound();
            }
            return Json( retorno, behavior );
        }
    }
}
