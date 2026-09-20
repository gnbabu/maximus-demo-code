$(function () {
    loadInsurance();
    loadMalpracticeClaims();
});

function loadInsurance() {

    $("#insuranceAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    CredentialingService.getInsuranceByRegID(function (response) {
        renderInsuranceGrid(response);
    });
}

function renderInsuranceGrid(data) {

    $('#insurance-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'CarrierName', title: 'Carrier Name', type: 'text', sortable: true },
                { key: 'PolicyNumber', title: 'Policy Number', type: 'text', sortable: true },
                { key: 'PolicyHolder', title: 'Policy Holder', type: 'text', sortable: true },
                { key: 'EffectiveDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'ExpirationDate', title: 'End Date', type: 'date', sortable: true },
                { key: 'TypeOfCoverageName', title: 'Type of Coverage', type: 'text', sortable: true },
                { key: 'CoverageAmountPerOccurance', title: 'Coverage Amount', type: 'text', sortable: true },
                { key: 'CoverageAmountPerAggregate', title: 'Coverage Amount Per Aggregate', type: 'text', sortable: true }

            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No insurance records found.',
            idProperty: 'PolicyNumber', // typically unique

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}

function loadMalpracticeClaims() {

    $("#malpracticeClaimsAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    CredentialingService.getMalpracticeClaimsByRegID(function (response) {
        renderMalpracticeClaimsGrid(response);
    });
}
function renderMalpracticeClaimsGrid(data) {

    $('#malpractice-claims-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'DateOfOccurrence', title: 'Date of Occurrence', type: 'date', sortable: true },
                { key: 'DateClaimFiled', title: 'Date Claim Filed', type: 'date', sortable: true },
                { key: 'CarrierInvolved', title: 'Carrier Involved', type: 'text', sortable: true },
                { key: 'PolicyNumber', title: 'Policy Number', type: 'text', sortable: true },
                { key: 'StatusOfClaim', title: 'Status of Claim', type: 'text', sortable: true },
                { key: 'ClaimSettledDate', title: 'Claim Settled Date', type: 'date', sortable: true },
                { key: 'MethodOfResolution', title: 'Method of Resolution', type: 'text', sortable: true },
                { key: 'SettlementAmount', title: 'Settlement Amount', type: 'text', sortable: true },
                { key: 'RoleInCase', title: 'Your Role in the Case', type: 'text', sortable: true },
                { key: 'AllegedInjury', title: 'Describe the Alleged Injury to the Patient', type: 'text', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Malpractice Claims found.',
            idProperty: 'PolicyNumber', // adjust if your API returns a unique ID

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}