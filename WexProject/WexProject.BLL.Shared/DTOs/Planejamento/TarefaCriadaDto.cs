using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class TarefaCriadaDto
    {
        #region Propriedades
        
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

        /// <summary>
        /// Oid de cronogramaTarefa que foi criado
        /// </summary>
        [DataMember]
        public Guid OidCronogramaTarefa
        {
            get;
            set;
        }

        /// <summary>
        /// Oid do cronograma
        /// </summary>
        [DataMember]
        public Guid OidCronograma
        {
            get;
            set;
        }

        /// <summary>
        /// Oid da tarefa que foi criada
        /// </summary>
        [DataMember]
        public Guid OidTarefa
        {
            get;
            set;
        }

        /// <summary>
        /// Atributo que guarda o id real gerado para a tarefa que foi criada
        /// </summary>
        [DataMember]
        public Int16 NbIdTarefa
        {
            get;
            set;
        }

        [DataMember]
        public DateTime DtAtualizadoEm
        {
            get;
            set;
        }

        [DataMember]
        public string TxAtualizadoPor
        {
            get;
            set;
        }

        [DataMember]
        public DateTime dataHoraAcao
        {
            get;
            set;
        }

        [DataMember]
        public string TxDescricaoTarefa
        {
            get;
            set;
        }

        [DataMember]
        public Int16 NbEstimativaInicial
        {
            get;
            set;
        }

        [DataMember]
        public string TxObservacaoTarefa
        {
            get;
            set;
        }

        [DataMember]
        public DateTime DtInicio
        {
            get;
            set;
        }

        [DataMember]
        public string TxDescricaoColaborador
        {
            get;
            set;
        }

        [DataMember]
        public bool CsLinhaBaseSalva
        {
            get;
            set;
        }

        [DataMember]
        public Guid OidSituacaoPlanejamentoTarefa
        {
            get;
            set;
        }

        [DataMember]
        public string TxDescricaoSituacaoPlanejamentoTarefa
        {
            get;
            set;
        }

        [DataMember]
        public double NbEstimativaRestante
        {
            get;
            set;
        }

        [DataMember]
        public double NbRealizado
        {
            get;
            set;
        }
        #endregion
    }
}
