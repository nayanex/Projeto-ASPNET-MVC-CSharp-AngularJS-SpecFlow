using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Models.Custos
{
	public class RubricaMes
	{
		[Key]
		public int RubricaMesId { get; set; }

		[ForeignKey("Rubrica")]
		public int RubricaId { get; set; }

		public CsMesDomain CsMes { get; set; }
		public int NbAno { get; set; }
		public Boolean PossuiGastosRelacionados { get; set; }
		public Nullable<Decimal> NbPlanejado { get; set; }
		public Nullable<Decimal> NbReplanejado { get; set; }
		public Nullable<Decimal> NbGasto { get; set; }
       

		public virtual Rubrica Rubrica { get; set; }
	}
}
