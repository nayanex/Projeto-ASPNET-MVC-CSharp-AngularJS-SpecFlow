using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using System.Data.Entity;
using WexProject.BLL.Contexto;
using WexProject.BLL.Extensions.Entities;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class TarefaLogAlteracaoDao
    {
        #region Consultar

        /// <summary>
        /// Retornar uma lista de alterações de uma tarefa baseada no guid da tarefa ( cast em string)
        /// </summary>
        /// <param name="sessao">Sessao Atual do Banco</param>
        /// <param name="oidTarefa">oid da tarefa selecionada</param>
        /// <returns>lista de alterações ocorridas na tarefa</returns>
        public static List<TarefaLogAlteracao> ConsultarAlteracoesTarefaPorOid( WexDb contexto, Guid oidTarefa )
        {
            return contexto.TarefaLogAlteracao.Include( o => o.Colaborador.Usuario.Person ).Include( o => o.Tarefa ).Where( o => o.Tarefa.Oid == oidTarefa ).ToList();
        }

        #endregion

        #region Salvar

        /// <summary>
        /// Método responsável por salvar um Log de alteração
        /// </summary>
        /// <param name="tarefaLogAlteracao">Objeto que será salvo</param>
        public static void Salvar( TarefaLogAlteracao tarefaLogAlteracao )
        {
            TarefaLogAlteracao copia = tarefaLogAlteracao.Clone();
            copia.Colaborador = null;
            copia.Tarefa = null;

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                try
                {
                    if(contexto.TarefaLogAlteracao.Existe( o => o.Oid == copia.Oid ))
                    {
                        contexto.TarefaLogAlteracao.Attach( copia );
                        contexto.Entry<TarefaLogAlteracao>( copia ).State = System.Data.EntityState.Modified;
                    }
                    else
                    {
                        contexto.TarefaLogAlteracao.Add( copia );
                    }
                    
                    contexto.SaveChanges();
                }
                catch(Exception)
                {
                    if(contexto.TarefaLogAlteracao.Existe( o => o.Oid == copia.Oid ))
                    {
                        contexto.Entry<TarefaLogAlteracao>( copia ).Reload();
                        contexto.TarefaLogAlteracao.Attach( copia );
                        contexto.Entry<TarefaLogAlteracao>( copia ).State = System.Data.EntityState.Modified;
                    }
                    else
                    {
                        contexto.TarefaLogAlteracao.Add( copia );
                    }
                    
                    contexto.SaveChanges();
                }
            }
        }

        #endregion
    }
}
