<%@ control language="C#" autoeventwireup="true" inherits="UserControls_AdverseActionHeader, App_Web_p4ixifjm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/AdverseAction.ascx" TagName="AdverseAction" TagPrefix="uc" %>
<table border="0" style="width:100%;">
    <tr>
        <td style="text-align:left;padding-left: 2px">
                <asp:Label ID="lblTitle" runat="server" Text="Results" CssClass="pageHeader" />
                        
        </td>
        <td style="text-align:right;">
            <asp:Button runat="server" ID="btnCreateAdverseAction"  CssClass="buttonBox" Text="Create Adverse Action" OnClick="btnCreateAdverseAction_Click" />
        </td>
    </tr>
</table>
<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlHeader" >
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" style="display:none">
    <asp:Panel ID="pnlHeader" CssClass="popupPanelHeader" runat="server" >
        <div>&nbsp;&nbsp;
            <asp:Label ID="lblPopUpTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White">Adverse Action - OIG Exclusion</asp:Label>
        </div>
    </asp:Panel>  
    <uc:AdverseAction runat="server" id="ucAdverseAction" OnCreateAdverseActionEvent="ucAdverseAction_CreateAdverseActionEvent" />
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />