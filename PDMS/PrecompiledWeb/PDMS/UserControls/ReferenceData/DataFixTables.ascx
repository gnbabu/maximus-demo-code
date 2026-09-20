<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_DataFixTables, App_Web_iq0r534d" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center">
        <asp:Literal ID="pagelabel" runat="server" Text="Data Management: Data Fix Tables"></asp:Literal>
    </p>
</asp:Panel>

<telerik:radgrid id="rgDataFixTables" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgDataFixTables_NeedDataSource" allowfilteringbycolumn="True" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgDataFixTables_InsertCommand" onupdatecommand="rgDataFixTables_UpdateCommand" Ondeletecommand="rgDataFixTables_DeleteCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False" AllowAutomaticDeletes="False">
    <clientsettings>

        <scrolling allowscroll="True" scrollheight="" usestaticheaders="True" savescrollposition="true"></scrolling>

    </clientsettings>
    <mastertableview commanditemdisplay="Top" gridlines="None" DataKeyNames="ID">
        <columns>
            <telerik:grideditcommandcolumn headertext="<span style='display:none'>Edit</span>">
            </telerik:grideditcommandcolumn>
            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this item?"
                CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" HeaderText="Delete">
            </telerik:GridButtonColumn>
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
                            <div class="col-sm-3 text-right">Table Name : </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="150" ID="txtTableName" runat="server" Text='<%# Bind("TABLE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTableName" runat="server" ErrorMessage="* Table Name is required"
                                    ControlToValidate="txtTableName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Table Value : </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtTableValue" runat="server" Text='<%# Bind("TABLE_VALUE") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTableValue" runat="server" ErrorMessage="* Table Value is required"
                                    ControlToValidate="txtTableValue"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Table Type : </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtTableType" runat="server" Text='<%# Bind("TABLE_TYPE") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTableType" runat="server" ErrorMessage="* Table Type is required"
                                    ControlToValidate="txtTableType"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Is Visible : </div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="ckbIsVisible" Checked='<%# Bind("IS_VISIBLE") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Primary Key Column : </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtPkColumn" runat="server" Text='<%# Bind("PK_COLUMN") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPKColumn" runat="server" ErrorMessage="* Primary Key Column is required"
                                    ControlToValidate="txtPkColumn"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Columns To Hide : </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtColumnsHide" runat="server" Text='<%# Bind("COLUMNS_HIDE") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Read Only Column : </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReadonlyColumns" runat="server" Text='<%# Bind("READONLY_COLUMNS") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row text-center">
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                CommandName="Cancel"></asp:Button>
                            <%--</td>--%>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
