<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_AppSettings, App_Web_iq0r534d" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

                <asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
                        <p style="text-align: center">
                            <asp:Literal ID="pagelabel" runat="server" text="Reference Data Management: App Settings" ></asp:Literal>
                        </p>
                </asp:Panel>
<script type="text/javascript">
    $(document).ready(function () {
         
        Input = $('.rgFilterRow').children("td").children("input"); 
        Input.attr('Title', 'Filter');

        pagesize = $('.rcbReadOnly').children("input"); 
        pagesize.attr('Title', 'Pagesize'); 

        var tablegriddata = document.getElementById("ctl00_MainContent_AppSettingsID_rgAppSettings_ctl00");
        // Remove the thead element
        var thead = tablegriddata.querySelector("thead");
        tablegriddata.removeChild(thead);
        var tablepager = document.getElementById("ctl00_MainContent_AppSettingsID_rgAppSettings_ctl00_Pager");
        // Remove the thead element
        var thead = tablepager.querySelector("thead");
        tablepager.removeChild(thead);
    });
</script>

    <telerik:RadGrid ID="rgAppSettings" runat="server" RenderMode="Lightweight"  AllowPaging="True" AllowSorting="True"
                    OnNeedDataSource="rgAppSettings_NeedDataSource" AllowFilteringByColumn="True" Skin="PDMSModern"
                    CellSpacing="0" GridLines="None" OnInsertCommand="rgAppSettings_InsertCommand" OnUpdateCommand="rgAppSettings_UpdateCommand"
                    AutoGenerateColumns="false" AutoGenerateEditColumn="False" >
         <ClientSettings>

            <Scrolling AllowScroll="True" ScrollHeight="" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>

        </ClientSettings>
         <MasterTableView CommandItemDisplay="Top" GridLines="None"> 
                 <Columns> 
                     <telerik:GridEditCommandColumn HeaderText="<span style='display:none'>Edit</span>">
                    </telerik:GridEditCommandColumn>
                    <telerik:GridBoundColumn DataField="AppSettingsKey" HeaderText="App Settings Key" UniqueName="AppSettingsKey">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="AppSettingsValue" HeaderText="App Settings Value" UniqueName="AppSettingsValue">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="AppSettingsReadOnly" HeaderText="App Settings Read Only" UniqueName="AppSettingsReadOnly" DataType="System.Boolean">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="AppSettingsNotes" HeaderText="App Settings Notes" UniqueName="AppSettingsNotes">  
                    </telerik:GridBoundColumn> 
                     <telerik:GridBoundColumn DataField="AppSettingsEnvironSpecific" HeaderText="App Settings Environment Specific" UniqueName="AppSettingsEnvSpecific" DataType="System.Boolean">  
                    </telerik:GridBoundColumn> 
                    <telerik:GridBoundColumn DataField="CanOverwrite" HeaderText="Disable App Settings Deployment Updates?" UniqueName="CanOverwrite" DataType="System.Boolean">  
                    </telerik:GridBoundColumn> 
                </Columns> 
              <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCol">                
                    </EditColumn>                   
                    <FormTemplate>
                        <div class="WhiteBox">
                        <div class="gridEditTable">                   
                            <div class="row">
                                <div class="col-sm-3 text-right">App Settings Key</div>
                                 <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="150" ID="txtAppSettingsKey" runat="server" Text='<%# Bind("AppSettingsKey") %>' CssClass="formField">
                                        </asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* App Settings Key is required"
            ControlToValidate="txtAppSettingsKey"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-3 text-right">App Settings Value</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="1000" ID="txtAppSettingsValue" runat="server" Text='<%# Bind("AppSettingsValue") %>' CssClass="formField">
                                        </asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* App Settings Value is required"
            ControlToValidate="txtAppSettingsValue"></asp:RequiredFieldValidator>
                                </div>
                            </div> 
                            <div class="row">
                                <div class="col-sm-3 text-right">Take Action Allowed</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkTakeActionAllowed" Checked='<%# Bind("AppSettingsReadOnly") %>'/>
                                </div>
                            </div>
                               <div class="row">
                                <div class="col-sm-3 text-right">App Settings Notes</div>
                                 <div class="col-sm-9 text-left">
                                      <asp:TextBox MaxLength="1500" ID="TextBox1" runat="server" Text='<%# Bind("AppSettingsNotes") %>' CssClass="formFieldMultiline">
                                        </asp:TextBox>
                                </div>
                            </div> 
                            <div class="row">
                                <div class="col-sm-3 text-right">App Settings Environment Specific</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkAppSettingsEnvironSpecific" Checked='<%# Bind("AppSettingsEnvironSpecific") %>'/>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-sm-3 text-right">Disable App Settings Deployment Updates?</div>
                                <div class="col-sm-9 text-left">
                                    <asp:CheckBox runat="server" ID="chkCanOverwrite" Checked='<%# Bind("CanOverwrite") %>'/>
                                </div>
                            </div>
                              <div class="row text-center">
                               <%-- <td colspan="2" align="center">--%>
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