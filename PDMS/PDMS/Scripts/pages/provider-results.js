let currentCredentialActivity = null;
$(document).ready(function () {

    const activityTypeId = getQueryString("ActivityTypeId");
    const activityId = getQueryString("ActivityId");
    const activityTypeScreeningName = getQueryString("ScreeningName");

    if (!activityTypeId || !activityId) {
        console.warn("Missing ActivityTypeId or ActivityId");
        return;
    }

    // Load header text
    CredentialingActivityService.getCredentialActivityById(
        activityId,
        $("[id*=hdnRegId]").val(),
        function (data) {

            currentCredentialActivity = data;

            $("#lblHeader2").text(data.Activity.ScreeningActivityTypeName);            
            populateCredentialSpecificData(activityTypeScreeningName);
        }
    );


});


const CredentialActivityType = {
    OIG_LEIE_VERIFICATION: "OIG LEIE Verification",
    SAM_EPLS_VERIFICATION: "SAM/EPLS Verification",
    LICENSE_VERIFICATION: "License Verification",
    MEDICARE_OPT_OUT: "Medicare Opt Out",
    SITE_VISIT_ACCREDITATION: "Site Visit / Accreditation",
    BOARD_VERIFICATION: "Board Verification",
    BED_REGISTRATION: "Bed Registration",
    MEDICAID_CERTIFICATION: "Medicaid Certification",
    MEDICARE_CERTIFICATION: "Medicare Certification",
    NPDB_VERIFICATION: "NPDB Verification",
    DEA_VERIFICATION: "DEA Verification",
    CONTROLLED_SUBSTANCE_VERIFICATION: "Controlled Substance Verification",
    PROVIDER_ATTESTATION: "Provider Attestation",
    MALPRACTICE_INSURANCE: "Malpractice Insurance",
    FIVE_YEAR_WORK_HISTORY: "5-Year Work History",
    EDUCATION_VERIFICATION: "Education Verification",
    HOSPITAL_PRIVILEGE: "Hospital Privilege",
    FACILITY_LICENSE: "Facility License",
    MATERNITY_LICENSE_VERIFICATION: "Maternity License Verification",
    MEDICARE_EXCLUSION_VERIFICATION: "Medicare Exclusion Verification",
    MEDICAID_EXCLUSION_VERIFICATION: "Medicaid Exclusion Verification",
    NPPES_VERIFICATION: "NPPES Verification"
};


function getQueryString(param) {
    const params = new URLSearchParams(window.location.search);
    return params.get(param);
}

function populateCredentialSpecificData(activityTypeScreeningName) {
    
    switch (activityTypeScreeningName) {

        case CredentialActivityType.NPPESVerification:
            loadNPPESResults();
            break;

        case CredentialActivityType.MALPRACTICE_INSURANCE:
            loadMalpracticeInsurance();
            break;

        case CredentialActivityType.FIVE_YEAR_WORK_HISTORY:
            loadWorkHistoryResults();//Need to check API
            break;

        case CredentialActivityType.DEA_VERIFICATION:
            loadProviderDEA();
            break;

        case CredentialActivityType.LICENSE_VERIFICATION:
            loadProviderLicenses();
            break;

        case CredentialActivityType.BOARD_VERIFICATION:
            loadProviderSpecialties();
            break;

        case CredentialActivityType.EDUCATION_VERIFICATION:
            loadProviderEducation();
            break;

        case CredentialActivityType.CONTROLLED_SUBSTANCE_VERIFICATION:
            loadProviderCDSNumbers();
            break;

        case CredentialActivityType.NPDB_VERIFICATION:
            loadProviderNPDBResults();
            break;

        default:
            $(".credential-view").hide();
            break;
    }

}
function hideView(viewId) {
    $("#" + viewId).addClass("d-none");
}

function showView(viewId) {
    $("#" + viewId).removeClass("d-none");
}
function loadNPPESResults() {

    if (!currentCredentialActivity || !currentCredentialActivity.Provider) {
        hideView("vwNPPESResults");
        return;
    }

    populateNPPESMatchResults(currentCredentialActivity.Provider);
}
function setLabelText(labelIdSuffix, value) {
    $(`[id$='${labelIdSuffix}']`).text(value || '-');
}
function populateNPPESMatchResults(provider) {

    if (!provider) {
        hideView("vwNPPESResults");
        return;
    }

    showView("vwNPPESResults");

    setLabelText("lblNPPESNPI", provider.Npi);
    setLabelText("lblNPPESEntityType", provider.EntityType);
    setLabelText("lblNPPESOrgName", provider.OrganizationName);
    setLabelText("lblNPPESFirstName", provider.FirstName);
    setLabelText("lblNPPESMiddleName", provider.MiddleName);
    setLabelText("lblNPPESLastName", provider.LastName);
    setLabelText("lblNPPESAddressState", provider.MailingAddressStateName);
}


function loadMalpracticeInsurance() {
    showView("vwMalpracticeResults");
    CredentialingActivityService.getMalpracticeInsurance(function (response) {
        renderMalpracticeInsuranceGrid(response);
    });
}

function renderMalpracticeInsuranceGrid(data) {

    $('#provider-insurance-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                {
                    key: 'CarrierName',
                    title: 'Carrier Name',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'PolicyHolder',
                    title: 'Policy Holder',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'PolicyNumber',
                    title: 'Policy Number',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'EffectiveDate',
                    title: 'Original Effective / Issue Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'ExpirationDate',
                    title: 'Expiration Date',
                    type: 'date',
                    sortable: true
                },

                {
                    key: 'CoverageAmountPerOccurance',
                    title: 'Coverage Amount per Occurrence',
                    sortable: true,
                    cellTemplate: function (row) {
                        return `
                            <span>
                                $${row.CoverageAmountPerOccurance ?? ''}
                            </span>`;
                    }
                },
                {
                    key: 'CoverageAmountPerAggregate',
                    title: 'Coverage Amount per Aggregate',
                    sortable: true,
                    cellTemplate: function (row) {
                        return `
                            <span>
                                 $${row.CoverageAmountPerAggregate ?? ''}
                            </span>`;
                    }
                }

            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No malpractice insurance records found.',
            idProperty: 'PolicyNumber',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}


function loadWorkHistoryResults() {
    showView("vwWorkHistoryResults");

    CredentialingActivityService.getWorkHistoryResults(function (response) {
        renderWorkHistoryResultsGrid(response);
    });
}

function renderWorkHistoryResultsGrid(data) {

    $('#provider-workhistory-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                {
                    key: 'ReasonTextTitle',
                    title: 'Title',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'EmployerName',
                    title: 'Employer Name',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'StartDate',
                    title: 'From Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'EndDate',
                    title: 'To Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'FullAddress',
                    title: 'Address',
                    type: 'text',
                    sortable: false
                },
                {
                    key: 'ContactPhoneNumber',
                    title: 'Contact Phone',
                    type: 'text',
                    sortable: false
                },
                {
                    key: 'MilitaryReserve',
                    title: 'Military Reserve',
                    type: 'text',
                    sortable: true,
                    cellTemplate: function (row) {
                        return `
                            <span>
                                ${row.MilitaryReserve === true ? 'Yes' :
                                row.MilitaryReserve === false ? 'No' : ''}
                            </span>`;
                    }
                }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No records found.',
            idProperty: 'RecordId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function getFormattedContact(name, email, phone) {
    let contact = '';

    if (name) {
        contact = `Name:${name}`;
    }

    if (email) {
        contact = contact
            ? `${contact},<br/>Email:${email}`
            : `Email:${email}`;
    }

    if (phone) {
        contact = contact
            ? `${contact},<br/>Phone:${formatPhone(phone)}`
            : `Phone:${formatPhone(phone)}`;
    }

    return contact;
}

function formatPhone(phone) {
    if (!phone) return '';

    const digits = phone.replace(/\D/g, '');

    if (digits.length === 10) {
        return `(${digits.substr(0, 3)}) ${digits.substr(3, 3)}-${digits.substr(6)}`;
    }

    return phone;
}


function loadProviderDEA() {
    showView("vwDEAResults");
    CredentialingActivityService.getProviderDEA(function (response) {
        renderProviderDEAGrid(response);
    });
}

function renderProviderDEAGrid(data) {

    $('#provider-dea-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                {
                    key: 'DEANumber',
                    title: 'DEA Number',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'DEAState',
                    title: 'State',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'EffectiveDate',
                    title: 'Effective Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'ExpirationDate',
                    title: 'Expiration Date',
                    type: 'date',
                    sortable: true
                }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No DEA numbers found.',
            idProperty: 'DEANumber',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderLicenses() {    
    showView("vwLicenseResults");

    CredentialingActivityService.getProviderLicenses(function (response) {        
        renderProviderLicensesGrid(response);
    });
}

function renderProviderLicensesGrid(data) {
    
    $('#provider-licenses-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                {
                    key: 'LicenseNumber',
                    title: 'License Number',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'LicenseState',
                    title: 'State',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'LicenseTypeName',
                    title: 'Type',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'LicenseStatus',
                    title: 'License Status',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'EffectiveDate',
                    title: 'Begin Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'ExpirationDate',
                    title: 'Expiration Date',
                    type: 'date',
                    sortable: true
                }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No licenses found.',
            idProperty: 'RegLicensureId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderSpecialties() {
    showView("vwAMBSResults");
    CredentialingActivityService.getProviderSpecialties(function (response) {
        renderProviderSpecialtiesGrid(response);
    });
}

function renderProviderSpecialtiesGrid(data) {

    $('#provider-specialties-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                {
                    key: 'BoardCertificationName',
                    title: 'Board Name',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'BoardSpecialtyName',
                    title: 'Specialty',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'EffectiveDate',
                    title: 'Original Effective / Issue Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'ExpirationDate',
                    title: 'Expiration Date',
                    type: 'date',
                    sortable: true
                }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No records found.',
            idProperty: 'BoardSpecialtyName',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderCDSNumbers() {
    showView("vwCDSResults");
    CredentialingActivityService.getProviderCDSNumbers(function (response) {
        renderProviderCDSGrid(response);
    });
}

function renderProviderCDSGrid(data) {

    $('#provider-cds-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                {
                    key: 'CDSNumber',
                    title: 'CDS Number',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'State',
                    title: 'State',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'DateIssued',
                    title: 'Effective Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'ExpirationDate',
                    title: 'Expiration Date',
                    type: 'date',
                    sortable: true
                }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No CDS numbers found.',
            idProperty: 'CDSNumber',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderEducation() {
    showView("vwEducationResults");
    CredentialingActivityService.getProviderEducation(function (response) {
        renderProviderEducationGrid(response);
    });
}

function renderProviderEducationGrid(data) {

    $('#provider-education-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                {
                    key: 'School',
                    title: 'Name of School',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'EducationType',
                    title: 'Degree / Certificate',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'FieldOfStudy',
                    title: 'Field Of Study / Specialty',
                    type: 'text',
                    sortable: true
                },
                {
                    key: 'StartDate',
                    title: 'Start Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'EndDate',
                    title: 'End Date',
                    type: 'date',
                    sortable: true
                }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No records found.',
            idProperty: 'School',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderNPDBResults() {

    CredentialingActivityService.getProviderNPDBResults(function (response) {

        showView("vwNPDBResults");

        renderProviderNPDBResults(response[0]);
    });
}

function renderProviderNPDBResults(data) {



    $("[id$='lblName']").text(data.Name || '');
    $("[id$='lblGender']").text(data.Gender || '');
    $("[id$='lbl_DOB']").text(
        data.DateOfBirth ? formatDate(data.DateOfBirth) : ''
    );
    $("[id$='lbl_Org']").text(data.Name || '');
    $("[id$='lbl_SSN']").text(
        data.TaxId ? formatSSN(data.TaxId) : ''
    );
    $("[id$='lbl_Address']").text(
        data.FullAddress || ''
    );
}

function formatDate(date) {
    const d = new Date(date);
    return isNaN(d) ? '' :
        `${String(d.getMonth() + 1).padStart(2, '0')}/` +
        `${String(d.getDate()).padStart(2, '0')}/` +
        `${d.getFullYear()}`;
}

function formatSSN(inputSSN, redact = false) {
    let rtn = '';

    if (inputSSN) {
        if (redact) {
            rtn = `XXX-XX-${inputSSN.substring(5)}`;
        } else {
            rtn = `${inputSSN.substring(0, 3)}-${inputSSN.substring(3, 5)}-${inputSSN.substring(5)}`;
        }
    }

    return rtn;
}
