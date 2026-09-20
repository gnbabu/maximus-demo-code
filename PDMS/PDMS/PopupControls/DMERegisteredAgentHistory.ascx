<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_DMERegisteredAgentHistory" Codebehind="DMERegisteredAgentHistory.ascx.cs" %>

<br />
<asp:GridView runat="server" Width="98%" ID="gvRegisteredAgentHistory" AutoGenerateColumns="False" HorizontalAlign="Left" 
    CssClass="gridview" EmptyDataText="No entries found." onrowdatabound="gvRegisteredAgentHistory_RowDataBound">
    <Columns>
         
        <asp:BoundField DataField="AGENT_NAME" HeaderText="Agent Name" />
        <asp:BoundField DataField="COMPANY_NAME" HeaderText="Company Name" />
        <asp:BoundField DataField="ADDRESS1" HeaderText="Address" />
        <asp:BoundField DataField="ADDRESS2" HeaderText="Address 2" />
        <asp:BoundField DataField="CITY" HeaderText="City" />
        <asp:BoundField DataField="STATE" HeaderText="State" />
        <asp:BoundField DataField="ZIP" HeaderText="Zip" />
        <asp:BoundField DataField="EXT_ZIP" HeaderText="Zip Ext" />
        <asp:BoundField DataField="COUNTY" HeaderText="County" />
        <asp:BoundField DataField="PHONE" HeaderText="Phone Number" />
        <asp:BoundField DataField="FAX" HeaderText="FAX" />
        <asp:BoundField DataField="WEBSITE" HeaderText="Website" />

        <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="EmailAddress" />

        <asp:BoundField DataField="UserName" HeaderText="Last Modified User" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
            HeaderText="Last Modified Date" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
