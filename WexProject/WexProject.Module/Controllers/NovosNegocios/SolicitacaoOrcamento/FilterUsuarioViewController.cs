using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.NovosNegocios;
using System.Collections;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo.DB;
using DevExpress.ExpressApp.SystemModule;

namespace WexProject.Module.Lib.ControllerPraWinWeb.NovosNegocios
{
    /// <summary>
    /// Classe
    /// </summary>
    public partial class FilterUsuarioViewController : ViewController
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public FilterUsuarioViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        /// <summary>
        /// Ao ativar o componentes
        /// </summary>
        protected override void OnActivated()
        {
            PopulateActionUsuario();
            Frame.GetController<RefreshController>().RefreshAction.Execute += new SimpleActionExecuteEventHandler(RefreshAction_Execute);
            base.OnActivated();
        }

        void RefreshAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PopulateActionUsuario();
        }

        /// <summary>
        /// Popula os comboboxes de usuários e situações
        /// </summary>
        private void PopulateActionUsuario()
        {
            Session session = ((ObjectSpace)View.ObjectSpace).Session;

            this.singleChoiceActionUsuarios.Items.Clear();

            this.singleChoiceActionUsuarios.Items.Add(new ChoiceActionItem("Todos", 0));
            var selec = 0;
            var selecSituacao = 0;
            Colaborador colab = Colaborador.GetColaboradorCurrent(session);
            using (XPCollection<User> collection = new XPCollection<User>(((ObjectSpace)(View.ObjectSpace)).Session)) 
            {
                collection.Sorting.Add(new SortProperty("FullName", SortingDirection.Ascending));

                foreach (User usuario in collection)
                {
                    if (SolicitacaoOrcamento.RnSeotsPorResponsavel((((ObjectSpace)(View.ObjectSpace)).Session), usuario))
                    {
                        this.singleChoiceActionUsuarios.Items.Add(new ChoiceActionItem(usuario.FullName, usuario.Oid));
                        if (usuario.Oid.Equals(colab.ColaboradorUltimoFiltro.LastUsuarioFilterSeot))
                            selec = this.singleChoiceActionUsuarios.Items.Count-1;
                    }
                }
            }

            this.singleChoiceActionSituacao.Items.Clear();

            this.singleChoiceActionSituacao.Items.Add(new ChoiceActionItem("Todas", 0));

            using (XPCollection<ConfiguracaoDocumentoSituacao> collection = new XPCollection<ConfiguracaoDocumentoSituacao>(((ObjectSpace)(View.ObjectSpace)).Session)) 
            {
                collection.Sorting.Add(new SortProperty("TxDescricao", SortingDirection.Ascending));

                foreach (ConfiguracaoDocumentoSituacao situacao in collection)
                {
                    if (SolicitacaoOrcamento.RnSeotsPorSituacao((((ObjectSpace)(View.ObjectSpace)).Session), situacao))
                    {
                        this.singleChoiceActionSituacao.Items.Add(new ChoiceActionItem(situacao.TxDescricao, situacao.Oid));
                        if (situacao.Oid.Equals(colab.ColaboradorUltimoFiltro.LastSituacaoFilterSeot))
                            selecSituacao = this.singleChoiceActionSituacao.Items.Count - 1;
                    }
                }
            }

            CriteriaOperator criteriaResponsavel = null, criteriaSituacao = null;
            ListView listView = (ListView)View;
            Colaborador colaboradorResponsavel = Colaborador.GetColaboradorCurrent(session, colab.ColaboradorUltimoFiltro.LastUsuarioFilterSeot);
            Guid seotSituacao = colab.ColaboradorUltimoFiltro.LastSituacaoFilterSeot;
            
            singleChoiceActionUsuarios.SelectedIndex = selec;
            singleChoiceActionSituacao.SelectedIndex = selecSituacao;

            if (colaboradorResponsavel != null)
            {
                criteriaResponsavel = CriteriaOperator.Parse("Responsavel = ?", colaboradorResponsavel.Oid);
                listView.CollectionSource.Criteria["FiltroPorResponsavel"] = criteriaResponsavel;
            }

            if (seotSituacao != Guid.Empty)
            {
                criteriaSituacao = CriteriaOperator.Parse("Situacao =?", seotSituacao);
                listView.CollectionSource.Criteria["FiltroPorSituacao"] = criteriaSituacao;
            }
        }

        /// <summary>
        /// Executa a pesquisa baseado nos responsáveis
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos de pesquisa</param>
        private void SingleChoiceActionUsuarios_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ListView listView = (ListView)View;

            Session session = ((ObjectSpace)View.ObjectSpace).Session;

            CriteriaOperator criteria = null;

            if (!e.SelectedChoiceActionItem.Caption.Equals("Todos"))
            {
                Colaborador result = session.FindObject<Colaborador>(CriteriaOperator.Parse(String.Format("Usuario = '{0}'",
                    e.SelectedChoiceActionItem.Data)));

                if (result != null)
                {
                    Colaborador.RnSalvarUsuarioUltimaSEOT(session, result.Usuario.Oid, Colaborador.GetColaboradorCurrent(session));

                    criteria = CriteriaOperator.Parse("Responsavel = ?", result.Oid);
                }
            }
            else
            {
                Colaborador.RnSalvarUsuarioUltimaSEOT(session, Guid.Empty, Colaborador.GetColaboradorCurrent(session));
            }
                 
            listView.CollectionSource.Criteria["FiltroPorResponsavel"] = criteria;
        }

        /// <summary>
        /// Executa a pesquisa para situações
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos da ação</param>
        private void SingleChoiceActionSituacao_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ListView listView = (ListView)View;

            Session session = ((ObjectSpace)View.ObjectSpace).Session;

            CriteriaOperator criteria = null;

            if (!e.SelectedChoiceActionItem.Caption.Equals("Todas"))
            {
                ConfiguracaoDocumentoSituacao result = session.FindObject<ConfiguracaoDocumentoSituacao>(CriteriaOperator.Parse(
                    String.Format("Oid = '{0}'", e.SelectedChoiceActionItem.Data)));

                if (result != null)
                {
                    Colaborador.RnSalvarSituacaoUltimaSEOT(session, result.Oid, Colaborador.GetColaboradorCurrent(session));

                    criteria = CriteriaOperator.Parse("Situacao = ?", result.Oid);
                }
            }
            else
            {
                Colaborador.RnSalvarSituacaoUltimaSEOT(session, Guid.Empty, Colaborador.GetColaboradorCurrent(session));

                if (!singleChoiceActionUsuarios.SelectedItem.Caption.Equals("Todos"))
                {
                    Colaborador result = session.FindObject<Colaborador>(CriteriaOperator.Parse(String.Format("Usuario = '{0}'",
                        singleChoiceActionUsuarios.SelectedItem.Data)));

                    if (result != null)
                    {
                        criteria = CriteriaOperator.Parse("Responsavel = ?", result.Oid);
                    }
                }
            }

            listView.CollectionSource.Criteria["FiltroPorSituacao"] = criteria;
        }

        protected override void OnDeactivated()
        {
            Frame.GetController<RefreshController>().RefreshAction.Execute -= new SimpleActionExecuteEventHandler(RefreshAction_Execute);
            base.OnDeactivated();
    }
}
}
