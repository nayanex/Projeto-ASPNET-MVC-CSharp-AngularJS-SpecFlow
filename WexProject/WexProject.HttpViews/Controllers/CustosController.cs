using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.DTOs.Projeto;
using WexProject.Library.Exceptions.Geral;
using WexProject.Library.Libs.Collection;
using WexProject.Library.Libs.Generic;
using WexProject.Library.Libs.Str;
using ProjetoDto = WexProject.BLL.Shared.DTOs.Custos.ProjetoDto;

namespace WexProject.HttpViews.Controllers
{
    public class CustosController : Controller
    {
        // GET: /Custos/

        /// <summary>
        ///     Página Inicial da área de custos
        /// </summary>
        /// <param name="id">Id do Projeto</param>
        /// <returns>Página de custos do projeto ou página de seleção de projeto</returns>
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult Index(Guid? id = null)
        {
            if (id != null)
            {
                Guid oid = id ?? Guid.Empty;

				ProjetoDto projetoDto = ProjetoBo.Instancia.ConsultarProjeto(oid);

                ViewBag.StatusList = new SelectList(from s in Enum.GetNames(typeof (CsProjetoSituacaoDomain))
                    select new
                    {
                        Text = StrUtil.InserirEspacosStringPascalCase(s),
                        Value = s,
                    }, "Value", "Text");

                return View(projetoDto);
            }
            ViewBag.StatusList = new SelectList(new List<SelectListItem>());

            return View(new ProjetoDto());
        }

        [ChildActionOnly]
        public ActionResult TodosOsProjetos()
        {
            List<ProjetoDto> projetos = ProjetoBo.Instancia.ListarProjetos();

            return PartialView("_ListaProjetos", projetos);
        }

        [HttpGet]
        [ActionName("Projetos")]
        public ActionResult ListarProjetosMacros(Guid? id = null)
        {
            ModeloDinamico<ProjetoDto> modelo = null;
            NameValueCollection queryString;

            queryString = new NameValueCollection(Request.QueryString);

            try
            {
                string campos = queryString.Get("campos");

                if (campos != null)
                {
                    modelo = new ModeloDinamico<ProjetoDto>(campos);
                    queryString.Remove("campos");
                }
            }
            catch (PropriedadeNaoExisteException e)
            {
                // Será que deve ser 400 ou 409 nesse caso?
                Response.StatusCode = 400;
                return Json(new {message = e.Message}, JsonRequestBehavior.AllowGet);
            }

            if (id.HasValue)
            {
                dynamic projeto;

                if (modelo == null)
                {
					projeto = ProjetoBo.Instancia.ConsultarProjeto(id.Value);
                }
                else
                {
                    projeto = ProjetoBo.Instancia.ConsultarProjeto(modelo, id.Value);
                }

                return Json(new {projeto}, JsonRequestBehavior.AllowGet);
            }
            dynamic projetos;

            try
            {
                Filtro<ProjetoDto> filtro = queryString.CriarFiltro<ProjetoDto>();

                // Somente projetos Macros
                filtro.Add("ProjetoMacroOid", null);

                if (modelo == null)
                {
                    projetos = ProjetoBo.Instancia.ListarProjetos(filtro);
                }
                else
                {
                    projetos = ProjetoBo.Instancia.ListarProjetos(modelo, filtro);
                }
            }
            catch (PropriedadeNaoExisteException e)
            {
                Response.StatusCode = 409;
                return Json(new {message = e.Message}, JsonRequestBehavior.AllowGet);
            }

            return Json(new {projetos}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("Aditivos")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult AditivosGet(Guid projetoOid)
        {
            List<AditivoDto> aditivos = AditivoBo.Instance.ListarAditivos(projetoOid);

            return Json(new {aditivos}, JsonRequestBehavior.AllowGet);
        }

        // GET: Custos/Aditivo/{aditivoId}

        [HttpGet]
        [ActionName("Aditivo")]
        [Authorize(Roles = "Sudo, Desenvolvimento, Administrativo, Aportes")]
        public ActionResult AditivoGet(int aditivoId = -1)
        {
            if (aditivoId > 0)
            {
                AditivoDto aditivo = AditivoBo.Instance.PesquisarAditivo(aditivoId);

                if (HttpContext.Request.AcceptTypes.Contains("application/json"))
                {
                    return Json(new {aditivo}, JsonRequestBehavior.AllowGet);
                }
                return new EmptyResult();
            }
            return new EmptyResult();
        }

        // POST: Custos/Aditivo/{aditivoId}

        [AcceptVerbs("POST", "PUT")]
        [ActionName("Aditivo")]
        public ActionResult AditivoPostPut(AditivoDto aditivoDto)
        {
            TryValidateModel(aditivoDto);

            if (ModelState.IsValid)
            {
                int id = AditivoBo.Instance.SalvarAditivo(aditivoDto);

                return Json(new {id});
            }

            var errosValidacao = (from c in ModelState.Keys
                where ModelState[c].Errors.Count > 0
                select new
                {
                    chave = c,
                    erros = ModelState[c].Errors.Select(e => e.ErrorMessage).ToArray()
                }).ToArray();

            return Json(errosValidacao);
        }

        // DELETE: Custos/Aditivo/{aditivoId}

        [HttpDelete]
        [ActionName("Aditivo")]
        public ActionResult AditivoDelete(int aditivoId, Boolean force = false)
        {
            int id;
            try
            {
                id = AditivoBo.Instance.RemoverAditivo(aditivoId, force);
            }
            catch (AditivoNaoVazioException e)
            {
                Response.StatusCode = 405;

                return Json(new {message = e.Message});
            }

            return Json(new {id});
        }

        [HttpGet]
        [ActionName("PatrocinadorAditivo")]
        public ActionResult PatrocinadorAditivoGet(int aditivoId)
        {
            List<EmpresaInstituicaoDto> patrocinadores = AditivoBo.Instance.ListarPatrocinadores(aditivoId);

            return Json(new {patrocinadores}, JsonRequestBehavior.AllowGet);
        }

        // POST: Custos/Aditivo/{aditivoId}/Patrocinadores/{patrocinadorOid}

        [HttpPost]
        [ActionName("PatrocinadorAditivo")]
        public ActionResult PatrocinadorAditivoPost(int aditivoId, Guid patrocinadorOid)
        {
            int id = AditivoBo.Instance.AssociarPatrocinador(aditivoId, patrocinadorOid);

            return Json(new {id});
        }

        // DELETE /Custos/Aditivo/{aditivoId}/Patrocinadores/{patrocinadorOid}

        [HttpDelete]
        [ActionName("PatrocinadorAditivo")]
        public ActionResult PatrocinadorAditivoDelete(int aditivoId, Guid patrocinadorOid)
        {
            int id = AditivoBo.Instance.DesassociarPatrocinador(aditivoId, patrocinadorOid);

            return Json(new {id});
        }

        [HttpGet]
        [ActionName("CentroCustoAditivo")]
        public ActionResult CentroCustoAditivoGet(int aditivoId)
        {
            List<CentroCustoDto> centrosCusto = AditivoBo.Instance.ListarCentrosCustos(aditivoId);

            return Json(new {centrosCusto}, JsonRequestBehavior.AllowGet);
        }

        // POST: Custos/Aditivo/{aditivoId}/CentrosCusto/{centroCustoId}

        [HttpPost]
        [ActionName("CentroCustoAditivo")]
        public ActionResult CentroCustoAditivoPost(int aditivoId, int centroCustoId)
        {
            int id = AditivoBo.Instance.AssociarCentroCusto(aditivoId, centroCustoId);

            return Json(new {id});
        }

        // DELETE: Custos/Aditivo/{aditivoId}/CentrosCusto/{centroCustoId}

        [HttpDelete]
        [ActionName("CentroCustoAditivo")]
        public ActionResult CentroCustoAditivoDelete(int aditivoId, int centroCustoId)
        {
            int id = AditivoBo.Instance.DesassociarCentroCusto(aditivoId, centroCustoId);

            return Json(new {id});
        }

        [HttpGet]
        [ActionName("RubricaAditivo")]
        [Authorize]
        public ActionResult RubricaAditivoGet(int aditivoId, int? rubricaId = null,
            CsClasseRubrica classe = CsClasseRubrica.Tudo)
        {
            bool userHasAccess = Roles.IsUserInRole(User.Identity.Name, classe.ToString());
            bool isSuperUser = Roles.IsUserInRole(User.Identity.Name, "Sudo");
            bool isGerenteDesenvolvimento = Roles.IsUserInRole(User.Identity.Name, "Desenvolvimento");

            //Liberando acesso ao papel de Desenvolvimento às rubricas administrativas
            if (isGerenteDesenvolvimento && classe.ToString() == "Administrativo")
            {
                userHasAccess = true;
            }

            if (isSuperUser)
            {
                userHasAccess = true;
            }

            if (userHasAccess && HttpContext.Request.AcceptTypes.Contains("application/json"))
            {
                if (rubricaId.HasValue)
                {
                    RubricaDto rubrica = RubricaBo.Instance.PesquisarRubrica(rubricaId.Value);
                    return Json(new {rubrica}, JsonRequestBehavior.AllowGet);
                }
                List<RubricaDto> rubricas;

                if (classe == CsClasseRubrica.Tudo)
                {
                    rubricas = RubricaBo.Instance.ListarRubricas(aditivoId);
                }
                else
                {
                    rubricas = RubricaBo.Instance.PesquisarRubricas(aditivoId, classe);
                }

                return Json(new {rubricas}, JsonRequestBehavior.AllowGet);
            }
            return new EmptyResult();
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put)]
        [ActionName("RubricaAditivo")]
        public ActionResult RubricaAditivoPostPut(RubricaDto rubricaDto)
        {
            int id;
            int? paiId;

            try
            {
                id = RubricaBo.Instance.SalvarRubrica(rubricaDto, out paiId);
            }
            catch (RubricaTipoDiferenteException e)
            {
                Response.StatusCode = 409;

                return Json(new {message = e.Message});
            }
            catch (ProjetoSemTipoException e)
            {
                Response.StatusCode = 409;

                return Json(new {message = e.Message});
            }

            return Json(new {id, pai = paiId});
        }

        [HttpDelete]
        [ActionName("RubricaAditivo")]
        public ActionResult RubricaAditivoDelete(int rubricaId, Boolean force = false)
        {
            int id = 0;
            List<int> filhos;

            try
            {
                id = RubricaBo.Instance.RemoverRubrica(rubricaId, force, out filhos);
            }
            catch (RubricaNaoVaziaException e)
            {
                Response.StatusCode = 405;

                return Json(new {message = e.Message});
            }

            return Json(new {id, filhos});
        }

        //TODO: Corrigir retorno (Retornar todos os ids)
        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put)]
        [ActionName("RubricaMesAditivo")]
        public ActionResult RubricaMesAditivoPostPut(RubricaMesDto rubricaMesDto)
        {
            int id = 0;

            id = RubricaMesBo.Instance.SalvarRubricaMes(rubricaMesDto);

            return Json(new {id});
        }

        [HttpGet]
        [ActionName("TiposRubricas")]
        [Authorize]
        public ActionResult TiposRubricasGet(int? tipoProjetoId)
        {
            bool isSuperUser = Roles.IsUserInRole(User.Identity.Name, "Sudo");
            bool isGerenteDesenvolvimento = Roles.IsUserInRole(User.Identity.Name, "Desenvolvimento");

            List<TipoRubricaDto> tiposRubricas;

            if (tipoProjetoId.HasValue)
            {
                tiposRubricas = TipoRubricaBo.Instance.ListarTiposRubricas(tipoProjetoId.Value);
            }
            else
            {
                tiposRubricas = TipoRubricaBo.Instance.ListarTiposRubricas();
            }

			List<string> classesNomes = new List<string>();
			Dictionary<string, int> classesMap = new Dictionary<string, int>();

			foreach (CsClasseRubrica classe in Enum.GetValues(typeof(CsClasseRubrica)))
			{
				var nomeClasse = classe.ToString();

				if ((classe != CsClasseRubrica.Tudo && (classe & CsClasseRubrica.Tudo) != 0) &&
					(isSuperUser || Roles.IsUserInRole(User.Identity.Name, nomeClasse) ||
					(classe == CsClasseRubrica.Administrativo && isGerenteDesenvolvimento)))
				{
					classesNomes.Add(nomeClasse);
				}

				classesMap.Add(nomeClasse, (int)classe);
			}

            return Json(new {tipos = tiposRubricas, classes = classesNomes, classesMap},
                JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}