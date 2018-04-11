using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Models.Custos
{
	public class CentroCusto
	{
		[Key]
		public int CentroCustoId { get; set; }

		public int Codigo { get; set; }

		public String Nome { get; set; }
	}
}
