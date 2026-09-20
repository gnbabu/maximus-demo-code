$(function () {
    loadGroupAffiliations();
    loadHospitalAffiliations();
});

function loadGroupAffiliations() {

    $("#groupAffiliationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.SelectGroupAffiliationByRegID(function (response) {
        renderGroupAffiliationsGrid(response);
    });
}

function renderGroupAffiliationsGrid(data) {

    $('#group-affiliation-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'GroupName', title: 'Group Name', type: 'text', sortable: true },
                { key: 'Npi', title: 'NPI', type: 'text', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'EndDate', title: 'End Date', type: 'date', sortable: true },
                { key: 'Status', title: 'Status', type: 'text', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Group Affiliations found.',
            idProperty: 'correspondenceId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}

function loadHospitalAffiliations() {

    $("#hospitalAffiliationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.selectHealthCareFacilityAffiliationByRegID(function (response) {
        renderHospitalAffiliationsGrid(response);
    });
}

function renderHospitalAffiliationsGrid(data) {

    $('#hospital-affiliation-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'FacilityName', title: 'Facility Name', type: 'text', sortable: true },
                { key: 'FacilityMedicaidID', title: 'Hospital ID', type: 'text', sortable: true },
                { key: 'StaffCategory', title: 'Staff Category', type: 'text', sortable: true },
                { key: 'StatusofPrivileges', title: 'Status of Privileges', type: 'text', sortable: true },
                { key: 'Is_Primary_Facility', title: 'Primary Facility', type: 'boolean', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'EndDate', title: 'End Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Hospital Affiliations found.',
            idProperty: 'correspondenceId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}
