using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Analise;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Test.Fixtures.Factory;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class AnaliseCustosTest : BaseEntityFrameworkTest
    {
        Projeto projeto1, projeto2, projeto3, projeto4;
        Aditivo aditivo1, aditivo2, aditivo3, aditivo4, aditivo5;

        private void InicializarProjetos()
        {
            DateTime? TerminoRealVazio = null;

            projeto1 = ProjetoAnaliseFactory.CriarProjetoCustos("PROJETO DE TESTE", CsProjetoSituacaoDomain.EmAndamento, new DateTime(2013, 08, 05), new DateTime(2014, 11, 01), TerminoRealVazio);
            projeto2 = ProjetoAnaliseFactory.CriarProjetoCustos("BugTest", CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 01, 14), new DateTime(2014, 01, 09), new DateTime(2014, 03, 12));
            projeto3 = ProjetoAnaliseFactory.CriarProjetoCustos("P5", CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 01, 02), new DateTime(2014, 01, 02), TerminoRealVazio);
            projeto4 = ProjetoAnaliseFactory.CriarProjetoCustos("Wex TIM", CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 02, 17), new DateTime(2014, 07, 04), new DateTime(2014, 07, 17));
        }

        private void InicializarAditivos()
        {
            aditivo1 = AditivoFactory.CriarAditivo("Aditivo1 De P1", 93802, 2, new DateTime(2013, 11, 01), new DateTime(2014, 11, 01), projeto1.Oid);
            aditivo2 = AditivoFactory.CriarAditivo("Aditivo2 De P1", 150000, 3, new DateTime(2013, 08, 05), new DateTime(2014, 12, 05), projeto1.Oid);
            aditivo3 = AditivoFactory.CriarAditivo("Aditivo De P2", 350000, 12, new DateTime(2014, 01, 14), new DateTime(2014, 03, 11), projeto2.Oid);
            aditivo4 = AditivoFactory.CriarAditivo("Aditivo De P3", 50100, 3, new DateTime(2014, 01, 02), new DateTime(2014, 12, 21), projeto3.Oid);
            aditivo5 = AditivoFactory.CriarAditivo("Aditivo De P4", 50100, 3, new DateTime(2014, 02, 13), new DateTime(2014, 02, 28), projeto4.Oid);

        }

        [TestInitialize]
        public void InicializarDependenciasBanco()
        {
            InicializarProjetos();
            InicializarAditivos();
        }

        [TestMethod]
        public void CalcularTempoConsumido_DataDeTerminoRealMaiorQueDataDeTerminoDoProjeto_RetornarProcentagemMaiorQue100()
        {
            List<Aditivo> aditivos = new List<Aditivo>();
            aditivos.Add(aditivo5);

            int tempoPlanejamentoProj = AnaliseBo.Instance.CalcularTempoPlanejado(aditivos);
            int tempoConsumido = AnaliseBo.Instance.CalcularTempoConsumido(projeto4, aditivos);
            
            Assert.AreEqual(6 , tempoConsumido);

            Assert.AreEqual(1, tempoPlanejamentoProj);

            Assert.IsTrue(tempoConsumido > tempoPlanejamentoProj);

            Assert.AreEqual(600, AnaliseBo.CalcularPorcentagem(tempoConsumido, tempoPlanejamentoProj) );

        }

        [TestMethod]
        public void CalcularTempoConsumido_DataDeTerminoRealNaoFornecida_RetornarValorBaseadoEmDataAtual()
        {
            List<Aditivo> aditivosDeProjeto1 = new List<Aditivo>();
            List<Aditivo> aditivosDeProjeto3 = new List<Aditivo>();

            aditivosDeProjeto1.Add(aditivo1);
            aditivosDeProjeto1.Add(aditivo2);

            aditivosDeProjeto3.Add(aditivo4);

            int tempoPlanejamentoProj1 = AnaliseBo.Instance.CalcularTempoPlanejado(aditivosDeProjeto1);
            int tempoPlanejamentoProj3 = AnaliseBo.Instance.CalcularTempoPlanejado(aditivosDeProjeto3);

            int tempoConsumidoProj1 = AnaliseBo.Instance.CalcularTempoConsumido(projeto1, aditivosDeProjeto1);
            int tempoConsumidoProj3 = AnaliseBo.Instance.CalcularTempoConsumido(projeto3, aditivosDeProjeto3);
            
            Assert.AreEqual(AnaliseBo.CalcularQuantidadeDeMesesEntreDatas(aditivosDeProjeto1[1].DtInicio, DateTime.Now), tempoConsumidoProj1);
            Assert.AreEqual(17, tempoPlanejamentoProj1);

            Assert.AreEqual(AnaliseBo.CalcularQuantidadeDeMesesEntreDatas(aditivosDeProjeto3[0].DtInicio, DateTime.Now), tempoConsumidoProj3);
            Assert.AreEqual(12, tempoPlanejamentoProj3);
         
        }

        
        [TestMethod]
        public void CalcularQuantidadeDeMesesEntreDuasDatas_ValoresDeMesesIguais_RetornarUmMes()
        {
            DateTime dataInicial = new DateTime(2014, 01, 01);
            DateTime dataFinal = new DateTime(2014, 01, 14);

            int qtdDeMeses = AnaliseBo.CalcularQuantidadeDeMesesEntreDatas(dataInicial, dataFinal);

            Assert.AreEqual(1, qtdDeMeses);

        }

        [TestMethod]
        public void CalcularQuantidadeDeMesesEntreDuasDatas_ValoresDeMesesDiferentes_RetornarDoisMeses()
        {
            DateTime dataInicial = new DateTime(2014, 02, 28);
            DateTime dataFinal = new DateTime(2014, 03, 01);

            int qtdDeMeses = AnaliseBo.CalcularQuantidadeDeMesesEntreDatas(dataInicial, dataFinal);

            Assert.AreEqual(2, qtdDeMeses);
        }

		// /// <summary>
		///// Testar se calcula porcentagem de tempo ocorrido com sucesso 
		///// </summary>
		//[TestMethod]
		//public void TestSeCalculaTempoOcorridoComSucesso() {
		//	DateTime DataInicial = Convert.ToDateTime("01/01/2012");
		//	DateTime DataFinal = Convert.ToDateTime("01/07/2012");

		//	var Analise = new AnaliseBo();
		//	Assert.AreEqual(50, Analise.TempoOcorrido(12, DataInicial, DataFinal));
		//}

		///// <summary>
		///// Testar se calcula porcentagem de consumo de orçamento com sucesso
		///// </summary>
		//[TestMethod]
		//public void TestSeCalculaOrcamentoConsumidoComSucesso()
		//{
		//	var Analise = new AnaliseBo();
		//	Assert.AreEqual(50, Analise.OrcamentoConsumido(100, 50));
		//}
    }
}
