<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_SelectedControl" Codebehind="SelectedControl.ascx.cs" %>

<style type="text/css">
    .Selected
    {
        background-color: Green;
        color: White;
        padding: 3px;
    }
    .NotSelected
    {
        background-color: White;
        color: Black;
        padding: 3px;
    }    
</style>

<asp:Panel ID="pnlControl" runat="server">
    <asp:Label ID="lblText" runat="server" />
</asp:Panel>
