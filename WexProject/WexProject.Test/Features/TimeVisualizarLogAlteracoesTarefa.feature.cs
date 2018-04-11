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
    public partial class TimeDeDesenvolvimentoVisualizarOLogDeAlteracoesDeUmaTarefaFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "TimeVisualizarLogAlteracoesTarefa.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Time de Desenvolvimento visualizar o log de alterações de uma tarefa", "          Dado um time de Desenvolvimento\r\n\tQuando estiver no cronograma \r\n      " +
                    "    E visualizar o log de atualizações de uma determinada tarefa\r\n          Enta" +
                    "o poder visualizar todas as modificações da tarefa selecionada", ProgrammingLanguage.CSharp, new string[] {
                        "pbi_4.03.12"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")))
            {
                WexProject.Test.Features.TimeDeDesenvolvimentoVisualizarOLogDeAlteracoesDeUmaTarefaFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("01 - Um colaborador modificar uma tarefa. Este deverá ser registrado no log de at" +
            "ualizações da tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_4.03.12")]
        public virtual void _01_UmColaboradorModificarUmaTarefa_EsteDeveraSerRegistradoNoLogDeAtualizacoesDaTarefa()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01 - Um colaborador modificar uma tarefa. Este deverá ser registrado no log de at" +
                    "ualizações da tarefa", ((string[])(null)));
#line 10
this.ScenarioSetup(scenarioInfo);
#line 11
   testRunner.Given("um cronograma \'cronograma01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 12
   testRunner.And("ter colaborador(es) \'colaborador01\',\'colaborador02\',\'colaborador03\',\'colaborador0" +
                    "4\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 13
   testRunner.And("data atual for \'20/02/2012 22:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 14
   testRunner.And("usuario logado for \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table1.AddRow(new string[] {
                        "id",
                        "01"});
            table1.AddRow(new string[] {
                        "nome",
                        "tarefa01"});
#line 15
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table1, "E ");
#line 19
   testRunner.And("data atual for \'22/02/2012 22:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table2.AddRow(new string[] {
                        "nome",
                        "tarefazeroum"});
#line 20
   testRunner.When("o colaborador \'colaborador01\' modificar a tarefa \'tarefa01\':", ((string)(null)), table2, "Quando ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "data e hora",
                        "responsavel",
                        "alterações"});
            table3.AddRow(new string[] {
                        "22/02/12 22:00:00",
                        "colaborador01",
                        "Descrição alterada de \'tarefa01\' para \'tarefazeroum\'\n"});
            table3.AddRow(new string[] {
                        "20/02/12 22:00:00",
                        "colaborador01",
                        "Criação da tarefa\n"});
#line 23
   testRunner.Then("o histórico de log da \'tarefa01\' deve estar:", ((string)(null)), table3, "Entao ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table4.AddRow(new string[] {
                        "data",
                        "22/02/12 22:00"});
            table4.AddRow(new string[] {
                        "responsável",
                        "colaborador01"});
#line 27
   testRunner.And("a tarefa \'tarefa01\' deve estar com os dados de última atualização:", ((string)(null)), table4, "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("02 - Dois colaboradores modificarem uma mesma tarefa. Estes deverão ser registrad" +
            "os no log de atualizações da tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_4.03.12")]
        public virtual void _02_DoisColaboradoresModificaremUmaMesmaTarefa_EstesDeveraoSerRegistradosNoLogDeAtualizacoesDaTarefa()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("02 - Dois colaboradores modificarem uma mesma tarefa. Estes deverão ser registrad" +
                    "os no log de atualizações da tarefa", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
            testRunner.Given("um cronograma \'cronograma01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 34
   testRunner.And("ter colaborador(es) \'colaborador01\',\'colaborador02\',\'colaborador03\',\'colaborador0" +
                    "4\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 35
   testRunner.And("data atual for \'20/02/2012 22:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 36
   testRunner.And("usuario logado for \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table5.AddRow(new string[] {
                        "id",
                        "01"});
            table5.AddRow(new string[] {
                        "nome",
                        "tarefa01"});
#line 37
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table5, "E ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table6.AddRow(new string[] {
                        "nome",
                        "tarefazeroum"});
            table6.AddRow(new string[] {
                        "quando",
                        "22/02/2012 22:00:00"});
#line 41
            testRunner.And("o colaborador \'colaborador01\' ter modificado a tarefa \'tarefa01\':", ((string)(null)), table6, "E ");
#line 45
   testRunner.And("data atual for \'23/02/2012 16:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table7.AddRow(new string[] {
                        "nome",
                        "tarefazeroone"});
#line 46
   testRunner.When("o colaborador \'colaborador02\' modificar a tarefa \'tarefa01\':", ((string)(null)), table7, "Quando ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "data e hora",
                        "responsavel",
                        "alterações"});
            table8.AddRow(new string[] {
                        "23/02/12 16:00:00",
                        "colaborador02",
                        "Descrição alterada de \'tarefazeroum\' para \'tarefazeroone\'\n"});
            table8.AddRow(new string[] {
                        "22/02/12 22:00:00",
                        "colaborador01",
                        "Descrição alterada de \'tarefa01\' para \'tarefazeroum\'\n"});
            table8.AddRow(new string[] {
                        "20/02/12 22:00:00",
                        "colaborador01",
                        "Criação da tarefa\n"});
#line 49
            testRunner.Then("o histórico de log da \'tarefa01\' deve estar:", ((string)(null)), table8, "Entao ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table9.AddRow(new string[] {
                        "data",
                        "23/02/12 16:00"});
            table9.AddRow(new string[] {
                        "responsável",
                        "colaborador02"});
#line 54
   testRunner.And("a tarefa \'tarefa01\' deve estar com os dados de última atualização:", ((string)(null)), table9, "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("03 - Dois colaboradores modificarem tarefas distintas. Estes deveram ser registra" +
            "do no log de atualizações de cada tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_4.03.12")]
        public virtual void _03_DoisColaboradoresModificaremTarefasDistintas_EstesDeveramSerRegistradoNoLogDeAtualizacoesDeCadaTarefa()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("03 - Dois colaboradores modificarem tarefas distintas. Estes deveram ser registra" +
                    "do no log de atualizações de cada tarefa", ((string[])(null)));
#line 59
this.ScenarioSetup(scenarioInfo);
#line 60
   testRunner.Given("um cronograma \'cronograma01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 61
   testRunner.And("ter colaborador(es) \'colaborador01\',\'colaborador02\',\'colaborador03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 62
   testRunner.And("data atual for \'20/02/2012 22:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 63
   testRunner.And("usuario logado for \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table10.AddRow(new string[] {
                        "id",
                        "01"});
            table10.AddRow(new string[] {
                        "nome",
                        "tarefa01"});
#line 64
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table10, "E ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table11.AddRow(new string[] {
                        "id",
                        "02"});
            table11.AddRow(new string[] {
                        "nome",
                        "tarefa02"});
#line 68
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table11, "E ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table12.AddRow(new string[] {
                        "nome",
                        "tarefazeroum"});
            table12.AddRow(new string[] {
                        "quando",
                        "22/02/2012 22:00:00"});
#line 72
            testRunner.And("o colaborador \'colaborador01\' ter modificado a tarefa \'tarefa01\':", ((string)(null)), table12, "E ");
#line 76
   testRunner.And("data atual for \'23/02/2012 16:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table13.AddRow(new string[] {
                        "nome",
                        "tarefazerodois"});
#line 77
   testRunner.When("o colaborador \'colaborador02\' modificar a tarefa \'tarefa02\':", ((string)(null)), table13, "Quando ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "data e hora",
                        "colaborador",
                        "alterações"});
            table14.AddRow(new string[] {
                        "23/02/12 16:00:00",
                        "colaborador02",
                        "Descrição alterada de \'tarefa02\' para \'tarefazerodois\'\n"});
            table14.AddRow(new string[] {
                        "20/02/12 22:00:00",
                        "colaborador01",
                        "Criação da tarefa\n"});
#line 80
            testRunner.Then("o histórico de log da \'tarefa02\' deve estar:", ((string)(null)), table14, "Entao ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table15.AddRow(new string[] {
                        "data",
                        "22/02/12 22:00"});
            table15.AddRow(new string[] {
                        "responsável",
                        "colaborador01"});
#line 84
            testRunner.And("a tarefa \'tarefa01\' deve estar com os dados de última atualização:", ((string)(null)), table15, "E ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table16.AddRow(new string[] {
                        "data",
                        "23/02/12 16:00"});
            table16.AddRow(new string[] {
                        "responsável",
                        "colaborador02"});
#line 88
   testRunner.And("a tarefa \'tarefa02\' deve estar com os dados de última atualização:", ((string)(null)), table16, "E ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("04 - Ter uma tarefa selecionada. O botão de visualizar o Histórico de Alterações " +
            "deve estar habilitado")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_4.03.12")]
        public virtual void _04_TerUmaTarefaSelecionada_OBotaoDeVisualizarOHistoricoDeAlteracoesDeveEstarHabilitado()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("04 - Ter uma tarefa selecionada. O botão de visualizar o Histórico de Alterações " +
                    "deve estar habilitado", ((string[])(null)));
#line 93
this.ScenarioSetup(scenarioInfo);
#line 94
   testRunner.Given("um cronograma \'cronograma01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table17.AddRow(new string[] {
                        "id",
                        "01"});
            table17.AddRow(new string[] {
                        "nome",
                        "tarefa01"});
#line 95
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table17, "E ");
#line 99
   testRunner.And("ter colaborador(es) \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table18.AddRow(new string[] {
                        "nome",
                        "tarefazeroum"});
            table18.AddRow(new string[] {
                        "quando",
                        "22/02/2012 22:00:00"});
#line 100
            testRunner.And("o colaborador \'colaborador01\' ter modificado a tarefa \'tarefa01\':", ((string)(null)), table18, "E ");
#line 104
   testRunner.When("selecionar a(s) tarefa(s) \'tarefa01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 105
   testRunner.Then("o botão \'Histórico de Atualização\' deve estar habilitado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("05 - Ter mais de uma tarefa selecionada. O botão de visualizar o Histórico de Alt" +
            "erações não deve estar habilitado")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_4.03.12")]
        public virtual void _05_TerMaisDeUmaTarefaSelecionada_OBotaoDeVisualizarOHistoricoDeAlteracoesNaoDeveEstarHabilitado()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("05 - Ter mais de uma tarefa selecionada. O botão de visualizar o Histórico de Alt" +
                    "erações não deve estar habilitado", ((string[])(null)));
#line 107
this.ScenarioSetup(scenarioInfo);
#line 108
   testRunner.Given("um cronograma \'cronograma01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table19.AddRow(new string[] {
                        "id",
                        "01"});
            table19.AddRow(new string[] {
                        "nome",
                        "tarefa01"});
#line 109
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table19, "E ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table20.AddRow(new string[] {
                        "id",
                        "02"});
            table20.AddRow(new string[] {
                        "nome",
                        "tarefa02"});
#line 113
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table20, "E ");
#line 117
   testRunner.And("ter colaborador(es) \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table21.AddRow(new string[] {
                        "nome",
                        "tarefazeroum"});
            table21.AddRow(new string[] {
                        "quando",
                        "22/02/2012 22:00:00"});
#line 118
            testRunner.And("o colaborador \'colaborador01\' ter modificado a tarefa \'tarefa01\':", ((string)(null)), table21, "E ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table22.AddRow(new string[] {
                        "nome",
                        "tarefazerodois"});
            table22.AddRow(new string[] {
                        "quando",
                        "23/02/2012 22:00:00"});
#line 122
            testRunner.And("o colaborador \'colaborador01\' ter modificado a tarefa \'tarefa02\':", ((string)(null)), table22, "E ");
#line 126
   testRunner.When("selecionar a(s) tarefa(s) \'tarefa01\',\'tarefa02\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 127
   testRunner.Then("o botão \'Histórico de Atualização\' não deve estar habilitado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("06 - Ter uma tarefa selecionada e obter o Histórico de Alterações da mesma")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_4.03.12")]
        public virtual void _06_TerUmaTarefaSelecionadaEObterOHistoricoDeAlteracoesDaMesma()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("06 - Ter uma tarefa selecionada e obter o Histórico de Alterações da mesma", ((string[])(null)));
#line 129
this.ScenarioSetup(scenarioInfo);
#line 130
   testRunner.Given("um cronograma \'cronograma01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 131
   testRunner.And("ter colaborador(es) \'colaborador01\',\'colaborador02\',\'colaborador03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 132
   testRunner.And("data atual for \'20/02/2012 22:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 133
   testRunner.And("usuario logado for \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table23.AddRow(new string[] {
                        "id",
                        "01"});
            table23.AddRow(new string[] {
                        "nome",
                        "tarefa01"});
#line 134
   testRunner.And("uma tarefa do cronograma \'cronograma01\' com os dados:", ((string)(null)), table23, "E ");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table24.AddRow(new string[] {
                        "nome",
                        "tarefazeroum"});
            table24.AddRow(new string[] {
                        "quando",
                        "22/02/2012 22:00:00"});
#line 138
            testRunner.And("o colaborador \'colaborador01\' ter modificado a tarefa \'tarefa01\':", ((string)(null)), table24, "E ");
#line hidden
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table25.AddRow(new string[] {
                        "nome",
                        "tarefa"});
            table25.AddRow(new string[] {
                        "quando",
                        "10/03/2012 20:00:00"});
#line 142
   testRunner.And("o colaborador \'colaborador02\' ter modificado a tarefa \'tarefa01\':", ((string)(null)), table25, "E ");
#line hidden
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table26.AddRow(new string[] {
                        "nome",
                        "tarefaalterada"});
            table26.AddRow(new string[] {
                        "quando",
                        "15/04/2012 10:00:00"});
#line 146
   testRunner.And("o colaborador \'colaborador03\' ter modificado a tarefa \'tarefa01\':", ((string)(null)), table26, "E ");
#line 150
   testRunner.When("selecionar a(s) tarefa(s) \'tarefa01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line hidden
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "data e hora",
                        "responsavel",
                        "alterações"});
            table27.AddRow(new string[] {
                        "15/04/12 10:00",
                        "colaborador03",
                        "Descrição alterada de \'tarefa\' para \'tarefaalterada\'\n"});
            table27.AddRow(new string[] {
                        "10/03/12 20:00",
                        "colaborador02",
                        "Descrição alterada de \'tarefazeroum\' para \'tarefa\'\n"});
            table27.AddRow(new string[] {
                        "22/02/12 22:00",
                        "colaborador01",
                        "Descrição alterada de \'tarefa01\' para \'tarefazeroum\'\n"});
            table27.AddRow(new string[] {
                        "20/02/12 22:00",
                        "colaborador01",
                        "Criação da tarefa\n"});
#line 151
   testRunner.Then("o histórico de log da \'tarefa01\' deve estar:", ((string)(null)), table27, "Entao ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("07 - Um colaborador criar uma tarefa. Deverá ser registrado no log de atualizaçõe" +
            "s da tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Time de Desenvolvimento visualizar o log de alterações de uma tarefa")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("pbi_4.03.12")]
        public virtual void _07_UmColaboradorCriarUmaTarefa_DeveraSerRegistradoNoLogDeAtualizacoesDaTarefa()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("07 - Um colaborador criar uma tarefa. Deverá ser registrado no log de atualizaçõe" +
                    "s da tarefa", ((string[])(null)));
#line 158
this.ScenarioSetup(scenarioInfo);
#line 159
   testRunner.Given("um cronograma \'cronograma01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 160
   testRunner.And("ter colaborador(es) \'colaborador01\',\'colaborador02\',\'colaborador03\',\'colaborador0" +
                    "4\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 161
   testRunner.And("usuario logado for \'colaborador01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 162
   testRunner.And("data atual for \'22/02/2012 22:00:00\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line hidden
            TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                        "id",
                        "nome"});
            table28.AddRow(new string[] {
                        "01",
                        "tarefa01"});
#line 163
   testRunner.When("o colaborador logado criar as seguintes tarefas para o cronograma \'cronograma01\':" +
                    "", ((string)(null)), table28, "Quando ");
#line hidden
            TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                        "data e hora",
                        "responsavel",
                        "alterações"});
            table29.AddRow(new string[] {
                        "22/02/12 22:00:00",
                        "colaborador01",
                        "Criação da tarefa\n"});
#line 166
   testRunner.Then("o histórico de log da \'tarefa01\' deve estar:", ((string)(null)), table29, "Entao ");
#line hidden
            TechTalk.SpecFlow.Table table30 = new TechTalk.SpecFlow.Table(new string[] {
                        "campo",
                        "valor"});
            table30.AddRow(new string[] {
                        "data",
                        "22/02/12 22:00"});
            table30.AddRow(new string[] {
                        "responsável",
                        "colaborador01"});
#line 169
   testRunner.And("a tarefa \'tarefa01\' deve estar com os dados de última atualização:", ((string)(null)), table30, "E ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion