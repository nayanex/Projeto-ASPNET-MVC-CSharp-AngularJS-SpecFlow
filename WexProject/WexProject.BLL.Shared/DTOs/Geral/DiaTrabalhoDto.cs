using System;
using System.Collections.Generic;
using System.Linq;

namespace WexProject.BLL.Shared.DTOs.Geral
{
    /// <summary>
    /// Classe para tranporte da dados Referente aos dias de trabalho e seus periodos de trabalho para o dia atual
    /// </summary>
    public class DiaTrabalhoDto
    {
        #region Atributos


        /// <summary>
        /// Dia Atual da Semana
        /// </summary>
        public DayOfWeek DiaSemana { get; set; }

        /// <summary>
        /// Periodos de trabalho para o dia atual da semana
        /// </summary>
        public List<PeriodoTrabalhoDto> PeriodosTrabalho { get; set; }


        #endregion
    }
}
