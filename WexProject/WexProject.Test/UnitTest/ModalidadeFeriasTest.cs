using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Rh;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Xaf;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes da classe ModalidadeFerias
    /// </summary>
    [TestClass]
    public class ModalidadeFeriasTest : BaseTest
    {
        /// <summary>
        /// Testar cadastrar a quantidade de dias maior que o valor máximo de dias que
        /// se pode tirar férias (definido nas configurações).
        /// </summary>
        [TestMethod]
        public void ModalidadeFeriasTest_TestarCadastrarQuantidadeDiasMaiorValorMaximoDiasPodeTirarFerias()
        {
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);
            ModalidadeFerias modalidade = ModalidadeFeriasFactory.CriarModalidadeFerias(
                SessionTest, 45, false, CsSituacao.Ativo, false);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(modalidade,
                "ModalidadeFerias_RnVerificarDiasMaxFerias", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar cadastrar a quantidade de dias maior que o valor máximo de
        /// dias que se pode vender férias (definido nas configurações).
        /// </summary>
        [TestMethod]
        public void ModalidadeFeriasTest_TestarCadastrarQuantidadeDiasMaiorValorMaximoDiasPodeVenderFerias()
        {
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);
            ModalidadeFerias modalidade = ModalidadeFeriasFactory.CriarModalidadeFerias(
                SessionTest, 11, true, CsSituacao.Inativo, false);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(modalidade,
                "ModalidadeFerias_RnVerificarDiasMaxVendaFerias", DefaultContexts.Save));
        }
    }
}