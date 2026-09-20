<%@ control language="C#" autoeventwireup="true" inherits="UserControls_MessageModal, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style type="text/css">
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.2;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        border-width: 1px;
        border-style: solid;
        border-color: black;
        padding: 0px;
        width: 50%;
        height: auto;
    }
</style>

<!-- ModalPopupExtender -->
<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="Panel1" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style="display:none">
    <asp:Panel ID="pnlHeader" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div align="left">&nbsp;&nbsp;
        <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>  
    <asp:Panel ID="pnlLabel" runat="server">
        <div style="text-align: left;padding: 15px">
            <asp:Label id="lblMessage" runat="server" />
        </div>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="3" align="center" role="presentation">
        <tr>
            <td>
                <asp:Button id="btnContinue"  runat="server" Text="Continue" CssClass="buttonBox" CausesValidation="false" onclick="btnContinue_Click" />
            </td>
            <td>
                <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" 
                    CausesValidation="false" />
            </td>
        </tr>
    </table> 
    <br />
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
