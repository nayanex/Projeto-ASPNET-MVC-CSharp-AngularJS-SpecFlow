using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    /// <summary>
    /// Classe pare gerenciamento de atualização de tarefas impactadas
    /// </summary>
    public class GerenciadorTarefasImpactadas
    {
        /// <summary>
        /// Responsável por armazenar a ultima hora de atualização das tarefas
        /// </summary>
        Dictionary<string, DateTime> tarefasAtualizadas;

        /// <summary>
        /// Responsável por publicar para testes as atualizações ocorridas
        /// </summary>
        public Dictionary<string, DateTime> TarefasAtualizadas
        {
            get { return tarefasAtualizadas; }
        }

        public GerenciadorTarefasImpactadas() 
        {
            tarefasAtualizadas = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// Método responsável por listar as atualizações válidas retornando apenas as mais atuais para cada tarefa
        /// </summary>
        /// <param name="impactadas">lista de tarefas impactadas</param>
        /// <param name="dataAtualizacao">data de atualização</param>
        /// <returns></returns>
        public Dictionary<string, Int16> ListarAtualizacoesValidas( Dictionary<string, Int16> impactadas, DateTime dataAtualizacao ) 
        {
            Dictionary<string, Int16> impactadasAtualizadas = new Dictionary<string, short>();
            lock(tarefasAtualizadas)
            {
                foreach(var item in impactadas)
                {
                    if(AplicarDataAtualizacao( item.Key, dataAtualizacao )) 
                    {
                        impactadasAtualizadas.Add( item.Key, item.Value );
                    }
                }
            }
            return impactadasAtualizadas;
        }

        /// <summary>
        /// método responsável por aplicar a data de modificação na tarefa caso seja mais atualizada
        /// </summary>
        /// <param name="oidCronogramaTarefa">oid da tarefa a ser atualizada</param>
        /// <param name="dataModificacao"></param>
        /// <returns></returns>
        public bool AplicarDataAtualizacao( Guid oidCronogramaTarefa, DateTime dataModificacao )
        {
            return AplicarDataAtualizacao( oidCronogramaTarefa.ToString(), dataModificacao );
        }

        /// <summary>
        /// método responsável por aplicar a data de modificação na tarefa caso seja mais atualizada
        /// </summary>
        /// <param name="oidCronogramaTarefa">oid da tarefa a ser atualizada</param>
        /// <param name="dataModificacao"></param>
        /// <returns></returns>
        public bool AplicarDataAtualizacao( string oidCronogramaTarefa, DateTime dataModificacao )
        {
            if(!tarefasAtualizadas.ContainsKey( oidCronogramaTarefa ))
            {
                tarefasAtualizadas.Add( oidCronogramaTarefa, dataModificacao );
                return true;
            }
            else
            {
                if(dataModificacao.Ticks >= tarefasAtualizadas[oidCronogramaTarefa].Ticks)
                {
                    tarefasAtualizadas[oidCronogramaTarefa] = dataModificacao;
                    return  true;
                }
            }
            return false;
        }
    }
}
