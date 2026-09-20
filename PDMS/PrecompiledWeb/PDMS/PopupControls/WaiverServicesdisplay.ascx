<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_WaiverServicesdisplay, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>

<asp:Panel ID="pnlActiveServiceSpan" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
    <div class="container">
        <uc1:Separator ID="Separator12" runat="server" Header="DODD Waiver" />
        <asp:GridView ID="DODDWaiverServiceGrid" runat="server" Width="98%" AutoGenerateColumns="False" OnDataBound="DODDWaiverServiceGrid_DataBound" EmptyDataText="No Contracts details available.">
            <Columns>
                <asp:BoundField DataField="SERVICE_NAME" HeaderText="Service" SortExpression="Service"></asp:BoundField>
                <asp:BoundField DataField="START_DATE" HeaderText="Effective Date" SortExpression="Effective Reason" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                <asp:BoundField DataField="END_DATE" HeaderText="Expiration Date" SortExpression="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        <br />
    </div>
    <div class="container">
        <uc1:Separator ID="Separator2" runat="server" Header="ODA Assisted Living Waiver" />
        <asp:GridView ID="ODAAssistedWaiverServiceGrid" runat="server" Width="98%" AutoGenerateColumns="False" OnDataBound="ODAAssistedWaiverServiceGrid_DataBound" EmptyDataText="No Contracts details available.">
            <Columns>
                <asp:BoundField DataField="SERVICE_NAME" HeaderText="Service" SortExpression="Service"></asp:BoundField>
                <asp:BoundField DataField="START_DATE" HeaderText="Effective Date" SortExpression="Effective Reason" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                <asp:BoundField DataField="END_DATE" HeaderText="Expiration Date" SortExpression="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        <br />
    </div>
    <div class="container">
        <uc1:Separator ID="Separator3" runat="server" Header="ODA Passport Waiver" />
        <asp:GridView ID="ODAPassportWaiverServiceGrid" runat="server" Width="98%" AutoGenerateColumns="False" OnDataBound="ODAPassportWaiverServiceGrid_DataBound" EmptyDataText="No Contracts details available.">
            <Columns>
                <asp:BoundField DataField="SERVICE_NAME" HeaderText="Service" SortExpression="Service"></asp:BoundField>
                <asp:BoundField DataField="START_DATE" HeaderText="Effective Date" SortExpression="Effective Reason" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                <asp:BoundField DataField="END_DATE" HeaderText="Expiration Date" SortExpression="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        <br />
    </div>
    <div class="container">
        <uc1:Separator ID="Separator4" runat="server" Header="ODA Transportation Waiver" />
        <asp:GridView ID="ODAChoicesGrid" runat="server" Width="98%" AutoGenerateColumns="False" OnDataBound="ODAChoicesGrid_DataBound" EmptyDataText="No Contracts details available.">
            <Columns>
                <asp:BoundField DataField="SERVICE_NAME" HeaderText="Service" SortExpression="Service"></asp:BoundField>
                <asp:BoundField DataField="START_DATE" HeaderText="Effective Date" SortExpression="Effective Reason" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                <asp:BoundField DataField="END_DATE" HeaderText="Expiration Date" SortExpression="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        <br />
    </div>
    <div class="container">
        <uc1:Separator ID="Separator5" runat="server" Header="ODA Other Waiver" />
        <asp:GridView ID="ODAOthers" runat="server" Width="98%" AutoGenerateColumns="False" DataBound="ODAOthers_DataBound" EmptyDataText="No Contracts details available.">
            <Columns>
                <asp:BoundField DataField="SERVICE_NAME" HeaderText="Service" SortExpression="Service"></asp:BoundField>
                <asp:BoundField DataField="START_DATE" HeaderText="Effective Date" SortExpression="Effective Reason" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                <asp:BoundField DataField="END_DATE" HeaderText="Expiration Date" SortExpression="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        <br />
    </div>
</asp:Panel>