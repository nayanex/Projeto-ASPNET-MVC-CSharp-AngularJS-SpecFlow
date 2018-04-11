using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.Schedule.Test.Builders
{
    public class DiaTrabalhoDtoBuilder
    {
        DiaTrabalhoDto diaTrabalhoDto;
        public DiaTrabalhoDtoBuilder()
        {
            diaTrabalhoDto = new DiaTrabalhoDto();
        }

        public DiaTrabalhoDtoBuilder(DiaTrabalhoDto dia)
        {
            if(dia == null)
                diaTrabalhoDto = new DiaTrabalhoDto();
            else
                diaTrabalhoDto = dia;
        }

        public DiaTrabalhoDtoBuilder AddPeriodo( string inicioPeriodo, string fimPeriodo ) 
        {
            diaTrabalhoDto.PeriodosTrabalho.Add( new PeriodoTrabalhoDto( inicioPeriodo, fimPeriodo ) );
            return this;
        }

        public DiaTrabalhoDtoBuilder DiaSemana( DayOfWeek diaSemana )
        {
            diaTrabalhoDto.DiaSemana = diaSemana;
            return this;
        }

        public DiaTrabalhoDto Criar() 
        {
            return diaTrabalhoDto;
        }
    }
}
