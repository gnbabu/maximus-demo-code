<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_ApplicationFeePaymentType, App_Web_iq0r534d" %>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                        <p style="text-align: center">
                            <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Application Fee Payment Type" ></asp:Literal>
                        </p>
                </asp:Panel>

    <telerik:RadGrid ID="rgApplicationFeePaymentType" runat="server" RenderMode="Lightweight"  AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgApplicationFeePaymentType_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgApplicationFeePaymentType_InsertCommand" OnUpdateCommand="rgApplicationFeePaymentType_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" >
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="PAYMENT_TYPE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="PAYMENT_TYPE_NAME" HeaderText="Payment Type" UniqueName="PAYMENT_TYPE_NAME">  
                    </telerik:GridBoundColumn>                     
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
                        <div class="WhiteBox">
                        <table class="gridEditTable">
                             <tr>
                                <td>Payment Type Name</td>
                                 <td>
                                      <asp:TextBox MaxLength="100" ID="txtPaymentTypeName" runat="server" Text='<%# Bind("PAYMENT_TYPE_NAME") %>' Width ="100%">
                                       </asp:TextBox>
                                </td>
                            </tr>
                                                                             
                              <tr>
                                <td colspan="2" align="center">
                                    <asp:Button  ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                    <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                        CommandName="Cancel"></asp:Button>
                                </td>
                            </tr> 
                            </table>
                            </div>                              
                       </FormTemplate>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
             </MasterTableView>
    </telerik:RadGrid> 
  </div>

