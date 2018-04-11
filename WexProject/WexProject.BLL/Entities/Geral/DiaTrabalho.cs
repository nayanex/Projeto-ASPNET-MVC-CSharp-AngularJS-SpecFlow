using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Entities.Geral
{
    /// <summary>
    /// classe representa um dia da semana de trabalho, armazenando os períodos de trabalho para a rotina deste dia na semana
    /// </summary>
    public class DiaTrabalho
    {
        #region Atributos

        /// <summary>
        /// responsável por armazenar o dia da semana que será representado
        /// </summary>
        public DayOfWeek diaDaSemana;

        /// <summary>
        /// representar os turnos de trabalho para este dia de trabalho
        /// </summary>
        public List<PeriodoTrabalho> periodosTrabalho;

        #endregion

        #region Propriedades
        /// <summary>
        /// Propriedade Somente Leitura armazenando o dia da semana Atual
        /// </summary>
        public DayOfWeek DiaDaSemana { get { return diaDaSemana; } }

        /// <summary>
        /// Propriedade Somente Leitura armazendando os Períodos de trabalho do dia atual
        /// </summary>
        public List<PeriodoTrabalho> PeriodosDeTrabalho { get { return periodosTrabalho; } set { periodosTrabalho = value; } }

        #endregion

        #region Construtores
        /// <summary>
        /// construtor armazenando do dia da semana a ser representado
        /// </summary>
        /// <param name="dia">dia da semana</param>
        public DiaTrabalho( DayOfWeek dia )
        {
            diaDaSemana = dia;
            periodosTrabalho = new List<PeriodoTrabalho>();
        }
        #endregion
    }
}
