$(function () {
    loadOrgInfo();
   
});

function loadOrgInfo() {
    
    CredentialingService.getTaxonomyByRegID(function (response) {
        renderTaxonomyGrid(response);
    });

    CredentialingService.getSpecialtyByRegID(function (response) {
        renderSpecialtyGrid(response);
    });
}

function renderTaxonomyGrid(data) {

    $('#satellite-taxonomy-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'TaxonomyCode', title: 'Taxonomy Code', type: 'text', sortable: true },
                { key: 'TaxonomyName', title: 'Taxonomy Name', type: 'text', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'EndDate', title: 'End Date', type: 'date', sortable: true },
                { key: 'ExpirationDate', title: 'Expiration Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Taxonomies found.',
            idProperty: 'REG_TAXONOMY_ID',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}
function renderSpecialtyGrid(data) {

    $('#satellite-specialty-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'SpecialtyName', title: 'Specialty Name', type: 'text', sortable: true },
                { key: 'IsPrimary', title: 'Is Primary', type: 'text', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'EndDate', title: 'End Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Specialties found.',
            idProperty: 'REG_SPECIALTY_ID',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}