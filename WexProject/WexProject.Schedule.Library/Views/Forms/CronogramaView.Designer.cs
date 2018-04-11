using WexProject.Schedule.Library.Views.Components;
using WexProject.MultiAccess.Library.Delegates;
using DevExpress.XtraEditors;
using WexProject.Schedule.Library.Presenters;
using System.Windows.Forms;
using System;

namespace WexProject.Schedule.Library.Views.Forms
{
    partial class CronogramaView
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
			this.components = new System.ComponentModel.Container();
			DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CronogramaView));
			DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
			DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
			DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint("22/04/2014 00:00:00", new object[] {
            ((object)(60D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint2 = new DevExpress.XtraCharts.SeriesPoint("23/04/2014 00:00:00", new object[] {
            ((object)(50D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint3 = new DevExpress.XtraCharts.SeriesPoint("24/04/2014 00:00:00", new object[] {
            ((object)(40D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint4 = new DevExpress.XtraCharts.SeriesPoint("25/04/2014 00:00:00", new object[] {
            ((object)(30D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint5 = new DevExpress.XtraCharts.SeriesPoint("28/04/2014 00:00:00", new object[] {
            ((object)(20D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint6 = new DevExpress.XtraCharts.SeriesPoint("29/04/2014 00:00:00", new object[] {
            ((object)(10D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint7 = new DevExpress.XtraCharts.SeriesPoint("30/04/2014 00:00:00", new object[] {
            ((object)(0D))});
			DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
			DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.PointOptions pointOptions2 = new DevExpress.XtraCharts.PointOptions();
			DevExpress.XtraCharts.SeriesPoint seriesPoint8 = new DevExpress.XtraCharts.SeriesPoint("22/04/2014 00:00:00", new object[] {
            ((object)(60D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint9 = new DevExpress.XtraCharts.SeriesPoint("23/04/2014 00:00:00", new object[] {
            ((object)(45D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint10 = new DevExpress.XtraCharts.SeriesPoint("24/04/2014 00:00:00", new object[] {
            ((object)(40D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint11 = new DevExpress.XtraCharts.SeriesPoint("25/04/2014 00:00:00", new object[] {
            ((object)(35D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint12 = new DevExpress.XtraCharts.SeriesPoint("28/04/2014 00:00:00", new object[] {
            ((object)(15D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint13 = new DevExpress.XtraCharts.SeriesPoint("29/04/2014 00:00:00", new object[] {
            ((object)(10D))});
			DevExpress.XtraCharts.SeriesPoint seriesPoint14 = new DevExpress.XtraCharts.SeriesPoint("30/04/2014 00:00:00", new object[] {
            ((object)(0D))});
			DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel3 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
			DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
			this.gridColunaDescricaoTarefa = new DevExpress.XtraGrid.Columns.GridColumn();
			this.txDescricaoTarefaTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
			this.tarefaHistoricoView1 = new WexProject.Schedule.Library.Views.Components.TarefaHistoricoView();
			this.cronogramaTarefaGridControl = new DevExpress.XtraGrid.GridControl();
			this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColunaId = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColunaIcone = new DevExpress.XtraGrid.Columns.GridColumn();
			this.iconeEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
			this.gridColunaObs = new DevExpress.XtraGrid.Columns.GridColumn();
			this.TextAreaObservacaoTarefa = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
			this.gridColunaRealizado = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColunaInicio = new DevExpress.XtraGrid.Columns.GridColumn();
			this.dtInicioEdit = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
			this.gridColunaEstimativaInicial = new DevExpress.XtraGrid.Columns.GridColumn();
			this.comboEstimativaInicial = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.gridColunaSituacao = new DevExpress.XtraGrid.Columns.GridColumn();
			this.comboGridSituacaoPlanejamento = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
			this.gridColunaEstimativaRestante = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repositorioPopUpContainer = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
			this.gridColunaAtualizadoEm = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColunaAtualizadoPor = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColunaResponsavel = new DevExpress.XtraGrid.Columns.GridColumn();
			this.comboColaboradoresResponsaveis = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
			this.Menu = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.imageCollection3 = new DevExpress.Utils.ImageCollection(this.components);
			this.barraBtNovoCronograma = new DevExpress.XtraBars.BarButtonItem();
			this.barraBtExcluirCronograma = new DevExpress.XtraBars.BarButtonItem();
			this.barraBtAtualizarCronograma = new DevExpress.XtraBars.BarButtonItem();
			this.barraBotao_Fechar = new DevExpress.XtraBars.BarButtonItem();
			this.barraBtNovaTarefa = new DevExpress.XtraBars.BarButtonItem();
			this.barraBtExcluirTarefa = new DevExpress.XtraBars.BarButtonItem();
			this.CronogramaEdit = new DevExpress.XtraBars.BarEditItem();
			this.barEditItem_Nome = new DevExpress.XtraBars.BarEditItem();
			this.bar_CronogramaSituacao = new DevExpress.XtraBars.BarEditItem();
			this.barraTextoCronograma = new DevExpress.XtraBars.BarEditItem();
			this.barButtonGoDesc = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonGoEst = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonGoObs = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonGoSearch = new DevExpress.XtraBars.BarButtonItem();
			this.ribbonGaleriaImagensSituacaoPlanejamento = new DevExpress.XtraBars.RibbonGalleryBarItem();
			this.HistoricoBarButton = new DevExpress.XtraBars.BarButtonItem();
			this.ExibirHistoricoCheck = new DevExpress.XtraBars.BarCheckItem();
			this.barraTxDescricaoCronograma = new DevExpress.XtraBars.BarEditItem();
			this.barraRepositorioTxDescricaoCronograma = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.barraTxDescricaoSituacaoPlanejmaneto = new DevExpress.XtraBars.BarEditItem();
			this.barraRepositorioTxDescricaoSituacaoPlanejamento = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.barraComboCronogramaSelecionado = new DevExpress.XtraBars.BarEditItem();
			this.barraRepositorioComboCronogramaSelecionado = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
			this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
			this.lbColaboradorConectado = new DevExpress.XtraBars.BarStaticItem();
			this.lbEstadorServidor = new DevExpress.XtraBars.BarStaticItem();
			this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
			this.barEditItem3 = new DevExpress.XtraBars.BarEditItem();
			this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.lbUltimaAlteracao = new DevExpress.XtraBars.BarStaticItem();
			this.txEditIdPopup = new DevExpress.XtraBars.BarEditItem();
			this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.lbEstadoAcoes = new DevExpress.XtraBars.BarStaticItem();
			this.filtroSituacaoTodas = new DevExpress.XtraBars.BarCheckItem();
			this.filtroSituacaoPendentes = new DevExpress.XtraBars.BarCheckItem();
			this.filtroSituacaoEncerradas = new DevExpress.XtraBars.BarCheckItem();
			this.filtroSituacaoCustom = new DevExpress.XtraBars.BarCheckItem();
			this.barraDtInicioCronograma = new DevExpress.XtraBars.BarEditItem();
			this.dtInicioCronograma = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
			this.barraDtTerminoCronograma = new DevExpress.XtraBars.BarEditItem();
			this.dtTerminoCronograma = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
			this.imageCollection4 = new DevExpress.Utils.ImageCollection(this.components);
			this.ribbonMiniToolbar1 = new DevExpress.XtraBars.Ribbon.RibbonMiniToolbar(this.components);
			this.ribbonMiniToolbar2 = new DevExpress.XtraBars.Ribbon.RibbonMiniToolbar(this.components);
			this.Planejamento = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.ribbonHome = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonEdicao = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonDadosGerais = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPeriodo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonCronogramaSelecionado = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonLinhaBase = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonFechar = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.Tarefas = new DevExpress.XtraBars.Ribbon.RibbonPage();
			this.newTask = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.barraTarefasGridControl = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.Historico = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.Janela = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
			this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.repositoryItemBorderLineStyle1 = new DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle();
			this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.repositoryMoverPopup = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
			this.repositoryItemPopupContainerEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
			this.repositoryItemPopupContainerEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
			this.barraStatus = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.barraUsuariosConectados = new WexProject.Schedule.Library.Views.Components.BarraUsuariosConectados(this.components);
			this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
			this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
			this.imageCollection2 = new DevExpress.Utils.ImageCollection(this.components);
			this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
			this.MultiAccessClient = new WexProject.MultiAccess.Library.WexMultiAccessClient(this.components);
			this.MensagemPopup = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
			this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
			this.dockPanelBurndown = new DevExpress.XtraBars.Docking.DockPanel();
			this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
			this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
			this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
			this.imageCollectionSituacao = new DevExpress.Utils.ImageCollection(this.components);
			this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
			this.imageCollectionGrid = new DevExpress.Utils.ImageCollection(this.components);
			this.ribbonControl1 = new DevExpress.DXCore.Controls.XtraBars.Ribbon.RibbonControl();
			this.cronogramaTarefaBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
			((System.ComponentModel.ISupportInitialize)(this.txDescricaoTarefaTextEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
			this.popupContainerControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cronogramaTarefaGridControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iconeEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TextAreaObservacaoTarefa)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioEdit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioEdit.VistaTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.comboEstimativaInicial)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.comboGridSituacaoPlanejamento)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositorioPopUpContainer)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.comboColaboradoresResponsaveis)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Menu)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barraRepositorioTxDescricaoCronograma)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barraRepositorioTxDescricaoSituacaoPlanejamento)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barraRepositorioComboCronogramaSelecionado)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioCronograma)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioCronograma.VistaTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtTerminoCronograma)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtTerminoCronograma.VistaTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineStyle1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryMoverPopup)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barraUsuariosConectados)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
			this.dockPanelBurndown.SuspendLayout();
			this.dockPanel1_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollectionSituacao)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollectionGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cronogramaTarefaBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridColunaDescricaoTarefa
			// 
			this.gridColunaDescricaoTarefa.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
			this.gridColunaDescricaoTarefa.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaDescricaoTarefa.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaDescricaoTarefa.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.gridColunaDescricaoTarefa.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaDescricaoTarefa.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaDescricaoTarefa.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaDescricaoTarefa.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.gridColunaDescricaoTarefa.Caption = "Tarefa";
			this.gridColunaDescricaoTarefa.ColumnEdit = this.txDescricaoTarefaTextEdit;
			this.gridColunaDescricaoTarefa.FieldName = "TxDescricaoTarefa";
			this.gridColunaDescricaoTarefa.MinWidth = 350;
			this.gridColunaDescricaoTarefa.Name = "gridColunaDescricaoTarefa";
			this.gridColunaDescricaoTarefa.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
			this.gridColunaDescricaoTarefa.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
			this.gridColunaDescricaoTarefa.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
			this.gridColunaDescricaoTarefa.Visible = true;
			this.gridColunaDescricaoTarefa.VisibleIndex = 2;
			this.gridColunaDescricaoTarefa.Width = 350;
			// 
			// txDescricaoTarefaTextEdit
			// 
			this.txDescricaoTarefaTextEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.txDescricaoTarefaTextEdit.AutoHeight = false;
			this.txDescricaoTarefaTextEdit.Mask.IgnoreMaskBlank = false;
			this.txDescricaoTarefaTextEdit.MaxLength = 8000;
			this.txDescricaoTarefaTextEdit.Name = "txDescricaoTarefaTextEdit";
			this.txDescricaoTarefaTextEdit.ValidateOnEnterKey = true;
			// 
			// popupContainerControl1
			// 
			this.popupContainerControl1.Controls.Add(this.tarefaHistoricoView1);
			this.popupContainerControl1.Location = new System.Drawing.Point(67, 216);
			this.popupContainerControl1.Name = "popupContainerControl1";
			this.popupContainerControl1.Size = new System.Drawing.Size(413, 283);
			this.popupContainerControl1.TabIndex = 9;
			// 
			// tarefaHistoricoView1
			// 
			this.tarefaHistoricoView1.DtRealizado = new System.DateTime(((long)(0)));
			this.tarefaHistoricoView1.Location = new System.Drawing.Point(0, 0);
			this.tarefaHistoricoView1.Margin = new System.Windows.Forms.Padding(4);
			this.tarefaHistoricoView1.Name = "tarefaHistoricoView1";
			this.tarefaHistoricoView1.NbHoraFinal = "";
			this.tarefaHistoricoView1.NbHoraInicial = null;
			this.tarefaHistoricoView1.NbHoraRealizado = "00:00";
			this.tarefaHistoricoView1.NbHoraRestante = "00:00";
			this.tarefaHistoricoView1.OidSituacaoPlanejamento = new System.Guid("00000000-0000-0000-0000-000000000000");
			this.tarefaHistoricoView1.Size = new System.Drawing.Size(413, 286);
			this.tarefaHistoricoView1.TabIndex = 8;
			this.tarefaHistoricoView1.TxComentario = "";
			this.tarefaHistoricoView1.TxJustificativaDeReducao = "";
			// 
			// cronogramaTarefaGridControl
			// 
			this.cronogramaTarefaGridControl.AllowDrop = true;
			this.cronogramaTarefaGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cronogramaTarefaGridControl.Enabled = false;
			this.cronogramaTarefaGridControl.Location = new System.Drawing.Point(26, 142);
			this.cronogramaTarefaGridControl.LookAndFeel.SkinName = "Office 2010 Blue";
			this.cronogramaTarefaGridControl.MainView = this.GridView;
			this.cronogramaTarefaGridControl.MenuManager = this.Menu;
			this.cronogramaTarefaGridControl.Name = "cronogramaTarefaGridControl";
			this.cronogramaTarefaGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.comboGridSituacaoPlanejamento,
            this.TextAreaObservacaoTarefa,
            this.comboColaboradoresResponsaveis,
            this.repositorioPopUpContainer,
            this.txDescricaoTarefaTextEdit,
            this.iconeEdit,
            this.dtInicioEdit,
            this.comboEstimativaInicial});
			this.cronogramaTarefaGridControl.Size = new System.Drawing.Size(1130, 126);
			this.cronogramaTarefaGridControl.TabIndex = 2;
			this.cronogramaTarefaGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView});
			this.cronogramaTarefaGridControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.CronogramaTarefaGrid_DragDrop);
			this.cronogramaTarefaGridControl.DragOver += new System.Windows.Forms.DragEventHandler(this.CronogramaTarefaGrid_DragOver);
			this.cronogramaTarefaGridControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CronogramaTarefaGrid_MouseDown);
			this.cronogramaTarefaGridControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CronogramaTarefaGrid_MouseMove);
			// 
			// GridView
			// 
			this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColunaId,
            this.gridColunaDescricaoTarefa,
            this.gridColunaIcone,
            this.gridColunaObs,
            this.gridColunaRealizado,
            this.gridColunaInicio,
            this.gridColunaEstimativaInicial,
            this.gridColunaSituacao,
            this.gridColunaEstimativaRestante,
            this.gridColunaAtualizadoEm,
            this.gridColunaAtualizadoPor,
            this.gridColunaResponsavel});
			styleFormatCondition1.Appearance.BackColor2 = System.Drawing.Color.Red;
			styleFormatCondition1.Column = this.gridColunaDescricaoTarefa;
			styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.NotEqual;
			styleFormatCondition1.Value1 = "";
			this.GridView.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
			this.GridView.GridControl = this.cronogramaTarefaGridControl;
			this.GridView.GroupPanelText = "Arraste uma coluna para agrupar.";
			this.GridView.GroupRowHeight = 0;
			this.GridView.Name = "GridView";
			this.GridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
			this.GridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
			this.GridView.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
			this.GridView.OptionsBehavior.AutoExpandAllGroups = true;
			this.GridView.OptionsBehavior.FocusLeaveOnTab = true;
			this.GridView.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
			this.GridView.OptionsMenu.ShowGroupSummaryEditorItem = true;
			this.GridView.OptionsSelection.MultiSelect = true;
			this.GridView.OptionsView.ShowChildrenInGroupPanel = true;
			this.GridView.OptionsView.ShowFooter = true;
			this.GridView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColunaId, DevExpress.Data.ColumnSortOrder.Ascending)});
			this.GridView.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GridView_RowCellClick);
			this.GridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.GridView_CustomDrawCell);
			this.GridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.GridView_RowCellStyle);
			this.GridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.GridView_ShowingEditor);
			this.GridView.HiddenEditor += new System.EventHandler(this.GridView_HiddenEditor);
			this.GridView.ShownEditor += new System.EventHandler(this.GridView_ShownEditor);
			this.GridView.ColumnFilterChanged += new System.EventHandler(this.GridView_ColumnFilterChanged);
			this.GridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridView_KeyDown);
			this.GridView.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridView_ValidatingEditor);
			// 
			// gridColunaId
			// 
			this.gridColunaId.AppearanceCell.BackColor = System.Drawing.Color.LightCyan;
			this.gridColunaId.AppearanceCell.ForeColor = System.Drawing.Color.Black;
			this.gridColunaId.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaId.AppearanceCell.Options.UseForeColor = true;
			this.gridColunaId.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaId.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaId.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
			this.gridColunaId.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaId.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaId.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaId.Caption = "ID";
			this.gridColunaId.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
			this.gridColunaId.FieldName = "NbID";
			this.gridColunaId.MinWidth = 31;
			this.gridColunaId.Name = "gridColunaId";
			this.gridColunaId.OptionsColumn.AllowEdit = false;
			this.gridColunaId.OptionsColumn.AllowFocus = false;
			this.gridColunaId.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
			this.gridColunaId.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
			this.gridColunaId.OptionsColumn.FixedWidth = true;
			this.gridColunaId.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
			this.gridColunaId.Visible = true;
			this.gridColunaId.VisibleIndex = 0;
			this.gridColunaId.Width = 31;
			// 
			// gridColunaIcone
			// 
			this.gridColunaIcone.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
			this.gridColunaIcone.AppearanceCell.ForeColor = System.Drawing.Color.Black;
			this.gridColunaIcone.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaIcone.AppearanceCell.Options.UseForeColor = true;
			this.gridColunaIcone.Caption = " ";
			this.gridColunaIcone.ColumnEdit = this.iconeEdit;
			this.gridColunaIcone.FieldName = "Icone";
			this.gridColunaIcone.MaxWidth = 38;
			this.gridColunaIcone.MinWidth = 38;
			this.gridColunaIcone.Name = "gridColunaIcone";
			this.gridColunaIcone.OptionsColumn.AllowEdit = false;
			this.gridColunaIcone.OptionsColumn.AllowFocus = false;
			this.gridColunaIcone.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
			this.gridColunaIcone.OptionsColumn.AllowMove = false;
			this.gridColunaIcone.OptionsColumn.ReadOnly = true;
			this.gridColunaIcone.OptionsColumn.ShowCaption = false;
			this.gridColunaIcone.OptionsFilter.AllowAutoFilter = false;
			this.gridColunaIcone.OptionsFilter.AllowFilter = false;
			this.gridColunaIcone.OptionsFilter.AllowFilterModeChanging = DevExpress.Utils.DefaultBoolean.False;
			this.gridColunaIcone.OptionsFilter.FilterBySortField = DevExpress.Utils.DefaultBoolean.False;
			this.gridColunaIcone.Visible = true;
			this.gridColunaIcone.VisibleIndex = 1;
			this.gridColunaIcone.Width = 38;
			// 
			// iconeEdit
			// 
			this.iconeEdit.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.False;
			this.iconeEdit.Name = "iconeEdit";
			this.iconeEdit.NullText = " ";
			this.iconeEdit.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.Image;
			this.iconeEdit.ReadOnly = true;
			this.iconeEdit.ShowMenu = false;
			this.iconeEdit.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.False;
			// 
			// gridColunaObs
			// 
			this.gridColunaObs.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
			this.gridColunaObs.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaObs.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaObs.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaObs.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaObs.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaObs.Caption = "Obs";
			this.gridColunaObs.ColumnEdit = this.TextAreaObservacaoTarefa;
			this.gridColunaObs.FieldName = "TxObservacaoTarefa";
			this.gridColunaObs.MaxWidth = 31;
			this.gridColunaObs.MinWidth = 31;
			this.gridColunaObs.Name = "gridColunaObs";
			this.gridColunaObs.Visible = true;
			this.gridColunaObs.VisibleIndex = 3;
			this.gridColunaObs.Width = 31;
			// 
			// TextAreaObservacaoTarefa
			// 
			this.TextAreaObservacaoTarefa.AutoHeight = false;
			this.TextAreaObservacaoTarefa.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.TextAreaObservacaoTarefa.MaxLength = 8000;
			this.TextAreaObservacaoTarefa.Name = "TextAreaObservacaoTarefa";
			this.TextAreaObservacaoTarefa.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.TextAreaObservacaoTarefa_Closed);
			// 
			// gridColunaRealizado
			// 
			this.gridColunaRealizado.AppearanceCell.BackColor = System.Drawing.Color.LightCyan;
			this.gridColunaRealizado.AppearanceCell.ForeColor = System.Drawing.Color.Black;
			this.gridColunaRealizado.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaRealizado.AppearanceCell.Options.UseForeColor = true;
			this.gridColunaRealizado.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaRealizado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaRealizado.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaRealizado.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaRealizado.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaRealizado.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaRealizado.Caption = "Realizado";
			this.gridColunaRealizado.FieldName = "TxRealizado";
			this.gridColunaRealizado.FieldNameSortGroup = "NbRealizado";
			this.gridColunaRealizado.MaxWidth = 100;
			this.gridColunaRealizado.MinWidth = 100;
			this.gridColunaRealizado.Name = "gridColunaRealizado";
			this.gridColunaRealizado.OptionsColumn.AllowEdit = false;
			this.gridColunaRealizado.OptionsColumn.ReadOnly = true;
			this.gridColunaRealizado.Visible = true;
			this.gridColunaRealizado.VisibleIndex = 8;
			this.gridColunaRealizado.Width = 100;
			// 
			// gridColunaInicio
			// 
			this.gridColunaInicio.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaInicio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaInicio.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaInicio.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaInicio.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaInicio.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaInicio.Caption = "Início";
			this.gridColunaInicio.ColumnEdit = this.dtInicioEdit;
			this.gridColunaInicio.DisplayFormat.FormatString = "dd/MM/yyyy";
			this.gridColunaInicio.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.gridColunaInicio.FieldName = "DtInicio";
			this.gridColunaInicio.MaxWidth = 100;
			this.gridColunaInicio.MinWidth = 100;
			this.gridColunaInicio.Name = "gridColunaInicio";
			this.gridColunaInicio.Visible = true;
			this.gridColunaInicio.VisibleIndex = 9;
			this.gridColunaInicio.Width = 100;
			// 
			// dtInicioEdit
			// 
			this.dtInicioEdit.AutoHeight = false;
			this.dtInicioEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtInicioEdit.Name = "dtInicioEdit";
			this.dtInicioEdit.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.dtInicioEdit.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.dtInicioEdit_Closed);
			// 
			// gridColunaEstimativaInicial
			// 
			this.gridColunaEstimativaInicial.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
			this.gridColunaEstimativaInicial.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaEstimativaInicial.AppearanceCell.Options.UseForeColor = true;
			this.gridColunaEstimativaInicial.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaEstimativaInicial.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaEstimativaInicial.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaEstimativaInicial.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaEstimativaInicial.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaEstimativaInicial.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaEstimativaInicial.Caption = "Estimativa Inicial";
			this.gridColunaEstimativaInicial.ColumnEdit = this.comboEstimativaInicial;
			this.gridColunaEstimativaInicial.FieldName = "TxEstimativaInicial";
			this.gridColunaEstimativaInicial.FieldNameSortGroup = "NbEstimativaInicial";
			this.gridColunaEstimativaInicial.MaxWidth = 100;
			this.gridColunaEstimativaInicial.MinWidth = 100;
			this.gridColunaEstimativaInicial.Name = "gridColunaEstimativaInicial";
			this.gridColunaEstimativaInicial.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom, "NbEstimativaInicial", "", "EstimativaInicialSum")});
			this.gridColunaEstimativaInicial.Visible = true;
			this.gridColunaEstimativaInicial.VisibleIndex = 7;
			this.gridColunaEstimativaInicial.Width = 100;
			// 
			// comboEstimativaInicial
			// 
			this.comboEstimativaInicial.AutoHeight = false;
			this.comboEstimativaInicial.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.comboEstimativaInicial.Items.AddRange(new object[] {
            "0h",
            "1h",
            "2h",
            "4h",
            "6h",
            "8h",
            "16h"});
			this.comboEstimativaInicial.Name = "comboEstimativaInicial";
			this.comboEstimativaInicial.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.comboEstimativaInicial.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.comboEstimativaInicial_Closed);
			// 
			// gridColunaSituacao
			// 
			this.gridColunaSituacao.AppearanceCell.BackColor = System.Drawing.Color.Transparent;
			this.gridColunaSituacao.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaSituacao.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaSituacao.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaSituacao.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaSituacao.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaSituacao.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaSituacao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaSituacao.Caption = "Situação";
			this.gridColunaSituacao.ColumnEdit = this.comboGridSituacaoPlanejamento;
			this.gridColunaSituacao.FieldName = "OidSituacaoPlanejamentoTarefa";
			this.gridColunaSituacao.MaxWidth = 120;
			this.gridColunaSituacao.MinWidth = 120;
			this.gridColunaSituacao.Name = "gridColunaSituacao";
			this.gridColunaSituacao.Visible = true;
			this.gridColunaSituacao.VisibleIndex = 4;
			this.gridColunaSituacao.Width = 120;
			// 
			// comboGridSituacaoPlanejamento
			// 
			this.comboGridSituacaoPlanejamento.AutoHeight = false;
			this.comboGridSituacaoPlanejamento.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.comboGridSituacaoPlanejamento.Name = "comboGridSituacaoPlanejamento";
			this.comboGridSituacaoPlanejamento.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.comboGridSituacaoPlanejamento_Closed);
			// 
			// gridColunaEstimativaRestante
			// 
			this.gridColunaEstimativaRestante.AppearanceCell.BackColor = System.Drawing.Color.LightCyan;
			this.gridColunaEstimativaRestante.AppearanceCell.ForeColor = System.Drawing.Color.Black;
			this.gridColunaEstimativaRestante.AppearanceCell.Options.UseBackColor = true;
			this.gridColunaEstimativaRestante.AppearanceCell.Options.UseForeColor = true;
			this.gridColunaEstimativaRestante.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaEstimativaRestante.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaEstimativaRestante.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaEstimativaRestante.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaEstimativaRestante.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaEstimativaRestante.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaEstimativaRestante.Caption = "Estimativa Restante";
			this.gridColunaEstimativaRestante.ColumnEdit = this.repositorioPopUpContainer;
			this.gridColunaEstimativaRestante.FieldName = "TxRestante";
			this.gridColunaEstimativaRestante.FieldNameSortGroup = "NbEstimativaRestante";
			this.gridColunaEstimativaRestante.MaxWidth = 120;
			this.gridColunaEstimativaRestante.MinWidth = 120;
			this.gridColunaEstimativaRestante.Name = "gridColunaEstimativaRestante";
			this.gridColunaEstimativaRestante.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom, "NbEstimativaRestante", "", "EstimativaRestanteSum")});
			this.gridColunaEstimativaRestante.Visible = true;
			this.gridColunaEstimativaRestante.VisibleIndex = 6;
			this.gridColunaEstimativaRestante.Width = 120;
			// 
			// repositorioPopUpContainer
			// 
			this.repositorioPopUpContainer.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositorioPopUpContainer.CloseOnLostFocus = false;
			this.repositorioPopUpContainer.CloseOnOuterMouseClick = false;
			this.repositorioPopUpContainer.Name = "repositorioPopUpContainer";
			this.repositorioPopUpContainer.PopupControl = this.popupContainerControl1;
			this.repositorioPopUpContainer.PopupSizeable = false;
			this.repositorioPopUpContainer.ShowPopupCloseButton = false;
			this.repositorioPopUpContainer.Popup += new System.EventHandler(this.repositorioPopUpContainer_Popup);
			// 
			// gridColunaAtualizadoEm
			// 
			this.gridColunaAtualizadoEm.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaAtualizadoEm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaAtualizadoEm.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaAtualizadoEm.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaAtualizadoEm.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaAtualizadoEm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaAtualizadoEm.Caption = "Atualizado Em";
			this.gridColunaAtualizadoEm.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.gridColunaAtualizadoEm.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.gridColunaAtualizadoEm.FieldName = "DtAtualizadoEm";
			this.gridColunaAtualizadoEm.MaxWidth = 120;
			this.gridColunaAtualizadoEm.MinWidth = 120;
			this.gridColunaAtualizadoEm.Name = "gridColunaAtualizadoEm";
			this.gridColunaAtualizadoEm.OptionsColumn.AllowEdit = false;
			this.gridColunaAtualizadoEm.OptionsColumn.ReadOnly = true;
			this.gridColunaAtualizadoEm.Visible = true;
			this.gridColunaAtualizadoEm.VisibleIndex = 10;
			this.gridColunaAtualizadoEm.Width = 120;
			// 
			// gridColunaAtualizadoPor
			// 
			this.gridColunaAtualizadoPor.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaAtualizadoPor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaAtualizadoPor.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaAtualizadoPor.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaAtualizadoPor.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaAtualizadoPor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaAtualizadoPor.Caption = "Atualizado Por";
			this.gridColunaAtualizadoPor.FieldName = "TxAtualizadoPor";
			this.gridColunaAtualizadoPor.MaxWidth = 120;
			this.gridColunaAtualizadoPor.MinWidth = 120;
			this.gridColunaAtualizadoPor.Name = "gridColunaAtualizadoPor";
			this.gridColunaAtualizadoPor.OptionsColumn.AllowEdit = false;
			this.gridColunaAtualizadoPor.OptionsColumn.ReadOnly = true;
			this.gridColunaAtualizadoPor.Visible = true;
			this.gridColunaAtualizadoPor.VisibleIndex = 11;
			this.gridColunaAtualizadoPor.Width = 120;
			// 
			// gridColunaResponsavel
			// 
			this.gridColunaResponsavel.AppearanceCell.Options.UseTextOptions = true;
			this.gridColunaResponsavel.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaResponsavel.AppearanceHeader.BackColor = System.Drawing.Color.DarkGray;
			this.gridColunaResponsavel.AppearanceHeader.Options.UseBackColor = true;
			this.gridColunaResponsavel.AppearanceHeader.Options.UseTextOptions = true;
			this.gridColunaResponsavel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.gridColunaResponsavel.Caption = "Responsável";
			this.gridColunaResponsavel.ColumnEdit = this.comboColaboradoresResponsaveis;
			this.gridColunaResponsavel.FieldName = "TxDescricaoColaborador";
			this.gridColunaResponsavel.MaxWidth = 130;
			this.gridColunaResponsavel.MinWidth = 130;
			this.gridColunaResponsavel.Name = "gridColunaResponsavel";
			this.gridColunaResponsavel.Visible = true;
			this.gridColunaResponsavel.VisibleIndex = 5;
			this.gridColunaResponsavel.Width = 130;
			// 
			// comboColaboradoresResponsaveis
			// 
			this.comboColaboradoresResponsaveis.AutoHeight = false;
			this.comboColaboradoresResponsaveis.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.comboColaboradoresResponsaveis.IncrementalSearch = true;
			this.comboColaboradoresResponsaveis.Name = "comboColaboradoresResponsaveis";
			this.comboColaboradoresResponsaveis.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.comboColaboradoresResponsaveis_Closed);
			// 
			// Menu
			// 
			this.Menu.ApplicationButtonText = null;
			this.Menu.AutoSizeItems = true;
			this.Menu.ExpandCollapseItem.Id = 0;
			this.Menu.ExpandCollapseItem.Name = "Tarefa01";
			this.Menu.Images = this.imageCollection3;
			this.Menu.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Menu.ExpandCollapseItem,
            this.barraBtNovoCronograma,
            this.barraBtExcluirCronograma,
            this.barraBtAtualizarCronograma,
            this.barraBotao_Fechar,
            this.barraBtNovaTarefa,
            this.barraBtExcluirTarefa,
            this.CronogramaEdit,
            this.barEditItem_Nome,
            this.bar_CronogramaSituacao,
            this.barraTextoCronograma,
            this.barButtonGoDesc,
            this.barButtonGoEst,
            this.barButtonGoObs,
            this.barButtonGoSearch,
            this.ribbonGaleriaImagensSituacaoPlanejamento,
            this.HistoricoBarButton,
            this.ExibirHistoricoCheck,
            this.barraTxDescricaoCronograma,
            this.barraTxDescricaoSituacaoPlanejmaneto,
            this.barraComboCronogramaSelecionado,
            this.barStaticItem1,
            this.barStaticItem2,
            this.lbColaboradorConectado,
            this.lbEstadorServidor,
            this.barButtonItem1,
            this.barEditItem3,
            this.lbUltimaAlteracao,
            this.txEditIdPopup,
            this.lbEstadoAcoes,
            this.filtroSituacaoTodas,
            this.filtroSituacaoPendentes,
            this.filtroSituacaoEncerradas,
            this.filtroSituacaoCustom,
            this.barraDtInicioCronograma,
            this.barraDtTerminoCronograma});
			this.Menu.LargeImages = this.imageCollection4;
			this.Menu.Location = new System.Drawing.Point(0, 0);
			this.Menu.MaxItemId = 138;
			this.Menu.MiniToolbars.Add(this.ribbonMiniToolbar1);
			this.Menu.MiniToolbars.Add(this.ribbonMiniToolbar2);
			this.Menu.Name = "Menu";
			this.Menu.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.Planejamento,
            this.Tarefas});
			this.Menu.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.barraRepositorioTxDescricaoCronograma,
            this.repositoryItemTextEdit3,
            this.barraRepositorioTxDescricaoSituacaoPlanejamento,
            this.repositoryItemBorderLineStyle1,
            this.repositoryItemButtonEdit1,
            this.repositoryItemComboBox2,
            this.repositoryItemTextEdit2,
            this.barraRepositorioComboCronogramaSelecionado,
            this.repositoryMoverPopup,
            this.repositoryItemPopupContainerEdit1,
            this.repositoryItemPopupContainerEdit2,
            this.repositoryItemTextEdit4,
            this.dtInicioCronograma,
            this.dtTerminoCronograma});
			this.Menu.Size = new System.Drawing.Size(1156, 142);
			this.Menu.StatusBar = this.barraStatus;
			this.Menu.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
			// 
			// imageCollection3
			// 
			this.imageCollection3.ImageSize = new System.Drawing.Size(32, 32);
			this.imageCollection3.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection3.ImageStream")));
			this.imageCollection3.Images.SetKeyName(0, "Action_Refresh_32x32.png");
			this.imageCollection3.Images.SetKeyName(1, "Action_Close_32x32.png");
			this.imageCollection3.Images.SetKeyName(2, "Action_Delete_32x32.png");
			this.imageCollection3.Images.SetKeyName(7, "icon-linha-base-wex.png");
			this.imageCollection3.Images.SetKeyName(8, "excluir_tarefa16.png");
			this.imageCollection3.Images.SetKeyName(9, "nova_tarefa16.png");
			this.imageCollection3.Images.SetKeyName(10, "nova_tarefa16.png");
			this.imageCollection3.Images.SetKeyName(11, "saveBaseLine16.png");
			this.imageCollection3.Images.SetKeyName(12, "saveBaseLine16.png");
			this.imageCollection3.Images.SetKeyName(13, "saveBaseLine16.png");
			this.imageCollection3.Images.SetKeyName(14, "save-line-base.png");
			this.imageCollection3.Images.SetKeyName(15, "historico.png");
			// 
			// barraBtNovoCronograma
			// 
			this.barraBtNovoCronograma.Caption = "Novo Cronograma";
			this.barraBtNovoCronograma.Hint = "Cria um novo cronograma.";
			this.barraBtNovoCronograma.Id = 13;
			this.barraBtNovoCronograma.ItemAppearance.Normal.Options.UseImage = true;
			this.barraBtNovoCronograma.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
			this.barraBtNovoCronograma.LargeImageIndex = 6;
			this.barraBtNovoCronograma.LargeImageIndexDisabled = 6;
			this.barraBtNovoCronograma.Name = "barraBtNovoCronograma";
			this.barraBtNovoCronograma.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barraBotaoNovoCronograma_ItemClick);
			// 
			// barraBtExcluirCronograma
			// 
			this.barraBtExcluirCronograma.Caption = "Excluir Cronograma";
			this.barraBtExcluirCronograma.Hint = "Exclui o cronograma selecionado.";
			this.barraBtExcluirCronograma.Id = 16;
			this.barraBtExcluirCronograma.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.D));
			this.barraBtExcluirCronograma.LargeImageIndex = 2;
			this.barraBtExcluirCronograma.LargeImageIndexDisabled = 2;
			this.barraBtExcluirCronograma.Name = "barraBtExcluirCronograma";
			this.barraBtExcluirCronograma.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barraBtExcluirCronograma_ItemClick);
			// 
			// barraBtAtualizarCronograma
			// 
			this.barraBtAtualizarCronograma.Caption = "Atualizar";
			this.barraBtAtualizarCronograma.Hint = "Atualiza o cronograma selecionado.";
			this.barraBtAtualizarCronograma.Id = 18;
			this.barraBtAtualizarCronograma.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
			this.barraBtAtualizarCronograma.LargeImageIndex = 0;
			this.barraBtAtualizarCronograma.Name = "barraBtAtualizarCronograma";
			// 
			// barraBotao_Fechar
			// 
			this.barraBotao_Fechar.Caption = "Fechar";
			this.barraBotao_Fechar.Hint = "Fecha a janela.";
			this.barraBotao_Fechar.Id = 19;
			this.barraBotao_Fechar.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W));
			this.barraBotao_Fechar.LargeImageIndex = 1;
			this.barraBotao_Fechar.LargeImageIndexDisabled = 1;
			this.barraBotao_Fechar.Name = "barraBotao_Fechar";
			this.barraBotao_Fechar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barraBotao_Fechar_ItemClick_1);
			// 
			// barraBtNovaTarefa
			// 
			this.barraBtNovaTarefa.Caption = "Nova  Tarefa";
			this.barraBtNovaTarefa.Enabled = false;
			this.barraBtNovaTarefa.Hint = "Inclui uma nova tarefa ao cronograma selecionado.";
			this.barraBtNovaTarefa.Id = 23;
			this.barraBtNovaTarefa.ImageIndex = 26;
			this.barraBtNovaTarefa.ImageIndexDisabled = 26;
			this.barraBtNovaTarefa.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.Insert);
			this.barraBtNovaTarefa.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barraBtNovaTarefa.LargeGlyph")));
			this.barraBtNovaTarefa.Name = "barraBtNovaTarefa";
			this.barraBtNovaTarefa.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.NovaTarefa_ItemClick);
			// 
			// barraBtExcluirTarefa
			// 
			this.barraBtExcluirTarefa.Caption = "Excluir Tarefa";
			this.barraBtExcluirTarefa.Enabled = false;
			this.barraBtExcluirTarefa.Hint = "Exclui a tarefa selecionada do cronograma.";
			this.barraBtExcluirTarefa.Id = 24;
			this.barraBtExcluirTarefa.ImageIndex = 25;
			this.barraBtExcluirTarefa.ImageIndexDisabled = 25;
			this.barraBtExcluirTarefa.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete));
			this.barraBtExcluirTarefa.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barraBtExcluirTarefa.LargeGlyph")));
			this.barraBtExcluirTarefa.Name = "barraBtExcluirTarefa";
			this.barraBtExcluirTarefa.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barraBtExcluirTarefa_ItemClick);
			// 
			// CronogramaEdit
			// 
			this.CronogramaEdit.Caption = "Cronograma";
			this.CronogramaEdit.Edit = null;
			this.CronogramaEdit.Id = 25;
			this.CronogramaEdit.Name = "CronogramaEdit";
			this.CronogramaEdit.Width = 100;
			// 
			// barEditItem_Nome
			// 
			this.barEditItem_Nome.Caption = "  Nome";
			this.barEditItem_Nome.Edit = null;
			this.barEditItem_Nome.Id = 37;
			this.barEditItem_Nome.Name = "barEditItem_Nome";
			this.barEditItem_Nome.Width = 130;
			// 
			// bar_CronogramaSituacao
			// 
			this.bar_CronogramaSituacao.Caption = "  Situação";
			this.bar_CronogramaSituacao.Edit = null;
			this.bar_CronogramaSituacao.Enabled = false;
			this.bar_CronogramaSituacao.Id = 53;
			this.bar_CronogramaSituacao.Name = "bar_CronogramaSituacao";
			this.bar_CronogramaSituacao.Width = 130;
			// 
			// barraTextoCronograma
			// 
			this.barraTextoCronograma.Caption = "  Cronograma: ";
			this.barraTextoCronograma.Edit = null;
			this.barraTextoCronograma.Hint = "Nomeia o cronograma selecionado.";
			this.barraTextoCronograma.Id = 57;
			this.barraTextoCronograma.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6);
			this.barraTextoCronograma.Name = "barraTextoCronograma";
			this.barraTextoCronograma.Width = 130;
			// 
			// barButtonGoDesc
			// 
			this.barButtonGoDesc.Caption = "Ir para Descrição";
			this.barButtonGoDesc.Hint = "Posiciona o cursor no campo \"descrição\" da tarefa selecionada.";
			this.barButtonGoDesc.Id = 61;
			this.barButtonGoDesc.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2);
			this.barButtonGoDesc.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonGoDesc.LargeGlyph")));
			this.barButtonGoDesc.Name = "barButtonGoDesc";
			this.barButtonGoDesc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonGoDesc_ItemClick);
			// 
			// barButtonGoEst
			// 
			this.barButtonGoEst.Caption = "Ir para Estimativas";
			this.barButtonGoEst.Hint = "Posiciona o cursor no campo \"estimativa restante\" da tarefa selecionada.";
			this.barButtonGoEst.Id = 62;
			this.barButtonGoEst.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E));
			this.barButtonGoEst.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonGoEst.LargeGlyph")));
			this.barButtonGoEst.Name = "barButtonGoEst";
			this.barButtonGoEst.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonGoEst_ItemClick);
			// 
			// barButtonGoObs
			// 
			this.barButtonGoObs.Caption = "Ir para Observações";
			this.barButtonGoObs.Hint = "Posiciona o cursor no campo \"observação\" da tarefa selecionada.";
			this.barButtonGoObs.Id = 63;
			this.barButtonGoObs.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O));
			this.barButtonGoObs.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonGoObs.LargeGlyph")));
			this.barButtonGoObs.Name = "barButtonGoObs";
			this.barButtonGoObs.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonGoObs_ItemClick);
			// 
			// barButtonGoSearch
			// 
			this.barButtonGoSearch.Caption = "Pesquisar Tarefas";
			this.barButtonGoSearch.Hint = "Ir para Busca.";
			this.barButtonGoSearch.Id = 64;
			this.barButtonGoSearch.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3);
			this.barButtonGoSearch.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonGoSearch.LargeGlyph")));
			this.barButtonGoSearch.Name = "barButtonGoSearch";
			this.barButtonGoSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonGoSearch_ItemClick);
			// 
			// ribbonGaleriaImagensSituacaoPlanejamento
			// 
			this.ribbonGaleriaImagensSituacaoPlanejamento.Caption = "ribbonGalleryBarItem1";
			// 
			// ribbonGaleriaImagensSituacaoPlanejamento
			// 
			this.ribbonGaleriaImagensSituacaoPlanejamento.Gallery.ShowItemText = true;
			this.ribbonGaleriaImagensSituacaoPlanejamento.Hint = "Apresenta as opções de situação para as tarefas.";
			this.ribbonGaleriaImagensSituacaoPlanejamento.Id = 71;
			this.ribbonGaleriaImagensSituacaoPlanejamento.ItemAppearance.Hovered.ForeColor = System.Drawing.Color.Black;
			this.ribbonGaleriaImagensSituacaoPlanejamento.ItemAppearance.Hovered.Options.UseForeColor = true;
			this.ribbonGaleriaImagensSituacaoPlanejamento.Name = "ribbonGaleriaImagensSituacaoPlanejamento";
			this.ribbonGaleriaImagensSituacaoPlanejamento.GalleryItemClick += new DevExpress.XtraBars.Ribbon.GalleryItemClickEventHandler(this.ribbonGaleriaImagensSituacaoPlanejamento_GalleryItemClick);
			// 
			// HistoricoBarButton
			// 
			this.HistoricoBarButton.Caption = "Histórico de Atualização";
			this.HistoricoBarButton.Enabled = false;
			this.HistoricoBarButton.Hint = "Histórico de Atualização da Tarefa selecionada.";
			this.HistoricoBarButton.Id = 84;
			this.HistoricoBarButton.ImageIndex = 15;
			this.HistoricoBarButton.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H));
			this.HistoricoBarButton.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("HistoricoBarButton.LargeGlyph")));
			this.HistoricoBarButton.Name = "HistoricoBarButton";
			this.HistoricoBarButton.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.HistoricoBarButton_ItemClick);
			// 
			// ExibirHistoricoCheck
			// 
			this.ExibirHistoricoCheck.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
			this.ExibirHistoricoCheck.Caption = "Exibir colunas de atualização na lista de tarefas";
			this.ExibirHistoricoCheck.Checked = true;
			this.ExibirHistoricoCheck.Id = 87;
			this.ExibirHistoricoCheck.Name = "ExibirHistoricoCheck";
			this.ExibirHistoricoCheck.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.ExibirHistoricoCheck_CheckedChanged);
			// 
			// barraTxDescricaoCronograma
			// 
			this.barraTxDescricaoCronograma.Caption = "Cronograma:";
			this.barraTxDescricaoCronograma.Edit = this.barraRepositorioTxDescricaoCronograma;
			this.barraTxDescricaoCronograma.Enabled = false;
			this.barraTxDescricaoCronograma.Hint = "Nomeia o cronograma selecionado.";
			this.barraTxDescricaoCronograma.Id = 93;
			this.barraTxDescricaoCronograma.Name = "barraTxDescricaoCronograma";
			this.barraTxDescricaoCronograma.Width = 130;
			this.barraTxDescricaoCronograma.HiddenEditor += new DevExpress.XtraBars.ItemClickEventHandler(this.AoEncerrarEdicaoDadosCronograma);
			this.barraTxDescricaoCronograma.ShownEditor += new DevExpress.XtraBars.ItemClickEventHandler(this.AoIniciarEdicaoDadosCronograma);
			// 
			// barraRepositorioTxDescricaoCronograma
			// 
			this.barraRepositorioTxDescricaoCronograma.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.barraRepositorioTxDescricaoCronograma.Appearance.ForeColor = System.Drawing.Color.Black;
			this.barraRepositorioTxDescricaoCronograma.Appearance.Options.UseBackColor = true;
			this.barraRepositorioTxDescricaoCronograma.Appearance.Options.UseForeColor = true;
			this.barraRepositorioTxDescricaoCronograma.AutoHeight = false;
			this.barraRepositorioTxDescricaoCronograma.Name = "barraRepositorioTxDescricaoCronograma";
			// 
			// barraTxDescricaoSituacaoPlanejmaneto
			// 
			this.barraTxDescricaoSituacaoPlanejmaneto.Caption = "Situação:";
			this.barraTxDescricaoSituacaoPlanejmaneto.Edit = this.barraRepositorioTxDescricaoSituacaoPlanejamento;
			this.barraTxDescricaoSituacaoPlanejmaneto.Enabled = false;
			this.barraTxDescricaoSituacaoPlanejmaneto.Id = 95;
			this.barraTxDescricaoSituacaoPlanejmaneto.Name = "barraTxDescricaoSituacaoPlanejmaneto";
			this.barraTxDescricaoSituacaoPlanejmaneto.Width = 130;
			// 
			// barraRepositorioTxDescricaoSituacaoPlanejamento
			// 
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.Appearance.Options.UseBackColor = true;
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.AppearanceDisabled.BackColor = System.Drawing.Color.LightGray;
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.AppearanceDisabled.Options.UseBackColor = true;
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.AppearanceDisabled.Options.UseForeColor = true;
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.AutoHeight = false;
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.Name = "barraRepositorioTxDescricaoSituacaoPlanejamento";
			this.barraRepositorioTxDescricaoSituacaoPlanejamento.ReadOnly = true;
			// 
			// barraComboCronogramaSelecionado
			// 
			this.barraComboCronogramaSelecionado.Edit = this.barraRepositorioComboCronogramaSelecionado;
			this.barraComboCronogramaSelecionado.Id = 99;
			this.barraComboCronogramaSelecionado.Name = "barraComboCronogramaSelecionado";
			this.barraComboCronogramaSelecionado.Width = 150;
			this.barraComboCronogramaSelecionado.EditValueChanged += new System.EventHandler(this.barraCronogramaSelecionado_EditValueChanged);
			this.barraComboCronogramaSelecionado.ShowingEditor += new DevExpress.XtraBars.ItemCancelEventHandler(this.barraComboCronogramaSelecionado_ShowingEditor);
			// 
			// barraRepositorioComboCronogramaSelecionado
			// 
			this.barraRepositorioComboCronogramaSelecionado.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.barraRepositorioComboCronogramaSelecionado.Appearance.ForeColor = System.Drawing.Color.Black;
			this.barraRepositorioComboCronogramaSelecionado.Appearance.Options.UseBackColor = true;
			this.barraRepositorioComboCronogramaSelecionado.Appearance.Options.UseForeColor = true;
			this.barraRepositorioComboCronogramaSelecionado.AutoHeight = false;
			this.barraRepositorioComboCronogramaSelecionado.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.barraRepositorioComboCronogramaSelecionado.Name = "barraRepositorioComboCronogramaSelecionado";
			this.barraRepositorioComboCronogramaSelecionado.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			// 
			// barStaticItem1
			// 
			this.barStaticItem1.Caption = "Mario Criou a Tarefa \'T1\'";
			this.barStaticItem1.Id = 101;
			this.barStaticItem1.Name = "barStaticItem1";
			this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
			// 
			// barStaticItem2
			// 
			this.barStaticItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
			this.barStaticItem2.Caption = "Servidor Desconectado";
			this.barStaticItem2.Id = 102;
			this.barStaticItem2.Name = "barStaticItem2";
			this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
			// 
			// lbColaboradorConectado
			// 
			this.lbColaboradorConectado.Id = 100;
			this.lbColaboradorConectado.Name = "lbColaboradorConectado";
			this.lbColaboradorConectado.TextAlignment = System.Drawing.StringAlignment.Near;
			// 
			// lbEstadorServidor
			// 
			this.lbEstadorServidor.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
			this.lbEstadorServidor.Id = 105;
			this.lbEstadorServidor.Name = "lbEstadorServidor";
			this.lbEstadorServidor.TextAlignment = System.Drawing.StringAlignment.Near;
			// 
			// barButtonItem1
			// 
			this.barButtonItem1.Caption = "Ir para Busca";
			this.barButtonItem1.Hint = "Ir para Busca.";
			this.barButtonItem1.Id = 64;
			this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3);
			this.barButtonItem1.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.LargeGlyph")));
			this.barButtonItem1.Name = "barButtonItem1";
			// 
			// barEditItem3
			// 
			this.barEditItem3.Caption = "barEditItem3";
			this.barEditItem3.Edit = this.repositoryItemTextEdit2;
			this.barEditItem3.Id = 111;
			this.barEditItem3.Name = "barEditItem3";
			// 
			// repositoryItemTextEdit2
			// 
			this.repositoryItemTextEdit2.AutoHeight = false;
			this.repositoryItemTextEdit2.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
			this.repositoryItemTextEdit2.Mask.BeepOnError = true;
			this.repositoryItemTextEdit2.Mask.EditMask = "[0-9]{1,5}";
			this.repositoryItemTextEdit2.Mask.IgnoreMaskBlank = false;
			this.repositoryItemTextEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
			this.repositoryItemTextEdit2.Mask.ShowPlaceHolders = false;
			this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
			// 
			// lbUltimaAlteracao
			// 
			this.lbUltimaAlteracao.Id = 118;
			this.lbUltimaAlteracao.Name = "lbUltimaAlteracao";
			this.lbUltimaAlteracao.TextAlignment = System.Drawing.StringAlignment.Near;
			// 
			// txEditIdPopup
			// 
			this.txEditIdPopup.Caption = "Mover Para:";
			this.txEditIdPopup.Edit = this.repositoryItemTextEdit4;
			this.txEditIdPopup.Id = 123;
			this.txEditIdPopup.Name = "txEditIdPopup";
			// 
			// repositoryItemTextEdit4
			// 
			this.repositoryItemTextEdit4.AutoHeight = false;
			this.repositoryItemTextEdit4.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.None;
			this.repositoryItemTextEdit4.Mask.BeepOnError = true;
			this.repositoryItemTextEdit4.Mask.EditMask = "[0-9]{1,5}";
			this.repositoryItemTextEdit4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
			this.repositoryItemTextEdit4.Mask.ShowPlaceHolders = false;
			this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
			// 
			// lbEstadoAcoes
			// 
			this.lbEstadoAcoes.Id = 125;
			this.lbEstadoAcoes.Name = "lbEstadoAcoes";
			this.lbEstadoAcoes.TextAlignment = System.Drawing.StringAlignment.Near;
			// 
			// filtroSituacaoTodas
			// 
			this.filtroSituacaoTodas.Caption = "Todas";
			this.filtroSituacaoTodas.Checked = true;
			this.filtroSituacaoTodas.GroupIndex = 5;
			this.filtroSituacaoTodas.Id = 132;
			this.filtroSituacaoTodas.LargeGlyph = global::WexProject.Schedule.Library.Properties.Resources._32x32_Pesquisar_Tudo;
			this.filtroSituacaoTodas.LargeWidth = 80;
			this.filtroSituacaoTodas.Name = "filtroSituacaoTodas";
			this.filtroSituacaoTodas.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.filtroSituacao_ItemClick);
			// 
			// filtroSituacaoPendentes
			// 
			this.filtroSituacaoPendentes.Caption = "Pendentes";
			this.filtroSituacaoPendentes.GroupIndex = 5;
			this.filtroSituacaoPendentes.Id = 133;
			this.filtroSituacaoPendentes.LargeGlyph = global::WexProject.Schedule.Library.Properties.Resources._32x32_Tarefas_Pendentes;
			this.filtroSituacaoPendentes.LargeWidth = 80;
			this.filtroSituacaoPendentes.Name = "filtroSituacaoPendentes";
			this.filtroSituacaoPendentes.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.filtroSituacao_ItemClick);
			// 
			// filtroSituacaoEncerradas
			// 
			this.filtroSituacaoEncerradas.Caption = "Encerradas";
			this.filtroSituacaoEncerradas.GroupIndex = 5;
			this.filtroSituacaoEncerradas.Id = 134;
			this.filtroSituacaoEncerradas.LargeGlyph = global::WexProject.Schedule.Library.Properties.Resources._32x32_Tarefas_Encerradas;
			this.filtroSituacaoEncerradas.LargeWidth = 80;
			this.filtroSituacaoEncerradas.Name = "filtroSituacaoEncerradas";
			this.filtroSituacaoEncerradas.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.filtroSituacao_ItemClick);
			// 
			// filtroSituacaoCustom
			// 
			this.filtroSituacaoCustom.Caption = "Personalizado";
			this.filtroSituacaoCustom.GroupIndex = 5;
			this.filtroSituacaoCustom.Id = 135;
			this.filtroSituacaoCustom.LargeGlyph = global::WexProject.Schedule.Library.Properties.Resources._32x32_Pesquisa_Personalizado;
			this.filtroSituacaoCustom.LargeWidth = 80;
			this.filtroSituacaoCustom.Name = "filtroSituacaoCustom";
			this.filtroSituacaoCustom.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.filtroSituacao_ItemClick);
			// 
			// barraDtInicioCronograma
			// 
			this.barraDtInicioCronograma.Caption = "Início";
			this.barraDtInicioCronograma.Edit = this.dtInicioCronograma;
			this.barraDtInicioCronograma.Id = 136;
			this.barraDtInicioCronograma.Name = "barraDtInicioCronograma";
			this.barraDtInicioCronograma.Width = 130;
			this.barraDtInicioCronograma.HiddenEditor += new DevExpress.XtraBars.ItemClickEventHandler(this.AoEncerrarEdicaoDadosCronograma);
			this.barraDtInicioCronograma.ShownEditor += new DevExpress.XtraBars.ItemClickEventHandler(this.AoIniciarEdicaoDadosCronograma);
			// 
			// dtInicioCronograma
			// 
			this.dtInicioCronograma.AutoHeight = false;
			this.dtInicioCronograma.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtInicioCronograma.Name = "dtInicioCronograma";
			this.dtInicioCronograma.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			// 
			// barraDtTerminoCronograma
			// 
			this.barraDtTerminoCronograma.Caption = "Término";
			this.barraDtTerminoCronograma.Edit = this.dtTerminoCronograma;
			this.barraDtTerminoCronograma.Id = 137;
			this.barraDtTerminoCronograma.Name = "barraDtTerminoCronograma";
			this.barraDtTerminoCronograma.Width = 130;
			this.barraDtTerminoCronograma.HiddenEditor += new DevExpress.XtraBars.ItemClickEventHandler(this.AoEncerrarEdicaoDadosCronograma);
			this.barraDtTerminoCronograma.ShownEditor += new DevExpress.XtraBars.ItemClickEventHandler(this.AoIniciarEdicaoDadosCronograma);
			// 
			// dtTerminoCronograma
			// 
			this.dtTerminoCronograma.AutoHeight = false;
			this.dtTerminoCronograma.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtTerminoCronograma.Name = "dtTerminoCronograma";
			this.dtTerminoCronograma.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			// 
			// imageCollection4
			// 
			this.imageCollection4.ImageSize = new System.Drawing.Size(32, 32);
			this.imageCollection4.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection4.ImageStream")));
			this.imageCollection4.Images.SetKeyName(0, "Action_Refresh_32x32.png");
			this.imageCollection4.Images.SetKeyName(1, "Action_Close_32x32.png");
			this.imageCollection4.Images.SetKeyName(2, "Action_Delete_32x32.png");
			this.imageCollection4.Images.SetKeyName(6, "Action_New_32x32.png");
			this.imageCollection4.Images.SetKeyName(7, "icon-linha-base-wex.png");
			this.imageCollection4.Images.SetKeyName(8, "nova_tarefa16.png");
			// 
			// Planejamento
			// 
			this.Planejamento.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonHome,
            this.ribbonEdicao,
            this.ribbonDadosGerais,
            this.ribbonPeriodo,
            this.ribbonCronogramaSelecionado,
            this.ribbonLinhaBase,
            this.ribbonFechar});
			this.Planejamento.Name = "Planejamento";
			this.Planejamento.Text = "Dados do Cronograma";
			// 
			// ribbonHome
			// 
			this.ribbonHome.ItemLinks.Add(this.barraBtNovoCronograma);
			this.ribbonHome.Name = "ribbonHome";
			this.ribbonHome.ShowCaptionButton = false;
			this.ribbonHome.Text = "Criação";
			// 
			// ribbonEdicao
			// 
			this.ribbonEdicao.ItemLinks.Add(this.barraBtExcluirCronograma);
			this.ribbonEdicao.Name = "ribbonEdicao";
			this.ribbonEdicao.ShowCaptionButton = false;
			this.ribbonEdicao.Text = "Edição";
			// 
			// ribbonDadosGerais
			// 
			this.ribbonDadosGerais.ItemLinks.Add(this.barEditItem_Nome);
			this.ribbonDadosGerais.ItemLinks.Add(this.bar_CronogramaSituacao);
			this.ribbonDadosGerais.ItemLinks.Add(this.barraTextoCronograma);
			this.ribbonDadosGerais.ItemLinks.Add(this.barraTxDescricaoCronograma);
			this.ribbonDadosGerais.ItemLinks.Add(this.barraTxDescricaoSituacaoPlanejmaneto);
			this.ribbonDadosGerais.Name = "ribbonDadosGerais";
			this.ribbonDadosGerais.ShowCaptionButton = false;
			this.ribbonDadosGerais.Text = "Dados Gerais";
			// 
			// ribbonPeriodo
			// 
			this.ribbonPeriodo.AllowTextClipping = false;
			this.ribbonPeriodo.ItemLinks.Add(this.barraDtInicioCronograma);
			this.ribbonPeriodo.ItemLinks.Add(this.barraDtTerminoCronograma);
			this.ribbonPeriodo.Name = "ribbonPeriodo";
			this.ribbonPeriodo.Text = "Período";
			// 
			// ribbonCronogramaSelecionado
			// 
			this.ribbonCronogramaSelecionado.AllowTextClipping = false;
			this.ribbonCronogramaSelecionado.ItemLinks.Add(this.barraComboCronogramaSelecionado);
			this.ribbonCronogramaSelecionado.Name = "ribbonCronogramaSelecionado";
			this.ribbonCronogramaSelecionado.ShowCaptionButton = false;
			this.ribbonCronogramaSelecionado.Text = "Cronograma Atual";
			// 
			// ribbonLinhaBase
			// 
			this.ribbonLinhaBase.ItemLinks.Add(this.barButtonGoSearch);
			this.ribbonLinhaBase.Name = "ribbonLinhaBase";
			this.ribbonLinhaBase.ShowCaptionButton = false;
			this.ribbonLinhaBase.Text = "Tarefas";
			// 
			// ribbonFechar
			// 
			this.ribbonFechar.AllowTextClipping = false;
			this.ribbonFechar.ItemLinks.Add(this.barraBotao_Fechar);
			this.ribbonFechar.Name = "ribbonFechar";
			this.ribbonFechar.ShowCaptionButton = false;
			this.ribbonFechar.Text = "Janela";
			// 
			// Tarefas
			// 
			this.Tarefas.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.newTask,
            this.barraTarefasGridControl,
            this.ribbonPageGroup2,
            this.Historico,
            this.ribbonPageGroup1,
            this.Janela});
			this.Tarefas.Name = "Tarefas";
			this.Tarefas.Text = "Tarefas";
			// 
			// newTask
			// 
			this.newTask.ItemLinks.Add(this.barraBtNovaTarefa);
			this.newTask.ItemLinks.Add(this.barraBtExcluirTarefa);
			this.newTask.Name = "newTask";
			this.newTask.ShowCaptionButton = false;
			this.newTask.Text = "Opções";
			// 
			// barraTarefasGridControl
			// 
			this.barraTarefasGridControl.ItemLinks.Add(this.barButtonGoSearch);
			this.barraTarefasGridControl.ItemLinks.Add(this.barButtonGoDesc);
			this.barraTarefasGridControl.ItemLinks.Add(this.barButtonGoObs);
			this.barraTarefasGridControl.ItemLinks.Add(this.barButtonGoEst);
			this.barraTarefasGridControl.Name = "barraTarefasGridControl";
			this.barraTarefasGridControl.ShowCaptionButton = false;
			this.barraTarefasGridControl.Text = "Controladores do Grid";
			// 
			// ribbonPageGroup2
			// 
			this.ribbonPageGroup2.AllowMinimize = false;
			this.ribbonPageGroup2.ItemLinks.Add(this.ribbonGaleriaImagensSituacaoPlanejamento);
			this.ribbonPageGroup2.Name = "ribbonPageGroup2";
			this.ribbonPageGroup2.ShowCaptionButton = false;
			this.ribbonPageGroup2.Text = "Situação de Planejamento";
			// 
			// Historico
			// 
			this.Historico.ItemLinks.Add(this.HistoricoBarButton);
			this.Historico.ItemLinks.Add(this.ExibirHistoricoCheck);
			this.Historico.Name = "Historico";
			this.Historico.ShowCaptionButton = false;
			this.Historico.Text = "Histórico";
			// 
			// ribbonPageGroup1
			// 
			this.ribbonPageGroup1.AllowTextClipping = false;
			this.ribbonPageGroup1.ItemLinks.Add(this.filtroSituacaoTodas);
			this.ribbonPageGroup1.ItemLinks.Add(this.filtroSituacaoPendentes);
			this.ribbonPageGroup1.ItemLinks.Add(this.filtroSituacaoEncerradas);
			this.ribbonPageGroup1.ItemLinks.Add(this.filtroSituacaoCustom);
			this.ribbonPageGroup1.Name = "ribbonPageGroup1";
			this.ribbonPageGroup1.Text = "Filtros Situações Tarefa";
			// 
			// Janela
			// 
			this.Janela.AllowTextClipping = false;
			this.Janela.ItemLinks.Add(this.barraBotao_Fechar);
			this.Janela.Name = "Janela";
			this.Janela.Text = "Janela";
			// 
			// repositoryItemTextEdit3
			// 
			this.repositoryItemTextEdit3.AutoHeight = false;
			this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
			// 
			// repositoryItemBorderLineStyle1
			// 
			this.repositoryItemBorderLineStyle1.AutoHeight = false;
			this.repositoryItemBorderLineStyle1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemBorderLineStyle1.Control = null;
			this.repositoryItemBorderLineStyle1.Name = "repositoryItemBorderLineStyle1";
			// 
			// repositoryItemButtonEdit1
			// 
			this.repositoryItemButtonEdit1.AutoHeight = false;
			this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
			// 
			// repositoryItemComboBox2
			// 
			this.repositoryItemComboBox2.AutoHeight = false;
			this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
			// 
			// repositoryMoverPopup
			// 
			this.repositoryMoverPopup.AutoHeight = false;
			this.repositoryMoverPopup.Mask.BeepOnError = true;
			this.repositoryMoverPopup.Mask.EditMask = "[0-9]{1,5}";
			this.repositoryMoverPopup.Mask.IgnoreMaskBlank = false;
			this.repositoryMoverPopup.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
			this.repositoryMoverPopup.Mask.ShowPlaceHolders = false;
			this.repositoryMoverPopup.Name = "repositoryMoverPopup";
			this.repositoryMoverPopup.ValidateOnEnterKey = true;
			// 
			// repositoryItemPopupContainerEdit1
			// 
			this.repositoryItemPopupContainerEdit1.AutoHeight = false;
			this.repositoryItemPopupContainerEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemPopupContainerEdit1.Name = "repositoryItemPopupContainerEdit1";
			this.repositoryItemPopupContainerEdit1.PopupSizeable = false;
			// 
			// repositoryItemPopupContainerEdit2
			// 
			this.repositoryItemPopupContainerEdit2.AutoHeight = false;
			this.repositoryItemPopupContainerEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.repositoryItemPopupContainerEdit2.Name = "repositoryItemPopupContainerEdit2";
			// 
			// barraStatus
			// 
			this.barraStatus.ItemLinks.Add(this.lbColaboradorConectado);
			this.barraStatus.ItemLinks.Add(this.lbEstadorServidor);
			this.barraStatus.ItemLinks.Add(this.lbEstadoAcoes, true);
			this.barraStatus.ItemLinks.Add(this.lbUltimaAlteracao, true);
			this.barraStatus.Location = new System.Drawing.Point(0, 534);
			this.barraStatus.Name = "barraStatus";
			this.barraStatus.Ribbon = this.Menu;
			this.barraStatus.Size = new System.Drawing.Size(1156, 27);
			// 
			// barraUsuariosConectados
			// 
			this.barraUsuariosConectados.Dock = System.Windows.Forms.DockStyle.Left;
			this.barraUsuariosConectados.Location = new System.Drawing.Point(0, 142);
			this.barraUsuariosConectados.Name = "barraUsuariosConectados";
			this.barraUsuariosConectados.Padding = new System.Windows.Forms.Padding(4);
			this.barraUsuariosConectados.Size = new System.Drawing.Size(26, 126);
			this.barraUsuariosConectados.TabIndex = 5;
			// 
			// popupMenu1
			// 
			this.popupMenu1.ItemLinks.Add(this.txEditIdPopup);
			this.popupMenu1.Name = "popupMenu1";
			this.popupMenu1.Ribbon = this.Menu;
			this.popupMenu1.CloseUp += new System.EventHandler(this.popupMenu1_CloseUp);
			// 
			// imageCollection1
			// 
			this.imageCollection1.ImageSize = new System.Drawing.Size(32, 32);
			this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
			this.imageCollection1.Images.SetKeyName(0, "Action_Refresh_32x32.png");
			this.imageCollection1.Images.SetKeyName(1, "Action_Close_32x32.png");
			this.imageCollection1.Images.SetKeyName(2, "Action_Delete_32x32.png");
			this.imageCollection1.Images.SetKeyName(7, "icon-linha-base-wex.png");
			this.imageCollection1.Images.SetKeyName(8, "excluir_tarefa16.png");
			this.imageCollection1.Images.SetKeyName(9, "nova_tarefa16.png");
			this.imageCollection1.Images.SetKeyName(10, "nova_tarefa16.png");
			this.imageCollection1.Images.SetKeyName(11, "saveBaseLine16.png");
			this.imageCollection1.Images.SetKeyName(12, "saveBaseLine16.png");
			this.imageCollection1.Images.SetKeyName(13, "saveBaseLine16.png");
			this.imageCollection1.Images.SetKeyName(14, "save-line-base.png");
			this.imageCollection1.Images.SetKeyName(15, "historico.png");
			// 
			// imageCollection2
			// 
			this.imageCollection2.ImageSize = new System.Drawing.Size(32, 32);
			this.imageCollection2.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection2.ImageStream")));
			this.imageCollection2.Images.SetKeyName(0, "Action_Refresh_32x32.png");
			this.imageCollection2.Images.SetKeyName(1, "Action_Close_32x32.png");
			this.imageCollection2.Images.SetKeyName(2, "Action_Delete_32x32.png");
			this.imageCollection2.Images.SetKeyName(6, "Action_New_32x32.png");
			this.imageCollection2.Images.SetKeyName(7, "icon-linha-base-wex.png");
			this.imageCollection2.Images.SetKeyName(8, "nova_tarefa16.png");
			// 
			// barEditItem2
			// 
			this.barEditItem2.Caption = "  Cronograma: ";
			this.barEditItem2.Edit = null;
			this.barEditItem2.Hint = "Nomeia o cronograma selecionado.";
			this.barEditItem2.Id = 57;
			this.barEditItem2.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F6);
			this.barEditItem2.Name = "barEditItem2";
			this.barEditItem2.Width = 130;
			// 
			// MultiAccessClient
			// 
			this.MultiAccessClient.Buffer = null;
			this.MultiAccessClient.Conectado = false;
			this.MultiAccessClient.EnderecoIp = "10.0.2.31";
			this.MultiAccessClient.Login = "WexUser";
			this.MultiAccessClient.NomeServidor = null;
			this.MultiAccessClient.OidCronograma = "C1";
			this.MultiAccessClient.Porta = 8000;
			// 
			// MensagemPopup
			// 
			this.MensagemPopup.AllowHotTrack = false;
			this.MensagemPopup.AppearanceCaption.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MensagemPopup.AppearanceCaption.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.MensagemPopup.AppearanceCaption.Options.UseFont = true;
			this.MensagemPopup.AppearanceCaption.Options.UseForeColor = true;
			this.MensagemPopup.AppearanceHotTrackedText.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.MensagemPopup.AppearanceHotTrackedText.Options.UseFont = true;
			this.MensagemPopup.AppearanceHotTrackedText.Options.UseForeColor = true;
			this.MensagemPopup.AppearanceText.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MensagemPopup.AppearanceText.Options.UseFont = true;
			this.MensagemPopup.AppearanceText.Options.UseTextOptions = true;
			this.MensagemPopup.AppearanceText.TextOptions.Trimming = DevExpress.Utils.Trimming.None;
			this.MensagemPopup.AppearanceText.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
			this.MensagemPopup.AppearanceText.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.MensagemPopup.AutoFormDelay = 4000;
			this.MensagemPopup.FormDisplaySpeed = DevExpress.XtraBars.Alerter.AlertFormDisplaySpeed.Fast;
			this.MensagemPopup.FormMaxCount = 5;
			this.MensagemPopup.FormShowingEffect = DevExpress.XtraBars.Alerter.AlertFormShowingEffect.SlideVertical;
			this.MensagemPopup.LookAndFeel.SkinName = "Seven Classic";
			this.MensagemPopup.ShowPinButton = false;
			// 
			// barManager1
			// 
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.DockManager = this.dockManager;
			this.barManager1.Form = this;
			this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1});
			this.barManager1.MaxItemId = 1;
			// 
			// barDockControlTop
			// 
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(1156, 0);
			// 
			// barDockControlBottom
			// 
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 561);
			this.barDockControlBottom.Size = new System.Drawing.Size(1156, 0);
			// 
			// barDockControlLeft
			// 
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 561);
			// 
			// barDockControlRight
			// 
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(1156, 0);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 561);
			// 
			// dockManager
			// 
			this.dockManager.Form = this;
			this.dockManager.MenuManager = this.barManager1;
			this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelBurndown});
			this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
			// 
			// dockPanelBurndown
			// 
			this.dockPanelBurndown.Controls.Add(this.dockPanel1_Container);
			this.dockPanelBurndown.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
			this.dockPanelBurndown.ID = new System.Guid("61b79934-7a5b-4b98-a571-fa8e5de2052d");
			this.dockPanelBurndown.Location = new System.Drawing.Point(0, 268);
			this.dockPanelBurndown.Name = "dockPanelBurndown";
			this.dockPanelBurndown.Options.AllowDockLeft = false;
			this.dockPanelBurndown.OriginalSize = new System.Drawing.Size(431, 266);
			this.dockPanelBurndown.Size = new System.Drawing.Size(1156, 266);
			this.dockPanelBurndown.Text = "Burndown";
			this.dockPanelBurndown.VisibilityChanged += new DevExpress.XtraBars.Docking.VisibilityChangedEventHandler(this.dockPanelBurndown_VisibilityChanged);
			this.dockPanelBurndown.ClosingPanel += new DevExpress.XtraBars.Docking.DockPanelCancelEventHandler(this.dockPanelBurndown_ClosingPanel);
			this.dockPanelBurndown.Click += new System.EventHandler(this.dockPanelBurndown_Click);
			// 
			// dockPanel1_Container
			// 
			this.dockPanel1_Container.Controls.Add(this.chartControl1);
			this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
			this.dockPanel1_Container.Name = "dockPanel1_Container";
			this.dockPanel1_Container.Size = new System.Drawing.Size(1148, 239);
			this.dockPanel1_Container.TabIndex = 0;
			// 
			// chartControl1
			// 
			xyDiagram1.AxisX.DateTimeOptions.Format = DevExpress.XtraCharts.DateTimeFormat.Custom;
			xyDiagram1.AxisX.DateTimeOptions.FormatString = "dd/MMM";
			xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
			xyDiagram1.AxisX.Range.SideMarginsEnabled = false;
			xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
			xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
			xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
			xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
			this.chartControl1.Diagram = xyDiagram1;
			this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chartControl1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Right;
			this.chartControl1.Location = new System.Drawing.Point(0, 0);
			this.chartControl1.Name = "chartControl1";
			pointSeriesLabel1.Angle = 25;
			pointSeriesLabel1.LineVisible = true;
			pointOptions1.Pattern = "{V}h";
			pointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
			pointSeriesLabel1.PointOptions = pointOptions1;
			series1.Label = pointSeriesLabel1;
			series1.LegendText = "Planejado";
			series1.Name = "Series 1";
			series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            seriesPoint1,
            seriesPoint2,
            seriesPoint3,
            seriesPoint4,
            seriesPoint5,
            seriesPoint6,
            seriesPoint7});
			lineSeriesView1.Color = System.Drawing.Color.IndianRed;
			lineSeriesView1.LineMarkerOptions.Size = 5;
			lineSeriesView1.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dash;
			series1.View = lineSeriesView1;
			pointSeriesLabel2.Angle = 180;
			pointSeriesLabel2.LineVisible = true;
			pointOptions2.Pattern = "{V}h";
			pointOptions2.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
			pointSeriesLabel2.PointOptions = pointOptions2;
			series2.Label = pointSeriesLabel2;
			series2.LegendText = "Restante";
			series2.Name = "Series 3";
			series2.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            seriesPoint8,
            seriesPoint9,
            seriesPoint10,
            seriesPoint11,
            seriesPoint12,
            seriesPoint13,
            seriesPoint14});
			series2.SeriesPointsSorting = DevExpress.XtraCharts.SortingMode.Ascending;
			lineSeriesView2.Color = System.Drawing.Color.CornflowerBlue;
			lineSeriesView2.LineMarkerOptions.Size = 5;
			series2.View = lineSeriesView2;
			this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
			pointSeriesLabel3.LineVisible = true;
			this.chartControl1.SeriesTemplate.Label = pointSeriesLabel3;
			this.chartControl1.SeriesTemplate.View = lineSeriesView3;
			this.chartControl1.Size = new System.Drawing.Size(1148, 239);
			this.chartControl1.TabIndex = 0;
			chartTitle1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			chartTitle1.Text = "";
			this.chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
			// 
			// barSubItem1
			// 
			this.barSubItem1.Caption = "barSubItem1";
			this.barSubItem1.Id = 0;
			this.barSubItem1.Name = "barSubItem1";
			// 
			// imageCollectionSituacao
			// 
			this.imageCollectionSituacao.ImageSize = new System.Drawing.Size(32, 32);
			this.imageCollectionSituacao.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionSituacao.ImageStream")));
			// 
			// barEditItem1
			// 
			this.barEditItem1.Edit = null;
			this.barEditItem1.Id = -1;
			this.barEditItem1.Name = "barEditItem1";
			// 
			// imageCollectionGrid
			// 
			this.imageCollectionGrid.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionGrid.ImageStream")));
			this.imageCollectionGrid.Images.SetKeyName(0, "acima.png");
			this.imageCollectionGrid.Images.SetKeyName(1, "abaixo.png");
			this.imageCollectionGrid.Images.SetKeyName(2, "excluido.png");
			// 
			// ribbonControl1
			// 
			this.ribbonControl1.ApplicationButtonKeyTip = "";
			this.ribbonControl1.ApplicationIcon = null;
			this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
			this.ribbonControl1.Name = "ribbonControl1";
			this.ribbonControl1.Size = new System.Drawing.Size(0, 20);
			// 
			// barButtonItem3
			// 
			this.barButtonItem3.Caption = "Adicionar seta";
			this.barButtonItem3.Id = 127;
			this.barButtonItem3.Name = "barButtonItem3";
			// 
			// CronogramaView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1156, 561);
			this.Controls.Add(this.popupContainerControl1);
			this.Controls.Add(this.cronogramaTarefaGridControl);
			this.Controls.Add(this.barraUsuariosConectados);
			this.Controls.Add(this.dockPanelBurndown);
			this.Controls.Add(this.barraStatus);
			this.Controls.Add(this.Menu);
			this.Controls.Add(this.barDockControlLeft);
			this.Controls.Add(this.barDockControlRight);
			this.Controls.Add(this.barDockControlBottom);
			this.Controls.Add(this.barDockControlTop);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CronogramaView";
			this.Text = "Cronograma";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TelaCronograma_FormClosing);
			this.Load += new System.EventHandler(this.CronogramaView_Load);
			((System.ComponentModel.ISupportInitialize)(this.txDescricaoTarefaTextEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
			this.popupContainerControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cronogramaTarefaGridControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iconeEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TextAreaObservacaoTarefa)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioEdit.VistaTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioEdit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.comboEstimativaInicial)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.comboGridSituacaoPlanejamento)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositorioPopUpContainer)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.comboColaboradoresResponsaveis)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Menu)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barraRepositorioTxDescricaoCronograma)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barraRepositorioTxDescricaoSituacaoPlanejamento)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barraRepositorioComboCronogramaSelecionado)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioCronograma.VistaTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtInicioCronograma)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtTerminoCronograma.VistaTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtTerminoCronograma)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineStyle1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryMoverPopup)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barraUsuariosConectados)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
			this.dockPanelBurndown.ResumeLayout(false);
			this.dockPanel1_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollectionSituacao)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageCollectionGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cronogramaTarefaBindingSource)).EndInit();
			this.ResumeLayout(false);

        }
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.Utils.ImageCollection imageCollection2;
        private DevExpress.XtraBars.BarEditItem barEditItem2;
        private MultiAccess.Library.WexMultiAccessClient MultiAccessClient;
        private DevExpress.XtraBars.Alerter.AlertControl MensagemPopup;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.Ribbon.RibbonMiniToolbar ribbonMiniToolbar1;
        private DevExpress.XtraBars.Ribbon.RibbonMiniToolbar ribbonMiniToolbar2;
        private DevExpress.Utils.ImageCollection imageCollection3;
        private DevExpress.XtraBars.BarButtonItem barraBtNovoCronograma;
        private DevExpress.XtraBars.BarButtonItem barraBtExcluirCronograma;
        private DevExpress.XtraBars.BarButtonItem barraBtAtualizarCronograma;
        private DevExpress.Utils.ImageCollection imageCollection4;

        #endregion

       
        public DevExpress.XtraGrid.GridControl gridCronograma;
        public DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit SelectCheckResponsavel;
        public DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox itemImageSituacao;
        public DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit TextAreaObservacao;
        public DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        public DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
        public DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit6;
        public DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        public DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox2;
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox9;
        
        
        private DevExpress.Utils.ImageCollection imageCollectionSituacao;
        
        private DevExpress.Utils.ImageCollection colecaoImagensInfo;
        private DevExpress.XtraGrid.GridControl cronogramaTarefaGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaId;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaDescricaoTarefa;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryInfoGridRow;
        public DevExpress.XtraBars.Ribbon.RibbonControl Menu;
        private DevExpress.XtraBars.BarButtonItem barraBotaoNovoCronograma;
        private DevExpress.XtraBars.BarButtonItem barButton_Delete;
        private DevExpress.XtraBars.BarButtonItem barButton_Refresh;
        private DevExpress.XtraBars.BarButtonItem barraBotao_Fechar;
        private DevExpress.XtraBars.BarButtonItem barraBtNovaTarefa;
        private DevExpress.XtraBars.BarButtonItem barraBtExcluirTarefa;
        private DevExpress.XtraBars.BarEditItem CronogramaEdit;
        private DevExpress.XtraBars.BarEditItem barEditItem_Nome;
        private DevExpress.XtraBars.BarEditItem bar_CronogramaSituacao;
        private DevExpress.XtraBars.BarEditItem barraTextoCronograma;
        private DevExpress.XtraBars.BarButtonItem barButtonGoDesc;
        private DevExpress.XtraBars.BarButtonItem barButtonGoEst;
        private DevExpress.XtraBars.BarButtonItem barButtonGoObs;
        private DevExpress.XtraBars.BarButtonItem barButtonGoSearch;
        public DevExpress.XtraBars.RibbonGalleryBarItem ribbonGaleriaImagensSituacaoPlanejamento;
        protected DevExpress.XtraBars.BarButtonItem HistoricoBarButton;
        private DevExpress.XtraBars.BarCheckItem ExibirHistoricoCheck;
        public DevExpress.XtraBars.Ribbon.RibbonPage Planejamento;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonEdicao;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonDadosGerais;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonCronogramaSelecionado;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonLinhaBase;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup FecharAplicacao;
        public DevExpress.XtraBars.Ribbon.RibbonPage Tarefas;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup newTask;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup barraTarefasGridControl;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup Historico;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup Janela;

       
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        

        private System.Windows.Forms.ToolTip toolTip1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barMenuId;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit MenuId;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand Responsave;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand Situacao;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrincipal;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn NbId;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn TxTarefa;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand Realizado;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn itemImageSituacaoName;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn Responsavel;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand Estimativa;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn NbRestante;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn NbInicial;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn NbRealizado;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn TxObs;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn DtInicio;
        private DevExpress.XtraGrid.Columns.GridColumn bandedNbId;
        private DevExpress.XtraGrid.Columns.GridColumn bandedTxTarefa;
        private DevExpress.XtraGrid.Columns.GridColumn bandedResponsavel;
        private DevExpress.XtraGrid.Columns.GridColumn bandedNbRealizado;
        private DevExpress.XtraGrid.Columns.GridColumn bandedTxObs;
        private DevExpress.XtraGrid.Columns.GridColumn bandedDtInicio;
        private DevExpress.XtraGrid.Columns.GridColumn bandedNbInicial;
        private DevExpress.XtraGrid.Columns.GridColumn bandeditemImageSituacaoName;
        private DevExpress.XtraGrid.Columns.GridColumn bandedNbRestante;
        private DevExpress.XtraGrid.Columns.GridColumn bandedAtualizadoEm;
        private DevExpress.XtraGrid.Columns.GridColumn bandedAtualizadoPor;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraBars.BarEditItem barraTxDescricaoCronograma;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit barraRepositorioTxDescricaoCronograma;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraBars.BarEditItem barraTxDescricaoSituacaoPlanejmaneto;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit barraRepositorioTxDescricaoSituacaoPlanejamento;

        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit TextAreaObservacaoTarefa;
        private DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle repositoryItemBorderLineStyle1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraBars.BarEditItem barraComboCronogramaSelecionado;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox barraRepositorioComboCronogramaSelecionado;
        public DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit comboColaboradoresResponsaveis;
        private DevExpress.XtraBars.BarStaticItem lbColaboradorConectado;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar barraStatus;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem lbEstadorServidor;
        public BarraUsuariosConectados barraUsuariosConectados;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositorioPopUpContainer;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txDescricaoTarefaTextEdit;
        private DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
        private TarefaHistoricoView tarefaHistoricoView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaIcone;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit iconeEdit;
        private DevExpress.Utils.ImageCollection imageCollectionGrid;
        public DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox comboGridSituacaoPlanejamento;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaObs;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaRealizado;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaInicio;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaEstimativaInicial;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaSituacao;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaEstimativaRestante;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaAtualizadoEm;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaAtualizadoPor;
        private DevExpress.XtraGrid.Columns.GridColumn gridColunaResponsavel;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup FecharAplicativo;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonFechar;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit dtInicioEdit;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryMoverPopup;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupContainerEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupContainerEdit2;
        private DevExpress.XtraBars.BarStaticItem lbUltimaAlteracao;
        private DevExpress.DXCore.Controls.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarEditItem txEditIdPopup;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraBars.BarStaticItem lbEstadoAcoes;

        private System.Windows.Forms.BindingSource cronogramaTarefaBindingSource;
        private DevExpress.XtraBars.BarEditItem barEditItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarCheckItem filtroSituacaoTodas;
        private DevExpress.XtraBars.BarCheckItem filtroSituacaoPendentes;
        private DevExpress.XtraBars.BarCheckItem filtroSituacaoEncerradas;
        private DevExpress.XtraBars.BarCheckItem filtroSituacaoCustom;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox comboEstimativaInicial;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraBars.Docking.DockPanel dockPanelBurndown;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPeriodo;
        private DevExpress.XtraBars.BarEditItem barraDtInicioCronograma;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit dtInicioCronograma;
        private DevExpress.XtraBars.BarEditItem barraDtTerminoCronograma;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit dtTerminoCronograma;
    }

}