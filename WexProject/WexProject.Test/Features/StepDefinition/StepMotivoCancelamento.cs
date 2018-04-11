using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Execucao;
using WexProject.Test.Fixtures.Factory;

using WexProject.BLL.Models.Geral;
using WexProject.Test.Features.StepDefinition;
using System.Text.RegularExpressions;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.BLL.DAOs;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Test.Features.Steps
{
    /// <summary>
    /// Step do Motivo de Cancelamento de Ciclo
    /// </summary>
    [Binding]
    public class StepMotivoCancelamento : BaseTest
    {
        #region Objects
        
        /// <summary>
        /// Dicionário com os motivo cancelamento usados no Step
        /// TxMeta => Objeto do motivo cancelamento
        /// </summary>
        public static Dictionary<string, MotivoCancelamento> motivoCancelamentoDic;

        private Dictionary<string, MotivoCancelamento> motivosAtivos;

        #endregion

        #region BefoneScenario
        // Reinicia os valores das listas
        [BeforeScenario]
        public void ReiniciarValores()
        {
            motivoCancelamentoDic = new Dictionary<string, MotivoCancelamento>();
        }
        #endregion

        #region Scenarios

        [Given(@"um motivo de cancelamento '([\w\s]+)' usado no cancelamento de um ciclo '([\w\s]+)'$")]
        public void DadoUmMotivoDeCancelamentoMotivo01UsadoNoCancelamentoDeUmCicloCiclo01(string motivo, string ciclo)
        {
            // Lista de Estórias
            List<string> listaEstorias = new List<string>();

            listaEstorias.Add("estoria 01;Não Iniciado");

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "projeto 01", true);
            StepCiclo.CriarCicloEstoriasCiclo(projeto, ciclo, listaEstorias, SessionTest);

            StepCiclo.ciclosDic[ciclo].CsSituacaoCiclo = StepCiclo.SituacaoCicloByText("Em andamento");

            CriarMotivoCancelamento(motivo, CsStatusMotivoCancelamento.Ativo, SessionTest);

            StepCiclo.ciclosDic[ciclo].MotivoCancelamento = motivoCancelamentoDic[motivo];

            StepCiclo.ciclosDic[ciclo].Save();
        }

        [Given(@"um motivo '([\w\s]+)'$")]
        public void DadoUmMotivoDeCancelamentoMotivo01UsadoNoCancelamentoDeUmCicloCiclo01(string motivo)
        {
            CriarMotivoCancelamento(motivo, CsStatusMotivoCancelamento.Ativo, SessionTest);
        }

        [Then(@"exibir a excessão '([\w\s]+)'$")]
        public void EntaoExibirAExcessaoOMotivoDeCancelamentoEstaSendoUsadoPorUmCancelamentoDeCiclo(string textoMensagem)
        {
            Assert.IsFalse(motivoCancelamentoDic["motivo 01"].RnNaoPermitirDeletarSeMotivoAssociado, textoMensagem);
        }

        [When(@"excluir o motivo de cancelamento '([\w\s]+)'$")]
        public void QuandoExcluirOMotivoDeCancelamentoMotivo01(string motivo)
        {
            motivoCancelamentoDic[motivo].Delete();
        }

        [Given(@"o\(s\) ciclo\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+) cancelado\(s\) com o motivo '([\w\s]+)'$")]
        public void DadoOSCicloSCiclo01ComoCanceladoComOMotivoMotivo01(string ciclos, string naousado, string motivo)
        {

            Colaborador colaborador01 = ColaboradorFactory.CriarColaborador(SessionTest, "000", DateTime.Now, "nome@fpf.br",
                "Solicitacao", "Orcamento", "Historico", "nome.completo");

            User u1 = ColaboradorFactory.CriarUsuario(SessionTest, "nome.completo", "Nome", "Completo",
                "nome@fpf.br", true);

            UsuarioDAO.CurrentUser = colaborador01.Usuario;

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

                StepCiclo.ciclosDic[value01].CsSituacaoCiclo = CsSituacaoCicloDomain.Cancelado;
                StepCiclo.ciclosDic[value01].MotivoCancelamento = motivoCancelamentoDic[motivo];
            }
        }

        [Given(@"o\(s\) ciclo\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+) como '([\w\s]+)'$")]
        public void DadoOSCicloSCiclo02Ciclo03ComoNaoPlanejado(string ciclos, string naousado, string situacao)
        {
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
                StepCiclo.ciclosDic[value01].CsSituacaoCiclo = StepCiclo.SituacaoCicloByText(situacao);
            }
        }

        [When(@"cancelar o ciclo '([\w\s]+)'$")]
        public void QuandoCancelarUmCertoCiclo(string ciclo)
        {
            StepCiclo.ciclosDic[ciclo].RnCancelarCiclo(null, DateTime.MinValue);
        }

		[When(@"cancelar o ciclo '([\w\s]+)' com o motivo '([\w\s]+)'$")]
        public void QuandoCancelarUmCertoCicloComUmCertoMotivo(string ciclo,string motivo)
        {
            StepCiclo.ciclosDic[ciclo].RnCancelarCiclo(motivoCancelamentoDic[motivo], DateTime.MinValue);
        }

        [When(@"cancelar o ciclo '([\w\s]+)' com o motivo '([\w\s]+)' e data de início do próximo ciclo com '(.*)'$")]
        public void QuandoCancelarUmCertoCicloComUmCertoMotivo(string ciclo, string motivo, string data)
        {
            StepCiclo.ciclosDic[ciclo].RnCancelarCiclo(motivoCancelamentoDic[motivo], DateTime.Parse(data));
        }

        [Then(@"o motivo do cancelamento do\(s\) ciclo\(s\) '([\w\s]+)' deve\(m\) estar com o motivo '([\w\s]+)'$")]
        public void EntaoOMotivoDoCancelamentoDoSCicloSCiclo02DeveMEstarComOMotivoMotivo01(string ciclo, string motivo)
        {
            Assert.AreEqual(motivoCancelamentoDic[motivo], StepCiclo.ciclosDic[ciclo].MotivoCancelamento, "Deveria vir o ultimo motivo de cancelamento utilizado por este usuário.");
        }

        //[When(@"indicar, no cancelamento do ciclo '(.*)', o motivo '(.*)'")]
        //public void QuandoIndicarNoCancelamentoDoCicloCiclo01OMotivoMotivo01(string ciclo, string motivo)
        //{
        //    StepCiclo.ciclosDic[ciclo].MotivoCancelamento = motivoCancelamentoDic[motivo];
        //}

        [When(@"obter lista de motivos ativos")]
        public void QuandoIndicarNoCancelamentoDoCicloCiclo01OMotivoMotivo01()
        {
            motivosAtivos = new Dictionary<string, MotivoCancelamento>();

            foreach(MotivoCancelamento mot in MotivoCancelamento.GetMotivosAtivos(SessionTest))
            {
                if (!motivosAtivos.ContainsKey(mot.TxDescricao))
                {
                    motivosAtivos.Add(mot.TxDescricao, mot);
                }
            }
        }

        [Given(@"os motivos de cancelamento (('[\w\sçãáéíóú]+'\s-\sstatus\s'[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void DadoOsMotivosDeCancelamentoMotivo01_StatusAtivoMotivo02_StatusAtivoMotivo03_StatusAtivoMotivo04_StatusInativoMotivo05_StatusAtivoMotivo06_StatusAtivo(string motivos, string naousado)
        {
            foreach (string motivo in motivos.Split(','))
            {
                string value01, value02;
                MatchCollection collection = Regex.Matches(motivo.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 2)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valores encontrados
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples
                value02 = collection[1].Value.Substring(1, collection[1].Length - 2); // retiradas das aspas simples

                CriarMotivoCancelamento(value01, StatusMotivoByText(value02), SessionTest);
            }
        }

        [Then(@"os motivos (('[\w\sçãáéíóú]+',?[\s]*?)+) devem vir na lista de motivos ativos")]
        public void EntaoCriarUmaSolecaoComOsMotivosDeCancelamentoMotivo01Motivo02Motivo03(string motivos, string naousado)
        {
            foreach (string motivo in motivos.Split(','))
            {
                string value01;
                MatchCollection collection = Regex.Matches(motivo.Trim(), @"'([\w\sçãáéíóú]+)'");

                if (collection.Count != 1)
                {
                    new Exception("Erro na expressão regular.");
                }

                // Valor encontrado
                value01 = collection[0].Value.Substring(1, collection[0].Length - 2); // retiradas das aspas simples

                if (motivosAtivos.ContainsKey(value01))
                {
                    motivosAtivos.Remove(value01);
                }
                else
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual(motivosAtivos.Count, 0, "A lista não veio conforme especificado.");
        }



        #endregion

        #region Resources
        /// <summary>
        /// Criar o Ciclo e as Estórias do Ciclo
        /// </summary>
        /// <param name="ciclo">Valor texto do Ciclo</param>
        /// <param name="estorias">Lista valores texto de Estórias do Ciclo</param>
        public static void CriarMotivoCancelamento(string motivoCancelamento, CsStatusMotivoCancelamento statusMotivo, Session session)
        {
            // Inserindo no dicionário
            if (!motivoCancelamentoDic.ContainsKey(motivoCancelamento))
            {
                MotivoCancelamento motivoCancelamentoObj = MotivoCancelamentoFactory.CriarMotivoCancelamento(session, motivoCancelamento, statusMotivo, true);
                motivoCancelamentoDic.Add(motivoCancelamento, motivoCancelamentoObj);
            }
        }

        /// <summary>
        /// Retorna a situação do Ciclo a partir do valor texto da mesma
        /// </summary>
        /// <param name="situacao">Valor texto da situação</param>
        public static CsStatusMotivoCancelamento StatusMotivoByText(string situacao)
        {
            CsStatusMotivoCancelamento retorno = CsStatusMotivoCancelamento.Ativo;

            switch (situacao)
            {
                case "Inativo":
                    retorno = CsStatusMotivoCancelamento.Inativo;
                    break;

                default:
                    new Exception("Status do Motivo não encontrado.");
                    break;
            }

            return retorno;
        }
        #endregion
    }
}