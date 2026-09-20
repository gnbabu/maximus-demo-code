<%@ page language="C#" autoeventwireup="true" inherits="Process_ProviderHomeNew, App_Web_sdbcnqyo" masterpagefile="~/MasterPage.master" title="Provider Management Home" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Src="~/PopupControls/ProviderSummaryView.ascx" TagName="ProviderSummaryView" TagPrefix="viewSummary" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%--<%@ Register Src="~/PopupControls/ProviderManagementView.ascx" TagName="ProviderManagementView" TagPrefix="viewDetail" %>--%>
<%--<%@ Register Src="~/PopupControls/ProviderAddView.ascx" TagName="ProviderAddView" TagPrefix="viewAddProvider" %>--%>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .groupBoxPA{
            margin-left:20% !important;
        }

        .groupBoxSP{
            margin-left:10% !important;
        }

        .btn-align-right{
           margin-left: auto;   
        }
 
        .alert-info{
            color: #000000
        }

        .addUserModalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.2;
            height: auto;
        }

         .addUserModalPopup
        {
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding: 0px;
            width: auto;
            height: auto;
        }

         .downloadbuttonStyle{
            padding-top: 0.5rem;
            margin-left: auto;
            padding-right: 15rem;
         }
        @media only screen and (max-width: 760px)  {
         .btn-align-right {
               margin-left: 0px;
         }
        }
        @media only screen and (max-width: 990px)  {
         .btn-align-right {
               margin-left: 0px;
               margin-right: 5px;
         }
        }

    </style>
    <%--    <script type="text/javascript">
        var column = null;
        function MenuShowing(sender, args) {
            if (column == null) return;
            var menu = sender; var items = menu.get_items();
            if (column.get_dataType() == "System.String") {
                var i = 0;
                while (i < items.get_count()) {
                    if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '', 'NotIsEmpty': '', 'IsEmpty': '', 'NotEqualTo': '', 'EqualTo': '' })) {
                        var item = items.getItem(i);
                        if (item != null)
                            item.set_visible(false);
                    }
                    else {
                        var item = items.getItem(i);
                        if (item != null)
                            item.set_visible(true);
                    } i++;
                }
            }
            if (column.get_dataType() == "System.Int64") {
                var j = 0; while (j < items.get_count()) {
                    if (!(items.getItem(j).get_value() in { 'NoFilter': '', 'GreaterThan': '', 'LessThan': '', 'NotEqualTo': '', 'EqualTo': '' })) {
                        var item = items.getItem(j); if (item != null)
                            item.set_visible(false);
                    }
                    else { var item = items.getItem(j); if (item != null) item.set_visible(true); } j++;
                }
            }
            column = null;
            menu.repaint();
        }
        function filterMenuShowing(sender, eventArgs) {
            column = eventArgs.get_column();
        }
    </script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ctl00_MainContent_RadGridExportData').removeClass('RadGrid RadGrid_DCPDMS');
        });
    </script>


    <asp:UpdateProgress runat="server" ID="upProgress" DisplayAfter="0">
        <ProgressTemplate>
            <div class="loading">
                <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" ToolTip="Loading" />
                Loading...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="pnllanguage" runat="server" Visible="false">
       <p style="font-size:small;color:black;text-align:left;">
        Welcome to the PDMS system.<br /><br />
       <u><b>Existing Providers - How to Re-enroll or Update Existing Information</b></u><br /><br />
       If you are an existing provider who needs to re-enroll or update existing information, your current enrollment appears in the “Other Providers with Same Tax ID” section at the bottom of this page. To re-enroll or update information, click on the “Manage” link in the last column of the row for the provider that requires an update or a re-enrollment. Once you have started your re-enrollment or update, the provider will appear in the “My Providers” section at the top of this page<br /><br />
        <u><b>New Providers or New Locations – How to Apply</b></u><br /><br />
       If you are requesting a new Medicaid ID, select the correct application type from the list below and then click “Begin Enrollment”.<br /><br />
       When completing any of the listed applications please note to complete the application in its entirety, including the upload of all requested required documents.  Failure to upload all necessary attachments will delay the processing of your application. Also please note that once an application is started you have <asp:Label ID="lblElapsedDays" runat="server" /> calendar days to complete your application or your information will be deleted and you will have to start over. 
       </p>
    </asp:Panel>

    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <div style="height: auto; min-height: 100px;">
                <div class="group-hdg">
                    <table style="width: 100%;" role="presentation">
                        <tr>
                            <td class="provider-home-btn" style="text-align: left;"><%--<span class="pageHeader2">My Providers</span>--%>
                                <asp:Button ID="btnMyProviders" runat="server" CssClass="buttonBoxFocus provider-btn-margin" Text="My Providers" ToolTip="My Providers" OnClick="btnMyProviders_Click" />
                                <div id="divHideSelectProvider" runat="server" style="display: none; visibility: hidden">
                                    <%-- OHPNM-7774 hide Select Provider Button; also removed the btnSelectProviders_Click for additional measures --%>
                                    <asp:Button ID="btnSelectProviders" runat="server" CssClass="buttonBoxFocus provider-btn-margin" Text="Select Provider" ToolTip="Select Provider" Enabled="false" OnClick="btnSelectProvidersVoid_Click" Visible="false" />
                                </div>
                                  <div id="div1" runat="server" style="display: none; visibility: hidden">
                                   <%-- OHPNM-8167 Since we have hidden the 'Select Provider' functionality, we should hide the 'Pending Agent Requests' since we are not letting the agents request access.  --%>
                                <asp:Button ID="btnPendingAgents" runat="server" CssClass="buttonBoxFocus provider-btn-margin" Text="Pending Agent Requests" ToolTip="Pending Agent Requests" OnClick="btnPendingAgents_Click" Visible="false" />
                                 </div>
                                <asp:Button ID="btnAccountAdmin" runat="server" CssClass="buttonBoxFocus provider-btn-margin" Text="Account Administration" ToolTip="Account Administration" OnClick="btnAccountAdmin_Click" Visible ="false" />
                                <asp:Button ID="btnAffiliateUpdate" runat="server" CssClass="buttonBoxFocus provider-btn-margin" Text="Affiliate Update" ToolTip="Affiliate Update" OnClick="btnAffiliateUpdate_Click" Visible ="false"  />
                                <asp:Button ID="btnDDAcountAdmin" runat="server" CssClass="buttonBoxFocus provider-btn-margin" Text="DD Account Administration" ToolTip="DD Account Administration" OnClick="btnDDAcountAdmin_Click" Visible ="false" />
                                <asp:Button ID="btnInternalApplication" runat="server" CssClass="buttonBoxFocus provider-btn-margin" Text="Internal Application" ToolTip="Internal Application" OnClick="btnInternalApplication_Click" />
                                <div style="display:none">
                                  <span class="pageHeader2 btn-align-right" style="text-align:left">Tax ID:</span><asp:Label ID="lblTaxID" runat="server" CssClass="pageHeader2" Text="565654" />
                                </div>
                                <div id="DownloadDataButtons" class="downloadbuttonStyle">
                                    <telerik:RadGrid ID="RadGridExportData" runat="server" Visible="true">
                                    <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                                        <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Provider Details" ForceTextWrap="true"  PageLeftMargin="50" PageRightMargin="50">
                                            <PageFooter>
                                                <RightCell Text="Page <?page-number?>" />
                                            </PageFooter>
                                        </Pdf>
                                    </ExportSettings>
                                    <MasterTableView AutoGenerateColumns="false">
                                        <Columns>
                                            <telerik:GridBoundColumn UniqueName="col1" HeaderText="Reg ID" DataField="RegID"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col2" HeaderText="Provider" DataField="ProviderName"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col3" HeaderText="Status" DataField="RegistrationStatusType"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col4" HeaderText="Provider Type" DataField="ProviderTypeName"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col5" HeaderText="NPI" DataField="NPI"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col6" HeaderText="Medicaid ID" DataField="MedicaidID"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col7" HeaderText="Specialty" DataField="SpecialtyTypeName"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col8" HeaderText="DD Contract Number" DataField="DD_Contract_num"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col9" HeaderText="DD Facility Number" DataField="DD_Faciity_num"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col10" HeaderText="Location" DataField="PracticeLocationZip"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col11" HeaderText="Effective Date" DataField="EffectiveDateTime" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col12" HeaderText="Submit Date" DataField="SubmitDateTime" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="col13" HeaderText="Revalidation Due Date" DataField="RevalidationDate" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                    </telerik:RadGrid>
                                    <asp:LinkButton ID="lnkBtnExcel" runat="server" ToolTip="Excel" OnClick="lnkBtnExcel_Click"><img src="../Images/Excel_24x24.png" alt="XLS" /></asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkBtnPDF" runat="server" ToolTip="PDF" OnClick="lnkBtnPDF_Click"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
                                </div>
                                <button type="button" id="divNewProvider" runat="server" class="buttonBoxFocus" onclick="window.location.href='NewProvider.aspx'">New Provider ? </button> 
                            </td>
                        </tr>
                    </table>
                </div>
                <br />

                <asp:Panel runat="server" ID="pnlAlertSuccess" Visible="false">
                    <div class="alert alert-success" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <strong>Congratulations!</strong> Dimensions Healthcare Associates has successfully completed registration <a href="#" class="alert-link">view provider file</a>.
                    </div>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlAlertAddlInfo" Visible="false">
                    <div class="alert alert-danger" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <span runat="server" id="spnAddlInfo"><strong>Immediate Action!</strong> </span>
                        <asp:HyperLink ID="hlnkAddlInfo" NavigateUrl="#" Text="Click here to update." ToolTip="Immediate Action!" runat="server" class="alert-link" />
                    </div>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlAlertEnrollmentAlert" Visible="false">
                    <div class="alert alert-danger" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <span runat="server" id="spnEnrollment"><strong>Immediate Action!</strong>  </span>
                        <asp:HyperLink ID="hlnkRenewal" NavigateUrl="#" Text="Click here to re-enroll." ToolTip="Immediate Action!" runat="server" class="alert-link" />
                    </div>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlAlertWarning" Visible="false">
                    <div class="alert alert-warning" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <span runat="server" id="spnAlertWarning"><strong>Heads up!</strong> </span>
                        <asp:HyperLink ID="hlnkAlertWarning" NavigateUrl="#" Text="Click here to renew." ToolTip="Heads up!" runat="server" class="alert-link" />
                    </div>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlAlertLicenseRenewal" Visible="false">
                    <div class="alert alert-warning" role="alert">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close" title=">Warning! PG2 Associates is nearing 30 day re-enrollment"><span aria-hidden="true">&times;</span></button>
                        <strong>Warning!</strong> PG2 Associates is nearing 30 day re-enrollment.</span>
                    </div>
                </asp:Panel>

                <%--                    <telerik:RadPersistenceManager ID="RadPersistenceManagerProviders" runat="server">
                        <PersistenceSettings>
                            <telerik:PersistenceSetting ControlID="gvMyProviders" />
                        </PersistenceSettings>
                    </telerik:RadPersistenceManager>--%>

                <telerik:RadGrid ID="gvMyProviders" role="definition" aria-label="MyProviders" TabIndex="0" runat="server" Width="100%" OnNeedDataSource="gvMyProviders_NeedDataSource" aria-busy="true"  
                    AllowFilteringByColumn="true" OnItemCommand="gvMyProviders_ItemCommand" OnSelectedIndexChanged="gvMyProviders_SelectedIndexChanged"
                    OnPageIndexChanged="gvMyProviders_PageIndexChanged" OnSortCommand="gvMyProviders_Sorting"  EnableAriaSupport="true"  OnItemDataBound="gvMyProviders_DataBound" Skin="PDMSModern" EnableEmbeddedSkins="false">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowSorting="true" PageSize="10"
                        AllowPaging="True" Width="100%" AutoGenerateColumns="false"
                        DataKeyNames="UserID, RegID" TableLayout="Auto" EnableHeaderContextMenu="true">
                        <NoRecordsTemplate>
                            No providers found
                        </NoRecordsTemplate>
                        <Columns>
                            <%--DataImageUrlFields="CustomerID" ImageHeight="110px" ImageWidth="90px" DataImageUrlFormatString="IMG/{0}.jpg" DataAlternateTextField="Category Type"
                                <telerik:GridImageColumn DataType="System.String"
                                    AlternateText="Provider Cat Type image" ImageAlign="Middle" ImageUrl="../Content/images/individual.png" ImageHeight="30px" ImageWidth="30px"
                                    AllowFiltering="false" HeaderStyle-Width="50px" />--%>
                            <telerik:GridTemplateColumn HeaderText="Reg ID" SortExpression="RegID" DataField="RegID" DataType="System.String">
                                <ItemTemplate>
                                    <asp:LinkButton ToolTip="Manage RegID" ID="btnManageRegID" runat="server" CommandName="ManageRegID"
                                        CommandArgument='<%# ((GridItem)Container).RowIndex %>' Text='<%# Eval("RegID") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Provider" SortExpression="ProviderName" DataField="ProviderName">
                                <ItemTemplate>
                                    <asp:LinkButton ToolTip="Manage Provider" ID="btnManageProvider" runat="server" CommandName="ManageProvider"
                                        CommandArgument='<%# ((GridItem)Container).RowIndex %>' Text='<%# Eval("ProviderName") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridBoundColumn DataField="ProviderName" HeaderText="Provider Name" SortExpression="ProviderName">
                            </telerik:GridBoundColumn>--%>
                            <telerik:GridBoundColumn DataField="RegistrationStatusType" HeaderText="Status" SortExpression="RegistrationStatusType">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="RadComboBoxStatus" DataTextField="RegistrationStatusType" Skin="PDMSModern" OnInit="RadComboBoxStatus_Init" ToolTip="Registraion Status"
                                        DataValueField="RegistrationStatusType" Width="100%" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("RegistrationStatusType").CurrentFilterValue %>'
                                        runat="server" OnClientSelectedIndexChanged="StatusTypeIndexChanged" AppendDataBoundItems="false" EnableEmbeddedSkins="false">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                        <script type="text/javascript">

                                            function StatusTypeIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                var value = args.get_item().get_value();
                                                if (value === "All") {
                                                    value = ''
                                                }
                                                tableView.filter("RegistrationStatusType", value, "EqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="ProviderTypeName" HeaderText="Provider Type" SortExpression="ProviderTypeName">
                                <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("ProviderTypeName") %>' runat="server" ID="Label2" />
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="NPI" HeaderText="NPI" SortExpression="NPI">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("NPI") %>' runat="server" ID="Label3" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("MedicaidID") %>' runat="server" ID="Label4" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="SpecialtyTypeName" HeaderText="Specialty" SortExpression="SpecialtyTypeName">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="RadComboBoxSpecialtyType" DataTextField="SpecialtyTypeName" OnInit="RadComboBoxSpecialtyType_Init" ToolTip="Specialty Type"
                                        DataValueField="SpecialtyTypeName" Width="100%" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("SpecialtyTypeName").CurrentFilterValue %>'
                                        runat="server" OnClientSelectedIndexChanged="SpecialtyTypeIndexChanged" AppendDataBoundItems="false" Skin="PDMSModern" EnableEmbeddedSkins="false">
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                        <script type="text/javascript">

                                            function SpecialtyTypeIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                var value = args.get_item().get_value();
                                                if (value === "All") {
                                                    value = ''
                                                }
                                                tableView.filter("SpecialtyTypeName", value, "EqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="DD_Contract_num" HeaderText="DD Contract Number" SortExpression="DD_Contract_num">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("DD_Contract_num") %>' runat="server" ID="Label5" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="DD_Faciity_num" HeaderText="DD Facility Number" SortExpression="DD_Faciity_num">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("DD_Faciity_num") %>' runat="server" ID="Label6" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="PracticeLocationZip" HeaderText="Location" SortExpression="PracticeLocationZip">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("PracticeLocationZip") %>' runat="server" ID="Label7" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="EffectiveDateTime" HeaderText="Effective Date"  SortExpression="EffectiveDateTime">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("EffectiveDateTime","{0:MM/dd/yyyy}") %>'  runat="server" ID="Label8" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="SubmitDateTime" HeaderText="Submit Date"  SortExpression="SubmitDateTime">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("SubmitDateTime","{0:MM/dd/yyyy}") %>'  runat="server" ID="Label9" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="RevalidationDate" HeaderText="Revalidation Due Date" SortExpression="RevalidationDate">
                                 <ItemTemplate>                                    
                                    <asp:Label Text='<%# Bind("RevalidationDate","{0:MM/dd/yyyy}") %>'  runat="server" ID="Label10" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridTemplateColumn HeaderText="Manage Agents" SortExpression="RegID" DataField="isAgents" UniqueName="lnkAgents">
                                <ItemTemplate>
                                    <asp:LinkButton ToolTip="Manage Agent" ID="btnManageAgent" runat="server" CommandName="ManageAgents"
                                        CommandArgument='<%# ((GridItem)Container).RowIndex %>' Text="Manage Agents" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Resizing AllowColumnResize="true" />
                        <ClientEvents OnFilterMenuShowing="filterMenuShowing" OnGridCreated="addscope"/>
                    </ClientSettings>
                    <FilterMenu OnClientShowing="MenuShowing" CssClass="gridviewFilter" />
                </telerik:RadGrid>
            </div>
            <br />
            <br />
            <asp:Panel ID="pnlSelectProviders" runat="server" Visible="false">
                <cc1:GroupBox ID="gbSelectProviders" runat="server" Width="80%" HorizontalAlign="Center" BorderStyle="Groove" BorderWidth="1" CssClass="groupBoxSP">
                     <br />           
                    <asp:Label ID="UNR11_ERR" runat="server" style="color:Red!important;" Visible="false" Text="* Select only one Provider at a time." CssClass="failureNotification" />
                <div class="boxPanelData">

             <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblMedicaidID" runat="server" Text="Medicaid ID" CssClass="formLabel200"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txt_MedicaidID" runat="server" MaxLength="10" CssClass="formField"/>
                </div>
            </div>
              <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbl4NPI" runat="server" Text="NPI" CssClass="formLabel200" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txt_NPI" runat="server" MaxLength="10" CssClass="formField"/>
                </div>
            </div>
              <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbl_TaxID" runat="server" Text="Tax ID" CssClass="formLabel200" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txt_TaxID" runat="server" MaxLength="10" CssClass="formField"/>
                </div>
            </div>
             <br></br>
             <div>
                 <div class="col-sm-8">
                        <asp:Label ID="lbl_ProvAdminValidation" runat="server" style="color:Red!important;" Visible="true" Text="* Enter 2 of the 3 criteria" CssClass="failureNotification" />
                  </div>
                  <div class="col-sm-8" style="margin-left:22.5%">
                        <asp:Label ID="lbl_ProvAdminError" runat="server" style="color:Red!important;" Visible="false" Text="" CssClass="failureNotification" />
                  </div>
             </div>
            </div>
            <div class="btnBox" style="margin-right:10%;">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" CausesValidation="true" Enabled="true" OnClick="btnSave_Click"/>
                <asp:Button ID="btnCancelProv" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" OnClick="btnCancel_Click"/>
            </div>
                </cc1:GroupBox>
                  <asp:HiddenField ID="hdnSelRegID" runat="server" />
                  <asp:HiddenField ID="hdnSelTaxID" runat="server" />
                  <asp:HiddenField ID="hdnSelMedID" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlSelProvDODD" runat="server" Visible="false">
                <cc1:GroupBox ID="gbSelProvDODD" runat="server" Width="80%" HorizontalAlign="Center" BorderStyle="Groove" BorderWidth="1" CssClass="groupBoxSP">
                     <br />           
                <div class="boxPanelData">
              <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbl_FacilityNo" runat="server" Text="Facility Number" CssClass="formLabel200"/>
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txt_FacilityNo" runat="server" MaxLength="10" CssClass="formField" />
                </div>
            </div>
              <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbl_ContractNo" runat="server" Text="Contract Number" CssClass="formLabel200" />
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txt_ContractNo" runat="server" MaxLength="10" CssClass="formField" />
                </div>
            </div>   
               <br></br>
             <div>
                  <div class="col-sm-8">
                    <asp:Label ID="lbl_ProvDODDValidation" runat="server" style="color:Red!important;" Visible="true" Text="* Enter any one" CssClass="failureNotification" />
                  </div>
                  <div class="col-sm-8" style="margin-left:11.2%">
                        <asp:Label ID="lbl_ProvDODDError" runat="server" style="color:Red!important;" Visible="false" Text="" CssClass="failureNotification" />
                  </div>
              </div>
            </div>
            <div class="btnBox" style="margin-right:10%;">
                <asp:Button ID="btn_Save_DODD" runat="server" Text="Save" CssClass="buttonBoxFocus" CausesValidation="true" Enabled="true" OnClick="btn_Save_DODD_Click" />
                <asp:Button ID="btn_Cancel_DODD" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" OnClick="btn_Cancel_DODD_Click" />
            </div>
                </cc1:GroupBox>
            </asp:Panel>
            <asp:Panel ID="pnlPendingAgents" runat="server" Visible="false">
                <cc1:GroupBox ID="gbPendingAgents" runat="server" Width="60%" HorizontalAlign="Center" BorderStyle="Groove" BorderWidth="1" CssClass="groupBoxPA">
                <div class="boxPanelData">
                    <br />
                <div style="float:left;margin-left:10%;text-align:center;">
                    <asp:Button ID="btnSelectAll" runat="server" Text="Select All" OnClick="btnSelectAll_Click" />
                </div>
                <br />
                <asp:GridView 
                    runat="server" 
                    Width="80%" 
                    ID="gvPendingAgents"  
                    AutoGenerateColumns="False" 
                    HorizontalAlign="Center" 
                    CssClass="gridViewSmallFont" 
                    EmptyDataText="No actions found."
                     DataKeyNames="UserId">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAgent" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ReadOnly="true" DataField="UserName" HeaderText="Agent/Accountant Name" />
                        <asp:BoundField ReadOnly="true" DataField="UserId" HeaderText="Agent/Accountant User ID" />
                    </Columns>
                    <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" /> 
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
            <div class="btnBox" style="margin-right:10%;">
                <asp:Button ID="btnSavePendingAgent" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSavePendingAgent_Click" CausesValidation="true" />
                <asp:Button ID="btnCancelPendingAgent" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" OnClick="btnCancel_Click" />
            </div>
                    </cc1:GroupBox>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
     <!-- ModalPopupExtender -->
<ajax:ModalPopupExtender ID="mpeSelProv" runat="server" PopupControlID="pnlSelProv" TargetControlID="ButtonDummy2" 
    BackgroundCssClass="modalBackground">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlSelProv" runat="server" CssClass="modalPopup" style="display:none;width:50%;height:auto;">    
    <asp:Panel ID="Panel2" CssClass="popHeader" runat="server" >
        <div class="popTitle">
            <asp:Label ID="Label1"  runat="server" Text="Provider Information"  />
        </div>
    </asp:Panel>  
     <asp:Panel ID="Panel3" runat="server" Style="margin-right:10px">      
        <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
            <asp:ValidationSummary ID="vsSelectProv" DisplayMode="List" runat="server" ValidationGroup="valSelectProv" ShowSummary="true" />         
           <div class="row">
                 <div class="col-sm-4  text-right">
                        <asp:Label ID="lblMedID" runat="server" Text="Medicaid ID*" CssClass="formLabel300" />
                </div>
                <div class="col-sm-8 text-left">
                    <asp:TextBox ID="txtMedID" runat="server" CssClass="formField" aria-Label="Medicaid ID" ToolTip="Medicaid Id" />
                    <asp:RequiredFieldValidator ID="valMedReqd" runat="server" ControlToValidate="txtMedID" Enabled="true" SetFocusOnError="true" 
                                    Display="Dynamic" Text="*"  ValidationGroup="valSelectProv" ErrorMessage="* Medicaid ID is required. Please include leading zeroes as applicable."></asp:RequiredFieldValidator>
                </div>   
           </div>
           <div class="row">         
                <div class="col-sm-4  text-right">
                        <asp:Label ID="lblTax" runat="server" Text="Tax ID*" CssClass="formLabel300" />
                </div>
                <div class="col-sm-8 text-left">
                    <asp:TextBox ID="txtTax" runat="server" CssClass="formField"  ToolTip="Tax Id"/>
                    <asp:RequiredFieldValidator ID="regTaxID" runat="server" ControlToValidate="txtTax"
                            Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                            ValidationGroup="valSelectProv" ErrorMessage="* Tax ID is required."></asp:RequiredFieldValidator>
                </div>             
           </div>
        </div>
    </asp:Panel><br />
    <div class="btnBox btnBoxCenter" style="padding-top: 10px; width: 93%;">
         <asp:Button runat="server" ID="btnSelectSave" Text="Save" CssClass="buttonBox buttonBoxFocus" OnClick="btnSelectSave_Click" ValidationGroup="valSelectProv" ToolTip="Save" />
         <asp:Button runat="server" ID="btnSelCancel" Text="Cancel" CssClass="buttonBox" OnClick="btnSelCancel_Click" ToolTip="Cancel" />
    </div>
</asp:Panel>
    </div>
</asp:Panel>
    <ajax:ModalPopupExtender ID="Mod_ProviderAdminConfirm" runat="server" PopupControlID="pnl_ProviderAdminConfirm" TargetControlID="ButtonDummy3" 
         BackgroundCssClass="addUserModalBackground" PopupDragHandleControlID="pnl_ProviderAdminConfirm">
    </ajax:ModalPopupExtender>
<asp:Panel ID="pnl_ProviderAdminConfirm" Width="36%" Height="20%" runat="server" CssClass="addUserModalPopup" align="left" Style="display: none;">
    <asp:Panel ID="pnl_ProviderAdmin" Style="cursor: move; padding: 5px;" BackColor="#205794" runat="server" HorizontalAlign="Left">
        <div style="text-align: left">
            &nbsp;&nbsp;
            <asp:Label ID="lbl_Confirmation" CssClass="bodyTextBold" runat="server" Text="Confirmation" ForeColor="White" />
        </div>
    </asp:Panel>
    <br /> 
    <div style="text-align: left;"  class="tablepad">
        <div class="row">
            <div class="col-sm-12 text-left" style="margin-left:14%">
                <span class="formLabel">Request has been sent to the Provider Administrator for the current record.</span>
            </div>
        </div>
    </div>
    <div class="btnBox" style="margin-right:44%;margin-top:3%" >
        <asp:Button ID="Btn_Ok" runat="server" Text="Ok" CssClass="buttonBoxFocus" OnClick="Btn_Ok_Click"  />
    </div>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy2" aria-Label="Dummy Button" Style="display: none" Text="ButtonDummy" ToolTip="hidden"/>
<asp:Button runat="server" ID="ButtonDummy3" aria-Label="Dummy Button" Style="display: none" Text="ButtonDummy" ToolTip="hidden"/>
    <script type="text/javascript">

        function addscope(sender, args) {
            var gridTable = sender.get_masterTableView().get_element();
            var rows = gridTable.getElementsByTagName("tr");
            for (var i = 0; i < rows.length; i++) {
                $(rows[i]).attr("scope", "col");
            }
            var cells = gridTable.getElementsByTagName("td");
            for (var i = 0; i < cells.length; i++) {
                $(cells[i]).attr("scope", "col");
            }
            var headers = gridTable.getElementsByTagName("th");
            for (var i = 0; i < headers.length; i++) {
                $(headers[i]).attr("scope", "col");
            }
        }


        function clearGridTextBoxes() {

            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn1').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn2').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn3').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn4').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn5').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn6').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn7').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn8').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn9').val('');
            $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn10').val('');

        }


        $(document).ready(function ()
        {
            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn1');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_TemplateColumn2');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_NPI');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_MedicaidID');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_DD_Contract_num');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_DD_Faciity_num');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_PracticeLocationZip');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_EffectiveDateTime');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_SubmitDateTime');
            Text.removeAttr("alt");

            Text = $('#ctl00_MainContent_gvMyProviders_ctl00_ctl02_ctl02_FilterTextBox_RevalidationDate');
            Text.removeAttr("alt");

            var $table = $(".RadGrid").children("table");
            $table.removeAttr("summary");

            var $table1 = $(".RadComboBox").children("table");
            $table1.removeAttr("summary");

            var $table2 = $(".RadComboBox").children("table");
            $table2.attr("role", "presentation");

            var $table3 = $(".RadGrid").children("table").children("tfoot").children("tr").children("td").children("table");
            $table3.removeAttr("summary");
            $table3.attr("role", "presentation");

            var $table4 = $(".RadGrid").children("table").children("tfoot").children("tr").children("td").children("table").children("caption");
            $table4.remove("caption");

            var $table5 = $(".RadComboBox").children("table").children("caption");
            $table5.remove("caption");

            var $table6 = $(".rgPager").children("td").children("table").children("thead").children("tr").children("th");
            $table6.remove("th");

        });
    </script>
 </asp:Content>
