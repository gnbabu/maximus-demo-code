<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_ReferringProvider" Codebehind="ReferringProvider.ascx.cs" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script type="text/javascript">
    
    function loaderReferringProvider(e) {
        $("#<%= errReferringProviderNPI.ClientID %>").text('');
            getTextboxdetail(e);
            var refernpi = $("#<%= txtReferringProviderNPI.ClientID %>").val();
            var npiLenth = refernpi.length;
            if ((npiLenth > 0) && (npiLenth < 10)) {

                ReferringClerFields();
                primarycareClerFields();
                localStorage.setItem("ReferingProviderNPI", "");
                $("#<%= errReferringProviderNPI.ClientID %>").text("10 digit NPI is required");
            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", true);
            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "LightGray" });
            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                return false;
            }
            localStorage.setItem("ReferingProviderNPI", "" + refernpi + "");
            var renderingprovnpi = localStorage.getItem("RenderingProviderNPI");
        if (!(refernpi === null || refernpi === undefined || refernpi === "" || refernpi.length == 0) && !(renderingprovnpi === null || renderingprovnpi === undefined || renderingprovnpi === "" || renderingprovnpi.length == 0) && refernpi == renderingprovnpi) {
            $("#<%= errReferringProviderNPI.ClientID %>").text("Referring Provider cannot be same as Rendering Provider");
            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", true);
            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "LightGray" });
            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
            ReferringClerFields();
            primarycareClerFields();
            return false;
        }
        else if (npiLenth == 10 && npiLenth !== null && npiLenth !== undefined) {
            var txtrefNpiCode = $("#<%= txtReferringProviderNPI.ClientID %>").first().val();
            var txtrefMedId = $("#<%= hdnRefMedId.ClientID %>").first().val(); // lblRefProviderMedicaidID.ClientID %>").text();

            var hdnReferringProviderClaimType = $("#<%= hdnReferringProviderClaimType.ClientID %>").first().val();
            $("#<%= errReferringProviderNPI.ClientID %>").text("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetnpiCount?RefNpi=" + txtrefNpiCode + "&&RefMedId=" + txtrefMedId + "&&ClaimType=" + hdnReferringProviderClaimType + "",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result === "") {
                        $("#<%= errReferringProviderNPI.ClientID %>").text("NPI is not found in the system");

                        $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", true);
                        $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "LightGray" });
                        $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                        ReferringClerFields();
                        primarycareClerFields();
                        return false;
                    }
                    else {
                        $("#<%= txtPrimaryCareProviderNPI.ClientID %>").removeAttr("disabled");
                        var txtrefNpiCode = $("#<%= txtReferringProviderNPI.ClientID %>").first().val();
                        if (result == "NPI is not found in the system") {
                            $("#<%= errReferringProviderNPI.ClientID %>").text("NPI is not found in the system");
                            <%--$("#<%= txtPrimaryCareProviderNPI.ClientID %>").attr("disabled", "enabled");--%>
                            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", true);
                            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "LightGray" });
                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                            ReferringClerFields();
                            primarycareClerFields();
                            return false;
                        }
                        else if (result == "10-digit number is required") {
                            $("#<%= errReferringProviderNPI.ClientID %>").text("10 - digit number is required");
                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                            ReferringClerFields();
                            primarycareClerFields();
                            return false;
                        }
                        else if (result == "Non-individual provider cannot be entered as referring provider") {

                            $("#<%= errReferringProviderNPI.ClientID %>").text("Non-individual provider cannot be entered as referring provider");
                            <%--$("#<%= txtPrimaryCareProviderNPI.ClientID %>").attr("disabled", "enabled");--%>
                            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", true);
                            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "LightGray" });
                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                            ReferringClerFields();
                            primarycareClerFields();
                            return false;

                        }
                        else if (result !== null && result !== undefined && result !== "NPI is not found in the system" && result !== "10 - digit number is required") {
                            $("#<%= errReferringProviderNPI.ClientID %>").text('');
                            // $("#<%= lblRefProviderMedicaidID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                            var RefProviderMedicaidID = result[0]["MEDICAID_ID"];
                            localStorage.setItem("RefProviderMedicaidID", RefProviderMedicaidID);
                            $("#<%= lblReffProviderFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                            $("#<%= lblReffProviderLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                            $("#<%= hdnRefMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                            $("#<%= hdnRefFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                            $("#<%= hdnRefLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
                            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").attr("disabled", "disabled");
                            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "LightGray" });
                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');

                            var medid = result[0]["MEDICAID_ID"];
                            var txtrefNpiCode = $("#<%= txtReferringProviderNPI.ClientID %>").first().val();
                            if (medid !== null && medid !== undefined) {

                                $("#<%= errReferringProviderNPI.ClientID %>").text('');
                                var APIToken = $("[id*=hdnAccessToken]").val();
                                $.ajax({
                                    type: "GET",
                                    url: webApiClaims + "GetnpiCount?RefNpi=" + txtrefNpiCode + "&&RefMedId=" + medid + "&&ClaimType=" + hdnReferringProviderClaimType + "",
                                    headers: {
                                        "Access-Control-Allow-Origin": "*",
                                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                        "Authorization": "Bearer " + APIToken
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        if (result == "Non-individual provider cannot be entered as referring provider") {

                                            $("#<%= errReferringProviderNPI.ClientID %>").html("Non-individual provider cannot be entered as referring provider");
                                           <%-- $("#<%= txtPrimaryCareProviderNPI.ClientID %>").attr("disabled", "enabled");--%>
                                            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", true);
                                            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "LightGray" });
                                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                                            ReferringClerFields();
                                            primarycareClerFields();
                                            return false;
                                        }
                                        else {
                                            $("#<%= errReferringProviderNPI.ClientID %>").text('');
                                            // $("#<%= lblRefProviderMedicaidID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                                            $("#<%= lblReffProviderFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                                            $("#<%= lblReffProviderLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                                            $("#<%= hdnRefMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                                            $("#<%= hdnRefFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                                            $("#<%= hdnRefLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
                                            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", false);
                                          <%--  $("#<%= txtPrimaryCareProviderNPI.ClientID %>").attr("disabled", "disabled");--%>
                                            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "" });

                                        }

                                    },
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        $("#<%= errReferringProviderNPI.ClientID %>").text("Something Went Wrong..Try again");
                                    }
                                });

                            }
                            return false;
                        }

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("#<%= errReferringProviderNPI.ClientID %>").text("Something Went Wrong..Try again");
                }
            });
        }
        else if ((($("#<%= lblRefProviderMedicaidID.ClientID %>").text() === null) || ($("#<%= lblRefProviderMedicaidID.ClientID %>").text() === undefined)) && (($("#<%= txtReferringProviderNPI.ClientID %>").val() === null) || ($("#<%= txtReferringProviderNPI.ClientID %>").val() === undefined))) {
            $("#<%= errReferringProviderNPI.ClientID %>").html("NPI is Unknown");
            ReferringClerFields();
            primarycareClerFields();
            return false;
        }
        else if ((($("#<%= txtReferringProviderNPI.ClientID %>").text() === null) || ($("#<%= txtReferringProviderNPI.ClientID %>").text() === undefined)) && (($("#<%= txtPrimaryCareProviderNPI.ClientID %>").val() === null) || ($("#<%= txtPrimaryCareProviderNPI.ClientID %>").val() === undefined))) {
            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", false);
            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "" });
            return false;
        }
        else if (npiLenth == 0) // NPI was removed, clear out fields
        {
            $("#<%= lblRefProviderMedicaidID.ClientID %>").html('');
            $("#<%= lblReffProviderFirstName.ClientID %>").html('');
            $("#<%= lblReffProviderLastName.ClientID %>").html('');

            $("#<%= lblRefProviderMedicaidID.ClientID %>").text('');
            $("#<%= lblReffProviderFirstName.ClientID %>").text('');
            $("#<%= lblReffProviderLastName.ClientID %>").text('');

            $("#<%= hdnRefMedId.ClientID %>").val('');
            $("#<%= hdnRefFirstName.ClientID %>").val('');
            $("#<%= hdnRefLastName.ClientID %>").val('');
        }
        return false;
    }

    function ReferringClerFields() {
        localStorage.setItem("ReferingProviderNPI", "");
        localStorage.setItem("RefProviderMedicaidID", "");
        $("#<%= txtReferringProviderNPI.ClientID %>").val("");
        $("#<%= lblRefProviderMedicaidID.ClientID %>").text('');
        $("#<%= lblReffProviderLastName.ClientID %>").text('');
        $("#<%= lblReffProviderFirstName.ClientID %>").text('');
        $("#<%= hdnRefMedId.ClientID %>").val('');
        $("#<%= hdnRefFirstName.ClientID %>").val('');
        $("#<%= hdnRefLastName.ClientID %>").val('');

    }
    function loaderReferringProvider1(e, search = "false") {

        if (search == "true") {
            localStorage.setItem("ArgName", "btnSearchPrimaryCareProvider");
            localStorage.setItem("search", "hide");
            setTimeout(GetNPIDetails, 100);
        }
        else {
            localStorage.setItem("search", "");
        }
        $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
        getTextboxdetail(e);
        var primarynpi = $("#<%= txtPrimaryCareProviderNPI.ClientID %>").val();
        var npiLenth = primarynpi.length;
        if ((npiLenth >= 0) && (npiLenth < 10)) {
            primarycareClerFields();
            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("10 digit NPI is required");
            localStorage.setItem("PrimarycareReferingProviderNPI", "");
            return false;
        }
        localStorage.setItem("PrimarycareReferingProviderNPI", "" + primarynpi + "");
        var primaryprovider = $("#<%= txtPrimaryCareProviderNPI.ClientID %>").val();
        var referingprovider = $("#<%= txtReferringProviderNPI.ClientID %>").val();
        var renderingprovnpi = localStorage.getItem("RenderingProviderNPI");
        if (!(primaryprovider === null || primaryprovider === undefined || primaryprovider === "" || primaryprovider.length == 0) && !(referingprovider === null || referingprovider === undefined || referingprovider === "" || referingprovider.length == 0) && primaryprovider === referingprovider) {

            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("Primary Provider cannot be same as Referring Provider");
            primarycareClerFields();
            return false;
        }
        else if (!(primaryprovider === null || primaryprovider === undefined || primaryprovider === "" || primaryprovider.length == 0) && !(renderingprovnpi === null || renderingprovnpi === undefined || renderingprovnpi === "" || renderingprovnpi.length == 0) && primaryprovider == renderingprovnpi) {

            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("Rendering provider cannot be same as primary provider");
            $('#<%=txtPrimaryCareProviderNPI.ClientID %>').prop("disabled", false);
            $("#<%= txtPrimaryCareProviderNPI.ClientID %>").css({ "background-color": "" });
            primarycareClerFields();
            return false;
        }
        else if (npiLenth == 10 && npiLenth !== null && npiLenth !== undefined) {
            var txtPrimaryrefNpiCode = $("#<%= txtPrimaryCareProviderNPI.ClientID %>").first().val();
            var txtPrimaryrefMedId = $("#<%= hdnPrimaryRefMedId.ClientID %>").first().val(); // lbPrimaryCareProvMedicaidID.ClientID %>").text();

            var hdnReferringProviderClaimType = $("#<%= hdnReferringProviderClaimType.ClientID %>").first().val();
            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetnpiCount?RefNpi=" + txtPrimaryrefNpiCode + "&&RefMedId=" + txtPrimaryrefMedId + "&&ClaimType=" + hdnReferringProviderClaimType + "",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result === "") {
                        $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("NPI is not found in the system");

                        primarycareClerFields();
                        return false;
                    }
                    else {
                        $("#<%= txtPrimaryCareProviderNPI.ClientID %>").removeAttr("disabled");
                        if (result == "NPI is not found in the system") {
                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("NPI is not found in the system");
                            primarycareClerFields();
                            return false;
                        }
                        else if (result == "10-digit number is required") {
                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("10 - digit number is required");
                            primarycareClerFields();
                            return false;
                        }
                        else if (result == "Non-individual provider cannot be entered as referring provider") {

                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("Non-individual provider cannot be entered as Primary care provider");
                            primarycareClerFields();
                            return false;

                        }
                        if (result !== null && result !== undefined && result !== "NPI is not found in the system" && result !== "10 - digit number is required") {
                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                            // $("#<%= lbPrimaryCareProvMedicaidID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                            var PrimaryCareProvMedicaidID = result[0]["MEDICAID_ID"];
                            localStorage.setItem("PrimaryCareProvMedicaidID", PrimaryCareProvMedicaidID);
                            $("#<%= lblPrimaryCareProvFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                            $("#<%= lblPrimaryCareProvLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                            $("#<%= hdnPrimaryRefMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                            $("#<%= hdnPrimaryRefFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                            $("#<%= hdnPrimaryRefLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);

                            var medid = result[0]["MEDICAID_ID"];
                            if (medid !== null && medid !== undefined) {

                                $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                                var APIToken = $("[id*=hdnAccessToken]").val();
                                $.ajax({
                                    type: "GET",
                                    url: webApiClaims + "GetnpiCount?RefNpi=" + txtPrimaryrefNpiCode + "&&RefMedId=" + medid + "&&ClaimType=" + hdnReferringProviderClaimType + "",
                                    headers: {
                                        "Access-Control-Allow-Origin": "*",
                                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                        "Authorization": "Bearer " + APIToken
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        if (result == "Non-individual provider cannot be entered as referring provider") {

                                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").html("Non-individual provider cannot be entered as primary care provider");

                                            primarycareClerFields();
                                            return false;
                                        }
                                        else {
                                            $("#<%= errPrimaryCareProviderNPI.ClientID %>").text('');
                                            // $("#<%= lbPrimaryCareProvMedicaidID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                                            $("#<%= lblPrimaryCareProvFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                                            $("#<%= lblPrimaryCareProvLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                                            $("#<%= hdnPrimaryRefMedId.ClientID %>").val(result[0]["MEDICAID_ID"]);
                                            $("#<%= hdnPrimaryRefFirstName.ClientID %>").val(result[0]["FIRST_NAME"]);
                                            $("#<%= hdnPrimaryRefLastName.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
                                        }
                                    },
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("Something Went Wrong..Try again");
                                    }
                                });

                            }
                            return false;
                        }

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("#<%= errPrimaryCareProviderNPI.ClientID %>").text("Something Went Wrong..Try again");
                }
            });
        }
        else if (npiLenth == 0) {
            $("#<%= lbPrimaryCareProvMedicaidID.ClientID %>").html('');
            var PrimaryCareProvMedicaidID = $("#<%= lbPrimaryCareProvMedicaidID.ClientID %>").html();
            localStorage.setItem("PrimaryCareProvMedicaidID", PrimaryCareProvMedicaidID);
            $("#<%= lblPrimaryCareProvFirstName.ClientID %>").html('');
            $("#<%= lblPrimaryCareProvLastName.ClientID %>").html('');

            $("#<%= lbPrimaryCareProvMedicaidID.ClientID %>").text('');
            $("#<%= lblPrimaryCareProvFirstName.ClientID %>").text('');
            $("#<%= lblPrimaryCareProvLastName.ClientID %>").text('');

            $("#<%= hdnPrimaryRefMedId.ClientID %>").val('');
            $("#<%= hdnPrimaryRefFirstName.ClientID %>").val('');
            $("#<%= hdnPrimaryRefLastName.ClientID %>").val('');
        }

        return false;

    }
    function primarycareClerFields() {
        localStorage.setItem("PrimarycareReferingProviderNPI", "");
        localStorage.setItem("PrimaryCareProvMedicaidID", "");
        $("#<%= txtPrimaryCareProviderNPI.ClientID %>").val("");
        $("#<%= lbPrimaryCareProvMedicaidID.ClientID %>").html('');
        $("#<%= lblPrimaryCareProvLastName.ClientID %>").html('');
        $("#<%= lblPrimaryCareProvFirstName.ClientID %>").html('');
        $("#<%= hdnPrimaryRefMedId.ClientID %>").val('');
        $("#<%= hdnPrimaryRefFirstName.ClientID %>").val('');
        $("#<%= hdnPrimaryRefLastName.ClientID %>").val('');

    }


</script>

<asp:UpdatePanel ID="upnlReferringProvider" runat="server">
    <ContentTemplate>
        <div class="row" style="background-color: #ADD8E6;">
            <div class="col-md-5">
                <span class="ohio-field-label" style="font-size: 17px; text-align: center; font-weight: bold; padding-left: 10px;"><span style="color: red">*</span>NPI</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">Medicaid ID</span>
            </div>
            <div class="col-md-2">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">Last Name</span>
            </div>
            <div class="col-md-2">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">First Name</span>
            </div>
        </div>

        <div class="row" id="dvReferringProvider" runat="server">
            <div class="col-sm-5">
                <div class="row" id="lblReferringProviderNPI" runat="server" style="width: 115%;">
                    <div class="col-sm-4">
                        <div id="dvlblReferringProvider" runat="server"><span class="ohio-field" style="font-size: 15px; text-align: center">Referring Provider</span></div>
                    </div>
                    <div class="col-sm-8">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtReferringProviderNPI" runat="server" OnChange="return loaderReferringProvider(this);" CssClass="formFieldTextBox" MaxLength="10" Style="height: 30px; width: 200px;"></asp:TextBox>
                            <%--OnTextChanged="txtReferringProviderNPI_TextChanged"--%>
                            <asp:Label ID="SearchReferringProviderNPI" runat="server">
                                <button id="btnloading" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>


                                <button type="button" id="btnSearch6" onclick="getbuttondetail(this)" class="btn btn-link"
                                    data-toggle="modal" data-target="#myModal">
                                    Search
                                </button>
                            </asp:Label>
                            <asp:RequiredFieldValidator ID="rfvReferringProviderNPI" runat="server" ControlToValidate="txtReferringProviderNPI"
                                ErrorMessage="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI' class='errorlist'> *Referring Provider NPI 10-digit number is required</a></div>" Display="None" ValidationGroup="validate"></asp:RequiredFieldValidator>
                        </span>
                        <br />
                        <asp:Label ID="errReferringProviderNPI" Style="color: red" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="lblReffProMedicaidID" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: left;">

                            <asp:Label ID="lblRefProviderMedicaidID" runat="server" Style="height: 30px; width: 200px; padding-left: 1rem;" />

                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="row" id="lblRefProviderLastName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: left;">
                            <asp:Label ID="lblReffProviderLastName" runat="server" Style="height: 30px; width: 200px; padding-left: 3rem;" />

                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="row" id="lblRefProviderFirstName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: left;">
                            <asp:Label ID="lblReffProviderFirstName" runat="server" Style="height: 30px; width: 200px; padding-left: 5rem;" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" id="dvPrimaryCareProvider" runat="server">
            <div class="col-sm-5">
                <div class="row" id="lblPrimaryCareProvider" runat="server" style="width: 115%;">
                    <div class="col-sm-4">
                        <span class="ohio-field" style="font-size: 15px; text-align: center">Primary Care Provider</span>
                    </div>
                    <div class="col-sm-8">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtPrimaryCareProviderNPI" OnChange="return loaderReferringProvider1(this)" runat="server" CssClass="formFieldTextBox" MaxLength="10" Style="height: 30px; width: 200px;"></asp:TextBox>
                            <button id="btnloading2" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                            <asp:Label ID="SearchPrimaryCareProviderNPI" runat="server">
                                
                            
                            <button type="button" id="btnSearchPrimaryCareProvider" onclick="getbuttondetail(this)"
                            class="btn btn-link" data-toggle="modal" data-target="#myModal">
                            Search
                            </button>
                            </asp:Label><br />
                            <asp:RequiredFieldValidator ID="rfvprimarycareprovider" runat="server" ControlToValidate="txtReferringProviderNPI"
                                ErrorMessage="<div>*Primary Care Provider NPI 10-digit number is required</div>"
                                Text="*" Display="None" ValidationGroup="validateClaims1"></asp:RequiredFieldValidator>
                        </span>

                        <asp:Label ID="errPrimaryCareProviderNPI" Style="color: red" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-sm-3">
                <div class="row" id="lblPrimaryCareProvMedicaidID" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: left;">
                            <asp:Label ID="lbPrimaryCareProvMedicaidID" runat="server" Style="height: 30px; width: 200px; padding-left: 1rem;" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="row" id="lblPrimaryCareProLastName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: left;">
                            <asp:Label ID="lblPrimaryCareProvLastName" runat="server" Style="height: 30px; width: 200px; padding-left: 3rem;" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="row" id="lblPrimaryCareProFirstName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: left;">
                            <asp:Label ID="lblPrimaryCareProvFirstName" runat="server" Style="height: 30px; width: 200px; padding-left: 5rem;" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div id="divRefErrorMessage" class="row" runat="server" style="display: none">
            <div class="col-sm-12">
                <asp:Label ID="lblRefErrorMessage" runat="server" Text="Referring provider and Primary care provider cannot be same." CssClass="failureNotification"></asp:Label>
            </div>
        </div>
        <div class="col-sm-12 row">
            <asp:Label ID="lblCompareErrorMes" runat="server" CssClass="failureNotification"></asp:Label>
        </div>
        <asp:HiddenField ID="hdnReferringProviderClaimID" runat="server" />
        <asp:HiddenField ID="hdnReferringProviderClaimType" runat="server" />
        <asp:HiddenField ID="hdnErrorMessageRef" runat="server" />
        <asp:HiddenField ID="hdnErrorMessagePri" runat="server" />
        <asp:HiddenField ID="hdnErrorRef" runat="server" />
        <asp:HiddenField ID="hdnErrorPri" runat="server" />
        <asp:HiddenField ID="hdnRefMedId" runat="server" />
        <asp:HiddenField ID="hdnRefFirstName" runat="server" />
        <asp:HiddenField ID="hdnRefLastName" runat="server" />
        <asp:HiddenField ID="hdnPrimaryRefMedId" runat="server" />
        <asp:HiddenField ID="hdnPrimaryRefFirstName" runat="server" />
        <asp:HiddenField ID="hdnPrimaryRefLastName" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<%--<ajax:ModalPopupExtender BehaviorID="mpeNPISearchPopup" ID="mpeSubmitClaimSearchProc" runat="server" PopupControlID="pnlSubmitClaimSearchPop"--%>
<%--    TargetControlID="Button9" BackgroundCssClass="modalBackground" CancelControlID="btnCloseCH9" />

<asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 1px; min-width: 800px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSesarchPopHeader"  runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseCH9"  Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
            CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left:50px">NPI</span>
            <span style="padding-left:60px">MEDICAID ID</span>
            <span style="padding-left:50px">BUSINESS/LAST NAME</span>
            <span style="padding-left:60px">FIRST NAME</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <uc:NPISearchPopup runat="server" ID="ucNPISearchPopup" Visible="true" EnableViewState="true" />
            </div>
        </div>
        
    </asp:Panel>
</asp:Panel>--%>
<asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />
