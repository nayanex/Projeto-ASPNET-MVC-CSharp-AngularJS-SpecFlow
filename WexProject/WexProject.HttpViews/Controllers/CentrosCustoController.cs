using System.Web.Mvc;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;

namespace WexProject.HttpViews.Controllers
{
    public class CentrosCustoController : Controller
    {
        // GET: /CentrosCusto/

		/// <summary>
		/// Retorna lista de centros de custo filtrados por termo
		/// </summary>
		/// <returns>Uma lista em Json de centros de custo filtrados</returns>
        public ActionResult Index(string term)
		{
            var centrosCusto = CentroCustoBo.Instance.ListarCentrosCustos();

			return Json(new { centrosCusto = centrosCusto }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs("POST", "PUT")]
        [ActionName("Salvar")]
        public ActionResult SalvarCentroCusto(CentroCustoDto centroCustoDto)
        {
            int id = CentroCustoBo.Instance.SalvarCentroCusto(centroCustoDto);

            return Json (new { id });
        }
    }
}
