using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de ModalidadeFerias
    /// </summary>
    [Binding]
    class StepModalidadeFerias : BaseTest
    {
        #region Properties
        /// <summary>
        /// Dicionario de Estorias usados no Step
        /// </summary>
        public static Dictionary<string, ModalidadeFerias> ModalidadeFeriasDic { get; set; }

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            ModalidadeFeriasDic = new Dictionary<string, ModalidadeFerias>();
        }

        #endregion

        #region Given
        [Given(@"existam as modalidades de ferias:")]
        public void DadoExistamAsModalidadesDeFerias(Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string dias = table.Rows[position][table.Header.ToList()[0]];
                string pVenda = table.Rows[position][table.Header.ToList()[1]];
                string pAtivo = table.Rows[position][table.Header.ToList()[2]];
                
                bool venda = false;
                CsSituacao ativo = CsSituacao.Inativo;

                if (pVenda.ToUpper().Equals("SIM"))
                {
                    venda = true;
                }
                if (pAtivo.ToUpper().Equals("SIM"))
                {
                    ativo = CsSituacao.Ativo;
                }

                ModalidadeFerias m = ModalidadeFeriasFactory.CriarModalidadeFerias(SessionTest, uint.Parse(dias), venda, ativo);
                ModalidadeFeriasDic.Add(dias, m);
            }
        }

        #endregion
    }
}
