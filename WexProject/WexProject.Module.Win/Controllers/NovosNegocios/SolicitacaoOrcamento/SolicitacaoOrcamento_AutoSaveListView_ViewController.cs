using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;

namespace WexProject.Module.Win.Projeto.Controller_AutoSaveListView
{
    /// <summary>
    /// Classe para ListView poder executar auto save das informaçoes contidas no componente
    /// </summary>
    public partial class SolicitacaoOrcamento_AutoSaveListView_ViewController : ViewController
    {
        /// <summary>
        /// contrutor da classe
        /// </summary>
        public SolicitacaoOrcamento_AutoSaveListView_ViewController()
        {
            InitializeComponent();
            RegisterActions(components);
            TargetViewId = "SolicitacaoOrcamento_ListView";
        }

        /// <summary>
        /// Procedimento para ativação da ação de auto salvamento da listview
        /// </summary>
        protected override void OnActivated()
        {
            base.OnActivated();

            WinDetailViewController controller = Frame.GetController<WinDetailViewController>();

            if (controller != null)
                controller.AutoCommitListView = true;

        }
    }
}