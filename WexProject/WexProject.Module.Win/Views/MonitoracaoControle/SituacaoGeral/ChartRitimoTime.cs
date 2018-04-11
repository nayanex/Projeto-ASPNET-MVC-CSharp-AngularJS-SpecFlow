using System;
using System.Collections.Generic;

namespace WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral
{
    public partial class ChartRitimoTime : DevExpress.XtraEditors.XtraUserControl
    {
        public ChartRitimoTime()
        {
            InitializeComponent();
        }

        public static void MontaSeries()
        {
            Guid projetoSelecionado = WexProject.BLL.Models.Geral.Projeto.SelectedProject;
            if (projetoSelecionado != null && !projetoSelecionado.Equals(new Guid()))
            {
                graficoRitimo.DataSource = Services.Execucao.ExecucaoService.GetDadosGraficoRitmoTimeProjeto( projetoSelecionado );
            }
        }
    }
}
