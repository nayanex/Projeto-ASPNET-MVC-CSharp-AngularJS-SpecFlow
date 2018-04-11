using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Test.UnitTest;
using TechTalk.SpecFlow.Assist;
using WexProject.Test.Helpers.BDD.Bind;
using System.Linq;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.Test.Fixtures.Factory;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding]
    public class StepProjetoCustos : BaseEntityFrameworkTest
    {
        private Projeto _projeto;
        private Aditivo _aditivo;
        private Rubrica _rubrica;
        private RubricaMes _rubricaMes;
        private CustosRubricasDto _custoRubricasMes;
        private List<Projeto> _projetos;

        private void InicializarCentrosCusto()
        {
            CentroCustoFactory.CriarCentrosDeCusto(1010, "ATC CONTROL");
            CentroCustoFactory.CriarCentrosDeCusto(2010, "LIFE TEST");
            CentroCustoFactory.CriarCentrosDeCusto(3010, "ANDROID TOYS");
        }

        [Given(@"que existem os projetos a seguir:")]
        public void DadoQueExistemOsProjetosASeguir(Table table)
        {
            InicializarCentrosCusto();
            foreach (var item in table.CreateSet<ProjetoCustosBindHelper>())
            {
				_projeto = ProjetoCustosFactory.CriarProjetoCustos(item.Nome, 0, item.Situacao, item.InicioPlanejado, item.InicioReal, 1);
                ScenarioContext.Current.Add("Projeto_" + item.Nome, _projeto);
            }
                   
        }

        [Given(@"que existem os aditivos a seguir:")]
        public void DadoQueExistemOsAditivosASeguir(Table table)
        {
            foreach (var item in table.CreateSet<AditivoBindHelper>())
            {
                Projeto p = ScenarioContext.Current.Get<Projeto>("Projeto_" + item.Projeto);
                _aditivo = new Aditivo
                {
                    TxNome = item.Descricao,
                    DtInicio = item.Inicio,
                    DtTermino = item.Termino,
                    NbDuracao = item.QtdeMeses,
                    NbOrcamento = item.OrcamentoAprovado,
                    ProjetoOid = p.Oid
                };

                List<Aditivo> aditivos = new List<Aditivo>();
                aditivos.Add(_aditivo);
                p.Aditivos = aditivos;

                ScenarioContext.Current.Add(_aditivo.TxNome, _aditivo);
                AditivoDao.Instance.SalvarAditivo(_aditivo);
            }
        }

        [Given(@"que o aditivo '(.*)' do projeto '(.*)' possui as seguintes rubricas:")]
        public void DadoQueOAditivoDoProjetoPossuiAsSeguintesRubricas(string aditivo, string projeto, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                TipoRubrica tp = ScenarioContext.Current.Get<TipoRubrica>(row["nome"]);
                Aditivo ad = ScenarioContext.Current.Get<Aditivo>(aditivo);
                _rubrica = new Rubrica
                {
                    TipoRubricaId = tp.TipoRubricaId,
                    AditivoId = ad.AditivoId,
                    NbTotalPlanejado = 1000
                };

                ScenarioContext.Current.Add(aditivo + row["nome"], _rubrica);
                RubricaDao.Instance.SalvarRubrica(_rubrica);
            }
        }

        [Given(@"que o aditivo '(.*)' do projeto '(.*)' possui as seguintes configurações das rubricas:")]
        public void DadoQueOAditivoDoProjetoPossuiAsSeguintesConfiguracoesDasRubricas(string aditivo, string projeto, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                Rubrica r = ScenarioContext.Current.Get<Rubrica>(aditivo + row["nome"]);
                _rubricaMes = new RubricaMes
                {
                    RubricaId = r.RubricaId,
                    CsMes = (CsMesDomain)Enum.Parse(typeof(CsMesDomain), row["Mes"]),
                    NbAno = Convert.ToInt32(row["Ano"]),
                    PossuiGastosRelacionados = false
                };

                RubricaMesDao.Instance.SalvarRubricaMes(_rubricaMes);
            }
        }

        [When(@"o usuário listar as rubricas do tipo '(.*)' no mês de '(.*)' de '(.*)'")]
        public void QuandoOUsuarioListarAsRubricasDoTipoNoMesDeDe(string tipoRubrica, string mes, int ano)
        {
           var classeTipoRubrica = (CsClasseRubrica)Enum.Parse(typeof(CsClasseRubrica), tipoRubrica);
           var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));          
           var _ano = (int)(ano);

           _custoRubricasMes = TipoRubricaBo.Instance.DetalharCustosTipoRubrica(classeTipoRubrica, _ano, _mes);
        }

        [Then(@"devo encontrar as seguintes rubricas administrativas:")]
        public void EntaoDevoEncontrarAsSeguintesRubricasAdministrativas(Table table)
        {
            var tamanhoTable = table.RowCount;
            var tamanhoLista = _custoRubricasMes.TiposRubricas.Count;

            Assert.IsNotNull(_custoRubricasMes, "Deveria trazer uma lista de rubricas");
            Assert.AreEqual(tamanhoTable, tamanhoLista, "Deveriam ser do mesmo tamanho");
            int i = 0;
            foreach (var rubrica in table.Rows)
            {
                Assert.AreEqual(rubrica["rubrica"], _custoRubricasMes.TiposRubricas[i].Nome);
                i++;
            }

        }

        [When(@"o usuário listar os projetos '(.*)' da rubrica '(.*)' no mês de '(.*)' de '(.*)'")]
        public void QuandoOUsuarioListarOsProjetosDaRubricaNoMesDeDe(string situacao, string rubrica, string mes, int ano)
        {
            TipoRubrica tp = ScenarioContext.Current.Get<TipoRubrica>(rubrica);
            var idTipoRubrica = tp.TipoRubricaId;
            var _mes = (int)((CsMesDomain)Enum.Parse(typeof(CsMesDomain), mes));

            _projetos = ProjetoDao.Instancia.ConsultarProjetosPorTipoRubrica(idTipoRubrica, ano, _mes);
        }


        [Then(@"devo encontrar os seguintes projetos:")]
        public void EntaoDevoEncontrarOsSeguintesProjetos(Table table)
        {
            var tamanhoTable = table.RowCount;
            var tamanhoListaProjetos = _projetos.Count;

            Assert.IsNotNull(_projetos, "Deveria trazer uma lista de projetos");
            Assert.AreEqual(tamanhoTable, tamanhoListaProjetos, "Deveriam ser do mesmo tamanho");

            int i = 0;
            foreach (var rubrica in table.Rows)
            {
                Assert.AreEqual(rubrica["projeto"], _projetos[i].TxNome);
                i++;
            }

        }


    }
}