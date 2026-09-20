<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_PaperRequestDocumentType, App_Web_iq0r534d" %>
 <style>
    caption {
    visibility:hidden !important
      }
    </style>
                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                        
             <h1 style="text-align: center;font-size: 25px;font-weight: bold"><asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: Paper Request Document Type" ></asp:Literal></h1>
                        
                </asp:Panel>

    <telerik:RadGrid ID="rgPaperRequestDocumentType" runat="server" MasterTableView-Caption="PaperRequestDocumentType" RenderMode="Lightweight"  AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgPaperRequestDocumentType_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgPaperRequestDocumentType_InsertCommand" OnUpdateCommand="rgPaperRequestDocumentType_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" >
         <MasterTableView CommandItemDisplay="Top" GridLines="None"  
                DataKeyNames="PAPER_REQUEST_DOCUMENT_TYPE_ID"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="PAPER_REQUEST_DOCUMENT_TYPE_NAME" HeaderText="Paper Request Document Type" UniqueName="PAPER_REQUEST_DOCUMENT_TYPE_NAME">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION" HeaderText="Paper Request Document Type Description" UniqueName="PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION">  
                    </telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE" HeaderText="Paper Request Document Type On Base Code" UniqueName="PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE">  
                    </telerik:GridBoundColumn>                            
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
                        <div class="WhiteBox">
                        <div class="gridEditTable">
                             <div class="row">
                                <div class="col-sm-3 text-right">Paper Request Document Type Name</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="255" aria-label="PaperRequestDocumentTypeName" ID="txtPaperRequestDocumentTypeName" runat="server" Text='<%# Bind("PAPER_REQUEST_DOCUMENT_TYPE_NAME") %>' CssClass="formField">
                                       </asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
            ControlToValidate="txtPaperRequestDocumentTypeName"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3 text-right">Paper Request Document Type Description</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="500" aria-label="PaperRequestDocumentTypeDescription" ID="txtPaperRequestDocumentTypeDescription" runat="server" Text='<%# Bind("PAPER_REQUEST_DOCUMENT_TYPE_DESCRIPTION") %>' CssClass="formFieldMultiline">
                                       </asp:TextBox>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-3 text-right">Paper Request Document Type OnBase Code</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="120" ID="txtPaperRequestOnBaseCode" aria-label="PaperRequestOnBaseCode" runat="server" Text='<%# Bind("PAPER_REQUEST_DOCUMENT_TYPE_ONBASE_CODE") %>' CssClass="formField">
                                       </asp:TextBox>
                                </div>
                            </div>                                              
                              <div class="row text-center">
                               <%-- <td colspan="2" align="center">--%>
                                    <asp:Button  ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                    <asp:Button  ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                        CommandName="Cancel"></asp:Button>
                               <%-- </td>--%>
                            </div> 
                            </div>
                            </div>                              
                       </FormTemplate>
                    <PopUpSettings ScrollBars="None" />
                </EditFormSettings>
             </MasterTableView>
    </telerik:RadGrid> 
