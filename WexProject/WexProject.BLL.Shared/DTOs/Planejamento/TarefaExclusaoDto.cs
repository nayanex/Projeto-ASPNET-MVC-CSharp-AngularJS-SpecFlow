using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
	public class TarefaExclusaoDto
	{
		public Guid? OidCronograma { get; set; }

		public List<Guid> OidsCronogramaTarefas { get; set; }
	}
}
