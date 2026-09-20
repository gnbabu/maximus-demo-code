<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_MessageBox, App_Web_glma3lal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!-- ModalPopupExtender -->
<cc1:ModalPopupExtender ID="mpeMessageBox" runat="server" TargetControlID="ButtonDummy"
    CancelControlID="btnOK" BackgroundCssClass="modalBackground" 
    PopupControlID="Panel1" PopupDragHandleControlID="pnlHeader" >
</cc1:ModalPopupExtender>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style="display:none; width:auto; height:auto; min-height: 170px; min-width:270px;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" >
        <table style="width: 100%" role="presentation">
            <tr>
                <td class="popTitle"><asp:Label ID="lblTitle" runat="server" Text="Title"  /> </td>
                <td style="text-align: right">
                    <asp:Label ID="lblCurrentDateTime" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
                </td>
            </tr>
        </table>
    </asp:Panel>  
    <asp:Panel ID="pnlLabel" runat="server">
        <div style="text-align: left;padding: 15px">
            <asp:Label id="lblMessage" runat="server" />
        </div>
    </asp:Panel>
    <div style="text-align: center">
        <asp:Button id="btnOK"  runat="server" Text="OK" CssClass="buttonBox" CausesValidation="false" />
    </div>
    <br />
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>
