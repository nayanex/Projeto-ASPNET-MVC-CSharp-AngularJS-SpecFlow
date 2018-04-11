using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	/// <summary>
	/// DTO de Lote de Mão de Obra.
	/// </summary>
	public class LoteMaoDeObraDto
	{
		public int LoteId { get; set; }
		public DateTime DataAtualizacao { get; set; }
		public int CentroCustoImportacao { get; set; }
		public int RubricaMesId { get; set; }
	}
}
