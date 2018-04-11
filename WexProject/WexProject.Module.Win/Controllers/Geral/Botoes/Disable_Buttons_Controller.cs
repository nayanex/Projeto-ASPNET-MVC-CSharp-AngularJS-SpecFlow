using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Validation.AllContextsView;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.SystemModule;

namespace WexProject.Module.Win.Projeto.Controller_DisableButtons
{
    /// <summary>
    /// Classe para desabilitar botões
    /// </summary>
    public partial class Disable_Buttons_Controller : ViewController
    {
        /// <summary>
        /// construtor da  classe 
        /// </summary>
        public Disable_Buttons_Controller()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        /// <summary>
        /// Procedimento para quando for ativado a classe para desabilitar botões.
        /// </summary>
        protected override void OnActivated()
        {

            Frame.GetController<ShowAllContextsController>().Active.SetItemValue("Active", false);

            switch (View.Id)
            {
                case "CicloDesenv_DetailView":
                    Frame.GetController<DeleteObjectsViewController>().Active.SetItemValue("Active", false);
                    break;
                case "CicloDesenv_DesenvEstorias_ListView":
                    Frame.GetController<DeleteObjectsViewController>().Active.SetItemValue("Active", true);
                    break;
                case "ProjetoCliente_DetailView":
                    Frame.GetController<WinDetailViewController>().SaveAction.Active.SetItemValue("Active", false);
                    Frame.GetController<WinDetailViewController>().SaveAndCloseAction.Active.SetItemValue("Active", false);
                    Frame.GetController<WinDetailViewController>().SaveAndNewAction.Active.SetItemValue("Active", false);
                    Frame.GetController<NewObjectViewController>().Active.SetItemValue("Active", false);
                    break;
                case "ProjetoParteInteressada_DetailView":
                    Frame.GetController<WinDetailViewController>().SaveAction.Active.SetItemValue("Active", false);
                    Frame.GetController<WinDetailViewController>().SaveAndCloseAction.Active.SetItemValue("Active", false);
                    Frame.GetController<WinDetailViewController>().SaveAndNewAction.Active.SetItemValue("Active", false);
                    Frame.GetController<NewObjectViewController>().Active.SetItemValue("Active", false);
                    break;
            }
            base.OnActivated();
        }
        /// <summary>
        /// Função para desabilitar botões
        /// </summary>
        /// <param name="sender">Objeto de envio</param>
        /// <param name="e">Argumentos de evento</param>
        private void Disable_Buttons_Controller_Activated(object sender, EventArgs e)
        {

        }
    }
}
