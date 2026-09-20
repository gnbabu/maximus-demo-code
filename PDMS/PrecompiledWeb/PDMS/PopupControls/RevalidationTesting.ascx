<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_RevalidationTesting, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="pnlRevalidation" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;" GroupingText="Revalidation Testing">
    <div>
        <asp:Label ID="lblRevalidationRegId" runat="server" Text="Reg Id"></asp:Label>
        <asp:TextBox ID="txtRevalidationRegId" runat="server" MaxLength="20" aria-Label="Revalidation Reg ID"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfv_txtRevalidationRegId" runat="server" ControlToValidate="txtRevalidationRegId" 
            ErrorMessage="Required" ForeColor="Red" Display="Dynamic" ValidationGroup="revalidation"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="rev_txtRevalidationRegId" runat="server" ControlToValidate="txtRevalidationRegId"
            ForeColor="Red" Display="Dynamic" ValidationGroup="revalidation" 
            ErrorMessage="Invalid" ValidationExpression="[0-9]*$"></asp:RegularExpressionValidator>
        <br />
        <asp:Label ID="lblRevalidationEndDate" runat="server" Text="Change End Date to"></asp:Label>
        <asp:TextBox ID="txtRevalidationEndDate" runat="server" MaxLength="10" aria-Label="Revalidation End Date"></asp:TextBox>
            <cc1:CalendarExtender ID="calEndDate" TargetControlID="txtRevalidationEndDate" runat="server" />
        <asp:RequiredFieldValidator ID="rfv_txtRevalidationEndDate" runat="server" ControlToValidate="txtRevalidationEndDate" 
            ErrorMessage="Required" ForeColor="Red" Display="Dynamic" ValidationGroup="revalidation"></asp:RequiredFieldValidator>            
        <br />
        <br />            
        <asp:Button ID="btnUpdateEndDate" runat="server" Text="Update End Date" CssClass="buttonBox" 
            OnClick="btnUpdateEndDate_Click" ValidationGroup="revalidation" CauseValidation="true" />
    </div>
    <div>
        <hr />
    </div>
    <div>
        <asp:Label ID="lblAppSettingKey" runat="server" Text=""></asp:Label><br />
        <asp:TextBox ID="txtAppSettingKey" runat="server" MaxLength="60" aria-Label="Key Name"></asp:TextBox>
        <asp:Button ID="btnAppSettingKey" runat="server" Text="Refresh" CssClass="buttonBoxFocus" OnClick="btnAppSettingKey_Click" />
    </div>
</asp:Panel>