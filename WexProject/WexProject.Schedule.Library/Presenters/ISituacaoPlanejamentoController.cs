using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Presenters
{
	public interface ISituacaoPlanejamentoController
	{
		void RecusarSituacaoPlanejamento(CronogramaTarefaDecorator decorator, Guid oidSituacaoPlanejamento);

		void NotificarMensagem(string titulo,string mensagem , bool alerta = true);
	}
}
