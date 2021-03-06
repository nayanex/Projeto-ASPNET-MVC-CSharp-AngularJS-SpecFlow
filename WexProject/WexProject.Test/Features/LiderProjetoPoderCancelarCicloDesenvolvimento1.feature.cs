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
    [NUnit.Framework.DescriptionAttribute("Cancelamento de ciclo de desenvolvimento do projeto")]
    [NUnit.Framework.CategoryAttribute("pbi_4.06")]
    public partial class CancelamentoDeCicloDeDesenvolvimentoDoProjetoFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "LiderProjetoPoderCancelarCicloDesenvolvimento.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Cancelamento de ciclo de desenvolvimento do projeto", "     Como um Líder de Projeto\r\n     Eu quero poder cancelar um ciclo de desenvolv" +
                    "imento\r\n     Para que eu possa registrar quando um ciclo não pode ser realizado " +
                    "por interferências externas e terei todos os meus ciclos seguintes replanejados", ProgrammingLanguage.CSharp, new string[] {
                        "pbi_4.06"});
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
        [NUnit.Framework.DescriptionAttribute("01 - Excluir um motivo de cancelamento que está sendo usado (RF_3.05)")]
        public virtual void _01_ExcluirUmMotivoDeCancelamentoQueEstaSendoUsadoRF_3_05()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01 - Excluir um motivo de cancelamento que está sendo usado (RF_3.05)", ((string[])(null)));
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
  testRunner.Given("um motivo de cancelamento \'motivo 01\' usado no cancelamento de um ciclo \'ciclo 01" +
                    "\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 11
  testRunner.When("excluir o motivo de cancelamento \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 12
  testRunner.Then("exibir a excessão \'O Motivo de Cancelamento está sendo usado por um Cancelamento " +
                    "de Ciclo\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("02 - Cancelar um ciclo com a situação diferente de \"Não Iniciado\" (RF_4.02)")]
        public virtual void _02_CancelarUmCicloComASituacaoDiferenteDeNaoIniciadoRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("02 - Cancelar um ciclo com a situação diferente de \"Não Iniciado\" (RF_4.02)", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 15
  testRunner.Given("ciclo \'ciclo 01\' na situação \'Em andamento\' com as estórias: \'estória 01\' - situa" +
                    "ção \'Não Iniciado\', \'estória 02\' - situação \'Em Desenvolvimento\', \'estória 03\' -" +
                    " situação \'Em Desenvolvimento\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 16
  testRunner.When("cancelar o ciclo \'ciclo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 17
  testRunner.Then("exibir mensagem \"Não se pode cancelar um Ciclo que não esteja com a situação \'Não" +
                    " Iniciado\'\" para o cancelamento do ciclo \'ciclo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("03 - Obter a data sugerida no cancelamento de Ciclo (RF_4.02)")]
        public virtual void _03_ObterADataSugeridaNoCancelamentoDeCicloRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("03 - Obter a data sugerida no cancelamento de Ciclo (RF_4.02)", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 21
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 22
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 23
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 24
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 25
  testRunner.When("cancelar o ciclo \'ciclo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 26
  testRunner.Then("o campo \"data de início do próximo ciclo\" deve ser exibido no cancelamento do cic" +
                    "lo \'ciclo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line 27
  testRunner.And("o campo \"data de início do próximo ciclo\" no cancelamento do ciclo \'ciclo 01\' dev" +
                    "e estar com um valor \'21/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("04 - Obter, para o cancelamento de ciclo, o Motivo de cancelamento (RF_4.02)")]
        public virtual void _04_ObterParaOCancelamentoDeCicloOMotivoDeCancelamentoRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("04 - Obter, para o cancelamento de ciclo, o Motivo de cancelamento (RF_4.02)", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 31
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 32
  testRunner.And("um motivo \'motivo 02\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 33
  testRunner.And("ciclo \'ciclo 01\' na situação \'Cancelado - motivo 01\', data de início no dia \'05/0" +
                    "3/2012\' e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 34
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 35
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 36
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 37
  testRunner.When("cancelar o ciclo \'ciclo 02\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 38
  testRunner.Then("o campo \"motivo\" no cancelamento do ciclo \'ciclo 02\' deve estar com o motivo \'mot" +
                    "ivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("05 - Cancelar um ciclo quando a data de término do ciclo atual seja menor que a d" +
            "ata atual (RF_4.02)")]
        public virtual void _05_CancelarUmCicloQuandoADataDeTerminoDoCicloAtualSejaMenorQueADataAtualRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("05 - Cancelar um ciclo quando a data de término do ciclo atual seja menor que a d" +
                    "ata atual (RF_4.02)", ((string[])(null)));
#line 40
this.ScenarioSetup(scenarioInfo);
#line 41
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 42
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 43
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 44
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 45
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 46
  testRunner.And("data atual for \'02/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 47
  testRunner.When("cancelar o ciclo \'ciclo 01\' com o motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 48
  testRunner.Then("o campo \"data de início do próximo ciclo\" não deve ser exibido no cancelamento do" +
                    " ciclo \'ciclo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line 49
  testRunner.And("o ciclo \'ciclo 01\' deve ser cancelado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 50
  testRunner.And("o campo \"data de início\" do ciclo \'ciclo 01\' deve ser \'05/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 51
  testRunner.And("o campo \"data de término\" do ciclo \'ciclo 01\' deve ser \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("06 - Cancelar um ciclo quando existir, nos itens posteriores ao mesmo, ao menos a" +
            "lgum com a situação diferente de \"Não Planejado\" (RF_4.02)")]
        public virtual void _06_CancelarUmCicloQuandoExistirNosItensPosterioresAoMesmoAoMenosAlgumComASituacaoDiferenteDeNaoPlanejadoRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("06 - Cancelar um ciclo quando existir, nos itens posteriores ao mesmo, ao menos a" +
                    "lgum com a situação diferente de \"Não Planejado\" (RF_4.02)", ((string[])(null)));
#line 53
this.ScenarioSetup(scenarioInfo);
#line 54
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 55
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 56
  testRunner.And("ciclo \'ciclo 01\' na situação \'Concluído\', data de início no dia \'05/03/2012\' e da" +
                    "ta de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 57
  testRunner.And("ciclo \'ciclo 02\' na situação \'Cancelado\', data de início no dia \'03/04/2012\' e da" +
                    "ta de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 58
  testRunner.And("ciclo \'ciclo 03\' na situação \'Cancelado\', data de início no dia \'02/05/2012\' e da" +
                    "ta de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 59
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 60
  testRunner.When("cancelar o ciclo \'ciclo 01\' com o motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 61
  testRunner.Then("o campo \"data de início do próximo ciclo\" não deve ser exibido no cancelamento do" +
                    " ciclo \'ciclo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line 62
  testRunner.And("o ciclo \'ciclo 01\' deve ser cancelado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 63
  testRunner.And("o campo \"data de início\" do ciclo \'ciclo 01\' deve ser \'05/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 64
  testRunner.And("o campo \"data de término\" do ciclo \'ciclo 01\' deve ser \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("07 - Cancelar o último ciclo do projeto")]
        public virtual void _07_CancelarOUltimoCicloDoProjeto()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("07 - Cancelar o último ciclo do projeto", ((string[])(null)));
#line 66
this.ScenarioSetup(scenarioInfo);
#line 67
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 68
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 69
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 70
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 71
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 72
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 73
  testRunner.When("cancelar o ciclo \'ciclo 03\' com o motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 74
  testRunner.Then("o campo \"data de início do próximo ciclo\" não deve ser exibido no cancelamento do" +
                    " ciclo \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line 75
  testRunner.And("o ciclo \'ciclo 03\' deve ser cancelado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 76
  testRunner.And("o campo \"data de início\" do ciclo \'ciclo 03\' deve ser \'02/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 77
  testRunner.And("o campo \"data de término\" do ciclo \'ciclo 03\' deve ser \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("08 - Cancelar um ciclo quando todos os itens posteriores ao mesmo estiverem com a" +
            " situação \"Não Iniciado\" (RF_4.02)")]
        public virtual void _08_CancelarUmCicloQuandoTodosOsItensPosterioresAoMesmoEstiveremComASituacaoNaoIniciadoRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("08 - Cancelar um ciclo quando todos os itens posteriores ao mesmo estiverem com a" +
                    " situação \"Não Iniciado\" (RF_4.02)", ((string[])(null)));
#line 79
this.ScenarioSetup(scenarioInfo);
#line 80
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 81
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 82
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 83
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 84
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 85
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 86
  testRunner.When("cancelar o ciclo \'ciclo 01\' com o motivo \'motivo 01\' e data de início do próximo " +
                    "ciclo com \'28/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 87
  testRunner.Then("o ciclo \'ciclo 01\' deve ser cancelado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line 88
  testRunner.And("o campo \"data de início\" do ciclo \'ciclo 01\' deve ser \'05/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 89
  testRunner.And("o campo \"data de término\" do ciclo \'ciclo 01\' deve ser \'27/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 90
  testRunner.And("o campo \"data de início\" do ciclo \'ciclo 02\' deve ser \'28/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 91
  testRunner.And("o campo \"data de término\" do ciclo \'ciclo 02\' deve ser \'10/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 92
  testRunner.And("o campo \"data de início\" do ciclo \'ciclo 03\' deve ser \'11/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 93
  testRunner.And("o campo \"data de término\" do ciclo \'ciclo 03\' deve ser \'24/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("09 - Cancelar um ciclo com a \"data de início do próximo ciclo\" com uma data menor" +
            " que o início do ciclo atual (RF_4.02)")]
        public virtual void _09_CancelarUmCicloComADataDeInicioDoProximoCicloComUmaDataMenorQueOInicioDoCicloAtualRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("09 - Cancelar um ciclo com a \"data de início do próximo ciclo\" com uma data menor" +
                    " que o início do ciclo atual (RF_4.02)", ((string[])(null)));
#line 95
this.ScenarioSetup(scenarioInfo);
#line 96
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 97
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 98
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 99
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 100
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 101
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 102
  testRunner.When("validar o cancelamento do ciclo \'ciclo 01\' passando a data \'04/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 103
  testRunner.Then("exibir mensagem para o ciclo \'ciclo 01\' \'A data de Início do Próximo Ciclo deve e" +
                    "star entre 06/03/2012 e 03/04/2012\' para o cancelamento com a data incorreta", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10 - Cancelar um ciclo com a \"data de início do próximo ciclo\" com uma data igual" +
            " a do início do ciclo atual (RF_4.02)")]
        public virtual void _10_CancelarUmCicloComADataDeInicioDoProximoCicloComUmaDataIgualADoInicioDoCicloAtualRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10 - Cancelar um ciclo com a \"data de início do próximo ciclo\" com uma data igual" +
                    " a do início do ciclo atual (RF_4.02)", ((string[])(null)));
#line 105
this.ScenarioSetup(scenarioInfo);
#line 106
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 107
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 108
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 109
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 110
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 111
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 112
  testRunner.When("validar o cancelamento do ciclo \'ciclo 01\' passando a data \'05/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 113
  testRunner.Then("exibir mensagem para o ciclo \'ciclo 01\' \'A data de Início do Próximo Ciclo deve e" +
                    "star entre 06/03/2012 e 03/04/2012\' para o cancelamento com a data incorreta", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("11 - Cancelar um ciclo com a \"data de início do próximo ciclo\" com uma data maior" +
            " que o início do próximo ciclo (RF_4.02)")]
        public virtual void _11_CancelarUmCicloComADataDeInicioDoProximoCicloComUmaDataMaiorQueOInicioDoProximoCicloRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("11 - Cancelar um ciclo com a \"data de início do próximo ciclo\" com uma data maior" +
                    " que o início do próximo ciclo (RF_4.02)", ((string[])(null)));
#line 115
this.ScenarioSetup(scenarioInfo);
#line 116
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 117
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 118
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 119
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 120
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 121
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 122
  testRunner.When("validar o cancelamento do ciclo \'ciclo 01\' passando a data \'04/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 123
  testRunner.Then("exibir mensagem para o ciclo \'ciclo 01\' \'A data de Início do Próximo Ciclo deve e" +
                    "star entre 06/03/2012 e 03/04/2012\' para o cancelamento com a data incorreta", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("12 - Concatenar a situação do ciclo com o motivo de cancelamento (RF_4.02)")]
        public virtual void _12_ConcatenarASituacaoDoCicloComOMotivoDeCancelamentoRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("12 - Concatenar a situação do ciclo com o motivo de cancelamento (RF_4.02)", ((string[])(null)));
#line 125
this.ScenarioSetup(scenarioInfo);
#line 126
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 127
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 128
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 129
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 130
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 131
  testRunner.And("data atual for \'02/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 132
  testRunner.When("cancelar o ciclo \'ciclo 01\' com o motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 133
  testRunner.Then("a situação do ciclo \'ciclo 01\' deverá ser \'Cancelado - motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("13 - Definir um ciclo sem estórias pendentes como cancelado (RF_4.02)")]
        public virtual void _13_DefinirUmCicloSemEstoriasPendentesComoCanceladoRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("13 - Definir um ciclo sem estórias pendentes como cancelado (RF_4.02)", ((string[])(null)));
#line 135
this.ScenarioSetup(scenarioInfo);
#line 136
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 137
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 138
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 139
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 140
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 141
  testRunner.And("data atual for \'02/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 142
  testRunner.When("cancelar o ciclo \'ciclo 01\' com o motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 143
  testRunner.Then("não deverá exibir, na janela de cancelamento do ciclo \'ciclo 01\', a área de desti" +
                    "no de estórias pendentes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("14 - Obter a Lista de Motivos Ativos (RF_3.05)")]
        public virtual void _14_ObterAListaDeMotivosAtivosRF_3_05()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("14 - Obter a Lista de Motivos Ativos (RF_3.05)", ((string[])(null)));
#line 145
this.ScenarioSetup(scenarioInfo);
#line 146
  testRunner.Given("os motivos de cancelamento \'motivo 01\' - status \'Ativo\', \'motivo 02\' - status \'At" +
                    "ivo\', \'motivo 03\' - status \'Ativo\', \'motivo 04\' - status \'Inativo\', \'motivo 05\' " +
                    "- status \'Inativo\', \'motivo 06\' - status \'Ativo\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 147
  testRunner.When("obter lista de motivos ativos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 148
  testRunner.Then("os motivos \'motivo 01\', \'motivo 02\', \'motivo 03\', \'motivo 06\' devem vir na lista " +
                    "de motivos ativos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("15 - Salvar o cancelamento de Ciclo sem informar um Motivo (RF_4.02)")]
        public virtual void _15_SalvarOCancelamentoDeCicloSemInformarUmMotivoRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("15 - Salvar o cancelamento de Ciclo sem informar um Motivo (RF_4.02)", ((string[])(null)));
#line 150
this.ScenarioSetup(scenarioInfo);
#line 151
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 152
  testRunner.And("um motivo \'motivo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 153
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 154
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 155
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 156
  testRunner.And("data atual for \'20/02/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 157
  testRunner.When("cancelar o ciclo \'ciclo 01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 158
  testRunner.And("validar o cancelamento do ciclo \'ciclo 01\' sem passar o motivo", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 159
  testRunner.Then("exibir mensagem para o ciclo \'ciclo 01\' \'É necessário informar um Motivo de Cance" +
                    "lamento\' para o cancelamento sem motivo", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("16 - O Ciclo só poderá ser cancelado se seu periodo estiver no passado data atual" +
            " (RF_4.02)")]
        public virtual void _16_OCicloSoPoderaSerCanceladoSeSeuPeriodoEstiverNoPassadoDataAtualRF_4_02()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("16 - O Ciclo só poderá ser cancelado se seu periodo estiver no passado data atual" +
                    " (RF_4.02)", ((string[])(null)));
#line 161
this.ScenarioSetup(scenarioInfo);
#line 162
  testRunner.Given("um projeto \'projeto 01\' com o(s) ciclo(s) \'ciclo 01\', \'ciclo 02\', \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 163
  testRunner.And("ciclo \'ciclo 01\' na situação \'Não Planejado\', data de início no dia \'05/03/2012\' " +
                    "e data de término no dia \'30/03/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 164
  testRunner.And("ciclo \'ciclo 02\' na situação \'Não Planejado\', data de início no dia \'03/04/2012\' " +
                    "e data de término no dia \'30/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 165
  testRunner.And("ciclo \'ciclo 03\' na situação \'Não Planejado\', data de início no dia \'02/05/2012\' " +
                    "e data de término no dia \'29/05/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 166
  testRunner.And("data atual for \'28/04/2012\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 167
  testRunner.When("cancelar o ciclo \'ciclo 03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 168
  testRunner.Then("o \'ciclo 03\' não deverá ser cancelado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
