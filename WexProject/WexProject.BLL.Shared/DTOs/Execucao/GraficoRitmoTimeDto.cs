using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Execucao
{
    /// <summary>
    /// Classe com os dados necessários para montar o gráfico de Ritmo do Time
    /// </summary>
    [DataContract]
    public class GraficoRitmoTimeDTO
    {
        /// <summary>
        /// Guid do Projeto
        /// </summary>        
       
        public Guid ProjetoOid
        {
            get;
            set;
        }

        /// <summary>
        /// Ciclo atual
        /// </summary>
        [DataMember]
        public int Ciclo
        {
            get;
            set;
        }

        /// <summary>
        /// Valor do ritmo do time no Ciclo atual
        /// </summary>
        [DataMember]
        public double? Ritmo
        {
            get;
            set;
        }

        /// <summary>
        /// Meta do time.
        /// </summary>
        [DataMember]
        public double? Meta
        {
            get;
            set;
        }


        /// <summary>
        /// Meta do time.
        /// </summary>
        [DataMember]
        public double? Planejado
        {
            get;
            set;
        }
    }
}