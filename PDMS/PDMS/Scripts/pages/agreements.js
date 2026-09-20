$(function () {
    loadSubmittedAgreements();
});
function loadSubmittedAgreements() {
    CredentialingService.getSubmittedAgreementsByRegID(function (response) {
        renderSubmittedAgreementsGrid(response);
    });
}

function renderSubmittedAgreementsGrid(data) {

    $('#submitted-agreements-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'AgreementSubmittedDate', title: 'Agreement Submitted Date', type: 'date', sortable: true },
                { key: 'AgreementEffectiveDate', title: 'Agreement Effective Date', type: 'date', sortable: true },
                { key: 'SubmittedBy', title: 'Submitted By', type: 'text', sortable: true },
                { key: 'FileName', title: 'File Name', type: 'text', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Submitted Agreements found.',
            idProperty: 'RegId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}