using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.Test.Fixtures.Factory;

using WexProject.BLL.Models.Geral;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Parte Interessada
    /// </summary>
    [Binding]
    class StepProjetoParteInteressada : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Partes Interessadas usados no Step
        /// </summary>
        public static Dictionary<string, ProjetoParteInteressada> PartesInteressadasDic { get; set; }

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            PartesInteressadasDic = new Dictionary<string, ProjetoParteInteressada>();
        }

        #endregion

        #region Dados

        [Given(@"parte interessada '(.*)'")]
        public void DadoParteInteressadaAlexandreAmorim(string parte)
        {
            PartesInteressadasDic.Add(parte,
                ProjetoParteInteressadaFactory.Criar(SessionTest,
                    ProjetoFactory.Criar(SessionTest, 0, string.Format("Projeto - {0}", parte), true),
                    true));
        }

        #endregion
    }
}