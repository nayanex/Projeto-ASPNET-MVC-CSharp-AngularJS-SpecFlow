namespace WexProject.Module.Win.TelasForaPadrao.Projeto.Ciclo
{
    partial class CicloDesenvolvimentoViewController
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
            this.BtCancelCiclo = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BtCancelCiclo
            // 
            this.BtCancelCiclo.Caption = "Cancelar Ciclo";
            this.BtCancelCiclo.Category = "Edit";
            this.BtCancelCiclo.ConfirmationMessage = null;
            this.BtCancelCiclo.Id = "BtCancelCiclo";
            this.BtCancelCiclo.ImageName = "Action_Deny";
            this.BtCancelCiclo.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.BtCancelCiclo.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.BtCancelCiclo.Shortcut = "Cancelar ciclo";
            this.BtCancelCiclo.Tag = null;
            this.BtCancelCiclo.TargetObjectsCriteria = null;
            this.BtCancelCiclo.TargetObjectType = typeof(WexProject.BLL.Models.Execucao.CicloDesenv);
            this.BtCancelCiclo.TargetViewId = null;
            this.BtCancelCiclo.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.BtCancelCiclo.ToolTip = null;
            this.BtCancelCiclo.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.BtCancelCiclo.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BtCancelCiclo_Execute);
            // 
            // CicloDesenvolvimentoViewController
            // 
            this.TargetObjectType = typeof(WexProject.BLL.Models.Execucao.CicloDesenv);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BtCancelCiclo;


    }
}
