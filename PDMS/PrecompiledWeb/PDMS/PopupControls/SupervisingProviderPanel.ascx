<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SupervisingProviderPanel, App_Web_l5y5araq" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">
    function loaderSupervisingProvider(e) {
        getTextboxdetail(e);
        $("#<%= errSupervisingProviderNPI.ClientID %>").text('');
            var SuperVisingnpi = $("#<%= txtSupervisingProviderNPI.ClientID %>").val();
            var SuperVisingnpiLenth = SuperVisingnpi.length;
            if ( (SuperVisingnpiLenth > 0) && (SuperVisingnpiLenth < 10) ) {
                $("#<%= errSupervisingProviderNPI.ClientID %>").text('10 digit NPI is required');
            SuperVisingClerFields();
            localStorage.setItem("SuperVisingProviderNPI", "");
            return false;
        }
        localStorage.setItem("SuperVisingProviderNPI", "" + SuperVisingnpi + "");
        var renderingprovnpi = localStorage.getItem("RenderingProviderNPI");
        var assistentSur = localStorage.getItem("AsstProviderNPI");
        if (!(renderingprovnpi === null || renderingprovnpi === undefined || renderingprovnpi === "" || renderingprovnpi.length == 0) && !(SuperVisingnpi === null || SuperVisingnpi === undefined || SuperVisingnpi === "" || SuperVisingnpi.length == 0) && SuperVisingnpi == renderingprovnpi) {
            $("#<%= errSupervisingProviderNPI.ClientID %>").text("Supervising Provider cannot be same as Rendering Provider");
            SuperVisingClerFields();
            return false;
        }
        else if (!(assistentSur === null || assistentSur === undefined || assistentSur === "" || assistentSur.length == 0) && !(SuperVisingnpi === null || SuperVisingnpi === undefined || SuperVisingnpi === "" || SuperVisingnpi.length == 0) && assistentSur === SuperVisingnpi) {
            if ($("#<%= hdnClaimType_SuperVising.ClientID %>").first().val() === "0" || $("#<%= hdnClaimType_SuperVising.ClientID %>").first().val() === "2") {
                $("#<%= errSupervisingProviderNPI.ClientID %>").text("Assistant surgeon and supervising provider cannot be same");
                AsstClerFields();
            }
            if ($("#<%= hdnClaimType_SuperVising.ClientID %>").first().val() === "1") {
                $("#<%= errSupervisingProviderNPI.ClientID %>").text("Operating Physician cannot be same as Other Operating Physician Provider");
                AsstClerFields();
            }
            return false;
        }
        else if (SuperVisingnpiLenth == 10 && SuperVisingnpiLenth !== null && SuperVisingnpiLenth !== undefined) {
            var txtrefNpiCode = $("#<%= txtSupervisingProviderNPI.ClientID %>").first().val();
            var txtrefMedId = $("#<%= hdnMedicaidSuper.ClientID %>").first().val();

            var hdnSupervisingProviderClaimType = $("#<%= hdnClaimType_SuperVising.ClientID %>").first().val();
            $("#<%= errSupervisingProviderNPI.ClientID %>").text("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetAsstnpiCount?AsstNpi=" + txtrefNpiCode + "&&AsstMedId=" + txtrefMedId + "&&ClaimType=" + hdnSupervisingProviderClaimType + "",
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
                        $("#<%= errSupervisingProviderNPI.ClientID %>").text("NPI is not found in the system");
                        SuperVisingClerFields();
                        return false;
                    }
                    else {
                        if (result == "NPI is not found in the system") {
                            $("#<%= errSupervisingProviderNPI.ClientID %>").text("NPI is not found in the system");
                            SuperVisingClerFields();
                            return false;
                        }
                        else if (result == "10-digit number is required") {

                            $("#<%= errSupervisingProviderNPI.ClientID %>").text("10 - digit number is required");
                            SuperVisingClerFields();
                            return false;
                        }

                        else if (result !== null && result !== undefined && result !== "NPI is not found in the system" && result !== "10 - digit number is required") {
                            $("#<%= errSupervisingProviderNPI.ClientID %>").text('');
                            // $("#<%= lblSupervisingProviderMediID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                            var SupervisingProviderMediID = result[0]["MEDICAID_ID"];
                            localStorage.setItem("SupervisingProviderMediID", SupervisingProviderMediID);
                            $("#<%= lblSupervisingFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                            $("#<%= lblSupervisingLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                            $("#<%= hdnMedicaidSuper.ClientID %>").val(result[0]["MEDICAID_ID"]);
                            $("#<%= hdnFirstNameSuper.ClientID %>").val(result[0]["FIRST_NAME"]);
                            $("#<%= hdnLastNameSuper.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
                            var medid = result[0]["MEDICAID_ID"];
                            if (medid !== null && medid !== undefined) {

                                $("#<%= errSupervisingProviderNPI.ClientID %>").text('');
                                var APIToken = $("[id*=hdnAccessToken]").val();
                                $.ajax({
                                    type: "GET",
                                    url: webApiClaims + "GetAsstnpiCount?AsstNpi=" + txtrefNpiCode + "&&AsstMedId=" + medid + "&&ClaimType=" + hdnSupervisingProviderClaimType + "",
                                    headers: {
                                        "Access-Control-Allow-Origin": "*",
                                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                        "Authorization": "Bearer " + APIToken
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        if (result === "Non-individual provider cannot be entered as referring provider") {
                                            if (hdnSupervisingProviderClaimType == "0" || hdnSupervisingProviderClaimType == "2") {
                                                $("#<%= errSupervisingProviderNPI.ClientID %>").html("Non - individual provider cannot be entered as supervising provider");
                                                SuperVisingClerFields();
                                                return false;
                                            }
                                            else if (hdnSupervisingProviderClaimType == "1") {

                                                $("#<%= errSupervisingProviderNPI.ClientID %>").html("Non - individual provider cannot be entered as operative physician");
                                                SuperVisingClerFields();
                                                return false;
                                            }
                                        }
                                        if (result === "Non - individual provider cannot be entered as other operative physician") {

                                            $("#<%= errSupervisingProviderNPI.ClientID %>").html("Non - individual provider cannot be entered as operating physician");
                                            SuperVisingClerFields();
                                            return false;
                                        }
                                        else {
                                            $("#<%= errSupervisingProviderNPI.ClientID %>").text('');
                                            // $("#<%= lblSupervisingProviderMediID.ClientID %>").html(result[0]["MEDICAID_ID"]);
                                            $("#<%= lblSupervisingFirstName.ClientID %>").html(result[0]["FIRST_NAME"]);
                                            $("#<%= lblSupervisingLastName.ClientID %>").html(result[0]["LAST_OR_BUSINESS_NAME"]);

                                            $("#<%= hdnMedicaidSuper.ClientID %>").val(result[0]["MEDICAID_ID"]);
                                            $("#<%= hdnFirstNameSuper.ClientID %>").val(result[0]["FIRST_NAME"]);
                                            $("#<%= hdnLastNameSuper.ClientID %>").val(result[0]["LAST_OR_BUSINESS_NAME"]);
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
        else if (SuperVisingnpiLenth == 0) {
            SuperVisingClerFields();
        }
        return false;
    }
    function SuperVisingClerFields() {
        localStorage.setItem("SuperVisingProviderNPI", "");
        localStorage.setItem("SupervisingProviderMediID", "");
        $("#<%= txtSupervisingProviderNPI.ClientID %>").val("");
        $("#<%= lblSupervisingProviderMediID.ClientID %>").text('');       
        $("#<%= lblSupervisingLastName.ClientID %>").text('');        
        $("#<%= lblSupervisingFirstName.ClientID %>").text('');
        $("#<%= hdnMedicaidSuper.ClientID %>").val('');
        $("#<%= hdnFirstNameSuper.ClientID %>").val('');
        $("#<%= hdnLastNameSuper.ClientID %>").val('');

    }
</script>

<asp:Panel ID="pnlSupervisingProvider" runat="server" Style="overflow-x: hidden;">

    <asp:HiddenField ID="hdnClaimIdSuperVising" runat="server" />
    <asp:HiddenField ID="hdnClaimType_SuperVising" runat="server" />
    <asp:HiddenField ID="hdnMedicaidSuper" runat="server" />
    <asp:HiddenField ID="hdnLastNameSuper" runat="server" />
    <asp:HiddenField ID="hdnFirstNameSuper" runat="server" />
    <asp:HiddenField ID="hdnErrorMessageSuper" runat="server" />

    <asp:UpdatePanel ID="upnlSupervisingProvider" runat="server">
        <ContentTemplate>
            <div class="col-sm-12 row" id="divSupervisingRow" runat="server" style="background-color: lightblue;
                margin-left: 0px">
                <div class="col-md-3">
                    <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; text-align: left;
                        font-weight: bold; padding-left: 110px;">NPI</span>
                </div>
                <div class="col-md-3">
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;
                        padding-left: 70px;">Medicaid ID</span>
                </div>
                <div class="col-md-3">
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;
                        padding-left: 30px;">Last Name</span>
                </div>
                <div class="col-md-3">
                    <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold;">
                        First Name</span>
                </div>
            </div>
            <div class="row" style="padding-left: 100px;">
                <div class="col-sm-3">
                    <div class="row" id="lblSupervisingProviderNPI" runat="server">                        
                        <div class="col-sm-12">
                            <span style="text-align: left;">
                    <asp:TextBox ID="txtSupervisingProviderNPI" runat="server" CssClass="formFieldTextBox" MaxLength="10"
                                    Style="height: 30px; width: 200px;" OnChange="return loaderSupervisingProvider(this);"  />
                                <asp:Label ID="Searchdiv" runat="server">
                                     
                                      
                              
                                    <button type="button" id="btnSearch8"   onclick="getbuttondetail(this)" class="btn btn-link"
                                        data-toggle="modal"
                                        data-target="#myModal">
                                        Search
                                    </button>
                                </asp:Label>
                                <br />
                            </span>

                            <asp:Label ID="errSupervisingProviderNPI" Style="color: red" runat="server"></asp:Label>                             
                        </div>
                        <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="row" id="lblSupervisingProvMediID" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: justify;">
                                <asp:Label ID="lblSupervisingProviderMediID" runat="server" Style="font-size: 20px;
                                    padding-left: 40px;" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="row" id="lblSupervisingProvName" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: justify;">
                                <asp:Label ID="lblSupervisingLastName" runat="server" Style="font-size: 20px; padding-left: 50px;" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="row" id="lblSupervisingProvFirstName" runat="server">
                        <div class="col-sm-12">
                            <span style="text-align: justify;">
                                <asp:Label ID="lblSupervisingFirstName" runat="server" Style="font-size: 20px; padding-left: 70px;" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
           <div id="divSupervisingErrorMessage" class="row" runat="server">
            <div class="col-sm-12">
                <asp:Label ID="lblSupervisingError" runat="server" ForeColor="Red" class="col-sm-12"></asp:Label>
            </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Panel>
