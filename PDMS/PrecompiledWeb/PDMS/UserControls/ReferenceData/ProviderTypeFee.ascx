<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_ProviderTypeFee, App_Web_iq0r534d" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style>
    caption {
    visibility:hidden !important
      }
    </style>
<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_ProviderTypeFee_rgProviderTypeFee_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                          <h1 style="text-align: center;font-size: 25px;font-weight: bold;">  <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Provider Type Fee" ></asp:Literal></h1>
                </asp:Panel>

    <telerik:RadGrid ID="rgProviderTypeFee" runat="server" RenderMode="Lightweight" MasterTableView-Caption="ProviderTypeFee"  AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgProviderTypeFee_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgProviderTypeFee_InsertCommand"  OnUpdateCommand="rgProviderTypeFee_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnItemDataBound="rgProviderTypeFee_ItemDataBound">
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="PROVIDER_TYPE_FEE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" UniqueName="PROVIDER_TYPE_NAME">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="IS_FEE_REQUIRED" HeaderText="Required" UniqueName="IS_REQUIRED" DataType="System.Boolean">
                     </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="FEE_AMOUNT" HeaderText="Fee Amount" UniqueName="FEE_AMOUNT" DataType="System.Decimal">  
                    </telerik:GridBoundColumn>  
                    <telerik:GridBoundColumn DataField="ENTITY_TYPE_NAME" HeaderText="Entity Type Name" UniqueName="ENTITY_TYPE_NAME">  
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="APPLICATION_TYPE_NAME" HeaderText="Application Type Name" UniqueName="APPLICATION_TYPE_NAME">  
                    </telerik:GridBoundColumn>                     
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
                       <div class="WhiteBox">
                        <div class="gridEditTable">
                            <div class="row">
                                <div class="col-sm-3 text-right">Provider Type</div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlProviderType" runat="server" CssClass="formDropDown"> </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Provider Type is required"
            ControlToValidate="ddlProviderType"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">Fee Required</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkFeeRequired" Checked='<%# Bind("IS_FEE_REQUIRED") %>'/>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-3 text-right">Fee Amount</div>
                                 <div class="col-sm-9 text-left">
                                     <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" DataType="System.Decimal"
                                         DbValue='<%# Bind("FEE_AMOUNT") %>' Type="Currency" ID="txtFeeAmount" CssClass="formField"
                                         MaxValue="99999.99" MinValue="0" Width="450px">
                                     </telerik:RadNumericTextBox>
                                </div>
                            </div>
                              <div class="row">
                                <div class="col-sm-3 text-right">Entity Type</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:DropDownList ID="ddlEntityType" aria-label="EntityType" runat="server" CssClass="formDropDown"> </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Entity Type is required"
            ControlToValidate="ddlEntityType"></asp:RequiredFieldValidator>
                                </div>
                            </div>                     
                            <div class="row">
                                <div class="col-sm-3 text-right">Application Type</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:DropDownList ID="ddlApplicationType" runat="server" CssClass="formDropDown"> </asp:DropDownList>
                                </div>
                            </div>
                                                                             
                              <div class="row text-center">
 <%--                               <td colspan="2" align="center">--%>
                                    <asp:Button  ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                    <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                        CommandName="Cancel"></asp:Button>
                              <%--  </td>--%>
                            </div> 
                            </div>
                            </div>                              
                       </FormTemplate>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
             </MasterTableView>
    </telerik:RadGrid> 
