using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Models.Geral;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Colaborador
    /// </summary>
    [Binding]
    class StepColaborador : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Colaboradores usados no Step
        /// </summary>
        public static Dictionary<string, Colaborador> ColaboradoresDic { get; set; }

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            ColaboradoresDic = new Dictionary<string, Colaborador>();
        }

        #endregion

        #region Dados

        [Given(@"ter colaborador\(es\) (('[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void DadoTerColaboradorEsColaborador01Colaborador02Colaborador03Colaborador04(string colaboradores, string naousado)
        {
            foreach (string colaborador in colaboradores.Split(','))
            {
                string value01;

                // Valores encontrados
                value01 = colaborador.Substring(1, colaborador.Length - 2); // retiradas das aspas simples

                ColaboradoresDic.Add(value01, ColaboradorFactory.CriarColaborador(SessionTest, value01));
            }
        }

        [Given(@"que existam os colaboradores:")]
        public void DadoQueExistamOsColaboradores(Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string nome = table.Rows[position][table.Header.ToList()[0]];
                string superior = table.Rows[position][table.Header.ToList()[1]];
                string admissao = table.Rows[position][table.Header.ToList()[2]];

                Colaborador c = ColaboradorFactory.CriarColaborador(SessionTest, nome, true);
                c.DtAdmissao = DateTime.Parse(admissao);

                if (superior != null && !superior.Equals(""))
                {
                    c.Coordenador = ColaboradoresDic[superior];
                }

                c.Save();
                ColaboradoresDic.Add(nome, c);
            }
        }

        [Given(@"que existam os usuarios:")]
        public void DadoQueExistamOsUsuarios(Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string username = table.Rows[position][table.Header.ToList()[0]];

                Colaborador c = ColaboradorFactory.CriarColaborador(SessionTest, username, false);
                c.Save();
                ColaboradoresDic.Add(username, c);
            }
        }

        #endregion
        #region When
       
        #endregion
    }
}