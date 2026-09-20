<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_DataFix_DelegateAffiliationUploads" Codebehind="DelegateAffiliationUploads.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>

<style>
    caption {
        visibility: hidden !important
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center; margin-right: 40%">
        <asp:Literal ID="pagelabel" runat="server" Text="ReProcess Delegate Affiliate File"></asp:Literal>
    </p>
</asp:Panel>
<div>
    <asp:ValidationSummary ID="valSummaryDataFix2" runat="server" DisplayMode="List" ValidationGroup="DataFixDelegates" CssClass="failureNotification" />
</div>
<fieldset>
    <legend></legend>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2">
            <span class="formLabel150">
                <asp:Label ID="lblTaskID" runat="server" AssociatedControlID="txtDelegateAffiliationFileID" Text="Delegate Affiliation ID : " />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:TextBox ID="txtDelegateAffiliationFileID" runat="server" CssClass="textEntry" OnTextChanged="txtDelegateAffiliationFileID_TextChanged" Style="width: 100%; min-width: 100%" />
            <asp:RegularExpressionValidator ID="valDelegateAffiliationFileID" runat="server" ControlToValidate="txtDelegateAffiliationFileID"
                ValidationExpression="\d{0,10}" ErrorMessage="* Enter a valid Delegate Affiliation File ID Number"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="DataFix" Display="Dynamic" ForeColor="Red" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2">
            <span class="formLabel150">
                <asp:Label ID="Label3" runat="server" AssociatedControlID="txtDelegateAffiliationFileID" Text="ReProcess Only Staging : " />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:CheckBox ID="chkReProcessOnlyStagingID" runat="server" Checked="false" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2">
            <span class="formLabel150">
                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtDelegateAffiliationFileID" Text="Delegate Update Upload File : " />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <mms:encryptedfileupload runat="server" id="efuUpdateUploadFileID" viewstatemode="Enabled" cssclass="fileControl" style="width: 100%; min-width: 100%" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2">
            <span class="formLabel150">
                <asp:Label ID="Label2" runat="server" AssociatedControlID="txtDelegateAffiliationFileID" Text="Delegate Update Response File : " />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <mms:encryptedfileupload runat="server" id="efuUpdateResponseFileID" viewstatemode="Enabled" cssclass="fileControl" style="width: 100%; min-width: 100%" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px; margin-left: 23%">
        <asp:Label ID="lblButtonResponse" Text="" runat="server"></asp:Label>
    </div>
    <div class="row" style="margin-top: 10px; margin-left: 2%">
        <asp:Button ID="btnProcessID" runat="server" CausesValidation="true" Text="Process" OnClick="btnProcess_Click" CssClass="buttonBoxFocus" ValidationGroup="DataFix" Width="15%" />
        <asp:Button ID="btnUpdateUploadFileID" runat="server" CausesValidation="true" Text="Update Upload File" OnClick="btnUpdateUploadFile_Click" CssClass="buttonBoxFocus" ValidationGroup="DataFix" Width="15%" />
        <asp:Button ID="btnUpdateResponseFileID" runat="server" CausesValidation="true" Text="Update Response File" OnClick="btnUpdateResponseFile_Click" CssClass="buttonBoxFocus" ValidationGroup="DataFix" Width="15%" />
    </div>
</fieldset>
<fieldset>
    <legend></legend>
    <asp:GridView
        ID="gvDelegateAffiliationUploadsResults"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Records Found"
        RowStyle-VerticalAlign="Top"
        AllowPaging="true"
        AllowCustomPaging="true"
        PageSize="10"
        GridViewSortDirection="Descending" OnRowCommand="gvDelegateAffiliationUploadsResults_RowCommand"
        PageIndexChanged="gvDelegateAffiliationUploadsResults_PageIndexChanged"
        OnPageIndexChanging="gvDelegateAffiliationUploadsResults_PageIndexChanging"
        DataKeyNames="DELEGATE_DOCUMENT_UPLOAD_ID,DELEGATE_FILE_NAME,RESPONSE_DELEGATE_FILE_NAME">
        <Columns>
            <asp:BoundField DataField="DELEGATE_DOCUMENT_UPLOAD_ID" HeaderText="DELEGATE DOCUMENT UPLOAD ID" />
            <asp:BoundField DataField="UserName" HeaderText="USER NAME" />
            <asp:BoundField DataField="CONTACT_NAME" HeaderText="CONTACT NAME" />
            <asp:BoundField DataField="DOCUMENT_ID" HeaderText="DOCUMENT ID" />
            <asp:BoundField DataField="DELEGATE_FILE_NAME" HeaderText="DELEGATE FILE NAME" />
            <asp:TemplateField HeaderText="UPLOADED DELEGATE FILE NAME" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkRequestFile"
                        runat="server"
                        CausesValidation="false"
                        Text='<%# Eval("DELEGATE_FILE_NAME").ToString()!=""?"View Upload" : "" %>'
                        CommandName="upload"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CssClass="gridLink" Width="150" headertext="DELEGATE FILE NAME" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DELEGATE_FILE_DESCRIPTION" HeaderText="DELEGATE FILE DESCRIPTION" />
            <asp:BoundField DataField="STATUS" HeaderText="STATUS" />
            <asp:BoundField DataField="CREATED_ON_DATE_TIME" HeaderText="CREATED ON DATE TIME" />
            <asp:BoundField DataField="EMAIL_SENT" HeaderText="EMAIL SENT" />
            <asp:BoundField DataField="RESPONSE_DELEGATE_FILE_NAME" HeaderText="RESPONSE DELEGATE FILE NAME" />
            <asp:TemplateField HeaderText="RESPONSE DELEGATE FILE NAME" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkResponsePath"
                        runat="server"
                        CausesValidation="false"
                        Text='<%# Eval("RESPONSE_DELEGATE_FILE_NAME").ToString()!=""?"View Response" : "" %>'
                        CommandName="response"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CssClass="gridLink" Width="150" headertext="RESPONSE DELEGATE FILE NAME"
                        Visible='<%# Eval("RESPONSE_DELEGATE_FILE_NAME").ToString()==""?false : true %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RESPONSE_FILE_DOCUMENT_ID" HeaderText="RESPONSE FILE" />
            <asp:BoundField DataField="Error_Codes" HeaderText="ERROR CODES" />
        </Columns>
    </asp:GridView>
</fieldset>
