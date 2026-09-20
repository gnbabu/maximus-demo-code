<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_WorkflowEventInfo" Codebehind="WorkflowEventInfo.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<script src="../Scripts/jspdf.umd.min.js"></script>
<script src="../Scripts/jspdf.plugin.autotable.min.js"></script>


<style>
    /* Overlay behind the modal */
    .modalBackground {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        /*background: rgba(0, 0, 0, 0.6);*/
        background-color: rgba(0,0,0,0.5);
        z-index: 10000;
    }

    /* Main popup panel */
    .modalPopupPanel {
        position: fixed;
        /* top: 50%;
        left: 50%;*/
        transform: translate(-50%, -50%);
        width: 400px;
        padding: 20px;
        background: #fff;
        border-radius: 4px;
        z-index: 10001;
        margin: auto;
        left: 400px !important;
        top: 100px !important;
    }

    /* Draggable header */
    .modalHeader {
        cursor: move;
        margin-bottom: 1rem;
    }

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
            color: #4C4C4C;
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
        min-height: 175px !important;
        overflow-x: auto !important;
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
        /*height: 200px !important;*/
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
        width: 48px !important
    }

    .dcBtn {
        border-color: #808080 !important
    }

    input.column-toggle1 {
        width: 32px !important;
        vertical-align: middle !important;
        height: 25px !important;
    }

    @media (max-width: 767px) {
        #reset-all-filters {
            margin-top: 5px !important;
        }

        .responsive-buttons-container {
            margin-top: 5px !important;
            margin-left: -6px !important;
        }

        .whsearch {
            justify-content: start !important;
            margin-top: 5px !important;
        }
    }
</style>

<script>

    function checkLength(textbox, labelMsg) {
        var warning = document.getElementById(labelMsg);
        if (textbox.value.length >= 500) {
            textbox.value = textbox.value.substring(0, 500);
            warning.style.display = "inline";
        } else {
            warning.style.display = "none";
        }
    }


    <%--function positionModal() {
        var modal = $get('<%= pnlAddEventNoteModal.ClientID %>');
        if (modal) {
            modal.classList.add('modalTopLeft');
        }
    }--%>

  <%--  Sys.Application.add_load(function () {
        var mpe = $find('<%= mpeAddEventNote.ClientID %>');
        if (mpe) {
            mpe.add_shown(positionModal);
        }
    });--%>

    function openCommentPopup() {
        $('#<%= txtDate.ClientID %>').val('');
        $('#<%= txtOHID.ClientID %>').val('');
        $('#<%= txtODMReview.ClientID %>').val('');
        $('#<%= txtEnrollmentType.ClientID %>').val('');
        $('#<%= txtFinalDisposition.ClientID %>').val('');
        $('#<%= txtNotes.ClientID %>').val('');

        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();
        var regId = $("[id*=hdnRegId]").val();

        $.ajax({
            type: "GET",
            url: webApiEnrollment + "GetProviderFeedAddNotesDetails?regId=" + regId,
            //async: false,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#<%= txtDate.ClientID %>').val(result.Note_Date);
                $('#<%= txtOHID.ClientID %>').val('<%=HttpContext.Current.User.Identity.Name%>');
                $('#<%= txtODMReview.ClientID %>').val('N/A');
                <%--$('#<%= txtODMReview.ClientID %>').val(result.Lasted_Reviewed_By);--%>
                $('#<%= txtEnrollmentType.ClientID %>').val(result.Enrollment_Type);
                $('#<%= txtFinalDisposition.ClientID %>').val('N/A');
                //$('#<%= txtFinalDisposition.ClientID %>').val(result.Final_Disposition);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error(errorThrown);
            },
            complete: function () {
                $find('<%= mpeCommentPopup.ClientID %>').show();
            }

        });
        return false;
    }

    function pageLoad() {
        try {
            $('[data-toggle="popover"]').popover()
        }
        catch (err) {
            console.log('that popover method does not exist at this point: ' + err);
        }
    }

    const columns = [
        { key: 'NotesDate', title: 'Event Start Date', type: 'text' },
        { key: 'InitiatedBy', title: 'Initiator ID', type: 'text' },
        { key: 'PersonReviewedBy', title: 'Last Reviewer ID', type: 'text' },
        { key: 'EnrollmentType', title: 'Event Type', type: 'text' },
        { key: 'FinalDispossion', title: 'Final Disposition', type: 'text' },
        { key: 'Notes', title: 'Notes', type: 'object' },
    ];

    let data = [];
    let page = 1;
    let pageSize = 5;
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
                    <input type="checkbox" class="column-toggle1" data-key="${col.key}" ${visibleColumns[col.key] ? 'checked' : ''}>
                    ${col.title}
                </label>
                </li>
            `);
            $dropdown.append($li);
        });

        // Bind change events
        $('.column-toggle1').off('change').on('change', function () {
            const key = $(this).data('key');
            visibleColumns[key] = $(this).is(':checked');
            buildTableHeader();  // Rebuild header to hide/show
            renderTable();       // Re-render table body
        });
    }

    function fetchDataFromApi() {
        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();
        var regId = $("[id*=hdnRegId]").val();

        $.ajax({
            type: "GET",
            url: webApiEnrollment + "GetProviderFeedNotesDetails?regId=" + regId,
            //async: false,
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
                //alert("Error loading data.");
                console.error(errorThrown);
            },
        });
        return false;
    }

    function saveNewEventNoteData(type) {
        var webApi = $("[id*=hdnWebAPIURL]").val();
        var webApiEnrollment = webApi + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();

        var providerFeedID = 0;
        var note = type == "add" ? $('#<%= txtNotes.ClientID %>').val() : $('#<%= txtPopupComment.ClientID %>').val();

        var warning = type == "add" ? document.getElementById('warningMsg3') : document.getElementById('warningMsg4');
        if (note == '') {
            warning.style.display = "inline";
            return false;
        }

        warning.style.display = "none"; var notes = [note];
  <%--var enrollmentType = $('#<%= txtEnrollmentType.ClientID %>').val();--%>
        var enrollmentType = 'Ad-hoc Comment';
        if (type == 'edit') {
            providerFeedID = $('#<%= hdnSelectedRowId.ClientID %>').val();
            notes = [$('#<%= txtPopupComment.ClientID %>').val()];
            enrollmentType = $('#<%= hdnEventType.ClientID %>').val();
        }
        const payload = {
            RegID: $("[id*=hdnRegId]").val(),
            NotesDate: $('#<%= txtDate.ClientID %>').val(),
            InitiatedBy: '<%=HttpContext.Current.User.Identity.Name%>',
            PersonReviewedBy: $('#<%= txtODMReview.ClientID %>').val(),
            EnrollmentType: enrollmentType,
            FinalDispossion: $('#<%= txtFinalDisposition.ClientID %>').val(),
            Notes: notes,
            ProviderFeedID: providerFeedID,
            CreatedBy: '<%=HttpContext.Current.User.Identity.Name%>'
        };

        $.ajax({
            type: "POST",
            url: webApiEnrollment + "InsertProviderFeedNotes",
            //async: false,
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
                data = result;
                page = 1;
                fetchDataFromApi();
                //buildColumnVisibilityDropdown();
                //buildTableHeader();
                //renderTable();
                var finalTable = document.getElementById('table-body');

                $find('<%= mpeAddEventNote.ClientID %>').hide();
                $find('<%= mpeCommentPopup.ClientID %>').hide();
                $('#<%= txtNotes.ClientID %>').val('');

                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error(err);
                $find('<%= mpeAddEventNote.ClientID %>').hide();
                $find('<%= mpeCommentPopup.ClientID %>').hide();
                return false;
            },
            complete: function () {
                return false;
            }
        });
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
                page = 1;
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
        <ul class="dropdown-menu dropdown-menu-right p-2" style="padding: 10px; min-width:140px !important;">
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
                    <div class="col-xs-5 text-left" style="padding-left: 0px;padding-top: 10px;">
                        <button type="button" class="btn btn-xs btn-primary apply-filter resetAttr" data-key="${col.key}">Apply</button>
                    </div>
                    <div class="col-xs-5 text-right" style="padding-right: 10px;padding-top: 10px;">
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
            const $wrapper = col.key == 'Notes' ? $('<div style="display: flex; flex-direction: column;width:auto !important;min-width:130px !important;max-width:300px !important;"></div>') :
                $('<div style="display: flex; flex-direction: column;width:auto !important;min-width:130px !important;max-width:200px !important;"></div>');
            $wrapper.append($label).append($inputGroup);
            $th.append($wrapper);
            $thead.append($th);
        });
        $thead.append('<th style="width:30px">Add<br /> Event<br /> Note</th>');
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
        const date = new Date(`${mm}/${dd}/${yyyy}`);
        return isNaN(date.getTime()) ? null : date;
    }

    function formatValueForSearch_C(value, col) {
        if (value == null) return '';

        if (col.type === 'date') {
            const date = new Date(value);
            if (!isNaN(date.getTime())) {
                const day = String(date.getDate()).padStart(2, '0');
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const year = date.getFullYear();
                return `${yyyy}-${MM}-${year}`.toLowerCase();
            }
        }

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
                        formatValueForSearch_C(row[col.key], col).includes(search)
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
        if (paged.length == 0) {
            const noDataMessageForEventInfo = 'There are currently no items to display.';
            const colspan = columns.filter(col => visibleColumns[col.key]).length + 1;
            $tbody.append(`<tr><td colspan="${colspan}" style="text-align: center;">${noDataMessageForEventInfo}</td></tr>`);

        } else {

            paged.forEach(row => {
                const $tr = $('<tr>');
                const $btn = $(`<td  style="text - align: center!important; "><span style="text - align: center; ">
              <img
                src="../Images/add_old.png"
                alt="Add"
                title="Add note"
                style="cursor:pointer; width:16px; height:16px;"
                class="add-row-btn"
              /></span>
              </td>
            `);

                // Store row’s ID
                $btn.data('rowId', row);
                // Bind click: open popup
                $btn.on('click', function () {
                    var data = $(this).data('rowId');
                    // set hidden field
                    $('#<%= hdnSelectedRowId.ClientID %>').val(data.ProviderFeedID);
                    $('#<%= hdnEventType.ClientID %>').val(data.EnrollmentType);
                    // clear previous comment
                    $('#<%= txtPopupComment.ClientID %>').val('');
                    // show the modal
                    $find('<%= mpeAddEventNote.ClientID %>').show();
                    // Directly show the panel by setting display:block
                    $('#<%= pnlAddEventNoteModal.ClientID %>').css('display', 'block');
                });

                columns.forEach(col => {
                    if (!visibleColumns[col.key]) return;
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

                    // Format date as mm/dd/yy if column is of type 'date'
                    if (col.type === 'object' && value) {
                        value = formatObject(value);
                        $tr.append(`<td style="white-space:normal; word-wrap:break-word;min-width:300px !important;max-width:350px !important;">${value}</td>`);
                    } else {
                        $tr.append(`<td style="white-space:normal; word-wrap:break-word;min-width:150px !important;max-width:200px !important;">${value}</td>`);
                    }

                });

                $tr.append($btn); //Added + button

                $tbody.append($tr);
            });

        }
        if (filtered.length > 0) {
            $('#page-info').text(`Showing ${start + 1} to ${Math.min(start + pageSize, filtered.length)} of ${filtered.length} Entries`).show();
            $('#pagination').parent().show(); // show pagination container
            renderPagination(Math.ceil(filtered.length / pageSize));
        } else {
            $('#page-info').hide();
            $('#pagination').parent().hide(); // hide pagination container
        }
    }

    function formatObject(value) {
        return Array.isArray(value)
            ? value.map(item => `${item}<br />`).join('')
            : String(value);
    }

    function goBack() {
        window.history.back();
    }

    function renderPagination(totalPages) {
        const $pagination = $('#pagination').empty();
        const $firstPage = $('<li><a style="font-size: 20px;" href="#">&lt;&lt;</a></li>');
        $firstPage.click(function (e) {
            e.preventDefault();
            page = 1;
            renderTable();
        });
        $pagination.append($firstPage);

        var pageDisplay = Math.floor(page / 10);
        if (page >= 10) {
            const $prevPage = $('<li><a style="font-size: 20px;" href="#">&lt;</a></li>');
            $prevPage.click(function (e) {
                e.preventDefault();
                page = ((pageDisplay) * 10) - 1;
                renderTable();
            });
            $pagination.append($prevPage);
        }

        for (let i = 1; i <= totalPages; i++) {
            if ((i >= (pageDisplay * 10)) && (i < ((pageDisplay * 10) + 10))) {
                const $li = $(`<li class="${i === page ? 'active' : ''}"><a style="font-size:20px" href="#">${i}</a></li>`);
                $li.click(function (e) {
                    e.preventDefault();
                    page = i;
                    renderTable();
                });
                $pagination.append($li);
            }

            if (i == ((pageDisplay * 10) + 10)) {
                const $nextPage = $(`<li class="${i === page ? 'active' : ''}"><a style="font-size:20px" href="#">&gt;</a></li>`);
                $nextPage.click(function (e) {
                    e.preventDefault();
                    page = i;
                    renderTable();
                });
                $pagination.append($nextPage);
            }
        }

        const $lastPage = $('<li><a style="font-size: 20px;" href="#">&gt;&gt;</a></li>');
        $lastPage.click(function (e) {
            e.preventDefault();
            page = totalPages;
            renderTable();
        });
        $pagination.append($lastPage);
    }


    $(document).ready(function () {

        $('.whModalHeader').on('click', function () {
            const $content = $(this).next('.whModalContent');// assumes the content to toggle is the next sibling
            const $plus = $(this).find('.plus');
            const $minus = $(this).find('.minus');

            $content.slideToggle(); // toggle visibility with animation
            $plus.toggle();         // toggle plus icon
            $minus.toggle();        // toggle minus icon
        });

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

            $('#page-size-dropdown').val('5');
            pageSize = 5;
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
                columns.forEach(col => {
                    if (!visibleColumns[col.key]) return;
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

        $('#pdf-btn').click(() => {
            const { jsPDF } = window.jspdf;
            const doc = new jsPDF('l', 'pt', 'a4');

            const visibleCols = columns.filter(c => visibleColumns[c.key]);
            const filteredData = applyFilters(data);

            const head = [visibleCols.map(col => col.title)];
            // const body = filteredData.map(row => visibleCols.map(col => row[col.key] ?? ''));
            var body = [];
            var rowValues = [];

            filteredData.forEach(row => {
                columns.forEach(col => {
                    if (!visibleColumns[col.key]) return;
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

        $('#whsearch').on('input', function () {
            globalSearchText = $(this).val().toLowerCase();
            page = 1;
            renderTable();
        });
    });
</script>

<div runat="server">

    <asp:Panel runat="server">
        <div class="container-fluid">
            <br />
            <div class="whModalHeader d-flex justify-content-between align-items-center">
                <h3 class="m-0"><strong>Provider Feed (starting 08/13/2025)</strong></h3>
                <div class="toggle-icons">
                    <span class="plus" style="display: none;">+</span>
                    <span class="minus">−</span>
                </div>
            </div>
            <div class="whModalContent" style="display: block;">
                <!-- Page Size Dropdown -->
                <div class="row">
                    <div class="col-12 col-md-9">

                        <asp:Button runat="server" ID="btnShowCommentPopup" OnClientClick="return openCommentPopup();" CssClass="btn btn-primary" Text="Add New"></asp:Button>
                        <label for="page-size-dropdown" class="control-label" style="margin-top: 5px; margin-left: 10px;">
                            Rows per page:
                        </label>
                        <select id="page-size-dropdown" class="form-control input-md" style="width: 75px !important; min-width: 75px !important; height: 38px; display: inline-block; margin-left: 5px;">
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
                            <ul class="dropdown-menu column-visibility-dropdown" style="padding: 10px;">
                                <!-- Populated dynamically -->
                            </ul>
                            <div class="btn-group d-flex flex-wrap" role="group">
                                <div class="dropup">

                                    <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        Export <span class="caret"></span>
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
                    <div class="col-12 col-md-3">
                        <div class="form-group whsearch">
                            <span class="ohio-tooltip"
                                data-toggle="popover"
                                data-trigger="hover"
                                data-placement="top"
                                data-content="<asp:Literal ID='HISTORY_POPUP_TEXT' runat='server' Text='For the Date to return results in the Search, the date entered must match the format in the table below' />"
                                aria-hidden="true">
                                <label for="whsearch">Search</label>
                                &nbsp;
                            </span>
                            <input type="text" class="form-control" id="whsearch" placeholder="Search" />
                        </div>
                    </div>
                </div>

                <!-- Table -->
                <div class="gridTable table-responsive">
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
            </div>
        </div>
        <asp:HiddenField ID="hdnEventType" runat="server" />
        <asp:HiddenField ID="hdnSelectedRowId" runat="server" />
        <asp:HiddenField ID="hdnAccessToken" runat="server" />
        <asp:HiddenField ID="hdnWebAPIURL" runat="server" />
        <asp:HiddenField ID="hdnRegId" runat="server" />

    </asp:Panel>

    <ajax:modalpopupextender id="mpeAddEventNote" runat="server" backgroundcssclass="modalBackground"
        cancelcontrolid="btnEventNoteClose" popupcontrolid="pnlAddEventNoteModal" popupdraghandlecontrolid="pnlAddEventNoteModal" targetcontrolid="ButtonDummy3" />
    <asp:Panel ID="pnlAddEventNoteModal" runat="server" CssClass="modalPopupPanel" Style="display: none; top: 10px;">
        <asp:Panel ID="pnlMain1" runat="server" Style="margin-right: 10px">
            <h4>Add Note</h4>
            <asp:TextBox ID="txtPopupComment" runat="server" TextMode="MultiLine" Rows="5"
                Columns="50" CssClass="form-control" onkeyup="checkLength(this,'warningMsg1')" />
            <span id="warningMsg1" style="color: red; display: none;">⚠️ You’ve reached the maximum allowed characters limit 500.
            </span>

            <span id="warningMsg4" style="color: red; display: none;">⚠️ Note field is mandatory</span>

            <div class="row">
                <div class="btnBox" style="text-align: right;">
                    <button
                        id="btnPopupSave"
                        type="button"
                        class="btn btn-primary"
                        onclick="return saveNewEventNoteData('edit');">
                        Save
                    </button>
                    <asp:Button ID="btnEventNoteClose" runat="server" CausesValidation="false" CssClass="btn btn-secondary" Text="Cancel" />
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" aria-label="Dummy Button" ID="ButtonDummy3" Style="display: none" Text="ButtonDummy3" />


    <!-- Popup Panel -->
    <asp:Panel
        ID="pnlCommentPopup"
        runat="server"
        CssClass="modalPopupPanel"
        Style="display: none">

        <!-- Drag Handle / Header -->
        <div
            id="pnlCommentHeader"
            runat="server"
            class="modalHeader">
            <h4>Add Note</h4>
        </div>

        <!-- Date -->
        <div class="form-field">
            <asp:Label
                ID="lblDate"
                runat="server"
                AssociatedControlID="txtDate"
                Text="Date" />
            <asp:TextBox
                ID="txtDate"
                runat="server"
                CssClass="form-control"
                ReadOnly="true" />
        </div>

        <!-- Initiator OHID -->
        <div class="form-field">
            <asp:Label
                ID="lblOHID"
                runat="server"
                AssociatedControlID="txtOHID"
                Text="OHID for Initiator" />
            <asp:TextBox
                ID="txtOHID"
                runat="server"
                CssClass="form-control"
                ReadOnly="true" />
        </div>

        <!-- Last ODM Review OHID -->
        <div class="form-field">
            <asp:Label
                ID="lblODMReview"
                runat="server"
                AssociatedControlID="txtODMReview"
                Text="OHID for Last ODM Review" />
            <asp:TextBox
                ID="txtODMReview"
                runat="server"
                CssClass="form-control"
                ReadOnly="true" />
        </div>

        <!-- Enrollment Type -->
        <div class="form-field">
            <asp:Label
                ID="lblEnrollmentType"
                runat="server"
                AssociatedControlID="txtEnrollmentType"
                Text="Enrollment Type" />
            <asp:TextBox
                ID="txtEnrollmentType"
                runat="server"
                CssClass="form-control"
                ReadOnly="true" />
        </div>

        <!-- Final Disposition -->
        <div class="form-field">
            <asp:Label
                ID="lblFinalDisposition"
                runat="server"
                AssociatedControlID="txtFinalDisposition"
                Text="Final Disposition" />
            <asp:TextBox
                ID="txtFinalDisposition"
                runat="server"
                CssClass="form-control"
                ReadOnly="true" />
        </div>

        <!-- Notes TextArea -->
        <div class="form-field">
            <asp:Label
                ID="lblNotes"
                runat="server"
                AssociatedControlID="txtNotes"
                Text="Notes" />
            <asp:TextBox
                ID="txtNotes"
                runat="server"
                CssClass="form-control"
                TextMode="MultiLine"
                Rows="5"
                onkeyup="checkLength(this,'warningMsg2')" />
            <span id="warningMsg2" style="color: red; display: none;">⚠️ You’ve reached the maximum allowed characters limit 500.</span>
            <span id="warningMsg3" style="color: red; display: none;">⚠️ Note field is mandatory</span>
        </div>

        <!-- Action Buttons -->
        <div class="btnBox">
            <button
                id="btnSaveComment"
                type="button"
                class="btn btn-primary"
                onclick="return saveNewEventNoteData('add');">
                Save
            </button>
            <asp:Button
                ID="btnCancelComment"
                runat="server"
                Text="Cancel"
                CssClass="btn btn-secondary"
                CausesValidation="false" />
        </div>
    </asp:Panel>

    <!-- ModalPopupExtender -->
    <ajax:modalpopupextender
        id="mpeCommentPopup"
        runat="server"
        targetcontrolid="btnShowCommentPopup"
        popupcontrolid="pnlCommentPopup"
        backgroundcssclass="modalBackground"
        cancelcontrolid="btnCancelComment"
        popupdraghandlecontrolid="pnlCommentPopup" />

</div>

