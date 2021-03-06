﻿using System;
using System.Collections.Generic;

namespace WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral
{
    public partial class ChartEscopoCompletude : DevExpress.XtraEditors.XtraUserControl
    {
        public ChartEscopoCompletude()
        {
            InitializeComponent();
            ChartEscopoCompletude.MontaSeries();
        }

        public static void MontaSeries()
        {
            Guid projetoSelecionado = WexProject.BLL.Models.Geral.Projeto.SelectedProject;
            if (projetoSelecionado != null && !projetoSelecionado.Equals(new Guid()))
            {
                grafico.DataSource = Services.Escopo.EscopoService.GetDadosGraficoEscopoCompletude( projetoSelecionado );
            }
        }
    }
}
