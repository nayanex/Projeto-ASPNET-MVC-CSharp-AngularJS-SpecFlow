using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Escopo;

namespace WexProject.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Módulo
    /// </summary>
    [Binding]
    class StepModulo : BaseTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Modulos usados no Step
        /// </summary>
        public static Dictionary<string, Modulo> ModulosDic { get; set; }

        /// <summary>
        /// Dicionario de Modulos por projetos
        /// </summary>
        public static Dictionary<string, Dictionary<string, Modulo>> ProjetoModulosDic { get; set; }

        
        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            ModulosDic = new Dictionary<string, Modulo>();
            ProjetoModulosDic = new Dictionary<string, Dictionary<string, Modulo>>();
        }

        #endregion

        #region Given

        [Given(@"que existam os seguintes modulos para o '(.*)':")]
        public void GivenQueExistamOsSeguintesModulosParaOProjeto(String projeto, Table table)
        {
            for (int position = 0; position < table.RowCount; position++)
            {
                string nome = table.Rows[position][table.Header.ToList()[0]];
                uint tamanho = uint.Parse(table.Rows[position][table.Header.ToList()[1]]);
                string moduloPai = table.Rows[position][table.Header.ToList()[2]];
                Modulo m;
                if (moduloPai != null && !moduloPai.Equals(""))
                {
                    m = ModuloFactory.CriarFilho(SessionTest, ProjetoModulosDic[projeto][moduloPai], nome, tamanho, true);
                }
                else
                {
                    m = ModuloFactory.Criar(SessionTest, StepProjeto.ProjetosDic[projeto], nome, tamanho, true);
                }
                
                if (!ProjetoModulosDic.ContainsKey(projeto)) {
                    ProjetoModulosDic.Add(projeto, new Dictionary<string,Modulo>());
                }
                ProjetoModulosDic[projeto].Add(m.TxNome, m);
                
            }
        }

        
        [Given(@"o módulo '(.*)'")]
        public void DadoOModulo2_Escopo(string modulo)
        {
            ModulosDic.Add(modulo,
                ModuloFactory.Criar(SessionTest,
                ProjetoFactory.Criar(SessionTest, 0, string.Format("Projeto - {0}", modulo), true),
                modulo,
                true)
                );
        }

        #endregion

        #region Then
        [Then(@"os modulos a seguir do projeto '([\w\s]+)' devem estar com os seguintes valores:")]
        public void EntaoOsModulosASeguirDoProjetoADevemEstarComOsSeguintesValores(string projeto, Table table)
        {
            string nome = table.Header.ToList()[0];
            string tamanho = table.Header.ToList()[1];
            //string moduloPai = table.Header.ToList()[2];
            //string nomeModulo = string.Empty;

            
            for (int position = 0; position < table.RowCount; position++)
            {
                Assert.AreEqual(Convert.ToDouble(table.Rows[position][tamanho]), ProjetoModulosDic[projeto][table.Rows[position][nome]].NbPontosTotal);
            }
        }
        #endregion

    }
}