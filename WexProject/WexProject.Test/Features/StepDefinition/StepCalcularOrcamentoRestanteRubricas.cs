using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models.Custos;

namespace WexProject.Test.Features.StepDefinition
{
	[Binding]
	public class StepCalcularOrcamentoRestanteRubricas
	{
		private Decimal _valorRestante;

		[Given(@"que o aditivo '(.*)' do projeto '(.*)' esta sendo planejado nesse momento")]
		public void DadoQueOAditivoDoProjetoEstaSendoPlanejadoNesseMomento(string p0, string p1)
		{
			
		}

		[When(@"o orcamento aprovado do aditivo '(.*)' do projeto '(.*)' tiver o valor restante para rubricas recalculado")]
		public void QuandoOOrcamentoAprovadoDoAditivoDoProjetoTiverOValorRestanteParaRubricasRecalculado(string aditivo, string projeto)
		{
			Aditivo ad = ScenarioContext.Current.Get<Aditivo>(aditivo);
			_valorRestante = AditivoBo.Instance.CalcularOrcamentoRestante(ad.AditivoId);
		}

		[Then(@"devo encontrar no orcamento aprovado do projeto um valor restante de '(.*)'")]
		public void EntaoDevoEncontrarNoOrcamentoAprovadoDoProjetoUmValorRestanteDe(Decimal valorRestante)
		{
			Assert.AreEqual(valorRestante, _valorRestante);
		}

		[When(@"o orcamento aprovado do aditivo '(.*)' do projeto '(.*)' receber o valor de '(.*)' na rubrica '(.*)'")]
		public void QuandoOOrcamentoAprovadoDoAditivoDoProjetoReceberOValorDeNaRubrica(string aditivo, string projeto, Decimal orcamentoAprovado, string rubrica)
		{
			Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + rubrica);

			r.NbTotalPlanejado = orcamentoAprovado;

			RubricaDao.Instance.SalvarRubrica(r);
		}

		[Given(@"que o orcamento aprovado do aditivo '(.*)' do projeto '(.*)' foi replanejado conforme a seguir:")]
		public void DadoQueOOrcamentoAprovadoDoAditivoDoProjetoFoiReplanejadoConformeASeguir(string aditivo, string projeto, Table table)
		{
			foreach (var row in table.Rows)
			{
				Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + row["rubrica"]);

				r.NbTotalPlanejado = Convert.ToDecimal(row["Total"]);

				RubricaDao.Instance.SalvarRubrica(r);
			}
		}
	}
}
