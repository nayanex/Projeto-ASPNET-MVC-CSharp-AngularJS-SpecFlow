﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.32559
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
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Manter o total de pontos dos modulos inalterados quando o total de pontos do proj" +
        "eto sofrer alteracao.")]
    public partial class ManterOTotalDePontosDosModulosInalteradosQuandoOTotalDePontosDoProjetoSofrerAlteracao_Feature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "GestorProdutoManterOTotalDePontosDosModulosInalterado.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Manter o total de pontos dos modulos inalterados quando o total de pontos do proj" +
                    "eto sofrer alteracao.", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("01 - Um projeto é criado e posteriormente tem seu total de pontos alterado.")]
        [NUnit.Framework.CategoryAttribute("bug")]
        public virtual void _01_UmProjetoECriadoEPosteriormenteTemSeuTotalDePontosAlterado_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01 - Um projeto é criado e posteriormente tem seu total de pontos alterado.", new string[] {
                        "bug"});
#line 6
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "tamanho",
                        "total de ciclos",
                        "ritmo do time"});
            table1.AddRow(new string[] {
                        "projeto01",
                        "50",
                        "1",
                        "10"});
#line 7
  testRunner.Given("que exista(m) o(s) projeto(s) a seguir:", ((string)(null)), table1, "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "tamanho",
                        "modulo pai"});
            table2.AddRow(new string[] {
                        "modulo01",
                        "20",
                        ""});
            table2.AddRow(new string[] {
                        "modulo02",
                        "15",
                        ""});
            table2.AddRow(new string[] {
                        "modulo03",
                        "5",
                        ""});
#line 10
     testRunner.And("que existam os seguintes modulos para o \'projeto01\':", ((string)(null)), table2, "E ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "tamanho",
                        "total de ciclos",
                        "ritmo do time"});
            table3.AddRow(new string[] {
                        "projeto01",
                        "100",
                        "1",
                        "10"});
#line 15
  testRunner.And("o projeto \'projeto01\' tenha seu tamanho alterado para \'100\'", ((string)(null)), table3, "E ");
#line 18
  testRunner.When("o projeto \'projeto01\' for salvo:", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "tamanho",
                        "modulo pai"});
            table4.AddRow(new string[] {
                        "modulo01",
                        "20",
                        ""});
            table4.AddRow(new string[] {
                        "modulo02",
                        "15",
                        ""});
            table4.AddRow(new string[] {
                        "modulo03",
                        "5",
                        ""});
#line 19
  testRunner.Then("os modulos a seguir do projeto \'projeto01\' devem estar com os seguintes valores:", ((string)(null)), table4, "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion