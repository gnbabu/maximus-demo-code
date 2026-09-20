<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_DMEProductsAndServicesHistory, App_Web_rqhgepvh" %>

<br />
<asp:GridView runat="server" Width="98%" ID="gvProductsAndServicesHistory" AutoGenerateColumns="False" HorizontalAlign="Left" 
    CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
         
        <asp:BoundField DataField="DME_PRODUCT_SERVICE_CATEGORY_TYPE_NAME" HeaderText="Product/Service Category" HtmlEncode="False" />
                <asp:BoundField DataField="DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_NAME" HeaderText="Product/Service SubCategories" />
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
