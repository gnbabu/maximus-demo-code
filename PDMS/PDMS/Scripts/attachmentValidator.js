 
window.FileValidator = {
    allowedExtensions: [
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".pdf", ".csv",
            ".xls", ".xlsx", ".ppt", ".pptx", ".doc", ".docx", ".xlsm", ".mdi",
          ".jpe", ".tif", ".pi", ".ec", ".msg", ".acrbak"
    ],

    isExtensionAllowed: function (filename) {
        const ext = filename.slice(filename.lastIndexOf(".")).toLowerCase();
        return this.allowedExtensions.includes(ext);
    } 
};