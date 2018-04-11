using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CasoTestTest
    /// </summary>
    [TestClass]
    public class CasoTestTest : BaseTest
    {
        /// <summary>
        /// método CriarIdCasoTeste
        /// </summary>
        [TestMethod]
        public void CriarIdCasoTeste()
        {
            /**
             * Cenário 1: Serão criado 1 caso de teste para um requisito.
             * O sistema deverá salvá-lo com o ID CT_1.1.1
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            Assert.AreEqual("CT_01.01.01", casoteste1.TxID, "O ID do caso de teste deveria ser CT_01.1.01");


        }
        /// <summary>
        /// método GuardarValorAntigoDoRequisito 
        /// </summary>
        [TestMethod]
        public void GuardarValorAntigoDoRequisito()
        {
            /**
             * Cenário 2: Serão criado 2 casos de teste para um requisito.
             * Ao se criar o primeiro, o sistema deverá guardar o valor do requisito e exibi-lo como sugestão quando for cadastrar o segundo
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto1.Oid; // Projeto atual

            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            //Requisito requisito2 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            Assert.AreEqual("CT_01.01.01", casoteste1.TxID, "O ID do caso de teste deveria ser CT_01.1.01");

            CasoTeste casoteste2 = new CasoTeste(SessionTest);
            Assert.AreEqual(requisito1, casoteste2.Requisito, "O requisito deveria ser o mesmo do anterior");
        }

        /// <summary>
        /// Para excluir caso de teste
        /// </summary>
        [TestMethod]
        public void ExcluirCasoTeste()
        {
            /*
             * Cenário: Será criado o casoteste01, será associado o ele uma descrição, um passo e um resultado esperado
                                     * o casoteste01 será excluido, verificar se o casoteste01 foi excluido com sucesso
                                     */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto1.Oid;

            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            //Criação do casoteste01
            CasoTeste casoteste01 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            casoteste01.Save();
            //Exclusão do casoteste01
            casoteste01.Delete();

            Assert.AreEqual(0, requisito1.RequisitoCasosTeste.Count, "Deveria existir 0 casoteste associado ao requisito");
        }
    }
}
