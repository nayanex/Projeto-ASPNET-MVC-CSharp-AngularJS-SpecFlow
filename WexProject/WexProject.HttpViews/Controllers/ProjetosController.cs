using System;
using System.Linq;
using System.Web.Mvc;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.RH;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;
using WexProject.Library.Libs.Collection;
using WexProject.Library.Libs.Enumerator;

namespace WexProject.HttpViews.Controllers
{
	public class ProjetosController : Controller
	{
		//
		// GET: /Projetos/

		[HttpGet]
		[ActionName("Projetos")]
		public ActionResult ListarProjetos(Guid? projetoOid)
		{
            if (!Request.AcceptTypes.Contains("application/json"))
            {
                return View("/Views/Projeto/View.cshtml");
            }

			if (projetoOid.HasValue)
			{
				var projeto = ProjetoBo.Instancia.ConsultarProjeto(projetoOid.Value);

				return Json(new { projeto = projeto }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var filtro = new Filtro<ProjetoDto>();

				filtro.Add("Status", (int) CsProjetoSituacaoDomain.EmAndamento);

                //ConsultarProjetos não existe BOs.Projeto, somente no Geral. Isso deve ser refatorado para o BOs.Projeto. Ass.: Ayrton
				var projetos = ProjetoBo.Instancia.ListarProjetos(filtro);

				return Json(new { projetos = projetos }, JsonRequestBehavior.AllowGet);
			}
		}

        [HttpGet]
        [ActionName("Dados")]
        public ActionResult DadosProjeto(Guid idProjeto)
        {
            DadosBasicoProjetoDto projetoDto = ProjetoBo.Instancia.DadosProjeto(idProjeto);
            return Json(projetoDto, JsonRequestBehavior.AllowGet);
        }

		[HttpGet]
		[ActionName("Macros")]
		[Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
		public ActionResult ListarProjetosMacros()
		{
			var projetosMacro = ProjetoBo.Instancia.ListarProjetosMacro();

			return Json(projetosMacro, JsonRequestBehavior.AllowGet);
		}

        [HttpGet]
        [ActionName("Micros")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult ListarProjetosMicros(string IdProjetoMacro)
        {
            if (IdProjetoMacro == null)
            {
                return new HttpNotFoundResult();
            }

            Guid GuidProjetoMacro;

            try
            {
                GuidProjetoMacro = new Guid(IdProjetoMacro);
            }
            catch (FormatException e)
            {
                return new HttpNotFoundResult();
            }

            var projetosMicro = ProjetoBo.Instancia.ListarProjetosMicro(GuidProjetoMacro);

            return Json(projetosMicro, JsonRequestBehavior.AllowGet);

        }

		[HttpGet]
		[ActionName("Situacoes")]
		public ActionResult SituacoesGet()
        //Usar Listar ao invés de Get. Ass.: Ayrton
		{
            //Passar isso para um BO. Ass.: Ayrton
			var situacoes = (from s in Enum.GetValues(typeof(CsProjetoSituacaoDomain)).Cast<int>()
							 select new
							 {
								 Valor = s,
								 Desc = EnumUtil.DescricaoEnum((CsProjetoSituacaoDomain)s)
							 }).ToList();


			return Json(new { situacoes = situacoes }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[ActionName("Classes")]
		public ActionResult ListarClasses()
		{
			var classes = ClasseProjetoBo.ListaClassesProjeto();

			return Json(new { classes = classes }, JsonRequestBehavior.AllowGet);
		}

        [HttpGet]
        [ActionName("Gerentes")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult ListarGerentes()
        {
            var gerentes = ColaboradorBo.Instancia.ListarGerentes();
            return Json(gerentes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("CentrosCustos")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult ListarCentrosCustos()
        {
			var centrosCustos = CentroCustoBo.Instance.ListarCentrosCustos();

            return Json(centrosCustos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("Clientes")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult ListarClientes()
        {
			var clientes = EmpresaInstituicaoBo.Instancia.ListarEmpresasInstituicoes();

            return Json(clientes, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs("POST", "PUT")]
        [ActionName("Projetos")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult SalvarProjeto(DadosBasicoProjetoDto projetoDto)
        {
			try
			{
				projetoDto.IdProjeto = ProjetoBo.Instancia.SalvarProjeto(projetoDto);

				return Json(projetoDto, JsonRequestBehavior.AllowGet);

			}
			catch (ValidationException e)
			{
				Response.StatusCode = 409;

				return Json(new
				{
					codigo = "validacao",
					mensagem = "Erro ao processar os dados do projeto",
					validacoes = e.Validacoes
				}, JsonRequestBehavior.AllowGet);

			}
			catch (ProjetoNaoVazioException e)
			{
				Response.StatusCode = 403;

				return Json(new { message = e.Message });
			}

        }
	}
}
