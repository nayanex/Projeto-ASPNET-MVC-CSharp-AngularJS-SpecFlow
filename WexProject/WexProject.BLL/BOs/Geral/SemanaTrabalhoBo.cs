using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.BOs.Geral
{
    public class SemanaTrabalhoBo
    {
        /// <summary>
        /// Responsável por armazenar um dia de trabalho
        /// </summary>
        public static DiaTrabalho AdicionarDiaDeTrabalho( SemanaTrabalho semanaTrabalho, DayOfWeek dia )
        {
            if(semanaTrabalho.diasTrabalho == null)
                return null;

            DiaTrabalho diaTrabalho = semanaTrabalho.diasTrabalho.FirstOrDefault( o => o.DiaDaSemana.Equals( dia ) );
            if(diaTrabalho == null)
            {
                diaTrabalho = new DiaTrabalho( dia );
                semanaTrabalho.diasTrabalho.Add( diaTrabalho );
            }
            return diaTrabalho;
        }

        /// <summary>
        /// responsável por armazenar um período de trabalho à um dia de trabalho
        /// </summary>
        /// <param name="dia">dia da semana</param>
        /// <param name="inicioPeriodo">inicio de periodo de trabalho</param>
        /// <param name="fimPeriodo">fim do periodo de trabalho</param>
        public static void AdicionarPeriodoDeTrabalho( SemanaTrabalho semanaTrabalho, DayOfWeek dia, string inicioPeriodo, string fimPeriodo )
        {
            if(semanaTrabalho.diasTrabalho == null)
                return;

            DiaTrabalho diaTrabalho = ( semanaTrabalho.diasTrabalho.FirstOrDefault( o => o.DiaDaSemana.Equals( dia ) ) );
            if(diaTrabalho == null)
                diaTrabalho = AdicionarDiaDeTrabalho( semanaTrabalho, dia );

            DiaTrabalhoBo.AdicionarPeriodoDeTrabalho( inicioPeriodo, fimPeriodo, diaTrabalho );
        }

        /// <summary>
        /// Método responsável por gerar uma semana de trabalho padrão
        /// de Segundo a Sexta com períodos de trabalho de 8:00 as 12:00 e 13:00 as 18:00
        /// </summary>
        /// <returns>Semana de trabalho com dias de trabalho e periodos de trabalho predefinidos no método</returns>
        public static List<DiaTrabalho> GerarSemanaTrabalhoPadrao( SemanaTrabalho semanaTrabalho )
        {
            semanaTrabalho.diasTrabalho.Clear();
            DiaTrabalho diaAtual;
            foreach(DayOfWeek diaSemana in Enum.GetValues( typeof( DayOfWeek ) ))
            {
                switch(diaSemana)
                {
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Saturday:
                        break;
                    default:
                        diaAtual = new DiaTrabalho( (DayOfWeek)diaSemana );
                        DiaTrabalhoBo.AdicionarPeriodoDeTrabalho( "8:00", "12:00", diaAtual );
                        DiaTrabalhoBo.AdicionarPeriodoDeTrabalho( "13:00", "18:00", diaAtual );
                        semanaTrabalho.diasTrabalho.Add( diaAtual );
                        break;
                }
            }

            return semanaTrabalho.diasTrabalho;
        }

        /// <summary>
        /// Método responsável por verificar se existe periodo de trabalho para o dia atual
        /// </summary>
        /// <param name="diaSemana">dia da semana (DataSelecionada.DayOfWeek)</param>
        /// <param name="diasTrabalho">lista de dias da semana</param>
        /// <returns></returns>
        public static bool DiaAtualPossuiPeriodoTrabalho( SemanaTrabalho semanaTrabalho, DayOfWeek diaSemana )
        {
            //Caso seja nulo, não possua periodos ou não contenha o indice do dia da semana
            if(semanaTrabalho.diasTrabalho == null || semanaTrabalho.diasTrabalho.Count == 0)
                return false;

            DiaTrabalho dia = semanaTrabalho.diasTrabalho.FirstOrDefault( o => o.DiaDaSemana.Equals( diaSemana ) );
            if(dia == null)
                return false;

            return dia.PeriodosDeTrabalho != null && dia.PeriodosDeTrabalho.Count > 0;
        }

        #region Factories

        /// <summary>
        /// Criar Dto da classe
        /// </summary>
        /// <returns></returns>
        public static SemanaTrabalhoDto DtoFactory( SemanaTrabalho semanaTrabalho )
        {
            //TODO: Refatorar quando Classe passar a ser persistente
            semanaTrabalho = SemanaTrabalhoBo.SemanaTrabalhoPadraoFactory();
            List<DiaTrabalhoDto> diasTrabalho = new List<DiaTrabalhoDto>( semanaTrabalho.diasTrabalho.Select( o => DiaTrabalhoBo.DtoFactory( o ) ) );
            return new SemanaTrabalhoDto() { diasTrabalho = diasTrabalho };
        }

        /// <summary>
        /// Retornar uma semana de trabalho Padrao
        /// </summary>
        /// <returns></returns>
        public static SemanaTrabalho SemanaTrabalhoPadraoFactory()
        {
            SemanaTrabalho semanaTrabalho = new SemanaTrabalho();
            SemanaTrabalhoBo.GerarSemanaTrabalhoPadrao( semanaTrabalho );

            return semanaTrabalho;
        }


        #endregion
    }
}
