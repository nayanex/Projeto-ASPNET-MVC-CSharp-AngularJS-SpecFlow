using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Data.Filtering;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Shared.Domains.Rh;

namespace WexProject.Module.Lib.ControllerPraWinWeb
{
    /// <summary>
    /// Filtros de Planejamento de Férias
    /// </summary>
    public partial class ControleFeriasViewController : ViewController
    {
        /// <summary>
        /// Objeto de Colaborador
        /// </summary>
        private Colaborador colaborador = null;

        /// <summary>
        /// Inicializador de classe
        /// </summary>
        public ControleFeriasViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        /// <summary>
        /// Ao controller ser ativado
        /// </summary>
        protected override void OnActivated()
        {
            base.OnActivated();

            colaborador = Colaborador.GetColaboradorCurrent(((ObjectSpace)View.ObjectSpace).Session);

            if (colaborador.ColaboradorUltimoFiltro.LastPeriodoFilterPlanejamentoFerias >= 0)
            {
                SingleChoiceFilterPeriodo.SelectedIndex =
                    colaborador.ColaboradorUltimoFiltro.LastPeriodoFilterPlanejamentoFerias;

                PeriodoSelecionado();
            }

            if (colaborador.ColaboradorUltimoFiltro.LastSituacaoFilterPlanejamentoFerias >= 0)
            {
                SingleChoiceFilterSituacao.SelectedIndex =
                    colaborador.ColaboradorUltimoFiltro.LastSituacaoFilterPlanejamentoFerias;

                SituacaoSelecionada();
            }
        }

        /// <summary>
        /// Filtro de Período
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void SingleChoiceFilterPeriodo_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            PeriodoSelecionado();
        }

        /// <summary>
        /// Filtro de Situação
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void SingleChoiceFilterSituacao_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            SituacaoSelecionada();
        }

        /// <summary>
        /// Seleção de Projeto
        /// </summary>
        private void PeriodoSelecionado()
        {
            ListView listView = (ListView)View;
            CriteriaOperator criteria = null;

            if (SingleChoiceFilterPeriodo.SelectedItem.Data != null ||
                !SingleChoiceFilterPeriodo.SelectedItem.Caption.Equals("Todos"))
            {
                DateTime date01 = DateTime.MinValue, date02 = DateTime.MinValue;

                if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("u15"))
                {
                    date01 = DateTime.Today.Date.AddDays(-15);
                    date02 = DateTime.Today;
                }
                else if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("u30"))
                {
                    date01 = DateTime.Today.Date.AddDays(-30);
                    date02 = DateTime.Today;
                }
                else if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("u45"))
                {
                    date01 = DateTime.Today.Date.AddDays(-45);
                    date02 = DateTime.Today;
                }
                else if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("p15"))
                {
                    date01 = DateTime.Today;
                    date02 = DateTime.Today.Date.AddDays(15);
                }
                else if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("p30"))
                {
                    date01 = DateTime.Today;
                    date02 = DateTime.Today.Date.AddDays(30);
                }
                else if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("pm"))
                {
                    date01 = DateTime.Today.AddMonths(1);

                    if (date01.Day != 1)
                    {
                        date01 = date01.AddDays(-(date01.Day - 1));
                    }

                    date02 = date01.AddMonths(1).AddDays(-1);
                }
                else if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("am"))
                {
                    date01 = DateTime.Today.AddMonths(-1);

                    if (date01.Day != 1)
                    {
                        date01 = date01.AddDays(-(date01.Day - 1));
                    }

                    date02 = date01.AddMonths(1).AddDays(-1);
                }
                else if (SingleChoiceFilterPeriodo.SelectedItem.Data.Equals("ma"))
                {
                    date01 = DateTime.Today;

                    if (date01.Day != 1)
                    {
                        date01 = date01.AddDays(-(date01.Day - 1));
                    }

                    date02 = date01.AddMonths(1).AddDays(-1);
                }

                criteria = CriteriaOperator.Parse("(DtInicio >= ? AND _DtRetorno <= ?) OR (DtInicio <= ? AND _DtRetorno >= ? AND " +
                    "_DtRetorno <= ?) OR (DtInicio >= ? AND DtInicio <= ? AND _DtRetorno >= ?)", date01.Date, date02.Date, date01.Date,
                    date01.Date, date02.Date, date01.Date, date02.Date, date02.Date);
            }

            listView.CollectionSource.Criteria["periodo"] = criteria;
            Colaborador.RnSalvarPeriodoUltimoPlanejamentoFerias(colaborador.Session, colaborador, SingleChoiceFilterPeriodo.SelectedIndex);
        }

        /// <summary>
        /// Seleção de Situação
        /// </summary>
        private void SituacaoSelecionada()
        {
            ListView listView = (ListView)View;
            CriteriaOperator criteria = null;

            if (SingleChoiceFilterSituacao.SelectedItem.Data != null ||
                !SingleChoiceFilterSituacao.SelectedItem.Caption.Equals("Todos"))
            {
                CsSituacaoFerias result;

                if (SingleChoiceFilterSituacao.SelectedItem.Data.Equals("EmAtraso") || SingleChoiceFilterSituacao.SelectedItem.Data.Equals("Realizado") ||
                    SingleChoiceFilterSituacao.SelectedItem.Data.Equals("Planejado"))
                {
                    if (SingleChoiceFilterSituacao.SelectedItem.Data.Equals("EmAtraso"))
                    {
                        result = CsSituacaoFerias.EmAtraso;
                    }
                    else if (SingleChoiceFilterSituacao.SelectedItem.Data.Equals("Realizado"))
                    {
                        result = CsSituacaoFerias.Realizado;
                    }
                    else
                    {
                        result = CsSituacaoFerias.Planejado;
                    }


                    criteria = CriteriaOperator.Parse("CsSituacao = ?", result);
                }
                else
                {
                    criteria = CriteriaOperator.Parse("CsSituacao = ? || CsSituacao = ?", CsSituacaoFerias.EmAtraso, CsSituacaoFerias.Planejado);
                }
            }

            listView.CollectionSource.Criteria["situacao"] = criteria;
            Colaborador.RnSalvarPeriodoUltimaSituacaoFerias(colaborador.Session, colaborador, SingleChoiceFilterSituacao.SelectedIndex);
        }
    }
}