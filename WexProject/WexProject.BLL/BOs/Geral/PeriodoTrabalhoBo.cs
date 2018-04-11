using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.BOs.Geral
{
    public class PeriodoTrabalhoBo
    {
        #region Factories


        /// <summary>
        /// Método reponsável por Converter um período de PeriodoDeTrabalho para PeriodoTrabalhoDto
        /// </summary>
        /// <param name="periodo">C</param>
        public static PeriodoTrabalhoDto DtoFactory( string horaInicial, string horaFinal )
        {
            return new PeriodoTrabalhoDto( horaInicial, horaFinal );
        }


        #endregion
    }
}
