namespace WexProject.Module.Win.Projeto
{
    partial class SingleChoiceProjeto
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
            this.Projetos = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // Projetos
            // 
            this.Projetos.Caption = "Projetos";
            this.Projetos.Category = "Filters";
            this.Projetos.ConfirmationMessage = null;
            this.Projetos.Id = "ProjetosFilter";
            this.Projetos.ImageName = null;
            this.Projetos.Shortcut = null;
            this.Projetos.ShowItemsOnClick = true;
            this.Projetos.Tag = null;
            this.Projetos.TargetObjectsCriteria = null;
            this.Projetos.TargetViewId = null;
            this.Projetos.ToolTip = null;
            this.Projetos.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.Projetos.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SingleChoiceAction1_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction Projetos;



    }
}
