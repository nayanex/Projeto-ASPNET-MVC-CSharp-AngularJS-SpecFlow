using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel;
using WexProject.Module.Win.Projeto;

namespace WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral
{
    partial class ChartEstimadoRealizado
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Strip strip1 = new DevExpress.XtraCharts.Strip();
            DevExpress.XtraCharts.RectangleGradientFillOptions rectangleGradientFillOptions1 = new DevExpress.XtraCharts.RectangleGradientFillOptions();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions2 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel3 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions3 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel4 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions4 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.PointSeriesView pointSeriesView1 = new DevExpress.XtraCharts.PointSeriesView();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel5 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView4 = new DevExpress.XtraCharts.LineSeriesView();
            grafico = new DevExpress.XtraCharts.ChartControl();
            graficoEstimadoRealizadoDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(grafico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(strip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(graficoEstimadoRealizadoDtoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // grafico
            // 
            grafico.AppearanceNameSerializable = "Gray";
            grafico.DataSource = graficoEstimadoRealizadoDtoBindingSource;
            xyDiagram1.AxisX.GridSpacingAuto = false;
            xyDiagram1.AxisX.NumericOptions.Format = DevExpress.XtraCharts.NumericFormat.FixedPoint;
            xyDiagram1.AxisX.NumericOptions.Precision = 0;
            xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisX.Range.SideMarginsEnabled = true;
            xyDiagram1.AxisX.Title.Text = "Ciclos";
            xyDiagram1.AxisX.Title.Visible = true;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.NumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            xyDiagram1.AxisY.NumericOptions.Precision = 1;
            xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
            xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
            strip1.MaxLimit.AxisValueSerializable = "1";
            strip1.MinLimit.AxisValueSerializable = "0";
            strip1.Name = "Strip 1";
            strip1.Visible = false;
            xyDiagram1.AxisY.Strips.AddRange(new DevExpress.XtraCharts.Strip[] {
            strip1});
            xyDiagram1.AxisY.Title.Text = "Pontos";
            xyDiagram1.AxisY.Title.Visible = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            xyDiagram1.DefaultPane.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Gradient;
            rectangleGradientFillOptions1.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            xyDiagram1.DefaultPane.FillStyle.Options = rectangleGradientFillOptions1;
            xyDiagram1.EnableAxisXScrolling = true;
            xyDiagram1.EnableAxisXZooming = true;
            grafico.Diagram = xyDiagram1;
            grafico.Dock = System.Windows.Forms.DockStyle.Fill;
            grafico.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            grafico.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            grafico.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            grafico.Legend.EquallySpacedItems = false;
            grafico.Location = new System.Drawing.Point(0, 0);
            grafico.Name = "grafico";
            grafico.PaletteName = "Concourse";
            series1.ArgumentDataMember = "Ciclo";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel1.Angle = 60;
            pointSeriesLabel1.Antialiasing = true;
            pointSeriesLabel1.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            pointSeriesLabel1.Border.Visible = false;
            pointSeriesLabel1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            pointSeriesLabel1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            pointSeriesLabel1.LineVisible = false;
            pointOptions1.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions1.ArgumentNumericOptions.Precision = 1;
            pointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions1.ValueNumericOptions.Precision = 1;
            pointSeriesLabel1.PointOptions = pointOptions1;
            pointSeriesLabel1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series1.Label = pointSeriesLabel1;
            series1.Name = "Realizado";
            series1.ValueDataMembersSerializable = "Realizado";
            lineSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            lineSeriesView1.LineMarkerOptions.BorderVisible = false;
            lineSeriesView1.LineMarkerOptions.Size = 7;
            series1.View = lineSeriesView1;
            series2.ArgumentDataMember = "Ciclo";
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel2.Angle = 60;
            pointSeriesLabel2.Antialiasing = true;
            pointSeriesLabel2.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            pointSeriesLabel2.Border.Visible = false;
            pointSeriesLabel2.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            pointSeriesLabel2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            pointSeriesLabel2.LineVisible = false;
            pointOptions2.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions2.ArgumentNumericOptions.Precision = 1;
            pointOptions2.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions2.ValueNumericOptions.Precision = 1;
            pointSeriesLabel2.PointOptions = pointOptions2;
            pointSeriesLabel2.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series2.Label = pointSeriesLabel2;
            series2.Name = "Estimado";
            series2.ValueDataMembersSerializable = "Estimado";
            lineSeriesView2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            lineSeriesView2.LineMarkerOptions.BorderVisible = false;
            lineSeriesView2.LineMarkerOptions.Size = 7;
            series2.View = lineSeriesView2;
            series3.ArgumentDataMember = "Ciclo";
            series3.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel3.Angle = 60;
            pointSeriesLabel3.Antialiasing = true;
            pointSeriesLabel3.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            pointSeriesLabel3.Border.Visible = false;
            pointSeriesLabel3.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            pointSeriesLabel3.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            pointSeriesLabel3.LineVisible = false;
            pointOptions3.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions3.ArgumentNumericOptions.Precision = 1;
            pointOptions3.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions3.ValueNumericOptions.Precision = 1;
            pointSeriesLabel3.PointOptions = pointOptions3;
            pointSeriesLabel3.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series3.Label = pointSeriesLabel3;
            series3.Name = "Tendência";
            series3.ValueDataMembersSerializable = "Tendencia";
            lineSeriesView3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            lineSeriesView3.LineMarkerOptions.BorderVisible = false;
            lineSeriesView3.LineMarkerOptions.Size = 7;
            lineSeriesView3.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot;
            series3.View = lineSeriesView3;
            series4.ArgumentDataMember = "Ciclo";
            series4.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel4.Angle = 70;
            pointSeriesLabel4.Antialiasing = true;
            pointSeriesLabel4.BackColor = System.Drawing.Color.Transparent;
            pointSeriesLabel4.Border.Visible = false;
            pointSeriesLabel4.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            pointSeriesLabel4.LineVisible = false;
            pointOptions4.Pattern = "{S} : {V}";
            pointSeriesLabel4.PointOptions = pointOptions4;
            pointSeriesLabel4.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series4.Label = pointSeriesLabel4;
            series4.Name = "Ritmo Sugerido";
            series4.ValueDataMembersSerializable = "RitimoSugerido";
            pointSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            pointSeriesView1.PointMarkerOptions.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Solid;
            pointSeriesView1.PointMarkerOptions.Size = 6;
            series4.View = pointSeriesView1;
            grafico.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3,
        series4};
            pointSeriesLabel5.LineVisible = true;
            grafico.SeriesTemplate.Label = pointSeriesLabel5;
            grafico.SeriesTemplate.View = lineSeriesView4;
            grafico.Size = new System.Drawing.Size(660, 529);
            grafico.TabIndex = 0;
            // 
            // graficoEstimadoRealizadoDtoBindingSource
            // 
            graficoEstimadoRealizadoDtoBindingSource.DataSource = typeof(WexProject.BLL.Shared.DTOs.Escopo.GraficoEstimadoRealizadoDTO);
            // 
            // ChartEstimadoRealizado
            // 
            this.Controls.Add(grafico);
            this.Name = "ChartEstimadoRealizado";
            this.Size = new System.Drawing.Size(660, 529);
            ((System.ComponentModel.ISupportInitialize)(strip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(grafico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(graficoEstimadoRealizadoDtoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource graficoEstimadoRealizadoDtoBindingSource;
        private static DevExpress.XtraCharts.ChartControl grafico;

    }
}
