using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WexProject.Schedule.Library.Presenters;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.Schedule.Library.Views.Forms;
using DevExpress.XtraEditors;
using WexProject.BLL.Shared.Domains.Planejamento;
using System.Diagnostics;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;
using WexProject.Schedule.Library.Properties;
using System.Drawing;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Views.Components
{
    public partial class TarefaHistoricoView : XtraUserControl, ITarefaHistoricoView
    {

        #region Atributos

        /// <summary>
        /// Prensenter representante da view, responsável por tratar as regras de tela, refletindo diretamente na view
        /// </summary>
        protected TarefaHistoricoPresenter presenter;

        bool FoiInicializado { get; set; }
        #endregion

        #region Propriedades

        /// <summary>
        /// Propriedade responsável por armazenar a quantidade de hora consumidas no esforço para realização da tarefa
        /// </summary>
        public string NbHoraRealizado
        {
            get { return (string)popupNbHoraRealizado.EditValue; }
            set { popupNbHoraRealizado.EditValue = value; }
        }

        /// <summary>
        /// Propriedade responsável por  armazenar a data de inicio do esforço de resolução de uma determinada tarefa
        /// </summary>
        public DateTime DtRealizado
        {
            get
            {
                if(popupDtRealizado.EditValue == null)
                    return new DateTime();
                return Convert.ToDateTime( popupDtRealizado.EditValue );
            }
            set { popupDtRealizado.EditValue = value; }
        }

        /// <summary>
        /// Propriedade responsável por armazenar o Horário Inicial do esforço realizado
        /// </summary>
        public string NbHoraInicial
        {
            get
            { return (string)popupNbHoraInicial.EditValue; }
            set
            { popupNbHoraInicial.EditValue = value; }
        }

        /// <summary>
        /// Propriedade responsável por armazenar o Horário final do esforço realizado
        /// </summary>
        public string NbHoraFinal
        {
            get { return (string)popupNbHoraFinal.Text; }
            set { popupNbHoraFinal.Text = value; }
        }

        /// <summary>
        /// Propriedade responsável por armazenar eventuais comentários sobre a realização da tarefa
        /// </summary>
        public string TxComentario
        {
            get { return Convert.ToString( popupTxComentario.EditValue ); }
            set { popupTxComentario.EditValue = value; }
        }

        /// <summary>
        /// Propriedade responsável por armazenar a quantidade tempo restante para a realização de uma tarefa
        /// </summary>
        public string NbHoraRestante
        {
            get
            {
                return (string)popupNbHoraRestante.EditValue;
            }
            set
            {
                popupNbHoraRestante.EditValue = value;
            }
        }

        /// <summary>
        /// Propriedade responsável por armazenar o Oid Valor da Situação Planejamento
        /// </summary>
        public Guid OidSituacaoPlanejamento
        {
            get
            {
                if(popupComboImagensSituacaoPlanejamento.EditValue == null)
                    return new Guid();

                return (Guid)popupComboImagensSituacaoPlanejamento.EditValue;
            }
            set
            {
                popupComboImagensSituacaoPlanejamento.EditValue = value;
            }
        }

        /// <summary>
        /// Propriedade responsável por armazenar a justificativa da redução de estimativa
        /// </summary>
        public string TxJustificativaDeReducao
        {
            get
            {
                return Convert.ToString( popupTxJustificativaDeReducao.EditValue );
            }
            set
            {
                popupTxJustificativaDeReducao.EditValue = value;
            }
        }

        #endregion

        #region Regras de Tela

        /// <summary>
        /// Método responsável por configurações de inicialização do presenter
        /// </summary>
        public virtual void InicializarPresenter()
        {
            presenter = new TarefaHistoricoPresenter( this );
            CronogramaView.autorizarSalvarEdicao = false;
            presenter.QuandoHoraFinalUltrapassarMeiaNoite += QuandoHoraFinalCalculadaUltrapassarOLimiteDoDia;
        }

        /// <summary>
        /// Método responsável por acessar o presenter e inicializar os valores do popup.
        /// </summary>
        /// <param name="cronogramaTarefaItem">Objeto Dto de CronogramaTarefa</param>
        /// <param name="login">Login do usuário</param>
        /// <param name="situacoesPlanejamento">Lista contendo todas as situações de planejamento</param>
        public void InicializarJanela( CronogramaTarefaDecorator cronogramaTarefaItem, string login )
        {
            presenter.InicializarHistoricoTarefa( cronogramaTarefaItem, login );
            popupNbHoraRealizado.Focus();
            FoiInicializado = true;
        }

        /// <summary>
        /// Método responsável por acessar o presenter e inicializar os valores do popup.
        /// </summary>
        /// <param name="cronogramaTarefaItem">Objeto Dto de CronogramaTarefa</param>
        /// <param name="login">Login do usuário</param>
        /// <param name="situacoesPlanejamento">Lista contendo todas as situações de planejamento</param>
        public void InicializarJanela( CronogramaTarefaDecorator cronogramaTarefaItem, string login, Guid oidSituacaoPlanejamento )
        {
            presenter.InicializarHistoricoTarefa( cronogramaTarefaItem, login, oidSituacaoPlanejamento, true );
            FoiInicializado = true;
        }

        /// <summary>
        /// Método responsável por carregar as situações de planejamento dentro do presenter.
        /// </summary>
        /// <param name="situacoes"></param>
        public void InicializarSituacoesPlanejamento( List<SituacaoPlanejamentoDTO> situacoes )
        {
            presenter.CarregarSituacoesPlanejamentoPorTipo( situacoes );
        }

        /// <summary>
        /// Método utilizado para que o presenter consiga modificar o estado de ativação do painal que armaneza o campo de justificativa de redução de horas restantes
        /// </summary>
        /// <param name="ativo">booleano para indicar a a possibilidade de edição do campo justificativa restante</param>
        public void AlterarEstadoAtivacaoPainelJustificativa( bool ativo )
        {
            panJustificativa.Enabled = ativo;
        }

        /// <summary>
        /// Método responsável pelo comportamento da tela quando o horario final calculado ultrapassar o limite do dia atual(meia-noite)
        /// </summary>
        /// <param name="diferenca">Diferença de horas calculada</param>
        public void QuandoHoraFinalCalculadaUltrapassarOLimiteDoDia( TimeSpan diferenca )
        {
            //Todo: refatorar esta mensagem
            string mensagem = string.Format( "Horário limite 00:00," +
                   " considere estimar {0}h(s) ao próximo dia!", diferenca.ToString( @"hh\:mm" ) );
            NotificarMensagem( "Limite atingido", mensagem );
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Ao salvar histórico.
        /// </summary>
        /// <param name="oidCronogramaTarefa"></param>
        public delegate void AtualizarLinhaGridEventHandler( Guid oidCronogramaTarefa );

        /// <summary>
        /// Ao fechar popup
        /// </summary>
        public delegate void FecharPopupGridEventHandler( TarefaHistoricoView tarefaHistoricoView );

        #endregion

        #region Eventos

        /// <summary>
        /// Evento disparado quando é salvo o histórico de uma tarefa
        /// </summary>
        public event AtualizarLinhaGridEventHandler AoSalvarHistoricoTarefa;

        /// <summary>
        /// Evento disparado quando o botão cancelar ou ok for clicado.
        /// </summary>
        public event FecharPopupGridEventHandler AoFecharJanela;

        /// <summary>
        /// Método responsável pelo comportamento de tela do campo hora inicial enquanto estiver sendo modificado
        /// </summary>
        /// <param name="sender">objeto que efetuou o disparo do evento</param>
        /// <param name="e">paramentros relativos ao disparo do evento</param>
        private void popupNbHoraInicial_EditValueChanged( object sender, EventArgs e )
        {
            if(!FoiInicializado)
                return;
            //sincronizar o valor do atributo com o valor preenchido no presenter
            //verificação das modificações na hora inicial e recalcular hora final caso haja alteração
            presenter.HoraFinalForAlterada();
        }

        /// <summary>
        /// Método responsável pelo comportamento da view quando o valor de horas consumidas na tarefa for alterada
        /// </summary>
        /// <param name="sender">objeto que efetuou o disparo do evento</param>
        /// <param name="e">paramentros relativos ao disparo do evento</param>
        private void popupNbHoraRealizado_EditValueChanged( object sender, EventArgs e )
        {
            //presenter.HoraRealizadoForAlterado();
            //presenter.AtivarJustificativaCasoHoraRestanteForDiminuida();
        }

        /// <summary>
        /// método responsável pelo comportamento do campo hora restante quando sair de edição
        /// </summary>
        /// <param name="sender">objeto que efetuou o disparo do evento</param>
        /// <param name="e">paramentros relativos ao disparo do evento</param>
        private void popupNbHoraRestante_Leave( object sender, EventArgs e )
        {
            if(!FoiInicializado)
                return;
            presenter.HoraRestanteForAlterada();
        }

        /// <summary>
        /// método responsável pelo comportamento da tela quando a data realizado for alterada.
        /// </summary>
        /// <param name="sender">objeto que efetuou o disparo do evento</param>
        /// <param name="e">paramentros relativos ao disparo do evento</param>
        private void popupDtRealizado_Closed( object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e )
        {
            presenter.DtRealizadoForAlterado();
        }

        /// <summary>
        /// (Evento) Método que é acionado quando o combo de Situação Planejamento é fechado.
        /// Obs: ele altera a situação e envia para a validação.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupComboImagensSituacaoPlanejamento_Properties_Closed( object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e )
        {
            Guid oidSituacaoPlanejamentoAnterior = (Guid)popupComboImagensSituacaoPlanejamento.OldEditValue;
            presenter.AlterarSituacaoPlanejamento();
        }

        /// <summary>
        /// (Evento) Método que é acionado quando o botão OK do popup é acionado pelo usuário.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupBtOk_Click( object sender, EventArgs e )
        {
            if(presenter.JustificativaReducaoAtiva)
            {
                if(String.IsNullOrWhiteSpace( presenter.view.TxJustificativaDeReducao ) || String.IsNullOrEmpty( presenter.view.TxJustificativaDeReducao ))
                {
                    NotificarMensagem( "Preenchimento obrigatório", "Você precisa preencher o campo justificativa de redução." );
                    return;
                }
            }

            if(presenter.HoraRestante == new TimeSpan(0,0,0))
            {
                if(TarefaHistoricoPresenter.situacoesPlanejamento.FirstOrDefault( o => o.Oid == presenter.view.OidSituacaoPlanejamento ).CsTipo == CsTipoPlanejamento.Cancelamento)
                {
                    NotificarMensagem( "Preenchimento obrigatório", "Você precisa estimar o campo Horas Restante." );
                    return;
                } 
            }

            presenter.RnCriarHistoricoTarefa();
            AoSalvarHistoricoTarefa( presenter.cronogramaTarefa.OidCronogramaTarefa );
            if(AoFecharJanela != null)
                AoFecharJanela( this );
        }

        /// <summary>
        /// Método disparado quando o botão de cancelar é clicado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupBtCancelar_Click( object sender, EventArgs e )
        {
            if(AoFecharJanela != null)
                AoFecharJanela( this );
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor da classe, representa a tela de CronogramaView para que as regras possam ser realizadas
        /// </summary>
        public TarefaHistoricoView()
        {
            InitializeComponent();
            InicializarPresenter();
        }

        #endregion

        private void popupNbHoraRealizado_Leave( object sender, EventArgs e )
        {
            if(!FoiInicializado)
                return;

            presenter.HoraRealizadoForAlterado();
        }

        /// <summary>
        /// Método para chamar uma mensagem de tela
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensagem"></param>
        public void NotificarMensagem( string titulo, string mensagem )
        {
            XtraMessageBox.Show( LookAndFeel, this, mensagem, titulo );
        }

        /// <summary>
        /// Desabilitar TarefaHistoricoView
        /// </summary>
        public void Desabilitar()
        {
            Enabled = false;
        }

        /// <summary>
        /// Habilitar TarefaHistoricoView
        /// </summary>
        public void Habilitar()
        {
            Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupDtRealizado_EditValueChanged( object sender, EventArgs e )
        {
            DateEdit input = sender as DateEdit;
            if(input.EditValue == null)
                input.EditValue = presenter.DataSugerida;
            else
            {
                DateTime dataSelecionada = (DateTime)input.EditValue;
                if(dataSelecionada.Year < DateTime.Now.Year - 3)
                {
                    input.EditValue = presenter.DataSugerida;
                }
            }
        }
    }
}
