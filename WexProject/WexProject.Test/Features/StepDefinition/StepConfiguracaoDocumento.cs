using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Geral;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Geral;
using System.Reflection;
using WexProject.Library.Libs.Enumerator;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de ConfiguracaoDocumento
    /// </summary>
    [Binding]
    public class StepConfiguracaoDocumento : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Configurações de Documento usados no Step
        /// </summary>
        public static Dictionary<string, ConfiguracaoDocumento> ConfiguracoesDocumentoDic { get; set; }

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            ConfiguracoesDocumentoDic = new Dictionary<string, ConfiguracaoDocumento>();
        }

        #endregion

        #region Dados

        [Given(@"as seguintes configuracoes de documento:")]
        public void DadoAsSeguintesConfiguracoesDeDocumento(Table table)
        {
            string documento = table.Header.ToList()[0];

            foreach (TableRow row in table.Rows)
            {
                string documentoRow = row[documento];
                ConfiguracaoDocumento configuracao = ConfiguracaoDocumentoFactory.CriarConfiguracaoDocumento(SessionTest,
                    CsTipoDocumento.SolicitacaoOrcamento, false);

                // Set do tipo de documento
                PropertyInfo info = typeof(ConfiguracaoDocumento).GetProperty("CsTipoDocumento");
                info.SetValue(configuracao, EnumUtil.ValueEnum(typeof(CsTipoDocumento), documentoRow), null);

                configuracao.Save();
                ConfiguracoesDocumentoDic.Add(documentoRow, configuracao);
            }
        }

        #endregion
    }
}
