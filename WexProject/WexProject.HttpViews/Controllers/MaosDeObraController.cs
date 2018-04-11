using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.Externo.Totvs.BOs.Custos;
using WexProject.Library.Libs.Logger;

namespace WexProject.HttpViews.Controllers
{
    public class MaosDeObraController : Controller
    {
        public MaosDeObraController()
        {
            // Injetando implementação de importação
            MaosDeObraBo.Instance.MaosDeObraExterno = MaoDeObraBo.Instancia;
        }

        //
        // GET: /MaosDeObra/

        [HttpGet]
        [ActionName("Index")]
        public ActionResult ListarMaosDeObra(int centroCustoId, int aditivoId, int ano, int mes)
        {
            List<MaoDeObraDto> maosDeObra;
            LoteMaoDeObraDto lote;
            Decimal somaValorTotal;
            int quantidadeColaboradores;

            try
            {
                MaosDeObraBo.Instance.ListarMaosDeObra(centroCustoId, aditivoId, ano, mes, out lote, out maosDeObra,
                    out somaValorTotal, out quantidadeColaboradores);
            }
            catch (EntidadeNaoEncontradaException e)
            {
                Response.StatusCode = 404;
                return Json(new {Response.StatusCode, e.Message}, JsonRequestBehavior.AllowGet);
            }

            return Json(new {lote, maosDeObra, somaValorTotal, quantidadeColaboradores}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("Importar")]
        public ActionResult Importar(int centroCustoId, int aditivoId, int ano, int mes)
        {
			try
			{
				LoteMaoDeObraDto lote = MaosDeObraBo.Instance.Importar(centroCustoId, aditivoId, ano, mes);

				return Json(new { lote }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				WexLogger.Debug("Erro desconhecido ao importar", e);

				Response.StatusCode = 500;
				return Json(new { e.Message }, JsonRequestBehavior.AllowGet);
			}
        }

        [HttpGet]
        [ActionName("VerificarAtualizacao")]
        public ActionResult VerificarAtualizacao(int centroCustoId, int ano, int mes)
        {
            Boolean requerAtualizacao;
            try
            {
                requerAtualizacao = MaosDeObraBo.Instance.VerificarNovaAtualizacao(centroCustoId, ano, mes);
            }
            catch (EntidadeNaoEncontradaException e)
            {
                Response.StatusCode = 404;
                return Json(new {Response.StatusCode, e.Message}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {requerAtualizacao}, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [ActionName("AplicarSomatorioCustos")]
        public ActionResult AplicarSomatorioCustos(int centroCustoId, int aditivoId, int ano, int mes)
        {
            try
            {
                MaosDeObraBo.Instance.AplicarSomatorioCustos(centroCustoId, aditivoId, ano, mes);
            }
            catch (EntidadeNaoEncontradaException e)
            {
                Response.StatusCode = 404;
                return Json(new {Response.StatusCode, e.Message}, JsonRequestBehavior.AllowGet);
            }
            return new EmptyResult();
        }
    }
}