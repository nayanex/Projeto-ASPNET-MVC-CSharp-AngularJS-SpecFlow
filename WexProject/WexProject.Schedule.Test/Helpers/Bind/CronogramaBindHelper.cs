using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Test.Helpers.Bind
{
    public class CronogramaBindHelper
    {
        public string Nome { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Final { get; set; }
    }
}
