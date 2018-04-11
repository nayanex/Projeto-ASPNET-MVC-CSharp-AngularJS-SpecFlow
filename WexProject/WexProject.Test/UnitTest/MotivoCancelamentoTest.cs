using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Models.Execucao;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.Library.Libs.Xaf;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes da classe MotivoCancelamento
    /// </summary>
    [TestClass]
    public class MotivoCancelamentoTest : BaseTest
    {
        /// <summary>
        /// Testar se ocorre invalidação ao tentar salvar um nulo
        /// </summary>
        [TestMethod]
        public void MootivoCancelamentoTxDescricaoNulo()
        {
            MotivoCancelamento modalidade = MotivoCancelamentoFactory.CriarMotivoCancelamento(
                SessionTest, "", CsStatusMotivoCancelamento.Ativo, true);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(modalidade,
                "MotivoCancelamento_TxDescricao_Required", DefaultContexts.Save));

            MotivoCancelamento modalidade1 = MotivoCancelamentoFactory.CriarMotivoCancelamento(
                SessionTest, "modalidade1", CsStatusMotivoCancelamento.Ativo, true);

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(modalidade1,
                "MotivoCancelamento_TxDescricao_Required", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar se ocorre invalidação ao tentar salvar um já existente
        /// </summary>
        [TestMethod]
        public void MootivoCancelamentoTxDescricaoUnicos()
        {
            MotivoCancelamento modalidade = MotivoCancelamentoFactory.CriarMotivoCancelamento(
                SessionTest, "modalidade1", CsStatusMotivoCancelamento.Ativo, true);

            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(modalidade,
                "MotivoCancelamento_TxDescricao_Unique", DefaultContexts.Save));

            MotivoCancelamento modalidade1 = MotivoCancelamentoFactory.CriarMotivoCancelamento(
                SessionTest, "modalidade1", CsStatusMotivoCancelamento.Ativo, true);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(modalidade1,
                "MotivoCancelamento_TxDescricao_Unique", DefaultContexts.Save));
        }


    }
}