using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.DTOs.Geral
{
	public class EmpresaInstituicaoDto
	{
		public Guid Oid { get; set; }
		public String Nome { get; set; }
		public String Sigla { get; set; }
		public String Email { get; set; }
		public String FoneFax { get; set; }
	}
}
