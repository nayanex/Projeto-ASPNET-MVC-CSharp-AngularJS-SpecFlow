<%@ Control Language="C#" AutoEventWireup="true" Inherits="PasteFromWordForm" Codebehind="PasteFromWordForm.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dxhe" %>
<%@ Register Assembly="DevExpress.Web.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>

<%-- B141214 --%>
<dxe:ASPxLabel ID="lblB141214" runat="server" Style="display: none;" />

<dxp:ASPxPanel ID="MainPanel" runat="server" Width="100%" DefaultButton="btnOk" RenderMode="Table">
    <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
            <table cellpadding="0" cellspacing="0" id="dxPasteFromWordForm">
                <tr>
                    <td class="contentCell">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <dxe:ASPxLabel ID="lblDescription" runat="server">
                                    </dxe:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="pasteContainerCell">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <iframe id='<% =HtmlEditor.ClientID + "_dxePasteFromWordContainer"%>' class="pasteContainer" title='<% =DevExpress.Web.ASPxHtmlEditor.Localization.ASPxHtmlEditorLocalizer.GetString(DevExpress.Web.ASPxHtmlEditor.Localization.ASPxHtmlEditorStringId.PasteRtf_Instructions)%>'>
                                                </iframe>                                            
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="checkBoxCell">
                                    <dxe:ASPxCheckBox ID="ckbStripFont" runat="server" ClientInstanceName="_dxeCkbStripFont">
                                    </dxe:ASPxCheckBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="buttonsCell">
                        <table cellpadding="0" cellspacing="0" style="display:inline-block;">
                            <tr>
                                <td>
                                    <dxe:ASPxButton ID="btnOk" runat="server" AutoPostBack="False" Height="23px"
                                        Width="74px" ClientInstanceName="_dxeBtnOk" CausesValidation="False" >
                                        <clientsideevents click="function(s, e) {OnOkButtonClick_PasteFromWordForm();}" />
                                    </dxe:ASPxButton>
                                </td>
                                <td class="cancelButton">
                                    <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False"
                                        Height="23px" Width="74px" CausesValidation="False" >
                                        <clientsideevents click="function(s, e) {OnCancelButtonClick_PasteFromWordForm();}" />
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>        
        </dxp:PanelContent>
    </PanelCollection>
</dxp:ASPxPanel>
<script type="text/javascript" id="dxss_PasteFromWordForm">
    function OnOkButtonClick_PasteFromWordForm() {
        var res = {
            html: ASPxIFrame.GetDocumentBody('<% =HtmlEditor.ClientID + "_dxePasteFromWordContainer"%>').innerHTML,
            stripFontFamily: _dxeCkbStripFont.GetChecked()
        };
        aspxDialogComplete(1, res);
    }
    function OnCancelButtonClick_PasteFromWordForm() {
        aspxDialogComplete(0, null);
    }
</script>