<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ProviderSpecialtySearch" Codebehind="ProviderSpecialtySearch.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>

<script src="../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script src="../Scripts/jquery.loadTemplate.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui.js" type="text/javascript"></script>
<script src="../Scripts/bootstrap.min.js"></script>
<script src="https://unpkg.com/xlsx/dist/xlsx.full.min.js"></script>

<link href="../Content/custom-style.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<link href="<%# Page.ResolveClientUrl("~/App_Themes/Modernization/Modern.css") %>" rel="stylesheet" />

<style type="text/css">
    .csTable thead tr {
        background: #435363;
        color: #ffffff;
    }

    .csTable tbody tr {
        background: #fff
    }

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

</style>
<script type="text/javascript">
    let currentPage = 1;
    let SpecSearchResults; 
    const pageSize = 10;
    let specialtySearchResultData;
    let specSearchResultFileterd;
</script>

<script type="text/javascript">

    function btnclearSearch() {
        document.getElementById('txtNPI').value = '';
        document.getElementById('txtMedID').value = '';

        document.getElementById('spantxtNPI').innerHTML = '';
        document.getElementById('spantxtMedID').innerHTML = '';

        document.getElementById('pnlSpecSearchResult').innerHTML = '';
        document.getElementById('psspagination').innerHTML = '';

        document.getElementById('pnlSpecSearchErrResults').innerHTML = '';

        $('.specSearchresultContainer2').removeClass("active");
    }

    function clearErrorMessages() {
        document.getElementById('spantxtNPI').innerHTML = '';
        document.getElementById('spantxtMedID').innerHTML = '';

        document.getElementById('pnlSpecSearchResult').innerHTML = '';
        document.getElementById('psspagination').innerHTML = '';

        document.getElementById('pnlSpecSearchErrResults').innerHTML = '';

        $('.specSearchresultContainer2').removeClass("active");
    } 

    function ValidateData() {
        /*alert("Validating");*/
        clearErrorMessages();
        var message = '';
        var txtNPIVal = document.getElementById('txtNPI').value;
        var txtMedIDVal = document.getElementById('txtMedID').value;
        var valid = true;

        //alert(txtNPIVal);
        //alert(txtMedIDVal);

        if ((txtMedIDVal == null || txtMedIDVal == '') && (txtNPIVal == null || txtNPIVal == '')) {           
            message = "* Please Enter NPI or Medicaid ID.";
            document.getElementById('spantxtNPI').innerHTML = message;
            valid = false;
        }

        if (txtNPIVal != null && txtNPIVal != '') {
            if (txtNPIVal.length < 10 || txtNPIVal.length > 10) {
                message = "* 10-digits number required.";
                document.getElementById('spantxtNPI').innerHTML = message;
                valid = false;
            }
            else if (!/^[0-9]+$/.test(txtNPIVal)) {
                document.getElementById('spantxtNPI').innerHTML = "Please enter numeric characters only for NPI.";
                valid = false;
            }
        }       

        

        if (txtMedIDVal != null && txtMedIDVal != '') {
            if (txtMedIDVal.length < 7 || txtMedIDVal.length > 7) {
                message = "* 7-digits number required.";
                document.getElementById('spantxtMedID').innerHTML = message;
                valid = false;
            }
        }

        return valid;
    }

    function getFormattedDate(date) {
        var d = new Date(date + 'T00:00'),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();
        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [month, day, year].join('/');
    }

    
    function btnProvSpecialtySearch() {
        if (!ValidateData()) {
            return false;
        }
        
        var APIToken = $("[id*=hdnAccessToken]").val();

        var message = '';
        var npi = document.getElementById('txtNPI').value;
        var medID = document.getElementById('txtMedID').value;     

        var SpecSearchInputParams = {
            ProviderNPI: npi,
            ProviderMedID: medID
        }
        var inputData = JSON.stringify(SpecSearchInputParams);  
        //alert('here');
        //alert(inputData);
        $.ajax({
            type: "POST",
            url: webApiEnrollment + "GetProviderSpecialtySearchResults",
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
                /*console.log(result);*/
                if (result != null) {
                    try {
                        specialtySearchResultData = result;
                        var SpecSearchDisplay = specialtySearchResultData.SpecialtySearchResult;
                        //console.log(SpecSearchDisplay);
                        //console.log(specialtySearchResultData.ResponseCode);
                        if (specialtySearchResultData.ResponseCode == '200' && SpecSearchDisplay != null && SpecSearchDisplay != '') {
                            SpecSearchResults = SpecSearchDisplay;
                            var SpecSearchDisplaySliced = SpecSearchResults.slice(0, pageSize);
                            var totalPages = Math.ceil(SpecSearchResults.length / pageSize);
                            
                            var table = "<table id='specResultstbl' class='table csTable'><thead><tr><th class='cursor' onclick='filterTable(0)'>Medicaid ID <span id='icon-0' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(1)'>Provider Type <span id='icon-1' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(2)'>Specialty <span id='icon-2' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(3)'>Primary <span id='icon-3' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(4)'>Start Date <span id='icon-4' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(5)'>End Date <span id='icon-5' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(6)'>Enroll Status <span id='icon-6' class='sort-icon'>&#8597;</span></th></tr></thead><tbody>";
                            for (var i = 0; i < SpecSearchDisplaySliced.length; i++) {

                                var MedIDDisplay = '';
                                var PTTypeDisplay = '';
                                var SpecialtyDisplay = '';
                                var PrimaryDisplay = '';
                                var StartDateDisplay = '';
                                var EndDateDisplay = '';
                                var EnrollStsDisplay = '';

                                if (SpecSearchDisplaySliced[i].MedicaidID != null) {
                                    MedIDDisplay = SpecSearchDisplaySliced[i].MedicaidID;
                                }
                                if (SpecSearchDisplaySliced[i].ProviderType != null) {
                                    PTTypeDisplay = SpecSearchDisplaySliced[i].ProviderType;
                                }
                                if (SpecSearchDisplaySliced[i].SpecialtyType != null) {
                                    SpecialtyDisplay = SpecSearchDisplaySliced[i].SpecialtyType;
                                }
                                if (SpecSearchDisplaySliced[i].PrimaryFlag != null) {
                                    PrimaryDisplay = SpecSearchDisplaySliced[i].PrimaryFlag;
                                }
                                if (SpecSearchDisplaySliced[i].StartDate != null) {
                                    StartDateDisplay = SpecSearchDisplaySliced[i].StartDate;// getFormattedDate(SpecSearchDisplaySliced[i].StartDate.split('T')[0]);
                                }
                                if (SpecSearchDisplaySliced[i].EndDate != null) {
                                    EndDateDisplay = SpecSearchDisplaySliced[i].EndDate;// getFormattedDate(SpecSearchDisplaySliced[i].EndDate.split('T')[0]);
                                }
                                if (SpecSearchDisplaySliced[i].EnrollStatusDesc != null) {
                                    EnrollStsDisplay = SpecSearchDisplaySliced[i].EnrollStatusDesc;
                                }

                                table = table + "<tr><td>" + MedIDDisplay + "</td><td>" + PTTypeDisplay + "</td><td>" + SpecialtyDisplay + "</td><td>" + PrimaryDisplay + "</td><td>" + StartDateDisplay + "</td><td>" + EndDateDisplay + "</td><td>" + EnrollStsDisplay + "</td></tr>";
                            }

                            table = table + "</tbody></table>";
                            document.getElementById('pnlSpecSearchResult').innerHTML = '';
                            document.getElementById('pnlSpecSearchResult').innerHTML = table;
                            setupPagination(totalPages, 1);

                        } else if (result.ResponseCode == '300' || result.ResponseCode == '301' || result.ResponseCode == '302') {
                            document.getElementById('pnlSpecSearchErrResults').innerHTML = '';
                            document.getElementById('pnlSpecSearchErrResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">' + result.ResponseDescp + '</span>';
                        } else {
                            document.getElementById('pnlSpecSearchErrResults').innerHTML = '';
                            document.getElementById('pnlSpecSearchErrResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">No data returned: ' + '</span>';
                        }
                    }
                    catch (err) {
                        document.getElementById('pnlSpecSearchErrResults').innerHTML = '';
                        document.getElementById('pnlSpecSearchErrResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error : ' + err + '</span>';
                        console.log(err);
                    }
                }


            },
            error: function (jqXHR, textStatus, errorThrown) {
                var status = jqXHR.status;
                document.getElementById('pnlSpecSearchErrResults').innerHTML = '';
                document.getElementById('pnlSpecSearchErrResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error : ' + status + ' No data returned</span>';
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function setupPagination(totalPages, currentPage) {
        var paginationDiv = document.getElementById('psspagination');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev' onclick='PreviousPageClick(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage_" + i + "' class='mybtnpage' type='button' onclick='btnPaging_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext' onclick='NextPageClick(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
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
        table = document.getElementById("specResultstbl");
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

    function PreviousPageClick(totalPages, curPage) {

        if (curPage > 1) {
            currentPage--;
            GetSpecialtySearchResults(currentPage);
        }
    }

    function NextPageClick(totPages, currPage) {

        if (currPage < totPages) {
            currentPage++;
            GetSpecialtySearchResults(currentPage);
        }
    }

    function btnPaging_Click(pagenumber) {


        var pageId = 'btnPage_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);

        const stylesdiv = window.getComputedStyle(paginationDiv);
        if (stylesdiv.fontWeight !== 'bold' && parseInt(stylesdiv.fontWeight) !== 700) {



            const buttons = document.querySelectorAll(".mybtnpage");
            currentPage = pagenumber;
            GetSpecialtySearchResults(pagenumber);
            buttons.forEach(button => {
                button.style.fontWeight = 'normal';
            });

            if (paginationDiv !== null && paginationDiv !== undefined) {
                paginationDiv.style.fontWeight = 'bold'
            }
        }
    }
    function GetSpecialtySearchResults(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var SpecSearchDisplaySliced = specialtySearchResultData.SpecialtySearchResult.slice(startIndex, endIndex);
        //var SpecSearchDisplaySliced = specSearchResultFileterd;
        var totalPages = Math.ceil(specialtySearchResultData.SpecialtySearchResult.length / pageSize);
        //alert('heeeee');
        //alert(SpecSearchDisplaySliced.length);
        var table = "<table id='specResultstbl' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable(0)'>Medicaid ID <span id='icon-0' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(1)'>Provider Type <span id='icon-1' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(2)'>Specialty <span id='icon-2' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(3)'>Primary <span id='icon-3' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(4)'>Start Date <span id='icon-4' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(5)'>End Date <span id='icon-5' class='sort-icon'>&#8597;</span></th><th class='cursor' onclick='sortTable(6)'>Enroll Status <span id='icon-6' class='sort-icon'>&#8597;</span></th></tr></thead><tbody>";
        for (var i = 0; i < SpecSearchDisplaySliced.length; i++) {
           /* alert('heeeee1');*/
            var MedIDDisplay = '';
            var PTTypeDisplay = '';
            var SpecialtyDisplay = '';
            var PrimaryDisplay = '';
            var StartDateDisplay = '';
            var EndDateDisplay = '';
            var EnrollStsDisplay = '';

            if (SpecSearchDisplaySliced[i].MedicaidID != null) {
                MedIDDisplay = SpecSearchDisplaySliced[i].MedicaidID;
            }
            if (SpecSearchDisplaySliced[i].ProviderType != null) {
                PTTypeDisplay = SpecSearchDisplaySliced[i].ProviderType;
            }
            if (SpecSearchDisplaySliced[i].SpecialtyType != null) {
                SpecialtyDisplay = SpecSearchDisplaySliced[i].SpecialtyType;
            }
            if (SpecSearchDisplaySliced[i].PrimaryFlag != null) {
                PrimaryDisplay = SpecSearchDisplaySliced[i].PrimaryFlag;
            }
            if (SpecSearchDisplaySliced[i].StartDate != null) {
                StartDateDisplay = SpecSearchDisplaySliced[i].StartDate;
            }
            if (SpecSearchDisplaySliced[i].EndDate != null) {
                EndDateDisplay = SpecSearchDisplaySliced[i].EndDate;
            }
            if (SpecSearchDisplaySliced[i].EnrollStatusDesc != null) {
                EnrollStsDisplay = SpecSearchDisplaySliced[i].EnrollStatusDesc;
            }

            table = table + "<tr><td>" + MedIDDisplay + "</td><td>" + PTTypeDisplay + "</td><td>" + SpecialtyDisplay + "</td><td>" + PrimaryDisplay + "</td><td>" + StartDateDisplay + "</td><td>" + EndDateDisplay + "</td><td>" + EnrollStsDisplay + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlSpecSearchResult').innerHTML = '';
        document.getElementById('pnlSpecSearchResult').innerHTML = table;
        setupPagination(totalPages, pagenumber);
    }
    function downloadJson() {
        try {
            // Convert JSON data to worksheet
            const worksheet = XLSX.utils.json_to_sheet(specialtySearchResultData.SpecialtySearchResult);

            // Create a new workbook
            const workbook = XLSX.utils.book_new();

            // Append the worksheet to the workbook
            XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");

            // Write the workbook to a file
            XLSX.writeFile(workbook, "ProviderSpecialtyExport.xlsx");
         
        }
        catch (err) {
            alert(err);
        }
    }

    function filterTable(n) {
        //alert('here');
        Object.filter = (obj, predicate) =>
            Object.fromEntries(Object.entries(obj).
                filter(([key, value]) =>
                    predicate(value)));

        specSearchResultFileterd =
            Object.filter(SpecSearchResults, employee =>
                employee.PrimaryFlag === "No");
        GetSpecialtySearchResults(1);
        //alert(JSON.stringify(specSearchResultFileterd));
    }
</script>

<div>
     <h1> <Legend style="border-bottom:1px solid #65659f;"><span class="pssSectionHeader" id="pssSectionHeader_1" runat="server">Search Criteria</span></Legend> </h1>
</div>

<div class="specSearchresultContainer1 active">
    <div class="content" style="padding-top: 15px">
        <div id="pnlSpecSearchErrResults" style="overflow: auto; width: 100%"></div>
        <div><span tabindex="0" style="font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">NPI or Medicaid ID is required</span></div>
        
        <div class="row">
            <div class="col-sm-2  text-right">
                <label class="formLabel" for="txtNPI"> NPI </label> 

            </div>
            <div class="col-sm-10 text-left">
                <input class="formField" id="txtNPI" name="NPI" value="" max="9999999999" maxlength="10" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" />
                <span class="failureNotification" id="spantxtNPI"></span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-2  text-right">
                <label class="formLabel" for="txtMedID"> Medicaid ID </label>
            </div>
            <div class="col-sm-10 text-left">
                <input class="formField" id="txtMedID" name="Medicaid ID" value="" max="9999999999" maxlength="7" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" />
                <span class="failureNotification" id="spantxtMedID"></span>
            </div>
        </div>
        <div class="row">
            <div class="row-centered col-lg-12" style="padding-top: 30px; text-align: center; padding-bottom: 10px;">
                <button id="btnSearch" type="button" class="btn btn-primary margin-right-n btn-success" style="background-color: #65659f !important; min-width: 200px; height:40px;" onclick="btnProvSpecialtySearch()">Search</button>
                <button id="btnclear" type="button" class="btn btn-default margin-right-n btn-danger " style="background-color: #707070 !important; min-width: 200px; height:40px;" onclick="btnclearSearch()">Clear</button>
                <button id="btnDownload" type="button" class="btn btn-default margin-right-n btn-danger " style="background-color: #707070 !important; min-width: 200px; height:40px;" onclick="downloadJson();">Download</button>
            </div>
        </div>
    </div>
</div>

<div>
     <h1> <Legend style="border-bottom:1px solid #65659f;"><span class="pssSectionHeader1" id="Span1" runat="server">Search Results</span></Legend> </h1>
</div>
<div class="specSearchresultContainer2">
    <div id="divpnlpssLoader" style="display: none">
        <img src='../Images/loader.gif' style='height: 38px; width: 35px;'>
    </div>
    <div id="pnlSpecSearchResult" style="overflow: auto; width: 100%;"></div>
    <div id="psspagination"></div>
    <div id="pnlSearchResultJson"  style="display: none" />
</div>