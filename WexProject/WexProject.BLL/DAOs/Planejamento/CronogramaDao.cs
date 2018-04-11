using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using System.Data.Entity;
using WexProject.BLL.Contexto;
using System.Linq.Expressions;
using WexProject.BLL.Extensions.Entities;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class CronogramaDao
    {
        #region Consultas

        /// <summary>
        /// Método responsável por pesquisar cronograma pelo nome
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="txDescricaoCronograma">Descrição do cronograma</param>
        /// <returns>Cronograma encontrado</returns>
        public static Cronograma ConsultarCronogramaPorNome( WexDb contexto, String txDescricaoCronograma )
        {
            return contexto.Cronograma.Include( o => o.SituacaoPlanejamento ).FirstOrDefault( o => o.TxDescricao == txDescricaoCronograma && !o.CsExcluido );
        }

        /// <summary>
        /// Método responsável por buscar todos os cronogramas no banco
        /// É usado pelo serviço quando solicitado pela tela de cronograma
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <returns>Uma coleção com os cronogramas</returns>
        public static List<Cronograma> ConsultarCronogramas()
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.Cronograma.Include( o => o.SituacaoPlanejamento ).OrderBy( o => o.TxDescricao ).Where( o => !o.CsExcluido ).ToList();
            }
        }

        /// <summary>
        /// Métodos responsável por buscar um cronograma pelo Oid.
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <param name="oidCronograma">Oid do Cronograma (ID)</param>
        /// <returns>Objeto Cronograma</returns>
        public static Cronograma ConsultarCronogramaPorOid( Guid oidCronograma, params Expression<Func<Cronograma, object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return ConsultarCronogramaPorOid( contexto, oidCronograma, includes );
            }
        }

        /// <summary>
        /// Métodos responsável por buscar um cronograma pelo Oid.
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <param name="oidCronograma">Oid do Cronograma (ID)</param>
        /// <returns>Objeto Cronograma</returns>
        public static Cronograma ConsultarCronogramaPorOid( WexDb contexto, Guid oidCronograma, params Expression<Func<Cronograma, object>>[] includes )
        {
            if(contexto == null)
                contexto = ContextFactoryManager.CriarWexDb();
            return contexto.Cronograma.MultiploInclude( includes ).FirstOrDefault( o => o.Oid == oidCronograma && !o.CsExcluido );
        }

        /// <summary>
        /// Método responsável por consultar cronograma e as relações de cronograma com tarefa.
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="oidCronograma">Oid do banco a ser pesquisado</param>
        /// <returns>Objeto cronograma carregado com as relaçoes</returns>
        public static Cronograma ConsultarCronogramaCronogramaTarefasETarefasPodOidCronograma( WexDb contexto, Guid oidCronograma )
        {
            return contexto.Cronograma.Include( o => o.SituacaoPlanejamento ).Include( o => o.CronogramaTarefas.Select( a => a.Tarefa.TarefaHistoricoTrabalhos ) ).FirstOrDefault( o => !o.CsExcluido && o.Oid == oidCronograma );
        }

        /// <summary>
        /// Método responsável por consultar o cronograma por oid e trazer também seus cronogramaTarefas, Tarefas e Historicos das tarefas
        /// </summary>
        /// <param name="contexto">cointexto do banco</param>
        /// <param name="oidCronograma">Oid do cronograma</param>
        /// <returns>Cronograma pesquisado</returns>
        public static Cronograma ConsultarCronogramaCronogramaTarefasEHistoricosPorOidCronograma( WexDb contexto, Guid oidCronograma )
        {
            return contexto.Cronograma.Include( o => o.CronogramaTarefas.Select( b => b.Tarefa ).Select( c => c.TarefaHistoricoTrabalhos ) ).FirstOrDefault( o => !o.CsExcluido && o.Oid == oidCronograma );
        }

        #endregion

        /// <summary>
        /// Método responsável por salvar um novo cronograma
        /// </summary>
        /// <param name="cronograma"></param>
        /// <returns></returns>
        public static bool SalvarCronograma( Cronograma cronograma )
        {
            try
            {
                using(var contexto = ContextFactoryManager.CriarWexDb())
                {
                    var cronogramaSelecionado = ConsultarCronogramaPorOid( cronograma.Oid );

                    if(cronogramaSelecionado == null)
                    {
                        contexto.Cronograma.Add( cronograma );
                    }
                    else
                    {
                        cronogramaSelecionado.OidSituacaoPlanejamento = cronograma.OidSituacaoPlanejamento;
                        cronogramaSelecionado.DtInicio = cronograma.DtInicio;
                        cronogramaSelecionado.DtFinal = cronograma.DtFinal;
                        contexto.Entry( cronogramaSelecionado ).State = EntityState.Modified;
                    }

                    contexto.SaveChanges();
                    return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
