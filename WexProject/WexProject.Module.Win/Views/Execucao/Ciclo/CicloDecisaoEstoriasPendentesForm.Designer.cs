namespace WexProject.Module.Win.TelasForaPadrao.Execucao
{
    partial class CicloDecisaoEstoriasPendentesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CicloDecisaoEstoriasPendentesForm));
            this.ItensGroup = new DevExpress.XtraEditors.GroupControl();
            this.CancelarBtn = new System.Windows.Forms.Button();
            this.SalvarBtn = new System.Windows.Forms.Button();
            this.ListaItensPendentesCicloUserControl = new WexProject.Module.Win.TelasForaPadrao.Execucao.UserControl.ListaItensPendentesCicloUserControl();
            ((System.ComponentModel.ISupportInitialize)(this.ItensGroup)).BeginInit();
            this.ItensGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItensGroup
            // 
            this.ItensGroup.Controls.Add(this.CancelarBtn);
            this.ItensGroup.Controls.Add(this.SalvarBtn);
            this.ItensGroup.Controls.Add(this.ListaItensPendentesCicloUserControl);
            this.ItensGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItensGroup.Location = new System.Drawing.Point(0, 0);
            this.ItensGroup.Name = "ItensGroup";
            this.ItensGroup.Size = new System.Drawing.Size(815, 430);
            this.ItensGroup.TabIndex = 0;
            this.ItensGroup.Text = "Itens do Ciclo";
            // 
            // CancelarBtn
            // 
            this.CancelarBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelarBtn.Location = new System.Drawing.Point(723, 398);
            this.CancelarBtn.Name = "CancelarBtn";
            this.CancelarBtn.Size = new System.Drawing.Size(87, 25);
            this.CancelarBtn.TabIndex = 4;
            this.CancelarBtn.Text = "&Cancelar";
            this.CancelarBtn.UseVisualStyleBackColor = true;
            // 
            // SalvarBtn
            // 
            this.SalvarBtn.Location = new System.Drawing.Point(630, 398);
            this.SalvarBtn.Name = "SalvarBtn";
            this.SalvarBtn.Size = new System.Drawing.Size(87, 25);
            this.SalvarBtn.TabIndex = 3;
            this.SalvarBtn.Text = "&Salvar";
            this.SalvarBtn.UseVisualStyleBackColor = true;
            this.SalvarBtn.Click += new System.EventHandler(this.SalvarBtn_Click);
            // 
            // ListaItensPendentesCicloUserControl
            // 
            this.ListaItensPendentesCicloUserControl.Ciclo = null;
            this.ListaItensPendentesCicloUserControl.Location = new System.Drawing.Point(5, 25);
            this.ListaItensPendentesCicloUserControl.MaximumSize = new System.Drawing.Size(805, 367);
            this.ListaItensPendentesCicloUserControl.MinimumSize = new System.Drawing.Size(805, 367);
            this.ListaItensPendentesCicloUserControl.Name = "ListaItensPendentesCicloUserControl";
            this.ListaItensPendentesCicloUserControl.Size = new System.Drawing.Size(805, 367);
            this.ListaItensPendentesCicloUserControl.TabIndex = 0;
            // 
            // CicloDecisaoEstoriasPendentesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelarBtn;
            this.ClientSize = new System.Drawing.Size(815, 430);
            this.Controls.Add(this.ItensGroup);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(831, 468);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(831, 468);
            this.Name = "CicloDecisaoEstoriasPendentesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Informe o destino dos Itens Pendentes";
            ((System.ComponentModel.ISupportInitialize)(this.ItensGroup)).EndInit();
            this.ItensGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl ItensGroup;
        private UserControl.ListaItensPendentesCicloUserControl ListaItensPendentesCicloUserControl;
        private System.Windows.Forms.Button CancelarBtn;
        private System.Windows.Forms.Button SalvarBtn;

    }
}