using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Library.Properties;
using WexProject.Schedule.Library.Libs.Delegates.Tempo;
using WexProject.Schedule.Library.Domains;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.Schedule.Library.Presenters
{
	public class SituacaoPlanejamentoRegrasComboUtil : SituacaoPlanejamentoRegrasBaseUtil
	{
		#region Atributos

		/// <summary>
		/// Objeto Tarefa
		/// </summary>
		CronogramaTarefaDecorator decorator;

		/// <summary>
		/// Login do usuário
		/// </summary>
		private string login;

		/// <summary>
		/// Controlador que efetua a comunicação das regras com a view.
		/// </summary>
		private ISituacaoPlanejamentoComboController situacaoController;

		#endregion

		#region Construtor

		public SituacaoPlanejamentoRegrasComboUtil( List<SituacaoPlanejamentoDTO> situacoesPlanejamento, ISituacaoPlanejamentoComboController controller, string login )
			: base( situacoesPlanejamento )
		{
			situacaoController = controller;
			this.login = login;
		}

		/// <summary>
		/// Método utilizado para atribuir um controle.
		/// </summary>
		/// <param name="controller"></param>
		public void SetController( ISituacaoPlanejamentoComboController controller )
		{
			situacaoController = controller;
		}

		#endregion

		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Execução
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected override CsSituacaoPlanejamentoTipoRetorno AlterarParaExecucao( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento )
		{
			if(NaoPossuiEstimativaInicial( decorator ))
			{
				situacaoController.RecusarSituacaoPlanejamento( decorator, situacaoPlanejamento.Oid );
				situacaoController.NotificarMensagem( Resources.Caption_Atencao, Resources.Alerta_DevePossuirDuracaoTarefa );
				situacaoController.ForcarFimEdicao();
				return CsSituacaoPlanejamentoTipoRetorno.SituacaoPlanejamentoRecusada;
			}
			else
			{
				if(NaoPossuiHorasRestantes( decorator ))
				{
					if(decorator.EstimativaInicial.Ticks <= decorator.NbRealizado)
					{
						decorator.NbEstimativaRestante = decorator.EstimativaInicial.Ticks;
					}
					else
					{
						decorator.NbEstimativaRestante = decorator.EstimativaInicial.Ticks - decorator.NbRealizado;
					}

					situacaoController.InicializarFormularioHistoricoView( situacaoPlanejamento.Oid );
					return CsSituacaoPlanejamentoTipoRetorno.NaoConsumiuHoras;
				}
				else
				{
					decorator.OidSituacaoPlanejamentoTarefa = situacaoPlanejamento.Oid;
					return CsSituacaoPlanejamentoTipoRetorno.NaoConsumiuHoras;
				}
			}
		}

		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Planejamento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected override CsSituacaoPlanejamentoTipoRetorno AlterarParaPlanejamento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento )
		{
			if(PossuiEsforcoRealizado( decorator ))
			{
				situacaoController.RecusarSituacaoPlanejamento( decorator, situacaoPlanejamento.Oid );
				situacaoController.NotificarMensagem( Resources.Caption_Atencao, Resources.Alerta_PossuiEsforcoRealizadoCadastrado );
				situacaoController.LimparBarraStatus();
				situacaoController.ForcarFimEdicao();

				return CsSituacaoPlanejamentoTipoRetorno.SituacaoPlanejamentoRecusada;
			}
			decorator.OidSituacaoPlanejamentoTarefa = situacaoPlanejamento.Oid;
			return CsSituacaoPlanejamentoTipoRetorno.NaoConsumiuHoras;
		}

		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Cancelamento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected override CsSituacaoPlanejamentoTipoRetorno AlterarParaCancelamento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento )
		{
			decorator.NbEstimativaRestante = 0;
			decorator.OidSituacaoPlanejamentoTarefa = situacaoPlanejamento.Oid;
			return CsSituacaoPlanejamentoTipoRetorno.NaoConsumiuHoras;
		}

		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Impedimento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected override CsSituacaoPlanejamentoTipoRetorno AlterarParaImpedimento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento )
		{
			decorator.OidSituacaoPlanejamentoTarefa = situacaoPlanejamento.Oid;
			return CsSituacaoPlanejamentoTipoRetorno.NaoConsumiuHoras;
		}

		/// <summary>
		/// Método responsável por selecionar o periodo de trabalho do dia atual
		/// </summary>
		public PeriodoTrabalhoDto[] SelecionarPeriodoTrabalhoDiaAtual( DiaTrabalhoDto diaTrabalho, out TimeSpan horaInicioExpediente, out TimeSpan horaFimExpediente )
		{
			PeriodoTrabalhoDto[] periodos;
			if(diaTrabalho != null)
			{
				periodos = diaTrabalho.PeriodosTrabalho.OrderBy( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraInicial ) ).ToArray();
				horaInicioExpediente = new TimeSpan( periodos.Min( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraInicial ) ) );
				horaFimExpediente = new TimeSpan( periodos.Max( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraFinal ) ) );
			}
			else
			{
				PeriodoTrabalhoDto periodo = new PeriodoTrabalhoDto( "08:00", "18:00" );
				horaInicioExpediente = new TimeSpan( 8, 0, 0 );
				horaFimExpediente = new TimeSpan( 18, 0, 0 );
				periodos = new PeriodoTrabalhoDto[] { periodo };
			}
			return periodos;
		}

		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Encerramento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected override CsSituacaoPlanejamentoTipoRetorno AlterarParaEncerramento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento )
		{
			if(NaoPossuiEstimativaInicial( decorator ))
			{
				situacaoController.RecusarSituacaoPlanejamento( decorator, situacaoPlanejamento.Oid );
				situacaoController.NotificarMensagem( Resources.Caption_Atencao, Resources.Alerta_DevePossuirDuracaoTarefa );
				situacaoController.ForcarFimEdicao();
				( (ISituacaoPlanejamentoComboController)situacaoController ).LimparBarraStatus();
				return CsSituacaoPlanejamentoTipoRetorno.SituacaoPlanejamentoRecusada;
			}

			if(NaoPossuiHorasRestantes( decorator ))
			{
				if(decorator.EstimativaInicial.Ticks <= decorator.NbRealizado)
					decorator.NbEstimativaRestante = decorator.EstimativaInicial.Ticks;
				else
					decorator.NbEstimativaRestante = decorator.EstimativaInicial.Ticks - decorator.NbRealizado;
			}

			situacaoController.InicializarFormularioHistoricoView( situacaoPlanejamento.Oid );

			return CsSituacaoPlanejamentoTipoRetorno.NaoConsumiuHoras;
		}

		/// <summary>
		/// Validar situacaoPlanejamento
		/// </summary>
		/// <param name="tipo"></param>
		/// <returns></returns>
		public bool ValidarSituacaoPlanejamento( CsTipoPlanejamento tipo )
		{
			return ValidarSituacaoPlanejamento( tipo, decorator );
		}


		/// <summary>
		/// Método responsável por iniciar e chamar outras validações para situação planejamento
		/// </summary>
		/// <param name="oidSituacaoPlanejamentoAlterada">Oid situação planejamento desejada</param>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <returns>Validação</returns>
		public override CsSituacaoPlanejamentoTipoRetorno Executar( Guid oidSituacaoPlanejamentoAlterada, CronogramaTarefaDecorator decorator )
		{
			SituacaoPlanejamentoDTO situacaoAlterada = GetSituacaoPlanejamentoSelecionada( oidSituacaoPlanejamentoAlterada );

			if(situacaoAlterada.Oid == decorator.OidSituacaoPlanejamentoTarefa)
				situacaoController.ForcarFimEdicao();

			switch(situacaoAlterada.CsTipo)
			{
				case CsTipoPlanejamento.Encerramento:
					return AlterarParaEncerramento( decorator, situacaoAlterada );
				case CsTipoPlanejamento.Execução:
					return AlterarParaExecucao( decorator, situacaoAlterada );
				case CsTipoPlanejamento.Impedimento:
					return AlterarParaImpedimento( decorator, situacaoAlterada );
				case CsTipoPlanejamento.Planejamento:
					return AlterarParaPlanejamento( decorator, situacaoAlterada );
				case CsTipoPlanejamento.Cancelamento:
					return AlterarParaCancelamento( decorator, situacaoAlterada );
				default:
					return CsSituacaoPlanejamentoTipoRetorno.SituacaoPlanejamentoRecusada;
			}
		}
	}
}
