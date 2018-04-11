using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxHtmlEditor.Localization;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses.Internal;

/// <summary>
/// classe PasteFromWordForm
/// </summary>
public partial class PasteFromWordForm : HtmlEditorUserControl
{
    /// <summary>
    /// método PrepareChildControls
    /// </summary>
    protected override void PrepareChildControls()
    {
        Localize();
        lblB141214.Visible = RenderUtils.Browser.IsChrome; // B141214

        base.PrepareChildControls();
    }
    /// <summary>
    /// método Localize 
    /// </summary>
    public void Localize()
    {
        lblDescription.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.PasteRtf_Instructions);
        ckbStripFont.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.PasteRtf_StripFont);
        btnOk.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonOk);
        btnCancel.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonCancel);
    }
    /// <summary>
    /// método GetChildDxEdits
    /// </summary>
    /// <returns>ASPxEditBase[]</returns>
    protected override ASPxEditBase[] GetChildDxEdits()
    {
        return new ASPxEditBase[] {
        lblDescription, ckbStripFont
        };
    }
    /// <summary>
    /// método GetChildDxButtons
    /// </summary>
    /// <returns>ASPxButton</returns>
    protected override ASPxButton[] GetChildDxButtons()
    {
        return new ASPxButton[] { btnOk, btnCancel };
    }
}
