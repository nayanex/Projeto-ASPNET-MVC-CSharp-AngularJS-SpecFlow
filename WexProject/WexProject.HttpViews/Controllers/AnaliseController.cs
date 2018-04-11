using System;
using System.Linq;
using System.Web.Mvc;
using WexProject.BLL.BOs.Analise;
using WexProject.BLL.BOs.Analise.Custos;
using WexProject.BLL.Shared.DTOs.Analise.Custos.Geral;

namespace WexProject.HttpViews.Controllers
{
	public class AnaliseController : Controller
	{
		// Retorna tipos de análise
		// GET: /Analise/ 

		[HttpGet]
		[Authorize]
		public ActionResult Index()
		{
			return Json(new { }, JsonRequestBehavior.AllowGet);
		}

		// Retorna projetos para análise
		// GET: /Analise/Custos
		[HttpGet]
		[ActionName("Custos")]
		[Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
		public ActionResult Custos(Guid? id = null)
		{
			if (Request.AcceptTypes == null || !Request.AcceptTypes.Contains("application/json"))
			{
				return View();
			}

			if (id.HasValue)
			{
				var projeto = AnaliseBo.Instance.GerarAnaliseCriticaProjeto(id.Value);
				return Json(new { projeto }, JsonRequestBehavior.AllowGet);
			}

			var projetos = AnaliseBo.Instance.ListarCustosProjetos();

			return Json(new { projetos }, JsonRequestBehavior.AllowGet);
		}

		// Retorna projetos para análise
		// GET: /Analise/Custos/Geral/{tipo}/{projetoOid}
		[HttpGet]
		[ActionName("Geral")]
		[Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
		public ActionResult GeralGet(String tipo = null, Guid? projetoOid = null)
		{
			var json = Request.AcceptTypes != null && Request.AcceptTypes.Contains("application/json");

			if (projetoOid.HasValue && json)
			{
				var rubricas = GeralBo.Instancia.AnaliseProjeto(projetoOid.Value, tipo);

				return Json(new { rubricas }, JsonRequestBehavior.AllowGet);
			}

			if (tipo == null)
			{
				if (!json)
				{
					return View();
				}

				GeralDto custosProjetos;
				GeralDto fluxoProjetos;

				GeralBo.Instancia.AnaliseGeral(out custosProjetos, out fluxoProjetos);
				return Json(new { custosProjetos, fluxoProjetos }, JsonRequestBehavior.AllowGet);
			}

			Response.StatusCode = 400;

			return new EmptyResult();
		}
	}
}
