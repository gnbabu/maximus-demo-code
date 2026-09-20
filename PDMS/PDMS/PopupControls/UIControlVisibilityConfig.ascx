<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_UIControlVisibilityConfig" Codebehind="UIControlVisibilityConfig.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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

    #ui-page-info {
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

    #reset-all-filters-ui {
        font-size: 2rem;
        padding: 4px 15px;
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
        width: 56px !important;
    }

    .dcBtn {
        border-color: #808080 !important;
    }

    input.column-toggle {
        width: 32px !important;
        vertical-align: middle !important;
        height: 25px !important;
    }

    .pagination > li > a {
        border-width: 0px !important;
        background: transparent !important;
    }

    .pagination > .active > a {
        background: transparent !important;
        color: #fff !important;
        background-color: #545487 !important;
    }

    @media (max-width: 767px) {
        #reset-all-filters-ui {
            margin-top: 5px !important;
        }

        .responsive-buttons-container {
            margin-top: 5px !important;
            margin-left: -6px !important;
        }

        .uisearch {
            justify-content: start !important;
            margin-top: 5px !important;
        }
    }
</style>

<style type="text/css">
    .UIControlVisibility {
        display: none;
    }

    .UIControlVisibilitySelectValuesFix {
        display: none;
    }

    .rgEdit {
        width: 15px;
        height: 15px;
        display: inline-block;
        text-indent: -17px !important;
    }

    .rgDelIcon {
        width: 15px;
        height: 15px;
        display: inline-block;
        text-indent: -17px !important;
    }

    .RadCalendarPopup {
        background: #d2deef;
    }

    .RadCalendarPopupShadows {
        background: #d2deef;
    }

    .ChkBoxClass input {
        width: 25px;
        height: 25px;
        text-align: center;
        font-size: 14px;
        margin: 5px;
    }

    .redBoldText {
        color: red;
    }
</style>

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
        margin-bottom: 8px;
    }

    .hModalHeader h3 {
        margin: 0px !important;
    }

    .uisearch {
        display: flex;
        align-content: center;
        width: 100%;
        justify-content: end;
        line-height: 2;
    }

    .uisearch input {
        max-width: 200px;
        height: 40px;
    }

    .uisearch label {
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

<script>
    let selectedUIRowIds = [];
    const UIColumns = [
        { key: 'PageName', title: 'Page Name', type: 'text' },
        { key: 'ControlID', title: 'Control ID', type: 'text' },
        { key: 'Reg_Id', title: 'Reg ID', type: 'text' },
        { key: 'IsVisible', title: 'Is Visible', type: 'text' },
        { key: 'Record_Status', title: 'Record Status', type: 'text' },
        { key: 'Approval_Date', title: 'ApprovalDate', type: 'date' },
        { key: 'Review_Date', title: 'ReviewDate', type: 'date' },
        { key: 'IsActive', title: 'Is Active', type: 'text' },
    ];

    let UIData = [];
    let UIPage = 1;
    let UIPageSize = 5;
    let UISortKey = null;
    let UISortAsc = true;
    let UIFilters = {};
    let visibleUIColumns = {};
    let UIGlobalSearchText = '';

    UIColumns.forEach(col => (visibleUIColumns[col.key] = true));

    function pageLoad() {
        try {
            $('[data-toggle="popover"]').popover();
        } catch (err) {
            console.log('that popover method does not exist at this point: ' + err);
        }
    }

    function updateDeleteButtonState() {
        const isDisabled = selectedUIRowIds.length === 0;
        $('#delete-selected-ui').prop('disabled', isDisabled);
        $('#bulk-approve-ui').prop('disabled', isDisabled);
    }

    function buildUIColumnVisibilityDropdown() {
        const $dropdown = $('.column-visibility-dropdown-ui').empty();

        UIColumns.forEach(col => {
            const $li = $(`
                <li>
                    <label style="font-weight: normal; font-size: 1.72rem !important; line-height: 1;">
                        <input type="checkbox" class="column-toggle" data-key="${col.key}" ${visibleUIColumns[col.key] ? 'checked' : ''}>
                        ${col.title}
                    </label>
                </li>
            `);
            $dropdown.append($li);
        });

        $('.column-toggle').off('change').on('change', function () {
            const key = $(this).data('key');
            visibleUIColumns[key] = $(this).is(':checked');
            buildUITableHeader();
            renderUITable();
        });
    }

    function updateApprovalStatus(action) {
        const webApi = $("[id*=hdnWebAPIURL]").val();
        const webApiEnrollment = webApi + "Enrollment/";
        const APIToken = $("[id*=hdnAccessToken]").val();

        if (!selectedUIRowIds.length) {
            alert("No records selected for deletion.");
            return;
        }

        $.ajax({
            type: "POST",
            url: webApiEnrollment + "UpdateUIControlVisibilityStatus",
            async: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken,
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ids: selectedUIRowIds, Action: action }),
            success() {
                buildUIColumnVisibilityDropdown();
                buildUITableHeader();
                renderUITable();
            },
            error(jqXHR, textStatus, errorThrown) {
                console.error(errorThrown);
                alert("Failed to update records: " + errorThrown);
            },
        });
    }

    function deleteUIControls(selectedIds) {
        const webApi = $("[id*=hdnWebAPIURL]").val();
        const webApiEnrollment = webApi + "Enrollment/";
        const APIToken = $("[id*=hdnAccessToken]").val();

        if (!selectedIds.length) {
            alert("No records selected for deletion.");
            return;
        }

        $.ajax({
            type: "POST",
            url: webApiEnrollment + "DeleteUIControlConfig",
            async: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken,
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ids: selectedUIRowIds }),
            success(result) {
                if (Array.isArray(result)) {
                    UIData = result;
                } else if (result && result.deletedCount !== undefined) {
                    UIData = UIData.filter(row => !selectedIds.includes(row.Id));
                }
                UIPage = 1;
                buildUIColumnVisibilityDropdown();
                buildUITableHeader();
                renderUITable();
            },
            error(jqXHR, textStatus, errorThrown) {
                console.error(errorThrown);
                alert("Failed to delete records: " + errorThrown);
            },
        });

        return false;
    }

    function fetchUIControlsDataFromApi() {
        const webApi = $("[id*=hdnWebAPIURL]").val();
        const webApiEnrollment = webApi + "Enrollment/";
        const APIToken = $("[id*=hdnAccessToken]").val();
        const regId = $("#ctl00_MainContent_ucUIControlVisibilityConfig_txtRegId").val();
        //showLoader();
        $.ajax({
            type: "GET",
            url: `${webApiEnrollment}GetUIControlVisibilityByRegId?regId=${regId}`,
            async: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken,
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success(result) {
                UIData = result;
                UIPage = 1;
                buildUIColumnVisibilityDropdown();
                buildUITableHeader();
                renderUITable();
                hideLoader();
                updateDeleteButtonState();
            },
            error(jqXHR, textStatus, errorThrown) {
                hideLoader();
                console.error(errorThrown);
            },
        });
        return false;
    }

    function buildUITableHeader() {
        const $thead = $('#table-ui-head').empty();
        const $tr = $('<tr></tr>');

        const $selectAllTh = $('<th></th>');
        const $selectAllCheckbox = $('<input type="checkbox" style="width:20px !important; height:20px !important" id="select-all-ui">');
        $selectAllTh.append($selectAllCheckbox);
        $tr.append($selectAllTh);

        $selectAllCheckbox.on('change', function () {
            const checked = $(this).is(':checked');
            $('.row-checkbox-ui').prop('checked', checked).trigger('change');
        });

        UIColumns.forEach(col => {
            if (!visibleUIColumns[col.key]) return;
            const $th = $('<th></th>');
            const sortIcon = UISortKey === col.key ? (UISortAsc ? ' ▲' : ' ▼') : '';
            const $label = $(`<strong style="cursor:pointer;">${col.title}${sortIcon}</strong>`).on('click', () => {
                UIPage = 1;
                if (UISortKey === col.key) UISortAsc = !UISortAsc;
                else {
                    UISortKey = col.key;
                    UISortAsc = true;
                }
                buildUITableHeader();
                renderUITable();
            });
            $th.append($label);
            $tr.append($th);
        });

        $thead.append($tr);
    }

    function applyUIFilters(data) {
        return data.filter(row =>
            Object.entries(UIFilters).every(([key, filter]) => {
                const rawVal = row[key];
                const val = rawVal != null ? rawVal.toString().toLowerCase() : '';
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
                        const numVal = parseFloat(val);
                        const numVal1 = parseFloat(filter.value1);
                        const numVal2 = parseFloat(filter.value2);
                        if (!isNaN(numVal) && !isNaN(numVal1) && !isNaN(numVal2)) return numVal >= numVal1 && numVal <= numVal2;
                        const dateVal = parseDate(val);
                        const dateVal1 = parseDate(filter.value1);
                        const dateVal2 = parseDate(filter.value2);
                        if (dateVal && dateVal1 && dateVal2) return dateVal >= dateVal1 && dateVal <= dateVal2;
                        return false;
                    }
                    case 'contains': return val.includes(value1);
                    case 'startsWith': return val.startsWith(value1);
                    case 'endsWith': return val.endsWith(value1);
                    default: return true;
                }
            })
        );
    }

    function parseDate(str) {
        const [mm, dd, yyyy] = str.split('/');
        if (!dd || !mm || !yyyy) return null;
        const date = new Date(`${yyyy}-${mm}/${dd}`);
        return isNaN(date.getTime()) ? null : date;
    }

    function formatValueForSearch_H(value, col) {
        if (value == null) return '';
        if (col.type === 'date' || col.type === 'datetime') {
            const date = new Date(value);
            if (!isNaN(date.getTime())) return formatDateTime(value);
        }
        return value.toString().toLowerCase();
    }

    function renderUITable() {
        const $tbody = $('#table-ui-body').empty();
        let filtered = applyUIFilters(UIData);

        if (UIGlobalSearchText) {
            const search = UIGlobalSearchText.trim().toLowerCase();
            filtered = filtered.filter(row =>
                UIColumns.some(col =>
                    visibleUIColumns[col.key] &&
                    formatValueForSearch_H(row[col.key], col).includes(search)
                )
            );
        }

        if (UISortKey) {
            filtered.sort((a, b) => {
                let valA = a[UISortKey];
                let valB = b[UISortKey];
                if (typeof valA === 'string') valA = valA.toLowerCase();
                if (typeof valB === 'string') valB = valB.toLowerCase();
                return UISortAsc ? (valA > valB ? 1 : -1) : (valA < valB ? 1 : -1);
            });
        }

        const start = (UIPage - 1) * UIPageSize;
        const paged = filtered.slice(start, start + UIPageSize);

        if (!paged.length) {
            const colspan = Object.values(visibleUIColumns).filter(v => v).length + 1;
            $tbody.append(`<tr><td colspan="${colspan}" style="text-align:center;">No data available</td></tr>`);
        } else {
            paged.forEach((row, index) => {
                const $tr = $('<tr></tr>');
                const rowId = row.UIControlId || `row-${start + index}`;
                row.ID = rowId;

                const $checkboxTd = $('<td style="align:center;"></td>');
                const $checkbox = $(`<input type="checkbox" style="margin-left:10px !important; width:20px !important; height:20px !important" class="row-checkbox-ui" data-id="${rowId}">`);

                $checkbox.on('change', function () {
                    const id = $(this).data('id');
                    if ($(this).is(':checked')) {
                        if (!selectedUIRowIds.includes(id)) selectedUIRowIds.push(id);
                    } else {
                        selectedUIRowIds = selectedUIRowIds.filter(i => i !== id);
                        $('#select-all-ui').prop('checked', false);
                    }
                    updateDeleteButtonState();
                });

                $checkboxTd.append($checkbox);
                $tr.append($checkboxTd);

                UIColumns.forEach(col => {
                    if (!visibleUIColumns[col.key]) return;
                    let value = row[col.key] || '';
                    if (col.type === 'date' || col.type === 'datetime') value = formatDateTime(value);
                    $tr.append($(`<td style="white-space:normal; word-wrap:break-word;">${value}</td>`));
                });

                $tbody.append($tr);
            });
        }

        if (filtered.length > 0) {
            $('#ui-page-info').text(`Showing ${start + 1} to ${Math.min(start + UIPageSize, filtered.length)} of ${filtered.length} Entries`).show();
            $('#uiPagination').parent().show();
            renderUIPagination(Math.ceil(filtered.length / UIPageSize));
        } else {
            $('#ui-page-info').hide();
            $('#uiPagination').parent().hide();
        }
    }

    function formatDateTime(value) {
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

    function renderUIPagination(totalPages) {
        const uiPagination = $('#uiPagination').empty();

        const $firstPage = $('<li><a style="font-size: 20px;" href="#">&lt;&lt;</a></li>').on('click', e => {
            e.preventDefault();
            UIPage = 1;
            renderUITable();
        });
        uiPagination.append($firstPage);

        const pageDisplay = Math.floor(UIPage / 10);
        if (UIPage >= 10) {
            const $prevPage = $('<li><a style="font-size: 20px;" href="#">&lt;</a></li>').on('click', e => {
                e.preventDefault();
                UIPage = (pageDisplay * 10) - 1;
                renderUITable();
            });
            uiPagination.append($prevPage);
        }

        for (let i = 1; i <= totalPages; i++) {
            if (i >= pageDisplay * 10 && i < (pageDisplay * 10) + 10) {
                const $li = $(`<li class="${i === UIPage ? 'active' : ''}"><a style="font-size:20px" href="#">${i}</a></li>`).on('click', e => {
                    e.preventDefault();
                    UIPage = i;
                    renderUITable();
                });
                uiPagination.append($li);
            }
            if (i === (pageDisplay * 10) + 10) {
                const $nextPage = $(`<li class="${i === UIPage ? 'active' : ''}"><a style="font-size:20px" href="#">&gt;</a></li>`).on('click', e => {
                    e.preventDefault();
                    UIPage = i;
                    renderUITable();
                });
                uiPagination.append($nextPage);
            }
        }

        const $lastPage = $('<li><a style="font-size: 20px;" href="#">&gt;&gt;</a></li>').on('click', e => {
            e.preventDefault();
            UIPage = totalPages;
            renderUITable();
        });
        uiPagination.append($lastPage);
    }

    $(document).ready(() => {
        selectedUIRowIds = [];

        $('#ctl00_MainContent_ucUIControlVisibilityConfig_txtRegId').on('mouseout', fetchUIControlsDataFromApi);

        $('#reset-all-filters-ui').click(() => {
            UIFilters = {};
            $('.filter-input').val('');
            $('.filter-operator').val('eq');
            $('.filter-val2').val('').hide();
            $('.dropdown').removeClass('open');
            Object.keys(visibleUIColumns).forEach(key => (visibleUIColumns[key] = true));
            $('#uisearch').val('');
            UIGlobalSearchText = '';
            $('#page-size-dropdown-ui').val('5');
            UIPageSize = 5;
            UIPage = 1;
            fetchUIControlsDataFromApi();
            return false;
        });

        $('#copy-btn-ui').click(() => {
            const visibleCols = UIColumns.filter(c => visibleUIColumns[c.key]);
            const filteredData = applyUIFilters(UIData);

            let copyText = visibleCols.map(col => col.title).join('\t') + '\n';
            filteredData.forEach(row => {
                copyText += visibleCols.map(col => row[col.key] ?? '').join('\t') + '\n';
            });

            const $tempTextArea = $('<textarea>').val(copyText).css({ position: 'absolute', top: '-9999px', left: '-9999px', opacity: 0 }).appendTo('body');
            $tempTextArea[0].focus();
            $tempTextArea[0].select();

            try {
                const successful = document.execCommand('copy');
                alert(successful ? 'Copied to clipboard!' : 'Copy failed.');
            } catch {
                alert('Copy failed.');
            }

            $tempTextArea.remove();
        });

        $('#excel-btn-ui').click(() => {
            const visibleCols = UIColumns.filter(c => visibleUIColumns[c.key]);
            const filteredData = applyUIFilters(UIData);

            let csvContent = visibleCols.map(col => `"${col.title}"`).join(',') + '\n';

            filteredData.forEach(row => {
                visibleCols.forEach(col => {
                    let value = row[col.key];
                    if (col.type === 'date' && value) {
                        const date = new Date(value);
                        if (!isNaN(date)) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();
                            value = `${month}/${day}/${year}`;
                        }
                    }
                    if (col.type === 'datetime' && value) {
                        const date = new Date(value);
                        if (!isNaN(date)) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();
                            const hour = String(date.getHours()).padStart(2, '0');
                            const minute = String(date.getMinutes()).padStart(2, '0');
                            const second = String(date.getSeconds()).padStart(2, '0');
                            value = `${month}/${day}/${year} ${hour}:${minute}:${second}`;
                        }
                    }
                    csvContent += `"${String(value).replace(/<br\s*\/?>/gi, '|')}",`;
                });
                csvContent += '\n';
            });

            const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
            const url = URL.createObjectURL(blob);
            const link = document.createElement("a");
            link.href = url;
            link.download = "export.csv";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            return false;
        });

        $('#pdf-btn-ui').click(() => {
            const { jsPDF } = window.jspdf;
            const doc = new jsPDF('l', 'pt', 'a4');

            const visibleCols = UIColumns.filter(c => visibleUIColumns[c.key]);
            const filteredData = applyUIFilters(UIData);

            const head = [visibleCols.map(col => col.title)];
            const body = [];

            filteredData.forEach(row => {
                const rowValues = [];
                visibleCols.forEach(col => {
                    let value = row[col.key];
                    if (col.type === 'date' && value) {
                        const date = new Date(value);
                        if (!isNaN(date)) {
                            const day = String(date.getDate()).padStart(2, '0');
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const year = date.getFullYear();
                            value = `${month}/${day}/${year}`;
                        }
                    }
                    if (col.type === 'datetime' && value) {
                        const date = new Date(value);
                        if (!isNaN(date)) {
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

            doc.autoTable({ head, body, startY: 10, styles: { fontSize: 6 } });
            doc.save('export.pdf');
            return false;
        });

        $('#uisearch').on('input', function () {
            UIGlobalSearchText = $(this).val().toLowerCase();
            UIPage = 1;
            renderUITable();
        });

        $('#delete-selected-ui').on('click', () => {
            if (!selectedUIRowIds.length) {
                alert('Please select at least one record to delete.');
                return;
            }
            if (!confirm(`Are you sure you want to delete ${selectedUIRowIds.length} selected record(s)?`)) return;

            deleteUIControls(selectedUIRowIds);
            alert("Deleted Successfully....!");
            selectedUIRowIds = [];
            fetchUIControlsDataFromApi();
        });

        $('#bulk-approve-ui').on('click', () => {
            if (!selectedUIRowIds.length) {
                alert('Please select at least one record to Approve.');
                return;
            }
            $find("mpeUIConfirmActions").show();
        });

        $('#bulk-applychanges-ui').on('click', () => {
            updateApprovalStatus('Apply');
            selectedUIRowIds = [];
            fetchUIControlsDataFromApi();
            updateDeleteButtonState();
            $find("mpeUIConfirmActions").hide();
        });

        $('#bulk-queue-ui').on('click', () => {
            updateApprovalStatus('Queue');
            selectedUIRowIds = [];
            fetchUIControlsDataFromApi();
            updateDeleteButtonState();
            $find("mpeUIConfirmActions").hide();
        });
    });
</script>

<div style="padding-top: 10px">
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2"></div>
        <div class="col-sm-6 text-left">
            <span class="formLabel150">
                <asp:Label ID="lblError" runat="server" Text="" Visible="false" CssClass="redBoldText" />
            </span>
        </div>
    </div>

    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblRegId" runat="server" Text="Registration ID:" AssociatedControlID="txtRegId" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:TextBox ID="txtRegId" runat="server" CssClass="textEntry" MaxLength="100" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRegId"
                ErrorMessage="* Enter a Reg Id Name"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="UIControlVisibility" Display="Dynamic" ForeColor="Red" />
        </div>

        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblPageName" runat="server" AssociatedControlID="ddlPageName" Text="Page Name" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:DropDownList ID="ddlPageName" runat="server" CssClass="DropDownList" AutoPostBack="true" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlPageName"
                ErrorMessage="* Select a Page Name"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="UIControlVisibility" Display="Dynamic" ForeColor="Red" />
        </div>

        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="Label1" runat="server" AssociatedControlID="ddlControlID" Text="Field Name" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:DropDownList runat="server" ID="ddlControlID" CssClass="DropDownList" AutoPostBack="false">
                <asp:ListItem Text="Select an Action" Value=""></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlControlID"
                ErrorMessage="* Select a valid Field Name"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="UIControlVisibility" Display="Dynamic" ForeColor="Red" />
        </div>

        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblDataTypeId" runat="server" AssociatedControlID="chkIsVisible" Text="Is Visible" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:CheckBox runat="server" ID="chkIsVisible" AutoPostBack="false" />
        </div>

        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblchkIsActive" runat="server" AssociatedControlID="chkIsActive" Text="Is Active" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:CheckBox runat="server" ID="chkIsActive" AutoPostBack="false" />
        </div>

        <div class="row" style="margin-top: 10px; display: none">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="Label4" runat="server" AssociatedControlID="lblApprovalStatus" Text="Approval Status" />
                </span>&nbsp;&nbsp;
            </div>
            <div class="col-sm-4 text-left">
                <div class="col-sm-2 text-right">
                    <span class="formLabel150">
                        <asp:Label ID="lblApprovalStatus" runat="server" Text="" />
                    </span>&nbsp;&nbsp;
                </div>
            </div>
        </div>

        <div class="row" style="margin-top: 10px; display: none">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="lblApprovalDate" Text="Approval Date" />
                </span>&nbsp;&nbsp;
            </div>
            <div class="col-sm-4 text-left">
                <div class="col-sm-2 text-right">
                    <span class="formLabel150">
                        <asp:Label ID="lblApprovalDate" runat="server" Text="" />
                    </span>&nbsp;&nbsp;
                </div>
            </div>
        </div>

        <div class="row" style="margin-top: 10px; display: none">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="Label3" runat="server" AssociatedControlID="lblReviewDate" Text="Review Date" />
                </span>&nbsp;&nbsp;
            </div>
            <div class="col-sm-4 text-left">
                <div class="col-sm-2 text-right">
                    <span class="formLabel150">
                        <asp:Label ID="lblReviewDate" runat="server" Text="" />
                    </span>&nbsp;&nbsp;
                </div>
            </div>
        </div>
    </div>

    <br />

    <div class="row" style="margin-top: 10px; margin-left: 23%; text-align: center">
        <asp:Button ID="btnClearValues" runat="server" Text="Clear Selected Values" CssClass="buttonBoxFocus" OnClick="btnCelarValues_Click" />
        <asp:Button ID="Button1" runat="server" CausesValidation="true" Text="Save" CssClass="buttonBoxFocus" ValidationGroup="UIControlVisibility"
            OnClick="btnSave_Click" />
    </div>
</div>
<br />
<div>
    <asp:ValidationSummary ID="valSummaryUIControlVisibility" runat="server" DisplayMode="List" ValidationGroup="UIControlVisibility" CssClass="UIControlVisibility" />
</div>
<div>
    <asp:ValidationSummary ID="ValSummaryDynamicControlSelectValues" runat="server" DisplayMode="List" ValidationGroup="UIControlVisibilitySelectValuesFix" CssClass="UIControlVisibilitySelectValuesFix" />
</div>


<!-- Confirm pop-up modal -->
<asp:UpdatePanel ID="upConfirmAdd" runat="server">
    <ContentTemplate>
        <ajax:modalpopupextender id="mpeConfirmAdd1" runat="server" popupcontrolid="pnlConfirmAdd" targetcontrolid="ButtonDummy1"
            backgroundcssclass="modalBackground" behaviorid="mpeConfirmAdd1">
        </ajax:modalpopupextender>
        <asp:Panel ID="pnlConfirmAdd" runat="server" CssClass="modalPopup" Style="display: none; width: 25%; height: auto;">
            <asp:Panel ID="pnlConfirmTitle" CssClass="popHeader" runat="server">
                <div class="popTitle">
                    <asp:Label ID="lbl_title" runat="server" Text="Information" />
                </div>
            </asp:Panel>
            <br />
            <asp:Panel ID="pnConfirmMsg" runat="server" Style="margin-right: 10px">
                <div class="container-fluid">
                    <div class="row">
                        <p style="text-align: left; background-color: white; width: 90%; margin-left: 5%;">
                            <br />
                            <asp:Label ID="lblConfirm" runat="server" Text="Your record has been saved successfully." />
                        </p>
                    </div>
                </div>
            </asp:Panel>
            <br />
            <div class="btnBox btnBoxCenter" style="padding-top: 10px; width: 93%;">
                <asp:Button runat="server" ID="btnOk" Text="Ok" CssClass="buttonBox" OnClick="btnOk_Click" />
            </div>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonDummy1" Style="display: none" Text="ButtonDummy1" />
    </ContentTemplate>
</asp:UpdatePanel>

<div>
    <div class="container-fluid">
        <div class="hModalContent" style="display: block;">
            <div class="row">
                <div class="col-sm-9">
                    <label class="control-label" style="margin-top: 5px">
                        Rows per page:
                    </label>
                    <select id="page-size-dropdown-ui" class="form-control input-md" style="width: 75px !important; min-width: 75px !important; height: 38px; display: inline-block; margin-left: 5px;">
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
                        <ul class="dropdown-menu column-visibility-dropdown-ui" style="padding: 10px;">
                            <!-- Populated dynamically -->
                        </ul>
                        <div class="btn-group d-flex flex-wrap" role="group">
                            <div class="dropup">
                                <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Export <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu dropdown-menu-right hExport" style="top: 100%; right: auto; height: fit-content;">
                                    <li>
                                        <button class="btn btn-sm btn-primary" id="copy-btn-ui">Copy</button>
                                    </li>
                                    <li>
                                        <button class="btn btn-sm btn-danger" id="pdf-btn-ui">Pdf</button>
                                    </li>
                                    <li>
                                        <button class="btn btn-sm btn-success" id="excel-btn-ui">Excel</button>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <button id="reset-all-filters-ui" type="button" class="btn btn-danger">Reset All Filters</button>
                        <button class="btn btn-success btn-sm" type="button" disabled id="bulk-approve-ui">Approve</button>
                        <button class="btn btn-warning btn-sm" type="button" disabled id="bulk-disable-ui">Disable</button>
                        <button class="btn btn-danger btn-sm" type="button" disabled id="delete-selected-ui">Delete</button>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group uisearch">
                        <span class="ohio-tooltip"
                            data-toggle="popover"
                            data-trigger="hover"
                            data-placement="top"
                            data-content="<asp:Literal ID='HISTORY_POPUP_TEXT' runat='server' Text='For the Date to return results in the Search, the date entered must match the format in the table below' />"
                            aria-hidden="true">
                            <label for="uisearch">Search</label>
                        </span>
                        <input type="text" class="form-control" id="uisearch" placeholder="Search" />
                    </div>
                </div>
            </div>

            <div class="gridHistoryTable table-responsive">
                <table class="table table-bordered table-striped">
                    <thead id="table-ui-head"></thead>
                    <tbody id="table-ui-body"></tbody>
                </table>
            </div>

            <div class="row" style="margin-top: 10px;">
                <div class="col-sm-6">
                    <div id="ui-page-info" class="text-muted" style="padding-top: 6px;"></div>
                </div>
                <div class="col-sm-6 text-right">
                    <ul class="pagination pagination-sm" id="uiPagination" style="margin: 0; display: inline-block;"></ul>
                </div>
            </div>
        </div>
    </div>
</div>

<asp:HiddenField ID="hdnAccessTokens" runat="server" />

<ajax:modalpopupextender id="mpeUIConfirmActions" runat="server" popupcontrolid="pnUIConfirmActionMsg" targetcontrolid="ButtonDummy2"
    backgroundcssclass="modalBackground" behaviorid="mpeUIConfirmActions">
</ajax:modalpopupextender>
<asp:Panel ID="pnUIConfirmActionMsg" runat="server" CssClass="modalPopup" Style="display: none; width: 35%; height: auto; font-family: 'Source Sans Pro', sans-serif !important; position: relative;">
    <asp:Panel ID="pnlConfirmActionsTitle" CssClass="popHeader" runat="server" Style="position: relative;">
        <div class="popConfirmActionTitle" style="text-align: center; padding-right: 30px; position: relative;">
            <asp:Label ID="lbl_confirm_title" runat="server" Style="color: white;" Text="Confirm Your Action" />
            <span id="btnUICloseModal" title="Close"
                style="cursor: pointer; position: absolute; top: -2px; right: 5px; font-size: 28px; color: white; font-weight: bold;">&times;
            </span>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnUConfirmActionMsg" runat="server">
        <div class="container-fluid">
            <div class="row">
                <p style="text-align: center; background-color: white; font-size: 20px;">
                    <br />
                    <asp:Label ID="lblUIConfirmAction" runat="server" Text="Please choose any of the below actions to proceed:" />
                </p>
            </div>
        </div>
    </asp:Panel>
    <br />
    <div class="btnBoxCenter" style="padding-top: 10px; padding-bottom: 40px;">
        <button id="bulk-applychanges-ui" style="margin-left: 10px;" type="button" class="buttonBox">Apply Changes Now</button>
        <button class="buttonBoxFocus" style="margin-left: 10px;" type="button" id="bulk-queue-ui">Queue for Processing</button>
    </div>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="ButtonDummy2" />

<script>
    document.getElementById('btnUICloseModal').addEventListener('click', function () {
        $find('mpeUIConfirmActions').hide();
    });
</script>