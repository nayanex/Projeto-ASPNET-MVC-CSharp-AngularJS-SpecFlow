using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	public class DespesaRealDto
	{
		public Guid ProjetoOid { get; set; }
		public int TipoRubricaId { get; set; }
		public CsMesDomain Mes { get; set; }
		public int Ano { get; set; }
		public Decimal? DespesaReal { get; set; }
	}
}
