using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.BLL.BOs.Planejamento
{
	public class CronogramaUltimaSelecaoBO
	{
		public static void SalvarUltimoCronogramaSelecionado( string login, Guid oidCronograma )
		{
			using(WexDb contexto = ContextFactoryManager.CriarWexDb())
			{
				CronogramaUltimaSelecaoDao.SalvarUltimoCronogramaSelecionado( contexto, login, oidCronograma );
			}
		}

        public static CronogramaDto ConsultarUltimoCronogramaSelecionadoDto(WexDb contexto, string login)
        {
            var cronogramaDto = CronogramaUltimaSelecaoDao.ConsultarUltimoCronogramaSelecionadoDto(contexto, login);
            return cronogramaDto;
        }
	}
}
