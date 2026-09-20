<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerPenalty" Codebehind="OwnerPenalty.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<style type="text/css">
    .auto-style2 {
        height: 168px;
    }
</style>
<%--<style type="text/css">
     .table>tbody>tr>th,
    .table>tbody>tr>td {
        border:none !important;
        margin-bottom:0px !important;
    }
</style>--%>
<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <asp:ValidationSummary ID="vsOwnerConviction" runat="server" DisplayMode="List" ValidationGroup="valOwnerConviction" />
        </div>
        <div>
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <ProgressTemplate>
                    <div>
                        <img src="../Images/ajax-loader.gif" alt="AJAX Loader" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <%-- <table>
            <tr>
                <td>--%>
        <%--<table id="ParentTable" runat="server" class="table">
     <tr><td>--%>
         <table id="Table1" runat="server" border="0" cellpadding="0" cellspacing="0" align="center" style="width: 800px !important">
        <tr>
                <td style="text-align: right !important; font-weight: bold">Person 1*</td>
                <td>
                   <asp:DropDownList ID="ddlOwner" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="ddlOwner_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerConviction"
                        ControlToValidate="ddlOwner" ErrorMessage="*Select an Owner" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                </td>
            </tr>
             <tr>
            
                <td style="text-align:match-parent; font-weight: bold; margin-right:10px" colspan="2" >Provide an explanation of the offense. Include details such as the time frame, matter of the offense, jurisdiction, date, program area, and sanction period</td>
             </tr> 
           
              <tr>
                <td style="text-align: right !important; font-weight: bold">Explanation*</td>
                <td>
                    <asp:TextBox ID="txtExplaination" runat="server" CssClass="formField" MaxLength="499" TextMode="MultiLine" Rows="4" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="valOwnerConviction"
                        ControlToValidate="txtExplaination" ErrorMessage="*Fill the Explanation" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                </td>
            </tr>
            </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlOwner" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

<asp:HiddenField ID="hdnRegOwnerPenaltyID" runat="server" />
