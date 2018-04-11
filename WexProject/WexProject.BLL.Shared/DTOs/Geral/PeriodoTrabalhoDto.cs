using System;
using System.Collections.Generic;
using System.Linq;

namespace WexProject.BLL.Shared.DTOs.Geral
{
    public class PeriodoTrabalhoDto
    {
        #region Atributos

        /// <summary>
        /// responsável por armazenar a hora inicial de uma periodo de trabalho
        /// </summary>
        public string HoraInicial { get; set; }
        

        /// <summary>
        /// responsável por armazenar a horas final de um periodo de trabalho
        /// </summary>
        public string HoraFinal { get; set; }

        #endregion

        #region Construtor

        public PeriodoTrabalhoDto()
        {
        }

        /// <summary>
        /// Construtor do periodo de trabalho armazenando a hora inicial e final de um periodo de trabalho
        /// </summary>
        /// <param name="inicio">início do período de expediente</param>
        /// <param name="final">fim do período de expediente</param>
        public PeriodoTrabalhoDto( string inicio, string final ) 
        {
            HoraInicial = inicio;
            HoraFinal = final;
        }

        #endregion
    }
}
