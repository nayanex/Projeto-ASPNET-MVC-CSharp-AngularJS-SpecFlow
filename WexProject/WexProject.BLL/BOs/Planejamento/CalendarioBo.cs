using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.BOs.Planejamento
{
    public class CalendarioBo
    {
        /// <summary>
        /// Método responsável por calcular os dias uteis para um determinado intervalo
        /// </summary>
        /// <param name="dataInicio"></param>
        /// <param name="dataTermino"></param>
        /// <param name="calendariosDias"></param>
        /// <returns></returns>
        public static List<DateTime> CalcularDiasUteis( DateTime dataInicio, DateTime dataTermino)
        {
            List<Calendario> calendariosDias = CalendarioDAO.ConsultarCalendarioPorPeriodo( dataInicio, dataTermino );
            List<DateTime> diasUteis = new List<DateTime>(), datasPeriodo = new List<DateTime>();

            if(dataInicio > dataTermino)
                return diasUteis;

            DateTime dataAtual = new DateTime( dataInicio.Date.Ticks );
            while(dataAtual.Date <= dataTermino.Date)
            {
                datasPeriodo.Add( dataAtual );
                dataAtual = dataAtual.AddDays( 1 );
            }

            var diasTrabalho = datasPeriodo.Where( o => EhDiaTrabalho( o, calendariosDias ) ).ToList();
            datasPeriodo.RemoveAll( o => EhFimDeSemana( o ) || EhFeriado( o, calendariosDias ) );
            diasUteis = diasTrabalho.Union( datasPeriodo ).OrderBy( o => o ).ToList();
            return diasUteis;
        }

        public static bool EhDiaTrabalho( DateTime dataAtual, List<Calendario> dias )
        {
            return CalendarioPossuiDiaCadastrado( dataAtual, dias, CsCalendarioDomain.Trabalho );
        }

        public static bool EhFeriado( DateTime dataAtual, List<Calendario> dias )
        {
            return CalendarioPossuiDiaCadastrado( dataAtual, dias, CsCalendarioDomain.Folga );
        }

        public static bool CalendarioPossuiDiaCadastrado( DateTime dataAtual, List<Calendario> diasCadastradosCalendario, CsCalendarioDomain tipo )
        {
            return diasCadastradosCalendario.Any( o =>
                    o.CsCalendario == (int)tipo
                  && (
                     ( o.CsVigencia == (int)CsVigenciaDomain.PorDiaMesAno && o.DtInicio.Value >= dataAtual.Date && o.DtInicio.Value <= dataAtual.Date )
                  || ( o.CsVigencia == (int)CsVigenciaDomain.PorDiaMes && o.NbDia == dataAtual.Day && o.CsMes.Value == dataAtual.Month )
                  || ( o.CsVigencia == (int)CsVigenciaDomain.PorPeriodo && dataAtual.Date >= o.DtInicio.Value.Date && dataAtual.Date <= o.DtTermino.Value.Date )
                  ) );
        }

        public static bool EhFimDeSemana( DateTime dataAtual )
        {
            return dataAtual.DayOfWeek == DayOfWeek.Saturday || dataAtual.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
