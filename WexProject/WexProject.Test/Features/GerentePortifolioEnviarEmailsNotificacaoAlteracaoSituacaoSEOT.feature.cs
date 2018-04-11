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
    public partial class GerenteDePortifolioEnviarAutomaticamenteEmailsDeNotificacaoSobreAlteracaoDaSituacaoDaSolicitacaoDeOrcamentoFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "GerentePortifolioEnviarEmailsNotificacaoAlteracaoSituacaoSEOT.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Gerente de Portifólio enviar automaticamente emails de notificação sobre alteraçã" +
                    "o da situação da solicitação de orçamento", @"          Dado um Gerente de Portifólio
	Gostaria de enviar automaticamente emails de notificação sobre alteração da situação da solicitação de orçamento
          Então poder acompanhar através do e-mail a evolução da concepção do documento e me manter informado", ProgrammingLanguage.CSharp, new string[] {
                        "pbi_9.26"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "Gerente de Portifólio enviar automaticamente emails de notificação sobre alteraçã" +
                            "o da situação da solicitação de orçamento")))
            {
                WexProject.Test.Features.GerenteDePortifolioEnviarAutomaticamenteEmailsDeNotificacaoSobreAlteracaoDaSituacaoDaSolicitacaoDeOrcamentoFeature.FeatureSetup(null);
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
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("01 - Criar uma nova Solicitação e a mesma deverá poder ser salva com sucesso quan" +
            "do os dados estiverem corretos.")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Gerente de Portifólio enviar automaticamente emails de notificação sobre alteraçã" +
            "o da situação da solicitação de orçamento")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_9.26")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("bug")]
        public virtual void _01_CriarUmaNovaSolicitacaoEAMesmaDeveraPoderSerSalvaComSucessoQuandoOsDadosEstiveremCorretos_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01 - Criar uma nova Solicitação e a mesma deverá poder ser salva com sucesso quan" +
                    "do os dados estiverem corretos.", new string[] {
                        "bug"});
#line 10
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "descrição"});
            table1.AddRow(new string[] {
                        "P&D de Lei de Informática"});
#line 11
   testRunner.Given("os seguintes tipos de solicitacao de orcamento:", ((string)(null)), table1, "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "documento"});
            table2.AddRow(new string[] {
                        "SolicitacaoOrcamento"});
#line 14
   testRunner.And("as seguintes configuracoes de documento:", ((string)(null)), table2, "E ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "documento",
                        "situação",
                        "cc",
                        "cco",
                        "padrão?"});
            table3.AddRow(new string[] {
                        "SolicitacaoOrcamento",
                        "Não Iniciada",
                        "email@email.com.br;emailtwo@email.com.br",
                        "emailtwo@email.com.br;email@email.com.br",
                        "false"});
#line 17
   testRunner.And("as seguintes situacoes de configuracao de documento:", ((string)(null)), table3, "E ");
#line 20
   testRunner.And("ter colaborador(es) \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "nome",
                        "máscara de telefone",
                        "situação",
                        "país padrão?"});
            table4.AddRow(new string[] {
                        "Brasil",
                        "55 00 0000-0000",
                        "Ativo",
                        "True"});
#line 21
   testRunner.And("os seguintes paises:", ((string)(null)), table4, "E ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "sigla",
                        "nome",
                        "email",
                        "pais",
                        "fone/fax"});
            table5.AddRow(new string[] {
                        "FPF",
                        "Fundação Paulo Feitoza",
                        "fpf@email.com",
                        "Brasil",
                        "55 92 0000-0000"});
#line 24
   testRunner.And("as seguintes empresas/instituicoes:", ((string)(null)), table5, "E ");
#line 27
   testRunner.And("usuario logado for \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "responsável",
                        "situação",
                        "tipo de solicitação",
                        "prioridade",
                        "título",
                        "prazo",
                        "cliente"});
            table6.AddRow(new string[] {
                        "colaborador01",
                        "Não Iniciada",
                        "P&D de Lei de Informática",
                        "Alta",
                        "Desenvolvimento de Aplicativos",
                        "30/11/2012",
                        "FPF"});
#line 28
   testRunner.When("as solicitações de orçamento a seguir forem criadas:", ((string)(null)), table6, "Quando ");
#line 31
   testRunner.Then("a solicitação de orçamento estará apta a salvar", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
