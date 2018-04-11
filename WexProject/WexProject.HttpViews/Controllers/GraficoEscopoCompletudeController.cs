using System;
using System.Net;
using System.Web.Mvc;
using WexProject.BLL.BOs.Escopo;

namespace WexProject.HttpViews.Controllers
{
    public class GraficoEscopoCompletudeController : Controller
    {
        // GET: {base_url}/GraficoEscopoCompletude/
        [HttpGet]
        public ActionResult Index( Guid? id )
        {
            if(id.HasValue)
                return Json( GraficoEscopoCompletudeBO.CalcularGraficoEscopoCompletude( id.Value ), JsonRequestBehavior.AllowGet );

            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }
    }
}
