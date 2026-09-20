<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Account_ProviderRoleSelection" Codebehind="ProviderRoleSelection.aspx.cs" %>

<%@ Register Src="~/UserControls/SelectedControl.ascx" TagName="SelectedControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    User Profile
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="WhiteBox">  
        <br />
        <asp:Label ID="UNR11_ERR" runat="server" Style="color: Red!important;" Visible="false" Text="* We are unable to send an email." CssClass="failureNotification" />
        <asp:Panel ID="pnlRoleSelect" runat="server">
            <div class="row">
                <div class="col-sm-12">
                    <p style="font-size: larger; font-weight: bold; color: #545488;">
                        What type of Provider Account do you need to create?
                    </p>
                    <div style="margin-left: 3%;">
                        <asp:RadioButtonList ID="rblProviderRole" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
                            <asp:ListItem Text="Provider Administrator" Value="ProviderAdministrator" />
                            <asp:ListItem Text="Provider Agent" Value="ProviderAgent" />
                            <asp:ListItem Text="CEO Certified (DODD)" Value="CEOCertified" />
                            <asp:ListItem Text="Secondary User (DODD)" Value="DODDAgent" />
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlBtns" runat="server">
            <div style="text-align: center; margin-left: 20%;">
                <asp:Button ID="btnSaveAcc" runat="server" CssClass="buttonBox buttonBoxFocus" OnClick="btnSaveAcc_Click" Text="Save" />
                <asp:Button ID="btnReturn" runat="server" CssClass="buttonBox" Text="Cancel" />
            </div>
        </asp:Panel>
        <asp:HiddenField ID="hdnRoleName" runat="server" />
    </div>
    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        function nospaces(t) {
            if (t.value.match(/\s/g)) {
                t.value = t.value.replace(/\s/g, '');
            }
        }
    </script>
</asp:Content>
