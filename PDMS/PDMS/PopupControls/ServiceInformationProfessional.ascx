<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ServiceInformationProfessional" Codebehind="ServiceInformationProfessional.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

   

<script type="text/javascript">
    function ddlProfessionalDentalReleaseofInfochange() {
        var ddlProfReleaseofInfo = $("#<%=ddlProfessionalDentalReleaseofInfo.ClientID %> option:selected").text();

        if ((ddlProfReleaseofInfo != "") && (ddlProfReleaseofInfo != null)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_releaseInformationError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlProfessionalDentalReleaseofInfo').css("background-color", "white");
        }
    }

    function dropdownFirst_Selected_IndexChanged() {
        var strEPSDTFullOptions = '<option value=""></option><option value="5">S2</option><option value="6">ST</option><option value="7">NU</option><option value="8">AV</option>'
        var firstSelection = document.getElementById("<%=ddlESPSDTCODEPRofessionalFirst.ClientID %>").value;
        document.getElementById('<%= hdnEPSDTCodeProfessionalFirst.ClientID %>').value = firstSelection;
        document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').innerHTML = strEPSDTFullOptions;
        for (i = 0; i < document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').options.length; i++) {
            if (document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').options[i].value == firstSelection) {
                document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').remove(i);
                break;
            }
        }
    }

    function dropdownSecond_Selected_IndexChanged() {
        var strEPSDTFullOptions = '<option value=""></option><option value="5">S2</option><option value="6">ST</option><option value="7">NU</option><option value="8">AV</option>'
        var firstSelection = document.getElementById("<%=ddlESPSDTCODEPRofessionalFirst.ClientID %>").value;
        var secondSelection = document.getElementById("<%=ddlESPSDTCODEPRofessionalSecond.ClientID %>").value;
        document.getElementById('<%= hdnEPSDTCodeProfessionalSecond.ClientID %>').value = secondSelection;
        document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').innerHTML = strEPSDTFullOptions;
        for (i = document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').options.length - 1; i > 0; i--) {
            if ( (document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').options[i].value == firstSelection) || (document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').options[i].value == secondSelection) ) {
                document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').remove(i);
            }
        }
    }

    function dropdownThird_Selected_IndexChanged() {
        var thirdSelection = document.getElementById("<%=ddlESPSDTCODEPRofessionalThird.ClientID %>").value;
        document.getElementById('<%= hdnEPSDTCodeProfessionalThird.ClientID %>').value = thirdSelection;
    }

   function DropDwonEnableCheck() {
       var ddlProfessionalEPSDTCondition = document.getElementById("<%=ddlProfessionalEPSDTCondition.ClientID %>").value;
       if (ddlProfessionalEPSDTCondition != "") {
           document.getElementById('<%= ddlESPSDTCODEPRofessionalFirst.ClientID %>').disabled = false;
            document.getElementById('<%= ddlESPSDTCODEPRofessionalFirst.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').disabled = false
            document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').style.backgroundColor = "#FFFFFF";
            document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').disabled = false;
            document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').style.backgroundColor = "#FFFFFF";
       }
       else {
           if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlClaimFilingIndicator") != null) {
               $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlClaimFilingIndicator').val('');
           }
           if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlESPSDTCODEPRofessionalFirst") != null) {
               $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlESPSDTCODEPRofessionalFirst').val('');
           }
           if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlESPSDTCODEPRofessionalSecond") != null) {
               $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlESPSDTCODEPRofessionalSecond').val('');
           }
           if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlESPSDTCODEPRofessionalThird") != null) {
               $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlESPSDTCODEPRofessionalThird').val('');
           }
           document.getElementById('<%= ddlESPSDTCODEPRofessionalFirst.ClientID %>').style.backgroundColor = "#D3D3D3";
           document.getElementById('<%= ddlESPSDTCODEPRofessionalFirst.ClientID %>').disabled = true;
           document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').style.backgroundColor = "#D3D3D3";
           document.getElementById('<%= ddlESPSDTCODEPRofessionalSecond.ClientID %>').disabled = true;
           document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').style.backgroundColor = "#D3D3D3";
           document.getElementById('<%= ddlESPSDTCODEPRofessionalThird.ClientID %>').disabled = true;
       }

   }
   
    function onlyDotsAndNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            else
                return false;;
        }
        if (txt.value.indexOf(".") > 0) {
            var txtlen = txt.value.length;
            var dotpos = txt.value.indexOf(".");
            if ((txtlen - dotpos) > 2)
                return false;
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        else
            return true;
    }

    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    function IsNumeric(e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        document.getElementById("error").style.display = ret ? "none" : "inline";
        return ret;
    }
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
    function validateDate20() {
        var daterequested = document.getElementById("<%= txtProfessionalHospitalDischargeDate.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProffHospDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProffHospDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_txtProfessionalHospitalDischargeDate').val('');

        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProffHospDateRequiredError1').css('display', 'none');
             }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProffHospDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProffHospDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_txtProfessionalHospitalDischargeDate').val('');
        }
    }

    function validateDate21() {
        var daterequested = document.getElementById("<%= txtProfessionalLstmenstural.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProfessionalLstmensturalDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProfessionalLstmensturalDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_txtProfessionalLstmenstural').val('');

                }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProfessionalLstmensturalDateRequiredError1').css('display', 'none');
        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProfessionalLstmensturalDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ProfessionalLstmensturalDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_txtProfessionalLstmenstural').val('');
        }
    }

</script>
<asp:UpdatePanel ID="upServiceDetailsS" runat="server">
    <ContentTemplate>
        <div> <label id="txtErrormsg4" runat="server"></label></div>
<div class="row">
     <asp:HiddenField ID="hdnClaimId" runat="server" />
     <asp:HiddenField ID="hdnClaimType" runat="server" />
    <div class="col-sm-4">
        <div class="row">
            <div class="col-sm-7">
        <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>Release of Information</span>
    </div>
    <div class="col-sm-5">
        <span style="text-align: left;">
            <asp:DropDownList onchange="ddlProfessionalDentalReleaseofInfochange()" ID="ddlProfessionalDentalReleaseofInfo" EnableViewState="true" runat="server"
                AppendDataBoundItems="True" Style= "width: 150px; min-width: 150px;  font-size: 14px; height: 30px" class="selectdropdown"> 
                <asp:ListItem Value="0" Text="" />
                <asp:ListItem Value="N" Text="No" />
                <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
            </asp:DropDownList><br />
            <asp:Label ID="releaseInformationError" runat="server" Text="" CssClass="error-message"></asp:Label>
                  
           <%-- <asp:RequiredFieldValidator runat="server" ID="rfvddlProfessionalReleaseOfINformation" SetFocusOnError="true" ForeColor="Red"
                ValidationGroup="validateClaims" ControlToValidate="ddlProfessionalDentalReleaseofInfo" 
                ErrorMessage="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlProfessionalDentalReleaseofInfo' class='errorlist'>*Release of Information is required</a></div>"  Display="None" InitialValue="" />--%>
        </span>
    </div>
        </div>
        <div class="row">
            <div class="col-sm-7">
        <span class="ohio-field" style="font-size: 15px; text-align: right">Place Of Service</span>
    </div>
    <div class="col-sm-5">        
                        <asp:Label ID="lblPlcServiceProfessional"   runat="server"></asp:Label>     
    </div>
        </div>
        <div class="row">
            <div class="col-sm-7">
        <span class="ohio-field" style="font-size: 15px; text-align: right">Special Program Indicator</span>
    </div>
    <div class="col-sm-5">
        <span style="text-align: left;">
            <asp:DropDownList ID="ddlSpcPrgmIndProfessional" runat="server" Style="height: 30px; font-size: 14px; width: 150px; min-width: 150px;">
               
            </asp:DropDownList>
        </span>
    </div>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="row">
            <div class="col-sm-6">
        <span class="ohio-field" style="font-size: 15px; text-align: right">EPSDT Condition Indicator</span>
    </div>
    <div class="col-sm-6">
        <span style="text-align: left;">
            <asp:DropDownList ID="ddlProfessionalEPSDTCondition" EnableViewState="true" runat="server" 
                AppendDataBoundItems="True" Style="height: 30px; font-size: 14px; width: 150px; min-width: 150px;" OnChange="DropDwonEnableCheck()">
                 <asp:ListItem Value="" Text="" />
                <asp:ListItem Value="N" Text="No" />
                <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
            </asp:DropDownList>             
                     
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" SetFocusOnError="true" ForeColor="Red"
                ValidationGroup="validateServiceInformationProfessional" ControlToValidate="ddlProfessionalEPSDTCondition" ErrorMessage="*EPSDTCondition" Text="*" Display="Dynamic" InitialValue="0" />
        </span>
    </div>
            </div>
        <div class="row">
            <div class="col-sm-6">
        <span class="ohio-field" style="font-size: 15px; text-align: right">EPSDT Condition Code</span>
    </div>
    <div class="col-sm-6">
        <span style="text-align: left;">
            <asp:DropDownList ID="ddlESPSDTCODEPRofessionalFirst" runat="server" Style="height: 30px; font-size: 14px; width: 55px; min-width: 50px;" OnChange="dropdownFirst_Selected_IndexChanged(); return false;" AutoPostBack="false" />
            <asp:DropDownList ID="ddlESPSDTCODEPRofessionalSecond" runat="server" Style="height: 30px; font-size: 14px; width: 55px; min-width: 50px;" OnChange="dropdownSecond_Selected_IndexChanged(); return false;" AutoPostBack="false" />
            <asp:DropDownList ID="ddlESPSDTCODEPRofessionalThird" runat="server" Style="height: 30px; font-size: 14px; width: 55px; min-width: 50px;" OnChange="dropdownThird_Selected_IndexChanged(); return false;" AutoPostBack="false" />
            <asp:Label ID="ddlESPSDTCODEPRofessionalFirstErrorMessage" runat="server" Text="" CssClass="error-message" style="display:none;"></asp:Label>
        </span>
        <asp:HiddenField ID="hdnEPSDTCodeProfessionalFirst" runat="server" />
        <asp:HiddenField ID="hdnEPSDTCodeProfessionalSecond" runat="server" />
        <asp:HiddenField ID="hdnEPSDTCodeProfessionalThird" runat="server" />
    </div>
        </div>
    </div>

    <div class="col-sm-4">

        <div class="row">
             <div class="col-sm-7">
        <span class="ohio-field" style="font-size: 15px; text-align: right">Patient Amount Paid</span>
    </div>
    <div class="col-sm-5">
        <span style="text-align: left;">
            <asp:TextBox ID="txtProfessionalPatientAmountPaid" runat="server" CssClass="formField" MaxLength="18" Style="height: 30px; width: 110px"   onkeypress='return event.charCode == 46 || (event.charCode >= 48 && event.charCode <= 57)'  />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtProfessionalPatientAmountPaid" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                            ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            <%-- onkeypress="return onlyDotsAndNumbers(this,event);"--%>
        </span>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtProfessionalPatientAmountPaid" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
    </div>
        </div>
        <div class="row">
            <div class="col-sm-7">
        <span class="ohio-field" style="font-size: 15px; text-align: right">Hospital Discharge Date</span>
    </div>
    <div class="col-sm-5" style="height: 80px;">
        <span style="text-align: left;">
            <asp:TextBox ID="txtProfessionalHospitalDischargeDate" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 75%; min-width: 75px;" OnTextChanged="txtCheckForDate_TextChanged" AutoPostBack="true" />
            <asp:Image ID="imgSericeFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
            <ajax:CalendarExtender ID="ceProfessionalHospitalDischargeDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtProfessionalHospitalDischargeDate" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" EnabledOnClient="true" />
            <%--<asp:CompareValidator ID="cvProfessionalHospitalDischargeDate" runat="server" ErrorMessage="Future date not allowed" ForeColor="Red"
                Operator="LessThanEqual" ControlToValidate="txtProfessionalHospitalDischargeDate" ValidationGroup="valDischarge" Type="Date"></asp:CompareValidator>--%>
             <asp:Label ID="ProffHospDateRequiredError1" runat="server" Text="" CssClass="error-message" style="display:none;"></asp:Label>
        </span>
    </div>
        </div>
        <div class="row">
        <div class="col-sm-7">
        <span class="ohio-field" style="font-size: 15px; text-align: right">Last Menstrual Period</span>
    </div>
    <div class="col-sm-5">
        <span style="text-align: left;">
             
            <asp:TextBox ID="txtProfessionalLstmenstural" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 75%; min-width: 75px;" />
             <asp:Image ID="imgProfessionalLstmenstural" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
            <ajax:CalendarExtender ID="calextnderProfessional" runat="server" Format="MM/dd/yyyy" TargetControlID="txtProfessionalLstmenstural" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" EnabledOnClient="true" />
            <asp:CompareValidator ID="cpVldtProfessionalLstMenPrd" runat="server" ErrorMessage="Future date not allowed" ForeColor="Red" 
                Operator="LessThanEqual" ControlToValidate="txtProfessionalLstmenstural" ValidationGroup="valDischarge" type="Date"></asp:CompareValidator>
             <asp:RegularExpressionValidator ID="RegularExpressionValidatorLstMenstural" runat="server" ControlToValidate="txtProfessionalLstmenstural" 
                 ValidationExpression="^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$"  ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="</br>Please Enter in MM/DD/YYYY Format" Display="Dynamic" ForeColor="Red" />
            <asp:Label ID="ProfessionalLstmensturalDateRequiredError1" runat="server" Text="" CssClass="error-message" style="display:none;"></asp:Label>
       
        </span>
    </div>
</div>
    </div>

</div>
        </ContentTemplate>
    </asp:UpdatePanel>


