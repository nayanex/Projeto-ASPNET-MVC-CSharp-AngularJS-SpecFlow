using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CasoTestePreCondicaoAnexoTest
    /// </summary>
    [TestClass]
    public class CasoTestePreCondicaoAnexoTest : BaseTest
    {
        /// <summary>
        /// método AdicionarAnexo
        /// </summary>
        [TestMethod]
        public void AdicionarAnexo()
        {
            /**
             * Cenário 1: Serão adicionados 3 anexos para uma pré-condição.
             * O sistema deverá informar quantos anexos estão adicionados no sistema.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Requisito requisito1 = RequisitoFactory.Criar(SessionTest, modulo1, "nome", "descricao", "link", true);
            CasoTeste casoteste1 = CasoTesteFactory.Criar(SessionTest, requisito1, "passos", "sumário", "precondicoes", true);
            CasoTestePreCondicao casotestePreCondicao1 = CasoTestePreCondicaoFactory.Criar(SessionTest, casoteste1, "", true);

            CasoTestePreCondicaoAnexo casoTestePreCondicaoAnexo1 = CasoTestePreCondicaoAnexoFactory.Criar(SessionTest, casotestePreCondicao1, "", true);
            CasoTestePreCondicaoAnexo casoTestePreCondicaoAnexo2 = CasoTestePreCondicaoAnexoFactory.Criar(SessionTest, casotestePreCondicao1, "", true);
            CasoTestePreCondicaoAnexo casoTestePreCondicaoAnexo3 = CasoTestePreCondicaoAnexoFactory.Criar(SessionTest, casotestePreCondicao1, "", true);
            Assert.AreEqual(3, casotestePreCondicao1._NbAnexos, "O valor deveria ser 3");
        }

    }
}
