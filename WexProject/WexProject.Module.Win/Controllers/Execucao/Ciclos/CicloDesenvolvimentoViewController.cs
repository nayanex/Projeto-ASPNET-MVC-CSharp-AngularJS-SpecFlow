using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Xpo;
using WexProject.BLL.Models.Execucao;
using WexProject.Module.Win.TelasForaPadrao.Execucao;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Editors;
using WexProject.BLL.Shared.Domains.Execucao;

namespace WexProject.Module.Win.TelasForaPadrao.Projeto.Ciclo
{
    /// <summary>
    /// Controller para a Classe CicloDesenvolvimento
    /// </summary>
    public partial class CicloDesenvolvimentoViewController : ViewController
    {
        #region Properties
        /// <summary>
        /// Sessão atual para este controller
        /// </summary>
        private Session Session { get; set; }
        /// <summary>
        /// Lista de ciclos selecionados para o cancelamento
        /// ou
        /// para os itens pendentes do ciclo
        /// </summary>
        private CicloDesenv Ciclo { get; set; }
        /// <summary>
        /// Referência do Formulário de Decisão do Ciclo
        /// </summary>
        private CicloDecisaoEstoriasPendentesForm FormDecisaoCiclo { get; set; }

        private CancelamentoCicloForm FormCancelamentoCiclo { get; set; }
        /// <summary>
        /// Flag indica se pode ser executada a ação de cancelar um ciclo
        /// </summary>
        private bool CanCancel { get; set; }

        #endregion

        #region Constructors
        public CicloDesenvolvimentoViewController()
        {
            InitializeComponent();
            RegisterActions(components);

        }
        #endregion

        #region Events
        /// <summary>
        /// Evento Disparado quando o botão de Cancelamento de Ciclo é clicado
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SimpleActionExecuteEventArgs</param>
        private void BtCancelCiclo_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (CanCancel)
            {
                CallPopUp(1, e.CurrentObject as CicloDesenv);
            }
            else
            {
                throw new UserFriendlyException("Você NÃO pode cancelar um ciclo nos seguintes casos:\n\n - Quando o ciclo já estiver cancelado;\n - Quando o ciclo já estiver concluído;\n - Quando o ciclo é futuro.");
                //System.Windows.Forms.MessageBox.Show("Você NÃO pode cancelar um ciclo nos seguintes casos:\n\n - Quando o ciclo já estiver cancelado;\n- Quando o ciclo já estiver concluído;\n - Quando o ciclo é futuro.");
            }
        }
        /// <summary>
        /// Evento para chamar o popup de Decisão de Ciclo
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CancelEventArgs</param>
        public void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            CicloDesenv ciclo = (CicloDesenv)View.CurrentObject;

            if (ciclo.CsSituacaoCiclo == CsSituacaoCicloDomain.Cancelado)
            {
                CallPopUp(1, ciclo);
                e.Cancel = FormCancelamentoCiclo == null ? false : FormCancelamentoCiclo.IsCancel;
            }
            else
            {
                CallPopUp(0, ciclo);
                e.Cancel = FormDecisaoCiclo == null ? false : FormDecisaoCiclo.IsCancel;
            }
        }
        /// <summary>
        /// Evento para chamar o popup de Decisão de Ciclo
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CancelEventArgs</param>
        public void SaveAndCloseAction_Executing(object sender, CancelEventArgs e)
        {

            CicloDesenv ciclo = (CicloDesenv)View.CurrentObject;

            if (ciclo.CsSituacaoCiclo == CsSituacaoCicloDomain.Cancelado)
            {
                CallPopUp(1, ciclo);
                e.Cancel = FormCancelamentoCiclo == null ? false : FormCancelamentoCiclo.IsCancel;
            }
            else
            {
                CallPopUp(0, ciclo);
                e.Cancel = FormDecisaoCiclo == null ? false : FormDecisaoCiclo.IsCancel;
            }

        }
        /// <summary>
        /// Evento para chamar o popup de Decisão de Ciclo
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CancelEventArgs</param>
        public void SaveAndNewAction_Executing(object sender, CancelEventArgs e)
        {
            CicloDesenv ciclo = (CicloDesenv)View.CurrentObject;

            if (ciclo.CsSituacaoCiclo == CsSituacaoCicloDomain.Cancelado)
            {
                CallPopUp(1, ciclo);
                e.Cancel = FormCancelamentoCiclo == null ? false : FormCancelamentoCiclo.IsCancel;
            }
            else
            {
                CallPopUp(0, ciclo);
                e.Cancel = FormDecisaoCiclo == null ? false : FormDecisaoCiclo.IsCancel;
            }
        }
        /// <summary>
        /// Evento para adicionar itens a lista de Ciclos toda a vez
        /// que a seleção de itens do grid mudar
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        public void CicloDesenvolvimentoViewController_SelectionChanged(object sender, EventArgs e)
        {
            if (((ListView)View).SelectedObjects.Count == 1)
            {
                CicloDesenv ciclo = ((ListView)View).SelectedObjects[0] as CicloDesenv;

                if (ciclo != null)
                {
                    Ciclo = ciclo;
                    CanCancel = ciclo.RnCancelamentoSituacaoNaoIniciado();
                }
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Método chamdado assim que a view termina de ser constrída
        /// se a view for do tipo list view adiciona o Evento CicloDesenvolvimentoViewController_SelectionChanged
        /// caso seja detail view
        /// adiciona os eventos SaveAndCloseAction_Execute e SaveAction_Execute para os botões de
        /// Salvar e Fechar e Salvar respectivamente
        /// </summary>
        protected override void OnViewControlsCreated()
        {
            Session = ((ObjectSpace)View.ObjectSpace).Session;

            if (Ciclo != null)
            {
                
                if (Ciclo.CsSituacaoCiclo.Equals(CsSituacaoCicloDomain.Cancelado) || Ciclo.CsSituacaoCiclo.Equals(CsSituacaoCicloDomain.Concluido))
                {
                    CanCancel = false;
                }
                else
                {
                    CanCancel = true;
                }
            }

            if (View is ListView)
            {
                ((ListView)View).SelectionChanged += CicloDesenvolvimentoViewController_SelectionChanged;
            }
            else if (View is DetailView)
            {

                if (((DetailView)View).CurrentObject != null)
                {
                    Ciclo = ((DetailView)View).CurrentObject as CicloDesenv;
                }

                DevExpress.ExpressApp.SystemModule.DetailViewController control = Frame.GetController<DevExpress.ExpressApp.SystemModule.DetailViewController>();
                control.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                control.SaveAction.Executing += SaveAction_Executing;
                control.SaveAndNewAction.Executing += SaveAndNewAction_Executing;
            }

            RetiraOpcaoCanceladoEmAtrasoDomainSituacaoCiclo();

            base.OnViewControlsCreated();
        }
        /// <summary>
        /// Método chamado assim que a controller é desativado
        /// Desapilha todos os eventos para a view Ciclo Desevolvimento
        /// </summary>
        protected override void OnDeactivated()
        {
            if (Ciclo != null)
            {
            Ciclo.CsSituacaoCiclo = CsSituacaoCicloDomain.Cancelado;
            CanCancel = false;
            if (View is ListView)
            {
                ((ListView)View).SelectionChanged -= CicloDesenvolvimentoViewController_SelectionChanged;
            }
            else if (View is DetailView)
            {
                DevExpress.ExpressApp.SystemModule.DetailViewController control = Frame.GetController<DevExpress.ExpressApp.SystemModule.DetailViewController>();
                control.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                control.SaveAction.Executing -= SaveAction_Executing;
            }
            }

            base.OnDeactivated();
        }

        public void RetiraOpcaoCanceladoEmAtrasoDomainSituacaoCiclo()
        {
            if (View is DetailView)
            {
                DetailView detail = View as DetailView;
                
                EnumPropertyEditor editor = detail.FindItem("CsSituacaoCiclo") as EnumPropertyEditor;

                if (editor != null && editor.CurrentObject != null)
                {
                    var control = editor.Control as DevExpress.XtraEditors.ComboBoxEdit;
                    
                    if (Ciclo.CsSituacaoCiclo != CsSituacaoCicloDomain.Cancelado)
                    {
                        control.Properties.Items.RemoveAt(5);
                    }

                    if (Ciclo.CsSituacaoCiclo != CsSituacaoCicloDomain.EmAtraso)
                    {
                        control.Properties.Items.RemoveAt(4);
                    }
                }
            }
        }

        #endregion

        #region Utils
        /// <summary>
        /// Chama os popups de Decisão de Destino ou Cancelamento de Ciclo
        /// 0 -> Decisão de Destino
        /// 1 -> Cancelamento de Ciclo
        /// </summary>
        /// <param name="popup">index do popup</param>
        /// <param name="ciclos">lista dos ciclos para os popups</param>
        private void CallPopUp(int popup, CicloDesenv ciclo)
        {
            switch (popup)
            {
                case 0: if (ciclo.IsExibirJanelaDestinoItensPendentes()) { FormDecisaoCiclo = new CicloDecisaoEstoriasPendentesForm(ciclo); FormDecisaoCiclo.ShowDialog(); } break;

                case 1:

                    FormCancelamentoCiclo = new CancelamentoCicloForm(ciclo);
                    FormCancelamentoCiclo.ShowDialog();

                    break;
            }
        }
        #endregion
        
    }
}