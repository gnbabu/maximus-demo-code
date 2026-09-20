<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_ApplicationType, App_Web_iq0r534d" %>
 <style>
    caption {
    visibility:hidden !important
      }
    </style>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                        
                           <h1 style="text-align: center"><asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Application Type" ></asp:Literal></h1>
                        
                </asp:Panel>

    <telerik:RadGrid ID="rgApplicationType" runat="server" RenderMode="Lightweight" MasterTableView-Caption="Application type" AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgApplicationType_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgApplicationType_InsertCommand" OnUpdateCommand="rgApplicationType_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnItemCommand="rgApplicationType_ItemCommand">
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="APPLICATION_TYPE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn HeaderText="<span style='display:none'>edit</span>">
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="APPLICATION_TYPE_NAME" HeaderText="Provider Type Abbreviation" UniqueName="ProviderTypeAbbreviation">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="APPLICATION_TYPE_DESC" HeaderText="Provider Type Name" UniqueName="ProviderTypeName">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="IS_USED_IN_MMIS" HeaderText="Used in MMIS" UniqueName="UsedInMMIS" DataType="System.Boolean">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="MMIS_APPLICATION_TYPE_ID" HeaderText="MMIS Application Type ID" UniqueName="MMISApplicationTypeID" >  
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="IsVisible" HeaderText="Is Visible" UniqueName="Visible" DataType="System.Boolean">  
                    </telerik:GridBoundColumn> 
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
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
                            <div class="col-sm-3 text-right" >
                                <asp:label id=checkbox text="Used in MMIS" runat="server" AssociatedControlID="chkIsUsedInMMIS" Style="font-weight:100"></asp:label>
                            </div>
                                <div class="col-sm-9 text-left" >
                                  <asp:CheckBox runat="server" ID="chkIsUsedInMMIS" Checked='<%# Eval("IS_USED_IN_MMIS") == DBNull.Value ? false :  Eval("IS_USED_IN_MMIS") %>' />
                             </div>     
                            </div> 
                             <div class="row">
                                <div class="col-sm-3 text-right">MMIS Application Type ID</div>
                                 <div class="col-sm-9 text-left" >
                                      <asp:TextBox MaxLength="20" ID="txtMMISApplicationTypeID" runat="server" Text='<%# Bind("MMIS_APPLICATION_TYPE_ID") %>' CssClass="formField">
                                        </asp:TextBox>
                                </div>
                            </div> 
                            <div class="row">
                                <div class="col-sm-3 text-right"> 
                                    <asp:label id=Label1 text="Visible" runat="server" AssociatedControlID="chkIsVisible" Style="font-weight:100"></asp:label></div>
                                <div class="col-sm-9 text-left" >
                                    <asp:CheckBox runat="server" ID="chkIsVisible" Checked='<%# Eval("IsVisible") == DBNull.Value ? false :  Eval("IsVisible") %>' />
                                </div>
                            </div>
                              <div class="row text-center">
                                <%--<td colspan="2" align="center">--%>
                                    <asp:Button  ID="btnUpdate" Text='<%# (Container is Telerik.Web.UI.GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is Telerik.Web.UI.GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
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
