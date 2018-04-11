using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Xpo;
using WexProject.BLL.Models.Planejamento;
using WexProject.BLL.Models.Rh;
using WexProject.Module.Win.TelasForaPadrao.RH.TimeLineView;
using WexProject.Schedule.Library.Views.Forms;
using WexProject.Schedule.Library.Presenters;
using WexProject.Schedule.Library.ServiceUtils;

namespace WexProject.Module.Win.Projeto.Controller_Cronograma
{
    /// <summary>
    /// Controlador do Form do Cronograma
    /// </summary>
    public partial class Controller_AdicionCronogramaForm : ShowNavigationItemController
    {
        
        /// <summary>
        /// Construtor do Cronograma
        /// </summary>
        public Controller_AdicionCronogramaForm()
        {
            TargetWindowType = WindowType.Main;
        }
 
        /// <summary>
        /// Método chamado quando o usuário abre uma janela
        /// </summary>
        /// <param name="e">SingleChoiceActionExecuteEventArgs</param>
        protected override void ShowNavigationItem(SingleChoiceActionExecuteEventArgs e)
        {
            if ((e.SelectedChoiceActionItem != null)
                  && e.SelectedChoiceActionItem.Id == "Cronograma")
            {
                ObjectSpace objectSpace = (ObjectSpace)Application.CreateObjectSpace();

                CronogramaPresenter.ServicoPlanejamento = new PlanejamentoServiceUtil();
                CronogramaPresenter.ServicoGeral = new GeralServiceUtil();
                CronogramaView cronograma = new CronogramaView();

                if(!cronograma.IsDisposed)
                    cronograma.Show();

                return;
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
            else if (e.SelectedChoiceActionItem.Id != null && e.SelectedChoiceActionItem.Id == "PlanejamentoFerias_DetailView")
            {
                ObjectSpace obj = Application.CreateObjectSpace() as ObjectSpace;

                TimeLine line = new TimeLine(obj.Session);
                line.Show();

                return;

            }
            else if (e.SelectedChoiceActionItem.Id != null && e.SelectedChoiceActionItem.Id == "MeusDados_DetailView")
            {

                ObjectSpace obj = Application.CreateObjectSpace() as ObjectSpace;

                e.SelectedChoiceActionItem.Data = null;

                Colaborador colab = Colaborador.GetColaboradorCurrent(obj.Session, new Guid(SecuritySystem.CurrentUserId.ToString()));

                e.ShowViewParameters.CreatedView = Application.CreateDetailView(obj, "MeusDados_DetailView", true, colab);

                //e.SelectedChoiceActionItem.Id = "PlanejamentoFerias_DetailView";

                e.ShowViewParameters.TargetWindow = TargetWindow.Default;

                return;
            }



            //Continue the default processing for other navigation items.
            base.ShowNavigationItem(e);
        }
    }
}