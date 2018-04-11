using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Custos;

namespace WexProject.BLL.Models.Custos
{
	public class TipoRubrica
	{
		[Key]
		public int TipoRubricaId { get; set; }

		[ForeignKey("TipoPai")]
		public Nullable<int> TipoPaiId { get; set; }

		[ForeignKey("TipoProjeto")]
		public Nullable<int> TipoProjetoId { get; set; }

		public String TxNome { get; set; }
		public CsClasseRubrica CsClasse { get; set; }

		public virtual TipoRubrica TipoPai { get; set; }
		public virtual TipoProjeto TipoProjeto { get; set; }
		public virtual ICollection<TipoRubrica> TiposFilhos { get; set; }
	}
}
