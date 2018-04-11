namespace WexProject.Module.Lib.ControllerPraWinWeb.NovosNegocios
{
    partial class FilterUsuarioViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.singleChoiceActionUsuarios = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.singleChoiceActionSituacao = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // singleChoiceActionUsuarios
            // 
            this.singleChoiceActionUsuarios.Caption = "Responsável";
            this.singleChoiceActionUsuarios.Category = "Filters";
            this.singleChoiceActionUsuarios.ConfirmationMessage = null;
            this.singleChoiceActionUsuarios.Id = "SEOTResponsavelFilter";
            this.singleChoiceActionUsuarios.ImageName = null;
            this.singleChoiceActionUsuarios.Shortcut = null;
            this.singleChoiceActionUsuarios.Tag = null;
            this.singleChoiceActionUsuarios.TargetObjectsCriteria = null;
            this.singleChoiceActionUsuarios.TargetViewId = null;
            this.singleChoiceActionUsuarios.ToolTip = null;
            this.singleChoiceActionUsuarios.TypeOfView = null;
            this.singleChoiceActionUsuarios.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SingleChoiceActionUsuarios_Execute);
            // 
            // singleChoiceActionSituacao
            // 
            this.singleChoiceActionSituacao.Caption = "Situação";
            this.singleChoiceActionSituacao.Category = "Filters";
            this.singleChoiceActionSituacao.ConfirmationMessage = null;
            this.singleChoiceActionSituacao.Id = "SEOTSituacaoFilter";
            this.singleChoiceActionSituacao.ImageName = null;
            this.singleChoiceActionSituacao.Shortcut = null;
            this.singleChoiceActionSituacao.Tag = null;
            this.singleChoiceActionSituacao.TargetObjectsCriteria = null;
            this.singleChoiceActionSituacao.TargetViewId = null;
            this.singleChoiceActionSituacao.ToolTip = null;
            this.singleChoiceActionSituacao.TypeOfView = null;
            this.singleChoiceActionSituacao.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SingleChoiceActionSituacao_Execute);
            // 
            // FilterUsuarioViewController
            // 
            this.TargetObjectType = typeof(WexProject.BLL.Models.NovosNegocios.SolicitacaoOrcamento);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceActionUsuarios;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceActionSituacao;
    }
}
