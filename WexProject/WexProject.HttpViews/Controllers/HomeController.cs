using System.Web.Mvc;

namespace WexProject.HttpViews.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
