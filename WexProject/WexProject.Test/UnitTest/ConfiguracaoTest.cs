using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Rh;
using WexProject.Test.Fixtures.Factory;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Xaf;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes da classe Configuracao
    /// </summary>
    [TestClass]
    public class ConfiguracaoTest : BaseTest
    {
        /// <summary>
        /// Teste de inserção de quantidade máxima de venda de férias com valor 0
        /// </summary>
        [TestMethod]
        public void ConfiguracaoTest_001_TestarInserirQuantidadeMaximaDeVendaDeFeriasComValorZero()
        {
            Configuracao configuracao = ConfiguracaoFactory.CriarConfiguracao(SessionTest, 0, 30, 12, false);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(configuracao,
                "Qtde máxima de venda", DefaultContexts.Save));
        }

        /// <summary>
        /// Teste de inserção de quantidade máxima para tirar férias com valor 0
        /// </summary>
        [TestMethod]
        public void ConfiguracaoTest_002_TestarInserirQuantidadeMaximaDeFeriasComValorZero()
        {
            Configuracao configuracao = ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 0, 12, false);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(configuracao,
                "Qtde máxima de férias", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar alterar a quantidade de dias de venda de férias na configuração.
        /// </summary>
        [TestMethod]
        public void ConfiguracaoTest_003_TestarAlterarQuantidadeDiasVendaFeriasConfiguracao()
        {
            Configuracao config = ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);
            ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 8, true, CsSituacao.Ativo);
            ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 6, true, CsSituacao.Inativo);

            config.NbQtdeMaxVenda = 7;

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(config,
                "Configuracao_RnVerificarDiasMaxVendaFerias", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar alterar a quantidade de máximo de dias de férias na configuração.
        /// </summary>
        [TestMethod]
        public void ConfiguracaoTest_004_TestarAlterarQuantidadeMaximoDiasFeriasConfiguracao()
        {
            Configuracao config = ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);
            ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 8, true, CsSituacao.Ativo);
            ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 6, false, CsSituacao.Ativo);

            config.NbQtdeMaxFerias = 7;

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(config,
                "Configuracao_RnVerificarDiasMaxFerias", DefaultContexts.Save));
        }
    }
}