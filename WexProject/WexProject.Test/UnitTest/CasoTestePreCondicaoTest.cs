using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CasoTestePreCondicaoTest
    /// </summary>
    [TestClass]
    public class CasoTestePreCondicaoTest : BaseTest
    {
        /// <summary>
        /// método CriarSequenciaPreCondicao
        /// </summary>
        [TestMethod]
        public void CriarSequenciaPreCondicao()
        {
            /**
             * Cenário 1: Serão criados 3 pré-condições para um caso de teste.
             * O sistema deverá cria-los com a sequencia 1, 2, 3 respectivamente.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(1, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePreCondicao casotestePreCondicao2 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(2, casotestePreCondicao2.NbSequencia, "A sequência deveria ser 2");


            CasoTestePreCondicao casotestePreCondicao3 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(3, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 3");

        }
        /// <summary>
        /// método ReordenarSequencia
        /// </summary>
        [TestMethod]
        public void ReordenarSequencia()
        {

            /**
             * Cenário 2: Serão criados 3 pré-condições para um caso de teste.
             * Em seguida  as sequências serão mudadas.
             * O sistema deverá reordenar corretamente
             */

            //Passo 1

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(1, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 1");

            //Passo 2

            CasoTestePreCondicao casotestePreCondicao2 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePreCondicao casotestePreCondicao3 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(2, casotestePreCondicao2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 3");

            //Passo 3

            casotestePreCondicao1.NbSequencia = 2;
            Assert.AreEqual(1, casotestePreCondicao2.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 3");

            //Passo 4

            casotestePreCondicao2.NbSequencia = 3;
            Assert.AreEqual(1, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePreCondicao2.NbSequencia, "A sequência deveria ser 3");

            //Passo 5

            casotestePreCondicao2.NbSequencia = 2;
            Assert.AreEqual(1, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePreCondicao2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 3");

            //Passo 6

            casotestePreCondicao3.NbSequencia = 1;
            Assert.AreEqual(1, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePreCondicao2.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método DeletarUmaPreCondicao
        /// </summary>
        [TestMethod]
        public void DeletarUmaPreCondicao()
        {
            /**
             * Cenário 4: Serão criadas 3 pré-condições.
             * A pré-condição 2 deverá ser deletada e as outras duas devem ser reordenadas para 1 e 2 respectivamente.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(1, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePreCondicao casotestePreCondicao2 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(2, casotestePreCondicao2.NbSequencia, "A sequência deveria ser 2");


            CasoTestePreCondicao casotestePreCondicao3 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(3, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 3");

            casotestePreCondicao2.Delete();

            Assert.AreEqual(1, casotestePreCondicao1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePreCondicao3.NbSequencia, "A sequência deveria ser 3");
        }
    }
}
