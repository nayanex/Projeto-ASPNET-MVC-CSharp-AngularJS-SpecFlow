using System.Web.Mvc;
using System.Web.Routing;
using WexProject.Library.Libs.Web.Mvc;

namespace WexProject.HttpViews
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            MapearCustosAdministrativos(routes);
			MapearNotasFiscais(routes);

            routes.MapRoute("CentroCustoAditivo", "Custos/Aditivo/{aditivoId}/CentrosCusto/{centroCustoId}",
                new {controller = "Custos", action = "CentroCustoAditivo", centroCustoId = UrlParameter.Optional}
                );

            routes.MapRoute("PatrocinadorAditivo", "Custos/Aditivo/{aditivoId}/Patrocinadores/{patrocinadorOid}",
                new {controller = "Custos", action = "PatrocinadorAditivo", patrocinadorOid = UrlParameter.Optional}
                );

            routes.MapRoute("RubricaAditivo", "Custos/Aditivo/{aditivoId}/Rubricas/{rubricaId}",
                new {controller = "Custos", action = "RubricaAditivo", rubricaId = UrlParameter.Optional}
                );

            routes.MapRoute("RubricaMesAditivo", "Custos/Aditivo/{aditivoId}/RubricasMeses/{RubricaMesId}",
                new {controller = "Custos", action = "RubricaMesAditivo", RubricaMesId = UrlParameter.Optional}
                );

            routes.MapRoute("Aditivo", "Custos/Aditivo/{aditivoId}",
                new {controller = "Custos", action = "Aditivo", aditivoId = UrlParameter.Optional}
                );

            routes.MapRoute("TiposRubricas", "Custos/Rubricas/Tipos",
                new {controller = "Custos", action = "TiposRubricas"}
                );

            routes.MapRoute("AditivoProjeto", "Custos/{projetoOid}/Aditivos",
                new {controller = "Custos", action = "Aditivos"}
                );

            routes.MapRoute("Custos", "Custos/{id}",
                new {controller = "Custos", action = "Index", id = UrlParameter.Optional},
                new {id = new GuidConstraint(true)}
                );

            routes.MapRoute("ProjetosDefault", "Projetos/{action}", new {controller = "Projetos"},
                new {action = "[a-zA-Z]+"}
                );

            routes.MapRoute("Projetos", "Projetos/{projetoOid}",
                new {controller = "Projetos", action = "Projetos", projetoOid = UrlParameter.Optional}
                );

            routes.MapRoute("Geral", "Analise/Custos/Geral/{tipo}/{projetoOid}",
                new
                {
                    controller = "Analise",
                    action = "Geral",
                    tipo = UrlParameter.Optional,
                    projetoOid = UrlParameter.Optional
                }, new {tipo = @"(Planejado|Real|)"}
                );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }

        /// <summary>
        /// </summary>
        /// <param name="routes"></param>
        private static void MapearCustosAdministrativos(RouteCollection routes)
        {
            routes.MapRoute("SalvarDespesaRealProjeto",
                "custos/administrativos/{ano}-{mes}/rubricas/{tipoRubricaId}/projetos/{projetoOid}/despesa-real", new
                {
                    controller = "CustosRubricasAdministrativas",
                    action = "despesa-real",
                }, new {tipoRubricaId = @"\d+"}
            );

            routes.MapRoute("CustosAdministrativosProjetos",
                "custos/administrativos/{ano}-{mes}/rubricas/{tipoRubricaId}/projetos",
                new {controller = "CustosRubricasAdministrativas", action = "projetos"}, new {tipoRubricaId = @"\d+"}
            );

            routes.MapRoute("CustosAdministrativosRubricas",
                "custos/administrativos/{ano}-{mes}/rubricas",
                new {controller = "CustosRubricasAdministrativas", action = "rubricas"}
            );

            routes.MapRoute("TelaCustosAdministrativos",
                "custos/administrativos",
                new {controller = "CustosRubricasAdministrativas", action = "index"}
            );
        }

		private static void MapearNotasFiscais(RouteCollection routes)
		{
			routes.MapRoute("NotaFiscalCentroCusto", "Custos/CentrosCustos/{centroCustoId}/NotasFiscais/{notaFiscalId}",
				new { controller = "NotasFiscais", action = "NotaFiscalCentroCusto", notaFiscalId = UrlParameter.Optional }
				);

			routes.MapRoute("NotasFiscaisTiposRubricas", "Custos/Aditivos/{aditivoId}/NotasFiscais/TiposRubricas",
				new { controller = "Rubricas", action = "NotasFiscaisTiposRubricas" }
				);

			routes.MapRoute("NotaFiscalRubrica", "Custos/Aditivos/{aditivoId}/Rubricas/{rubricaId}/NotasFiscais/{notaFiscalId}",
				new { controller = "NotasFiscais", action = "NotaFiscalRubrica", notaFiscalId = UrlParameter.Optional }
				);
		}

    }

}