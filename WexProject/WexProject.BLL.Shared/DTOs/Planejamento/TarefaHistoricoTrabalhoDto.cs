using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class TarefaHistoricoTrabalhoDto
    {
        #region Propriedades

        public Guid OidTarefaHistorico { get; set; }

        public TimeSpan NbRealizado { get; set; }

        public DateTime DtRealizado { get; set; }

        public String TxComentario { get; set; }

        public TimeSpan NbRestante { get; set; }

        public Guid OidSituacaoPlanejamento { get; set; }

        public String TxJustificativaDeReducao { get; set; }

        public Guid OidTarefa { get; set; }

        public Guid OidColaborador { get; set; }

        public TimeSpan NbHoraInicio { get; set; }

        public TimeSpan NbHoraFinal { get; set; }
        #endregion
    }
}
