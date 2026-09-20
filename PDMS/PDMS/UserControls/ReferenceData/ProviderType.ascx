<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_ProviderType" Codebehind="ProviderType.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style type="text/css">
.RadGrid_PDMSModern .rgFilterBox {
    background-color: #fff !important;
    font-size: medium !important;
    color: #000;
    height: 30px !important;
    width: 70%;
}

    caption {
    visibility:hidden !important
      }
    </style>
   
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    
        <p style="text-align: center" class="page-main-header">
            <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Provider Type"></asp:Literal>
        </p>
   
</asp:Panel>

<telerik:RadGrid ID="rgProviderType" aria-label="ProviderType" enableariasupport="true" MasterTableView-Caption="ProviderType" runat="server" RenderMode="Lightweight" AllowPaging="True" AllowSorting="True"
    OnNeedDataSource="rgProviderType_NeedDataSource" AllowFilteringByColumn="True" Skin="PDMSModern"
    CellSpacing="0" GridLines="None" OnInsertCommand="rgProviderType_InsertCommand" OnUpdateCommand="rgProviderType_UpdateCommand"
    AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnItemDataBound="rgProviderType_ItemDataBound" EnableEmbeddedSkins="false" OnItemCreated="rgProviderType_ItemCreated">
    <GroupingSettings CaseSensitive="false" />
    <ClientSettings>
        <Resizing AllowColumnResize="true" />
        <ClientEvents OnFilterMenuShowing="filterMenuShowing" />
    </ClientSettings>
    <FilterMenu OnClientShowing="MenuShowing" CssClass="gridviewFilter" />
    <MasterTableView CommandItemDisplay="Top" GridLines="None"
        DataKeyNames="PROVIDER_TYPE_ID">
        <Columns>
            <telerik:GridEditCommandColumn>
            </telerik:GridEditCommandColumn>
            <telerik:GridBoundColumn DataField="PROVIDER_TYPE_ABBREVIATION" HeaderText="Provider Type Abbreviation" UniqueName="ProviderTypeAbbreviation">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type Name" UniqueName="ProviderTypeName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="IS_USED_IN_MMIS"  HeaderText="Used in MMIS" UniqueName="UsedInMMIS">
                <FilterTemplate>
                <telerik:RadComboBox ID="RadMMIS" Width="100%"  EnableViewState="false" Skin="PDMSModern"   
                        runat="server" OnClientSelectedIndexChanged="RadMMISIndexChanged" EnableEmbeddedSkins="false">
                       <Items>
                           <telerik:RadComboBoxItem Text="All" Value="" />
                           <telerik:RadComboBoxItem Text="Yes" Value="Y" />
                           <telerik:RadComboBoxItem Text="No" Value="N" />
                       </Items>
                    </telerik:RadComboBox>
                     <telerik:RadScriptBlock ID="RadScriptBlock4" runat="server">
                        <script type="text/javascript">

                            function RadMMISIndexChanged(sender, args) {
                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                tableView.filter("UsedInMMIS", args.get_item().get_value(), "EqualTo");
                            }
                        </script>
                    </telerik:RadScriptBlock>
                    </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PROVIDER_CATEGORY_TYPE_NAME" HeaderText="Provider Category" UniqueName="ProviderCategoryTypeName" ReadOnly="true">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="MMIS_PROVIDER_TYPE_ID" HeaderText="MMIS Provider Type ID" UniqueName="MMISProviderTypeID">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="REQUIRE_NPI" HeaderText="NPI Required" UniqueName="NPIRequired" DataType="System.Boolean">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PROVIDER_RISK_LEVEL_NAME" HeaderText="Provider Risk Level" UniqueName="ProviderRiskLevelName">
                <FilterTemplate>
                    <telerik:RadComboBox ID="RadComboBoxRisk" aria-label="ProviderType" enableariasupport="true"  DataTextField="PROVIDER_RISK_LEVEL_NAME" Skin="PDMSModern" 
                        DataValueField="PROVIDER_RISK_LEVEL_NAME" Width="100%"  EnableViewState="false"
                        runat="server" OnClientSelectedIndexChanged="RiskIndexChanged" EnableEmbeddedSkins="false">
                       
                    </telerik:RadComboBox>
                    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                        <script type="text/javascript">

                            function RiskIndexChanged(sender, args) {
                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                var filtervalue = args.get_item().get_value();
                                if (filtervalue == "All") filtervalue = "";
                                tableView.filter("ProviderRiskLevelName", filtervalue, "EqualTo");
                            }
                        </script>
                    </telerik:RadScriptBlock>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="APPLICATION_TYPE_NAME" HeaderText="Application Type" UniqueName="ApplicationTypeName">
                <FilterTemplate>
                    <telerik:RadComboBox ID="RadComboBoxApplication" DataTextField="APPLICATION_TYPE_NAME" Skin="PDMSModern"  
                        DataValueField="APPLICATION_TYPE_NAME" Width="100%"   EnableViewState="false"
                        runat="server" OnClientSelectedIndexChanged="ApplicationIndexChanged" EnableEmbeddedSkins="false">
                       
                    </telerik:RadComboBox>
                    <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                        <script type="text/javascript">

                            function ApplicationIndexChanged(sender, args) {
                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                var filtervalue = args.get_item().get_value();
                                if (filtervalue == "All") filtervalue = "";
                                tableView.filter("ApplicationTypeName", filtervalue, "EqualTo");
                            }
                        </script>
                    </telerik:RadScriptBlock>
                </FilterTemplate>
            </telerik:GridBoundColumn>
        </Columns>
        <EditFormSettings EditFormType="Template">
            <EditColumn UniqueName="EditCol">
            </EditColumn>
            <FormTemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Type Abbreviation </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="10" ID="txtProviderTypeAbbreviation" aria-label="ProviderTypeAbbreviation"  runat="server" Text='<%# Bind("PROVIDER_TYPE_ABBREVIATION") %>' CssClass="formField"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Type Name </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="256" aria-label="ProviderTypeName" ID="txtProviderTypeName" runat="server" Text='<%# Bind("PROVIDER_TYPE_NAME") %>' CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtProviderTypeName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Used in MMIS</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkIsUsedInMMIS" Checked='<%# Eval("IS_USED_IN_MMIS") == DBNull.Value ? false :  Eval("IS_USED_IN_MMIS").ToString()== "Y" ? true :false  %>' /></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Category Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlProviderCategoryType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="ddlProviderCategoryType"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">MMIS Provider Type ID</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="5" ID="txtMMISProviderTypeID" aria-label="MMISProviderTypeID" runat="server" Text='<%# Bind("MMIS_PROVIDER_TYPE_ID") %>' CssClass="formField"></asp:TextBox></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"> <asp:label id=checkbox text="NPI Requireds" runat="server" AssociatedControlID="chkIsUsedInMMIS" Style="font-weight:100"></asp:label>
</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkNPIRequired" Checked='<%# Eval("REQUIRE_NPI") == DBNull.Value ? false :  Eval("REQUIRE_NPI") %>' /></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Provider Risk Level</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlProviderRiskLevel" aria-label="ProviderRiskLevel" runat="server" CssClass="formDropDown"></asp:DropDownList></div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Application Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddlApplicationType" aria-label="ApplicationType" runat="server" CssClass="formDropDown"></asp:DropDownList></div>
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
            </FormTemplate>
            <PopUpSettings ScrollBars="None" />
        </EditFormSettings>
    </MasterTableView>
</telerik:RadGrid>
