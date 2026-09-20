<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospitalAddressHistory, App_Web_l5y5araq" %>

<div id="DataDiv" style="overflow-y: scroll;overflow-x: hidden;  width: 100%; height: 400px;">
    <asp:GridView runat="server" Width="98%"  ID="grdHospitalAddressHistory" AutoGenerateColumns="False" HorizontalAlign="Left"  EmptyDataText="No entries found." CssClass="gridview"
      AllowPaging="true" AllowSorting="True" PageSize="10"
        OnPageIndexChanging="grdHospitalAddressHistory_PageIndexChanging" OnSorting="grdHospitalAddressHistory_Sorting">
        <Columns>
            <asp:BoundField DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
            <asp:BoundField DataField="PRACTICE_NAME"       HeaderText="Name"           SortExpression="PRACTICE_NAME" />
            <asp:BoundField DataField="LOCATION_TYPE_NAME"  HeaderText="Location Type"  SortExpression="LOCATION_TYPE_NAME" />
            <asp:BoundField DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
            <asp:BoundField DataField="EMAIL1"              HeaderText="Email Address"  SortExpression="EMAIL1" />
            <asp:BoundField DataField="PHONE1"              HeaderText="Phone Number"   SortExpression="PHONE1" />
            <asp:BoundField DataField="UserName"            HeaderText="User"           SortExpression="UserName" />
            <asp:BoundField DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>

