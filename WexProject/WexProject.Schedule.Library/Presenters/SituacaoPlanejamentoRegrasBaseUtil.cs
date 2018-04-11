using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Library.Domains;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Library.Libs.Delegates.Tempo;
using WexProject.Schedule.Library.Properties;
using WexProject.Schedule.Library.ServiceUtils;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;

namespace WexProject.Schedule.Library.Presenters
{
	/// <summary>
	/// Classe abstrata para implementação de um validador de regras para a edição da situação de planejamento de um tarefa
	/// </summary>
	public abstract class SituacaoPlanejamentoRegrasBaseUtil
	{
		/// <summary>
		/// Armazenar uma lista de situações de planejamento possíveis para um tarefa
		/// </summary>
		protected List<SituacaoPlanejamentoDTO> SituacoesPlanejamento
		{
			get { return situacoesPlanejamento; }
			set
			{
				if(value == null)
					throw new ArgumentNullException( "Deve ser passada uma lista de situações de planejamento" );
				situacoesPlanejamento = value;
			}
		}

		/// <summary>
		/// Armazenar um client de consumo do webservice de dados de planejamento
		/// </summary>
		protected IPlanejamentoServiceUtil planejamentoServiceClient;

		/// <summary>
		/// Armazenar um client de consumo do webservice de dados gerais
		/// </summary>
		protected IGeralServiceUtil geralServiceClient;

		/// <summary>
		/// Atributo Responsável por armazenar a quantidade de horas estimadas inicialmente
		/// </summary>
		protected TimeSpan horaEstimativaInicial;

		/// <summary>
		/// responsável por armazenar a hora do inicio do esforço da realização da tarefa
		/// </summary>
		protected TimeSpan horaInicial;

		/// <summary>
		/// responsável por armazenar a hora do fim do esforço da realização da tarefa
		/// </summary>
		protected TimeSpan horaFinal;

		/// <summary>
		/// responsável por armazenar a(s) horas que restam programadas para a solução da tarefa
		/// </summary>
		protected TimeSpan horaRestante;

		/// <summary>
		/// responsável por armazenar a(s) a quantidade de horas realizadas no esforço da tarefa
		/// </summary>
		protected TimeSpan horaRealizada;
		/// <summary>
		/// Atributo responsável por armazenar a última hora trabalhada de um determinado colaborador.
		/// </summary>
		protected TimeSpan nbUltimaHoraTrabalhadaColaborador;

		#region Atributos

		/// <summary>
		/// Armazenar uma lista de situações de planejamento possíveis para um tarefa
		/// </summary>
		protected List<SituacaoPlanejamentoDTO> situacoesPlanejamento;
		#endregion

		#region Propriedades

		/// <summary>
		/// Responsável por armazenar os periodos de trabalho do dia atual
		/// </summary>
		public PeriodoTrabalhoDto[] PeriodosDeTrabalho { get; set; }

		/// <summary>
		/// Responsável por armazenar o dia de trabalho selecionado
		/// </summary>
		public DiaTrabalhoDto DiaTrabalhoAtual { get; set; }

		/// <summary>
		/// Login do colaborador
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// responsável por armazenar a hora de inicio do expediente
		/// </summary>
		public TimeSpan HoraInicioExpediente { get; set; }

		/// <summary>
		/// responsável por armazenar a hora final do expediente
		/// </summary>
		public TimeSpan HoraFinalExpediente { get; set; }

		#endregion

		#region Construtores

		public SituacaoPlanejamentoRegrasBaseUtil( List<SituacaoPlanejamentoDTO> situacoesPlanejamento )
		{
			SituacoesPlanejamento = situacoesPlanejamento;
			planejamentoServiceClient = new PlanejamentoServiceUtil();
			geralServiceClient = new GeralServiceUtil();
			geralServiceClient.ConsultarSemanaDeTrabalhoPadrao();
		}

		#endregion

		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Execucao
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected abstract CsSituacaoPlanejamentoTipoRetorno AlterarParaExecucao( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento );

		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Encerramento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected abstract CsSituacaoPlanejamentoTipoRetorno AlterarParaPlanejamento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento );


		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Cancelamento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected abstract CsSituacaoPlanejamentoTipoRetorno AlterarParaCancelamento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento );


		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Encerramento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected abstract CsSituacaoPlanejamentoTipoRetorno AlterarParaEncerramento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento );


		/// <summary>
		/// Método responsável por validar a regra quando for alterado para Impedimento
		/// </summary>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <param name="situacaoPlanejamento">Situação planejamento desejada</param>
		/// <returns>Validação</returns>
		protected abstract CsSituacaoPlanejamentoTipoRetorno AlterarParaImpedimento( CronogramaTarefaDecorator decorator, SituacaoPlanejamentoDTO situacaoPlanejamento );


		/// <summary>
		/// Método responsável por iniciar e chamar outras validações para situação planejamento
		/// </summary>
		/// <param name="oidSituacaoPlanejamentoAlterada">Oid situação planejamento desejada</param>
		/// <param name="decorator">Objeto Tarefa</param>
		/// <returns>Validação</returns>
		public abstract CsSituacaoPlanejamentoTipoRetorno Executar( Guid oidSituacaoPlanejmento, CronogramaTarefaDecorator decorator );

		#region Métodos de validação

		/// <summary>
		/// Retornar se a situação de uma situação planejamento é válida
		/// </summary>
		/// <param name="tipo">tipo de situação</param>
		/// <param name="decorator">tarefa com os dados para serem validados</param>
		/// <returns></returns>
		public bool ValidarSituacaoPlanejamento( CsTipoPlanejamento tipo, CronogramaTarefaDecorator decorator )
		{
			SituacaoPlanejamentoDTO situacao = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == tipo );
			if(situacao == null)
				return false;

			switch(situacao.CsTipo)
			{
				case CsTipoPlanejamento.Encerramento:
					return PossuiEsforcoRealizado( decorator ) && NaoPossuiHorasRestantes( decorator );
				case CsTipoPlanejamento.Execução:
					return PossuiEstimativaInicial( decorator ) && PossuiHorasRestantes( decorator );
				case CsTipoPlanejamento.Planejamento:
					return NaoPossuiEsforcoRealizado( decorator );
				case CsTipoPlanejamento.Cancelamento:
					return NaoPossuiHorasRestantes( decorator );
				case CsTipoPlanejamento.Impedimento:
					return true;
			}
			return false;
		}

		/// <summary>
		/// Método que verifica se tarefa atual pode receber um tipo de situacaoPlanejamento
		/// </summary>
		/// <param name="tarefa">instancia da tarefa</param>
		/// <param name="tipo">tipo de planejamento que irá receber</param>
		protected bool ValidarEdicaoSituacaoPlanejamento( CronogramaTarefaDto tarefa, CsTipoPlanejamento tipo, out string motivo )
		{
			motivo = null;
			if(tarefa == null)
				return false;

			switch(tipo)
			{
				case CsTipoPlanejamento.Encerramento:
					if(PossuiEstimativaInicial( tarefa ))
						return true;

					motivo = Resources.Alerta_DevePossuirDuracaoTarefa;
					return false;
				case CsTipoPlanejamento.Execução:

					if(PossuiHorasRestantes( tarefa ))
						return true;

					motivo = Resources.Alerta_DevePossuirDuracaoTarefa;
					return false;
				case CsTipoPlanejamento.Planejamento:
					if(!PossuiEsforcoRealizado( tarefa ))
						return true;

					motivo = Resources.Alerta_PossuiEsforcoRealizadoCadastrado;
					return false;
				default:
					return true;
			}
		}

		/// <summary>
		/// Método que verifica se uma tarefa possui estimativa inicial
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		protected bool PossuiEstimativaInicial( CronogramaTarefaDto tarefa )
		{
			if(TarefaNula( tarefa ))
				return false;
			return PossuiTempoEstimado( tarefa.NbEstimativaInicial );
		}

		/// <summary>
		/// Verifica se uma tarefa não possui estimativa inicial
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		protected bool NaoPossuiEstimativaInicial( CronogramaTarefaDto tarefa )
		{
			return !PossuiEstimativaInicial( tarefa );
		}

		/// <summary>
		/// Verifica se uma tarefa não possui estimativa inicial
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		protected bool NaoPossuiHorasRestantes( CronogramaTarefaDto tarefa )
		{
			return !PossuiHorasRestantes( tarefa );
		}

		/// <summary>
		/// Retorna se a tarefa possui horas restantes
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		protected bool PossuiHorasRestantes( CronogramaTarefaDto tarefa )
		{
			if(TarefaNula( tarefa ))
				return false;
			return PossuiTempoEstimado( tarefa.NbEstimativaRestante );
		}

		/// <summary>
		/// Retorna se a tarefa possui esforço realizado
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		protected bool PossuiEsforcoRealizado( CronogramaTarefaDto tarefa )
		{
			if(TarefaNula( tarefa ))
				return false;
			return PossuiTempoEstimado( tarefa.NbRealizado );
		}

		/// <summary>
		/// Retorna se a tarefa possuir ou não esforço realizado
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		protected bool NaoPossuiEsforcoRealizado( CronogramaTarefaDto tarefa )
		{
			return !PossuiEsforcoRealizado( tarefa );
		}

		/// <summary>
		/// Verificar se um horario string possui um valor estimado maior que zero
		/// </summary>
		/// <param name="tarefa">tarefa atual</param>
		/// <param name="hora">propriedade tipo hora</param>
		/// <returns>retorna se possui ou não tempo estimado</returns>
		protected bool PossuiTempoEstimado( double hora )
		{
			return hora > 0;
		}

		/// <summary>
		/// Método para verificar se uma tarefa é nula
		/// </summary>
		/// <param name="tarefa"></param>
		/// <returns></returns>
		protected bool TarefaNula( CronogramaTarefaDto tarefa )
		{
			return tarefa == null;
		}

		/// <summary>
		/// Método para verificar se uma propriedade de uma tarefa é nula
		/// </summary>
		/// <param name="parametro"></param>
		/// <returns></returns>
		protected bool PropriedadeNula( Object parametro )
		{
			return parametro == null;
		}
		#endregion

		/// <summary>
		/// Retorna a situação planejamento selecionada por Oid
		/// </summary>
		/// <param name="oidSituacaoPlanejamento"></param>
		/// <returns></returns>
		protected SituacaoPlanejamentoDTO GetSituacaoPlanejamentoSelecionada( Guid oidSituacaoPlanejamento )
		{
			return situacoesPlanejamento.FirstOrDefault( o => o.Oid.Equals( oidSituacaoPlanejamento ) );
		}

		/// <summary>
		/// Método responsável por selecionar o dia de trabalho atual
		/// </summary>
		/// <param name="dataSelecionada">data para selecionar o dia atual</param>
		public void SelecionarDiaTrabalho( DateTime dataSelecionada )
		{
			if(string.IsNullOrEmpty( Login ))
				throw new Exception( "Deveria ter sido preenchido o login do colaborador" );

			InicializadorEstimativaDto inicializador = planejamentoServiceClient.ConsultarHorarioUltimaTarefaDiaColaborador( Login, dataSelecionada );
			DiaTrabalhoAtual = inicializador.DiaAtual;
			horaInicial = inicializador.HoraInicialEstimativa;
			SelecionarPeriodoTrabalhoDiaAtual();
			CalcularHoraFinal();
		}

		/// <summary>
		/// Método responsável por selecionar o periodo de trabalho do dia atual
		/// </summary>
		public void SelecionarPeriodoTrabalhoDiaAtual()
		{
			if(DiaTrabalhoAtual != null)
			{
				PeriodosDeTrabalho = DiaTrabalhoAtual.PeriodosTrabalho.OrderBy( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraInicial ) ).ToArray();
				HoraInicioExpediente = new TimeSpan( PeriodosDeTrabalho.Min( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraInicial ) ) );
				HoraFinalExpediente = new TimeSpan( PeriodosDeTrabalho.Max( o => ConversorTimeSpan.ConverterHorasDeStringParaTicks( o.HoraFinal ) ) );
			}
			else
			{
				PeriodoTrabalhoDto periodo = new PeriodoTrabalhoDto( "08:00", "18:00" );
				PeriodosDeTrabalho = new PeriodoTrabalhoDto[1];
				PeriodosDeTrabalho[0] = periodo;
			}
		}

		/// <summary>
		/// Método responsável por calcular o valor da hora final
		/// </summary>
		public bool CalcularHoraFinal()
		{
			TimeSpan inicioPeriodo, fimPeriodo;
			horaFinal = horaInicial + horaRealizada;
			int quantidadePeriodos = 0;
			long horasIntervalo = 0;

			if(PeriodosDeTrabalho != null && PeriodosDeTrabalho.Length > 1)
				foreach(var periodo in PeriodosDeTrabalho)
				{
					inicioPeriodo = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( periodo.HoraInicial );
					fimPeriodo = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( periodo.HoraFinal );

					if(horaInicial > fimPeriodo)
						continue;

					quantidadePeriodos++;
					if(quantidadePeriodos > 1)
					{
						for(int i = 0; i < quantidadePeriodos; i++)
						{
							if(i + 1 < quantidadePeriodos)
							{
								TimeSpan _horaInicio = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( PeriodosDeTrabalho[i + 1].HoraInicial );
								TimeSpan _horaFinal = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( PeriodosDeTrabalho[i].HoraFinal );
								horasIntervalo += _horaInicio.Ticks - _horaFinal.Ticks;
							}
						}
					}

					if(horaFinal <= fimPeriodo)
						break;
				}

			horaFinal += new TimeSpan( horasIntervalo );

			if(horaFinal.Days > 0)
			{
				//calculando a diferenca que será subtraída do esforço realizado restando apenas o esforço máximo atual
				TimeSpan diferenca = horaFinal - new TimeSpan( 24, 0, 0 );
				horaFinal -= diferenca;
				horaRealizada -= diferenca;
				horaRestante += diferenca;

				if(diferenca > TimeSpan.Zero)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Método responsável por calcular o valor da hora final
		/// </summary>
		public TimeSpan CalcularHoraFinal( PeriodoTrabalhoDto[] PeriodosDeTrabalho, TimeSpan horaInicio, ref TimeSpan esforcoRealizado, ref TimeSpan horaRestante, ref bool ultrapassouLimiteDia )
		{
			TimeSpan inicioPeriodo, fimPeriodo;
			TimeSpan horaFinal = horaInicio + esforcoRealizado;
			int quantidadePeriodos = 0;
			long horasIntervalo = 0;

			if(PeriodosDeTrabalho != null && PeriodosDeTrabalho.Length > 1)
				foreach(var periodo in PeriodosDeTrabalho)
				{
					inicioPeriodo = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( periodo.HoraInicial );
					fimPeriodo = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( periodo.HoraFinal );

					if(horaInicio > fimPeriodo)
						continue;

					quantidadePeriodos++;
					if(quantidadePeriodos > 1)
					{
						for(int i = 0; i < quantidadePeriodos; i++)
						{
							if(i + 1 < quantidadePeriodos)
							{
								TimeSpan hi = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( PeriodosDeTrabalho[i + 1].HoraInicial );
								TimeSpan hf = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( PeriodosDeTrabalho[i].HoraFinal );
								horasIntervalo += hi.Ticks - hf.Ticks;
							}
						}
					}

					if(horaFinal <= fimPeriodo)
						break;
				}

			horaFinal += new TimeSpan( horasIntervalo );

			if(horaFinal.Days > 0)
			{
				//calculando a diferenca que será subtraída do esforço realizado restando apenas o esforço máximo atual
				TimeSpan diferenca = horaFinal - new TimeSpan( 24, 0, 0 );
				horaFinal -= diferenca;
				esforcoRealizado -= diferenca;
				horaRestante += diferenca;

				if(diferenca > TimeSpan.Zero)
					ultrapassouLimiteDia = true;
			}
			return horaFinal;
		}

		/// <summary>
		/// Calcula a situação planejamento ideal conforme os dados da tarefa
		/// </summary>
		/// <param name="decorator"></param>
		public void CalcularSituacaoPlanejamentoIdeal( CronogramaTarefaDecorator decorator )
		{
			SituacaoPlanejamentoDTO situacaoSelecionada = GetSituacaoPlanejamentoSelecionada( decorator.OidSituacaoPlanejamentoTarefa );
			if(ValidarSituacaoPlanejamento( situacaoSelecionada.CsTipo,decorator ))
				return;

			if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento , decorator ))
			{
				decorator.OidSituacaoPlanejamentoTarefa = GetOidSituacaoPlanejamentoPorTipo(CsTipoPlanejamento.Planejamento);
				return;
			}

			if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Execução , decorator ))
			{
				decorator.OidSituacaoPlanejamentoTarefa = GetOidSituacaoPlanejamentoPorTipo( CsTipoPlanejamento.Execução );
				return;
			}

			if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Encerramento , decorator ))
			{
				decorator.OidSituacaoPlanejamentoTarefa = GetOidSituacaoPlanejamentoPorTipo( CsTipoPlanejamento.Encerramento );
				return;
			}
		}

		/// <summary>
		/// Retornar o guid de identifação da situação planejamento pelo tipo selecionado
		/// </summary>
		/// <param name="tipo"></param>
		/// <returns></returns>
		private Guid GetOidSituacaoPlanejamentoPorTipo(CsTipoPlanejamento tipo) 
		{
			SituacaoPlanejamentoDTO situacao = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo.Equals( tipo ));
			if(situacao != null)
				return situacao.Oid;
			return new Guid();
		}

	}
}
