using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Classe CasoTestePassoResultadoInfoAdicionalTest
    /// </summary>
    [TestClass]
    public class CasoTestePassoResultadoInfoAdicionalTest : BaseTest
    {
        /// <summary>
        /// método CriarSequenciaPreCondicao
        /// </summary>
        [TestMethod]
        public void CriarSequenciaPreCondicao()
        {
            /**
             * Cenário 1: Serão criadas 3 informações adicionais para um resultado esperado.
             * O sistema deverá salvá-lo com a sequência correta
             */
            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casotestePasso1, "", true);

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional1 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional2 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional3 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método ReordenarSequencia
        /// </summary>
        [TestMethod]
        public void ReordenarSequencia()
        {

            /**
             * Cenário 2: Serão criadoas 3 informações adicionais para um resultado esperado.
             * Em seguida  as sequências serão mudadas.
             * O sistema deverá reordenar corretamente
             */

            //Passo 1
            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casotestePasso1, "", true);

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional1 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            //Passo 2
            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional2 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional3 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);

            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            //Passo 3

            casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia = 2;
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            //Passo 4

            casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia = 3;
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 3");

            //Passo 5

            casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia = 2;
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            //Passo 6

            casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia = 1;
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método DeletarUmaPreCondicao
        /// </summary>
        [TestMethod]
        public void DeletarUmaPreCondicao()
        {
            /**
             * Cenário 4: Serão criadas 3 informações adicionais para um resultado esperado.
             * A informação adicional 2 deverá ser deletada e as outras duas devem ser reordenadas para 1 e 2 respectivamente.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casotestePasso1, "", true);

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional1 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional2 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional3 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            casoTestePassoResultadoEsperadoInfoAdicional2.Delete();

            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método AdicionarInformação
        /// </summary>
        [TestMethod]
        public void AdicionarInformação()
        {
            /**
             * Cenário 5: Serão criadas 3 informações adicionais para um resultado esperado.
             * O sistema deverá informar quantas informações estão cadastradas no sistema para aquele resultado esperado.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casotestePasso1, "", true);

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional1 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(1, casoTestePassoResultadoEsperadoInfoAdicional1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional2 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(2, casoTestePassoResultadoEsperadoInfoAdicional2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional3 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(3, casoTestePassoResultadoEsperadoInfoAdicional3.NbSequencia, "A sequência deveria ser 3");

            CasoTestePassoResultadoEsperadoInformacaoAdicional casoTestePassoResultadoEsperadoInfoAdicional4 = CasoTestePassoResultadoEsperadoInformacaoAdicionalFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, true);
            Assert.AreEqual(4, casoTestePassoResultadoEsperadoInfoAdicional4.NbSequencia, "A sequência deveria ser 4");

            casoTestePassoResultadoEsperadoInfoAdicional3.Delete();

            //Assert.AreEqual(3, casoTestePassoResultadoEsperado1._NbInformacaoAdicional, "O valor deveria ser 3");
        }
    }
}
