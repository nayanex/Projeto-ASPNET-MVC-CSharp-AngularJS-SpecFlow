using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.NovosNegocios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Rh;
using DevExpress.Persistent.BaseImpl;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.NovosNegocios;
using System.Reflection;
using DevExpress.Persistent.Validation;
using WexProject.Library.Libs.Enumerator;

namespace WexProject.Test.Features.StepDefinition
{
    
    /// <summary>
    /// Step de SEOT
    /// </summary>
    [Binding]
    public class StepSolicitacaoOrcamento : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionário com as SEOTs usadas no Step
        /// TxTitulo => Objeto de Solicitacao de Orcamento
        /// </summary>
        public static Dictionary<string, SolicitacaoOrcamento> seotsDic { get; set; }

        /// <summary>
        ///  Dicionário com os Colaboradores usados no Step
        ///  Usuario.FirstName => Objeto de Colaborador
        /// </summary>
        public static Dictionary<string, Colaborador> colaboradorDic { get; set; }

        /// <summary>
        ///  Dicionário com os Colaboradores usados no Step
        ///  Usuario.FirstName => Objeto de Colaborador
        /// </summary>
        public static Dictionary<string, ConfiguracaoDocumentoSituacao> situacaoDic { get; set; }

        #endregion

        #region BeforeScenarios

        // Reinicia os valores das listas
        [BeforeScenario]
        public void ReiniciarValores()
        {
            seotsDic = new Dictionary<string, SolicitacaoOrcamento>();
            colaboradorDic = new Dictionary<string, Colaborador>();
            situacaoDic = new Dictionary<string, ConfiguracaoDocumentoSituacao>();
        }

        #endregion

        #region Dados

        [Given(@"a\(s\) SEOT\(s\) com os seguintes valor\(es\):")]
        public void DadoASSEOTSComOsSeguintesValorEs(Table table)
        {
            int cont = 0;
            int contRow = 0;
            foreach (var row in table.Rows)
            {
                ConfiguracaoDocumentoSituacao situacao = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(SessionTest,"Descricao","Red", CsColorDomain.Custom ,false);

                Colaborador colab = ColaboradorFactory.CriarColaborador(SessionTest, "", DateTime.Now, "", "", "", "", "", null, false);

                SolicitacaoOrcamento seot = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colab, situacao, CsPrioridade.Media, "Titulo",
                    DateTime.Now, StepEmpresaInsituicao.EmpresaInstituicaoDict["Empresa01"], true);
                
                cont = 0;

                foreach (var item in table.Rows[contRow].Values)
                {
                    if (cont == 0)
                    {
                        seot.TxTitulo = item.ToString();
                    }
                    if (cont == 1)
                    {
                        colab.Usuario.FirstName = item.ToString();
                        seot.Responsavel = colab;
                        colaboradorDic[item.ToString()] = seot.Responsavel;
                    }
                    if (cont == 2)
                    {
                        situacao.TxDescricao = item.ToString();
                        seot.Situacao = situacao;
                        situacaoDic[item.ToString()] = situacao;
                    }
                    if (cont == 5)
                    {
                        seot.TipoSolicitacao = StepTipoSolicitacaoOrcamento.situacaoTS[item];
                    }


                    cont++;
                    seotsDic[seot.TxCodigo] = seot;
                    seotsDic[seot.TxCodigo].Save();
                }
                contRow++; 
            }
        }

        //Cenario 3
        [Given(@"a situação '(.*)' sem SEOT associada a ela")]
        public void DadoASituacaoSituacaoXSemSEOTAssociadaAEla(string situacaoX)
        {
            ConfiguracaoDocumentoSituacao situacao = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(SessionTest, "", "Red", CsColorDomain.Custom, true);
            situacaoDic[situacaoX] = situacao;
        }

        //Cenario 4
        [Given(@"o colaborador '(.*)' sem SEOT associada a ele")]
        public void DadoOColaboradorColaboradorXSemSEOTAssociadaAEle(string colaboradorX)
        {
            Colaborador colab = ColaboradorFactory.CriarColaborador(SessionTest, "", DateTime.Now, "", "", "", "", "", null, false);
            colaboradorDic[colaboradorX] = colab;
        }

        [Given(@"as seguintes solicitacoes de orcamento:"), When(@"as solicitações de orçamento a seguir forem criadas:")]
        public void GivenAsSeguintesSolicitacoesDeOrcamento(Table table)
        {
            string responsavel = table.Header.ToList()[0];
            string situacao = table.Header.ToList()[1];
            string tipoSolicitacao = table.Header.ToList()[2];
            string prioridade = table.Header.ToList()[3];
            string titulo = table.Header.ToList()[4];
            string prazo = table.Header.ToList()[5];
            string cliente = table.Header.ToList()[6];

            foreach (TableRow row in table.Rows)
            {
                string responsavelRow = row[responsavel];
                string situacaoRow = row[situacao];
                string tipoSolicitacaoRow = row[tipoSolicitacao];
                string prioridadeRow = row[prioridade];
                string tituloRow = row[titulo];
                string prazoRow = row[prazo];
                string clienteRow = row[cliente];

                SolicitacaoOrcamento seot = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest,
                    StepColaborador.ColaboradoresDic[responsavelRow],
                    StepConfiguracaoDocumentoSituacao.ConfiguracaoDocumentoSituacoesDic[situacaoRow],
                    CsPrioridade.Alta, tituloRow, DateTime.Parse(prazoRow),
                    StepEmpresaInsituicao.EmpresaInstituicaoDict[clienteRow],
                    StepTipoSolicitacaoOrcamento.situacaoTS[tipoSolicitacaoRow], false);

                // Set da prioridade
                PropertyInfo info = typeof(SolicitacaoOrcamento).GetProperty("CsPrioridade");
                info.SetValue(seot, EnumUtil.ValueEnum(typeof(CsPrioridade), prioridadeRow), null);

                seot.Save();
                seotsDic.Add(seot.TxCodigo, seot);
            }
        }

        #endregion

        #region Quando

        [When(@"verificar se as opções do filtro de responsavel são: '(.*)','(.*)','(.*)'")]
        public void QuandoVerificarAsOpcoesDoFiltroDeResponsavel(string colaborador1, string colaborador2, string colaborador3)
        {
            User usuario1 = colaboradorDic[String.Format("'{0}'", colaborador1)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario1);

            User usuario2 = colaboradorDic[String.Format("'{0}'", colaborador2)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario2);

            User usuario3 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario3);

            User usuario4 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario4);
        }

        [When(@"verificar as opções do filtro de situacao são: '(.*)', '(.*)', '(.*)'")]
        public void QuandoVerificarAsOpcoesDoFiltroDeSituacao(string situacao1, string situacao2, string situacao3)
        {
            ConfiguracaoDocumentoSituacao situacaoObj1 = situacaoDic[String.Format("'{0}'", situacao1)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj1);

            ConfiguracaoDocumentoSituacao situacaoObj2 = situacaoDic[String.Format("'{0}'", situacao2)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj2);

            ConfiguracaoDocumentoSituacao situacaoObj3 = situacaoDic[String.Format("'{0}'", situacao3)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj3);

            ConfiguracaoDocumentoSituacao situacaoObj4 = situacaoDic[String.Format("'{0}'", situacao3)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj4);
        }

        [When(@"verificar as opções do filtro de responsavel é:'(.*)'  e situacao são: '(.*)', '(.*)', '(.*)'")]
        public void QuandoVerificarAsOpcoesDoFiltroDeResponsavelEColaborador1ESituacaoSaoSituacao1Situacao2Situacao3(string colaborador1, string situacao1, string situacao2, string situacao3)
        {
            ConfiguracaoDocumentoSituacao situacaoObj1 = situacaoDic[String.Format("'{0}'", situacao1)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj1);

            ConfiguracaoDocumentoSituacao situacaoObj2 = situacaoDic[String.Format("'{0}'", situacao2)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj2);

            ConfiguracaoDocumentoSituacao situacaoObj3 = situacaoDic[String.Format("'{0}'", situacao3)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj3);

            ConfiguracaoDocumentoSituacao situacaoObj4 = situacaoDic[String.Format("'{0}'", situacao3)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj4);

            User usuario1 = colaboradorDic[String.Format("'{0}'", colaborador1)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario1);
        }

        [When(@"verificar se as opções do filtro de responsavel são '(.*)', '(.*)', '(.*)'  e situacao é '(.*)'")]
        public void QuandoVerificarSeAsOpcoesDoFiltroDeResponsavelSaoColaborador1Colaborador2Colaborador3ESituacaoESituacao1(string colaborador1, string colaborador2, string colaborador3, string situacao1)
        {
            User usuario1 = colaboradorDic[String.Format("'{0}'", colaborador1)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario1);

            User usuario2 = colaboradorDic[String.Format("'{0}'", colaborador2)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario2);

            User usuario3 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario3);

            User usuario4 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario4);

            ConfiguracaoDocumentoSituacao situacaoObj1 = situacaoDic[String.Format("'{0}'", situacao1)];
            SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj1);
        }

        [When(@"for criado uma nova Solicitação de Orçamento '(.*)' \(codigo '(.*)'\)")]
        public void QuandoForCriadoUmaNovaSolicitacaoDeOrcamentoSeot01(string seotCriado, string codigo)
        {
            SolicitacaoOrcamento seot = new SolicitacaoOrcamento(SessionTest);
            seot.TxTitulo = seotCriado;
            seot.TxCodigo = codigo;
            seot.Save();

            seotsDic[codigo] = seot;
        }

        [When(@"as solicitações de orçamento a seguir forem modificadas:")]
        public void QuandoOColaboradorColaborador01ModificarAsSeguintesSolicitacoesDeOrcamento(Table table)
        {
            string codigo = table.Header.ToList()[0];
            string responsavel = table.Header.ToList()[1];
            string situacao = table.Header.ToList()[2];
            string tipoSolicitacao = table.Header.ToList()[3];
            string prioridade = table.Header.ToList()[4];
            string titulo = table.Header.ToList()[5];
            string prazo = table.Header.ToList()[6];
            string cliente = table.Header.ToList()[7];
            string comentario = table.Header.ToList()[8];

            foreach (TableRow row in table.Rows)
            {
                string codigoRow = row[codigo];
                string responsavelRow = row[responsavel];
                string situacaoRow = row[situacao];
                string tipoSolicitacaoRow = row[tipoSolicitacao];
                string prioridadeRow = row[prioridade];
                string tituloRow = row[titulo];
                string prazoRow = row[prazo];
                string clienteRow = row[cliente];
                string comentarioRow = row[comentario];

                SolicitacaoOrcamento seot = SolicitacaoOrcamentoFactory.AlterarSolicitacaoOrcamento(SessionTest,
                    seotsDic[codigoRow], StepColaborador.ColaboradoresDic[responsavelRow],
                    StepConfiguracaoDocumentoSituacao.ConfiguracaoDocumentoSituacoesDic[situacaoRow],
                    CsPrioridade.Alta, tituloRow, DateTime.Parse(prazoRow),
                    StepEmpresaInsituicao.EmpresaInstituicaoDict[clienteRow],
                    StepTipoSolicitacaoOrcamento.situacaoTS[tipoSolicitacaoRow], false);

                // Set da prioridade
                PropertyInfo info = typeof(SolicitacaoOrcamento).GetProperty("CsPrioridade");
                info.SetValue(seot, EnumUtil.ValueEnum(typeof(CsPrioridade), prioridadeRow), null);

                seot.TxUltimoComentario = comentarioRow;
                seot.Save();
            }
        }

        #endregion

        #region Então

        //Cenario 1
        [Then(@"apenas o\(s\) colaborador\(es\) '(.*)', '(.*)', '(.*)' devem ser opções no filtro de responsavel")]
        public void EntaoApenasOSColaboradorEsColaborador1Colaborador2Colaborador3DevemSerOpcoesNoFiltroDeResponsavel(string colaborador1, string colaborador2, string colaborador3)
        {
            User usuario1 = colaboradorDic[String.Format("'{0}'", colaborador1)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario1));

            User usuario2 = colaboradorDic[String.Format("'{0}'", colaborador2)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario2));

            User usuario3 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario3));

            User usuario4 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario4));
        }

        //Cenario 2
        [Then(@"apenas as situacoes '(.*)', '(.*)', '(.*)' devem ser opções no filtro de situação")]
        public void EntaoApenasAsSituacoesSituacao1Situacao2Situacao3DevemSerOpcoesNoFiltroDeSituacao(string situacao1, string situacao2, string situacao3)
        {
            ConfiguracaoDocumentoSituacao situacaoObj1 = situacaoDic[String.Format("'{0}'", situacao1)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj1));

            ConfiguracaoDocumentoSituacao situacaoObj2 = situacaoDic[String.Format("'{0}'", situacao2)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj2));

            ConfiguracaoDocumentoSituacao situacaoObj3 = situacaoDic[String.Format("'{0}'", situacao3)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj3));

            ConfiguracaoDocumentoSituacao situacaoObj4 = situacaoDic[String.Format("'{0}'", situacao3)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj4));
        }

        [Then(@"apenas as situacoes '(.*)', '(.*)', '(.*)' devem ser opções no filtro de situação e apenas o colaborador '(.*)' deve ser opção no filtro de responsavel e a situacao '(.*)' não deve aparecer")]
        public void EntaoApenasAsSituacoesSituacao1Situacao2Situacao3DevemSerOpcoesNoFiltroDeSituacaoEApenasOColaboradorColaborador1DeveSerOpcaoNoFiltroDeResponsavel(string situacao1, string situacao2, string situacao3, string colaborador1, string situacaoX)
        {
            ConfiguracaoDocumentoSituacao situacaoObj1 = situacaoDic[String.Format("'{0}'", situacao1)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj1));

            ConfiguracaoDocumentoSituacao situacaoObj2 = situacaoDic[String.Format("'{0}'", situacao2)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj2));

            ConfiguracaoDocumentoSituacao situacaoObj3 = situacaoDic[String.Format("'{0}'", situacao3)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj3));

            ConfiguracaoDocumentoSituacao situacaoObj4 = situacaoDic[String.Format("'{0}'", situacao3)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj4));

            User usuario1 = colaboradorDic[String.Format("'{0}'", colaborador1)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario1));

            ConfiguracaoDocumentoSituacao situacaoObjX = situacaoDic[situacaoX];
            Assert.IsFalse(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObjX));
        }

        [Then(@"apenas a situacao '(.*)' deve ser opção no filtro de situação e os colaboradores '(.*)', '(.*)', '(.*)' devem ser opções no filtro de responsavel e o colaborador '(.*)' não deve aparecer como opção")]
        public void EntaoApenasASituacaoSituacao1DeveSerOpcaoNoFiltroDeSituacaoEOsColaboradoresColaborador1Colaborador2Colaborador3DevemSerOpcoesNoFiltroDeResponsavel(string situacao1, string colaborador1, string colaborador2, string colaborador3, string colaboradorX)
        {
            User usuario1 = colaboradorDic[String.Format("'{0}'", colaborador1)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario1));

            User usuario2 = colaboradorDic[String.Format("'{0}'", colaborador2)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario2));

            User usuario3 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario3));

            User usuario4 = colaboradorDic[String.Format("'{0}'", colaborador3)].Usuario;
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuario4));

            ConfiguracaoDocumentoSituacao situacaoObj1 = situacaoDic[String.Format("'{0}'", situacao1)];
            Assert.IsTrue(SolicitacaoOrcamento.RnSeotsPorSituacao(SessionTest, situacaoObj1));

            User usuarioX = colaboradorDic[colaboradorX].Usuario;
            Assert.IsFalse(SolicitacaoOrcamento.RnSeotsPorResponsavel(SessionTest, usuarioX));
        }

        [Then(@"o campo 'Cliente' deve estar preechido com '(.*)' para a seot '(.*)'")]
        public void EntaoOCampoClienteDeveEstarPreechidoComEmpresa01ParaASeotSeot02(string nomeEmpresa, string seotCriada)
        {
            Assert.AreEqual(seotsDic[seotCriada].Cliente.TxNome, nomeEmpresa, "Deve estar definido como a ultima empresa selecionada pelo usuário");
        }

        [Then(@"o campo 'TipoSolicitacao' deve estar preechido com '(.*)' para a seot '(.*)'")]
        public void EntaoOCampoTipoSolicitacaoDeveEstarPreechidoComTipo01ParaASeotSeot02(string lastTipo, string seotCriada)
        {
            Assert.AreEqual(seotsDic[seotCriada].TipoSolicitacao, StepTipoSolicitacaoOrcamento.situacaoTS[lastTipo], "Deve estar definido como o último tipo definido pelo usuário.");
        }

        [Then(@"o campo ""Prazo"" da '(.*)' a ser sugerido deve ser 9 dias úteis após a data atual, sendo '(.*)'")]
        public void EntaoOCampoPrazoDaSeot01ASerSugeridoDeveSer7DiasUteisAposADataAtualSendo29032012(string seot, string dataPrazo)
        {
            Assert.AreEqual(seotsDic[seot].DtPrazo, DateTime.Parse(dataPrazo), "O Prazo deve ser sete dias úteis após a data atual");
        }

        [Then(@"o historico da solicitacao de orcamento '(.*)' deve ser:")]
        public void EntaoOHistoricoDaSolicitacoesDeOrcamentoDeveSer(string codigoSEOT, Table table)
        {
            string dataHora = table.Header.ToList()[0];
            string responsavel = table.Header.ToList()[1];
            string situacao = table.Header.ToList()[2];
            string comentario = table.Header.ToList()[3];
            string atualizadoPor = table.Header.ToList()[4];

            Assert.AreEqual(table.Rows.Count, seotsDic[codigoSEOT].SolicitacaoOrcamentoHistoricos.Count,
                "A quantidade de históricos não está de acordo.");

            for (int position = 0; position < table.Rows.Count; position++)
            {
                TableRow row = table.Rows[position];
                SolicitacaoOrcamentoHistorico historico = seotsDic[codigoSEOT].SolicitacaoOrcamentoHistoricos[position];

                string dataHoraRow = row[dataHora];
                string responsavelRow = row[responsavel];
                string situacaoRow = row[situacao];
                string comentarioRow = row[comentario];
                string atualizadoPorRow = row[atualizadoPor];

                // Verificação da data e hora
                Assert.AreEqual(DateTime.Parse(dataHoraRow), historico.DataHora,
                    "A data e hora não estão de acordo.");

                // Verificação do Responsável
                Assert.AreEqual(StepColaborador.ColaboradoresDic[responsavelRow], historico.ResponsavelHistorico,
                    "O responsável não está de acordo.");

                // Verificação da Situação
                Assert.AreEqual(StepConfiguracaoDocumentoSituacao.ConfiguracaoDocumentoSituacoesDic[situacaoRow],
                    historico.Situacoes, "A situação não está de acordo.");

                // Verificação do comentário
                if (string.IsNullOrEmpty(comentarioRow))
                {
                    Assert.IsTrue(string.IsNullOrEmpty(historico.Comentario),
                        "O comentário não está de acordo.");
                }
                else
                {
                    Assert.AreEqual(comentarioRow, historico.Comentario,
                        "O comentário não está de acordo.");
                }

                // Verificação do campo "Atualizado Por"
                Assert.AreEqual(StepColaborador.ColaboradoresDic[atualizadoPorRow], historico.AtualizadoPor,
                    "O campo 'Atualizado Por' não está de acordo.");
            }
        }

        [Then(@"o último comentário de alteração das SEOTs devem ser os seguintes:")]
        public void EntaoOUltimoComentarioDeAlteracaoDasSEOTsDevemSerOsSeguintes(Table table)
        {
            string seot = table.Header.ToList()[0];
            string comentario = table.Header.ToList()[1];

            foreach (TableRow row in table.Rows)
            {
                string seotRow = row[seot];
                string comentarioRow = row[comentario];

                Assert.AreEqual(comentarioRow, seotsDic[seotRow].TxUltimoComentario,
                    "Os comentários não estão de acordo.");
            }
        }

        [Then(@"a solicitação de orçamento estará apta a salvar")]
        public void EntaoASolicitacaoDeOrcamentoEstaraAptaASalvar()
        {
            RuleSetValidationResult rsvr = (new RuleSet()).ValidateTarget(
                seotsDic.Last(), DefaultContexts.Save);

            foreach (RuleSetValidationResultItem item in rsvr.Results)
            {
                Assert.AreEqual(ValidationState.Valid, item.State);
            }
        }

        #endregion
    }
}
