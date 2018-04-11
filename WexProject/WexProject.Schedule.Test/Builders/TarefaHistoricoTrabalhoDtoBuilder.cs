using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Schedule.Test.Builders
{
    public class TarefaHistoricoTrabalhoDtoDataBuilder
    {
        TarefaHistoricoTrabalhoDto historico;
        public TarefaHistoricoTrabalhoDtoDataBuilder()
        {
            historico = new TarefaHistoricoTrabalhoDto();
            historico.OidTarefaHistorico = Guid.NewGuid();
        }

        public TarefaHistoricoTrabalhoDtoDataBuilder RealizadoEm( DateTime dataRealizado ) 
        {
            historico.DtRealizado = dataRealizado;
            return this;
        }

        public TarefaHistoricoTrabalhoDtoDataBuilder ConcluidoAs( string horaFinal )
        {
            historico.NbHoraFinal = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan(horaFinal);
            return this;
        }

        public TarefaHistoricoTrabalhoDtoDataBuilder IniciadoAs( string horaInicial )
        {
            historico.NbHoraInicio = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( horaInicial );
            return this;
        }

        public TarefaHistoricoTrabalhoDtoDataBuilder Realizou( string horaRealizado )
        {
            historico.NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( horaRealizado );
            return this;
        }

        public TarefaHistoricoTrabalhoDtoDataBuilder Restaram( string horaRestante )
        {
            historico.NbRestante = ConversorTimeSpan.CalcularHorasTimeSpan( horaRestante );
            return this;
        }

        public TarefaHistoricoTrabalhoDtoDataBuilder OidTarefa( Guid oidTarefa )
        {
            historico.OidTarefa = oidTarefa;
            return this;
        }

        public TarefaHistoricoTrabalhoDto Criar() 
        {
            return historico;
        }
    }
}
