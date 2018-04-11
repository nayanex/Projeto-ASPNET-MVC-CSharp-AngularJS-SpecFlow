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
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

/// <summary>
/// classe InsertLinkForm
/// </summary>
public partial class InsertLinkForm : HtmlEditorUserControl
{
    /// <summary>
    /// mátodo PrepareChildControls
    /// </summary>
    protected override void PrepareChildControls()
    {
        Localize();

        base.PrepareChildControls();

        bool documentSelectorEnabled = HtmlEditor.SettingsDocumentSelector.Enabled;
        txbURL.Buttons[0].Visible = documentSelectorEnabled;
        BrowsePopup.Visible = documentSelectorEnabled;
    }
    /// <summary>
    /// método Localize
    /// </summary>
    public void Localize()
    {
        rblLinkType.Items.Add(ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_Url), "URL");
        rblLinkType.Items.Add(ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_Email), "Email");
        lblUrl.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_Url) + ":";
        txbURL.ValidationSettings.RequiredField.ErrorText = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.RequiredFieldError);
        lblEmailTo.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_EmailTo) + ":";
        txbEmailTo.ValidationSettings.RequiredField.ErrorText = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.RequiredFieldError);
        lblSubject.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_Subject) + ":";
        lblLinkDisplay.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_DisplayProperties);
        lblText.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_Text) + ":";
        lblToolTip.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_ToolTip) + ":";
        ckbOpenInNewWindow.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_OpenInNewWindow);
        btnOk.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonOk);
        btnChange.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonChange);
        btnCancel.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonCancel);
        BrowsePopup.HeaderText = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.SelectDocument);
        txbURL.Buttons[0].ToolTip = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertLink_SelectDocument);
    }
    /// <summary>
    /// método GetChildDxEdits
    /// </summary>
    /// <returns>ASPxEditBase</returns>
    protected override ASPxEditBase[] GetChildDxEdits()
    {
        return new ASPxEditBase[] {
        rblLinkType,
        lblUrl, txbURL,
        lblEmailTo, txbEmailTo,
        lblSubject, txbSubject,
        lblLinkDisplay, lblText,
        txbText, lblToolTip,
        txbToolTip, ckbOpenInNewWindow
        };
    }
    /// <summary>
    /// GetChildDxButtons
    /// </summary>
    /// <returns>ASPxButton</returns>
    protected override ASPxButton[] GetChildDxButtons()
    {
        return new ASPxButton[] { btnOk, btnCancel, btnChange };
    }
    /// <summary>
    /// método GetChildDxHtmlEditorRoundPanel
    /// </summary>
    /// <returns>rpInsertLink</returns>
    protected override ASPxHtmlEditorRoundPanel GetChildDxHtmlEditorRoundPanel()
    {
        return rpInsertLink;
    }
}
