<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_PaperRequestDocumentType" Codebehind="PaperRequestDocumentType.ascx.cs" %>
<style>
    caption {
        visibility: hidden !important
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Paper Request Document Type"></asp:Literal>
    </p>

</asp:Panel>

<telerik:radgrid id="rgPaperRequestDocumentType" runat="server" mastertableview-caption="PaperRequestDocumentType" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgPaperRequestDocumentType_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgPaperRequestDocumentType_InsertCommand" onupdatecommand="rgPaperRequestDocumentType_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="PAPER_REQUEST_DOCUMENT_TYPE_ID">
        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="PAPER_REQUEST_DOCUMENT_TYPE_NAME" headertext="Paper Request Document Type" uniquename="PAPER_REQUEST_DOCUMENT_TYPE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION" headertext="Paper Request Document Type Description" uniquename="PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE" headertext="Paper Request Document Type On Base Code" uniquename="PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Paper Request Document Type Name</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="255" aria-label="PaperRequestDocumentTypeName" ID="txtPaperRequestDocumentTypeName" runat="server" Text='<%# Bind("PAPER_REQUEST_DOCUMENT_TYPE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtPaperRequestDocumentTypeName"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-3 text-right">Paper Request Document Type Description</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="500" aria-label="PaperRequestDocumentTypeDescription" ID="txtPaperRequestDocumentTypeDescription" runat="server" Text='<%# Bind("PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION") %>' CssClass="formFieldMultiline">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Paper Request Document Type OnBase Code</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="120" ID="txtPaperRequestOnBaseCode" aria-label="PaperRequestOnBaseCode" runat="server" Text='<%# Bind("PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row text-center">
                            <%-- <td colspan="2" align="center">--%>
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                CommandName="Cancel"></asp:Button>
                            <%-- </td>--%>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
