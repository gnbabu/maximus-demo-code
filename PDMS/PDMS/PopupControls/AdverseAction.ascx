<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_AdverseAction" Codebehind="AdverseAction.ascx.cs" %>
<div style="text-align: left;padding: 15px">
    <div><asp:ValidationSummary ID="vsAdverseAction" runat="server" DisplayMode="List" ValidationGroup="valAdverseAction" /></div>
    <asp:RadioButtonList runat="server" ID="rblAdverseAction" BorderStyle="None" CellPadding="0" CellSpacing="0" 
                RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="QstRadioList">
        <asp:ListItem Text="Create Adverse Action" Value="1" Selected="True" />
    </asp:RadioButtonList>
    <br />
    <table border="0" cellpadding="2" cellspacing="2">
        <tr valign="top">
            <td><span class="formLabel">Comments</span></td>
            <td><asp:TextBox ID="txtComments" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /></td>
        </tr>
    </table>
   <table border="0" cellpadding="0" cellspacing="5" align="center">
        <tr>
            <td>
                <asp:Button id="btnSave" runat="server" Text="Save" CssClass="buttonBox" CausesValidation="true" ValidationGroup="valAdverseAction" OnClick="btnSave_Click" />
            </td>
            <td>
                <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</div>