<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_SubmissionConfirmation, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="WhiteBox" style="margin: auto; width:850px;">
    <center>
         <span class="boxPanelHeader">Submission Confirmation</span>
        <br /><br />
        <span style="font-size: 12pt"><asp:Literal ID="res_Message17" runat="server" Text="<%$ Resources:BrandingResource , res_Message17 %>" /></span><br />
        <span style="font-size: 12pt">Please allow at least 10 days for processing before attempting to submit any changes.</span>
        <div style="padding-top: 20px">
            <asp:Button ID="btnReturn" runat="server" Text="Return to Home Page" 
                CssClass="buttonBoxFocus" onclick="btnReturn_Click" />&nbsp;&nbsp;
            <asp:Button ID="btnAddProviderType" runat="server" Text="Add Provider Type" 
                CssClass="buttonBox" onclick="btnAddProviderType_Click" Visible="false" />
        </div>
        <br />
    </center>
        </div>
</asp:Content>

