using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Entities.Geral
{
    /// <summary>
    /// Responsável por armazenar a rotina diária de trabalho semanal de um colaborador
    /// </summary>
    public class SemanaTrabalho
    {
        #region Atributos

        /// <summary>
        /// responsável por armazenar os dias de trabalho semanais
        /// </summary>
        public List<DiaTrabalho> diasTrabalho;

        #endregion

        #region Construtor

        public SemanaTrabalho()
        {
            diasTrabalho = new List<DiaTrabalho>();
        }

        #endregion
    }
}
