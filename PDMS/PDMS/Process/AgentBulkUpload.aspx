<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_AgentBulkUpload" MasterPageFile="~/MasterPage.master" Codebehind="AgentBulkUpload.aspx.cs" %>

<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ register src="~/PopupControls/UploadFilePT.ascx" tagprefix="ucUF" tagname="UploadFile" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HtmlHead" runat="server">
    <style type="text/css">
       
    </style>

</asp:Content>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    <h2>Bulk Agent Upload</h2>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="container my-4">
        <p class="mb-3">
            Field mapping and validations for the template upload are defined in the <strong>Reference</strong> tab in the template.
        </p>
        <div class="row"><br /></div>
        <div class="row mb-4">
            <div class="col-md-6 d-flex align-items-center gap-3">
                <span>Agent Bulk Upload Template:</span>
                <asp:LinkButton ID="lbTemplate" runat="server" runat="server" Text="Download File" OnClientClick="window.location.href='DownloadFile.aspx?fileName=Bulk_Upload_Agent_Template.xlsx'; return false;" CssClass="btn btn-link p-0">
                    <img src="../Images/Excel_24x24.png" class="ms-1" alt="Download bulk agent upload template" />
                </asp:LinkButton>
            </div>
            <div class="col-md-6 d-flex align-items-center gap-3">
                <span>Agent Role List:</span>
                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download File" OnClientClick="window.location.href='DownloadFile.aspx?fileName=Agent_Roles.xlsx'; return false;" CssClass="btn btn-link p-0">
                    <img src="../Images/Excel_24x24.png" class="ms-1" alt="Download agent role list" />
                </asp:LinkButton>
            </div>
        </div>
        <br /><br />
         <div class="row mb-4 align-items-center">
            <%-- <asp:Button ID="btnUploadFile" runat="server" CommandName="UploadFile" OnClick="btnUploadFile_Click" ToolTip="Upload File" Text="Upload File" CssClass="buttonBox" />--%>
             <ucuf:uploadfile id="ucUploadFile" runat="server" RedirectToPage="2" />
         </div>
     <%--   <div class="row mb-4 align-items-center">
            <div class="col-md-3">
                <label for="fuBulkAgent" class="form-label">Upload New File:</label>
            </div>
            <div class="col-md-4">
                <asp:FileUpload ID="fuBulkAgent" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblFileInfo" runat="server" Text="No file chosen" CssClass="text-danger small" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="UploadButton" Text="Upload file" OnClick="UploadButton_Click" runat="server" CssClass="btn btn-primary w-100" />
            </div>
        </div>--%>
           <br /><br />
         <asp:Label ID="lblErrorMsg" runat="server" CssClass="text-danger small" visible ="false" />
        <div class="mb-5">
            <h3>Bulk Agent Upload History</h3>
            <asp:GridView ID="gvAgentUploadHistory" Width="100%" runat="server" AllowSorting="true" OnSorting="gvAgentUploadHistory_SortCommand" CssClass="gridview" EmptyDataText="No Bulk Agent Uploads found"
                AutoGenerateColumns="false" HorizontalAlign="Left" OnRowCommand="gvAgentUploadHistory_RowCommand" AllowPaging="True" PageSize="10"
                OnPageIndexChanging="gvAgentUploadHistory_PageIndexChanging" OnRowDataBound="gvAgentUploadHistory_RowDataBound" ShowHeaderWhenEmpty="true"
                DataKeyNames="Id,Request_File,Response_File,ONBASE_DOCUMENT_ID,RESPONSE_FILE_ONBASE_DOC_ID">
                <columns>
                    <asp:BoundField DataField="File_Upload_Date" HeaderText="File Upload Date" SortExpression="File_Upload_Date" />
                    <asp:BoundField DataField="Request_File" HeaderText="File Name" />
                    <asp:BoundField DataField="File_Upload_Status" HeaderText="File Upload Status" SortExpression="File_Upload_Status" />
                    <asp:BoundField DataField="Id" HeaderText="Row ID" Visible="false" />
                    <asp:BoundField DataField="ONBASE_DOCUMENT_ID" HeaderText="OnBase Doc ID" Visible="false" />
                    <asp:BoundField DataField="RESPONSE_FILE_ONBASE_DOC_ID" HeaderText="Response OnBase Doc ID" Visible="false" />
                    <asp:TemplateField HeaderText="Upload" ItemStyle-HorizontalAlign="Center">
                        <itemtemplate>
                            <asp:LinkButton ID="lnkRequestFile"
                                runat="server"
                                CausesValidation="false"
                                Text="View Upload"
                                CommandName="upload"
                                CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                CssClass="gridLink" Width="150" HeaderText="Upload" />
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Response" ItemStyle-HorizontalAlign="Center">
                        <itemtemplate>
                            <asp:LinkButton ID="lnkResponsePath"
                                runat="server"
                                CausesValidation="false"
                                Text='<%# Eval("Response_File").ToString()!=""?"View Response" : "" %>'
                                CommandName="response"
                                CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                CssClass="gridLink" Width="150"
                                Visible='<%# Eval("Response_File").ToString()==""?false : true %>' />
                        </itemtemplate>
                    </asp:TemplateField>
                </columns>
                <pagerstyle cssclass="gridpager" horizontalalign="Right" />
                <headerstyle cssclass="gridViewHeader" width="100px" />
                <alternatingrowstyle cssclass="gridViewAltRow" />
                <rowstyle cssclass="gridViewRow" />
                <footerstyle cssclass="gridViewFooter" />
            </asp:GridView>
        </div>
        <div class="row"><br /><br /><br /><br /><br /><br /></div>
        <div class="mb-5">
            <h3>Bulk Agent Error Code Reference</h3>
            <table class="table table-striped table-bordered">
                <thead style="background-color:#545487; color:white;">
                    <tr>
                        <th>Error Code</th>
                        <th>Field(s)</th>
                        <th>Validation</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>BU1</td>
                        <td>Provider Administrator Id</td>
                        <td>Administrator Not Found In OH ID</td>
                    </tr>
                    <tr>
                        <td>BU2</td>
                        <td>Agent OH ID</td>
                        <td>Agent Not Found In OH ID</td>
                    </tr>
                    <tr>
                        <td>BU3</td>
                        <td>Medicaid ID</td>
                        <td>Administrator not assigned to Med ID</td>
                    </tr>
                    <tr>
                        <td>BU4</td>
                        <td>Granted Role to Agent</td>
                        <td>Agent Role does not exist</td>
                    </tr>
                    <tr>
                        <td>BU5</td>
                        <td></td>
                        <td>Provider Type/Role Mismatch</td>
                    </tr>
                    <tr>
                        <td>BU6</td>
                        <td>Agent OH ID</td>
                        <td>Agent OH ID is a provider admin</td>
                    </tr>
                     <tr>
                         <td>BU7</td>
                         <td>All fields are required</td>
                         <td>Required Data Elements are missing</td>
                     </tr>
                    <tr>
                        <td>BU8</td>
                        <td>Update Type</td>
                        <td>Update Type should be either A(Add) or D(Delete)</td>
                    </tr>
                    <tr>
                        <td>BU9</td>
                        <td>Agent Roles</td>
                        <td>"All Roles" is allowed only when Update Type is "D"</td>
                    </tr>
                    <tr>
                        <td>BU10</td>
                        <td>Agent Roles</td>
                        <td>Agent with this role for the medicaid id is a duplicate</td>
                     </tr>
                     <tr>
                         <td>BU11</td>
                         <td>Agent Roles</td>
                         <td>Agent with this role for the medicaid id does not exist to delete</td>
                      </tr>
                      <tr>
                        <td>BU12</td>
                        <td>Medicaid ID</td>
                        <td>You are not a Power Agent for the Provider Administrator of this Medicaid ID.</td>
                     </tr>
                </tbody>
            </table>
        </div>
    </div>

   <%-- <cc1:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy" backgroundcssclass="modalBackground" />
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 300px; min-width: 900px; height: auto; width: auto;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
        <div class="popTitle">
            <asp:Label ID="lblTitle" CssClass="bodyTextBold hospitalAffiliationsTitle" runat="server" Text="Title" />
        </div>
    </asp:Panel>
    <div style="text-align: left; padding: 15px" class="container-fluid">
        <div class="row">
            <ucuf:uploadfile id="ucUploadFile" runat="server" />
        </div>
    </div>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />--%>
<cc2:messagebox id="MessageBox2" runat="server" />
</asp:Content>
