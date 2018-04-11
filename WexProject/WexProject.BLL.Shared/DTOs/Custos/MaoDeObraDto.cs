using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	/// <summary>
	/// DTO de Mão de Obra.
	/// </summary>
	public class MaoDeObraDto
	{
		public int MaoDeObraId { get; set; }
        public int Matricula { get; set; }
        public String Nome { get; set; }
        public String Cargo { get; set; }
		public int PercentualAlocacao { get; set; }
        public Decimal ValorTotalSemProvisoes { get; set; }
		public Decimal ValorTotal { get; set; }
		public int LoteId { get; set; }
	}
}
