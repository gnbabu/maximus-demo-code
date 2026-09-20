<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_RegPageSetting, App_Web_iq0r534d" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_RegPageSetting_rgRegPageSetting_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>
    <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
        <p style="text-align: center">
            <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Reg Page Setting"></asp:Literal>
        </p>
    </asp:Panel>

    <telerik:RadGrid ID="rgRegPageSetting" runat="server" RenderMode="Lightweight" AllowPaging="True" AllowSorting="True" EnableLinqExpressions="false"
        AllowFilteringByColumn="True" Skin="PDMSModern"
        CellSpacing="0" GridLines="None" OnInsertCommand="rgRegPageSetting_InsertCommand" OnUpdateCommand="rgRegPageSetting_UpdateCommand" OnItemCommand="rgRegPageSetting_ItemCommand"
        AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnItemDataBound="rgRegPageSetting_ItemDataBound">
        <ClientSettings>
            <Scrolling AllowScroll="True" ScrollHeight="" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
        </ClientSettings>
        <ExportSettings FileName="RegPageSettings" ExportOnlyData="true" IgnorePaging="true" UseItemStyles="true" OpenInNewWindow="true" />
        <MasterTableView CommandItemDisplay="Top" GridLines="None"
            DataKeyNames="REG_PAGE_SETTING_ID">
            <CommandItemSettings RefreshText="Clear Filters" ShowExportToExcelButton="true" ShowExportToCsvButton="true" />
            <Columns>
                <telerik:GridEditCommandColumn>
                </telerik:GridEditCommandColumn>
                <telerik:GridBoundColumn DataField="ENTITY_TYPE_NAME" HeaderText="Entity Type" UniqueName="ENTITY_TYPE_NAME" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableEmbeddedSkins="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxEntity" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxEntity_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                                </telerik:RadComboBox>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" UniqueName="PROVIDER_TYPE_NAME" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxProviderType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxProviderType_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                            </telerik:RadComboBox>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="REG_PAGE_NAME" HeaderText="Reg Page Name" UniqueName="REG_PAGE_NAME" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxPageType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxPageType_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                            </telerik:RadComboBox>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="REG_PAGE_SECTION" HeaderText="Reg Page Section" UniqueName="REG_PAGE_SECTION" AllowFiltering="false" />
                <telerik:GridBoundColumn DataField="IS_VISIBLE" HeaderText="Visible" UniqueName="IS_VISIBLE" DataType="System.Boolean" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxIsVisible" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxIsVisible_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                            </telerik:RadComboBox>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="IS_EDITABLE" HeaderText="Editable" UniqueName="IS_EDITABLE" DataType="System.Boolean" AllowFiltering="false" />
                <telerik:GridBoundColumn DataField="TASK_NAME" HeaderText="Task Name" UniqueName="TASK_NAME" AllowFiltering="false" />
                <telerik:GridBoundColumn DataField="IS_REQUIRED" HeaderText="Required" UniqueName="IS_REQUIRED" DataType="System.Boolean" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxIsRequired" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxIsRequired_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                            </telerik:RadComboBox>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="APPLICATION_TYPE_NAME" HeaderText="Application Name" UniqueName="APPLICATION_TYPE_NAME" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxAppType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxAppType_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                            </telerik:RadComboBox>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="REG_SECTION_TYPE_NAME" HeaderText="Reg Section Type Name" UniqueName="REG_SECTION_TYPE_NAME" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxSectionType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxSectionType_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                            </telerik:RadComboBox>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="IS_REVIEW_REQUIRED" HeaderText="Review Required" UniqueName="IS_REVIEW_REQUIRED" DataType="System.Boolean" ShowFilterIcon="false">
                    <FilterTemplate>
                                <telerik:RadComboBox RenderMode="Lightweight" AllowCustomText="false" EnableLoadOnDemand="false" MarkFirstMatch="false"  Skin="PDMSModern" ID="RadComboBoxIsReviewRequired" AutoPostBack="true" runat="server" OnSelectedIndexChanged="RadComboBoxIsReviewRequired_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                    </Items>
                            </telerik:RadComboBox>
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
                                <div class="col-sm-3 text-right">Entity Type</div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlEntityType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Taxonomy Name </div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlProviderType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Reg Page Name</div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox MaxLength="80" ID="txtRegPageName" runat="server" Text='<%# Bind("REG_PAGE_NAME") %>' CssClass="formField">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
            ControlToValidate="txtRegPageName"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Reg Page Section</div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox MaxLength="80" ID="txtRegPageSection" runat="server" Text='<%# Bind("REG_PAGE_SECTION") %>' CssClass="formField">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Visible</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkIsVisible" Checked='<%# Bind("IS_VISIBLE") %>' />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Editable</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkIsEditable" Checked='<%# Bind("IS_EDITABLE") %>' />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Task Name</div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox MaxLength="80" ID="txtTaskName" runat="server" Text='<%# Bind("TASK_NAME") %>' CssClass="formField">
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
                                <div class="col-sm-3 text-right">Review Required</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkReviewRequired" Checked='<%# Bind("IS_REVIEW_REQUIRED") %>' />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Application Type</div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlApplicationType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Reg Page Type</div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlRegPageType" runat="server" CssClass="formDropDown"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Reg Section Type</div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlRegSectionType" runat="server" CssClass="formDropDown"></asp:DropDownList>
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
                </FormTemplate>
                <PopUpSettings ScrollBars="None" />
            </EditFormSettings>
        </MasterTableView>
    </telerik:RadGrid>
