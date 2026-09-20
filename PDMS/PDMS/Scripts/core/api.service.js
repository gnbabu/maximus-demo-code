var ApiService = (function () {

    function getToken() {
        return localStorage.getItem("token");
    }

    function request(method, url, data, success, errorCallback) {

        let headers = {};
        let token = getToken();

        if (token) {
            headers["Authorization"] = "Bearer " + token;
        }

        $.ajax({
            url: AppConfig.apiUrl + url,
            type: method,
            data: data ? JSON.stringify(data) : null,
            contentType: "application/json",
            dataType: "json",
            headers: headers,

            success: function (response) {
                if (success) success(response);
            },

            error: function (xhr, status, error) {
                console.error("API Error:", xhr);
                if (errorCallback) errorCallback(xhr, status, error);
            }
        });
    }

    function requestDownload(method, url, data, success, errorCallback) {

        let headers = {};
        let token = getToken();

        if (token) {
            headers["Authorization"] = "Bearer " + token;
        }

        $.ajax({
            url: AppConfig.apiUrl + url,
            type: method,
            data: data ? JSON.stringify(data) : null,
            contentType: "application/json",
            xhrFields: {
                responseType: "blob"   // ✅ important difference
            },
            headers: headers,

            success: function (response, status, xhr) {

                let fileName = "download";

                const disposition = xhr.getResponseHeader("Content-Disposition");
                const contentType = xhr.getResponseHeader("Content-Type") || "application/octet-stream";

                if (disposition && disposition.indexOf("filename") !== -1) {
                    const match = disposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/);
                    if (match && match[1]) {
                        fileName = match[1].replace(/['"]/g, '');
                    }
                }

                // ✅ CRITICAL FIX
                const blob = new Blob([response], { type: contentType });

                const link = document.createElement("a");
                link.href = window.URL.createObjectURL(blob);
                link.download = fileName;

                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

                if (success) success(fileName);
            },

            error: function (xhr, status, error) {
                console.error("Download Error:", xhr);
                if (errorCallback) errorCallback(xhr, status, error);
            }
        });
    }

    return {

        get: function (url, success) {
            request("GET", url, null, success);
        },

        post: function (url, data, success) {
            request("POST", url, data, success);
        },

        put: function (url, data, success) {
            request("PUT", url, data, success);
        },

        delete: function (url, success) {
            request("DELETE", url, null, success);
        },

        download: function (url, success) {
            requestDownload("GET", url, null, success);
        }

    };

})();