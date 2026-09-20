<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_RegSectionUploadControl" Codebehind="RegSectionUploadControl.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style>
    caption {
        visibility: hidden !important
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">

    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Reg Section Upload Control"></asp:Literal>
    </p>

</asp:Panel>

<telerik:radgrid id="rgRegSectionUploadControl" rendermode="Lightweight" mastertableview-caption="RegSectionUploadControl" runat="server" allowpaging="True" allowsorting="True"
    onneeddatasource="rgRegSectionUploadControl_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgRegSectionUploadControl_InsertCommand" onupdatecommand="rgRegSectionUploadControl_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False" onitemdatabound="rgRegSectionUploadControl_ItemDataBound">
    <clientsettings>

        <scrolling allowscroll="True" scrollheight="" usestaticheaders="True" savescrollposition="true"></scrolling>

    </clientsettings>
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="REG_SECTION_UPLOAD_CONTROL_ID">

        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="APPLICATION_TYPE_NAME" headertext="Application Type" uniquename="APPLICATION_TYPE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PROVIDER_TYPE_NAME" headertext="Provider Type" uniquename="PROVIDER_TYPE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PROVIDER_CATEGORY_TYPE_NAME" headertext="Provider Category Type Name" uniquename="PROVIDER_CATEGORY_TYPE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REG_PAGE_NAME" headertext="Reg Page Type" uniquename="REG_PAGE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="TITLE" headertext="Title" uniquename="TITLE">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="DESCRIPTION" headertext="Description" uniquename="DESCRIPTION">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="IS_REQUIRED" headertext="Required" uniquename="IS_REQUIRED" datatype="System.Boolean">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REG_PAGE_SECTION" headertext="Reg Page Section" uniquename="REG_PAGE_SECTION">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REG_PAGE_NAME" headertext="Reg Page Name" uniquename="REG_PAGE_NAME">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Application Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlApplicationType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Category Type </div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlProviderCategoryType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlProviderType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Reg Page Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlRegPageType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Title</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="150" aria-label="Title" ID="txtTitle" runat="server" Text='<%# Bind("TITLE") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Description</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="500" ID="txtDescription" aria-label="Description" runat="server" Text='<%# Bind("DESCRIPTION") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Required</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkIsRequired" Checked='<%# Bind("IS_REQUIRED") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Reg Page Section</div>
                            <div class="col-sm-9 text-left">
                                <%-- <asp:TextBox MaxLength="150" ID="txtRegPageSection" runat="server" Text='<%# Bind("REG_PAGE_SECTION") %>' Width ="100%">
                                        </asp:TextBox>--%>
                                <asp:DropDownList ID="ddlRegPageSection" aria-label="RegPageSection" runat="server" CssClass="formDropDown"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Reg Page Name</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="150" ID="txtRegPageName" ari-label="RegPageName" runat="server" Text='<%# Bind("REG_PAGE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row text-center">
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                CommandName="Cancel"></asp:Button>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
s