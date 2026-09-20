<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_SpecialtyType, App_Web_iq0r534d" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style>
    caption {
    visibility:hidden !important
      }
    </style>
<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_SpecialtyType_rgSpecialtyType_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                          <h1 style="text-align: center;font-size: 25px;font-weight: bold;">  <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Specialty Type" ></asp:Literal></h1>
                        
                </asp:Panel>

    <telerik:RadGrid ID="rgSpecialtyType" runat="server" RenderMode="Lightweight" MasterTableView-Caption="SpecialtyType"    AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgSpecialtyType_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgSpecialtyType_InsertCommand" OnUpdateCommand="rgSpecialtyType_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" >
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="SPECIALTY_TYPE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_NAME" HeaderText="Specialty Type" UniqueName="SPECIALTY_TYPE_NAME" ReadOnly="true"/>
                     <telerik:GridBoundColumn DataField="MMIS_SPECIALTY_TYPE_ID" HeaderText="MMIS Specialty Type ID" UniqueName="MMISSpecialtyTypeID"/> 
                     <telerik:GridBoundColumn DataField="EXTERNAL_SPECIALTY_TYPE_NAME" HeaderText="External Specialty Type Name" UniqueName="EXTERNAL_SPECIALTY_TYPE_NAME"/> 
                     <telerik:GridBoundColumn DataField="IsVisible" HeaderText="IsActive" UniqueName="IsVisible"/> 
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
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
                                    <asp:CheckBox runat="server" ID="chkIsVisible" Checked='<%# Bind("IsVisible") %>'/>
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
