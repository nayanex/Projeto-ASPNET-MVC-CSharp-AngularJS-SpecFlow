using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.Domains.Custos;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	public class RubricaDto
	{
		public RubricaDto()
		{
			this.Anos = new List<RubricaAnoDto>();
		}

		public int RubricaId { get; set; }
		public int AditivoId { get; set; }
		public Nullable<int> PaiId { get; set; }
		public CsClasseRubrica Classe { get; set; }
		public int Tipo { get; set; }
		public String Nome { get; set; }
		public Decimal TotalPlanejado { get; set; }
		public ICollection<RubricaAnoDto> Anos { get; set; }
	}
}
