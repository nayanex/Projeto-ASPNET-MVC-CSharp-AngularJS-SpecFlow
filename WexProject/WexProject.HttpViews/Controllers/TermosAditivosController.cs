using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.BOs.Geral;
using WexProject.Library.Libs.Collection;

namespace WexHttpAPI.Controllers
{
    public class TermosAditivosController : Controller
    {
        //
        // GET: /TermoAditivo/

		[HttpGet]
		[ActionName("TermosAditivos")]
        public ActionResult TermosAditivosGet()
        {
			if (Request.AcceptTypes.Contains("application/json"))
			{
				var termosAditivos = TermoAditivoBo.Instancia.ListarTermoAditivo();

				return Json(new {termosAditivos = termosAditivos}, JsonRequestBehavior.AllowGet);
			}
			
			return View();
        }

		[AcceptVerbs("post", "put")]
		[ActionName("TermosAditivos")]
		public ActionResult TermosAditivosPostPut(TermoAditivoDto termoAditivoDto)
		{
			var termoAditivoId = TermoAditivoBo.Instancia.SalvarTermoAditivo(termoAditivoDto);

			return Json(new { id = termoAditivoId });
		}

		[HttpDelete]
		[ActionName("TermosAditivos")]
		public ActionResult TermosAditivosDelete(int termoAditivoId, Boolean force = false)
		{
			try
			{
				TermoAditivoBo.Instancia.ExcluirTermoAditivo(termoAditivoId, force);
			}
			catch (TermoAditivoNaoVazioException e)
			{
				Response.StatusCode = 405;

				return Json(new { message = e.Message });
			}
			catch (TermoAditivoNaoEncontradoException e)
			{
				Response.StatusCode = 404;

				return Json(new { message = e.Message });
			}

			return Json(new { id = termoAditivoId });
		}

		[HttpGet]
		[ActionName("Projetos")]
		public ActionResult ProjetosTermosAditivosGet(int termoAditivoId)
		{
			var filtro = Request.QueryString.CriarFiltro<ProjetoDto>();

			filtro.Add("TermoAditivoId", termoAditivoId);

			var projetos = ProjetoBo.Instancia.ConsultarProjetos(filtro);

			return Json(new { projetos = projetos }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ActionName("Projetos")]
		public ActionResult ProjetosTermosAditivosPost(int termoAditivoId, Guid projetoOid)
		{
			try
			{
				TermoAditivoBo.Instancia.AssociarProjeto(termoAditivoId, projetoOid);
			}
			catch (ProjetoJaAssociadoATermoAditivoException e)
			{
				Response.StatusCode = 409;

				return Json(new { message = e.Message });
			}

			return new EmptyResult();
		}

		[HttpPut]
		[ActionName("Projetos")]
		public ActionResult ProjetosTermosAditivosPut(int termoAditivoId, Guid actionId)
		{
			var projetoOid = actionId;

			throw new NotImplementedException();
		}

		[HttpDelete]
		[ActionName("Projetos")]
		public ActionResult ProjetosTermosAditivosDelete(int termoAditivoId, Guid actionId)
		{
			var projetoOid = actionId;

			try
			{
				TermoAditivoBo.Instancia.DisassociarProjeto(termoAditivoId, projetoOid);
			}
			catch (ProjetoComValorException e)
			{
				Response.StatusCode = 405;

				return Json(new { message = e.Message });
			}

			return new EmptyResult();
		}

    }
}
