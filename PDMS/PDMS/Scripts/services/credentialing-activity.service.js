var CredentialingActivityService = {

    getCredentialActivityById: function (activityId, regId, success) {
        var url = "CredentialActivity/GetCredentialActivityById"
            + "?activityId=" + activityId
            + "&regId=" + regId;

        ApiService.get(url, success);
    },

    getMalpracticeInsurance: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetMalpracticeInsuranceMapper"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getProviderDEA: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetProviderDEA"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getProviderLicenses: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetProviderLicenses"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getWorkHistoryResults: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetWorkHistoryResults"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getProviderSpecialties: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetProviderSpecialties"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getProviderEducation: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetProviderEducation"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getProviderCDSNumbers: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetProviderCDSNumbers"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getProviderNPDBResults: function (success) {
        var regId = $("[id*=hdnRegId]").val();
        var url = "CredentialActivity/GetProviderNPDBResults"
            + "?registrationId=" + regId;

        ApiService.get(url, success);
    },

    getProviderDocuments: function (providerDocumentRequest, success) {
        var url = "CredentialActivity/GetProviderDocuments";
        ApiService.post(url, providerDocumentRequest, success);
    },

    deleteProviderDocument: function (deleteRequest, success) {
        var url = "CredentialActivity/DeleteProviderDocument";
        ApiService.post(url, deleteRequest, success);
    }
};