using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Shared.DTOs.Geral
{
    public class ProjetoDto
    {
        public Guid IdProjeto { get; set; }
        public string Nome { get; set; }
		public CsProjetoSituacaoDomain Situacao { get; set; }
    }
}
