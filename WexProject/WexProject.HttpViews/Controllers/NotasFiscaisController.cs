using System.Collections.Generic;
using System.Web.Mvc;
using WexProject.BLL.BOs.TotvsWex;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;
using WexProject.BLL.Shared.DTOs.TotvsWex;

namespace WexProject.HttpViews.Controllers
{
    public class NotasFiscaisController : Controller
    {
        [HttpGet]
        [ActionName("NotaFiscalCentroCusto")]
        public ActionResult Index(int centroCustoId, int ano = 0, int mes = 0)
        {
            var centroCustoDto = new CentroCustoDto {IdCentroCusto = centroCustoId};

            IEnumerable<NotaFiscalDto> notasFiscaisDto =
                NotasFiscaisBo.Instance.ListarNotasFiscais(centroCustoDto, ano, mes);

            return Json(new {notasFiscaisDto}, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        [ActionName("NotaFiscalCentroCusto")]
        public ActionResult Atualizar(NotaFiscalDto notaFiscalDto)
        {
            NotasFiscaisBo.Instance.AtualizarNotaFiscal(notaFiscalDto);
            return new EmptyResult();
        }

        [HttpGet]
        [ActionName("NotaFiscalRubrica")]
        public ActionResult ListarNotasFiscaisRubrica(int rubricaId, int ano, int mes)
        {
            var rubricaDto = new RubricaDto {RubricaId = rubricaId};

            IEnumerable<NotaFiscalDto> notasFiscaisDto =
                NotasFiscaisBo.Instance.ListarNotasFiscais(rubricaDto, ano, mes);

            return Json(new {notasFiscaisDto}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("NotaFiscalRubrica")]
        public ActionResult AssociarNotasFiscais(NotaFiscalDto notaFiscalDto, int rubricaId)
        {
            NotasFiscaisBo.Instance.AssociarNotaFiscal(notaFiscalDto, rubricaId);
            return new EmptyResult();
        }

        [HttpDelete]
        [ActionName("NotaFiscalRubrica")]
        public ActionResult DesassociarNotasFiscais(int rubricaId, int notaFiscalId)
        {
            NotasFiscaisBo.Instance.DesassociarNotaFiscal(rubricaId, notaFiscalId);
            return new EmptyResult();
        }

        [HttpGet]
        [ActionName("Importar")]
        public ActionResult Importar()
        {
            NotasFiscaisBo.Instance.Importar();
            return Json(new {success = true}, JsonRequestBehavior.AllowGet);
        }
    }
}