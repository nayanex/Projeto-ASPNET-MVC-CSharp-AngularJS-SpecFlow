using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using System.Collections.Generic;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Templates.ActionContainers;

/// <summary>
/// Nested Frame
/// </summary>
[ParentControlCssClass("NestedFrameControl")]
public partial class NestedFrameControl : System.Web.UI.UserControl, IFrameTemplate, ISupportActionsToolbarVisibility, IDynamicContainersTemplate, IViewHolder
{
    /// <summary>
    /// contexte de menu
    /// </summary>
    private ContextActionsMenu contextMenu;
    /// <summary>
    /// Action controller
    /// </summary>
    private ActionContainerCollection actionContainers = new ActionContainerCollection();
    /// <summary>
    /// view
    /// </summary>
    private View view;
    /// <summary>
    /// Quando a página pre renderizar
    /// </summary>
    /// <param name="sender">objeto sender</param>
    /// <param name="e">argumentos</param>
    private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
    {
        WebWindow.CurrentRequestWindow.PagePreRender -= new EventHandler(CurrentRequestWindow_PagePreRender);
        if (ToolBar != null)
        {
            ToolBar.Visible = actionsToolbarVisibility == ActionsToolbarVisibility.Hide ? false : true;
        }
    }
    /// <summary>
    /// Controlador do frame
    /// </summary>
    public NestedFrameControl()
    {
        contextMenu = new ContextActionsMenu(this, "Edit", "RecordEdit", "ListView");
        actionContainers.AddRange(contextMenu.Containers);
    }
    /// <summary>
    /// Quando o objeto carregar
    /// </summary>
    /// <param name="e">argumentos</param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (WebWindow.CurrentRequestWindow != null)
        {
            WebWindow.CurrentRequestWindow.PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);
        }
    }
    //B157146, B157117
    /// <summary>
    /// método Dispose
    /// </summary>
    public override void Dispose()
    {
        if (ToolBar != null)
        {
            ToolBar.Dispose();
            ToolBar = null;
        }
        if (contextMenu != null)
        {
            contextMenu.Dispose();
            contextMenu = null;
        }
        base.Dispose();
    }
    #region IFrameTemplate Members
    /// <summary>
    /// prpriedade DefaultContainer
    /// </summary>
    public IActionContainer DefaultContainer
    {
        get { return ToolBar.FindActionContainerById("View"); }
    }
    /// <summary>
    /// mátodo GetContainers
    /// </summary>
    /// <returns>result</returns>
    public ICollection<IActionContainer> GetContainers()
    {
        return actionContainers.ToArray();
    }
    /// <summary>
    /// mátodo SetView
    /// </summary>
    /// <param name="view">view</param>
    public void SetView(DevExpress.ExpressApp.View view)
    {
        this.view = view;
        if (view != null)
        {
            contextMenu.CreateControls(view);
        }

        OnViewChanged(view);
    }
    #endregion
    /// <summary>
    /// Quando a view modificar
    /// </summary>
    /// <param name="view">view</param>
    protected virtual void OnViewChanged(DevExpress.ExpressApp.View view)
    {
        if (ViewChanged != null)
        {
            ViewChanged(this, new TemplateViewChangedEventArgs(view));
        }
    }

    #region IActionBarVisibilityManager Members

    /// <summary>
    /// Visibilidade das ações
    /// </summary>
    private ActionsToolbarVisibility actionsToolbarVisibility = ActionsToolbarVisibility.Default;

    /// <summary>
    /// Visibilidade do toolbar
    /// </summary>
    public ActionsToolbarVisibility ActionsToolbarVisibility
    {
        get
        {
            return actionsToolbarVisibility;
        }
        set
        {
            actionsToolbarVisibility = value;
        }
    }
    #endregion
    #region IDynamicContainersTemplate Members

    /// <summary>
    /// Quando o container modificar
    /// </summary>
    /// <param name="args">argumentos</param>
    private void OnActionContainersChanged(ActionContainersChangedEventArgs args)
    {
        if (ActionContainersChanged != null)
        {
            ActionContainersChanged(this, args);
        }
    }

    /// <summary>
    /// Registros de ações do container
    /// </summary>
    /// <param name="actionContainers"></param>
    public void RegisterActionContainers(IEnumerable<IActionContainer> actionContainers)
    {
        IEnumerable<IActionContainer> addedContainers = this.actionContainers.TryAdd(actionContainers);
        if (DevExpress.ExpressApp.Utils.Enumerator.Count(addedContainers) > 0)
        {
            OnActionContainersChanged(new ActionContainersChangedEventArgs(addedContainers, ActionContainersChangedType.Added));
        }
    }

    /// <summary>
    /// Desregistrar ações de container
    /// </summary>
    /// <param name="actionContainers">ações do controlador</param>
    public void UnregisterActionContainers(IEnumerable<IActionContainer> actionContainers)
    {
        IList<IActionContainer> removedContainers = new List<IActionContainer>();
        foreach (IActionContainer actionContainer in actionContainers)
        {
            if (this.actionContainers.Contains(actionContainer))
            {
                this.actionContainers.Remove(actionContainer);
                removedContainers.Add(actionContainer);
            }
        }
        if (DevExpress.ExpressApp.Utils.Enumerator.Count(removedContainers) > 0)
        {
            OnActionContainersChanged(new ActionContainersChangedEventArgs(removedContainers, ActionContainersChangedType.Removed));
        }
    }

    /// <summary>
    /// quando o container de ação é modificado
    /// </summary>
    public event EventHandler<ActionContainersChangedEventArgs> ActionContainersChanged;
    #endregion

    /// <summary>
    /// View
    /// </summary>
    public DevExpress.ExpressApp.View View
    {
        get { return view; }
    }

    #region ISupportViewChanged Members

    /// <summary>
    /// Quando a view modificar
    /// </summary>
    public event EventHandler<TemplateViewChangedEventArgs> ViewChanged;

    #endregion
}
