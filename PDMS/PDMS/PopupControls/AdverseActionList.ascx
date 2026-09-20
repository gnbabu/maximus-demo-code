<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_AdverseActionList" Codebehind="AdverseActionList.ascx.cs" %>
<br />
<div class="divGrid" style="display:inline-block;width:100%;">
    <asp:GridView runat="server" Width="98%" ID="grdAdverseActions" AutoGenerateColumns="False" HorizontalAlign="Left" 
        CssClass="gridview" EmptyDataText="No entries found.">
        <Columns>
            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
            <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
            <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                HeaderText="Last Modified Date" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
