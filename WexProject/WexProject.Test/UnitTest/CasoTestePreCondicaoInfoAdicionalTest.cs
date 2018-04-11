using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CasoTestePreCondicaoInfoAdicionalTest
    /// </summary>
    [TestClass]
    public class CasoTestePreCondicaoInfoAdicionalTest : BaseTest
    {
        /// <summary>
        /// método CriarSequenciaPreCondicao
        /// </summary>
        [TestMethod]
        public void CriarSequenciaPreCondicao()
        {
            /**
             * Cenário 1: Serão criadas 3 informações adicionais para uma pré-condição.
             * O sistema deverá salvá-lo com a sequência correta
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional1 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional2 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional3 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método ReordenarSequencia
        /// </summary>
        [TestMethod]
        public void ReordenarSequencia()
        {

            /**
             * Cenário 2: Serão criadoas 3 informações adicionais para uma pré-condição.
             * Em seguida  as sequências serão mudadas.
             * O sistema deverá reordenar corretamente
             */

            //Passo 1

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional1 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            //Passo 2

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional2 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional3 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            //Passo 3
            casoTestePreCondicaoInfoAdicional1.NbSequencia = 2;
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            //Passo 4

            casoTestePreCondicaoInfoAdicional2.NbSequencia = 3;
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 3");

            //Passo 5

            casoTestePreCondicaoInfoAdicional2.NbSequencia = 2;
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            //Passo 6

            casoTestePreCondicaoInfoAdicional3.NbSequencia = 1;
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método DeletarUmaPreCondicao
        /// </summary>
        [TestMethod]
        public void DeletarUmaPreCondicao()
        {
            /**
             * Cenário 4: Serão criadas 3 informações adicionais para uma pré-condição.
             * A informação adicional 2 deverá ser deletada e as outras duas devem ser reordenadas para 1 e 2 respectivamente.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional1 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional2 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional3 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            casoTestePreCondicaoInfoAdicional2.Delete();

            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método AdicionarInformação
        /// </summary>
        [TestMethod]
        public void AdicionarInformação()
        {
            /**
             * Cenário 5: Serão criadas 3 informações adicionais para uma pré-condição.
             * O sistema deverá informar quantas informações estão cadastradas no sistema.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional1 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(1, casoTestePreCondicaoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional2 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(2, casoTestePreCondicaoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional3 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(3, casoTestePreCondicaoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            CasoTestePreCondicaoInformacaoAdicional casoTestePreCondicaoInfoAdicional4 = CasoTestePreCondicaoInformacaoAdicionalFactory.Criar(SessionTest, casotestePreCondicao1, true);
            Assert.AreEqual(4, casoTestePreCondicaoInfoAdicional4.NbSequencia, "A sequência deveria ser 3");

            casoTestePreCondicaoInfoAdicional3.Delete();

            Assert.AreEqual(3, casotestePreCondicao1._NbInformacoesAdicionais, "O valor deveria ser 3");
        }

    }
}
