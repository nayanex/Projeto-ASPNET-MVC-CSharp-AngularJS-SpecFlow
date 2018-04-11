using System.Web.Optimization;

namespace WexProject.HttpViews
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
		{

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                       "~/Content/bootstrap.css",
                       "~/Content/themes/base/jquery.ui.core.css",
                       "~/Content/themes/base/jquery.ui.resizable.css",
                       "~/Content/themes/base/jquery.ui.selectable.css",
                       "~/Content/themes/base/jquery.ui.accordion.css",
                       "~/Content/themes/base/jquery.ui.autocomplete.css",
                       "~/Content/themes/base/jquery.ui.button.css",
                       "~/Content/themes/base/jquery.ui.dialog.css",
                       "~/Content/themes/base/jquery.ui.slider.css",
                       "~/Content/themes/base/jquery.ui.tabs.css",
                       "~/Content/themes/base/jquery.ui.datepicker.css",
                       "~/Content/themes/base/jquery.ui.progressbar.css",
                       "~/Content/themes/base/jquery.ui.theme.css",
                       "~/Content/reset.css",
                       "~/Content/fonts.css",
                       "~/Content/site.css",
                       "~/Content/resolucaoAjuste.css",
                       "~/Content/themes/temaPadrao.css",
                       "~/Content/hint.css"));

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Scripts/modernizr-2.6.2/modernizr-2.6.2.js",
                        "~/Scripts/jquery/jquery-1.9.1.min.js",
                        "~/Scripts/jquery/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery/jquery.validate-vsdoc.js",
                        "~/Scripts/jquery/jquery.validate.min.js",
                        "~/Scripts/jquery/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery/jquery-ui-1.8.24.min.js",
                        "~/Scripts/jquery/jquery-stickytableheaders.js",
                        "~/Scripts/angular/angular.min.js",
                        "~/Scripts/angular/angular-resource.min.js",
                        "~/Scripts/angular/i18n/angular-locale_pt-br.js",
                        "~/Scripts/angular/angular-translate.min.js",
                        "~/Scripts/angular/angular-translate-loader-static-files.min.js",
						"~/Scripts/angular/ngStorage.min.js",
                        "~/Scripts/ui-bootstrap/ui-bootstrap-tpls-0.9.0.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/app").Include(
						"~/Scripts/wex/main.js",
						"~/Scripts/wex/common.js",
                        "~/Scripts/wex/service.js",
						"~/Scripts/wex/eventos/config.js",
						"~/Scripts/wex/eventos/aditivos.js",
                        "~/Scripts/wex/maodeobra/config.js",
                        "~/Scripts/wex/maodeobra/resource.js",
                        "~/Scripts/wex/notasfiscais/config.js",
                        "~/Scripts/wex/notasfiscais/resource.js",
                        "~/Scripts/wex/rubrica/config.js",
                        "~/Scripts/wex/rubrica/resource.js",
                        "~/Scripts/wex/projeto/config.js",
                        "~/Scripts/wex/projeto/resource.js",
                        "~/Scripts/wex/projeto/view.js",
						"~/Scripts/wex/projetos/config.js",
						"~/Scripts/wex/projetos/selecao.js",
                        "~/Scripts/wex/centrocusto/resource.js",
                        "~/Scripts/wex/colaborador/resource.js",
                        "~/Scripts/wex/custos/config.js",
						"~/Scripts/wex/custos/rubricas.js",
						"~/Scripts/wex/custos/app.js",
						"~/Scripts/wex/custos/termoAditivo.js",
						"~/Scripts/wex/analise/config.js",
						"~/Scripts/wex/analise/custos.js",
						"~/Scripts/wex/analise/geral.js",
						"~/Scripts/wex/situacao/resource.js",
						"~/Scripts/wex/custos/administrativos/config.js",
                        "~/Scripts/wex/custos/administrativos/resource.js",
                        "~/Scripts/wex/custos/administrativos/view.model.js",
                        "~/Scripts/wex/custos/administrativos/command.js",
						"~/Scripts/wex/custos/administrativos.js"));

			//BundleTable.EnableOptimizations = true;
        }
    }
}