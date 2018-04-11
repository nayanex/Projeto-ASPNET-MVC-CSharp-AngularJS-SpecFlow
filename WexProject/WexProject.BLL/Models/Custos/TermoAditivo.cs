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
	public class TermoAditivo
	{
		[Key]
		public int TermoAditivoId { get; set; }

		public String TxNome { get; set; }
		public String TxDescricao { get; set; }
		public DateTime DtInicio { get; set; }
		public DateTime DtTermino { get; set; }
		public Decimal NbAporteTotal { get; set; }

		[ForeignKey("Patrocinador")]
		public Guid PatrocinadorId { get; set; }

		public virtual List<Projeto> Projetos { get; set; }
		public virtual EmpresaInstituicao Patrocinador { get; set; }
	}
}
