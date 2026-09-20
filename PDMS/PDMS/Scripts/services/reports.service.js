var ReportsService = {

    // Optional: You can add other methods, such as fetching reports by date range
    getReportsByDateRange: function (startDate, endDate, success) {
        var request = {
            startDate: startDate,
            endDate: endDate
        };

        //var url = `Reports/GetReportsByDateRange?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}`;

        ApiService.get('Reports/reports-by-date-range', success);
    },
    getStandardReports: function (success) {
        ApiService.get('Reports/standard', success);
    },

    getCustomReports: function (success) {
        ApiService.get('Reports/custom', success);
    },


    /* ============================================
       Re‑Credentialing Due Report
       ============================================ */
    getProviderReCredentialingDueReport: function (startDate, endDate, success) {

        var url = `Reports/GetProviderReCredentialingDueReport?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}`;

        ApiService.get(url, success);
    },

    /* ============================================
       Credentialing Clean File Report
       ============================================ */
    getProviderCredentialingCleanFile: function (startDate, endDate, success) {

        var url = `Reports/GetProviderCredentialingCleanFile?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}`;

        ApiService.get(url, success);
    },

    /* ============================================
       Credentialing Flagged Files
       ============================================ */
    getProviderCredentialingFlaggedFiles: function (startDate, endDate, success) {

        var url = `Reports/GetProviderCredentialingFlaggedFiles?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}`;

        ApiService.get(url, success);
    },

    /* ============================================
       Credentialing > 36 Months Report
       ============================================ */
    getProviderCredentialinggraterthan36MonthsReport: function (startDate, endDate, success) {

        var url = `Reports/GetProviderCredentialinggraterthan36MonthsReport?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}`;

        ApiService.get(url, success);
    },

    /* ============================================
       Clean + Flagged Files Summary
       ============================================ */
    getCredentialngCleanandFlaggedfiles: function (startDate, endDate, success) {

        var url = `Reports/GetCredentialngCleanandFlaggedfiles?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}`;

        ApiService.get(url, success);
    },

    /* ============================================
      Download Re‑Credentialing Due Report
      ============================================ */
    downloadProviderReCredentialingDueReport: function (startDate, endDate, format) {

        var url = `Reports/DownloadProviderReCredentialingDueReport?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}&format=${format}`;

        ApiService.download(url);
    },

    /* ============================================
       Download Credentialing Clean File Report
       ============================================ */
    downloadProviderCredentialingCleanFile: function (startDate, endDate, format) {

        const url =
            `Reports/DownloadProviderCredentialingCleanFile?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}&format=${format}`;

        ApiService.download(url, function (fileName) {
            console.log("Downloaded:", fileName);
        });
    },

    /* ============================================
       Download Credentialing Flagged Files
       ============================================ */
    downloadProviderCredentialingFlaggedFiles: function (startDate, endDate, format) {

        var url = `Reports/DownloadProviderCredentialingFlaggedFiles?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}&format=${format}`;

        ApiService.download(url);
    },

    /* ============================================
       Download Credentialing > 36 Months Report
       ============================================ */
    downloadProviderCredentialinggraterthan36MonthsReport: function (startDate, endDate, format) {

        var url = `Reports/DownloadProviderCredentialinggraterthan36MonthsReport?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}&format=${format}`;

        ApiService.download(url);
    },

    /* ============================================
       Download Clean + Flagged Files Summary
       ============================================ */
    downloadCredentialngCleanandFlaggedfiles: function (startDate, endDate, format) {

        console.log('api firing ');

        var url = `Reports/DownloadCredentialngCleanandFlaggedfiles?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}&format=${format}`;

        ApiService.download(url);
    }

};