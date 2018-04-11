<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InsertImageForm.ascx.cs" Inherits="InsertImageForm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dxhe" %>
<%@ Register Assembly="DevExpress.Web.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHiddenField" tagprefix="dxhf" %>
<%@ Register Src="ImagePropertiesForm.ascx" TagName="ImagePropertiesForm" TagPrefix="ucip" %>
<%@ Register Src="SelectImageForm.ascx" TagName="SelectImageForm" TagPrefix="ucip" %>

<dxp:ASPxPanel ID="Panel1" runat="server" Width="100%" DefaultButton="btnInsertImage">
    <PanelCollection>
        <dxp:PanelContent runat="server">
    <table cellpadding="0" cellspacing="0" id="dxInsertImageForm">
        <tr>
            <td class="contentCell">    
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="typeRadionButtonListCell">
                            <table cellpadding="0" cellspacing="0" class="radioButtonTable">
                                <tr>
                                    <td>
                                        <dxe:ASPxRadioButton ID="rblFromTheWeb" runat="server" GroupName="InsertImageFormGroup" Checked="True" TextWrap="False" ClientInstanceName="_dxeRblImageFromTheWeb">
                                            <ClientSideEvents CheckedChanged="function(s, e) { OnImageFromTypeChanged__InsertImageForm(s); }" />
                                        </dxe:ASPxRadioButton>                                    
                                    </td>
                                    <td>
                                        <dxe:ASPxRadioButton ID="rblFromThisComputer" runat="server" GroupName="InsertImageFormGroup" TextWrap="False" ClientInstanceName="_dxeRblImageFromThisComputer">
                                            <ClientSideEvents CheckedChanged="function(s, e) { OnImageFromTypeChanged__InsertImageForm(s); }" />
                                        </dxe:ASPxRadioButton>                                    
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>        
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="vertical-align:top">
                                        <dxhe:ASPxHtmlEditorRoundPanel ID="rpInsertImage" runat="server" Width="315px">
                                            <PanelCollection>
                                                <dxhe:HtmlEditorRoundPanelContent runat="server">
                                                    <!-- FromTheWeb -->                                                    
                                                    <table cellpadding="0" cellspacing="0" id='<% =HtmlEditor.ClientID + "_dxeFromTheWeb" %>' class="fromTheWeb" >
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblImageUrl" runat="server" AssociatedControlID="txbInsertImageUrl">
                                                                </dxe:ASPxLabel>
                                                                <dxe:ASPxButtonEdit ID="txbInsertImageUrl" ClientInstanceName="_dxeTbxInsertImageUrl"
                                                                    runat="server" Width="100%" AutoCompleteType="Disabled">
                                                                    <ClientSideEvents TextChanged="function(s, e) { aspxInsertImageSrcValueChanged(s.GetText()); }"
                                                                        ButtonClick="function(s,e){aspxHEOnBrowseButtonClick();}" />
                                                                    <Buttons>
                                                                        <dxe:EditButton>
                                                                        </dxe:EditButton>
                                                                    </Buttons>
                                                                    <ValidationSettings ErrorDisplayMode="Text" ErrorTextPosition="Bottom" SetFocusOnError="True"
                                                                        ValidateOnLeave="False" ValidationGroup="_dxeTbxInsertImageUrlGroup" Display="Dynamic">
                                                                        <RequiredField IsRequired="True" />
                                                                        <ErrorFrameStyle Font-Size="10px">
                                                                        </ErrorFrameStyle>
                                                                    </ValidationSettings>
                                                                </dxe:ASPxButtonEdit>
                                                                <dx:ASPxPopupControl ID="BrowsePopup" runat="server" CloseAction="CloseButton" PopupAction="None"
                                                                    PopupElementID="Panel1" ClientInstanceName="BrowsePopup" PopupHorizontalAlign="Center"
                                                                    PopupVerticalAlign="Middle" AllowDragging="True" Modal="true" EnableAnimation="false" RenderMode="Lightweight">
                                                                    <ContentStyle Paddings-Padding="0px">
                                                                    </ContentStyle>
                                                                    <ClientSideEvents PopUp="function(s, e) { aspxHEOnBrowserPopupOpen(); }" CloseUp="function(s, e) { aspxHEOnBrowserPopupClose(); }" />
                                                                    <ModalBackgroundStyle Opacity="1">
                                                                    </ModalBackgroundStyle>
                                                                    <ContentCollection>
                                                                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                                            <ucip:SelectImageForm ID="ucSelectImageForm" runat="server" />
                                                                        </dx:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                </dx:ASPxPopupControl>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="saveToServerCheckBoxCell">
                                                                <dxe:ASPxCheckBox ID="ckbSaveToServer" ClientInstanceName="_dxeCkbSaveToServer" runat="server">
                                                                </dxe:ASPxCheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="imagePreview"> 
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                                    <tr>
                                                                        <td class="imagePreviewCell">
                                                                            <span id='<% =HtmlEditor.ClientID + "_dxInsertImagePreviewText" %>'><% =DevExpress.Web.ASPxHtmlEditor.Localization.ASPxHtmlEditorLocalizer.GetString(DevExpress.Web.ASPxHtmlEditor.Localization.ASPxHtmlEditorStringId.InsertImage_Preview)%></span>
                                                                            <img id='<% =HtmlEditor.ClientID + "_dxInsertImagePreviewImage"%>' style="display:none;" width="180" height="100" alt="<% =DevExpress.Web.ASPxHtmlEditor.Localization.ASPxHtmlEditorLocalizer.GetString(DevExpress.Web.ASPxHtmlEditor.Localization.ASPxHtmlEditorStringId.InsertImage_Preview) %>"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>                                                    
                                                    <!-- FromThisComputer -->                    
                                                    <table cellpadding="0" cellspacing="0" id='<% =HtmlEditor.ClientID + "_dxeFromThisComputer" %>' style="display: none;">
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxLabel ID="lblBrowse" runat="server" AssociatedControlID="uplImage">
                                                                </dxe:ASPxLabel>
                                                                <div class="captionIndent"></div>
                                                                <%--  Since the client FileUploadComplete event is not generated if the length of a request (the size of a file to upload) exceeds the maximum allowed request length, we duplicate a call to the aspxImageUploadComplete method within the FilesUploadComplete event handler.
                                                                      This is required to switch the Insert Image dialog to its active state (to remove the Loading Div element).  --%>
                                                                <dxhe:ASPxHtmlEditorUploadControl ID="uplImage" runat="server" ClientInstanceName="_dxeUplImage"
                                                                    OnFileUploadComplete="uplImage_FileUploadComplete" Width="100%">
                                                                    <ClientSideEvents FileUploadComplete="function(s, e) { aspxImageUploadComplete(e); }" FilesUploadComplete="function(s, e) { aspxImageUploadComplete(e); }"
                                                                     TextChanged="function(s, e) { aspxImageUploadTextChanged(); }" FileUploadStart="function(s, e) { aspxImageUploadStart(); }"/>
                                                                </dxhe:ASPxHtmlEditorUploadControl>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dxhe:HtmlEditorRoundPanelContent>
                                            </PanelCollection>
                                        </dxhe:ASPxHtmlEditorRoundPanel>                                        
                                    </td>
                                    <%-- Image Properties --%>                                    
                                    <td id='<% =HtmlEditor.ClientID + "_dxeMoreImagePropertiesRow"%>' class="imagePropertiesCell" style="display:none">
                                        <ucip:ImagePropertiesForm ID="ucImagePropertiesForm" runat="server" />
                                    </td>                            
                                </tr>
                            </table>                                                
                        </td>
                    </tr>
                    <tr>
                        <td class="moreOptionsCell">
                            <dxe:ASPxCheckBox ID="ckbMoreImageOptions" ClientInstanceName="_dxeCkbMoreImageOptions" runat="server">
                                <ClientSideEvents CheckedChanged="function(s, e) { OnMoreImageOptionsCheckedChanged(s.GetChecked()) }" />                    
                            </dxe:ASPxCheckBox>            
                        </td>
                    </tr>
                </table>                            
            </td>        
        </tr>
        <tr>
            <%-- Button Cells --%>
            <td class="buttonsCell">
                <table cellpadding="0" cellspacing="0" style="display:inline-block;">
                    <tr>
                        <td>
                            <dxe:ASPxButton ID="btnInsertImage" runat="server" Height="26px" Width="82px"
                                Autopostback="false" ClientInstanceName="_dxeBtnInsertImage" CausesValidation="False" >
                                <clientsideevents click="function(s, e) { OnOkButtonClick_InsertImageForm(); }" />
                            </dxe:ASPxButton>
                            <dxe:ASPxButton ID="btnChangeImage" runat="server" AutoPostBack="False" Height="26px"
                                Width="74px" ClientInstanceName="_dxeBtnChangeImage" ClientVisible="False" CausesValidation="False">
                                <clientsideevents click="function(s, e) { OnOkButtonClick_InsertImageForm(); }" />
                            </dxe:ASPxButton>
                        </td>
                        <td class="cancelButton">
                            <dxe:ASPxButton ID="btnInsertImageCancel" runat="server" Height="26px"
                                Width="82px" Autopostback="false" CausesValidation="False">
                                <clientsideevents click="function(s, e) { OnCancelButtonClick_InsertImageForm(); }" />
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
<dxhf:ASPxHiddenField ID="dxHiddenField" runat="server" ClientInstanceName="_dxHiddenField" SyncWithServer="False"></dxhf:ASPxHiddenField>
<script type="text/javascript" id="dxss_InsertLinkForm">
    function GetDialogData_InsertImageForm() {
        var useOriginalSize = _dxeCmbSize.GetValue().toLowerCase() == "original";
        
        return {
            src: _dxeTbxInsertImageUrl.GetText(),
            cssClass: "",
            width: useOriginalSize ? "" : _dxeSpnWidth.GetValue(),
            height: useOriginalSize ? "" : _dxeSpnHeight.GetValue(),
            align: _dxeCmbImagePosition.GetValue(),
            alt: _dxeTxbDescription.GetText(),
            useFloat: _dxeCkbWrapTextArroundImage.GetChecked()
        };
    }
    function IsValidFields_InsertImageForm() {
        var ret = true;
        if (_dxeTbxInsertImageUrl.IsVisible())
            ret = ASPxClientEdit.ValidateGroup("_dxeTbxInsertImageUrlGroup") && ret;
        else {
            ret = (_dxeUplImage.GetText() != "") && ret;
            if(!ret)
                _dxeUplImage.UpdateCommonErrorDiv(_dxHiddenField.Get("RequiredFieldError"), true);
        }
        if (_dxeCkbCreateThumbnail.GetChecked())
            ret = ASPxClientEdit.ValidateGroup("_dxeThumbnailFileNameGroup") && ret;
        return ret;
    }
    function OnOkButtonClick_InsertImageForm() {
        if (!ASPxDialog.GetCurrentDialog().isPreventKeyPress && IsValidFields_InsertImageForm())        
            aspxDialogComplete(1, GetDialogData_InsertImageForm());
    }
    function OnCancelButtonClick_InsertImageForm() {
        aspxDialogComplete(0, GetDialogData_InsertImageForm());
    }
    function OnMoreImageOptionsCheckedChanged(isChecked) {
        _aspxSetElementDisplay(_aspxGetElementById('<% =HtmlEditor.ClientID + "_dxeMoreImagePropertiesRow"%>'), isChecked);
        if (isChecked)
            aspxAdjustControlsSizeInDialogWindow();
    }        
    function OnImageFromTypeChanged__InsertImageForm() {
        var fromWebArea = _aspxGetElementById('<% =HtmlEditor.ClientID + "_dxeFromTheWeb" %>');
        var fromThisComputerArea = _aspxGetElementById('<% =HtmlEditor.ClientID + "_dxeFromThisComputer" %>');
        _aspxSetElementDisplay(fromWebArea, _dxeRblImageFromTheWeb.GetChecked());
        _aspxSetElementDisplay(fromThisComputerArea, _dxeRblImageFromThisComputer.GetChecked());
        aspxOnImageFromTypeChanged();
    }
    function aspxHEOnBrowseButtonClick() {
        FileManager.browsePopup = BrowsePopup;
        BrowsePopup.Show();
        FileManager.Refresh();
        FileManager.Focus();
    }
    function aspxHEOnBrowserPopupOpen() {
        ASPxDialog.GetCurrentDialog().isPreventKeyPress = true;
        _aspxChangeAttribute(_aspxGetElementById('<% =HtmlEditor.ClientID + "_insertimagedialog_Panel1" %>'), "onkeypress", "");
    }
    function aspxHEOnBrowserPopupClose() {
        ASPxDialog.GetCurrentDialog().isPreventKeyPress = false;
        _aspxRestoreAttribute(_aspxGetElementById('<% =HtmlEditor.ClientID + "_insertimagedialog_Panel1" %>'), "onkeypress");
        _dxeTbxInsertImageUrl.Focus();
    }
</script>