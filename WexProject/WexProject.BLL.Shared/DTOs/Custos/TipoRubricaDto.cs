using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Custos;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	public class TipoRubricaDto
	{
		public int TipoRubricaId { get; set; }
		public Nullable<int> PaiId { get; set; }
		public String Pai { get; set; }
		public String Nome { get; set; }
		public CsClasseRubrica Classe { get; set; }
	}
}
