<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ApproveProvider" Codebehind="ApproveProvider.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
                
                <ContentTemplate>
<div><asp:ValidationSummary ID="vsApproveProviderInfo" runat="server" DisplayMode="List" ValidationGroup="valApproveProviderInfo" /></div>
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="33%" />
        <col width="33%" />
        
    </colgroup>
    
    <tr>
        <td><asp:Label ID="lblClosedEndAgreement" runat="server" Text="Closed End Agreement*" CssClass="formLabel150" /></td>
        <td><asp:RadioButtonList ID="rblClosedEndAgreement" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal">
            <asp:ListItem Value="1" Text="Yes" />
            <asp:ListItem Value="2" Text="No" />
            </asp:RadioButtonList>

        </td>
        <td>            <asp:RequiredFieldValidator ID="valClosedEndAgrReqd" runat="server" ControlToValidate="rblClosedEndAgreement"  
                 Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                 ValidationGroup ="valApproveProviderInfo" ErrorMessage="* Select Closed End Agreement."></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date" CssClass="formLabel150" /></td>
        <td class="wd150">
            <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField" style="float:left" />
            <ajax:CalendarExtender ID="calStart" TargetControlID="txtEffectiveDate" runat="server" />

        </td>
        
    </tr>
    
     <tr>
        <td class="verticalAlignTop"><span class="formLabel150 verticalAlignTop">Comments</span></td>
        <td class="wd150">
            <asp:TextBox ID="txtComments" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" />
            

        </td>
         
    </tr>
    <tr>
           
        
        </tr>
    </table>
                     </ContentTemplate>
</asp:UpdatePanel>
