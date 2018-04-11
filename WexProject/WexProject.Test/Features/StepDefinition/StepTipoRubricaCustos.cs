using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;
using WexProject.Test.UnitTest;
using System.Data.Entity.Migrations;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.Custos;
using TechTalk.SpecFlow.Assist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Helpers.BDD.Bind;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding]
    public class StepTipoRubricaCustos : BaseEntityFrameworkTest
    {
        TipoRubrica tipoRubrica;
        Rubrica rubrica;
        RubricaMes rubricaMes;

        private void InicializarClassesProjetos()
        {
            using (var _db = ContextFactoryManager.CriarWexDb())
            {
                _db.ClassesProjetos.AddOrUpdate(
                    new ClasseProjeto { ClasseProjetoId = 1, TxNome = "Projeto Patrocinado" },
                    new ClasseProjeto { ClasseProjetoId = 2, TxNome = "Projeto sem Patrocínio" },
                    new ClasseProjeto { ClasseProjetoId = 3, TxNome = "Setor" }
                );

                _db.SaveChanges();
            }
        }

        private void InicializarTiposProjetos()
        {
            using (var _db = ContextFactoryManager.CriarWexDb())
            {
                _db.TiposProjetos.AddOrUpdate(
                    new TipoProjeto { TipoProjetoId = 1, ClasseProjetoId = 1, TxNome = "Projeto Base" },
                    new TipoProjeto { TipoProjetoId = 2, ClasseProjetoId = 3, TxNome = "Setor de Administração" },
                    new TipoProjeto { TipoProjetoId = 3, ClasseProjetoId = 2, TxNome = "Projeto Base" }
                );

                _db.SaveChanges();
            }
        }

        [Given(@"que existem os seguintes tipos de rubricas:")]
        public void DadoQueExistemOsSeguintesTiposDeRubricas(Table table)
        {
            InicializarClassesProjetos();
            InicializarTiposProjetos();
            int idTipoRubrica = 1;
            foreach (var item in table.CreateSet<TipoRubricaBindHelper>())
            {
                tipoRubrica = new TipoRubrica
                {
                    TipoRubricaId = idTipoRubrica,
                    TxNome = item.Nome,
                    CsClasse = item.Classe,
                    TipoProjetoId = 1
                };

                ScenarioContext.Current.Add(tipoRubrica.TxNome, tipoRubrica);
                TipoRubricaDao.Instance.SalvarTipoRubrica(tipoRubrica);
                idTipoRubrica++;
            }
        }

        [Given(@"que o aditivo '(.*)' do projeto '(.*)' possui o seguinte planejamento para uso do orcamento aprovado:")]
        public void DadoQueOAditivoDoProjetoPossuiOSeguintePlanejamentoParaUsoDoOrcamentoAprovado(string aditivo, string projeto, Table table)
        {
            foreach (var item in table.CreateSet<RubricaBindHelper>())
            {
                TipoRubrica tp = ScenarioContext.Current.Get<TipoRubrica>(item.Rubrica);
                Aditivo ad = ScenarioContext.Current.Get<Aditivo>(aditivo);

                rubrica = new Rubrica 
                {
                    TipoRubricaId = tp.TipoRubricaId,
					AditivoId = ad.AditivoId,
                    NbTotalPlanejado = item.Total,
                    TipoRubrica = tp
                };

				ad.Rubricas.Add(rubrica);

                ScenarioContext.Current.Add(aditivo + item.Rubrica, rubrica);
                RubricaDao.Instance.SalvarRubrica(rubrica);
            }

            foreach (var row in table.Rows)
            {
                Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + row["rubrica"]);
                var numeroColunas = table.Header.Count;
                List<CsMesDomain> meses = new List<CsMesDomain>();
                int i = 2;
                while (i < numeroColunas)
                {
                    meses.Add((CsMesDomain)Enum.Parse(typeof(CsMesDomain), table.Header.ToList()[i]));
                    i++;
                }
                int j = 0;
                while (j < meses.Count)
                {
                    rubricaMes = new RubricaMes
                    {
                        RubricaId = r.RubricaId,
                        CsMes = meses[j],
                        NbAno = 2014,
                        NbPlanejado = (Decimal?)(row[meses[j].ToString()].Equals("") ? 0 : Convert.ToDecimal(row[meses[j].ToString()])),
                        PossuiGastosRelacionados = false
                    };
                    RubricaMesDao.Instance.SalvarRubricaMes(rubricaMes);
                    j++;
                }                
            }            
        }

        [Given(@"que o aditivo '(.*)' do projeto '(.*)' possua as seguintes despesas reais informadas:")]
        public void DadoQueOAditivoDoProjetoPossuaAsSeguintesDespesasReaisInformadas(string aditivo, string projeto, Table table)
        {
            foreach (var row in table.Rows)
            {
                Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + row["rubrica"]);
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
                    rubricaMes = new RubricaMes
                    {
                        RubricaId = r.RubricaId,
                        CsMes = meses[j],
                        NbAno = 2014,
                        NbGasto = (Decimal?)(row[meses[j].ToString()].Equals("") ? 0 : Convert.ToDecimal(row[meses[j].ToString()])),
                        PossuiGastosRelacionados = false
                    };
                    RubricaMesDao.Instance.SalvarRubricaMes(rubricaMes);
                    j++;
                }
            }                     
        }

        [When(@"os custos administrativos dos projetos '(.*)' forem calculados para o mes de '(.*)' de '(.*)'")]
        public void QuandoOsCustosAdministrativosDosProjetosForemCalculadosParaOMesDeDe(string situacaoProjeto, string mes, int ano)
        {
            var _ano = (int)(ano);
            var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));

            var tipoRubricaMesesDto = TipoRubricaBo.Instance.ListarCustosTiposRubricas(CsClasseRubrica.Administrativo, _ano, _mes);

			ScenarioContext.Current.Add("TipoRubricaMes_" + mes + "_" + ano, tipoRubricaMesesDto);
        }

        [Then(@"devo encontrar os seguintes custos administrativos no mes de '(.*)' de '(.*)':")]
        public void EntaoDevoEncontrarOsSeguintesCustosAdministrativosNoMesDeDe(string mes, int ano, Table table)
        {
            var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));
			var chave = "TipoRubricaMes_" + mes + "_" + ano;

			List<CustoTipoRubricaDto> tipoRubricaMesesDto;

			if (ScenarioContext.Current.ContainsKey(chave))
			{
				tipoRubricaMesesDto = ScenarioContext.Current.Get<List<CustoTipoRubricaDto>>(chave);
			}
			else
			{
				tipoRubricaMesesDto = TipoRubricaBo.Instance.ListarCustosTiposRubricas(CsClasseRubrica.Administrativo, ano, _mes);
			}

			Assert.IsNotNull(tipoRubricaMesesDto);
            List<CustoProjetoDto> todosProjetos = new List<CustoProjetoDto>();

            foreach (var row in table.Rows)
            {
				foreach (var custo in tipoRubricaMesesDto)
                {
                    if (custo.Nome.Equals(row["rubrica"]))
                    {
                        var projetos = RubricaMesBo.Instance.ListarCustosProjetos(custo.TipoRubricaId, ano, _mes);
                        todosProjetos.AddRange(projetos);
                    }
                }
            }

            int i = 0;
            foreach (var row in table.Rows)
            {
                Assert.AreEqual(row["projeto"], todosProjetos[i].NomeProjeto);
                Assert.AreEqual(Convert.ToDecimal(row["orcamento aprovado"]), todosProjetos[i].OrcamentoAprovado);
                Assert.AreEqual(Convert.ToDecimal(row["saldo disponivel"]), todosProjetos[i].SaldoDisponivel);
                Assert.AreEqual(Convert.ToDecimal(row["despesa real"]), todosProjetos[i].DespesaReal.HasValue ? todosProjetos[i].DespesaReal.Value : 0);
                i++;
            }        
        }
    }
}
