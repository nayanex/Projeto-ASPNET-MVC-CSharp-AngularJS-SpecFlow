using System;
using System.Collections.Generic;
using System.Text;

namespace WexProject.Library.Libs.DataHora
{
    /// <summary>
    /// Classe utilitária com funções para Data e Hora.
    /// </summary>
    public class DateUtil
    {
        /// <summary>
        /// Utilizado no cenário de testes para fornecer uma data atual do sistema
        /// apropriada para os testes.
        /// </summary>
        private static DateTime currentDateTime = DateTime.MinValue;
        /// <summary>
        /// Atributo utilizado pelos cenarios de teste
        /// para fornecer a data e hora do sistema.
        /// </summary>
        public static DateTime CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }
            set
            {
                currentDateTime = value;
            }
        }

        /// <summary>
        /// Retorna a primeira ocorrênci do dia da semana
        /// passado como parâmetro.
        /// </summary>
        /// <param name="targetDayOfWeek">Dia da semana para qual trará a data</param>
        /// <param name="startDate">A partir de qual data</param>
        /// <returns>Próxima data no dia da semana especificado</returns>
        public static DateTime ConsultarDataDoProximoDiaDaSemana( DayOfWeek targetDayOfWeek, DateTime startDate )
        {
            while(startDate.DayOfWeek != targetDayOfWeek)
                startDate = startDate.AddDays( 1 );

            return startDate.Date;
        }

        /// <summary>
        /// Retorna a primeira ocorrênci do dia da semana
        /// passado como parâmetro.
        /// </summary>
        /// <param name="targetDayOfWeek">Dia da semana para qual trará a data</param>
        /// <returns>Próxima data no dia da semana especificado</returns>
        public static DateTime ConsultarNextDayOfWeeok( DayOfWeek targetDayOfWeek )
        {
            return ConsultarDataDoProximoDiaDaSemana( targetDayOfWeek, ConsultarDataHoraAtual() );
        }

        /// <summary>
        /// retorna o proximo DayOfWeek
        /// </summary>
        /// <param name="diaSemana"> dia da semnada</param>
        /// <returns></returns>
        public static DayOfWeek CalcularProximoDiaSemana( DayOfWeek diaSemana )
        {
            int valor = (int)diaSemana;
            valor++;
            if(valor > 6)
                valor = 0;
            return (DayOfWeek)valor;
        }

        /// <summary>
        /// Retorna a data e hora atuais do sistema.
        /// </summary>
        /// <returns>Retorna o usuário da sessão atual</returns>
        public static DateTime ConsultarDataHoraAtual()
        {
            if(currentDateTime == DateTime.MinValue)
                return DateTime.Now;
            else
                return CurrentDateTime;
        }

        /// <summary>
        /// Retorna o último dia do mês seguinte ao corrente.
        /// </summary>
        /// <returns>Data</returns>
        public static DateTime ConsultarUltimoDiaDoProximoMes()
        {
            DateTime lastDay;
            DateTime date = ConsultarDataHoraAtual();
            date = date.AddMonths( 1 );
            //tenta criar a data de proximo
            try
            {
                lastDay = new DateTime( date.Year, date.Month, 31 );
            } //meses de 31 dias
            catch
            {
                try
                {
                    lastDay = new DateTime( date.Year, date.Month, 30 );
                } //meses de 30 dias 
                catch
                {
                    try
                    {
                        lastDay = new DateTime( date.Year, date.Month, 29 );
                    } //fevereiro - bissexto
                    catch
                    {
                        lastDay = new DateTime( date.Year, date.Month, 28 );
                    } //fevereiro
                }
            }

            return lastDay;
        }

        /// <summary>
        /// Retorna o primeiro dia do mês seguinte ao corrente
        /// </summary>
        /// <returns>Data</returns>
        public static DateTime ConsultarPrimeiroDiaDoProximoMes()
        {
            DateTime date = ConsultarDataHoraAtual();
            date = date.AddMonths( 1 );
            return new DateTime( date.Year, date.Month, 01 );
        }
    }
}
