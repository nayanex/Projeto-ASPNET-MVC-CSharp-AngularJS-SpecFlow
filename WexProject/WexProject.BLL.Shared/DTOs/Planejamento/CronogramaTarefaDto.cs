using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class CronogramaTarefaDto
    {
        #region Propriedades

        [DataMember]
        public Guid OidCronogramaTarefa
        {
            get;
            set;
        }

        [DataMember]
        public Int16? NbID
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
        public Guid OidTarefa
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
        public DateTime DtHoraConsulta
        {
            get;
            set;
        }

        #endregion
    }
}
