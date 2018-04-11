using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Shared.DTOs.Custos
{
	public class TermoAditivoDto
	{
		public int TermoAditivoId { get; set; }
		public String Nome { get; set; }
		public String Descricao { get; set; }
		public DateTime DataInicio { get; set; }
		public DateTime DataTermino { get; set; }
		public Decimal AporteTotal { get; set; }
		public List<ProjetoDto> Projetos { get; set; }
		public EmpresaInstituicaoDto Patrocinador { get; set; }
	}
}
