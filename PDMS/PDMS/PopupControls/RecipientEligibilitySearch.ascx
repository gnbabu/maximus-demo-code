<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_RecipientEligibilitySearch" Codebehind="RecipientEligibilitySearch.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script type="text/javascript">
    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day earlier than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }

    function checkDate_todate(sender, args) {
        // var fromdate = new date($("#<%=txtToDos.ClientID%>").val());
        var fromdate = document.getElementById("#<%=txtToDos.ClientID%>");
        console.log(fromdate);
        console.log(sender._selectedDate);
        if (sender._selectedDate > fromdate) {
            alert("You cannot select a day earlier than From date!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
    function btnSearch_Click() {
        var selectedOption = document.getElementById("txtMedicaidBillingNumber").value;
        if (selectedOption === "") {
            document.getElementById("errorMessage").style.display = "block";
            return false; // Prevent form submission
        }
    }

    function EligibilitySearch() {
        var button = document.getElementById("eligibilitysearch").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("eligibilitysearch").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("eligibilitysearch").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("eligibilitysearch").setAttribute("aria-expanded", button);
    };

    function RecipientInformation() {
        var button = document.getElementById("recipientinfo").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("recipientinfo").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("recipientinfo").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("recipientinfo").setAttribute("aria-expanded", button);
    };
    function BenfitAssignmentPlan() {
        var button = document.getElementById("benfitassplan").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("benfitassplan").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("benfitassplan").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("benfitassplan").setAttribute("aria-expanded", button);
    };
    function ManagedCarePlan() {
        var button = document.getElementById("managedcareplan").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("managedcareplan").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("managedcareplan").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("managedcareplan").setAttribute("aria-expanded", button);
    };
    function ThirdPartyLiability() {
        var button = document.getElementById("thirdpartyliability").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("thirdpartyliability").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("thirdpartyliability").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("thirdpartyliability").setAttribute("aria-expanded", button);
    };
    function PatientLiability() {
        var button = document.getElementById("patientliability").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("patientliability").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("patientliability").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("patientliability").setAttribute("aria-expanded", button);
    };
    function LongTermCareFacilityPlacement() {
        var button = document.getElementById("longtermcarefacilityplacement").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("longtermcarefacilityplacement").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("longtermcarefacilityplacement").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("longtermcarefacilityplacement").setAttribute("aria-expanded", button);
    };
    function LockIn() {
        var button = document.getElementById("lockin").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("lockin").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("lockin").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("lockin").setAttribute("aria-expanded", button);
    };
    function Medicare() {
        var button = document.getElementById("medicare").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("medicare").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("medicare").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("medicare").setAttribute("aria-expanded", button);
    };
    function LevelOfCareDetermination() {
        var button = document.getElementById("levelofcaredetermination").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("levelofcaredetermination").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("levelofcaredetermination").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("levelofcaredetermination").setAttribute("aria-expanded", button);
    };
    function ServiceLimitation() {
        var button = document.getElementById("servicelimitation").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("servicelimitation").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("servicelimitation").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("servicelimitation").setAttribute("aria-expanded", button);
    };
    function RestrictedCoverage() {
        var button = document.getElementById("restrictedcoverage").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("restrictedcoverage").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("restrictedcoverage").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("restrictedcoverage").setAttribute("aria-expanded", button);
    };
    function AssociatedChild() {
        var button = document.getElementById("associatedchild").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("associatedchild").addClass("panelHeaderStyleEligbilitysearch");
        } else {
            button = "true"
            $("associatedchild").addClass("panelHeaderStyleEligbilitysearch");
        }
        document.getElementById("associatedchild").setAttribute("aria-expanded", button);
    };

</script>
<style type="text/css">
    .mySearchButton {
        background: linear-gradient(180deg,#ACCEFF 0%,#2E80FD 47.91%,#6BA5FF 97.92%,#3974CF 100% );
        border: 1px solid transparent;
        display: inline-block;
        cursor: pointer;
        color: #ffffff;
        font-family: Arial;
        font-size: 18px;
        font-weight: bold;
        padding: 6px 12px;
        text-decoration: none;
    }

    .btnBox {
        float: right;
        text-align: right;
        margin-top: 39px;
        margin-bottom: 6px;
        margin-right: -349px;
        margin-left: 0px;
        width: 100%;
    }

    .errorclass {
        color: #D33421;
    }

    select {
        min-width: 368px;
        height: 26px;
        border: 1px solid #ccc;
    }

    .panelHeaderStyleEligbilitysearch {
        background-color: transparent !important;
        border-style: none;
    }
</style>

<div class="row col-sm-15" style="border: groove; margin-left: 10px">
    <ajax:CollapsiblePanelExtender ID="cpeEligbilitysearch" runat="server" Collapsed="false" TargetControlID="pnlEligbilitysearch"
        ExpandControlID="pnlsepEligbilitysearch" CollapseControlID="pnlsepEligbilitysearch" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpeeligsearch" />
    <asp:Panel runat="server" ID="pnlsepEligbilitysearch" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" 
        CssClass="OwnerEligbilitysearch CollapsingSeparator">
        <%-- <div class="pageHeader" id="sepEligbilitysearch" runat="server" style="background-color: cornflowerblue; padding-left: 10px">* Eligibility Search</div>--%>
     <h1><asp:label runat="server" ID="lblcpeeligsearch" CssClass="pageHeader ph2">-</asp:label><span id="sepEligbilitysearch" runat="server"  class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="eligibilitysearch" aria-expanded="true" onclick="EligibilitySearch()">ELIGIBILITY SEARCH</button></span></h1>
    </asp:Panel>
            <span tabindex="0" style="color:#CC0505; font-size: 14pt !important; font-weight:100 !important";>An asterisk * indicates a required field</span>

    <asp:Panel ID="pnlEligbilitysearch" runat="server" Style="min-height: 250px; min-width: 400px; height: 250px; width: auto; max-width: 1200px;">
        <span id="errorMessage" role="alert" aria-live="assertive"><asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valProviderInfoHeader" /></span>
        <div><asp:Label ID="lblErrorMsg" runat="server"  ForeColor="Red" style="margin-left: 20px;"></asp:Label></div>

        
        <%--<div class="row" id="divMedicaidBillNumber" runat="server">--%>
        <div class="row" id="divMedicaidBillNumber1" runat="server">
            <div class="col-sm-4" id="divMedicaidBillNumber" runat="server">
                <div class="col-sm-5">
                    <asp:Label ID="lblMedBillNo" class="ohio-field"  style="font-size: 15px; text-align: right" AssociatedControlID="txtMedicaidBillingNumber" runat="server"> <span class="errorclass">* </span>Medicaid Billing Number</asp:Label>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtMedicaidBillingNumber" aria-label="MedicaidBillingNumber" runat="server" CssClass="formField" MaxLength="12" Style="height: 30px; width: 200px"/>
                        <asp:RequiredFieldValidator ID="rfvMedicaidBillingNumber" runat="server" ControlToValidate="txtMedicaidBillingNumber" 
                                ErrorMessage="* Enter Medicaid Billing Number" Display="Dynamic" ValidationGroup="valProviderInfoHeader" ></asp:RequiredFieldValidator>
                       
                        <asp:RegularExpressionValidator runat="server" ID="revMedicaidBillingNumber"
                            Display="Dynamic" ValidationGroup="valProviderInfoHeader"
                            ControlToValidate="txtMedicaidBillingNumber"
                            ValidationExpression="[0-9]{12}$"
                            ErrorMessage="12-digit number is required">
                        </asp:RegularExpressionValidator>
                    </span>
                </div>
            </div>
            <div class="col-sm-4" id="divSSN" runat="server">
                <div class="col-sm-5">
                    <asp:Label ID="lblSSN" class="ohio-field"  AssociatedControlID="txtSSN"  style="font-size: 15px; text-align: right" runat="server"><b>OR</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="errorclass">* </span>SSN</asp:Label>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtSSN" CssClass="formField" aria-label="SSN" Style="height: 30px; width: 150px" MaxLength="9" runat="server" ToolTip="Social Security Number" />
                        <asp:RequiredFieldValidator ID="rfvSSN" runat="server" ControlToValidate="txtSSN" 
                                ErrorMessage="* 9 digits number required" Display="Dynamic" ValidationGroup="valProviderInfoHeader" 
                                ></asp:RequiredFieldValidator>
           
                        <asp:RegularExpressionValidator runat="server" ID="revSSN" Display="Dynamic" ValidationGroup="valProviderInfoHeader"
                            ControlToValidate="txtSSN" ValidationExpression="^[0-9]{9}$" ErrorMessage="SSN 9 digits number required">
                        </asp:RegularExpressionValidator>
                    </span>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <asp:Label ID="lblFromDos"  class="ohio-field" style="font-size: 15px; text-align: right" AssociatedControlID="txtFromDos" runat="server"> <span class="errorclass">* </span>From DOS</asp:Label>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtFromDos" CssClass="formField" aria-label="from Dos" Style="height: 30px; width: 200px" runat="server" ToolTip="From Date of Service" />
                        <ajax:CalendarExtender ID="ceFromDos" runat="server" SelectedDate="<%# DateTime.Today %>" Format="MM/dd/yyyy" TargetControlID="txtFromDos"
                        PopupPosition="BottomLeft" CssClass="QstCalendarCSS" PopupButtonID="" EnabledOnClient="true" />

                        <asp:RequiredFieldValidator ID ="texFormDos" runat ="server" ValidationGroup="valProviderInfoHeader" ControlToValidate ="txtFromDos" Display ="Dynamic" Text ="*" ForeColor ="Red"></asp:RequiredFieldValidator>
                                                
                        <asp:CompareValidator ID="cvFromDos" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFromDos"
                            ErrorMessage="Select a valid PNM Date Available From" Display="Dynamic" ValueToCompare="MM/dd/yyyy" ValidationGroup="valProviderInfoHeader"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                    </span>
                </div>
            </div>
        </div>

        <%--<div class="col-sm-6">

        </div>--%>

        <div class="row">
            <div class="col-sm-4" id="divDOB" runat="server">
                <div class="col-sm-5">
                    <asp:Label class="ohio-field" ID="lblDOB" AssociatedControlID="txtBirthDate"  style="font-size: 15px; text-align: right" runat="server"> <span class="errorclass">* </span>Date of Birth</asp:Label>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtBirthDate" runat="server" aria-label="Birth Date" CssClass="formField" MaxLength="12" Style="height: 30px; width: 200px" />
                        <ajax:CalendarExtender ID="ceDateofbirth" runat="server" Format="MM/dd/yyyy" TargetControlID="txtBirthDate"
                        PopupPosition="BottomLeft" CssClass="QstCalendarCSS" PopupButtonID="" EnabledOnClient="true" />
                        <asp:RequiredFieldValidator ID="rfvBirthDate" runat="server"
                            ControlToValidate="txtBirthDate" ErrorMessage="* Please Enter Date Of Birth"
                            Display="Dynamic" ValidationGroup="valProviderInfoHeader">
                        </asp:RequiredFieldValidator>
                       
                        <asp:CompareValidator ID="cvDateofbirth" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate" ErrorMessage="Select a valid To date"
                            Display="Dynamic" ValueToCompare="MM/dd/yyyy" ValidationGroup="valProviderInfoHeader" SetFocusOnError="true"> 
                        </asp:CompareValidator>
                    </span>
                </div>
            </div>
            <div class="col-sm-4">&nbsp;</div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                  <asp:Label ID="lblToDos" class="ohio-field" AssociatedControlID="txtToDos"  style="font-size: 15px; text-align: right " runat="server"> <span class="errorclass">* </span>To DOS</asp:Label>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtToDos" CssClass="formField" aria-label="To Dos" Style="height: 30px; width: 200px" runat="server" ToolTip="To Date of Service" />
                        <ajax:CalendarExtender ID="ceToDos" runat="server" SelectedDate="<%# DateTime.Today %>" Format="MM/dd/yyyy" TargetControlID="txtToDos"
                        PopupPosition="Bottomleft" CssClass="QstCalendarCSS" PopupButtonID="" EnabledOnClient="true" />
                          
                         <asp:RequiredFieldValidator ID ="RequiredFieldValidator1" ValidationGroup="valProviderInfoHeader" runat ="server" ControlToValidate ="txtToDos" Display ="Dynamic" Text ="*" ForeColor ="Red"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvToDos" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtToDos"
                            ErrorMessage="Select a valid PNM Date Available To" Display="Dynamic" ValueToCompare="MM/dd/yyyy" ValidationGroup="valProviderInfoHeader"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                    </span>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-4">&nbsp;</div>
            <div class="col-sm-4">&nbsp;</div>
            <div class="col-sm-4" id="divProcedureCode" runat="server">
                <div class="col-sm-5">
<%--                    <span class="ohio-field" style="font-size: 15px; text-align: right">Procedure Code</span>--%>
                    <asp:Label ID="lblProcCode" class="ohio-field" AssociatedControlID="txtProcedureCode"  style="font-size: 15px; text-align: right" runat="server">Procedure Code</asp:Label>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtProcedureCode" aria-label="Procedure code" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="15" runat="server" ToolTip="Procedure Code" />
                        <%--<asp:DropDownList ID="ddlProcedureCode" CssClass="formField" Style="width: 150px" runat="server" AutoPostBack="True" />--%>
                        <asp:RequiredFieldValidator ID="rfvProcedurecode" runat="server" ControlToValidate="txtProcedureCode" 
                             ErrorMessage="* Please Enter Procedure Code" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader" 
                             ForeColor="Red"></asp:RequiredFieldValidator>                        
                    </span>
                </div>
            </div>
        </div>

        <div class="col-md-12 text-right">
            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="mySearchButton" OnClick="btnSearch_Click" ValidationGroup="valRetrieveReports" ToolTip="Search" />
            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocusred" OnClick="btnClear_Click" ToolTip="Clear" />
        </div>        

        <%--<div class="btnBox btnBoxCenter">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="valRetrieveReports" OnClientClick="showProgress()" ToolTip="Search" />
                            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" OnClientClick="showProgress()" ToolTip="Clear" />

                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSearch" />
                            <asp:PostBackTrigger ControlID="btnClear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>--%>        

    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="cpeRecipientInfo" runat="server" Collapsed="false" TargetControlID="pnlRecipientInfo"
        ExpandControlID="pnlsepRecipientInfo" CollapseControlID="pnlsepRecipientInfo" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcperecipientinfo" />
    <asp:Panel runat="server" ID="pnlsepRecipientInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
        ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInfo CollapsingSeparator">
        <h2><asp:label runat="server" ID="lblcperecipientinfo" CssClass="pageHeader ph2">-</asp:label><span id="sepRecipientInfo" runat="server"  class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="recipientinfo" aria-expanded="true" onclick="RecipientInformation()"> RECIPIENT INFORMATION</button></span></h2></asp:Panel>
        <asp:Panel ID="pnlRecipientInfo" runat="server" Style="min-height: 200px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">

        <div class="col-sm-6">
            <div class="row">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Medicaid Billing Number</span> </div><div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtRecinfoMedicaidbillNumber" aria-label="RecinfoMedicaidbillNumber" runat="server" CssClass="ohio-field-input" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" MaxLength="12" ToolTip="Medicaid Bill Number" />
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="row">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Date of Birth</span> </div><div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtDOB" runat="server" aria-label="DOBb" CssClass="ohio-field-input" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" ToolTip="Date Of Birth" />
                    </span>
                </div>
            </div>
        </div>

        <div class="col-sm-6">
            <div class="row">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Last Name</span> </div><div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtLast" runat="server" CssClass="ohio-field-input" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" ToolTip="Last Name" />
                    </span>
                </div>
            </div>
        </div>

        <div class="col-sm-6">
            <div class="row">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Date Of Death</span> </div><div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtDOD" runat="server" CssClass="ohio-field-input" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" ToolTip="Date of Death" />
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="row">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">First Name, MI</span> </div><div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="ohio-field-input" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" ToolTip="First Name and Middle Initial" />
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="row">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">SSN</span> </div><div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtSSN2" runat="server" aria-label="ssn" CssClass="ohio-field-input" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" MaxLength="09" ToolTip="Social Security Number" />
                    </span>
                </div>
            </div>
        </div>
    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="CPEBenefitsassplan" runat="server" Collapsed="false" TargetControlID="pnlBenefitsassplan"
        ExpandControlID="pnlsepBenefitsassplan" CollapseControlID="pnlsepBenefitsassplan" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpebenfitassplan" />
    <asp:Panel runat="server" ID="pnlsepBenefitsassplan" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"  ToolTip="Click to Expand/Collapse" CssClass="OwnerBenefitsassplan CollapsingSeparator">
       <h2><asp:label runat="server" ID="lblcpebenfitassplan" CssClass="pageHeader ph2">-</asp:label><span id="sepBenefitsassplan" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="benfitassplan" aria-expanded="true" onclick="BenfitAssignmentPlan()"> BENEFIT/ASSIGNMENT PLAN(S)</button></span></h2></asp:Panel>
        <asp:Panel ID="pnlBenefitsassplan" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divGrid" style="overflow-y: scroll;height: 200px; padding-top: 1px">
            <asp:GridView ID="gvBenefitsassplan" runat="server" Width="100%" Height="300px" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="AssignmentPlan" HeaderText="Benefit/Assignment Plan" AccessibleHeaderText="Benefit/Assignment Plan" />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" AccessibleHeaderText="Effective Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" AccessibleHeaderText="End Date" DataFormatString="{0:d}" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="cpeManagedCarePlan" runat="server" Collapsed="false" TargetControlID="pnlManagedCarePlan"
        ExpandControlID="pnlsepManagedCarePlan" CollapseControlID="pnlsepManagedCarePlan" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpemangcareplan" />
    <asp:Panel runat="server" ID="pnlsepManagedCarePlan" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerManagedCarePlan CollapsingSeparator">
    
       <h2><asp:label runat="server" ID="lblcpemangcareplan" CssClass="pageHeader ph2">-</asp:label><span id="sepManagedCarePlan" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="managedcareplan" aria-expanded="true" onclick="ManagedCarePlan()"> MANAGED CARE PLANS</button></span></h2></asp:Panel>
        <asp:Panel ID="pnlManagedCarePlan" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divManagedCarePlan" style="overflow-y: scroll;height: 200px; padding-top: 1px">
            <asp:GridView ID="gvManagedCarePlan" runat="server" Width="100%" AllowSorting="false" CssClass="gridview" Style="margin-left: 2px!important; margin-right: 2px"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" >
                <Columns>
                    <asp:BoundField DataField="PlanName" HeaderText="Plan Name" AccessibleHeaderText="Plan Name" />
                    <asp:BoundField DataField="PlanId" HeaderText="Payer Id" AccessibleHeaderText="Payer Id" />
                    <asp:BoundField DataField="PlanDescription" HeaderText="Plan Description" AccessibleHeaderText="Plan Description" />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" AccessibleHeaderText="Effective Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" AccessibleHeaderText="End Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="ManagedCareBenefits" HeaderText="Managed Care Benefits" AccessibleHeaderText="Managed Care Benefits" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="cpeThirdPartyInsurance" runat="server" Collapsed="false" TargetControlID="pnlThirdPartyInsurance"
        ExpandControlID="pnlsepThirdPartyInsurance" CollapseControlID="pnlsepThirdPartyInsurance" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpethirdpartyliab" />
    <asp:Panel runat="server" ID="pnlsepThirdPartyInsurance" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerThirdPartyInsurance CollapsingSeparator">
       
       <h2>
           <asp:label runat="server" ID="lblcpethirdpartyliab" CssClass="pageHeader ph2">-</asp:label><span id="sepThirdPartyInsurance" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="thirdpartyliability" aria-expanded="true" onclick="ThirdPartyLiability()"> THIRD PARTY LIABILITY</button></span>
       </h2>

    </asp:Panel>
        <asp:Panel ID="pnlThirdPartyInsurance" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divThirdPartyInsurance" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvThirdPartyInsurance" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" style="margin-left: 2px; margin-right: 2px;">
                <Columns>
                    <asp:BoundField DataField="CarrierName" HeaderText="Carrier Name" AccessibleHeaderText="Carrier Name" />
                    <asp:BoundField DataField="CarrierNumber" HeaderText="Carrier Number" AccessibleHeaderText="Carrier Number" />
                    <asp:BoundField DataField="Policynumber" HeaderText="Policy Number" AccessibleHeaderText="Policy Number" />
                    <asp:BoundField DataField="Policyholder" HeaderText="Policy Holder" AccessibleHeaderText="Policy Holder" />
                    <asp:BoundField DataField="CoverageType" HeaderText="Coverage Type" AccessibleHeaderText="Coverage Type" />
                    <asp:BoundField DataField="Coverage" HeaderText="Coverage" AccessibleHeaderText="Coverage" />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" AccessibleHeaderText="Effective Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" AccessibleHeaderText="End Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="GroupNumber" HeaderText="Group Number" AccessibleHeaderText="Group Number" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="cpePatientLiability" runat="server" Collapsed="false" TargetControlID="pnlPatientLiability"
        ExpandControlID="pnlsepPatientLiability" CollapseControlID="pnlsepPatientLiability" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpepatientliab" />
    <asp:Panel runat="server" ID="pnlsepPatientLiability" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerPatientLiability CollapsingSeparator">
      <h2>  
          <asp:label runat="server" ID="lblcpepatientliab" CssClass="pageHeader ph2">-</asp:label><span id="sepPatientLiability" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="patientliability" aria-expanded="true" onclick="PatientLiability()"> PATIENT LIABILITY</button></span>
      </h2>

    </asp:Panel>
        <asp:Panel ID="pnlPatientLiability" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divPatientLiability" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvPatientLiability" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="FinancialPayer" HeaderText="Financial Payer" AccessibleHeaderText="Financial Payer"  />
                    <asp:BoundField DataField="MonthlyAmount" HeaderText="Monthly Amount" AccessibleHeaderText="Monthly Amount" />
                    <asp:BoundField DataField="Type" HeaderText="Type" AccessibleHeaderText="Liability Type" />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" AccessibleHeaderText="Effective Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" AccessibleHeaderText="End Date" DataFormatString="{0:d}" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="cpeLongTermCareFacilityPlacements" runat="server" Collapsed="false" TargetControlID="pnlLongTermCareFacilityPlacements"
        ExpandControlID="pnlsepLongTermCareFacilityPlacements" CollapseControlID="pnlsepLongTermCareFacilityPlacements" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpelongtermcare" />
    <asp:Panel runat="server" ID="pnlsepLongTermCareFacilityPlacements" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerLongTermCareFacilityPlacements CollapsingSeparator">
        <h2>
            <asp:label runat="server" ID="lblcpelongtermcare" CssClass="pageHeader ph2">-</asp:label><span id="sepLongTermCareFacilityPlacements" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="longtermcarefacilityplacement" aria-expanded="true" onclick="LongTermCareFacilityPlacement()"> LONG TERM CARE FACILITY PLACEMENTS</button></span>
        </h2>
     </asp:Panel>
        <asp:Panel ID="pnlLongTermCareFacilityPlacements" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divLongTermCareFacilityPlacements" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvLongTermCareFacilityPlacements" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="FacilityType" HeaderText="Facility Type" AccessibleHeaderText="Facility Type" />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Date of Admission" AccessibleHeaderText="Date of Admission" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="Discharge Date" AccessibleHeaderText="Discharge Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EffectiveDateMedicaidCoverage" HeaderText="Effective Date of Medicaid Coverage" AccessibleHeaderText="Effective Date of Medicaid Coverage" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDateMedicaidCoverage" HeaderText="End Date of Medicaid Coverage" AccessibleHeaderText="End Date of Medicaid Coverage" DataFormatString="{0:d}" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="cpeLockin" runat="server" Collapsed="false" TargetControlID="pnlLockin"
        ExpandControlID="pnlsepLockin" CollapseControlID="pnlsepLockin" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpelockin" />
    <asp:Panel runat="server" ID="pnlsepLockin" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerLockin CollapsingSeparator">
      <h2>   
          <asp:label runat="server" ID="lblcpelockin" CssClass="pageHeader ph2">-</asp:label><span id="sepLockin" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="lockin" aria-expanded="true" onclick="LockIn()">LOCK IN</button></span>
      </h2>
    </asp:Panel>
        <asp:Panel ID="pnlLockin" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divLockin" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvLockin" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="LockinPlan" HeaderText="Lock-In Plan" AccessibleHeaderText="Lock-In Plan" />
                    <asp:BoundField DataField="LockinType" HeaderText="Lock In Type" AccessibleHeaderText="Lock In Type" />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" AccessibleHeaderText="Effective Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" AccessibleHeaderText="End Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="ProviderNPI" HeaderText="Provider NPI" AccessibleHeaderText="Provider NPI" />
                    <asp:BoundField DataField="ProviderName" HeaderText="Provider Name" AccessibleHeaderText="Provider Name" />
                    <asp:BoundField DataField="ProviderPhoneNumber" HeaderText="Provider Phone" AccessibleHeaderText="Provider Phone" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="cpeMedicare" runat="server" Collapsed="false" TargetControlID="pnlMedicare"
        ExpandControlID="pnlsepMedicare" CollapseControlID="pnlsepMedicare"  ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpemedicare" />
    <asp:Panel runat="server" ID="pnlsepMedicare" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerMedicare CollapsingSeparator">
       <h2>
           <asp:label runat="server" ID="lblcpemedicare" CssClass="pageHeader ph2">-</asp:label><span id="sepMedicare" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="medicare" aria-expanded="true" onclick="Medicare()"> MEDICARE </button></span>
       </h2>
    </asp:Panel>
        <asp:Panel ID="pnlMedicare" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divMedicare" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvMedicare" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="Coverage" HeaderText="Coverage" AccessibleHeaderText="Coverage" />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" AccessibleHeaderText="Effective Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" AccessibleHeaderText="End Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="PlanName" HeaderText="Plan Name" AccessibleHeaderText="Plan Name" />
                    <asp:BoundField DataField="PlanId" HeaderText="Plan ID" AccessibleHeaderText="Plan ID" />
                    <asp:BoundField DataField="MedicareId" HeaderText="Medicare ID" AccessibleHeaderText="Medicare ID" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="cpeLevelofCareDetermination" runat="server" Collapsed="false" TargetControlID="pnlLevelofCareDetermination"
        ExpandControlID="pnlsepLevelofCareDetermination" CollapseControlID="pnlsepLevelofCareDetermination" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpelevelcaredeter" />
    <asp:Panel runat="server" ID="pnlsepLevelofCareDetermination" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerLevelofCareDetermination CollapsingSeparator">
       <h2> 
           <asp:label runat="server" ID="lblcpelevelcaredeter" CssClass="pageHeader ph2">-</asp:label><span id="sepLevelofCareDetermination" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="levelofcaredetermination" aria-expanded="true" onclick="LevelOfCareDetermination()">LEVEL OF CARE DETERMINATION</button></span>
       </h2>
    </asp:Panel>
        <asp:Panel ID="pnlLevelofCareDetermination" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divLevelofCareDetermination" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvLevelofCareDetermination" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="FacilityType" HeaderText="Facility Type" AccessibleHeaderText="Facility Type" />
                    <asp:BoundField DataField="Status" HeaderText="Status" AccessibleHeaderText="Status" />
                    <asp:BoundField DataField="DeterminationDate" HeaderText="Determination Date" AccessibleHeaderText="Determination Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="LOCDetermination" HeaderText="LOC Determination" AccessibleHeaderText="LOC Determination" />
                    <asp:BoundField DataField="Description" HeaderText="Description" AccessibleHeaderText="Description" />
                    <asp:BoundField DataField="StartDate" HeaderText="LOC Start Date" AccessibleHeaderText="LOC Start Date" DataFormatString="{0:d}"/>
                    <asp:BoundField DataField="EndDate" HeaderText="LOC End Date" AccessibleHeaderText="LOC End Date" DataFormatString="{0:d}" />
                </Columns>

                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="cpeServiceLimitation" runat="server" Collapsed="false" TargetControlID="pnlServiceLimitation"
        ExpandControlID="pnlsepServiceLimitation" CollapseControlID="pnlsepServiceLimitation" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpeservicelimitation" />
        <asp:Panel runat="server" ID="pnlsepServiceLimitation" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"  ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceLimitation CollapsingSeparator">
             <h2>
                <asp:label runat="server" ID="lblcpeservicelimitation" CssClass="pageHeader ph2">-</asp:label><span id="sepServiceLimitation" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="servicelimitation" aria-expanded="true" onclick="ServiceLimitation()"> SERVICE LIMITATION </button></span>
             </h2>
        </asp:Panel>
        <asp:Panel ID="pnlServiceLimitation" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divServiceLimitation" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvServiceLimitation" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left"
                OnRowDataBound="gvServiceLimitation_RowDataBound" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="ProcedureCode" HeaderText="Procedure Code" AccessibleHeaderText="Procedure Code" />
                    <asp:BoundField DataField="ServiceLimitDescription" HeaderText="Description" AccessibleHeaderText="Description" />
                    <asp:BoundField DataField="BenefitDescription" HeaderText="Benefit Description" AccessibleHeaderText="Benefit Description" />
                    <asp:BoundField DataField="TotalLimits" HeaderText="Total Limits" AccessibleHeaderText="Total Limits" />
                    <asp:BoundField DataField="UsedLimits" HeaderText="Used Limits" AccessibleHeaderText="Used Limits" />
                    <asp:BoundField DataField="RemainingLimits" HeaderText="Remaining Limits" AccessibleHeaderText="Remaining Limits" />
                    <asp:BoundField DataField="Timeframe" HeaderText="Time Frame"  AccessibleHeaderText="Time Frame"  />
                    <asp:BoundField DataField="DateOfNextService" HeaderText="Date of Next Service" AccessibleHeaderText="Date of Next Service" DataFormatString="{0:d}" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
        <%--<div class="col-sm-12">--%>
        <div class="row col-sm-11" id="divError" runat="server">
<%--            <div class="col-sm-1">
                <span class="ohio-field" style="font-size: 5px; text-align: left"></span>
            </div>--%>
            <div class="col-sm-11">
                <span style="text-align: left; color: red">
                    <asp:Label ID="lblErrorDisplay" runat="server" Text=""></asp:Label>
                </span>
            </div>
        </div>
       <%-- </div>--%>

    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="cpeRestrictedCoverage" runat="server" Collapsed="false" TargetControlID="pnlRestrictedCoverage"
        ExpandControlID="pnlsepRestrictedCoverage" CollapseControlID="pnlsepRestrictedCoverage" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcperestrictedcoverage" />
        <asp:Panel runat="server" ID="pnlsepRestrictedCoverage" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerRestrictedCoverage CollapsingSeparator">
          <h2>
             <asp:label runat="server" ID="lblcperestrictedcoverage" CssClass="pageHeader ph2">-</asp:label><span id="sepRestrictedCoverage" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="restrictedcoverage" aria-expanded="true" onclick="RestrictedCoverage()"> RESTRICTED COVERAGE</button></span>
          </h2>
        </asp:Panel>
        <asp:Panel ID="pnlRestrictedCoverage" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divRestrictedCoverage" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvRestrictedCoverage" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" AccessibleHeaderText="Effective Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" AccessibleHeaderText="End Date" DataFormatString="{0:d}" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="cpeAssociatedChildren" runat="server" Collapsed="false" TargetControlID="pnlAssociatedChildren"
        ExpandControlID="pnlsepAssociatedChildren" CollapseControlID="pnlsepAssociatedChildren" ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical" SuppressPostBack="true" TextLabelID="lblcpeassociatedchild" />
        <asp:Panel runat="server" ID="pnlsepAssociatedChildren" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerAssociatedChildren CollapsingSeparator">
            <h2>
                <asp:label runat="server" ID="lblcpeassociatedchild" CssClass="pageHeader ph2">-</asp:label><span id="sepAssociatedChildren" runat="server" class="pageHeader ph2"><button type="button" class="panelHeaderStyleEligbilitysearch" tabindex="0" id="associatedchild" aria-expanded="true" onclick="AssociatedChild()"> ASSOCIATED CHILD(REN)</button></span>
            </h2> 
        </asp:Panel>
        <asp:Panel ID="pnlAssociatedChildren" runat="server" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divAssociatedChildren" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvAssociatedChildren" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="MedicaidId" HeaderText="Medicaid Billing Number" AccessibleHeaderText="Medicaid Billing Number" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" AccessibleHeaderText="First Name" />
                    <asp:BoundField DataField="MiddleInitial" HeaderText="MI" AccessibleHeaderText="Middle Initial" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" AccessibleHeaderText="Last Name" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" AccessibleHeaderText="Gender" />
                    <asp:BoundField DataField="DateOfBirth" HeaderText="Date of Birth" AccessibleHeaderText="Date of Birth" DataFormatString="{0:d}" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <ajax:CollapsiblePanelExtender ID="cpeSpecialProgram" runat="server" Collapsed="false" TargetControlID="pnlSpecialProgram"
        ExpandControlID="pnlsepSpecialProgram" CollapseControlID="pnlsepSpecialProgram" />
        <asp:Panel runat="server" ID="pnlsepSpecialProgram" class="CollapsingSeparator" Visible="false" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerSpecialProgram CollapsingSeparator">
           <h2> 
                <span id="sepSpecialProgram" runat="server" class="pageHeader ph2">- SPECIAL PROGRAM</span>
            </h2>
        </asp:Panel>
        <asp:Panel ID="pnlSpecialProgram" runat="server" Visible="false" Style="min-height: 200px; min-width: 350px; height: auto; width: auto; max-width: 1500px;">
        <div class="divSpecialProgram" style="overflow-y: scroll;height: 200px; padding-top: 1px">

            <asp:GridView ID="gvSpecialProgram" runat="server" Width="100%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="" AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 2px; margin-right: 2px">
                <Columns>
                    <asp:BoundField DataField="eligType" HeaderText="Special Program" AccessibleHeaderText="Special Program" />
                    <asp:BoundField DataField="dteAppReceived" HeaderText="Date of Application Received" AccessibleHeaderText="Date of Application Received" />
                    <asp:BoundField DataField="applicationStatus" HeaderText="Status of Application" AccessibleHeaderText="Status of Application" />
                    <asp:BoundField DataField="provName" HeaderText="Provider Name" AccessibleHeaderText="Provider Name" />
                    <%-- Not Found Provider Name in WSDLL and MITS documents--%>
                    <asp:BoundField DataField="provPhone" HeaderText="Provider Phone Number" AccessibleHeaderText="Provider Phone Number" />
                    <asp:BoundField DataField="optoutStartDate" HeaderText="Program Start Date" AccessibleHeaderText="Program Start Date" />
                    <%-- Not Found Program Start Date in WSDLL and MITS documents--%>
                    <asp:BoundField DataField="optOutEndDate" HeaderText="Program End Date" AccessibleHeaderText="Program End Date" />
                    <%-- Not Found Program End Date in WSDLL and MITS documents--%>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
    <asp:TextBox ID="txthddnNPI" Visible ="false" runat="server"></asp:TextBox>
</div>