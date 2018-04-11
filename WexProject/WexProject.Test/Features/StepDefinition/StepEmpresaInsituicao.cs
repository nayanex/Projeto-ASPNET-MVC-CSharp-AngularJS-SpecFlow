using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.Test.Fixtures.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.Xaf;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// StepDefinition de Empresa/Insituição
    /// </summary>
    [Binding]
    public class StepEmpresaInsituicao : BaseTest
    {
        #region Propierties

        /// <summary>
        /// Dicionario de Estórias usados no Step
        /// </summary>
        public static Dictionary<string, EmpresaInstituicao> EmpresaInstituicaoDict { get; set; }

        #endregion

        #region BeforeCenarios

        // Reinicia os valores das listas
        [BeforeScenario]
        public void ReiniciarValores()
        {
            EmpresaInstituicaoDict = new Dictionary<string, EmpresaInstituicao>();
        }

        #endregion
                
        #region Dados

        [Given(@"uma empresa com o nome '(.*)' e sigla '(.*)'")]
        public void DadoUmaEmpresaComUmCertoNomeESigla(string nome_empresa, string sigla)
        {
            EmpresaInstituicao emp = EmpresaInstituicaoFactory.Criar(SessionTest, nome_empresa, sigla);

            EmpresaInstituicaoDict.Add(nome_empresa, emp);
        }

        [Given(@"o país '(.*)' que está sendo usado na Empresa/Instituição '(.*)'")]
        public void DadoOSPaisEsPais01QueEstaSendoUsadoNaEmpresaInstituicaoEmpresa01(string pais, string empresa)
        {
            EmpresaInstituicaoDict[empresa].Pais = StepPais.PaisesDict[pais];
            EmpresaInstituicaoDict[empresa].Save();
        }

        [Given(@"as seguintes empresas/instituicoes:")]
        public void DadoAsSeguintesEmpresasInstituicoes(Table table)
        {
            string sigla = table.Header.ToList()[0];
            string nome = table.Header.ToList()[1];
            string email = table.Header.ToList()[2];
            string pais = table.Header.ToList()[3];
            string fone = table.Header.ToList()[4];

            foreach (TableRow row in table.Rows)
            {
                string siglaRow = row[sigla];
                string nomeRow = row[nome];
                string emailRow = row[email];
                string paisRow = row[pais];
                string foneRow = row[fone];

                EmpresaInstituicao empresa = EmpresaInstituicaoFactory.Criar(SessionTest, nomeRow, siglaRow,
                    emailRow, foneRow, false);

                empresa.Pais = StepPais.PaisesDict[paisRow];
                empresa.Save();
                EmpresaInstituicaoDict.Add(siglaRow, empresa);
            }
        }

        #endregion

        #region Quando
        
        [When(@"salvar a empresa '(.*)'")]
        public void QuandoSalvarAUmaCertaEmpresa(string nome_empresa)
        {
            EmpresaInstituicaoDict[nome_empresa].Save();
        }

        [When(@"uma empresa for criada com o nome '(.*)' e sigla '(.*)'")]
        public void QuandoUmaEmpresaForCriadaComUmNomeEmpresaESigla(string nome_empresa,string sigla)
        {
            EmpresaInstituicao emp = EmpresaInstituicaoFactory.Criar(SessionTest, nome_empresa, sigla);
            emp.Save();
            EmpresaInstituicaoDict.Add(nome_empresa, emp);
        }

        [When(@"selecionar o '(.*)' para a empresa '(.*)'")]
        public void QuandoSelecionarOPais02ParaAEmpresaEmpresa01(string nome_pais,string nome_empresa)
        {
            EmpresaInstituicaoDict[nome_empresa].Pais = StepPais.PaisesDict[nome_pais];
            EmpresaInstituicaoDict[nome_empresa].Save();
        }

        [When(@"deselecionar o pais '(.*)' da empresa '(.*)'")]
        public void QuandoDeselecionarOPaisPais02DaEmpresaEmpresa01(string nome_pais,string nome_empresa)
        {
            EmpresaInstituicaoDict[nome_empresa].Pais = null;
            EmpresaInstituicaoDict[nome_empresa].Save();
        }

        #endregion

        #region Entao

        [Then(@"a empresa '(.*)' não pode ser salva e deve exbir a mensagem 'Já existe uma empresa cadastrada com este mesmo nome'")]
        public void EntaoAEmpresaEmpresa01NaoPodeSerSalvaEDeveExbirAMensagemJaExisteUmaEmpresaCadastradaComEsteMesmoNome(string nome_empresa)
        {
            EmpresaInstituicao empresa = EmpresaInstituicaoDict[nome_empresa];
            
            var var = ValidationUtil.GetRuleStateID(empresa, "EmpresaInstituicao_TxNome_Unique", DevExpress.Persistent.Validation.DefaultContexts.Save);

            Assert.AreEqual(DevExpress.Persistent.Validation.ValidationState.Invalid, var);

        }

        [Then(@"deveria vir na lista de países o\(s\) país\(es\) '(([\w\sçãáéíóú]+,?[\s]*?)+)' para a empresa '(.*)'")]
        public void EntaoDeveriaVirNaListaDePaisesOSPaisEsPais01ParaAEmpresaEmpresa01(string paises, string naousado, string nome_empresa)
        {
            string[] s_paises = paises.Split(',');

            EmpresaInstituicao empresa = EmpresaInstituicaoDict[nome_empresa];

            Assert.AreEqual(s_paises.Length, empresa._PaisesAtivos.Count);

        }

        [Then(@"o pais da empresa '(.*)' deverá ser '(.*)'")]
        public void EntaoOPaisDaEmpresaEmpresa01DeveraSerPais02(string nome_empresa,string nome_pais)
        {

            Pais pais = StepPais.PaisesDict[nome_pais];
            EmpresaInstituicao empresa = EmpresaInstituicaoDict[nome_empresa];

            Assert.AreEqual(pais.Oid, empresa.Pais.Oid);

        }

        [Then(@"o campo '(.*)' da empresa '(.*)' deve estar com a máscara '(.*)'")]
        public void EntaoOCampoTelefoneDaEmpresaEmpresa01DeveEstarComAMascaraXxXxxx_Xxxx(string campo,string nome_empresa,string mascara)
        {
            EmpresaInstituicao empresa = EmpresaInstituicaoDict[nome_empresa];

            Assert.AreEqual(empresa._MascaraPais, mascara);

        }

        #endregion
    }
}
