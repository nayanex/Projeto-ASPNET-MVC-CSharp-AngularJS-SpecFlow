using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using WexProject.BLL.DAOs;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Module.Win.Projeto.Controller
{
    /// <summary>
    /// ControllerNavigationMeusDados
    /// </summary>
    public class ControllerNavigationMeusDados : ShowNavigationItemController
    {
        /// <summary>
        /// newController
        /// </summary>
        private NewObjectViewController newController;
        /// <summary>
        /// createdDetailView
        /// </summary>
        private DetailView createdDetailView;
        /// <summary>
        /// CreateNewObjectItemId
        /// </summary>
        private const string CreateNewObjectItemId = "MeusDados";
        /// <summary>
        /// ControllerNavigationMeusDados
        /// </summary>
        public ControllerNavigationMeusDados()
        {
            TargetWindowType = WindowType.Main;
        }
        /// <summary>
        /// ShowNavigationItem
        /// </summary>
        /// <param name="e">e</param>
        protected override void ShowNavigationItem(SingleChoiceActionExecuteEventArgs e)
        {
            if ((e.SelectedChoiceActionItem != null)
            && e.SelectedChoiceActionItem.Enabled.ResultValue
            && e.SelectedChoiceActionItem.Id == CreateNewObjectItemId)
            {

                Frame workFrame = Application.CreateFrame(TemplateContext.ApplicationWindow);
                //workFrame.SetView(Application.CreateListView(Application.CreateObjectSpace(), typeof(Colaborador), true));
                workFrame.SetView(Application.CreateDetailView(Application.CreateObjectSpace(), "Meus_Dados_DetailView", true));
                newController = workFrame.GetController<NewObjectViewController>();

                if (newController != null)
                {
                    ChoiceActionItem newObjectItem = FindNewObjectItem();
                    if (newObjectItem != null)
                    {
                        newController.NewObjectAction.Executed += NewObjectAction_Executed;
                        newController.NewObjectAction.DoExecute(newObjectItem);
                        newController.NewObjectAction.Executed -= NewObjectAction_Executed;
                        e.ShowViewParameters.TargetWindow = TargetWindow.Default;

                        ObjectSpace objSpaceDetailView = (ObjectSpace)createdDetailView.ObjectSpace;
                        Session sessao = objSpaceDetailView.Session;
                        User usuarioObjectSpace = UsuarioDAO.GetUsuarioLogado(sessao);
                        Colaborador colaboradorObjectSpace = sessao.FindObject<Colaborador>(CriteriaOperator.Parse("Usuario.Oid = ?", usuarioObjectSpace.Oid));
                        createdDetailView.CurrentObject = colaboradorObjectSpace;
                        bool teste = createdDetailView.CurrentObject.Equals(new Guid());
                        e.ShowViewParameters.CreatedView = createdDetailView;

                        //Cancel the default processing for this navigation item. 
                        return;
                    }
                }
            }
            else if (e.SelectedChoiceActionItem != null
                && (e.SelectedChoiceActionItem.Id == "Configuracao_ListView" || e.SelectedChoiceActionItem.Id == "Configuracao_DetailView"
                || e.SelectedChoiceActionItem.Id == "Configuração"))
            {
                ObjectSpace objectSpace = (ObjectSpace)Application.CreateObjectSpace();

                using (XPCollection itens = new XPCollection(objectSpace.Session, typeof(Configuracao)))
                {
                    if (itens.Count > 0)
                    {
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, itens[0]);
                    }
                    else
                    {
                        e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, new Configuracao(objectSpace.Session));
                    }
                }

                e.ShowViewParameters.TargetWindow = TargetWindow.Default;

                // Cancel the default processing for this navigation item.
                return;
            }
            else if (e.SelectedChoiceActionItem.Id == "PlanejamentoFerias_DetailView" || e.SelectedChoiceActionItem.Id == "PlanejamentoFerias_DetailView")
            {
                ObjectSpace obj = Application.CreateObjectSpace() as ObjectSpace;

                //e.SelectedChoiceActionItem.Data = null;

                e.ShowViewParameters.CreatedView = Application.CreateDetailView(obj, "PlanejamentoFerias_DetailView",true);
                
                e.SelectedChoiceActionItem.Id = e.ShowViewParameters.CreatedView.Id;

                e.ShowViewParameters.TargetWindow = TargetWindow.Default;

                return;

            }

            //Continue the default processing for other navigation items. 
            base.ShowNavigationItem(e);
        }
        /// <summary>
        /// FindNewObjectItem
        /// </summary>
        /// <returns>item</returns>
        private ChoiceActionItem FindNewObjectItem()
        {
            foreach (ChoiceActionItem item in newController.NewObjectAction.Items)
                if (item.Data == typeof(Colaborador))
                    return item;
            return null;
        }
        /// <summary>
        /// NewObjectAction_Executed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void NewObjectAction_Executed(object sender, ActionBaseEventArgs e)
        {
            createdDetailView = e.ShowViewParameters.CreatedView as DetailView;
            //Cancel showing the default View by the NewObjectAction 
            e.ShowViewParameters.CreatedView = null;
        }
    }
}