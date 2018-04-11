using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxHtmlEditor.Localization;

/// <summary>
/// InsertTableForm
/// </summary>
public partial class InsertTableForm : HtmlEditorUserControl
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
    /// método Localize
    /// </summary>
    public void Localize()
    {
        btnOk.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonOk);
        btnCancel.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.ButtonCancel);

        lblSize.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Size) + ":";
        lblColumns.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Columns) + ":";
        lblRows.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Rows) + ":";
        lblWidth.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Width) + ":";
        lblHeight.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Height) + ":";
        cmbWidth.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_FullWidth);
        cmbWidth.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_AutoFitToContent);
        cmbWidth.Items[2].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Custom);
        ckbColumnsEqualWidth.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_EqualColumnWidths);

        cmbHeight.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_AutoFitToContent);
        cmbHeight.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Custom);

        lblLayout.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Layout) + ":";
        lblPadding.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_CellPaddings) + ":";
        lblAlign.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment) + ":";
        cmbAlign.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_None);
        cmbAlign.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment_Left);
        cmbAlign.Items[2].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment_Center);
        cmbAlign.Items[3].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Alignment_Right);
        lblSpacing.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_CellSpacing) + ":";

        lblAppearance.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Appearance) + ":";
        lblBorderColor.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_BorderColor) + ":";
        lblBorderSize.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_BorderSize) + ":";
        lblBackgroundColor.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_BgColor) + ":";

        ckbAccessibility.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Accessibility);
        lblHeaders.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Headers) + ":";
        cmbHeaders.Items[0].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_None);
        cmbHeaders.Items[1].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_FirstRow);
        cmbHeaders.Items[2].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_FirstColumn);
        cmbHeaders.Items[3].Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Both);
        lblCaption.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Caption) + ":";
        lblSummary.Text = ASPxHtmlEditorLocalizer.GetString(ASPxHtmlEditorStringId.InsertTable_Summary) + ":";
    }
    /// <summary>
    /// método GetChildDxEdits
    /// </summary>
    /// <returns>ASPxEditBase</returns>
    protected override ASPxEditBase[] GetChildDxEdits()
    {
        return new ASPxEditBase[] { spnRows, spnColumns, lblLayout, lblSize, lblWidth, cmbWidth, txbWidth,
        lblHeight, cmbHeight, txbHeight, spnPadding, spnSpacing, cmbAlign, lblAppearance,
        lblBorderSize, lblBorderColor, spnBorderSize, txbBorderColor, txbBackgroundColor,
        ckbAccessibility, cmbHeaders, txbCaption, txbSummary, ckbColumnsEqualWidth, cmbWidthType, cmbHeightType };
    }
    /// <summary>
    /// método GetChildDxButtons
    /// </summary>
    /// <returns>ASPxButton</returns>
    protected override ASPxButton[] GetChildDxButtons()
    {
        return new ASPxButton[] { btnOk, btnCancel };
    }
    /// <summary>
    /// método GetChildDxHtmlEditorRoundPanels
    /// </summary>
    /// <returns>ASPxHtmlEditorRoundPanel</returns>
    protected override ASPxHtmlEditorRoundPanel[] GetChildDxHtmlEditorRoundPanels()
    {
        return new ASPxHtmlEditorRoundPanel[] { rpSize, rpLayout, rpAppearance, rpAccessibility };
    }
}
