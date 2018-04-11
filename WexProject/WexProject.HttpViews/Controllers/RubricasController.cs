using System.Collections.Generic;
using System.Web.Mvc;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.HttpViews.Controllers
{
    public class RubricasController : Controller
    {
        // GET: /Rubricas/

        /// <summary>
        ///     Lista as rubricas utilizadas na associação de notas ficais em custos por projeto
        /// </summary>
        /// <returns>Uma lista em Json de centros de custo filtrados</returns>
        [HttpGet]
		[ActionName("NotasFiscaisTiposRubricas")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult ListarRubricaNotasFiscais(int aditivoId)
        {
            List<RubricaDto> rubricas = RubricaBo.Instance.PesquisarRubricasNotasFiscais(aditivoId);
            return Json(rubricas, JsonRequestBehavior.AllowGet);
        }
    }
}