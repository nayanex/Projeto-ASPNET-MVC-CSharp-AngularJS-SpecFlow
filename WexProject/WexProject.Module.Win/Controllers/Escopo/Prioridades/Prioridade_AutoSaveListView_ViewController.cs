using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;

namespace WexProject.Module.Win.Projeto.Controller_AutoSaveListView
{
    /// <summary>
    /// Classe para ListView poder executar auto save das informaçoes contidas no componente
    /// </summary>
    public partial class Prioridade_AutoSaveListView_ViewController :  ViewController
    {

        /// <summary>
        /// contrutor da classe
        /// </summary>
        public Prioridade_AutoSaveListView_ViewController()
        {
            InitializeComponent();
            RegisterActions(components);
            TargetViewId = "Prioridade_ListView";

        }
        /// <summary>
        /// Procedimento para ativação da ação de auto salvamento da listview da necessidade prioridade.
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
