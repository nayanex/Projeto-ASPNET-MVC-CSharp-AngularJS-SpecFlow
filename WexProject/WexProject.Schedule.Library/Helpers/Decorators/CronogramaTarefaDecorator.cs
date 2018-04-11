using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Library.Helpers
{
    public class CronogramaTarefaDecorator : CronogramaTarefaDto
    {
        public TimeSpan EstimativaRestante
        {
            get
            {
                return new TimeSpan( (long)NbEstimativaRestante );
            }
        }

        public TimeSpan EstimativaInicial
        {
            get
            {
                return TimeSpan.FromHours( (int)NbEstimativaInicial );
            }
        }

        public TimeSpan Realizado
        {
            get
            {
                return new TimeSpan( (long)NbRealizado );
            }
        }
    }
}
