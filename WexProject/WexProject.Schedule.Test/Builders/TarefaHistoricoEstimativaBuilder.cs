using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.Schedule.Test.Builders
{
    public class TarefaHistoricoEstimativaBuilder
    {
        private TarefaHistoricoEstimativa instancia;

        public TarefaHistoricoEstimativaBuilder()
        {
            instancia = new TarefaHistoricoEstimativa();
        }

        public TarefaHistoricoEstimativaBuilder Data( string data )
        {
            DateTime dataValida;
            instancia.DtPlanejado = DateTime.TryParse( data, out dataValida ) ? dataValida : DateTime.MinValue;
            return this;
        }

        public TarefaHistoricoEstimativaBuilder Data( DateTime data)
        {
            instancia.DtPlanejado = data;
            return this;
        }

        public TarefaHistoricoEstimativaBuilder HorasRestantes( int horasRestantes )
        {
            instancia.NbHoraRestante = horasRestantes.HorasParaTicks();
            return this;
        }

        public TarefaHistoricoEstimativaBuilder OidTarefa( Guid oidTarefa )
        {
            instancia.OidTarefa = oidTarefa;
            return this;
        }

        public TarefaHistoricoEstimativa Criar()
        {
            return instancia;
        }
    }
}
