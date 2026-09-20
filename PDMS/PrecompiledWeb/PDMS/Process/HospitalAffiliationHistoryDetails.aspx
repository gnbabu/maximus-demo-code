<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_AffiliationHistoryDetails, App_Web_14kfymdt" enableEventValidation="false" stylesheettheme="Default" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/RegistrationNavigation.ascx" TagName="RegistrationNavigation" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script src="../Scripts/jspdf.umd.min.js"></script>
    <script src="../Scripts/jspdf.plugin.autotable.min.js"></script>

    <style>
        .table-striped > tbody > tr:nth-child(odd) > td,
        .table-striped > tbody > tr:nth-child(odd) > th {
            text-align: left;
            vertical-align: top;
            background-color: #EAEFF7;
        }

        .table-striped > tbody > tr:nth-child(even) > td,
        .table-striped > tbody > tr:nth-child(even) > th {
            background-color: #E8F0F9;
            text-align: left;
            vertical-align: top;
        }

        .backbutton {
            background-color: #007BFF;
            color: white;
            padding: 10px 20px;
            font-size: 16px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        backbutton:hover {
            background-color: #45a049;
        }

        .whModalHeader {
            background-color: #545487;
            width: 100%;
            display: flex;
            justify-content: space-between;
            padding: 14px 16px;
            color: #ffffff;
            margin-bottom: 8px
        }

        .whModalHeader h3 {
            margin: 0px !important
        }

        .whsearch {
            display: flex;
            align-content: center;
            width: 100%;
            justify-content: end;
            line-height: 2
        }

        .whsearch input {
            max-width: 200px;
            height: 40px;
        }

        .whsearch label {
            padding-right: 8px;
            margin-bottom: 0px;
            display: inline-block;
        }

        .whExport li {
            padding-bottom: 8px;
        }

        .whExport li button {
            width: 90%;
        }

        .pagination > li > a {
            border-width: 0px !important;
            background: transparent !important
        }

        .pagination > .active > a {
            background: transparent !important;
            color: #fff !important;
            background-color: #545487 !important
        }
    </style>


    <style>
        /* Prevent input text boxes in header from stretching */
        th > div {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
        }

        input.form-control.input-sm {
            max-width: 150px;
            font-size: 12px;
            padding: 3px 6px;
        }

        .table > thead > tr > th,
        .table > tbody > tr > td {
            white-space: nowrap;
            vertical-align: middle;
        }

        #page-info {
            padding-top: 6px;
        }

        #pagination {
            margin: 0;
        }

        .table-responsive {
            overflow-x: auto;
        }

        .btn-group button {
            margin-left: 8px !important;
            min-width: auto !important;
            font-size: 2rem;
            padding: 4px 15px;
        }

        #reset-all-filters {
            font-size: 2rem;
            padding: 4px 15px
        }

        .gridTable {
            overflow: auto;
            background: #fff;
            max-height: calc(100vh - 285px);
        }

        .gridTable thead {
            background: #545486;
            color: #ffffff;
        }

        .gridTable tbody {
             background: #E8F0F9;
        }

        .gridTable th {
            padding: 14px !important;
        }

        .gridTable select {
             min-width: inherit !important;
        }

        .resetAttr {
            width: 56px !important
        }

        .dcBtn {
            border-color: #808080 !important
        }

        input.column-toggle {
            width: 32px !important;
            vertical-align: middle !important;
            height: 25px !important;
        }
    </style>

    <script>
        const columns = [
            { key: 'Operation', title: 'Operation', type: 'text' },
            { key: 'IsInpatientSetting', title: 'Do you practice exclusively within the inpatiend setting?', type: 'text' },
            { key: 'IsHospitalPrivileges', title: 'Do you have hospital privileges?', type: 'text' },
            { key: 'HospitalPrivilegesReason', title: 'If No, Please Specify', type: 'text' },
            { key: 'Is_Primary_Facility', title: 'Is this your primary facility?', type: 'text' },
            { key: 'FacilityMedicaidID', title: 'OH Medicaid ID', type: 'text' },
            { key: 'FacilityName', title: 'Facility Name', type: 'text' },
            { key: 'StaffCategory', title: 'Staff Category', type: 'text' },
            { key: 'StatusOfPrivileges', title: 'Status of Privileges', type: 'text' },
            { key: 'StartDate', title: 'Start Date', type: 'date' },
            { key: 'EndDate', title: 'End Date', type: 'date' },
            { key: 'IsRestrictedPrivilege', title: 'Any Past or Present Restriction of Privilege?', type: 'text' },
            { key: 'Reason', title: 'If Yes, Please Specify', type: 'text' },
            { key: 'UserName', title: 'User Name', type: 'text' },
            { key: 'DateOfAction', title: 'Update Date', type: 'date' }
        ];

        let data = [];
        let page = 1;
        let pageSize = 10;
        let sortKey = null;
        let sortAsc = true;
        let filters = {};
        let visibleColumns = {}; // { colKey: true/false }
        let globalSearchText = '';

        columns.forEach(col => {
            visibleColumns[col.key] = true;
        });

        function buildColumnVisibilityDropdown() {
            const $dropdown = $('.column-visibility-dropdown').empty();

            columns.forEach(col => {
                const $li = $(`
                  <li>
                    <label style="font-weight: normal;font-size:1.72rem !important;line-height:1">
                      <input type="checkbox" class="column-toggle" data-key="${col.key}" ${visibleColumns[col.key] ? 'checked' : ''}>
                      ${col.title}
                    </label>
                  </li>
                `);
                $dropdown.append($li);
            });

            // Bind change events
            $('.column-toggle').off('change').on('change', function () {
                const key = $(this).data('key');
                visibleColumns[key] = $(this).is(':checked');
                buildTableHeader();  // Rebuild header to hide/show
                renderTable();       // Re-render table body
            });
        }

        function fetchDataFromApi() {

            var APIToken = $("[id*=hdnAccessToken]").val();

            let urlParams = new URLSearchParams(window.location.search);
            let regId = urlParams.get("regId");

            $.ajax({
                type: "GET",
                url: webApiEnrollment + "GetHealthCareAffiliationHistoryData?regId=" + regId,
                async: false,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    data = result;
                    page = 1;
                    buildColumnVisibilityDropdown();
                    buildTableHeader();
                    renderTable();
                    var finalTable = document.getElementById('table-body');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("Error loading data.");
                    console.error(err);
                }
            });
            return false;
        }

        function buildTableHeader() {
            const $thead = $('#table-head').empty();

            columns.forEach(col => {

                if (!visibleColumns[col.key]) return; // skip hidden columns

                const inputType = col.type === 'number' ? 'number' : col.type === 'date' ? 'date' : 'text';
                const filter = filters[col.key] || {};
                const sortIcon = sortKey === col.key ? (sortAsc ? ' ▲' : ' ▼') : '';

                const $th = $('<th></th>');

                const $label = $(`<strong style="cursor:pointer;">${col.title}${sortIcon}</strong>`);
                $label.click(() => {
                    if (sortKey === col.key) {
                        sortAsc = !sortAsc;
                    } else {
                        sortKey = col.key;
                        sortAsc = true;
                    }
                    buildTableHeader();
                    renderTable();
                });

                const $inputGroup = $(`
    <div class="input-group input-group-sm" style="margin-top: 5px;">
        <input type="${inputType}" class="form-control filter-input filter-val1" placeholder="Filter..." data-key="${col.key}" value="${filter.value1 || ''}">
        <div class="input-group-btn">
            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                <span class="glyphicon glyphicon-filter"></span>
            </button>
            <ul class="dropdown-menu dropdown-menu-right p-2" style="padding: 10px;">
                <li style="margin-bottom: 8px;">
                    <select class="form-control input-sm filter-operator" data-key="${col.key}">
                        <option value="eq">Equal</option>
                        <option value="neq">Not Equal</option>
                        ${col.type === 'number' || col.type === 'date'
                        ? `<option value="lt">Less Than</option>
                               <option value="gt">Greater Than</option>
                               <option value="lte">Less Than or Equal</option>
                               <option value="gte">Greater Than or Equal</option>
                               <option value="between">Between</option>`
                        : `<option value="contains">Contains</option>
                               <option value="startsWith">Starts With</option>
                               <option value="endsWith">Ends With</option>`}
                    </select>
                </li>
                <li style="margin-top: 5px;">
                    <input type="${inputType}" class="form-control input-sm filter-val2" placeholder="Second value (for between)" style="display: none;" value="${filter.value2 || ''}">
                </li>
                <li style="margin-top: 10px;">
                    <div class="row" style="margin: 0;">
                        <div class="col-xs-6 text-left" style="padding-left: 0px;padding-top: 10px;">
                            <button type="button" class="btn btn-xs btn-primary apply-filter resetAttr" data-key="${col.key}">Apply</button>
                        </div>
                        <div class="col-xs-6 text-right" style="padding-right: 0px;padding-top: 10px;">
                            <button type="button" class="btn btn-xs btn-default clear-filter resetAttr" data-key="${col.key}">Reset</button>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
`);

                const $dropdown = $inputGroup.find('.dropdown-menu');
                const $operator = $dropdown.find('.filter-operator');
                const $val1 = $inputGroup.find('.filter-val1');
                const $val2 = $dropdown.find('.filter-val2');

                // Toggle second value input
                $operator.on('change', function () {
                    if ($(this).val() === 'between') {
                        $val2.show();
                    } else {
                        $val2.hide().val('');
                    }
                });

                if (filter.op === 'between') {
                    $operator.val('between');
                    $val2.show();
                } else if (filter.op) {
                    $operator.val(filter.op);
                }

                // Apply filter
                $dropdown.find('.apply-filter').click(function () {
                    const op = $operator.val();
                    const value1 = $val1.val().trim();
                    const value2 = op === 'between' ? $val2.val().trim() : '';

                    if (op === 'between') {
                        if (!value1 || !value2) {
                            alert("Please enter both values for 'Between' filter.");
                            return;
                        }
                    } else if (!value1) {
                        // If value1 is empty, don't apply the filter
                        delete filters[col.key];
                        $val1.val('');
                        $val2.val('').hide();
                        page = 1;
                        renderTable();
                        $dropdown.parent().removeClass('open');
                        return;
                    }

                    // Store the raw values
                    filters[col.key] = { op, value1, value2 };

                    // For non-'between' operators, you could update the input if needed.
                    // For 'between', leave the inputs unchanged.
                    if (op !== 'between') {
                        $val1.val(value1);
                    }
                    // Optionally, if you want to display a summary without changing the input,
                    // you could add an extra element in your dropdown and update it like this:
                    // else {
                    //     $dropdown.find('.filter-summary').text(`${value1} ~ ${value2}`);
                    // }

                    page = 1;
                    renderTable();

                    // Close dropdown
                    $dropdown.parent().removeClass('open');
                });


                // Clear filter
                $dropdown.find('.clear-filter').click(function () {
                    delete filters[col.key];
                    $operator.val('eq');
                    $val1.val('');
                    $val2.val('').hide();
                    page = 1;
                    renderTable();
                    $dropdown.parent().removeClass('open');
                });

                $dropdown.on('click', function (e) {
                    e.stopPropagation();
                });

                const $wrapper = $('<div style="display: flex; flex-direction: column;min-width:162px"></div>');
                $wrapper.append($label).append($inputGroup);
                $th.append($wrapper);
                $thead.append($th);
            });
        }

        function applyFilters(data) {
            return data.filter(row => {
                return Object.entries(filters).every(([key, filter]) => {
                    const rawVal = row[key];
                    const val = rawVal !== null && rawVal !== undefined ? rawVal.toString().toLowerCase() : '';
                    const value1 = (filter.value1 || '').toLowerCase();
                    const value2 = (filter.value2 || '').toLowerCase();

                    switch (filter.op) {
                        case 'eq': return val === value1;
                        case 'neq': return val !== value1;
                        case 'lt': return val < value1;
                        case 'gt': return val > value1;
                        case 'lte': return val <= value1;
                        case 'gte': return val >= value1;
                        case 'between': {
                            if (!filter.value1 || !filter.value2) return false;

                            // Try to parse as number
                            const numVal = parseFloat(val);
                            const numVal1 = parseFloat(filter.value1);
                            const numVal2 = parseFloat(filter.value2);

                            if (!isNaN(numVal) && !isNaN(numVal1) && !isNaN(numVal2)) {
                                return numVal >= numVal1 && numVal <= numVal2;
                            }

                            // Try to parse as date (dd-mm-yyyy)
                            const dateVal = parseDate(val);
                            const dateVal1 = parseDate(filter.value1);
                            const dateVal2 = parseDate(filter.value2);

                            if (dateVal && dateVal1 && dateVal2) {
                                return dateVal >= dateVal1 && dateVal <= dateVal2;
                            }

                            return false;
                        }
                        case 'contains': return val.includes(value1);
                        case 'startsWith': return val.startsWith(value1);
                        case 'endsWith': return val.endsWith(value1);
                        default: return true;
                    }
                });
            });
        }

        function parseDate(str) {
            const [dd, mm, yyyy] = str.split('-');
            if (!dd || !mm || !yyyy) return null;
            const date = new Date(`${yyyy}-${mm}-${dd}`);
            return isNaN(date.getTime()) ? null : date;
        }

        function formatValueForSearch(value, col) {
            if (value == null) return '';

            // If it's a date column, format as dd-mm-yyyy
            if (col.type === 'date') {
                const date = new Date(value);
                if (!isNaN(date.getTime())) {
                    const day = String(date.getDate()).padStart(2, '0');
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const year = date.getFullYear();
                    return `${day}-${month}-${year}`.toLowerCase();
                }
            }

            // Fallback: treat as string
            return value.toString().toLowerCase();
        }

        function renderTable() {
            const $tbody = $('#table-body').empty();

            let filtered = applyFilters(data);

            // 🔍 Global search filter
            if (globalSearchText) {
                const search = globalSearchText?.toLowerCase().trim();
                if (search) {
                    filtered = filtered.filter(row =>
                        columns.some(col =>
                            visibleColumns[col.key] &&
                            formatValueForSearch(row[col.key], col).includes(search)
                        )
                    );
                }
            }


            if (sortKey) {
                filtered.sort((a, b) => {
                    let valA = a[sortKey], valB = b[sortKey];
                    if (typeof valA === "string") valA = valA.toLowerCase();
                    if (typeof valB === "string") valB = valB.toLowerCase();
                    return sortAsc ? (valA > valB ? 1 : -1) : (valA < valB ? 1 : -1);
                });
            }

            const start = (page - 1) * pageSize;
            const paged = filtered.slice(start, start + pageSize);

            paged.forEach(row => {

                const $tr = $('<tr>');
                columns.forEach(col => {

                    if (!visibleColumns[col.key]) return;

                    let value = row[col.key];

                    if (value === null || value === undefined || value === '') {
                        value = '';
                    }

                    // Format date as dd-mm-yyyy if column is of type 'date'
                    if (col.type === 'date' && value) {
                        const date = new Date(value);
                        if (!isNaN(date.getTime())) {
                            const day = String(date.getDate()).padStart(2, '0');
                            if (!isNaN(date.getTime())) {
                                const day = String(date.getDate()).padStart(2, '0');
                                var month = String(date.getMonth() + 1).padStart(2, '0');
                                if (month == '01') {
                                    month = 'JAN';
                                }
                                if (month == '02') {
                                    month = 'FEB';
                                }
                                if (month == '03') {
                                    month = 'MAR';
                                }
                                if (month == '04') {
                                    month = 'APR';
                                }
                                if (month == '05') {
                                    month = 'MAY';
                                }
                                if (month == '06') {
                                    month = 'JUN';
                                }
                                if (month == '07') {
                                    month = 'JUL';
                                }
                                if (month == '08') {
                                    month = 'AUG';
                                }
                                if (month == '09') {
                                    month = 'SEP';
                                }
                                if (month == '10') {
                                    month = 'OCT';
                                }
                                if (month == '11') {
                                    month = 'NOV';
                                }
                                if (month == '12') {
                                    month = 'DEC';
                                }
                                const year = date.getFullYear();
                                value = `${year}-${month}-${day}`;
                            }
                        }
                    }

                    $tr.append(`<td>${value}</td>`);
                });
                $tbody.append($tr);
            });


            if (filtered.length > 0) {
                $('#page-info').text(`Showing ${start + 1} to ${Math.min(start + pageSize, filtered.length)} of ${filtered.length} Entries`).show();
                $('#pagination').parent().show(); // show pagination container
                renderPagination(Math.ceil(filtered.length / pageSize));
            } else {
                $('#page-info').hide();
                $('#pagination').parent().hide(); // hide pagination container
            }
        }

        function goBack() {
            window.history.back();
        }

        function renderPagination(totalPages) {
            const $pagination = $('#pagination').empty();
            for (let i = 1; i <= totalPages; i++) {
                const $li = $(`<li class="${i === page ? 'active' : ''}"><a style="font-size:20px" href="#">${i}</a></li>`);
                $li.click(function (e) {
                    e.preventDefault();
                    page = i;
                    renderTable();
                });
                $pagination.append($li);
            }
        }

        $(document).ready(function () {
            fetchDataFromApi();

            $('#page-size-dropdown').change(function () {
                pageSize = parseInt($(this).val());
                page = 1;
                renderTable();
            });

            $('#reset-all-filters').click(function () {

                // Clear the filters object
                filters = {};

                // For each column filter input group
                $('.filter-input').each(function () {
                    $(this).val('');
                });

                $('.filter-operator').each(function () {
                    $(this).val('eq'); // default operator
                });

                $('.filter-val2').each(function () {
                    $(this).val('').hide();
                });

                // Close any open dropdowns
                $('.dropdown').removeClass('open');

                Object.keys(visibleColumns).forEach(key => {
                    visibleColumns[key] = true;
                });

                //Clear global serch
                $('#whsearch').val('');
                globalSearchText = '';

                $('#page-size-dropdown').val('10');
                pageSize = 10;
                page = 1;

                fetchDataFromApi();

                return false;
            });

            $('#copy-btn').click(() => {
                const visibleCols = columns.filter(c => visibleColumns[c.key]);
                const filteredData = applyFilters(data); // get your filtered data

                let copyText = '';
                // Header row
                copyText += visibleCols.map(col => col.title).join('\t') + '\n';

                // Data rows
                filteredData.forEach(row => {
                    copyText += visibleCols.map(col => row[col.key] ?? '').join('\t') + '\n';
                });

                // Copy to clipboard via hidden textarea
                const $tempTextArea = $('<textarea>');
                $tempTextArea.val(copyText)
                    .css({ position: 'absolute', top: '-9999px', left: '-9999px', opacity: 0 })
                    .appendTo('body');

                $tempTextArea[0].focus();
                $tempTextArea[0].select();

                try {
                    const successful = document.execCommand('copy');
                    alert(successful ? 'Copied to clipboard!' : 'Copy failed.');
                } catch (err) {
                    alert('Copy failed.');
                }

                $tempTextArea.remove();
            });

            $('#excel-btn').click(() => {
                const visibleCols = columns.filter(c => visibleColumns[c.key]);
                const filteredData = applyFilters(data);

                let csvContent = '';
                csvContent += visibleCols.map(col => `"${col.title}"`).join(',') + '\n';

                filteredData.forEach(row => {
                    csvContent += visibleCols.map(col => `"${row[col.key] ?? ''}"`).join(',') + '\n';
                });

                const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
                const url = URL.createObjectURL(blob);

                const link = document.createElement("a");
                link.setAttribute("href", url);
                link.setAttribute("download", "export.csv");
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                return false;
            });

            $('#pdf-btn').click(() => {
                const { jsPDF } = window.jspdf;
                const doc = new jsPDF();

                const visibleCols = columns.filter(c => visibleColumns[c.key]);
                const filteredData = applyFilters(data);

                const head = [visibleCols.map(col => col.title)];
                const body = filteredData.map(row => visibleCols.map(col => row[col.key] ?? ''));

                doc.autoTable({
                    head: head,
                    body: body,
                    startY: 10,
                    styles: { fontSize: 8 },
                });

                doc.save('export.pdf');

                return false;
            });

            $('#whsearch').on('input', function () {
                globalSearchText = $(this).val().toLowerCase();
                page = 1;
                renderTable();
            });
        });
    </script>

    <div class="container-fluid" style="min-height: 300px;">
        <br />
        <br />
        <br />

        <div class="whModalHeader">
            <h3 class="m-0"><strong>Affiliation History</strong> </h3>
            <img src="../Images/close-button.png" style="cursor: pointer;" onclick="goBack()" alt="Close button Icon" class="hclose-btn" />
        </div>
        <!-- Page Size Dropdown -->
        <div class="row" style="margin-bottom: 10px;">
            <div class="col-sm-9">
                <label class="control-label" style="margin-top: 5px;">
                    Rows per page:
                </label>
                <select id="page-size-dropdown" class="form-control input-md" style="width: auto; min-width: 60px; height: 38px; display: inline-block; margin-left: 10px;">
                    <option>5</option>
                    <option selected>10</option>
                    <option>20</option>
                    <option>50</option>
                    <option>100</option>
                </select>

                <div class="btn-group">
                    <button type="button" class="btn btn-default dropdown-toggle dcBtn" data-toggle="dropdown">
                        Display Columns <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu column-visibility-dropdown" style="padding: 10px;">
                        <!-- Populated dynamically -->
                    </ul>
                    <div class="btn-group" role="group">
                        <div class="dropup">

                            <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                Export<span class="caret"></span>
                            </button>

                            <ul class="dropdown-menu  dropdown-menu-right whExport" style="top: 100%; right: auto; height: fit-content;">
                                <li>
                                    <button class="btn btn-sm btn-primary" id="copy-btn">Copy</button>
                                </li>
                                <li>
                                    <button class="btn btn-sm btn-danger" id="pdf-btn">Pdf</button>
                                </li>
                                <li>
                                    <button class="btn btn-sm btn-success" id="excel-btn">Excel</button>

                                </li>
                            </ul>
                        </div>

                    </div>
                    <button id="reset-all-filters" class="btn btn-danger">Reset All Filters</button>
                </div>



            </div>
            <div class="col-sm-3">
                <div class="form-group whsearch">
                    <label for="whsearch">Search</label>
                    <input type="text" class="form-control" id="whsearch" placeholder="Search" />
                </div>
            </div>
        </div>

        <!-- Table -->
        <div class="gridTable">
            <table class="table table-bordered table-striped">
                <thead>
                    <tr id="table-head"></tr>
                </thead>
                <tbody id="table-body"></tbody>
            </table>
        </div>

        <!-- Pagination -->
        <div class="row" style="margin-top: 10px;">
            <div class="col-sm-6">
                <div id="page-info" class="text-muted" style="padding-top: 6px;"></div>
            </div>
            <div class="col-sm-6 text-right">
                <ul class="pagination pagination-sm" id="pagination" style="margin: 0; display: inline-block;"></ul>
            </div>
        </div>
        <div class="footerBtn text-right" style="margin-top: 6px;">
            <div class="btn-group" role="group">
                                <button id="whOk" style="width: 100px;" class="btn btn-info" onclick="goBack(); return false;">Ok</button>


            </div>
        </div>

    </div>
</asp:Content>


