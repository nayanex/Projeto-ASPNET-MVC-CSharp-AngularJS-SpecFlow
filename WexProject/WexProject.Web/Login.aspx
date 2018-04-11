<%@ Page Language="C#" AutoEventWireup="true" Inherits="LoginPage" CodeBehind="Login.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v11.2" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v11.2" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="tc" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v11.2" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v11.2" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Logon</title>
	<style type="text/css">
        @charset "utf-8";
        *  {margin:0;padding:0;}
        html{height:100%;}
        body {background-image:url(Images/background.png);margin: 0 auto;height:100%;}
        .geral {min-height:100%;position:relative;}
        #bg_topo {margin: 0 auto;height: 107px;background: center  url(Images/bg_up.png);}
        #bg_footer {position:absolute; bottom:0;width:100%;margin: 0 auto;height: 145px;background: center url(Images/bg_down.png);}
        #bg_box_login {margin: 0 auto;margin-top: 50px;width: 877px;height: 415px;background-image:url(Images/bg_box_login.png);}
        #bg_box_login .column-info{width:155px; float:left; display:block; height:125px; padding:135px 0px 0px 0px;}
        #bg_box_login .column-inputs{width:570px; float:left; display:block; height:180px; padding:140px 0px 0px 0px;}
        #TableCell4 {padding: 0px !important;}
    </style>
</head>
<body class="Dialog">
<div class="geral">
   <div id="PageContent" class="PageContent DialogPageContent">
    <script src="MoveFooter.js" type="text/javascript"></script>
	<form id="form1" runat="server">
        <!-- new -->
	        <div id="bg_topo"></div> 
	        <div id="bg_box_login">
		        <div class="column-info">
		        </div>
		        <div class="column-inputs">
    	            <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
                    <cc3:XafUpdatePanel ID="UPEI" runat="server">
                        <tc:ErrorInfoControl ID="ErrorInfo" Style="margin: 10px 0px 10px 0px" runat="server" />
                    </cc3:XafUpdatePanel>
                    <asp:Table ID="Table1" CssClass="Logon" runat="server" BorderWidth="0px" CellPadding="0"
                        CellSpacing="0">
                        <asp:TableRow ID="TableRow2" runat="server">
                            <asp:TableCell runat="server">
                                <cc3:XafUpdatePanel ID="UPVSC" runat="server">
                                    <cc4:ViewSiteControl ID="viewSiteControl" runat="server"/>
                                </cc3:XafUpdatePanel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="TableRow3" runat="server">
                            <asp:TableCell runat="server" ID="TableCell4" HorizontalAlign="Right" Style="padding: 20px 0px 20px 0px">
                                <cc2:ActionContainerHolder ID="PopupActions" runat="server" Categories="PopupActions"
                                    Style="margin-left: 10px; display: inline" Orientation="Horizontal" ContainerStyle="Buttons">
                                    <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right"/>
                                </cc2:ActionContainerHolder>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
		        </div>
	        </div>
      
        <!-- new new -->
	</form>
        <div id="Spacer" class="Spacer">
        </div>
    <script type="text/javascript">
    <!--
	    function OnLoad() {
	        DXMoveFooter();
            DXattachEventToElement(document.getElementById('formTable'), "resize", DXWindowOnResize);
            DXattachEventToElement(window, "resize", DXWindowOnResize);
            detachElementEvent(document.body, "keypress", disableEnterKey);
        }
    //-->	    
    </script>
   </div>
     <div id="bg_footer"></div>
</div>
</body>
</html>
