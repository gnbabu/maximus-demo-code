<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OtherAddressHistory" Codebehind="OtherAddressHistory.ascx.cs" %>
<div id ="divHeader" style="width: 100%;">
    <table style="background-color: #666666; width: 100%; text-align: left;">
        <tr style="width: 100%;">
            <td class="HistoryHeaderRow">Name</td>
            <td class="HistoryHeaderRow">Address</td>
            <td class="HistoryHeaderRow">Email Address</td>
            <td class="HistoryHeaderRow">Phone Number</td>
            <td class="HistoryHeaderRow">User Name</td>
            <td class="HistoryHeaderRow">Update Date</td>
        </tr>
    </table>
</div>
<div id="DataDiv" style="overflow-y: scroll;overflow-x: hidden;  width: 100%; height: 400px;">
    <asp:GridView runat="server" Width="100%" ID="grdOtherAddressHistory" AutoGenerateColumns="False" GridLines="Both" ShowHeader="False" AllowPaging="True" HorizontalAlign="Left"
        EmptyDataText="No entries found." AllowSorting="True" PageSize="10"
        OnPageIndexChanging="grdOtherAddressHistory_PageIndexChanging" OnSorting="grdOtherAddressHistory_Sorting">
        <Columns>
            <asp:BoundField DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
            <asp:BoundField DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
            <asp:BoundField DataField="EMAIL1"              HeaderText="Email"          SortExpression="EMAIL1" />
            <asp:BoundField DataField="PHONE1"              HeaderText="Phone"          SortExpression="PHONE1" />
            <asp:BoundField DataField="ADR_EFFECTIVE_DATE"  HeaderText="Effective Date" SortExpression="ADR_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
            <asp:BoundField DataField="ADR_END_DATE"        HeaderText="End Date"       SortExpression="ADR_END_DATE"       DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
            <asp:BoundField DataField="UserName"            HeaderText="User Name"      SortExpression="UserName" />
            <asp:BoundField DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction"       DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
