using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Utils;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Qualidade;
using DevExpress.Xpo.DB;
using DevExpress.XtraCharts;
using DevExpress.ExpressApp.Editors;
using System.Collections.Generic;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Models.Geral;
using WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral;

namespace WexProject.Module.Win.Projeto
{
    /// <summary>
    /// Criação da classe
    /// </summary>
    public partial class SingleChoiceProjeto : ViewController
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public SingleChoiceProjeto()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        /// <summary>
        /// Faz o filtro do projeto mediante as opções de usuário com diversas possibilidades de filtro adicionadas a função
        /// </summary>
        private void FiltrarPorProjeto()
        {
            if (View.Id == "Requisito_ListView" || View.Id == "Requisito_DetailView" || View.Id == "Requisito_LookupListView"
            || View.Id == "CasoTeste_ListView" || View.Id == "CasoTeste_DetailView" || View.Id == "CasoTeste_LookupListView"
            || View.Id == "Estoria_DetailView" || View.Id == "Estoria_ListView" || View.Id == "Estoria_LookupListView"
            || View.Id == "Modulo_DetailView" || View.Id == "Modulo_ListView" || View.Id == "Modulo_LookupListView"
            || View.Id == "ProjetoParteInteressada_ListView" || View.Id == "ProjetoParteInteressada_LookupListView"
            || View.Id == "Prioridade_ListView"
            || View.Id == "Dashboard_SituacaoGeral"
            || View.Id == "CicloDesenv_ListView" || View.Id == "CicloDesenv_DetailView")
            {
                this.Projetos.Items.Clear();

                XPCollection<WexProject.BLL.Models.Geral.Projeto> xpCollectionProjeto = new XPCollection<WexProject.BLL.Models.Geral.Projeto>
                (((ObjectSpace)View.ObjectSpace).Session);
                xpCollectionProjeto.Sorting.Add(new SortProperty("TxNome", SortingDirection.Ascending));

                //recupera colaborador atual.
                Colaborador colaborador = Colaborador.GetColaboradorCurrent( ( (ObjectSpace)View.ObjectSpace ).Session );

                Guid oidProjeto = Services.Geral.GeralService.ConsultarUltimoProjetoSelecionado( colaborador.Oid );

                //compõe a lista de projetos existentes.
                foreach(WexProject.BLL.Models.Geral.Projeto projeto in xpCollectionProjeto)
                {
                    ChoiceActionItem item = new ChoiceActionItem( CaptionHelper.GetMemberCaption( typeof( WexProject.BLL.Models.Geral.Projeto ), projeto.TxNome ), null );
                    item.Data = projeto;
                    Projetos.Items.Add(item);
                    if (WexProject.BLL.Models.Geral.Projeto.SelectedProject != null && projeto.Oid == WexProject.BLL.Models.Geral.Projeto.SelectedProject)
                    {
                        Projetos.SelectedItem = item;
                    }
                    //se o oid do projeto for igual ao oid do último projeto selecionado, ele é setado automaticamente.
                    if (projeto.Oid == oidProjeto)
                    {
                        Projetos.SelectedItem = item;
                        WexProject.BLL.Models.Geral.Projeto.SelectedProject = projeto.Oid;
                    }
                }

                RnListView();
            }
        }
        /// <summary>
        /// Método de ativação (view, criação e etc)
        /// </summary>
        protected override void OnViewControlsCreated()
        {
            switch (View.Id)
            {
                case "Requisito_ListView":
                    FiltrarPorProjeto();
                    break;
                case "Requisito_DetailView":
                    FiltrarPorProjeto();
                    break;
                case "Requisito_LookupListView":
                    FiltrarPorProjeto();
                    break;
                case "CasoTeste_ListView":
                    FiltrarPorProjeto();
                    break;
                case "CasoTeste_DetailView":
                    FiltrarPorProjeto();
                    break;
                case "CasoTeste_LookupListView":
                    FiltrarPorProjeto();
                    break;
                case "Estoria_ListView":
                    FiltrarPorProjeto();
                    break;
                case "Estoria_DetailView":
                    FiltrarPorProjeto();
                    break;
                case "Estoria_LookupListView":
                    FiltrarPorProjeto();
                    break;
                case "Modulo_ListView":
                    FiltrarPorProjeto();
                    break;
                case "Modulo_DetailView":
                    FiltrarPorProjeto();
                    break;
                case "Modulo_LookupListView":
                    FiltrarPorProjeto();
                    break;
                case "Dashboard_SituacaoGeral":
                    FiltrarPorProjeto();
                    break;
                case "Prioridade_ListView":
                    FiltrarPorProjeto();
                    break;
                case "CicloDesenv_ListView":
                    FiltrarPorProjeto();
                    break;
                case "CicloDesenv_DetailView":
                    FiltrarPorProjeto();
                    break;
                case "CicloDesenv_LookupListView":
                    FiltrarPorProjeto();
                    break;
                case "ProjetoParteInteressada_ListView":
                    FiltrarPorProjeto();
                    break;
                case "ProjetoParteInteressada_DetailView":
                    FiltrarPorProjeto();
                    break;
                case "ProjetoParteInteressada_LookupListView":
                    FiltrarPorProjeto();
                    break;
            }

            base.OnViewControlsCreated();
        }


        /// <summary>
        /// Action para execução de itens (ação simples)
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SingleChoiceActionExecuteEventArgs</param>
        private void SingleChoiceAction1_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            WexProject.BLL.Models.Geral.Projeto projeto = (WexProject.BLL.Models.Geral.Projeto)e.SelectedChoiceActionItem.Data;
            WexProject.BLL.Models.Geral.Projeto.SelectedProject = projeto.Oid;

            //recupera colaborador atual.
            Colaborador colaborador = Colaborador.GetColaboradorCurrent( ( (ObjectSpace)View.ObjectSpace ).Session );

            Services.Geral.GeralService.SalvarUltimoProjetoSelecionado( colaborador.Oid, projeto.Oid );

            if(( View.Id == "Estoria_DetailView" ) && ( WexProject.BLL.Models.Geral.Projeto.SelectedProject != new Guid() ))
            {
                Estoria estoria = (Estoria)View.CurrentObject;
                estoria.RnSelecionarProjeto(projeto);
            }
            else
                if(( View.Id == "CasoTeste_DetailView" ) && ( WexProject.BLL.Models.Geral.Projeto.SelectedProject != new Guid() ))
                {
                    CasoTeste casoteste = (CasoTeste)View.CurrentObject;
                    casoteste.RnSelecionarProjeto(projeto);
                }
                else
                    if(( View.Id == "ProjetoParteInteressada_DetailView" ) && ( WexProject.BLL.Models.Geral.Projeto.SelectedProject != new Guid() ))
                    {
                        ProjetoParteInteressada projetoParteInteressada = (ProjetoParteInteressada)View.CurrentObject;
                        projetoParteInteressada.RnSelecionarProjeto(projeto);
                    }
            RnListView();
            View.Refresh();
        }
        /// <summary>
        /// Método da ListView que a cria e verifica critérios e confições pré-determinadas
        /// </summary>
        private void RnListView()
        {
            if (View is ListView)
            {
                ListView listView = (ListView)View;

                if (View.Id.Equals("Modulo_ListView") || View.Id.Equals("Modulo_LookupListView") || View.Id.Equals("DetailView"))
                {
                    listView.CollectionSource.Criteria["listView1"] = CriteriaOperator.Parse("Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                else if (View.Id.Equals("Estoria_ListView") || View.Id.Equals("Estoria_LookupListView") || View.Id.Equals("Estoria_DetailView") ||
                    View.Id.Equals("Requisito_ListView") || View.Id.Equals("Requisito_LookupListView") || View.Id.Equals("Requisito_DetailView"))
                {
                        listView.CollectionSource.Criteria["listView2"] = CriteriaOperator.Parse("Modulo.Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                     else if (View.Id.Equals("EstoriaCasoTeste_ListView") || View.Id.Equals("EstoriaCasoTeste_LookupListView") || View.Id.Equals("EstoriaCasoTeste_DetailView"))
                {
                            listView.CollectionSource.Criteria["listView3"] = CriteriaOperator.Parse("Estoria.Modulo.Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                          else if (View.Id.Equals("CasoTeste_ListView") || View.Id.Equals("CasoTeste_LookupListView") || View.Id.Equals("CasoTeste_DetailView"))
                {
                                listView.CollectionSource.Criteria["listView4"] = CriteriaOperator.Parse("Requisito.Modulo.Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                               else if (View.Id.Equals("Prioridade_ListView"))
                {
                                    listView.CollectionSource.Criteria["listView5"] = CriteriaOperator.Parse("Modulo.Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                                    else if (View.Id.Equals("Dashboard_SituacaoGeral"))
                {
                                        listView.CollectionSource.Criteria["listView7"] = CriteriaOperator.Parse("Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                                         else if (View.Id.Equals("ProjetoParteInteressada_LookupListView") || View.Id.Equals("ProjetoParteInteressada_DetailView") || View.Id.Equals("ProjetoParteInteressada_ListView"))
                {
                                            listView.CollectionSource.Criteria["listView8"] = CriteriaOperator.Parse("Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                                              else if (View.Id.Equals("Dashboard_Item_Escopo_Completude"))
                {
                                                listView.CollectionSource.Criteria["listView9"] = CriteriaOperator.Parse("Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                                                   else if (View.Id.Equals("CicloDesenv_ListView") || View.Id.Equals("CicloDesenv_DetailView"))
                {
                                                    listView.CollectionSource.Criteria["listView10"] = CriteriaOperator.Parse("Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
                                                        else if (View.Id.Equals("Dashboard_Item_Time_Ritmo") || View.Id.Equals("Dashboard_Item_Situacao_Ciclo"))
                {
                                                            listView.CollectionSource.Criteria["listView11"] = CriteriaOperator.Parse("Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                }
            }
            else if (View is DashboardView)
            {
                if (View.Id.Equals("Dashboard_SituacaoGeral"))
                {
                    foreach (DashboardViewItem item in (View as DashboardView).Items)
                    {
                        if (item.Id == "Dashboard_Escopo_Completude")
                        {
                            (item.InnerView as ListView).CollectionSource.Criteria["listView9"] =
                                CriteriaOperator.Parse("Projeto.Oid = ?", WexProject.BLL.Models.Geral.Projeto.SelectedProject);
                        }
                        else if (item.Id == "Dashboard_Time_Ritmo" || item.Id == "Dashboard_Situacao_Ciclo")
                        {
                            MontaGraficos();
                        }
                    }
                }
            }
        }

        static void MontaGraficos()
        {
            ChartEstimadoRealizado.MontaSeries();
            ChartRitimoTime.MontaSeries();
            ChartEscopoCompletude.MontaSeries();
        }

        public void SingleChoiceProjeto_ControlsCreated(object sender, EventArgs e)
        {
            (((ListView)View).Control as ChartControl).BoundDataChanged += new BoundDataChangedEventHandler(BoundDataChanged);
        }

        public void BoundDataChanged(object sender, EventArgs e)
        {
            if (WexProject.BLL.Models.Geral.Projeto.SelectedProject != null)
            {
                XYDiagram diagram = (((ListView)View).Control as ChartControl).Diagram as XYDiagram;
                int ultimo = (((ListView)View).CollectionSource.Collection as ProxyCollection).Count;

                diagram.AxisX.Range.MaxValue = diagram.AxisX.Range.MaxValueInternal = ultimo;
                diagram.AxisX.Range.MinValue = diagram.AxisX.Range.MinValueInternal = 0;

            }
        }
    }
}