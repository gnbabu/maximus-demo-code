<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_DynamicControlsInfo" Codebehind="DynamicControlInfo.ascx.cs" %>
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

    input.column-toggle1 {
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
    fndisableApproveButttons(true);
    let selectedHistoryRowIds = [];
    function pageLoad() {
        try {
            $('[data-toggle="popover"]').popover()
        }
        catch (err) {
            console.log('that popover method does not exist at this point: ' + err);
        }
    }

    const historyColumns = [
        //{ key: 'ProviderType', title: 'Provider Type', type: 'text' },
        { key: 'RegID', title: 'Registration ID', type: 'text' },
        { key: 'ProviderType', title: 'Provider Type', type: 'text' },
        { key: 'SectionType', title: 'Section Type', type: 'text' },
        { key: 'FieldName', title: 'Field Name', type: 'text' },
        { key: 'DataType', title: 'Data Type', type: 'text' },
        { key: 'ControlType', title: 'Control Type', type: 'text' },
        { key: 'ControlLevel', title: 'Control Level', type: 'text' },
        { key: 'ControlId', title: 'Control Id', type: 'text' },
        { key: 'IsActive', title: 'Is Active', type: 'text' },
        { key: 'SelectedValues', title: 'Selected Values', type: 'text' },
        { key: 'Record_Status', title: 'Record Status', type: 'text' },
        { key: 'Approval_Date', title: 'ApprovalDate', type: 'date' },
        { key: 'Review_Date', title: 'ReviewDate', type: 'date' },
        //{ key: 'ModifiedUser', title: 'Modified User', type: 'text' },
        //{ key: 'ModifiedDate', title: 'Modified Date', type: 'text' }
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

    function updateHistDeleteButtonState() {
        $('#delete-selected-hist').prop('disabled', selectedHistoryRowIds.length === 0);
    }

    function deleteHistControls(selectedIds) {
        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();

       if (!selectedIds || selectedIds.length === 0) {
            alert("No records selected for deletion.");
            return;
        }

        //showLoader();
        const payload = {
            ids: selectedHistoryRowIds
        };
        $.ajax({
            type: "POST",
            url: webApiEnrollment + "DeleteDynamicFieldConfig",
            async: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(payload),
            success: function (result) {
                // Assuming result contains updated UIData or success message
                // If API does not return updated data, remove deleted items from UIData locally
                if (Array.isArray(result)) {
                    UIData = result;
                } else if (result && result.deletedCount !== undefined) {
                    UIData = UIData.filter(row => !selectedIds.includes(row.Id));
                }

                UIPage = 1;

                hideLoader();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                hideLoader();
                console.error(errorThrown);
                alert("Failed to delete records: " + errorThrown);
            }
        });

        return false;
    }

    function buildHistoryColumnVisibilityDropdown() {
        const $dropdown = $('.column-visibility-dropdown-history').empty();

        historyColumns.forEach(col => {
            const $li = $(`
                <li>
                <label style="font-weight: normal;font-size:1.72rem !important;line-height:1">
                    <input type="checkbox" class="column-toggle1" data-key="${col.key}" ${visibleHistoryColumns[col.key] ? 'checked' : ''}>
                    ${col.title}
                </label>
                </li>
            `);
            $dropdown.append($li);
        });

        // Bind change events
        $('.column-toggle1').off('change').on('change', function () {
            const key = $(this).data('key');
            visibleHistoryColumns[key] = $(this).is(':checked');
            updateHistDeleteButtonState();
            buildHistoryTableHeader();
            renderHistoryTable();

        });
    }



    //$(document).on('click', '#bulk-approve-history', function (e) {

    //    if (selectedHistoryRowIds.length === 0) {
    //        alert("Please select at least one row to approve.");
    //        return;
    //    }
    //    fnSaveDynamicsFieldsBulk(selectedHistoryRowIds, 1);
    //});
    $(document).on('click', '#bulk-disable-history', function (e) {


        if (selectedHistoryRowIds.length === 0) {
            alert("Please select at least one row to disable.");
            return;
        }
        fnSaveDynamicsFieldsBulk(selectedHistoryRowIds, 0);
    });

    function fnSaveDynamicsFieldsBulk(ids, status) {


        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();

        var UpdateDynamicControls = {
            Ids: ids,
            Status: status
        }
        var inputData = JSON.stringify(UpdateDynamicControls);

        /*showLoader();*/
        $.ajax({
            type: "POST",
            url: webApiEnrollment + "UpdateDynamicControlStatus",
            async: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: inputData,
            success: function () {

                historyPage = 1;
                /*hideLoader();*/
                fetchDynamicControlsDataFromApi();
            },
            error: function (jqXHR, textStatus, errorThrown) {

                /* hideLoader();*/
                console.error(errorThrown);
            }
        });
        selectedHistoryRowIds = [];
        fndisableApproveButttons(true);
        $('#select-all-history').prop('checked', false);
        return false;
    }

    function formatDateTimeDynamic(value) {
        if (!value) return '';

        const date = new Date(value);
        if (isNaN(date)) return '';

        const dd = String(date.getDate()).padStart(2, '0');
        const mm = String(date.getMonth() + 1).padStart(2, '0');
        const yyyy = date.getFullYear();
        const hh = String(date.getHours()).padStart(2, '0');
        const min = String(date.getMinutes()).padStart(2, '0');
        const ss = String(date.getSeconds()).padStart(2, '0');

        return `${dd}-${mm}-${yyyy} ${hh}:${min}:${ss}`;
    }

    function fndisableApproveButttons(flag) {

        const disableButton = document.getElementById('bulk-disable-history');
        if (disableButton)
            disableButton.disabled = flag;

        const approveButton = document.getElementById('bulk-approve-history');
        if (approveButton)
            approveButton.disabled = flag;

    }

    function fetchDynamicControlsDataFromApi() {
        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();
        /*showLoader();*/

        $.ajax({
            type: "GET",
            url: webApiEnrollment + "GetDynamicControlsData",
            async: true,
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

                /*hideLoader();*/
            },
            error: function (jqXHR, textStatus, errorThrown) {

                /*hideLoader();*/
                console.error(errorThrown);
            }
        });
        return false;
    }

    function updateDynamicApprovalStatus(act) {
        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();


        if (!selectedHistoryRowIds || selectedHistoryRowIds.length === 0) {
            alert("No records selected for configuration.");

            return;
        }

        const payload = {
            Ids: selectedHistoryRowIds,
            Status: act
        };

        $.ajax({
            type: "POST",
            url: webApiEnrollment + "UpdateDynamicConfigApproval",
            async: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(payload),
            success: function (result) {
                // Assuming result contains updated UIData or success message
                // If API does not return updated data, remove deleted items from UIData locally
                if (Array.isArray(result)) {
                    UIData = result;
                } else if (result && result.deletedCount !== undefined) {
                    UIData = UIData.filter(row => !selectedIds.includes(row.Id));
                }

                UIPage = 1;
                fetchDynamicControlsDataFromApi();

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error(errorThrown);
                alert("Failed to update records: " + errorThrown);
            }
        });

    }

    function buildHistoryTableHeader() {
        const $thead = $('#table-history-head').empty();
        const $tr = $('<tr></tr>');

        // Add "Select All" checkbox in header
        const $selectAllTh = $('<th></th>').css('width', historyColumns[0].width || '40px');
        const $selectAllCheckbox = $('<input type="checkbox" style=" width:20px !important; height:20px !important" id="select-all-history">');
        $selectAllTh.append($selectAllCheckbox);
        $tr.append($selectAllTh);

        // Bind select all checkbox
        $selectAllCheckbox.on('change', function () {
            const isChecked = $(this).is(':checked');
            $('.row-checkbox-history').prop('checked', isChecked).trigger('change');
            if (!isChecked) {

                fndisableApproveButttons(true);
            } else {
                fndisableApproveButttons(false);
            }
        });

        // Add remaining columns
        historyColumns.forEach(col => {
            if (!visibleHistoryColumns[col.key]) return;

            const $th = $('<th></th>');
            const sortIcon = historySortKey === col.key ? (historySortAsc ? ' ▲' : ' ▼') : '';
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

            $th.append($label);
            $tr.append($th);
        });

        $thead.append($tr);
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
                value = formatDateTimeDynamic(value);
                return value;
            }
            //const date = new Date(value);
            //if (!isNaN(date.getTime())) {
            //    const day = String(date.getDate()).padStart(2, '0');
            //    const month = String(date.getMonth() + 1).padStart(2, '0');
            //    const year = date.getFullYear();

            //    return `${year}-${month}-${day}`.toLowerCase();
            //}

        }

        return value.toString().toLowerCase();
    }

    function renderHistoryTable() {
        const $tbody = $('#table-history-body').empty();
        let filtered = applyHistoryFilters(historyData);

        // Global search
        if (historyGlobalSearchText) {
            const search = historyGlobalSearchText.toLowerCase().trim();
            filtered = filtered.filter(row =>
                historyColumns.some(col =>
                    visibleHistoryColumns[col.key] &&
                    formatValueForSearch_H(row[col.key], col).includes(search)
                )
            );
        }

        // Sorting
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

        if (paged.length === 0) {
            const colspan = Object.keys(visibleHistoryColumns).filter(k => visibleHistoryColumns[k]).length + 1;
            $tbody.append(`<tr><td colspan="${colspan}" style="text-align: center;">No data available</td></tr>`);
        } else {
            paged.forEach((row, index) => {
                const $tr = $('<tr></tr>');
                const rowId = row.ID || `row-${start + index}`;
                row.ID = rowId;

                // Add checkbox cell
                const $checkboxTd = $('<td style="align:center;"></td>');
                const $checkbox = $(`<input type="checkbox" style="margin-left:10px !important; width:20px !important; height:20px !important"  class="row-checkbox-history" data-id="${rowId}">`);
                $checkboxTd.append($checkbox);
                $tr.append($checkboxTd);

                // Bind checkbox change
                $checkbox.on('change', function () {
                    const id = $(this).data('id');
                    if ($(this).is(':checked')) {
                        if (!selectedHistoryRowIds.includes(id)) {
                            selectedHistoryRowIds.push(id);
                        }
                    } else {
                        selectedHistoryRowIds = selectedHistoryRowIds.filter(i => i !== id);
                        $('#select-all-history').prop('checked', false);
                    }

                    if (selectedHistoryRowIds.length === 0) {

                        fndisableApproveButttons(true);
                    } else {
                        fndisableApproveButttons(false);
                    }
                    updateHistDeleteButtonState();
                });

                // Add data cells
                historyColumns.forEach(col => {
                    if (!visibleHistoryColumns[col.key]) return;
                    let value = row[col.key] || '';

                    if (col.type === 'date' || col.type === 'datetime') {
                        //const date = new Date(value);
                        //if (!isNaN(date.getTime())) {
                        //    const formatted = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;
                        //    value = formatted;
                        //}
                        value = formatDateTimeDynamic(value);
                    }

                    const $td = $(`<td style="white-space:normal; word-wrap:break-word;">${value}</td>`);
                    $tr.append($td);
                });

                $tbody.append($tr);


            });
        }

        // Pagination info
        if (filtered.length > 0) {
            $('#history-page-info').text(`Showing ${start + 1} to ${Math.min(start + historyPageSize, filtered.length)} of ${filtered.length} Entries`).show();
            $('#historyPagination').parent().show();
            renderHistoryPagination(Math.ceil(filtered.length / historyPageSize));
        } else {
            $('#history-page-info').hide();
            $('#historyPagination').parent().hide();
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

        selectedHistoryRowIds = [];
        fndisableApproveButttons(true);
        fetchDynamicControlsDataFromApi();

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

            fetchDynamicControlsDataFromApi();

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

        // Delete button click event
        $('#delete-selected-hist').on('click', function () {
            if (selectedHistoryRowIds.length === 0) {
                alert('Please select at least one record to delete.');
                return;
            }
            if (!confirm(`Are you sure you want to delete ${selectedHistoryRowIds.length} selected record(s)?`)) {
                return;
            }

            deleteHistControls(selectedHistoryRowIds);
            alert("Deleted Successfully....!");
            selectedHistoryRowIds = [];
            fetchDynamicControlsDataFromApi();

        });

        $('#bulk-applychanges-hist').on('click', function () {

            updateDynamicApprovalStatus('Apply');
            selectedHistoryRowIds = [];
            fetchDynamicControlsDataFromApi();
            $find("mpehistConfirmActions").hide();
        });
        $('#bulk-queue-hist').on('click', function () {
            updateDynamicApprovalStatus('Queue');
            selectedHistoryRowIds = [];
            fetchDynamicControlsDataFromApi();
            $find("mpehistConfirmActions").hide();
        });
        $('#bulk-approve-history').on('click', function () {
            if (selectedHistoryRowIds.length === 0) {
                alert('Please select at least one record to Approve.');
                return;
            }
            $find("mpehistConfirmActions").show();
        });
    });
</script>

<div>

    <asp:Panel>

        <div class="container-fluid">

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
                            <button id="reset-all-filters-history" type="button" class="btn btn-danger">Reset All Filters</button>
                            <button class="btn btn-success btn-sm" type="button" disabled id="bulk-approve-history">Approve</button>
                            <button class="btn btn-warning btn-sm" type="button" disabled id="bulk-disable-history">Disable</button>
                            <button class="btn btn-danger btn-sm" type="button" disabled id="delete-selected-hist">Delete</button>
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
                        <thead id="table-history-head">
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

<ajax:modalpopupextender id="mpehistConfirmActions" runat="server" popupcontrolid="pnhistConfirmActionMsg" targetcontrolid="ButtonDummy2"
    backgroundcssclass="modalBackground" behaviorid="mpehistConfirmActions">
</ajax:modalpopupextender>
<asp:Panel ID="pnhistConfirmActionMsg" runat="server" CssClass="modalPopup" Style="display: none; width: 35%; height: auto; font-family: 'Source Sans Pro', sans-serif !important; position: relative;">
    <asp:Panel ID="pnlConfirmActionsTitle" CssClass="popHeader" runat="server" Style="position: relative;">
        <div class="popConfirmActionTitle" style="text-align: center; padding-right: 30px; position: relative;">
            <asp:Label ID="lbl_confirm_title" runat="server" Style="color: white;" Text="Confirm Your Action" />
            <span id="btnhistCloseModal" title="Close"
                style="cursor: pointer; position: absolute; top: -2px; right: 5px; font-size: 28px; color: white; font-weight: bold;">&times;
            </span>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnhisttConfirmActionMsg" runat="server">
        <div class="container-fluid">
            <div class="row">
                <p style="text-align: center; background-color: white; font-size: 20px;">
                    <br />
                    <asp:Label ID="lblhistConfirmAction" runat="server" Text="Please choose any of the below actions to proceed:" />
                </p>
            </div>
        </div>
    </asp:Panel>
    <br />
    <div class="btnBoxCenter" style="padding-top: 10px; padding-bottom: 40px;">
        <button id="bulk-applychanges-hist" style="margin-left: 10px;" type="button" class="buttonBox">Apply Changes Now</button>
        <button class="buttonBoxFocus" style="margin-left: 10px;" type="button" id="bulk-queue-hist">Queue for Processing</button>
    </div>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="ButtonDummy2" />
<script>
    document.getElementById('btnhistCloseModal').addEventListener('click', function () {
        $find('mpehistConfirmActions').hide();
    });
</script>
