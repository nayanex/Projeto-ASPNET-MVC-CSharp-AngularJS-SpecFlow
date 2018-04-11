using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Helpers.BDD.Bind
{
    public class RubricaMesBindHelper
    {
        public string Rubrica { get; set; }
        public CsMesDomain Mes { get; set; }
        public int Ano { get; set; }
        public decimal? OrcamentoAprovado { get; set; }
        public decimal? DespesaReal { get; set; }
    }
}
