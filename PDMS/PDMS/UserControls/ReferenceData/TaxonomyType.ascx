<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_TaxonomyType" Codebehind="TaxonomyType.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>

    <style>
    caption {
    visibility:hidden !important
      }
    </style>
<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_ucTaxonomyType_rgTaxonomy_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                        <p style="text-align: center" class="page-main-header">
                            <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Taxonomy Type" ></asp:Literal>
                        </p>
                </asp:Panel>

    <telerik:RadGrid ID="rgTaxonomy" runat="server" MasterTableView-Caption="Application type" RenderMode="Lightweight"  AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgTaxonomy_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgTaxonomy_InsertCommand"  OnUpdateCommand="rgTaxonomy_UpdateCommand" 
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnItemDataBound="rgTaxonomy_ItemDataBound">
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="TAXONOMY_TYPE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="TAXONOMY_CODE" HeaderText="Taxonomy Code" UniqueName="TaxonomyCode">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="TAXONOMY_NAME" HeaderText="Taxonomy Name" UniqueName="TaxonomyName">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_NAME" HeaderText="Specialty Type" UniqueName="SPECIALTY_TYPE_NAME" ReadOnly="true">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" UniqueName="PROVIDER_TYPE_NAME" ReadOnly="true">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="EXPIRATION_DATE" HeaderText="Expiration Date" UniqueName="ExpirationDate" DataType="System.DateTime" 
                        DataFormatString="{0:MM/dd/yyyy}">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="MMIS Specialty Type ID" UniqueName="MMISSpecialtyTypeID">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="NPI_REQUIRED" HeaderText="NPI Required" UniqueName="NPIRequired" DataType="System.Boolean">
                     </telerik:GridBoundColumn>
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
                        <div class="WhiteBox">
                        <div class="gridEditTable">                     
                            <div class="row">
                                <div class="col-sm-3 text-right">Taxonomy Code </div>
                                 <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="80" ID="txtTaxCode" aria-label="TaxCode" runat="server" Text='<%# Bind("TAXONOMY_CODE") %>' CssClass="formField">
                                        </asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Taxonomy Code is required"
            ControlToValidate="txtTaxCode"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Taxonomy Name </div>
                                 <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="256" ID="txtTaxName" aria-label="taxname" runat="server" Text='<%# Bind("TAXONOMY_NAME") %>' CssClass="formField">
                                        </asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Taxonomy Name is required"
            ControlToValidate="txtTaxName"></asp:RequiredFieldValidator>
                                </div>
                            </div> 
                        <div class="row">
                            <div class="col-sm-3 text-right">Specialty Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddSpecialtyType" runat="server" CssClass="formDropDown" > </asp:DropDownList>
                            </div>     
                        </div>
                          <div class="row">
                            <div class="col-sm-3 text-right">Provider Type</div>
                            <div class="col-sm-9 text-left">
                                <asp:DropDownList ID="ddProviderType" runat="server" CssClass="formDropDown" > </asp:DropDownList>
                            </div>     
                        </div>   
                             <div class="row">
                                <div class="col-sm-3 text-right">Expiration Date</div>
                                 <div class="col-sm-9 text-left">
                                      <ajx:CalendarExtender ID="calExpirationDate" runat="server" 
                                        Format="MM/dd/yyyy"  TargetControlID="txtExpirationDate" PopupPosition="BottomRight"  
                                        CssClass="QstCalendarCSS" PopupButtonID="imgExpirationDate"  EnabledOnClient="true" />
                                     <asp:TextBox ID="txtExpirationDate" runat="server" aria-label="ExpirationDate" Text='<%# Bind("EXPIRATION_DATE","{0:MM/dd/yyyy}") %>' CssClass="formField"/>
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
                                <div class="col-sm-3 text-right">NPI Required</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkNPIRequired" Checked='<%# Bind("NPI_REQUIRED") %>'/>
                                </div>
                            </div>

                              <div class="row text-center">
                                    <asp:Button  ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                    <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" CssClass="buttonBox"></asp:Button>
                            </div> 
                            </div>
                            </div>                              
                       </FormTemplate>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
             </MasterTableView>
    </telerik:RadGrid> 
