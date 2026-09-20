<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_UploadDocument" Codebehind="UploadDocument.ascx.cs" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<style type="text/css">
    .fileControl {
        display:inline !important;
    }
</style>
<asp:Label ID="DMELabel" font-size="10pt" CssClass="bodyTextBold" runat="server" Visible="false" Text="Upload  Licenses and Certifications for all Products and Services" ForeColor="Black" />
<br />
<br />
<div class="pg-hint4">
    <p>Please note that you will not be able to delete uploaded documents once your application has been submitted.</p>
</div>

<center>
    <asp:updatePanel ID="pnlUpdUploadDocs" runat="server">
<ContentTemplate>
    <asp:GridView ID="gvUploadedDocs" runat="server" AutoGenerateColumns="False" Title="Upload Document"
        HorizontalAlign="Center" Width="100%"  ShowHeaderWhenEmpty="true"
            CssClass="gridViewSmallFont" EmptyDataText="No uploaded documents found." 
        onrowdeleting="gvUploadedDocs_RowDeleting" 
        onrowcommand="gvUploadedDocs_RowCommand" 
        onrowdatabound="gvUploadedDocs_RowDataBound" 
        onrowediting="gvUploadedDocs_RowEditing">
        <Columns>
            <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
            <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
            <asp:BoundField DataField="FILE_NAME" HeaderText="File Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
            <asp:BoundField DataField="Document_Upload_Page" HeaderText="Page Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
            <asp:BoundField DataField="Username" HeaderText="Username" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
            <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="RoleName" HeaderText="Role" Visible="false" /> 
            <asp:BoundField DataField="ONBASE_DOCUMENT_ID" HeaderText="ONBASE_DOCUMENT_ID" Visible="false" /> 
            <asp:BoundField DataField="IS_CONVERSION" HeaderText="IS_CONVERSION" Visible="false" /> 
            <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" ShowHeader="true">
                <ItemTemplate>
                    <asp:ImageButton ID="imgView" ImageUrl="~/Images/search.png" alt="Search Button" runat="server" ToolTip="View" CommandName="Edit" />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ShowHeader="true">
                <ItemTemplate>
                    <asp:ImageButton ID="imgCancel" ImageUrl="~/Images/cancel.png" AlternateText="Delete" runat="server" CommandName="Delete" ToolTip="Delete" 
                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DOCUMENT_ID") %>' />
                </ItemTemplate> 
            </asp:TemplateField>
        </Columns>   
        <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
        <HeaderStyle CssClass="gridViewHeader" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" /> 
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    </ContentTemplate></asp:updatePanel>
    <br />
    <div id="divupload" runat="server">
        <div class="row">
            <div class="col-sm-3"></div>
            <div class ="col-sm-9 text-left">
                <mms:EncryptedFileUpload runat="server" ID="filUploadFile" aria-label="Fileupload" Width="400px" ViewStateMode="Enabled" CssClass="fileControl"/>
                <!--<asp:FileUpload id="filUploadFile1" runat="server" Width="400px" size="100" />-->
            </div>
        </div>
        <%--<tr><td colspan="2">&nbsp;</td></tr>--%>
        <div class="row">
            <div class="col-sm-12 text-center"><asp:ValidationSummary ID="vsUpdateDocument" runat="server" DisplayMode="SingleParagraph" ValidationGroup="valUpdateDocument" /></div>
        </div>
        <div class="row">
            <div id="colname1" class="col-sm-3 text-right"><span class="formLabel200">Name</span></div>
            <div id="colname2" class="col-sm-9 text-left">
                <asp:TextBox ID="txtName" runat="server" aria-label="FileName" CssClass="formField wd400" MaxLength="100" />
                <%--Consolidated List - DR60: Name not required. If not entered, use file name.--%>
                <%--<asp:RequiredFieldValidator runat="server" ID="reqName" ValidationGroup="valProviderInfoHeader"
                    ControlToValidate="txtName" ErrorMessage="*Enter Name" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" />--%>               
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">Description</span></div>
            <div class="col-sm-9 text-left">
                <asp:TextBox ID="txtDescription" aria-label="FileDescription" runat="server" Rows="5" CssClass="formFieldMultiline wd400" TextMode="MultiLine" MaxLength="500" />
            </div>
        </div>
    </div>
    <br />
    <asp:Button id="UploadButton" Text="Upload file" OnClick="UploadButton_Click" runat="server" CssClass="buttonBox"
        ValidationGroup="valUpdateDocument" /> 
    <p><b><asp:Label ID="lblStatusMsg" runat="server" /></b></p>  
</center>
