using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
	public class CriarHistoricoTarefaDto
	{
		public Guid OidTarefa { get; set; }

		public string Autor { get; set; }

		public double NbHoraRealizado { get; set; }

		public DateTime DtRealizado { get; set; }

		public double NbHoraInicial { get; set; }

		public double NbHoraFinal { get; set; }

		public double NbHoraRestante { get; set; }

		public string Comentario { get; set; }

		public Guid OidSituacaoPlanejamento { get; set; }

		public string JustificativaReducao { get; set; }
	}
}
