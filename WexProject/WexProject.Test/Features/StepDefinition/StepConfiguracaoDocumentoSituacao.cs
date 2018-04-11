using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step da Solicitacao de Orcamento
    /// </summary>
    [Binding]
    public class StepConfiguracaoDocumentoSituacao : BaseTest
    {
        #region Properties

        /// <summary>
        ///  Dicionário com os Colaboradores usados no Step
        ///  Usuario.FirstName => Objeto de Colaborador
        /// </summary>
        public static Dictionary<string, ConfiguracaoDocumentoSituacao> ConfiguracaoDocumentoSituacoesDic { get; set; }

        #endregion

        #region BeforeScenarios

        // Reinicia os valores das listas
        [BeforeScenario]
        public void ReiniciarValores()
        {
            ConfiguracaoDocumentoSituacoesDic = new Dictionary<string, ConfiguracaoDocumentoSituacao>();
        }

        #endregion

        #region Dados

        [Given(@"\(a\)as Configuração\(ões\) de Documento\(s\) de Situação\(ões\) com o\(s\) seguinte\(s\) valore\(s\):")]
        public void DadoAAsConfiguracaoOesDeDocumentoSDeSituacaoOesComOSSeguinteSValoreS(Table table)
        {
            int cont = 0;
            int contRow = 0;

            ConfiguracaoDocumento config = ConfiguracaoDocumento.GetConfiguracaoPorTipo(SessionTest, CsTipoDocumento.SolicitacaoOrcamento);

            if (config == null)
            {
                ConfiguracaoDocumentoFactory.CriarConfiguracaoDocumento(SessionTest, CsTipoDocumento.SolicitacaoOrcamento, true);
            }

            foreach (var row in table.Rows)
            {
                ConfiguracaoDocumentoSituacao situacao = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(SessionTest, "Descricao", "Red", CsColorDomain.Custom, false);

                foreach (var item in table.Rows[contRow].Values)
                {
                    if (cont == 0)
                        situacao.TxDescricao = item;

                    if (cont == 1)
                        situacao.TxNomeCor = item;

                    if (cont == 2)
                        if (item == "true")
                            situacao.IsSituacaoInicial = true;
                    
                    cont++;
                }

                ConfiguracaoDocumentoSituacoesDic[situacao.TxDescricao] = situacao;
                ConfiguracaoDocumentoSituacoesDic[situacao.TxDescricao].ConfiguracaoDocumento = config;
                ConfiguracaoDocumentoSituacoesDic[situacao.TxDescricao].Save();
                contRow++;
                cont = 0;
            }
        }

        [Given(@"a\(s\) Configuração\(ões\) de Documento\(s\) com os seguintes valor\(es\):")]
        public void DadoASConfiguracaoOesDeDocumentoSComOsSeguintesValorEs(Table table)
        {
            int cont = 0;
            int contRow = 0;
            foreach (var row in table.Rows)
            {
                ConfiguracaoDocumento configuracaoDocumento = ConfiguracaoDocumentoFactory.CriarConfiguracaoDocumento(SessionTest, CsTipoDocumento.SolicitacaoOrcamento, true);

                foreach (var item in table.Rows[contRow].Values)
                {
                    if (cont == 0)
                        if (item.Equals(CsTipoDocumento.SolicitacaoOrcamento.ToString()))
                            configuracaoDocumento.CsTipoDocumento = CsTipoDocumento.SolicitacaoOrcamento;
                        else if (item.Equals(CsTipoDocumento.PropostaTecnica.ToString()))
                            configuracaoDocumento.CsTipoDocumento = CsTipoDocumento.PropostaTecnica;
                        else
                            configuracaoDocumento.CsTipoDocumento = CsTipoDocumento.PropostaFinanceira;

                    if (cont == 1)
                    {
                        string[] situacoes = item.Split(',');
                        foreach (string situacao in situacoes)
                        {
                            configuracaoDocumento.Situacoes.Add(ConfiguracaoDocumentoSituacoesDic[situacao]);
                        }
                    }
                    cont++;
                }
                contRow++;
                cont = 0;
            }
        }

        [Given(@"as seguintes situacoes de configuracao de documento:")]
        public void GivenAsSeguintesSituacoesDeConfiguracoesDeDocumento(Table table)
        {
            string documento = table.Header.ToList()[0];
            string descricao = table.Header.ToList()[1];            
            string cc = table.Header.ToList()[2];
            string cco = table.Header.ToList()[3];
            string padrao = table.Header.ToList()[4];

            ConfiguracaoDocumento config = ConfiguracaoDocumento.GetConfiguracaoPorTipo(SessionTest, CsTipoDocumento.SolicitacaoOrcamento);

            if (config == null)
            {
                ConfiguracaoDocumentoFactory.CriarConfiguracaoDocumento(SessionTest, CsTipoDocumento.SolicitacaoOrcamento, true);
            }

            foreach (TableRow row in table.Rows)
            {
                string documentoRow = row[documento];
                string descricaoRow = row[descricao];
                string ccRow = row[cc];
                string ccoRow = row[cco];
                string padraoRow = row[padrao];

                ConfiguracaoDocumentoSituacao situacao = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacaoComConfiguracao(
                    SessionTest, StepConfiguracaoDocumento.ConfiguracoesDocumentoDic[documentoRow], descricaoRow, false);

                situacao.IsSituacaoInicial = bool.Parse(padraoRow);

                // Emails CC
                foreach (string email in ccRow.Split(';'))
                {
                    ConfiguracaoDocumentoSituacaoEmailCc copia = new ConfiguracaoDocumentoSituacaoEmailCc(SessionTest)
                    {
                        ConfiguracaoDocumentoSituacao = situacao,
                        TxEmail = email
                    };

                    copia.Save();
                    situacao.ComCopia.Add(copia);
                }

                // Emails CCO
                foreach (string email in ccoRow.Split(';'))
                {
                    ConfiguracaoDocumentoSituacaoEmailCco copia = new ConfiguracaoDocumentoSituacaoEmailCco(SessionTest)
                    {
                        ConfiguracaoDocumentoSituacao = situacao,
                        TxEmail = email
                    };

                    copia.Save();
                    situacao.ComCopiaOculta.Add(copia);
                }

                situacao.ConfiguracaoDocumento = config;
                situacao.Save();
                ConfiguracaoDocumentoSituacoesDic.Add(descricaoRow, situacao);
            }
        }

        #endregion

        #region Quando
        [When(@"criar uma nova Configuracao de Documento de Situacao com o\(s\) seguinte\(s\) valores:")]
        public void QuandoCriarUmaNovaConfiguracaoDeDocumentoDeSituacaoComOSSeguinteSValores(Table table)
        {
            int cont = 0;
            int contRow = 0;
            ConfiguracaoDocumento config = ConfiguracaoDocumento.GetConfiguracaoPorTipo(SessionTest, CsTipoDocumento.SolicitacaoOrcamento);

            if (config == null)
            {
                ConfiguracaoDocumentoFactory.CriarConfiguracaoDocumento(SessionTest, CsTipoDocumento.SolicitacaoOrcamento, true);
            }

            foreach (var row in table.Rows)
            {
                ConfiguracaoDocumentoSituacao situacao =
                    ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(SessionTest, "Descricao", "Red", CsColorDomain.Custom, false);

                situacao.ConfiguracaoDocumento = config;

                foreach (var item in table.Rows[contRow].Values)
                {
                    if (cont == 0)
                        situacao.TxDescricao = item;

                    if (cont == 1)
                        situacao.TxNomeCor = item;

                    if (cont == 2)
                        if (item == "true")
                            situacao.IsSituacaoInicial = true;

                    cont++;
                }

                ConfiguracaoDocumentoSituacoesDic[situacao.TxDescricao] = situacao;
                ConfiguracaoDocumentoSituacoesDic[situacao.TxDescricao].Save();
                ConfiguracaoDocumentoSituacoesDic[situacao.TxDescricao].RnTrocaSituacaoInicial();
                ConfiguracaoDocumentoSituacoesDic[situacao.TxDescricao].Save();
                contRow++;
                cont = 0;
            }         
        }
        #endregion

        #region Então

        [Then(@"a Configuração de Documento de Situação definida como padrão é '(.*)'")]
        public void EntaoAConfiguracaoDeDocumentoDeSituacaoDefinidaComoPadraoEDescricao02(string descricaoPadrao)
        {
            Assert.AreEqual(descricaoPadrao, ConfiguracaoDocumentoSituacao.GetSituacaoInicial(
                ConfiguracaoDocumento.GetConfiguracaoPorTipo(SessionTest, CsTipoDocumento.SolicitacaoOrcamento)).TxDescricao,
                "A situação inicial ativa deve ser a última definida.");
        }

        [Then(@"o campo 'Situacao' deve estar preechido com '(.*)'")]
        public void EntaoOCampoSituacaoDeveEstarPreechidoComDescricao01(string descricaoPadrao)
        {
            Assert.AreEqual(descricaoPadrao, ConfiguracaoDocumentoSituacao.GetSituacaoInicial(
                ConfiguracaoDocumento.GetConfiguracaoPorTipo(SessionTest, CsTipoDocumento.SolicitacaoOrcamento)).TxDescricao,
                "A situação inicial ativa deve ser a última definida.");
        }

        #endregion
        
    }
}
