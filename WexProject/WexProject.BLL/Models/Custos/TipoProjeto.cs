using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Models.Custos
{
	public class TipoProjeto
	{
		[Key]
		public int TipoProjetoId { get; set; }

		[ForeignKey("ClasseProjeto")]
		public int ClasseProjetoId { get; set; }

		public String TxNome { get; set; }

		public virtual ClasseProjeto ClasseProjeto { get; set; }
		public virtual List<TipoRubrica> TiposRubricas { get; set; }
		public virtual List<Projeto> Projetos { get; set; }
	}
}
