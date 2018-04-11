using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Models.Custos
{
	/// <summary>
	/// Modelo de Lote de Mão de Obra.
	/// </summary>
	public class LoteMaoDeObra
	{
		[Key]
		public int LoteId { get; set; }
		public DateTime DataAtualizacao { get; set; }
        public int CodigoImportacao { get; set; }

		[ForeignKey("CentroCusto")]
		public int CentroCustoImportacao { get; set; }

		[ForeignKey("RubricaMes")]
		public int RubricaMesId { get; set; }

		public CentroCusto CentroCusto { get; set; }
		public RubricaMes RubricaMes { get; set; }
		public ICollection<MaoDeObra> MaosDeObra { get; set; }
	}
}
