using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Planejamento;
using WexProject.Test.Fixtures.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Shared.Domains.Geral;
using System.Reflection;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.Enumerator;
using WexProject.Library.Libs.Xaf;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de País
    /// </summary>
    [Binding]
    public class StepPais : BaseTest
    {
        #region Properties

        public static Dictionary<string,Pais> PaisesDict { get; set; }

        public static Dictionary<string, ValidationState> MensagemPaisesDict { get; set; }

        #endregion

        #region BeforeScenarios

        // Reinicia os valores das listas
        [BeforeScenario]
        public void ReiniciarValores()
        {
            PaisesDict = new Dictionary<string, Pais>();
            MensagemPaisesDict = new Dictionary<string, ValidationState>();
        }

        #endregion

        #region Dados

        [Given(@"o pais '(.*)' marcado como '(.*)'")]
        public void DadoOPaisMarcadoComUmEstado(string nome_pais, string state)
        {
            ChangeStatePais(PaisesDict[nome_pais], state);
            PaisesDict[nome_pais].Save();
        }

        [Given(@"o\(s\) país\(es\) (('[\w\sçãáéíóú]+',?[\s]*?)+)")]
        public void DadoOSPaisEs(string paises, string naousado)
        {
            string[] pais = paises.Split(',');

            foreach (var item in pais)
            {
                Pais p = PaisFactory.Criar(SessionTest,item.Substring(1, item.Length - 2));

                PaisesDict.Add(item.Substring(1, item.Length - 2), p);
            }
        }

        [Given(@"o pais '(.*)' ter como mascara '(.*)'")]
        public void DadoOPaisPais02TerComoMascaraXxXxxx_Xxxx(string nome_pais,string mascara)
        {
            PaisesDict[nome_pais].TxMascara = mascara;
            PaisesDict[nome_pais].Save();
        }

        [Given(@"os seguintes paises:")]
        public void GivenOsSeguintesPaises(Table table)
        {
            string nome = table.Header.ToList()[0];
            string mascara = table.Header.ToList()[1];
            string situacao = table.Header.ToList()[2];
            string padrao = table.Header.ToList()[3];

            foreach (TableRow row in table.Rows)
            {
                string nomeRow = row[nome];
                string mascaraRow = row[mascara];
                string situacaoRow = row[situacao];
                string padraoRow = row[padrao];

                Pais pais = PaisFactory.Criar(SessionTest, nomeRow, mascaraRow, CsSituacaoDomain.Ativo, false);

                // Set de situação
                PropertyInfo info = typeof(Pais).GetProperty("CsSituacao");
                info.SetValue(pais, EnumUtil.ValueEnum(typeof(CsSituacaoDomain), situacaoRow), null);

                pais.IsPadrao = bool.Parse(padraoRow);
                pais.Save();
                PaisesDict.Add(nomeRow, pais);
            }
        }

        #endregion

        #region Quando

        [When(@"tentar excluir o pais '(.*)'")]
        public void QuandoTentarExcluirOPaisPais01(string pais)
        {
            ValidationState state = ValidationUtil.GetRuleState(PaisesDict[pais],
                "RuleIsReferenced_EmpresaInstituicaoPais", DefaultContexts.Delete);

            MensagemPaisesDict.Add(pais, state);
        }

        [When(@"criar um pais '(.*)' com situação '(.*)' e marcado como '(.*)'")]
        public void QuandoCriarUmPaisPais02ComSituacaoAtivoEMarcadoComoPadrao(string pais, string situacao, string padrao)
        {
            PaisesDict.Add(pais, new Pais(SessionTest));

            ChangeStatePais(PaisesDict[pais], situacao);
            ChangeStatePais(PaisesDict[pais], padrao);
        }

        [When(@"definir que o país padrão agora deve ser o país '(.*)'")]
        public void QuandoDefinirQueOPaisPadraoAgoraDeveSerOPaisPais01(string pais)
        {
            PaisesDict[pais].RnMudarPaisPadrao();
        }

        [When(@"exibir a janela de modificação de pais padrão para o pais '(.*)'")]
        public void QuandoExibirAJanelaDeModificacaoDePaisPadraoParaOPaisPais02(string pais)
        {
            Assert.IsTrue(PaisesDict[pais].RnIsExibirJanelaMudancaPaisPadrao());
        }

        #endregion

        #region Então

        [Then(@"deverá exibir a seguinte excessão para o pais '(.*)': '(.*)'")]
        public void EntaoDeveraExibirASeguinteExcessaoParaOPaisPais01OPaisEstaSendoUsadoNumaEmpresaInstituicao(string pais, string mensagem)
        {
            Assert.IsTrue(MensagemPaisesDict.ContainsKey(pais), "Deveria haver mensagem para o País");

            Assert.AreEqual(ValidationState.Invalid, MensagemPaisesDict[pais], "A mensagem não veio conforme esperada");
        }

        [Then(@"a Máscara do País '(.*)' deve estar com o valor padrão '(.*)'")]
        public void EntaoAMascaraDoPaisPais02DeveEstarComOValorPadrao55(string pais, string mascara)
        {
            Assert.AreEqual(mascara, PaisesDict[pais].TxMascara);
        }

        [Then(@"exibir a janela de modificação de pais padrão para o pais '(.*)'")]
        public void EntaoDeveraExibirAJanelaDeModificacaoDePaisPadraoParaOPaisPais02(string nome_pais)
        {
            Pais pais = PaisesDict[nome_pais];
            Assert.IsTrue(pais.RnIsExibirJanelaMudancaPaisPadrao());
        }

        [Then(@"não exibir a janela de modificação de pais padrão para o pais '(.*)'")]
        public void EntaoNaoDeveraExibirAJanelaDeModificacaoDePaisPadraoParaOPaisPais03(string nome_pais)
        {
            Pais pais = PaisesDict[nome_pais];
            Assert.IsFalse(pais.RnIsExibirJanelaMudancaPaisPadrao());
        }

        [Then(@"o pais '(.*)' deve estar como '(.*)'")]
        public void EntaoOPaisPais01DeveSerMarcadoComoNaoPadrao(string pais, string state)
        {
            switch (state)
            {
                case "ativo":
                    Assert.AreEqual(PaisesDict[pais].CsSituacao, CsSituacaoDomain.Ativo);
                    break;

                case "inativo":
                    Assert.AreEqual(PaisesDict[pais].CsSituacao, CsSituacaoDomain.Inativo);
                    break;

                case "padrão":
                    Assert.IsTrue(PaisesDict[pais].IsPadrao);
                    break;

                case "não padrão":
                    Assert.IsFalse(PaisesDict[pais].IsPadrao);
                    break;
            }
        }

        #endregion

        #region Utils

        private void ChangeStatePais(Pais pais, string state) 
        {
            switch (state)
            {
                case "ativo":
                    pais.CsSituacao = CsSituacaoDomain.Ativo;
                    break;

                case "inativo":
                    pais.CsSituacao = CsSituacaoDomain.Inativo;
                    break;

                case "padrão":
                    pais.IsPadrao = true;
                    break;

                case "não padrão":
                    pais.IsPadrao = false;
                    break;
            }
        }

        #endregion
    }
}
