using System;
using System.Net;
using System.Web.Mvc;
using WexProject.BLL.BOs.Escopo;

namespace WexProject.HttpViews.Controllers
{
    public class GraficoEstimadoRealizadoController : Controller
    {
        // GET: {base_url}/GraficoEstimadoRealizado/
        [HttpGet]
        public ActionResult Index( Guid? id )
        {
            if (id.HasValue)
                return Json( GraficoEstimadoRealizadoBO.CalcularGraficoEstimadoVsRealizadoProjeto( id.Value ), JsonRequestBehavior.AllowGet );

            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }
    }
}
