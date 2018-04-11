using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Beneficiado
    /// </summary>
    [Binding]
    class StepBeneficiado : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Beneficiados usados no Step
        /// </summary>
        public static Dictionary<string, Beneficiado> BeneficiadosDic { get; set; }

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            BeneficiadosDic = new Dictionary<string, Beneficiado>();
        }

        #endregion

        #region Dados

        [Given(@"benefeciado '(.*)'")]
        public void DadoBenefeciadoAnalistaDeNegocios(string beneficiado)
        {
            addBeneficiado(beneficiado);
        }

        public static void  addBeneficiado(string beneficiado)
        {
            BeneficiadosDic.Add(beneficiado, BeneficiadoFactory.Criar(SessionTest, beneficiado, true));
        }

        #endregion
    }
}
