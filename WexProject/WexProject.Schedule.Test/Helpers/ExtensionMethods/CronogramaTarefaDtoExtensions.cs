using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Test.Helpers.ExtensionMethods
{
    public static class CronogramaTarefaDtoExtensions
    {
        /// <summary>
        /// Método para converter um objeto CronogramaTarefaDto em CronogramaTarefaDecorator
        /// </summary>
        /// <param name="dto">Instancia do objeto CronogramaTarefaDto</param>
        /// <returns></returns>
        public static CronogramaTarefaDecorator ToDecorator(this CronogramaTarefaDto dto)
        {
            string dtoString = JsonConvert.SerializeObject( dto );
            return JsonConvert.DeserializeObject<CronogramaTarefaDecorator>(dtoString);
        }
    }
}
