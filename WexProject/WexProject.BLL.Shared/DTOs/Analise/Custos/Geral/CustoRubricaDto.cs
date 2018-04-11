using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos.Geral
{
	public class CustoRubricaDto
	{
		public int Ano { get; set; }
		public CsMesDomain Mes { get; set; }
		public int TipoRubrica { get; set; }
		public Decimal? Planejado { get; set; }
		public Decimal? Replanejado { get; set; }
		public Decimal? Gasto { get; set; }
		public Boolean Entrada { get; set; }
	}
}
