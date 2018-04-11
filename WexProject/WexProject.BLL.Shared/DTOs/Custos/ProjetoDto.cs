using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	public class ProjetoDto
	{
		public Guid Oid { get; set; }
		public String Nome { get; set; }
		public int TipoProjetoId { get; set; }
		public Nullable<DateTime> DataInicial { get; set; }
		public Nullable<DateTime> DataFinal { get; set; }
		public Decimal Valor { get; set; }
		public Int64 Duracao { get; set; }
		public String Cliente { get; set; }
		public List<String> Clientes { get; set; }
		public String Gerente { get; set; }
		public Nullable<int> CentroCustoId { get; set; }
		public String CentroCusto { get; set; }
		public int Status { get; set; }
		public int Classe { get; set; }
		public Nullable<Guid> ProjetoMacroOid { get; set; }
		public Nullable<int> TermoAditivoId { get; set; }
	}
}
