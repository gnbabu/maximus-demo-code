<%@ page title="Provider Directory" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_PublicSearchNew, App_Web_qtcaivva" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
<%-- Provider Directory--%>
    <div style="display:inline;">
        <span style="text-align:center"><asp:Label ID="lblTitle" runat="server" Text="Find a Provider" /></span>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        
        .RadComboBox_PDMSModern td.rcbArrowCell
        {
            background-position-x: 0 !important;
            background-position-y: 0 !important;
        }
        table {
            text-align: center;
        }

        th, td {
            padding: 5px; 
        }

        .align-checkbox {
            text-align: center;
        }

        .popover {
            width: 240px;
        }

        @media only screen and (max-width: 760px) {
           input[type=text], input[type=password], textarea, select {
                width: 100% !important;
                min-width: 100px !important;
            }
            .col-sm-12{
                padding-right: 0px;
                padding-left: 0px;
            }
            .WhiteBox{
                padding: 0px;
            }   
        }
        @media only screen and (max-width: 990px){
            input[type=text], input[type=password], textarea, select {
                width: 100% !important;
                min-width: 100px !important;
            }
        }
        @media only screen and (max-width: 400px){
            .form-control {
                font-size: 11px;
            }
            .WhiteBox{
                padding: 0px;
            }
        }

        .rddlPopup .rddlItem {
            background-color: white;
        }       
    </style>
    <script type="text/javascript">
        function pageLoad() {
            try {
                $('[data-toggle="popover"]').popover()
            }
            catch (err) {
                console.log('that popover method does not exist at this point: ' + err);
            }
        }
        function handleStateChange() {
            var element = $find("<%= rcbCounty.ClientID %>");
            if (document.getElementById('ctl00_MainContent_ddlState').value == 'OH') {
                element.enable();
            } else {
                element.disable();  
            }

            if (document.getElementById('ctl00_MainContent_ddlState').selectedIndex == 0) {
                document.getElementById('ctl00_MainContent_rfvddlState').style.display = 'inline';
            } else {
                document.getElementById('ctl00_MainContent_rfvddlState').style.display = 'hidden';
            }
        }

        function validateFields() {
            if (document.getElementById('ctl00_MainContent_ddlState').selectedIndex == 0) {
                document.getElementById('ctl00_MainContent_rfvddlState').style.display = 'inline';
            }

            if (document.getElementById('ctl00_MainContent_ddlHealthPlan').selectedIndex == 0) {
                document.getElementById('ctl00_MainContent_rfvddlHealthPlan').style.display = 'inline';
                return false;
            }

            if ((document.getElementById('ctl00_MainContent_txtZip').value == '') && (document.getElementById('ctl00_MainContent_txtCity').value == '') && (document.getElementById('ctl00_MainContent_rcbCounty_Input').value == '')) {
                document.getElementById('ctl00_MainContent_rfvtxtZip').style.display = 'block';
                return false;
            }
            return true;

        }

        function exportPopup() {
            var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
            if (rowCount > 10000) {
                $("#divRowCount").dialog();
            }
        }
        function OnClientFocusHandler(sender, eventArgs) {
            if (!sender.get_dropDownVisible()) {
                sender.showDropDown();
            }
        }

        $(document).ready(function () {
            var $table = $(".RadComboBox").find("table");
            $table.removeAttr("summary");
            $table.attr("role", "presentation");

        })
    </script>

    <div class="WhiteBox">
       <div style="width:98%;text-align:right;"> <a href="PublicSearchAPI.aspx" id="lnkAPI" runat="server" visible="false"><b>API</b></a></div>
         <cc1:GroupBox ID="gbSearch" HorizontalAlign="Center" Width="98%" Caption="<span style='display:none'>Public Search</span>" runat="server">
           <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
            <div>
                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="PublicSearch" ShowSummary="true" />
            </div>
            <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
                <asp:UpdateProgress ID="updateProgress" runat="server">
                    <ProgressTemplate>
                        <div style="padding-right: 30px">
                            <img src="../Images/ajax-loader.gif" alt="" />
                            Loading ...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div id="divPublicSearch">
                    <div id="divPublicSearchG1G2" class="col-sm-12 col-md-12">
                        <div id="divPublicSearchGroup1" class="col-sm-12 col-md-12 col-lg-6">
                            <div class="row">
                                <div class="col-sm-12 text-left">
                                    <span class="formLabelSmall">Provider Information / Health Plan</span>
                                    <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblHealthPlan" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlHealthPlan">Health Plan <span class="RedAsterisk">*</span></asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlHealthPlan" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Health Plan" />
                                    <asp:RequiredFieldValidator ID="rfvddlHealthPlan" InitialValue="-1" ValidationGroup="PublicSearch" runat="server" ControlToValidate="ddlHealthPlan" ErrorMessage="* Health Plan is required" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblProgram" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlProgram">Program</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Program" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblProviderType" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbProviderType">Provider Type</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbProviderType" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="Provider Type"
                                         EnableCheckAllItemsCheckBox="false" Filter="Contains" BackColor="White" OnItemChecked="rcbProviderType_ItemChecked" AutoPostBack="false"
                                        Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength"  OnClientFocus="OnClientFocusHandler"
                                        Width="100%">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblFacilityType" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlFacilityType">Facility Type</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlFacilityType" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Facility Type" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblPrimaryCareProvidersOnly" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlPrimaryCareProviders">Primary Care Providers Only</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlPrimaryCareProviders" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Primary Care Providers Only" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblProviderNameFullPath" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtOrgName">Provider Name (Full or Partial)</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="txtOrgName" class="form-control" aria-label="..." runat="server" />
                                        <span class="input-group-btn">
                                            <asp:DropDownList ID="ddlOrgNameSearch" Style="min-width: 100px; margin: 0px; padding: 0px" CssClass="btn btn-default active" runat="server" ToolTip="Provider Name (Full or Partial)">
                                                <asp:ListItem Value="equals" Text="Equal to" Selected="True" />
                                                <asp:ListItem Value="begins" Text="Begins with" />
                                                <asp:ListItem Value="contains" Text="Contains" />
                                                <asp:ListItem Value="ends" Text="Ends with" />
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblDMEProductsServices" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbDMEProductsServices">DME Products & Services</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox  RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbDMEProductsServices" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="DME Products & Services"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength"  BackColor="White" 
                                        Width="100%" OnClientFocus="OnClientFocusHandler">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div id="divPublicSearchGroup2" class="col-sm-12 col-md-12 col-lg-6">

                            <div class="row">
                                <div class="col-sm-12 text-left">
                                    <span class="formLabelSmall">Patient Details</span>
                                    <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPatientsAsYoungAs" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtAcceptsPatientsAsYoungAs">Accepts Patients As Young As</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox ID="txtAcceptsPatientsAsYoungAs" class="form-control" aria-label="..." runat="server" ToolTip="Accepts Patients As Young As" />
                                    <asp:RegularExpressionValidator ID="revtxtAcceptsPatientsAsYoungAs" runat="server" ControlToValidate="txtAcceptsPatientsAsYoungAs"
                                        ValidationExpression="^[0-9]\d*$" ErrorMessage="* Enter a numeric value for Accepts Patients As Young As."
                                        Enabled="true" SetFocusOnError="true" Text=""
                                        ValidationGroup="PublicSearch" Display="None" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPatientsAsOldAs" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtAcceptsPatientsAsOldAs">Accepts Patients As Old As</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox ID="txtAcceptsPatientsAsOldAs" class="form-control" aria-label="..." runat="server" CausesValidation="true" ValidationGroup="PublicSearch" ToolTip="Accepts Patients As Old As" />
                                    <asp:RegularExpressionValidator ID="revtxtAcceptsPatientsAsOldAs" runat="server" ControlToValidate="txtAcceptsPatientsAsOldAs"
                                        ValidationExpression="^[0-9]\d*$" ErrorMessage="* Enter a numeric value for Accepts Patients As Old As."
                                        Enabled="true" SetFocusOnError="true" Text=""
                                        ValidationGroup="PublicSearch" Display="None" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPatientsofGender" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsPatientsofGender">Accepts Patients of Gender</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlAcceptsPatientsofGender" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts Patients of Gender" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsNewPatients" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsNewPatients">Accepts New Patients</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlAcceptsNewPatients" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts New Patients" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsNewborns" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsNewborns">Accepts Newborns</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlAcceptsNewborns" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts Newborns" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPregnantWomen" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsPregnantWomen">Accepts Pregnant Women</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlAcceptsPregnantWomen" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts Pregnant Women" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="divPublicSearchG3G4" class="col-sm-12 col-md-12">
                        <div id="divPublicSearchGroup3" class="col-sm-12 col-lg-6">

                            <div class="row">
                                <div class="col-sm-12 text-left" style="margin-top: 10px;">
                                    <span class="formLabelSmall">Location</span>
                                    <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblrcbCounty" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbCounty">County</asp:Label>  <asp:RequiredFieldValidator ID="rfvrcbCounty" runat="server" ControlToValidate="rcbCounty" InitialValue="*" ErrorMessage="County is required" Display="Dynamic" />
                                </div>
                                <div class="col-sm-8 text-left">
                                   <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbCounty" runat="server" CheckBoxes="true" AllowCustomText="false" ToolTip="County"
                                         EnableCheckAllItemsCheckBox="true" Filter="Contains" BackColor="White" AutoPostBack="false"
                                        Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength"  OnClientFocus="OnClientFocusHandler"
                                        Width="100%">
                                    </telerik:RadComboBox>
                                </div> 
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lbltxtCity" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtCity">City</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="txtCity" class="form-control" aria-label="..." runat="server" ToolTip="City" />
                                        <asp:RegularExpressionValidator ID="revtxtCity" runat="server" ControlToValidate="txtCity"
                                            ValidationExpression="^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$" ErrorMessage="* Enter only aphabetic characters for City."
                                            Enabled="true" SetFocusOnError="true" Text=""
                                            ValidationGroup="PublicSearch" Display="None" />
                                        <span class="input-group-btn" style="padding-right: 0px; margin-left: 0px">

                                            <asp:DropDownList ID="ddlCitySearch" Style="min-width: 100px; margin: 0px; padding: 0px" CssClass="btn btn-default active" runat="server" ToolTip="City Search Type" >
                                                <asp:ListItem Value="equals" Text="Equal to" Selected="True" />
                                                <asp:ListItem Value="begins" Text="Begins with" />
                                                <asp:ListItem Value="contains" Text="Contains" />
                                                <asp:ListItem Value="ends" Text="Ends with" />
                                            </asp:DropDownList>
                                        </span>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlState" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlState">State <span class="RedAsterisk">*</span></asp:Label>
                                </div>
                               <div  class="col-sm-8 text-left" >
                                   <asp:DropDownList  ID="ddlState" runat="server"  OnSelectedIndexChanged="SelectCounty_OnSelectedStateIndexChanged"   ToolTip="State" 
                                       CssClass="form-control unsetPublicSearchDDLLength" ></asp:DropDownList>
                                   <asp:RequiredFieldValidator ID="rfvddlState" runat="server" ControlToValidate="ddlState"
                                        InitialValue="*" ErrorMessage="State is required" Enabled="true" SetFocusOnError="true" ValidationGroup="PublicSearch" Display="Dynamic" />
								</div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lbltxtZip" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtZip">
                                     
                                        <span tabindex="0" class="publicSearchLabels">Zip Code <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_DIRECTORY_POPUP_ZIP' runat='server' Text='Radius searches are only available if a Zip Code is entered' />" ><asp:Image ID="imgInfoIcon" AlternateText="INFO" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>


                                    </asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" ToolTip="Zip Code" />
                                    <asp:RegularExpressionValidator ID="revtxtZip" runat="server" ControlToValidate="txtZip"
                                        ValidationExpression="^[0-9]\d*$" ErrorMessage="* Enter a numeric value for ZipCode."
                                        Enabled="true" SetFocusOnError="true" Text=""
                                        ValidationGroup="PublicSearch" Display="Dynamic" />
                                    <asp:RequiredFieldValidator ID="rfvtxtZip" runat="server" ControlToValidate="txtZip"
                                        InitialValue="" ErrorMessage="You must enter a County, City or Zip Code to complete the search." Enabled="false" SetFocusOnError="true" ValidationGroup="PublicSearch" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlRadiusMiles" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlRadiusMiles">Radius (Miles) <span class="RedAsterisk">*</span></asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlRadiusMiles" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Radius (Miles)" />
                                    <asp:RequiredFieldValidator ID="rfvddlRadiusMiles" runat="server" ControlToValidate="ddlRadiusMiles"
                                        InitialValue="*" ErrorMessage="Radius is required" Enabled="true" SetFocusOnError="true" ValidationGroup="PublicSearch" Display="Dynamic" />
                                </div>
                            </div>
                        </div>
                        <div id="divPublicSearchGroup4" class="col-sm-12 col-lg-6">

                            <div class="row">
                                <div class="col-sm-12 text-left" style="margin-top: 10px;">
                                    <span class="formLabelSmall">Additional Provider Details</span>
                                    <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlProviderSpeciality" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbProviderSpeciality">Provider Speciality</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbProviderSpeciality" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="Provider Speciality"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" Filter="Contains" BackColor="White"
                                        Width="100%">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlProviderGender" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlProviderGender">Provider Gender</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlProviderGender" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Provider Gender" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlHospitalAffiliation" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbHospitalAffiliation">Hospital Affiliation</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbHospitalAffiliation" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="Hospital Affiliation"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" Filter="Contains" BackColor="White"
                                        Width="100%" OnClientFocus="OnClientFocusHandler">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlLanguagesSpoken" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbLanguagesSpoken">Languages Spoken</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbLanguagesSpoken" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="Languages Spoken"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" Filter="Contains" BackColor="White"
                                        Width="100%" OnClientFocus="OnClientFocusHandler">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlSpecializedTraining" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbSpecializedTraining">Specialized Training</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbSpecializedTraining" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="Specialized Training"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" BackColor="White"
                                        Width="100%" OnClientFocus="OnClientFocusHandler">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlCulturalCompetencies" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbCulturalCompetencies">Cultural Competencies</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbCulturalCompetencies" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="Cultural Competencies"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" BackColor="White"
                                        Width="100%" OnClientFocus="OnClientFocusHandler">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlADAAccommodations" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbADAAccommodations">ADA Accommodations</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbADAAccommodations" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="ADA Accommodations"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" BackColor="White"
                                        Width="100%" OnClientFocus="OnClientFocusHandler">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlBoardCertifications" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbBoardCertifications">Board Certifications</asp:Label>

                                </div>
                                <div class="col-sm-8 text-left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbBoardCertifications" runat="server" CheckBoxes="true" AllowCustomText="true" ToolTip="Board Certifications"
                                        EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" BackColor="White"
                                        Width="100%" OnClientFocus="OnClientFocusHandler">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlTelehealth" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlTelehealth">Telehealth</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlTelehealth" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Telehealth" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlCHIP" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlCHIP">CHIP</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlCHIP" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="CHIP" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlNewMedicaid" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlNewMedicaid">New Medicaid Patients</asp:Label>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:DropDownList ID="ddlNewMedicaid" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts New Medicaid Patients" />
                                </div>
                            </div>


                        </div>
                    </div>



                </div>
                <div class="row" style="text-align: center">
                    <br />
                    <br />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" CausesValidation="true" ValidationGroup="PublicSearch" ToolTip="Search" OnClientClick="return validateFields();" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" ToolTip="Clear" />
                    <%-- <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />--%>
                </div>
            </asp:Panel>
        </cc1:GroupBox>
        <br />
        <div style="width: 100%; text-align: right">
            <asp:HiddenField ID="hdnRowCount" runat="server" />
            <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
            <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
                <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
            </div>
            <asp:Panel ID="pnlMain" runat="server">
            <telerik:RadGrid ID="gvProviders" runat="server" AllowCustomPaging="false" AllowSorting="true" PageSize="50" OnItemCommand="gvProviders_ItemCommand"
                OnSortCommand="gvProviders_SortCommand" OnPageIndexChanged="gvProviders_PageIndexChanged" Skin="PDMSModern" EnableEmbeddedSkins="false"
                AutoGenerateColumns="false" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
                <GroupingSettings CaseSensitive="false" />
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="false">
                    <Pdf PageHeight="8.5in" PageWidth="15in" PageTitle="Provider Search" ForceTextWrap="true"  PageLeftMargin="50" PageRightMargin="50">
                        <PageFooter>
                            <RightCell Text="Page <?page-number?>" />
                        </PageFooter>
                    </Pdf>
                    <Excel Format="Biff" />
                </ExportSettings>
                <MasterTableView Width="100%" AllowSorting="true" AllowPaging="true" PageSize="50" AutoGenerateColumns="false" TableLayout="Auto"
                    DataKeyNames="REG_ID,REG_ADDRESS_ID" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Provider Name" SortExpression="OrganizationName">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkReview"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# ((Telerik.Web.UI.GridItem)Container).RowIndex %>'
                                    CommandName="ReviewRow"
                                    Text='<%# Eval("OrganizationName") %>'
                                    CssClass="gridLink" Font-Bold="false" ToolTip="Review Provider" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="FacilityName" HeaderText="Facility Name" SortExpression="FacilityName" HeaderTooltip="Facility Name" />
                        <telerik:GridBoundColumn DataField="HealthPlans" HeaderText="Plan" SortExpression="HealthPlans" HeaderTooltip="Plan" />
                        <telerik:GridBoundColumn DataField="ProviderTypeName" HeaderText="Provider Type" SortExpression="ProviderTypeName" HeaderTooltip="Provider Type" />
                        <telerik:GridBoundColumn DataField="SpecialtyTypeName" HeaderText="Primary Specialty" SortExpression="SpecialtyTypeName" HeaderTooltip="Primary Specialty" />
                        <telerik:GridBoundColumn DataField="ADDRESS_1" HeaderText="Address 1" SortExpression="ADDRESS_1" HeaderTooltip="Address 1" />
                        <telerik:GridBoundColumn DataField="ADDRESS_2" HeaderText="Address 2" SortExpression="ADDRESS_2" HeaderTooltip="Address 2" />
                        <telerik:GridBoundColumn DataField="ContactCity" HeaderText="City" SortExpression="ContactCity" HeaderTooltip="City" />
                        <telerik:GridBoundColumn DataField="ContactState" HeaderText="State" SortExpression="ContactState" HeaderTooltip="State"  />
                        <telerik:GridBoundColumn DataField="ContactZip" HeaderText="Zip" SortExpression="ContactZip" HeaderTooltip="Zip" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
                </asp:Panel>
        </div>
        <cc2:MessageBox ID="MessageBox2" runat="server" />

        <ajax:ModalPopupExtender ID="mpeReviewProvider" runat="server" PopupControlID="pnlViewProvider" TargetControlID="btnDummy"
            RepositionMode="RepositionOnWindowScroll" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlViewProvider">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlViewProvider" runat="server" CssClass="modalPopup" align="center" Style="display: none; width: 55%; height: 80%;" ScrollBars="Auto">
            <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                <div class="popTitle">Provider Information</div>
            </asp:Panel>
            <asp:Panel ID="pnlAdminMaintenance" runat="server" Style="margin-right: 10px; margin-left: 20px;">
                <asp:MultiView ID="mltViewProvider" runat="server" ActiveViewIndex="0" EnableViewState="true">
                    <asp:View ID="vwProvider" runat="server">
                        <div>
                            <asp:ValidationSummary ID="valExpressSummary" runat="server" DisplayMode="List" ValidationGroup="ExpressMaintSelection" ShowSummary="true" />
                        </div>
                        <br />
                        <%-- <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">--%>
                        <div id="divPublicSearchProviderInfoPopUp" class="row">
                            <div class="col-sm-10 text-left">
                                <div class="row">
                                    <div class="col-sm-12 text-left">
                                        <span class="formLabel200">Provider Information</span>
                                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">
                                        <span class="formLabel wd150">Provider name</span>
                                    </div>
                                    <div class="col-sm-9 text-left" style="white-space: inherit;">
                                        <asp:Label ID="lblProviderName" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">
                                        <span class="formLabel wd150">Address</span>
                                    </div>
                                    <div class="col-sm-9 text-left" style="white-space: inherit;">
                                        <asp:Label ID="lblAddress" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">
                                        <span class="formLabel wd150">City, State, Zip</span>
                                    </div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblCityStateZip" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">County</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblCounty" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Specialty</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblSpecialty" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Accepting New Patients</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblAcceptingNewPatients" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Gender</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblGender" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Hospital Affiliation(s)</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblHospitalAffiliations" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Board Certification</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblBoardCertification" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12 text-left">
                                        <br />
                                        <span class="formLabel200">Office Information</span>
                                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Monday</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeMon" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Tuesday</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeTue" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Wednesday</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeWed" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Thursday</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeThu" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Friday</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeFri" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Saturday</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeSat" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Sunday</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeSun" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Office Phone</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficePhone" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12 text-left">
                                        <br />
                                        <span class="formLabel200">Other Information</span>
                                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Cultural Competencies</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblCulturalCompetenciesLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">ADA Accommodations</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblOfficeAccommodationsLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Languages Spoken</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblLanguagesSpokenLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">DME Products and Services</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblDMEProductsLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Telehealth</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblTelehealthLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">CHIP</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblCHIPLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right"><span class="formLabel wd150">Accepts New Medicaid Patients</span></div>
                                    <div class="col-sm-9 text-left"  style="white-space: inherit;">
                                        <asp:Label ID="lblNewMedicaidLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <asp:HyperLink ID="hlGoogleMaps" runat="server" Text="Get Directions" Target="_blank"></asp:HyperLink>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
            <div class="row text-center" style="padding-top: 20px; padding-right: 10px;">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>
            <br />
            <br />
        </asp:Panel>
        <asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy" />
        <script>
            $(document).ready(function () {
                // Get the button elementvar 
                button = $('.rcbActionButton'); // Set the name attribute
                button.attr('title', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
                button.attr('aria-label', 'Provider Type');
            });
        </script>
    </div>
</asp:Content>

