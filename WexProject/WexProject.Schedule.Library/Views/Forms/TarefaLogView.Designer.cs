namespace WexProject.Schedule.Library.Views.Forms
{
    partial class TarefaLogView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AlteracaoColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.PanelControlGrid = new DevExpress.XtraEditors.PanelControl();
            this.LogGridControl = new DevExpress.XtraGrid.GridControl();
            this.LogGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DataColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HoraColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColaboradorColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PanelControlButton = new DevExpress.XtraEditors.PanelControl();
            this.CloseButton = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelControlGrid)).BeginInit();
            this.PanelControlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelControlButton)).BeginInit();
            this.PanelControlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // AlteracaoColumn
            // 
            this.AlteracaoColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.AlteracaoColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AlteracaoColumn.Caption = "Alteração";
            this.AlteracaoColumn.ColumnEdit = this.repositoryItemMemoEdit1;
            this.AlteracaoColumn.FieldName = "descricaoAlteracao";
            this.AlteracaoColumn.Name = "AlteracaoColumn";
            this.AlteracaoColumn.OptionsColumn.AllowEdit = false;
            this.AlteracaoColumn.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.AlteracaoColumn.OptionsColumn.AllowMove = false;
            this.AlteracaoColumn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.AlteracaoColumn.Visible = true;
            this.AlteracaoColumn.VisibleIndex = 3;
            this.AlteracaoColumn.Width = 795;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // PanelControlGrid
            // 
            this.PanelControlGrid.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelControlGrid.Controls.Add(this.LogGridControl);
            this.PanelControlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelControlGrid.Location = new System.Drawing.Point(0, 0);
            this.PanelControlGrid.Name = "PanelControlGrid";
            this.PanelControlGrid.Size = new System.Drawing.Size(684, 359);
            this.PanelControlGrid.TabIndex = 3;
            // 
            // LogGridControl
            // 
            this.LogGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogGridControl.Location = new System.Drawing.Point(0, 0);
            this.LogGridControl.MainView = this.LogGridView;
            this.LogGridControl.Name = "LogGridControl";
            this.LogGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.LogGridControl.ShowOnlyPredefinedDetails = true;
            this.LogGridControl.Size = new System.Drawing.Size(684, 359);
            this.LogGridControl.TabIndex = 1;
            this.LogGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.LogGridView});
            // 
            // LogGridView
            // 
            this.LogGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DataColumn,
            this.HoraColumn,
            this.ColaboradorColumn,
            this.AlteracaoColumn});
            this.LogGridView.GridControl = this.LogGridControl;
            this.LogGridView.GroupPanelText = "Arraste uma coluna para agrupar.";
            this.LogGridView.Name = "LogGridView";
            this.LogGridView.OptionsView.RowAutoHeight = true;
            this.LogGridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.DataColumn, DevExpress.Data.ColumnSortOrder.Descending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.HoraColumn, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // DataColumn
            // 
            this.DataColumn.AppearanceCell.Options.UseTextOptions = true;
            this.DataColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DataColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.DataColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DataColumn.Caption = "Data";
            this.DataColumn.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.DataColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DataColumn.FieldName = "DtDataEHora";
            this.DataColumn.Name = "DataColumn";
            this.DataColumn.OptionsColumn.AllowEdit = false;
            this.DataColumn.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.DataColumn.Visible = true;
            this.DataColumn.VisibleIndex = 0;
            this.DataColumn.Width = 181;
            // 
            // HoraColumn
            // 
            this.HoraColumn.AppearanceCell.Options.UseTextOptions = true;
            this.HoraColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.HoraColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.HoraColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.HoraColumn.Caption = "Hora";
            this.HoraColumn.FieldName = "Hora";
            this.HoraColumn.Name = "HoraColumn";
            this.HoraColumn.OptionsColumn.AllowEdit = false;
            this.HoraColumn.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.HoraColumn.Visible = true;
            this.HoraColumn.VisibleIndex = 1;
            this.HoraColumn.Width = 126;
            // 
            // ColaboradorColumn
            // 
            this.ColaboradorColumn.AppearanceCell.Options.UseTextOptions = true;
            this.ColaboradorColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColaboradorColumn.AppearanceHeader.Options.UseTextOptions = true;
            this.ColaboradorColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ColaboradorColumn.Caption = "Colaborador";
            this.ColaboradorColumn.FieldName = "descricaoColaborador";
            this.ColaboradorColumn.Name = "ColaboradorColumn";
            this.ColaboradorColumn.OptionsColumn.AllowEdit = false;
            this.ColaboradorColumn.Visible = true;
            this.ColaboradorColumn.VisibleIndex = 2;
            this.ColaboradorColumn.Width = 392;
            // 
            // PanelControlButton
            // 
            this.PanelControlButton.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelControlButton.Controls.Add(this.CloseButton);
            this.PanelControlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelControlButton.Location = new System.Drawing.Point(0, 359);
            this.PanelControlButton.Name = "PanelControlButton";
            this.PanelControlButton.Size = new System.Drawing.Size(684, 53);
            this.PanelControlButton.TabIndex = 4;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(597, 18);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Fechar";
            // 
            // TarefaLogView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(684, 412);
            this.Controls.Add(this.PanelControlGrid);
            this.Controls.Add(this.PanelControlButton);
            this.Name = "TarefaLogView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Histórico de Atualizações";
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelControlGrid)).EndInit();
            this.PanelControlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelControlButton)).EndInit();
            this.PanelControlButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn AlteracaoColumn;
        private DevExpress.XtraEditors.PanelControl PanelControlGrid;
        private DevExpress.XtraGrid.GridControl LogGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView LogGridView;
        private DevExpress.XtraGrid.Columns.GridColumn DataColumn;
        private DevExpress.XtraGrid.Columns.GridColumn HoraColumn;
        private DevExpress.XtraGrid.Columns.GridColumn ColaboradorColumn;
        private DevExpress.XtraEditors.PanelControl PanelControlButton;
        private DevExpress.XtraEditors.SimpleButton CloseButton;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
    }
}