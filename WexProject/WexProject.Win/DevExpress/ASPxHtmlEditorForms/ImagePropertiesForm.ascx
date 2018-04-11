<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImagePropertiesForm.ascx.cs" Inherits="ImagePropertiesForm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.2, Version=11.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<table cellpadding="0" cellspacing="0" id="dxImagePropertiesForm">
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblSize" runat="server" AssociatedControlID="cmbSize"></dxe:ASPxLabel>
            <div class="captionIndent"></div>            
            <dxe:ASPxComboBox ID="cmbSize" runat="server" ValueType="System.String" ClientInstanceName="_dxeCmbSize">
                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbSizeSelectedIndexChanged(s); }" />                
            </dxe:ASPxComboBox>
        </td>
    </tr>
    
    <tr style="display:none;" id='<% =HtmlEditor.ClientID + "_dxeSizePropertiesRow" %>'>
        <td class="imageSizeEditorsCell">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="captionCell">                                
                                    <dxe:ASPxLabel ID="lblWidth" runat="server" AssociatedControlID="spnWidth"></dxe:ASPxLabel></td>
                                <td>
                                    <dxe:ASPxSpinEdit ID="spnWidth" ClientInstanceName="_dxeSpnWidth" runat="server" Height="21px" Number="1" Width="52px" AllowNull="False" NumberType="Integer" MaxValue="10000" MinValue="1">
                                        <SpinButtons ShowIncrementButtons="False">
                                        </SpinButtons>
                                        <ClientSideEvents NumberChanged="function(s, e) { aspxSizeSpinNumberChanged(&quot;width&quot;); }" KeyUp="function(s, e) { aspxSizeSpinKeyUp(&quot;width&quot;, e.htmlEvent); }"/>
                                    </dxe:ASPxSpinEdit>                        
                                </td>
                                <td class="pixelSizeCell"><dxe:ASPxLabel ID="lblPixelWidth" runat="server"></dxe:ASPxLabel></td>
                            </tr>
                            <tr>
                                <td colspan="3" class="separatorCell"></td>
                            </tr>
                            <tr>
                                <td class="captionCell"><dxe:ASPxLabel ID="lblHeight" runat="server" AssociatedControlID="spnHeight"></dxe:ASPxLabel></td>
                                <td>
                                    <dxe:ASPxSpinEdit ID="spnHeight" ClientInstanceName="_dxeSpnHeight" runat="server" Height="21px" Number="1" Width="52px" AllowNull="False" MinValue="1" NumberType="Integer" MaxValue="10000">
                                        <SpinButtons ShowIncrementButtons="False">
                                        </SpinButtons>
                                        <ClientSideEvents NumberChanged="function(s, e) { aspxSizeSpinNumberChanged(&quot;height&quot;); }" KeyUp="function(s, e) { aspxSizeSpinKeyUp(&quot;height&quot;, e.htmlEvent);  }"/>
                                    </dxe:ASPxSpinEdit>
                                </td>
                                <td class="pixelSizeCell"><dxe:ASPxLabel ID="lblPixelHeight" runat="server"></dxe:ASPxLabel></td> 
                            </tr>
                        </table>                    
                    </td>
                    <td class="constrainProportionsCell">
                        <table cellpadding="0" cellspacing="0">
                            <tr><td id="cellContarainTop" runat="server"></td></tr>
                            <tr><td id="cellContarainSwitcher" runat="server" style="cursor: pointer;"></td></tr>
                            <tr><td id="cellContarainBottom" runat="server"></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="fieldSeparator"></div>
            <dxe:ASPxCheckBox ID="ckbCreateThumbnail" runat="server" ClientInstanceName="_dxeCkbCreateThumbnail">
                <ClientSideEvents CheckedChanged="function(s, e) { OnCreateThumbnailCheckedChanged(s.GetChecked()) }" />
            </dxe:ASPxCheckBox>
            <div id='<% =HtmlEditor.ClientID + "_dxeThumbnailFileNameArea" %>' class="thumbnailFileNameArea" style="display:none;">
                <dxe:ASPxLabel ID="lblThumbnailFileName" runat="server" AssociatedControlID="txbThumbnailFileName"></dxe:ASPxLabel>            
                <div class="captionIndent"></div>            
                <dxe:ASPxTextBox ID="txbThumbnailFileName" ClientInstanceName="_dxeThumbnailFileName" runat="server" Width="170px" AutoCompleteType="Disabled">
                    <ValidationSettings ErrorDisplayMode="Text" ErrorTextPosition="Bottom" SetFocusOnError="True" ValidateOnLeave="False" ValidationGroup="_dxeThumbnailFileNameGroup">
                        <ErrorFrameStyle Font-Size="10px">
                            <Paddings PaddingRight="0px" />                        
                        </ErrorFrameStyle>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dxe:ASPxTextBox>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="fieldSeparator"></div>        
        </td>
    </tr>
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblImagePosition" runat="server" AssociatedControlID="cmbImagePosition"></dxe:ASPxLabel>
            <div class="captionIndent"></div>                        
            <dxe:ASPxComboBox ID="cmbImagePosition" ClientInstanceName="_dxeCmbImagePosition" runat="server" ValueType="System.String">
                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnImagePositionChanged(s); }" />
            </dxe:ASPxComboBox>            
        </td>
    </tr>
    <tr>
        <td class="ckbWrapTextCell">
            <div>
                <dxe:ASPxCheckBox ID="ckbWrapTextArroundImage" ClientInstanceName="_dxeCkbWrapTextArroundImage" runat="server" />
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="fieldSeparator"></div>        
        </td>
    </tr>    
    <tr>
        <td>
            <dxe:ASPxLabel ID="lblImageDescription" runat="server" AssociatedControlID="txbDescription"></dxe:ASPxLabel>
            <div class="captionIndent"></div>
            <dxe:ASPxTextBox ID="txbDescription" ClientInstanceName="_dxeTxbDescription" runat="server" Width="170px" AutoCompleteType="Disabled">
            </dxe:ASPxTextBox>
        </td>
    </tr>    
</table>
<script type="text/javascript" id="dxss_ImagePropertiesForm">
    function OnCmbSizeSelectedIndexChanged(cmb) {
        var isShow = cmb.GetValue() != "original";
        _aspxSetElementDisplay(_aspxGetElementById('<% =HtmlEditor.ClientID + "_dxeSizePropertiesRow" %>'), isShow);
        if (isShow)
            aspxAdjustControlsSizeInDialogWindow();
    }
    function OnCreateThumbnailCheckedChanged(isChecked) {    
        _aspxSetElementDisplay(_aspxGetElementById('<% =HtmlEditor.ClientID + "_dxeThumbnailFileNameArea" %>'), isChecked);
        if (isChecked)
            aspxAdjustControlsSizeInDialogWindow();
    }
    function OnImagePositionChanged(cmb) {
        var selectedItem = cmb.GetItem(cmb.GetSelectedIndex());
        _dxeCkbWrapTextArroundImage.SetEnabled(selectedItem.value != "center");
    }
</script>