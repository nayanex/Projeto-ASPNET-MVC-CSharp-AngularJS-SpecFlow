using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.NovosNegocios;
using WexProject.Test.Fixtures.Factory;

namespace WexProject.Test.Features.StepDefinition
{
    
    /// <summary>
    /// Step da Tipo Solicitação de Orçamento
    /// </summary>
    [Binding]
    public class StepTipoSolicitacaoOrcamento : BaseTest
    {
        #region Proprieties
        /// <summary>
        ///  Dicionário com os Colaboradores usados no Step
        ///  Usuario.FirstName => Objeto de Colaborador
        /// </summary>
        public static Dictionary<string, TipoSolicitacao> situacaoTS { get; set; }
        #endregion

        #region BeforeScenarios
        // Reinicia os valores das listas
        [BeforeScenario]
        public void ReiniciarValores()
        {
            situacaoTS = new Dictionary<string, TipoSolicitacao>();
        }
        #endregion

        #region Dados

        [Given(@"o\(a\) Tipo\(s\) de Solicitação de Orçamento com os seguintes valores:")]
        public void DadoOATipoSDeSolicitacaoDeOrcamentoComOsSeguintesValores(Table table)
        {
            int cont = 0;
            int contRow = 0;
            foreach (var row in table.Rows)
            {
                TipoSolicitacao tipoSolicitacao = new TipoSolicitacao(SessionTest);

                foreach (var item in table.Rows[contRow].Values)
                {
                    if (cont == 0)
                        tipoSolicitacao.TxDescricao = item;

                    cont++;
                }
                situacaoTS[tipoSolicitacao.TxDescricao] = tipoSolicitacao;
                situacaoTS[tipoSolicitacao.TxDescricao].Save();
                contRow++;
                cont = 0;
            }
        }

        [Given(@"os seguintes tipos de solicitacao de orcamento:")]
        public void DadoOsSeguintesTiposDeSolicitacaoDeOrcamento(Table table)
        {
            string descricao = table.Header.ToList()[0];

            foreach (TableRow row in table.Rows)
            {
                string descricaoRow = row[descricao];

                situacaoTS.Add(descricaoRow,
                    TipoSolicitacaoFactory.CriarTipoSolicitacao(SessionTest, descricaoRow, true));
            }
        }

        #endregion

        #region Quando

        #endregion

        #region Então
        #endregion
        
    }
}
