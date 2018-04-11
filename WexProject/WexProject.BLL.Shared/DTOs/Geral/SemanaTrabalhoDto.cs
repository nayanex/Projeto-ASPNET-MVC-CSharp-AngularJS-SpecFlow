using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Shared.DTOs.Geral
{
    /// <summary>
    /// Responsável por armazenar os dias de trabalho padrão semanais, tais como o(s) período(s) diário(s) de trabalho
    /// </summary>
    public class SemanaTrabalhoDto
    {
        #region Propriedades

        /// <summary>
        /// responsável por armazenar os dias de trabalho semanais
        /// </summary>
        public List<DiaTrabalhoDto> diasTrabalho { get; set; } 
        #endregion
    }
}
