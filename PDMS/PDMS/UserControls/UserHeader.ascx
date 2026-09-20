<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_UserHeader" Codebehind="UserHeader.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/MessageModalControls.ascx" TagName="MessageModalControls" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/Notes.ascx" TagName="Notes" TagPrefix="uc2" %>
<%@ Register Src="~/PopupControls/EmailNotifications.ascx" TagName="Emails" TagPrefix="uc2" %>


<style>

    .dropbtnheader {
    background-color: #FFFFFF;
    color: Blue;
    padding: 10px;
    font-size: 14px;
    border: none;
    cursor: pointer;
   text-decoration: underline;
}
.dropbtn {
    background-color: #FFFFFF;
    color: Blue;
    padding: 10px;
    font-size: 13px;
    border: none;
    cursor: pointer;
}

.dropdown {
    position: relative;
    display: inline-block;
}

.dropdown-content {
    display: none;
    position: absolute;
    background-color: #FFFFFF;
    min-width: 160px;
    box-shadow: 0px 8px 8px 0px rgba(0,0,0,0.2);
    z-index: 1;
}

.dropdown-content a {
    color: blue;
    padding: 0px 0px;
    
    text-decoration: underline;
    display: block;
}

.dropdown-content a:hover {background-color: #f1f1f1}

.dropdown:hover .dropdown-content {
    display: block;
}

.dropdown:hover .dropbtn {
    background-color: #FFFFFF;
}
</style>
<asp:Panel ID="pnlHeader" runat="server" CssClass="UserHeader">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" role="presentation">
        <colgroup>
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
        </colgroup>
        <tr style="vertical-align: middle">
            <td align="left">
                <asp:Label ID="lblUser" runat="server" CssClass="formLabelHeader" Text="User: superuser13" />
            </td>
            <td>
                <asp:Label ID="lblDate" runat="server" CssClass="formLabelHeader" Text="Wednesday, May 7, 2013" />
            </td>

            

            <td align="right">
                <table border="0" cellpadding="0" cellspacing="7" role="presentation">
                    <tr>

                        <td>
                            <div class="dropdown">
                            <button class="dropbtnheader">Menu</button>                            
                            <div class="dropdown-content">
                            <asp:Menu ID="mnuLeftNav" runat="server" Orientation="Vertical" CssClass="dropbtn"
                            DataSourceID="SiteMapDataSource1" StaticEnableDefaultPopOutImage="false"
                            DynamicEnableDefaultPopOutImage="false" >
                            <DynamicMenuStyle CssClass="dropdown-content" />
                            <DynamicHoverStyle CssClass="dropdown-content" />
                            <DynamicMenuStyle CssClass="dropdown-content" />
                            <DynamicMenuItemStyle CssClass="dropdown-content" />
                            </asp:Menu>
                            <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="false"
                            SiteMapProvider="XmlSiteMapProviderData" />
                            </div>
                            </div>
                        </td>

                                                <td>
                            <asp:ImageButton ID="btnHome" ImageUrl="~/Images/home.jpg" runat="server"
                                ToolTip="Home" OnClick="btnHome_Click" Width="24px" CausesValidation="false" />
                        </td>
                        <td style="text-align:right;">
                            <asp:LinkButton ID="lnkHome" runat="server" Text="Home" OnClick="lnkHome_Click" CausesValidation="false" /></td>
                        <td>
                            <asp:ImageButton ID="btnNotes" ImageUrl="~/Images/notes.jpg" runat="server" CausesValidation="false"
                                ToolTip="Notes" Width="24px" OnClick="btnNotes_Click" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkNotes" runat="server" Text="Notes" OnClick="lnkNotes_Click" CausesValidation="false" /></td>
                        <td>
                            <asp:ImageButton ID="btnEmail" ImageUrl="~/Images/email.jpg" runat="server"
                                ToolTip="Email" Width="24px" OnClick="btnEmail_Click" CausesValidation="false" Style="height: 24px" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkEmail" runat="server" Text="Email" OnClick="lnkEmail_Click" CausesValidation="false" /></td>
                        <td>
                            <asp:ImageButton ID="btnEventHistory" ImageUrl="~/Images/actionhistory.jpg" runat="server"
                                ToolTip="Event History" Width="24px" OnClick="btnEventHistory_Click" CausesValidation="false" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkEventHistory" runat="server" Text="Event History" OnClick="lnkEventHistory_Click" CausesValidation="false" /></td>
                        <td>
                            <asp:ImageButton ID="btnHelp" ImageUrl="~/Images/help.jpg" runat="server" CausesValidation="false"
                                ToolTip="Help" Width="24px" OnClick="btnHelp_Click" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkHelp" runat="server" Text="Help" OnClick="lnkHelp_Click" CausesValidation="false" /></td>
                        <td>
                            <asp:ImageButton ID="btnLogout" ImageUrl="~/Images/logout.jpg" runat="server" CausesValidation="false"
                                ToolTip="Logout" Width="24px" OnClick="btnLogout_Click" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkLogout" runat="server" Text="Logout" OnClick="lnkLogout_Click" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>

<uc1:MessageModalControls ID="ucMessageModal" runat="server" />

<cc1:ModalPopupExtender ID="mpeNotesGrid" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 100px; height: auto; width: auto;">
    <asp:Panel ID="Panel1" CssClass="popHeader" runat="server" >
        <div class="popTitle">Notes</div>
    </asp:Panel>
    <uc2:Notes ID="ucNotes" runat="server" Mode="Grid" />
    <div class="btnBox" style="padding-right: 5px;">
        <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="buttonBox" CausesValidation="false" />
    </div>
</asp:Panel>

<asp:Button runat="server" ID="ButtonDummy" Style="display: none" />

<cc1:ModalPopupExtender ID="mpeEmailsGrid" runat="server" PopupControlID="pnlModalEmails" TargetControlID="ButtonDummy"
    CancelControlID="btnClose" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModalEmails" runat="server" CssClass="modalPopup" align="center" Style="display: none">
    <asp:Panel ID="Panel3" CssClass="popHeader" runat="server">
            <div class="popTitle">Email Notifications</div>
    </asp:Panel>
        <asp:Panel ID="Panel4" runat="server">
            <asp:Panel runat="server" ID="Panel5">
                <div id="divTest1" runat="server" style="max-height:700px; overflow-y:scroll;">
                    <uc2:Emails ID="ucEmails" runat="server" />
                </div>
            </asp:Panel>
        </asp:Panel>
    <div style="padding-bottom: 10px; margin-left:auto; margin-right:auto;">
    
        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonBox"  CausesValidation="false" />
    </div>
</asp:Panel>
