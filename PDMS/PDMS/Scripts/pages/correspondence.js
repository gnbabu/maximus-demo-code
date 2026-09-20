// Global map to store rows by ID
var correspondenceRowMap = {};


$(function () {
    $('#ctl00_MainContent_txtRegId')
        .on('input', validateRequest);   // real-time validation

    $('#ctl00_MainContent_txtRegId')
        .on('blur', function () {
            const val = $(this).val().trim();

            // ✅ only validate on blur if user typed something
            if (val !== '') {
                validateRequest();
            } else {
                // ✅ hide error if empty
                $('#regIdError')
                    .addClass('d-none')
                    .text('Reg ID is required.');
            }
        });

    const fields = [
        '#ctl00_MainContent_txtMedicaidID',
        '#ctl00_MainContent_txtRegId',
        '#ctl00_MainContent_txtProviderId',
        '#ctl00_MainContent_txtNPI',
        '#ctl00_MainContent_txtDateAvailableFrom',
        '#ctl00_MainContent_txtDateAvailableTo'
    ];

    $(fields.join(',')).on('input change', function () {
        toggleSearchButton();
    });

    toggleSearchButton(); // initial state
});


$(document).on('click', '.open-popup', function () {

    const id = $(this).data('id');
    const row = correspondenceRowMap[id];

    if (!row) {
        console.error("Correspondence row not found for ID:", id);
        return;
    }

    console.log("FULL ROW:", row);

    const request = {
        DocumentId: String(row.CommunicationEventId)
    };

    CorrespondenceService.updateViewed(
        request,
        function onSuccess() {
            searchCorrespondence();
            hideClientLoader();
        },
        function onError(err) {
            console.error("UpdateViewed failed", err);
        }
    );

    openCorrespondencePreview(row);
});


function formatDateInput(el) {

    // ✅ remove all non-digits
    let val = el.value.replace(/\D/g, '');

    // ✅ format MM/DD/YYYY
    if (val.length >= 3 && val.length <= 4)
        val = val.slice(0, 2) + '/' + val.slice(2);

    else if (val.length > 4)
        val = val.slice(0, 2) + '/' + val.slice(2, 4) + '/' + val.slice(4, 8);

    el.value = val;
}

function toggleSearchButton() {

    const getVal = (selector) => $.trim($(selector).val());

    const medicaidId = getVal('#ctl00_MainContent_txtMedicaidID');
    const regId = getVal('#ctl00_MainContent_txtRegId');
    const providerId = getVal('#ctl00_MainContent_txtProviderId');
    const npi = getVal('#ctl00_MainContent_txtNPI');
    const dateFrom = getVal('#ctl00_MainContent_txtDateAvailableFrom');
    const dateTo = getVal('#ctl00_MainContent_txtDateAvailableTo');

    const hasCoreField =
        medicaidId !== '' ||
        regId !== '' ||
        providerId !== '' ||
        npi !== '';

    const hasOnlyDates =
        (dateFrom !== '' || dateTo !== '') &&
        !hasCoreField;

    // ✅ Enable only if core field exists (ignore date-only case)
    const shouldEnable = hasCoreField && !hasOnlyDates;

    $('#btnSearch').prop('disabled', !shouldEnable);
}

function searchCorrespondence() {

    if ($('#btnSearch').prop('disabled')) {
        return false;
    }

    if (!validateRequest()) {
        return false; // ⛔ stop search
    }


    const request = {
        medicaidId: $("#ctl00_MainContent_txtMedicaidID").val()?.trim() || '',
        regId: $("#ctl00_MainContent_txtRegId").val()?.trim() || '',
        providerId: $("#ctl00_MainContent_txtProviderId").val()?.trim() || '',
        npi: $("#ctl00_MainContent_txtNPI").val()?.trim() || '',
        FromDate: parseDate($("#ctl00_MainContent_txtDateAvailableFrom").val()),
        ToDate: parseDate($("#ctl00_MainContent_txtDateAvailableTo").val()),
        userId: $("#ctl00_MainContent_hdnUserId").val() || '',
        correspondenceType: '1',
        // Paging / Sorting defaults (can be made dynamic later)
        sortField: "DATE_SENT",
        sortDirection: "DESC",
        pageSize: 100,
        pageIndex: 0
    };

    loadCorrespondence(request);
}

function resetCorrespondenceSearch() {

    /* ---------- Search input fields ---------- */
    $("#ctl00_MainContent_txtMedicaidID").val('');
    $("#ctl00_MainContent_txtRegId").val('');
    $("#ctl00_MainContent_txtProviderId").val('');
    $("#ctl00_MainContent_txtNPI").val('');
    $("#ctl00_MainContent_txtDateAvailableFrom").val('');
    $("#ctl00_MainContent_txtDateAvailableTo").val('');


    /* ---------- Clear & hide grid ---------- */
    const $grid = $("#correspondence-grid-container");
    $grid.addClass("d-none").empty();

    /* ---------- Clear in‑memory row map ---------- */
    if (typeof correspondenceRowMap !== "undefined") {
        correspondenceRowMap = {};
    }

    // ✅ Hide your custom RegId error
    $('#regIdError').addClass('d-none');


    $('#correspondence-results').addClass('d-none');


    // ✅ Reset ASP.NET validators (clears date errors)
    if (typeof (Page_Validators) !== "undefined") {
        for (var i = 0; i < Page_Validators.length; i++) {
            Page_Validators[i].isvalid = true;
            ValidatorUpdateDisplay(Page_Validators[i]);
        }
    }

    if (typeof (Page_IsValid) !== "undefined") {
        Page_IsValid = true;
    }


    toggleSearchButton();
}

function validateRequest() {

    const getVal = (selector) => $.trim($(selector).val());

    const medicaidId = getVal('#ctl00_MainContent_txtMedicaidID');
    const regId = getVal('#ctl00_MainContent_txtRegId');
    const providerId = getVal('#ctl00_MainContent_txtProviderId');
    const npi = getVal('#ctl00_MainContent_txtNPI');

    let isValid = true;

    // ✅ At least one core field must be entered
    if (!medicaidId && !regId && !providerId && !npi) {
        $('#regIdError')
            .text('Enter at least one search field (Reg ID, Medicaid ID, Provider ID, or NPI).')
            .removeClass('d-none');
        return false;
    }

    // ✅ Validate Reg ID only if user entered it
    if (regId && isNaN(regId)) {
        $('#regIdError')
            .text('Reg ID must be numeric.')
            .removeClass('d-none');
        return false;
    }

    // ✅ (Optional but recommended) Validate NPI = 10-digit numeric
    if (npi && (!/^\d{10}$/.test(npi))) {
        $('#regIdError')
            .text('NPI must be a 10-digit number.')
            .removeClass('d-none');
        return false;
    }

    // ✅ Clear error if everything is valid
    $('#regIdError').addClass('d-none');

    return isValid;
}


function parseDate(value) {
    if (!value) return null;

    const m = moment(value, "MM/DD/YYYY", true);
    return m.isValid() ? m.toISOString() : null;
}

function loadCorrespondence(request) {

    $('#correspondence-results').removeClass('d-none');

    showClientLoader();
    CorrespondenceService.searchCorrespondence(request,
        function onSuccess(response) {
            const items = Array.isArray(response) ? response : [];
            renderCorrespondenceGrid(items);
            hideClientLoader();
        },
        function onError(err) {
            console.error(err);
            hideClientLoader();
        });
}

function renderCorrespondenceGrid(data) {

    correspondenceRowMap = {}; // reset

    data.forEach(function (row) {
        correspondenceRowMap[row.CommunicationEventId] = row;
    });


    $('#correspondence-grid-container')
        .removeClass("d-none")
        .dataGrid({
            data: data,
            columns: [
                {
                    key: 'Subject',
                    title: 'Correspondence Subject',
                    type: 'text',
                    sortable: true,

                    cellTemplate: function (row) {
                        return `
                            <a href="javascript:void(0);"
                               class="subject-link open-popup"
                               data-id="${row.CommunicationEventId}">
                               ${row.Subject}
                            </a>`;
                    }
                },
                { key: 'CorrespondenceType', title: 'Correspondence Type', type: 'text', sortable: true },
                { key: 'DateSent', title: 'Date Sent', type: 'date', sortable: true },
                { key: 'DateViewed', title: 'Date Viewed', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No correspondence found.',
            idProperty: 'CommunicationEventId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function openCorrespondencePreview(row) {
    // Subject
    setPreviewSubject(row.Subject);

    // Body
    setupBodyClient(row.Body);

    // Send To
    setupSendToClient(row.EmailTo);

    // Attachments (optional)
    setupAttachmentsClient(row);

    // Update viewed (optional API call)
    //updateViewedIfRequired(row);

    // Show modal
    $find('ctl00_MainContent_mpeEmailPreview').show();
}

function setupBodyClient(bodyText) {

    const $txtBody = $("#ctl00_MainContent_txtBody");
    const $divBody = $("#ctl00_MainContent_divBody");

    if (!bodyText) {
        $txtBody.hide().val('');
        $divBody.hide().html('');
        return;
    }

    // Detect HTML content
    const htmlRegex = /<\s*([^ >]+)[^>]*>.*?<\s*\/\s*\1\s*>/i;

    if (htmlRegex.test(bodyText)) {
        $txtBody.hide();
        $divBody.show().html(bodyText);
    } else {
        $divBody.hide();
        $txtBody.show().val(bodyText);
    }
}

function setupSendToClient(emailTo) {

    const emailList = [];

    const addIfValid = (email) => {
        if (email && emailList.indexOf(email) === -1) {
            emailList.push(email);
        }
    };

    addIfValid(emailTo);
    addIfValid(UserEmailAddress);
    addIfValid(ContactRegEmailAddress);
    addIfValid(CredentialingEmailAddress);

    const $listBox = $("#ctl00_MainContent_lboxSendTo");
    $listBox.empty();

    emailList.forEach(email => {
        $('<option>', { value: email, text: email }).appendTo($listBox);
    });
}

function setPreviewSubject(subject) {
    $("#ctl00_MainContent_txtSubject").val(
        (subject || '').replace("(State)", "MCS")
    );
}

function updateViewedIfRequired(row) {
    if (row.DocumentId &&
        row.Eligibility === "TRUE" &&
        !row.DateViewed) {

        ApiService.post("Correspondence/UpdateViewed", {
            documentId: row.DocumentId
        });
    }
}

function setupAttachmentsClient(row) {
    // Implement if attachments are in row
    // Uses DocumentId / FileName / DocumentType
}

function showClientLoader() {
    $("#client-loader").removeClass("d-none");
}
function hideClientLoader() {
    $("#client-loader").addClass("d-none");
}