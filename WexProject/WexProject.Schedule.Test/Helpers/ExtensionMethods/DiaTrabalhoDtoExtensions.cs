using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.Schedule.Test.Helpers.ExtensionMethods
{
    public static class DiaTrabalhoDtoExtensions
    {
        public static DiaTrabalhoDto AdicionarPeriodo( this DiaTrabalhoDto diaAtual, string horaInicio, string HoraFinal )
        {
            if(diaAtual == null)
                diaAtual = new DiaTrabalhoDto();
            if(diaAtual.PeriodosTrabalho == null)
                diaAtual.PeriodosTrabalho = new List<PeriodoTrabalhoDto>();
            diaAtual.PeriodosTrabalho.Add( new PeriodoTrabalhoDto( horaInicio, HoraFinal ) );
            return diaAtual;
        }
        public static DiaTrabalhoDto DiaSemana( this DiaTrabalhoDto diaAtual, DayOfWeek diaSemana )
        {
            if(diaAtual == null)
                diaAtual = new DiaTrabalhoDto();
            diaAtual.DiaSemana = diaSemana;
            return diaAtual;
        }
    }
}
