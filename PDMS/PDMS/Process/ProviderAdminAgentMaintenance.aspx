<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Process_ProviderAdminAgentMaintenance" Codebehind="ProviderAdminAgentMaintenance.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div class="row"><br /></div>
    <div class="row"> Provider Account Administration</div>
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

        .divAgentSearchResults {
            overflow: auto;
            max-height: calc(100vh - 285px);
            width: 100%;
        }

            .divAgentSearchResults thead {
                background: #545486;
                color: #ffffff;
            }

            .divAgentSearchResults tbody {
                background: #E8F0F9;
            }

            .divAgentSearchResults th {
                padding: 14px !important;
            }

            .divAgentSearchResults select {
                min-width: inherit !important;
            }

        .table > tbody > tr:nth-child(odd) > td,
        .table > tbody > tr:nth-child(odd) > th {
            text-align: left;
            vertical-align: top;
            background-color: #EAEFF7;
            border: solid 1px black;
        }

        .table > tbody > tr:nth-child(even) > td,
        .table > tbody > tr:nth-child(even) > th {
            text-align: left;
            vertical-align: top;
            background-color: #E8F0F9;
            border: solid 1px black;
        }

        .table > thead > tr > th,
        .table > tbody > tr > td {
            white-space: nowrap;
            vertical-align: middle;
        }

         /* The Modal (background) */
        .modal1 {
                display: none; /* Hidden by default */
                position: fixed; /* Stay in place */
                z-index: 100; /* Sit on top */
                left: 50%;
                top: 50%;
                width: 80%;
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
       

        /* The Close Button */
        .close {
            color: #aaaaaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .btn-group1 button {
            font-size: 2rem;
        }

        .input-group-btn {
            position: initial !important;
        }

        /*for drop down*/
        .select-container {
            position: relative;
            width: 100%;
        }


        .select-button {
            border: 1px solid #ccc;
            padding: 8px;
            cursor: pointer;
            background-color: #f9f9f9;
            display: flex;
            justify-content: space-between;
            align-items: center;
            min-height: 46px !important;
            font-size: 2rem;
            text-align: center;
        }


        .select-dropdown {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            border: 1px solid #ccc;
            border-top: none;
            display: none;
            background-color: #fff;
            z-index: 1;
        }


            .select-dropdown.show {
                display: block;
            }


        .search-input {
            padding: 8px;
            border-bottom: 1px solid #eee;
            position: sticky;
            height: 46px;
            font-size: 16pt !important;
        }


        .options-list {
            max-height: 200px;
            overflow-y: auto;
            padding: 0;
            margin: 0;
            list-style: none;
            padding: 8px;
        }


            .options-list li {
                padding: 8px;
                cursor: pointer;
            }


                .options-list li:hover {
                    background-color: #eee;
                }


                .options-list li input[type="checkbox"] {
                    margin-right: 8px;
                }

        .select-container .form-control, .input-group-btn {
            display: table-cell;
        }

        .ddlMedIDContainer1 {
            z-index: 4;
        }       

        /* 3-column layout styling */
        #divAddAgentRoleList {
            display: grid;
            grid-template-columns: repeat(3, 1fr); /* 3 equal columns */
            gap: 10px; /* space between items */
            max-width: 90%;
        }

            #divAddAgentRoleList div {
                display: flex;
                align-items: center;
            }

            #divAddAgentRoleList label {
                margin-left: 5px;
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

        .btnBox .buttonBoxFocus {
            background-color: #005ea2 !important;
        }

        .highlighted-row {
            text-align: left;
            vertical-align: top;
            border: solid 1px black;
            background-color: yellow !important;
        }

        .mt-15 {
            margin-top: 15px;
        }
       /* #btnExcel{
            background:url('../Images/Excel_24x24.png');
        }*/
    </style>

    <script type="text/javascript">
        let selectedAgentOHIDs = '';
        let selectedMedIDs = '';
        let selectedAgentRoles = '';
        let searchResults = [];
        let isCostRptMgmtAgent = 'false';
        let page = 1;
        let pageSize = 10;
        let sortColumn = null; // can be 'MedicaidID' or 'AgentOHID'
        let sortDirection = 'asc'; // 'asc' or 'desc'

        const columns = [
            { key: 'Edit', title: '', type: 'text' },
            { key: 'Deactivate', title: '', type: 'text' },
            { key: 'MedicaidID', title: 'Medicaid ID', type: 'text' },
            { key: 'ProviderName', title: 'Provider Name', type: 'text' },
            { key: 'AgentOHID', title: 'Agent OH ID', type: 'text' },
            { key: 'AgentName', title: 'Agent Name', type: 'text' },
            { key: 'AgentRoles', title: 'Agent Roles', type: 'text' }
        ];        

        $(function () {
            initializeAgentDropdowns();
            isCostRptMgmtAgent = $('#<%=hdnCostReportMgmtUser.ClientID%>').val();
            $('#divExcel').hide();
        });

        function initializeAgentDropdowns() {
            $('.ddlAgentRoleSearchtext, .ddlAgentOHIDSearchtext, .ddlMedIDSearchtext1').val('');
            const APIToken = $("[id*=hdnAccessToken]").val();
            const userId = $('#<%=hdnUserName.ClientID%>').val();
            const selectedProviderAdminuserId = $('#<%=hdnSelectedProviderAdmin.ClientID%>').val();
            console.log(selectedProviderAdminuserId);
            $.ajax({
                type: "GET",
                url: webApiUserMaintenance + "GetAgentAdministrationDropdowns?loggedinUserID=" + userId + "&&selectedProvAdminUserID=" + selectedProviderAdminuserId,
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
                    setupAgentRolesDropdown(result.AgentRole);
                    setupMedicaidIDDropdown(result.AssignedMedID);
                    setupAgentOHIDDropdown(result.AssignedAgent);
                }
            }).fail(function (jqXHR) {
                handleAjaxError(jqXHR, 'pnlShowError1', 'Error loading dropdown data: ');
            });
        }

        function setupAgentRolesDropdown(agentRoles) {
            if (!agentRoles || !agentRoles.length) return;

            const $container = $('.ddlAgentRoleContainer');
            const $input1 = $('.ddlAgentRolebutton1');
            const $input2 = $('.ddlAgentRolebutton2');
            const $dropdown = $('.ddlAgentRoleSelectDropdown');
            const $list = $('.agent-role-dropdown-checkbox-list').empty();

            $input1.prop("readonly", true).css({ backgroundColor: '#fff' });

            agentRoles.forEach(role => {
                const roleDesc = role.AGENT_SUB_ROLES_DESC;
                $list.append(
                    $('<li>').append(
                        $('<label>').css({ fontWeight: 'normal', fontSize: '2rem' }).append(
                            $('<input>', { type: 'checkbox', value: roleDesc }),
                            roleDesc
                        )
                    )
                );
            });

            $input1.add($input2).off('click.agentRoleToggle').on('click.agentRoleToggle', () => $dropdown.toggleClass('show'));

            const $searchInput = $('.ddlAgentRoleSearchtext');
            $searchInput.off('input.agentRoleSearch').on('input.agentRoleSearch', function () {
                const searchText = $(this).val().toLowerCase();
                $list.children('li').each(function () {
                    const text = $(this).text().toLowerCase();
                    $(this).toggle(text.indexOf(searchText) > -1);
                });
            });

            $(document).off('click.agentRoleDismiss').on('click.agentRoleDismiss', function (e) {
                if (!$container.is(e.target) && $container.has(e.target).length === 0) {
                    $dropdown.removeClass('show');
                }
            });

            $list.off('click.agentRoleCheckbox').on('click.agentRoleCheckbox', 'input[type=checkbox]', function () {
                const checked = $list.find('input[type=checkbox]:checked').map(function () {
                    return this.value;
                }).get();

                $input1.val(checked.length ? checked.join(', ') : '');
                selectedAgentRoles = $input1.val();
            });
            syncDropdownCheckboxesFromInput(selectedAgentRoles, 'role');
        }

        function setupMedicaidIDDropdown(medIDList) {
            if (!medIDList || !medIDList.length) return;

            const $container = $('.ddlMedIDContainer1');
            const $input1 = $('.ddlMedIDbutton1');
            const $input2 = $('.ddlMedIDbutton2');
            const $dropdown = $('.ddlMedIDSelectDropdown1');
            const $list = $('.medicaidid-dropdown-checkbox-list1').empty();

            $input1.prop("readonly", true).css({ backgroundColor: '#fff' });

            medIDList.forEach(item => {
                const medId = item.MEDICAID_ID;
                $list.append(
                    $('<li>').append(
                        $('<label>').css({ fontWeight: 'normal', fontSize: '2rem' }).append(
                            $('<input>', { type: 'checkbox', value: medId }),
                            medId
                        )
                    )
                );
            });

            $input1.add($input2).off('click.medIDToggle').on('click.medIDToggle', () => $dropdown.toggleClass('show'));

            const $searchInput = $('.ddlMedIDSearchtext1');
            $searchInput.off('input.medIDSearch').on('input.medIDSearch', function () {
                const searchText = $(this).val().toLowerCase();
                $list.children('li').each(function () {
                    const text = $(this).text().toLowerCase();
                    $(this).toggle(text.indexOf(searchText) > -1);
                });
            });

            $(document).off('click.medIDDismiss').on('click.medIDDismiss', function (e) {
                if (!$container.is(e.target) && $container.has(e.target).length === 0) {
                    $dropdown.removeClass('show');
                }
            });

            $list.off('click.medIDCheckbox').on('click.medIDCheckbox', 'input[type=checkbox]', function () {
                const checked = $list.find('input[type=checkbox]:checked').map(function () {
                    return this.value;
                }).get();

                $input1.val(checked.length ? checked.join(', ') : '');
                selectedMedIDs = $input1.val();
            });
            syncDropdownCheckboxesFromInput(selectedMedIDs, 'med');
        }

        function setupAgentOHIDDropdown(agentList) {
            if (!agentList || !agentList.length) return;

            const $container = $('.ddlAgentOHIDContainer');
            const $input1 = $('.ddlAgentOHIDbutton1');
            const $input2 = $('.ddlAgentOHIDbutton2');
            const $dropdown = $('.ddlAgentOHIDSelectDropdown');
            const $list = $('.agent-ohid-dropdown-checkbox-list').empty();

            $input1.prop("readonly", true).css({ backgroundColor: '#fff' });

            agentList.forEach(agent => {
                const agentOHID = agent.AGENT_OH_ID;
                $list.append(
                    $('<li>').append(
                        $('<label>').css({ fontWeight: 'normal', fontSize: '2rem' }).append(
                            $('<input>', { type: 'checkbox', value: agentOHID }),
                            agentOHID
                        )
                    )
                );
            });

            $input1.add($input2).off('click.agentOHIDToggle').on('click.agentOHIDToggle', () => $dropdown.toggleClass('show'));

            const $searchInput = $('.ddlAgentOHIDSearchtext');
            $searchInput.off('input.agentOHIDSearch').on('input.agentOHIDSearch', function () {
                const searchText = $(this).val().toLowerCase();
                $list.children('li').each(function () {
                    const text = $(this).text().toLowerCase();
                    $(this).toggle(text.indexOf(searchText) > -1);
                });
            });

            $(document).off('click.agentOHIDDismiss').on('click.agentOHIDDismiss', function (e) {
                if (!$container.is(e.target) && $container.has(e.target).length === 0) {
                    $dropdown.removeClass('show');
                }
            });

            $list.off('click.agentOHIDCheckbox').on('click.agentOHIDCheckbox', 'input[type=checkbox]', function () {
                const checked = $list.find('input[type=checkbox]:checked').map(function () {
                    return this.value;
                }).get();

                $input1.val(checked.length ? checked.join(', ') : '');
                selectedAgentOHIDs = $input1.val();
            });
            syncDropdownCheckboxesFromInput(selectedAgentOHIDs, 'ohid');
        }

        function syncDropdownCheckboxesFromInput(selectedValues, ddlType) {

            // Clear all checkboxes first
            if (ddlType === 'med')
                $('.medicaidid-dropdown-checkbox-list1 input[type=checkbox]').prop('checked', false);
            else if (ddlType === 'role')
                $('.agent-role-dropdown-checkbox-list input[type=checkbox]').prop('checked', false);
            else if (ddlType === 'ohid')
                $('.agent-ohid-dropdown-checkbox-list input[type=checkbox]').prop('checked', false);

            if (selectedValues) {
                var valuesArray = selectedValues.split(',').map(function (item) {
                    return item.trim();
                });

                valuesArray.forEach(function (value) {
                    if (ddlType === 'med')
                        $('.medicaidid-dropdown-checkbox-list1 input[type=checkbox][value="' + value + '"]').prop('checked', true);
                    else if (ddlType === 'role')
                        $('.agent-role-dropdown-checkbox-list input[type=checkbox][value="' + value + '"]').prop('checked', true);
                    else if (ddlType === 'ohid')
                        $('.agent-ohid-dropdown-checkbox-list input[type=checkbox][value="' + value + '"]').prop('checked', true);

                });
            }
        }

        function PopulateSearchResults() {
            $('#pnlShowError1').text('');

            if (!selectedMedIDs && !selectedAgentOHIDs && !selectedAgentRoles) {
                $('#pnlShowError1').text('Please select at least one search criteria');
                return;
            }

            const APIToken = $("[id*=hdnAccessToken]").val();
            const userId = $('#<%=hdnSelectedProviderAdmin.ClientID%>').val();
            const loggedinUserID = $('#<%=hdnUserName.ClientID%>').val();

            const agentSearchParams = {
                MedIdList: selectedMedIDs,
                AgentIdList: selectedAgentOHIDs,
                AgentRoleList: selectedAgentRoles,
                LoggedInUserID: loggedinUserID,
                SelectedProvAdminUserID: userId
            };

            $.ajax({
                type: "POST",
                url: webApiUserMaintenance + "GetProviderAgentsBySearchCriteria",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(agentSearchParams)
            }).done(function (result) {
                if (result === '300' || result == "300") {
                    $('#pnlNoSearchResults').text('No agents found for the search criteria selected.');
                    $('#gridAgentSearchResults').hide();
                    $('#divExcel').hide();
                } else if (result) {
                    $('#pnlNoSearchResults').text('');
                    $('#gridAgentSearchResults').show();
                    $('#divExcel').show();
                    searchResults = result;
                    page = 1;
                    renderSearchResultGrid();
                }
            }).fail(function (jqXHR) {
                handleAjaxError(jqXHR, 'pnlShowError1', 'Search results error: ', 'pnlNoSearchResults');
            });
        }

        function getSortIndicator(columnKey) {
            if (sortColumn !== columnKey) return '';
            return sortDirection === 'asc' ? '&#9650;' : '&#9660;'; // ▲ or ▼ arrows
        }

        function renderSearchResultGrid() {
            const $thead = $('#table-head').empty();
            columns.forEach(col => {
                // Only add sorting to MedicaidID and AgentOHID columns
                if (col.key === 'MedicaidID' || col.key === 'AgentOHID') {
                    // Create clickable header with sort indicator
                    const $th = $('<th>')
                        .css('cursor', 'pointer')
                        .text(col.title)
                        .append(
                            $('<span>').css({
                                'margin-left': '5px',
                                'font-size': '16px',
                                'user-select': 'none'
                            }).html(getSortIndicator(col.key))
                        )
                        .on('click', () => {
                            if (sortColumn === col.key) {
                                // Toggle sort direction
                                sortDirection = (sortDirection === 'asc') ? 'desc' : 'asc';
                            } else {
                                // Set new sort column and default to ascending
                                sortColumn = col.key;
                                sortDirection = 'asc';
                            }
                            page = 1; // reset to first page on sort
                            renderSearchResultGrid();
                        });
                    $thead.append($th);
                } else {
                    // Non-sortable columns
                    $thead.append($('<th>').text(col.title || ''));
                }
            });

            if (sortColumn) {
                searchResults.sort((a, b) => {
                    let valA = a[sortColumn];
                    let valB = b[sortColumn];

                    // Handle null/undefined
                    valA = (valA === undefined || valA === null) ? '' : valA.toString().toLowerCase();
                    valB = (valB === undefined || valB === null) ? '' : valB.toString().toLowerCase();

                    if (valA < valB) return (sortDirection === 'asc') ? -1 : 1;
                    if (valA > valB) return (sortDirection === 'asc') ? 1 : -1;
                    return 0;
                });
            }


            const $tbody = $('#table-body').empty();

            const start = (page - 1) * pageSize;
            const pagedResults = searchResults.slice(start, start + pageSize);

            pagedResults.forEach(item => {
                const $tr = $('<tr>').attr('id', `tr${item.AgentOHID}`);

                const $btnEdit = $('<button>')
                    .addClass('buttonBoxFocus')
                    .attr({ type: 'button', id: `btnEdit${item.AgentOHID}` })
                    .text('Edit')
                    .on('click', () => OpenEditAgentModal(item.MedicaidID, item.AgentOHID, item.AgentRoles, item.AgentName));

                const $btnRemove = $('<button>')
                    .addClass('buttonBox')
                    .attr({ type: 'button', id: `btnRemove${item.AgentOHID}` })
                    .text('Deactivate')
                    .prop('disabled', item.DisableDeactivate === 1)
                    .on('click', () => DeactivateAgent(item.MedicaidID, item.AgentOHID, item.AgentName));

                $tr.append(
                    $('<td>').append($btnEdit),
                    $('<td>').append($btnRemove),
                    $('<td>').text(item.MedicaidID),
                    $('<td>').text(item.ProviderName),
                    $('<td>').text(item.AgentOHID),
                    $('<td>').text(item.AgentName),
                    $('<td>').text(item.AgentRoles)
                );

                $tbody.append($tr);
            });

            if (searchResults.length > 0) {
                $('#page-info').text(`Showing ${start + 1} to ${Math.min(start + pageSize, searchResults.length)} of ${searchResults.length} Entries`).show();
                $('#pagination').parent().show();
                renderPagination(Math.ceil(searchResults.length / pageSize));
            } else {
                $('#page-info').hide();
                $('#pagination').parent().hide();
            }
        }

        function renderPagination(totalPages) {
            const $pagination = $('#pagination').empty();

            const createPageItem = (label, pageIndex, isActive = false) => {
                const $li = $('<li>').toggleClass('active', isActive);
                const $a = $('<a>').attr('href', '#').css('font-size', '20px').text(label);
                $a.on('click', e => {
                    e.preventDefault();
                    page = pageIndex;
                    renderSearchResultGrid();
                });
                return $li.append($a);
            };

            $pagination.append(createPageItem('<<', 1));

            const pageDisplay = Math.floor((page - 1) / 10);

            if (page > 10) {
                $pagination.append(createPageItem('<', pageDisplay * 10));
            }

            const startPage = pageDisplay * 10 + 1;
            const endPage = Math.min(startPage + 9, totalPages);

            for (let i = startPage; i <= endPage; i++) {
                $pagination.append(createPageItem(i, i, i === page));
            }

            if (endPage < totalPages) {
                $pagination.append(createPageItem('>', endPage + 1));
            }

            $pagination.append(createPageItem('>>', totalPages));
        }

        function OpenEditAgentModal(medID, agentOHID, agentRoles, agentName) {
            const $modal = $("#modalAddAgent");
            $modal.show();

            $('#txtMedicaidID').val(medID).prop('disabled', true);
            $('#txtAgentOHID').val(agentOHID).prop('disabled', true);
            $('#txtAgentEmail').val(agentName).prop('disabled', true);
            $('#divAgentEmail').html("Agent Name:");

            $("#btnSaveAgent").text("Save");

            PopulateAgentRolesonModal(medID, agentOHID, agentName, agentRoles, "Edit");
        }

        function DeactivateAgent(medID, agentOHID, agentName) {
            if (!confirm(`Are you sure to delete agent: ${agentOHID} for Medicaid ID: ${medID}`)) {
                return;
            }

            const APIToken = $("[id*=hdnAccessToken]").val();
            const userId = $('#<%=hdnUserName.ClientID%>').val();

            $.ajax({
                type: "POST",
                url: webApiUserMaintenance + `DeleteProviderAgentByMedicaidID?agentOHID=${agentOHID}&&medID=${medID}`,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function () {
                $('#pnlShowError1').text('Agent Deactivated Successfully');
                initializeAgentDropdowns();
                PopulateSearchResults();
            }).fail(function (jqXHR) {
                handleAjaxError(jqXHR, 'pnlShowError1', 'Error deleting agent: ');
            });
        }
        function ResetPage() {
            // Clear search input text boxes
            $('.ddlMedIDbutton1, .ddlAgentRolebutton1, .ddlAgentOHIDbutton1').val('');
            $('.ddlAgentRoleSearchtext, .ddlAgentOHIDSearchtext, .ddlMedIDSearchtext1').val('');

            // Uncheck all checkboxes in dropdown lists
            $('.medicaidid-dropdown-checkbox-list1 input[type=checkbox]').prop('checked', false);
            $('.agent-role-dropdown-checkbox-list input[type=checkbox]').prop('checked', false);
            $('.agent-ohid-dropdown-checkbox-list input[type=checkbox]').prop('checked', false);

            // Clear selected values variables
            selectedAgentOHIDs = '';
            selectedMedIDs = '';
            selectedAgentRoles = '';

            // Clear search results and reset page
            searchResults = [];
            page = 1;

            // Clear error and no results panels
            $('#pnlShowError1, #pnlNoSearchResults').text('');

            // Clear pagination info and hide pagination controls
            $('#page-info').hide().text('');
            $('#pagination').empty().parent().hide();

            // Clear grid table body and head
            $('#table-body').empty();
            $('#table-head').empty();

            // Hide the grid container
            $('#gridAgentSearchResults').hide();
            $('#divExcel').hide();
            // Optionally close dropdowns if open
            $('.ddlMedIDSelectDropdown1, .ddlAgentRoleSelectDropdown, .ddlAgentOHIDSelectDropdown').removeClass('show');

            initializeAgentDropdowns();
        }

        function OpenAgentModal() {
            const $modal = $("#modalAddAgent");
            $modal.show();

            $("#btnSaveAgent").text("Add Agent");
            $('#divAgentEmail').html("Agent Email:");
            $('#divAddAgentRoleList').empty();
        }

        function FetchAddAgentRoles() {
            if (!ValidateAddAgentInput()) return false;

            const medID = $('#txtMedicaidID').val();
            const agentOHID = $('#txtAgentOHID').val();
            const agentEmail = $('#txtAgentEmail').val();

            PopulateAgentRolesonModal(medID, agentOHID, agentEmail, '', "Add");
        }

        function PopulateAgentRolesonModal(medID, agentOHID, agentEmail, agentRoles, callType) {
            const APIToken = $("[id*=hdnAccessToken]").val();
            const userId = $('#<%=hdnSelectedProviderAdmin.ClientID%>').val();

            const params = {
                MedID: medID,
                AgentID: agentOHID,
                AgentEmail: agentEmail,
                UserID: userId,
                CallType: callType
            };

            $.ajax({
                type: "POST",
                url: webApiUserMaintenance + "GetAgentRolesByProvAdminAndMedID",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(params)
            }).done(function (result) {
                const $container = $('#divAddAgentRoleList').empty();
                const $errorPanel = $('#pnlShowErrorMpe').empty();

                if (!result) return;

                if (result.ResponseCode === '200' && result.AgentRoles) {
                    clearValidationErrors();
                    result.AgentRoles.forEach(role => {
                        const isChecked = callType === 'Edit' && agentRoles.includes(role.AGENT_SUB_ROLES_DESC);
                       
                        const $checkbox = $('<input>', {
                            type: 'checkbox',
                            id: `checkbox-${role.AGENT_SUB_ROLES_ID}`,
                            name: 'options',
                            value: role.AGENT_SUB_ROLES_ID,
                            disabled: !role.IS_ENABLED,
                            checked: isChecked,
                            title: role.AGENT_SUB_ROLES_TOOLTIP
                        });

                        const $label = $('<label>', { for: $checkbox.attr('id'), title: role.AGENT_SUB_ROLES_TOOLTIP }).text(role.AGENT_SUB_ROLES_DESC);

                        $container.append($('<div>').append($checkbox, $label));
                    });
                } else if (['300', '301', '302', '303', '304'].includes(result.ResponseCode)) {
                    $errorPanel.html(`<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">${result.ResponseDesc}</span><br /><br />`);
                } else {
                    $errorPanel.html(`<span tabindex="0" style="color:#CC0505; font-size:14pt; padding-left:10px; font-weight:100;">Error: ${result.ResponseCode} : ${result.ResponseDesc}</span>`);
                }
            }).fail(function (jqXHR) {
                const $errorPanel = $('#pnlShowErrorMpe');
                if (jqXHR && jqXHR.status === 401) {
                    $errorPanel.text('Your token got expired. Kindly logout and login again.');
                } else {
                    $errorPanel.text('Error loading agent roles: ' + JSON.stringify(jqXHR));
                }
                console.log(JSON.stringify(jqXHR));
            });

            function clearValidationErrors() {
                ['spantxtAgentOHID', 'spantxtMedicaidID', 'spantxtAgentEmail'].forEach(id => {
                    $(`#${id}`).empty();
                });
                ['txtMedicaidID', 'txtAgentOHID', 'txtAgentEmail'].forEach(id => {
                    $(`#${id}`).removeClass('errMsgInput');
                });
                $('#pnlShowErrorMpe').empty();
            }
        }

        function SaveAgentRoles(btn) {
            if ($(btn).text() === "Add Agent" && (!ValidateAddAgentInput()
                || ($('#pnlShowErrorMpe').text().trim().length > 0 && $('#pnlShowErrorMpe').text().trim() !== 'Please select at least one role.'))) {
                return false;
            }
            
            const selectedCheckboxes = $('#divAddAgentRoleList input[type="checkbox"]:checked:not(:disabled)');
            const selectedIds = selectedCheckboxes.map(function () { return this.value; }).get();
            const selectedRoles = selectedIds.join(', ');

            if (!selectedRoles) {
                $('#pnlShowErrorMpe').text('Please select at least one role.');
                return false;
            }

            const userId = $('#<%=hdnUserName.ClientID%>').val();
            const APIToken = $("[id*=hdnAccessToken]").val();
            const medID = $('#txtMedicaidID').val();
            const agentOHID = $('#txtAgentOHID').val();

            const params = {
                MedID: medID,
                AgentID: agentOHID,
                UserID: userId,
                AgentRoleList: selectedRoles
            };

            $.ajax({
                type: "POST",
                url: webApiUserMaintenance + "SaveAgentRolesByProviderAdminAndMedID",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(params)
            }).done(function () {
                CloseMpeAddAgent();
                initializeAgentDropdowns();
                if (selectedMedIDs || selectedAgentOHIDs || selectedAgentRoles)
                    PopulateSearchResults();
            }).fail(function (jqXHR) {
                $('#pnlShowErrorMpe').text('Error saving agent roles: ' + JSON.stringify(jqXHR));
                console.log(JSON.stringify(jqXHR));
            });
        }

        function ValidateAddAgentInput() {
            let valid = true;

            const $medIDInput = $('#txtMedicaidID');
            const medIDVal = $medIDInput.val().trim();
            const $medIDError = $('#spantxtMedicaidID');

            if (!medIDVal) {
                $medIDError.text("* Please Enter a Medicaid ID.");
                $medIDInput.addClass('errMsgInput');
                valid = false;
            } else {
                $medIDError.empty();
                $medIDInput.removeClass('errMsgInput');
            }

            const $agentOHIDInput = $('#txtAgentOHID');
            const agentOHIDVal = $agentOHIDInput.val().trim();
            const $agentOHIDError = $('#spantxtAgentOHID');

            if (!agentOHIDVal) {
                $agentOHIDError.text("* Please Enter Agent OHID.");
                $agentOHIDInput.addClass('errMsgInput');
                valid = false;
            } else {
                $agentOHIDError.empty();
                $agentOHIDInput.removeClass('errMsgInput');
            }

            const $agentEmailInput = $('#txtAgentEmail');
            const agentEmailVal = $agentEmailInput.val().trim();
            const $agentEmailError = $('#spantxtAgentEmail');

            if (!agentEmailVal) {
                $agentEmailError.text("* Please Enter Agent Email.");
                $agentEmailInput.addClass('errMsgInput');
                valid = false;
            } else {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(agentEmailVal)) {
                    $agentEmailError.text("* Please enter a valid Agent Email.");
                    $agentEmailInput.addClass('errMsgInput');
                    valid = false;
                } else {
                    $agentEmailError.empty();
                    $agentEmailInput.removeClass('errMsgInput');
                }
            }

            return valid;
        }

        function CloseMpeAddAgent() {
            const $modal = $("#modalAddAgent");
            $modal.hide();

            $('#txtMedicaidID, #txtAgentOHID, #txtAgentEmail').prop('disabled', false).val('');
            $('#txtMedicaidID, #txtAgentOHID, #txtAgentEmail').removeClass('errMsgInput');
            $('#divAgentEmail').html('Agent Email:');
            $('#spantxtMedicaidID, #spantxtAgentEmail, #spantxtAgentOHID').text('');
            $('#pnlShowErrorMpe').empty();
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
        /* Start Reassign Admin */
        function OpenReassignModal() {
            /*alert("Reassign");*/
            const $modal = $("#modalReassignAdmin");
            $modal.show();
        }

        function SaveReassignAdmin() {
            let valid = true;
            const $medIDInput = $('#txtRAMedicaidID');
            const medIDVal = $medIDInput.val().trim();
            const $medIDError = $('#spantxtRAMedicaidID');

            if (!medIDVal) {
                $medIDError.text("* Please Enter a Medicaid ID.");
                $medIDInput.addClass('errMsgInput');
                valid = false;
            } else {
                $medIDError.empty();
                $medIDInput.removeClass('errMsgInput');
            }

            const $adminOHIDInput = $('#txtAdminOHID');
            const adminOHIDVal = $adminOHIDInput.val().trim();
            const $adminOHIDError = $('#spantxtAdminOHID');

            if (!adminOHIDVal) {
                $adminOHIDError.text("* Please Enter Admin OHID.");
                $adminOHIDInput.addClass('errMsgInput');
                valid = false;
            } else {
                $adminOHIDError.empty();
                $adminOHIDInput.removeClass('errMsgInput');
            }            

            if (!valid)
                return false;

            const userId = $('#<%=hdnUserName.ClientID%>').val();
            const APIToken = $("[id*=hdnAccessToken]").val();

            $.ajax({
                type: "POST",
                url: webApiUserMaintenance + "SaveReassignAdministratorByMedicaidID?adminOHID=" + adminOHIDVal + "&&medID=" + medIDVal + "&&userID=" + userId,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            }).done(function (result) {

                const $errorPanel = $('#pnlShowErrorMpe1').empty();

                if (!result) return;

                if (result.ResponseCode === '200') {
                    clearValidationErrors1();

                    $errorPanel.html(`<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">${result.ResponseDesc}</span><br /><br />`);
                } else if (['300', '301', '302', '303'].includes(result.ResponseCode)) {
                    $errorPanel.html(`<br /><span tabindex="0" style="color:#CC0505; font-size:16pt; padding-left:10px; font-weight:100;">${result.ResponseDesc}</span><br /><br />`);
                } else {
                    $errorPanel.html(`<span tabindex="0" style="color:#CC0505; font-size:14pt; padding-left:10px; font-weight:100;">Error: ${result.ResponseCode} : ${result.ResponseDesc}</span>`);
                }
            }).fail(function (jqXHR) {
                const $errorPanel = $('#pnlShowErrorMpe1');
                if (jqXHR && jqXHR.status === 401) {
                    $errorPanel.text('Your token got expired. Kindly logout and login again.');
                } else {
                    $errorPanel.text('Error Reassigning admin: ' + JSON.stringify(jqXHR));
                }
                console.log(JSON.stringify(jqXHR));
            });

            function clearValidationErrors1() {
                ['spantxtAdminOHID', 'spantxtRAMedicaidID'].forEach(id => {
                    $(`#${id}`).empty();
                });
                ['txtAdminOHID', 'txtRAMedicaidID'].forEach(id => {
                    $(`#${id}`).removeClass('errMsgInput');
                });
                $('#pnlShowErrorMpe1').empty();
            }
        }

        function CloseMpeReassignAdmin() {
            const $modal = $("#modalReassignAdmin");
            $modal.hide();

            $('#txtRAMedicaidID, #txtAdminOHID').prop('disabled', false).val('');
            $('#spantxtRAMedicaidID, #spantxtAdminOHID').text('');

            $('#txtRAMedicaidID, #txtAdminOHID').removeClass('errMsgInput');
            $('#pnlShowErrorMpe1').empty();

            initializeAgentDropdowns();
        };
        /* End Reassign Admin */

        function ExportToExcel() {
            const filteredColumns = columns.filter(col => col.key !== 'Edit' && col.key !== 'Deactivate');
            let csvContent = '';
            csvContent += filteredColumns.map(col => `"${col.title}"`).join(',') + '\n';
            searchResults.forEach(row => {
                filteredColumns.forEach((col, index) => {
                    let value = row[col.key];
                    if (value === undefined || value === "undefined") {
                        value = '';
                    }
                    value = String(value).replace(/<br\s*\/?>/gi, '|').replace(/"/g, '""');

                    csvContent += `"${value}"`;
                    if (index < filteredColumns.length - 1) {
                        csvContent += ',';
                    }
                });

                csvContent += '\n';
            });

            const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
            const url = URL.createObjectURL(blob);
            const link = document.createElement("a");
            link.setAttribute("href", url);
            link.setAttribute("download", "AgentResultsExport.csv");
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            return false;
        }
    </script>

    <div class="container-fluid" style="min-height: 300px; font-size: 16pt !important;">
        <asp:HiddenField ID="hdnUserName" runat="server" />
        <asp:HiddenField ID="hdnCostReportMgmtUser" runat="server" />
        <asp:HiddenField ID="hdnSelectedProviderAdmin" runat="server" />
        <br />

        <div id="divpnlLoader" style="display: none">
            <img src='../Images/loader.gif' style="height: 38px; width: 35px;" />
        </div>
        <div id="pnlShowError1" style="overflow: auto; width: 100%; color: red; font-size: 16pt !important;"></div>
        <br />
        <br />
        <div class="row">
            <div class="col-sm-2 text-right">
                <label>Medicaid ID:</label>
            </div>
            <div class="col-sm-4 text-left">
                <div class="input-group input-group-lg ddlMedIDContainer1">
                    <input type="text" placeholder="Search Medicaid ID..." class="form-control ddlMedIDbutton1" />
                    <div class="input-group-btn">
                        <button type="button" class="btn ddlMedIDbutton2"><span class="caret"></span></button>
                        <div class="select-dropdown ddlMedIDSelectDropdown1">
                            <input type="text" class="form-control form-group-lg search-input ddlMedIDSearchtext1" placeholder="Search..." />
                            <div class="options-list medicaidid-dropdown-checkbox-list1">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-2 text-right">
                <label>Agent Sub Role:</label>
            </div>
            <div class="col-sm-4 text-left">
                <div class="input-group input-group-lg ddlAgentRoleContainer">
                    <input type="text" placeholder="Search Agent Sub Role..." class="form-control ddlAgentRolebutton1" />
                    <div class="input-group-btn">
                        <button type="button" class="btn ddlAgentRolebutton2"><span class="caret"></span></button>
                        <div class="select-dropdown ddlAgentRoleSelectDropdown">
                            <input type="text" class="form-control form-group-lg search-input ddlAgentRoleSearchtext" placeholder="Search..." />
                            <div class="options-list agent-role-dropdown-checkbox-list">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-sm-2 text-right">
                <label>Agent ID:</label>
            </div>
            <div class="col-sm-4 text-left">
                <div class="input-group input-group-lg ddlAgentOHIDContainer">
                    <input type="text" placeholder="Search Agent ID..." class="form-control ddlAgentOHIDbutton1" />
                    <div class="input-group-btn">
                        <button type="button" class="btn ddlAgentOHIDbutton2"><span class="caret"></span></button>
                        <div class="select-dropdown ddlAgentOHIDSelectDropdown">
                            <input type="text" class="form-control form-group-lg search-input ddlAgentOHIDSearchtext" placeholder="Search..." />
                            <div class="options-list agent-ohid-dropdown-checkbox-list">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
             <div class="col-lg-12 text-right btnBox" style="padding: 28px 0 10px 0">
                 <button type="button" class="buttonBoxFocus" id="btnsearch" onclick="PopulateSearchResults()">Search</button>        
                 <button type="button" class="buttonBox" id="btnAddNew" onclick="OpenAgentModal()">Add New Agent</button>
                 <button type="button" class="buttonBoxFocus" id="btnReassignAdmin" onclick="OpenReassignModal()">Reassign Admin</button>
                 <button type="button" class="buttonBoxFocusRed3B" id="btnClear" onclick="ResetPage()">Clear</button>
            </div>
        </div>
        <br />
        <br />

        <!-- Table -->
        <div class="col-lg-12 text-right btnBox" style="padding: 28px 0 10px 0" id="divExcel">
            <button type="button" id="btnExcel" onclick="ExportToExcel()"><img src="../Images/Excel_24x24.png" alt="Export to Excel" /></button>
        </div>
        <div class="divAgentSearchResults">
            <div id="pnlNoSearchResults" style="overflow: auto; width: 100%; color: red; font-size: 16pt !important;"></div>
            <div id="gridAgentSearchResults">                
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr id="table-head"></tr>
                    </thead>
                    <tbody id="table-body"></tbody>
                </table>
            </div>
        </div>
        <!-- Pagination -->
        <div class="row" style="margin-top: 10px;" id="divPagination">
            <div class="col-sm-6">
                <div id="page-info" class="text-muted" style="padding-top: 6px;"></div>
            </div>
            <div class="col-sm-6 text-right">
                <ul class="pagination pagination-sm" id="pagination" style="margin: 0; display: inline-block;"></ul>
            </div>
        </div>

        <!-- Modal pop up content for Add agent-->
        <div id="modalAddAgent" class="modal1">
            <div class="modal-content">
                <header style="cursor: move; padding: 5px; background-color: #205794; text-align: left; color: white; height: 40px;">
                    Agent Information
                <span id="spn1closeModal" class="close" style="color: white;" onclick="CloseMpeAddAgent();">&times;</span>
                </header>
                <br />
                <br />
                <div id="pnlShowErrorMpe" style="overflow: auto; width: 100%; color: red; font-size: 16pt !important;"></div>
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <label>Agent OH|ID:</label>
                    </div>
                    <div class="col-sm-4 text-left">
                        <input type="text" class="form-control" id="txtAgentOHID" placeholder="Enter Agent OH|ID" onblur="FetchAddAgentRoles()" />
                        <small class="errMsg" id="spantxtAgentOHID"></small>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <label id="divAgentEmail">Agent Email:</label>
                    </div>
                    <div class="col-sm-4 text-left">
                        <input type="text" class="form-control" id="txtAgentEmail" placeholder="Enter Agent OH|ID Email" onblur="FetchAddAgentRoles()" />
                        <small class="errMsg" id="spantxtAgentEmail"></small>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <label>Medicaid ID:</label>
                    </div>
                    <div class="col-sm-4 text-left">
                        <input type="text" class="form-control" id="txtMedicaidID" placeholder="Enter Medicaid ID" onblur="FetchAddAgentRoles()" />
                        <small class="errMsg" id="spantxtMedicaidID"></small>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-sm-2 text-left">
                    </div>
                    <div class="col-sm-10">
                        <div id="divAddAgentRoleList" style="padding: 10px;">
                            <!-- Populated dynamically -->
                        </div>
                    </div>
                </div>
                <br />
                <br />
                <div class="row">
                     <div class="col-lg-12 text-center btnBox btnBoxCenter" style="padding: 28px 0 10px 0">
                        <button type="button" class="buttonBoxFocus" id="btnSaveAgent" onclick="SaveAgentRoles(this)">Add Agent</button>
                        <button type="button" class="buttonBoxFocusRed3B" id="btnClose2" onclick ="CloseMpeAddAgent()">Close</button>
                    </div>
                </div>
            </div>
        </div>
          <!-- Modal pop up content for Reassign Admin-->
         <div id="modalReassignAdmin" class="modal1"> 
           <div class="modal-content">
                <header style="cursor: move; padding: 5px; background-color:#205794; text-align: left; color:white; height:40px;">
                    Reassign Administrator
                    <span id="spn2closeModal" class="close" style="color:white;" onclick ="CloseMpeReassignAdmin()">&times;</span>
                </header>
               <br /> <br />
               <div id="pnlShowErrorMpe1" style="overflow: auto; width: 100%; color:red; font-size:16pt !important;"></div>      
                 <div class="row">
                    <div class="col-sm-4 text-right">
                        <label>Medicaid ID:</label>
                    </div>
                    <div class="col-sm-4 text-left">                            
                       <input type="text" class="form-control" id="txtRAMedicaidID" placeholder="Enter Medicaid ID" />
                        <small class="errMsg" id="spantxtRAMedicaidID"></small>
                    </div>
                </div>     
               <br />
                <div class="row">
                     <div class="col-sm-4 text-right">
                        <label>New Administrator OH|ID:</label>
                     </div>
                     <div class="col-sm-4 text-left">                            
                        <input type="text" class="form-control" id="txtAdminOHID" placeholder="Enter Administrator OH|ID" />
                         <small class="errMsg" id="spantxtAdminOHID"></small>
                     </div>
                  </div>         
               <br /> <br />
                <div class="row">
                   <div class="col-lg-12 text-center btnBox btnBoxCenter" style="padding: 28px 0 10px 0">
                        <button id="btnSaveReassign" type="button" onclick="SaveReassignAdmin()" class="buttonBoxFocus">Save</button>
                        <button id="btnCloseReassign" type="button" onclick="CloseMpeReassignAdmin()" class="buttonBoxFocusRed3B">Close</button>
                    </div>
                 </div>
           </div>
        </div>
    </div>

</asp:Content>