$(function () {
    loadEducation();
    loadWorkHistory();
});

function loadEducation() {

    $("#educationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    //$("#militaryServiceAccordian").maximusAccordion({
    //    allowMultiple: false,
    //    defaultOpen: 0
    //});


    CredentialingService.getEducationByRegID(function (response) {
        renderEducationGrid(response);
    });
}

function renderEducationGrid(data) {

    $('#education-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'School', title: 'Name of School', type: 'text', sortable: true },
                { key: 'EDUCATION_TYPE_DESC', title: 'Education Type', type: 'text', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'EndDate', title: 'End Date', type: 'date', sortable: true },
                { key: 'DegreeOrCertificate', title: 'Degree/Certificate Awarded', type: 'text', sortable: true },
                { key: 'Address1', title: 'Address 1', type: 'text', sortable: true },
                { key: 'Address2', title: 'Address 2', type: 'text', sortable: true },
                { key: 'City', title: 'City', type: 'text', sortable: true },
                { key: 'State', title: 'State', type: 'text', sortable: true },
                { key: 'Zip', title: 'Zip Code', type: 'text', sortable: true },
                { key: 'Country', title: 'Country', type: 'text', sortable: true },
                { key: 'PhoneNumber', title: 'Phone Number', type: 'text', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No education records found',
            idProperty: 'School', // assuming school name is unique enough per entry

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}

function loadWorkHistory() {

    $("#workHistoryAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    CredentialingService.getWorkHistoryByRegID(function (response) {
        renderWorkHistoryGrid(response);
    });
}

function parseDate(value) {
    if (!value) return null;

    // .NET format: /Date(123456789)/
    if (typeof value === "string" && value.includes("/Date(")) {
        return new Date(parseInt(value.replace(/[^0-9]/g, ''), 10));
    }

    // ISO / normal
    let d = new Date(value);
    return isNaN(d) ? null : d;
}

function formatDate(date) {
    if (!date) return '--';

    const d = new Date(date);
    if (isNaN(d)) return '--';

    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    const yyyy = d.getFullYear();

    return `${mm}-${dd}-${yyyy}`;
}
function renderWorkHistoryGrid(data) {

    // ✅ normalize data FIRST
    data.forEach(row => {
        row.StartDateObj = parseDate(row.StartDate);
        row.EndDateObj = parseDate(row.EndDate);
        row.StartDateFormatted = formatDate(row.StartDateObj);
        row.EndDateFormatted = formatDate(row.EndDateObj);
    });

    $('#work-history-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'EmployerName', title: 'Employer Name', type: 'text', sortable: true },
                { key: 'FullAddress', title: 'Address', type: 'text', sortable: true },
                { key: 'ContactPhoneNumber', title: 'Phone Number', type: 'text', sortable: true },
                { key: 'StartDateObj', title: 'Start Date', type: 'date', sortable: true, width: '185px' },
                { key: 'EndDateObj', title: 'End Date', type: 'date', sortable: true, width: '185px' },
                { key: 'MilitaryReserve', title: 'Is Military Service', type: 'text', sortable: true }
            ],

            rowTemplate: function (row) {
                return `
                <tr class="work-main-row">

                    <td rowspan="2">${row.EmployerName}</td>
                    <td>${row.FullAddress || '--'}</td>
                    <td>${row.ContactPhoneNumber}</td>

                    <!-- ✅ FIXED -->
                    <td>${row.StartDateFormatted}</td>
                    <td>${row.EndDateFormatted}</td>                   
                    <td>${row.MilitaryReserve === true || row.MilitaryReserve === 'True' ? 'Yes' : 'No'}</td>

                </tr>

                 <tr class="reason-row">
                    <td>${row.ReasonTextTitle}</td>
                    <td colspan="3">${row.ReasonText}</td>
                    <td>${row.ReasonTextTitle === "Reason for Gap" ? '' : ''}</td>
                </tr>
                `;
            },

            tableClass: 'maximus-base-table',
            noDataMessage: 'No work history found.',
            enableColumnFilters: true,
            enableSorting: true,
            gridTitle: '',
            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}