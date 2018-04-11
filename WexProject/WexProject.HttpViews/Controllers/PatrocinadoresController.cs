using System.Web.Mvc;
using WexProject.BLL.BOs.Geral;

namespace WexProject.HttpViews.Controllers
{
    public class PatrocinadoresController : Controller
    {

        // GET: /Patrocinadores/

		/// <summary>
		/// Retorna lista de empresas e instituições filtradas por termo
		/// </summary>
		/// <param name="term">Termo para usar para filtrar</param>
		/// <returns>Uma lista em Json de empresas e instituições filtradas</returns>
		public ActionResult Index()
		{
			var patrocinadores = EmpresaInstituicaoBo.Instancia.ListarEmpresasInstituicoes();

			return Json(new { patrocinadores = patrocinadores }, JsonRequestBehavior.AllowGet);
		}
    }
}
