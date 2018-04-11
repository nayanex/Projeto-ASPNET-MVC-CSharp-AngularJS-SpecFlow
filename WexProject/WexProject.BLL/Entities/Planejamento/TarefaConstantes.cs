using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Entities.Planejamento
{
    /// <summary>
    /// Classe Criada para padronizar as contantes que serão indices das propriedades armazenadas em hashtables presentes em Dtos
    /// </summary>
    public static class TarefaConstantes
    {
        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar uma coleção de tarefas impactadas
        /// </summary>
        public const string TAREFAS_IMPACTADAS = "tarefasImpactadas";
        /// <summary>
        /// 
        /// </summary>
        public const string TAREFAS_EXCLUIDAS = "tarefasExcluidas";
        /// <summary>
        /// Armazena uma string indice na hashtable propriedades, responsável por representar um dicionário de tarefas reordenadas 
        /// como chave o oidTarefa e como valor o nbId da tarefa atual.
        /// </summary>
        public const string TAREFAS_REORDENADAS = "tarefasReordenadas";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o Oid de uma tarefaa atual.
        /// </summary>
        public const string OIDTAREFA = "oidTarefa";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar um vetor de oidTarefas
        /// </summary>
        public const string TAREFAS = "tarefas";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar um vetor de oidTarefas
        /// </summary>
        public const string TAREFAS_NAO_EXCLUIDAS = "tarefasNaoExcluidas";
    }
}
