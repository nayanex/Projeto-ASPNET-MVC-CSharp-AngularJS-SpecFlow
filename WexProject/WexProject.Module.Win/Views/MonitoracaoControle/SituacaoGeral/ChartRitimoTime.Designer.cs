namespace WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral
{
    partial class ChartRitimoTime
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
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel4 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView4 = new DevExpress.XtraCharts.LineSeriesView();
            graficoRitimo = new DevExpress.XtraCharts.ChartControl();
            this.graficoRitmoTimeDtoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(graficoRitimo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.graficoRitmoTimeDtoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // graficoRitimo
            // 
            graficoRitimo.DataSource = this.graficoRitmoTimeDtoBindingSource;
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
            xyDiagram1.AxisY.Title.Text = "Pontos";
            xyDiagram1.AxisY.Title.Visible = true;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            xyDiagram1.DefaultPane.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Gradient;
            rectangleGradientFillOptions1.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            xyDiagram1.DefaultPane.FillStyle.Options = rectangleGradientFillOptions1;
            graficoRitimo.Diagram = xyDiagram1;
            graficoRitimo.Dock = System.Windows.Forms.DockStyle.Fill;
            graficoRitimo.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            graficoRitimo.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            graficoRitimo.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            graficoRitimo.Legend.EquallySpacedItems = false;
            graficoRitimo.Location = new System.Drawing.Point(0, 0);
            graficoRitimo.Name = "graficoRitimo";
            graficoRitimo.PaletteName = "Civic";
            series1.ArgumentDataMember = "Ciclo";
            series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel1.Angle = 40;
            pointSeriesLabel1.Antialiasing = true;
            pointSeriesLabel1.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            pointSeriesLabel1.Border.Visible = false;
            pointSeriesLabel1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            pointSeriesLabel1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            pointSeriesLabel1.LineLength = 5;
            pointSeriesLabel1.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot;
            pointSeriesLabel1.LineVisible = false;
            pointOptions1.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions1.ArgumentNumericOptions.Precision = 1;
            pointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions1.ValueNumericOptions.Precision = 1;
            pointSeriesLabel1.PointOptions = pointOptions1;
            pointSeriesLabel1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series1.Label = pointSeriesLabel1;
            series1.Name = "Ritimo";
            series1.ValueDataMembersSerializable = "Ritmo";
            lineSeriesView1.Color = System.Drawing.Color.Blue;
            lineSeriesView1.LineMarkerOptions.BorderVisible = false;
            lineSeriesView1.LineMarkerOptions.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            lineSeriesView1.LineMarkerOptions.Size = 7;
            series1.View = lineSeriesView1;
            series2.ArgumentDataMember = "Ciclo";
            series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel2.Angle = 90;
            pointSeriesLabel2.Antialiasing = true;
            pointSeriesLabel2.Border.Visible = false;
            pointSeriesLabel2.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            pointSeriesLabel2.LineVisible = false;
            pointOptions2.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions2.ArgumentNumericOptions.Precision = 1;
            pointOptions2.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions2.ValueNumericOptions.Precision = 1;
            pointSeriesLabel2.PointOptions = pointOptions2;
            pointSeriesLabel2.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            pointSeriesLabel2.Visible = false;
            series2.Label = pointSeriesLabel2;
            series2.Name = "Meta";
            series2.ValueDataMembersSerializable = "Meta";
            lineSeriesView2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            lineSeriesView2.LineMarkerOptions.BorderVisible = false;
            lineSeriesView2.LineMarkerOptions.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            lineSeriesView2.LineMarkerOptions.Size = 5;
            lineSeriesView2.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot;
            series2.View = lineSeriesView2;
            series3.ArgumentDataMember = "Ciclo";
            series3.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel3.Angle = 150;
            pointSeriesLabel3.Antialiasing = true;
            pointSeriesLabel3.Border.Visible = false;
            pointSeriesLabel3.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Empty;
            pointSeriesLabel3.LineVisible = false;
            pointOptions3.ArgumentNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions3.ArgumentNumericOptions.Precision = 1;
            pointOptions3.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Number;
            pointOptions3.ValueNumericOptions.Precision = 1;
            pointSeriesLabel3.PointOptions = pointOptions3;
            pointSeriesLabel3.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            pointSeriesLabel3.Visible = false;
            series3.Label = pointSeriesLabel3;
            series3.Name = "Planejado";
            series3.ValueDataMembersSerializable = "Planejado";
            lineSeriesView3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            lineSeriesView3.LineMarkerOptions.BorderVisible = false;
            lineSeriesView3.LineMarkerOptions.Color = System.Drawing.Color.Gray;
            lineSeriesView3.LineMarkerOptions.Size = 4;
            lineSeriesView3.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dot;
            series3.View = lineSeriesView3;
            graficoRitimo.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3};
            graficoRitimo.SeriesTemplate.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            pointSeriesLabel4.LineVisible = true;
            graficoRitimo.SeriesTemplate.Label = pointSeriesLabel4;
            graficoRitimo.SeriesTemplate.View = lineSeriesView4;
            graficoRitimo.Size = new System.Drawing.Size(713, 491);
            graficoRitimo.TabIndex = 0;
            // 
            // graficoRitmoTimeDtoBindingSource
            // 
            this.graficoRitmoTimeDtoBindingSource.DataSource = typeof(WexProject.BLL.Shared.DTOs.Execucao.GraficoRitmoTimeDTO);
            // 
            // ChartRitimoTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(graficoRitimo);
            this.Name = "ChartRitimoTime";
            this.Size = new System.Drawing.Size(713, 491);
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
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(graficoRitimo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.graficoRitmoTimeDtoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource graficoRitmoTimeDtoBindingSource;
        private static DevExpress.XtraCharts.ChartControl graficoRitimo;



    }
}
