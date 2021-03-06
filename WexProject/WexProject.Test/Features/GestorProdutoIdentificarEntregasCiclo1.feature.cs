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
    [NUnit.Framework.DescriptionAttribute("O Gestor do Produto definir as entregas de um ciclo de desenvolvimento")]
    [NUnit.Framework.CategoryAttribute("pbi_3.1")]
    public partial class OGestorDoProdutoDefinirAsEntregasDeUmCicloDeDesenvolvimentoFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "GestorProdutoIdentificarEntregasCiclo.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "O Gestor do Produto definir as entregas de um ciclo de desenvolvimento", @"Como um Gestor do Produto
Eu quero identificar quais entregas priorizadas farão parte de um ciclo de desenvolvimento
Para que eu possa informar ao time de desenvolvimento a meta do ciclo, de forma que as entregas agreguem valor ao negócio da minha empresa.", ProgrammingLanguage.CSharp, new string[] {
                        "pbi_3.1"});
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
        [NUnit.Framework.DescriptionAttribute("[BUG] - Um colaborador troca o status de uma estória de pronto para replanejado")]
        [NUnit.Framework.CategoryAttribute("rf_4.02")]
        [NUnit.Framework.CategoryAttribute("bug")]
        public virtual void BUG_UmColaboradorTrocaOStatusDeUmaEstoriaDeProntoParaReplanejado()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[BUG] - Um colaborador troca o status de uma estória de pronto para replanejado", new string[] {
                        "rf_4.02",
                        "bug"});
#line 10
this.ScenarioSetup(scenarioInfo);
#line 11
     testRunner.Given("um projeto \'Teste\' com o(s) ciclo(s) \'ciclo 1\', \'ciclo 2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "estória",
                        "situação",
                        "pontos",
                        "tipo",
                        "valor do negócio"});
            table1.AddRow(new string[] {
                        "Teste 01",
                        "Pronto",
                        "5",
                        "EscopoContratado",
                        "Mandatorio"});
            table1.AddRow(new string[] {
                        "Teste 02",
                        "Replanejado",
                        "3",
                        "EscopoContratado",
                        "Mandatorio"});
#line 12
     testRunner.And("ciclo \'1\' do projeto \'Teste\' na situação \'Concluido\' com as estórias:", ((string)(null)), table1, "E ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "estória",
                        "situação",
                        "pontos",
                        "tipo",
                        "valor do negócio"});
            table2.AddRow(new string[] {
                        "Teste 03",
                        "NaoIniciado",
                        "5",
                        "EscopoContratado",
                        "Mandatorio"});
#line 16
  testRunner.And("ciclo \'2\' do projeto \'Teste\' na situação \'NaoPlanejado\' com as estórias:", ((string)(null)), table2, "E ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "estória",
                        "situação"});
            table3.AddRow(new string[] {
                        "Teste 01",
                        "Replanejado"});
#line 19
   testRunner.When("mudar a situacao do ciclo \'ciclo 1\' para \'Em Andamento\'", ((string)(null)), table3, "Quando ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "estória",
                        "situação"});
            table4.AddRow(new string[] {
                        "Teste 01",
                        "Replanejado"});
            table4.AddRow(new string[] {
                        "Teste 02",
                        "Replanejado"});
#line 22
      testRunner.Then("as estorias do projeto \'Teste\' devem estar com a situacao", ((string)(null)), table4, "Então ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
