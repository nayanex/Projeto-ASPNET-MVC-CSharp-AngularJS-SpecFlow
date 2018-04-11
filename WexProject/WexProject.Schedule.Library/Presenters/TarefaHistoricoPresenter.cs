using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.Schedule.Library.ServiceUtils;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;
using WexProject.Schedule.Library.Properties;
using WexProject.Library.Libs.Delegates.Logger;
using WexProject.Library.Libs.DataHora.Extension;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Library.Libs.Delegates.Tempo;

namespace WexProject.Schedule.Library.Presenters
{
    public class TarefaHistoricoPresenter
    {
		public TarefaHistoricoPresenter()
		{
		}

        #region Atributos
        /// <summary>
        /// View que alimenta o presenter com informações
        /// </summary>
        public ITarefaHistoricoView view;

        /// <summary>
        /// Servico de consumo de dados 
        /// </summary>
        private IPlanejamentoServiceUtil servicoPlanejamento;

        /// <summary>
        /// Servico de consumo de dados 
        /// </summary>
        private IGeralServiceUtil servicoGeral;

        /// <summary>
        /// Atributo Responsável por armazenar a quantidade de horas estimadas inicialmente
        /// </summary>
        private TimeSpan horaEstimativaInicial;

        /// <summary>
        /// responsável por armazenar a hora do inicio do esforço da realização da tarefa
        /// </summary>
        private TimeSpan horaInicial;

        /// <summary>
        /// responsável por armazenar a hora do fim do esforço da realização da tarefa
        /// </summary>
        private TimeSpan horaFinal;

        /// <summary>
        /// responsável por armazenar a(s) horas que restam programadas para a solução da tarefa
        /// </summary>
        private TimeSpan horaRestante;

        /// <summary>
        /// responsável por armazenar a(s) a quantidade de horas realizadas no esforço da tarefa
        /// </summary>
        private TimeSpan horaRealizada;
        /// <summary>
        /// Atributo responsável por armazenar a última hora trabalhada de um determinado colaborador.
        /// </summary>
        private TimeSpan nbUltimaHoraTrabalhadaColaborador;

        /// Responsável por armazenar a necessidade de visibilidade e validação do campo de justificação de redução das horas estimadas
        /// </summary>
        private bool justificativaReducaoAtiva;

        /// <summary>
        /// Atributo responsável por guardar a primeira ocorrência de uma situação planejamento do tipo Planejamento.
        /// </summary>
        private static SituacaoPlanejamentoDTO situacaoPlanejamentoTipoPlanejamento;

        /// <summary>
        /// Atributo responsável por guardar a primeira ocorrência de uma situação planejamento do tipo Execucao.
        /// </summary>
        private static SituacaoPlanejamentoDTO situacaoPlanejamentoTipoExecucao;

        /// <summary>
        /// Atributo responsável por guardar a primeira ocorrência de uma situação planejamento do tipo Encerramento.
        /// </summary>
        private static SituacaoPlanejamentoDTO situacaoPlanejamentoTipoEncerramento;

        /// <summary>
        /// Atributo responsável por guardar a primeira ocorrência de uma situação planejamento do tipo Impedimento.
        /// </summary>
        private static SituacaoPlanejamentoDTO situacaoPlanejamentoTipoImpedimento;

        /// <summary>
        /// Atributo responsável por guardar a primeira ocorrência de uma situação planejamento do tipo Cancelamento.
        /// </summary>
        private static SituacaoPlanejamentoDTO situacaoPlanejamentoTipoCancelamento;

        /// <summary>
        /// Atributo responsável por guardar todas as situações de planejamento.
        /// </summary>
        public static List<SituacaoPlanejamentoDTO> situacoesPlanejamento;

        /// <summary>
        /// Atributo responsável por guardar a tarefa que está sendo utilizada naquele momento.
        /// </summary>
        public CronogramaTarefaDecorator cronogramaTarefa;

        /// <summary>
        /// Atributo responsável por guardar o login do colaborador que está utilizando o popup.
        /// </summary>
        public string loginColaborador;

        /// <summary>
        /// Atributo que indica se houve ou não alterações.
        /// </summary>
        public bool alteracoes;
        #endregion

        #region Eventos e Delegates

        /// <summary>
        /// evento disparado quando a HoraInicial for inválida
        /// </summary>
        public event AoValidarTempoEventHandler QuandoHoraInicialForInvalida;

        /// <summary>
        /// evento disparado quando a HoraFinal calculada ultrapassar o limite do dia
        /// </summary>
        public event AoValidarTempoEventHandler QuandoHoraFinalUltrapassarMeiaNoite;

        /// <summary>
        /// Evento disparado quando salvar o historico de uma tarefa
        /// </summary>
        public event EventHandler AoSalvarHistorico;

        /// <summary>
        /// Evento disparado quando ocorrer algum erro
        /// </summary>
        public event NotificarLogEventHandler LogarAoOcorrerErro;

        /// <summary>
        /// Evento disparado quando ocorrer algum erro
        /// </summary>
        public event NotificarLogEventHandler LogarAoOcorrerDebug;

        /// <summary>
        /// Evento disparado quando ocorrer algum comportamento que invalide o salvar
        /// </summary>
        public event Action<string> AoInvalidarSalvar;

        #endregion

        #region Propriedades

        /// <summary>
        /// Responsável por armazenar os periodos de trabalho do dia atual
        /// </summary>
        public PeriodoTrabalhoDto[] PeriodosDeTrabalho { get; set; }

        /// <summary>
        /// Atributo responsável por armazenar a quantidade de horas estimadas atualmente  
        /// </summary>
        public TimeSpan estimativaTotal { get; set; }

        /// <summary>
        /// Ultima Hora de trabalhada  do colaborador atual baseada em um dia especifico
        /// </summary>
        public TimeSpan NbUltimaHoraTrabalhadaColaborador
        {
            get { return nbUltimaHoraTrabalhadaColaborador; }
            set { nbUltimaHoraTrabalhadaColaborador = value; }
        }

        /// <summary>
        /// responsável por armazenar a hora do inicio do esforço da realização da tarefa
        /// </summary>
        public TimeSpan HoraInicial { get { return horaInicial; } set { horaInicial = value; } }

        /// <summary>
        /// responsável por armazenar a hora do fim do esforço da realização da tarefa
        /// </summary>
        public TimeSpan HoraFinal { get { return horaFinal; } set { horaFinal = value; } }

        /// <summary>
        /// responsável por armazenar a(s) horas que restam programadas para a solução da tarefa
        /// </summary>
        public TimeSpan HoraRestante { get { return horaRestante; } set { horaRestante = value; } }

        /// <summary>
        /// responsável por armazenar a(s) a quantidade de horas realizadas no esforço da tarefa
        /// </summary>
        public TimeSpan HoraRealizada { get { return horaRealizada; } set { horaRealizada = value; } }

        /// <summary>
        /// Responsável por armazenar a necessidade de visibilidade e validação do campo de justificação de redução das horas estimadas
        /// </summary>
        public bool JustificativaReducaoAtiva { get { return justificativaReducaoAtiva; } }

        /// <summary>
        /// responsável por armazenar a hora de inicio do expediente
        /// </summary>
        public TimeSpan HoraInicioExpediente { get; set; }

        /// <summary>
        /// responsável por armazenar a hora final do expediente
        /// </summary>
        public TimeSpan HoraFinalExpediente { get; set; }

        /// <summary>
        /// responsável por exibir a estimativa inicial
        /// </summary>
        public TimeSpan EstimativaInicial { get { return horaEstimativaInicial; } }

        /// <summary>
        /// Responsável por armazenar o dia de trabalho selecionado
        /// </summary>
        public DiaTrabalhoDto DiaTrabalhoAtual { get; set; }

        /// <summary>
        /// Armazena a hora inicial sugerida pelo serviço
        /// </summary>
        public TimeSpan HoraInicialSugerida { get; private set; }

        /// <summary>
        /// Armazenar o login do colaborador
        /// </summary>
        public string LoginColaborador { get { return loginColaborador; } }

        /// <summary>
        /// Propriedade para armazenar se foi invocado via tecla de atalho
        /// </summary>
        public bool ModoAtalho { get; set; }

        /// <summary>
        /// Armazenar a ultima data sugerida pelo serviço
        /// </summary>
        public DateTime DataSugerida { get; private set; }
        #endregion

        #region Regras de Tela

        /// <summary>
        /// Método responsável por inicializar os valores dos campos do popup a partir do último histórico de uma tarefa, caso não tenha inicializa valores padrões.
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid da Tarefa</param>
        /// <param name="nbEstimativaInicial">Estimativa Inicial (Hora) da tarefa</param>
        public virtual void InicializarHistoricoTarefa( CronogramaTarefaDecorator cronogramaTarefaItem, string login )
        {
            view.Desabilitar();
            view.TxJustificativaDeReducao = "";
            loginColaborador = login;

            InicializadorEstimativaDto inicializador = GetInicializadorEstimativaInicial( login );
            horaInicial = inicializador.HoraInicialEstimativa;
            HoraInicialSugerida = inicializador.HoraInicialEstimativa;
            view.DtRealizado = inicializador.DataEstimativa;
            DiaTrabalhoAtual = inicializador.DiaAtual;
            DataSugerida = inicializador.DataEstimativa;
            SelecionarPeriodoTrabalhoDiaAtual();
            cronogramaTarefa = cronogramaTarefaItem;
            horaEstimativaInicial = cronogramaTarefa.EstimativaInicial ;
            view.OidSituacaoPlanejamento = cronogramaTarefa.OidSituacaoPlanejamentoTarefa;
            view.TxComentario = "";
            horaRealizada = new TimeSpan( 0 );
            horaRestante = cronogramaTarefa.NbEstimativaRestante.ToTimeSpan() ;
            horaFinal = horaInicial;
            estimativaTotal = new TimeSpan( horaRestante.Ticks );
            AtualizarValoresHorariosDaView();
            view.Habilitar();
            //sempre iniciar desativado o painel de justificativa
            view.AlterarEstadoAtivacaoPainelJustificativa( false );
        }

        /// <summary>
        /// Método para verificar se  a hora inicial informada é a mesma que a sugerida pelo serviço
        /// </summary>
        /// <param name="horaInicial"></param>
        /// <returns></returns>
        public bool HoraInicialForAHoraInicialSugerida( string horaInicial )
        {
            return !string.IsNullOrEmpty( horaInicial ) && HoraInicialSugerida == ConversorTimeSpan.CalcularHorasTimeSpan( horaInicial );
        }

        /// <summary>
        /// Método responsável por inicializar os valores dos campos do popup a partir do último histórico de uma tarefa, caso não tenha inicializa valores padrões.
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid da Tarefa</param>
        /// <param name="nbEstimativaInicial">Estimativa Inicial (Hora) da tarefa</param>
        /// <param name="oidSituacaoPlanejamento">oid da situação planejamento que foi setado</param>
        public virtual void InicializarHistoricoTarefa( CronogramaTarefaDecorator cronogramaTarefaDto, string login, Guid oidSituacaoPlanejamento, bool aoIniciarView = false )
        {
            InicializarHistoricoTarefa( cronogramaTarefaDto, login );
            SituacaoPlanejamentoDTO situacaoPlanejamento = situacoesPlanejamento.First( o => o.Oid.Equals( oidSituacaoPlanejamento ) );
            switch(situacaoPlanejamento.CsTipo)
            {
                case CsTipoPlanejamento.Encerramento:
                    SetarTarefaComoEmEncerramento( aoIniciarView );
                    break;
                case CsTipoPlanejamento.Execução:
                    SetarTarefaComoEmExecucao( aoIniciarView );
                    break;
                case CsTipoPlanejamento.Impedimento:
                    SetarTarefaComoEmImpedimento( aoIniciarView );
                    break;
                case CsTipoPlanejamento.Planejamento:
                    SetarTarefaComoEmPlanejamento( aoIniciarView );
                    break;
                case CsTipoPlanejamento.Cancelamento:
                    SetarTarefaComoEmCancelamento();
                    break;
                default:
                    return;
            }
        }

        public virtual InicializadorEstimativaDto GetInicializadorEstimativaInicial( string login )
        {
            return servicoPlanejamento.ConsultarInicializadorEstimativaInicialColaborador( login );
        }



        /// <summary>
        /// Método responsável por verificar se existe um histórico anterior da tarefa.
        /// </summary>
        /// <param name="oidTarefaHistorico">Guid do histórico da tarefa</param>
        /// <returns>Bool se existe ou não</returns>
        public static bool RnValidarHistoricoTarefa( Guid oidTarefaHistorico )
        {
            //Se não existir histórico da tarefa
            if(oidTarefaHistorico == new Guid())
                return false;
            else
                return true;
        }

        /// <summary>
        /// Método responsável por enviar a requisição para que um histórico seja criado para uma determinada tarefa.
        /// Obs: ele pega todos os atributos com os valores atuais que se encontram no popup e os envia para que o histórico seja salvo.
        /// </summary>
        public void RnCriarHistoricoTarefa()
        {
            SituacaoPlanejamentoDTO situacaoAtual = GetSituacaoPlajenamentoAtual();
            if(situacaoAtual != null && ValidarSituacaoPlanejamento( situacaoAtual.CsTipo ))
            {
                if(( !situacaoAtual.CsTipo.Equals( CsTipoPlanejamento.Cancelamento ) && ( !situacaoAtual.CsTipo.Equals( CsTipoPlanejamento.Impedimento )) && !situacaoAtual.CsTipo.Equals( CsTipoPlanejamento.Planejamento ) ) && horaEstimativaInicial.Ticks <= 0)
                {
                    NotificarMensagem( Resources.Caption_Atencao, "Não foi possível salvar a alteração, pois não foi estimado um tempo para a realização da tarefa." );
                    return;
                }

                if(!HoraInicialForAHoraInicialSugerida( view.NbHoraInicial ) && horaRealizada.Ticks <= 0)
                {
                    NotificarMensagem( Resources.Caption_Atencao, "Não foi possível salvar a alteração.\nNão é permitido alterar a hora inicial sem ter informado um esforço realizado." );
                    return;
                }

                if(ModoAtalho && situacaoAtual.CsTipo == CsTipoPlanejamento.Impedimento)
                    horaRealizada = TimeSpan.Zero;

                servicoPlanejamento.CriarHistoricoTarefa( cronogramaTarefa.OidTarefa, loginColaborador, horaRealizada,
                                                                              view.DtRealizado, horaInicial, horaFinal, view.TxComentario,
                                                                              horaRestante, view.OidSituacaoPlanejamento, view.TxJustificativaDeReducao );
                if(AoSalvarHistorico != null)
                    AoSalvarHistorico( cronogramaTarefa, new EventArgs() );
            }
            else
            {
                if(!ModoAtalho)
                    NotificarMensagem( Resources.Caption_SituacaoPlanejamentoInvalida, Resources.Alerta_SituacaoPlanejamentoInvalidaAoSalvar );
            }
        }

        /// <summary>
        /// Método Responsável por realizar atualização da view
        /// </summary>
        private void AtualizarValoresHorariosDaView()
        {
            view.NbHoraFinal = horaFinal.ToString( @"hh\:mm" );
            view.NbHoraInicial = horaInicial.ToString( @"hh\:mm" );
            view.NbHoraRealizado = horaRealizada.ToString( @"hh\:mm" );
            view.NbHoraRestante = ConversorTimeSpan.CalcularStringHoras(horaRestante);
        }

        /// <summary>
        /// Método Responsável por realizar atualização dos valores dos atributos, a partir do preenchimento da view
        /// </summary>
        public void AtualizarValoresAtributos()
        {
            GetValorHoraInicialView();
            horaRealizada = GetValorHoraRealizadoView();
            horaRestante = GetValorHoraRestanteView();
            CalcularHoraFinal();
        }

        /// <summary>
        /// Método responsável por alterar os valores impactados pela mudança do campo Realizado.
        /// </summary>
        public void HoraRealizadoForAlterado()
        {
            horaRealizada = GetValorHoraRealizadoView();
            horaRestante = estimativaTotal - horaRealizada;
            if(horaRestante.Ticks < 0)
                horaRestante = new TimeSpan( 0 );
            CalcularHoraFinal();
            CalcularSituacaoPlanejamento();
            VerificarSeNecessitaJustificativa();
        }


        /// <summary>
        /// Método responsável tratar os campos impactados no view quando a data for alterada
        /// </summary>
        public void DtRealizadoForAlterado()
        {
            SelecionarDiaTrabalho( view.DtRealizado );
            CalcularHoraFinal();
        }

        /// <summary>
        /// Método responsável por selecionar o dia de trabalho atual
        /// </summary>
        /// <param name="dataSelecionada">data para selecionar o dia atual</param>
        public void SelecionarDiaTrabalho( DateTime dataSelecionada )
        {
            InicializadorEstimativaDto inicializador = servicoPlanejamento.ConsultarHorarioUltimaTarefaDiaColaborador( LoginColaborador, dataSelecionada );
            DiaTrabalhoAtual = inicializador.DiaAtual;
            horaInicial = inicializador.HoraInicialEstimativa;
            SelecionarPeriodoTrabalhoDiaAtual();
            CalcularHoraFinal();
            AtualizarValoresHorariosDaView();
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
        public void CalcularHoraFinal()
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
                horaRealizada -= diferenca;
                horaRestante += diferenca;
                if(diferenca > new TimeSpan( 0 ))
                    if(QuandoHoraFinalUltrapassarMeiaNoite != null)
                        QuandoHoraFinalUltrapassarMeiaNoite( diferenca );
            }
            AtualizarValoresHorariosDaView();
        }

        /// <summary>
        /// Método responsável por capturar o NbHoraRealizado da view
        /// </summary>
        /// <returns>Estrutura timespan com o tempo atual da view</returns>
        public virtual TimeSpan GetValorHoraRealizadoView()
        {
            return ConversorTimeSpan.CalcularHorasTimeSpan( view.NbHoraRealizado );
        }

        /// <summary>
        /// Método responsável por capturar o NbHoraRealizado da view
        /// </summary>
        /// <returns>Estrutura timespan com o tempo atual da view</returns>
        public virtual TimeSpan GetValorHoraRestanteView()
        {
            return ConversorTimeSpan.CalcularHorasTimeSpan( view.NbHoraRestante );
        }

        /// <summary>
        /// Método responsável por capturar a horaInicial da view
        /// </summary>
        /// <returns>Estrutura timespan com o tempo atual da view</returns>
        public virtual TimeSpan GetValorHoraInicialView()
        {
            return ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( view.NbHoraInicial );
        }

        /// <summary>
        /// Método responsável por validar e alterar a situação planejamento, quando outros campos influenciarem a situação de planejamento, de acordo com a hora restante e hora realizado.
        /// </summary>
        public virtual void CalcularSituacaoPlanejamento()
        {
            SituacaoPlanejamentoDTO situacaoPlanejamentoAtual = GetSituacaoPlajenamentoAtual();
            if(ValidarSituacaoPlanejamento( situacaoPlanejamentoAtual.CsTipo ))
                return;

            if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ))
            {
                view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoPlanejamento();
                return;
            }

            if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Execução ))
            {
                view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoExecucao();
                return;
            }

            if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Encerramento ))
            {
                view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoEncerramento();
                return;
            }
        }

		/// <summary>
		/// Método responsável por validar e alterar a situação planejamento, quando outros campos influenciarem a situação de planejamento, de acordo com a hora restante e hora realizado.
		/// </summary>
		public virtual void CalcularSituacaoPlanejamento(ref Guid oidSituacaoPlanejamento)
		{
			SituacaoPlanejamentoDTO situacaoPlanejamentoAtual = GetSituacaoPlajenamentoAtual();
			if(ValidarSituacaoPlanejamento( situacaoPlanejamentoAtual.CsTipo ))
				return;

			if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ))
			{
				view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoPlanejamento();
				return;
			}

			if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Execução ))
			{
				view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoExecucao();
				return;
			}

			if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Encerramento ))
			{
				view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoEncerramento();
				return;
			}
		}

        /// <summary>
        /// Retornar a situação planejamento selecionada atualmente
        /// </summary>
        /// <returns></returns>
        public SituacaoPlanejamentoDTO GetSituacaoPlajenamentoAtual()
        {
            return situacoesPlanejamento.FirstOrDefault( o => o.Oid.Equals( view.OidSituacaoPlanejamento ) );
        }

        /// <summary>
        /// Validar se um tipo de situação planejamento é ou não válido considerando as estimativas atuais
        /// </summary>
        /// <param name="tipo">Tipo de situação planejamento</param>
        /// <returns> True para tipo de situação planejamento válido em relação as estimativas e  False para não válido</returns>
        public bool ValidarSituacaoPlanejamento()
        {
            CsTipoPlanejamento tipo = situacoesPlanejamento.First( o => o.Oid.Equals( view.OidSituacaoPlanejamento ) ).CsTipo;
            TimeSpan horaRealizada = GetValorHoraRealizadoView();
            TimeSpan horaRestante = GetValorHoraRestanteView();
            return ValidarSituacaoPlanejamento( tipo, situacoesPlanejamento, horaRealizada, horaRestante, cronogramaTarefa );
        }

        /// <summary>
        /// Validar se um tipo de situação planejamento é ou não válido considerando as estimativas atuais
        /// </summary>
        /// <param name="tipo">Tipo de situação planejamento</param>
        /// <returns> True para tipo de situação planejamento válido em relação as estimativas e  False para não válido</returns>
        public bool ValidarSituacaoPlanejamento( CsTipoPlanejamento tipo )
        {
            TimeSpan horaRealizada = GetValorHoraRealizadoView();
            TimeSpan horaRestante = GetValorHoraRestanteView();
            return ValidarSituacaoPlanejamento( tipo, situacoesPlanejamento, horaRealizada, horaRestante, cronogramaTarefa );
        }

        /// <summary>
        /// Método para validar se a situação planejamento de uma tarefa é válida
        /// </summary>
        /// <param name="tipo">Tipo de situação planejamento</param>
        /// <param name="situacoesPlanejamento">lista das situações planejamento existentes</param>
        /// <param name="horaRealizada">Esforco realizado</param>
        /// <param name="horasRestantes">Esforço restante</param>
        /// <param name="tarefa">Tarefa</param>
        /// <returns></returns>
        public static bool ValidarSituacaoPlanejamento( CsTipoPlanejamento tipo, List<SituacaoPlanejamentoDTO> situacoesPlanejamento, TimeSpan horaRealizada, TimeSpan horaRestante, CronogramaTarefaDecorator tarefa )
        {
            SituacaoPlanejamentoDTO situacao = situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == tipo );
            if(situacao == null)
                return false;

            TimeSpan horasRealizadas =  tarefa.Realizado ;
            horasRealizadas += horaRealizada;
            switch(situacao.CsTipo)
            {
                case CsTipoPlanejamento.Encerramento:
                    return horasRealizadas > TimeSpan.Zero && horaRestante <= TimeSpan.Zero;
                case CsTipoPlanejamento.Execução:
                    return horaRestante > TimeSpan.Zero;
                case CsTipoPlanejamento.Planejamento:
                    return horasRealizadas == TimeSpan.Zero;
                case CsTipoPlanejamento.Cancelamento:
                    return horaRestante == TimeSpan.Zero;
                case CsTipoPlanejamento.Impedimento:
                    //Não há regras para ir para impedimento
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Método responsável pelo comportamento do presenter quando a Estimativa Restante foi alterada
        /// </summary>
        public void HoraRestanteForAlterada()
        {
            VerificarSeNecessitaJustificativa();
            horaRestante = GetValorHoraRestanteView();
            horaRealizada = GetValorHoraRealizadoView();
            CalcularHoraFinal();
            CalcularSituacaoPlanejamento();
        }

        /// <summary>
        /// Método Responsável por Buscar no Serviço o Oid  da Situação Planejamento do Tipo Planejamento
        /// </summary>
        /// <returns>oid da situação planejamento</returns>
        public virtual Guid GetOidSituacaoPlanejamentoTipoPlanejamento()
        {
            if(situacaoPlanejamentoTipoPlanejamento == null)
                return new Guid();

            return situacaoPlanejamentoTipoPlanejamento.Oid;
        }

        /// <summary>
        /// Método Responsável por Buscar no Serviço o Oid  da Situação Planejamento do Tipo Encerramento
        /// </summary>
        /// <returns>oid da situação planejamento</returns>
        public virtual Guid GetOidSituacaoPlanejamentoTipoEncerramento()
        {
            if(situacaoPlanejamentoTipoEncerramento == null)
                return new Guid();

            return situacaoPlanejamentoTipoEncerramento.Oid;
        }

        /// <summary>
        /// Método Responsável por Buscar no Serviço o Oid  da Situação Planejamento do Tipo Cancelamento
        /// </summary>
        /// <returns>oid da situação planejamento</returns>
        public virtual Guid GetOidSituacaoPlanejamentoTipoCancelamento()
        {
            if(situacaoPlanejamentoTipoCancelamento == null)
                return new Guid();

            return situacaoPlanejamentoTipoCancelamento.Oid;
        }

        /// <summary>
        /// Método Responsável por Buscar no Serviço o Oid  da Situação Planejamento do Tipo Impedimento
        /// </summary>
        /// <returns>oid da situação planejamento</returns>
        public virtual Guid GetOidSituacaoPlanejamentoTipoImpedimento()
        {
            if(situacaoPlanejamentoTipoImpedimento == null)
                return new Guid();

            return situacaoPlanejamentoTipoImpedimento.Oid;
        }

        /// <summary>
        /// Método responsável por verificar a validade da Hora Inicial
        /// </summary>
        public void HoraFinalForAlterada()
        {
            horaInicial = GetValorHoraInicialView();
            CalcularHoraFinal();
            CalcularSituacaoPlanejamento();
        }

        /// <summary>
        /// Método responsável pelo comportamento do presenter quando a  hora restante for diminuida
        /// </summary>
        public void VerificarSeNecessitaJustificativa()
        {
            TimeSpan horaRestanteView = GetValorHoraRestanteView();
            horaRealizada = GetValorHoraRealizadoView();
            if(( horaRestanteView < horaRestante || horaRestanteView.Ticks < cronogramaTarefa.NbEstimativaRestante  ) && horaRealizada == TimeSpan.Zero)
            {
                view.AlterarEstadoAtivacaoPainelJustificativa( true );
                justificativaReducaoAtiva = true;
            }
            else
            {
                view.AlterarEstadoAtivacaoPainelJustificativa( false );
                justificativaReducaoAtiva = false;
                view.TxJustificativaDeReducao = "";
            }
        }

        /// <summary>
        /// Método utilizado para retornar data atual
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetDataAtual()
        {
            return DateTime.Now;
        }
        /// <summary>
        /// Méetodo responsável por retornar ocorrencia da primeira hora de trabalho dos periodos de trabalho atuais
        /// </summary>
        /// <returns>hora de inicio do dia de trabalho</returns>
        public TimeSpan GetHoraInicioPrimeiroPeriodoDeTrabalho()
        {
            TimeSpan horaInicioDiaDeTrabalho;
            if(PeriodosDeTrabalho != null && PeriodosDeTrabalho.Length > 0)
            {
                horaInicioDiaDeTrabalho = PeriodosDeTrabalho.Min( o => ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( o.HoraInicial ) );
            }
            else
                horaInicioDiaDeTrabalho = new TimeSpan( 0 );
            return horaInicioDiaDeTrabalho;
        }

        /// <summary>
        /// Método responsável por retornar a semana programada de trabalho
        /// </summary>
        /// <returns>retorna a semana programada de trabalho</returns>
        public virtual SemanaTrabalhoDto GetSemanaTrabalhoDto()
        {
            return servicoGeral.ConsultarSemanaDeTrabalhoPadrao();
        }

        /// <summary>
        /// Método responsável por validar a situação planejamento quando o usuário alterar diretamente no combo de situações planejamento.
        /// </summary>
        /// <param name="autoCalcular">Indicar se deve ou não recalcular a situação planejamento caso seja inválida</param>
        public void AlterarSituacaoPlanejamento( bool autoCalcular = true )
        {
            SituacaoPlanejamentoDTO viewSituacaoPlanejamento = GetSituacaoPlanejamento( view.OidSituacaoPlanejamento );
            switch(viewSituacaoPlanejamento.CsTipo)
            {
                case CsTipoPlanejamento.Encerramento:
                    SetarTarefaComoEmEncerramento( false, autoCalcular );
                    break;
                case CsTipoPlanejamento.Execução:
                    SetarTarefaComoEmExecucao( false, autoCalcular );
                    break;
                case CsTipoPlanejamento.Impedimento:
                    SetarTarefaComoEmImpedimento( false, autoCalcular );
                    break;
                case CsTipoPlanejamento.Planejamento:
                    SetarTarefaComoEmPlanejamento( false, autoCalcular );
                    break;
                case CsTipoPlanejamento.Cancelamento:
                    SetarTarefaComoEmCancelamento();
                    break;
            }
        }

        /// <summary>
        /// Método para selecionar a situação planejamento escolhida na view
        /// </summary>
        /// <param name="oidSituacaoPlanejamento">oid de identificação da situação planejamento selecionada</param>
        /// <returns>situação planejamento selecionada</returns>
        public SituacaoPlanejamentoDTO GetSituacaoPlanejamento( Guid oidSituacaoPlanejamento )
        {
            SituacaoPlanejamentoDTO viewSituacaoPlanejamento = situacoesPlanejamento.First( o => o.Oid.Equals( oidSituacaoPlanejamento ) );
            return viewSituacaoPlanejamento;
        }

        /// <summary>
        /// Método responsável tentar setar a situação planejamento como Encerramento (Pronto)
        /// </summary>
        /// <param name="aoIniciarView">Indica se a alteração está ocorrendo enquanto está sendo iniciada a view</param>
        /// <param name="autoCalcular">Indicar se deve ou não recalcular a situação planejamento caso seja inválida</param>
        public void SetarTarefaComoEmEncerramento( bool aoIniciarView = false, bool autoCalcular = true )
        {
            if(horaRealizada.Ticks <= 0)
                horaRealizada = new TimeSpan( horaRestante.Ticks );
            else
                if(horaRestante.Ticks > 0)
                    horaRealizada += horaRestante;
            horaRestante = new TimeSpan( 0 );
            AtualizarValoresHorariosDaView();

            if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Encerramento ))
            {
                view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoEncerramento();
                CalcularHoraFinal();
                VerificarSeNecessitaJustificativa();
            }
            else
            {
                if(!aoIniciarView)
                {
                    string mensagem = string.Format( "{0} Deveria haver esforço realizado. Informe algum esforço.", Resources.Alerta_SituacaoPlanejamentoInvalida );
                    NotificarMensagem( Resources.Caption_SituacaoPlanejamentoInvalida, mensagem );
                }
                if(autoCalcular)
                {
                    AtualizarValoresHorariosDaView();
                    CalcularSituacaoPlanejamento();
                }
            }
        }

        /// <summary>
        /// Método responsável tentar setar a situação planejamento como Cancelamento (Cancelado)
        /// </summary>
        public void SetarTarefaComoEmCancelamento()
        {
            SituacaoPlanejamentoDTO situacaoPlanejamento = situacoesPlanejamento.First( o => o.CsTipo.Equals( CsTipoPlanejamento.Cancelamento ) );
            horaRestante = new TimeSpan( 0 );
            view.OidSituacaoPlanejamento = situacaoPlanejamento.Oid;
            horaFinal = horaInicial;
            CalcularHoraFinal();
            VerificarSeNecessitaJustificativa();
        }

        /// <summary>
        /// Método responsável tentar setar a situação planejamento como execução (Em andamento)
        /// </summary>
        /// <param name="aoIniciarView">Indica se a alteração está ocorrendo enquanto está sendo iniciada a view</param>
        /// <param name="autoCalcular">Indicar se deve ou não recalcular a situação planejamento caso seja inválida</param>
        public void SetarTarefaComoEmExecucao( bool aoIniciarView = false, bool autoCalcular = true )
        {
            if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Execução ))
                view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoExecucao();
            else
            {
                if(!aoIniciarView)
                {
                    NotificarMensagem( Resources.Caption_SituacaoPlanejamentoInvalida, string.Format( "{0} Deveria conter horas restantes.", Resources.Alerta_SituacaoPlanejamentoInvalida ) );
                }
                if(autoCalcular)
                    CalcularSituacaoPlanejamento();
            }
            VerificarSeNecessitaJustificativa();
        }

        /// <summary>
        /// Método responsável tentar setar a situação planejamento como Impedimento (Impedido)
        /// </summary>
        /// <param name="aoIniciarView">Indica se a alteração está ocorrendo enquanto está sendo iniciada a view</param>
        /// <param name="autoCalcular">Indicar se deve ou não recalcular a situação planejamento caso seja inválida</param>
        public void SetarTarefaComoEmImpedimento( bool aoIniciarView = false, bool autoCalcular = true )
        {
            view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoImpedimento();
            VerificarSeNecessitaJustificativa();
        }

        /// <summary>
        /// Método que efetua o disparo de uma mensagem para a view
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensagem"></param>
        private void NotificarMensagem( string titulo, string mensagem )
        {
            view.NotificarMensagem( titulo, mensagem );
            if(AoInvalidarSalvar != null)
                AoInvalidarSalvar( mensagem );
        }

        /// <summary>
        /// Método responsável tentar setar a situação planejamento como Planejamento (Não iniciado)
        /// </summary>
        /// <param name="aoIniciarView">Indica se a alteração está ocorrendo enquanto está sendo iniciada a view</param>
        /// <param name="autoCalcular">Indicar se deve ou não recalcular a situação planejamento caso seja inválida</param>
        public void SetarTarefaComoEmPlanejamento( bool aoIniciarView = false, bool autoCalcular = true )
        {
            if(ValidarSituacaoPlanejamento( CsTipoPlanejamento.Planejamento ))
                view.OidSituacaoPlanejamento = GetOidSituacaoPlanejamentoTipoPlanejamento();
            else
            {
                if(!aoIniciarView)
                    NotificarMensagem( Resources.Caption_SituacaoPlanejamentoInvalida, string.Format( "{0} Não deveria haver esforço realizado.", Resources.Alerta_SituacaoPlanejamentoInvalida ) );
                if(autoCalcular)
                    CalcularSituacaoPlanejamento();
            }
            VerificarSeNecessitaJustificativa();
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Método Responsável por Buscar no Serviço o Oid  da Situação Planejamento do Tipo Execução
        /// </summary>
        /// <returns></returns>
        public virtual Guid GetOidSituacaoPlanejamentoTipoExecucao()
        {
            if(situacaoPlanejamentoTipoExecucao == null)
                return new Guid();

            return situacaoPlanejamentoTipoExecucao.Oid;
        }

        /// <summary>
        /// Método responsável por requisitar ao serviço o histórico mais atual de uma determinada tarefa.
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid da Tarefa</param>
        /// <returns>objeto TarefaHistoricoTrabalhoDto</returns>
        public virtual TarefaHistoricoTrabalhoDto GetTarefaHistoricoTrabalhoAtual( Guid oidTarefa )
        {
            TarefaHistoricoTrabalhoDto historicoDto = servicoPlanejamento.ConsultarTarefaHistoricoTrabalhoAtual( oidTarefa );
            return historicoDto;
        }

        #endregion

        #region Utilitários

        /// <summary>
        /// Método responsável por buscar a situação planejamento de cada tipo ordenada por ordem alfabética.
        /// </summary>
        /// <param name="situacoesPlanejamento">Lista contendo todas as situações de planejamento</param>
        public void CarregarSituacoesPlanejamentoPorTipo( List<SituacaoPlanejamentoDTO> situacoesPlanejamentoDto )
        {
            //preenche o atributo estático
            situacoesPlanejamento = situacoesPlanejamentoDto;

            //retirando valores de referência
            List<SituacaoPlanejamentoDTO> situacoesAtivas = new List<SituacaoPlanejamentoDTO>( situacoesPlanejamento );

            if(situacoesAtivas.Count <= 0)
                return;

            List<SituacaoPlanejamentoDTO> selecao = situacoesAtivas.GroupBy( o => o.CsTipo ).Select( o => o.First() ).ToList();
            situacaoPlanejamentoTipoPlanejamento = selecao.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Planejamento ) );
            situacaoPlanejamentoTipoExecucao = selecao.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Execução ) );
            situacaoPlanejamentoTipoEncerramento = selecao.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Encerramento ) );
            situacaoPlanejamentoTipoImpedimento = selecao.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Impedimento ) );
            situacaoPlanejamentoTipoCancelamento = selecao.FirstOrDefault( o => o.CsTipo.Equals( CsTipoPlanejamento.Cancelamento ) );
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor do Presenter
        /// </summary>
        /// <param name="view">objeto controlado pelo presenter</param>
        public TarefaHistoricoPresenter( ITarefaHistoricoView view )
        {
            this.view = view;
            servicoPlanejamento = CronogramaPresenter.ServicoPlanejamento;
            servicoGeral = CronogramaPresenter.ServicoGeral;
        }

        #endregion
    }
}
