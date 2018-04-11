using System;
using System.Collections.Generic;
using WexProject.BLL.Models.Rh;
using DevExpress.XtraEditors;
using WexProject.BLL.Models.Planejamento;
using System.ComponentModel;
using DevExpress.ExpressApp;
using WexProject.BLL.Models.Escopo;
using DevExpress.ExpressApp.SystemModule;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.NovosNegocios;

namespace WexProject.Module.Lib.ControllerPraWinWeb
{
    /// <summary>
    /// Controller geral do Wex atua tanto no web e win
    /// e não está ligado a nenhum objeto e nenhum tipo de view
    /// </summary>
    public partial class WexProjectViewController : ViewController
    {
        #region Properties

        /// <summary>
        /// Objeto de Estoria
        /// </summary>        
        public static Estoria Estoria { get; set; }

        /// <summary>
        /// Objeto de ConfiguracaoDocumentoSituacao
        /// </summary>
        public static ConfiguracaoDocumentoSituacao ConfiguracaoDocumentoS { get; set; }

        /// <summary>
        /// Controller de Detail
        /// </summary>
        public DetailViewController DetailController { get; set; }

        /// <summary>
        /// Type do objeto atual
        /// </summary>
        private string currentObjectType = string.Empty;

        /// <summary>
        /// Lista de objetos aceitos para verificação
        /// </summary>
        private List<string> listObjectTypes = new List<string>
        {
            "TipoAfastamento",
            "Pais",
            "ConfiguracaoDocumentoSituacao",
            "SolicitacaoOrcamento"
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Construtor
        /// </summary>
        public WexProjectViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Ao ativar o controller
        /// </summary>
        protected override void OnActivated()
        {
            DetailController = Frame.GetController<DetailViewController>();
            currentObjectType = View.ObjectTypeInfo != null ? View.ObjectTypeInfo.Type.Name : string.Empty;

            if (DetailController != null && listObjectTypes.Contains(currentObjectType))
            {
                DetailController.SaveAction.Executing += SaveAction_Executing;
                DetailController.SaveAndNewAction.Executing += SaveAction_Executing;
                DetailController.SaveAndCloseAction.Executing += SaveAction_Executing;
            }

            if (Application is DevExpress.ExpressApp.Win.WinApplication)
            {
                if (Application.MainWindow != null)
                {
                    ((DevExpress.ExpressApp.Win.WinWindow)Application.MainWindow).Form.GotFocus += Form_GotFocus;
                }
            }

            Frame.ViewChanged += Frame_ViewChanged;
            
            base.OnActivated();
        }

        /// <summary>
        /// Desativação de controller
        /// </summary>
        protected override void OnDeactivated()
        {
            if (DetailController != null && listObjectTypes.Contains(currentObjectType))
            {
                DetailController.SaveAction.Executing -= SaveAction_Executing;
                DetailController.SaveAndNewAction.Executing -= SaveAction_Executing;
                DetailController.SaveAndCloseAction.Executing -= SaveAction_Executing;
            }

            Frame.ViewChanged -= Frame_ViewChanged;

            if (Application is DevExpress.ExpressApp.Win.WinApplication)
            {
                if (Application.MainWindow != null)
                {
                    ((DevExpress.ExpressApp.Win.WinWindow)Application.MainWindow).Form.GotFocus -= Form_GotFocus;
                }
            }

            base.OnDeactivated();
        }

        #endregion
        
        #region Utils

        /// <summary>
        /// Verifica a escolha feita pelo usuário
        /// </summary>
        /// <returns>Prosseguir?</returns>
        private bool VerificarEscolhaUsuarioTipoAfastamento()
        {
            if (!(View.CurrentObject is TipoAfastamento))
            {
                return true;
            }

            bool verificacao = (View.CurrentObject as TipoAfastamento).
                RnVerificarExistenciaOutroTipoAfastamentoParaFeriasRealizadas();

            if (!verificacao || (verificacao && XtraMessageBox.Show("Já existe um tipo de afastamento para férias realizadas.\n" +
                "Deseja que o atual se torne para férias realizadas e que\no existente se torne para férias não realizadas?", "Aviso",
                System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation,
                System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verificar escolha do usuário para o País padrão
        /// </summary>
        /// <returns>Prosseguir?</returns>
        private bool VerificarEscolhaUsuarioPais()
        {
            if (!(View.CurrentObject is Pais))
            {
                return true;
            }

            bool verificacao = (View.CurrentObject as Pais).RnIsExibirJanelaMudancaPaisPadrao();

            if (!verificacao || (verificacao && XtraMessageBox.Show("Já existe um país padrão. Deseja que o atual\npasse " +
                "a ocupar essa posição a partir de agora?", "Aviso",
                System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation,
                System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
            {
                if (verificacao)
                {
                    (View.CurrentObject as Pais).RnMudarPaisPadrao();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Verificar escolha do usuário para o Situação Inicial
        /// </summary>
        /// <returns>Prosseguir?</returns>
        private bool VerificarEscolhaSituacaoInicialConfiguracaoDocumento()
        {
            if (!(View.CurrentObject is ConfiguracaoDocumentoSituacao))
            {
                return true;
            }

            bool verificacao = (View.CurrentObject as ConfiguracaoDocumentoSituacao).RnIsExibirJanelaMudancaoSituacaoPlanejamento();

            if (!verificacao || (verificacao && XtraMessageBox.Show("Já existe uma situação inicial definida. Deseja que o atual\npasse " +
                "a ocupar essa posição a partir de agora?", "Aviso",
                System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation,
                System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes))
            {
                if (verificacao)
                {
                    (View.CurrentObject as ConfiguracaoDocumentoSituacao).RnTrocaSituacaoInicial();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// EnableSavesAction
        /// </summary>
        /// <returns>True</returns>
        private bool EnableSavesAction() 
        {
            if (VerificarEscolhaUsuarioTipoAfastamento() && VerificarEscolhaUsuarioPais()
                && VerificarEscolhaSituacaoInicialConfiguracaoDocumento()) 
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Quando estiver executando o SaveAndNew
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        public void SaveAndNewAction_Executing(object sender, CancelEventArgs e)
        {
            if (View.CurrentObject is Estoria)
            {
                Estoria = (Estoria)View.CurrentObject;
            }

            if (View.CurrentObject is ConfiguracaoDocumentoSituacao)
            {
                ConfiguracaoDocumentoS = (ConfiguracaoDocumentoSituacao)View.CurrentObject;
            }

            e.Cancel = EnableSavesAction();
        }

        /// <summary>
        /// View Changed do Frame
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void Frame_ViewChanged(object sender, ViewChangedEventArgs e)
        {
            if (View.CurrentObject != null && Estoria != null && View.CurrentObject is Estoria)
            {
                Estoria.GetDadosEstoriaCurrent(((ObjectSpace)View.ObjectSpace).Session,(Estoria)View.CurrentObject,Estoria);
                Estoria = null;
            }
        }

        /// <summary>
        /// Evento disparado quando o ato de Salvar estiver sendo executado
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CancelEventArgs</param>
        public void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            e.Cancel = EnableSavesAction();
        }

        /// <summary>
        /// Evento para atualizar o list view de país assim que for mudado
        /// o País padrão
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        public void Form_GotFocus(object sender, EventArgs e)
        {
            if (View.Id == Application.FindListViewId(typeof(Pais)))
            {
                ((ListView)View).ObjectSpace.Refresh();
            }
        }

        #endregion
        
    }
}