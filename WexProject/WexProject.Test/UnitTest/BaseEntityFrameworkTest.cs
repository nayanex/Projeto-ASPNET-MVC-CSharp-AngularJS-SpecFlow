using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models;
using TechTalk.SpecFlow;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Contexto;
using TechTalk.SpecFlow.Assist;
using WexProject.Test.Helpers.Mocks;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Classe necessária para instanciar um banco em memória para ser utilizado pelo EntityFramework
    /// </summary>
    [TestClass]
    public class BaseEntityFrameworkTest
    {
        #region Atributos

        public static DbConnection conexao;

        /// <summary>
        /// atributo que guarda o contexto para ser utilizado com banco em memória.
        /// </summary>
        public static WexDb contexto;

        /// <summary>
        /// Colaborador para ser usado nos testes
        /// </summary>
        public Colaborador colaboradorPadrao;

        #endregion

        #region Propriedades

        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        #region Métodos
        
        /// <summary>
        /// Método necessário para criar o contexto padrão para ser utilizado em testes.
        /// </summary>
        [BeforeScenario]
        [TestInitialize]
        public void ConstruirContextoPadraoParaIniciarTeste()
        {
            ContextFactoryMock.AbrirConexao = AbrirConexaoMock;
            ContextFactoryManager.SetContextFactory( new ContextFactoryMock() );
            contexto = ContextFactoryManager.CriarWexDb();
            colaboradorPadrao = CriarColaboradoPadrao();
        }

        /// <summary>
        /// Abrir uma conexão de banco em memória
        /// </summary>
        /// <returns>retornar uma conexão de banco em memória</returns>
        private WexDb AbrirConexaoMock() 
        {
            string nomeConexao;
            if(TestContext != null)
            {
                nomeConexao = TestContext.TestName;
            }
            else
            {
                nomeConexao = ScenarioContext.Current.ScenarioInfo.Title.Replace( " ", "" );
            }
            return new WexDb( Effort.DbConnectionFactory.CreatePersistent( nomeConexao ) );
        }

        /// <summary>
        /// Método que cria um colaborador padrão para ser usado nos testes
        /// </summary>
        /// <param name="contexto">Contexto do banco em memória</param>
        protected Colaborador CriarColaboradoPadrao()
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                Colaborador colaborador = new Colaborador();
                colaborador.Usuario = new User()
                {
                    Person = new Person()
                    {
                        Party = new Party()
                    }
                };
                colaborador.Usuario.UserName = "joaquim.barbosa";
                colaborador.Usuario.Person.FirstName = "Joaquim";
                colaborador.Usuario.Person.LastName = "Barbosa";
                contexto.Colaboradores.Add( colaborador );
                contexto.SaveChanges();

                return colaborador;
            }
        }

        /// <summary>
        /// Destrói o colaborador padrão criado
        /// </summary>
        protected void DestruirColaboradorPadrao()
        {
            colaboradorPadrao = null;
        }

        /// <summary>
        /// Método necessário para destruir o cenário que foi utilizado nos testes.
        /// </summary>
        [AfterScenario]
        [TestCleanup]
        public void DestruirContextoPadraoAposTestar()
        {
            DestruirColaboradorPadrao();
        }

        #endregion
    }
}