<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_SpecialtyType" Codebehind="SpecialtyType.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style>
    caption {
        visibility: hidden !important
    }
</style>
<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_SpecialtyType_rgSpecialtyType_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Specialty Type"></asp:Literal>
    </p>

</asp:Panel>

<telerik:radgrid id="rgSpecialtyType" runat="server" rendermode="Lightweight" mastertableview-caption="SpecialtyType" allowpaging="True" allowsorting="True"
    onneeddatasource="rgSpecialtyType_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgSpecialtyType_InsertCommand" onupdatecommand="rgSpecialtyType_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="SPECIALTY_TYPE_ID">
        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="SPECIALTY_TYPE_NAME" headertext="Specialty Type" uniquename="SPECIALTY_TYPE_NAME" readonly="true" />
            <telerik:gridboundcolumn datafield="MMIS_SPECIALTY_TYPE_ID" headertext="MMIS Specialty Type ID" uniquename="MMISSpecialtyTypeID" />
            <telerik:gridboundcolumn datafield="EXTERNAL_SPECIALTY_TYPE_NAME" headertext="External Specialty Type Name" uniquename="EXTERNAL_SPECIALTY_TYPE_NAME" />
            <telerik:gridboundcolumn datafield="IsVisible" headertext="IsActive" uniquename="IsVisible" />
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Specialty Type Name </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="256" ID="txtSpecialtyTypeName" aria-label="SpecialtyTypeName" runat="server" Text='<%# Bind("SPECIALTY_TYPE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtSpecialtyTypeName"></asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">MMIS Specialty Type ID</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="5" ID="txtMMISSpecialtyTypeID" aria-label="MMISSpecialtyTypeID" runat="server" Text='<%# Bind("MMIS_SPECIALTY_TYPE_ID") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">External Specialty Type Name</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="256" ID="txtExternalSpecialtyTypeId" aria-label="ExternalSpecialtyTypeId" runat="server" Text='<%# Bind("EXTERNAL_SPECIALTY_TYPE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Is Active</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkIsVisible" Checked='<%# Bind("IsVisible") %>' />
                            </div>
                        </div>
                        <div class="row text-center">
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                CommandName="Cancel" CssClass="buttonBox"></asp:Button>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
