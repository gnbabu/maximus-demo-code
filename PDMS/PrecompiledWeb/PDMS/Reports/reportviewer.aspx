<%@ page title="" language="C#" masterpagefile="~/NonModal.master" autoeventwireup="true" inherits="Reports_reportviewer, App_Web_xmsld3al" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<style type="text/css">
#mainForm
{
    margin-left: 20px;  

}
.RightBox
{
    width: 100%;
}
.UserHeader
{
    
    width: 1520px;
    
}

    .Footer {
        position: fixed;
    }
#ctl00_MainContent_ReportViewer1_fixedTable
{
    margin-left:auto;
    margin-right:auto;
    width: 90%;
}

.report-header-text {
    font-family: "Arial Narrow";
    color: #036;
    font-size : 24px;
    font-weight:700;
    height:auto;
    line-height:inherit;
    text-align: center;
    text-size-adjust:100%;
}
#ctl00_MainContent_ReportViewer1{
    width:auto !important;
    height:100% !important;
}
</style> 
<div style="display:flex; justify-content:center; text-align:center; margin-left:20px; margin-right:20px">
    <div style="width:100%">
        <div>
            <asp:ImageButton ID="ImgBtnHome" runat="server" ImageUrl="~/Images/glyphicon-home-icon-fill.jpg" AlternateText="Home" OnClick="ImageButtonHome_Click" ImageAlign="left" Height="60px" Width="50px" ToolTip="Home" style="padding-bottom:10px"/>
            <span class="report-header-text"> <asp:Label runat="server" Text="Reports" ID="lbltitle"></asp:Label>&nbsp</span>
            <span class="report-header-text"><asp:Label runat="server" Text="" ID="lblSideContent" style="font-size:medium" ></asp:Label></span>
        </div>
        <br />
        <div>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"  AsyncRendering="true"
            DocumentMapCollapsed="True" ShowBackButton="False"  ShowReportBody ="false" OnSubmittingParameterValues ="rv_SubmittingParameterValues"
            ShowDocumentMapButton="False" ShowFindControls="False" ShowZoomControl="False" SizeToReportContent="true"
             style="margin-bottom:80px; text-align:center; margin-left:0%; margin-top:10px;" >
            </rsweb:ReportViewer>
            <asp:Button ID="BtnReturn" runat="server" Text="Return" onclick="BtnReturn_Click" CssClass="buttonBox"/>
        </div>
    </div>
</div>
</asp:Content>
