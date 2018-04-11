using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Rh;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.Models.Geral;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes da classe FeriasPlanejamento
    /// </summary>
    [TestClass]
    public class FeriasPlanejamentoTest : BaseTest
    {
        /// <summary>
        /// Testar definir um controle de férias com realizado
        /// </summary>
        [TestMethod]
        public void FeriasPlanejamentoTest_001_TestarDefinirControleFeriasComoRealizado()
        {
            #region Pré-condições

            // Configurações
            ConfiguracaoFactory.CriarConfiguracao(SessionTest, 10, 30, 12);

            // Cargo: Programador Jr 2
            Cargo cargoJr2 = CargoFactory.Criar(SessionTest, "Programador Jr 2", true);

            // Modalidade: 10 dias
            ModalidadeFerias modalidade10 = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, 10, false);

            // Usuário: João Souza
            Colaborador colaboradorJoaoSouza = ColaboradorFactory.CriarColaborador(SessionTest, "123",
                new DateTime(2011, 1, 1), "joao.souza@fpf.br", "João", string.Empty, "Souza", "joao.souza", cargoJr2);

            // Planejamento de Férias
            FeriasPlanejamento planejamento = FeriasPlanejamentoFactory.CriarPlanejamentoFerias(SessionTest,
                colaboradorJoaoSouza.PeriodosAquisitivos[0], modalidade10, new DateTime(2012, 1, 1));

            // Tipo de Afastamento
            TipoAfastamento tipoAfastamento = TipoAfastamentoFactory.CriarTipoAfastamento(SessionTest, "Férias", true, true);

            // Data atual
            DateUtil.CurrentDateTime = new DateTime(2012, 2, 1);

            #endregion

            #region Passos

            // Definindo Planejamento como Realizado
            planejamento.Reload();
            planejamento.Realizadas = true;
            planejamento.Save();

            // Verificação de criação de Afastamento
            Assert.AreEqual(1, colaboradorJoaoSouza.Afastamentos.Count,
                "Deveria ter sido criado um Afastamento.");

            // Verificação do Tipo do Afastamento
            Assert.AreEqual(tipoAfastamento, colaboradorJoaoSouza.Afastamentos[0].TipoAfastamento,
                "O Afastamento deveria ser o mesmo que existe para Férias Realizadas.");

            #endregion
        }
    }
}