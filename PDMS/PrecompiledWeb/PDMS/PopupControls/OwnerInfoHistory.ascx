<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OwnerInfoHistory, App_Web_glma3lal" %>

<br />
<asp:UpdatePanel ID="upOwnerInfoHistory" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mltOwnerHistory" runat="server">
    <asp:View ID="vwOwnerInfoHistory" runat="server">
        <div style="height:400px; overflow-y:scroll">
<asp:GridView runat="server" Width="98%" ID="grdOwnerInfo" AutoGenerateColumns="False" HorizontalAlign="Left" 
    CssClass="gridview" EmptyDataText="No owner information found." AllowPaging = "true" PageSize = "20" 
    OnPageIndexChanging = "GrdOwnerInfo_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="OWNER_TYPE_NAME" HeaderText="Type" />
        <asp:BoundField DataField="NAME" HeaderText="Name" />
        <asp:BoundField DataField="DSC_Title" HeaderText="Title" />
        <asp:BoundField DataField="PERCENTAGE_OF_OWNERSHIP" HeaderText="Percentage" />
        <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
        <asp:BoundField DataField="DateofAction" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
            HeaderText="Last Modified Date" />       
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
        </div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
</asp:UpdatePanel>
