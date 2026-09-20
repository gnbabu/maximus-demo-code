<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_ProviderCategoryType" Codebehind="ProviderCategoryType.ascx.cs" %>
<style>
    caption {
        visibility: hidden !important
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">

    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Provider Category Type"></asp:Literal>
    </p>

</asp:Panel>

<telerik:radgrid id="rgProviderCategoryType" runat="server" mastertableview-caption="ProviderCategoryType" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgProviderCategoryType_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgProviderCategoryType_InsertCommand" onupdatecommand="rgProviderCategoryType_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="PROVIDER_CATEGORY_TYPE_ID">
        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="PROVIDER_CATEGORY_TYPE_NAME" headertext="Specialty Type" uniquename="PROVIDER_CATEGORY_TYPE_NAME" readonly="true">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="IsActive_PHASEII" headertext="Is Active Phase 2" uniquename="IsActive_PHASEII" datatype="System.Boolean">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="MMIS_PROVIDER_CATEGORY_TYPE_ID" headertext="MMIS Provider Category Type ID" uniquename="MMIS_PROVIDER_CATEGORY_TYPE_ID">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="IMAGE_SRC" headertext="Image Source" uniquename="IMAGE_SRC">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Category Type Name </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="80" ID="txtProviderCategoryTypeName" aria-label="ProviderCategoryTypeName" runat="server" Text='<%# Bind("PROVIDER_CATEGORY_TYPE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtProviderCategoryTypeName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Is Active Phase 2</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chIsActivePhaseII" Checked='<%# Bind("IsActive_PHASEII") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">MMIS Provider Category Type ID </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1" ID="txtMMISProviderCategoryTypeID" aria-label="MMISProviderCategoryTypeID" runat="server" Text='<%# Bind("MMIS_PROVIDER_CATEGORY_TYPE_ID") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Image Source </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtImageSrc" aria-label="ImageSrc" runat="server" Text='<%# Bind("IMAGE_SRC") %>' CssClass="formField">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row text-center">
                            <%--<td colspan="2" align="center">--%>
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
