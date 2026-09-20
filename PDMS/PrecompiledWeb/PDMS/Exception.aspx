<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Exception, App_Web_jwgsiblf" enableEventValidation="false" stylesheettheme="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <asp:Label ID="lblHeader" runat="server" Text="An Error has Occurred" CssClass="header center"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">
        <div id="resource-link-box">
            <div id="divRender" runat="server">
                <asp:PlaceHolder ID="phAutoGenerate" runat="server"></asp:PlaceHolder>
            </div>
            <div class="center">
                An unexpected error has occurred on our website.  The Administrator has been notified.
        <br />
                <asp:Label ID="lblMessage" runat="server" CssClass="redAsterisk" />
                <br />
                <br />
                <asp:Button ID="btnReturn" runat="server" Text="Return to Home Page"
                    CssClass="buttonBox" OnClick="btnReturn_Click" />
                <br />
                <br />
            </div>
        </div>
    <div>
        <asp:Button ID="btnShowDetail" runat="server" Text="Show Detail" OnClick="btnShowDetail_Click" />
    </div>
        <asp:Panel ID="pnlDetails" runat="server" Visible="false">
            <div>
                <asp:Label ID="lblErrorType" runat="server" CssClass="Header" ></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblStackTrace" runat="server"></asp:Label>
            </div>
        </asp:Panel>
    </div>

</asp:Content>


