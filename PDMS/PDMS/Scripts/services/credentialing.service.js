var CredentialingService = {

    SelectGroupAffiliationByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "Credentialing/SelectGroupAffiliationByRegID?regId=" + regId;

        ApiService.get(url, success);
    },

    getTaxonomyByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "Credentialing/GetTaxonomyByRegID?regId=" + regId;

        ApiService.get(url, success);
    },

    getSpecialtyByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "Credentialing/GetSpecialtyByRegID?regId=" + regId;

        ApiService.get(url, success);
    },

    selectPendingAffiliationByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "Credentialing/SelectPendingAffiliationByRegID?regId=" + regId;

        ApiService.get(url, success);
    },

    selectHealthCareFacilityAffiliationByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "Credentialing/SelectHealthCareFacilityAffiliationByRegID?regId=" + regId;

        ApiService.get(url, success);
    },

    getProfessionalLicensesByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var certType = 'LICENSES'; // TODO

        var url = `Credentialing/GetProfessionalLinceses?regId=${regId}&certType=${certType}`;

        ApiService.get(url, success);
    },

    getBoardCertificationsByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var certType = "BOARD_CERTIFICATION";

        var url = "Credentialing/GetBoardCertificationsAsync?regId=" + regId + "&certType=" + certType;

        ApiService.get(url, success);
    },

    getCliaCertificationsByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var certType = "CLIA";

        var url = "Credentialing/GetCliaCertifications?regId=" + regId + "&certType=" + certType;

        ApiService.get(url, success);
    },

    getDeaCertificatesByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var state = "DEA";

        var url = "Credentialing/GetDeaCertificates?regId=" + regId + "&state=" + state;

        ApiService.get(url, success);
    },

    getCdsCertificatesByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var state = "state_cds_number";

        var url = "Credentialing/GetCdsCertificates?regId=" + regId + "&state=" + state;

        ApiService.get(url, success);
    },

    getSatellitePracticeLocationsByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var addressType = 26;

        var url = "Credentialing/GetSatellitePracticeLocations?regId=" + regId + "&addressType=" + addressType;

        ApiService.get(url, success);
    },

    getEducationByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();

        var url = "Credentialing/GetEducation?regId=" + regId;

        ApiService.get(url, success);
    },

    getWorkHistoryByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();

        var url = "Credentialing/GetWorkHistoryNew?regId=" + regId;

        ApiService.get(url, success);
    },

    getSubmittedAgreementsByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var regPageTypeId = 11;
        var regPageSection = "ApplicationPDF";

        var url = "Credentialing/GetSubmittedAgreements?regId=" + regId +
            "&regPageTypeId=" + regPageTypeId +
            "&regPageSection=" + regPageSection;

        ApiService.get(url, success);
    },

    getInsuranceByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();

        var url = "Credentialing/GetInsurance?regId=" + regId;

        ApiService.get(url, success);
    },

    getMalpracticeClaimsByRegID: function (success) {
        var regId = $("[id*=hdnRegId]").val();

        var url = "Credentialing/GetMalpracticeClaims?regId=" + regId;

        ApiService.get(url, success);
    },
    getDocuments: function (section, success) {
        var regId = $("[id*=hdnRegId]").val();

        var url = "Credentialing/GetDocuments?regId=" + encodeURIComponent(regId) +
            "&section=" + encodeURIComponent(section);
        ApiService.get(url, success);
    },
    getHelpText: function (pageName, success) {
        var userId = window.currentUserId;
        var url = "Credentialing/GetHelpText?pageName=" + encodeURIComponent(pageName) +
            "&userID=" + encodeURIComponent(userId);
        ApiService.get(url, success);
    },
    getCvoQueue: function (userId, roleName, success) {
        if (!userId) {
            console.error("userId is required");
            return;
        }

        if (!roleName) {
            console.error("roleName is required");
            return;
        }

        var url = "Credentialing/GetCvoQueue" +
            "?userId=" + encodeURIComponent(userId) +
            "&roleName=" + encodeURIComponent(roleName);

        ApiService.get(url, success);
    },

    getCommitteeQueue: function (userId, roleName, includeOtherSteps, success) {
        if (!userId) {
            console.error("userId is required");
            return;
        }

        if (!roleName) {
            console.error("roleName is required");
            return;
        }

        var url = "Credentialing/GetCommitteeQueue" +
            "?userId=" + encodeURIComponent(userId) +
            "&roleName=" + encodeURIComponent(roleName) +
            "&includeOtherSteps=" + encodeURIComponent(includeOtherSteps);

        ApiService.get(url, success);
    },

    approveProviders: function (requests, success) {

        if (!requests || !requests.length) {
            console.error("approveProviders: request list is required");
            return false;
        }

        var url = "Credentialing/ApproveProviders";
        ApiService.post(url, requests, success);
    },

    updateHidePreviousNotesFlag: function (request, success) {

        if (!request) {
            console.error("updateHidePreviousNotesFlag: request is required");
            return false;
        }

        var url = "Credentialing/UpdateHidePreviousNotesFlag";
        ApiService.post(url, request, success);
    },

    saveCredentialingPendingVerification: function (request, success) {

        if (!request) {
            console.error("saveCredentialingPendingVerification: request is required");
            return false;
        }

        var url = "Credentialing/SaveCredentialingPendingVerification";
        ApiService.post(url, request, success);
    },

    getCredentialingPendingVerification: function (regId, credentialingId, success) {

        var url = "Credentialing/GetCredentialingPendingVerification?regId="
            + encodeURIComponent(regId)
            + "&credentialingId="
            + encodeURIComponent(credentialingId);

        ApiService.get(url, success);
    }

};

