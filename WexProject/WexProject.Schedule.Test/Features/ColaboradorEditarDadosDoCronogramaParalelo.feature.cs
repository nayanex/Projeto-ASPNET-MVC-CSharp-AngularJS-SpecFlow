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
namespace WexProject.Schedule.Test.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class OColaboradorEditarOsDadosDoCronogramaFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ColaboradorEditarDadosDoCronogramaParalelo.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "O colaborador editar os dados do cronograma", "\tgerenciar a edição dos dados do cronograma", ProgrammingLanguage.CSharp, new string[] {
                        "CronogramaPresenter"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "O colaborador editar os dados do cronograma")))
            {
                WexProject.Schedule.Test.Features.OColaboradorEditarOsDadosDoCronogramaFeature.FeatureSetup(null);
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
#line 8
 testRunner.Given("que exista o servidor \'localhost\' com a porta \'8000\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "inicio",
                        "final"});
            table1.AddRow(new string[] {
                        "C1",
                        "05/06/2014",
                        "09/06/2014"});
            table1.AddRow(new string[] {
                        "C2",
                        "04/06/2014",
                        "08/06/2014"});
#line 9
 testRunner.Given("que exista(m) o(s) cronograma(s)", ((string)(null)), table1, "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "login"});
            table2.AddRow(new string[] {
                        "gabriel",
                        "gabriel.matos"});
            table2.AddRow(new string[] {
                        "anderson",
                        "anderson.lins"});
#line 13
 testRunner.And("que existam os usuarios no cronograma \'C1\':", ((string)(null)), table2, "E ");
#line 17
 testRunner.And("que o cronograma \'C1\' esta sendo utilizado pelo usuario \'gabriel\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("01.01 O usuario iniciar a edicao de dados do cronograma")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "O colaborador editar os dados do cronograma")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CronogramaPresenter")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("Dto")]
        public virtual void _01_01OUsuarioIniciarAEdicaoDeDadosDoCronograma()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01.01 O usuario iniciar a edicao de dados do cronograma", new string[] {
                        "Dto"});
#line 19
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 20
 testRunner.When("o cronograma atual iniciar a edicao de dados do cronograma", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 21
  testRunner.Then("o cronograma atual devera solicitar a permissao de edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("02.01 O usuario iniciar a edicao de dados do cronograma e receber a permissao de " +
            "edicao enquanto estiver editando")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "O colaborador editar os dados do cronograma")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CronogramaPresenter")]
        public virtual void _02_01OUsuarioIniciarAEdicaoDeDadosDoCronogramaEReceberAPermissaoDeEdicaoEnquantoEstiverEditando()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("02.01 O usuario iniciar a edicao de dados do cronograma e receber a permissao de " +
                    "edicao enquanto estiver editando", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 24
 testRunner.When("o cronograma atual iniciar a edicao de dados do cronograma", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 25
   testRunner.And("o cronograma atual recebeu a permissao de edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 26
  testRunner.Then("o cronograma atual deve se manter em edicao", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line 27
 testRunner.When("o cronograma atual encerrar a edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 28
  testRunner.Then("o cronograma atual devera comunicar automaticamente o fim da edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("02.02 O usuario iniciar a edicao de dados do cronograma e receber a permissao de " +
            "edicao quando tiver encerrado a edicao")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "O colaborador editar os dados do cronograma")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CronogramaPresenter")]
        public virtual void _02_02OUsuarioIniciarAEdicaoDeDadosDoCronogramaEReceberAPermissaoDeEdicaoQuandoTiverEncerradoAEdicao()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("02.02 O usuario iniciar a edicao de dados do cronograma e receber a permissao de " +
                    "edicao quando tiver encerrado a edicao", ((string[])(null)));
#line 30
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 31
 testRunner.When("o cronograma atual iniciar a edicao de dados do cronograma", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 32
   testRunner.And("o cronograma atual encerrar a edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 33
      testRunner.And("o cronograma atual recebeu a permissao de edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 34
  testRunner.Then("o cronograma atual devera comunicar automaticamente o fim da edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("03.01 O usuario iniciar a edicao de dados do cronograma e for recusado enquanto e" +
            "stiver editando")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "O colaborador editar os dados do cronograma")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CronogramaPresenter")]
        public virtual void _03_01OUsuarioIniciarAEdicaoDeDadosDoCronogramaEForRecusadoEnquantoEstiverEditando()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("03.01 O usuario iniciar a edicao de dados do cronograma e for recusado enquanto e" +
                    "stiver editando", ((string[])(null)));
#line 36
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 37
 testRunner.When("o cronograma atual iniciar a edicao de dados do cronograma", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 38
      testRunner.And("o cronograma atual recebeu a recusa de edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 39
  testRunner.Then("o cronograma atual deve forcar o fim da edicao", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line 40
   testRunner.And("o cronograma devera atualizar os dados a partir do servico", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("03.02 O usuario iniciar a edicao de dados do cronograma e for recusado quando tiv" +
            "er encerrado a edicao")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "O colaborador editar os dados do cronograma")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CronogramaPresenter")]
        public virtual void _03_02OUsuarioIniciarAEdicaoDeDadosDoCronogramaEForRecusadoQuandoTiverEncerradoAEdicao()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("03.02 O usuario iniciar a edicao de dados do cronograma e for recusado quando tiv" +
                    "er encerrado a edicao", ((string[])(null)));
#line 42
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 43
 testRunner.When("o cronograma atual iniciar a edicao de dados do cronograma", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 44
   testRunner.And("o cronograma atual encerrar a edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 45
      testRunner.And("o cronograma atual recebeu a recusa de edicao dos dados", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 46
  testRunner.Then("o cronograma devera atualizar os dados a partir do servico", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
