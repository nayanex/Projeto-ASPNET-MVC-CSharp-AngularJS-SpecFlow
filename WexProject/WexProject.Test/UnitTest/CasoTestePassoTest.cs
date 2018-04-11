using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CasoTestePassoTest
    /// </summary>
    [TestClass]
    public class CasoTestePassoTest : BaseTest
    {
        /// <summary>
        /// método CriarSequenciaPasso
        /// </summary>
        [TestMethod]
        public void CriarSequenciaPasso()
        {
            /**
                        * Cenário 1: Serão criados 3 passos para um caso de teste.
                        * O sistema deverá cria-los com a sequencia 1, 2, 3 respectivamente.
                        */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(1, casotestePasso1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePasso casotestePasso2 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(2, casotestePasso2.NbSequencia, "A sequência deveria ser 1");

            CasoTestePasso casotestePasso3 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(3, casotestePasso3.NbSequencia, "A sequência deveria ser 1");

        }
        /// <summary>
        /// método ReordenarSequencia
        /// </summary>
        [TestMethod]
        public void ReordenarSequencia()
        {

            /**
             * Cenário 2: Serão criados 3 passos para um caso de teste.
             * Em seguida  as sequência dos passo serão mudadas.
             * O sistema deverá reordenar as sequências dos passos restantes corretamente.
             */

            //Passo 1

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(1, casotestePasso1.NbSequencia, "A sequência deveria ser 1");

            //Passo 2

            CasoTestePasso casotestePasso2 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePasso casotestePasso3 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(2, casotestePasso2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePasso3.NbSequencia, "A sequência deveria ser 3");

            //Passo 3

            casotestePasso1.NbSequencia = 2;
            Assert.AreEqual(1, casotestePasso2.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePasso1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePasso3.NbSequencia, "A sequência deveria ser 3");

            //passo 4

            casotestePasso2.NbSequencia = 3;
            Assert.AreEqual(1, casotestePasso1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePasso3.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePasso2.NbSequencia, "A sequência deveria ser 3");

            //Passo 5

            casotestePasso2.NbSequencia = 2;
            Assert.AreEqual(1, casotestePasso1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePasso2.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePasso3.NbSequencia, "A sequência deveria ser 3");

            //Passo 6

            casotestePasso3.NbSequencia = 1;
            Assert.AreEqual(1, casotestePasso3.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePasso1.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, casotestePasso2.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método DeletarUmPasso
        /// </summary>
        [TestMethod]
        public void DeletarUmPasso()
        {
            /**
             * Cenário 3: Serão criados 3 passos.
             * O passo 2 deverá ser deletado e os outros dois devem ser reordenados para 1 e 2 respectivamente.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(1, casotestePasso1.NbSequencia, "A sequência deveria ser 1");

            CasoTestePasso casotestePasso2 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(2, casotestePasso2.NbSequencia, "A sequência deveria ser 1");

            CasoTestePasso casotestePasso3 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            Assert.AreEqual(3, casotestePasso3.NbSequencia, "A sequência deveria ser 1");



            casotestePasso2.Delete();


            Assert.AreEqual(1, casotestePasso1.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, casotestePasso3.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método CriarUmNovoPassoComAcessoRapido
        /// </summary>
        //[TestMethod]
        public void CriarUmNovoPassoComAcessoRapido()
        {
            /**
             * Cenário 4: Será criado um passo para o caso de teste
             * O primeiro resultado esperado será inserido através do acesso rápido
             * O segundo resultado será criado através do detail de resultado esperado
             * O sistema deverá concatenar corretamente os valores dos resultados esperados
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);

            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);


        }
    }
}
