using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Presenters
{
	public interface ISituacaoPlanejamentoComboController : ISituacaoPlanejamentoController
	{
		void LimparBarraStatus();
		void ForcarFimEdicao();
		void AtualizarDadosTarefa( Guid oidTarefa );
		void InicializarFormularioHistoricoView( Guid oidSituacaoPlanejamento );
	}
}
