<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ServiceFacilityInformation, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script type="text/javascript">
    function loaderServiceFacility(e) {
        getTextboxdetail(e);

        var ServiceFacilityNpi = $("#<%= txtNPIServiceFacilityLocation.ClientID %>").val();
        var ServiceFacilitynpiLenth = ServiceFacilityNpi.length;
        if ((ServiceFacilitynpiLenth > 0) && (ServiceFacilitynpiLenth < 10)) {
            $("#<%= errServiceFacilityInfo.ClientID %>").text('10 digit NPI is required');
            ServiceFacilityClerFields();
            localStorage.setItem("ServiceFacilityNPI", "");
            return false;
        }

        else if (ServiceFacilitynpiLenth == 0) {
            ServiceFacilityClerFields();
            return false;
        }
        else if (ServiceFacilitynpiLenth == 10 && ServiceFacilitynpiLenth !== null && ServiceFacilitynpiLenth !== undefined) {
            var txtServiceFacilityNpiCode = $("#<%= txtNPIServiceFacilityLocation.ClientID %>").first().val();
            var txtServiceFacilitydMedId = $("#<%= hdnServiceFacilityMedId.ClientID %>").first().val(); // lblServiceFacilityLocationMedicaid.ClientID %>").text();

            $("#<%= errServiceFacilityInfo.ClientID %>").html("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetnpiCountServiceFacility?ServiceNpi=" + txtServiceFacilityNpiCode + "&&ServiceMedId=" + txtServiceFacilitydMedId + "",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result === "" || result === 0) {
                        $("#<%= errServiceFacilityInfo.ClientID %>").text("NPI is not found in the system");
                        ServiceFacilityClerFields();
                        return false;
                    }
                    else {
                        $("#<%= errServiceFacilityInfo.ClientID %>").removeAttr("disabled");
                        if (result == "NPI is not found in the system") {
                            $("#<%= errServiceFacilityInfo.ClientID %>").text("NPI is not found in the system");
                            ServiceFacilityClerFields();
                            return false;
                        }
                        else if (result == "10-digit number is required") {
                            $("#<%= errServiceFacilityInfo.ClientID %>").text("10 - digit number is required");
                            ServiceFacilityClerFields();
                            return false;
                        }
                        else if (result == "Non-individual provider cannot be entered as referring provider") {
                            $("#<%= errServiceFacilityInfo.ClientID %>").text("Individual provider cannot be entered as facility provider");
                            ServiceFacilityClerFields();
                            return false;
                        }
                        else if (result !== null && result !== undefined && result !== "NPI is not found in the system" && result !== "10 - digit number is required") {
                            $("#<%= errServiceFacilityInfo.ClientID %>").text('');
                            // $("#<%= lblServiceFacilityLocationMedicaid.ClientID %>").html(result[0]["MEDICAID_ID"]);
                            var ServiceFacilityLocationMedicaid = $("#<%= lblServiceFacilityLocationMedicaid.ClientID %>").html();
                            localStorage.setItem("ServiceFacilityLocationMedicaid", ServiceFacilityLocationMedicaid);
                            var lname = result[0]["LAST_OR_BUSINESS_NAME"];
                            var fname = result[0]["FIRST_NAME"];
                            var name = "";
                            if ((lname !== null) && (lname !== "undefined"))
                                name = lname;
                            else
                                lname = "";
                            if ((fname !== null) && (fname !== "undefined"))
                                name = lname + " " + fname;
                            $("#<%= lblServiceFacilityLocationName.ClientID %>").html(name);
                            $("#<%= lblServiceFacilityLocationAddress1.ClientID %>").html(result[0]["ADDRESS1"]);
                            $("#<%= lblServiceFacilityLocationAddress2.ClientID %>").html(result[0]["ADDRESS2"]);
                            $("#<%= lblServiceFacilityLocationCity.ClientID %>").html(result[0]["CITY"]);
                            $("#<%= lblServiceFacilityLocationState.ClientID %>").html(result[0]["STATE"]);
                            $("#<%= lblServiceFacilityLocationZip.ClientID %>").html(result[0]["ZIP"]);

                            $("#<%= hdnServiceFacilityMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                            $("#<%= hdnServiceFacilityName.ClientID %>").val(result[0]["NAME"]);
                            $("#<%= hdnServiceFacilityAddress1.ClientID %>").val(result[0]["ADDRESS1"]);
                            $("#<%= hdnServiceFacilityAddress2.ClientID %>").val(result[0]["ADDRESS2"]);
                            $("#<%= hdnServiceFacilityCity.ClientID %>").val(result[0]["CITY"]);
                            $("#<%= hdnServiceFacilityState.ClientID %>").val(result[0]["STATE"]);
                            $("#<%= hdnServiceFacilityZip.ClientID %>").val(result[0]["ZIP"]);

                            var medid = result[0]["MEDICAID_ID"];

                            if (medid !== null && medid !== undefined)
                            {

                                $("#<%= errServiceFacilityInfo.ClientID %>").text("");
                                var APIToken = $("[id*=hdnAccessToken]").val();
                                $.ajax({
                                    type: "GET",
                                    url: webApiClaims + "GetnpiCountServiceFacility?ServiceNpi=" + txtServiceFacilityNpiCode + "&&ServiceMedId=" + medid + "",
                                    headers: {
                                        "Access-Control-Allow-Origin": "*",
                                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                        "Authorization": "Bearer " + APIToken
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        if (result == "Individual provider cannot be entered as facility provider") {
                                            $("#<%= errServiceFacilityInfo.ClientID %>").text("Individual provider cannot be entered as facility provider");
                                            ServiceFacilityClerFields();
                                            return false;
                                        }
                                        else if (result == "2" || result == 0) {
                                            ServiceFacilityClerFields();
                                            return false;
                                        }
                                        else {
                                            $("#<%= errServiceFacilityInfo.ClientID %>").text('');
                                            var fName = result[0]["FIRST_NAME"];
                                            var lName = result[0]["LAST_OR_BUSINESS_NAME"];
                                            if ((fName === undefined) || (fname === "undefined"))
                                                fName = "";
                                            if ((lName === undefined) || (lname === "undefined"))
                                                lName = "";
                                            if (!(fName === "" && lName === ""))
                                                name = fName + " " + lName;

                                            // $("#<%= lblServiceFacilityLocationMedicaid.ClientID %>").html(result[0]["MEDICAID_ID"]);
                                            $("#<%= lblServiceFacilityLocationName.ClientID %>").html(name);
                                            $("#<%= lblServiceFacilityLocationAddress1.ClientID %>").html(result[0]["ADDRESS1"]);
                                            $("#<%= lblServiceFacilityLocationAddress2.ClientID %>").html(result[0]["ADDRESS2"]);
                                            $("#<%= lblServiceFacilityLocationCity.ClientID %>").html(result[0]["CITY"]);
                                            $("#<%= lblServiceFacilityLocationState.ClientID %>").html(result[0]["STATE"]);
                                            $("#<%= lblServiceFacilityLocationZip.ClientID %>").html(result[0]["ZIP"]);

                                            $("#<%= hdnServiceFacilityMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                                            $("#<%= hdnServiceFacilityName.ClientID %>").val(result[0]["FIRST_NAME"] + " " + result[0]["LAST_OR_BUSINESS_NAME"]);
                                            $("#<%= hdnServiceFacilityAddress1.ClientID %>").val(result[0]["ADDRESS1"]);
                                            $("#<%= hdnServiceFacilityAddress2.ClientID %>").val(result[0]["ADDRESS2"]);
                                            $("#<%= hdnServiceFacilityCity.ClientID %>").val(result[0]["CITY"]);
                                            $("#<%= hdnServiceFacilityState.ClientID %>").val(result[0]["STATE"]);
                                            $("#<%= hdnServiceFacilityZip.ClientID %>").val(result[0]["ZIP"]);

                                        }

                                    },

                                });
                            }
                            return false;
                        }
                    }
                },

            });
        }
        return false;
    }
    function ServiceFacilityClerFields() {
        localStorage.setItem("ServiceFacilityNPI", "");
        localStorage.setItem("ServiceFacilityLocationMedicaid", "");
        $("#<%= txtNPIServiceFacilityLocation.ClientID %>").val("");
        $("#<%= lblServiceFacilityLocationMedicaid.ClientID %>").text('');
        $("#<%= lblServiceFacilityLocationName.ClientID %>").text('');
        $("#<%= lblServiceFacilityLocationAddress1.ClientID %>").text('');
        $("#<%= lblServiceFacilityLocationAddress2.ClientID %>").text('');
        $("#<%= lblServiceFacilityLocationCity.ClientID %>").text('');
        $("#<%= lblServiceFacilityLocationState.ClientID %>").text('');
        $("#<%= lblServiceFacilityLocationZip.ClientID %>").text('');

        $("#<%= lblServiceFacilityLocationMedicaid.ClientID %>").html('');
        $("#<%= lblServiceFacilityLocationName.ClientID %>").html('');
        $("#<%= lblServiceFacilityLocationAddress1.ClientID %>").html('');
        $("#<%= lblServiceFacilityLocationAddress2.ClientID %>").html('');
        $("#<%= lblServiceFacilityLocationCity.ClientID %>").html('');
        $("#<%= lblServiceFacilityLocationState.ClientID %>").html('');
        $("#<%= lblServiceFacilityLocationZip.ClientID %>").html('');

        $("#<%= hdnServiceFacilityMedId.ClientID %>").val('');
        $("#<%= hdnServiceFacilityName.ClientID %>").val('');
        $("#<%= hdnServiceFacilityAddress1.ClientID %>").val('');
        $("#<%= hdnServiceFacilityAddress2.ClientID %>").val('');
        $("#<%= hdnServiceFacilityCity.ClientID %>").val('');
        $("#<%= hdnServiceFacilityState.ClientID %>").val('');
        $("#<%= hdnServiceFacilityZip.ClientID %>").val('');
    }

    $(document).ready(function () {
        $("#divSubmitClaimSearchPage").hide();
        $("#btnSearch").click(function () {
            $("#divSubmitClaimSearchPage").show();
        })
    });

    //function getbuttondetail(e) {
    //    $('#hdnSearchId').val(e.id);
    //    $("#divSubmitClaimSearchPage").hide();
    //}
    function closemodalServiceFacility() {
        debugger;
        var hdn_NPI = $('#ctl00_MainContent_uc5SubmitClaim_hdnNPI').val();
        var hdn_MedicaidId = $('#ctl00_MainContent_uc5SubmitClaim_hdnMedicaidId').val();
        var hdn_ProviderName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderName').val();
        var hdn_ProviderLastName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderLastName').val();
        var hdn_ProviderFirstName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderFirstName').val();
        var hdn_ProviderAddress = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderAddress').val();
        var hdn_ProviderCity = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderCity').val();
        var hdn_ProviderState = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderState').val();
        var hdn_ProviderZip = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderZip').val();
        var hdn_ProviderAddress2 = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderAddress2').val();
        var hdn_SearchId = $('#hdnSearchId').val();
        if (hdn_SearchId == 'btnSearch9') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPIServiceFacilityLocation').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidIdServiceFacilityLocation').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtNameServiceFacilityLocation').val(hdn_ProviderName);
            $('#ctl00_MainContent_uc5SubmitClaim_txtAddressServiceFacilityLocation').val(hdn_ProviderAddress);
            $('#ctl00_MainContent_uc5SubmitClaim_txtCityServiceFacilityLocation').val(hdn_ProviderCity);
            $('#ctl00_MainContent_uc5SubmitClaim_txtStateServiceFacilityLocation').val(hdn_ProviderState);
            $('#ctl00_MainContent_uc5SubmitClaim_txtZipServiceFacilityLocation').val(hdn_ProviderZip);
            $('#ctl00_MainContent_uc5SubmitClaim_txtAddressServiceFacilityLocation2').val(hdn_ProviderAddress2);
        }
        $('#myModal').modal('hide');
    }
</script>



<ajax:CollapsiblePanelExtender ID="cpeServiceFacility" runat="server" Collapsed="true"
    TargetControlID="pnlServiceFacilityLocationInformation" ExpandControlID="pnlSepServiceFacilityLocationInformation"
    CollapseControlID="pnlSepServiceFacilityLocationInformation" />
<asp:Panel runat="server" ID="pnlSepServiceFacilityLocationInformation" class="CollapsingSeparator"
    onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
    ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
    <span id="Span2" runat="server" class="pageHeader pH2">+ SERVICE FACILITY LOCATION INFORMATION
    </span>

</asp:Panel>
<asp:Panel ID="pnlServiceFacilityLocationInformation" runat="server" Style="overflow-x: hidden">
    <asp:UpdatePanel ID="uppnlServiceFacilityLocationInformation" runat="server">
        <ContentTemplate>


            <div class="row" id="divServiceFacilityLocationInformation" runat="server" style="background-color: lightblue;">
                <div class="col-sm-2">
                    <span class="ohio-field-label" style="font-size: 17px; padding-left: 30px; font-weight: bold;">
                        <span style="color: red">*</span>NPI</span>
                </div>
                <div class="col-sm-1">
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">Medicaid ID</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Name</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Address1</span>
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Address2</span>
                </div>
                <div class="col-sm-1">
                    <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">City</span>
                </div>
                <div class="col-sm-1">
                    <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">State</span>
                </div>
                <div class="col-sm-1">
                    <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Zip</span>
                </div>
            </div>
            <div class="row" id="serviceFacilityInformationPanel">
                <asp:HiddenField ID="hdnServiceFacilityInfoId" runat="server" />
                <div class="col-sm-2">

                    <div class="col-sm-12">
                        <span>


                            <asp:TextBox ID="txtNPIServiceFacilityLocation" runat="server" CssClass="formFieldcalender"
                                MaxLength="10" Style="height: 30px; width: 100px;" OnChange="return loaderServiceFacility(this);"
                                AutoPostBack="true" data-target="#myModalpop" />
                            <asp:Label ID="Searchdv" runat="server">
                                <button id="btnloading" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>


                                <button type="button" id="btnSearch9" onclick="getbuttondetail(this)" class="btn btn-link"
                                    data-toggle="modal" data-target="#myModal">
                                    Search   
                                </button>
                            </asp:Label>

                            <%-- <asp:RequiredFieldValidator ID="rfvServiceFacilityNPI" runat="server" ControlToValidate="txtNPIServiceFacilityLocation"
                                ErrorMessage="<div id='msg8_txtNPIServiceFacilityLocation' ><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation' class='errorlist'>*Service Facility Location NPI 10-digit number is required.</a></div>"
                                Display="None" ValidationGroup="validate"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator7"
                                Display="None"
                                ValidationGroup="validate"
                                ControlToValidate="txtNPIServiceFacilityLocation"
                                ValidationExpression="^[0-9]{10}$"
                                Text="Service facility provider is not found in the system."
                                ErrorMessage="*Service Facility location NPI 10-digit is required">
                            </asp:RegularExpressionValidator>--%>
                        </span>
                        <asp:Label ID="errServiceFacilityInfo" Style="color: red" runat="server"></asp:Label>
                    </div>

                </div>
                <div class="col-sm-1">
                    <div class="row" id="lblServiceFacilityLocationMedicaid" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: center; padding-left: 25px;">
                                <asp:Label ID="txtMedicaidIdServiceFacilityLocation" runat="server" MaxLength="7"
                                    Style="background-color: transparent;" ReadOnly="true" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="row" id="lblServiceFacilityLocationName" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: left;">
                                <asp:Label ID="txtNameServiceFacilityLocation" runat="server" MaxLength="60" Style="background-color: transparent;"
                                    ReadOnly="true" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="row" id="lblServiceFacilityLocationAddress1" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: left;">
                                <asp:Label ID="txtAddressServiceFacilityLocation" runat="server" MaxLength="55" Style="background-color: transparent;"
                                    ReadOnly="true" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="row" id="lblServiceFacilityLocationAddress2" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: left;">
                                <asp:Label ID="txtAddressServiceFacilityLocation2" runat="server" MaxLength="55"
                                    Style="background-color: transparent;" ReadOnly="true" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="row" id="lblServiceFacilityLocationCity" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: left;">
                                <asp:Label ID="txtCityServiceFacilityLocation" runat="server" MaxLength="30" Style="background-color: transparent;"
                                    ReadOnly="true" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="row" id="lblServiceFacilityLocationState" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: left;">
                                <asp:Label ID="txtStateServiceFacilityLocation" runat="server" MaxLength="2" Style="background-color: transparent; width: 100px"
                                    ReadOnly="true" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <div class="row" id="lblServiceFacilityLocationZip" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: left;">
                                <asp:Label ID="txtZipServiceFacilityLocation" runat="server" MaxLength="5" Style="background-color: transparent; width: 100px; text-align: left;"
                                    ReadOnly="true" />

                            </span>
                        </div>
                    </div>
                </div>
            </div>





        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Panel>

<asp:HiddenField ID="hdnClaimId" runat="server" />
<asp:HiddenField ID="hdnServiceFacilityMedId" runat="server" />
<asp:HiddenField ID="hdnServiceFacilityName" runat="server" />
<asp:HiddenField ID="hdnServiceFacilityAddress1" runat="server" />
<asp:HiddenField ID="hdnServiceFacilityAddress2" runat="server" />
<asp:HiddenField ID="hdnServiceFacilityCity" runat="server" />
<asp:HiddenField ID="hdnServiceFacilityState" runat="server" />
<asp:HiddenField ID="hdnServiceFacilityZip" runat="server" />
