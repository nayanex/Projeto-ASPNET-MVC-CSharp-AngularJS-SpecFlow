<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectDocumentForm.ascx.cs" Inherits="SelectDocumentForm" %>
<%@ Register Assembly="DevExpress.Web.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxFileManager" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_SelectDocumentForm">
    function aspxHESelectDocumentForm_OnSelectedFileChanged(s, e) {
        dxheSelectDocumentForm_SelectButton.SetEnabled(!!e.file);
    }
    function aspxHEClosePopup() {
        BrowsePopup.Hide();
    }
    function aspxHESelectDocumentForm_DocumentSelected() {
        aspxHEClosePopup();
        var documentUrl = FileManager.cp_RootFolderRelativePath + FileManager.GetSelectedFile().GetFullName("\/", true);
        _dxeTxbURL.SetText(documentUrl);
        _dxeTxbURL.Validate();
    }
    function aspxHESelectDocumentForm_Canceled() {
        aspxHEClosePopup();
    }
</script>
<dx:ASPxPanel ID="MainPanel" runat="server" Width="100%" DefaultButton="SelectButton">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <table cellpadding="0" cellspacing="0" id="dxSelectDocumentForm">
                <tr>
                    <td>
                        <dx:HtmlEditorFileManager ID="FileManager" runat="server" Height="500px" Width="800px" ClientInstanceName="FileManager" OnCustomJSProperties="FileManager_CustomJSProperties" Border-BorderStyle="None">
                            <ClientSideEvents SelectedFileChanged="aspxHESelectDocumentForm_OnSelectedFileChanged" SelectedFileOpened="aspxHESelectDocumentForm_DocumentSelected" />
                            <Styles>
                                <FolderContainer Width="250px">
                                </FolderContainer>
                                <File>
                                    <Margins Margin="1px"></Margins>
                                </File>
                            </Styles>
                        </dx:HtmlEditorFileManager>
                    </td>
                </tr>
                <tr>
                    <td class="buttonsCell">
                        <table cellpadding="0" cellspacing="0" style="display:inline-block;">
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="SelectButton" runat="server" AutoPostBack="False" Height="23px"
                                        Width="74px" CausesValidation="False" ClientEnabled="false" ClientInstanceName="dxheSelectDocumentForm_SelectButton">
                                        <ClientSideEvents Click="aspxHESelectDocumentForm_DocumentSelected" />
                                    </dx:ASPxButton>
                                </td>
                                <td class="cancelButton">
                                    <dx:ASPxButton ID="CancelButton" runat="server" AutoPostBack="False" Height="23px"
                                        Width="74px" CausesValidation="False">
                                        <ClientSideEvents Click="aspxHESelectDocumentForm_Canceled" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>
