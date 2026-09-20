<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ProviderSpecialtySearch1, App_Web_wenzyumt" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script src="../Scripts/customizeTelerik.js" type="text/javascript"></script>

<style type="text/css">
    select {
        min-width: 90%;
    }

    .align-checkbox {
        text-align: center;
    }

    @media only screen and (max-width: 400px){
        .ohio-select-select-el{
            font-size: 10px;
        }
    }

</style>


 <div>
     <h1> <Legend style="border-bottom:1px solid #65659f;"><span class="pssSectionHeader" id="pssSectionHeader_1" runat="server">Search Criteria</span></Legend> </h1>
</div>

<asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
     <div class="container-fluid">
         <div class="row">
              <asp:Label ID="lblErrorText" runat="server" ForeColor="Red" Visible="false" />
         </div>
         <div class="row">
             <div class="col-sm-2  text-right">
                 <asp:Label ID="lblNPI" runat="server" Text="NPI" CssClass="formLabel" />
              </div>
              <div class="col-sm-10 text-left">
                 <asp:TextBox ID="txtNPI" CssClass="formField formField" runat="server" MaxLength="10" />
             </div>
         </div>
          <div class="row">
            <div class="col-sm-2  text-right">
               <asp:Label ID="lblMedicaidID" runat="server" Text="Medicaid ID" CssClass="formLabel" />
              </div>
                 <div class="col-sm-10 text-left">
                    <asp:TextBox ID="txtMedicaidID" CssClass="formField formField" runat="server" MaxLength="7" />
                </div>           
          </div>
         <div class="btnBox btnBoxCenter">
            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ProviderSearch" />
            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />        
         </div>
    </div>
    <div>
        <h1> <Legend style="border-bottom:1px solid #65659f;"><span class="pssSectionHeader1" id="Span1" runat="server">Search Results</span></Legend> </h1>
    </div>
     <div id="divExport" style="width: 100%; text-align: right;">
     <asp:HiddenField ID="hdnRowCount" runat="server" />
     <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="return exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="XLS" /></asp:LinkButton>

     <telerik:RadGrid ID="grdSearchResultExport" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
         AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
         <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
             <Excel Format="Biff" />
         </ExportSettings>
         <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
             DataKeyNames="REG_SPECIALTY_ID" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
             <Columns>
                 <telerik:GridBoundColumn DataField="MEDICAID_ID"                       HeaderText="Medicaid ID"      SortExpression="MEDICAID_ID"          />
                 <telerik:GridBoundColumn DataField="MMIS_PROVIDER_TYPE_ID"               HeaderText="Provider Type" SortExpression="MMIS_PROVIDER_TYPE_ID" />
                 <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_NAME"                        HeaderText="Specialty"      SortExpression="SPECIALTY_TYPE_NAME" />
                 <telerik:GridBoundColumn DataField="PRIMARY_FLAG_EXPORT"              HeaderText="Primary"             SortExpression="PRIMARY_FLAG_EXPORT" />
                 <telerik:GridBoundColumn DataField="START_DATE"                      HeaderText="Start Date"     SortExpression="START_DATE"     DataFormatString="{0:MM/dd/yyyy}" />
                 <telerik:GridBoundColumn DataField="END_DATE"                        HeaderText="End Date"       SortExpression="END_DATE"       DataFormatString="{0:MM/dd/yyyy}" />                 
                 <telerik:GridBoundColumn DataField="ENROLL_STATUS_DESC"  HeaderText="Enroll Status"            SortExpression="ENROLL_STATUS_DESC" />
             </Columns>
         </MasterTableView>
     </telerik:RadGrid>
    </div>
    <br /> <br />
    <div id="pnlSpecialtySearchResults" runat="server">
    <div class="divGrid">
        <telerik:radgrid id="grdSpecSearchResults" role="definition" aria-label="Specialties" tabindex="0" runat="server" width="100%" aria-busy="true" skin="PDMSModern" allowsorting="True"
            allowfilteringbycolumn="true"
            enableariasupport="true" enableembeddedskins="false" allowpaging="True" onpageindexchanged="grdSpecSearchResults_PageIndexChanging">
            <groupingsettings casesensitive="false" />
            <clientsettings>
                <resizing allowcolumnresize="true" />
                <clientevents onfiltermenushowing="filterMenuShowing" />
            </clientsettings>
            <filtermenu onclientshowing="MenuShowing" cssclass="gridviewFilter" />
            <mastertableview gridlines="None" datakeynames="REG_SPECIALTY_ID"
                autogeneratecolumns="false" allowpaging="true" PageSize="10">
                <norecordstemplate>
                    No records found
                </norecordstemplate>
                <columns>
                    <telerik:gridboundcolumn datafield="MEDICAID_ID" headertext="Medicaid ID" uniquename="MedcaidID" sortexpression="MEDICAID_ID"></telerik:gridboundcolumn>
                    <telerik:gridboundcolumn datafield="MMIS_PROVIDER_TYPE_ID" headertext="Provider Type" uniquename="ProviderTypeID" sortexpression="MMIS_PROVIDER_TYPE_ID"></telerik:gridboundcolumn>
                    <telerik:gridboundcolumn datafield="SPECIALTY_TYPE_NAME" headertext="Specialty" uniquename="SpecialtyTypeName" sortexpression="SPECIALTY_TYPE_NAME"></telerik:gridboundcolumn>
                     <telerik:gridtemplatecolumn datafield="PRIMARY_FLAG" headertext="Primary" uniquename="Primary" sortexpression="PRIMARY_FLAG" datatype="System.Boolean">
                         <itemtemplate>
                             <asp:Label ID="PrimaryFlagLbl" runat="server" Text='<%# Convert.ToBoolean(Eval("PRIMARY_FLAG")) == true ? "Yes" : "No" %>'></asp:Label>
                         </itemtemplate>
                     </telerik:gridtemplatecolumn>
                    <telerik:gridboundcolumn datafield="START_DATE" headertext="Start Date" dataformatstring="{0:MM/dd/yyyy}" sortexpression="START_DATE" />

                    <telerik:gridboundcolumn datafield="END_DATE" headertext="End Date" dataformatstring="{0:MM/dd/yyyy}" sortexpression="END_DATE" />

                    <telerik:gridboundcolumn datafield="ENROLL_STATUS_DESC" headertext="Enroll Status" uniquename="EnrollStatus">
                        <filtertemplate>
                            <telerik:radcombobox id="RadComboBoxEnrollStatus" aria-label="Enroll Status" enableariasupport="true" datatextfield="ENROLL_STATUS_DESC" skin="PDMSModern"
                                datavaluefield="ENROLL_STATUS_DESC" width="100%" enableviewstate="false"
                                runat="server" onclientselectedindexchanged="EnrollStatusIndexChanged" enableembeddedskins="false">
                            </telerik:radcombobox>
                            <telerik:radscriptblock id="RadScriptEnrollStatus" runat="server">
                                <script type="text/javascript">
                                    function EnrollStatusIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var filtervalue = args.get_item().get_value();
                                        if (filtervalue == "All") filtervalue = "";
                                        tableView.filter("EnrollStatus", filtervalue, "EqualTo");
                                    }
                                    $(document).ready(function () {
                                        Input = $('.rcbReadOnly').children("td").children("input");
                                        Input.attr('aria-label', 'Select Enroll Status');
                                    });
                                </script>
                            </telerik:radscriptblock>
                        </filtertemplate>
                    </telerik:gridboundcolumn>
                </columns>
             <pagerstyle mode="NextPrevAndNumeric" alwaysvisible="true" />
            </mastertableview>
        </telerik:radgrid>
    </div>
 </div>
    
</asp:Panel>