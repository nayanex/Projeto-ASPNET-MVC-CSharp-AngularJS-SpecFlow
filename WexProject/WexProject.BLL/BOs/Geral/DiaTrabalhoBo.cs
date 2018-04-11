using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.BOs.Geral
{
    public class DiaTrabalhoBo
    {
        /// <summary>
        /// responsável por adicionar um período de trabalho ao dia de trabalho
        /// </summary>
        /// <param name="inicial">hora inicial do período de trabalho</param>
        /// <param name="final">hora final do período de trabalho</param>
        public static void AdicionarPeriodoDeTrabalho( string inicial, string final, DiaTrabalho diaTrabalho )
        {
            PeriodoTrabalho periodo = new PeriodoTrabalho( inicial, final );
            diaTrabalho.periodosTrabalho.Add( periodo );
        }


        #region Factories

        /// <summary>
        /// Método responsável por efetuar a transposição da classe DiaTrabalho para seu respectivo Dto
        /// </summary>
        /// <param name="dia">dia atual com os periodos de trabalho cadastrados</param>
        /// <returns></returns>
        public static DiaTrabalhoDto DtoFactory( DiaTrabalho diaTrabalho )
        {
            List<PeriodoTrabalhoDto> periodos = diaTrabalho.periodosTrabalho.Select( o => PeriodoTrabalhoBo.DtoFactory( o.HoraInicial, o.HoraFinal ) ).ToList();
            return new DiaTrabalhoDto() { DiaSemana = diaTrabalho.DiaDaSemana, PeriodosTrabalho = periodos };
        }

        #endregion
    }
}
