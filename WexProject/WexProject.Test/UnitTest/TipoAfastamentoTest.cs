using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes da classe TipoAfastamento
    /// </summary>
    [TestClass]
    public class TipoAfastamentoTest : BaseTest
    {
        /// <summary>
        /// Testar cadastrar um tipo de afastamento para férias realizadas quando já existir um tipo para férias realizadas.
        /// </summary>
        [TestMethod]
        public void TipoAfastamentoTest_TestarCadastrarUmTipoAfastamentoParaFeriasRealizadasQuandoJaExistirUmTipoParaFeriasRealizadas()
        {
            TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Férias", true, true, CsSituacao.Inativo, true);

            #region Passo 1

            // Novo tipo de afastamento
            TipoAfastamento tipo01 = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest,
                "Tipo de Afastamento", true, true, CsSituacao.Ativo);

            Assert.IsFalse(tipo01.RnVerificarExistenciaOutroTipoAfastamentoParaFeriasRealizadas(),
                "Não deveria ter encontrado um outro item para férias realizadas");

            // Persistência
            tipo01.Save();

            Assert.AreEqual(2, new XPCollection<TipoAfastamento>(SessionTest).Count,
                "Deveriam ter 2 itens salvos");

            #endregion

            #region Passo 2

            // Novo tipo de afastamento
            TipoAfastamento tipo02 = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest,
                "Tipo", true, true, CsSituacao.Ativo);

            Assert.IsTrue(tipo02.RnVerificarExistenciaOutroTipoAfastamentoParaFeriasRealizadas(),
                "Deveria ter encontrado um outro item para férias realizadas");

            #endregion
        }
    }
}