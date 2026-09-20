(function ($) {
    $.fn.dataGrid = function (options) {
        // Default settings
        let settings = $.extend({
            apiUrl: null,
            data: [],
            columns: [],
            rowTemplate: null,
            enableRowSelection: false,
            enableStatusLabel: false,
            checkboxColumnWidth: '40px',
            pageSizeOptions: [5, 10, 20, 50, 100],
            initialPageSize: 10,
            onPageChange: null,
            onPageSizeChange: null,
            tableClass: 'maximus-base-table',
            theadClass: '',
            rowClass: '',
            gridTitle: 'Data Grid',
            noDataMessage: 'No data available.',
            idProperty: 'id',
            statusProperty: 'DataRankName',
            enableAllColumnSearch: false,
            enableColumnFilters: false,
            enableSorting: false, // overall sorting, defaults to false
            dateFormat: 'MM-DD-YYYY',
            includeTime: false,//  option for formatting dates with time,
            enableClientSideSortNoReset: false,
            //if you are auto generating data without column def but want actions columns 
            autoGenerateActions: false, //atuo generate actions
            //   edit //
            callBackEditFunction: null, //function name for the actions edit
            //   delete //
            callBackDeleteFunction: null, //function name for the actions delete
        }, options);

        let $element = $(this);
        let data = settings.data;
        let page = 1;
        let pageSize = settings.initialPageSize;
        let sortKey = null;
        let sortAsc = true;
        let filters = {};
        let visibleColumns = {};
        let globalSearchText = '';
        let selectedRows = new Set();
        let totalRecords = data.length;


        //handle if now column def defiend so can inject raw json.
        if (settings.columns === undefined || settings.columns.length == 0) settings.columns = dynamicBuildColumns(data);




        settings.columns.forEach(col => {
            col.visible = col.visible !== undefined ? col.visible : true;  // Default to true if not specified
            visibleColumns[col.key] = col.visible;
        });


        //Plugin functionality helper methods

        //Get all selected rows
        $element[0].getSelectedRows = function () {
            return Array.from(selectedRows);
        };

        $element[0].getSelectedRowObjects = function () {

            const idKey = settings.idProperty;

            return data.filter(row =>
                selectedRows.has(String(row[idKey]))  // correct
            );
        };


        // Select a row programmatically
        $element[0].selectRow = function (id) {

            selectedRows.add(id);
            $element.find(`.dg-row-select[data-id="${id}"]`).prop('checked', true);
            updateSelectAllState();
        };

        // Clear row selection
        $element[0].clearSelection = function () {

            selectedRows.clear();
            $element.find('.dg-row-select').prop('checked', false);
            updateSelectAllState();
        };

        // Helper function to toggle column visibility
        $element[0].toggleColumnVisibility = function (columnKey, isVisible) {
            const column = settings.columns.find(col => col.key === columnKey);

            if (column) {
                column.visible = isVisible !== undefined ? isVisible : true;
                visibleColumns[columnKey] = column.visible;
                renderTable();  // Re-render the table after changing visibility
            }
        };

        $element[0].setData = function (newData, total) {
            data = newData || [];
            totalRecords = total || data.length;

            renderServerData();
        };

        // Date parsing function
        function parseDate(str) {
            const date = moment(str, settings.dateFormat, true); // strict mode
            return date.isValid() ? date.toDate() : null;
        }
        function formatValueForSearch(value, col) {
            if (value == null) return '';

            if (col.type === 'date') {
                const date = moment(value);
                return date.isValid()
                    ? date.format(settings.dateFormat).toLowerCase()
                    : '';
            }
            return value.toString().toLowerCase();
        }
        function formatDateForDisplay(dateVal) {
            if (!dateVal) return '';

            const m = moment(dateVal, [settings.dateFormat, "YYYY-MM-DD", moment.ISO_8601], true);
            if (!m.isValid()) return '';

            const formatStr = settings.includeTime
                ? `${settings.dateFormat} HH:mm:ss`
                : settings.dateFormat;

            return m.format(formatStr);
        }
        function updateSortIconsOnly() {

            // reset all icons
            $element.find('.sort-icon').text('unfold_more');

            if (sortKey) {
                const icon = sortAsc ? 'arrow_upward' : 'arrow_downward';

                $element
                    .find(`span[data-key="${sortKey}"] .sort-icon`)
                    .text(icon);
            }
        }

        function scrollActivePageIntoView() {

            const $container = $element.find('.dg-page-numbers');
            const $active = $container.find('.dg-page-num.active');

            if (!$active.length) return;

            const container = $container[0];
            const active = $active[0];

            const containerLeft = container.scrollLeft;
            const containerRight = containerLeft + container.clientWidth;

            const activeLeft = active.offsetLeft;
            const activeRight = activeLeft + active.offsetWidth;

            let newScroll = containerLeft;

            if (activeLeft < containerLeft) {
                newScroll = activeLeft;
            } else if (activeRight > containerRight) {
                newScroll = activeRight - container.clientWidth;
            } else {
                return;
            }

            container.scrollTo({
                left: newScroll,
                behavior: 'smooth'
            });
        }


        function buildTableHeader() {
            const $thead = $element.find('#table-head').empty();

            if (settings.enableRowSelection) {

                const $th = $(`
                            <th style="min-width:0 !important; width:${settings.checkboxColumnWidth}">
                                <input type="checkbox" class="dg-select-all" aria-label="Select all rows">
                            </th>
                        `);

                $thead.append($th);
            }

            if (settings.enableStatusLabel) {

                const $th = $(`
                            <th style="min-width:50px !important; ">
                              <label for="statusField" class="visually-hidden">Verification Status</label>
                            </th>
                        `);

                $thead.append($th);
            }

            settings.columns.forEach(col => {
                if (col.visible === false) return;  // Skip this column if it's not visible

                if (!visibleColumns[col.key]) return;

                const inputType = col.type === 'number' ? 'number' : 'text'; // Always use 'text' for jQuery datepicker
                const filter = filters[col.key] || {};

                let sortIcon = '';

                if (settings.enableSorting !== false && col.sortable !== false) {

                    if (sortKey === col.key) {
                        sortIcon = sortAsc
                            ? '<span class="material-icons sort-icon">arrow_upward</span>'
                            : '<span class="material-icons sort-icon">arrow_downward</span>';
                    } else {
                        sortIcon = '<span class="material-icons sort-icon">unfold_more</span>';
                    }
                }

                const $th = $('<th scope="col"></th>');
                if (col.width) $th.css('width', col.width);  // Apply the column width here

                const hasTitle = col.title != null && col.title !== '';
                const labelText = hasTitle ? col.title + sortIcon : '';
                const $labelSpan = $('<span>')
                    .attr('data-key', col.key)   // 🔥 IMPORTANT
                    .html(col.title + (sortIcon ? ' ' + sortIcon : ''));

                // Sorting
                if (settings.enableSorting !== false && col.sortable !== false) {
                    $labelSpan.css('cursor', 'pointer').click(() => {
                        sortKey === col.key ? sortAsc = !sortAsc : (sortKey = col.key, sortAsc = true);
                        if (settings.enableClientSideSortNoReset) {
                            updateSortIconsOnly(); // only icon update
                            renderTable(); // no header rebuild → no page reset

                        } else {

                            // existing behavior untouched
                            buildTableHeader();
                            renderTable();
                        }
                    });
                } else {
                    $labelSpan.css('cursor', 'default');
                }

                if (settings.enableColumnFilters && hasTitle && col.type) {
                    const placeholderStr = col.type === 'date'
                        ? settings.dateFormat
                            .match(/(DD|MM|YYYY)/g)
                            ?.join('-') || 'DD-MM-YYYY'
                        : 'Filter...';

                    const value1 = col.type === 'date' && filter.value1
                        ? moment(filter.value1, [settings.dateFormat, "YYYY-MM-DD", moment.ISO_8601])
                            .format(settings.dateFormat)
                        : '';

                    const value2 = col.type === 'date' && filter.value2
                        ? moment(filter.value2, [settings.dateFormat, "YYYY-MM-DD", moment.ISO_8601])
                            .format(settings.dateFormat)
                        : '';

                    const $inputGroup = $(`
                <div class="input-group input-group-sm mt-1">
                    <input type="${inputType}" class="form-control filter-input filter-val1" placeholder="${placeholderStr}" data-key="${col.key}" value="${value1}">
                    <button class="btn btn-outline-secondary dropdown-toggle btn-margin" type="button" data-bs-toggle="dropdown" aria-expanded="false" aria-haspopup="true" aria-label="Filter options">
                        <i class="bi bi-funnel"></i>
                    </button>
                    <ul class="dropdown-menu dropdown-menu-end p-3" style="min-width: 250px;">
                        <li class="mb-3">
                            <select class="form-select form-select-custom filter-operator" data-key="${col.key}">
                            <option value="contains">Contains</option>    
                            <option value="eq">Equal</option>
                                <option value="neq">Not Equal</option>
                                ${(col.type === 'number' || col.type === 'date') ? `
                                    <option value="lt">Less Than</option>
                                    <option value="gt">Greater Than</option>
                                    <option value="lte">Less Than or Equal</option>
                                    <option value="gte">Greater Than or Equal</option>
                                    <option value="between">Between</option>` : `                                    
                                    <option value="startsWith">Starts With</option>
                                    <option value="endsWith">Ends With</option>`}
                            </select>
                        </li>
                        <li class="mb-2">
                            <input type="${inputType}" class="form-control filter-val2 filter-input-between" placeholder="Second value (for between)" style="display: none;" value="${value2}">
                        </li>
                        <li class="d-flex justify-content-between mt-2">
                            <button type="button" class="btn btn-primary apply-filter" data-key="${col.key}">Apply</button>
                            <button type="button" class="btn btn-outline-purple clear-filter" data-key="${col.key}">Reset</button>
                        </li>
                    </ul>
                </div>
            `);

                    const $dropdown = $inputGroup.find('.dropdown-menu');
                    const $operator = $dropdown.find('.filter-operator');
                    const $val1 = $inputGroup.find('.filter-val1');
                    const $val2 = $dropdown.find('.filter-val2');

                    $operator.on('change', function () {
                        $(this).val() === 'between' ? $val2.show() : $val2.hide().val('');
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

                        if (op === 'between' && (!value1 || !value2)) {
                            alert("Please enter both values for 'Between' filter.");
                            return;
                        }

                        if (!value1) {
                            delete filters[col.key];
                            $val1.val('');
                            $val2.val('').hide();
                            page = 1;
                            renderTable();
                            bootstrap.Dropdown.getInstance($inputGroup.find('.dropdown-toggle')[0])?.hide();
                            return;
                        }

                        filters[col.key] = { op, value1, value2 };
                        page = 1;
                        renderTable();
                        bootstrap.Dropdown.getInstance($inputGroup.find('.dropdown-toggle')[0])?.hide();
                    });

                    // Clear filter
                    $dropdown.find('.clear-filter').off('click').on('click', function () {

                        delete filters[col.key];

                        // Reset UI properly
                        $operator.prop('selectedIndex', 0);   // FIXED
                        $val1.val('');
                        $val2.val('').hide();

                        page = 1;
                        renderTable();

                        bootstrap.Dropdown
                            .getInstance($inputGroup.find('.dropdown-toggle')[0])
                            ?.hide();
                    });

                    $dropdown.on('click', function (e) {
                        e.stopPropagation();
                    });

                    const $wrapper = $('<div class="d-flex flex-column"></div>');
                    $wrapper.append($labelSpan).append($inputGroup);
                    $th.append($wrapper);

                    // Initialize jQuery datepicker for date inputs
                    if (col.type === 'date') {
                        setTimeout(() => {
                            $th.find('.filter-val1, .filter-val2').datepicker({
                                dateFormat: convertToJqueryDateFormat(settings.dateFormat),
                                changeMonth: true,
                                changeYear: true
                            });
                        }, 0);
                    }

                    // ENTER KEY HANDLER
                    function handleEnterKey(e) {
                        if (e.key === 'Enter') {
                            e.preventDefault(); // prevent page refresh

                            const op = $operator.val();
                            const value1 = $val1.val().trim();
                            const value2 = op === 'between' ? $val2.val().trim() : '';

                            // BETWEEN validation
                            if (op === 'between') {
                                if (!value1 || !value2) {
                                    alert("Please enter both values for 'Between' filter.");
                                    return;
                                }
                            }

                            // If empty => clear filter
                            if (!value1) {
                                delete filters[col.key];
                                page = 1;
                                renderTable();
                                return;
                            }

                            // APPLY FILTER
                            filters[col.key] = { op, value1, value2 };
                            page = 1;
                            renderTable();
                        }
                    }

                    // bind ENTER to BOTH inputs
                    $val1.on('keydown', handleEnterKey);
                    $val2.on('keydown', handleEnterKey);


                } else {
                    $th.append($labelSpan);
                }

                $thead.append($th);
            });
        }

        // Helper to convert Moment-like format to jQuery UI format
        function convertToJqueryDateFormat(format) {
            return format
                .replace(/DD/g, 'dd')
                .replace(/MM/g, 'mm')
                .replace(/YYYY/g, 'yy')
                .replace(/YY/g, 'y');
        }

        function applyFilters(data) {
            return data.filter(row => {
                return Object.entries(filters).every(([key, filter]) => {
                    const col = settings.columns.find(c => c.key === key);
                    const colType = col?.type || 'text';

                    const rawVal = row[key];
                    const val = rawVal != null ? rawVal.toString().toLowerCase() : '';
                    const value1 = (filter.value1 || '').toLowerCase();
                    const value2 = (filter.value2 || '').toLowerCase();

                    if (colType === 'number') {
                        const numVal = parseFloat(val);
                        const numVal1 = parseFloat(value1);
                        const numVal2 = parseFloat(value2);

                        switch (filter.op) {
                            case 'eq': return numVal === numVal1;
                            case 'neq': return numVal !== numVal1;
                            case 'lt': return numVal < numVal1;
                            case 'gt': return numVal > numVal1;
                            case 'lte': return numVal <= numVal1;
                            case 'gte': return numVal >= numVal1;
                            case 'between': return !isNaN(numVal) && !isNaN(numVal1) && !isNaN(numVal2) && numVal >= numVal1 && numVal <= numVal2;
                            default: return true;
                        }

                    } else if (colType === 'date') {
                        const dateVal = moment(rawVal, [settings.dateFormat, "YYYY-MM-DD", moment.ISO_8601], true);
                        const dateVal1 = moment(filter.value1, [settings.dateFormat, "YYYY-MM-DD", moment.ISO_8601], true);
                        const dateVal2 = moment(filter.value2, [settings.dateFormat, "YYYY-MM-DD", moment.ISO_8601], true);

                        if (!dateVal.isValid()) return false;

                        switch (filter.op) {
                            case 'eq': return dateVal.isSame(dateVal1, 'day');
                            case 'neq': return !dateVal.isSame(dateVal1, 'day');
                            case 'lt': return dateVal.isBefore(dateVal1, 'day');
                            case 'gt': return dateVal.isAfter(dateVal1, 'day');
                            case 'lte': return dateVal.isSameOrBefore(dateVal1, 'day');
                            case 'gte': return dateVal.isSameOrAfter(dateVal1, 'day');
                            case 'between': return dateVal1.isValid() && dateVal2.isValid() && dateVal.isBetween(dateVal1, dateVal2, 'day', '[]');
                            default: return true;
                        }

                    } else {
                        // text/string comparisons
                        switch (filter.op) {
                            case 'eq': return val === value1;
                            case 'neq': return val !== value1;
                            case 'contains': return val.includes(value1);
                            case 'startsWith': return val.startsWith(value1);
                            case 'endsWith': return val.endsWith(value1);
                            default: return true;
                        }
                    }
                });
            });
        }


        function renderTable() {
            const $tbody = $element.find('#table-body').empty();
            let filtered = applyFilters(data);

            if (globalSearchText) {
                const search = globalSearchText?.toLowerCase().trim();
                if (search) {
                    filtered = filtered.filter(row =>
                        settings.columns.some(col =>
                            visibleColumns[col.key] &&
                            formatValueForSearch(row[col.key], col).includes(search)
                        )
                    );
                }
            }

            // Apply sorting if enabled and sortKey exists
            if (settings.enableSorting !== false && sortKey) {
                filtered.sort((a, b) => {
                    let valA = a[sortKey], valB = b[sortKey];
                    if (typeof valA === "string") valA = valA.toLowerCase();
                    if (typeof valB === "string") valB = valB.toLowerCase();
                    return sortAsc ? (valA > valB ? 1 : -1) : (valA < valB ? 1 : -1);
                });
            }

            let paged;

            // 🔥 HYBRID MODE (server paging + client sort)
            if (settings.enableClientSideSortNoReset) {

                // 👉 DO NOT slice again (data already paged from server)
                paged = [...filtered];

                // apply sorting ONLY on current page
                if (settings.enableSorting !== false && sortKey) {
                    paged.sort((a, b) => {

                        let valA = a[sortKey], valB = b[sortKey];

                        if (typeof valA === "string") valA = valA.toLowerCase();
                        if (typeof valB === "string") valB = valB.toLowerCase();

                        return sortAsc
                            ? (valA > valB ? 1 : -1)
                            : (valA < valB ? 1 : -1);
                    });
                }

            } else {

                // 👉 normal client-side mode
                const start = (page - 1) * pageSize;
                paged = filtered.slice(start, start + pageSize);
            }

            if (paged.length === 0) {
                let colspan = settings.columns.filter(col => visibleColumns[col.key]).length;

                if (settings.enableRowSelection) colspan += 1;

                if (settings.enableStatusLabel) colspan += 1;


                $tbody.append(`<tr><td colspan="${colspan}" class="text-center no-records">${settings.noDataMessage}</td></tr>`);
            } else {
                paged.forEach((row, index) => {

                    // If rowTemplate is provided use it
                    if (settings.rowTemplate && typeof settings.rowTemplate === "function") {
                        const zebra = index % 2 === 0 ? 'dg-even' : 'dg-odd';

                        // create formatted row copy
                        const formattedRow = Object.assign({}, row);

                        settings.columns.forEach(col => {
                            if (col.type === 'date' && formattedRow[col.key]) {
                                formattedRow[col.key] = formatDateForDisplay(formattedRow[col.key]);
                            }
                        });


                        const html = settings.rowTemplate(formattedRow, settings.columns);

                        const $rows = $(html).addClass(zebra);

                        $tbody.append($rows);

                        return;
                    }

                    const $tr = $('<tr class="dg-data-row">');

                    const selectedId = $("[id*=hdnSelectedCredentialingId]").val();

                    if (selectedId && String(row[settings.idProperty]) === String(selectedId)) {
                        $tr.addClass("highlight-row");
                    }

                    if (settings.enableRowSelection) {

                        const rowId = row[settings.idProperty];
                        const checked = selectedRows.has(rowId) ? 'checked' : '';

                        $tr.append(`
                                    <td>
                                        <input type="checkbox"
                                               class="dg-row-select"
                                               data-id="${rowId}"
                                               ${checked}
                                               aria-label="Select row ${rowId}">
                                    </td>
                                `);
                    }


                    if (settings.enableStatusLabel) {

                        const status = row[settings.statusProperty];
                        const rowId = row[settings.idProperty];

                        let icon = '';
                        let iconColor = '';
                        if (status === 'Pending') {
                            icon = 'Empty-checkbox-20.png';
                            iconColor = 'Empty Checkbox';
                        } else if (status === 'Pass') {
                            icon = 'checkbox-20.png';
                            iconColor = 'Green Checkbox';
                        } else if (status === 'Fail') {
                            icon = 'failed-20.png';
                            iconColor = 'Red Checkbox';

                        }
                        /* ${status}*/
                        $tr.append(`
                                    <td>                                   
                                        <img type="image" alt="${iconColor}"  class="dg-row-lsbrl" data-id="${rowId}" src="${imageBasePath}${icon}">
                                    </td>
                                `);
                    }



                    settings.columns.forEach(col => {
                        if (col.visible === false) return;

                        if (!visibleColumns[col.key]) return;

                        let value = row[col.key];
                        if (value === null || value === undefined || value === '') {
                            value = '';
                        }

                        if (col.cellTemplate !== null && col.cellTemplate !== undefined && col.cellTemplate !== '') {
                            if (typeof col.cellTemplate === 'function') {
                                value = col.cellTemplate(row);
                            } else if (typeof col.cellTemplate === 'string') {
                                value = col.cellTemplate.replace(/{([^}]+)}/g, (match, p1) => row[p1] || '');
                            }
                        }
                        else if (col.type === 'date' && value) {
                            // If date, format for display
                            value = formatDateForDisplay(value);
                        }

                        const widthAttr = col.width ? ` style="width: ${col.width};"` : '';
                        $tr.append(`<td${widthAttr}>${value}</td>`);
                    });

                    //this is if autogenerate actions is set to true
                    if (settings.autoGenerateActions) {
                        $tr.append(processCommandColumn(row));
                    }

                    $tbody.append($tr);
                });

                updateSelectAllState();
            }

            const total = settings.enableClientSideSortNoReset
                ? totalRecords
                : filtered.length;

            if (total > 0) {

                const startIndex = (page - 1) * pageSize + 1;
                const endIndex = Math.min(page * pageSize, total);

                $element.find('#page-info').text(
                    `${startIndex} to ${endIndex} of ${total} entries`
                ).show();

                $element.find('.dg-pagination-wrapper').show();

                renderPagination(Math.ceil(total / pageSize));

            } else {

                page = 1; // keep this

                $element.find('#page-info').hide();

                $element.find('.dg-pagination-wrapper').hide();
            }
        }

        function renderServerData() {

            const $tbody = $element.find('#table-body').empty();

            if (data.length === 0) {
                let colspan = settings.columns.filter(col => visibleColumns[col.key]).length;
                if (settings.enableRowSelection) colspan += 1;

                $tbody.append(`<tr>
            <td colspan="${colspan}" class="text-center no-records">
                ${settings.noDataMessage}
            </td>
        </tr>`);
            } else {

                data.forEach((row) => {

                    const $tr = $('<tr class="dg-data-row">');

                    if (settings.enableRowSelection) {
                        const rowId = row[settings.idProperty];
                        const checked = selectedRows.has(rowId) ? 'checked' : '';

                        $tr.append(`
                    <td>
                        <input type="checkbox"
                               class="dg-row-select"
                               data-id="${rowId}"
                               ${checked}
                               aria-label="Select row ${rowId}">
                    </td>
                `);
                    }

                    settings.columns.forEach(col => {

                        if (col.visible === false) return; // FIX
                        if (!visibleColumns[col.key]) return;

                        let value = row[col.key] ?? '';

                        if (col.cellTemplate) {
                            value = typeof col.cellTemplate === 'function'
                                ? col.cellTemplate(row)
                                : value;
                        }
                        else if (col.type === 'date' && value) {
                            value = formatDateForDisplay(value);
                        }

                        $tr.append(`<td>${value}</td>`);
                    });

                    $tbody.append($tr);
                });
            }

            // always call this
            updateSelectAllState();

            const start = (page - 1) * pageSize;

            const total = settings.enableClientSideSortNoReset
                ? totalRecords
                : data.length;

            if (total > 0) {

                const startIndex = (page - 1) * pageSize + 1;
                const endIndex = Math.min(page * pageSize, total);

                $element.find('#page-info').text(
                    `${startIndex} to ${endIndex} of ${total} entries`
                ).show();

                $element.find('#pagination').parent().show();

                renderPagination(Math.ceil(total / pageSize));

            } else {
                $element.find('#page-info').hide();
                $element.find('#pagination').parent().hide();
            }
        }

        function updateSelectAllState() {

            if (!settings.enableRowSelection) return;

            const $rows = $element.find('.dg-row-select');

            if ($rows.length === 0) {
                $element.find('.dg-select-all').prop('checked', false);
                return;
            }

            const allChecked = $rows.length === $rows.filter(':checked').length;

            $element.find('.dg-select-all').prop('checked', allChecked);
        }

        function renderPagination(totalPages) {

            const $wrapper = $element.find('.dg-pagination-wrapper');
            const $prev = $wrapper.find('.dg-page-prev');
            const $numbers = $wrapper.find('.dg-page-numbers');
            const $next = $wrapper.find('.dg-page-next');

            // BUILD buttons + bind events ONLY ONCE
            if ($numbers.children().length === 0) {

                // build prev once
                $prev.html(`
                    <button type="button" class="btn btn-sm btn-light dg-prev-btn">
                        Previous
                    </button>
                `);

                // build next once
                $next.html(`
                    <button type="button" class="btn btn-sm btn-light dg-next-btn">
                        Next
                    </button>
                `);

                // bind events ONCE

                // page click
                $numbers.on('click', '.dg-page-num', function () {
                    page = parseInt($(this).data('page'));
                    renderTable();
                });

                // prev click
                $prev.on('click', '.dg-prev-btn', function () {
                    if (page === 1) return;
                    page--;
                    renderTable();
                });

                // next click
                $next.on('click', '.dg-next-btn', function () {

                    const total = settings.enableClientSideSortNoReset
                        ? totalRecords
                        : applyFilters(data).length;

                    const currentTotalPages = Math.ceil(total / pageSize);

                    if (page >= currentTotalPages) return;

                    page++;
                    renderTable();
                });
            }

            // REBUILD page numbers ONLY if count changed
            const existingPages = $numbers.children().length;

            if (existingPages !== totalPages) {

                let html = '';
                for (let i = 1; i <= totalPages; i++) {
                    html += `
                <span class="dg-page-num" data-page="${i}">
                    ${i}
                </span>
            `;
                }

                $numbers.html(html);
            }

            // ALWAYS update UI state (active, disabled, scroll)
            updatePaginationUI(totalPages);
        }

        function updatePaginationUI(totalPages) {

            const $wrapper = $element.find('.dg-pagination-wrapper');
            const $numbers = $wrapper.find('.dg-page-num');

            const isFirst = page === 1;
            const isLast = page === totalPages;

            // update active class ONLY
            $numbers.removeClass('active');
            $numbers.filter(`[data-page="${page}"]`).addClass('active');

            // update buttons
            $wrapper.find('.dg-prev-btn')
                .toggleClass('disabled', isFirst);

            $wrapper.find('.dg-next-btn')
                .toggleClass('disabled', isLast);

            // SMALL SCROLL ONLY (no jump)
            scrollActiveIntoViewStable();
        }

        function scrollActiveIntoViewStable() {

            const container = $element.find('.dg-page-numbers')[0];
            const active = $element.find('.dg-page-num.active')[0];

            if (!container || !active) return;

            const containerRect = container.getBoundingClientRect();
            const activeRect = active.getBoundingClientRect();

            const isFullyVisible =
                activeRect.left >= containerRect.left &&
                activeRect.right <= containerRect.right;

            if (isFullyVisible) {
                return;
            }

            const scrollAmount =
                activeRect.left -
                containerRect.left -
                (container.clientWidth / 2) +
                (active.clientWidth / 2);

            container.scrollLeft += scrollAmount;
        }


        function updatePaginationState(totalPages) {
            const $pagination = $element.find('#pagination');

            $pagination.find('.page-item').removeClass('active disabled');

            // Active page
            $pagination.find(`.page-num[data-page="${page}"]`).addClass('active');

            // Disable Previous
            if (page === 1) {
                $pagination.find('.page-prev').addClass('disabled');
            }

            // Disable Next
            if (page === totalPages) {
                $pagination.find('.page-next').addClass('disabled');
            }
        }

        function fetchDataFromApi() {
            if (settings.apiUrl) {
                $.ajax({
                    url: settings.apiUrl,
                    method: 'GET',
                    success: function (response) {
                        data = response;
                        page = 1;
                        buildTableHeader();
                        renderTable();
                    },
                    error: function (err) {
                        alert("Error loading data.");
                        console.error(err);
                    }
                });
            } else {
                page = 1;
                buildTableHeader();
                renderTable();
            }
        }


        function dynamicBuildColumns(data) {

            if (!data || data.length === 0) return [];

            const _el = data[0];

            return Object.keys(_el).map(key => {

                const date = new Date(_el[key]);

                if (!isNaN(date.getTime()) && /^\d{4}-\d{2}-\d{2}/.test(_el[key])) {
                    return {
                        key: key,
                        type: 'date',
                        title: key,
                        sortable: true
                    };
                }
                else {
                    return {
                        key: key,
                        type: typeof _el[key],
                        title: key,
                        sortable: true
                    };
                }
            });
        }

        function isDateColumn(val) {
            let isoDateRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?Z$/;
            return typeof val === 'string' && isoDateRegex.test(val);
        }

        //buidls the dynamic edit or delete column for the row if set to.
        function processCommandColumn(row, widthAttr) {
            let retHtml = '<td><div class="dropdown pd-dropdown">';

            if (settings.callBackDeleteFunction || settings.callBackEditFunction) {
                retHtml += `
                                <button class="dropdown-button border-0 three-dots-btn"
                                          data-bs-toggle="dropdown"
                                          data-bs-auto-close="outside"
                                          aria-expanded="false"
                                          type="button"
                                          title="Actions">
                                    <span class="material-symbols-outlined">more_vert</span>
                                  </button>
                                  <ul class="dropdown-menu">`;

                if (settings.callBackEditFunction) {


                    retHtml += `      
                              <li>
                                      <a href="javascript:void(0)"
                                        class="dropdown-item "
                                        data-row='${JSON.stringify(row)}'
                                        onclick="${settings.callBackEditFunction}(this)"
                                        >
                                        Edit
                                    </a>
                                    </li>`;

                }


                if (settings.callBackDeleteFunction) {
                    retHtml += `
                        <li>
                        <a href="javascript:void(0)"
                            class="dropdown-item "
                            data-row='${JSON.stringify(row)}'
                            onclick="${settings.callBackDeleteFunction}(this)">
                            Delete
                        </a>
                    </li>`;
                }

                retHtml += '</ul></div></td>';
            }
            return retHtml;
        }

        function tableRowToJSON(row) {
            const table = row.closest('table');
            if (!table) throw new Error("Row is not inside a table.");

            const headers = Array.from(table.querySelectorAll('thead th'))
                .map(th => th.textContent.trim());

            const cells = Array.from(row.cells).map(td => td.textContent.trim());

            // Map headers to cell values
            const rowData = {};
            headers.forEach((header, index) => {
                rowData[header] = cells[index] !== undefined ? cells[index] : null;
            });
        }

        function initialize() {
            $element.html(`
                ${settings.gridTitle && settings.gridTitle.trim() !== ""
                    ? `<h3>${settings.gridTitle}</h3>`
                    : ""
                }
                <div class="row mb-3">
                   
                    ${settings.enableAllColumnSearch ? `
                    <div class="col-md-6 d-flex align-items-center justify-content-md-end mt-2 mt-md-0">
                        <label for="common-search" class="form-label mb-0 me-2">Search:</label>
                        <input type="search" id="common-search" class="form-control form-control-sm w-100 w-md-auto" style="max-width: 300px;" placeholder="Search all columns..." aria-label="Search all columns">
                    </div>
                    ` : ''}
                </div>
                <div class="table-responsive">
                    <table class="${settings.tableClass}">
                        <thead>
                            <tr id="table-head"></tr>
                        </thead>
                        <tbody id="table-body"></tbody>
                    </table>
                </div>
                <div class="row mt-3 align-items-center">

    <div class="col-md-6 d-flex align-items-center gap-2 flex-wrap">

        <span class="small">Showing</span>

        <select id="page-size-dropdown"
                class="form-select w-auto"
                aria-label="Rows per page select">
        </select>

        <span id="page-info" class="text-muted small"></span>

    </div>

    <div class="col-md-6">
        <nav aria-label="Page navigation" class="d-flex justify-content-md-end">
            <div class="dg-pagination-wrapper">
                <div class="dg-page-prev"></div>

                <div class="dg-page-numbers-wrapper">
                    <div class="dg-page-numbers"></div>
                </div>

                <div class="dg-page-next"></div>
            </div>
        </nav>
    </div>

</div>
            `);

            let $pageSizeDropdown = $element.find('#page-size-dropdown');
            settings.pageSizeOptions.forEach(size => {
                $pageSizeDropdown.append(`<option value="${size}" ${size === settings.initialPageSize ? 'selected' : ''}>${size}</option>`);
            });

            $element.find('#page-size-dropdown').change(function () {
                pageSize = parseInt($(this).val());
                page = 1;
                if (settings.onPageSizeChange) {
                    settings.onPageSizeChange({ page, pageSize });
                } else {
                    renderTable();
                }
            });

            if (settings.enableColumnFilters) {
                $element.find('#reset-all-filters').click(function () {
                    filters = {};
                    $element.find('.filter-input').val('');
                    $element.find('.filter-operator').val('eq');
                    $element.find('.filter-val2').val('').hide();
                    $element.find('.dropdown-menu.show').each(function () {
                        const toggleBtn = $(this).siblings('[data-bs-toggle="dropdown"]')[0];
                        if (toggleBtn) {
                            const bsDropdown = bootstrap.Dropdown.getInstance(toggleBtn);
                            if (bsDropdown) bsDropdown.hide();
                        }
                    });
                    Object.keys(visibleColumns).forEach(key => {
                        visibleColumns[key] = true;
                    });
                    $element.find('#common-search').val('');
                    globalSearchText = '';
                    $element.find('#page-size-dropdown').val(settings.initialPageSize);
                    pageSize = settings.initialPageSize;
                    sortKey = null;
                    sortAsc = true;
                    page = 1;
                    buildTableHeader();
                    renderTable();
                });
            }
            if (settings.enableAllColumnSearch) {
                $element.find('#common-search').on('input', function () {
                    globalSearchText = $(this).val().toLowerCase();
                    page = 1;
                    renderTable();
                });
            }

            // select all checkbox
            $element.on('change', '.dg-select-all', function () {

                const checked = $(this).is(':checked');

                $element.find('.dg-row-select').each(function () {

                    const id = String($(this).attr('data-id')); // FIX

                    $(this).prop('checked', checked);

                    if (checked) {
                        selectedRows.add(id);
                    } else {
                        selectedRows.delete(id);
                    }
                });
            });

            $element.on('change', '.dg-row-select', function () {


                const id = String($(this).attr('data-id')); // FIX

                if ($(this).is(':checked')) {
                    selectedRows.add(id);
                } else {
                    selectedRows.delete(id);
                }


                const total = $element.find('.dg-row-select').length;
                const selected = $element.find('.dg-row-select:checked').length;

                $element.find('.dg-select-all').prop('checked', total === selected);

            });


            //fetchDataFromApi();
            buildTableHeader();
            renderTable();
        }

        initialize();

        return this;
    };
})(jQuery);