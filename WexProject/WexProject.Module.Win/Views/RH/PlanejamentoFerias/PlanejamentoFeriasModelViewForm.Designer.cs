using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraLayout;
using DevExpress.ExpressApp.Win.Layout;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.ExpressApp.Win.Templates;
namespace WexProject.Module.Win.TelasForaPadrao.RH.PlanejamentoFerias.UserControls
{
    partial class PlanejamentoFeriasModelViewForm
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



        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlanejamentoFeriasModelViewForm));
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditColaborador = new DevExpress.XtraEditors.LookUpEdit();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.vendaFeriascomboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textBoxDtTermino = new System.Windows.Forms.TextBox();
            this.lookUpEditModalidade = new DevExpress.XtraEditors.LookUpEdit();
            this.textEditAtualizadoPor = new DevExpress.XtraEditors.TextEdit();
            this.textEditPlanejadoPor = new DevExpress.XtraEditors.TextEdit();
            this.labelControlAtualizadoPor = new DevExpress.XtraEditors.LabelControl();
            this.labelPlanejadoPor = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControlPLFeriasTermino = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditPLFeriasInicio = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditPeriodo = new DevExpress.XtraEditors.LookUpEdit();
            this.spinEditPeriodoAquisitivoFeriasPlanejadas = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditColaborador.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vendaFeriascomboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditModalidade.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAtualizadoPor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPlanejadoPor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditPLFeriasInicio.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditPLFeriasInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPeriodo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPeriodoAquisitivoFeriasPlanejadas.Properties)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl10
            // 
            resources.ApplyResources(this.labelControl10, "labelControl10");
            this.labelControl10.Name = "labelControl10";
            // 
            // lookUpEditColaborador
            // 
            resources.ApplyResources(this.lookUpEditColaborador, "lookUpEditColaborador");
            this.lookUpEditColaborador.Name = "lookUpEditColaborador";
            this.lookUpEditColaborador.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("lookUpEditColaborador.Properties.Buttons"))))});
            this.lookUpEditColaborador.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("lookUpEditColaborador.Properties.Columns"), resources.GetString("lookUpEditColaborador.Properties.Columns1"))});
            this.lookUpEditColaborador.Properties.NullText = global::WexProject.Module.Win.XRDesignRibbonControllerResources.Tarefa;
            this.lookUpEditColaborador.EditValueChanged += new System.EventHandler(this.lookUpEditColaborador_EditValueChanged);
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonCancelar.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(this.buttonCancelar, "buttonCancelar");
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupControl2
            // 
            resources.ApplyResources(this.groupControl2, "groupControl2");
            this.groupControl2.Controls.Add(this.vendaFeriascomboBoxEdit);
            this.groupControl2.Controls.Add(this.textBoxDtTermino);
            this.groupControl2.Controls.Add(this.lookUpEditModalidade);
            this.groupControl2.Controls.Add(this.textEditAtualizadoPor);
            this.groupControl2.Controls.Add(this.textEditPlanejadoPor);
            this.groupControl2.Controls.Add(this.labelControlAtualizadoPor);
            this.groupControl2.Controls.Add(this.labelPlanejadoPor);
            this.groupControl2.Controls.Add(this.comboBoxEdit1);
            this.groupControl2.Controls.Add(this.labelControl6);
            this.groupControl2.Controls.Add(this.labelControl7);
            this.groupControl2.Controls.Add(this.labelControlPLFeriasTermino);
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Controls.Add(this.dateEditPLFeriasInicio);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Name = "groupControl2";
            // 
            // vendaFeriascomboBoxEdit
            // 
            resources.ApplyResources(this.vendaFeriascomboBoxEdit, "vendaFeriascomboBoxEdit");
            this.vendaFeriascomboBoxEdit.Name = "vendaFeriascomboBoxEdit";
            this.vendaFeriascomboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("vendaFeriascomboBoxEdit.Properties.Buttons"))))});
            this.vendaFeriascomboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // textBoxDtTermino
            // 
            resources.ApplyResources(this.textBoxDtTermino, "textBoxDtTermino");
            this.textBoxDtTermino.Name = "textBoxDtTermino";
            // 
            // lookUpEditModalidade
            // 
            resources.ApplyResources(this.lookUpEditModalidade, "lookUpEditModalidade");
            this.lookUpEditModalidade.Name = "lookUpEditModalidade";
            this.lookUpEditModalidade.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("lookUpEditModalidade.Properties.Buttons"))))});
            this.lookUpEditModalidade.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("lookUpEditModalidade.Properties.Columns"), resources.GetString("lookUpEditModalidade.Properties.Columns1")),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("lookUpEditModalidade.Properties.Columns2"), resources.GetString("lookUpEditModalidade.Properties.Columns3")),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("lookUpEditModalidade.Properties.Columns4"), resources.GetString("lookUpEditModalidade.Properties.Columns5"))});
            this.lookUpEditModalidade.Properties.NullText = global::WexProject.Module.Win.XRDesignRibbonControllerResources.Tarefa;
            this.lookUpEditModalidade.EditValueChanged += new System.EventHandler(this.lookUpEdit1_EditValueChanged);
            // 
            // textEditAtualizadoPor
            // 
            resources.ApplyResources(this.textEditAtualizadoPor, "textEditAtualizadoPor");
            this.textEditAtualizadoPor.Name = "textEditAtualizadoPor";
            // 
            // textEditPlanejadoPor
            // 
            resources.ApplyResources(this.textEditPlanejadoPor, "textEditPlanejadoPor");
            this.textEditPlanejadoPor.Name = "textEditPlanejadoPor";
            // 
            // labelControlAtualizadoPor
            // 
            resources.ApplyResources(this.labelControlAtualizadoPor, "labelControlAtualizadoPor");
            this.labelControlAtualizadoPor.Name = "labelControlAtualizadoPor";
            // 
            // labelPlanejadoPor
            // 
            resources.ApplyResources(this.labelPlanejadoPor, "labelPlanejadoPor");
            this.labelPlanejadoPor.Name = "labelPlanejadoPor";
            // 
            // comboBoxEdit1
            // 
            resources.ApplyResources(this.comboBoxEdit1, "comboBoxEdit1");
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("comboBoxEdit1.Properties.Buttons"))))});
            this.comboBoxEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // labelControl6
            // 
            resources.ApplyResources(this.labelControl6, "labelControl6");
            this.labelControl6.Name = "labelControl6";
            // 
            // labelControl7
            // 
            resources.ApplyResources(this.labelControl7, "labelControl7");
            this.labelControl7.Name = "labelControl7";
            // 
            // labelControlPLFeriasTermino
            // 
            resources.ApplyResources(this.labelControlPLFeriasTermino, "labelControlPLFeriasTermino");
            this.labelControlPLFeriasTermino.Name = "labelControlPLFeriasTermino";
            // 
            // labelControl5
            // 
            resources.ApplyResources(this.labelControl5, "labelControl5");
            this.labelControl5.Name = "labelControl5";
            // 
            // dateEditPLFeriasInicio
            // 
            resources.ApplyResources(this.dateEditPLFeriasInicio, "dateEditPLFeriasInicio");
            this.dateEditPLFeriasInicio.Name = "dateEditPLFeriasInicio";
            this.dateEditPLFeriasInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("dateEditPLFeriasInicio.Properties.Buttons"))))});
            this.dateEditPLFeriasInicio.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditPLFeriasInicio.EditValueChanged += new System.EventHandler(this.dateEditPLFeriasInicio_EditValueChanged);
            // 
            // labelControl4
            // 
            resources.ApplyResources(this.labelControl4, "labelControl4");
            this.labelControl4.Name = "labelControl4";
            // 
            // groupControl1
            // 
            resources.ApplyResources(this.groupControl1, "groupControl1");
            this.groupControl1.Controls.Add(this.labelControl10);
            this.groupControl1.Controls.Add(this.labelControl11);
            this.groupControl1.Controls.Add(this.lookUpEditPeriodo);
            this.groupControl1.Controls.Add(this.spinEditPeriodoAquisitivoFeriasPlanejadas);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.lookUpEditColaborador);
            this.groupControl1.Name = "groupControl1";
            // 
            // labelControl11
            // 
            resources.ApplyResources(this.labelControl11, "labelControl11");
            this.labelControl11.Name = "labelControl11";
            // 
            // lookUpEditPeriodo
            // 
            resources.ApplyResources(this.lookUpEditPeriodo, "lookUpEditPeriodo");
            this.lookUpEditPeriodo.Name = "lookUpEditPeriodo";
            this.lookUpEditPeriodo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("lookUpEditPeriodo.Properties.Buttons"))))});
            this.lookUpEditPeriodo.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("lookUpEditPeriodo.Properties.Columns"), resources.GetString("lookUpEditPeriodo.Properties.Columns1"))});
            this.lookUpEditPeriodo.Properties.NullText = global::WexProject.Module.Win.XRDesignRibbonControllerResources.Tarefa;
            this.lookUpEditPeriodo.Properties.SortColumnIndex = 1;
            // 
            // spinEditPeriodoAquisitivoFeriasPlanejadas
            // 
            resources.ApplyResources(this.spinEditPeriodoAquisitivoFeriasPlanejadas, "spinEditPeriodoAquisitivoFeriasPlanejadas");
            this.spinEditPeriodoAquisitivoFeriasPlanejadas.Name = "spinEditPeriodoAquisitivoFeriasPlanejadas";
            this.spinEditPeriodoAquisitivoFeriasPlanejadas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // labelControl3
            // 
            resources.ApplyResources(this.labelControl3, "labelControl3");
            this.labelControl3.Name = "labelControl3";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupControl1);
            this.flowLayoutPanel1.Controls.Add(this.groupControl2);
            this.flowLayoutPanel1.Controls.Add(this.panelControl1);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.buttonCancelar);
            this.panelControl1.Controls.Add(this.buttonOk);
            resources.ApplyResources(this.panelControl1, "panelControl1");
            this.panelControl1.Name = "panelControl1";
            // 
            // PlanejamentoFeriasModelViewForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "PlanejamentoFeriasModelViewForm";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditColaborador.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vendaFeriascomboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditModalidade.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAtualizadoPor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPlanejadoPor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditPLFeriasInicio.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditPLFeriasInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditPeriodo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditPeriodoAquisitivoFeriasPlanejadas.Properties)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SpinEdit spinEditPeriodoAquisitivoFeriasPlanejadas;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.DateEdit dateEditPLFeriasInicio;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControlPLFeriasTermino;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit textEditAtualizadoPor;
        private DevExpress.XtraEditors.TextEdit textEditPlanejadoPor;
        private DevExpress.XtraEditors.LabelControl labelControlAtualizadoPor;
        private DevExpress.XtraEditors.LabelControl labelPlanejadoPor;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private Button buttonCancelar;
        private Button buttonOk;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditModalidade;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditColaborador;
        private FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditPeriodo;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private TextBox textBoxDtTermino;
        private DevExpress.XtraEditors.ComboBoxEdit vendaFeriascomboBoxEdit;
    }
}
