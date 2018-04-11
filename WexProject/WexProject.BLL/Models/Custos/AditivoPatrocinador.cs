using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Models.Custos
{
	public class AditivoPatrocinador
	{
		[Key]
		public int AditivoPatrocinadorId { get; set; }

		[ForeignKey("Aditivo")]
		public int AditivoId { get; set; }

		[ForeignKey("Patrocinador")]
		public Guid PatrocinadorOid { get; set; }

		public virtual Aditivo Aditivo { get; set; }
		public virtual EmpresaInstituicao Patrocinador { get; set; }
	}
}
