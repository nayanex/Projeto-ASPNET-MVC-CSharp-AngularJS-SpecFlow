using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Rh;
using WexProject.Test.Fixtures.Factory;
using DevExpress.Xpo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Geral;
using DevExpress.Persistent.Validation;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de PlanejamentoFerias
    /// </summary>
    [Binding]
    class StepPlanejamentoFerias : BaseTest
    {
        #region Properties

        public static Dictionary<string, FeriasPlanejamento> PlanejamentoFeriasDic { get; set; }

        public static XPCollection<FeriasPlanejamento> listaPlanejamentos;

        public static List<Projeto> projetosOcultos;

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas (antes dos cenários)
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            projetosOcultos = new List<Projeto>();
            listaPlanejamentos = new XPCollection<FeriasPlanejamento>();
            PlanejamentoFeriasDic = new Dictionary<string, FeriasPlanejamento>();
        }

        #endregion

        #region Given

        [Given(@"que existam os seguintes planejamentos de ferias:")]
        public void GivenQueExistamOsSeguintesPlanejamentosDeFerias(Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string colaborador = table.Rows[position][table.Header.ToList()[0]];
                string dataIncial = table.Rows[position][table.Header.ToList()[1]];
                string modalidade = table.Rows[position][table.Header.ToList()[2]];

                FeriasPlanejamento f = PlanejamentoFeriasFactory.Criar(SessionTest, StepColaborador.ColaboradoresDic[colaborador], DateTime.Parse(dataIncial), StepModalidadeFerias.ModalidadeFeriasDic[modalidade], true);
                PlanejamentoFeriasDic.Add(String.Format("{0}_{1}", colaborador, dataIncial), f);

            }
        }


        #endregion

        #region When

        [When(@"consultar o planejamento de ferias no periodo de '(.*)' a '(.*)':")]
        public void QuandoConsultarOPlanejamentoDeFeriasNoPeriodoDe(string incial, string final)
        {
            QuandoConsultarOPlanejamentoDeFeriasNoPeriodoDeESuperioresImediatos(incial, final, "", "");
        }

        [When(@"consultar o planejamento de ferias no periodo de '(.*)' a '(.*)' filtrando pelos superiores imediatos (('[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void QuandoConsultarOPlanejamentoDeFeriasNoPeriodoDeESuperioresImediatos(string incial, string final, string superiores, string vazioGambi)
        {
            DateTime startDate = new DateTime(int.Parse(incial.Split('/')[1]), int.Parse(incial.Split('/')[0]), 1);
            DateTime endDate = new DateTime(int.Parse(final.Split('/')[1]), int.Parse(final.Split('/')[0]), 1).AddMonths(+1).AddDays(-1);

            if (superiores != null && !superiores.Equals(""))
            {
                string[] superConsulta = new string[superiores.Split(',').Length];
                int i = 0;
                foreach(string superior in superiores.Split(',')) 
                {
                    superConsulta[i] = StepColaborador.ColaboradoresDic[superior.Replace("'","" )].Oid.ToString();
                    i++;
                }

                listaPlanejamentos = FeriasPlanejamento.GetPlanejamentoFerias(SessionTest, superConsulta, null, startDate, endDate);
            }
            else
            {
                listaPlanejamentos = FeriasPlanejamento.GetPlanejamentoFerias(SessionTest, null, null, startDate, endDate);
            }
        }   

        [When(@"ocultar os dados do '(.*)'")]
        public static void QuandoOcultarOsDadosDoProjeto(string projeto)
        {
            projetosOcultos.Add(StepProjeto.ProjetosDic[projeto]);
        }

        [When(@"criar planejamento de ferias para o colaborador '(.*)' iniciado '(.*)' na modalidade '(.*)'")]
        public static void QuandoCriarPlanejamentoDeFeriasPlan01NaModalidade30EIniciado15072012(string colaborador, string dtInicio, string modalidade)
        {
            PlanejamentoFeriasDic.Add(String.Format("{0}_{1}", colaborador, dtInicio),
                PlanejamentoFeriasFactory.Criar(SessionTest, StepColaborador.ColaboradoresDic[colaborador],
                DateTime.Parse(dtInicio), StepModalidadeFerias.ModalidadeFeriasDic[modalidade], false));
        }

        #endregion

        #region Then

        [Then(@"devem ser listados no plenejamento de ferias os colaboradores:")]
        public static void EntaoDevemSerListadosNoPlenejamentoDeFeriasOsColaboradores(Table table)
        {
            List<Colaborador> colaboradoresListados = FeriasPlanejamento.RNGetColaboradoresVisiveis(listaPlanejamentos, projetosOcultos);
            Assert.AreEqual(table.RowCount, colaboradoresListados.Count);
            
            //Verifica se todos planejamentos de ferias estão presentes na lista.
            for (int position = 0; position < table.RowCount; position++)
            {
                string colaborador = table.Rows[position][0];

                bool colaboradorPresente = false;
                foreach (Colaborador item in colaboradoresListados)
                {
                    if (item.Usuario.FirstName.Equals(colaborador))
                    {
                        colaboradorPresente = true;
                        break;
                    }
                }

                Assert.IsTrue(colaboradorPresente, String.Format("O colaborador {0} não está na lista", colaborador));
            }
        }

        [Then(@"devem ser listados os planejamentos de ferias:")]
        public void ThenDevemSerListadosOsPlanejamentosDeFerias(Table table)
        {
            List<FeriasPlanejamento> ferias = FeriasPlanejamento.RNGetPlanejamentosVisiveis(listaPlanejamentos, projetosOcultos);

            Assert.AreEqual(table.RowCount, ferias.Count);
            
            //Verifica se todos planejamentos de ferias estão presentes na lista.
            for (int position = 0; position < table.RowCount; position++)
            {
                string colaborador = table.Rows[position][0];

                bool colaboradorPresente = false;
                foreach (FeriasPlanejamento item in ferias)
                {
                    if (item.Periodo.Colaborador.Usuario.FirstName.Equals(colaborador))
                    {
                        colaboradorPresente = true;
                        break;
                    }
                }
                Assert.IsTrue(colaboradorPresente, String.Format("O colaborador {0} não está na lista", colaborador));
            }
        }

        [Then(@"o planejamento de férias para o colaborador '(.*)' iniciado '(.*)' estará apto a salvar")]
        public void EntaoOPlanejamentoDeFeriasParaOColaboradorColab1Iniciado15072012EstaraAptoASalvar(string colaborador, string dtInicio)
        {
            RuleSetValidationResult rsvr = (new RuleSet()).ValidateTarget(
                PlanejamentoFeriasDic[String.Format("{0}_{1}", colaborador, dtInicio)], DefaultContexts.Save);

            foreach (RuleSetValidationResultItem item in rsvr.Results)
            {
                Assert.AreEqual(ValidationState.Valid, item.State);
            }
        }

        #endregion
    }
}
