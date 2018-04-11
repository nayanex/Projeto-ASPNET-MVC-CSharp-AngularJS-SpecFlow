using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.DataHora.Extension;
using WexProject.Library.Libs.Extensions.Log;

namespace WexProject.BLL.BOs.Planejamento
{
    public class GraficoBurndownBO
    {
        #region Atributos

        /// <summary>
        /// Atributo que guarda a instância da classe para reutilizá-la aumentando assim a performance
        /// </summary>
        private static GraficoBurndownBO instancia;

        #endregion

        #region Propriedades

        /// <summary>
        /// Propriedade responsável por retornar uma instância da classe e guardá-la para reaproveitamento caso ainda não esteja criada.
        /// </summary>
        public static GraficoBurndownBO Instancia
        {
            get
            {
                if(instancia == null)
                    instancia = new GraficoBurndownBO();

                return instancia;
            }
        }

        #endregion

        #region Construtor

        public GraficoBurndownBO()
        {

        }

        #endregion

        /// <summary>
        /// Método responsável por realizar os cálculos necessários para formar o gráfico de Burndown
        /// </summary>
        /// <returns>Lista contendo os dados calculados que formam o gráfico</returns>
        public BurndownGraficoDto CalcularDadosGraficoBurndown( Guid oidCronograma )
        {
            List<BurndownDadosDto> dadosGrafico = new List<BurndownDadosDto>();

            Cronograma cronograma = CronogramaDao.ConsultarCronogramaPorOid( oidCronograma, o => o.CronogramaTarefas );

            var diasUteis = CalendarioBo.CalcularDiasUteis( cronograma.DtInicio, cronograma.DtFinal );

            double totalHorasCronograma = TarefaHistoricoEstimativaDao.ConsultarTotalHorasPlanejadasCronograma( oidCronograma );

            totalHorasCronograma = totalHorasCronograma.ToTimeSpan().TotalHours;

            double cargaDiaria = DividirCargaHoraria( totalHorasCronograma, diasUteis.Count - 1 );
            double horasRestantes = totalHorasCronograma;

            diasUteis.ForEach( data => dadosGrafico.Add( CriarDtoPlanejamentoGraficoBurndown( cargaDiaria, ref horasRestantes, data ) ) );

            var somatorios = TarefaHistoricoEstimativaDao.ConsultarTotalHorasRealizadasCronograma( oidCronograma, cronograma.DtInicio, cronograma.DtFinal );
            if(somatorios.Count > 0)
            {
                diasUteis = diasUteis.Union( somatorios.Keys.Where( o => o.Date <= DateUtil.ConsultarDataHoraAtual().Date ) ).OrderBy( o => o ).ToList();
                var ultimaEstimativa = somatorios.LastOrDefault();
                var dataUltimaEstimativa = ultimaEstimativa.Key.AddDays( 1 );
                var somaUltimaEstimativa = ultimaEstimativa.Value;

                while(dataUltimaEstimativa.Date <= cronograma.DtFinal && dataUltimaEstimativa.Date <= DateUtil.ConsultarDataHoraAtual().Date)
                {
                    if(diasUteis.Any( o => o.Date.Equals( dataUltimaEstimativa ) && o.Date <= DateUtil.ConsultarDataHoraAtual().Date ))
                    {
                        somatorios.Add( dataUltimaEstimativa, somaUltimaEstimativa );
                    }
                    dataUltimaEstimativa = dataUltimaEstimativa.AddDays( 1 );
                }
                dadosGrafico.AddRange( CriarDtoRealizadoGraficoBurndown( somatorios ) );
            }
            double desvio = CalcularDesvio( dadosGrafico );
            return new BurndownGraficoDto { DadosBurndown = dadosGrafico , Desvio = desvio };
        }

        /// <summary>
        /// Método responsável por efetuar o calculo da quantidade de horas em desvio do burndown
        /// </summary>
        /// <param name="dadosBurndown">lista de dados do burndown</param>
        /// <returns>resultado do calculo de desvio</returns>
        public static  double CalcularDesvio( List<BurndownDadosDto> dadosBurndown )
        {
            var dataAtual = DateUtil.ConsultarDataHoraAtual();
            var selecao = dadosBurndown.Where(o => o.Dia.Date <= dataAtual.Date).OrderByDescending( o => o.Dia );
            var ultimoDiaPlanejado = selecao.FirstOrDefault( q => q.CsTipo == CsTipoBurndown.Planejado );
            var ultimoDiaRealizado = selecao.FirstOrDefault( q => q.CsTipo == CsTipoBurndown.Realizado );
            var totalUltimoPlanejado = ultimoDiaPlanejado != null ? ultimoDiaPlanejado.QtdeHoras.GetValueOrDefault() : 0;
            var totalUltimoRealizado = ultimoDiaRealizado != null ? ultimoDiaRealizado.QtdeHoras.GetValueOrDefault() : 0;
            return totalUltimoPlanejado - totalUltimoRealizado;
        }


        /// <summary>
        /// Método responsável por criar os dtos de dados sobre a estimativa de realizaçãod o grafico de burndown do cronograma
        /// </summary>
        /// <param name="somatorios">lista com os somatórios do histórico horas restantes</param>
        /// <returns></returns>
        private static List<BurndownDadosDto> CriarDtoRealizadoGraficoBurndown( Dictionary<DateTime, double> somatorios )
        {
            if(somatorios == null)
                return new List<BurndownDadosDto>();

            return somatorios
             .Select( o => new BurndownDadosDto { CsTipo = CsTipoBurndown.Realizado, Dia = o.Key, QtdeHoras = o.Value.ToTimeSpan().TotalHours } )
             .ToList();
        }

        /// <summary>
        /// método para criar um dto de informações sobre o planejado de um dia da sprint
        /// </summary>
        /// <param name="ritmoDiario">ritmo de consumo diário</param>
        /// <param name="horasRestantes">quantidade de horas restantes</param>
        /// <param name="dataUtil">data do planejado</param>
        /// <returns></returns>
        private static BurndownDadosDto CriarDtoPlanejamentoGraficoBurndown( double ritmoDiario, ref double horasRestantes, DateTime dataUtil )
        {
            BurndownDadosDto dadosDia = new BurndownDadosDto
            {
                Dia = dataUtil,
                CsTipo = CsTipoBurndown.Planejado
            };

            if(horasRestantes > 0)
                dadosDia.QtdeHoras = Math.Round( horasRestantes, 2, MidpointRounding.AwayFromZero );

            horasRestantes -= ritmoDiario;
            return dadosDia;
        }

        /// <summary>
        /// Método para efetuar uma divisão segura
        /// </summary>
        /// <param name="totalHoras">carga horária total</param>
        /// <param name="quantidadeDias">quantidade de dias</param>
        /// <returns>retorna a carga horária diária</returns>
        private static double DividirCargaHoraria( double totalHoras, double quantidadeDias )
        {
            return quantidadeDias > 0 ? totalHoras / quantidadeDias : 0;
        }
    }
}
