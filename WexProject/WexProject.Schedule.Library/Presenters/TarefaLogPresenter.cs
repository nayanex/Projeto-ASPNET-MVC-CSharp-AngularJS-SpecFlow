using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.Schedule.Library.Views.Forms;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.ServiceUtils;

namespace WexProject.Schedule.Library.Presenters
{
    /// <summary>
    /// Classe presenter de gestão de TarefaLogView
    /// </summary>
    public class TarefaLogPresenter
    {
        /// <summary>
        /// Criar a instância do presenter para a view de TarefaLog
        /// </summary>
        /// <param name="view">view</param>
        /// <param name="oidCronogramaTarefa"> oid da tarefa selecionada</param>
        public TarefaLogPresenter(TarefaLogView view,Guid oidCronogramaTarefa)
        {
            CarregarHistoricoAlteracoesTarefa( view,oidCronogramaTarefa );
        }

        /// <summary>
        /// Método para validar se a tarefa ainda não foi salva
        /// </summary>
        /// <param name="oidTarefa">oid da tarefa selecionada</param>
        /// <returns></returns>
        private static bool ValidarTarefa(Guid oidTarefa) 
        {
            return new Guid() != oidTarefa;
        }

        /// <summary>
        /// Método para carregar
        /// </summary>
        /// <param name="view"></param>
        /// <param name="oidTarefa"></param>
        private static void CarregarHistoricoAlteracoesTarefa(TarefaLogView view,Guid oidTarefa) 
        {
            if(ValidarTarefa( oidTarefa ))
            {
                List<TarefaLogAlteracaoDto> alteracoesEfetuadas = CronogramaPresenter.ServicoPlanejamento.ConsultarTarefaLogAlteracaoPorOid( oidTarefa.ToString() );
                if(alteracoesEfetuadas != null && alteracoesEfetuadas.Count > 0)
                {
                    view.ListarAlteracoes( alteracoesEfetuadas );
                }
                else
                {
                    throw new Exception( "A tarefa selecionada não possui nenhum histórico de atualização" );
                }
            }
            else
            {
                throw new Exception( "A tarefa atual ainda não é uma tarefa salva!" );
            }
        }
    }
}
