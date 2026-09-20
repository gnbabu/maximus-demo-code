<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Process_AtypicalProviderEnrollment" Codebehind="AtypicalProviderEnrollment.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div class="row"><br /></div>
    <h2><b>Atypical Provider Enrollment</b></h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script src="../Scripts/jspdf.umd.min.js"></script>
    <script src="../Scripts/jspdf.plugin.autotable.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.loadtemplate/1.5.10/jquery.loadTemplate.min.js"></script>
    <script type="text/javascript" src="../Scripts/paging.js"></script>

  
<style>

    @media only screen and (max-width: 760px) {
        input[type=text], input[type=password], textarea, select {
            width: 100% !important;
            min-width: 100px !important;
        }

        .col-sm-12 {
            padding-right: 0px;
            padding-left: 0px;
        }

        .WhiteBox {
            padding: 0px;
        }
    }

    @media only screen and (max-width: 990px) {
        input[type=text], input[type=password], textarea, select {
            width: 100% !important;
            min-width: 100px !important;
        }
    }

    @media only screen and (max-width: 400px) {
        .form-control {
            font-size: 11px;
        }

        .WhiteBox {
            padding: 0px;
        }
    }

     .errMsgInput {
         border: 1px solid #f00 !important
     }

     .errMsg {
         color: #f00;
         /*position: absolute;
         display: block;
         text-align: right;
         width: 95%;*/
     }

      /* The Modal (background) */
    .modal1 {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 100; /* Sit on top */
            left: 50%;
            top: 50%;
            width: 50%;
            max-height: 80vh; /* Limit max height to 80% of viewport height */
            overflow: hidden; /* Hide overflow on container, scrolling inside modal-content */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
            border: 1px solid #888;
            transform: translate(-50%, -50%);
            box-sizing: border-box;
    }

    .modal-content {
            background-color: #fefefe;
            margin: auto;
            padding-top: 0px;
            padding-bottom: 20px;
            border: 1px solid #888;
            width: 100%;
            max-height: 80vh; /* Match container max height */
            overflow-y: auto; /* Enable vertical scroll inside modal content */
            box-sizing: border-box;
    }

     #divpnlLoader {
        text-align: center;
     }


</style>
<script type="text/javascript">   
    
    let isAddrChangeConfirmed =0;
    let newAddrLine1 = '';
    let newAddrLine2 = '';
    let newAddrCity = '';
    let newAddrState = '';
    let newAddrZip4 = '';
    let newAddrZip5 = '';
    let newAddrCountyName = '';
    let newAddrCountyNumber = '';


    const errorCodeResources = [
        { key: "ADDR_ERROR_CODE_00", value: "Default High-rise or Rural Route answer" },
        { key: "ADDR_ERROR_CODE_01", value: "Five Digit Zip no match, match found in finance number" },
        { key: "ADDR_ERROR_CODE_02", value: "Input add-on not found, replaced with correct add-on" },
        { key: "ADDR_ERROR_CODE_03", value: "No correlation between city and unique zip code, five digit zip deleted, no match" },
        { key: "ADDR_ERROR_CODE_04", value: "City name corrected" },
        { key: "ADDR_ERROR_CODE_05", value: "Address standardized" },
        { key: "ADDR_ERROR_CODE_06", value: "Street number is not a precise match to street range type. For example, an alphanumeric number like 10A fell within a numeric range of 1 to 99" },
        { key: "ADDR_ERROR_CODE_07", value: "Address non-deliverable. No add-on assigned" },
        { key: "ADDR_ERROR_CODE_08", value: "Secondary number is not a precise match to secondary range type. See error 06 for details" },
        { key: "ADDR_ERROR_CODE_09", value: "Address is delivery point alternate" },
        { key: "ADDR_ERROR_CODE_10", value: "City is a part of multiple counties" },
        { key: "ADDR_ERROR_CODE_11", value: "Results failed multi-component rule used by CASS, no validated answer possible. List of possible results returned. Absolute value of return code (rc) equal to number of results returned" },
        { key: "ADDR_ERROR_CODE_12", value: "Highrise default answer returned, all possible apartment range results returned. Last result in array is proceeded by a record padded with \"Z\"’s" },
        { key: "ADDR_ERROR_CODE_13", value: "Military address" },
        { key: "ADDR_ERROR_CODE_14", value: "Street address with appended apartment number" },
        { key: "ADDR_ERROR_CODE_15", value: "Near matches placed in results field" },
        { key: "ADDR_ERROR_CODE_16", value: "Preferred city name used" },
        { key: "ADDR_ERROR_CODE_17", value: "No match found in multiple-word range, search dropped down to one-word range" },
        { key: "ADDR_ERROR_CODE_18", value: "DPV False Positive" },
        { key: "ADDR_ERROR_CODE_19", value: "Record found in Early Warning System file, no match" },
        { key: "ADDR_ERROR_CODE_20", value: "Input street name modified" },
        { key: "ADDR_ERROR_CODE_21", value: "Account marked for DPV False Positive" },
        { key: "ADDR_ERROR_CODE_22", value: "Account marked for LACS Link False Positive" },
        { key: "ADDR_ERROR_CODE_23", value: "Unique zip no match with no city correlation, zip code deleted" },
        { key: "ADDR_ERROR_CODE_24", value: "LACS False Positive" },
        { key: "ADDR_ERROR_CODE_30", value: "Foreign Address" },
        { key: "ADDR_ERROR_CODE_31", value: "Geocoder files missing or corrupt" },
        { key: "ADDR_ERROR_CODE_40", value: "Multiple matches: wrong input Post-Directional" },
        { key: "ADDR_ERROR_CODE_41", value: "Multiple matches: wrong input Pre-Directional" },
        { key: "ADDR_ERROR_CODE_42", value: "Multiple matches: wrong input Street Suffix" },
        { key: "ADDR_ERROR_CODE_43", value: "Zip Code is PO Box or Rural Route only" },
        { key: "ADDR_ERROR_CODE_44", value: "Large amount of data dropped" },
        { key: "ADDR_ERROR_CODE_45", value: "Street Suffix was modified" },
        { key: "ADDR_ERROR_CODE_46", value: "Street Directional was modified" },
        { key: "ADDR_ERROR_CODE_47", value: "Address requires apartment/suite number: none input" },
        { key: "ADDR_ERROR_CODE_48", value: "Multiple matches: would resolve provided a pre-directional" },
        { key: "ADDR_ERROR_CODE_49", value: "Multiple matches: would resolve provided a post-directional" },
        { key: "ADDR_ERROR_CODE_50", value: "Multiple matches: would resolve provided a street suffix" },
        { key: "ADDR_ERROR_CODE_51", value: "Address does not require apartment/suite: none input" },
        { key: "ADDR_ERROR_CODE_52", value: "Address does not require apartment/suite: wrong input" },
        { key: "ADDR_ERROR_CODE_57", value: "Address not standardized: not enough information provided" },
        { key: "ADDR_ERROR_CODE_58", value: "Address not standardized: invalid Zip Code" },
        { key: "ADDR_ERROR_CODE_59", value: "Address not standardized: address belongs to a US Territory" },
        { key: "ADDR_ERROR_CODE_60", value: "Expired Verification Database" },
        { key: "ADDR_ERROR_CODE_61", value: "House number is not on street" },
        { key: "ADDR_ERROR_CODE_62", value: "Street is not in the Zip Code provided" },
        { key: "ADDR_ERROR_CODE_64", value: "Internal error" }
    ];

    const _errorResourceMap = errorCodeResources.reduce((map, item) => {
        map[item.key] = item.value;
        return map;
    }, {});

    function escapeHtml(str) {
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    $(function () {
        initializeDropdowns();
        $('#txtReqEffDate').val(formatTodayMMDDYYYY());
    });

    function formatTodayMMDDYYYY() {
        var d = new Date();
        var mm = String(d.getMonth() + 1).padStart(2, '0');
        var dd = String(d.getDate()).padStart(2, '0');
        var yyyy = d.getFullYear();
        return yyyy + '-' + mm + '-' + dd;
    }

    function initializeDropdowns() {
        $('#ddlProviderType, #ddlGender, #ddlState').val('');
        var APIToken = $("[id*=hdnAccessToken]").val();
        
         $.ajax({
             type: "GET",
             url: webApiEnrollment + "GetAtypicalAppDropdowns",
             headers: {
                 "Access-Control-Allow-Origin": "*",
                 "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                 "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                 "Authorization": "Bearer " + APIToken
             },
             contentType: "application/json; charset=utf-8",
             dataType: "json"
         }).done(function (result) {
             if (result) {
                 
                 BindProviderType(result.dtProviderTypes);
                 BindGender(result.dtGender);
                 BindState(result.dtState);
             }
         }).fail(function (jqXHR) {
             handleAjaxError(jqXHR, 'pnlShowErrorStream', 'Error loading dropdown data: ');
         });
    }
    function BindProviderType(provTypes) {
        if (!provTypes || !provTypes.length) return;
        
        const ddlProvType = document.getElementById('ddlProviderType');
        ddlProvType.innerHTML = '';
        const defaultOption = document.createElement('option');
        defaultOption.text = '';
        ddlProvType.add(defaultOption);
        for (var i = 0; i < provTypes.length; i++) {
            const option = document.createElement('option');
            option.value = provTypes[i].MMIS_PROVIDER_TYPE_ID;
            option.text = provTypes[i].PROVIDER_TYPE_NAME;
            ddlProvType.add(option);
        }
    }

    function BindGender(dtGender) {
        if (!dtGender || !dtGender.length) return;

        const ddlGender = document.getElementById('ddlGender');
        ddlGender.innerHTML = '';
        const defaultOption = document.createElement('option');
        defaultOption.text = '';
        ddlGender.add(defaultOption);
        for (var i = 0; i < dtGender.length; i++) {
            const option = document.createElement('option');
            option.value = dtGender[i].PROVIDER_GENDER_INITIAL;
            option.text = dtGender[i].PROVIDER_GENDER_NAME;
            ddlGender.add(option);
        }
    }

    function BindState(dtStates) {
        if (!dtStates || !dtStates.length) return;

        const ddlState = document.getElementById('ddlState');
        ddlState.innerHTML = '';
        const defaultOption = document.createElement('option');
        defaultOption.text = '';
        ddlState.add(defaultOption);
        for (var i = 0; i < dtStates.length; i++) {
            const option = document.createElement('option');
            option.value = dtStates[i].STATE_ID;
            option.text = dtStates[i].STATE_ABBREV;
            ddlState.add(option);
        }
    }

    function ValidateNPI() {
        if (!ValidateFormInputData()) {
            $("#divAddressData").hide();
            return false;
        }
        ValidateNPIfromNPPES(true);
    }

    async function ValidateNPIfromNPPES(isBindTaxonomy) {
        let valid = false;
        document.getElementById('divpnlLoader').style.display = "block";

        var APIToken = $("[id*=hdnAccessToken]").val();

        const npi = $('#txtNPI').val();
        const firstname = $('#txtFirstName').val();
        const lastname = $('#txtLastName').val();
        const gender = $('#ddlGender').val();
        const provType = $('#ddlProviderType').val();
        const ssn = $('#txtTaxID').val();

        const nppesReq = {
            NPI: npi,
            ProviderType: provType,
            FirstName: firstname,
            LastName: lastname,
            Gender: gender,
            SSN: ssn,
            UserID: $("[id*=hdnUserName]").val()
        };

        const result = await CommonAjaxCall({
            url: webApiEnrollment + "ValidateNPIfromNPPES",
            method: 'POST',
            data: nppesReq,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            }
        });

        /*console.log('Found NPPES');*/
        const $errorPanel = $('#pnlShowErrorStream').empty();

        if (!result) return;
        /*console.log(result.ResponseCode);*/
        if (result.ResponseCode === '200' && result.TaxnomyTypes) {
            document.getElementById('divpnlLoader').style.display = "none";

            valid = true;
            /* console.log('success response');*/
            ClearNPIValidationErrors();
            if (isBindTaxonomy) {
                LoadTaxonomyDropdown(result.TaxnomyTypes);
                LoadSpecialtyDropdown(result.SpecialtyTypes);
                $("#divAddressData").show();
            }
            SetFieldDisabledState(true);
            if (isAddrChangeConfirmed === 1) {
                $('#btnCreateReg').show();
            }

        } else if (['300'].includes(result.ResponseCode)) {
            document.getElementById('divpnlLoader').style.display = "none";
            valid = false;
            $errorPanel.html(`<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">${result.ResponseDesc}</span><br /><br />`);
        } else {
            document.getElementById('divpnlLoader').style.display = "none";
            valid = false;
            $errorPanel.html(`<span tabindex="0" style="color:#CC0505; font-size:14pt; padding-left:10px; font-weight:100;">Error: ${result.ResponseCode} : ${result.ResponseDesc}</span>`);
        }
        
        return valid;
    }

    function LoadTaxonomyDropdown(dtTaxonomies) {
        
        if (!dtTaxonomies || !dtTaxonomies.length) return;
        
        const ddlTaxonomy = document.getElementById('ddlTaxonomy');
        ddlTaxonomy.innerHTML = '';
        const defaultOption = document.createElement('option');
        defaultOption.text = '';
        ddlTaxonomy.add(defaultOption);
        for (var i = 0; i < dtTaxonomies.length; i++) {
            const option = document.createElement('option');
            option.value = dtTaxonomies[i].TAXONOMY_CODE;
            option.text = dtTaxonomies[i].TAXONOMY_DESCRIPTION;
            ddlTaxonomy.add(option);
        }

    }

    function LoadSpecialtyDropdown(dtSpecialties) {

        if (!dtSpecialties || !dtSpecialties.length) return;

        const ddlSpecialty = document.getElementById('ddlSpecialty');
        ddlSpecialty.innerHTML = '';
        const defaultOption = document.createElement('option');
        defaultOption.text = '';
        ddlSpecialty.add(defaultOption);
        for (var i = 0; i < dtSpecialties.length; i++) {
            const option = document.createElement('option');
            option.value = dtSpecialties[i].SPECIALTY_TYPE_ID;
            option.text = dtSpecialties[i].SPECIALTY_TYPE_NAME;
            ddlSpecialty.add(option);
        }

    }



    function ClearNPIValidationErrors() {
        
        ['spanddlProviderType', 'spantxtFirstName', 'spantxtMidInt', 'spantxtLastName', 'spantxtTaxID', 'spantxtNPI', 'spanddlGender', 'spantxtDOB'].forEach(id => {
            $(`#${id}`).empty();
        });
        ['ddlProviderType', 'txtFirstName', 'txtMidInt', 'txtLastName', 'txtTaxID', 'txtNPI', 'ddlGender', 'txtDOB'].forEach(id => {
            $(`#${id}`).removeClass('errMsgInput');
        });
        $('#pnlShowErrorStream').empty();
    }

    function ValidateFormInputData() {
        let valid = true;     

        const $provTypeSelect = $('#ddlProviderType');
        const provTypeVal = $provTypeSelect.val();
        const $provTypeError = $('#spanddlProviderType');

        if (!provTypeVal) {
            $provTypeError.text("* Provider Type is required.");
            $provTypeSelect.addClass('errMsgInput');
            valid = false;
        } else {
            $provTypeError.empty();
            $provTypeSelect.removeClass('errMsgInput');
        }

        const $firstNameInput = $('#txtFirstName');
        const firstNameVal = $firstNameInput.val().trim();
        const $firstNameError = $('#spantxtFirstName');

        if (!firstNameVal) {
            $firstNameError.text("* First Name is required.");
            $firstNameInput.addClass('errMsgInput');
            valid = false;
        } else {
            $firstNameError.empty();
            $firstNameInput.removeClass('errMsgInput');
        }

        const $midIntInput = $('#txtMidInt');
        const midIntVal = $midIntInput.val().trim();
        const $midIntError = $('#spantxtMidInt');
        const midIntRegex = /.*[a-zA-Z]+.*/;    /*regex check for at least one letter*/

        if (midIntVal && !midIntRegex.test(midIntVal)) {
            $midIntError.text("* Enter valid Middle Initial.");
            $midIntInput.addClass('errMsgInput');
            valid = false;
        } else {
            $midIntError.empty();
            $midIntInput.removeClass('errMsgInput');
        }

        const $lastNameInput = $('#txtLastName');
        const lastNameVal = $lastNameInput.val().trim();
        const $lastNameError = $('#spantxtLastName');

        if (!lastNameVal) {
            $lastNameError.text("* Last Name is required.");
            $lastNameInput.addClass('errMsgInput');
            valid = false;
        } else {
            $lastNameError.empty();
            $lastNameInput.removeClass('errMsgInput');
        }

        const $taxIDInput = $('#txtTaxID');
        const taxIDVal = $taxIDInput.val().trim();
        const $taxIDError = $('#spantxtTaxID');
        const taxIdRegex = /(?!078051120|219099999$)^(?!000|666)[0-8][0-9]{2}(?!00)[0-9]{2}(?!0000)[0-9]{4}$/;

        if (!taxIDVal) {
            $taxIDError.text("* Tax ID is required.");
            $taxIDInput.addClass('errMsgInput');
            valid = false;
        } else if (!taxIdRegex.test(taxIDVal)) {
            $taxIDError.text("* Enter a valid Tax ID.");
            $taxIDInput.addClass('errMsgInput');
            valid = false;
        } else {
            $taxIDError.empty();
            $taxIDInput.removeClass('errMsgInput');
        }

        const $npiInput = $('#txtNPI');
        const npiVal = $npiInput.val().trim();
        const $npiError = $('#spantxtNPI');

        if (!npiVal) {
            $npiError.text("* NPI is required.");
            $npiInput.addClass('errMsgInput');
            valid = false;
        } else if (!/^[1-9][0-9]{9}$/.test(npiVal)) {
            $npiError.text("* NPI must be a 10-digit number and cannot begin with 0.");
            $npiInput.addClass('errMsgInput');
            valid = false;
        } else {
            $npiError.empty();
            $npiInput.removeClass('errMsgInput');
        }

        const $dobInput = $('#txtDOB');
        const dobVal = $dobInput.val().trim();
        const $dobError = $('#spantxtDOB');

        if (!dobVal) {
            $dobError.text("* Date of Birth is required.");
            $dobInput.addClass('errMsgInput');
            valid = false;
        } else {
            // Check if the value is a valid date and not in the future
            const dobDate = new Date(dobVal);
            const today = new Date();
            // Remove time portion for accurate comparison
            dobDate.setHours(0, 0, 0, 0);
            today.setHours(0, 0, 0, 0);

            if (isNaN(dobDate.getTime())) {
                $dobError.text("* A valid Date of Birth is required (mm/dd/yyyy).");
                $dobInput.addClass('errMsgInput');
                valid = false;
            } else if (dobDate > today) {
                $dobError.text("* Date of Birth cannot be in the future.");
                $dobInput.addClass('errMsgInput');
                valid = false;
            } else {
                // Check if age is at least 18
                const ageDifMs = today - dobDate;
                const ageDate = new Date(ageDifMs);
                const age = Math.abs(ageDate.getUTCFullYear() - 1970);

                if (age < 18) {
                    $dobError.text("* Age must be 18 years or above.");
                    $dobInput.addClass('errMsgInput');
                    valid = false;
                } else {
                    $dobError.empty();
                    $dobInput.removeClass('errMsgInput');
                }
            }
        }

        const $genderSelect = $('#ddlGender');
        const genderVal = $genderSelect.val();
        const $genderError = $('#spanddlGender');

        if (!genderVal) {
            $genderError.text("* Gender is required.");
            $genderSelect.addClass('errMsgInput');
            valid = false;
        } else {
            $genderError.empty();
            $genderSelect.removeClass('errMsgInput');
        }

        if ($('#divAddressData').is(':visible')) {
            // divAddressData is visible validate the input fields
            /*console.log('divAddressData is visible');*/

            const $taxonomySelect = $('#ddlTaxonomy');
            const taxonomyVal = $taxonomySelect.val();
            const $taxonomyError = $('#spanddlTaxonomy');

            if (!taxonomyVal) {
                $taxonomyError.text("* Taxonomy is required.");
                $taxonomySelect.addClass('errMsgInput');
                valid = false;
            } else {
                $taxonomyError.empty();
                $taxonomySelect.removeClass('errMsgInput');
            }

            const $SpecialtySelect = $('#ddlSpecialty');
            const SpecialtyVal = $SpecialtySelect.val();
            const $SpecialtyError = $('#spanddlSpecialty');

            if (!SpecialtyVal) {
                $SpecialtyError.text("* Specialty is required.");
                $SpecialtySelect.addClass('errMsgInput');
                valid = false;
            } else {
                $SpecialtyError.empty();
                $SpecialtySelect.removeClass('errMsgInput');
            }

            const $adrContNameSelect = $('#txtContactName');
            const adrContNameVal = $adrContNameSelect.val();
            const $adrContNameError = $('#spantxtContactName');

            if (!adrContNameVal) {
                $adrContNameError.text("* Contact Name is required.");
                $adrContNameSelect.addClass('errMsgInput');
                valid = false;
            } else {
                $adrContNameError.empty();
                $adrContNameSelect.removeClass('errMsgInput');
            }

            const $addr1Select = $('#txtAddr1');
            const addr1Val = $addr1Select.val();
            const $addr1Error = $('#spantxtAddr1');

            if (!addr1Val) {
                $addr1Error.text("* Address Line 1 is required.");
                $addr1Select.addClass('errMsgInput');
                valid = false;
            } else {
                $addr1Error.empty();
                $addr1Select.removeClass('errMsgInput');
            }          

            var s = (addr1Val || '').toUpperCase();
            // remove dots and spaces to normalize variants like "P.O. B", "P O BOX", "PO. BOX"
            var normalized = s.replace(/[.\s]/g, '');
            if (normalized.indexOf('POB') !== -1 || normalized.indexOf('POBOX') !== -1 || s.trim().startsWith('BOX')) {
                // allow starts like: "POBOX", "POB", or "BOX"
                if (!(normalized.startsWith('POBOX') || normalized.startsWith('POB') || s.trim().startsWith('BOX'))) {
                    $addr1Error.text("* If the address is a PO Box, it must start with 'P.O. Box'");
                    $addr1Select.addClass('errMsgInput');
                    valid = false;
                }
                else {
                    $addr1Error.empty();
                    $addr1Select.removeClass('errMsgInput');
                }  
            }

            const $CitySelect = $('#txtCity');
            const CityVal = $CitySelect.val();
            const $CityError = $('#spantxtCity');

            if (!CityVal) {
                $CityError.text("* City is required.");
                $CitySelect.addClass('errMsgInput');
                valid = false;
            } else {
                $CityError.empty();
                $CitySelect.removeClass('errMsgInput');
            }

            const $stateSelect = $('#ddlState');
            const stateVal = $stateSelect.val();
            const $stateError = $('#spanddlState');

            if (!stateVal) {
                $stateError.text("* State is required.");
                $stateSelect.addClass('errMsgInput');
                valid = false;
            } else {
                $stateError.empty();
                $stateSelect.removeClass('errMsgInput');
            }

            const $countySelect = $('#ddlCounty');
            const countyVal = $countySelect.val();
            const $countyError = $('#spanddlCounty');

            if (!countyVal) {
                $countyError.text("* County is required.");
                $countySelect.addClass('errMsgInput');
                valid = false;
            } else {
                $countyError.empty();
                $countySelect.removeClass('errMsgInput');
            }

            const $zipSelect = $('#txtZip');
            const zipVal = $zipSelect.val();
            const $zipError = $('#spantxtZip');
            const zipRegex = /^(?!0{5})(?!9{5})\d{5}$/;

            if (!zipVal) {
                $zipError.text("* Enter Zip (First 5 digits)");
                $zipSelect.addClass('errMsgInput');
                valid = false;
            } else if (!zipRegex.test(zipVal)) {
                $zipError.text("* Enter 5 digits for the Zip (First 5)");
                $zipSelect.addClass('errMsgInput');
                valid = false;
            } else {
                $zipError.empty();
                $zipSelect.removeClass('errMsgInput');
            }

            const $extZipSelect = $('#txtExtZip');
            const extZipVal = $extZipSelect.val();
            const $extZipError = $('#spantxtExtZip');
            const extzipRegex = /^(?!0{4})(?!9{4})\d{4}$/;

            if (!extZipVal) {
                $extZipError.text("* Enter Zip Ext (Last 5 digits).");
                $extZipSelect.addClass('errMsgInput');
                valid = false;
            } else if (!extzipRegex.test(extZipVal)) {
                $extZipError.text("* Enter 4 digits for the Zip (Last 4)");
                $extZipSelect.addClass('errMsgInput');
                valid = false;
            } else {
                $extZipError.empty();
                $extZipSelect.removeClass('errMsgInput');
            }

            const $phoneSelect = $('#txtPhone');
            const phoneVal = $phoneSelect.val();
            const $phoneError = $('#spantxtPhone');

            if (!phoneVal) {
                $phoneError.text("* Phone Number is required.");
                $phoneSelect.addClass('errMsgInput');
                valid = false;
            } else if (!CheckPhoneLength(phoneVal)) {
                $phoneError.text("* Enter valid Phone Number");
                $phoneSelect.addClass('errMsgInput');
                valid = false;
            } else {
                $phoneError.empty();
                $phoneSelect.removeClass('errMsgInput');
            }

            const $emailSelect = $('#txtEmail');
            const emailVal = $emailSelect.val();
            const $emailError = $('#spantxtEmail');
            const emailRegex = /^[A-Z'a-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$/;
            if (!emailVal) {
                $emailError.text("* Email is required.");
                $emailSelect.addClass('errMsgInput');
                valid = false;
            } else if (!emailRegex.test(emailVal)) {
                $emailError.text("* Enter valid E-mail");
                $emailSelect.addClass('errMsgInput');
                valid = false;
            } else {
                $emailError.empty();
                $emailSelect.removeClass('errMsgInput');
            }

        } 

        return valid;
    }

    function CheckPhoneLength(phoneVal) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = phoneVal.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 10)
            return false;
        if (test[0] === 0 || test[0] === 1 || test[3] === 0 || test[3] === 1)
            return false;

        return true;
    }

    function PopulateCountyDropdown() {
        const APIToken = $("[id*=hdnAccessToken]").val();
        const userId = $('#<%=hdnUserName.ClientID%>').val();
        var stateIDVal = $('#ddlState').val();

        $.ajax({
            type: "POST",
            url: webApiEnrollment + `GetCountyByState?stateID=${stateIDVal}`,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (result) {
            
            if (!result || !result.length) return;            
            

            const ddlCounty = document.getElementById('ddlCounty');
            ddlCounty.innerHTML = '';
            const defaultOption = document.createElement('option');
            defaultOption.text = '';
            ddlCounty.add(defaultOption);
            for (var i = 0; i < result.length; i++) {
                const option = document.createElement('option');
                option.value = result[i].County_Value;
                option.text = result[i].County_Text;
                ddlCounty.add(option);
            }

        }).fail(function (jqXHR) {
            handleAjaxError(jqXHR, 'pnlShowErrorStream', 'Error Get County ');
        });
    }

    async function ValidateFromUSPS() {
        
        if (!ValidateFormInputData()) return false;    
        
        var overrideAddress = $('#chkOverrideAddressValidation').is(':checked') ? 1 : 0;

        if (overrideAddress === 0 && isAddrChangeConfirmed === 0) {
            document.getElementById('divpnlLoader').style.display = "block";
            const APIToken = $("[id*=hdnAccessToken]").val();
            const userId = $("[id*=hdnUserName]").val();

            const addrReq = {
                AddressLine: $('#txtAddr1').val().trim(),
                AddressLine2: $('#txtAddr2').val().trim(),
                City: $('#txtCity').val().trim(),
                State: $('#ddlState').val(),
                PostalCode: $('#txtZip').val().trim() + "-" + $('#txtExtZip').val().trim()
            };

            const result = await CommonAjaxCall({
                url: webApiEnrollment + "GetAddressVerificatonWSTiger",
                method: 'POST',
                data: addrReq,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                }
            });

                const $errorPanel = $('#pnlShowErrorStream').empty();

                if (!result) {
                    console.log('USPS Result null');
                    document.getElementById('divpnlLoader').style.display = "none";
                    const $modal = $("#modalAddrError");
                    $modal.show();
                    const $addrError = $('#paraWSError').empty();
                    const errorMsg = "Error Fetching Address Data.";
                    $addrError.html(errorMsg);
                    return;
                }
                console.log('USPS Result Found');
                var errCodesList = errorCodeStringToList(result && result.ErrorCodes ? result.ErrorCodes : '');

                var s = (result && result.ReturnCodes != null) ? String(result.ReturnCodes).trim() : '';

                if (!/^-?\d+$/.test(s)) {
                    document.getElementById('divpnlLoader').style.display = "none";
                    var errMsg = "Return code from address verification web service is invalid. Return code is: '" + result.ReturnCodes + "'";
                    $errorPanel.html('<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">' + errMsg + '</span><br /><br />');
                    return;
                }

                var returnCode = parseInt(s, 10);

                if (returnCode === 1) {
                    // success - no action required
                    $('#hdnLongitude').val(result.Longitude != null ? String(result.Longitude).trim() : '');
                    $('#hdnLatitude').val(result.Latitude != null ? String(result.Latitude).trim() : '');

                    const streetName = (result && result.StreetName) ? String(result.StreetName) : '';
                    const streetNumber = (result && result.StreetNumber) ? String(result.StreetNumber) : '';
                    const preDirectionalRaw = (result && result.PreDirectional) ? String(result.PreDirectional) : '';
                    const streetSuffix = (result && result.StreetSuffix) ? String(result.StreetSuffix) : '';

                    let newStreetAddress = '';

                    if (streetName.trim().toUpperCase() === 'PO BOX') {
                        newStreetAddress = streetName + (streetNumber ? ' ' + streetNumber : '');
                    } else {
                        const preDirectional = preDirectionalRaw === '' ? '' : preDirectionalRaw + ' ';
                            newStreetAddress = (streetNumber ? streetNumber + ' ' : '') + preDirectional + streetName + (streetSuffix ? ' ' + streetSuffix : '');
                        
                    }
                    newAddrLine1 = newStreetAddress;

                    const secondaryDesignation = (result && result.SecondaryDesignation) ? String(result.SecondaryDesignation) : '';
                    const secondaryNumber = (result && result.SecondaryNumber) ? String(result.SecondaryNumber) : '';

                    newAddrLine2 = secondaryDesignation + ' ' + secondaryNumber;
                    newAddrCity = result.City != null ? String(result.City).trim() : '';
                    newAddrState = result.State != null ? String(result.State).trim() : '';

                    
                    newAddrCountyName = result.CountyName != null ? String(result.CountyName).trim() : '';
                    newAddrCountyNumber = result.CountyNumber != null ? String(result.CountyNumber).trim() : '';
                    
                    var zipParts = (result.ZipAddon || "").split("-");

                    if (zipParts.length === 2) {
                        newAddrZip5 = zipParts[0];
                        newAddrZip4 = zipParts[1];
                    } else if (zipParts.length === 1) {
                        newAddrZip5 = zipParts[0];
                        newAddrZip4 = "";
                    } else {
                        newAddrZip5 = "";
                        newAddrZip4 = "";
                    }

                    document.getElementById('divpnlLoader').style.display = "none";
                    const $modal = $("#modalAddrConfirm");
                    $modal.show();
                    const $newAddr = $('#paraUSPSAddress').empty();
                    var newAddrMsg = "<br />" + escapeHtml(newAddrLine1) + "<br />" +
                        (newAddrLine2 != '' ? escapeHtml(newAddrLine2) + "<br />" : "") + (newAddrCountyName ? escapeHtml(newAddrCountyName) + "<br />" : "") +
                        escapeHtml(newAddrCity) + ", " + escapeHtml(newAddrState) + " " + escapeHtml(newAddrZip5) + (newAddrZip4 ? ("-" + escapeHtml(newAddrZip4)) : "");
                    $newAddr.html(newAddrMsg);

                } else if (returnCode > 1 || (returnCode < 0 && errCodesList.includes("11"))) {
                    document.getElementById('divpnlLoader').style.display = "none";
                    var errMsg = "Multiple possible addresses, but no exact match made. Number of possible addresses is '" + returnCode + "'";
                    $errorPanel.html('<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">' + errMsg + '</span><br /><br />');
                    return;
                } else if (returnCode === -99 || (returnCode === -1 && errCodesList.includes("07"))) {
                    document.getElementById('divpnlLoader').style.display = "none";
                    var errMsg = "Address validation failed. Could not find a valid destination for a mailing or package.";
                    $errorPanel.html('<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">' + errMsg + '</span><br /><br />');
                    return;
                } else if (returnCode === -3 && errCodesList.includes("05")) {
                    document.getElementById('divpnlLoader').style.display = "none";
                    var errMsg = "Street name normalized, but no matching address was found.";
                    $errorPanel.html('<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">' + errMsg + '</span><br /><br />');
                    return;
                } else {
                    document.getElementById('divpnlLoader').style.display = "none";
                    var errMsg = "Address not found. Details: '" + errorCodesToHTML(errCodesList) + "'";
                    $errorPanel.html('<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">' + errMsg + '</span><br /><br />');
                    return;
                }

        } else if (overrideAddress === 1) {
            $('#btnCreateReg').show();
        }


    }

    function errorCodeStringToList(errorCodesString) {
        var errorCodesList = [];
        if (!errorCodesString) return errorCodesList; // handle null/undefined/empty

        var errorCode = '';
        for (var i = 0; i < errorCodesString.length; i++) {
            errorCode += errorCodesString.charAt(i);
            if (errorCode.length === 2) {
                errorCodesList.push(errorCode);
                errorCode = '';
            }
        }

        return errorCodesList;
    }

    function errorCodesToHTML(errorCodes) {
        if (!Array.isArray(errorCodes) || errorCodes.length === 0) return '<ul></ul>';

        let html = '<ul>';
        for (let i = 0; i < errorCodes.length; i++) {
            const code = String(errorCodes[i]);
            const key = 'ADDR_ERROR_CODE_' + code;
            const msg = _errorResourceMap[key] || 'Unknown error code';
            html += '<li>' + escapeHtml(code + ': ' + msg) + '</li>';
        }
        html += '</ul>';
        return html;
    }

    async function handleCreateRegClick() {
        var regID = 0;
        regID = await CreateRegistration();

        setTimeout(function () {
            document.getElementById('divpnlLoader').style.display = "none";
            if (regID > 0) {
                console.log('Got regid' + regID);
                var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
                var redirectionUrl = urlpath + "/Registration.aspx?RegId=" + regID;
                window.location.href = redirectionUrl;
            }
            else {
                console.log('reg id 0 else');
                window.location.href = 'ProviderHomeNew.aspx';
            }
        }, 3000);        
       
    }

    async function CreateRegistration() {
        if (!ValidateFormInputData()) return false;
        document.getElementById('divpnlLoader').style.display = "block";
        var regId;
        const APIToken = $("[id*=hdnAccessToken]").val();
        const userId = $('#<%=hdnUserName.ClientID%>').val();

        var countyParts = ($('#ddlCounty').val() || "").split("-CountyName-");
        /*console.log($('#txtReqEffDate').val());*/
        const provRegRequest = {
            UserID: userId,
            SelectedProvAdminUserID: $('#<%=hdnSelectedProviderAdmin.ClientID%>').val(),
             FirstName: $('#txtFirstName').val().trim(),
             MiddleInitial: $('#txtMidInt').val().trim(),
             LastName: $('#txtLastName').val().trim(),
             Gender: $('#ddlGender').val(),
             BirthDate: $('#txtDOB').val(),
             TaxIDTypeID: $('input[name="idType"]:checked').val(),
             TaxID: $('#txtTaxID').val().trim(),
             NPI: $('#txtNPI').val().trim(),
             MMISProviderTypeID: $('#ddlProviderType').val(),
             RequestedEffectiveDate: $('#txtReqEffDate').val(),
             TaxonomyCode: $('#ddlTaxonomy').val(),
             TaxonomyName: $('#ddlTaxonomy option:selected').text(),
             SpecialtyTypeID: $('#ddlSpecialty').val(),
             AddressContactName: $('#txtContactName').val().trim(),
             AddressLine1: $('#txtAddr1').val().trim(),
             AddressLine2: $('#txtAddr2').val().trim(),
             City: $('#txtCity').val().trim(),
             State: $('#ddlState').val(),
             ZipCode: $('#txtZip').val().trim(),
             CountyName: $('#ddlCounty option:selected').text(),
             CountyCode: countyParts[0],
             PhoneNumber: $('#txtPhone').val().trim(),
             EmailAddress: $('#txtEmail').val().trim(),
             ZipExt: $('#txtExtZip').val().trim(),
             Longitude: $('#<%=hdnLongitude.ClientID%>').val(),
             Latitude: $('#<%=hdnLatitude.ClientID%>').val(),
             OverrideAddress: $('#chkOverrideAddressValidation').is(':checked') ? 1 : 0
         }

        const response = await CommonAjaxCall({
            url: webApiEnrollment + "CreateNewRegistrationStreamlined", 
            method: 'POST',
            data: provRegRequest,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            }
        });
        const $errorPanel = $('#pnlShowErrorStream').empty();

      
        if (!response) {
            document.getElementById('divpnlLoader').style.display = "none";
            $errorPanel.html(`<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">Error Creating new streamlined registration.</span><br /><br />`);
            return;
        }       
        
       
        if (response && response.ResponseCode === '200') {
            console.log('Reg id created is - ' + response.ResponseDesc);
            regId = response.ResponseDesc;
           /* document.getElementById('divpnlLoader').style.display = "none";*/
            //var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
            //var redirectionUrl = urlpath + "/Registration.aspx?RegId=" + regId;
            //window.location.href = redirectionUrl;
          
        } else {
            $errorPanel.html(`<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">${response.ResponseDesc}</span><br /><br />`);
        }

        return regId;
    }

    function CommonAjaxCall(options) {

        var defaults = {
            url: '',
            method: 'GET',
            data: {},
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            returnStatus: false,
            headers: {}, // Default empty headers
            success: function (response, textStatus, xhr) {
                console.log("Request succeeded:", response);
            },
            error: function (xhr, status, error) {
                console.error("Request failed:", error);
            }
        };

        var settings = $.extend({}, defaults, options);

        if (settings.method.toUpperCase() !== 'GET' && typeof settings.data === 'object') {
            settings.data = JSON.stringify(settings.data);
        }

        var dfd = $.Deferred();
        var jqXHRRequest = null;

        // Make AJAX request
        jqXHRRequest = $.ajax({
            url: settings.url,
            type: settings.method,
            data: settings.data,
            dataType: settings.dataType,
            contentType: settings.contentType,
            headers: settings.headers,
            success: function (response, textStatus, xhr) {
                settings.success(response, textStatus, xhr);
                if (settings.returnStatus && (response === undefined || response === null || response === '')) {
                    dfd.resolve({ status: xhr.status });
                } else {
                    dfd.resolve(response);
                }
            },
            error: function (xhr, status, error) {
                settings.error(xhr, status, error);
                dfd.reject(xhr, status, error);
            }
        });

        var promise = dfd.promise();
        promise.abort = function () {
            if (jqXHRRequest) {
                jqXHRRequest.abort();
            }
        };

        return promise;
    }   

    function handleAjaxError(jqXHR, errorElementId, messagePrefix = '', clearElementId) {
        const errorMsg = `${messagePrefix}${JSON.stringify(jqXHR)}`;
        const $errorElement = $(`#${errorElementId}`);
        if (jqXHR && jqXHR.status === 401) {
            $errorElement.text('Your token got expired. Kindly logout and login again.');
            if (clearElementId) {
                $(`#${clearElementId}`).empty();
            }
        } else {
            $errorElement.text(errorMsg);
            if (clearElementId) {
                $(`#${clearElementId}`).empty();
            }
        }
        console.log(errorMsg);
    }

    function btnConfirmWSErrorClick() {
        /*isAddrChangeConfirmed = 1;*/
        const $modal = $("#modalAddrError");
        $modal.hide();

        $('#btnCreateReg').show();       

        return false;
    }

    function ConfirmAddressChange() {
        isAddrChangeConfirmed = 1;
        const $modal = $("#modalAddrConfirm");
        $modal.hide();
        $('#txtAddr1').val(newAddrLine1);
        $('#txtAddr2').val(newAddrLine2);
        $('#txtAddrCity').val(newAddrCity);  
        $('#txtZip').val(newAddrZip5);
        $('#txtExtZip').val(newAddrZip4);

        if ($("#ddlState").first().val() !== newAddrState) {
            $("#ddlState").first().val(newAddrState);
            $("#ddlState").change();
        }
        $('#ddlState option:contains(' + newAddrState + ')').attr("selected", "selected");

        var county = (newAddrCountyName || '').toLowerCase();
        var $option = $('#ddlCounty option').filter(function () {
            return $(this).text().toLowerCase().indexOf(county) !== -1;
        }).first();
        if ($option.length) {
            $('#ddlCounty').val($option.val()).trigger('change');
        }
        
        SetFieldDisabledState(true);

        $('#btnCreateReg').show();
       

        return false;
    }

    function ClosePopup() {
        const $modal = $("#modalAddrConfirm");
        $modal.hide();
        $('#btnCreateReg').show();
    }

    function SetFieldDisabledState() {
        
        $('#txtFirstName').prop('disabled', true);
        $('#txtMidInt').prop('disabled', true);
        $('#txtLastName').prop('disabled', true);
        $('#txtNPI').prop('disabled', true);
        $('#ddlGender').prop('disabled', true);

        if ($('#divAddressData').is(':visible') && isAddrChangeConfirmed === 1) {
            $('#txtAddr1').prop('disabled', true);
            $('#txtAddr2').prop('disabled', true);
            $('#txtCity').prop('disabled', true);
            $('#ddlState').prop('disabled', true);
            $('#ddlCounty').prop('disabled', true);
            $('#txtZip').prop('disabled', true);
            $('#txtExtZip').prop('disabled', true);
        }
    }

    function CancelNewReg() {
        window.location.href = 'ProviderHomeNew.aspx';
    }

</script>


 <div class="container-fluid" style="min-height: 300px; font-size: 16pt !important;">
    <asp:HiddenField ID="hdnUserName" runat="server" />
    <asp:HiddenField ID="hdnSelectedProviderAdmin" runat="server" />
    <asp:HiddenField ID="hdnLongitude" runat="server" />
    <asp:HiddenField ID="hdnLatitude" runat="server" />
    
    <div id="div10DayMessage" style="width: 90%; margin: auto;">
        <p style="color: indigo; font-size: 24pt;">
            <h3>Please note that you have <span style="color: red">10 days to complete your application</span>. After 10 days, your information will be removed and you will
        have to re-start the process from the beginning of the application.</h3>

        </p>
    </div>
    <br />
    <div id="pnlShowErrorStream" style="overflow: auto; width: 100%; color: red; font-size: 16pt !important;"></div>
    <br />
    <div class="row">
       <div class="col-lg-12" style="border-bottom:2px solid #0074D9; padding-bottom:2px;">
        <span>Provider Key Identifiers - Entered values will populate throughout application</span>
       </div>
    </div>
     <br />
    <div class="row">
        <div class="col-sm-3 text-right">
            <label>Provider Type:</label>
        </div>
        <div class="col-sm-9 text-left">
            <select id="ddlProviderType" class="formDropDownMedium"></select>
            <small class="errMsg" id="spanddlProviderType"></small>
        </div>
    </div>
    <div class="row">
         <div class="col-sm-3 text-right">
             <label>First Name:</label>
         </div>
         <div class="col-sm-9 text-left">
             <input id="txtFirstName" type="text" class="formField" />
             <small class="errMsg" id="spantxtFirstName"></small>
         </div>
     </div>
     <div class="row">
         <div class="col-sm-3 text-right">
             <label>Middle Initial:</label>
         </div>
         <div class="col-sm-9 text-left">
             <input id="txtMidInt" type="text" class="formField" />
             <small class="errMsg" id="spantxtMidInt"></small>
         </div>
     </div>
     <div class="row">
        <div class="col-sm-3 text-right">
            <label>Last Name:</label>
        </div>
        <div class="col-sm-9 text-left">
            <input id="txtLastName" type="text" class="formField" />
            <small class="errMsg" id="spantxtLastName"></small>
        </div>
     </div>
     <div class="row">
        <div class="col-sm-3 text-right">
            <label>Tax ID Type:</label>
        </div>
        <div class="col-sm-9 text-left"  style="display:flex; gap:20px; align-items:center;">
             <label>
               <input type="radio" name="idType" value="16" disabled />
               EIN
             </label>
             <label>
               <input type="radio" name="idType" value="15" checked disabled />
               SSN
             </label>            
        </div>
     </div>
     <div class="row">
        <div class="col-sm-3 text-right">
            <label>Tax ID:</label>
        </div>
        <div class="col-sm-9 text-left">
            <input id="txtTaxID" type="text" class="formField" maxlength="9" />
            <small class="errMsg" id="spantxtTaxID"></small>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right">
            <label>NPI:</label>
        </div>
        <div class="col-sm-9 text-left">
            <input id="txtNPI" type="text" class="formField" />
            <small class="errMsg" id="spantxtNPI"></small>
        </div>
    </div>
    <div class="row">
         <div class="col-sm-3 text-right">
             <label>Requested Effective Date:</label>
         </div>
         <div class="col-sm-9 text-left">
             <input id="txtReqEffDate" type="date" class="formField" /> 
             <%--<span id="helpTaxID" class="help-taxid" style="cursor: pointer; display: inline-block;">What is this  
                  <asp:Image ID="imgHelpTaxID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" ToolTip="<%$ Resources:BrandingResource , REQUESTED_EFFECTIVE_DATE_HELPTEXT %>"/></span>  --%>          
         </div>
     </div>
     <div class="row">
         <div class="col-sm-3 text-right">
             <label>Gender:</label>
         </div>
         <div class="col-sm-9 text-left">
             <select id="ddlGender" class="formDropDownMedium"></select>
             <small class="errMsg" id="spanddlGender"></small>
         </div>
     </div>
     <div class="row">
        <div class="col-sm-3 text-right">
            <label>DateofBirth:</label>
        </div>
        <div class="col-sm-9 text-left">
            <input id="txtDOB" type="date" class="formField" />
            <small class="errMsg" id="spantxtDOB"></small>
        </div>
    </div>
    <div id="divAddressData">
        <div class="row">
             <div class="col-sm-3 text-right">
                 <label>Taxonomy:</label>
             </div>
             <div class="col-sm-9 text-left">
                 <select id="ddlTaxonomy" class="formDropDownMedium"></select> 
                 <small class="errMsg" id="spanddlTaxonomy"></small>
             </div>
         </div>
         <div class="row">
              <div class="col-sm-3 text-right">
                  <label>Specialty:</label>
              </div>
              <div class="col-sm-9 text-left">
                  <select id="ddlSpecialty" class="formDropDownMedium"></select> 
                  <small class="errMsg" id="spanddlSpecialty"></small>
              </div>
          </div> 
        <br />
         <div class="row">
            <div class="col-lg-12" style="border-bottom:2px solid #0074D9; padding-bottom:2px;">
            <span>Address Fields - Applied to all addresses</span>
            </div>
         </div>
         <br />
         <div class="row">
            <div class="col-sm-3 text-right">
                <label for="chkOverrideAddressValidation">Override Address Validation:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input type="checkbox" id="chkOverrideAddressValidation" />
                <label for="chkOverrideAddressValidation" style="margin-left:8px;"></label>
            </div>
        </div>
         <div class="row">
            <div class="col-sm-3 text-right">
                <label>Contact Name:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input id="txtContactName" type="text" class="formField" /> 
                <small class="errMsg" id="spantxtContactName"></small>
            </div>
         </div>
         <div class="row">
            <div class="col-sm-3 text-right">
                <label>Address 1:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input id="txtAddr1" type="text" class="formField" maxlength="60" /> 
                <small class="errMsg" id="spantxtAddr1"></small>
            </div>
         </div>
         <div class="row">
            <div class="col-sm-3 text-right">
                <label>Address 2:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input id="txtAddr2" type="text" class="formField" maxlength="60" /> 
                <small class="errMsg" id="spantxtAddr2"></small>
            </div>
         </div>
         <div class="row">
           <div class="col-sm-3 text-right">
               <label>City:</label>
           </div>
           <div class="col-sm-9 text-left">
               <input id="txtCity" type="text" class="formField" maxlength="30" /> 
               <small class="errMsg" id="spantxtCity"></small>
           </div>
         </div>
         <div class="row">
            <div class="col-sm-3 text-right">
                <label>State:</label>
            </div>
            <div class="col-sm-9 text-left">
                <select id="ddlState" class="formDropDownMedium" onchange="PopulateCountyDropdown()"></select>
                <small class="errMsg" id="spanddlState"></small>
            </div>
         </div>
         <div class="row">
            <div class="col-sm-3 text-right">
                <label>County:</label>
            </div>
            <div class="col-sm-9 text-left">
                <select id="ddlCounty" class="formDropDownMedium"></select>
                <small class="errMsg" id="spanddlCounty"></small>
            </div>
         </div>
         <div class="row">
            <div class="col-sm-3 text-right">
                <label>Zip:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input id="txtZip" type="text" class="formField" maxlength="5" /> 
                <small class="errMsg" id="spantxtZip"></small>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right">
                <label>Ext Zip:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input id="txtExtZip" type="text" class="formField" maxlength="4" /> 
                <small class="errMsg" id="spantxtExtZip"></small>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right">
                <label>Phone:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input id="txtPhone" type="text" class="formField" /> 
                <small class="errMsg" id="spantxtPhone"></small>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right">
                <label>Email:</label>
            </div>
            <div class="col-sm-9 text-left">
                <input id="txtEmail" type="text" class="formField" /> 
                <small class="errMsg" id="spantxtEmail"></small>
            </div>
        </div>

         <div class="row">
            <div class="col-sm-6 text-center" style="padding: 28px 0 10px 0">
                 <button id="btnUSPS" type="button" onclick="ValidateFromUSPS()" class="buttonBoxFocus">USPS</button>
            </div>
         </div>         
     </div>
      <div class="row">
           <div class="col-sm-12 text-center btnBox btnBoxCenter" style="padding: 28px 0 10px 0">
                <button id="btnCreateReg" type="button" onclick="handleCreateRegClick()" class="buttonBoxFocus" style="display:none">Continue to Main Application</button>
                <button id="btnCancelReg" type="button" onclick="CancelNewReg()" class="buttonBoxFocusRed3B">Cancel</button>
           </div>
        </div>
       <!-- Modal pop up content for Address Confirm-->
     <div id="modalAddrConfirm" class="modal1"> 
       <div class="modal-content">
           <header style="cursor: move; padding: 5px; background-color:#205794; text-align: left; color:white; height:40px;">
                 Confirmation
                 <span id="spn2closeModal" class="close" style="color:white;" onclick ="ClosePopup()">&times;</span>
            </header>
           <br /> <br />
           <div runat="server" id="divConfirmAddress" style="text-align: center">
                <p style="color: darkgreen">
                    According to the USPS database, the address entered is inaccurate. The following address was found:
                </p>
                <p style="color: darkgreen" id="paraUSPSAddress"></p>
                <p style="color: darkgreen">Click on 'Accept' to accept the corrections.</p>
                <br />
                <button id="btnConfirmAddress" type="button" onclick="ConfirmAddressChange()" class="buttonBoxFocus">Accept</button>
                <button id="btnCancelAddressCorrection" type="button" onclick="ClosePopup()" class="buttonBoxFocus">Cancel</button>
            </div>

       </div>
    </div>
        <!-- Modal pop up content for Address Error-->
     <div id="modalAddrError" class="modal1"> 
       <div class="modal-content">
            <header style="cursor: move; padding: 5px; background-color:#205794; text-align: left; color:white; height:40px;">
                 Error
                 <span id="spn2closeModal" class="close" style="color:white;" onclick ="ClosePopup()">&times;</span>
             </header>
            <br /> <br />
          <div runat="server" id="divWSError" style="text-align: left">
            <p style="color: darkgreen">
                An error occurred while validating the entered address.<br />
                You can continue to work, but any addresses will not be validated by the system.<br />
                <br />
                Error details:<br />
            </p>
            <p style="color: darkgreen" id="paraWSError"></p>
            <button id="btnConfirmWSError" type="button" onclick="btnConfirmWSErrorClick(); return false;" class="buttonBoxFocus">Ok</button>
          </div>

       </div>
    </div>

 </div>
</asp:Content>
