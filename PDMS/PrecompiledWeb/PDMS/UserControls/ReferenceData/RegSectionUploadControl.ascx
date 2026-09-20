<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_RegSectionUploadControl, App_Web_iq0r534d" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style>
    caption {
    visibility:hidden !important
      }
    </style>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                        
                <h1 style="text-align: center;font-size: 25px;font-weight: bold;">  <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Reg Section Upload Control" ></asp:Literal></h1>
                       
                </asp:Panel>

    <telerik:RadGrid ID="rgRegSectionUploadControl" RenderMode="Lightweight" MasterTableView-Caption="RegSectionUploadControl"   runat="server" AllowPaging="True" AllowSorting="True"   
                    OnNeedDataSource="rgRegSectionUploadControl_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgRegSectionUploadControl_InsertCommand"  OnUpdateCommand="rgRegSectionUploadControl_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnItemDataBound="rgRegSectionUploadControl_ItemDataBound" >
          <ClientSettings>

<Scrolling AllowScroll="True" ScrollHeight="" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>

</ClientSettings>
         <MasterTableView CommandItemDisplay="Top" GridLines="None"     
                DataKeyNames="REG_SECTION_UPLOAD_CONTROL_ID">
          
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                     <telerik:GridBoundColumn DataField="APPLICATION_TYPE_NAME" HeaderText="Application Type" UniqueName="APPLICATION_TYPE_NAME">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" UniqueName="PROVIDER_TYPE_NAME">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="PROVIDER_CATEGORY_TYPE_NAME" HeaderText="Provider Category Type Name" UniqueName="PROVIDER_CATEGORY_TYPE_NAME">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="REG_PAGE_NAME" HeaderText="Reg Page Type" UniqueName="REG_PAGE_NAME">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="TITLE" HeaderText="Title" UniqueName="TITLE" >  
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DESCRIPTION" HeaderText="Description" UniqueName="DESCRIPTION" >  
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="IS_REQUIRED" HeaderText="Required" UniqueName="IS_REQUIRED" DataType="System.Boolean">
                     </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="REG_PAGE_SECTION" HeaderText="Reg Page Section" UniqueName="REG_PAGE_SECTION">  
                    </telerik:GridBoundColumn>    
                    <telerik:GridBoundColumn DataField="REG_PAGE_NAME" HeaderText="Reg Page Name" UniqueName="REG_PAGE_NAME">  
                    </telerik:GridBoundColumn>                  
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
                        <div class="WhiteBox">
                        <div class="gridEditTable">                    
                            <div class="row">
                                <div class="col-sm-3 text-right" >Application Type</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:DropDownList ID="ddlApplicationType" runat="server" CssClass="formDropDown" > </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Provider Category Type </div>
                                 <div class="col-sm-9 text-left">
                                     <asp:DropDownList ID="ddlProviderCategoryType" runat="server" CssClass="formDropDown" > </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Provider Type</div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlProviderType" runat="server" CssClass="formDropDown" > </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Reg Page Type</div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlRegPageType" runat="server" CssClass="formDropDown" > </asp:DropDownList>
                                </div>
                            </div> 
                             <div class="row">
                                <div class="col-sm-3 text-right">Title</div>
                                 <div class="col-sm-9 text-left">
                                     <asp:TextBox MaxLength="150" aria-label="Title" ID="txtTitle" runat="server" Text='<%# Bind("TITLE") %>' CssClass="formField">
                                     </asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
            ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                                </div>
                            </div> 
                             <div class="row">
                                <div class="col-sm-3 text-right">Description</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="500" ID="txtDescription" aria-label="Description" runat="server" Text='<%# Bind("DESCRIPTION") %>' CssClass="formField">
                                        </asp:TextBox>
                                </div>
                            </div> 
                             <div class="row">
                                <div class="col-sm-3 text-right">Required</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkIsRequired" Checked='<%# Bind("IS_REQUIRED") %>'/>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-3 text-right">Reg Page Section</div>
                                 <div class="col-sm-9 text-left">
                                     <%-- <asp:TextBox MaxLength="150" ID="txtRegPageSection" runat="server" Text='<%# Bind("REG_PAGE_SECTION") %>' Width ="100%">
                                        </asp:TextBox>--%>
                                     <asp:DropDownList ID="ddlRegPageSection" aria-label="RegPageSection" runat="server" CssClass="formDropDown" > </asp:DropDownList>
                                </div>
                            </div> 
                              <div class="row">
                                <div class="col-sm-3 text-right">Reg Page Name</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="150" ID="txtRegPageName" ari-label="RegPageName" runat="server" Text='<%# Bind("REG_PAGE_NAME") %>' CssClass="formField">
                                        </asp:TextBox>
                                </div>
                            </div>                                                        
                              <div class="row text-center">
                                    <asp:Button  ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                    <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                        CommandName="Cancel"></asp:Button>
                            </div> 
                            </div>
                            </div>                              
                       </FormTemplate>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
             </MasterTableView>
    </telerik:RadGrid> 
s