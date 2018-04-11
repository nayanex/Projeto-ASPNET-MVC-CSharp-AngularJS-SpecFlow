using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using System.Data.Entity;
using WexProject.BLL.Contexto;
using System.Linq.Expressions;
using WexProject.BLL.Extensions.Entities;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class TarefaDao
    {
        #region Consultar

        /// <summary>
        /// Método responsável por buscar uma tarefa pelo Oid
        /// </summary>
        /// <param name="oidTarefa">Oid da tarefa</param>
        /// <returns>Objeto Tarefa</returns>
        public static Tarefa ConsultarTarefaPorOid( Guid oidTarefa , params Expression<Func<Tarefa,object>>[] includes)
        {
            if( oidTarefa == null)
                throw new ArgumentException( "O parâmetro oidTarefa não pode ser nulo." );
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.Tarefa.MultiploInclude(includes).FirstOrDefault( o => o.Oid.Equals( oidTarefa ) && !o.CsExcluido );
            }
        }

        /// <summary>
        /// Método responsável por buscar uma tarefa pelo Oid
        /// </summary>
        /// <param name="oidTarefa">Oid da tarefa</param>
        /// <returns>Objeto Tarefa</returns>
        public static Tarefa ConsultarTarefaPorOid(WexDb contexto, Guid oidTarefa, params Expression<Func<Tarefa, object>>[] includes )
        {
            if(oidTarefa == null)
                throw new ArgumentException( "O parâmetro oidTarefa não pode ser nulo." );
                return contexto.Tarefa.MultiploInclude( includes ).FirstOrDefault( o => o.Oid.Equals( oidTarefa ) && !o.CsExcluido );
        }


        #endregion

        #region Salvar

        /// <summary>
        /// Método responsável por salvar uma tarefa
        /// </summary>
        /// <param name="tarefa">Objeto Tarefa</param>
        /// <returns>retorna o objeto salvo</returns>
        public static Tarefa SalvarTarefa( Tarefa tarefa )
        {
            Tarefa copia = tarefa.Clone();
            copia.AtualizadoPor = null;
            copia.LogsAlteracao = null;
            copia.TarefaResponsaveis = null;
            copia.TarefaHistoricoTrabalhos = null;
            copia.SituacaoPlanejamento = null;
            copia.clone = null;

            try
            {
                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
                {
                    if(contexto.Tarefa.Existe( o => o.Oid == tarefa.Oid ))
                    {
                        contexto.Tarefa.Attach( copia );
                        contexto.Entry( copia ).State = EntityState.Modified;
                    }
                    else
                    {
                        contexto.Tarefa.Add( copia );
                    }

                    contexto.SaveChanges();
                }
            }
            catch(Exception)
            {
                try
                {
                    using(WexDb contexto = ContextFactoryManager.CriarWexDb())
                    {
                        if(contexto.Tarefa.Existe( o => o.Oid == tarefa.Oid ))
                        {
                            contexto.Tarefa.Attach( copia );
                            contexto.Entry( copia ).State = EntityState.Modified;
                        }
                        else
                        {
                            contexto.Tarefa.Add( copia );
                        }

                        contexto.SaveChanges();
                    }
                }
                catch(Exception)
                {
                    using(WexDb contexto = ContextFactoryManager.CriarWexDb())
                    {
                        if(contexto.Tarefa.Existe( o => o.Oid == tarefa.Oid ))
                        {
                            contexto.Tarefa.Attach( copia );
                            contexto.Entry( copia ).Reload();
                            contexto.Tarefa.Attach( copia );
                            contexto.Entry( copia ).State = EntityState.Modified;
                        }
                        else
                        {
                            contexto.Tarefa.Add( copia );
                        }

                        contexto.SaveChanges();
                    }
                }
            }
            
            return tarefa;
        }
        
        #endregion

        #region Excluir

        /// <summary>
        /// Método responsável por realizar a exclusão de um objeto Tarefa
        /// obs: caso ele não consiga, ele atualiza o objeto (caso tenha sido alterado por outra thread) e realiza novamente a tentativa de exclusão.
        /// </summary>
        /// <param name="oidTarefa">objeto para excluir</param>
        public static bool ExcluirTarefa( Guid oidTarefa )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                Tarefa tarefaExistente = ConsultarTarefaPorOid( oidTarefa );
                if(tarefaExistente == null)
                    return false;

                tarefaExistente.CsExcluido = true;
                contexto.Tarefa.Attach( tarefaExistente );
                contexto.Entry( tarefaExistente ).State = EntityState.Modified;
                contexto.SaveChanges();
                return true;
            }
        }
        #endregion
    }
}
