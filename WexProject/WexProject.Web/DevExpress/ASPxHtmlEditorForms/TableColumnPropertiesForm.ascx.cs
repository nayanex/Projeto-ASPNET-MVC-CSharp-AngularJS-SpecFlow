using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxHtmlEditor.Localization;

/// <summary>
/// classe TableColumnPropertiesForm
/// </summary>
public partial class TableColumnPropertiesForm : HtmlEditorUserControl
{
    /// <summary>
    /// método PrepareChildControls
    /// </summary>
    protected override void PrepareChildControls()
    {
        Localize();

        base.PrepareChildControls();
    }
    /// <summary>
    /// Localize()
    /// </summary>
    public void Localize()
    {
        btnChange.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonOk);
        btnCancel.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonCancel);

        lblSize.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ChangeTableColumn_Size) + ":";
        lblWidth.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Width) + ":";
        lblHeight.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Height) + ":";
        cmbWidth.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_FullWidth);
        cmbWidth.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_AutoFitToContent);
        cmbWidth.Items[2].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Custom);

        cmbHeight.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_AutoFitToContent);
        cmbHeight.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Custom);

        lblAlign.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment) + ":";
        lblHorizontalAlign.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_HorzAlignment) + ":";
        lblVerticalAlign.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_VertAlignment) + ":";
        cmbAlign.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_None);
        cmbAlign.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment_Left);
        cmbAlign.Items[2].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment_Center);
        cmbAlign.Items[3].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment_Right);

        cmbVAlign.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_None);
        cmbVAlign.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_VAlignment_Top);
        cmbVAlign.Items[2].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_VAlignment_Middle);
        cmbVAlign.Items[3].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_VAlignment_Bottom);

        lblAppearance.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Appearance) + ":";
        lblBackgroundColor.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_BgColor) + ":";

        ckbApplyForAllElements.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_ApplyToAllCell);
        // Text="Apply to all cells in the table"
    }
    /// <summary>
    /// GetChildDxEdits
    /// </summary>
    /// <returns>ASPxEditBase</returns>
    protected override ASPxEditBase[] GetChildDxEdits()
    {
        return new ASPxEditBase[] { lblAlign, lblHorizontalAlign, lblSize, lblWidth, cmbWidth, txbWidth,
        lblHeight, cmbHeight, txbHeight, cmbAlign, lblAppearance, txbBackgroundColor, lblVerticalAlign, cmbVAlign,
        ckbApplyForAllElements, cmbWidthType, cmbHeightType };
    }
    /// <summary>
    /// override ASPxButton[] GetChildDxButtons() 
    /// </summary>
    /// <returns>btnCancel, btnChange s</returns>
    protected override ASPxButton[] GetChildDxButtons()
    {
        return new ASPxButton[] { btnCancel, btnChange };
    }
    /// <summary>
    /// método GetChildDxHtmlEditorRoundPanels
    /// </summary>
    /// <returns>ASPxHtmlEditorRoundPanel</returns>
    protected override ASPxHtmlEditorRoundPanel[] GetChildDxHtmlEditorRoundPanels()
    {
        return new ASPxHtmlEditorRoundPanel[] { rpSize, rpLayout, rpAppearance };
    }
}
