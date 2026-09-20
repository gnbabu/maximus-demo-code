<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_ApplicationType" Codebehind="ApplicationType.ascx.cs" %>
<style>
    caption {
        visibility: hidden !important
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Application Type"></asp:Literal>
    </p>
</asp:Panel>

<telerik:radgrid id="rgApplicationType" runat="server" rendermode="Lightweight" mastertableview-caption="Application type" allowpaging="True" allowsorting="True"
    onneeddatasource="rgApplicationType_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgApplicationType_InsertCommand" onupdatecommand="rgApplicationType_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False" onitemcommand="rgApplicationType_ItemCommand">
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="APPLICATION_TYPE_ID">
        <columns>
            <telerik:grideditcommandcolumn headertext="<span style='display:none'>edit</span>">
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="APPLICATION_TYPE_NAME" headertext="Provider Type Abbreviation" uniquename="ProviderTypeAbbreviation">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="APPLICATION_TYPE_DESC" headertext="Provider Type Name" uniquename="ProviderTypeName">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="IS_USED_IN_MMIS" headertext="Used in MMIS" uniquename="UsedInMMIS" datatype="System.Boolean">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="MMIS_APPLICATION_TYPE_ID" headertext="MMIS Application Type ID" uniquename="MMISApplicationTypeID">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="IsVisible" headertext="Is Visible" uniquename="Visible" datatype="System.Boolean">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Application Type Name </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="50" ID="txtApplicationTypeName" aria-label="Applicationtype" runat="server" Text='<%# Bind("APPLICATION_TYPE_NAME") %>' CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtApplicationTypeName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Application Type Description </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="800" ID="txtApplicationTypeDesc" aria-label="Application type desc" runat="server" Text='<%# Bind("APPLICATION_TYPE_DESC") %>' CssClass="formFieldMultiline">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <asp:Label ID="checkbox" Text="Used in MMIS" runat="server" AssociatedControlID="chkIsUsedInMMIS" Style="font-weight: 100"></asp:Label>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkIsUsedInMMIS" Checked='<%# Eval("IS_USED_IN_MMIS") == DBNull.Value ? false :  Eval("IS_USED_IN_MMIS") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">MMIS Application Type ID</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="20" ID="txtMMISApplicationTypeID" runat="server" Text='<%# Bind("MMIS_APPLICATION_TYPE_ID") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <asp:Label ID="Label1" Text="Visible" runat="server" AssociatedControlID="chkIsVisible" Style="font-weight: 100"></asp:Label>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkIsVisible" Checked='<%# Eval("IsVisible") == DBNull.Value ? false :  Eval("IsVisible") %>' />
                            </div>
                        </div>
                        <div class="row text-center">
                            <%--<td colspan="2" align="center">--%>
                            <asp:Button ID="btnUpdate" Text='<%# (Container is Telerik.Web.UI.GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is Telerik.Web.UI.GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
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
