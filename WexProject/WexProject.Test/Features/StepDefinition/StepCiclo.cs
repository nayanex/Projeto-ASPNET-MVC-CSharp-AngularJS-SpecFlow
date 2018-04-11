using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Execucao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Escopo;

using System.Text.RegularExpressions;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using WexProject.Test.Features.Steps;
using WexProject.Test.Features.StepDefinition;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step do Ciclo
    /// </summary>
    [Binding]
    public class StepCiclo : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionário com os ciclos usados no Step
        /// TxMeta => Objeto do ciclo
        /// </summary>
        public static Dictionary<string, CicloDesenv> ciclosDic { get; set; }

        /// <summary>
        /// Dicionário com os estórias usados no Step
        /// TxTitulo => Objeto do estória
        /// </summary>
        public static Dictionary<string, CicloDesenvEstoria> estoriasDic { get; set; }

        /// <summary>
        /// Estórias selecionadas na lista
        /// </summary>
        public static List<CicloDesenvEstoria> estoriasSelecionadas { get; set; }

        /// <summary>
        /// INdices de estórias selecionadas na lista
        /// </summary>
        public static List<int> indicesSelecionadas { get; set; }

        /// <summary>
        /// Mensagem de erro de Ciclo
        /// </summary>
        private string mensagem;
        
        #endregion

        #region BeforeScenario

        // Reinicia os valores das listas
        [BeforeScenario]
        public void ReiniciarValores()
        {
            ciclosDic = new Dictionary<string, CicloDesenv>();
            estoriasDic = new Dictionary<string, CicloDesenvEstoria>();
            estoriasSelecionadas = new List<CicloDesenvEstoria>();
            mensagem = string.Empty;
        }

        #endregion

        #region Given

        [Given(@"que o ciclo '(.*)' do projeto '(.*)' esteja com situacao '(.*)' com as estorias:")]
        public void GivenQueOCiclo1DoProjetoProjeto01EstejaComSituacaoEmAndamentoComAsEstorias(string numCiclo, string projeto, string situacaoCiclo, Table table)
        {
            CicloDesenv ciclo = StepProjeto.ProjetosDic[projeto].Ciclos[(int.Parse(numCiclo)) - 1];
            ciclo.CsSituacaoCiclo = SituacaoCicloByText(situacaoCiclo);
            ciclo.Save();

            for (int position = 0; position < table.RowCount; position++)
            {
                string estoria = table.Rows[position][table.Header.ToList()[0]];
                string situacaoEstoria = table.Rows[position][table.Header.ToList()[1]];
                Estoria est = StepEstoria.EstoriasDic[estoria];
                CicloDesenvEstoria estoriaCiclo = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, est, situacaoEstoria, true);
            }
            ciclo.Save();
        }


        [Given(@"ciclo '([\w\s]+)' na situação '([\w\s]+)' com as estórias: (('[\w\sçãáéíóú]+'\s-\ssituação\s'[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void DadoCicloCiclo01NaSituacaoEmAndamentoComAsEstoriasEstoria01_SituacaoProntoEstoria02_SituacaoProntoEstoria03_SituacaoPronto(string ciclo, string situacao, string estorias, string naousado)
        {
            // Lista de Estórias
            List<string> listaEstorias = new List<string>();
            
            foreach(string estoria in estorias.Split(','))
            {
                string value01, value02;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 2)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valores encontrados
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples
                value02 = collection[1].Value.Substring(1, collection[1].Length - 2); // retiradas das aspas simples

                listaEstorias.Add(string.Format("{0};{1}", value01, value02));
            }

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "projeto 01", true);
            CriarCicloEstoriasCiclo(projeto, ciclo, listaEstorias, SessionTest);

            ciclosDic[ciclo].CsSituacaoCiclo = SituacaoCicloByText(situacao);

            projeto.Reload();
        }

        [Given(@"todas as estórias do ciclo '([\w\s]+)' estiverem na lista de '([\w\s]+)'$")]
        public void DadoTodasAsEstoriasDoCicloCiclo01EstiveremNaListaDeProximoCiclo(string ciclo, string lista)
        {
            // Reiniciando valores
            ciclosDic[ciclo]._ListaProximoCiclo = new List<CicloDesenvEstoria>();
            ciclosDic[ciclo]._ListaPrioridades = new List<CicloDesenvEstoria>();

            ciclosDic[ciclo].DesenvEstorias.Filter = CriteriaOperator.Parse("CsSituacao = ? OR CsSituacao = ? OR CsSituacao = ?", CsSituacaoEstoriaCicloDomain.NaoIniciado,
                CsSituacaoEstoriaCicloDomain.EmDesenv, CsSituacaoEstoriaCicloDomain.Replanejado);

            if (lista == "Próximo Ciclo")
            {
                foreach (CicloDesenvEstoria item in ciclosDic[ciclo].DesenvEstorias)
                {
                    ciclosDic[ciclo]._ListaProximoCiclo.Add(item);
                }
            }
            else
            {
                foreach (CicloDesenvEstoria item in ciclosDic[ciclo].DesenvEstorias)
                {
                    ciclosDic[ciclo]._ListaPrioridades.Add(item);
                }
            }

            ciclosDic[ciclo].DesenvEstorias.Filter = null;
        }

        [Given(@"um projeto '([\w\s]+)' com o\(s\) ciclo\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void DadoUmProjetoProjeto01ComOSCicloSCiclo01Ciclo02Ciclo03(string projeto, string ciclos, string naousado)
        {
            Projeto projetoObj = StepProjeto.CriarProjeto(10, projeto, true);
            ushort position = 1;

            foreach (string ciclo in ciclos.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(ciclo.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                // Criação do ciclo sem estórias
                CriarCicloEstoriasCiclo(projetoObj, value01, new List<string>(), SessionTest, position);
                position++;
            }
            projetoObj.Reload();
        }

        [Given(@"ciclo '([\w\s]+)' na situação '(.*)', data de início no dia '(.*)' e data de término no dia '(.*)'$")]
        public void DadoCicloNumaCertaSituacaoComDataDeInicioEmUmCertoDiaEDataDeTerminoEmUmCertoDia(string ciclo , string situacao,string data_inicio,string data_final)
        {
            CicloDesenv Ciclo = ciclosDic[ciclo];

            CsSituacaoCicloDomain situacaoCS;

            if (situacao.Split('-').Length == 1)
            {
                situacaoCS = SituacaoCicloByText(situacao);
            }
            else
            {
                situacaoCS = CsSituacaoCicloDomain.Cancelado;
                Ciclo.MotivoCancelamento = new MotivoCancelamento(SessionTest) { TxDescricao = situacao.Split('-')[1].Trim() };
            }

            Ciclo.CsSituacaoCiclo = situacaoCS;
            Ciclo.DtInicio = DateTime.Parse(data_inicio);
            Ciclo.DtTermino = DateTime.Parse(data_final);
            Ciclo.Save();
        }

        [Given(@"data atual for '(.*)'$")]
        public static void DadoDataAtual(string data_atual)
        {
            DateUtil.CurrentDateTime = DateTime.Parse(data_atual);
        }

        #endregion

        #region When

        [When(@"salvar o ciclo '(.*)' do projeto '(.*)'")]
        public void QuandoConcluirOCiclo2(string ciclo, string projeto)
        {
            foreach(CicloDesenv item in StepProjeto.ProjetosDic[projeto].Ciclos ) {
                if (item.NbCiclo.ToString().Equals(ciclo))
                {
                    item.CsSituacaoCiclo = CsSituacaoCicloDomain.EmAndamento;
                    item.Save();
                    break;
                }
            }
            
        }


        [When(@"remover a estoria '(.*)' do ciclo '(.*)' do projeto '(.*)'")]
        public void QuandoRemoverAEstoriaEstoria01DoCiclo2(string estoria, string ciclo, string projeto)
        {
            foreach (CicloDesenv item in StepProjeto.ProjetosDic[projeto].Ciclos)
            {
                if (item.NbCiclo.ToString().Equals(ciclo))
                {
                    foreach (var es in item.DesenvEstorias) 
                    {
                        if ((es as CicloDesenvEstoria).Estoria.TxTitulo.Contains(estoria))
                        {
                            (es as CicloDesenvEstoria).Delete();
                            break;
                        }
                    }
                    item.Save();
                    break;
                }
            }
        }



        [When(@"validar o cancelamento do ciclo '([\w\s]+)' sem passar o motivo")]
        public void QuandoValidarOCancelamentoDoCicloCiclo01SemPassarOMotivo(string ciclo)
        {
            mensagem = CicloDesenv.RnValidarMotivoCancelamento(null);
        }

        [When(@"indicar, no cancelamento do ciclo '(.*)', passar o motivo '(.*)' e a data de início do próximo ciclo com '(.*)")]
        public void QuandoIndicarNoCancelamentoDoCicloCiclo01PassarOMotivoMotivo01EADataDeInicioDoProximoCicloCom05032012(string ciclo, string motivo, string dataInicio)
        {
            CicloDesenv Ciclo = ciclosDic[ciclo];

            MotivoCancelamento Motivo = StepMotivoCancelamento.motivoCancelamentoDic[motivo];

            Ciclo.RnCancelarCiclo(Motivo, Ciclo._DataProximoCiclo);
        }

        [When(@"solicitar descer a posição das estórias selecionadas do ciclo '([\w\s]+)'$")]
        public void QuandoSolicitarDescerAPosicaoDasEstoriasSelecionadasDoCicloCiclo01(string ciclo)
        {
            ciclosDic[ciclo].RnTrocarPosicoesListaPendentes(estoriasSelecionadas, 0, false);
        }

        [When(@"definir a situação do ciclo '([\w\s]+)' como '([\w\s]+)'$")]
        public void QuandoDefinirASituacaoDoCicloCiclo01ComoConcluido(string ciclo, string situacao)
        {
            ciclosDic[ciclo].CsSituacaoCiclo = SituacaoCicloByText(situacao);
            ciclosDic[ciclo].IsExibirJanelaDestinoItensPendentes();
        }

        [When(@"a\(s\) estória\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+) estiver\(em\) na lista de '([\w\s]+)' do ciclo '([\w\s]+)'$")]
        public void DadoASEstoriaSEstoria02Estoria03EstiverEmNaListaDeProximoCicloDoCicloCiclo01(string estorias, string naousado, string lista, string ciclo)
        {
            foreach (string estoria in estorias.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                if (lista == "Próximo Ciclo")
                {
                    if (!ciclosDic[ciclo]._ListaProximoCiclo.Contains(estoriasDic[value01]))
                    {
                        ciclosDic[ciclo]._ListaProximoCiclo.Add(estoriasDic[value01]);
                    }

                    if (ciclosDic[ciclo]._ListaPrioridades.Contains(estoriasDic[value01]))
                    {
                        ciclosDic[ciclo]._ListaPrioridades.Remove(estoriasDic[value01]);
            }
        }
                else
                {
                    if (!ciclosDic[ciclo]._ListaPrioridades.Contains(estoriasDic[value01]))
                    {
                        ciclosDic[ciclo]._ListaPrioridades.Add(estoriasDic[value01]);
                    }

                    if (ciclosDic[ciclo]._ListaProximoCiclo.Contains(estoriasDic[value01]))
        {
                        ciclosDic[ciclo]._ListaProximoCiclo.Remove(estoriasDic[value01]);
                    }
                }
            }
        }

        [When(@"selecionar as estórias do ciclo '([\w\s]+)': (('[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void QuandoSelecionarAsEstoriasDoCicloCiclo01Estoria02Estoria03(string ciclo, string estorias, string naousado)
        {
            // Lista de Estórias
            estoriasSelecionadas = new List<CicloDesenvEstoria>();
            indicesSelecionadas = new List<int>();
            ciclosDic[ciclo].IsExibirJanelaDestinoItensPendentes();

            foreach (string estoria in estorias.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                // Seleção de Estória
                estoriasSelecionadas.Add(estoriasDic[value01]);
                indicesSelecionadas.Add(ciclosDic[ciclo]._ListaProximoCiclo.IndexOf(estoriasDic[value01]));
            }
        }

        [When(@"solicitar subir a posição das estórias selecionadas do ciclo '([\w\s]+)'$")]
        public void QuandoSolicitarSubirAPosicaoDasEstoriasSelecionadasDoCicloCiclo01(string ciclo)
        {
            ciclosDic[ciclo].RnTrocarPosicoesListaPendentes(estoriasSelecionadas, 0, true);
        }

        [When(@"selecionar as estórias da lista de '([\w\s]+)' do ciclo '([\w\s]+)': (('[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void QuandoSelecionarAsEstoriasDaListaDeProximoCicloDoCicloCiclo01Estoria01Estoria02(string lista, string ciclo, string estorias, string naousado)
            {
            // Lista de Estórias
            estoriasSelecionadas = new List<CicloDesenvEstoria>();

            foreach (string estoria in estorias.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
            }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                // Seleção de Estória
                estoriasSelecionadas.Add(estoriasDic[value01]);
        }
        }

        [When(@"solicitar enviar estórias da lista de '([\w\s]+)' para a lista de '([\w\s]+)' do ciclo '([\w\s]+)'$")]
        public void QuandoSolicitarEnviarEstoriasDaListaDeProximoCicloParaAListaDePrioridades(string listaOrigem, string listaDestino, string ciclo)
        {
            if (listaDestino == "Próximo Ciclo")
            {
                ciclosDic[ciclo].RnEnviarEstoriasSelecionadasDePrioridadesParaProximoCiclo(estoriasSelecionadas);
        }
            else
            {
                ciclosDic[ciclo].RnEnviarEstoriasSelecionadasDeProximoCicloParaPrioridades(estoriasSelecionadas);
            }
        }

        [When(@"solicitar enviar todas as estórias da lista de '([\w\s]+)' para a lista de '([\w\s]+)' do ciclo '([\w\s]+)'$")]
        public void QuandoSolicitarEnviarTodasAsEstoriasDaListaDeProximoCicloParaAListaDePrioridades(string listaOrigem, string listaDestino, string ciclo)
        {
            if (listaOrigem == "Próximo Ciclo")
            {
                estoriasSelecionadas = new List<CicloDesenvEstoria>(ciclosDic[ciclo]._ListaProximoCiclo);
                ciclosDic[ciclo].RnEnviarEstoriasSelecionadasDeProximoCicloParaPrioridades(estoriasSelecionadas);
        }
            else
            {
                estoriasSelecionadas = new List<CicloDesenvEstoria>(ciclosDic[ciclo]._ListaPrioridades);
                ciclosDic[ciclo].RnEnviarEstoriasSelecionadasDePrioridadesParaProximoCiclo(estoriasSelecionadas);
            }
        }

        [When(@"salvar o destino das estórias pentendes do ciclo '([\w\s]+)'$")]
        public void QuandoSalvarODestinoDasEstoriasPentendesDoCiclo(string ciclo)
        {
            ciclosDic[ciclo].RnSalvarDestinoEstoriasPendentes();
        }

        [When(@"validar o cancelamento do ciclo '([\w\s]+)' passando a data '(.*)'")]
        public void QuandoSalvarODestinoDasEstoriasPentendesDoCiclo(string ciclo, string data)
        {
            mensagem = ciclosDic[ciclo].RnDataProximoCiclo(DateTime.Parse(data));
        }

        /// <summary>
        /// Criar o Ciclo e as Estórias do Ciclo
        /// </summary>
        /// <param name="ciclo">Valor texto do Ciclo</param>
        /// <param name="estorias">Lista valores texto de Estórias do Ciclo</param>
        public static void CriarCicloEstoriasCiclo(Projeto projeto, string ciclo, List<string> estorias, Session session, ushort position = 1)
        {
            // Inserindo no dicionário
            if (!ciclosDic.ContainsKey(ciclo))
            {
                CicloDesenv cicloObj = CicloFactory.Criar(session, projeto, ciclo);
                cicloObj.NbCiclo = position;
                cicloObj.Save();

                ciclosDic.Add(ciclo, cicloObj);
        }

            foreach (string estoria in estorias)
        {
                string[] dados = estoria.Split(';');

                if (dados.Count() != 2)
                {
                    new Exception("O título e a situação da Estória devem vir separados por ';'");
        }

                string titulo = dados[0]; // Título da Estória
                string situacao = dados[1]; // Situação da Estória

                // Se a chave já existir, continua o método
                if (estoriasDic.ContainsKey(titulo))
        {
                    continue;
                }

                Modulo modulo = ModuloFactory.Criar(session, projeto, string.Format("modulo - {0}", ciclo), true);
                Beneficiado beneficiado = BeneficiadoFactory.Criar(session, string.Format("beneficiado - {0}", ciclo), true);
                Estoria estoriaObj = EstoriaFactory.Criar(session, modulo, titulo, "gostaria de",
                                        "então poderei", beneficiado, "observações", "referências", "dúvidas", true);

                estoriaObj.TxTitulo = titulo;
                estoriaObj.Save();

                // Estória no Ciclo
                CicloDesenvEstoria estoriaCiclo = CicloDesenvEstoriaFactory.Criar(session, ciclosDic[ciclo], estoriaObj, true);
                estoriaCiclo.CsSituacao = SituacaoEstoriaCicloByText(situacao);
                estoriaCiclo.Save();

                // Inserindo no dicionário
                estoriasDic.Add(titulo, estoriaCiclo);
            }

            ciclosDic[ciclo].IsExibirJanelaDestinoItensPendentes();
        }

        [When(@"mudar a situacao do ciclo '(.*)' para '(.*)'")]
        public void QuandoMudarASituacaoDoCicloPara(string ciclo, string situacao, Table table)
        {
            ciclosDic[ciclo].CsSituacaoCiclo = SituacaoCicloByText(situacao);

            for (int position = 0; position < table.RowCount; position++)
            {
                string estoriaRow = table.Rows[position][0];
                string situacaoRow = table.Rows[position][1];

                CicloDesenvEstoria estSelecionada = null;
                foreach (CicloDesenvEstoria est in ciclosDic[ciclo].DesenvEstorias ) {
                    if (est.Estoria.TxTitulo.Contains(estoriaRow)) {
                        estSelecionada = est;
                    }
                }

                estSelecionada.CsSituacao = SituacaoEstoriaCicloByText(situacaoRow);
            }

            ciclosDic[ciclo].Save();
        }


        #endregion

        #region Then

        [Then(@"a\(s\) estória\(s\) '([\w\s]+)' do ciclo '([\w\s]+)' deverá\(ão\) permenecer na mesma posição, '([\w\s]+)'$")]
        public void EntaoASEstoriaSEstoria01DoCicloCiclo01DeveraAoPermenecerNaMesmaPosicao(string estoria, string ciclo, string decisao)
        {
            if (decisao == "descendo")
                Assert.AreEqual(estoria, ciclosDic[ciclo]._ListaProximoCiclo[ciclosDic[ciclo]._ListaProximoCiclo.Count - 1].Estoria.TxTitulo, "A primeira estória não deveria ser movida.");
            else
                Assert.AreEqual(estoria, ciclosDic[ciclo]._ListaProximoCiclo[0].Estoria.TxTitulo, "A primeira estória não deveria ser movida.");
        }

        [Then(@"a\(s\) estória\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+) do ciclo '([\w\s]+)' deverá\(ão\) subir uma posição")]
        public void EntaoASEstoriaSEstoria02Estoria03DoCicloCiclo01DeveraAoSubirUmaPosicao(string estorias, string naousado, string ciclo)
        {
            int indice = 0;
            foreach (string estoria in estorias.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                Assert.AreEqual(value01, ciclosDic[ciclo]._ListaProximoCiclo[indicesSelecionadas[indice] + 1].Estoria.TxTitulo, "Está no indice correto");

                indice++;
            }
        }

        [Then(@"a\(s\) estória\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+) do ciclo '([\w\s]+)' deverá\(ão\) descer uma posição")]
        public void EntaoASEstoriaSEstoria01Estoria02DoCicloCiclo01DeveraAoDescerUmaPosicao(string estorias, string naousado, string ciclo)
                {
            int indice = 0;
            foreach (string estoria in estorias.Split(','))
                    {
                string value01;
                indice++;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                    {
                    new Exception("Erro na expressão regular.");
                    }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                Assert.AreEqual(value01, ciclosDic[ciclo]._ListaProximoCiclo[indice].Estoria.TxTitulo, "Está no indice correto");
                }
        }

        [Then(@"todas as estórias selecionadas do ciclo '([\w\s]+)' deverão descer uma posição")]
        public void EntaoTodasAsEstoriasSelecionadasDoCicloCiclo01DeveraoDescerUmaPosicao(string ciclo)
                {
            int indice = estoriasSelecionadas.Count;
            foreach (CicloDesenvEstoria estoria in estoriasSelecionadas)
                    {
                Assert.AreEqual(estoria.Estoria.TxTitulo, ciclosDic[ciclo]._ListaProximoCiclo[indice].Estoria.TxTitulo, "Descida certa.");
                indice--;
                    }
        }

        [Then(@"exibir mensagem para o ciclo '([\w\s]+)' '([\w\s]+)' para o cancelamento sem motivo")]
        public void EntaoExibirMensagemParaOCicloCiclo01ENecessarioInformarUmMotivoDeCancelamento_ParaOCancelamentoSemMotivo(string ciclo, string mensagem)
                    {
            Assert.AreEqual(mensagem, this.mensagem);
                    }

        [Then(@"não deverá exibir a janela de destino das estórias pendentes do ciclo '([\w\s]+)'$")]
        public void EntaoNaoDeveraExibirAJanelaDeDestinoDasEstoriasPendentesDoCicloCiclo01(string ciclo)
        {
            Assert.IsFalse(ciclosDic[ciclo].IsExibirJanelaDestinoItensPendentes(), "Não deveria exibir a janela para o ciclo atual");
        }

        [Then(@"as estórias pendentes do ciclo '([\w\s]+)' deverão ser sugeridas na lista de 'Próximo Ciclo'")]
        public void EntaoAsEstoriasPendentesDoCicloCiclo01DeveraoSerSugeridasNaListaDeProximoCiclo(string ciclo)
        {
            Assert.AreEqual(2, ciclosDic[ciclo]._ListaProximoCiclo.Count, 
                "Apenas duas estórias ficam prontas para entrar no próximo cilco.");
        }

        [Then(@"deverá exibir a janela de destino das estórias pendentes do ciclo '([\w\s]+)'$")]
        public void EntaoDeveraExibirAJanelaDeDestinoDasEstoriasPendentesDoCicloCiclo01(string ciclo)
        {
            Assert.IsTrue(ciclosDic[ciclo].IsExibirJanelaDestinoItensPendentes(), "Deveria exibir a janela para o ciclo atual");
        }

        [Then(@"todas as estórias selecionadas deverão sair da lista de '([\w\s]+)' e irão para a lista de '([\w\s]+)' do ciclo '([\w\s]+)'$")]
        public void EntaoTodasAsEstoriasSelecionadasDeveraoSairDaListaDeProximoCicloEIraoParaAListaDePrioridades(string listaOrigem, string listaDestino, string ciclo)
        {
            if (listaDestino == "Próximo Ciclo")
            {
                foreach (CicloDesenvEstoria estoria in estoriasSelecionadas)
                {
                    Assert.IsTrue(ciclosDic[ciclo]._ListaProximoCiclo.Contains(estoria),
                        "A Estória deveria estar na Lista de Próximo Ciclo");

                    Assert.IsFalse(ciclosDic[ciclo]._ListaPrioridades.Contains(estoria),
                        "A Estória não deveria estar na Lista de Prioridades");
                }
            }
            else
            {
                foreach (CicloDesenvEstoria estoria in estoriasSelecionadas)
                {
                    Assert.IsTrue(ciclosDic[ciclo]._ListaPrioridades.Contains(estoria),
                        "A Estória deveria estar na Lista de Prioridades");

                    Assert.IsFalse(ciclosDic[ciclo]._ListaProximoCiclo.Contains(estoria),
                        "A Estória deveria estar na Lista de Próximo Ciclo");
                }
            }
        }

        [Then(@"a lista de '([\w\s]+)' do ciclo '([\w\s]+)' deve estar vazia")]
        public void EntaoAListaDePrioridadesDoCicloCiclo01DeveEstarVazia(string lista, string ciclo)
        {
            if (lista == "Próximo Ciclo")
            {
                Assert.AreEqual(0, ciclosDic[ciclo]._ListaProximoCiclo.Count,
                    "A lista deveria estar vazia");
            }
            else
            {
                Assert.AreEqual(0, ciclosDic[ciclo]._ListaPrioridades.Count,
                    "A lista deveria estar vazia");
            }
        }

        [Then(@"a\(s\) estória\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+) deve\(m\) ser movidas para o backlog")]
        public void EntaoASEstoriaSEstoria01DeveMSerMovidasParaOBacklog(string estorias, string naousado)
        {
            foreach (string estoria in estorias.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                Assert.AreNotEqual(0, estoriasDic[value01].Estoria.NbPrioridade,
                    "Deveria vir pelo menos um objeto de Estória");
            }
        }

        [Then(@"a\(s\) estória\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+) deve\(m\) estar como '([\w\s]+)' no ciclo '([\w\s]+)'$")]
        public void EntaoASEstoriaSEstoria02Estoria03DeveMSerCopiadasComoNaoIniciadoParaOCicloCiclo02(string estorias, string naousado, string situacao, string ciclo)
        {
            foreach (string estoria in estorias.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(estoria.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                ciclosDic[ciclo].DesenvEstorias.Filter = CriteriaOperator.Parse("Estoria.TxTitulo = ?", value01);

                Assert.AreNotEqual(0, ciclosDic[ciclo].DesenvEstorias.Count,
                    "Deveria vir pelo menos um objeto de Estória");

                Assert.AreEqual(SituacaoEstoriaCicloByText(situacao), (ciclosDic[ciclo].DesenvEstorias[0] as CicloDesenvEstoria).CsSituacao,
                    "A situação não veio conforme o esperado.");
            }

            ciclosDic[ciclo].DesenvEstorias.Filter = null;
        }

        [Then(@"todas as estórias selecionadas do ciclo '([\w\s]+)' deverão subir uma posição")]
        public void EntaoTodasAsEstoriasSelecionadasDoCicloCiclo01DeveraoSubirUmaPosicao(string ciclo)
        {
            int indice = 0;
            foreach (CicloDesenvEstoria estoria in estoriasSelecionadas)
            {
                Assert.AreEqual(estoria.Estoria, ciclosDic[ciclo]._ListaProximoCiclo[indice].Estoria, "Subida certa");
                indice++;
            }
        }

        [Then(@"a situação do ciclo '(.*)' deverá ser '(.*)'$")]
        public void EntaoASituacaoDoCicloCiclo01DeveraSerCancelado_Motivo01(string ciclo, string situacao)
        {
            Assert.AreEqual(situacao, ciclosDic[ciclo]._SituacaoCiclo, "Ciclo Cancelado com Sucesso");
        }

        [Then(@"o campo ""(.*)"" deve ser exibido no cancelamento do ciclo '(.*)'")]
        public void EntaoUmCertoCampoDoCicloDeveSerExibido(string campo_name,string ciclo)
        {
            Assert.IsTrue(ciclosDic[ciclo].RnMostrarInicioProximoCiclo(),"Porque cú é diferente de bunda");
        }
        

        [Then(@"o campo ""(.*)"" no cancelamento do ciclo '(.*)' deve estar com um valor '(.*)'")]
        public void EntaoOCampoQualquerNoCancelamentoDeUmCertoCicloDeveEstarComUmValorQualquer(string campo_name, string ciclo, string valor)
        {
            if (campo_name.Equals("\"data de início do próximo ciclo\""))
            {
                string[] partitioned_date = valor.Split('/');
                DateTime date_now = new DateTime(Int32.Parse(partitioned_date[2]),Int32.Parse( partitioned_date[1]),Int32.Parse( partitioned_date[0]));
                Assert.AreEqual(date_now, ciclosDic[ciclo]._DataProximoCiclo);    
            }
        }

		[Then(@"não deverá exibir, na janela de cancelamento do ciclo '([\w\s]+)', a área de destino de estórias pendentes")]
        public void EntaoNaoDeveraExibirNaJanelaDeCancelamentoDeCicloAAreaDeDestinoDeEstoriasPendentes(string ciclo)
        {
            Assert.IsFalse(ciclosDic[ciclo].IsExibirJanelaDestinoItensPendentes());
        }
		[Then(@"o campo ""(.*)"" no cancelamento do ciclo '(.*)' deve estar com o motivo '(.*)'")]
        public void EntaoAlgumCampoNoCancelamentoDoCicloDeveEstarComAlgumMotivo(string campo,string ciclo,string motivo)
        {
            if (campo.Equals("motivo"))
            {
                Assert.AreEqual(CsSituacaoCicloDomain.Cancelado, ciclosDic[ciclo].CsSituacaoCiclo);
                Assert.AreEqual(ProjetoUltimoFiltro.GetUltimoFiltroByProject(SessionTest, ciclosDic[ciclo].Projeto).MotivoCancelamentoCiclo.TxDescricao, motivo);
            }
        }
        [Then(@"exibir mensagem para o ciclo '(.*)' '(.*)' para o cancelamento com a data incorreta")]
        public void EntaoExibirMensagemParaOCicloCiclo01ADataDeInicioDoProximoCicloDeveEstarEntre06032012E03042012(string ciclo, string mensagem)
        {
            CicloDesenv Ciclo = ciclosDic[ciclo];

            Assert.AreEqual(mensagem, this.mensagem, "A mensagem veio errada");
        }

        [Then(@"o campo ""data de início do próximo ciclo"" não deve ser exibido no cancelamento do ciclo '(.*)'")]
        public void EntaoOCampoDataDeInicioDoProximoCicloNaoDeveSerExibidoNoCancelamentoDoCicloCiclo01(string ciclo)
        {
            CicloDesenv cicloObj = ciclosDic[ciclo];
            cicloObj.CsSituacaoCiclo = CsSituacaoCicloDomain.Cancelado;
            cicloObj.Save();

            Assert.IsFalse(StepCiclo.ciclosDic[ciclo].RnMostrarInicioProximoCiclo(), "Não deveria mostrar o 'inicio do próximo ciclo'");
        }

        [Then(@"o campo ""data de início"" do ciclo '(.*)' deve ser '(.*)'")]
        public void EntaoOCampoDataDeInicioDoCicloCiclo01DeveSer05032012(string ciclo, string dataInicio)
        {
            Assert.AreEqual(Convert.ToDateTime(dataInicio), StepCiclo.ciclosDic[ciclo].DtInicio, "A data não é modificada");
        }

        [Then(@"o campo ""data de término"" do ciclo '(.*)' deve ser '(.*)'")]
        public void EntaoOCampoDataDeTerminoDoCicloCiclo01DeveSer30032012(string ciclo, string dataTermino)
        {
            Assert.AreEqual(Convert.ToDateTime(dataTermino), StepCiclo.ciclosDic[ciclo].DtTermino, "A data não é modificada");
        }

        [Then(@"o ciclo '(.*)' deve ser cancelado")]
        public void EntaoOCicloCiclo01DeveSerCancelado(string ciclo)
        {
            Assert.AreEqual(CsSituacaoCicloDomain.Cancelado, ciclosDic[ciclo].CsSituacaoCiclo,
                "O Ciclo deveria ser cancelado");

            Assert.IsTrue(ciclosDic[ciclo].IsCancelado, "O Ciclo deveria ser cancelado");
        }

        [Then(@"exibir mensagem ""Não se pode cancelar um Ciclo que não esteja com a situação '(.*)'"" para o cancelamento do ciclo '(.*)'")]
        public void EntaoExibirMensagemNaoSePodeCancelarUmCicloQueNaoEstejaComASituacaoNaoIniciadoParaOCancelamentoDoCicloCiclo01(string aituacao, string ciclo)
        {
            Assert.IsFalse(StepCiclo.ciclosDic[ciclo].RnCancelamentoSituacaoNaoIniciado(), "Quando a situação estiver 'Não Planejado', o retorno da função é falso, então não é habilitado o botão");
        }

        [Then(@"o '(.*)' não deverá ser cancelado")]
        public void EntaoOCiclo03NaoDeveraSerCancelado(string ciclo)
        {
            Assert.IsFalse(StepCiclo.ciclosDic[ciclo].RnCancelamentoSituacaoNaoIniciado(), "Quando a situação estiver 'Não Planejado', o retorno da função é falso, então não é habilitado o botão");
        }

        #endregion

        #region Util

        /// <summary>
        /// Retorna a situação do Ciclo a partir do valor texto da mesma
        /// </summary>
        /// <param name="situacao">Valor texto da situação</param>
        public static CsSituacaoCicloDomain SituacaoCicloByText(string situacao)
        {
            CsSituacaoCicloDomain retorno = CsSituacaoCicloDomain.NaoPlanejado;

            switch (situacao)
            {
                case "Não Planejado":
                    retorno = CsSituacaoCicloDomain.NaoPlanejado;
                    break;

                case "Concluído":
                    retorno = CsSituacaoCicloDomain.Concluido;
                    break;

                case "Em andamento":
                    retorno = CsSituacaoCicloDomain.EmAndamento;
                    break;

                case "Planejado":
                    retorno = CsSituacaoCicloDomain.Planejado;
                    break;

                case "Em atraso":
                    retorno = CsSituacaoCicloDomain.EmAtraso;
                    break;

                case "Cancelado":
                    retorno = CsSituacaoCicloDomain.Cancelado;
                    break;

                default:
                    new Exception("Situação do Ciclo não encontrada.");
                    break;
            }

            return retorno;
        }

        /// <summary>
        /// Retorna a situação da Estória do Ciclo a partir do valor texto da mesma
        /// </summary>
        /// <param name="situacao">Valor texto da situação</param>
        public static CsSituacaoEstoriaCicloDomain SituacaoEstoriaCicloByText(string situacao)
        {
            CsSituacaoEstoriaCicloDomain retorno = CsSituacaoEstoriaCicloDomain.NaoIniciado;

            switch (situacao)
            {
                case "Não Iniciado":
                    retorno = CsSituacaoEstoriaCicloDomain.NaoIniciado;
                    break;

                case "Em Desenvolvimento":
                    retorno = CsSituacaoEstoriaCicloDomain.EmDesenv;
                    break;
                case "EmDesenv":
                    retorno = CsSituacaoEstoriaCicloDomain.EmDesenv;
                    break;
                case "Pronto":
                    retorno = CsSituacaoEstoriaCicloDomain.Pronto;
                    break;

                case "Replanejado":
                    retorno = CsSituacaoEstoriaCicloDomain.Replanejado;
                    break;

                default:
                    new Exception("Situação da Estória não encontrada.");
                    break;
            }

            return retorno;
        }

#endregion
    }
}