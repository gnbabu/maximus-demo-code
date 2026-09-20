$(function () {
    loadSatellitePracticeLocations();
   
});

function loadSatellitePracticeLocations() {
    $("#primaryLocationAccordian").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    $("#billingAndPaymentAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    $("#correspondenceAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    $("#satellitePracticeAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.getSatellitePracticeLocationsByRegID(function (response) {
        renderSatellitePracticeLocationsGrid(response);
    });
}

function renderSatellitePracticeLocationsGrid(data) {

    $('#satellite-practice-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'AdditionalPracticeName', title: 'Additional Practice Name', type: 'text', sortable: true },
                { key: 'AdditionalPracticeAddress', title: 'Additional Practice Address', type: 'text', sortable: true },
                { key: 'AdditionalPracticePhoneNumber', title: 'Additional Practice Phone Number', type: 'text', sortable: true },
                { key: 'EffectiveDate', title: 'Effective Date', type: 'date', sortable: true },
                { key: 'EndDate', title: 'End Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Satellite Practice Locations found.',
            idProperty: 'RegAddressId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}