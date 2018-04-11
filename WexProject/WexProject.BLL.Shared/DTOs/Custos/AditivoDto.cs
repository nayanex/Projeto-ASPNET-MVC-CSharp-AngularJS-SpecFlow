using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	public class AditivoDto
	{
		public int AditivoId { get; set; }
		public String Nome { get; set; }
		public DateTime DataInicio { get; set; }
		public DateTime DataTermino { get; set; }
		public int Duracao { get; set; }
		public Decimal Orcamento { get; set; }
		public Decimal OrcamentoRestante { get; set; }
		public Guid Projeto { get; set; }
	}
}
