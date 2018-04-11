using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos.Geral
{
	public class RubricasDto
	{
		public RubricasDto()
		{
			this.Desenvolvimento = new List<DetalhamentoRubricaDto>();
			this.Administrativa = new List<DetalhamentoRubricaDto>();
		}

		public List<DetalhamentoRubricaDto> Desenvolvimento { get; set; }
		public List<DetalhamentoRubricaDto> Administrativa { get; set; }
	}
}
