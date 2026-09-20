<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ReturnToScreeningReasons" Codebehind="ReturnToScreeningReasons.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
                
                <ContentTemplate>
<div><asp:ValidationSummary ID="vsReturnToScreeningReasons" runat="server" DisplayMode="List" ValidationGroup="valReturnToScreeningReasons" /></div>
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="33%" />
        <col width="33%" />
        <col width="33%" />
    </colgroup>
    
    <tr>
        <td><asp:Label ID="lblComments" runat="server" Text="Comments*" CssClass="formLabel150" /></td>
        <td><asp:TextBox ID="txtComments" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" />

        </td>
        <td>            <asp:RequiredFieldValidator ID="valClosedEndAgrReqd" runat="server" ControlToValidate="txtComments"  
                 Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                 ValidationGroup ="valReturnToScreeningReasons" ErrorMessage="* Enter Comments."></asp:RequiredFieldValidator></td>
    </tr>

    <tr>
           
        
        </tr>
    </table>
                     </ContentTemplate>
</asp:UpdatePanel>
