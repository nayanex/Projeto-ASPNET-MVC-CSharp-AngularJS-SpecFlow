using System.Web.Mvc;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.HttpViews.Controllers
{
    public class CustosRubricasAdministrativasController : Controller
    {

        /// <summary>
        /// Tela inicial de custos administrativos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("index")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lista os custos de todas as rubricas administrativas em uma determinada data
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("rubricas")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult ListarCustosRubricas(int ano = 0, int mes = 0)
        {
            var custosRubricas =
                TipoRubricaBo.Instance.DetalharCustosTipoRubrica(CsClasseRubrica.Administrativo, ano, mes);
            return Json(custosRubricas, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lista os custos de todos os projetos de uma rubrica em uma determinada data
        /// </summary>
        /// <param name="tipoRubricaId"></param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("projetos")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult ListarCustosProjetos(int tipoRubricaId, int ano, int mes)
        {
            var custosProjetos = RubricaMesBo.Instance.ListarCustosProjetos(tipoRubricaId, ano, mes);
            return Json(custosProjetos, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs("POST", "PUT")]
        [ActionName("despesa-real")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult SalvarDespesaReal(DespesaRealDto despesaRealDto)
        {
            RubricaMesBo.Instance.SalvarDespesaReal(despesaRealDto);
            return new EmptyResult();
        }

    }

}