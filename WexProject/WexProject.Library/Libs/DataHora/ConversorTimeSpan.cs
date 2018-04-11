using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WexProject.Library.Libs.DataHora
{
    /// <summary>
    /// Classe responsável por Controlar a Gestão de Convesões com Tempo Em Horas e Minutos
    /// </summary>
    public class ConversorTimeSpan
    {
        /// <summary>
        /// Converter uma quantidades de long int para o formato de string do modelo de horas
        /// ex:
        ///  long         -  String Hora
        ///  361200000000 -> 10:02
        /// </summary>
        /// <param name="horasEmTicks">representação das horas em long int</param>
        /// <returns>Retornar string Hora</returns>
        public static string ConverterHorasDeTicksParaString(long horasEmTicks)
        {
            TimeSpan tempo = new TimeSpan(horasEmTicks);
            return tempo.ToString(@"hh\:mm");
        }

        /// <summary>
        /// Converter uma quantidades de long int para o formato de string do modelo de horas
        /// ex:
        ///  long         -  String Hora
        ///  361200000000 -> 10:02
        /// </summary>
        /// <param name="horasEmTicks">representação das horas em long int</param>
        /// <returns>Retornar string Hora</returns>
        public static string ConverterHorasDeTicksParaString( double horasEmTicks )
        {
            TimeSpan tempo = new TimeSpan((long)horasEmTicks );
            return tempo.ToString( @"hh\:mm" );
        }

        /// <summary>
        /// Converter uma quantidades de string de horas para um numero inteiro long int
        /// </summary>
        /// <param name="horasEmString">
        /// Horas a serem convertidas 
        /// Ex.:
        ///     10:02     -> 361200000000
        ///     10:02:25
        /// </param>
        /// <returns>Retorna a quantidade te ticks compostas em um determinado tempo</returns>
        public static long ConverterHorasDeStringParaTicks(string horasEmString) 
        {
            Regex expressao = new Regex(@"(0?\d|1\d|2[0-3]):[0-5]\d(:[0-5]\d)?");
            if (!string.IsNullOrEmpty(horasEmString) && expressao.IsMatch(horasEmString)) 
            {
                TimeSpan tempo;
                int[] digitos = horasEmString.Split(':').Select(o => Convert.ToInt32(o)).ToArray();
                    tempo = new TimeSpan(digitos[0],digitos[1],0);
                return tempo.Ticks;
            }
            else
                return 0;
        }

        /// <summary>
        /// Converter um número de horas em ticks
        /// </summary>
        /// <param name="quantidadeHoras">Quantidade de Horas Inteiras</param>
        /// <returns>quantidade de ticks que compõe um determinado número de horas</returns>
        public static long ConverterHoraInteiraParaTicks(int quantidadeHoras = 0) 
        {
            if (quantidadeHoras > 0) 
                return new TimeSpan(quantidadeHoras,0,0).Ticks;
            return 0;
        }

        /// <summary>
        /// Converter um número de horas em ticks
        /// </summary>
        /// <param name="quantidadeHoras">Quantidade de Horas Inteiras</param>
        /// <returns>quantidade de ticks que compõe um determinado número de horas</returns>
        public static TimeSpan ConverterHoraInteiraParaTimeSpan( int quantidadeHoras = 0 )
        {
            if(quantidadeHoras > 0)
                return new TimeSpan( quantidadeHoras, 0, 0 );
            return new TimeSpan( 0, 0, 0 );
        }

        /// <summary>
        /// Converter uma String de Hora com ou sem utilização de segundos
        /// </summary>
        /// <param name="horasEmString">String no formato de hora a ser convertida em TimeSpan</param>
        /// <returns>TimeSpan cofigurado com a Hora Selecionada</returns>
        public static TimeSpan ConverterHorasDeStringParaTimeSpan(string horasEmString) 
        {
            Regex expressao = new Regex(@"(0?\d|1\d|2[0-3]):[0-5]\d(:[0-5]\d)?");
            if (!string.IsNullOrEmpty(horasEmString) && expressao.IsMatch(horasEmString))
            {
                TimeSpan tempo;
                int[] digitos = horasEmString.Split(':').Select(o => Convert.ToInt32(o)).ToArray();
                if (digitos.Length == 2)
                    tempo = new TimeSpan(digitos[0],digitos[1],0);
                else
                    tempo = new TimeSpan(digitos[0],digitos[1],digitos[2]);

                return tempo;
            }
            else
                return new TimeSpan();
        }

        /// <summary>
        /// Responsável por converter um TimeSpan para String
        /// </summary>
        /// <param name="timeSpan">timeSpan</param>
        /// <param name="usarSegundos">
        /// booleano para definição da utilização dos segundos:
        /// True  - sinalizar utilização dos segundos
        /// False - sinalizar não utilização dos segundos
        /// </param>
        /// <returns>Hora string com ou sem casa dos segundos </returns>
        public static string ConverterTimeSpanParaString(TimeSpan timeSpan,bool usarSegundos = false)
        {
            if (!usarSegundos) 
            {
                return timeSpan.ToString(@"hh\:mm");
            }
            else
            {
               return timeSpan.ToString();
            }
        }

        /// <summary>
        /// Método para converter minutos para horas
        /// </summary>
        /// <param name="horas"></param>
        /// <returns></returns>
        public static int ConverterHorasParaMinutos( int horas )
        {
            return horas * 60;
        }

        /// <summary>
        /// Método para converter minutos para horas
        /// </summary>
        /// <param name="minutos"></param>
        /// <returns></returns>
        public static int ConverterMinutosParaHorasInteiras( int minutos )
        {
            if(minutos < 60)
                return 0;

            int diferenca = minutos % 60;
            if(diferenca > 0)
                minutos -= diferenca;

            return minutos / 60;
        }

        /// <summary>
        /// Converter as horas de string para TimeSpan
        /// </summary>
        /// <param name="horas"></param>
        /// <returns></returns>
        public static TimeSpan CalcularHorasTimeSpan( string horas )
        {
            if(horas != null)
                horas = horas.Trim();

            Regex exp = new Regex( @"^\d{1,3}:\d{2}$" );
            if(string.IsNullOrWhiteSpace( horas ) || !exp.IsMatch( horas ))
                return TimeSpan.Zero;

            int[] valores = horas.Split( ':' ).Select( o => Convert.ToInt32( o ) ).ToArray();

            return new TimeSpan( valores[0], valores[1], 0 );
        }

        /// <summary>
        /// Método para calcular horas para string no formato 99:99
        /// </summary>
        /// <param name="tempo"></param>
        /// <returns></returns>
        public static string CalcularStringHoras( TimeSpan tempo )
        {
            int dias = tempo.Days;
            int horas = tempo.Hours + ( dias * 24 );
            double minutos = tempo.Minutes;
            if(tempo > new TimeSpan( 99, 99, 0 ))
            {
                horas = 99;
                minutos = 99;
            }
            else 
            {
                if(tempo >= new TimeSpan( 99, 0, 0 )) 
                {
                    TimeSpan diferenca = tempo - new TimeSpan( 99, 0, 0 );
                    minutos = diferenca.TotalMinutes;
                    if(minutos > 59)
                        horas--;
                }
            }
            return string.Format( "{0:00}:{1:00}", horas, minutos );
        }
    }
}
