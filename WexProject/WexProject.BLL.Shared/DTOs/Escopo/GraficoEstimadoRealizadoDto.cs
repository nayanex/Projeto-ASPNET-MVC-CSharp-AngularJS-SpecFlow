using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WexProject.BLL.Shared.DTOs.Escopo
{
    /// <summary>
    /// Classe com os dados necessários para montar o gráfico Estimado vs Realizado
    /// </summary>
    [DataContract]
    public class GraficoEstimadoRealizadoDTO
    {
        /// <summary>
        /// Guid do Projeto
        /// </summary>
        [DataMember]
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
        /// Valor de pontos estimados no Ciclo atual
        /// </summary>
        [DataMember]
        public double Estimado
        {
            get;
            set;
        }

        /// <summary>
        /// Valor de pontos realizados no Ciclo atual
        /// </summary>
        [DataMember]
        public Double? Realizado
        {
            get;
            set;
        }

        /// <summary>
        /// Valor da tendência no Ciclo atual
        /// </summary>
        [DataMember]
        public Double? Tendencia
        {
            get;
            set;
        }

        
        /// <summary>
        /// Valor da tendência no Ciclo atual
        /// </summary>
        [DataMember]
        public Double? RitimoSugerido
        {
            get;
            set;
        }
    }
}