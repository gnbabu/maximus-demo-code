<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SearchClaims" Codebehind="SearchClaims.ascx.cs" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register assembly="SCS.WebControls.GroupBox" namespace="SCS.WebControls" tagprefix="cc1" %>

<script src="../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery.loadTemplate.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui.js" type="text/javascript"></script>


<script src="../Scripts/bootstrap.min.js"></script>

<link href="../Content/custom-style.css" rel="stylesheet" />

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<link href="<%# Page.ResolveClientUrl("~/App_Themes/Modernization/Modern.css") %>" rel="stylesheet" />
<style>
    .claimTitle {
        display: flex;
        background: #7993ac !important;
        padding: 5px 5px;
        font-size: 32px;
        align-items: center;
        width: 100%;
        justify-content: space-between
    }

    #paginationContainer {
        width: 100%;
    }

    #pagination {
        display: flex;
        align-items: baseline;
        width: 424px;
        margin-left: auto;
        margin-right: 16px;
        justify-content: end;
    }

    .pagenumber {
        max-width: 300px;
        display: flex;
        flex-wrap: nowrap;
        overflow: auto;
    }

        .pagenumber::-webkit-scrollbar {
            width: 4px;
        }

        /* Track */
        .pagenumber::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        /* Handle */
        .pagenumber::-webkit-scrollbar-thumb {
            background: #888;
        }

            /* Handle on hover */
            .pagenumber::-webkit-scrollbar-thumb:hover {
                background: #555;
            }

    .claimTitle span {
        font-size: 40px;
        line-height: 1;
        color: #fff
    }

    .cttile {
        display: flex
    }

    .claimTitle h3 {
        margin: 0px;
        padding-left: 8px;
        color: #fff;
        padding-top: 4px;
    }

    .claimTitle .plus {
        display: none
    }

    .claimTitle.active .minus {
        display: none
    }

    .claimTitle.active .plus {
        display: block
    }

    .cForm input, .cForm select {
        height: 44px;
        line-height: 1;
        font-size: 17px;
    }

    .maxclaim {
        display: flex;
        align-items: center;
        padding-top: 24px;
    }

    .claimbtn {
        width: 180px;
        font-size: 20px;
        background-color: #435363;
        /* color: #ffffff*/
    }

    .clearclaim {
        background-color: #ccc;
        /*  color: #545487*/
    }

    .csresult .plus {
        display: block !important
    }

    .csresult .minus {
        display: none !important
    }

    .csresult.active .minus {
        display: block !important
    }

    .csresult.active .plus {
        display: none !important
    }

    .csTable thead tr {
        background: #435363;
        color: #ffffff;
    }

    .csTable tbody tr {
        background: #fff
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
</style>

<style>
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

<style>
    .gridViewHeader a, .gridViewHeader a:link, .gridViewHeader a:active, .gridViewHeader a:hover, .gridViewHeader a:visited,
    .gridViewHeader th a, .gridViewHeader th a:link, .gridViewHeader th a:active, .gridViewHeader th a:hover, .gridViewHeader th a:visited,
    .rgHeader a, .rgHeader a:link, .rgHeader a:active, .rgHeader a:hover, .rgHeader a:visited,
    .rgHeader th a, .rgHeader th a:link, .rgHeader th a:active, .rgHeader th a:hover, .rgHeader th a:visited {
        color: #222222 !important;
        text-align: left;
    }

    .paging-nav {
        text-align: right;
        padding-top: 2px;
    }

        .paging-nav a {
            margin: auto 1px;
            text-decoration: none;
            display: inline-block;
            padding: 1px 7px;
            background: #91b9e6;
            color: white;
            border-radius: 3px;
        }

        .paging-nav .selected-page {
            background: #187ed5;
            font-weight: bold;
        }

    .paging-nav,
    #tableData {
        width: 400px;
        margin: 0 auto;
        font-family: Arial, sans-serif;
    }

    .cformContainer, .resultContainer {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer .cForm {
            padding: 20px
        }

        .resultContainer #pnlClaimSearchResult1 {
            padding: 20px
        }

        .cformContainer.active, .resultContainer.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }
</style>

<script type="text/javascript">

    let currentPage = 1;
    const pageSize = 5;
    $(document).ready(function () {
        bindClaimStatus();
    });

</script>

<script type="text/javascript">

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    function btnSearch_Click1() {
        var isDatabaseSearch = document.getElementById("ddlClaimStatus").value;
        if (isDatabaseSearch != "1") {
            var selectedOption = document.getElementById("ddlMangedCarePlan").value;
            if (selectedOption == "") {
                document.getElementById('ddlMangedCarePlan').classList.add('errMsgInput');
                document.getElementById('spanddlMangedCarePlan').innerHTML = "* Payor Name is Required";
                return false;
            }
            else {
                document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
                document.getElementById('spanddlMangedCarePlan').innerHTML = "";
            }
        } else {
            document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
            document.getElementById('spanddlMangedCarePlan').innerHTML = "";
        }
        const d = new Date();
        d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = 'showResults=true;';


    }

    function btnSearch_Click() {

        resultToggleShow();

        var st = validateBillingNumber();

        if (!validateICNNumber()) {
            return false;
        }

        if (!validateBillingNumber()) {
            return false;
        }
        if (!validateAccountNumber()) {
            return false;
        }
        if (!validateRenderingProvider()) {
            return false;
        }
        if (!validateAmountBilled()) {
            return false;
        }
        if (!validatePrescriptionNumber()) {
            return false;
        }

        document.getElementById('divpnlLoader').style.display = "block";

        $("#btnSearch").prop('disabled', true);

        var isDatabaseSearch = document.getElementById("ddlClaimStatus").value;
        if (isDatabaseSearch != "1") {
            var selectedOption = document.getElementById("ddlMangedCarePlan").value;
            if (selectedOption == "") {

                document.getElementById('ddlMangedCarePlan').classList.add('errMsgInput');
                document.getElementById('spanddlMangedCarePlan').innerHTML = "* Payor Name is Required";
                document.getElementById('divpnlLoader').style.display = "none";

                $("#btnSearch").prop('disabled', false);
                return false; // Prevent form submission
            }
            else {
                document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
                document.getElementById('spanddlMangedCarePlan').innerHTML = "";
            }
        } else {
            document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
            document.getElementById('spanddlMangedCarePlan').innerHTML = "";
        }
        const d = new Date();
        d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = 'showResults=true;';
        ClaimsSearch(null);





    }

    function isValidDate(dateString) {
        var date = new Date(dateString);
        return !isNaN(date.getTime());
    }

    function validateFDOSTDOSDates(txtReqFDOS, txtReqTDOS) {

        var fdos = new Date(txtReqFDOS);
        var tdos = new Date(txtReqTDOS);
        var current = new Date();
        const currentDate48months = current.setMonth(-48);
        var date48months = new Date(currentDate48months);
        var message = '';
        if (txtReqFDOS != null && txtReqFDOS != '') {

            if (tdos == null || tdos == '') {
                message = "* Date of Service (To) must exist if there is a Date of Service (From) date";
                return message;
            }
            else if (!isValidDate(fdos)) {
                message = "* Date of Service (From) is invalid";
                return message;
            }
            else if (!isValidDate(tdos)) {
                message = "* Date of Service (To) is invalid";
                return message;
            }
            else if (new Date(fdos) > new Date()) {
                message = "* Date of Service (From) cannot be future date";
                return message;
            }
            else if (new Date(tdos) > new Date()) {
                message = "* Date of Service (To) cannot be future date";
                return message;
            }
            else if (fdos > tdos) {
                message = "Todos is must be greater than FromDOS";
                return message;
            }
            else if (fdos < date48months) {
                message = "* System Allow up to 48 months back";
                return message;
            }
        }
        return message;
    }

    function prepareClaimSearchPayload() {

        var claimStatus = '';
        var TotalChargesSpecified = false;
        let totalAmount = 0;

        var RemittanceAdviceDateSpecified = false;
        var allowdbSearch = false;
        var ddlMangedCarePlan = document.getElementById("ddlMangedCarePlan").value;
        var isDatabaseSearch = document.getElementById("ddlClaimStatus").value;
        var pageSize = document.getElementById("ddlPageSize").value;

        var txtMedicaidBillingNumber = document.getElementById('txtMedicaidBillingNumber').value;


        var userName = document.getElementById('<%=hdnUserName.ClientID%>').value;

        if (txtMedicaidBillingNumber == null || txtMedicaidBillingNumber == '') {
            txtMedicaidBillingNumber = "";
        }

        var txtPatAccountNumber = document.getElementById('txtPatAccountNumber').value;
        if (txtPatAccountNumber == null || txtPatAccountNumber == '') {
            txtPatAccountNumber = "";
        }


        var txtPrescriptionNumber = document.getElementById('txtPrescriptionNumber').value;
        if (txtPrescriptionNumber == null || txtPrescriptionNumber == '') {
            txtPrescriptionNumber = "";
        }

        var txtAmountBilled = document.getElementById('txtAmountBilled').value;
        if (txtAmountBilled != null && txtAmountBilled != '') {
            totalAmount = txtAmountBilled;
        }

        var txtRenderingProviderId = document.getElementById('txtRenderingProviderId').value;
        if (txtRenderingProviderId == null || txtRenderingProviderId == '') {
            txtRenderingProviderId = "";
        }



        var txtDateofServfrom = document.getElementById('txtDateofServfrom').value;
        var txtDateofServto = document.getElementById('txtDateofServto').value;

        var medicaidId = document.getElementById("ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2").textContent;
        var ClaimType = document.getElementById('ddlClaimType').value;

        var txtRaDate = document.getElementById('txtRaDate').value;

        if (txtRaDate != null && txtRaDate != '') {
            RemittanceAdviceDateSpecified = true;
        } else {
            txtRaDate = "";
        }

        var txtICNTCN = document.getElementById('txtICNTCN').value;
        if (txtICNTCN == null || txtICNTCN == '') {
            txtICNTCN = "";
        }
        var claimtype = "";

        var ThruDOSSpecified = false;
        var FromDOSSpecified = false;

        if (txtDateofServto != null && txtDateofServto != '') {
            ThruDOSSpecified = true;
        } else {
            txtDateofServto = "";
        }
        if (txtDateofServfrom != null && txtDateofServfrom != '') {
            FromDOSSpecified = true;
        } else {
            txtDateofServfrom = "";
        }

        if (isDatabaseSearch == "1") {
            allowdbSearch = true;
            PayorType = ddlMangedCarePlan;
            if (ClaimType == '0') {
                claimtype = "D"
            }
            if (ClaimType == '1') {
                claimtype = "I"
            }
            if (ClaimType == '2') {
                claimtype = "P"
            }
            if (ClaimType == null || ClaimType == "") {
                claimtype = ""
            }
        }
        else {
            if (ClaimType == '0') {
                claimtype = "D"
            }
            if (ClaimType == '1') {
                claimtype = "I"
            }
            if (ClaimType == '2') {
                claimtype = "P"
            }
            if (ClaimType == null || ClaimType == "") {
                claimtype = ""
            }

            TotalChargesSpecified = true;

            if (ddlMangedCarePlan == "1") {
                //allowDBSearch = true;
                PayorType = "FFS";
            }
            else if (ddlMangedCarePlan == "2") {
                PayorType = "0021920";
            }
            else if (ddlMangedCarePlan == "3") {
                PayorType = "0002937";
            }
            else if (ddlMangedCarePlan == "4") {
                PayorType = "0021914";
            }
            else if (ddlMangedCarePlan == "5") {
                PayorType = "0004202";
            }
            else if (ddlMangedCarePlan == "6") {
                PayorType = "0003150";
            }
            else if (ddlMangedCarePlan == "7") {
                PayorType = "00021919";
            }
            else if (ddlMangedCarePlan == "8") {
                PayorType = "0007316";
            }
            else if (ddlMangedCarePlan == "9") {
                PayorType = "0007610";
            }
        }

        var dropdownlClaimStatus = document.getElementById("ddlClaimStatus");
        var claimStatus = dropdownlClaimStatus.options[dropdownlClaimStatus.selectedIndex].text;
        if (claimStatus == "ADJUDICATED") {
            claimStatus = "ADJUCATED"
        }

        var data = {
            AllowDBSearch: allowdbSearch,
            IsGridpaging: false,
            PayorType: PayorType,
            ICN: txtICNTCN,
            PatientAccountNumber: txtPatAccountNumber,
            MemberMedicaidId: txtMedicaidBillingNumber,
            RenderingProviderID: txtRenderingProviderId,
            BillingProviderID: medicaidId,
            PrescriptionNumber: txtPrescriptionNumber,
            ClaimType: claimtype,
            Status: claimStatus,
            TotalCharges: totalAmount,
            TotalChargesSpecified: TotalChargesSpecified,
            FromDOS: txtDateofServfrom,
            FromDOSSpecified: FromDOSSpecified,
            ThruDOS: txtDateofServto,
            ThruDOSSpecified: ThruDOSSpecified,
            RemittanceAdviceDate: txtRaDate,
            RemittanceAdviceDateSpecified: RemittanceAdviceDateSpecified,
            MaxRecords: pageSize,
            Offset: "0",
            PageSize: pageSize,
            UserName: userName
        };
        return data;
    }

    function ClaimsSearch(pageNumber) {
        $("#btnSearch").prop('disabled', true);
        document.getElementById('divpnlLoader').style.display = "block";
        var isDatabaseSearch = document.getElementById("ddlClaimStatus").value;
        if (isDatabaseSearch != "1") {
            var selectedOption = document.getElementById("ddlMangedCarePlan").value;
            if (selectedOption == "") {
                document.getElementById('ddlMangedCarePlan').classList.add('errMsgInput');
                document.getElementById('spanddlMangedCarePlan').innerHTML = "* Payor Name is Required";
                document.getElementById('divpnlLoader').style.visibility = "none";
                $("#btnSearch").prop('disabled', false);
                return false;
            }
            else {
                document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
                document.getElementById('spanddlMangedCarePlan').innerHTML = "";
            }
        } else {
            document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
            document.getElementById('spanddlMangedCarePlan').innerHTML = "";
        }
        const d = new Date();
        d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = 'showResults=true;';

        var txtReqFDOS = document.getElementById('txtDateofServfrom').value;
        var txtReqTDOS = document.getElementById('txtDateofServto').value;
        var txtRaDate = document.getElementById('txtRaDate').value;

        var txtMedicaidBillingNumber = document.getElementById('txtMedicaidBillingNumber').value;

        if (txtMedicaidBillingNumber != null && txtMedicaidBillingNumber != '') {
            if (txtMedicaidBillingNumber.length < 12) {

                message = "* 12-digit number is required.";
                document.getElementById('txtMedicaidBillingNumber').classList.add('errMsgInput');
                document.getElementById('spantxtMedicaidBillingNumber').innerHTML = message;
                document.getElementById('divpnlLoader').style.display = "none";
                $("#btnSearch").prop('disabled', false);
                return false;
            }
        }
        if (txtRaDate != null && txtRaDate != '') {

            if (new Date(txtRaDate) > new Date()) {
                message = "* RA Date cannot be future date.";
                document.getElementById('txtRaDate').classList.add('errMsgInput');
                document.getElementById('spantxtRaDate').innerHTML = message;
                document.getElementById('divpnlLoader').style.display = "none";
                $("#btnSearch").prop('disabled', false);
                return false;
            }
        }
        var message = validateFDOSTDOSDates(txtReqFDOS, txtReqTDOS);

        if (message != '') {
            document.getElementById('txtDateofServfrom').classList.add('errMsgInput');
            document.getElementById('txtDateofServto').classList.add('errMsgInput');
            document.getElementById('spantxtDateofServfrom').innerHTML = message;
            document.getElementById('divpnlLoader').style.display = "none";
            $("#btnSearch").prop('disabled', false);
            return false;
        }
        else {
            document.getElementById('txtDateofServfrom').classList.remove('errMsgInput');
            document.getElementById('txtDateofServto').classList.remove('errMsgInput');
            document.getElementById('spantxtDateofServfrom').innerHTML = '';
        }

        var APIToken = $("[id*=hdnAccessToken]").val();

        var allowdbSearch = false;
        var isDatabaseSearch = document.getElementById("ddlClaimStatus").value;

        if (isDatabaseSearch == "1") {
            allowdbSearch = true;
        }

        var data = prepareClaimSearchPayload();

        if (pageNumber == null || pageNumber == undefined || pageNumber == '') {
            data.Offset = "0";
        }
        else if (pageNumber > 0) {
            data.Offset = String((pageNumber - 1) * data.PageSize);
        }

        document.getElementById('pnlClaimSearchResult1').innerHTML = '';
        document.getElementById('pagination').innerHTML = '';
        document.getElementById('TotalItemsTextContainer').innerHTML = '';
        $.ajax({
            type: "POST",
            url: webApiClaimSearch + "ClaimSearch",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (result) {

                if (result != null) {
                    var respHeader = result.ResponseHeader;
                    if (respHeader != null && respHeader.ResponseType != null && respHeader.ResponseType != '' && respHeader.ResponseType == "FAILURE") {
                        var responseType = respHeader.ResponseType;
                        var ResponseMessage = respHeader.ResponseMessage;
                        var ResponseDetails = respHeader.ResponseDetails;
                        document.getElementById('pnlClaimSearchResult1').innerHTML = '';
                        document.getElementById('pnlClaimSearchResult1').innerHTML = '<table class="table csTable"><tbody><tr><td style="color:red;font-weight:bold" colspan="10">Failure from the Service : No data returned.</td></tr></tbody></table>';
                    }
                    else if (respHeader != null && respHeader.ResponseType != null && respHeader.ResponseType != '' && respHeader.ResponseType == "SUCCESS") {
                        if (result != null && result.TotalRecords != null && result.TotalRecords > 0) {

                            setupPagination(result.TotalPages, currentPage, result.TotalRecords);
                            var ClaimHeaderResponse = result.ClaimHeaderResponse
                            if (ClaimHeaderResponse != null && ClaimHeaderResponse.length > 0) {
                                var table = "<table id='claimSearchtt' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable(0)'>ICN <span id='icon-0' class='sort-icon'>&#8597;</span></th><th>Medicaid Billing Number</th><th>Patient Account Number</th><th>Billed Amount</th><th>Paid Amount</th><th>Claim Type</th><th>RA Date</th><th>From Date</th><th>To Date</th><th>Status</th><th>Attachment</th></tr></thead><tbody>";
                                for (var i = 0; i < ClaimHeaderResponse.length; i++) {

                                    var icn = ClaimHeaderResponse[i].ICN;
                                    var medicaidBillingNumber = ClaimHeaderResponse[i].MemberId;
                                    var patientAccountNumber = ClaimHeaderResponse[i].PatientAccountNumber;
                                    var billedAmount = ClaimHeaderResponse[i].TotalCharges;
                                    var paidAmount = ClaimHeaderResponse[i].TotalPaidAmount;
                                    var claimType = ClaimHeaderResponse[i].ClaimType;
                                    var raDate = "";
                                    var fromDate = "";
                                    var toDate = "";
                                    var status = ClaimHeaderResponse[i].ClaimStatus;
                                    var claimID = ClaimHeaderResponse[i].ClaimID;

                                    if (ClaimHeaderResponse[i].ClaimType == "D") {
                                        claimType = "DENTAL"
                                    }
                                    if (ClaimHeaderResponse[i].ClaimType == "I") {
                                        claimType = "INSTITUTIONAL"
                                    }
                                    if (ClaimHeaderResponse[i].ClaimType == "P") {
                                        claimType = "PROFESSIONAL"
                                    }

                                    const radate = ClaimHeaderResponse[i].RemittanceAdviceDate;

                                    if (radate != '' && radate != null && radate != undefined && radate.length > 0) {
                                        raDate = radate;
                                    }
                                    else {
                                        raDate = '';
                                    }

                                    const fdos = ClaimHeaderResponse[i].FromDOS;

                                    if (fdos != '' && fdos != null && fdos != undefined && fdos.length > 0) {
                                        const D = new Date(fdos);
                                        var fdosDate = getFormattedDate(D);
                                        fromDate = fdosDate;
                                    }
                                    else {
                                        fromDate = '';
                                    }

                                    const tdos = ClaimHeaderResponse[i].ThruDOS;

                                    if (tdos != '' && tdos != null && tdos != undefined && tdos.length > 0) {
                                        const D = new Date(tdos);
                                        var tdosDate = getFormattedDate(D);
                                        toDate = tdosDate;
                                    }
                                    else {
                                        toDate = '';
                                    }

                                    if (status == "ADJUCATED") {
                                        status = "ADJUDICATED";
                                    }

                                    if (status == "Pending Submission") {
                                        table = table + "<tr><td><a class='gridLink' onclick='return ShowDBClaimDetails(\"" + icn + "\",\"" + medicaidBillingNumber + "\",\"" + claimType + "\",\"" + claimID + "\")'>" + icn + "</a></td><td>" + medicaidBillingNumber + "</td><td><a class='gridLink' onclick='return RedirectClaim(\"" + icn + "\",\"" + medicaidBillingNumber + "\",\"" + claimType + "\",\"" + patientAccountNumber + "\")'>" + patientAccountNumber + "</a></td><td>" + billedAmount + "</td><td>" + paidAmount + "</td><td>" + claimType + "</td><td>" + radate + "</td><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + status + "</td></tr>";
                                    }
                                    else {
                                        if (status == "ADJUDICATED" || status == "DENY" || status == "OPEN" || status == "PAY" || status == "PEND") {
                                            table = table + "<tr><td><a class='gridLink' onclick='return ShowClaimDetails(\"" + icn + "\",\"" + medicaidBillingNumber + "\",\"" + claimType + "\")'>" + icn + "</a></td><td>" + medicaidBillingNumber + "</td><td><a class='gridLink' style='text - decoration: none!important; pointer - events: none; color: black; '>" + patientAccountNumber + "</a></td><td>" + billedAmount + "</td><td>" + paidAmount + "</td><td>" + claimType + "</td><td>" + radate + "</td><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + status + "</td><td><a class='gridLink' onclick='return RedirectAttachment(\"" + icn + "\",\"" + medicaidBillingNumber + "\",\"" + claimType + "\")'>" + "Upload</a></td></tr>";
                                        }
                                        else {
                                            table = table + "<tr><td><a class='gridLink' onclick='return ShowClaimDetails(\"" + icn + "\",\"" + medicaidBillingNumber + "\",\"" + claimType + "\")'>" + icn + "</a></td><td>" + medicaidBillingNumber + "</td><td><a class='gridLink' style='text - decoration: none!important; pointer - events: none; color: black; '>" + patientAccountNumber + "</a></td><td>" + billedAmount + "</td><td>" + paidAmount + "</td><td>" + claimType + "</td><td>" + radate + "</td><td>" + fromDate + "</td><td>" + toDate + "</td><td>" + status + "</td><td></td></tr>";
                                        }
                                    }
                                }
                                table = table + "</tbody></table>";
                                document.getElementById('pnlClaimSearchResult1').innerHTML = '';
                                document.getElementById('pnlClaimSearchResult1').innerHTML = table;
                                const element = document.getElementById("claimSearchtt"); // Replace "targetElement" with the actual ID of your element
                                element.scrollIntoView();
                            }
                            else {
                                document.getElementById('pnlClaimSearchResult1').innerHTML = '';
                                document.getElementById('pnlClaimSearchResult1').innerHTML = "<table class='table csTable'><tbody><tr><td colspan='10'>No results found.</td></tr></tbody></table>";
                            }
                        }
                        else {
                            document.getElementById('pnlClaimSearchResult1').innerHTML = '';
                            document.getElementById('pnlClaimSearchResult1').innerHTML = "<table class='table csTable'><tbody><tr><td colspan='10'>No results found.</td></tr></tbody></table>";
                        }
                    }
                    else {
                        document.getElementById('pnlClaimSearchResult1').innerHTML = '';
                        document.getElementById('pnlClaimSearchResult1').innerHTML = "<table class='table csTable'><tbody><tr><td colspan='10'>No results found.</td></tr></tbody></table>";
                    }
                }
                document.getElementById('divpnlLoader').style.display = "none";
                $("#btnSearch").prop('disabled', false);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormessage = JSON.stringify(jqXHR);

                if (jqXHR && jqXHR.status === 401) {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'Your token got expired. Kindly logout and login again.';
                }
                else {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'came to error : ' + JSON.stringify(jqXHR);
                }
                console.log(JSON.stringify(jqXHR));
                document.getElementById('divpnlLoader').style.display = "none";
                $("#btnSearch").prop('disabled', false);
            }
        });
    }

    let sortDirection = [null];

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

    function sortTable(n) {
        var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
        table = document.getElementById("claimSearchtt");
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

    function RedirectClaim(icn, memberId, claimType, patientAccountNumber) {
        var claimId = "";
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaimSearch + "GetSavedClaimID?icn=" + icn + "&&MedicalBillingNumber=" + memberId + "&&PatientAccountNumber=" + patientAccountNumber + "&&MedicaidID=" + txtDiagnosisCodedesc + "&&claimType=" + claimType,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result != null && result != undefined) {
                    claimId = result;
                    redirectToClaims(icn, memberId, claimType, patientAccountNumber, claimId);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById('pnlClaimSearchResult1').innerText = 'came to error : ' + JSON.stringify(jqXHR);
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function redirectToClaims(icn, memberId, claimType, patientAccountNumber, claimId) {
        var icn_encry = "";
        var memberId_encry = "";
        var claimType_encry = "";
        var paytype_encry = "";
        var pan_encry = "";
        var claimId_encry = "";
        let encryptionPyalods = [
            { KeyValue: icn, KeyName: "ICN", KeySecret: "" },
            { KeyValue: memberId, KeyName: "MemberId", KeySecret: "" },
            { KeyValue: claimType, KeyName: "ClaimType", KeySecret: "" },
            { KeyValue: patientAccountNumber, KeyName: "patientAccountNumber", KeySecret: "" },
            { KeyValue: claimId, KeyName: "ClaimId", KeySecret: "" }
        ];
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiClaimSearch + "EncryptData",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(encryptionPyalods),
            success: function (result) {

                if (result != null && result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].KeyName == "ICN") {
                            icn_encry = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "MemberId") {
                            memberId_encry = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "ClaimType") {
                            claimType_encry = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "patientAccountNumber") {
                            pan_encry = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "ClaimId") {
                            claimId_encry = result[i].KeySecret;
                        }
                    }
                    if (icn_encry != null && memberId_encry != null && claimType_encry != null && pan_encry != null && claimId_encry != null) {
                        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
                        window.location.href = urlpath + "/SubmitClaim.aspx?icn= " + icn + "&ct=" + claimType + "&pcn=" + memberId_encry + "&pan=" + pan_encry + "&cid=" + claimId_encry + "";
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormessage = JSON.stringify(jqXHR);
                if (jqXHR && jqXHR.status === 401) {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'Your token got expired. Kindly logout and login again.';
                }
                else {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'came to error : ' + JSON.stringify(jqXHR);
                }
                console.log(JSON.stringify(jqXHR));
            }
        });

    }
    function setCookie(c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = c_name + "=" + c_value;
    }
    function RedirectAttachment(icn, memberId, claimType) {
        var icn_encry = "";
        var memberId_encry = "";
        var claimType_encry = "";
        var paytype_encry = "";
        var pan_encry = "";
        var claimId_encry = "";
        setCookie("selectedOption", 10015, 1);
        let encryptionPyalods = [
            { KeyValue: icn, KeyName: "ICN", KeySecret: "" },
            { KeyValue: memberId, KeyName: "MemberId", KeySecret: "" },
            { KeyValue: claimType, KeyName: "ClaimType", KeySecret: "" }
        ];
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiClaimSearch + "EncryptData",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(encryptionPyalods),
            success: function (result) {

                if (result != null && result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].KeyName == "ICN") {
                            icn_encry = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "MemberId") {
                            memberId_encry = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "ClaimType") {
                            claimType_encry = result[i].KeySecret;
                        }

                    }
                    if (icn_encry != null && memberId_encry != null && claimType_encry != null && claimId_encry != null) {
                        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));


                        window.location.href = urlpath + "/StandaloneUploadAttachments.aspx?icn= " + icn_encry + "&TransactionTypeID=" + claimType + "&MemberID=" + memberId_encry + "";
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormessage = JSON.stringify(jqXHR);
                if (jqXHR && jqXHR.status === 401) {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'Your token got expired. Kindly logout and login again.';
                }
                else {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'came to error : ' + JSON.stringify(jqXHR);
                }
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function ShowClaimDetails(icn, memberId, claimType) {
        var ddlMangedCarePlan = document.getElementById("ddlMangedCarePlan").value;
        var PayorType = "";

        if (ddlMangedCarePlan == "1") {
            //allowDBSearch = true;
            PayorType = "FFS";
        }
        else if (ddlMangedCarePlan == "2") {
            PayorType = "0021920";
        }
        else if (ddlMangedCarePlan == "3") {
            PayorType = "0002937";
        }
        else if (ddlMangedCarePlan == "4") {
            PayorType = "0021914";
        }
        else if (ddlMangedCarePlan == "5") {
            PayorType = "0004202";
        }
        else if (ddlMangedCarePlan == "6") {
            PayorType = "0003150";
        }
        else if (ddlMangedCarePlan == "7") {
            PayorType = "00021919";
        }
        else if (ddlMangedCarePlan == "8") {
            PayorType = "0007316";
        }
        else if (ddlMangedCarePlan == "9") {
            PayorType = "0007610";
        }

        let encryptionPyalods = [
            { KeyValue: icn, KeyName: "ICN", KeySecret: "" },
            { KeyValue: memberId, KeyName: "MemberId", KeySecret: "" },
            { KeyValue: claimType, KeyName: "ClaimType", KeySecret: "" },
            { KeyValue: PayorType, KeyName: "PayorType", KeySecret: "" }
        ];
        var icn = "";
        var memberId = "";
        var claimType = "";
        var paytype = "";

        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiClaimSearch + "EncryptData",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(encryptionPyalods),
            success: function (result) {

                if (result != null && result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].KeyName == "ICN") {
                            icn = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "MemberId") {
                            memberId = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "ClaimType") {
                            claimType = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "PayorType") {
                            paytype = result[i].KeySecret;
                        }
                    }
                    if (icn != null && memberId != null && claimType != null && paytype != null) {
                        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
                        window.location.href = urlpath + "/SubmitClaim.aspx?icn= " + icn + "&ct=" + claimType + "&pt=" + paytype + "";
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormessage = JSON.stringify(jqXHR);
                if (jqXHR && jqXHR.status === 401) {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'Your token got expired. Kindly logout and login again.';
                }
                else {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'came to error : ' + JSON.stringify(jqXHR);

                }
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function ShowDBClaimDetails(icn, memberId, claimType, claimID) {
        var ddlMangedCarePlan = document.getElementById("ddlMangedCarePlan").value;
        var PayorType = "";

        if (ddlMangedCarePlan == "1") {
            //allowDBSearch = true;
            PayorType = "FFS";
        }
        else if (ddlMangedCarePlan == "2") {
            PayorType = "0021920";
        }
        else if (ddlMangedCarePlan == "3") {
            PayorType = "0002937";
        }
        else if (ddlMangedCarePlan == "4") {
            PayorType = "0021914";
        }
        else if (ddlMangedCarePlan == "5") {
            PayorType = "0004202";
        }
        else if (ddlMangedCarePlan == "6") {
            PayorType = "0003150";
        }
        else if (ddlMangedCarePlan == "7") {
            PayorType = "00021919";
        }
        else if (ddlMangedCarePlan == "8") {
            PayorType = "0007316";
        }
        else if (ddlMangedCarePlan == "9") {
            PayorType = "0007610";
        }

        let encryptionPyalods = [
            { KeyValue: icn, KeyName: "ICN", KeySecret: "" },
            { KeyValue: memberId, KeyName: "MemberId", KeySecret: "" },
            { KeyValue: claimType, KeyName: "ClaimType", KeySecret: "" },
            { KeyValue: PayorType, KeyName: "PayorType", KeySecret: "" },
            { KeyValue: claimID, KeyName: "ClaimID", KeySecret: "" }
        ];
        var icn = "";
        var memberId = "";
        var claimType = "";
        var paytype = "";
        var claimID = "";

        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiClaimSearch + "EncryptData",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(encryptionPyalods),
            success: function (result) {

                if (result != null && result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].KeyName == "ICN") {
                            icn = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "MemberId") {
                            memberId = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "ClaimType") {
                            claimType = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "PayorType") {
                            paytype = result[i].KeySecret;
                        }
                        if (result[i].KeyName == "ClaimID") {
                            claimID = result[i].KeySecret;
                        }
                    }
                    if (icn != null && memberId != null && claimType != null && paytype != null) {
                        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
                        window.location.href = urlpath + "/SubmitClaim.aspx?icn= " + icn + "&ct=" + claimType + "&pt=" + paytype + "" + "&cid=" + claimID + "";
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormessage = JSON.stringify(jqXHR);
                if (jqXHR && jqXHR.status === 401) {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'Your token got expired. Kindly logout and login again.';
                }
                else {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'came to error : ' + JSON.stringify(jqXHR);
                }
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function getFormattedDate(dateVal) {
        var year = dateVal.getFullYear();
        var month = (1 + dateVal.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = dateVal.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return month + '/' + day + '/' + year;
    }

    function bindClaimStatus() {

        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaimSearch + "GetSearchClaimsDropdowns",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result != null) {

                    var payers = result.Payers;
                    var claimStatus = result.ClaimStaus;
                    var claimTypes = result.ClaimTypes;
                    var pageSizes = result.PageSizes;

                    if (claimStatus != null && claimStatus.length > 0) {
                        const ddclaimStatusJS = document.getElementById('ddlClaimStatus');
                        ddclaimStatusJS.innerHTML = '';
                        const defaultOption = document.createElement('option');
                        defaultOption.text = '';
                        ddclaimStatusJS.add(defaultOption);
                        for (var i = 0; i < claimStatus.length; i++) {
                            const option = document.createElement('option');
                            option.value = claimStatus[i].StatusId;
                            option.text = claimStatus[i].StatusType;
                            ddclaimStatusJS.add(option);
                        }
                    }
                    if (payers != null && payers.length > 0) {

                        const ddlMangedCarePlan = document.getElementById('ddlMangedCarePlan');
                        ddlMangedCarePlan.innerHTML = '';
                        const defaultOption = document.createElement('option');
                        defaultOption.text = '';
                        ddlMangedCarePlan.add(defaultOption);
                        for (var i = 0; i < payers.length; i++) {
                            const option = document.createElement('option');
                            option.value = payers[i].PayerId;
                            option.text = payers[i].PayerDesc;
                            ddlMangedCarePlan.add(option);
                        }
                    }
                    if (claimTypes != null && claimTypes.length > 0) {

                        const ddlClaimType = document.getElementById('ddlClaimType');
                        ddlClaimType.innerHTML = '';
                        const defaultOption = document.createElement('option');
                        defaultOption.text = '';
                        ddlClaimType.add(defaultOption);
                        for (var i = 0; i < claimTypes.length; i++) {
                            const option = document.createElement('option');
                            option.value = claimTypes[i].Value;
                            option.text = claimTypes[i].Text;
                            ddlClaimType.add(option);
                        }
                    }

                    if (pageSizes != null && pageSizes.length > 0) {

                        const ddlPageSize = document.getElementById('ddlPageSize');
                        ddlPageSize.innerHTML = '';
                        for (var i = 0; i < pageSizes.length; i++) {
                            const option = document.createElement('option');
                            option.value = pageSizes[i].Value;
                            option.text = pageSizes[i].Size;
                            ddlPageSize.add(option);
                        }

                        var maxPageSize = document.getElementById('ddlPageSize');

                        for (var i = 0; i < maxPageSize.options.length; i++) {
                            if (maxPageSize.options[i].value == '20') {
                                maxPageSize.selectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var errormessage = JSON.stringify(jqXHR);
                if (jqXHR && jqXHR.status === 401) {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'Your token got expired. Kindly logout and login again.';
                }
                else {
                    document.getElementById('pnlClaimSearchResult1').innerText = 'came to error : ' + JSON.stringify(jqXHR);
                }
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function PreviousClick(totalPages, curPage) {

        if (curPage > 1) {
            currentPage--;
            ClaimsSearch(currentPage);
        }
    }

    function NextClick(totPages, currPage) {

        if (currPage < totPages) {
            currentPage++;
            ClaimsSearch(currentPage);
        }
    }

    function setupPagination(totalPages, currentPage, totalRecords) {

        var paginationDiv = document.getElementById('pagination');
        paginationDiv.innerHTML = '';

        document.getElementById('TotalItemsTextContainer').innerHTML = `<b>${totalRecords}</b>&nbsp;items in ${totalPages} pages&nbsp;<b></b>`;

        var buttonLis = "<button type='button' disabled id='btnPrev' onclick='PreviousClick(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";
        buttonLis = buttonLis + "<div class='pagenumber'>"
        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage_" + i + "' class='mybtnpage' type='button' onclick='btnPaging_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "</div>"
        buttonLis = buttonLis + "<button type='button' id='btnNext' onclick='NextClick(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
        paginationDiv.scrollIntoView();
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

    function btnPaging_Click(pagenumber) {


        var pageId = 'btnPage_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);

        const stylesdiv = window.getComputedStyle(paginationDiv);
        if (stylesdiv.fontWeight !== 'bold' && parseInt(stylesdiv.fontWeight) !== 700) {



            const buttons = document.querySelectorAll(".mybtnpage");
            currentPage = pagenumber;
            ClaimsSearch(pagenumber);
            buttons.forEach(button => {
                button.style.fontWeight = 'normal';
            });

            if (paginationDiv !== null && paginationDiv !== undefined) {
                paginationDiv.style.fontWeight = 'bold'
                paginationDiv.scrollIntoView();
            }
        }
    }

    function validateBillingNumber() {


        var status = true;
        const numbers = /^[0-9]*$/;

        document.getElementById('txtMedicaidBillingNumber').classList.remove('errMsgInput');
        document.getElementById('spantxtMedicaidBillingNumber').innerHTML = "";

        var txtMedicaidBillingNumber = document.getElementById('txtMedicaidBillingNumber').value;

        if (txtMedicaidBillingNumber == null || txtMedicaidBillingNumber == undefined || txtMedicaidBillingNumber == '') {
            status = true;
        }
        else {
            if (!numbers.test(txtMedicaidBillingNumber)) {
                document.getElementById('txtMedicaidBillingNumber').classList.add('errMsgInput');
                document.getElementById('spantxtMedicaidBillingNumber').innerHTML = "Please enter valid Medicaid billing number.";
                status = false;
            }
            else if (txtMedicaidBillingNumber.length < 12) {
                document.getElementById('txtMedicaidBillingNumber').classList.add('errMsgInput');
                document.getElementById('spantxtMedicaidBillingNumber').innerHTML = "* 12-digit number is required.";
                status = false;
            }

        }
        return status;
    }

    function validateAccountNumber() {
        var status = true;
        const alphaNumberic = /^[a-zA-Z0-9]+$/;
        document.getElementById('txtPatAccountNumber').classList.remove('errMsgInput');
        document.getElementById('spantxtPatAccountNumber').innerHTML = "";
        var txtPatAccountNumber = document.getElementById('txtPatAccountNumber').value;
        if (txtPatAccountNumber == null || txtPatAccountNumber == undefined || txtPatAccountNumber == '') {
            status = true;
        }
        else {
            if (!alphaNumberic.test(txtPatAccountNumber)) {
                document.getElementById('txtPatAccountNumber').classList.add('errMsgInput');
                document.getElementById('spantxtPatAccountNumber').innerHTML = "Please enter valid account number.";
                status = false;
            }
        }
        return status;
    }

    function validateAmountBilled() {
        var status = true;
        const amount = /^\d+(\.\d{0,2})?$/;
        document.getElementById('txtAmountBilled').classList.remove('errMsgInput');
        document.getElementById('spantxtAmountBilled').innerHTML = "";
        var txtAmountBilled = document.getElementById('txtAmountBilled').value;
        if (txtAmountBilled == null || txtAmountBilled == undefined || txtAmountBilled == '') {
            status = true;
        }
        else {
            if (!amount.test(txtAmountBilled)) {
                document.getElementById('txtAmountBilled').classList.add('errMsgInput');
                document.getElementById('spantxtAmountBilled').innerHTML = "Enter valid Total Paid Amount TO";
                status = false;
            }
        }
        return status;
    }

    function validatePrescriptionNumber() {
        var status = true;
        const alphaNumeric = /^[a-z0-9]+$/i;
        document.getElementById('txtPrescriptionNumber').classList.remove('errMsgInput');
        document.getElementById('spantxtPrescriptionNumber').innerHTML = "";
        var txtPrescriptionNumber = document.getElementById('txtPrescriptionNumber').value;
        if (txtPrescriptionNumber == null || txtPrescriptionNumber == undefined || txtPrescriptionNumber == '') {
            status = true;
        }
        else {
            if (!alphaNumeric.test(txtPrescriptionNumber)) {
                document.getElementById('txtPrescriptionNumber').classList.add('errMsgInput');
                document.getElementById('spantxtPrescriptionNumber').innerHTML = "* Please enter valid Prescription Number.";
                status = false;
            }
        }
        return status;
    }

    function validateRenderingProvider() {
        var status = true;
        const numbers = /^[0-9]*$/;

        document.getElementById('txtRenderingProviderId').classList.remove('errMsgInput');
        document.getElementById('spantxtRenderingProviderId').innerHTML = "";

        var txtRenderingProviderId = document.getElementById('txtRenderingProviderId').value;
        if (txtRenderingProviderId == null || txtRenderingProviderId == undefined || txtRenderingProviderId == '') {
            status = true;
        }
        else {
            if (!numbers.test(txtRenderingProviderId)) {
                document.getElementById('txtRenderingProviderId').classList.add('errMsgInput');
                document.getElementById('spantxtRenderingProviderId').innerHTML = "* Please enter valid Rendering Provider ID.";
                status = false;
            }
        }
        return status;
    }

    function validateICNNumber() {
        var status = true;
        const numbers = /^[a-zA-Z0-9]+$/;

        document.getElementById('txtICNTCN').classList.remove('errMsgInput');
        document.getElementById('spantxttxtICNTCN').innerHTML = "";

        var txtICNTCN = document.getElementById('txtICNTCN').value;

        if (txtICNTCN == null || txtICNTCN.trim() === '') {
            status = true;
        } else {
            if (!numbers.test(txtICNTCN)) {
                document.getElementById('txtICNTCN').classList.add('errMsgInput');
                document.getElementById('spantxttxtICNTCN').innerHTML = "Please enter a valid ICN.";
                status = false;
            }
        }
        return status;
    }

    function PageSize_Changed() {
        var pageSize = document.getElementById("ddlPageSize").value;
        btnSearch_Click();
    }

    function ddlMangedCarePlanChange() {
        var selectedOption = document.getElementById("ddlMangedCarePlan").value;
        if (selectedOption == "") {
            document.getElementById('ddlMangedCarePlan').classList.add('errMsgInput');
            document.getElementById('spanddlMangedCarePlan').innerHTML = "* Payor Name is Required";
            return false;
        }
        else {
            document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
            document.getElementById('spanddlMangedCarePlan').innerHTML = "";
        }
    }

    function clearForm() {

        //ddlClaimType.SelectedIndex = 0;
        //ddlClaimStatus.SelectedIndex = 0;
        //ddlMangedCarePlan.SelectedIndex = 0;


        document.getElementById('txtICNTCN').value = '';
        document.getElementById('txtMedicaidBillingNumber').value = '';
        document.getElementById('txtPatAccountNumber').value = '';
        document.getElementById('txtDateofServfrom').value = '';
        document.getElementById('txtDateofServto').value = '';
        document.getElementById('txtRaDate').value = '';
        document.getElementById('txtRenderingProviderId').value = '';
        document.getElementById('txtAmountBilled').value = '';
        document.getElementById('txtPrescriptionNumber').value = '';

        document.getElementById('ddlClaimType').selectedIndex = 0;
        document.getElementById('ddlClaimStatus').selectedIndex = 0;
        document.getElementById('ddlMangedCarePlan').selectedIndex = 0;
        document.getElementById('pnlClaimSearchResult1').innerHTML = '';

        document.getElementById('txtPatAccountNumber').classList.remove('errMsgInput');
        document.getElementById('spantxtPatAccountNumber').innerHTML = "";

        document.getElementById('txtMedicaidBillingNumber').classList.remove('errMsgInput');
        document.getElementById('spantxtMedicaidBillingNumber').innerHTML = "";

        document.getElementById('txtAmountBilled').classList.remove('errMsgInput');
        document.getElementById('spantxtAmountBilled').innerHTML = "";


        document.getElementById('txtPrescriptionNumber').classList.remove('errMsgInput');
        document.getElementById('spantxtPrescriptionNumber').innerHTML = "";

        document.getElementById('txtRenderingProviderId').classList.remove('errMsgInput');
        document.getElementById('spantxtRenderingProviderId').innerHTML = "";

        document.getElementById('ddlMangedCarePlan').classList.remove('errMsgInput');
        document.getElementById('spanddlMangedCarePlan').innerHTML = "";

        resultToggle();
    }

    function divToggle() {
        $('.cformContainer').toggleClass("active");
        $('.claimTitle2').toggleClass("active");
    }
    function resultToggle() {
        $('.resultContainer').toggleClass("active");
        $('.csresult').toggleClass("active");
    }
    function resultToggleShow() {
        $('.resultContainer').addClass("active");
        $('.csresult').addClass("active");
    }

</script>

<div class="claimTitle claimTitle2" onclick="divToggle()" id="ctForm">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">Claim Search</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="cformContainer active">

    <div class="row cForm">

        <div class="col-lg-12">
            <span style="color: #D33421; font-size: 14pt !important; font-weight: bold; position: relative; top: -17px">Please enter all available information before performing a search for an accurate retrieval. An open-ended search will delay or yield no results.</span>
            <asp:Label runat="server" Style="color: #D33421; font-size: 14pt !important; font-weight: bold; position: relative; top: -17px" ID="SearchClaimHelpTxt" />
        </div>

        <asp:HiddenField ID="hdnUserName" runat="server" />
        <asp:HiddenField ID="hdnTotalCount" runat="server" />
        <div class="col-md-6">
            <div class="form-group">
                <label>ICN</label>
                <input id="txtICNTCN" onblur="validateICNNumber()" type="text" class="form-control">
                <small class="errMsg" id="spantxttxtICNTCN"></small>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group ">
                <label>Claim Type</label>
                <select id="ddlClaimType" class="form-control"></select>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label>Medicaid Billing Number</label>
                <input id="txtMedicaidBillingNumber" onblur="validateBillingNumber()" type="text" class="form-control">
                <small class="errMsg" id="spantxtMedicaidBillingNumber"></small>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label>Claim Status</label>
                <select id="ddlClaimStatus" class="form-control"></select>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>Patient Account Number</label>
                <input id="txtPatAccountNumber" onblur="validateAccountNumber()" type="text" class="form-control">
                <small class="errMsg" id="spantxtPatAccountNumber"></small>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label>RA Date</label>
                <input id="txtRaDate" type="date" class="form-control">
                <small class="errMsg" id="spantxtRaDate"></small>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label>Rendering Provider ID</label>
                <input id="txtRenderingProviderId" onblur="validateRenderingProvider()" type="text" class="form-control">
                <small class="errMsg" id="spantxtRenderingProviderId"></small>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">

                <div class="containerfluid">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Date of Service From</label>
                                <input id="txtDateofServfrom" type="date" class="form-control">
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Date of Service To</label>
                                <input id="txtDateofServto" type="date" class="form-control">
                                <small class="errMsg" id="spantxtDateofServfrom"></small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label>Amount Billed</label>
                <input id="txtAmountBilled" onblur="validateAmountBilled()" type="text" class="form-control">
                <small class="errMsg" id="spantxtAmountBilled"></small>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label>Prescription Number</label>
                <input id="txtPrescriptionNumber" onblur="validatePrescriptionNumber()" type="text" class="form-control">
                <small class="errMsg" id="spantxtPrescriptionNumber"></small>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label>Payor Name *</label>
                <select id="ddlMangedCarePlan" onchange="ddlMangedCarePlanChange()" class="form-control"></select>
                <small class="errMsg" id="spanddlMangedCarePlan"></small>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group maxclaim">
                <label>Max Records</label>
                <div class="col-lg-6">
                    <select id="ddlPageSize" onchange="PageSize_Changed()" class="form-control"></select>
                </div>
            </div>
        </div>

        <div class="col-lg-12 text-center" style="padding: 28px 0 10px 0">
            <button id="btnSearch" type="button" onclick="btnSearch_Click()" class="buttonBoxFocusBlue">Search</button>
            <button id="btnClear" type="button" onclick="clearForm()" class="buttonBoxFocusred">Clear</button>

        </div>

    </div>
</div>
<div class="claimTitle csresult" onclick="resultToggle()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">Claim Search Result</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer">
    <div id="divpnlLoader" style="display: none">
        <img src='../Images/loader.gif' style='height: 38px; width: 35px;'>
    </div>
    <div id="pnlClaimSearchResult1" style="overflow: auto; width: 100%"></div>
    <div id="paginationContainer">
        <div id="TotalItemsTextContainer" style="float: left; align-items: baseline; display: flex; margin-left: 16px"></div>
        <div id="pagination" style="float: right;"></div>
    </div>
</div>

