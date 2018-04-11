using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    [Binding]
    public class StepGraficoBurndown
    {
        [When( @"o grafico burndown for calculado para o cronograma '(.*)'" )]
        public void QuandoOGraficoBurndownForCalculadoParaOCronograma( string nomeCronograma )
        {
            BurndownGraficoDto dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( StepCronograma.CronogramasDic[nomeCronograma].Oid );
            ArmazenarDadosBurndownContexto( nomeCronograma, dadosGrafico, true );
        }

        #region Método Auxiliares

        /// <summary>
        /// Armazena no contexto do cenário em execução os dados do retorno do cálculo para disponibilizar o dados globalmente entre os steps.
        /// </summary>
        /// <param name="nomeCronograma">Key do dictionary</param>
        /// <param name="dadosGrafico">Value que será armazenado no dictionary</param>
        /// <param name="retorno"></param>
        public static void ArmazenarDadosBurndownContexto( string nomeCronograma, BurndownGraficoDto dadosGrafico, bool retorno = false )
        {
            string prefixo = retorno ? "DadosRetorno" : "DadosEsperados";
            ScenarioContext.Current.Add( prefixo + nomeCronograma   , dadosGrafico );
        }

        #endregion
    }
}
