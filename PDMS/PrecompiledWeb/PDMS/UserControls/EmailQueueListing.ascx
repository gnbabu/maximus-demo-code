<%@ control language="C#" autoeventwireup="true" inherits="UserControls_EmailQueueListing, App_Web_p4ixifjm" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>

<div style="text-align: center;">
    <wc:GroupBox ID="TemplateHeaderGroup" Caption="Emails in Queue to be sent" CaptionStyle-CssClass="bodyTextBold" runat="server">
            <%-- WHATS IN THE QUEUE --%>
            <asp:GridView runat="server" ID="grdQueueItems" AllowPaging="True" AllowSorting="True" 
                          AutoGenerateColumns="false" CellPadding="3" CssClass="RadGrid_PDMSModern" OnPageIndexChanging="grdQueueItems_PageIndexChanging">

                <HeaderStyle Font-Size="10pt" />
                <PagerStyle Font-Size="10pt" />
                <RowStyle Font-Size="10pt" ForeColor="Black" />
                
                <Columns>
                    <asp:BoundField DataField="EmailQueueId" HeaderText="Id"  />
                    <asp:BoundField DataField="TemplateId" HeaderText="TPL Id"  />
                    <asp:BoundField DataField="BatchId" HeaderText="Batch"  />
                    <asp:BoundField DataField="NPI" HeaderText="NPI"  />
                    <asp:BoundField DataField="TaxId" HeaderText="Tax Id"  />
                    <asp:BoundField DataField="RegId" HeaderText="Reg Id" />
                    <asp:BoundField DataField="EmailAddresses" HeaderText="Email Addresses" />
                    <asp:BoundField DataField="EmailSubject" HeaderText="Subject" />
                </Columns>
            </asp:GridView>
    </wc:GroupBox>
    <div style="padding:5px;">
        <asp:Button CssClass="buttonBox" BackColor="#036"  runat="server" ID="btnSendBulkEmailNow" Text="Send Bulk Email Now" Visible="false" OnClick="btnSendBulkEmailNow_Click" />
        <asp:Button CssClass="buttonBox" BackColor="#036" runat="server" ID="btnSendBulkEmailInBatch" Text="Send Bulk Email in Batch" Visible="false" OnClick="btnSendBulkEmailInBatch_Click" />
    </div>
    
</div>


