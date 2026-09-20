<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ORProviderSearch" Codebehind="ORProviderSearch.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>

<script src="../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery.loadTemplate.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
<script src="../Scripts/bootstrap.min.js"></script>

<link href="../Content/custom-style.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<link href="<%# Page.ResolveClientUrl("~/App_Themes/Modernization/Modern.css") %>" rel="stylesheet" />

<style type="text/css">
    .cttile {
        display: flex
    }

    .orpTitle {
        display: flex;
        background: #7993ac !important;
        padding: 5px 5px;
        font-size: 25px;
        align-items: center;
        width: 100%;
        justify-content: space-between
    }

        .orpTitle span {
            font-size: 40px;
            line-height: 1;
            color: #fff
        }

        .orpTitle h3 {
            margin: 0px;
            padding-left: 8px;
            color: #fff;
            padding-top: 4px;
            font-size:25px !important;
        }

        .orpTitle .plus {
            display: none
        }

        .orpTitle.active .minus {
            display: none
        }

        .orpTitle.active .plus {
            display: block
        }

    .csresult1 .plus {
        display: block !important
    }

    .csresult1 .minus {
        display: none !important
    }

    .csresult1.active .minus {
        display: block !important
    }

    .csresult1.active .plus {
        display: none !important
    }

    .csTable thead tr {
        background: #435363;
        color: #ffffff;
    }

    .csTable tbody tr {
        background: #fff
    }


    .cForm input, .cForm select {
        height: 44px;
        line-height: 1;
        font-size: 17px;
        background-color: #FFF;
    }

    .csresult2 .plus {
        display: block !important
    }

    .csresult2 .minus {
        display: none !important
    }

    .csresult2.active .minus {
        display: block !important
    }

    .csresult2.active .plus {
        display: none !important
    }

    .cformContainer, .resultContainer1 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer .cForm {
            padding: 20px
        }

        .resultContainer1 #pnlClaimSearchResult1 {
            padding: 20px
        }

        .cformContainer.active, .resultContainer1.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer2 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }


        .resultContainer2 #pnlClaimSearchResult1 {
            padding: 20px
        }

        .cformContainer.active, .resultContainer2.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .mebtn {
        width: 180px;
        font-size: 20px;
        background-color: #435363;
        /* color: #ffffff*/
    }

    .clearme {
        width: 180px;
        font-size: 20px;
        background-color: #ccc;
    }

    .errMsgInput {
        border: 1px solid #f00 !important
    }

    .errMsg {
        color: #f00;
        position: absolute;
        display: block;
        text-align: right;
        width: 95%;
    }
</style>

<style type="text/css">
    label {
        display: inline-block;
        margin-bottom: 5px;
        font-weight: bold;
    }

    .form-control {
        display: block;
        width: 100%;
        height: 34px;
        padding: 6px 12px;
        font-size: 14px;
        line-height: 1.428571429;
        color: #555555;
        vertical-align: middle;
        background-color: #ffffff;
        border: 1px solid #cccccc;
        border-radius: 4px;
        -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
        box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
        -webkit-transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
        transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
    }

    .form-horizontal .control-label, .form-horizontal .radio, .form-horizontal .checkbox, .form-horizontal .radio-inline, .form-horizontal .checkbox-inline {
        padding-top: 7px;
        margin-top: 0;
        margin-bottom: 0;
    }
</style>

<style type="text/css">



    .formField {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 274px;
    }

    select {
        min-width: 273px;
        height: 41px;
        border: 1px solid #ccc;
    }

    .formLabel200 {
        width: 178px;
    }

    @media only screen and (max-width: 1200px) {
        #fakeDiv1, #fakeDiv2 {
            display: none;
        }
    }

    .container {
        width: 605px;
    }

    .cssPager td {
        padding-left: 4px;
        padding-right: 4px;
    }

    input[type=number]::-webkit-inner-spin-button,
    input[type=number]::-webkit-outer-spin-button {
        -webkit-appearance: none;
        -moz-appearance: none;
        appearance: none;
        margin: 0;
    }

    .hidden {
        display: none;
    }

    .icon-rtl {
      background: url("../Images/Calendaricon.png") no-repeat right;
      background-size: 20px;
      background-origin: content-box;
    }
</style>

<style type="text/css">
    .pagination {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination li.disabled {
                color: gray;
                cursor: not-allowed;
            }

    .cursor {
        cursor: pointer;
    }

    .sort-icon {
        margin-left: 5px;
    }

    #pagination {
        text-align: right
    }

        #pagination .mybtnpage {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination .mybtnpage:hover, #pagination .mybtnpage[style] {
                color: #187ed5
            }

        #pagination #btnPrev, #pagination #btnNext {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination #btnPrev:hover, #pagination #btnNext:hover {
                color: #187ed5
            }

    #divpnlLoader {
        text-align: center
    }

    .csTable th {
        border: 1px solid #f9f9f9
    }

    table.csTable td {
        border: 1px solid #e5e5e5 !important
    }

    table.csTable tr:nth-child(even) {
        background: #f2f9fd !important
    }

    .errMsgInput {
        border: 1px solid #f00 !important
    }

    .errMsg {
        color: #f00;
        position: absolute;
        display: block;
        text-align: right;
        width: 95%;
    }

    select {
        min-width: inherit !important
    }

</style>


<script type="text/javascript">
    let currentPage = 1;
    let ORPPanelLet;
    const pageSize = 10;
</script>

<script type="text/javascript">

    $(document).ready(function () {
        $('[data-toggle="popover"]').popover();
    });

    $(function () {
        $("#txtDOS").datepicker();
    });

    function formatDate(date) {
        var d = new Date(date + 'EST'),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }

    function makeclear() {
        document.getElementById('txtNPI').value = '';
        document.getElementById('txtDOS').value = '';

        document.getElementById('spantxtNPI').innerHTML = '';
        document.getElementById('spantxtDOS').innerHTML = '';

        document.getElementById('pnlORPSearchResult').innerHTML = '';
        document.getElementById('pagination').innerHTML = '';

        document.getElementById('txtNPI').classList.remove('errMsgInput');
        document.getElementById('txtDOS').classList.remove('errMsgInput');

        document.getElementById('pnlORPErrResults').innerHTML = '';

        $('.resultContainer2').removeClass("active");
        $('.csresult2').removeClass("active");
    }

    function clearErrorMessages() {
        document.getElementById('spantxtNPI').innerHTML = '';
        document.getElementById('spantxtDOS').innerHTML = '';

        document.getElementById('pnlORPSearchResult').innerHTML = '';
        document.getElementById('pagination').innerHTML = '';

        document.getElementById('txtNPI').classList.remove('errMsgInput');
        document.getElementById('txtDOS').classList.remove('errMsgInput');

        document.getElementById('pnlORPErrResults').innerHTML = '';

        $('.resultContainer2').removeClass("active");
        $('.csresult2').removeClass("active");
    }

    function validateFields() {
        clearErrorMessages();
        var message = '';
        var txtNPIVal = document.getElementById('txtNPI').value;
        var txtDOSVal = document.getElementById('txtDOS').value;
        var valid = true;

        if (txtNPIVal != null && txtNPIVal != '') {
            if (txtNPIVal.length < 10 || txtNPIVal.length > 10) {
                message = "* 10-digits number required.";
                document.getElementById('txtNPI').classList.add('errMsgInput');
                document.getElementById('spantxtNPI').innerHTML = message;
                valid = false;
            }
        }

        if (txtNPIVal == null || txtNPIVal == '') {
            message = "* Please Enter NPI.";
            document.getElementById('txtNPI').classList.add('errMsgInput');
            document.getElementById('spantxtNPI').innerHTML = message;
            valid = false;
        }

        if (txtDOSVal == null || txtDOSVal == '') {
            message = "* Please Enter Date of Service.";
            document.getElementById('txtDOS').classList.add('errMsgInput');
            document.getElementById('spantxtDOS').innerHTML = message;
            valid = false;
        }
        if (!isValidDate(txtDOSVal)) {
            message = "* Please enter valid Date of Service.";
            document.getElementById('txtDOS').classList.add('errMsgInput');
            document.getElementById('spantxtDOS').innerHTML = message;
            valid = false;
        }

        return valid;
    }

    // Validates that the input string is a valid date formatted as "mm/dd/yyyy"
    function isValidDate(dateString) {
        // First check for the pattern
        if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
            return false;

        // Parse the date parts to integers
        var parts = dateString.split("/");
        var day = parseInt(parts[1], 10);
        var month = parseInt(parts[0], 10);
        var year = parseInt(parts[2], 10);

        // Check the ranges of month and year
        if (year < 1000 || year > 3000 || month == 0 || month > 12)
            return false;

        var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        // Adjust for leap years
        if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
            monthLength[1] = 29;

        // Check the range of the day
        return day > 0 && day <= monthLength[month - 1];
    };

    function makeORPSearch() {

        if (!validateFields()) {
            return false;
        }

        var APIToken = $("[id*=hdnAccessToken]").val();

        var message = '';
        var txtNPIVal = document.getElementById('txtNPI').value;
        var txtDOSVal = document.getElementById('txtDOS').value;

        var orpSearchParams = {
            ProviderNPI: txtNPIVal,
            DOS: formatDate(txtDOSVal)
        }
        var inputData = JSON.stringify(orpSearchParams);

        $.ajax({
            type: "POST",
            url: webApiEnrollment + "FetchORPProviderSearchResults",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: inputData,
            success: function (result) {
                console.log(result);
                if (result != null) {
                    try {
                        var ORPSearchDisplay = result.ORPSearchDisplay;
                        if (result.ResponseCode == '200' && ORPSearchDisplay != null && ORPSearchDisplay != '') {
                            ORPPanelLet = ORPSearchDisplay;
                            var ORPSearchDisplaySliced = ORPPanelLet.slice(0, pageSize);
                            var totalPages = Math.ceil(ORPPanelLet.length / pageSize);

                            var table = "<table id='paneltbl' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable(0)'>NPI <span id='icon-0' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(1)'>Provider Name <span id='icon-1' class='sort-icon'>&#8597;</span></th></tr></thead><tbody>";
                            for (var i = 0; i < ORPSearchDisplaySliced.length; i++) {

                                var NPIDisplay = '';
                                var ProviderNameDisplay = '';

                                if (ORPSearchDisplaySliced[i].ProviderNPI != null) {
                                    NPIDisplay = ORPSearchDisplaySliced[i].ProviderNPI;
                                }
                                if (ORPSearchDisplaySliced[i].ProviderName != null) {
                                    ProviderNameDisplay = ORPSearchDisplaySliced[i].ProviderName;
                                }

                                table = table + "<tr><td>" + NPIDisplay + "</td><td>" + ProviderNameDisplay + "</td></tr>";
                            }

                            table = table + "</tbody></table>";
                            document.getElementById('pnlORPSearchResult').innerHTML = '';
                            document.getElementById('pnlORPSearchResult').innerHTML = table;
                            setupPagination(totalPages, 1);

                            $('.resultContainer2').toggleClass("active");
                            $('.csresult2').toggleClass("active");
                        } else if (result.ResponseCode == '300' || result.ResponseCode == '301' || result.ResponseCode == '302') {
                            document.getElementById('pnlORPSearchResult').innerHTML = '';
                            document.getElementById('pnlORPSearchResult').setAttribute('style', 'text-align: center;');
                            document.getElementById('pnlORPSearchResult').innerHTML = '<br /><span tabindex="0" style="color: #CC0505; font-size: 16pt !important; padding-left: 10px; font-weight: 100 !important;">' + result.ResponseDescp + '</span><br /><br />';

                            $('.resultContainer2').toggleClass("active");
                            $('.csresult2').toggleClass("active");
                        } else {
                            document.getElementById('pnlORPErrResults').innerHTML = '';
                            document.getElementById('pnlORPErrResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error: ' + result.ResponseCode + ' : ' + result.ResponseDescp + '</span>';
                        }
                    }
                    catch (err) {
                        document.getElementById('pnlORPErrResults').innerHTML = '';
                        document.getElementById('pnlORPErrResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error : ' + err + '</span>';
                        console.log(err);
                    }
                }



            },
            error: function (jqXHR, textStatus, errorThrown) {
                var status = jqXHR.status;
                document.getElementById('pnlORPErrResults').innerHTML = '';
                document.getElementById('pnlORPErrResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error : ' + status + ' No data returned</span>';
                console.log(JSON.stringify(jqXHR));
            }
        });
    }



    function setupPagination(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev' onclick='PreviousClick(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage_" + i + "' class='mybtnpage' type='button' onclick='btnPaging_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext' onclick='NextClick(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev = document.getElementById('btnPrev');

        if (currentPage === 1) {
            $("#btnPrev").prop('disabled', true);
        }
        else {
            $("#btnPrev").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext").prop('disabled', true);
        }
        else {
            $("#btnNext").prop('disabled', false);
        }
    }


    function sortTable(n) {
        var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
        table = document.getElementById("paneltbl");
        switching = true;
        dir = "asc"; // Set the sorting direction to ascending

        // Loop to keep switching until no switching is needed
        while (switching) {
            switching = false;
            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {
                shouldSwitch = false;
                x = rows[i].getElementsByTagName("TD")[n];
                y = rows[i + 1].getElementsByTagName("TD")[n];

                if (dir == "asc") {
                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                        shouldSwitch = true;
                        break;
                    }
                } else if (dir == "desc") {
                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                        shouldSwitch = true;
                        break;
                    }
                }
            }
            if (shouldSwitch) {
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
                switchcount++;
            } else {
                if (switchcount == 0 && dir == "asc") {
                    dir = "desc";
                    switching = true;
                }
            }
        }

        updateIcons(n, dir);
    }

    function updateIcons(colIndex, direction) {

        const icons = document.querySelectorAll('.sort-icon');
        icons.forEach(icon => icon.innerHTML = '&#8597;');
        const icon = document.getElementById(`icon-${colIndex}`);
        if (direction === 'asc') {
            icon.innerHTML = '&#8593;'; // Up arrow
        } else {
            icon.innerHTML = '&#8595;'; // Down arrow
        }
    }

    function resultToggle1() {
        $('.resultContainer1').toggleClass("active");
        $('.csresult1').toggleClass("active");
    }
    function resultToggle2() {
        $('.resultContainer2').toggleClass("active");
        $('.csresult2').toggleClass("active");
    }
</script>

<asp:HiddenField ID="hdnUserName" runat="server" />

<!-- #region ORP SEARCH -->
<div class="orpTitle csresult1" onclick="resultToggle1()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">Ordering / Referring / Prescribing Search</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer1 active">
    <div class="content cForm" style="padding-top: 15px">
        <div id="pnlORPErrResults" style="overflow: auto; width: 100%"></div>
        <div><span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">An asterisk * indicates a required field</span></div>
        
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtNPI"><span style="color: red;">*</span> NPI <span data-toggle="popover" data-trigger="hover"  title="HelpText" data-content="Return results will only display providers active on the Date of Service entered."><img src="../Images/Infoicon.png" alt="Help Text" style="vertical-align: inherit;" Height="13" Width="13" AlternateText="INFO"></span></label>
                <input class="form-control" id="txtNPI" name="NPI" type="number" value="" max="9999999999" maxlength="10" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" />
                <small class="errMsg" id="spantxtNPI"></small>
            </div>
        </div>
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtDOS"><span style="color: red;">*</span> Date of Service <span data-toggle="popover" data-trigger="hover"  title="HelpText" data-content="Start date of billing range."><img src="../Images/Infoicon.png" alt="Help Text" style="vertical-align: inherit;" Height="13" Width="13" AlternateText="INFO"></span></label>
                <input id="txtDOS" name="Date of Service" type="text" class="icon-rtl form-control" placeholder="mm/dd/yyyy" maxlength="10" />
                <small class="errMsg" id="spantxtDOS"></small>
            </div>
        </div>
        <div class="row">
            <div class="row-centered col-lg-12" style="padding-top: 30px !important; padding-bottom:10px !important; text-align: center; padding-bottom: 10px;">
                <button id="btnSearch" type="button" class="buttonBoxFocusBlue"  onclick="makeORPSearch()">Search</button>
                <button id="btnclear" type="button" class="buttonBoxFocusred" onclick="makeclear()">Clear</button>
            </div>
        </div>
    </div>
</div>
<!-- #endregion -->

<div class="orpTitle csresult2" onclick="resultToggle2()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">Search Results</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer2">
    <div id="divpnlLoader" style="display: none">
        <img src='../Images/loader.gif' style='height: 38px; width: 35px;'>
    </div>
    <div id="pnlORPSearchResult" style="overflow: auto; width: 100%;"></div>
    <div id="pagination"></div>
</div>