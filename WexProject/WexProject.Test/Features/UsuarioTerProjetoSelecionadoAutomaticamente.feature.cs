﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18052
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace WexProject.Test.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class UsuarioTerProjetoSelecionadoAutomaticamenteFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "UsuarioTerProjetoSelecionadoAutomaticamente.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "UsuarioTerProjetoSelecionadoAutomaticamente", "", ProgrammingLanguage.CSharp, new string[] {
                        "pbi_1.02"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "UsuarioTerProjetoSelecionadoAutomaticamente")))
            {
                WexProject.Test.Features.UsuarioTerProjetoSelecionadoAutomaticamenteFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "tamanho",
                        "total de ciclos",
                        "ritmo do time"});
            table1.AddRow(new string[] {
                        "safira",
                        "30",
                        "3",
                        "10"});
            table1.AddRow(new string[] {
                        "wex",
                        "30",
                        "3",
                        "10"});
#line 7
 testRunner.Given("que exista(m) o(s) projeto(s) a seguir:", ((string)(null)), table1, "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "login"});
            table2.AddRow(new string[] {
                        "roberto.sousa"});
            table2.AddRow(new string[] {
                        "alexandre.amorim"});
#line 11
    testRunner.And("que existam os usuarios:", ((string)(null)), table2, "E ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("01 - Ao selecionar um projeto essa informação deve ser armazenada no sistema vinc" +
            "ulada ao usuário atual")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "UsuarioTerProjetoSelecionadoAutomaticamente")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_1.02")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rn")]
        public virtual void _01_AoSelecionarUmProjetoEssaInformacaoDeveSerArmazenadaNoSistemaVinculadaAoUsuarioAtual()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01 - Ao selecionar um projeto essa informação deve ser armazenada no sistema vinc" +
                    "ulada ao usuário atual", new string[] {
                        "rn"});
#line 17
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 18
    testRunner.Given("usuario logado for \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 19
  testRunner.When("o projeto \'safira\' for selecionado pelo usuario \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 20
   testRunner.Then("o projeto \'safira\' deveria ter sido salvo como ultimo projeto selecionado para o " +
                    "usuario \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("02 - Ao selecionar outro projeto o sistema deve armazená-lo vinculada ao usuário " +
            "atual")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "UsuarioTerProjetoSelecionadoAutomaticamente")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_1.02")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rn")]
        public virtual void _02_AoSelecionarOutroProjetoOSistemaDeveArmazena_LoVinculadaAoUsuarioAtual()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("02 - Ao selecionar outro projeto o sistema deve armazená-lo vinculada ao usuário " +
                    "atual", new string[] {
                        "rn"});
#line 23
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 24
    testRunner.Given("usuario logado for \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 25
    testRunner.And("que o projeto \'safira\' tenha sido selecionado anteriormente pelo usuario \'roberto" +
                    ".sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 26
  testRunner.When("o projeto \'wex\' for selecionado pelo usuario \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 27
   testRunner.Then("o projeto \'wex\' deveria ter sido salvo como ultimo projeto selecionado para o usu" +
                    "ario \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("03 - Quando o usuário autenticar no sistema o filtro de projeto deve vim preenchi" +
            "do com o ultimo registro selecionado")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "UsuarioTerProjetoSelecionadoAutomaticamente")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_1.02")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rn")]
        public virtual void _03_QuandoOUsuarioAutenticarNoSistemaOFiltroDeProjetoDeveVimPreenchidoComOUltimoRegistroSelecionado()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("03 - Quando o usuário autenticar no sistema o filtro de projeto deve vim preenchi" +
                    "do com o ultimo registro selecionado", new string[] {
                        "rn"});
#line 30
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 31
    testRunner.Given("que o projeto \'safira\' tenha sido selecionado anteriormente pelo usuario \'roberto" +
                    ".sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 32
  testRunner.When("usuario logado for \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 33
   testRunner.Then("o projeto \'safira\' deve ficar preenchido automaticamente", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("04 - Quando o usuário autenticar no sistema e nao tiver selecionado nenhum projet" +
            "o anteriormente")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "UsuarioTerProjetoSelecionadoAutomaticamente")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_1.02")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rn")]
        public virtual void _04_QuandoOUsuarioAutenticarNoSistemaENaoTiverSelecionadoNenhumProjetoAnteriormente()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("04 - Quando o usuário autenticar no sistema e nao tiver selecionado nenhum projet" +
                    "o anteriormente", new string[] {
                        "rn"});
#line 36
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 37
 testRunner.Given("que nenhum projeto tenha sido selecionado anteriormente pelo usuario \'alexandre.a" +
                    "morim\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 38
  testRunner.When("usuario logado for \'alexandre.amorim\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 39
   testRunner.Then("o filtro de projeto deve ficar em branco", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("05 - O sistema nao deve preencher automaticamente projetos selecionados por outro" +
            "s usuários")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "UsuarioTerProjetoSelecionadoAutomaticamente")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_1.02")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("rn")]
        public virtual void _05_OSistemaNaoDevePreencherAutomaticamenteProjetosSelecionadosPorOutrosUsuarios()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("05 - O sistema nao deve preencher automaticamente projetos selecionados por outro" +
                    "s usuários", new string[] {
                        "rn"});
#line 42
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 43
    testRunner.Given("que o projeto \'safira\' tenha sido selecionado anteriormente pelo usuario \'roberto" +
                    ".sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 44
    testRunner.And("que o projeto \'wex\' tenha sido selecionado anteriormente pelo usuario \'alexandre." +
                    "amorim\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 45
  testRunner.When("usuario logado for \'roberto.sousa\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 46
   testRunner.Then("o projeto \'safira\' deve ficar preenchido automaticamente", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
