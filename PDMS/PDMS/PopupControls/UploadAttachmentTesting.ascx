<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_UploadAttachmentTesting" Codebehind="UploadAttachmentTesting.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style type="text/css">
    select {
        min-width: 90%;
    }
</style>

<br />
<div style="border-top: 2px solid black; margin-bottom: -10px">
    <br />
</div>

<asp:Panel ID="pnlPATesting" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlPATesting" id="Span1" runat="server"><b>Upload Attachment Status</b></span>
        <hr />
    </div>
    <div class="container-fluid">
        <div class="row">
            
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblmemberID" CssClass="ohio-field" AssociatedControlID="txtMemberID" runat="server">
                    <span class="ohio-field-label">Member ID: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the Transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtMemberID" CssClass="ohio-field-input" runat="server" />
                </asp:Label>
            </div>
        </div>
                <div class="row">
            
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblProviderID" CssClass="ohio-field" AssociatedControlID="txtProviderID" runat="server">
                    <span class="ohio-field-label">Provider ID: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the Transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtProviderID" CssClass="ohio-field-input" runat="server" />
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="divUploadAttachments" class="RetrieveReportsSearchHeader" runat="server" visible="false">Upload Attachment Status</div>
    <asp:GridView
        ID="gvUploadAttachmentStatus"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Records Found"
        RowStyle-VerticalAlign="Top"
        AllowPaging="true"
        AllowCustomPaging="true"
        PageSize="5"
        GridViewSortDirection="Descending"
        DataKeyNames="OutBound_Document_Uploads_ID, UploadType">
        <Columns>
            <asp:BoundField DataField="Member_ID" HeaderText="Member ID" />
            <asp:BoundField DataField="provider_id" HeaderText="Provider id" />
            <asp:BoundField DataField="OriginalDocumentName" HeaderText="Original Document Name" />
            <asp:BoundField DataField="Uploaded to S3" HeaderText="Uploaded to S3" />
            <asp:BoundField DataField="Documentname" HeaderText="Document Name" />
            <asp:BoundField DataField="Zip file created" HeaderText="Zip file created" />
        </Columns>
    </asp:GridView>
    <asp:HiddenField ID="hdnRowCount" runat="server" />
</asp:Panel>
<asp:Panel ID="Panel1" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="container-fluid">

        <div class="row" style="padding-left: 1in">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <div class="row">
                        <asp:Button ID="btnGetUploadStatus" runat="server" Text="Get Upload Status" OnClick="btnGetUploadStatus_Click" CssClass="buttonBoxFocus" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGetUploadStatus" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
