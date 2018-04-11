using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Entities.Geral
{
    public class PeriodoTrabalho
    {
        #region Atributos

        /// <summary>
        /// responsável por armazenar a hora inicial de uma periodo de trabalho
        /// </summary>
        private string horaInicial;

        /// <summary>
        /// responsável por armazenar a horas final de um periodo de trabalho
        /// </summary>
        private string horaFinal;

        #endregion

        #region Propriedades


        /// <summary>
        /// responsável por armazenar a hora inicial de uma periodo de trabalho
        /// </summary>
        public string HoraFinal
        {
            get { return horaFinal; }
            set { horaFinal = value; }
        }

        /// <summary>
        /// responsável por armazenar a horas final de um periodo de trabalho
        /// </summary>
        public string HoraInicial
        {
            get { return horaInicial; }
            set { horaInicial = value; }
        }

        #endregion

        #region Construtor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inicial"></param>
        /// <param name="final"></param>
        public PeriodoTrabalho( string inicial, string final )
        {
            horaInicial = inicial;
            horaFinal = final;
        }

        #endregion


    }
}
