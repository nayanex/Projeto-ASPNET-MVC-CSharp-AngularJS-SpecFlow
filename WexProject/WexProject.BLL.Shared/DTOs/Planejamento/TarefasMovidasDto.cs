using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class TarefasMovidasDto
    {
        /// <summary>
        /// Atributo que é um dicionário que guarda os Oids das tarefas e os novos IDs das tarefas
        /// que foram impactadas
        /// </summary>
        //Dicionário:
        //Key: Oid da Tarefa
        //Value: Novo Id da tarefa
        [DataMember]
        public Dictionary<string, Int16> TarefasImpactadas
        {
            get;
            set;
        }

        [DataMember]
        public Guid OidCronogramaTarefaMovida
        {
            get;
            set;
        }

        [DataMember]
        public Guid OidCronograma
        {
            get;
            set;
        }

        [DataMember]
        public short NbIDTarefaMovida
        {
            get;
            set;
        }

        [DataMember]
        public DateTime DataHoraAcao
        {
            get;
            set;
        }
    }
}
