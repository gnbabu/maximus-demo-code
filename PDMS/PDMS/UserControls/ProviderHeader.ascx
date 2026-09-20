<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ProviderHeader" Codebehind="ProviderHeader.ascx.cs" %>
<%@ Register Src="HeaderLine.ascx" TagName="HeaderLine" TagPrefix="cc1" %>

<cc1:HeaderLine ID="ucHeaderProviderInfo" runat="server" Header="Provider Information" />
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <colgroup>
        <col width="50%" align="left"" />
        <col width="50%" align="left"" />
    </colgroup>
    <tr>
        <td>
            <span class="formLabel170">Provider Name</span>
            <asp:MultiView ID="mltProviderName" runat="server" ActiveViewIndex="0">
                <asp:View ID="vwLabel" runat="server">
                    <asp:Label ID="lblProviderName" CssClass="formFieldDisplay" runat="server" /> 
                </asp:View>
                <asp:View ID="vwLink" runat="server">
                    <asp:LinkButton ID="lnkProviderName" CssClass="formFieldDisplay" runat="server" 
                        onclick="lnkProviderName_Click" />
                </asp:View>
            </asp:MultiView>
        </td>
        <td>
            <asp:MultiView ID="mltEmail" runat="server">
                <asp:View ID="vwNormal" runat="server">
                    <span class="formLabel170">Email Address</span>
                    <asp:Label ID="lblEmailNormal" CssClass="formFieldDisplay" runat="server" />
                </asp:View>
                <asp:View ID="vwLarge" runat="server">
                    <span class="formLabelAuto">Email</span>
                    <asp:Label ID="lblEmailLarge" CssClass="formFieldDisplayAuto" runat="server" />
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">Provider ID</span>
            <asp:Label ID="lblProviderId" CssClass="formFieldDisplay"  runat="server" />
        </td>
        <td>
            <span class="formLabel170">PDMS Status</span>
            <asp:Label ID="lblPDMSStatus" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">Cred Address Street1</span>
            <asp:Label ID="lblCredAddressStreet1" CssClass="formFieldDisplay" runat="server" />
        </td>
        <td>
            <span class="formLabel170">Cred Address Street2</span>
            <asp:Label ID="lblCredAddressStreet2" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">Cred City</span>
            <asp:Label ID="lblCredCity" CssClass="formFieldDisplay" runat="server" />
        </td>
        <td>
            <span class="formLabel170">Cred State/Zip</span>
            <asp:Label ID="lblCredStateZip" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">CAQH ID</span>
            <asp:Label ID="lblCAQHID" CssClass="formFieldDisplay"  runat="server" />
        </td>
        <td>
            <span class="formLabel170">PDMS Status Date</span>
            <asp:Label ID="lblPDMSStatusDate" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">NPI</span>
            <asp:Label ID="lblNPI" CssClass="formFieldDisplay"  runat="server" />
        </td>
        <td>
            <span class="formLabel170">CAQH Status</span>
            <asp:Label ID="lblCAQHStatus" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">SSN</span>
            <asp:Label ID="lblSSN" CssClass="formFieldDisplay"  runat="server" />
        </td>
        <td>
            <span class="formLabel170">Roster Status</span>
            <asp:Label ID="lblRosterStatus" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">DOB</span>
            <asp:Label ID="lblDOB" CssClass="formFieldDisplay"  runat="server" />
        </td>
        <td>
            <span class="formLabel170">Last Attest Date</span>
            <asp:Label ID="lblLastAttestDate" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <span class="formLabel170">Contact Phone</span>
            <asp:Label ID="lblPrimaryContactPhone" CssClass="formFieldDisplay" runat="server" />
        </td>
        <td>
            <span class="formLabel170">Contact Fax</span>
            <asp:Label ID="lblPrimaryContactFax" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>
            <span class="formLabel170">Prime Practice State</span>
            <asp:Label ID="lblPrimaryPracticeState" CssClass="formFieldDisplay" runat="server" />
        </td>
    </tr>
</table>
