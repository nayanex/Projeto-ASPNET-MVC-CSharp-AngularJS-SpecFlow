using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class CronogramaDto
    {
        #region Propriedades

        /// <summary>
        /// Oid do Cronograma
        /// </summary>
        [DataMember]
        public Guid Oid
        {
            get;
            set;
        }

        /// <summary>
        /// Nome do Cronograma
        /// </summary>
        [DataMember]
        public string TxDescricao
        {
            get;
            set;
        }

        /// <summary>
        /// Oid da Situação Planejamento do Cronograma
        /// </summary>
        [DataMember]
        public Guid OidSituacaoPlanejamento
        {
            get;
            set;
        }

        /// <summary>
        /// Nome da Situação Planejamento do Cronograma
        /// </summary>
        [DataMember]
        public string TxDescricaoSituacaoPlanejamento
        {
            get;
            set;
        }

        /// <summary>
        /// Data Início do Cronograma
        /// </summary>
        [DataMember]
        public DateTime DtInicio
        {
            get;
            set;
        }

        /// <summary>
        /// Data Final do Cronograma
        /// </summary>
        [DataMember]
        public DateTime DtFinal
        {
            get;
            set;
        }

        #endregion
    }
}
