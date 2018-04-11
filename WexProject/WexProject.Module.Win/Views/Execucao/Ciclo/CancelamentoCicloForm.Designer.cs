namespace WexProject.Module.Win.TelasForaPadrao.Projeto.Ciclo
{
    partial class CancelamentoCicloForm
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
            this.groupControlDadosGerais = new DevExpress.XtraEditors.GroupControl();
            this.DtInicioProxCiclo = new DevExpress.XtraEditors.DateEdit();
            this.lookUpEditMotivo = new DevExpress.XtraEditors.LookUpEdit();
            this.labelInicioProximo = new System.Windows.Forms.Label();
            this.labelMotivo = new System.Windows.Forms.Label();
            this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.GroupControl = new DevExpress.XtraEditors.GroupControl();
            this.ListaItensPendentesCicloUserControl = new WexProject.Module.Win.TelasForaPadrao.Execucao.UserControl.ListaItensPendentesCicloUserControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlDadosGerais)).BeginInit();
            this.groupControlDadosGerais.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DtInicioProxCiclo.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtInicioProxCiclo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditMotivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl)).BeginInit();
            this.GroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControlDadosGerais
            // 
            this.groupControlDadosGerais.Controls.Add(this.DtInicioProxCiclo);
            this.groupControlDadosGerais.Controls.Add(this.lookUpEditMotivo);
            this.groupControlDadosGerais.Controls.Add(this.labelInicioProximo);
            this.groupControlDadosGerais.Controls.Add(this.labelMotivo);
            this.groupControlDadosGerais.Location = new System.Drawing.Point(5, 7);
            this.groupControlDadosGerais.Name = "groupControlDadosGerais";
            this.groupControlDadosGerais.Size = new System.Drawing.Size(823, 92);
            this.groupControlDadosGerais.TabIndex = 0;
            this.groupControlDadosGerais.Text = "Dados Gerais";
            // 
            // DtInicioProxCiclo
            // 
            this.DtInicioProxCiclo.EditValue = null;
            this.DtInicioProxCiclo.Enabled = false;
            this.DtInicioProxCiclo.Location = new System.Drawing.Point(123, 57);
            this.DtInicioProxCiclo.Name = "DtInicioProxCiclo";
            this.DtInicioProxCiclo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtInicioProxCiclo.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DtInicioProxCiclo.Size = new System.Drawing.Size(149, 20);
            this.DtInicioProxCiclo.TabIndex = 3;
            // 
            // lookUpEditMotivo
            // 
            this.lookUpEditMotivo.Location = new System.Drawing.Point(123, 31);
            this.lookUpEditMotivo.Name = "lookUpEditMotivo";
            this.lookUpEditMotivo.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.lookUpEditMotivo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditMotivo.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("TxDescricao", "Descrição", 20, DevExpress.Utils.FormatType.None, global::WexProject.Module.Win.XRDesignRibbonControllerResources.Tarefa, true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lookUpEditMotivo.Properties.NullText = global::WexProject.Module.Win.XRDesignRibbonControllerResources.Tarefa;
            this.lookUpEditMotivo.Size = new System.Drawing.Size(532, 20);
            this.lookUpEditMotivo.TabIndex = 2;
            // 
            // labelInicioProximo
            // 
            this.labelInicioProximo.AutoSize = true;
            this.labelInicioProximo.Location = new System.Drawing.Point(8, 60);
            this.labelInicioProximo.Name = "labelInicioProximo";
            this.labelInicioProximo.Size = new System.Drawing.Size(111, 13);
            this.labelInicioProximo.TabIndex = 1;
            this.labelInicioProximo.Text = "Início do próximo ciclo";
            // 
            // labelMotivo
            // 
            this.labelMotivo.AutoSize = true;
            this.labelMotivo.Location = new System.Drawing.Point(82, 34);
            this.labelMotivo.Name = "labelMotivo";
            this.labelMotivo.Size = new System.Drawing.Size(39, 13);
            this.labelMotivo.TabIndex = 0;
            this.labelMotivo.Text = "Motivo";
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(665, 508);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.Text = "&Salvar";
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancelar
            // 
            this.ButtonCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancelar.Location = new System.Drawing.Point(746, 508);
            this.ButtonCancelar.Name = "ButtonCancelar";
            this.ButtonCancelar.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancelar.TabIndex = 1;
            this.ButtonCancelar.Text = "&Cancelar";
            // 
            // GroupControl
            // 
            this.GroupControl.Controls.Add(this.ListaItensPendentesCicloUserControl);
            this.GroupControl.Location = new System.Drawing.Point(5, 105);
            this.GroupControl.Name = "GroupControl";
            this.GroupControl.Size = new System.Drawing.Size(823, 397);
            this.GroupControl.TabIndex = 4;
            this.GroupControl.Text = "Itens do Ciclo";
            // 
            // ListaItensPendentesCicloUserControl
            // 
            this.ListaItensPendentesCicloUserControl.Ciclo = null;
            this.ListaItensPendentesCicloUserControl.Location = new System.Drawing.Point(8, 24);
            this.ListaItensPendentesCicloUserControl.MaximumSize = new System.Drawing.Size(805, 367);
            this.ListaItensPendentesCicloUserControl.MinimumSize = new System.Drawing.Size(805, 367);
            this.ListaItensPendentesCicloUserControl.Name = "ListaItensPendentesCicloUserControl";
            this.ListaItensPendentesCicloUserControl.Size = new System.Drawing.Size(805, 367);
            this.ListaItensPendentesCicloUserControl.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControlDadosGerais);
            this.panelControl1.Controls.Add(this.ButtonCancelar);
            this.panelControl1.Controls.Add(this.GroupControl);
            this.panelControl1.Controls.Add(this.ButtonOK);
            this.panelControl1.Location = new System.Drawing.Point(-4, -1);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(833, 545);
            this.panelControl1.TabIndex = 1;
            // 
            // CancelamentoCicloForm
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancelar;
            this.ClientSize = new System.Drawing.Size(826, 542);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(832, 570);
            this.MinimumSize = new System.Drawing.Size(832, 570);
            this.Name = "CancelamentoCicloForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cancelar Ciclo";
            ((System.ComponentModel.ISupportInitialize)(this.groupControlDadosGerais)).EndInit();
            this.groupControlDadosGerais.ResumeLayout(false);
            this.groupControlDadosGerais.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DtInicioProxCiclo.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtInicioProxCiclo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditMotivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupControl)).EndInit();
            this.GroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControlDadosGerais;
        private System.Windows.Forms.Label labelInicioProximo;
        private System.Windows.Forms.Label labelMotivo;
        private DevExpress.XtraEditors.DateEdit DtInicioProxCiclo;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditMotivo;
        private DevExpress.XtraEditors.SimpleButton ButtonOK;
        private DevExpress.XtraEditors.SimpleButton ButtonCancelar;
        private DevExpress.XtraEditors.GroupControl GroupControl;
        private Execucao.UserControl.ListaItensPendentesCicloUserControl ListaItensPendentesCicloUserControl;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}