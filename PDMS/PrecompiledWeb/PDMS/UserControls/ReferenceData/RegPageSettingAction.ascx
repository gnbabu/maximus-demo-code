<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_RegPageSettingAction, App_Web_iq0r534d" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                        <p style="text-align: center">
                            <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Reg Page Setting Action" ></asp:Literal>
                        </p>
                </asp:Panel>

    <telerik:RadGrid ID="rgRegPageSettingAction" RenderMode="Lightweight"  runat="server" AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgRegPageSettingAction_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgRegPageSettingAction_InsertCommand" OnUpdateCommand="rgRegPageSettingAction_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False"   >
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="REG_PAGE_SETTING_ACTION_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="REG_PAGE_NAME" HeaderText="Reg Page Name" UniqueName="REG_PAGE_NAME">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="ROLE_NAME" HeaderText="Role Name" UniqueName="ROLE_NAME">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="TAKE_ACTION_ALLOWED" HeaderText="Take Action Allowed" UniqueName="TAKE_ACTION_ALLOWED" DataType="System.Boolean">  
                    </telerik:GridBoundColumn> 
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
                        <div class="WhiteBox">
                        <div class="gridEditTable">               
                            <div class="row">
                                <div class="col-sm-3 text-right">Reg Page Name </div>
                                 <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="80" ID="txtRegPageName" runat="server" Text='<%# Bind("REG_PAGE_NAME") %>' CssClass="formField">
                                        </asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
            ControlToValidate="txtRegPageName"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-3 text-right">Role Name</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="256" ID="txtRoleName" runat="server" Text='<%# Bind("ROLE_NAME") %>' CssClass="formField">
                                        </asp:TextBox>
                                </div>
                            </div> 
                            <div class="row">
                                <div class="col-sm-3 text-right">Take Action Allowed</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkTakeActionAllowed" Checked='<%# Bind("TAKE_ACTION_ALLOWED") %>'/>
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
