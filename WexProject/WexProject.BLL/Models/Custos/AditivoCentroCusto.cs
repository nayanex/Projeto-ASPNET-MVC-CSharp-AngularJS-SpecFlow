using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Models.Custos
{
	public class AditivoCentroCusto
	{
		[Key]
		public int AditivoCentroCustoId { get; set; }

		[ForeignKey("Aditivo")]
		public int AditivoId { get; set; }

		[ForeignKey("CentroCusto")]
		public int CentroCustoId { get; set; }

		public virtual Aditivo Aditivo { get; set; }
		public virtual CentroCusto CentroCusto { get; set; }
	}
}
