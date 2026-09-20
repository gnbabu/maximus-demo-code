<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_ProviderCategoryType, App_Web_iq0r534d" %>
 <style>
    caption {
    visibility:hidden !important
      }
    </style>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                     
            <h1 style="text-align: center;font-size: 25px;font-weight: bold;"> <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Provider Category Type" ></asp:Literal></h1>
                        
                </asp:Panel>

    <telerik:RadGrid ID="rgProviderCategoryType" runat="server" MasterTableView-Caption="ProviderCategoryType"  RenderMode="Lightweight"  AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgProviderCategoryType_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgProviderCategoryType_InsertCommand" OnUpdateCommand="rgProviderCategoryType_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" >
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="PROVIDER_CATEGORY_TYPE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="PROVIDER_CATEGORY_TYPE_NAME" HeaderText="Specialty Type" UniqueName="PROVIDER_CATEGORY_TYPE_NAME" ReadOnly="true">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="IsActive_PHASEII" HeaderText="Is Active Phase 2" UniqueName="IsActive_PHASEII" DataType="System.Boolean">
                     </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="MMIS_PROVIDER_CATEGORY_TYPE_ID" HeaderText="MMIS Provider Category Type ID" UniqueName="MMIS_PROVIDER_CATEGORY_TYPE_ID">  
                    </telerik:GridBoundColumn> 
                       <telerik:GridBoundColumn DataField="IMAGE_SRC" HeaderText="Image Source" UniqueName="IMAGE_SRC">  
                    </telerik:GridBoundColumn>
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
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
                                    <asp:Button  ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                    <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
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
