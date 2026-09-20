var ProviderService = {

    providerSearch: function (request, success) {

        const $scope = $("#advancedSearchPanel");

        const $ctl = (suffix) => $scope.find("[id$='" + suffix + "']");

        const toNullIfEmpty = (v) => {
            if (v === undefined || v === null) return '';
            const s = String(v).trim();
            return s.length ? s : '';
        };

        const toIntOrDefault = (v, def) => {
            const n = Number(v);
            return Number.isFinite(n) && n > 0 ? n : def;
        };

        // Validate WebForms group (optional)
        if (typeof Page_ClientValidate === "function") {
            if (!Page_ClientValidate("ProviderSearch")) {
                return false;
            }
        }

        // Sorting + Paging
        const sortExpr = window.sortColWithDirection || "RegID DESC";
        const pageSize = toIntOrDefault($ctl("ddlPageSize").val(), 100);
        const pageNumber = toIntOrDefault($ctl("hdnPageNumber").val(), 1);

        // Payload
        //const request1 = {
        //    baseMedicaidID: toNullIfEmpty($ctl("txtMedicaidID").val()),
        //    groupName: toNullIfEmpty($ctl("txtGroupName").val()),
        //    dbaName: toNullIfEmpty($ctl("txtDBAName").val()),
        //    taxID: toNullIfEmpty($ctl("txtTaxID").val()),
        //    npi: toNullIfEmpty($ctl("txtNPI").val()),
        //    applicationTypeId: toNullIfEmpty($ctl("ddlApplicationType").val()),
        //    providerCategoryTypeId: toNullIfEmpty($ctl("ddlCategory").val()),
        //    waiverType: toNullIfEmpty($ctl("ddlWaiverType").val()),
        //    providerTypeId: toNullIfEmpty($ctl("ddlProviderType").val()),
        //    specialtyId: toNullIfEmpty($ctl("ddlSpecialty").val()),
        //    enrollmentStatusReason: toNullIfEmpty($ctl("ddlEnrollmentStatusReason").val()),
        //    doddContractNumber: toNullIfEmpty($ctl("txtDODDContractNumber").val()),
        //    county: toNullIfEmpty($ctl("txtCounty").val()),
        //    city: toNullIfEmpty($ctl("txtCity").val()),
        //    regID: toNullIfEmpty($ctl("txtRegID").val()),
        //    dateReceived: '', //toNullIfEmpty($ctl("txtDateReceived").val()),
        //    tennCareStatus: toNullIfEmpty($ctl("ddlTennCareStatus").val()),
        //    pnmEnrollmentStatus: toNullIfEmpty($ctl("ddlPNMEnrollStatus").val()),
        //    pnmApplicationStatus: toNullIfEmpty($ctl("ddlPNMAppStatus").val()),
        //    mcp: toNullIfEmpty($ctl("ddlMCP").val()),
        //    program: toNullIfEmpty($ctl("ddlProgram").val()),
        //    taxonomy: toNullIfEmpty($ctl("ddlTaxonomy").val()),
        //    pdmsStatus: toNullIfEmpty($ctl("ddlPDMSStatus").val()),
        //    pdmsStatusDate: '', //toNullIfEmpty($ctl("txtPDMSStatusDate").val()),
        //    medicareNumber: toNullIfEmpty($ctl("txtMedicareNumber").val()),
        //    area: toNullIfEmpty($ctl("ddlArea").val()),
        //    odaRegistrationStatus: toNullIfEmpty($ctl("ddlODARegistrationStatus").val()),
        //    doddRegistrationStatus: toNullIfEmpty($ctl("ddlDODDRegistrationStatus").val()),
        //    ContractType: '',
        //    ContractStatus: '',
        //    roleName: toNullIfEmpty($("[id$='hdnRoleName']").val()),

        //    sortExpression: sortExpr,
        //    pageSize: pageSize,
        //    pageNumber: pageNumber,

        //    startRowIndex: 0,
        //    getTotalResultCount: true
        //};

        var url = "Credentialing/ProviderSearch";
        ApiService.post(url, request, success);
    },

    getProviderCredentialActivitiesByRegId: function (success) {

        var regId = $("[id*=hdnRegId]").val();

        var url = "Credentialing/GetProviderCredentialActivities?regId=" + regId;

        ApiService.get(url, success);
    },

    getProviderCredentialActivitiesHistoryByCredRegId: function (regId, credId, success) {

        var url = "Credentialing/GetProviderCredentialActivitiesHistory?regId=" + regId + '&credentialingId=' + credId;

        ApiService.get(url, success);
    },

    getProviderCredentialHistoryByRegId: function (success) {

        var regId = $("[id*=hdnRegId]").val();

        var url = "Credentialing/GetProviderCredentialHistory?regId=" + regId;

        ApiService.get(url, success);
    },
    updateCredentialStatus: function (registrationId, statusId, userId, success) {

        var request = {
            registrationId: registrationId,
            statusId: statusId,
            userId: userId
        };

        console.log("RegistrationId:", registrationId);
        console.log("StatusId:", statusId);
        console.log("UserId:", userId);

        var url = "Credentialing/UpdateCredentialStatus";
        ApiService.post(url, request, success);
    },
    getAssignedToData: function (regId, currentStepId, isUserinRole, success) {

        var url = "Credentialing/GetAssignedToData"
            + "?regId=" + regId
            + "&currentStepId=" + currentStepId
            + "&isUserinRole=" + isUserinRole;

        ApiService.get(url, success);
    },

    assignedToUser: function (request, success) {

        if (!request) {
            console.error("assignedToUser: request is required");
            return false;
        }

        var url = "Credentialing/AssignedToUser";
        ApiService.post(url, request, success);
    },

    getAdminActions: function (success) {

        var url = "Credentialing/admin-actions";
        ApiService.get(url, success);
    },

    selectWorkflowStepsByRegID: function (success) {
        var processID = $("[id*=ddlProcessIdList]").val();
        var url = "Credentialing/SelectWorkflowStepsByRegID?processID=" + processID;

        ApiService.get(url, success);
    },

    executeAdminAction: function (request, success) {

        if (!request) {
            console.error("executeAdminAction: request is required");
            return;
        }

        var url = "Credentialing/ExecuteAdminAction";
        ApiService.post(url, request, success);
    }

};