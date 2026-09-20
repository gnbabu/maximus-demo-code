<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_RegPageType, App_Web_iq0r534d" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
   <style>
    caption {
    visibility:hidden !important
      }
    </style>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                            <h1 style="text-align: center;font-size: 25px;font-weight: bold;"> <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Reg Page Type " ></asp:Literal></h1>
                        
                </asp:Panel>

    <telerik:RadGrid ID="rgRegPageType" runat="server" RenderMode="Lightweight" MasterTableView-Caption="RegPageType"   AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgRegPageType_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgRegPageType_InsertCommand" OnUpdateCommand="rgRegPageType_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" >
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="REG_PAGE_TYPE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="REG_PAGE_NAME" HeaderText="Reg Page Name" UniqueName="REG_PAGE_NAME">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="SEQUENCE_ID" HeaderText="Sequence ID" UniqueName="SEQUENCE_ID" DataType="System.Int32">  
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
                                        <asp:TextBox MaxLength="80" ID="txtRegPageName" aria-label="RegPageName" runat="server" Text='<%# Bind("REG_PAGE_NAME") %>' CssClass="formField">
                                        </asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
            ControlToValidate="txtRegPageName"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-3 text-right">Sequence ID</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox  ID="txtSequenceID" aria-label="SequenceID" runat="server" Text='<%# Bind("SEQUENCE_ID") %>' CssClass="formField">
                                      </asp:TextBox>
                                      <ajx:FilteredTextBoxExtender ID="ftbeTxtSequenceID" runat="server" Enabled="True" TargetControlID="txtSequenceID" FilterType="Numbers" FilterMode="ValidChars">
                                      </ajx:FilteredTextBoxExtender>
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
