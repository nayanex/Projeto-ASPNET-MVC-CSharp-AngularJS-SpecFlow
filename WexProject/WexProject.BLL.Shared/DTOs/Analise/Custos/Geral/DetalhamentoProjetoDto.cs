using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos.Geral
{
	public class DetalhamentoProjetoDto : ValoresDto
	{
		public Guid Oid { get; set; }
		public String Nome { get; set; }
		public int Status { get; set; }
		public int Classe { get; set; }
	}
}
