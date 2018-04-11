using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.BOs.TotvsWex;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.TotvsWex;
using WexProject.Test.Fixtures.Factory.Custos;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding]
    public class StepAssociarNostasFiscais
    {
        private RubricaMes rubricaMes;
        private NotaFiscal gasto;
        private List<RubricaDto> rubricas;

        [Given(@"que existam as despesas reais no aditivo '(.*)' do projeto '(.*)' conforme informadas a seguir:")]
        public void DadoQueExistamAsDespesasReaisNoAditivoDoProjetoConformeInformadasASeguir(string aditivo, string projeto, Table table)
        {
            foreach (var row in table.Rows)
            {
                //Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + row["rubrica"]);
                var tipoRubrica = ScenarioContext.Current.Get<TipoRubrica>(row["rubrica"]);
                var Projeto = ScenarioContext.Current.Get<Projeto>("Projeto_" + projeto);

                foreach (var mes in table.Header.Skip(1))
                {
					CsMesDomain csMes = (CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes);

                    var despesaRealDto = new DespesaRealDto
                    {
                        ProjetoOid = Projeto.Oid,
                        TipoRubricaId = tipoRubrica.TipoRubricaId,
                        Mes = csMes,
                        Ano = 2014,
                        DespesaReal = (Decimal?)(row[mes].Equals("") ? 0 : Convert.ToDecimal(row[mes]))
                    };
                    RubricaMesBo.Instance.SalvarDespesaReal(despesaRealDto);
                }
            }
        }

        [Given(@"que existam as seguintes notas fiscais pendentes de associacao do aditivo '(.*)' do projeto '(.*)' no mes de '(.*)' de '(.*)'")]
        public void DadoQueExistamAsSeguintesNotasFiscaisPendentesDeAssociacaoDoAditivoDoProjetoNoMesDeDe(string aditivo, string projeto, string mes, int ano, Table table)
        {
            var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));
            var data = new DateTime(ano, _mes, 1);

            foreach (var row in table.Rows)
            {
                var descricao = row["descricao"];
                decimal valor = Convert.ToDecimal(row["valor"]);

                gasto = NotaFiscalFactory.CriarNotaFiscal(data, 1, descricao, valor, 1);
                ScenarioContext.Current.Add(ano.ToString() + mes + gasto.Descricao, gasto);
            }
        }

        [When(@"as notas fiscais abaixo forem associadas com a rubrica '(.*)' do aditivo '(.*)' do projeto '(.*)' no mes de '(.*)' de '(.*)':")]
        public void QuandoAsNotasFiscaisAbaixoForemAssociadasComARubricaDoAditivoDoProjetoNoMesDeDe(string rubrica, string aditivo, string projeto, string mes, int ano, Table table)
        {
            foreach (var row in table.Rows)
            {
                Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + rubrica);
                NotaFiscal gasto = ScenarioContext.Current.Get<NotaFiscal>(ano.ToString() + mes + row["descricao"]);

                var gastoDto = new NotaFiscalDto
                {
                    GastoId = gasto.Id,
                    CentroDeCustoId = gasto.CentroDeCustoId,
                    Descricao = gasto.Descricao,
                    Data = gasto.Data,
                    Valor = gasto.Valor
                };

                NotasFiscaisBo.Instance.AssociarNotaFiscal(gastoDto, r.RubricaId);
            }
        }

        [Then(@"as despesas reais seguintes devem ser encontradas para o aditivo '(.*)' do projeto '(.*)' na rubrica '(.*)' no mes de '(.*)' de '(.*)':")]
        public void EntaoAsDespesasReaisSeguintesDevemSerEncontradasParaOAditivoDoProjetoNaRubricaNoMesDeDe(string aditivo, string projeto, string rubrica, string mes, int ano, Table table)
        {
            var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));
	 
            Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + rubrica);
            var rm = RubricaMesDao.Instance.ConsultarRubricaMes(r, _mes, ano);
 
            Assert.AreEqual(table.Rows[0]["rubrica"], r.TipoRubrica.TxNome);	 
            Assert.AreEqual(Convert.ToDecimal(table.Rows[0]["despesa real"]), rm.NbGasto);
        }

        [Given(@"que existam as seguintes notas fiscais associadas a rubrica '(.*)' do aditivo '(.*)' do projeto '(.*)' no mes de '(.*)' de '(.*)'")]
        public void DadoQueExistamAsSeguintesNotasFiscaisAssociadasARubricaDoAditivoDoProjetoNoMesDeDe(string rubrica, string aditivo, string projeto, string mes, int ano, Table table)
        {
            var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));
            var data = new DateTime(ano, _mes, 1);

            foreach (var row in table.Rows)
            {
                var descricao = row["descricao"];
                decimal valor = Convert.ToDecimal(row["valor"]);

                var notaFiscal = NotaFiscalFactory.CriarNotaFiscal(data, 1, descricao, valor, 1);
                ScenarioContext.Current.Add(ano.ToString() + mes + notaFiscal.Descricao, notaFiscal);

                Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + rubrica);
                NotaFiscal gasto = ScenarioContext.Current.Get<NotaFiscal>(ano.ToString() + mes + row["descricao"]);

                var gastoDto = new NotaFiscalDto
                {
                    GastoId = gasto.Id,
                    CentroDeCustoId = gasto.CentroDeCustoId,
                    Descricao = gasto.Descricao,
                    Data = gasto.Data,
                    Valor = gasto.Valor
                };

                NotasFiscaisBo.Instance.AssociarNotaFiscal(gastoDto, r.RubricaId);
            }
        }

        [When(@"as notas fiscais abaixo forem desassociadas da rubrica '(.*)' do aditivo '(.*)' do projeto '(.*)' no mes de '(.*)' de '(.*)':")]
        public void QuandoAsNotasFiscaisAbaixoForemDesassociadasDaRubricaDoAditivoDoProjetoNoMesDeDe(string rubrica, string aditivo, string projeto, string mes, int ano, Table table)
        {
            foreach (var row in table.Rows)
            {
                Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + rubrica);
                NotaFiscal gasto = ScenarioContext.Current.Get<NotaFiscal>(ano.ToString() + mes + row["descricao"]);

                NotasFiscaisBo.Instance.DesassociarNotaFiscal(r.RubricaId, gasto.Id);
            }           
        }

        [When(@"as despesas reais do projeto '(.*)' e aditivo '(.*)' tiverem as rubricas para associar notas fiscais listadas")]
        public void QuandoAsDespesasReaisDoProjetoEAditivoTiveremAsRubricasParaAssociarNotasFiscaisListadas(string projeto, string aditivo)
        {
            Aditivo ad = ScenarioContext.Current.Get<Aditivo>(aditivo);
            rubricas = RubricaBo.Instance.PesquisarRubricasNotasFiscais(ad.AditivoId);
        }

        [Then(@"devo encontrar nas despesas reais as seguintes rubricas de desenvolvimento para vincular as notas fiscais:")]
        public void EntaoDevoEncontrarNasDespesasReaisAsSeguintesRubricasDeDesenvolvimentoParaVincularAsNotasFiscais(Table table)
        {
            Assert.AreEqual(table.Rows[0]["rubrica"], rubricas[0].Nome);
        }

    }
}
