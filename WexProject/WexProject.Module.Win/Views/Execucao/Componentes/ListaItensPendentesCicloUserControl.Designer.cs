namespace WexProject.Module.Win.TelasForaPadrao.Execucao.UserControl
{
    partial class ListaItensPendentesCicloUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaItensPendentesCicloUserControl));
            this.ListaPrioridadesGroup = new DevExpress.XtraEditors.GroupControl();
            this.LstPrioridade = new DevExpress.XtraEditors.ListBoxControl();
            this.ListaProximoCicloGroup = new DevExpress.XtraEditors.GroupControl();
            this.LstProximoCiclo = new DevExpress.XtraEditors.ListBoxControl();
            this.SubirBtn = new System.Windows.Forms.Button();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.DescerBtn = new System.Windows.Forms.Button();
            this.MoverDireitaBtn = new System.Windows.Forms.Button();
            this.MoverEsquerdaBtn = new System.Windows.Forms.Button();
            this.MoverTodosDireitaBtn = new System.Windows.Forms.Button();
            this.MoverTodosEsquerdaBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ListaPrioridadesGroup)).BeginInit();
            this.ListaPrioridadesGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LstPrioridade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListaProximoCicloGroup)).BeginInit();
            this.ListaProximoCicloGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LstProximoCiclo)).BeginInit();
            this.SuspendLayout();
            // 
            // ListaPrioridadesGroup
            // 
            this.ListaPrioridadesGroup.Controls.Add(this.LstPrioridade);
            this.ListaPrioridadesGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.ListaPrioridadesGroup.Location = new System.Drawing.Point(0, 0);
            this.ListaPrioridadesGroup.Name = "ListaPrioridadesGroup";
            this.ListaPrioridadesGroup.Size = new System.Drawing.Size(370, 367);
            this.ListaPrioridadesGroup.TabIndex = 0;
            this.ListaPrioridadesGroup.Text = "Mover para a Lista de Prioridades";
            // 
            // LstPrioridade
            // 
            this.LstPrioridade.AllowDrop = true;
            this.LstPrioridade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LstPrioridade.Location = new System.Drawing.Point(2, 21);
            this.LstPrioridade.Name = "LstPrioridade";
            this.LstPrioridade.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.LstPrioridade.Size = new System.Drawing.Size(366, 344);
            this.LstPrioridade.TabIndex = 1;
            this.LstPrioridade.SelectedIndexChanged += new System.EventHandler(this.LstPrioridade_SelectedIndexChanged);
            this.LstPrioridade.Click += new System.EventHandler(this.LstPrioridade_Click);
            this.LstPrioridade.DragDrop += new System.Windows.Forms.DragEventHandler(this.LstPrioridade_DragDrop);
            this.LstPrioridade.DragEnter += new System.Windows.Forms.DragEventHandler(this.LstPrioridade_DragEnter);
            this.LstPrioridade.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LstPrioridade_MouseDown);
            this.LstPrioridade.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LstPrioridade_MouseMove);
            // 
            // ListaProximoCicloGroup
            // 
            this.ListaProximoCicloGroup.Controls.Add(this.LstProximoCiclo);
            this.ListaProximoCicloGroup.Dock = System.Windows.Forms.DockStyle.Right;
            this.ListaProximoCicloGroup.Location = new System.Drawing.Point(435, 0);
            this.ListaProximoCicloGroup.Name = "ListaProximoCicloGroup";
            this.ListaProximoCicloGroup.Size = new System.Drawing.Size(370, 367);
            this.ListaProximoCicloGroup.TabIndex = 1;
            this.ListaProximoCicloGroup.Text = "Mover para o Próximo Ciclo";
            // 
            // LstProximoCiclo
            // 
            this.LstProximoCiclo.AllowDrop = true;
            this.LstProximoCiclo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LstProximoCiclo.Location = new System.Drawing.Point(2, 21);
            this.LstProximoCiclo.Name = "LstProximoCiclo";
            this.LstProximoCiclo.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.LstProximoCiclo.Size = new System.Drawing.Size(366, 344);
            this.LstProximoCiclo.TabIndex = 8;
            this.LstProximoCiclo.SelectedIndexChanged += new System.EventHandler(this.LstProximoCiclo_SelectedIndexChanged);
            this.LstProximoCiclo.Click += new System.EventHandler(this.LstProximoCiclo_Click);
            this.LstProximoCiclo.DragDrop += new System.Windows.Forms.DragEventHandler(this.LstProximoCiclo_DragDrop);
            this.LstProximoCiclo.DragEnter += new System.Windows.Forms.DragEventHandler(this.LstProximoCiclo_DragEnter);
            this.LstProximoCiclo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LstProximoCiclo_MouseDown);
            this.LstProximoCiclo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LstProximoCiclo_MouseMove);
            // 
            // SubirBtn
            // 
            this.SubirBtn.FlatAppearance.BorderSize = 0;
            this.SubirBtn.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.SubirBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.SubirBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.SubirBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SubirBtn.ImageIndex = 5;
            this.SubirBtn.ImageList = this.ImageList;
            this.SubirBtn.Location = new System.Drawing.Point(383, 66);
            this.SubirBtn.Name = "SubirBtn";
            this.SubirBtn.Size = new System.Drawing.Size(38, 38);
            this.SubirBtn.TabIndex = 2;
            this.SubirBtn.UseVisualStyleBackColor = true;
            this.SubirBtn.Click += new System.EventHandler(this.SubirBtn_Click);
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "allleft.png");
            this.ImageList.Images.SetKeyName(1, "allright.png");
            this.ImageList.Images.SetKeyName(2, "down.png");
            this.ImageList.Images.SetKeyName(3, "left.png");
            this.ImageList.Images.SetKeyName(4, "right.png");
            this.ImageList.Images.SetKeyName(5, "up.png");
            // 
            // DescerBtn
            // 
            this.DescerBtn.FlatAppearance.BorderSize = 0;
            this.DescerBtn.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.DescerBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.DescerBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.DescerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DescerBtn.ImageIndex = 2;
            this.DescerBtn.ImageList = this.ImageList;
            this.DescerBtn.Location = new System.Drawing.Point(383, 110);
            this.DescerBtn.Name = "DescerBtn";
            this.DescerBtn.Size = new System.Drawing.Size(38, 38);
            this.DescerBtn.TabIndex = 3;
            this.DescerBtn.UseVisualStyleBackColor = true;
            this.DescerBtn.Click += new System.EventHandler(this.DescerBtn_Click);
            // 
            // MoverDireitaBtn
            // 
            this.MoverDireitaBtn.FlatAppearance.BorderSize = 0;
            this.MoverDireitaBtn.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverDireitaBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverDireitaBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverDireitaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MoverDireitaBtn.ImageIndex = 4;
            this.MoverDireitaBtn.ImageList = this.ImageList;
            this.MoverDireitaBtn.Location = new System.Drawing.Point(383, 154);
            this.MoverDireitaBtn.Name = "MoverDireitaBtn";
            this.MoverDireitaBtn.Size = new System.Drawing.Size(38, 38);
            this.MoverDireitaBtn.TabIndex = 4;
            this.MoverDireitaBtn.UseVisualStyleBackColor = true;
            this.MoverDireitaBtn.Click += new System.EventHandler(this.MoverDireitaBtn_Click);
            // 
            // MoverEsquerdaBtn
            // 
            this.MoverEsquerdaBtn.FlatAppearance.BorderSize = 0;
            this.MoverEsquerdaBtn.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverEsquerdaBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverEsquerdaBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverEsquerdaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MoverEsquerdaBtn.ImageIndex = 3;
            this.MoverEsquerdaBtn.ImageList = this.ImageList;
            this.MoverEsquerdaBtn.Location = new System.Drawing.Point(383, 198);
            this.MoverEsquerdaBtn.Name = "MoverEsquerdaBtn";
            this.MoverEsquerdaBtn.Size = new System.Drawing.Size(38, 38);
            this.MoverEsquerdaBtn.TabIndex = 5;
            this.MoverEsquerdaBtn.UseVisualStyleBackColor = true;
            this.MoverEsquerdaBtn.Click += new System.EventHandler(this.MoverEsquerdaBtn_Click);
            // 
            // MoverTodosDireitaBtn
            // 
            this.MoverTodosDireitaBtn.FlatAppearance.BorderSize = 0;
            this.MoverTodosDireitaBtn.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverTodosDireitaBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverTodosDireitaBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverTodosDireitaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MoverTodosDireitaBtn.ImageIndex = 1;
            this.MoverTodosDireitaBtn.ImageList = this.ImageList;
            this.MoverTodosDireitaBtn.Location = new System.Drawing.Point(383, 242);
            this.MoverTodosDireitaBtn.Name = "MoverTodosDireitaBtn";
            this.MoverTodosDireitaBtn.Size = new System.Drawing.Size(38, 38);
            this.MoverTodosDireitaBtn.TabIndex = 6;
            this.MoverTodosDireitaBtn.UseVisualStyleBackColor = true;
            this.MoverTodosDireitaBtn.Click += new System.EventHandler(this.MoverTodosDireitaBtn_Click);
            // 
            // MoverTodosEsquerdaBtn
            // 
            this.MoverTodosEsquerdaBtn.FlatAppearance.BorderSize = 0;
            this.MoverTodosEsquerdaBtn.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverTodosEsquerdaBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverTodosEsquerdaBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.MoverTodosEsquerdaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MoverTodosEsquerdaBtn.ImageIndex = 0;
            this.MoverTodosEsquerdaBtn.ImageList = this.ImageList;
            this.MoverTodosEsquerdaBtn.Location = new System.Drawing.Point(383, 286);
            this.MoverTodosEsquerdaBtn.Name = "MoverTodosEsquerdaBtn";
            this.MoverTodosEsquerdaBtn.Size = new System.Drawing.Size(38, 38);
            this.MoverTodosEsquerdaBtn.TabIndex = 7;
            this.MoverTodosEsquerdaBtn.UseVisualStyleBackColor = true;
            this.MoverTodosEsquerdaBtn.Click += new System.EventHandler(this.MoverTodosEsquerdaBtn_Click);
            // 
            // ListaItensPendentesCicloUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MoverTodosEsquerdaBtn);
            this.Controls.Add(this.MoverTodosDireitaBtn);
            this.Controls.Add(this.MoverEsquerdaBtn);
            this.Controls.Add(this.MoverDireitaBtn);
            this.Controls.Add(this.DescerBtn);
            this.Controls.Add(this.SubirBtn);
            this.Controls.Add(this.ListaProximoCicloGroup);
            this.Controls.Add(this.ListaPrioridadesGroup);
            this.MaximumSize = new System.Drawing.Size(805, 367);
            this.MinimumSize = new System.Drawing.Size(805, 367);
            this.Name = "ListaItensPendentesCicloUserControl";
            this.Size = new System.Drawing.Size(805, 367);
            ((System.ComponentModel.ISupportInitialize)(this.ListaPrioridadesGroup)).EndInit();
            this.ListaPrioridadesGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LstPrioridade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListaProximoCicloGroup)).EndInit();
            this.ListaProximoCicloGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LstProximoCiclo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl ListaPrioridadesGroup;
        private DevExpress.XtraEditors.GroupControl ListaProximoCicloGroup;
        public DevExpress.XtraEditors.ListBoxControl LstPrioridade;
        public DevExpress.XtraEditors.ListBoxControl LstProximoCiclo;
        private System.Windows.Forms.Button SubirBtn;
        private System.Windows.Forms.Button DescerBtn;
        private System.Windows.Forms.Button MoverDireitaBtn;
        private System.Windows.Forms.Button MoverEsquerdaBtn;
        private System.Windows.Forms.Button MoverTodosDireitaBtn;
        private System.Windows.Forms.Button MoverTodosEsquerdaBtn;
        private System.Windows.Forms.ImageList ImageList;
    }
}