var CorrespondenceService = {

    searchCorrespondence: function (request, success) {

        if (!request) {
            console.error("SearchCorrespondence: request object is required");
            return false;
        }

        var url = "Correspondence/SearchCorrespondence";
        ApiService.post(url, request, success);
    },
    updateViewed: function (request, success) {

        if (!request) {
            console.error("UpdateViewed: request object is required");
            return false;
        }

        var url = "Correspondence/UpdateViewed";
        ApiService.post(url, request, success);
    }
};