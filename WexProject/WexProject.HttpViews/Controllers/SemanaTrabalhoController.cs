using System.Web.Mvc;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.HttpViews.Controllers
{
    public class SemanaTrabalhoController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            SemanaTrabalhoDto semanaTrabalho = SemanaTrabalhoBo.DtoFactory( new SemanaTrabalho() );

            return Json( semanaTrabalho, JsonRequestBehavior.AllowGet );
        }
    }
}
