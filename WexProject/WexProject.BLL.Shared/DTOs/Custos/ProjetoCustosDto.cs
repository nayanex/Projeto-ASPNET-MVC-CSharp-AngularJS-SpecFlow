using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Shared.DTOs.Custos
{
    public class ProjetoCustosDto
    {
        public Guid Oid { get; set; }
        public String Nome { get; set; }
        public Nullable<DateTime> DataInicioPlanejado { get; set; }
        public Nullable<DateTime> DataInicioReal { get; set; }
        public CsProjetoSituacaoDomain Situacao { get; set; }
    }
}
