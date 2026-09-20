<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_TransferOwnership, App_Web_yvhxe4ml" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc1" %>

<script type="text/javascript">
 <%--   function checkchanged(chkTerms) {
            
        var tabCon = "<%= tabFile.ClientID %>";
        if ()
            var tab1 = document.getElementById('<%= tabFile.FindControl("tab1").ClientID %>');
            var chkbox = chk.getElementsByTagName("input");
            document.getElementById('<%= Login1.FindControl("LoginButton").ClientID %>').disabled = chk.checked ? false : true;
        }--%>

    function clientActiveTabChanged(sender, args) {
        var index = sender.get_activeTabIndex();
        var tab1 = document.getElementById('<%= tabFile.FindControl("TabPanel1").FindControl("tab1").ClientID %>');    
        var tabPanel1 = document.getElementById('<%= tabFile.FindControl("TabPanel1").ClientID %>'); 
        var tabPanel2 = document.getElementById('<%= tabFile.FindControl("TabPanel2").ClientID %>'); 
        var pnlUploadDocs = document.getElementById('<%= tabFile.FindControl("TabPanel2").FindControl("pnlUploadDocs").ClientID %>'); 
        if (index == 0) {   
            //alert("Ramya " + tab1);
            tab1.style.display = "block";
            //tabPanel2.style.display = "block"; 
        }
        else {    
            //alert("Ramya1 " + tabPanel2);
            tabPanel1.style.display = "block";
            tab1.style.display = "none";
            tabPanel2.style.display = "block";
            pnlUploadDocs.style.display = "block";
        }
    }

</script>
 
<div style="padding:5px;width:auto;">
    <cc1:TabContainer ID="tabFile" runat="server" Width="100%" ActiveTabIndex="0" AutoPostBack="false" CssClass="Tab" TabStripPlacement="Top">        
        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Transfer Ownership" Enabled="true">
            <ContentTemplate>
                <asp:Panel id="tab1" runat="server">
                    <asp:Label ID="lblError" runat="server" Text="*Please choose the User Name(Future User)." CssClass="bodyTextRed" Visible="false"></asp:Label>
                     <div>
                         <br />
                          <div class="row">
                             <div class="col-sm-3 text-right"><span id="spnMedicaidId" runat="server" class="formLabel wd120">Medicaid ID :</span></div>
                             <div class="col-sm-9 text-left">
                                 <asp:Label id="lblMedicaidID" runat="server" />
                             </div> 
                          </div>
                         <div class="row">
                               <div class="col-sm-3 text-right"><span id="Span1" runat="server" class="formLabel wd120">Assigned To :</span></div>
                             <div class="col-sm-9 text-left">
                                 <asp:Label id="lblAssignedTo" runat="server"/>
                             </div>
                          </div>
                         <div class="row">
                               <div class="col-sm-3 text-right"><span id="Span2" runat="server" class="formLabel wd120">Transfer To* :</span></div>
                             <div class="col-sm-9 text-left">
                                 <asp:TextBox ID="txtTransferto" runat="server" CssClass="formField300" />
                             </div>
                          </div>
                         <div class="row">
                              <div class="col-sm-3 text-right pg-hint"> * Future User</div>
                              <div class="col-sm-9 text-left"> </div>
                          </div>     
                     </div>

                 </asp:Panel>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Upload Document" Enabled="true">
            <ContentTemplate>
                    <asp:Panel ID="pnlUploadDocs" runat="server" CssClass="UploadBox">
                         <asp:GridView ID="gvUploadedDocs" runat="server" AutoGenerateColumns="False" 
                            HorizontalAlign="Center" Width="100%"  ShowHeaderWhenEmpty="true"
                                CssClass="gridViewSmallFont" EmptyDataText="No uploaded documents found." 
                            onrowdeleting="gvUploadedDocs_RowDeleting" 
                            onrowcommand="gvUploadedDocs_RowCommand" 
                            onrowdatabound="gvUploadedDocs_RowDataBound" 
                            onrowediting="gvUploadedDocs_RowEditing">
                            <Columns>
                                <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
                                <asp:BoundField DataField="FILE_NAME" HeaderText="File Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
                                <asp:BoundField DataField="REG_PAGE_NAME" HeaderText="Page Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />            
                                <asp:BoundField DataField="Username" HeaderText="Username" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />  
                                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Upload Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" /> 
                                <asp:BoundField DataField="RoleName" HeaderText="Role" Visible="false" />
                                           
                                <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgView" ImageUrl="~/Images/search.png" runat="server" alt="Search Button" ToolTip="View" CommandName="Edit" />
                                    </ItemTemplate> 
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgCancel" ImageUrl="~/Images/cancel.png" runat="server" CommandName="Delete" ToolTip="Delete" 
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DOCUMENT_ID") %>' />
                                    </ItemTemplate> 
                                </asp:TemplateField>
                            </Columns>   
                            <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
                            <HeaderStyle CssClass="gridViewHeader" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" /> 
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                        <br />
                        <div id="divupload">
                            <div class="row">
                                <div class="col-sm-3"></div>
                                <div class ="col-sm-9 text-left">
                                    <mms:EncryptedFileUpload runat="server" ID="filUploadFile" Width="400px" ViewStateMode="Enabled" CssClass="fileControl"/>
                                    <!--<asp:FileUpload id="filUploadFile1" runat="server" Width="400px" size="100" />-->
                                </div>
                            </div>
                            <%--<tr><td colspan="2">&nbsp;</td></tr>--%>
                            <div class="row">
                                <div class="col-sm-12 text-center"><asp:ValidationSummary ID="vsUpdateDocument" runat="server" DisplayMode="SingleParagraph" ValidationGroup="valUpdateDocument" /></div>
                            </div>
                            <div class="row">
                                <div id="colname1" class="col-sm-3 text-right"><span class="formLabel200">Name</span></div>
                                <div id="colname2" class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="formField wd400" MaxLength="100" />
                                    <%--Consolidated List - DR60: Name not required. If not entered, use file name.--%>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="reqName" ValidationGroup="valProviderInfoHeader"
                                        ControlToValidate="txtName" ErrorMessage="*Enter Name" Text="*" Display="Dynamic" 
                                        SetFocusOnError="true" />--%>               
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel200">Description</span></div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtDescription" runat="server" Rows="5" CssClass="formFieldMultiline wd400" TextMode="MultiLine" MaxLength="500" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <asp:Button id="UploadButton" Text="Upload file" OnClick="UploadButton_Click" runat="server" CssClass="buttonBox"
                            ValidationGroup="valUpdateDocument" />  
                        <p><b><asp:Label ID="lblStatusMsg" runat="server" Visible="false" /></b></p>                         
                    </asp:Panel>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
     <div class="btnBox" style="padding-top: 20px; padding-right: 10px;">
        <asp:Button ID="btnTransfer" runat="server" Text="Transfer" CssClass="buttonBoxFocus" OnClick="btnTransfer_Click" CausesValidation="true" />           
        <asp:Button ID="btnCancelmpe1" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
     </div>
</div>




