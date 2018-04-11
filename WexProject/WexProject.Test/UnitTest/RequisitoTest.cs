using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe RequisitoTest
    /// </summary>
    [TestClass]
    public class RequisitoTest : BaseTest
    {
        /// <summary>
        /// método SalvarRequisito
        /// </summary>
        [TestMethod]
        public void SalvarRequisito()
        {
            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);

            /**
             * Cenário 1: Serão criados 2 requisitos para um projeto e o wex
             * deverá criar os IDs incrementados em sequencia.
             */

            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);

            Assert.AreEqual("RF_01.01", requisito1.TxID, "O ID do requisito deveria ser RF_01.01");

            requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);

            Assert.AreEqual("RF_01.02", requisito1.TxID, "O ID do requisito deveria ser RF_01.02");

        }
        /// <summary>
        /// método SalvarRequisitoJaExitenteEmUmProjeto
        /// </summary>
        [TestMethod]
        public void SalvarRequisitoJaExitenteEmUmProjeto()
        {

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);

            /**
             * Cenário 2: Serão criados 3 requisitos para o projeto e em seguida será deletado o segundo requisito.
             * Após isso, será criado mais 1 modulo, que o wex deverá criar como RF_4.
             */

            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);

            Assert.AreEqual("RF_01.01", requisito1.TxID, "O ID do modulo deveria ser RF_01.01");

            requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);

            Assert.AreEqual("RF_01.02", requisito1.TxID, "O ID do modulo deveria ser RF_01.02");

            Assert.AreEqual("RF_01.03", RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true).TxID, "O ID do modulo deveria ser RF_01.03");

            //Criar o delete Rf_1.2

            requisito1.Delete();

            Assert.AreEqual("RF_01.04", RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true).TxID, "O ID do modulo deveria ser RF_01.04");

        }
        /// <summary>
        /// método SalvarSubModulo
        /// </summary>
        [TestMethod]
        public void SalvarSubModulo()
        {

            /**
             * Cenário 3: Será criado um requisito para um sub-módulo e o wex deverá salvar da forma correta. Ex: RF_1.1.2
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Modulo moduloPai = ModuloFactory.CriarModuloFilho(SessionTest, modulo1, "", true);
            Requisito requisito = RequisitoFactory.Criar(SessionTest, moduloPai, "nome", "descricao", "link", true);

            Assert.AreEqual("RF_01.01.01", requisito.TxID, "O ID do modulo deveria ser RF_01.01.01");

            requisito = RequisitoFactory.Criar(SessionTest, moduloPai, "nome", "descricao", "link", true);
            Assert.AreEqual("RF_01.01.02", requisito.TxID, "O ID do modulo deveria ser RF_01.01.02");

        }
        /// <summary>
        /// método TrocarModuloModificarID
        /// </summary>
        [TestMethod]
        public void TrocarModuloModificarID()
        {

            /**
             * Cenário 4: Será um requisito com módulo 01 e depois troca-se-á para módulo 2 e será recalculado o id
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 10, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "Módulo 1", true);
            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto1, "Módulo 2", true);

            Requisito requisito = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);

            Assert.AreEqual("01", modulo1.TxID, "O ID do modulo deveria ser RF_1.01.01");
            Assert.AreEqual("02", modulo2.TxID, "O ID do modulo deveria ser RF_1.01.01");

            Assert.AreEqual("RF_01.01", requisito.TxID, "O ID do modulo deveria ser RF_1.01.01");

            requisito.Modulo = modulo2;
            requisito.Save();

            Assert.AreEqual("RF_02.01", requisito.TxID, "O ID do modulo deveria ser RF_1.01.02");

        }
    }
}
