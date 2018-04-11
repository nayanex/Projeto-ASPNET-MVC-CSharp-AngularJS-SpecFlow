using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CasoTestePassoResultadoEsperadoTest
    /// </summary>
    [TestClass]
    public class CasoTestePassoResultadoEsperadoTest : BaseTest
    {
        /// <summary>
        /// método CriarSequenciaResultadoEsperado
        /// </summary>
        [TestMethod]
        public void CriarSequenciaResultadoEsperado()
        {
            /**
             * Cenário 1: Serão criados 3 resultados esperados para um passo.
             * O sistema deverá cria-los com a sequencia 1, 2, 3 respectivamente.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casoTestePasso = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(1, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado2 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(2, casoTestePassoResultadoEsperado2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado3 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(3, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 3");

        }
        /// <summary>
        /// método DescerSequenciaResultadoEsperado
        /// </summary>
        [TestMethod]
        public void DescerSequenciaResultadoEsperado()
        {

            /**
             * Cenário 2: Serão criados 3 resultados esperados para um passo.
             * Em seguida  as sequências do resultado esperado serão mudadas.
             * O sistema deverá reordenar as sequências dos resultados restantes corretamente.
             */

            //Passo 1

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casoTestePasso = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(1, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 1");

            //Passo 2

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado2 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado3 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(2, casoTestePassoResultadoEsperado2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 3");

            //Passo 3

            casoTestePassoResultadoEsperado1.NbSequencia = 2;
            Assert.AreEqual(1, casoTestePassoResultadoEsperado2.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 3");

            //Passo 4

            casoTestePassoResultadoEsperado2.NbSequencia = 3;
            Assert.AreEqual(1, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperado2.NbSequencia, "A sequência deveria ser 3");

            //Passo 5

            casoTestePassoResultadoEsperado2.NbSequencia = 2;
            Assert.AreEqual(1, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperado2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 3");

            //Passo 6

            casoTestePassoResultadoEsperado3.NbSequencia = 1;
            Assert.AreEqual(1, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casoTestePassoResultadoEsperado2.NbSequencia, "A sequência deveria ser 3");
        }

        /// <summary>
        /// método DeletarUmResultadoEsperado
        /// </summary>
        [TestMethod]
        public void DeletarUmResultadoEsperado()
        {
            /**
             * Cenário 4: Serão criados 3 resultados esperados.
             * O resultado 2 deverá ser deletado e os outros dois devem ser reordenados para 1 e 2 respectivamente.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casoTestePasso = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(1, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado2 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(2, casoTestePassoResultadoEsperado2.NbSequencia, "A sequência deveria ser 2");

            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado3 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casoTestePasso, "", true);
            Assert.AreEqual(3, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 3");

            casoTestePassoResultadoEsperado2.Delete();

            Assert.AreEqual(1, casoTestePassoResultadoEsperado1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casoTestePassoResultadoEsperado3.NbSequencia, "A sequência deveria ser 2");
        }

    }
}
