namespace WexMultiAcessManagerProject
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if(disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.btDesconectar = new System.Windows.Forms.Button();
			this.btConectar = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.lbEndIp = new System.Windows.Forms.Label();
			this.txEnderecoIp = new System.Windows.Forms.TextBox();
			this.txPort = new System.Windows.Forms.TextBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.edicaoTarefaDataGridView = new System.Windows.Forms.DataGridView();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.txLog = new System.Windows.Forms.TextBox();
			this.oidCronogramaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.loginDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.usuarioConectadoBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.edicaoTarefaBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.edicaoTarefaDataGridView)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.usuarioConectadoBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.edicaoTarefaBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// btDesconectar
			// 
			this.btDesconectar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btDesconectar.Enabled = false;
			this.btDesconectar.Location = new System.Drawing.Point(583, 596);
			this.btDesconectar.Name = "btDesconectar";
			this.btDesconectar.Size = new System.Drawing.Size(85, 44);
			this.btDesconectar.TabIndex = 0;
			this.btDesconectar.Text = "Desconectar";
			this.btDesconectar.UseVisualStyleBackColor = true;
			this.btDesconectar.Click += new System.EventHandler(this.btDesconectar_Click);
			// 
			// btConectar
			// 
			this.btConectar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btConectar.Location = new System.Drawing.Point(9, 596);
			this.btConectar.Name = "btConectar";
			this.btConectar.Size = new System.Drawing.Size(85, 44);
			this.btConectar.TabIndex = 2;
			this.btConectar.Text = "Conectar";
			this.btConectar.UseVisualStyleBackColor = true;
			this.btConectar.Click += new System.EventHandler(this.btConectar_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(580, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Porta";
			// 
			// lbEndIp
			// 
			this.lbEndIp.AutoSize = true;
			this.lbEndIp.Location = new System.Drawing.Point(11, 7);
			this.lbEndIp.Name = "lbEndIp";
			this.lbEndIp.Size = new System.Drawing.Size(62, 13);
			this.lbEndIp.TabIndex = 4;
			this.lbEndIp.Text = "EnderecoIp";
			// 
			// txEnderecoIp
			// 
			this.txEnderecoIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txEnderecoIp.Location = new System.Drawing.Point(11, 23);
			this.txEnderecoIp.Name = "txEnderecoIp";
			this.txEnderecoIp.Size = new System.Drawing.Size(549, 20);
			this.txEnderecoIp.TabIndex = 5;
			// 
			// txPort
			// 
			this.txPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txPort.Location = new System.Drawing.Point(583, 23);
			this.txPort.Name = "txPort";
			this.txPort.Size = new System.Drawing.Size(85, 20);
			this.txPort.TabIndex = 6;
			this.txPort.Text = "8000";
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.oidCronogramaDataGridViewTextBoxColumn,
            this.loginDataGridViewTextBoxColumn});
			this.dataGridView1.DataSource = this.usuarioConectadoBindingSource;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(3, 3);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.Size = new System.Drawing.Size(645, 327);
			this.dataGridView1.TabIndex = 7;
			// 
			// edicaoTarefaDataGridView
			// 
			this.edicaoTarefaDataGridView.AllowUserToAddRows = false;
			this.edicaoTarefaDataGridView.AllowUserToDeleteRows = false;
			this.edicaoTarefaDataGridView.AutoGenerateColumns = false;
			this.edicaoTarefaDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.edicaoTarefaDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
			this.edicaoTarefaDataGridView.DataSource = this.edicaoTarefaBindingSource;
			this.edicaoTarefaDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.edicaoTarefaDataGridView.Location = new System.Drawing.Point(3, 3);
			this.edicaoTarefaDataGridView.Name = "edicaoTarefaDataGridView";
			this.edicaoTarefaDataGridView.ReadOnly = true;
			this.edicaoTarefaDataGridView.Size = new System.Drawing.Size(645, 254);
			this.edicaoTarefaDataGridView.TabIndex = 9;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(9, 49);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(659, 286);
			this.tabControl1.TabIndex = 10;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dataGridView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(651, 333);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Usuários Conectados";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.edicaoTarefaDataGridView);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(651, 260);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Tarefas em edição:";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// txLog
			// 
			this.txLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.txLog.Location = new System.Drawing.Point(12, 376);
			this.txLog.Multiline = true;
			this.txLog.Name = "txLog";
			this.txLog.ReadOnly = true;
			this.txLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txLog.Size = new System.Drawing.Size(659, 214);
			this.txLog.TabIndex = 0;
			// 
			// oidCronogramaDataGridViewTextBoxColumn
			// 
			this.oidCronogramaDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.oidCronogramaDataGridViewTextBoxColumn.DataPropertyName = "OidCronograma";
			this.oidCronogramaDataGridViewTextBoxColumn.HeaderText = "Cronograma";
			this.oidCronogramaDataGridViewTextBoxColumn.Name = "oidCronogramaDataGridViewTextBoxColumn";
			this.oidCronogramaDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// loginDataGridViewTextBoxColumn
			// 
			this.loginDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.loginDataGridViewTextBoxColumn.DataPropertyName = "Login";
			this.loginDataGridViewTextBoxColumn.HeaderText = "Colaborador";
			this.loginDataGridViewTextBoxColumn.Name = "loginDataGridViewTextBoxColumn";
			this.loginDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// usuarioConectadoBindingSource
			// 
			this.usuarioConectadoBindingSource.DataSource = typeof(WexMultiAcessManagerProject.Libs.UsuarioConectado);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn1.DataPropertyName = "OidCronograma";
			this.dataGridViewTextBoxColumn1.HeaderText = "Cronograma";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Login";
			this.dataGridViewTextBoxColumn2.HeaderText = "Login";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn3.DataPropertyName = "OidTarefa";
			this.dataGridViewTextBoxColumn3.HeaderText = "Tarefa";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			// 
			// edicaoTarefaBindingSource
			// 
			this.edicaoTarefaBindingSource.DataSource = typeof(WexMultiAcessManagerProject.Libs.EdicaoTarefa);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 360);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Log de informações:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 652);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txLog);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.txPort);
			this.Controls.Add(this.txEnderecoIp);
			this.Controls.Add(this.lbEndIp);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btConectar);
			this.Controls.Add(this.btDesconectar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Servidor de testes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.edicaoTarefaDataGridView)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.usuarioConectadoBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.edicaoTarefaBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btDesconectar;
        private System.Windows.Forms.Button btConectar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbEndIp;
        private System.Windows.Forms.TextBox txEnderecoIp;
        private System.Windows.Forms.TextBox txPort;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.BindingSource usuarioConectadoBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn oidCronogramaDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn loginDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource edicaoTarefaBindingSource;
		private System.Windows.Forms.DataGridView edicaoTarefaDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox txLog;
		private System.Windows.Forms.Label label1;
    }
}

