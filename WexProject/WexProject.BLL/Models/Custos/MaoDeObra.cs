using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Models.Custos
{
	/// <summary>
	/// Modelo de Mão de Obra.
	/// </summary>
	public class MaoDeObra
	{
		[Key]
		public int MaoDeObraId { get; set; }
        public int Matricula { get; set; }
        public String Nome { get; set; }
        public String Cargo { get; set; }
		public int PercentualAlocacao { get; set; }
		public Decimal ValorTotalSemProvisoes { get; set; }
		public Decimal ValorTotal { get; set; }

		[ForeignKey("Lote")]
		public int LoteId { get; set; }
        public LoteMaoDeObra Lote { get; set; }

	}
}
