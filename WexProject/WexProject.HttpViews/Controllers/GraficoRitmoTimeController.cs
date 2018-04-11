using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using WexProject.BLL.BOs.Execucao;
using WexProject.BLL.Shared.DTOs.Execucao;

namespace WexProject.HttpViews.Controllers
{
    public class GraficoRitmoTimeController : Controller
    {
        [HttpGet]
        public ActionResult Index( Guid? id )
        {
            if(id.HasValue)
            {
                List<GraficoRitmoTimeDTO> graficos = GraficoRitmoTimeBO.CalcularGraficoRitmoTimeProjeto( id.Value );

                return Json( graficos, JsonRequestBehavior.AllowGet );
            }
            
            return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
        }
    }
}
