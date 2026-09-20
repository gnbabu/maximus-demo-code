<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_ProviderTypeFee" Codebehind="ProviderTypeFee.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style>
    caption {
        visibility: hidden !important
    }
</style>
<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_ProviderTypeFee_rgProviderTypeFee_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Provider Type Fee"></asp:Literal>
    </p>
</asp:Panel>

<telerik:radgrid id="rgProviderTypeFee" runat="server" rendermode="Lightweight" mastertableview-caption="ProviderTypeFee" allowpaging="True" allowsorting="True"
    onneeddatasource="rgProviderTypeFee_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgProviderTypeFee_InsertCommand" onupdatecommand="rgProviderTypeFee_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False" onitemdatabound="rgProviderTypeFee_ItemDataBound">
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="PROVIDER_TYPE_FEE_ID">
        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="PROVIDER_TYPE_NAME" headertext="Provider Type" uniquename="PROVIDER_TYPE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="IS_FEE_REQUIRED" headertext="Required" uniquename="IS_REQUIRED" datatype="System.Boolean">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="FEE_AMOUNT" headertext="Fee Amount" uniquename="FEE_AMOUNT" datatype="System.Decimal">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="ENTITY_TYPE_NAME" headertext="Entity Type Name" uniquename="ENTITY_TYPE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="APPLICATION_TYPE_NAME" headertext="Application Type Name" uniquename="APPLICATION_TYPE_NAME">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlProviderType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Provider Type is required"
                                    ControlToValidate="ddlProviderType"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Fee Required</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkFeeRequired" Checked='<%# Bind("IS_FEE_REQUIRED") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Fee Amount</div>
                            <div class="col-sm-9 text-left">
                                <telerik:radnumerictextbox rendermode="Lightweight" runat="server" datatype="System.Decimal"
                                    dbvalue='<%# Bind("FEE_AMOUNT") %>' type="Currency" id="txtFeeAmount" cssclass="formField"
                                    maxvalue="99999.99" minvalue="0" width="450px">
                                </telerik:radnumerictextbox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Entity Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlEntityType" aria-label="EntityType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Entity Type is required"
                                    ControlToValidate="ddlEntityType"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Application Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlApplicationType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="row text-center">
                            <%--                               <td colspan="2" align="center">--%>
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                CommandName="Cancel"></asp:Button>
                            <%--  </td>--%>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
