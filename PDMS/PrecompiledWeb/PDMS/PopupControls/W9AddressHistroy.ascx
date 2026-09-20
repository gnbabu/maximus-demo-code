<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_W9AddressHistroy, App_Web_l5y5araq" %>
<br />
 <asp:GridView runat="server" Width="98%" ID="grdW9Address1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" 
             EmptyDataText="No W9 Address found."  AllowPaging="false" PageSize="1">
             <Columns>
                <asp:BoundField DataField="W9_ADDRESS1" HeaderText="Address 1"  />
                <asp:BoundField DataField="W9_ADDRESS2" HeaderText="Address 2"  />
                <asp:BoundField DataField="W9_CITY" HeaderText="City"  />
                <asp:BoundField DataField="W9_STATE" HeaderText="State" />
                <asp:BoundField DataField="W9_ZIP" HeaderText="Zip"  />
                <asp:BoundField DataField="W9_EXT_ZIP" HeaderText="Zip Ext"  />  
                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" SortExpression="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:d}" />       
             </Columns>
             <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>