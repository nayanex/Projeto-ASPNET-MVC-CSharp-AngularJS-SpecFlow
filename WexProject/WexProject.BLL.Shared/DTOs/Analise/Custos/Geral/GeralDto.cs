using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos.Geral
{
	public class GeralDto
	{
		public GeralDto()
		{
			this.Planejado = new DetalhamentoDto();
			this.Real = new DetalhamentoDto();
			this.Resultado = new ValoresDto();
			this.Acumulado = new ValoresDto();
		}

		public DetalhamentoDto Planejado { get; set; }
		public DetalhamentoDto Real { get; set; }
		public ValoresDto Resultado { get; set; }
		public ValoresDto Acumulado { get; set; }
	}
}
