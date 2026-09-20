<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SpecialtiesTaxonomiesHistory, App_Web_rqhgepvh" %>

<br />
<asp:GridView runat="server" Width="98%" ID="grdSpecialtiesTaxonomiesHistory" AutoGenerateColumns="False" HorizontalAlign="Left" 
    CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <%-- <asp:BoundField DataField="Index" HeaderText="Index" />--%>
        <asp:BoundField DataField="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
            HeaderText="Last Modified Date" />
        <asp:BoundField DataField="Operation" HeaderText="Operation" />
        <asp:BoundField DataField="SPECIALTY_TYPE_NAME" HeaderText="Specialty Type" />
        <asp:BoundField DataField="TAXONOMY_CODE" HeaderText="Taxonomy" />
        <asp:BoundField DataField="UserName" HeaderText="Last Modified User" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
