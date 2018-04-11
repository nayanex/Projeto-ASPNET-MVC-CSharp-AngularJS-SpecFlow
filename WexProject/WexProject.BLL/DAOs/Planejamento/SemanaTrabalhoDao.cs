using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class SemanaTrabalhoDao
    {
        /// <summary>
        /// Método responsável por gerar uma semana de trabalho padrão
        /// de Segundo a Sexta com períodos de trabalho de 8:00 as 12:00 e 13:00 as 18:00
        /// </summary>
        /// <returns>Semana de trabalho com dias de trabalho e periodos de trabalho predefinidos no método</returns>
        public static List<DiaTrabalho> SelecionarSemanaTrabalhoPadrao( SemanaTrabalho semanaTrabalho )
        {
            return semanaTrabalho.diasTrabalho;
        }

        /// <summary>
        /// Retornar um dia de trabalho a partir do dia da semana
        /// </summary>
        /// <param name="diaSemana"></param>
        /// <returns></returns>
        public static DiaTrabalho SelecionarDiaTrabalho( SemanaTrabalho semanaTrabalho, DayOfWeek diaSemana )
        {
            if(semanaTrabalho.diasTrabalho == null)
                return null;
            return semanaTrabalho.diasTrabalho.FirstOrDefault( o => o.DiaDaSemana.Equals( diaSemana ) );
        }
    }
}
