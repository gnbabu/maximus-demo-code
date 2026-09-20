<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_RecipientInformationPanel, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">
    function OnchangeofMedicaidBillingNo() {
        ClearRecipientUIData();
        $("[id*=lblErrorMsgDetils]").text('');
        var MedicaidBillingNumber = $("#<%= txtMedicaidBillingNumber.ClientID %>").first().val();
        var DateOfBirth = $("#<%= txtBirthDate.ClientID %>").first().val();
        if ((DateOfBirth == null || DateOfBirth == undefined || DateOfBirth == "") && (MedicaidBillingNumber == null || MedicaidBillingNumber == "" || MedicaidBillingNumber == undefined)) {
            ClearRecipientInfoData();
            return false;
        }
        if (MedicaidBillingNumber.length != 12) {
            $("[id*=lblErrorMsgDetils]").text('Medicaid Billing Number 12-digit number is required')
            return false;
        }
       
    }
    function txtCheckForPerson_TextChanged() {        
        $("[id*=lblErrorMsgDetils]").text('');
        ClearRecipientUIData();
        var RecipientInfoData = "";
        var DateOfBirth = $("#<%= txtBirthDate.ClientID %>").first().val();
        var MedicaidBillingNumber = $("#<%= txtMedicaidBillingNumber.ClientID %>").first().val();
        if ((DateOfBirth == null || DateOfBirth == undefined || DateOfBirth == "") && (MedicaidBillingNumber == null || MedicaidBillingNumber == "" || MedicaidBillingNumber == undefined)) {
            ClearRecipientInfoData();
            return false;
        }
        if ((DateOfBirth != null || DateOfBirth != undefined || DateOfBirth != "") && (MedicaidBillingNumber != null || MedicaidBillingNumber != "" || MedicaidBillingNumber != undefined)) {
            var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
            if (!(date_regex.test(DateOfBirth))) {
                $("[id*=lblErrorMsgDetils]").text('Date of birth should be in MM/dd/yyyy format')
                return false;
            }

            var Currentdate = new Date();
            var dateVal = DateOfBirth.split("/");
            var year = Currentdate.getFullYear();
            var month = Currentdate.getMonth();
            var day = Currentdate.getDay();
            if ( (year < dateVal[2]) ||( (year == dateVal[2]) && (month < dateVal[0]) ) || ( (year == dateVal[2]) && (month == dateVal[0]) && (day < dateVal[1]) ) ) {
                $("[id*=lblErrorMsgDetils]").text('DOB Does not allow future date')
                return false;//future date
            }

            if (dateVal[2] < 1900) {
                $("[id*=lblErrorMsgDetils]").text('DOB Must be beyond 1900')
                return false;//future date
            }

            if (MedicaidBillingNumber.length != 12) {
                $("[id*=lblErrorMsgDetils]").text('Medicaid Billing Number 12-digit number is required')
                return false;
            }
            var APIToken = $("[id*=hdnAccessToken]").val();
            var medicaidID = $("[id*=hdnMedicaidID]").val();
            var username = $("[id*=hdnUserName]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "txtCheckForPerson_TextChanged?MedicaidID=" + medicaidID + "&&Username=" + username + "&&DateOfBirth=" + DateOfBirth + "&&MedicaidBillingNumber=" + MedicaidBillingNumber + "",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    RecipientInfoData = result;
                    if (RecipientInfoData !== null && RecipientInfoData.FirstName != "") {
                        $("[id*=ucRecipientInformationPanel_lblLastName]").text(result.LastName);
                        $("[id*=ucRecipientInformationPanel_lblLastName]").val(result.LastName);

                        $("[id*=ucRecipientInformationPanel_lblfrstmi]").text(result.FirstName);
                        $("[id*=ucRecipientInformationPanel_hdnFirstName]").val(result.FirstName);

                        $("[id*=ucRecipientInformationPanel_lblMiddleName]").text(result.MiddleName);
                        $("[id*=ucRecipientInformationPanel_hdnMiddleName]").val(result.MiddleName);

                        $("[id*=ucRecipientInformationPanel_lblGender]").text(result.Gender);
                        $("[id*=ucRecipientInformationPanel_hdnGender]").val(result.Gender);

                        $("[id*=ucRecipientInformationPanel_lblAddress]").text(result.AddressLine1);
                        $("[id*=ucRecipientInformationPanel_hdnAddressLine1]").val(result.AddressLine1);

                        $("[id*=ucRecipientInformationPanel_lblAddressLine2]").text(result.AddressLine2);
                        $("[id*=ucRecipientInformationPanel_hdnAddressLine2]").val(result.AddressLine2);

                        $("[id*=ucRecipientInformationPanel_lblssn]").text(result.SSN);
                        $("[id*=ucRecipientInformationPanel_hdnSSN]").val(result.SSN);

                        $("[id*=ucRecipientInformationPanel_lblCity]").text(result.City);
                        $("[id*=ucRecipientInformationPanel_hdnCity]").val(result.City);

                        $("[id*=ucRecipientInformationPanel_lblZipcode]").text(result.ZipCode5);
                        $("[id*=ucRecipientInformationPanel_hdnZipCode]").val(result.ZipCode5);

                        $("[id*=ucRecipientInformationPanel_lblState]").text(result.StateCode);
                        $("[id*=ucRecipientInformationPanel_hdnState]").val(result.StateCode);
                        document.getElementById('<%= lblErrorMsgDetils.ClientID%>').innerHTML = "";
                        if (RecipientInfoData.Errors != null && RecipientInfoData.Errors.length > 0) {
                            var errorMessage = '';
                            for (var i = 0; i < RecipientInfoData.Errors.length; i++) {
                                if (errorMessage.length > 0)
                                    errorMessage = errorMessage + "; " + RecipientInfoData.Errors[i].Code + " : " + RecipientInfoData.Errors[i].Description;
                                else
                                    errorMessage = "Error " + RecipientInfoData.Errors[i].Code + " : " + RecipientInfoData.Errors[i].Description;
                            }
                            document.getElementById('<%= lblErrorMsgDetils.ClientID%>').innerHTML = errorMessage;
                            ClearRecipientInfoData();
                        }
                    }
                    else if (RecipientInfoData.FirstName == "" && RecipientInfoData.Errors != null && RecipientInfoData.Errors.length > 0) {
                        var errorMessage = '';
                        for (var i = 0; i < RecipientInfoData.Errors.length; i++) {
                            if (errorMessage.length > 0)
                                errorMessage = errorMessage + "; " + RecipientInfoData.Errors[i].Code + " : " + RecipientInfoData.Errors[i].Description;
                            else
                                errorMessage = "Error: " + RecipientInfoData.Errors[i].Code + " : " + RecipientInfoData.Errors[i].Description;
                        }
                        document.getElementById('<%= lblErrorMsgDetils.ClientID%>').innerHTML = errorMessage;
                        ClearRecipientInfoData();
                    }
                    else {
                        document.getElementById('<%= lblErrorMsgDetils.ClientID%>').innerHTML = 'Error: An error occurred while processing the request';
                        ClearRecipientInfoData();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMsgDetils]").text('Error: An error occurred while processing the request');
                    ClearRecipientInfoData();
                }
            });
        }
    }
    function ClearRecipientUIData() {
        $("[id*=lblLastName]").html('');
        $("[id*=lblfrstmi]").html('');
        $("[id*=lblMiddleName]").html('');
        $("[id*=lblGender]").html('');
        $("[id*=lblAddress]").html('');
        $("[id*=lblAddressLine2]").html('');
        $("[id*=lblssn]").html('');
        $("[id*=lblCity]").html('');
        $("[id*=lblZipcode]").html('');
        $("[id*=lblState]").html('');
    }
    function ClearRecipientInfoData() {
        $("[id*=lblLastName]").html('');
        $("[id*=lblfrstmi]").html('');
        $("[id*=lblMiddleName]").html('');
        $("[id*=lblGender]").html('');
        $("[id*=lblAddress]").html('');
        $("[id*=lblAddressLine2]").html('');
        $("[id*=lblssn]").html('');
        $("[id*=lblCity]").html('');
        $("[id*=lblZipcode]").html('');
        $("[id*=lblState]").html('');
        $("#<%= txtMedicaidBillingNumber.ClientID %>").val('');
        $("#<%= txtBirthDate.ClientID %>").val('');       
    }
    function patientctrlno() {
       
         var patientControlNumber = document.getElementById('<%= txtPatientControlNumber.ClientID %>').value;
            if ((patientControlNumber != "") && (patientControlNumber != null)) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_patientControlNumberError').text('');

            }
        else { $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_patientControlNumberError').text('*Patient Control Number is required'); }
        return false;
}
  


    function clearfields() {
        var medBillingNum = $("#<%= txtMedicaidBillingNumber.ClientID %>").val();
        var length = medBillingNum.length;
        if (length < 12) {
            $("#<%= txtMedicaidBillingNumber.ClientID %>").val("");
            $("#<%= txtBirthDate.ClientID %>").val("");
            $("#<%= txtPatientControlNumber.ClientID %>").val("");
            $("#<%= txtMedRecNumber.ClientID %>").val("");
            $("#<%= lblLastName.ClientID %>").text('');
            $("#<%= lblfrstmi.ClientID %>").text('');
            $("#<%= lblGender.ClientID %>").text('');
            $("#<%= lblAddress.ClientID %>").text('');
            $("#<%= lblMiddleName.ClientID %>").text('');
            $("#<%= lblAddressLine2.ClientID %>").text('');
            $("#<%= lblssn.ClientID %>").text('');
            $("#<%= lblCity.ClientID %>").text('');
            $("#<%= lblZipcode.ClientID %>").text('');
            $("#<%= lblState.ClientID %>").text('');
            $("#<%= dropdownPregnancyIndicator.ClientID %>").prop('selectedIndex', 0);          
            $("#<%= medicaIdNumbers1.ClientID %>").show();
        }
        else {
            $("#<%= medicaIdNumbers1.ClientID %>").hide();
        }
        return false;
    }
    function loaderRecipient1() {
       
        if ($("#<%= hdnClaimType_Recipient.ClientID %>").first().val() == "1")
        {
        document.getElementById('<%= txtMedicaidBillingNumber.ClientID %>').disabled = true; 
        document.getElementById('<%= txtBirthDate.ClientID %>').disabled = true;
        document.getElementById('<%= txtPatientControlNumber.ClientID %>').disabled = true;
        document.getElementById('<%= txtMedRecNumber.ClientID %>').disabled = true;
       <%-- document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';--%>
        }
       else if ($("#<%= hdnClaimType_Recipient.ClientID %>").first().val() == "0") {
            document.getElementById('<%= txtMedicaidBillingNumber.ClientID %>').disabled = true;
            document.getElementById('<%= txtBirthDate.ClientID %>').disabled = true;
        document.getElementById('<%= txtPatientControlNumber.ClientID %>').disabled = true;
           <%-- document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';--%>
        }
        else if ($("#<%= hdnClaimType_Recipient.ClientID %>").first().val() == "2") {
            document.getElementById('<%= txtMedicaidBillingNumber.ClientID %>').disabled = true;
            document.getElementById('<%= txtBirthDate.ClientID %>').disabled = true;
            document.getElementById('<%= txtPatientControlNumber.ClientID %>').disabled = true;
            document.getElementById('<%= txtMedRecNumber.ClientID %>').disabled = true;
            document.getElementById('<%= dropdownPregnancyIndicator.ClientID %>').disabled = true;
           <%-- document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';--%>

        }
        
    }
    function loaderRecipient2() {
        if ($("#<%= hdnClaimType_Recipient.ClientID %>").first().val() == "1") {
            document.getElementById('<%= txtMedicaidBillingNumber.ClientID %>').disabled = true;
            document.getElementById('<%= txtBirthDate.ClientID %>').disabled = true;
            document.getElementById('<%= txtPatientControlNumber.ClientID %>').disabled = true;
            document.getElementById('<%= txtMedRecNumber.ClientID %>').disabled = true;
           <%-- document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>
        }
        else if ($("#<%= hdnClaimType_Recipient.ClientID %>").first().val() == "0") {
            document.getElementById('<%= txtMedicaidBillingNumber.ClientID %>').disabled = true;
            document.getElementById('<%= txtBirthDate.ClientID %>').disabled = true;
            document.getElementById('<%= txtPatientControlNumber.ClientID %>').disabled = true;
           
           <%-- document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>
        }
      
        else if ($("#<%= hdnClaimType_Recipient.ClientID %>").first().val() == "2") {
    document.getElementById('<%= txtMedicaidBillingNumber.ClientID %>').disabled = true;
    document.getElementById('<%= txtBirthDate.ClientID %>').disabled = true;
    document.getElementById('<%= txtPatientControlNumber.ClientID %>').disabled = true;
    document.getElementById('<%= txtMedRecNumber.ClientID %>').disabled = true;
    document.getElementById('<%= dropdownPregnancyIndicator.ClientID %>').disabled = true;
    <%--document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>
        }

    }

function alphanumericOnly(obj) {
obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9- ]/g, '');
    }

    function validateDate() {
        var daterequested = document.getElementById("<%= txtBirthDate.ClientID%>").value;
    
        var tdate = new Date();
        var dd = tdate.getDate(); 
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;


        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
           
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_birthDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_birthDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_birthDateRequiredError1').css('display', 'none');
        }
        

        if ($('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_birthDateRequiredError1').is(':visible')) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblGender').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblAddress').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblLastName').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblfrstmi').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblMiddleName').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_divlblAddress2').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblCity').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblState').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblZipcode').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtBirthDate').val('');

        }
        

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_birthDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_birthDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblGender').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblAddress').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblLastName').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblfrstmi').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblMiddleName').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_divlblAddress2').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblCity').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblState').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblZipcode').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtBirthDate').val('');
        }
        
    }
   
</script>

<div>

    <asp:Panel ID="pnlRecipient2" runat="server" Style="min-height: 260px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
        <asp:UpdatePanel ID="upnRecipient" runat="server">
            <ContentTemplate>
                <%-- <asp:ValidationSummary ID="valerrormess" DisplayMode="List" runat="server" CssClass="failureNotification"
             ValidationGroup="validateClaims11" />--%>
                <asp:HiddenField ID="hdnClaimIdRecipient" runat="server" />
                <asp:HiddenField ID="hdnClaimType_Recipient" runat="server" />

                <asp:HiddenField ID="hdnFirstName" runat="server" />
                <asp:HiddenField ID="hdnLastName" runat="server" />
                <asp:HiddenField ID="hdnMiddleName" runat="server" />
                <asp:HiddenField ID="hdnGender" runat="server" />
                <asp:HiddenField ID="hdnAddressLine1" runat="server" />
                <asp:HiddenField ID="hdnAddressLine2" runat="server" />
                <asp:HiddenField ID="hdnCity" runat="server" />
                <asp:HiddenField ID="hdnState" runat="server" />
                <asp:HiddenField ID="hdnZipCode" runat="server" />
                 <asp:HiddenField ID="hdnSSN" runat="server" />
                
                 <asp:HiddenField ID="hdnMedicaidID" runat="server" />
                 <asp:HiddenField ID="hdnUserName" runat="server" />

                <div>
                    <asp:Label ID="lblErrorMsgDetils" runat="server" ForeColor="Red" Style="margin-left: 20px;"></asp:Label><br />
                    <asp:label runat="server" ID="lblDOBErrorMessage"  ForeColor="Red" Style="margin-left: 20px;"></asp:label>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" id="lblMedicaidBillingNumber" runat="server">
                            <div class="col-sm-6">
                                <span class="ohio-field" style="font-size: 16px; text-align: right"><span style="color: red">* </span>Medicaid Billing Number</span>
                            </div>
                            <div class="col-sm-6">
                                <span style="text-align: left;">

                                    <asp:TextBox ID="txtMedicaidBillingNumber" runat="server" CssClass="formFieldTextBox" onKeyUp="javascript:numericOnly(this);" MaxLength="12" Style="height: 33px; width: 300px" onChange="return OnchangeofMedicaidBillingNo(this)"   />
                                     <%--<button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
                     
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtMedicaidBillingNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                    <asp:Label  ID="medicaidrequiredError" runat="server" Text="" CssClass="error-message"></asp:Label>
                                   <br />
                                    <asp:Label Visible="false" ID="medicaIdNumbers" runat="server" Text="*Medicaid Billing Number 12-digit number is required" CssClass="error-message"></asp:Label>
                                    <asp:Label style="display:none;" ID="medicaIdNumbers1" runat="server" Text="*Medicaid Billing Number 12-digit number is required" CssClass="error-message"></asp:Label>
                                   

                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblBirthDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 16px; text-align: right"><span style="color: red">* </span>Date of Birth</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">

                                    <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formFieldcalender" Style="height: 33px; width: 180px" onChange="return txtCheckForPerson_TextChanged(this)"/>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon"  />
                                    <%-- <button id="btnloading2" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
                     
                                    <ajax:CalendarExtender ID="ceBirthdate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtBirthDate" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" PopupButtonID="imgBirthDate" EnabledOnClient="true" />
                                    <asp:Label ID="birthDateRequiredError" runat="server" Text="*Missing Recipient date of birth" CssClass="error-message" Visible="false"></asp:Label>
                                    <asp:Label ID="birthDateRequiredError1" runat="server" Text="" CssClass="error-message" style="display:none;"></asp:Label>
                                    <br />
                                  
                                   <%-- <asp:RequiredFieldValidator ID="rfvBirthdate" runat="server" ControlToValidate="txtBirthDate" ErrorMessage="<div id='msg1_rev_txtBirthDate'><a href='#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtBirthDate' class='errorlist'>*Missing Recipient date of birth</div>" Text="*" Display="Dynamic" ValidationGroup="validateClaims" />
                                    <asp:CompareValidator ID="cvtBirthDate" runat="server" Type="Date" Operator="LessThanEqual"
                                        ControlToValidate="txtBirthDate" ValidationGroup="validateClaims" ForeColor="Red" ErrorMessage="Future date not allowed"
                                        Display="Dynamic" SetFocusOnError="true" />--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4"  runat="server">
                        <div  runat="server">
                            <div class="col-sm-5 ">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Gender:</span>
                            </div>
                            <div class="col-sm-7 ">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblGender" runat="server" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row"  runat="server">
                            <div class="col-sm-6">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Last Name:</span>
                            </div>
                            <div class="col-sm-6">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblLastName" runat="server" MaxLength="60" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblPatientControlNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 16px; text-align: right"><span style="color: red">* </span>Patient Control Number</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <%--OnChange="validateText()"--%>
                                    <asp:TextBox ID="txtPatientControlNumber"  runat="server" onKeyUp="javascript:alphanumericOnly(this);" CssClass="formFieldTextBox" MaxLength="38" Style="height: 33px; width: 200px" />
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPatientControlNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                    
                                    <asp:Label  ID="patientControlNumberError" runat="server" Text="" CssClass="error-message"></asp:Label>
                                    

                              
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row"  runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Address Line 1:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                   <asp:Label ID="lblAddress" runat="server" Width="300px"  />
                                    <asp:Label Visible="false" ID="lblssn" runat="server" />
                                </span>
                            </div>
                        </div>
                        <div class="row"  runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Address Line 2: </span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblAddressLine2" runat="server" Width="300px" />
                                </span>
                            </div> 
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row"  runat="server">
                            <div class="col-sm-6 ">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">First Name:</span>
                            </div>
                            <div class="col-sm-6 ">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblfrstmi" runat="server"/>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-4" id="divMedRecNumber" runat="server" visible="false">
                        <div id="lblMedRecNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Medical Record Number:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtMedRecNumber" runat="server" CssClass="formFieldTextBox" Style="height: 33px; width: 200px" MaxLength="50" />
                                </span>
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMedRecNumber" ValidationExpression="^[A-Za-z0-9?\.\d,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
									
                            </div>
                            <asp:RegularExpressionValidator ID="regMedRecNumber" runat="server"
                                ControlToValidate="txtMedRecNumber" Display="None"
                                ErrorMessage="*Medical Record Number should be Alpha-numeric"
                                ValidationExpression="^[A-Za-z0-9_]+$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="col-sm-4" id="divMedicalRecordHideEmpty" runat="server" visible="false"></div>
                    <%--<div class="col-sm-4" runat="server" id="divAddressLine2">--%>
                        <div class="col-sm-4">
                            <div class="row"  runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">City:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblCity" runat="server"/>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" runat="server">
                            <div class="col-sm-6">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Middle Name:</span>
                            </div>
                            <div class="col-sm-6">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblMiddleName" runat="server" MaxLength="25" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4" id="divEmptyforDental" runat="server" visible="false" />

                    <div class="col-sm-4" id="divPregnancyIndicatorEmptyhide" runat="server" visible="false">
                        <div class="row" id="divpregnancyIndicatordrpdwn" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Pregnancy Indicator</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                  
                                    <asp:DropDownList ID="dropdownPregnancyIndicator" runat="server" OnChange="return patientctrlno()" 
                                        AppendDataBoundItems="True" Style="min-width: 180px; height: 33px">
                                        <asp:ListItem Value="N" Text=" "></asp:ListItem>
                                        <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" runat="server">
                            <div class="col-sm-5 ">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">State:</span>
                            </div>
                            <div class="col-sm-7 ">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblState" runat="server" />
                                </span>
                            </div>
                        </div>
                        <div class="row"  runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Zip Code:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:Label ID="lblZipcode" runat="server" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>