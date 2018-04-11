using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using WexProject.BLL.Entities.Geral;
using WexProject.Test.Fixtures.Factory;
using WexProject.Test.UnitTest;
using TechTalk.SpecFlow.Assist;
using WexProject.Test.Helpers.BDD.Bind;
using WexProject.BLL.Shared.DTOs.Escopo;
using WexProject.BLL.BOs.Escopo;
using WexProject.BLL.DAOs.Geral;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WexProject.Test.Features.StepDefinition
{
    [Binding, Scope(Tag="Entity")]
    public class StepProjetoEntity : BaseEntityFrameworkTest
    {
        public static Dictionary<Guid, List<GraficoEscopoCompletudeDTO>> DadosGraficoEscopoCompletudeDic { get; set; }

        [BeforeScenario]
        public static void ReiniciarValores()
        {
           DadosGraficoEscopoCompletudeDic = new Dictionary<Guid, List<GraficoEscopoCompletudeDTO>>();
        }

        [Given( @"que exista\(m\) o\(s\) projeto\(s\) a seguir:" )]
        public void DadoQueExistaMOSProjetoSASeguir( Table table )
        {
            List<Projeto> projetos = table.CreateSet<ProjetoBindHelper>().Select( o => o.CriarProjeto() ).ToList();

            for (int i = 0; i < projetos.Count; i++)
                ProjetoFactoryEntity.CriarProjetoRitmo( contexto, (Int32)projetos[i].NbTamanhoTotal, projetos[i].TxNome, Convert.ToInt32( projetos[i].NbCicloTotalPlan ), Convert.ToInt32( projetos[i].NbRitmoTime ) );

            contexto.SaveChanges();
                
        }

        [When( @"montar os dados necessarios para o grafico de escopo vs completude do projeto '([\w\s]+)'" )]
        public static void QuandoMontarOsDadosNecessariosParaOGraficoDeEscopoVsCompletudeDoProjeto( string nomeProjeto )
        {
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorNome( contexto, nomeProjeto );
            List<GraficoEscopoCompletudeDTO> result = GraficoEscopoCompletudeBO.CalcularGraficoEscopoCompletude( projeto.Oid );

            DadosGraficoEscopoCompletudeDic.Add( projeto.Oid, result );
        }

        [Then( @"os dados para o grafico de escopo vs completude do projeto '([\w\s]+)' devem ser:" )]
        public void EntaoOsDadosParaOGraficoDeEscopoVsCompletudeDoProjetoProjeto01DevemSer( string nomeProjeto, Table table )
        {
            string moduloCol = table.Header.ToList()[0];
            string pontosNaoIniciadosCol = table.Header.ToList()[1];
            string percNaoIniciadosCol = table.Header.ToList()[2];
            string pontoEmAnaliseCol = table.Header.ToList()[3];
            string percEmAnaliseCol = table.Header.ToList()[4];
            string pontoDesenvCol = table.Header.ToList()[5];
            string percDesenvCol = table.Header.ToList()[6];
            string pontosProntosCol = table.Header.ToList()[7];
            string percProntosCol = table.Header.ToList()[8];
            string pontosDesvioCol = table.Header.ToList()[9];
            string percDesvioCol = table.Header.ToList()[10];
            string pontosMudancaCol = table.Header.ToList()[11];
            string percMudancaCol = table.Header.ToList()[12];
            string totalModuloCol = table.Header.ToList()[13];

            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorNome( contexto, nomeProjeto );

            Assert.IsTrue( DadosGraficoEscopoCompletudeDic.ContainsKey( projeto.Oid ), "O projeto deveria existir no dicionário" );
            Assert.AreEqual( table.RowCount, DadosGraficoEscopoCompletudeDic[projeto.Oid].Count(), "As quantidades de registros deveriam ser as mesmas" );

            List<GraficoEscopoCompletudeDTO> lista = DadosGraficoEscopoCompletudeDic[projeto.Oid];

            for(int position = 0; position < table.RowCount; position++)
            {
                Assert.AreEqual( table.Rows[position][moduloCol], lista[position].Modulo );
                Assert.AreEqual( double.Parse( table.Rows[position][pontosNaoIniciadosCol] ), lista[position].PontoNaoIniciados );
                Assert.AreEqual( double.Parse( table.Rows[position][percNaoIniciadosCol] ), lista[position].PercNaoInciado );
                Assert.AreEqual( double.Parse( table.Rows[position][pontoEmAnaliseCol] ), lista[position].PontosEmAnalise );
                Assert.AreEqual( double.Parse( table.Rows[position][percEmAnaliseCol] ), lista[position].PercEmAnalise );
                Assert.AreEqual( double.Parse( table.Rows[position][pontoDesenvCol] ), lista[position].PontosEmDesenv );
                Assert.AreEqual( double.Parse( table.Rows[position][percDesenvCol] ), lista[position].PercEmDesenv );
                Assert.AreEqual( double.Parse( table.Rows[position][pontosProntosCol] ), lista[position].PontosProntos );
                Assert.AreEqual( double.Parse( table.Rows[position][percProntosCol] ), lista[position].PercProntos );
                Assert.AreEqual( double.Parse( table.Rows[position][pontosDesvioCol] ), lista[position].PontosDesvio );
                Assert.AreEqual( double.Parse( table.Rows[position][percDesvioCol] ), lista[position].PercDesvio );
                Assert.AreEqual( double.Parse( table.Rows[position][pontosMudancaCol] ), lista[position].PontosMudanca );
                Assert.AreEqual( double.Parse( table.Rows[position][percMudancaCol] ), lista[position].PercMudanca );
                Assert.AreEqual( double.Parse( table.Rows[position][totalModuloCol] ), lista[position].TotalPontosModulo );
            }
        }
    }
}
