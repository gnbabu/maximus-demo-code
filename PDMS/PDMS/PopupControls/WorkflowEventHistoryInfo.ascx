<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_WorkflowEventHistoryInfo" Codebehind="WorkflowEventHistoryInfo.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script src="../Scripts/jspdf.umd.min.js"></script>
<script src="../Scripts/jspdf.plugin.autotable.min.js"></script>


<style>
    /* Form field spacing */
    .form-field {
        margin-bottom: 1rem;
    }

    /* Align buttons to the right */
    .btnBox {
        text-align: right;
    }

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

    .hModalHeader {
        background-color: #545487;
        width: 100%;
        display: flex;
        justify-content: space-between;
        padding: 14px 16px;
        color: #ffffff;
        margin-bottom: 8px
    }

        .hModalHeader h3 {
            margin: 0px !important
        }

    .hsearch {
        display: flex;
        align-content: center;
        width: 100%;
        justify-content: end;
        line-height: 2
    }

        .hsearch input {
            max-width: 200px;
            height: 40px;
        }

        .hsearch label {
            padding-right: 8px;
            margin-bottom: 0px;
            display: inline-block;
            color: #4C4C4C;
        }

    .hExport li {
        padding-bottom: 8px;
    }

        .hExport li button {
            width: 90%;
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

    #history-page-info {
        padding-top: 6px;
    }

    .table-responsive {
        min-height: 175px !important;
        overflow-x: auto !important;
    }

    .btn-group button {
        margin-left: 8px !important;
        min-width: auto !important;
        font-size: 2rem;
        padding: 4px 15px;
    }

    #reset-all-filters-history {
        font-size: 2rem;
        padding: 4px 15px
    }

    .gridHistoryTable {
        overflow: auto;
        background: #fff;
        max-height: calc(100vh - 285px);
    }

        .gridHistoryTable thead {
            background: #545486;
            color: #ffffff;
        }

        .gridHistoryTable tbody {
            background: #E8F0F9;
        }

        .gridHistoryTable th {
            padding: 14px !important;
        }

        .gridHistoryTable select {
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

    .pagination > li > a {
        border-width: 0px !important;
        background: transparent !important
    }

    .pagination > .active > a {
        background: transparent !important;
        color: #fff !important;
        background-color: #545487 !important
    }

    @media (max-width: 767px) {
        #reset-all-filters-history {
            margin-top: 5px !important;
        }

        .responsive-buttons-container {
            margin-top: 5px !important;
            margin-left: -6px !important;
        }

        .hsearch {
            justify-content: start !important;
            margin-top: 5px !important;
        }
    }
</style>

<script>

    function pageLoad() {
        try {
            $('[data-toggle="popover"]').popover()
        }
        catch (err) {
            console.log('that popover method does not exist at this point: ' + err);
        }
    }

    const historyColumns = [
        { key: 'NOTE_DATE_TIME', title: 'Date', type: 'text' },
        { key: 'Type', title: 'Type', type: 'text' },
        { key: 'UserName', title: 'User', type: 'text' },
        { key: 'TASK_NAME', title: 'Workflow', type: 'text' },
        { key: 'REG_PAGE_NAME', title: 'Screen', type: 'text' },
        { key: 'NOTE_TEXT', title: 'Notes', type: 'object' },
    ];

    let historyData = [];
    let historyPage = 1;
    let historyPageSize = 5;
    let historySortKey = null;
    let historySortAsc = true;
    let historyFilters = {};
    let visibleHistoryColumns = {}; // { colKey: true/false }
    let historyGlobalSearchText = '';

    historyColumns.forEach(col => {
        visibleHistoryColumns[col.key] = true;
    });

    function buildHistoryColumnVisibilityDropdown() {
        const $dropdown = $('.column-visibility-dropdown-history').empty();

        historyColumns.forEach(col => {
            const $li = $(`
                <li>
                <label style="font-weight: normal;font-size:1.72rem !important;line-height:1">
                    <input type="checkbox" class="column-toggle" data-key="${col.key}" ${visibleHistoryColumns[col.key] ? 'checked' : ''}>
                    ${col.title}
                </label>
                </li>
            `);
            $dropdown.append($li);
        });

        // Bind change events
        $('.column-toggle').off('change').on('change', function () {
            const key = $(this).data('key');
            visibleHistoryColumns[key] = $(this).is(':checked');
            buildHistoryTableHeader();  // Rebuild header to hide/show
            renderHistoryTable();       // Re-render table body
        });
    }

    function fetchHistoryDataFromApi() {
        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();
        var regId = $("[id*=hdnRegId]").val();

        $.ajax({
            type: "GET",
            url: webApiEnrollment + "GetProviderFeedHistoryData?regId=" + regId,
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

                historyData = result;
                historyPage = 1;
                buildHistoryColumnVisibilityDropdown();
                buildHistoryTableHeader();
                renderHistoryTable();
                var finalTable = document.getElementById('table-history-body');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error(errorThrown);
            }
        });
        return false;
    }



    function buildHistoryTableHeader() {
        const $thead = $('#table-history-head').empty();

        historyColumns.forEach(col => {
            if (!visibleHistoryColumns[col.key]) return; // skip hidden columns

            const inputType = col.type === 'number' ? 'number' : col.type === 'date' ? 'date' : 'text';
            const filter = historyFilters[col.key] || {};
            const sortIcon = historySortKey === col.key ? (historySortAsc ? ' ▲' : ' ▼') : '';

            const $th = $('<th></th>');

            const $label = $(`<strong style="cursor:pointer;">${col.title}${sortIcon}</strong>`);
            $label.click(() => {
                historyPage = 1;
                if (historySortKey === col.key) {
                    historySortAsc = !historySortAsc;
                } else {
                    historySortKey = col.key;
                    historySortAsc = true;
                }
                buildHistoryTableHeader();
                renderHistoryTable();
            });

            const $inputGroup = $(`
<div class="input-group input-group-sm" style="margin-top: 5px;">
    <input type="${inputType}" class="form-control filter-input filter-val1" placeholder="Filter..." data-key="${col.key}" value="${filter.value1 || ''}">
    <div class="input-group-btn">
        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
            <span class="glyphicon glyphicon-filter"></span>
        </button>
        <ul class="dropdown-menu dropdown-menu-right p-2" style="padding: 10px;">
            <li>
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
                        <button type="button" class="btn btn-xs btn-primary apply-filter-h resetAttr" data-key="${col.key}">Apply</button>
                    </div>
                    <div class="col-xs-6 text-right" style="padding-right: 0px;padding-top: 10px;">
                        <button type="button" class="btn btn-xs btn-default clear-filter-h resetAttr" data-key="${col.key}">Reset</button>
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
            $dropdown.find('.apply-filter-h').click(function () {
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
                    delete historyFilters[col.key];
                    $val1.val('');
                    $val2.val('').hide();
                    historyPage = 1;
                    renderHistoryTable();
                    $dropdown.parent().removeClass('open');
                    return;
                }

                // Store the raw values
                historyFilters[col.key] = { op, value1, value2 };

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

                historyPage = 1;
                renderHistoryTable();

                // Close dropdown
                $dropdown.parent().removeClass('open');
            });


            // Clear filter
            $dropdown.find('.clear-filter-h').click(function () {
                delete historyFilters[col.key];
                $operator.val('eq');
                $val1.val('');
                $val2.val('').hide();
                historyPage = 1;
                renderHistoryTable();
                $dropdown.parent().removeClass('open');
            });

            $dropdown.on('click', function (e) {
                e.stopPropagation();
            });
            const $wrapper = col.key == 'Notes' ? $('<div style="display: flex; flex-direction: column;min-width:300px !important;max-width:350px !important;"></div>') :
                $('<div style="display: flex; flex-direction: column;min-width:150px !important;max-width:200px !important;"></div>');
            $wrapper.append($label).append($inputGroup);
            $th.append($wrapper);
            $thead.append($th);
        });
    }

    function applyHistoryFilters(historyData) {
        return historyData.filter(row => {
            return Object.entries(historyFilters).every(([key, filter]) => {
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

                        // Try to parse as date (mm/dd/yyyy)
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
        const [mm, dd, yyyy] = str.split('/');
        if (!dd || !mm || !yyyy) return null;
        const date = new Date(`${yyyy}-${mm}/${dd}`);
        return isNaN(date.getTime()) ? null : date;

    }

    function formatValueForSearch_H(value, col) {
        if (value == null) return '';

        if (col.type === 'date') {

            const date = new Date(value);
            if (!isNaN(date.getTime())) {
                const day = String(date.getDate()).padStart(2, '0');
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const year = date.getFullYear();

                return `${year}-${month}-${day}`.toLowerCase();
            }
        }

        return value.toString().toLowerCase();
    }

    function renderHistoryTable() {
        const $tbody = $('#table-history-body').empty();

        let filtered = applyHistoryFilters(historyData);

        // 🔍 Global search filter
        if (historyGlobalSearchText) {
            const search = historyGlobalSearchText?.toLowerCase().trim();
            if (search) {
                filtered = filtered.filter(row =>
                    historyColumns.some(col =>
                        visibleHistoryColumns[col.key] &&
                        formatValueForSearch_H(row[col.key], col).includes(search)
                    )
                );
            }
        }


        if (historySortKey) {
            filtered.sort((a, b) => {
                let valA = a[historySortKey], valB = b[historySortKey];
                if (typeof valA === "string") valA = valA.toLowerCase();
                if (typeof valB === "string") valB = valB.toLowerCase();
                return historySortAsc ? (valA > valB ? 1 : -1) : (valA < valB ? 1 : -1);
            });
        }

        const start = (historyPage - 1) * historyPageSize;
        const paged = filtered.slice(start, start + historyPageSize);

        if (paged.length == 0) {
            const noDataMessageForEventHistoryInfo = 'There are currently no items to display.';
            const colspan = columns.filter(col => visibleColumns[col.key]).length;
            $tbody.append(`<tr><td colspan="${colspan}" style="text-align: center;">${noDataMessageForEventHistoryInfo}</td></tr>`);

        } else {
            paged.forEach(row => {
                const $tr = $('<tr>');

                historyColumns.forEach(col => {
                    if (!visibleHistoryColumns[col.key]) return;
                    let value = row[col.key];
                    if (value === null || value === undefined || value === '') {
                        value = '';
                    }

                    // Format date as mm/dd/yy if column is of type 'date'
                    if (col.type === 'date' && value) {

                        const date = new Date(value);

                        if (!isNaN(date.getTime())) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();
                            value = `${year}-${month}-${day}`;
                        }
                    }

                    if (col.type === 'datetime' && value) {

                        const date = new Date(value);
                        if (!isNaN(date.getTime())) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();

                            const hour = String(date.getHours()).padStart(2, '0');
                            const minute = String(date.getMinutes()).padStart(2, '0');
                            const second = String(date.getSeconds()).padStart(2, '0');
                            value = `${year}-${month}-${day}`;
                        }
                    }

                    if (col.type === 'object' && value) {
                        value = formatObjectHist(value);
                        $tr.append(`<td style="white-space:normal; word-wrap:break-word;min-width:300px !important;max-width:350px !important;">${value}</td>`);
                    } else {
                        $tr.append(`<td style="white-space:normal; word-wrap:break-word;min-width:150px !important;max-width:200px !important;">${value}</td>`);
                    }

                });

                $tbody.append($tr);
            });
        }
        if (filtered.length > 0) {
            $('#history-page-info').text(`Showing ${start + 1} to ${Math.min(start + historyPageSize, filtered.length)} of ${filtered.length} Entries`).show();
            $('#historyPagination').parent().show(); // show pagination container
            renderHistoryPagination(Math.ceil(filtered.length / historyPageSize));
        } else {
            $('#history-page-info').hide();
            $('#historyPagination').parent().hide(); // hide pagination container
        }
    }

    function formatObjectHist(value) {
        if (value == null) return '';
        return Array.isArray(value)
            ? value.map((item) => `-- ${item} <br />`).join('')
            : String(value);
    }


    function goBack() {
        window.history.back();
    }

    function renderHistoryPagination(totalPages) {
        const historyPagination = $('#historyPagination').empty();
        const $firstPage = $('<li><a style="font-size: 20px;" href="#">&lt;&lt;</a></li>');
        $firstPage.click(function (e) {
            e.preventDefault();
            historyPage = 1;
            renderHistoryTable();
        });
        historyPagination.append($firstPage);

        var pageDisplay = Math.floor(historyPage / 10);
        if (historyPage >= 10) {
            const $prevPage = $('<li><a style="font-size: 20px;" href="#">&lt;</a></li>');
            $prevPage.click(function (e) {
                e.preventDefault();
                historyPage = ((pageDisplay) * 10) - 1;
                renderHistoryTable();
            });
            historyPagination.append($prevPage);
        }

        for (let i = 1; i <= totalPages; i++) {
            if ((i >= (pageDisplay * 10)) && (i < ((pageDisplay * 10) + 10))) {
                const $li = $(`<li class="${i === historyPage ? 'active' : ''}"><a style="font-size:20px" href="#">${i}</a></li>`);
                $li.click(function (e) {
                    e.preventDefault();
                    historyPage = i;
                    renderHistoryTable();
                });
                historyPagination.append($li);
            }

            if (i == ((pageDisplay * 10) + 10)) {
                const $nextPage = $(`<li class="${i === historyPage ? 'active' : ''}"><a style="font-size:20px" href="#">&gt;</a></li>`);
                $nextPage.click(function (e) {
                    e.preventDefault();
                    historyPage = i;
                    renderHistoryTable();
                });
                historyPagination.append($nextPage);
            }
        }

        const $lastPage = $('<li><a style="font-size: 20px;" href="#">&gt;&gt;</a></li>');
        $lastPage.click(function (e) {
            e.preventDefault();
            historyPage = totalPages;
            renderHistoryTable();
        });
        historyPagination.append($lastPage);
    }

    $(document).ready(function () {

        $('.hModalHeader').on('click', function () {
            const $content = $(this).next('.hModalContent');// assumes the content to toggle is the next sibling
            const $plus = $(this).find('.plus');
            const $minus = $(this).find('.minus');

            $content.slideToggle(); // toggle visibility with animation
            $plus.toggle();         // toggle plus icon
            $minus.toggle();        // toggle minus icon
        });

        fetchHistoryDataFromApi();

        $('#page-size-dropdown-h').change(function () {
            historyPageSize = parseInt($(this).val());
            historyPage = 1;
            renderHistoryTable();
        });

        $('#reset-all-filters-history').click(function () {

            // Clear the filters object
            historyFilters = {};

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

            Object.keys(visibleHistoryColumns).forEach(key => {
                visibleHistoryColumns[key] = true;
            });

            //Clear global serch
            $('#hsearch').val('');
            historyGlobalSearchText = '';

            $('#page-size-dropdown-h').val('5');
            historyPageSize = 5;
            historyPage = 1;

            fetchHistoryDataFromApi();

            return false;
        });

        $('#copy-btn-h').click(() => {
            const visibleCols = historyColumns.filter(c => visibleHistoryColumns[c.key]);
            const filteredData = applyHistoryFilters(historyData); // get your filtered data

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


        $('#excel-btn-h').click(() => {
            const visibleCols = historyColumns.filter(c => visibleHistoryColumns[c.key]);
            const filteredData = applyHistoryFilters(historyData);

            let csvContent = '';
            csvContent += visibleCols.map(col => `"${col.title}"`).join(',') + '\n';

            filteredData.forEach(row => {
                historyColumns.forEach(col => {
                    if (!visibleHistoryColumns[col.key]) return;
                    let value = row[col.key];
                    // Format date as mm/dd/yy if column is of type 'date'
                    if (col.type === 'date' && value) {
                        const date = new Date(value);
                        if (!isNaN(date.getTime())) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();
                            value = `${month}/${day}/${year}`;
                        }
                    }

                    if (col.type === 'datetime' && value) {
                        const date = new Date(value);
                        if (!isNaN(date.getTime())) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();

                            const hour = String(date.getHours()).padStart(2, '0');
                            const minute = String(date.getMinutes()).padStart(2, '0');
                            const second = String(date.getSeconds()).padStart(2, '0');
                            value = `${month}/${day}/${year} ${hour}:${minute}:${second}`;
                        }
                    }

                    csvContent += String(value).replace(/<br\s*\/?>/gi, '|') + ',';
                });
                csvContent += '\n';
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

        $('#pdf-btn-h').click(() => {
            const { jsPDF } = window.jspdf;
            const doc = new jsPDF('l', 'pt', 'a4');

            const visibleCols = historyColumns.filter(c => visibleHistoryColumns[c.key]);
            const filteredData = applyHistoryFilters(historyData);

            const head = [visibleCols.map(col => col.title)];
            // const body = filteredData.map(row => visibleCols.map(col => row[col.key] ?? ''));
            var body = [];
            var rowValues = [];

            filteredData.forEach(row => {
                historyColumns.forEach(col => {
                    if (!visibleHistoryColumns[col.key]) return;
                    let value = row[col.key];

                    if (col.type === 'date' && value) {
                        const date = new Date(value);
                        if (!isNaN(date.getTime())) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();
                            value = `${month}/${day}/${year}`;
                        }
                    }

                    if (col.type === 'datetime' && value) {
                        const date = new Date(value);
                        if (!isNaN(date.getTime())) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();

                            const hour = String(date.getHours()).padStart(2, '0');
                            const minute = String(date.getMinutes()).padStart(2, '0');
                            const second = String(date.getSeconds()).padStart(2, '0');
                            value = `${month}/${day}/${year} ${hour}:${minute}:${second}`;
                        }
                    }
                    rowValues.push(String(value).replace(/<br\s*\/?>/gi, '\n'));
                });
                body.push(rowValues);
            });

            doc.autoTable({
                head: head,
                body: body,
                startY: 10,
                styles: { fontSize: 6 },
            });
            doc.save('export.pdf');

            return false;
        });

        $('#hsearch').on('input', function () {
            historyGlobalSearchText = $(this).val().toLowerCase();
            historyPage = 1;
            renderHistoryTable();
        });
    });
</script>

<div runat="server">

    <asp:Panel runat="server">

        <div class="container-fluid">

            <div class="hModalHeader d-flex justify-content-between align-items-center">
                <h3 class="m-0"><strong>Historic Notes Archive 10/01/2022 - 08/12/2025 - Read Only</strong> </h3>
                <div class="toggle-icons">
                    <span class="plus" style="display: none;">+</span>
                    <span class="minus">−</span>
                </div>
            </div>
            <div class="hModalContent" style="display: block;">
                <!-- Page Size Dropdown -->
                <div class="row">
                    <div class="col-sm-9">
                        <label class="control-label" style="margin-top: 5px;">
                            Rows per page:
                        </label>
                        <select id="page-size-dropdown-h" class="form-control input-md" style="width: 75px !important; min-width: 75px !important; height: 38px; display: inline-block; margin-left: 5px;">
                            >
                        <option selected>5</option>
                            <option>10</option>
                            <option>20</option>
                            <option>50</option>
                            <option>100</option>
                        </select>

                        <div class="btn-group d-flex flex-wrap responsive-buttons-container">
                            <button type="button" class="btn btn-default dropdown-toggle dcBtn" data-toggle="dropdown">
                                Display Columns <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu column-visibility-dropdown-history" style="padding: 10px;">
                                <!-- Populated dynamically -->
                            </ul>
                            <div class="btn-group d-flex flex-wrap" role="group">
                                <div class="dropup">

                                    <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        Export <span class="caret"></span>
                                    </button>

                                    <ul class="dropdown-menu  dropdown-menu-right hExport" style="top: 100%; right: auto; height: fit-content;">
                                        <li>
                                            <button class="btn btn-sm btn-primary" id="copy-btn-h">Copy</button>
                                        </li>
                                        <li>
                                            <button class="btn btn-sm btn-danger" id="pdf-btn-h">Pdf</button>
                                        </li>
                                        <li>
                                            <button class="btn btn-sm btn-success" id="excel-btn-h">Excel</button>

                                        </li>
                                    </ul>
                                </div>

                            </div>
                            <button id="reset-all-filters-history" class="btn btn-danger">Reset All Filters</button>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group hsearch">
                            <span class="ohio-tooltip"
                                data-toggle="popover"
                                data-trigger="hover"
                                data-placement="top"
                                data-content="<asp:Literal ID='HISTORY_POPUP_TEXT' runat='server' Text='For the Date to return results in the Search, the date entered must match the format in the table below' />"
                                aria-hidden="true">
                                <label for="hsearch">Search</label>
                            </span>
                            <input type="text" class="form-control" id="hsearch" placeholder="Search" />

                        </div>
                    </div>
                </div>
                <!-- Table -->
                <div class="gridHistoryTable table-responsive">
                    <table class="table table-bordered table-striped">
                        <thead>
                            <tr id="table-history-head"></tr>
                        </thead>
                        <tbody id="table-history-body"></tbody>
                    </table>
                </div>
                <!-- Pagination -->
                <div class="row" style="margin-top: 10px;">
                    <div class="col-sm-6">
                        <div id="history-page-info" class="text-muted" style="padding-top: 6px;"></div>
                    </div>
                    <div class="col-sm-6 text-right">
                        <ul class="pagination pagination-sm" id="historyPagination" style="margin: 0; display: inline-block;"></ul>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hdnAccessTokens" runat="server" />
    <asp:HiddenField ID="hdnWebAPIURLs" runat="server" />
    <asp:HiddenField ID="hdnRegIds" runat="server" />
</div>
