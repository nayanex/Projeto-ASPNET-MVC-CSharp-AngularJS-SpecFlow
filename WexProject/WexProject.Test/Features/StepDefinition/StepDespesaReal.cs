using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.Test.Features.StepDefinition
{
	[Binding]
	public class StepDespesaReal
	{
		[When(@"as despesas reais do projeto '(.*)' forem informadas conforme a seguir:")]
		public void QuandoAsDespesasReaisDoProjetoForemInformadasConformeASeguir(string projeto, Table table)
		{
			foreach (var row in table.Rows)
			{
				var tipoRubrica = ScenarioContext.Current.Get<TipoRubrica>(row["rubrica"]);
				var Projeto = ScenarioContext.Current.Get<Projeto>("Projeto_" + projeto);

				var numeroColunas = table.Header.Count;
				List<CsMesDomain> meses = new List<CsMesDomain>();
				int i = 1;
				while (i < numeroColunas)
				{
					meses.Add((CsMesDomain)Enum.Parse(typeof(CsMesDomain), table.Header.ToList()[i]));
					i++;
				}
				int j = 0;
				while (j < meses.Count)
				{
					var despesaRealDto = new DespesaRealDto
					{
						ProjetoOid = Projeto.Oid,
						TipoRubricaId = tipoRubrica.TipoRubricaId,
						Mes = meses[j],
						Ano = 2014,
						DespesaReal = (row[meses[j].ToString()].Equals("") ? 0 : Convert.ToDecimal(row[meses[j].ToString()])),
					};
					RubricaMesBo.Instance.SalvarDespesaReal(despesaRealDto);
					j++;
				}
			}
		}

		[Then(@"devo encontrar as seguintes despesas reais no ano de (.*) para o aditivo '(.*)' do projeto '(.*)':")]
		public void EntaoDevoEncontrarAsSeguintesDespesasReaisNoAnoDeParaOAditivoDoProjeto(int ano, string aditivo, string projeto, Table table)
		{
			Projeto _projeto = ScenarioContext.Current.Get<Projeto>("Projeto_" + projeto);

			foreach (var row in table.Rows)
			{
				Rubrica rubrica = ScenarioContext.Current.Get<Rubrica>(aditivo + row["rubrica"]);

				foreach (var mes in table.Header.Skip(1))
				{
					var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));

					RubricaMes rubricaMes = RubricaMesDao.Instance.ConsultarRubricaMes(rubrica, _mes, ano);

					Assert.AreEqual(row[mes], rubricaMes.NbGasto);
				}
			}
		}
	}
}
