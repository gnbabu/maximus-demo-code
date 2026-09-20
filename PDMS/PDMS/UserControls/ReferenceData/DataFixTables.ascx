<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_DataFixTables" Codebehind="DataFixTables.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<style>
    .formDropDown {
        font-size: 17px;
        height: 44px;
        color: #000;
    }

    .formField {
        height: 44px;
        color: #000;
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Data Management: Data Fix Tables"></asp:Literal>
    </p>
    <br />
</asp:Panel>

<telerik:radgrid id="rgDataFixTables" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgDataFixTables_NeedDataSource" allowfilteringbycolumn="True" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgDataFixTables_InsertCommand" onupdatecommand="rgDataFixTables_UpdateCommand" ondeletecommand="rgDataFixTables_DeleteCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False" allowautomaticdeletes="False">
    <clientsettings>

        <scrolling allowscroll="True" scrollheight="" usestaticheaders="True" savescrollposition="true"></scrolling>

    </clientsettings>
    <mastertableview commanditemdisplay="Top" gridlines="None" datakeynames="ID">
        <columns>
            <telerik:grideditcommandcolumn headertext="<span style='display:none'>Edit</span>">
            </telerik:grideditcommandcolumn>
            <telerik:gridbuttoncolumn confirmtext="Are you sure you want to delete this item?"
                commandname="Delete" text="Delete" uniquename="DeleteColumn" headertext="Delete">
            </telerik:gridbuttoncolumn>
            <telerik:gridboundcolumn datafield="TABLE_NAME" headertext="Table Name" uniquename="TABLE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="TABLE_VALUE" headertext="Table Value" uniquename="TABLE_VALUE">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="TABLE_TYPE" headertext="Table Type" uniquename="TABLE_TYPE">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="IS_VISIBLE" headertext="Table Visibility" uniquename="IS_VISIBLE">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PK_COLUMN" headertext="PrimaryKey Columns" uniquename="PK_COLUMN">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="COLUMNS_HIDE" headertext="Columns To Hide" uniquename="COLUMNS_HIDE">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="READONLY_COLUMNS" headertext="ReadOnly Columns" uniquename="READONLY_COLUMNS">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">Table Name : </span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="150" ID="txtTableName" runat="server" Text='<%# Bind("TABLE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTableName" runat="server" ErrorMessage="* Table Name is required"
                                    ControlToValidate="txtTableName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Table Value : </span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtTableValue" runat="server" Text='<%# Bind("TABLE_VALUE") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTableValue" runat="server" ErrorMessage="* Table Value is required"
                                    ControlToValidate="txtTableValue"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Table Type :  </span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtTableType" runat="server" Text='<%# Bind("TABLE_TYPE") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTableType" runat="server" ErrorMessage="* Table Type is required"
                                    ControlToValidate="txtTableType"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Is Visible : </span></div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="ckbIsVisible" Checked='<%# Bind("IS_VISIBLE") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Primary Key Column : </span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtPkColumn" runat="server" Text='<%# Bind("PK_COLUMN") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPKColumn" runat="server" ErrorMessage="* Primary Key Column is required"
                                    ControlToValidate="txtPkColumn"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Columns To Hide :  </span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtColumnsHide" runat="server" Text='<%# Bind("COLUMNS_HIDE") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Read Only Column : </span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReadonlyColumns" runat="server" Text='<%# Bind("READONLY_COLUMNS") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row" style="padding-top:20px;">
                            <div class="col-sm-3 text-right"><span class="ohio-field"></span></div>
                            <div class="col-sm-9 text-left">
                                <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                    CommandName="Cancel"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
