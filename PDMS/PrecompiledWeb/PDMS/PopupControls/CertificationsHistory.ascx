<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CertificationsHistory, App_Web_c4une0e1" %>
<br />
<div>
    <asp:GridView runat="server" Width="98%" ID="grd" 					AutoGenerateColumns="False"  HorizontalAlign="Left" AllowPaging="True" AllowSorting="True" 
        PageSize="10"	CssClass="gridview"		OnPageIndexChanging="grd_PageIndexChanging" EmptyDataText="No entries found."  OnSorting="grd_Sorting">
        <Columns>
            <asp:BoundField DataField="Operation" HeaderText="Operation"    SortExpression="Operation" />
            <asp:BoundField DataField="DEA_NUMBER" HeaderText="DEA Number"  SortExpression="DEA_NUMBER" />
            <asp:BoundField DataField="UserName" HeaderText="User Name"     SortExpression="UserName" />
            <asp:BoundField DataField="DEA_STATE" HeaderText="DEA State"    SortExpression="DEA_STATE" />
            <asp:BoundField DataField="DEA_EFF_DATE" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}"   SortExpression="DEA_EFF_DATE" />
            <asp:BoundField DataField="DEA_END_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"  SortExpression="DEA_END_DATE" />
            <asp:BoundField DataField="DateOfAction" HeaderText="Update Date" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" SortExpression="DateOfAction" HtmlEncode="False" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
