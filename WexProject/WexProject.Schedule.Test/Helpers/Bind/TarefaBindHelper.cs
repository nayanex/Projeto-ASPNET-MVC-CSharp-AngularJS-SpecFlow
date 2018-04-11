using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.Schedule.Test.Helpers.Bind
{
    public class TarefaBindHelper
    {
        public short Id { get; set; }

        public string Descricao { get; set; }

        public CsTipoPlanejamento Situacao { get; set; }

        public short EstimativaInicial { get; set; }

        public int EstimativaRestante { get; set; }
    }
}
