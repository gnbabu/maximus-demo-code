<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OfficeHoursServiceLocation" Codebehind="OfficeHoursServiceLocation.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<script type="text/javascript">
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
    function CheckPhoneLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 10)
            args.IsValid = false;
        if (test[0] === 0 || test[0] === 1 || test[3] === 0 || test[3] === 1)
            args.IsValid = false;
        return false;
    }
    function IsNumeric(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
    function findTotal() {
        var arr = $('[id^=OFFICE]')
        //document.getElementsByName('qty');
        var tot = 0;
        for (var i = 0; i < arr.length; i++) {
            if (parseInt(arr[i].value))
                tot += parseInt(arr[i].value);
        }
        //document.getElementById('total').value = tot;
        alert(tot);
    }

</script>
<style type="text/css">
    .formLabel200 {
        width: auto !important;
        white-space: normal !important;
        text-align: left !important;
    }

    select {
        min-width: 60px !important;
    }

    @media (max-width: 767px) {
        .dynamicAlign {
            text-align: left;
        }
        .dynamicDDWidth {
            width: 180px !important;
        }
    }

    @media (min-width: 768px) {
        .dynamicAlign {
            text-align: right;
        }
    }

   @media (min-width: 768px) and (max-width: 990px) {
        .dynamicDDWidth {
            width: 120px !important;
        }
   }

   @media (min-width: 991px) {
        .dynamicDDWidth {
            width: 180px !important;
        }
   }

   .noWrap {
        white-space: nowrap !important;
   }

    input[type=checkbox] {
        height: 21px;
        width: 21px;
    }
    .formDropDown-w300 {
        width:300px !important;
    }
</style>

<asp:UpdatePanel ID="upOffice" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <asp:ValidationSummary ID="valsumOfficeHoursIndividual" runat="server" ValidationGroup="valOfficeHoursIndividual" />
        </div>
        <div id="ParentTable" runat="server">

            <div class="row">
                <div class="col-sm-6 pageHeader" style="font-size:20px">
                    <span class="formLabel200">
                        <asp:CheckBox ID="chkProviderDirectoryOpt" runat="server" CssClass="formFieldCheckBox" Style="border: none" />&nbsp;&nbsp;&nbsp;Provider Directory Opt-Out</span>
                </div>

            </div>

            <br />
            <span class="pageHeader" style="text-decoration: underline">Provider Information</span>
            <asp:Label ID="provHelp" runat="server" Text="*Only required for Individual registrations" ForeColor="#e50000" ></asp:Label>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Cultural Competencies</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadCulturalCompProv" runat="server" CheckBoxes="true"  AriaSettings-Label="Cultural Competencies" EnableAriaSupport="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Languages Spoken</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadLanguagesSpokenProv" runat="server" CheckBoxes="true"  AriaSettings-Label="Languages Spoken" EnableAriaSupport="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>

                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Specialized Training</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadSpecializedTrainingProv" runat="server" CheckBoxes="true" AriaSettings-Label="Specialized Training" EnableAriaSupport="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>

                </div>
            </div>
            <br />  <br />

            <span class="pageHeader" style="text-decoration: underline">Hours of Operation</span>
            <asp:Label ID="officeHelp" runat="server" Text="*Hours providers available for appointments" ForeColor="#e50000" ></asp:Label>
            <br />
            <br />
            <div class="row">
                <div class="col-sm-3 dynamicAlign"><span class="formLabel200">Monday</span></div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlMonStartTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Monday StartTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-3">
                     <asp:DropDownList ID="ddlMonEndTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Monday EndTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <span class="formLabel200 noWrap">
                        <asp:CheckBox ID="chkMon24Hours" runat="server" CssClass="formFieldCheckBox" Style="border: none" onclick="analyzeSelection(this)" />&nbsp;&nbsp;&nbsp;Open 24 Hours
                    </span>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 dynamicAlign"><span class="formLabel200">Tuesday</span></div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlTueStartTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Tuesday StartTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-3">
                    <asp:DropDownList ID="ddlTueEndTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Tuesday EndTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
               <div class="col-sm-2">
                    <span class="formLabel200 noWrap">
                        <asp:CheckBox ID="chkTue24Hours" runat="server" CssClass="formFieldCheckBox" Style="border: none" onclick="analyzeSelection(this)" />&nbsp;&nbsp;&nbsp;Open 24 Hours</span>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 dynamicAlign"><span class="formLabel200">Wednesday</span></div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlWedStartTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Wednesday StartTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-3">
                     <asp:DropDownList ID="ddlWedEndTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Wednesday EndTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
               <div class="col-sm-2">
                    <span class="formLabel200 noWrap">
                        <asp:CheckBox ID="chkWed24Hours" runat="server" CssClass="formFieldCheckBox" Style="border: none" onclick="analyzeSelection(this)" />&nbsp;&nbsp;&nbsp;Open 24 Hours</span>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 dynamicAlign"><span class="formLabel200">Thursday</span></div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlThuStartTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Thursday StartTime"  RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-3">
                     <asp:DropDownList ID="ddlThuEndTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Thursday EndTime"  RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
               <div class="col-sm-2">
                    <span class="formLabel200 noWrap">
                        <asp:CheckBox ID="chkThu24Hours" runat="server" CssClass="formFieldCheckBox" Style="border: none" onclick="analyzeSelection(this)" />&nbsp;&nbsp;&nbsp;Open 24 Hours</span>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 dynamicAlign"><span class="formLabel200">Friday</span></div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlFriStartTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Friday StartTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-3">
                     <asp:DropDownList ID="ddlFriEndTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Friday EndTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
               <div class="col-sm-2">
                    <span class="formLabel200 noWrap">
                        <asp:CheckBox ID="chkFri24Hours" runat="server" CssClass="formFieldCheckBox" Style="border: none" onclick="analyzeSelection(this)" />&nbsp;&nbsp;&nbsp;Open 24 Hours</span>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 dynamicAlign"><span class="formLabel200">Saturday</span></div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlSatStartTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Saturday StartTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-3">
                     <asp:DropDownList ID="ddlSatEndTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Saturday EndTime"  RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
               <div class="col-sm-2">
                    <span class="formLabel200 noWrap">
                        <asp:CheckBox ID="chkSat24Hours" runat="server" CssClass="formFieldCheckBox" Style="border: none" onclick="analyzeSelection(this)" />&nbsp;&nbsp;&nbsp;Open 24 Hours</span>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 dynamicAlign"><span class="formLabel200">Sunday</span></div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlSunStartTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Sunday StartTime" RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-3">
                     <asp:DropDownList ID="ddlSunEndTime" runat="server" CssClass="formDropDown dynamicDDWidth" style="width:300px !important;" aria-label="Sunday EndTime"  RepeatDirection="Horizontal">
                    </asp:DropDownList>
                </div>
               <div class="col-sm-2">
                    <span class="formLabel200 noWrap">
                        <asp:CheckBox ID="chkSun24Hours" runat="server" CssClass="formFieldCheckBox" Style="border: none" onclick="analyzeSelection(this)" />&nbsp;&nbsp;&nbsp;Open 24 Hours</span>
                </div>
            </div>
            <br />
            <span class="pageHeader" style="text-decoration: underline">Office Information</span>
            <br />
            <br />

            <div class="row">
                <div class="col-sm-3"><span class="formLabel200">Website</span></div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="formFieldLarge" aria-label="Website"  Style="width: 450px;" MaxLength="100" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">24-hour telephone coverage</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTel" runat="server" CssClass="formDropDown" aria-label="24-hour telephone coverage" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Public transportation access</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTrans" runat="server" CssClass="formDropDown" aria-label="Public transportation access"  RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Electronic billing</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlEbilling" runat="server" CssClass="formDropDown" aria-label="Electronic billing" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">TDD/TDY</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTDD" runat="server" CssClass="formDropDown" aria-label="TDD/TDY"  RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>


            <br />
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Cultural Competencies</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadCulturalComp" runat="server" CheckBoxes="true" AriaSettings-Label="Cultural Competencies" EnableAriaSupport="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>

                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Languages Spoken</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadLanguagesSpoken" runat="server" CheckBoxes="true" AriaSettings-Label="Languages Spoken" EnableAriaSupport="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>

                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Specialized Training</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadSpecializedTraining" runat="server" CheckBoxes="true" AriaSettings-Label="Specialized Training" EnableAriaSupport="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>

                </div>
            </div>
            

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">ADA Compliance*</span>
                </div>
                <div class="col-sm-9">
                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadADAAccommodation" runat="server" CheckBoxes="true" AriaSettings-Label="ADA Compliance" EnableAriaSupport="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" EmptyMessage="--Select ADA--" DataTextField="Description" Skin="PDMSModern"
                        DataValueField="ADAAccomomodationId" >
                    </telerik:RadComboBox>
                    
                    <asp:CompareValidator runat="server" ID="Comparevalidator1" ValueToCompare="--Select ADA--"
                            Operator="NotEqual" ControlToValidate="RadADAAccommodation" ErrorMessage="You must select an ADA Complaince!"
                            CssClass="validationClass" />
                </div>
            </div>
            
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">ASL Offered*</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlASL" runat="server" CssClass="formDropDown" aria-label="ASL Offered" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
             <div class="row" id="divTServices" runat="server" > 
                <div class="col-sm-3">
                    <span class="formLabel200">Translation Services</span>
                </div>
                <div class="col-sm-3">

                    <asp:CheckBoxList runat="server" ID="chkTranslationServiceType" CausesValidation="true" aria-label="Translation Services" RepeatDirection="Horizontal" >
                      <asp:ListItem Text="Language Line" Value="1"></asp:ListItem>
                       <asp:ListItem Text="Translation" Value="2"></asp:ListItem>                    
                    
                </asp:CheckBoxList>

                </div>
            </div>

                        <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Telehealth Offered</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTelehealth" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Unknown" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">CHIP</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlCHIP" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Unknown" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accepts New Medicaid Patients</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlNewMedicaid" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Unknown" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>

            <%-- <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Total Hours</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtTotal" runat="server" CssClass="formFieldLarge" MaxLength="3" onKeyUp="javascript:numericOnly(this);" />
                </div>
            </div>--%>
            <br />
            <span class="pageHeader" style="text-decoration: underline">Patient Information</span>
            <br />
            <br />

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept new patients</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptNewPatients" runat="server" CssClass="formDropDown" aria-label="Accept new patients" RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>

                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept new patients from referral only </span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptpatientsref" runat="server" CssClass="formDropDown" aria-label="Accept new patients from referral only"  RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3"><span class="formLabel200">Youngest patients accepted</span></div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtyoungestpatients" runat="server" aria-label="Youngest patients accepted" CssClass="formFieldLarge" Style="width: 450px;" MaxLength="2" onkeypress="return IsNumeric(event);" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3"><span class="formLabel200">Oldest patients accepted</span></div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtoldestpatients" aria-label="Oldest patients accepted"  runat="server" CssClass="formFieldLarge" Style="width: 450px;" MaxLength="3" onkeypress="return IsNumeric(event);" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Gender of patient Accepted</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlGenderofPatients" aria-label="Gender of patient Accepted"  runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept newborn*</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptnewborn" aria-label="Accept newborn" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6"
                    ControlToValidate="ddlAcceptnewborn" ErrorMessage="*Accept Newborn" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" ValidationGroup="valReliaCard" /> 
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept pregnant women</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptPregnanetwomen" runat="server" aria-label="Accept pregnant" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>




        </div>
    </ContentTemplate>

</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
<asp:TextBox ID="hidIndAddressID" runat="server" Visible="false" />
<asp:TextBox ID="hidIsInMaintenance" runat="server" Visible="false" />
