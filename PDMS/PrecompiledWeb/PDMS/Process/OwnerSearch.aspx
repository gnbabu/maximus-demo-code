<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_OwnerSearch, App_Web_qtcaivva" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/OwnerSearch.ascx" TagName="OwnerSearch" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <div style="display:inline;">
        <h1><span style="text-align:center" tabindex="0"><asp:Label ID="lblTitle" runat="server" Text="Owner Search" /></span></h1>
            <asp:Panel ID="pnlReturn" runat="server" CssClass="PanelReturn" style="display:inline; float:right;">
                    <!--<asp:Button id="btnReturn" runat="server" Text="Return" CssClass="buttonBox" onclick="btnReturn_Click" CausesValidation="false" />-->
                    <asp:Panel ID="pnlGA" runat="server" Visible="false">
                        &nbsp;&nbsp;
                        <asp:Button ID="btnReturnToGroupAffiliations" runat="server" Text="Return to Group Affiliations" CssClass="buttonBox" OnClick="btnReturnToGroupAffiliations_Click" CausesValidation="false" />
                    </asp:Panel>
                </asp:Panel>
    </div>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <script  type="text/javascript">
        function doClick(buttonName, e) {
            // The purpose of this function is to allow the enter key to 
            // point to the correct button to click.
            var key;

            if (window.event) key = window.event.keyCode;   // IE
            else key = e.which;                             // Firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
    <div  >
    <uc1:OwnerSearch ID="ucOwnerSearch" runat="server" />
        </div>
</asp:Content>

