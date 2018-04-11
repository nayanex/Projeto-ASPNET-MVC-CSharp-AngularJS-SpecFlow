using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.DAOs;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Test
{
    /// <summary>
    /// This is a test class for RnImportacaoBOMTest and is intended
    /// to contain all RnImportacaoBOMTest Unit Tests
    /// </summary>
    [TestClass]
    public class BaseTest
    {
    
        /// <summary>
        /// Seção que será criada a cada inicialização
        /// de um teste.
        /// </summary>
        protected static Session SessionTest { get; set; }

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        ///// <summary>
        ///// Gets or sets.
        ///// </summary>
        //protected string ScenarioDir { get; set; }

        #region Additional test attributes
        #endregion

        /// <summary>
        /// Chamado antes de cada teste para executar a limpeza da base.
        /// </summary>
        [BeforeScenario]
        [TestInitialize]
        public void ClearDataBase()
        {
            if (SessionTest == null)
            {
                SessionTest = new Session();
                SessionTest.ClearDatabase();

                User usuario = new User(SessionTest)
                {
                    UserName = "userTest",
                    FirstName = "Chico",
                    LastName = "de Teste",
                    Email = "user@fpf.br"
                };

                DateUtil.CurrentDateTime = DateTime.MinValue;
                UsuarioDAO.CurrentUser = usuario;
            }
        }

        //Use ClassInitialize to run code before running the first test in the class
        /// <summary>
        /// Fecha a seção.
        /// </summary>
        [AfterScenario]
        [TestCleanup]
        public void CloseSession()
        {
            if (SessionTest != null)
            {
                SessionTest.Dispose();
                SessionTest = null;
            }
        }
        /// <summary>
        /// Obtem o diretório raiz da aplicação.
        /// </summary>
        public BaseTest()
        {
            //ScenarioDir = Environment.CurrentDirectory;
            //ScenarioDir = ScenarioDir.Substring(0, ScenarioDir.IndexOf("\\TestResults\\"));
            //ScenarioDir = String.Format("{0}\\WexProject.Test\\Fixtures\\{1}", ScenarioDir, this.GetType().Name);
        }
    }
}
