using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Models.Custos
{
	public class ClasseProjeto
	{
		[Key]
		public int ClasseProjetoId { get; set; }

		public String TxNome { get; set; }

		public virtual List<TipoProjeto> TiposProjetos { get; set; }
	}
}
