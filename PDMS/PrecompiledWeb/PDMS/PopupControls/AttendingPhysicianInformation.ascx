<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_AttendingPhysicianInformation, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">
    function loaderAttendingPhysician(e) {
        getTextboxdetail(e);
        $("#<%= errAttendingPhysicianNPI.ClientID %>").text('');
        var AttendingNpi = $("#<%= txtAttendingPhysicianNPI.ClientID %>").val();
        var AttendingnpiLenth = AttendingNpi.length;
        if (AttendingnpiLenth < 10) {

            AttendingClerFields();
            $("#<%= errAttendingPhysicianNPI.ClientID %>").text("NPI 10-digit number is required");
            localStorage.setItem("AttendingProviderNPI", "");
            return false;
        }
        localStorage.setItem("AttendingProviderNPI", "" + AttendingNpi + "");

        if (AttendingnpiLenth == 10 && AttendingnpiLenth !== null && AttendingnpiLenth !== undefined) {
            var txtrendNpiCode = $("#<%= txtAttendingPhysicianNPI.ClientID %>").first().val();
            var txtrendMedId = $("#<%= hdnMedicaidAttendingPhysician.ClientID %>").first().val(); //  lblAttendingPhysicianMedicaidID.ClientID %>").text();
            var hdnAttendingProviderClaimType = $("#<%= hdnAttendingPhysicianClaimType.ClientID %>").first().val();
            $("#<%= errAttendingPhysicianNPI.ClientID %>").html("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetnpiCount?RefNpi=" + txtrendNpiCode + "&&RefMedId=" + txtrendMedId + "&&ClaimType=" + hdnAttendingProviderClaimType+"",
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
                        $("#<%= errAttendingPhysicianNPI.ClientID %>").text("NPI is not found in the system");
                        AttendingClerFields();
                        return false;
                    }
                    else {  
                        var result1 = result;
                        
                        if (result == "NPI is not found in the system") {
                            $("#<%= errAttendingPhysicianNPI.ClientID %>").text("NPI is not found in the system");
                            AttendingClerFields();
                            return false;
                        }
                        else if (result == "10-digit number is required") {

                            $("#<%= errAttendingPhysicianNPI.ClientID %>").text("10 - digit number is required");
                            AttendingClerFields();
                            return false;
                        }
                        else if (result == "Non-individual provider cannot be entered as referring provider") {
                            $("#<%= errAttendingPhysicianNPI.ClientID %>").text("Non-individual provider cannot be entered as referring provider");
                            AttendingClerFields();
                            return false;
                        }

                        else if (result !== null && result !== undefined && result !== "NPI is not found in the system" && result !== "10 - digit number is required" &&
                            result !== "Non-individual provider cannot be entered as referring provider") {
                            // $("#<%= lblAttendingPhysicianMedicaidID.ClientID %>").html(result1[0].MEDICAID_ID);
                            $("#<%= lblAttendingPhysicianFirstName.ClientID %>").html(result1[0].FIRST_NAME);
                            $("#<%= lblAttendingPhysicianLastName.ClientID %>").html(result1[0].LAST_OR_BUSINESS_NAME);
                            $("#<%= hdnMedicaidAttendingPhysician.ClientID %>").val(result1[0].MEDICAID_ID);
                            $("#<%= hdnFirstNameAttendingPhysician.ClientID %>").val(result1[0].FIRST_NAME);
                            $("#<%= hdnLastNameAttendingPhysician.ClientID %>").val(result1[0].LAST_OR_BUSINESS_NAME);
                            
                            return false;
                        }
                    }
                },
            });
        }
        else if (AttendingnpiLenth == 0) {
            $("#<%= lblAttendingPhysicianMedicaidID.ClientID %>").html('');
            $("#<%= lblAttendingPhysicianFirstName.ClientID %>").html('');
            $("#<%= lblAttendingPhysicianLastName.ClientID %>").html('');

            $("#<%= lblAttendingPhysicianMedicaidID.ClientID %>").text('');
            $("#<%= lblAttendingPhysicianFirstName.ClientID %>").text('');
            $("#<%= lblAttendingPhysicianLastName.ClientID %>").text('');

            $("#<%= hdnMedicaidAttendingPhysician.ClientID %>").val('');
            $("#<%= hdnFirstNameAttendingPhysician.ClientID %>").val('');
            $("#<%= hdnLastNameAttendingPhysician.ClientID %>").val('');
        }
        return false;
    }
    function AttendingClerFields() {      
        $("#<%= txtAttendingPhysicianNPI.ClientID %>").val("");
        $("#<%= lblAttendingPhysicianMedicaidID.ClientID %>").text('');       
        $("#<%= lblAttendingPhysicianLastName.ClientID %>").text('');        
        $("#<%= lblAttendingPhysicianFirstName.ClientID %>").text('');
        $("#<%= hdnMedicaidAttendingPhysician.ClientID %>").val('');
        $("#<%= hdnFirstNameAttendingPhysician.ClientID %>").val('');
        $("#<%= hdnLastNameAttendingPhysician.ClientID %>").val('');
    }
</script>
<asp:UpdatePanel ID="upnlAttendingPhysicianInfo" runat="server">
    <ContentTemplate>
        <div class="row" style="background-color: #ADD8E6;">
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 140px;"><span style="color: red">*</span>NPI</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 70px;">Medicaid ID</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 50px;">Last Name</span>
            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 10px;">First Name</span>
            </div>
        </div>
        <div class="row" style="padding-left: 100px;">
            <div class="col-sm-3">
                <div class="row" id="lblAttendingPhysicianNPI" runat="server">
                    <div class="col-sm-12" style="width:150%;">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtAttendingPhysicianNPI" OnChange="return loaderAttendingPhysician(this)" runat="server" CssClass="formFieldTextBox" 
                                 MaxLength="10" Style="height: 30px; width: 180px;"></asp:TextBox>
                            <asp:Label ID="searchAt" runat="server">
                                 <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
                            <button type="button" id="btnAttendingPhysician" onclick="getbuttondetail(this)"
                                    class="btn btn-link" data-toggle="modal" data-target="#myModal">
                                    Search
                            </button>
                            </asp:Label><br />
                            <asp:Label ID="errAttendingPhysicianNPI" Style="color: red;" runat="server"></asp:Label>
                            <%--<asp:RequiredFieldValidator ID="rfvAttendingPhysicianNPI" runat="server" ControlToValidate="txtAttendingPhysicianNPI"
                                ErrorMessage="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI' class='errorlist'>*Attending Physician Information 10-digit number is required</a></div>"
                                Display="None" ValidationGroup="validateClaims1" CssClass="failureNotification"></asp:RequiredFieldValidator--%>
                        </span>
                        
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="AttendingPhysicianMedicaidID" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: left;">
                            <asp:Label ID="lblAttendingPhysicianMedicaidID" runat="server" MaxLength="10" Style="height: 30px; width: 200px; padding-left: 4rem;"
                                Text="" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="AttendingPhysicianLastName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: center;">
                            <asp:Label ID="lblAttendingPhysicianLastName" runat="server" Style="height: 30px; width: 200px; padding-left: 7rem;"
                                Text="" />
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="row" id="AttendingPhysicianFirstName" runat="server">
                    <div class="col-sm-12">
                        <span style="text-align: right;">
                            <asp:Label ID="lblAttendingPhysicianFirstName" runat="server" Style="height: 30px; width: 200px; padding-left: 8rem;"
                                Text="" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnAttendingPhysicianClaimID" runat="server" />
        <asp:HiddenField ID="hdnAttendingPhysicianClaimType" runat="server" />
        <asp:HiddenField ID="hdnErrorMessageAttending" runat="server" />
         <asp:HiddenField ID="hdnNpiAttendingPhysician" runat="server" />
         <asp:HiddenField ID="hdnMedicaidAttendingPhysician" runat="server" />
         <asp:HiddenField ID="hdnLastNameAttendingPhysician" runat="server" />
         <asp:HiddenField ID="hdnFirstNameAttendingPhysician" runat="server" />
         <div id="divAttendingPhysicianErrorMessage" class="row" runat="server">
    <div class="col-sm-12">
    <asp:Label ID="lblAttendingPhysicianError" runat="server" ForeColor="Red" class="col-sm-12"></asp:Label>
</div>
</div>
    </ContentTemplate>
</asp:UpdatePanel>
