<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ProcessAppeal" Codebehind="ProcessAppeal.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
<div><asp:ValidationSummary ID="vsProcessAppealInfo" runat="server" DisplayMode="List" ValidationGroup="valProcessAppealInfo" /></div>
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="33%" />
        <col width="33%" />
        
    </colgroup>
    <tr>
        <td><asp:Label ID="lblreason" runat="server" Text="Reason" CssClass="formLabel wd170" /></td>
        <td class="wd150"><asp:DropDownList ID="ddlreason" runat="server" CssClass="DropDownList"  /></td>
        
    </tr>
    <tr>
        <td><asp:Label ID="lblImmediate" runat="server" Text="Immediate" CssClass="formLabel wd170" /></td>
        <td><asp:RadioButtonList ID="rblImmediate" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblImmediate_SelectedIndexChanged" AutoPostBack="true" >
            <asp:ListItem Value="1" Text="Yes" />
            <asp:ListItem Value="2" Text="No" />
            </asp:RadioButtonList></td>
        
    </tr>
    <tr>
        <td><asp:Label ID="lblAppealStartDate" runat="server" Text="Appeal Period Begin Date" CssClass="formLabel wd170" /></td>
        <td class="wd150">
            <asp:TextBox ID="txtAppealStartDate" runat="server" CssClass="formField" style="float:left" OnTextChanged="txtAppealStartDate_TextChanged" AutoPostBack="true"/>
            <ajax:CalendarExtender ID="calStart" TargetControlID="txtAppealStartDate" runat="server" />

        </td>
        
    </tr>
    <tr>
        <td><asp:Label ID="lblAppealEndDate" runat="server" Text="Appeal Period End Date" CssClass="formLabel wd170" /></td>
        <td class="wd150">
            <asp:TextBox ID="txtAppealEndDate" runat="server" CssClass="formField" style="float:left" />
            <ajax:CalendarExtender ID="calEnd" TargetControlID="txtAppealEndDate" runat="server" />

        </td>
        
    </tr>
    <tr>
        <td><asp:Label ID="lblAppealStatus" runat="server" Text="Appeal Status" CssClass="formLabel wd170" /></td>
        <td class="wd150"><asp:DropDownList ID="ddlAppealStatus" runat="server" CssClass="DropDownList" style="float:left" 
                             OnSelectedIndexChanged="ddlAppealStatus_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" /></td>
        
    </tr>
    <tr>
        <td><asp:Label ID="lblProviderEndDate" runat="server" Text="Provider End Date" CssClass="formLabel wd170" /></td>
        <td class="wd150">
            <asp:TextBox ID="txtProviderEndDate" runat="server" CssClass="formField" style="float:left" />
            <ajax:CalendarExtender ID="calProviderEndDate" TargetControlID="txtProviderEndDate" runat="server" />
            
        </td>
        
    </tr>
     <tr>
        <td><asp:Label ID="lblComments" runat="server" Text="Comments" CssClass="formLabel wd170" /></td>
        <td class="wd150">
            <asp:TextBox ID="txtComments" runat="server"  MaxLength="1000" TextMode="MultiLine" CssClass="formField220" style="float:left" />
            

        </td>
         
    </tr>
    <tr>
           
        
        </tr>
    </table>
                     </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="rblImmediate" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="ddlAppealStatus" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>