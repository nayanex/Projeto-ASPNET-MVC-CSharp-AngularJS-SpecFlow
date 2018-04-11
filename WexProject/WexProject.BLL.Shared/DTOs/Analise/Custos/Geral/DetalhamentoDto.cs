using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos.Geral
{
	public class DetalhamentoDto : ValoresDto
	{
		public DetalhamentoDto()
			: base()
		{
			this.Projetos = new List<DetalhamentoProjetoDto>();
		}

		public List<DetalhamentoProjetoDto> Projetos { get; set; }
	}
}
