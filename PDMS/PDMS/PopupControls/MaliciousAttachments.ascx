<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_MaliciousAttachments" Codebehind="MaliciousAttachments.ascx.cs" %>
<div class="divGrid" style="padding-top: 10px; padding-bottom: 40px;">
    <asp:GridView ID="gvMaliciousAttachments" runat="server" Width="100%" AllowSorting="false" 
        CssClass="gridview" EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" >
        <Columns>
        <asp:BoundField DataField="Member_Id" HeaderText="MemberId"  />
        <asp:BoundField DataField="OriginalDocumentName" HeaderText="Attachment"  />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Uploaded Date" />
        </Columns>
    </asp:GridView>
</div>