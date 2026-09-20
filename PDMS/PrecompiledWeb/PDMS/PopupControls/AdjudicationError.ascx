<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_AdjudicationError, App_Web_rqhgepvh" %>
<div class="divGrid" style="padding-top: 10px; padding-bottom: 40px;">
    <asp:GridView ID="gvAdjudicationErrorDetails" runat="server" Width="100%" AllowSorting="false" 
        CssClass="gridview" OnRowDataBound="gvServiceDetailDental_RowDataBound"
        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" >
        <Columns>
            <asp:TemplateField HeaderText="Service Line Number" ItemStyle-Width="20%">
                 <ItemTemplate>
                     <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("Service_Line") %>' ></asp:Label>
                 </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
		<HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
             </asp:TemplateField>          
            <asp:BoundField DataField="ErrorCode" HeaderText="ErrorCode"  ItemStyle-Width="20%" >
                                    <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
		<HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ErrorDescription" HeaderText="ErrorDescp"   ItemStyle-Width="60%" >
                                        <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
		<HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                    </asp:BoundField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
