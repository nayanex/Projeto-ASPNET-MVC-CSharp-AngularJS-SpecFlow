using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.SystemModule;

namespace WexProject.Module.Win.Projeto.Controllers.ControleAcesso
{
    /// <summary>
    /// Controller feito exclusivamente para MeusDados_DetailView
    /// </summary>
    public partial class MeusDadosViewController : ViewController
    {
        /// <summary>
        /// Construtor do Controller
        /// </summary>
        public MeusDadosViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        /// <summary>
        /// Override para Quando os controles da view forem criados
        /// desativar os controllers de novo e excluir
        /// </summary>
        protected override void OnViewControlsCreated()
        {
            //DeactivateControllers();
            base.OnViewControlsCreated();
        }
        /// <summary>
        /// Metodo feito para desativar os controllers de novo e excluir
        /// </summary>
        private void DeactivateControllers() 
        {
            
            Frame.GetController<NewObjectViewController>().Active.SetItemValue("Active", false);
            Frame.GetController<DeleteObjectsViewController>().Active.SetItemValue("Active", false);
            
        }

    }
}
