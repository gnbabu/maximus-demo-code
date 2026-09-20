<%@ control language="C#" autoeventwireup="true" inherits="UserControls_GroupReview, App_Web_2k5drnu4" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/DisenrollmentView.ascx" TagName="DisenrollmentView" TagPrefix="viewDisenroll" %>
<%@ Register Src="~/PopupControls/RetroEffectiveView.ascx" TagName="RetroView" TagPrefix="viewRetro" %>
<%@ Register Src="~/PopupControls/ExpressTerminationView.ascx" TagName="TermView" TagPrefix="viewTerm" %>
<%@ Register Src="~/PopupControls/ReactivationView.ascx" TagName="ReactivationView" TagPrefix="viewReactivate" %>
<%@ Register Src="~/PopupControls/ProviderManagementView.ascx" TagName="ProviderManagementView" TagPrefix="viewProviderManagement" %>
<%@ Register Src="~/PopupControls/ProviderSummaryView.ascx" TagName="ProviderSummaryView" TagPrefix="viewSummary" %>
<%@ Register Src="~/PopupControls/ProviderAddView.ascx" TagName="ProviderAddView" TagPrefix="viewAddProvider" %>
<%@ Register Src="~/PopupControls/SuspendView.ascx" TagName="SuspendView" TagPrefix="viewSuspend" %>
<%@ Register Src="~/PopupControls/NewTerminationDateView.ascx" TagName="NewTermDtView" TagPrefix="viewNewTermDt" %>
<script type="text/javascript">
    function pageLoad() {
		try {
			$('[data-toggle="popover"]').popover()
		}
		catch(err) {
			console.log('that popover method does not exist at this point: ' + err);
		}
    }
    function SelectAll(id) {
        var frm = document.forms[0];
        for (i = 0; i < frm.elements.length; i++) {
            var thisName = frm.elements[i].name;
            if (frm.elements[i].type == "checkbox" && thisName.indexOf("chkSelect") != -1) {
                frm.elements[i].checked = document.getElementById(id).checked;
            }
        }
    }

    function SetPageNumber(id, pageNumber, idClicked) {
        if (document.getElementById(id) != null) {
            document.getElementById(id).value = pageNumber;
        }
        if (document.getElementById(idClicked) != null) {
            document.getElementById(idClicked).value = "TRUE";
        }
    }

    function exportPopup() {
        var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
        if (rowCount > 10000) {
            $("#divRowCount").dialog();
        }
    }
    function SelectAllCheckboxes(spanChk) {

        // Added as ASPX uses SPAN for checkbox
        var oItem = spanChk.children;
        var theBox = (spanChk.type == "checkbox") ?
            spanChk : spanChk.children.item[0];
        xState = theBox.checked;
        elm = theBox.form.elements;

        for (i = 0; i < elm.length; i++)
            if (elm[i].type == "checkbox" &&
                elm[i].id != theBox.id) {
                //elm[i].click();
                if (elm[i].checked != xState)
                    elm[i].click();
                //elm[i].checked=xState;
            }
    }
    function showProgress() {
        var updateProgress = $get("<%= UpdateProgress.ClientID %>");
        updateProgress.style.display = "block";
    }
    
    function handleClick() {
        var IsProcessing = document.getElementById('<%= hdnIsProcessing.ClientID %>').value;
        if (IsProcessing === "true") {
            //Disable the button to prevent multiple clicks
            document.getElementById('<%= btnReconSave.ClientID %>').disabled = true;
            return true; //return true to allow server-side click event to proceed
        } else {
            //Allow the first click and set the hidden field to indicate that the processing is in progress
            document.getElementById('<%= hdnIsProcessing.ClientID %>').value = "true";
            return true; //return true to allow server-side click event to proceed
        }
    }
</script>
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

}

</style>

<cc1:GroupBox ID="gbSearch" CaptionStyle-CssClass="bodyTextBold" Width="100%" runat="server">
    <h1> <Legend><span class="pdsSectionHeader" id="pdsSectionHeader_1" runat="server">Common Search Criteria</span></Legend> </h1>
     <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
    
    <div>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="ProviderSearch" ShowSummary="true" />
   </div>
    <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
        <div id="divHeader_1" runat="server" class="test-left" style="text-align: left;">        
            <hr />
        </div>
        <div class="container-fluid provider-search-fields">
            <asp:UpdatePanel ID="pnlUpdate_1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblMedicaidID" CssClass="ohio-field" AssociatedControlID="txtMedicaidID" runat="server">
                                <span tabindex="0" class="ohio-field-label">Medicaid ID <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_30' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_MEDICAIDID %>' />" ><asp:Image ID="imgInfoIcon" AlternateText="INFO" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <asp:TextBox ID="txtMedicaidID" aria-label="medicaid id" CssClass="ohio-field-input" runat="server" MaxLength="11" />

                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblGroupName" CssClass="ohio-field" AssociatedControlID="txtGroupName" runat="server">
                                <span tabindex="0"  class="ohio-field-label">Provider Name <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_32' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_NAME %>' />"><asp:Image ID="Image1" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <asp:TextBox ID="txtGroupName" CssClass="ohio-field-input" aria-label="Provider Name" runat="server" MaxLength="50" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblDBAName" CssClass="ohio-field" AssociatedControlID="txtDBAName" runat="server">
                                <span tabindex="0"  class="ohio-field-label">DBA Name <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_2' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_DBA %>' />" ><asp:Image ID="Image2" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <asp:TextBox ID="txtDBAName" aria-label="DBA" CssClass="ohio-field-input" runat="server" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblTaxID" CssClass="ohio-field" AssociatedControlID="txtTaxID" runat="server">
                                <span tabindex="0"  class="ohio-field-label">Tax ID <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_31' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_TAXID %>' />" aria-hidden="true"><asp:Image ID="Image3" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <asp:TextBox ID="txtTaxID" aria-label="TAX ID" CssClass="ohio-field-input" runat="server" MaxLength="9" />
                            <asp:RegularExpressionValidator ID="valTaxIdFormat" runat="server" ControlToValidate="txtTaxID"
                                ValidationExpression="^\d{9}$" ErrorMessage="* Enter a 9 digit Tax ID."
                                Enabled="true" SetFocusOnError="true"
                                ValidationGroup="ProviderSearch" Display="Dynamic" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblNPI" CssClass="ohio-field" AssociatedControlID="txtNPI" runat="server">
                                <span tabindex="0"  class="ohio-field-label">Provider NPI <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_29' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_NPI %>' />" aria-hidden="true"><asp:Image ID="Image4" Height="13" AlternateText="INFO"  Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <asp:TextBox ID="txtNPI" aria-label="NPI" CssClass="ohio-field-input" runat="server" MaxLength="10" />
                            <asp:RegularExpressionValidator ID="valNPI" runat="server" ControlToValidate="txtNPI"
                                ValidationExpression="^\d{10}$" ErrorMessage="* Enter a 10 digit NPI."
                                Enabled="true" SetFocusOnError="true"
                                ValidationGroup="ProviderSearch" Display="Dynamic" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblApplicationType" class="ohio-select" AssociatedControlID="ddlApplicationType" runat="server">
                                <span tabindex="0" class="ohio-select-label">Application Type <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_5' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_APPTYPE %>' />" aria-hidden="true"><asp:Image ID="Image5" Height="13" AlternateText="INFO"  Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="Appication type">
                                <asp:DropDownList ID="ddlApplicationType"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlApplicationType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblCategory" class="ohio-select" AssociatedControlID="ddlCategory" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Category <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_6' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_CATEGORY %>' />" aria-hidden="true"><asp:Image ID="Image6" Height="13" AlternateText="INFO"  Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0"  aria-label="category">
                                <asp:DropDownList ID="ddlCategory" CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>                        
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblMCO" class="ohio-select" AssociatedControlID="" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Waiver Type <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_10' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP %>' />" aria-hidden="true"><asp:Image ID="Image7" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="Waiver Type" >
                                <asp:DropDownList ID="ddlWaiverType"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" aria-Label="Waiver Type" AutoPostBack="True" OnSelectedIndexChanged="ddlWaiver_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblProviderType" class="ohio-select" AssociatedControlID="ddlProviderType" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Provider Type <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_7' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_TYPE %>' />" aria-hidden="true"><asp:Image ID="Image8" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="Provider Type" >
                                <asp:DropDownList ID="ddlProviderType" CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="ddlProviderType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblSpecialty" class="ohio-select" AssociatedControlID="ddlSpecialty" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Specialty <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_8' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_SPECIALTY %>' />" aria-hidden="true"><asp:Image ID="Image9" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="specialty" >
                                <asp:DropDownList ID="ddlSpecialty"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblEnrollmentStatusReason" class="ohio-select" AssociatedControlID="ddlEnrollmentStatusReason" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Enrollment Status Reason <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_20' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_ESR %>' />" aria-hidden="true"><asp:Image ID="Image10" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="Enrollment Status Reason">
                                <asp:DropDownList ID="ddlEnrollmentStatusReason"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblDODDContractNumber" CssClass="ohio-field" AssociatedControlID="txtDODDContractNumber" runat="server">
                                <span tabindex="0"  class="ohio-field-label">DODD Contract Number <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_24' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_DODD %>' />" aria-hidden="true"><asp:Image ID="Image11" Height="13" AlternateText="INFO"  Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <asp:TextBox ID="txtDODDContractNumber" aria-label="DODD Number" CssClass="ohio-field-input" runat="server" MaxLength="7" />
                            <asp:RegularExpressionValidator ID="revtxtDODDContractNumber" runat="server" ControlToValidate="txtDODDContractNumber"
                                ValidationExpression="^\d{7}$" ErrorMessage="* Enter a 7 digit DODD Contract Number."
                                Enabled="true" SetFocusOnError="true"
                                ValidationGroup="ProviderSearch" Display="Dynamic" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblCounty" CssClass="ohio-field" AssociatedControlID="txtCounty" runat="server">
                                <span tabindex="0"  class="ohio-field-label">County <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_15' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_COUNTY %>' />" aria-hidden="true"><asp:Image ID="Image12" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0">
                                <asp:TextBox ID="txtCounty" aria-label="county" CssClass="ohio-field-input" runat="server" MaxLength="30" />
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblCity" CssClass="ohio-field" AssociatedControlID="txtCity" runat="server">
                                <span tabindex="0"  class="ohio-field-label">City <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_16' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_CITY %>' />" aria-hidden="true"><asp:Image ID="Image13" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0">
                                <asp:TextBox ID="txtCity" aria-label="city" CssClass="ohio-field-input" runat="server" MaxLength="30" />
                                <asp:RegularExpressionValidator ID="revtxtCity" runat="server" ControlToValidate="txtCity"
                                    ValidationExpression="[a-zA-Z\s]*$" ErrorMessage="* Enter only alphabetical letters for City."
                                    Enabled="true" SetFocusOnError="true"
                                    ValidationGroup="ProviderSearch" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblRegID" CssClass="ohio-field" AssociatedControlID="txtRegID" runat="server">
                                <span tabindex="0"  class="ohio-field-label">Registration ID <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_21' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_REGID %>' />" aria-hidden="true"><asp:Image ID="Image14" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div taxindex="0">
                                <asp:TextBox ID="txtRegID" aria-label="reg id" CssClass="ohio-field-input" runat="server"/>
                                <asp:RegularExpressionValidator ID="revtxtRegID" runat="server" ControlToValidate="txtRegID"
                                    ValidationExpression="\d{0,10}" ErrorMessage="* Enter a numeric Reg ID Number with max 10 digits."
                                    Enabled="true" SetFocusOnError="true"
                                    ValidationGroup="ProviderSearch" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblDateReceived" CssClass="ohio-field" AssociatedControlID="txtDateReceived" runat="server">
                                <span tabindex="0"  class="ohio-field-label">Date Received <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_13' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_DATERECEIVED %>' />" aria-hidden="true"><asp:Image ID="Image15" Height="13" AlternateText="INFO"  Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div taxindex="0">
                                <span class="input-group">
                                    <ajax:CalendarExtender
                                        ID="CalendarExtender2"
                                        runat="server"
                                        Format="MM/dd/yyyy"
                                        TargetControlID="txtDateReceived"
                                        PopupPosition="BottomLeft"
                                        CssClass=""
                                        PopupButtonID=""
                                        EnabledOnClient="true" />
                                    <asp:TextBox ID="txtDateReceived" aria-label="date recieved" CssClass="ohio-field-input" runat="server" />
                                      <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtDateReceived" WatermarkText="MM/DD/YYYY"
                            WatermarkCssClass="watermarked" />
                                    <asp:CompareValidator
                                        ID="cvtxtDateReceived"
                                        runat="server"
                                        Type="Date"
                                        Operator="DataTypeCheck"
                                        ControlToValidate="txtDateReceived"
                                        ErrorMessage="Select a valid PNM Date Reveived"
                                        Display="Dynamic"
                                        ValueToCompare="MM/dd/yyyy"
                                        ValidationGroup="ProviderSearch"
                                        SetFocusOnError="true"> 
                                    </asp:CompareValidator>
                                    <span class="input-group-addon">
                                        <span aria-hidden="true"><asp:Image ID="Image31" Height="22" Width="22" runat="server" AlternateText="INFO"  ImageUrl="~/Images/Calendaricon.png" /></span>
                                    </span>
                                </span>
                            </div>
                        </div>
                          <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblTennCareStatus" class="ohio-select" AssociatedControlID="ddlTennCareStatus" runat="server">
                                <span tabindex="0"  class="ohio-select-label">PNM Status <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_4' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_STATUS %>' />" aria-hidden="true"><asp:Image ID="Image16" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>                            
                            </asp:Label>
                            <div tabindex="0">
                                <div tabindex="0" aria-label="PNM Status" >
                                    <asp:DropDownList ID="ddlTennCareStatus"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                         <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblPNMEnrollStatus" class="ohio-select" AssociatedControlID="ddlPNMEnrollStatus" runat="server">
                                <span class="ohio-select-label">PNM Enrollment Status <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_33' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_PNMERS %>' />" aria-hidden="true"><asp:Image ID="Image17" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="PNM Enrollment Status" >
                                <asp:DropDownList ID="ddlPNMEnrollStatus"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                         <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblPNMAppStatus" class="ohio-select" AssociatedControlID="ddlPNMAppStatus" runat="server">
                                <span tabindex="0"  class="ohio-select-label">PNM Application Status <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_34' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_PNMSTATUS %>' />" aria-hidden="true"><asp:Image ID="Image18" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="PNM Application Status" >
                                <asp:DropDownList ID="ddlPNMAppStatus"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divHeader_2" runat="server" class="test-left" style="text-align: left;">
          <h2> <span class="pdsSectionHeader" id="pdsSectionHeader_2" runat="server">Additional Search Criteria</span></h2> 
            <hr />
        </div>
        <div class="container-fluid provider-search-fields">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                       <%-- <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblContractType" class="ohio-select" AssociatedControlID="ddlContractType" runat="server">
                                <span class="ohio-select-label">Contract Type <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_11' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_CONTRACTTYPE %>' />" aria-hidden="true"></span>
                                </span>
                                <span class="ohio-select-select fa" aria-hidden="true">
                                    <asp:DropDownList ID="ddlContractType" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </span>
                            </asp:Label>
                        </div>--%>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblMCP" class="ohio-select" AssociatedControlID="ddlMCP" runat="server">
                                <span tabindex="0"  class="ohio-select-label">MCP <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_12' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_MCP %>' />" aria-hidden="true"><asp:Image ID="Image19" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="MCP">
                                <asp:DropDownList ID="ddlMCP"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblProgram" class="ohio-select" AssociatedControlID="ddlProgram" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Program <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_9' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP %>' />" aria-hidden="true"><asp:Image ID="Image20" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="program">
                                <asp:DropDownList ID="ddlProgram"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblTaxonomy" class="ohio-select" AssociatedControlID="ddlTaxonomy" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Taxonomy <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_14' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_TAXONOMY %>' />" aria-hidden="true"><asp:Image ID="Image21" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="Taxonomy" >
                                <asp:DropDownList ID="ddlTaxonomy"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <%--<div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblContractStatus" class="ohio-select" AssociatedControlID="ddlContractStatus" runat="server">
                                <span class="ohio-select-label">Contract Status <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_18' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_CONTRACT_STATUS %>' />" aria-hidden="true"></span>
                                </span>
                                <span class="ohio-select-select fa" aria-hidden="true">
                                    <asp:DropDownList ID="ddlContractStatus" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </span>
                            </asp:Label>
                        </div>--%>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblPDMSStatus" CssClass="ohio-select" AssociatedControlID="ddlPDMSStatus" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Current Workflow Step <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_28' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_WF_STEP %>' />" aria-hidden="true"><asp:Image ID="Image22" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="Current Workflow Step" >
                                <asp:DropDownList ID="ddlPDMSStatus"  CssClass="form-control unsetPublicSearchDDLLength" EnableViewState="true" runat="server" AutoPostBack="true"
                                    AppendDataBoundItems="True" OnSelectedIndexChanged="ddlPDMSStatus_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblPDMSStatusDate" CssClass="ohio-field" AssociatedControlID="txtPDMSStatusDate" runat="server">
                                <span tabindex="0"  class="ohio-field-label">Status Date <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_27' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_STATUS_DATE %>' />" aria-hidden="true"><asp:Image ID="Image23" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0">
                                <span class="input-group provider-search-fields">
                                    <ajax:CalendarExtender
                                        ID="CalendarExtender1"
                                        runat="server"
                                        Format="MM/dd/yyyy"
                                        TargetControlID="txtPDMSStatusDate"
                                        PopupPosition="BottomLeft"
                                        CssClass=""
                                        PopupButtonID=""
                                        EnabledOnClient="true" />
                                    <asp:TextBox ID="txtPDMSStatusDate" aria-label="status date" CssClass="ohio-field-input" runat="server" />
                                      <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtPDMSStatusDate" WatermarkText="MM/DD/YYYY"
                            WatermarkCssClass="watermarked" />
                                    <asp:CompareValidator
                                        ID="dateValidator"
                                        runat="server"
                                        Type="Date"
                                        Operator="DataTypeCheck"
                                        ControlToValidate="txtPDMSStatusDate"
                                        ErrorMessage="Select a valid PNM Status Date"
                                        Display="Dynamic"
                                        ValueToCompare="MM/dd/yyyy"
                                        ValidationGroup="ProviderSearch"
                                        SetFocusOnError="true"> 
                                    </asp:CompareValidator>
                                    <span class="input-group-addon">
                                        <span aria-hidden="true"><asp:Image ID="Image32" Height="22" Width="22" runat="server" AlternateText="INFO"  ImageUrl="~/Images/Calendaricon.png" /></span>
                                    </span>
                                </span>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblMedicareNumber" CssClass="ohio-field provider-search-fields" AssociatedControlID="txtMedicareNumber" runat="server">
                                <span tabindex="0"  class="ohio-field-label">Medicare Number <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_22' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_MEDICARE %>' />" aria-hidden="true"><asp:Image ID="Image24" AlternateText="INFO"  Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0">
                                <asp:TextBox ID="txtMedicareNumber" aria-label="medicare number" CssClass="ohio-field-input" runat="server" MaxLength="11" />
                                <asp:RegularExpressionValidator ID="revtxtMedicareNumber" runat="server" ControlToValidate="txtMedicareNumber"
                                    ValidationExpression="^[a-zA-Z0-9]*$" ErrorMessage="* Enter a alphanumeric Medicare Number."
                                    Enabled="true" SetFocusOnError="true"
                                    ValidationGroup="ProviderSearch" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblRiskLevel" class="ohio-select" AssociatedControlID="ddlRiskLevel" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Risk Level <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_3' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_RISKLEVEL %>' />" aria-hidden="true"><asp:Image ID="Image25" AlternateText="INFO"  Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="risk level" >
                                <asp:DropDownList ID="ddlRiskLevel"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="lblArea" class="ohio-select" AssociatedControlID="ddlArea" runat="server">
                                <span tabindex="0"  class="ohio-select-label">Area <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_17' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_AREA %>' />" aria-hidden="true"><asp:Image ID="Image26" Height="13" AlternateText="INFO"  Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="area">
                                <asp:DropDownList ID="ddlArea"  CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="Label1" class="ohio-select" AssociatedControlID="" runat="server">
                                <span tabindex="0"  class="ohio-select-label">ODA Registration Status <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_PROVIDER_EMAILID' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_AREA %>' />" aria-hidden="true"><asp:Image ID="Image27" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="ODA Registration Status">
                                <asp:DropDownList ID="ddlODARegistrationStatus" aria-label="ODA Registration Status" CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Label ID="Label3" class="ohio-select" AssociatedControlID="" runat="server">
                                <span tabindex="0"  class="ohio-select-label">DODD Registration Status <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_PROVIDER_PIMS' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_AREA %>' />" aria-hidden="true"><asp:Image ID="Image28" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                            </asp:Label>
                            <div tabindex="0" aria-label="DODD Registration Status">
                                <asp:DropDownList ID="ddlDODDRegistrationStatus" aria-label="DODD Registration Status" CssClass="form-control unsetPublicSearchDDLLength" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Panel ID="seeAssignedPanel" runat="server" Visible="false">
                                <asp:Label ID="lblAssigned" class="ohio-select" AssociatedControlID="ddlAssigned" runat="server">
                                    <span tabindex="0" class="ohio-select-label">Assigned <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_25' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP %>' />" aria-hidden="true"><asp:Image ID="Image29" Height="13" Width="13" runat="server" AlternateText="INFO"  ImageUrl="~/Images/Infoicon.png" /></span></span>
                                </asp:Label>
                                <div tabindex="0"  class="ohio-select-select fa" aria-hidden="true">
                                    <asp:DropDownList ID="ddlAssigned" aria-label="assigned" CssClass="form-control unsetPublicSearchDDLLength" runat="server" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlAssigned_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <asp:Panel ID="seeAssignedUser" runat="server" Visible="false">
                                <asp:Label ID="lblAssignedUser" class="ohio-select" AssociatedControlID="ddlAssignedUser" runat="server">
                                    <span tabindex="0"  class="ohio-select-label">Assigned User <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_26' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP %>' />" aria-hidden="true"><asp:Image ID="Image30" Height="13" Width="13" AlternateText="INFO"  runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>
                                </asp:Label>
                                <div tabindex="0" class="ohio-select-select fa" aria-hidden="true">
                                    <asp:DropDownList ID="ddlAssignedUser" aria-label="assigned user" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="pg-hint3Center pg-hint3">Medicaid ID, NPI, and Tax ID are exact match search fields.</div>
        <div class="btnBox btnBoxCenter">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ProviderSearch" OnClientClick="showProgress()" />
                    <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" OnClientClick="showProgress()" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSearch" />
                    <asp:PostBackTrigger ControlID="btnClear" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server" DisplayAfter="1">
        <ProgressTemplate>
            <div style="padding-right: 30px">
                <img src="../Images/ajax-loader.gif" alt="" />
                Loading ...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</cc1:GroupBox>
<br />

<asp:Panel ID="pnlResultHeader" runat="server" CssClass="ResultHeader">
    <asp:Label ID="lblResultHeader" runat="server" />
</asp:Panel>

<div style="width: 100%; text-align: right">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <telerik:RadGrid ID="RadGridExport" runat="server" Visible="true">        
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
            <Pdf PageTitle="Provider Search" PageHeight="8.5in" PageWidth="18in">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" >
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" HeaderText="Provider Name" DataField="OrganizationName"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Provider Type" DataField="PROVIDER_TYPE_NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Tax ID" DataField="TaxId"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4" HeaderText="NPI" DataField="NPI"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col5" HeaderText="PNM Status" DataField="PDMSStatus"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col7" HeaderText="Assigned To" DataField="AssignedTo"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col8" HeaderText="Specialty" DataField="SPECIALTY_TYPE_NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col9" HeaderText="Reg ID" DataField="RegID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col10" HeaderText="Medicaid ID" DataField="BaseMedicaidID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col11" HeaderText="Enrollment Status Reason" DataField="ENROLLMENT_STATUS_REASON"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col12" HeaderText="Enroll Status Type" DataField="EnrollStatusType" ></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:Label ID="lnkRowCount" Visible="false" runat="server" Style="float: left" CssClass="formLabelAuto">Row Count: <%= System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(hdnRowCount.Value.ToString(),true) %></asp:Label>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10,000 results from the search will be exported.</p>
    </div>
</div>

<div id="LoadingPanel" style="display: none; color: red; text-align: center;">Setting up workflow for provider...</div>
<br />
<asp:Panel ID="seetblBulkManage" runat="server" Visible="false">
    <div class="row" id="tblBulkManage" runat="server" style="vertical-align: top; margin-left: 7px;">
        <asp:Button ID="btnBulkManage" runat="server" CausesValidation="true" Text="Bulk Assign" CssClass="btn btn-secondary btn-lg" OnClick="btnBulkAssign_Click" />
        
        <%--<asp:Button ID="btnSendBulkEmail" runat="server" CssClass="btn btn-primary btn-lg" OnClick="btnSendBulkEmail_Click" Text="Send Email" CausesValidation="true" />--%>
    </div>
</asp:Panel>
<asp:Panel ID="seetblAssignUser" runat="server" Visible="false">
    <div class="row" id="tblAssignUser" runat="server" style="margin-left: 7px;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAssignUser" runat="server" AssociatedControlID="ddlAssignUser" Text="Assign User" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlAssignUser" aria-label="assigned user" runat="server" CssClass="textEntry" />
                </td>
                <td>
                    <asp:Button ID="btnAssign" runat="server" CssClass="btn btn-primary btn-lg" OnClick="btnAssign_Click" Text="Assign" CausesValidation="true" Style="margin-left: 5px; margin-right: 4px;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>

<br />

<asp:Button ID="btnBulkEmail" runat="server" CausesValidation="false" Text="Email" CssClass="btn btn-secondary btn-lg" OnClick="btnBulkEmail_Click"  Visible="true"/>
<asp:Button ID="btnSendBulkEmail" Visible="false" runat="server" CssClass="buttonBox" OnClick="btnSendBulkEmail_Click" Text="Send Email" CausesValidation="true" />
<br />

<%--<asp:Panel ID="seetblSendEmail" runat="server" Visible="false">
    <div class="row" id="tblSendEmail" runat="server" style="margin-left: 7px;">
        <table>
            <tr>
                <td>
                    
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>--%>


<mms:SortablePagingGridView
    ID="gvProviders"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnRowCommand="gvProviders_RowCommand"
    OnRowDataBound="gvProviders_RowDataBound"
    OnPageIndexChanging="gvProviders_PageIndexChanging"
    OnSorting="gvProviders_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="OrganizationName" GridViewSortDirection="Ascending"
    DataKeyNames="RegID,NPI,CurrentStepID,ENTITY_TYPE_ID,UserID,REG_PROGRAM_STATUS_TYPE_ID,ENROLLMENT_STATUS_CODE,ENROLLMENT_STATUS_REASON,REGISTRATION_STATUS_TYPE_ID, BaseMedicaidID, END_DATE,PROCESS_ID, ENROLLMENT_STATUS_REASONS_CDE, APPLICATION_TYPE_ID, WAIVER_TYPE_ID, PROVIDER_TYPE_ID,PDMSStatus">

    <Columns>
        <asp:TemplateField Visible="False">
            <ItemTemplate>
                <asp:CheckBox ID="chkAssign" runat="server" CssClass="gridViewCheckBox" />
            </ItemTemplate>
            <HeaderTemplate>
                <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);" runat="server" type="checkbox" />
            </HeaderTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False" HeaderText="">
            <ItemTemplate>
                <asp:LinkButton
                    ID="lnkReview"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="ReviewRow"
                    Text="Review"
                    CssClass="gridLink" PostBackUrl="~/Process/Registration.aspx" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False" HeaderText="">
            <ItemTemplate>
                <asp:LinkButton
                    ID="lnkExpressAdmin"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="ExpressAdmin"
                    Text="Admin"
                    CssClass="gridLink" />
            </ItemTemplate>
        </asp:TemplateField>
         
        <asp:TemplateField ShowHeader="false" HeaderText="">
            <ItemTemplate>
                <asp:LinkButton
                    ID="lnkUpdate"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="OperatorUpdate"
                    Text="Update"
                    CssClass="gridLink"
                    OnClientClick="$('#LoadingPanel').show(); return true;" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="OrganizationName" HeaderText="Provider Name" SortExpression="OrganizationName" />
        <asp:BoundField DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" SortExpression="PROVIDER_TYPE_NAME" />
        <asp:BoundField DataField="TaxId" HeaderText="Tax ID" SortExpression="TaxId" />
        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
        <asp:BoundField DataField="PDMSStatus" HeaderText="PNM Status" SortExpression="PDMSStatus" />
        <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" SortExpression="AssignedTo" />
        <asp:BoundField DataField="SPECIALTY_TYPE_NAME" HeaderText="Specialty" SortExpression="SPECIALTY_TYPE_NAME" HeaderStyle-Width="30" HeaderStyle-Wrap="true" />
        <asp:BoundField DataField="RegID" HeaderText="Reg ID" SortExpression="RegID" />
        <asp:BoundField DataField="BaseMedicaidID" HeaderText="Medicaid ID" SortExpression="BaseMedicaidID" />
        <asp:BoundField DataField="EnrollStatusType" HeaderText="Enroll Status Type" SortExpression="EnrollStatusType" />
        <asp:BoundField DataField="ENROLLMENT_STATUS_REASON" HeaderText="Enrollment Status Reason" SortExpression="ENROLLMENT_STATUS_REASON" />
        <asp:BoundField DataField="REFER_TO_COMPLIANCE_REASON" HeaderText="Compliance Reason" SortExpression="REFER_TO_COMPLIANCE_REASON" />
         <asp:TemplateField ShowHeader="False" HeaderText="">
            <ItemTemplate>
                <asp:LinkButton
                    ID="lnkRServices"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="RestrictedServices"
                    Text="Restricted Services"
                    CssClass="gridLink" PostBackUrl="~/Process/Registration.aspx" Visible="false" />
            </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:TemplateField ShowHeader="False" HeaderText="">
            <ItemTemplate>
                <asp:LinkButton
                    ID="lnkContractMaintenance"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="ContractMaintenance"
                    Text="Contract Maintenance"
                    CssClass="gridLink" PostBackUrl="~/Process/Registration.aspx" Visible="false" />
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
</mms:SortablePagingGridView>



<cc2:MessageBox ID="MessageBox2" runat="server" />

<ajax:ModalPopupExtender ID="mpeMaint" runat="server" PopupControlID="pnlExpressAdmin" TargetControlID="btnDummy" 
    RepositionMode="RepositionOnWindowScroll" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlExpressAdmin">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlExpressAdmin" runat="server" CssClass="adminPopUpFullSize adminModalPopup" Style="display: none">
    <asp:MultiView ID="mltExpressAdmin" runat="server" ActiveViewIndex="0" EnableViewState="true">
        <asp:View ID="vwSelection" runat="server">
            <asp:Panel ID="pnlAdminMaintenance" runat="server" Style="height: auto; width: 100%;">
                <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                    <div class="popTitle">Express Administration Functions</div>
                </asp:Panel>
                <div>
                    <asp:Label ID="lblNPIReactivate" runat="server" ForeColor="red"></asp:Label>
                    <asp:ValidationSummary ID="valExpressSummary" runat="server" DisplayMode="List" ValidationGroup="ExpressMaintSelection" ShowSummary="true" />
                </div>
                <br />
                <div>
                    <div class="row">
                        <div class="col-sm-2 text-right">
                            <span class="formLabel wd100">
                                <asp:Label ID="lblAdminActions" runat="server" AssociatedControlID="ddlAdminActions" Text="Action" /></span>
                        </div>
                        <div class="col-sm-7 text-left">
                            <asp:HiddenField ID="hdnNPI" runat="server"></asp:HiddenField>
                            <asp:DropDownList ID="ddlAdminActions" aria-label="admin actions" runat="server" CssClass="formDropDown" />
                            <asp:CompareValidator runat="server" ID="valActionReqd" ControlToValidate="ddlAdminActions"
                                ValueToCompare="" Type="String" ErrorMessage="* An Action is required."
                                Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                                ValidationGroup="ExpressMaintSelection" />
                                
                        </div>
                    </div>
                </div>
                <div class="btnBox" style="padding-top: 20px; padding-right: 10px;">
                    <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="buttonBoxFocus" OnClick="btnContinue_Click" CausesValidation="true" ValidationGroup="ExpressMaintSelection" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
                </div>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwDisenrollment" runat="server">
            <asp:Panel ID="pnlDisenrollment" runat="server">
                <asp:Panel ID="pnlDisenrollHdr" CssClass="popHeader" runat="server">
                    <asp:Label ID="lblpopTitle" runat="server" CssClass="popTitle">Provider Disenrollment</asp:Label>
                </asp:Panel>
                <viewDisenroll:DisenrollmentView ID="ucDisenrollmentView" runat="server" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwRetroEffectiveDate" runat="server">
            <asp:Panel ID="pnlRetro" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlRetroHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">Change Effective Date</div>
                </asp:Panel>
                <viewRetro:RetroView ID="ucRetroView" runat="server" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwTerminate" runat="server">
            <asp:Panel ID="pnlTerm" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlTermHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">Provider Termination</div>
                </asp:Panel>
                <viewTerm:TermView ID="ucTermView" runat="server" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwSuspend" runat="server">
            <asp:Panel ID="pnlsuspend" runat="server" Style="height: auto; width: 80%;">
                <asp:Panel ID="pnlsuspendHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">Suspend Provider1</div>
                </asp:Panel>
                <viewSuspend:SuspendView ID="ucSuspendView" runat="server" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwReactivate" runat="server">
            <asp:Panel ID="pnlReactivate" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlReactivateHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">Reactivate Provider</div>
                </asp:Panel>
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <viewReactivate:ReactivationView ID="ucReactivateView" runat="server" />
                    </div>
                </div>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwUpdateConverted" runat="server">
            <asp:Panel ID="pnlUpdateConverted" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlUpdateConvertedHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">Update Registration</div>
                </asp:Panel>
                <viewAddProvider:ProviderAddView ID="ucProviderAddView" runat="server" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwNotProcessed" runat="server">
            <asp:Panel ID="pnlNotProcessed" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlNotProcessedHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">Not Processed</div>
                </asp:Panel>
                <div>
                    <div class="row" style="margin-top: 2em;">
                        <div class="col-sm-2 text-right" style="margin-top: 0.4em;">
                            <strong>Comments</strong><span class="text-danger">*</span>

                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtNotProcessedComments" aria-label="request" runat="server" 
                                  MaxLength="1000" CssClass="modalFormField" TextMode="MultiLine" Columns="100" Rows="7" />
                        </div>
                    </div>
                     <div class="btnBox" style="padding-top: 20px; padding-right: 10px;">
                        <asp:Button ID="btnSaveNotProcessed" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSaveNotProcessed_Click" CausesValidation="true" />
                        <asp:Button ID="btnCancelNotProcessed" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwCredentialEvent" runat="server">
            <asp:Panel ID="pnlCredEvent" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlCredEventHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">
                        <asp:Label ID="lblCredEvent" runat="server" Text="Credentialing Event"></asp:Label>
                    </div>
                </asp:Panel>
                <div>
                    <br />
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <span class="formLabel" style="font: bold; text-align: center">Start the Credentialing Event </span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <asp:Label ID="lblErrmsg" runat="server" Visible="false" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="btnBox" style="padding-top: 20px; padding-right: 10px;">
                        <asp:Button ID="btnSaveCredEvent" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSaveCredEvent_Click" CausesValidation="true" />
                        <asp:Button ID="btnCancelCredEvent" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancelCredEvent_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwProviderReconsideration" runat="server">
            <asp:Panel ID="pnlRequestReconsideration" runat="server" CssClass="ownerModalPopup" align="center" Style="height: 300px; width: 1000px">
                <asp:Panel ID="Panel1" CssClass="popHeader" runat="server">
                    <div class="popTitle">
                        <asp:Label ID="Label2" runat="server" Text="Request Reconsideration" />
                    </div>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
                    <div>
                        <asp:ValidationSummary ID="vsReconsideration" runat="server" DisplayMode="List" ValidationGroup="ReconsiderationProvider" CssClass="text-left" />
                    </div>
                    <div class="row" id="trdateRecon" runat="server">
                        <div class="col-sm-4  text-right">
                            <span class="formLabel wd200">
                                <asp:Label ID="Label4" runat="server" Text="Date of Reconsideration Request*" CssClass="formLabel wd200" /></span>
                        </div>
                        <div class="col-sm-8">

                            <span style="text-align: left;">
                                <asp:TextBox ID="txtDateofReconsidertaionRequest" aria-label="request" runat="server" CssClass="formField wd200" MaxLength="10" />
                                <ajax:CalendarExtender ID="CalendarExtender4" TargetControlID="txtDateofReconsidertaionRequest" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" ValidationGroup="ReconsiderationProvider" Text="*"
                                    ControlToValidate="txtDateofReconsidertaionRequest" ErrorMessage="* Reconsideration Effective Date is required." Display="Dynamic" Enabled="true" />
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="ReconsiderationProvider"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateofReconsidertaionRequest" Enabled="true"
                                    ErrorMessage="* A valid Reconsideration Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                            </span>
                        </div>
                    </div>

                    <br />
                    <div style="margin-left: 100px" class="col-sm-8">
                        <asp:Panel ID="Panel2" CssClass="popHeader" runat="server">
                            <div class="popTitle">
                                <asp:Label ID="Label5" runat="server" Text="Reconsideration Request" />
                            </div>
                        </asp:Panel>
                        <br />
                        <div class="row">
                            <asp:Label ID="lblreconMsg" runat="server" Text="*Please select at least one reason for Disenrollment." Visible="false" CssClass="bodyTextRed"></asp:Label>
                        </div>
                        <div class="row">

                            <mms:EncryptedFileUpload runat="server" ID="encRequestReconsiderationDoc" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" />
                        </div>
                    </div>

                    <div class="btnBox">
                        <asp:HiddenField ID="hdnIsProcessing" runat="server" Value="false" />
                        <asp:Button runat="server" ID="btnReconSave" Text="Save" CssClass="buttonBoxFocus" OnClientClick="return handleClick();" OnClick="btnSaveProvRec_Click" CausesValidation="true" />
                        <asp:Button runat="server" ID="btnReconCancel" Text="Cancel" CssClass="buttonBox" OnClick="btnCancelProvRec_Click" CausesValidation="false" />
                    </div>

                </asp:Panel>
                <br />

            </asp:Panel>
        </asp:View>        
        <asp:View ID="vwNewTermDt" runat="server">
            <asp:Panel ID="pnlNewTermDt" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlNewTermHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">New Termination Date</div>
                </asp:Panel>
                <viewNewTermDt:NewTermDtView ID="ucNewTermDt" runat="server" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vwRevertSuspend" runat="server">
            <asp:Panel ID="pnlRevertSuspend" runat="server" Style="height: auto; width: auto;">
                <asp:Panel ID="pnlRevertSuspendHdr" CssClass="popHeader" runat="server">
                    <div class="popTitle">Revert Suspension</div>
                </asp:Panel>
                <div>
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-10">
                            <asp:Label ID="lblErrRevertSuspend" runat="server" Visible="false" CssClass="failureNotification" />
                        </div>
                    </div>
                    <div class="row" style="margin-top: 2em;">
                        <div class="col-sm-2 text-right" style="margin-top: 0.4em;">
                            <strong>Comments</strong><span class="text-danger">*</span>

                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtRevertSuspendComments" aria-label="request" runat="server" 
                                  MaxLength="1000" CssClass="modalFormField" TextMode="MultiLine" Columns="100" Rows="7" />                              
                        </div>
                    </div>
                     <div class="btnBox" style="padding-top: 20px; padding-right: 10px;">
                        <asp:Button ID="btnSaveRevertSuspend" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSaveRevertSuspend_Click" CausesValidation="true" />
                        <asp:Button ID="btnCancelRevertSuspend" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
                    </div>
                </div>
            </asp:Panel>    
    </asp:View>
    </asp:MultiView>
</asp:Panel>
<asp:Button runat="server" ID="btnDummy" aria-Label="Dummybutton" Style="display: none" Text="btnDummy" />