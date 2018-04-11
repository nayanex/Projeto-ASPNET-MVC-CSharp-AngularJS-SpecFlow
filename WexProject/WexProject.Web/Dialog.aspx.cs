using System;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Templates;

public partial class DialogPage : BaseXafPage, ILookupPopupFrameTemplate
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewSiteControl = viewSiteControl;
        WebApplication.Instance.CreateControls(this);
    }
    public DialogPage() { }
    #region ILookupPopupFrameTemplate Members

    public bool IsSearchEnabled
    {
        get { return SearchActionContainer.Visible; }
        set { SearchActionContainer.Visible = value; }
    }

    public void SetStartSearchString(string searchString) { }

    #endregion
}
