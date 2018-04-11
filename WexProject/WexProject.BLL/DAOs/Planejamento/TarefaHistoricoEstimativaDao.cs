using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Helpers;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Extensions.Entities;
using WexProject.Library.Libs.Extensions.Log;
using WexProject.Library.Libs.DataHora.Extension;
using WexProject.Library.Libs.DataHora;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class TarefaHistoricoEstimativaDao
    {
        /// <summary>
        /// Método responsável por salvar no contexto um histórico de estimativa de uma determinada tarefa
        /// </summary>
        /// <param name="historicoEstimativa">histórico de estimativa</param>
        public static TarefaHistoricoEstimativa SalvarHistorico( TarefaHistoricoEstimativa historicoEstimativa )
        {
            if(historicoEstimativa == null)
                return null;

            using(var contexto = ContextFactoryManager.CriarWexDb())
            {
                try
                {
                    contexto.TarefaHistoricoEstimativa.Add( historicoEstimativa );
                    contexto.SaveChanges();
                }
                catch(Exception)
                {
                    return null;
                }
            }
            return historicoEstimativa;
        }


        /// <summary>
        /// Método responsável por efetuar a consulta de um histórico de uma tarefa por uma determinada condição
        /// </summary>
        /// <param name="contexto">contexto de conexão com o banco</param>
        /// <param name="oidTarefa">oid da tarefa específica</param>
        /// <param name="data">data especificada</param>
        /// <returns></returns>
        public static TarefaHistoricoEstimativa ConsultarHistoricoEstimativa( WexDb contexto, Func<TarefaHistoricoEstimativa, bool> predicado = null, params Expression<Func<TarefaHistoricoEstimativa, object>>[] relacionamentos )
        {
            if(contexto == null)
                throw new ArgumentNullException( "O contexto do banco está nulo." );

            return predicado == null ? contexto.TarefaHistoricoEstimativa.FirstOrDefault() : contexto.TarefaHistoricoEstimativa.FirstOrDefault( predicado );
        }

        /// <summary>
        /// Método responsável por efetuar a consulta de uma lista de historicos de estimativa
        /// </summary>
        /// <param name="contexto">contexto de conexão com o banco</param>
        /// <param name="oidTarefa">oid da tarefa específica</param>
        /// <param name="data">data especificada</param>
        /// <returns></returns>
        public static IQueryable<TarefaHistoricoEstimativa> ConsultarHistoricosEstimativa( WexDb contexto, Func<TarefaHistoricoEstimativa, bool> predicado = null )
        {
            if(contexto == null)
                throw new ArgumentNullException( "O contexto do banco está nulo." );

            return predicado == null ? contexto.TarefaHistoricoEstimativa.AsQueryable() : contexto.TarefaHistoricoEstimativa.Where( predicado ).AsQueryable();
        }

        /// <summary>
        /// Método responsável por efetuar a consulta de um histórico de uma tarefa para a data especificada
        /// </summary>
        /// <param name="contexto">contexto de conexão com o banco</param>
        /// <param name="oidTarefa">oid da tarefa específica</param>
        /// <param name="data">data especificada</param>
        /// <returns></returns>
        public static TarefaHistoricoEstimativa ConsultarHistoricoEstimativaPorOidTarefaEData( WexDb contexto, Guid oidTarefa, DateTime data )
        {
            if(contexto == null)
                throw new ArgumentNullException( "O contexto do banco está nulo." );

            if(oidTarefa == Guid.Empty)
                throw new ArgumentException( string.Format( "O oid da tarefa inválido. valor atual:{0}", oidTarefa ) );

            return contexto
                .TarefaHistoricoEstimativa
                .Where( o => o.DtPlanejado.Day == data.Day && o.DtPlanejado.Month == data.Month && o.DtPlanejado.Year == data.Year && o.OidTarefa == oidTarefa )
                .OrderByDescending( o => o.DtPlanejado )
                .FirstOrDefault();
        }

        /// <summary>
        /// Método responsável por carregar o total de horas planejadas para o cronograma por oid
        /// </summary>
        /// <param name="oidCronograma">oid de identificação do cronograma selecionado</param>
        /// <returns>retorna o total de horas planejadas </returns>
        public static double ConsultarTotalHorasPlanejadasCronograma( Guid oidCronograma )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                var cronograma = CronogramaDao.ConsultarCronogramaPorOid( contexto, oidCronograma );

                if(cronograma == null)
                    return 0;

                return ( from cronogramaTarefa in contexto.CronogramaTarefa
                         join historicoEstimativa in contexto.TarefaHistoricoEstimativa
                         on cronogramaTarefa.OidTarefa equals historicoEstimativa.OidTarefa
                         where cronogramaTarefa.OidCronograma == oidCronograma
						 && historicoEstimativa.DtPlanejado.Year <= cronograma.DtInicio.Year 
						 && ( (historicoEstimativa.DtPlanejado.Month < cronograma.DtInicio.Month ) || ( historicoEstimativa.DtPlanejado.Month <= cronograma.DtInicio.Month && historicoEstimativa.DtPlanejado.Day <= cronograma.DtInicio.Day ) )
                         && !cronogramaTarefa.CsExcluido && !cronogramaTarefa.Tarefa.CsExcluido
                         orderby historicoEstimativa.DtPlanejado descending
                         select historicoEstimativa )
						.ToList()
						 .GroupBy( historicoEstimativa => historicoEstimativa.OidTarefa )
						 .Select( historicoEstimativa => historicoEstimativa.FirstOrDefault() )
						 .Sum( historicoEstimativa => (double?)historicoEstimativa.NbHoraRestante ) ?? 0;
            }
        }

        /// <summary>
        /// Método responsável por carregar o total de horas realizadas até a data estipulada
        /// </summary>
        /// <param name="oidCronograma">oid de identificação do cronograma selecionado</param>
        /// <returns>retorna o total de horas planejadas </returns>
        public static Dictionary<DateTime, double> ConsultarTotalHorasRealizadasCronograma( Guid oidCronograma, DateTime dataInicio, DateTime dataFinal )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                var historicos = contexto.CronogramaTarefa
                .Where( o => o.OidCronograma == oidCronograma && !o.CsExcluido && !o.Tarefa.CsExcluido )
                .Join( contexto.TarefaHistoricoEstimativa, ct => ct.OidTarefa,
                    th => th.OidTarefa,
                    ( c, t ) => t )
                .ToList();

                var datas = historicos.Where( o => o.DtPlanejado.Date >= dataInicio.Date).Select( o => o.DtPlanejado.Date ).Distinct().OrderBy( o => o ).ToList();
                Dictionary<DateTime, double> somatorios = new Dictionary<DateTime, double>();
                foreach(var data in datas)
                {
                    if(data.Date > DateUtil.ConsultarDataHoraAtual().Date)
                        break;
                    double soma = 0;
                    soma = historicos.Where( o => o.DtPlanejado.Date <= data.Date )
                    .OrderByDescending( o => o.DtPlanejado )
                    .ToLookup( o => o.OidTarefa )
                    .Select( o => o.FirstOrDefault())
                    .ToList()
                    .Sum( o => o.NbHoraRestante);
                    somatorios.Add( data, soma );
                }
                return somatorios;
            }
        }
    }
}
