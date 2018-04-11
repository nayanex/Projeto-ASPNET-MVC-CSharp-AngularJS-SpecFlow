using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Libs.DataHora.Extension
{
    public static class TimeUtilExtension
    {

        /// <summary>
        /// Converte um valor double para TimeSpan
        /// </summary>
        /// <param name="valor">valor double a ser convertido</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan( this double valor )
        {
            return new TimeSpan( (long)valor );
        }

        /// <summary>
        /// Converte uma valor inteiro pra horas em TimeSpan
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static TimeSpan ToHoursTimeSpan( this short valor ) 
        {
            return TimeSpan.FromHours( (int)valor );
        }

        /// <summary>
        /// Converte uma valor inteiro pra horas em TimeSpan
        /// </summary>
        /// <param name="valor">Valor horas inteiras</param>
        /// <returns></returns>
        public static double ToTicks( this short valor )
        {
            return TimeSpan.FromHours( valor ).Ticks;
        }

        #region Inteiro para TimeSpan
        /// <summary>
        /// Converte uma valor inteiro pra horas em TimeSpan
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static TimeSpan ToHoursTimeSpan( this int valor )
        {
            return TimeSpan.FromHours( valor );
        }

        /// <summary>
        /// Converte uma valor inteiro pra horas em TimeSpan
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static double HorasParaTicks( this int valor )
        {
            return  valor * TimeSpan.TicksPerHour;
        }

        /// <summary>
        /// Converte uma valor inteiro pra horas em TimeSpan
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static double HorasParaTicks( this short valor )
        {
            return valor * TimeSpan.TicksPerHour;
        }

        /// <summary>
        /// Converte uma valor inteiro pra horas em TimeSpan
        /// </summary>
        /// <param name="valor">valor em minutos</param>
        /// <returns></returns>
        public static TimeSpan ToMinutesTimeSpan( this int valor )
        {
            return TimeSpan.FromMinutes( valor );
        }

        /// <summary>
        /// Converte uma valor inteiro pra segundos em TimeSpan
        /// </summary>
        /// <param name="valor">valor em segundos</param>
        /// <returns></returns>
        public static TimeSpan ToSecondsTimeSpan( this int valor )
        {
            return TimeSpan.FromSeconds( valor );
        }
        #endregion


        #region Valores por extenso

        /// <summary>
        /// Converter o TimeSpan para o formato por extenso usado no cronograma
        /// </summary>
        /// <param name="tempo"></param>
        /// <returns></returns>
        public static string PorExtenso( this TimeSpan tempo )
        {
            if(tempo.Ticks < TimeSpan.TicksPerMinute)
                return "0";
            StringBuilder builder = new StringBuilder( "" );
            if(tempo.Days > 0)
                builder.AppendFormat( "{0}d ", tempo.Days );

            if(tempo.Hours > 0)
                builder.AppendFormat( "{0}h ", tempo.Hours );

            if(tempo.Minutes > 0)
                builder.AppendFormat( "{0}min", tempo.Minutes );

            return builder.ToString().Trim();
        }

        /// <summary>
        /// Método para converter horas inteira para o formta por extenso usado no cronograma
        /// </summary>
        /// <param name="horasInteiras"></param>
        /// <returns></returns>
        public static string HorasPorExtenso( this int horasInteiras )
        {
            return ( (double)( horasInteiras * TimeSpan.TicksPerHour ) ).HorasPorExtenso();
        }

        /// <summary>
        /// Método para converter horas inteira para o formta por extenso usado no cronograma
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static string HorasPorExtenso( this double ticks )
        {
            return new TimeSpan((long)ticks).PorExtenso();
        }
        #endregion
    }
}
