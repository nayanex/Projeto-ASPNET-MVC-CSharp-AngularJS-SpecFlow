using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Models.Geral;
using System.Drawing;
using WexProject.Test.Fixtures.Factory;
using DevExpress.Xpo;
using System.Threading;
using TechTalk.SpecFlow;
using WexProject.Library.Libs.Test;
using WexProject.BLL.BOs.Geral;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class ProjetoColaboradorConfigTest : BaseTest
    {

        Projeto projeto1, projeto2, projeto3;
        Colaborador colaborador1, colaborador2, colaborador3, colaborador4;
        Session s1, s2, s3, s4;

        [TestInitialize]
        public void Inicializar()
        {
            // criando colaboradores
            colaborador1 = ColaboradorFactory.CriarColaborador( SessionTest, "anderson" );
            colaborador2 = ColaboradorFactory.CriarColaborador( SessionTest, "gabriel" );
            colaborador3 = ColaboradorFactory.CriarColaborador( SessionTest, "alexandre" );
            colaborador4 = ColaboradorFactory.CriarColaborador( SessionTest, "pedro" );
            // criando projetos
            projeto1 = ProjetoFactory.Criar( SessionTest, 100, "projeto 01", true );
            projeto2 = ProjetoFactory.Criar( SessionTest, 100, "projeto 02", true );
            projeto3 = ProjetoFactory.Criar( SessionTest, 100, "projeto 03", true );
            s1 = new Session();
            s2 = new Session();
            s3 = new Session();
            s4 = new Session();
        }

        [TestCleanup]
        public void LimparSessoes() 
        {
            if(s1 != null)
            {
                s1.Dispose();
                s1 = null;
            }

            if(s2 != null)
            {
                s2.Dispose();
                s2 = null;
            }

            if(s3 != null)
            {
                s3.Dispose();
                s3 = null;
            }

            if(s4 != null)
            {
                s4.Dispose();
                s4 = null;
            }
        }

        private static string ConverterCorToArgbString( Color cor )
        {
            return cor.ToArgb().ToString();
        }

        [TestMethod]
        public void GetProjetoColaboradorConfigQuandoNaoHouverCadastrada()
        {
            ProjetoColaboradorConfig config = ProjetoColaboradorConfig.GetProjetoColaboradorConfig( SessionTest, colaborador1.Oid, projeto1.Oid );
            Assert.IsNull( config, "Não deveria possuir config pois não existe nenhuma cadastrada!" );
        }

        [TestMethod]
        public void GetProjetoColaboradorConfigQuandoNaoHouverConfigCadastradaPesquisadaPorLogin()
        {
            ProjetoColaboradorConfig config = ProjetoColaboradorConfig.GetProjetoColaboradorConfig( SessionTest, colaborador1.Usuario.UserName, projeto1.Oid );
            Assert.IsNull( config, "Não deveria possuir config pois não existe nenhuma cadastrada!" );
        }

        [TestMethod]
        [ExpectedException( typeof( Exception ) )]
        public void RnEscolherCorQuandoNaoExistirProjeto()
        {
            try
            {
                string cor = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, Guid.NewGuid() );
            }
            catch(Exception e)
            {
                Assert.AreEqual( "A cor não pode ser selecionada, o projeto selecionado não existe!", e.Message, "Deveria ter levantado a exception com a mensagem atual" );
                throw e;
            }
        }

        [TestMethod]
        [ExpectedException( typeof( Exception ) )]
        public void RnEscolherCorQuandoNaoExistirOColaboradorSelecionado()
        {
            try
            {
                string cor = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, Guid.NewGuid(), projeto1.Oid );
            }
            catch(Exception e)
            {
                Assert.AreEqual( "A cor não pode ser selecionada, o colaborador selecionado não existe!", e.Message, "Deveria ter levantado a exception com a mensagem atual" );
                throw e;
            }
        }

        [TestMethod]
        [ExpectedException( typeof( Exception ) )]
        public void RnEscolherCorQuandoNaoExistirOColaboradorSelecionadoPorLogin()
        {
            try
            {
                string cor = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, "joao", projeto1.Oid );
            }
            catch(Exception e)
            {
                Assert.AreEqual( "A cor não pode ser selecionada, o colaborador selecionado não existe!", e.Message, "Deveria ter levantado a exception com a mensagem atual" );
                throw e;
            }
        }

        [TestMethod]
        public void RnEscolherCorQuandoExistirOColaboradorEOProjetoSelecionado()
        {
            string cor1 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );
            string corSelecionada = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );
            Assert.AreEqual( cor1, corSelecionada, "Deveria ter selecionada a primeira cor do vetor" );
        }

        [TestMethod]
        public void RnEscolherCorQuandoOColaboradorEOProjetoForemSelecionadosPelaSegundaVez()
        {
            string cor1 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );
            string corSelecionada = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );
            Assert.AreEqual( cor1, corSelecionada, "Deveria ter selecionada a primeira cor do vetor" );
            string corSelecionada2 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );
            Assert.AreEqual( cor1, corSelecionada2, "Deveria ter selecionada a primeira cor do vetor" );
        }

        [TestMethod]
        public void RnEscolherCorQuando2ColaboradoresDiferentesNoMesmoProjetoSelecionaremCor()
        {
            string cor1 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );
            string cor2 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[1] );

            string corSelecionada1 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );
            Assert.AreEqual( cor1, corSelecionada1, "Deveria ter selecionada a primeira cor do vetor" );
            string corSelecionada2;
            corSelecionada2 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador2.Oid, projeto1.Oid );

            Assert.AreEqual( cor2, corSelecionada2, "Deveria ter selecionada a segunda cor do vetor" );
        }
        /// <summary>
        /// Método responsável por testar se está criando uma cor para o colaborador naquele projeto se ele ainda não possuir uma.
        /// </summary>
        [TestMethod]
        public void RnEscolherCorQuandoNaoHouverCorAnteriorParaColaboradorNaqueleProjetoTest()
        {
            String cor = null;
            //Cria ou resgata cor do colaborador em um determinado projeto.
            cor = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );
            //Verificação
            Assert.IsNotNull( cor, "Cor não deveria ser nulo, pois uma cor deveria ter sido escolhida e salva no banco de dados" );
        }

        /// <summary>
        /// Método responsável por testar se está escolhendo cores diferentes para os colaboradores em um mesmo projeto.
        /// </summary>
        [TestMethod]
        public void RnEscolherCorQuandoHouverCorAnteriorParaOutroColaboradorTest()
        {
            String cor = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );
            String cor2 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[1] );
            String cor3 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[2] );
            string corSelecionada1, corSelecionada2, corSelecionada3;

            //Cria projeto e colaborador
            Colaborador colaborador = ColaboradorFactory.CriarColaborador( SessionTest, "anderson" );
            Projeto projeto = ProjetoFactory.Criar( SessionTest, 100, "projeto 01", true );

            corSelecionada1 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador.Oid, projeto.Oid );

            Colaborador colaborador2 = ColaboradorFactory.CriarColaborador( SessionTest, "paulo" );

            ProjetoColaboradorConfig projetoColaboradorConfig2 = new ProjetoColaboradorConfig( SessionTest );
            corSelecionada2 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador2.Oid, projeto.Oid );

            Colaborador colaborador3 = ColaboradorFactory.CriarColaborador( SessionTest, "pedro" );

            corSelecionada3 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador3.Oid, projeto.Oid );

            //Verificação 1
            Assert.AreNotEqual( corSelecionada1, corSelecionada2, "Cores deveriam ser diferentes, pois colaboradores são diferentes" );
            Assert.AreNotEqual( corSelecionada1, corSelecionada3, "Cores deveriam ser diferentes, pois colaboradores são diferentes" );
            Assert.AreNotEqual( corSelecionada3, corSelecionada2, "Cores deveriam ser diferentes, pois colaboradores são diferentes" );

            //Verificação 2
            //O primeiro recebe a cor "Black"
            Assert.AreEqual( cor, corSelecionada1,
                "Deveriam ser iguais, pois foi o primeiro colaborador a ser cadastrado no cronograma daquele projeto" );
            //O segundo recebe a cor "Aqua"
            Assert.AreEqual( cor2, corSelecionada2,
                "Deveriam ser iguais, pois foi o segundo colaborador a ser cadastrado no cronograma daquele projeto" );
            //O segundo recebe a cor "Beige"
            Assert.AreEqual( cor3, corSelecionada3,
                "Deveriam ser iguais, pois foi o terceiro colaborador a ser cadastrado no cronograma daquele projeto" );
        }

        /// <summary>
        /// Método responsável por testar a cor do colaborador escolhida caso o número passe de 29.
        /// </summary>
        [TestMethod]
        public void RnEscolherCorQuandoHouverMaisde29EscolhasAnteriorTest()
        {
            Dictionary<int, Colaborador> listaDeColaboradores = new Dictionary<int, Colaborador>();
            Projeto projeto = ProjetoFactory.Criar( SessionTest, 100, "projeto 01", true );

            string a = "a";
            string cor = null;
            for(Int32 i = 0; i <= 30; i++)
            {
                Colaborador colaborador = ColaboradorFactory.CriarColaborador( SessionTest, ( a + i.ToString() ) );
                listaDeColaboradores[i] = colaborador;
                ProjetoColaboradorConfig.RnEscolherCor( SessionTest, listaDeColaboradores[i].Oid, projeto.Oid );
            }

            cor = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, listaDeColaboradores[30].Oid, projeto.Oid );
            Assert.IsNotNull( cor );
            Color corSelecionada = Color.FromArgb( Convert.ToInt32( cor ) );
            Assert.IsTrue( !ColaboradorConfigBo.Cores.Contains( corSelecionada ) );
        }

        [TestMethod]
        public void RnEscolherCorQuandoHouverEscolhaAnteriorEmOutroProjetoTest()
        {
            //Cria projeto e colaborador
            Colaborador colaborador = ColaboradorFactory.CriarColaborador( SessionTest, "anderson" );
            Colaborador colaborador1 = ColaboradorFactory.CriarColaborador( SessionTest, "pedro" );

            Projeto projeto = ProjetoFactory.Criar( SessionTest, 100, "projeto 01", true );
            Projeto projeto1 = ProjetoFactory.Criar( SessionTest, 100, "projeto 02", true );

            String cor = null;
            String cor2 = null;
            String cor3 = null;

            //Cria ou resgata cor do colaborador em um determinado projeto.
            cor = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador.Oid, projeto.Oid );

            cor2 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto.Oid );

            cor3 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );

            Assert.IsNotNull( cor, "Deveria ter criado uma cor" );
            Assert.IsNotNull( cor2, "Deveria ter criado uma cor" );
            Assert.IsNotNull( cor3, "Deveria ter criado uma cor em outro projeto" );
            Assert.AreNotEqual( cor2, cor3,
                "As cores deveriam ser diferentes, pois no projeto 01, pedro foi o 2º a ser cadastrado e no projeto 2 foi o 1º a ser cadastrado " );
        }

        /*
         *  Cenário: realizar consulta a partir do userName (login)  do usuário
         *  Espectativa: devolver um objeto ProjetoColaboradorConfig
         *               as cores serem as mesmas
         */
        [TestMethod]
        public void GetCorColaboradorPorUserName()
        {
            string cor = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );
            string corSelecionada = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Usuario.UserName, projeto1.Oid );
            Assert.AreEqual( cor, corSelecionada, "Deveria ter selecionado a cor esperada" );
        }

        /*
         *  Cenário: realizar consulta por todas as cores de um determinado projeto
         *  Espectativa: retorna uma lista com os objetos
         */
        [TestMethod]
        public void GetCoresProjeto()
        {
            //Cria projeto e colaborador
            //Colaborador colaborador1 = ColaboradorFactory.CriarColaborador( SessionTest, "anderson" );
            //Colaborador colaborador2 = ColaboradorFactory.CriarColaborador( SessionTest, "pedro" );
            //Colaborador colaborador3 = ColaboradorFactory.CriarColaborador( SessionTest, "paulo" );
            //Colaborador colaborador4 = ColaboradorFactory.CriarColaborador( SessionTest, "henrique" );

            //Projeto projeto1 = ProjetoFactory.Criar( SessionTest, 100, "projeto 01", true );
            //Projeto projeto2 = ProjetoFactory.Criar( SessionTest, 100, "projeto 02", true );

            //string cor1 = null;
            //string cor2 = null;
            //string cor3 = null;
            //string cor4 = null;

            ////Cria ou resgata cor do colaborador em um determinado projeto.
            //cor1 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );
            //cor2 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador2.Oid, projeto1.Oid );
            //cor3 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador3.Oid, projeto2.Oid );
            //cor4 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador4.Oid, projeto2.Oid );

            //List<ProjetoColaboradorConfig> lstCores1 = ProjetoColaboradorConfig.GetCoresPorProjeto( SessionTest, projeto1.Oid );
            //List<ProjetoColaboradorConfig> lstCores2 = ProjetoColaboradorConfig.GetCoresPorProjeto( SessionTest, projeto2.Oid );

            //Assert.IsNotNull( lstCores1 );
            //Assert.IsNotNull( lstCores2 );
            //Assert.AreEqual( 2, lstCores1.Count );
            //Assert.AreEqual( 2, lstCores2.Count );
            //Assert.AreEqual( cor1, lstCores1[0].Cor );
            //Assert.AreEqual( cor2, lstCores1[1].Cor );
            //Assert.AreEqual( cor3, lstCores2[0].Cor );
            //Assert.AreEqual( cor4, lstCores2[1].Cor );
        }

        /*
         *  Cenário: realizar consulta para buscar cor de colaborador em um determinado projeto
         *  Espectativa: retorna a cor do colaborador naquele projeto.
         */
        [TestMethod]
        public void GetCorColaboradorPorProjeto()
        {
            ////Cria projeto e colaborador
            //Colaborador colaborador1 = ColaboradorFactory.CriarColaborador( SessionTest, "anderson" );
            //Colaborador colaborador2 = ColaboradorFactory.CriarColaborador( SessionTest, "pedro" );

            //Projeto projeto1 = ProjetoFactory.Criar( SessionTest, 100, "projeto 01", true );
            //Projeto projeto2 = ProjetoFactory.Criar( SessionTest, 100, "projeto 02", true );

            //string cor1 = null;
            //string cor2 = null;

            ////Cria ou resgata cor do colaborador em um determinado projeto.
            //cor1 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador1.Oid, projeto1.Oid );
            //cor2 = ProjetoColaboradorConfig.RnEscolherCor( SessionTest, colaborador2.Oid, projeto1.Oid );

            //ProjetoColaboradorConfig projColConfig = ProjetoColaboradorConfig.GetCorColaboradorPorProjeto( SessionTest, colaborador1.Oid, projeto1.Oid );

            //Assert.IsNotNull( projColConfig );
            //Assert.AreEqual( cor1, projColConfig.Cor );
            //Assert.AreEqual( colaborador1, projColConfig.Colaborador );
            //Assert.AreEqual( projeto1, projColConfig.Projeto );
        }

        [TestMethod]
        public void GetConfigColaboradoresQuandoVariosColaboradoresJaTiveramSuasCoresSelecionadas()
        {
            string cor1 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );
            string cor2 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[1] );
            string cor3 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[2] );
            string cor4 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );

            string corSelecionda1 = ProjetoColaboradorConfig.RnEscolherCor( s1, colaborador1.Oid, projeto1.Oid );
            SessionTest.DropIdentityMap();
            string corSelecionda2 = ProjetoColaboradorConfig.RnEscolherCor( s2, colaborador2.Oid, projeto1.Oid );
            SessionTest.DropIdentityMap();
            string corSelecionda3 = ProjetoColaboradorConfig.RnEscolherCor( s3, colaborador3.Oid, projeto1.Oid );
            SessionTest.DropIdentityMap();
            string corSelecionda4 = ProjetoColaboradorConfig.RnEscolherCor( s4, colaborador4.Oid, projeto2.Oid );

            List<ProjetoColaboradorConfig> configs = null;
            ControleDeEsperaUtil.AguardarAte( () => 
            {
                configs = new List<ProjetoColaboradorConfig>(ProjetoColaboradorConfig.GetConfigColaboradores(SessionTest,projeto1.Oid));
                return configs != null && configs.Count == 3;
            } );
            //List<ProjetoColaboradorConfig> configs = ProjetoColaboradorConfig.GetConfigColaboradores( SessionTest, projeto1.Oid );
            //Assert.AreEqual( 3, configs.Count, string.Format( "Deveria possuir 3 configs salvos para o {0}",projeto1.TxNome ) );
            Assert.AreEqual(cor1,corSelecionda1,"Deveria ter encontrado a cor selecionada para o usuário");
            Assert.AreEqual( cor2, corSelecionda2, "Deveria ter encontrado a cor selecionada para o usuário" );
            Assert.AreEqual( cor3, corSelecionda3, "Deveria ter encontrado a cor selecionada para o usuário" );
            Assert.AreEqual( cor4, corSelecionda4, "Deveria ter encontrado a cor selecionada para o usuário" );
        }

        [TestMethod]
        public void SelecionarCorQuandoNaoHouverNenhumaCorSelecionada() 
        {
            string cor1 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[0] );
            string cor2 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[1] );
            string cor3 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[2] );
            string cor4 = ConverterCorToArgbString( ColaboradorConfigBo.Cores[3] );
            List<string> todasCoresPadrao = new List<string>( ColaboradorConfigBo.Cores.Select( o => ConverterCorToArgbString( o ) ) );
            List<string> coresSelecionadas = new List<string>();

            string corSelecionda1 = ColaboradorConfigBo.SelecionarCor( coresSelecionadas );
            coresSelecionadas.Add( corSelecionda1 );

            string corSelecionda2 = ColaboradorConfigBo.SelecionarCor( coresSelecionadas );
            coresSelecionadas.Add( corSelecionda2 );

            string corSelecionda3 = ColaboradorConfigBo.SelecionarCor( coresSelecionadas );
            coresSelecionadas.Add( corSelecionda3 );

            string corSelecionda4 = ColaboradorConfigBo.SelecionarCor( coresSelecionadas );
            coresSelecionadas.Add( corSelecionda4 );

            Assert.AreEqual( cor1, corSelecionda1, "A cor selecionada deveria corresponder a primeira cor" );
            Assert.AreEqual( cor2, corSelecionda2, "A cor selecionada deveria corresponder a segunda cor" );
            Assert.AreEqual( cor3, corSelecionda3, "A cor selecionada deveria corresponder a terceira cor" );
            Assert.AreEqual( cor4, corSelecionda4, "A cor selecionada deveria corresponder a quarta cor" );

            string corSelecao;
            for(int i = 4; i < ColaboradorConfigBo.Cores.Length; i++)
            {
                corSelecao = ColaboradorConfigBo.SelecionarCor( coresSelecionadas );
                coresSelecionadas.Add( corSelecao );
            }

            CollectionAssert.AreEqual( todasCoresPadrao, coresSelecionadas ,"Todas as cores deveriam ter sido selecionadas");

            List<string> novasCores = new List<string>();
            for(int i = 0; i < 5; i++)
            {
                corSelecao = ColaboradorConfigBo.SelecionarCor( coresSelecionadas );
                novasCores.Add(corSelecao);
                coresSelecionadas.Add( corSelecao );
            }

            foreach(string novaCor in novasCores)
            {
                Assert.IsFalse( todasCoresPadrao.Contains( novaCor ),"Nenhuma cor deveria ser repetiva após a quantidade padrão de cores!" );
            }
        }
    }
}
