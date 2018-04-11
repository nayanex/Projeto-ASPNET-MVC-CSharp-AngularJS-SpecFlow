using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CasoTestePassoResultadoAnexoTest
    /// </summary>
    [TestClass]
    public class CasoTestePassoResultadoAnexoTest : BaseTest
    {
        /// <summary>
        /// método AdicionarAnexo
        /// </summary>
        [TestMethod]
        public void AdicionarAnexo()
        {
            /**
             * Cenário 1: Serão adicionados 3 anexos para um resultado esperado.
             * O sistema deverá informar quantos anexos estão cadastrados no sistema para aquele resultado esperado.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePasso casotestePasso1 = CasoTestePassoFactory.Criar(SessionTest, casoteste1, "", true);
            CasoTestePassoResultadoEsperado casoTestePassoResultadoEsperado1 = CasoTestePassoResultadoEsperadoFactory.Criar(SessionTest, casotestePasso1, "", true);

            CasoTestePassoResultadoEsperadoAnexo casoTestePassoResultadoEsperadoAnexo1 = CasoTestePassoResultadoEsperadoAnexoFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, "" , true);
            CasoTestePassoResultadoEsperadoAnexo casoTestePassoResultadoEsperadoAnexo2 = CasoTestePassoResultadoEsperadoAnexoFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, "", true);
            CasoTestePassoResultadoEsperadoAnexo casoTestePassoResultadoEsperadoAnexo3 = CasoTestePassoResultadoEsperadoAnexoFactory.Criar(SessionTest,
            casoTestePassoResultadoEsperado1, "", true);

            //Assert.AreEqual(3, casoTestePassoResultadoEsperado1._NbAnexo, "O valor deveria ser 3");
        }
    }
}
