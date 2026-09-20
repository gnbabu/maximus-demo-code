<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OrgInfo, App_Web_wenzyumt" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/EnrollmentHistory.ascx" TagPrefix="uc" TagName="EnrollmentHistory" %>

<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupOrgEventHandlers);
    });
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9 ]/g, '');
    }
    function setupOrgEventHandlers() {
        $("#divRequestedEffectiveDateInfo").hide();
        $("#tblImmigration").hide();
        $("#tblImmigrationAlien").hide();
        $("#tblImmigrationLbl").hide();


        $(".what-is-this-link").mouseover(function () {
            $("#divRequestedEffectiveDateInfo").show();
        });

        $("#divRequestedEffectiveDateInfo").mouseleave(function () {
            $("#divRequestedEffectiveDateInfo").hide();
        });

        $(".help-kfe").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            var keyfield = $(this).text();
            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            $(".key-field-title").text(keyfield);
            //show the menu directly over the placeholder

            if (this.id == "helpNPI") {
                //$("#helpKFEEditInfoUp").css({
                //    position: "absolute",
                //    top: pos.top + "px",
                //    left: (pos.left + width) + "px"
                //}).show();
            }
            else {
                $("#helpKFEEditInfo").css({
                    position: "absolute",
                    top: pos.top + "px",
                    left: (pos.left + width) + "px"
                }).show();
            }

        });

        $("#helpKFEEditInfo").mouseleave(function () {
            $("#helpKFEEditInfo").hide();
        });

        $("#helpKFEEditInfoUp").mouseleave(function () {
            $("#helpKFEEditInfoUp").hide();
        });

        $(".help-taxid").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpTaxIDInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
          $(".help-taxid").mouseleave(function () {
            $("#helpTaxIDInfo").hide();
        });
         $(".help-npi").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpNPIinfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
          $(".help-npi").mouseleave(function () {
            $("#helpNPIinfo").hide();
        });
        $(".help-provider").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpProviderinfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
         $(".help-provider").mouseleave(function () {
            $("#helpProviderinfo").hide();
        });
         $(".help-business").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpBusinessNameInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
         $(".help-business").mouseleave(function () {
            $("#helpBusinessNameInfo").hide();
        });

        $("#helpBusinessNameInfo").mouseleave(function () {
            $("#helpBusinessNameInfo").hide();
        });

    }

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    } 

    function showHistory() {
        document.getElementById('<%= pHistory.ClientID %>').style.display = 'block';
    }


</script>
<style type="text/css">
    .radioButtonList {
        margin-left: 0px !important;
        margin-right: 0px !important;
    }
</style>
<div onmouseover="removeDisabled();">
<div>
    <asp:ValidationSummary ID="vsOrgInfo" runat="server" DisplayMode="List" ValidationGroup="valOrgInfo" CssClass="failureNotification" />
    <asp:ValidationSummary ID="vsImmigrationInfo" runat="server" DisplayMode="List" ValidationGroup="valImmigrationInfo" />
</div>
<asp:panel id="upHistory" runat="server" style="min-width: 1400px; left: 200px; top: 250px; padding: 8px; ">
    <ajax:modalpopupextender id="mpeHistory" runat="server" popupcontrolid="pHistory" targetcontrolid="ButtonDummy3"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pHistory" CancelControlID="btnCloseHistory">
    </ajax:modalpopupextender>
    <asp:panel id="pHistory" runat="server" cssclass="modalPopup" style="padding: 20px; min-width: 1500px; display: none; overflow: auto;">
        <div>
             <asp:panel id="pnlHistoryDetails" runat="server">
                <div>
                    <asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found."
                        AllowPaging="true" AllowSorting="True" PageSize="10"
                        OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
                        <Columns>
                            <asp:BoundField DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                            <asp:BoundField DataField="NAME"           HeaderText="Name"          SortExpression="NAME" />
                            <asp:BoundField DataField="DBA"           HeaderText="DBA"          SortExpression="DBA" />
                            <asp:BoundField DataField="FIRST_NAME"           HeaderText="First Name"          SortExpression="FIRST_NAME" />
                            <asp:BoundField DataField="LAST_NAME"           HeaderText="Last Name"          SortExpression="LAST_NAME" />
                            <asp:BoundField DataField="TITLE"           HeaderText="Title"          SortExpression="TITLE" />
                            <asp:BoundField DataField="TAX_ID"           HeaderText="TAX ID"          SortExpression="TAX_ID" />
                            <asp:BoundField DataField="NPI"           HeaderText="NPI"          SortExpression="NPI" />
                            <asp:BoundField DataField="NPI_START_DATE"           HeaderText="NPI Start"          SortExpression="NPI_START_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="NPI_END_DATE"           HeaderText="NPI End"          SortExpression="NPI_END_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="GENDER"           HeaderText="Gender"          SortExpression="GENDER" />
                            <asp:BoundField DataField="BIRTH_DATE"           HeaderText="DOB"          SortExpression="BIRTH_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="MMIS_PROVIDER_TYPE_ID"           HeaderText="Provider Type"          SortExpression="MMIS_PROVIDER_TYPE_ID" />
                            <asp:BoundField DataField="TERM_DATE"           HeaderText="Term Date"          SortExpression="TERM_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="REVALIDATION_DATE_PRIOR_TO_TERMINATION"           HeaderText="Reval Date"          SortExpression="REVALIDATION_DATE_PRIOR_TO_TERMINATION"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="CAQH"           HeaderText="CAQH"          SortExpression="CAQH" /> 
                            <asp:BoundField DataField="IS_OHIO_RESIDENT"           HeaderText="OH Res"          SortExpression="IS_OHIO_RESIDENT" />
                            <asp:BoundField DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                            <asp:BoundField DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
            </asp:panel>
            <asp:Button id="btnCloseHistory"  runat="server" Text="OK" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
        </div>
        <asp:button runat="server" id="ButtonDummy3" style="display: none" text="”ButtonDummy3”" />
    </asp:panel>
</asp:panel>


<div id="helpKFEEditInfo" class="infoBox" style="top: 0; right: 0;">
    <div class="infoTitle">Key Identifier Field Help</div>
    <div class="infoContent">
        <span class="key-field-title">Key Field</span>
        <asp:Literal ID="ltlKFEHelp" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_EDIT_HELPTEXT %>"></asp:Literal>
    </div>
</div>

<div id="helpKFEEditInfoUp" class="infoBox" style="top: 0; right: 0;">
    <div class="infoTitle">Key Identifier Field Help</div>
    <div class="infoContent">
        <span class="key-field-title">Key Field</span>
        <asp:Literal ID="ltlKFEHelpUp" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_EDIT_HELPTEXT_UPDATEONLY %>"></asp:Literal>
    </div>
</div>



    <div id="MtDentalPrgms" runat="server" visible="false">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12 pageHeader"><span class="formLabelAuto">State Medicaid Program </span></div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:CheckBox ID="chkMTMed" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="chkCSHN_CheckedChanged" AutoPostBack="true" Style="border: none; background: none;" Text="I wish to participate as a provider in the State’s Medicaid Program only" />
                        <div id="msgMTMed" visible="false" runat="server" class="pg-hint3Center pg-hint3">If you wish to participate, upon successful re-enrollment, the program dates will be extended</div>
                    </div>
                </div>
                <div class="row" id="trMTEffectiveDate1" runat="server">
                    <div class="col-sm-4  text-right">
                        <span class="formLabel wd200">
                            <asp:Label ID="Label1" runat="server" Text="Effective Date*" CssClass="formLabel wd200" /></span>
                    </div>
                    <div class="col-sm-8">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtMTChangeEffectiveDate1" runat="server" aria-label="EffectiveDate" CssClass="formFieldReadOnly" />
                            <ajax:CalendarExtender ID="CalendarExtender4" TargetControlID="txtMTChangeEffectiveDate1" runat="server" />
                        </span>
                    </div>
                </div>
                <div class="row" id="divMTEndDate1" runat="server">
                    <div class="col-sm-4  text-right">
                        <span class="formLabel wd200">End Date</span>
                    </div>
                    <div class="col-sm-8">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtMTEndDate1" runat="server" CssClass="formFieldReadOnly" aria-label="enddate" />
                            <ajax:CalendarExtender ID="CalendarExtender5" TargetControlID="txtMTEndDate1" runat="server" />
                            <asp:RequiredFieldValidator ID="reqMTEndDate1" runat="server" SetFocusOnError="true" ValidationGroup="valOrgInfo"
                                Text="*" ControlToValidate="txtMTEndDate1" ErrorMessage="Enter End Date" Display="Dynamic" Enabled="false" />
                            <asp:CompareValidator ID="cmpMTEndDate1" runat="server" ValidationGroup="valOrgInfo"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtMTEndDate1" Enabled="false"
                                ErrorMessage="Select a valid Effective Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true"> 
                            </asp:CompareValidator>

                        </span>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-12 pageHeader"><span class="formLabelAuto">Children’s Health Insurance Program (CHIP)</span></div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:CheckBox ID="chkMTChip" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="chkCSHN_CheckedChanged" AutoPostBack="true" Style="border: none; background: none;" Text="I wish to participate in the State’s CHIP Program only. " />
                        <div id="msgMTChip" visible="false" runat="server" class="pg-hint3Center pg-hint3">If you wish to participate, upon successful re-enrollment, the program dates will be extended</div>
                    </div>
                </div>
                <div class="row" id="trMTEffectiveDate2" runat="server">
                    <div class="col-sm-4  text-right">
                        <span class="formLabel wd200">
                            <asp:Label ID="Label2" runat="server" Text="Effective Date*" CssClass="formLabel wd200" /></span>
                    </div>
                    <div class="col-sm-8">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtMTChangeEffectiveDate2" runat="server" CssClass="formFieldReadOnly" aria-label="Effective date 2" />
                            <ajax:CalendarExtender ID="CalendarExtender6" TargetControlID="txtMTChangeEffectiveDate2" runat="server" />
                        </span>
                    </div>
                </div>
                <div class="row" id="divMTEndDate2" runat="server">
                    <div class="col-sm-4  text-right">
                        <span class="formLabel wd200">End Date</span>
                    </div>
                    <div class="col-sm-8">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtMTEndDate2" runat="server" CssClass="formFieldReadOnly" aria-label="End date 2" />
                            <ajax:CalendarExtender ID="CalendarExtender7" TargetControlID="txtMTEndDate2" runat="server" />
                            <asp:RequiredFieldValidator ID="reqMTEndDate2" runat="server" SetFocusOnError="true" ValidationGroup="valOrgInfo"
                                Text="*" ControlToValidate="txtMTEndDate2" ErrorMessage="Enter End Date" Display="Dynamic" Enabled="false" />
                            <asp:CompareValidator ID="cmpMTEndDate2" runat="server" ValidationGroup="valOrgInfo"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtMTEndDate2" Enabled="false"
                                ErrorMessage="Select a valid Effective Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true"> 
                            </asp:CompareValidator>
                        </span>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row">
            <div class="col-sm-12">
                <asp:CheckBox ID="chkMTboth" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="chkCSHN_CheckedChanged" AutoPostBack="true" Style="border: none; background: none;" Text="I wish to participate in both Medicaid and CHIP" />
                <div id="msgMTboth" visible="false" runat="server" class="pg-hint3Center pg-hint3">If you wish to participate, upon successful re-enrollment, the program dates will be extended</div>
            </div>
        </div>
        <div class="horizontal-divider"></div>
    </div>

    <div id="trEntityType" class="row" runat="server">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200" style="height: 20px; margin-top: 7px;">Entity Type</span>
        </div>
        <div class="col-sm-8">
            <span class="fieldValue wd200" style="vertical-align: bottom;">
                <asp:RadioButtonList ID="rblEntityType" runat="server" RepeatDirection="Horizontal" CssClass="radioButtonList" Enabled="false">
                    <asp:ListItem Selected="False" Text="Individual" Value="Individual"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="Organization" Value="Organization"></asp:ListItem>
                </asp:RadioButtonList>
            </span>
        </div>
    </div>
    <div class="divHistoryAndAdd" style="vertical-align: middle;">
        <asp:LinkButton ID="btnHistory" runat="server" AlternateText="History" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" ToolTip="History" Text="History" OnClientClick="showHistory();" Style="vertical-align: middle; color: white; text-decoration: none; padding: 4px;" />
    </div>
                 <span style="color:#D10000 ; font-size: 14pt !important; font-weight:100 !important";>An asterisk * indicates a required field</span>
    <div id="trBusinessName" class="row">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">Name of Business Entity*</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtLegalBusinessName" runat="server" CssClass="formFieldReadOnly" aria-label="Business Name" aria-required="true" MaxLength="75" />
                <asp:RequiredFieldValidator runat="server" ID="reqLegalBusinessName"
                    ControlToValidate="txtLegalBusinessName" ErrorMessage="*Enter Legal Business Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valOrgInfo" />
                 <span id="helpBusinessName" class="help-business" style="cursor: pointer; display: inline-block;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" AlternateText="help text" /></span>
                  <span id="helpBusinessNameInfo" class="infoBox" style="top: 0; right: 0;">
                    <span class="infoTitle">Help Text</span>
                    <span class="infoContent">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_BUSINESS_NAME_HELPTEXT %>"></asp:Literal>
                    </span>
                </span>

            </span>
        </div>
    </div>
    <div id="trBusinessHelpText" runat="server" class="row">

        <div class="col-sm-12 centerblock">
            <i>Business Name as it appears on your IRS assignment letter.</i>
        </div>
    </div>
    <div id="divDBA" class="row" runat="server">
        <div class="col-sm-4  text-right">
            <asp:Label ID="lblDBA" runat="server" Text="DBA" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtDBA" runat="server" CssClass="formField" aria-label="DBA" MaxLength="75"  />
               
            </span>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblPracticeType" runat="server" Text="Practice Type*" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <asp:DropDownList ID="ddlPracticeType" AutoPostBack="false" runat="server" CssClass="formfield" Style="width: 450px" aria-label="Practice type" aria-required="true" ></asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="rfvddlPracticeType"
                ControlToValidate="ddlPracticeType" ErrorMessage="*Select Practice Type" Text="*" Display="Dynamic"
                SetFocusOnError="true" ValidationGroup="valOrgInfo" />

        </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblOwnerShip" runat="server" Text="Ownership Type*" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <asp:DropDownList ID="ddlOwnershiptype" AutoPostBack="false" runat="server" CssClass="formfield" Style="width: 450px" aria-label="Ownership type" aria-required="true" ></asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="rfvOwnershiptype"
                ControlToValidate="ddlOwnershiptype" ErrorMessage="*Select Ownership Type" Text="*" Display="Dynamic"
                SetFocusOnError="true" ValidationGroup="valOrgInfo" />

        </div>
    </div>


    <div id="trFirstName" class="row" runat="server">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblFirstName" runat="server" Text="First Name*" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="formFieldReadOnly" aria-label="First name" aria-required="true"  MaxLength="35" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                    ControlToValidate="txtFirstName" ErrorMessage="Enter First Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valOrgInfo" />
            </span>
        </div>
    </div>
    <div id="trMiddleInt" class="row" runat="server">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblMiddleInitial" runat="server" Text="Middle Initial" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtMiddleInitial" runat="server" CssClass="formFieldReadOnly" aria-label="Middle Intial" MaxLength="1"  /></span>
        </div>
    </div>

    <div id="trLastName" class="row" runat="server">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblLastName" runat="server" Text="Last Name*" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtLastName" runat="server" CssClass="formFieldReadOnly" aria-label="Last Name" aria-required="true"  MaxLength="35" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                    ControlToValidate="txtLastName" ErrorMessage="Enter Last Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valOrgInfo" />
            </span>
        </div>
    </div>
    <div class="row" id="trTitle" runat="server">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblTitle" runat="server" Text="Title" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:DropDownList ID="ddlTitle" runat="server" Style="width: 450px" aria-label="Title">
                </asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <span><span class="formLabel wd200">Tax ID*</span></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;" >
               <ew:NumericBox ID="nbTaxID" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formFieldReadOnly" aria-label="Tax ID" aria-required="true"/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbTaxID"
                    ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valOrgInfo" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                    ControlToValidate="nbTaxID" ErrorMessage="*Enter Tax ID" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valOrgInfo" />
                <span id="helpTaxID" class="help-taxid" style="cursor: pointer; display: inline-block;">
                    <asp:Image ID="imgHelpTaxID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" AlternateText="help text"/></span>
                <span id="helpTaxIDInfo" class="infoBox" style="top: 0; right: 0;">
                    <span class="infoTitle">Tax ID Help</span>
                    <span class="infoContent">
                        <asp:Literal ID="ltlTaxIdHelp" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_TAX_NOCHANGE_HELPTEXT %>"></asp:Literal>
                    </span>
                </span>

            </span>
        </div>
    </div>

    <div class="row" id="trTaxIdType" runat="server">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblTaxIdType" runat="server" Text="Tax ID Type" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:DropDownList ID="ddlTaxIdType" AutoPostBack="false" aria-label="TaxID type" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ValidationGroup="valOrgInfo"
                    ControlToValidate="ddlTaxIdType" ErrorMessage="*Select Tax Id Type" Text="*" Display="Dynamic"
                    SetFocusOnError="true" InitialValue="" />
                <span id="helpTaxIDType" class="help-kfe" style="cursor: pointer; display: inline-block;"><span class="help-parent-name" style="display: none;">Tax ID Type</span>
                    <asp:Image ID="imgHelpTaxIDType" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" /></span>
            </span>
        </div>
    </div>
    <div class="row" id="trNPI" runat="server">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">NPI</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="nbNPI" runat="server" DecimalPlaces="0" MaxLength="10" PositiveNumber="true" CssClass="formFieldReadOnly" aria-label="NPI" TabIndex="0" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="nbNPI"
                    ValidationExpression="^[1-9]\d{9}$" ErrorMessage="*NPI requires 10 digits and cannot start with 0" Text="*" Display="Dynamic"
                    ValidationGroup="valOrgInfo" />
                 <span id="helpnpi" class="help-npi" style="cursor: pointer; display: inline-block;">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" AlternateText="help text"/></span>
                <span id="helpNPIinfo" class="infoBox" style="top: 0; right: 0;">
               
                     <span class="infoTitle">Help Text</span>
                    <span class="infoContent">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:BrandingResource , HELP_TEXT_NPI %>"></asp:Literal>
                    </span>
                    </span>                   
                </span>
            </span>
        </div>
    </div>

    <div class="row" id="trNPIStartDate" runat="server">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">NPI Start Date</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtNPIStartDate" runat="server" CssClass="formField" aria-label="NPI Start Date" />
                <ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtNPIStartDate" runat="server" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valOrgInfo"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNPIStartDate"
                    ErrorMessage="Select a valid NPI Start Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> </asp:CompareValidator>
            </span>
        </div>
    </div>



    <div class="row" id="trGender" runat="server">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblGender" runat="server" Text="Gender*" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <fieldset>
                <legend>
           <span>
                <asp:DropdownList ID="ddlGender" runat="server" CssClass="formDropDownMedium">
                                </asp:DropdownList>
                                        <asp:CompareValidator runat="server" ID="cvddlGender" ControlToValidate="ddlGender"
                                        ValueToCompare="" Type="String" ErrorMessage="* Gender is required."
                                        Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                                        ValidationGroup="AddNewProvider" />
            </span>
                </legend>
            </fieldset>
        </div>
    </div>

    <div class="row" id="trBirthDate" runat="server">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblBirthDate" runat="server" CssClass="formLabel wd200" Text="Date of Birth*" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField" aria-label="Birth Date" aria-required="true" />
                <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtBirthDate" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" ValidationGroup="valOrgInfo" Text="*"
                    ControlToValidate="txtBirthDate" ErrorMessage="Enter Date of Birth" Display="Dynamic" />
                <asp:CompareValidator ID="CompareValidator3" runat="server" ValidationGroup="valOrgInfo"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate"
                    ErrorMessage="Select a valid Date of Birth" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </span>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblProviderType" runat="server" Text="Provider Type*" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <asp:DropDownList ID="ddlProviderType" AutoPostBack="false" runat="server" CssClass="formfield" Style="width: 450px" aria-label="Provider type" aria-required="true" ></asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7"
                ControlToValidate="ddlProviderType" ErrorMessage="*Enter Provider Type" Text="*" Display="Dynamic"
                SetFocusOnError="true" ValidationGroup="valOrgInfo" />
             <span id="helpProviderType" class="help-provider" style="cursor: pointer; display: inline-block;">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" AlternateText="help text" /></span>
                <span id="helpProviderinfo" class="infoBox" style="top: 0; right: 0;">
            
                     <span class="infoTitle">Help Text</span>
                    <span class="infoContent">
                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:BrandingResource , HELP_TEXT_PROVIDER_TYPE %>"></asp:Literal>
                    </span>
                    </span>  
        </div>
    </div>

    <div class="row">
        <div class="col-sm-4  text-right">
            <span>
                <asp:Label ID="lblTypeofPractice" runat="server" Text="Type of Practice*" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:DropDownList ID="ddlTypeofPractice" AutoPostBack="false" runat="server"></asp:DropDownList>
                <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8"
                ControlToValidate="ddlTypeofPractice" ErrorMessage="*Enter Type of Practice" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOrgInfo" /> --%>  
            </span>
        </div>
    </div>


    <div class="row" id="trTermDate" runat="server" visible="false">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">Terminated Date</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:Label ID="lblTermDate" runat="server" /></span>
        </div>
    </div>
    <div class="row" id="trTermReason" runat="server">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">
                <asp:Label ID="lblRevalidationReason" runat="server" Text="Termination Reason" CssClass="formLabel wd200 " /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:DropDownList ID="ddlTerminationReason" AutoPostBack="false" runat="server"></asp:DropDownList></span>
        </div>
    </div>
    <div class="row" id="trRevalidationDate" runat="server">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">
                <asp:Label ID="lblForRevalidationDate" runat="server" Text="Revalidation Date" CssClass="formLabel wd200" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtRevalidationDate" runat="server" CssClass="formField" Enabled="false" aria-label="RevalidationDate"/>
                <ajax:CalendarExtender ID="CalRevalidationDate" TargetControlID="txtRevalidationDate" runat="server" Enabled="false" />

                <asp:CompareValidator ID="CVRevalidationDate" runat="server" ValidationGroup="valOrgInfo"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtRevalidationDate"
                    ErrorMessage="Select a valid Revalidation Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" Enabled="false"> 
                </asp:CompareValidator>
            </span>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">Enrollment Status</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:Label ID="lblEnrollmentStatus" runat="server" CssClass="formFieldReadOnly" aria-label="Enrollment Status"></asp:Label></span>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">Enrollment Status Reason</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:Label ID="lblEnrollmentStatusReason" runat="server" CssClass="formFieldReadOnly" aria-label="Enrollment status reason"></asp:Label></span>
        </div>
    </div>
   <div id="divCountryDetails" runat="server">
        <div class="row">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">Birth Country</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:DropDownList ID="ddlBirthCountry" runat="server" aria-label="Birth country">
                </asp:DropDownList>
            </span>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">Birth State</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
               <asp:TextBox ID="txtBirthState" runat="server" CssClass="formField" aria-label="Birth State"/>
            </span>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">Birth City</span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <asp:TextBox ID="txtBirthCity" runat="server" CssClass="formField" onKeyUp="javascript:alphanumericOnly(this);" aria-label="Birth City"/>
            </span>
        </div>
    </div>

   </div>
 <div class="row">
        <div class="col-sm-4  text-right">
           <span class="formLabel wd200">
               <asp:Label ID="LabelCaqh" runat="server" Text="CAQH #" CssClass="formLabel wd170" />
           </span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                     <ew:NumericBox ID="txtCaqh" runat="server" DecimalPlaces="0" PositiveNumber="True" aria-label="Caqh" CssClass="formField" />
                    <asp:RegularExpressionValidator ID="rfValidator" runat="server" ControlToValidate="txtCaqh"
                        ValidationExpression="^\d+$" ErrorMessage="* Enter digits only" Text="*"
                        Display="Dynamic"  />                    
            </span>
        </div>
    </div>
    <div class="row" id="trOdhNumber" runat="server">
        <div class="col-sm-4  text-right">
            <asp:Label ID="LblOdhNumber" runat="server" CssClass="formLabel200" Text="ODH Home Number*"></asp:Label>
        </div>
        <div class="col-sm-8">
            <ew:NumericBox ID="nbOdhNumber" runat="server" DecimalPlaces="0" aria-label="ODH number" PositiveNumber="True" MaxLength="4" CssClass="formField" />
            <asp:RegularExpressionValidator ID="rfValidatorOdhHomeFormat" runat="server" ControlToValidate="nbOdhNumber"
                ValidationExpression="(?!0{4})(?!9{4})\d{4}$" ErrorMessage="* Enter 4 digits for ODH Home Number" Text="*"
                Display="Dynamic" ValidationGroup="valOrgInfo" />
            <asp:RequiredFieldValidator runat="server" ID="rfValidatorODHHome" ValidationGroup="valOrgInfo"
                ControlToValidate="nbOdhNumber" ErrorMessage="* Enter ODH Home Number" Text="*" Display="Dynamic"
                SetFocusOnError="true" Enabled="true" />
        </div>
        <div style="display: none;">
            <asp:Label ID="lblPDMSExtZip" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4 text-right">
            <span class="formLabel wd200">
                 <asp:Label ID="lblOhioResident" runat="server" Text=" Have you been a resident of the state OHIO for the last 5 years?*" style="white-space:normal;height:auto" CssClass="formLabel wd300" />
            </span>
        </div>
        <div class="col-sm-6 text-left" style="display: flex; flex-wrap: wrap; justify-content: space-evenly">
            <span style="text-align: left;">
                <asp:RadioButtonList ID="rblIsOhioResident" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:RadioButtonList>                
                              
            </span>
        </div>
    </div>
    <div class="row" id="trProviderNumber" runat="server">
        <div class="col-sm-4  text-right">
            <span class="formLabel wd200">
                <asp:Label ID="lblProviderNumber" runat="server" Text="Provider Number" CssClass="formLabel wd170" /></span>
        </div>
        <div class="col-sm-8">
            <span style="text-align: left;">
                <ew:NumericBox ID="nbProviderNumber" runat="server" DecimalPlaces="0" PositiveNumber="True" aria-label="Provider number" MaxLength="11" CssClass="formField" />
            </span>
        </div>
    </div>
</div>
<ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnOk" BackgroundCssClass="modalBackground">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="ownerModalPopup" Style="display: none;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <div class="popTitle">
            <asp:Label ID="Label3" runat="server" Text="Enrollment History" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnOk">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwEnrollmentHistory" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:EnrollmentHistory ID="ucEnrollmentHistory" runat="server" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <div class="row text-center" style="padding-right: 10px;">
        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="buttonBoxFocus" />
    </div>
    <br />
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />

<asp:HiddenField ID="hdnApplicationTypeId" runat="server" />
<asp:HiddenField ID="hdnProviderCategoryId" runat="server" />
<asp:HiddenField ID="hdnProviderTypeId" runat="server" />
<asp:HiddenField ID="hdnProviderNameChange" runat="server" />

<div style="width: 100%; text-align: right; display: none;">
    <telerik:RadGrid ID="grdHistoryExport" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="NAME"           HeaderText="Name"          SortExpression="NAME" />
                <telerik:GridBoundColumn DataField="DBA"           HeaderText="DBA"          SortExpression="DBA" />
                <telerik:GridBoundColumn DataField="FIRST_NAME"           HeaderText="First Name"          SortExpression="FIRST_NAME" />
                <telerik:GridBoundColumn DataField="LAST_NAME"           HeaderText="Last Name"          SortExpression="LAST_NAME" />
                <telerik:GridBoundColumn DataField="TITLE"           HeaderText="Title"          SortExpression="TITLE" />
                <telerik:GridBoundColumn DataField="TAX_ID"           HeaderText="TAX ID"          SortExpression="TAX_ID" />
                <telerik:GridBoundColumn DataField="NPI"           HeaderText="NPI"          SortExpression="NPI" />
                <telerik:GridBoundColumn DataField="NPI_START_DATE"           HeaderText="NPI Start"          SortExpression="NPI_START_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="NPI_END_DATE"           HeaderText="NPI End"          SortExpression="NPI_END_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="GENDER"           HeaderText="Gender"          SortExpression="GENDER" />
                <telerik:GridBoundColumn DataField="BIRTH_DATE"           HeaderText="DOB"          SortExpression="BIRTH_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="MMIS_PROVIDER_TYPE_ID"           HeaderText="Provider Type"          SortExpression="MMIS_PROVIDER_TYPE_ID" />
                <telerik:GridBoundColumn DataField="TERM_DATE"           HeaderText="Term Date"          SortExpression="TERM_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="REVALIDATION_DATE_PRIOR_TO_TERMINATION"           HeaderText="Reval Date"          SortExpression="REVALIDATION_DATE_PRIOR_TO_TERMINATION"  DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="CAQH"           HeaderText="CAQH"          SortExpression="CAQH" /> 
                <telerik:GridBoundColumn DataField="IS_OHIO_RESIDENT"           HeaderText="OH Res"          SortExpression="IS_OHIO_RESIDENT" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>


</div>