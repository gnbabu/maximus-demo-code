<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_TestELicenseVerification" Codebehind="TestELicenseVerification.ascx.cs" %>

<asp:Panel ID="Panel1" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;" GroupingText="Test eLicense Verification">
        <div>
            <asp:Label ID="Label4" runat="server" Text="Last digits of ssn"></asp:Label><asp:TextBox ID="tblast4ssn" runat="server" aria-Label="Last Digits of SSN"></asp:TextBox><br />
            <asp:Label ID="Label5" runat="server" Text="lastname"></asp:Label><asp:TextBox ID="tbLastName" runat="server" aria-Label="Last Name"></asp:TextBox><br />
            <asp:Label ID="Label6" runat="server" Text="Board"></asp:Label><asp:TextBox ID="tbBoardname" runat="server" aria-Label="Board"></asp:TextBox><br />                       
            <asp:Label ID="Label7" runat="server" Text="DOB yyyy-mm-dd"></asp:Label><asp:TextBox ID="tbdob" runat="server" aria-Label="Date of Birth in yyyy-mm-dd"></asp:TextBox><br />
            <asp:Label ID="Label8" runat="server" Text="Request sent"></asp:Label><asp:TextBox ID="tbRequestLicense" runat="server" TextMode="MultiLine" aria-Label="Request Sent"></asp:TextBox><br />
            <asp:Label ID="Label9" runat="server" Text="license Response"></asp:Label><asp:TextBox ID="tbresponselicense" runat="server" TextMode="MultiLine" aria-Label="License Response"></asp:TextBox><br />
            <asp:Label ID="Label10" runat="server" Text="Error"></asp:Label><asp:TextBox ID="tblicenseException" runat="server" TextMode="MultiLine" aria-Label="Error"></asp:TextBox><br />
            <asp:Button ID="Button1" runat="server" Text="Make elicense Request" CssClass="buttonBox" OnClick="btnRequestLicenseInfo_Click" />
        </div>
</asp:Panel>