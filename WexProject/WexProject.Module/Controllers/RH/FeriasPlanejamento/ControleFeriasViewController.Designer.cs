namespace WexProject.Module.Lib.ControllerPraWinWeb
{
    partial class ControleFeriasViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem8 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem9 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem10 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem11 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem12 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem13 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem14 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.SingleChoiceFilterPeriodo = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.SingleChoiceFilterSituacao = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // SingleChoiceFilterPeriodo
            // 
            this.SingleChoiceFilterPeriodo.Caption = "Período";
            this.SingleChoiceFilterPeriodo.Category = "Filters";
            this.SingleChoiceFilterPeriodo.ConfirmationMessage = null;
            this.SingleChoiceFilterPeriodo.Id = "FilterPeriodo";
            this.SingleChoiceFilterPeriodo.ImageName = null;
            choiceActionItem1.Caption = "15 últimos dias";
            choiceActionItem1.Data = "u15";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem2.Caption = "30 últimos dias";
            choiceActionItem2.Data = "u30";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem3.Caption = "45 últimos dias";
            choiceActionItem3.Data = "u45";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem4.Caption = "Mês atual";
            choiceActionItem4.Data = "ma";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem5.Caption = "Mês anterior";
            choiceActionItem5.Data = "am";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem6.Caption = "Próximos 15 dias";
            choiceActionItem6.Data = "p15";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem7.Caption = "Próximos 30 dias";
            choiceActionItem7.Data = "p30";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem8.Caption = "Próximo mês";
            choiceActionItem8.Data = "pm";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem9.BeginGroup = true;
            choiceActionItem9.Caption = "Todos";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem1);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem2);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem3);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem4);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem5);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem6);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem7);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem8);
            this.SingleChoiceFilterPeriodo.Items.Add(choiceActionItem9);
            this.SingleChoiceFilterPeriodo.Shortcut = null;
            this.SingleChoiceFilterPeriodo.Tag = null;
            this.SingleChoiceFilterPeriodo.TargetObjectsCriteria = null;
            this.SingleChoiceFilterPeriodo.TargetViewId = null;
            this.SingleChoiceFilterPeriodo.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SingleChoiceFilterPeriodo.ToolTip = null;
            this.SingleChoiceFilterPeriodo.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SingleChoiceFilterPeriodo.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SingleChoiceFilterPeriodo_Execute);
            // 
            // SingleChoiceFilterSituacao
            // 
            this.SingleChoiceFilterSituacao.Caption = "Situação";
            this.SingleChoiceFilterSituacao.Category = "Filters";
            this.SingleChoiceFilterSituacao.ConfirmationMessage = null;
            this.SingleChoiceFilterSituacao.Id = "FilterSituacao";
            this.SingleChoiceFilterSituacao.ImageName = null;
            choiceActionItem10.Caption = "Em Atraso";
            choiceActionItem10.Data = "EmAtraso";
            choiceActionItem10.ImageName = null;
            choiceActionItem10.Shortcut = null;
            choiceActionItem11.Caption = "Pendentes";
            choiceActionItem11.Data = "Pendente";
            choiceActionItem11.ImageName = null;
            choiceActionItem11.Shortcut = null;
            choiceActionItem12.Caption = "Planejado";
            choiceActionItem12.Data = "Planejado";
            choiceActionItem12.ImageName = null;
            choiceActionItem12.Shortcut = null;
            choiceActionItem13.BeginGroup = true;
            choiceActionItem13.Caption = "Realizado";
            choiceActionItem13.Data = "Realizado";
            choiceActionItem13.ImageName = null;
            choiceActionItem13.Shortcut = null;
            choiceActionItem14.BeginGroup = true;
            choiceActionItem14.Caption = "Todos";
            choiceActionItem14.ImageName = null;
            choiceActionItem14.Shortcut = null;
            this.SingleChoiceFilterSituacao.Items.Add(choiceActionItem10);
            this.SingleChoiceFilterSituacao.Items.Add(choiceActionItem11);
            this.SingleChoiceFilterSituacao.Items.Add(choiceActionItem12);
            this.SingleChoiceFilterSituacao.Items.Add(choiceActionItem13);
            this.SingleChoiceFilterSituacao.Items.Add(choiceActionItem14);
            this.SingleChoiceFilterSituacao.Shortcut = null;
            this.SingleChoiceFilterSituacao.Tag = null;
            this.SingleChoiceFilterSituacao.TargetObjectsCriteria = null;
            this.SingleChoiceFilterSituacao.TargetViewId = null;
            this.SingleChoiceFilterSituacao.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SingleChoiceFilterSituacao.ToolTip = null;
            this.SingleChoiceFilterSituacao.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SingleChoiceFilterSituacao.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SingleChoiceFilterSituacao_Execute);
            // 
            // ControleFeriasViewController
            // 
            this.TargetObjectType = typeof(WexProject.BLL.Models.Rh.FeriasPlanejamento);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.ListView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SingleChoiceFilterPeriodo;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SingleChoiceFilterSituacao;
    }
}