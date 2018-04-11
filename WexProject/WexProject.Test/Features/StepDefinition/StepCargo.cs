using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Escopo;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Cargo
    /// </summary>
    [Binding]
    class StepCargo : BaseTest
    {
        #region Properties
        
        public static Dictionary<string, Cargo> CargoDic { get; set; }

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas (antes dos cenários)
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            CargoDic = new Dictionary<string, Cargo>();
        }

        #endregion

        #region Util

        public static void CreateCargo(string cargo)
        {
            Cargo c = CargoFactory.Criar(SessionTest, cargo, true);
            CargoDic.Add(cargo, c);
        }

        #endregion
    }
}
