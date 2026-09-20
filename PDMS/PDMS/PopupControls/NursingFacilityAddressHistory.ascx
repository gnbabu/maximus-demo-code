<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_NursingFacilityAddressHistory" Codebehind="NursingFacilityAddressHistory.ascx.cs" %>
<div id ="divHeader" style="width: 100%;">
    <table style="background-color: #666666; width: 100%; text-align: left;">
        <tr style="width: 100%;">
            <td class="HistoryHeaderRow">Name</td>
            <td class="HistoryHeaderRow">Address</td>
            <td class="HistoryHeaderRow">Email Address</td>
            <td class="HistoryHeaderRow">Phone Number</td>
            <td class="HistoryHeaderRow">User Name</td>
            <td class="HistoryHeaderRow">Location Name</td>
            <td class="HistoryHeaderRow">Update Date</td>
        </tr>
    </table>
</div>
<div id="DataDiv" style="overflow-y: scroll;overflow-x: hidden;  width: 100%; height: 400px;">
    <asp:GridView runat="server" Width="100%" ID="grd" AutoGenerateColumns="False" GridLines="Both" ShowHeader="False" AllowPaging="False" HorizontalAlign="Left"
                  EmptyDataText="No entries found." AllowSorting="True" >
        <Columns>
            <asp:BoundField DataField="PRACTICE_NAME" ItemStyle-CssClass="HistoryItemStyle" />
            <asp:BoundField DataField="ADDRESS1" ItemStyle-CssClass="HistoryItemStyle" />
            <asp:BoundField DataField="EMAIL1" ItemStyle-CssClass="HistoryItemStyle" />
            <asp:BoundField DataField="PHONE1" ItemStyle-CssClass="HistoryItemStyle" />
            <asp:BoundField DataField="UserName" ItemStyle-CssClass="HistoryItemStyle" />
            <asp:BoundField DataField="LOCATION_TYPE_NAME" ItemStyle-CssClass="HistoryItemStyle" />
            <asp:BoundField DataField="UpdatedDate" DataFormatString="{0:d}" ItemStyle-CssClass="HistoryItemStyle" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
