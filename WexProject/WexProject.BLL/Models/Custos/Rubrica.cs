using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Models.Custos
{
	public class Rubrica
	{
		public Rubrica()
		{
			this.RubricaMeses = new List<RubricaMes>();
			this.Filhos = new List<Rubrica>();
		}

		[Key]
		public int RubricaId { get; set; }

		[ForeignKey("TipoRubrica")]
		public int TipoRubricaId { get; set; }

		[ForeignKey("Aditivo")]
		public int AditivoId { get; set; }

		[ForeignKey("Pai")]
		public Nullable<int> PaiId { get; set; }

		public Decimal NbTotalPlanejado { get; set; }

		public virtual TipoRubrica TipoRubrica { get; set; }
		public virtual Aditivo Aditivo { get; set; }
		public virtual Rubrica Pai { get; set; }
		public virtual ICollection<RubricaMes> RubricaMeses { get; set; }
		public virtual ICollection<Rubrica> Filhos { get; set; }
	}
}
