<%@ control language="C#" autoeventwireup="true" inherits="Controls_GlobalAdminChange" Codebehind="GlobalAdminChange.ascx.cs" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<style type="text/css">
    /* Previous styles remain the same... */
    .global-admin-section {
        margin-bottom: 20px;
    }

    .main-row {
        display: flex;
        align-items: flex-start;
        gap: 2rem;
        margin-bottom: 10px;
    }

    .title-column {
        display: flex;
        flex-direction: column;
        min-width: 200px;
        padding-top: 5px;
        text-align: left;
    }

    .lbl-bold {
        font-weight: bold;
        font-size: 16px;
        white-space: nowrap;
    }

    .help-text {
        color: #666666;
        font-size: 13px;
        font-style: italic;
        display: block;
        margin-top: 5px;
        line-height: 1.4;
    }

    .field-wrapper {
        display: flex;
        flex-direction: column;
        position: relative;
    }

    .field-header {
        display: flex;
        align-items: center;
        gap: 10px;
        margin-bottom: 5px;
    }

    .field-label {
        font-weight: 500;
        color: #333;
        font-size: 14px;
        white-space: nowrap;
    }

    .field-wrapper input[type="text"] {
        padding: 6px 10px;
        border: 1px solid #ced4da;
        border-radius: 3px;
        font-size: 14px;
        width: 200px;
        height: 34px;
    }

        .field-wrapper input[type="text"]:focus {
            outline: none;
            border-color: #80bdff;
            box-shadow: 0 0 0 0.2rem rgba(0,123,255,.25);
        }

    .validation-container {
        min-height: 20px;
        margin-top: 2px;
    }

    .validator {
        color: #dc3545;
        font-size: 12px;
        display: block;
        margin-top: 2px;
    }

    .button-wrapper {
        display: flex;
        flex-direction: column;
        padding-top: 28px;
    }

    .button-row {
        display: flex;
        gap: 10px;
    }

    .btn-upload-change {
        background-color: #5a7391;
        color: white;
        padding: 6px 20px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 500;
        font-size: 14px;
        height: 32px;
        white-space: nowrap;
    }

        .btn-upload-change:hover {
            background-color: #4a6381;
        }

    .btn-primary {
        background-color: #007bff;
        color: white;
        padding: 6px 20px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 500;
        font-size: 14px;
        height: 32px;
        white-space: nowrap;
    }

        .btn-primary:hover:not(:disabled) {
            background-color: #0056b3;
        }

        .btn-primary:disabled {
            background-color: #6c9bd1;
            cursor: not-allowed;
            opacity: 0.6;
        }

    .btn-secondary {
        background-color: #6c757d;
        color: white;
        padding: 6px 20px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 500;
        font-size: 14px;
        height: 32px;
    }

        .btn-secondary:hover {
            background-color: #5a6268;
        }

    /* Modal Background */
    .modal-bg {
        background-color: rgba(0, 0, 0, 0.5);
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        z-index: 9999;
    }

    /* Modal Panel - Centered */
    .modal-panel {
        background: white;
        padding: 25px 30px;
        border-radius: 8px;
        width: 500px;
        box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
        position: fixed;
        top: 50% !important;
        left: 50% !important;
        transform: translate(-50%, -50%);
        z-index: 10000;
    }

        .modal-panel h3 {
            margin-top: 0;
            margin-bottom: 20px;
            color: #333;
            font-size: 18px;
            font-weight: 600;
            text-align: left;
        }

        .modal-panel p {
            color: #555;
            font-size: 14px;
            margin-bottom: 20px;
            text-align: left;
        }

    .file-upload-wrapper {
        margin: 20px 0;
    }

    .file-upload-control {
        display: inline-block;
        padding: 8px 12px;
        background-color: #f8f9fa;
        border: 1px solid #ced4da;
        border-radius: 4px;
        font-size: 14px;
        cursor: pointer;
        width: 100%;
        box-sizing: border-box;
    }

        .file-upload-control:hover {
            background-color: #e9ecef;
        }

    .file-info {
        margin-top: 10px;
        font-size: 13px;
        color: #666;
    }

    .modal-buttons {
        margin-top: 25px;
        display: flex;
        gap: 10px;
        justify-content: flex-end;
    }

    /* Error state styling */
    .input-error {
        border-color: #dc3545 !important;
    }

    .ddl-Admin {
        min-width: 200px !important;
    }

    /* Responsive adjustments */
    @media (max-width: 1200px) {
        .main-row {
            flex-wrap: wrap;
        }

        .title-column {
            width: 100%;
            margin-bottom: 15px;
        }
    }

    @media (max-width: 768px) {
        .main-row {
            flex-direction: column;
            gap: 1rem;
        }

        .button-wrapper {
            padding-top: 0;
        }

        .field-wrapper input[type="text"] {
            width: 100%;
            max-width: 200px;
        }

        .modal-panel {
            width: 90%;
            max-width: 500px;
        }
    }

    .file-info {
        margin-top: 10px;
        font-size: 13px;
        color: #666;
    }

        .file-info.error {
            color: #dc3545;
            font-weight: 500;
        }
</style>

<asp:UpdatePanel ID="upGlobalAdmin" runat="server">
    <ContentTemplate>
        <div class="row">
            <asp:Label ID="lblMessage" runat="server" CssClass="failureNotification" Style="color: Red" Visible="false"></asp:Label>
        </div>
        <div class="global-admin-section">
            <!-- Main Row with all elements -->
            <div class="main-row">                
                <!-- Title Column with Help Text -->
                <div class="title-column">
                    <asp:Label ID="lblGlobalAdmin" CssClass="lbl-bold" runat="server"
                        Text="Global Administrator Change" />
                    <span class="help-text">Upon upload, a global new administrator will be assigned to the associated provider accounts.
                    </span>
                </div>

                <!-- Current OH ID Field -->
                <div class="field-wrapper">
                    <div class="field-header">
                        <asp:Label ID="lblCurrent" runat="server" CssClass="field-label"
                            AssociatedControlID="ddlProviderAdmins" Text="Current OH ID" />
                    </div>
                    <%-- <asp:TextBox ID="txtCurrentOHID" runat="server" MaxLength="50" />--%>
                    <asp:DropDownList ID="ddlProviderAdmins" runat="server" CssClass="ddl-Admin" />
                    <div class="validation-container">
                        <asp:RequiredFieldValidator ID="rfvCurrent" runat="server"
                            ControlToValidate="ddlProviderAdmins" Display="Dynamic"
                            ErrorMessage="Current OH ID is required."
                            CssClass="validator" ValidationGroup="vgGlobalAdmin"
                            InitialValue="0" />
                    </div>
                </div>

                <!-- New OH ID Field -->
                <div class="field-wrapper">
                    <div class="field-header">
                        <asp:Label ID="lblNew" runat="server" CssClass="field-label"
                            AssociatedControlID="txtNewOHID" Text="New OH ID" />
                    </div>
                    <asp:TextBox ID="txtNewOHID" runat="server" MaxLength="50" />
                    <div class="validation-container">
                        <asp:RequiredFieldValidator ID="rfvNew" runat="server"
                            ControlToValidate="txtNewOHID" Display="Dynamic"
                            ErrorMessage="New OH ID is required."
                            CssClass="validator" ValidationGroup="vgGlobalAdmin" />
                          <asp:Label ID="lblValidationMessage" runat="server" CssClass="validator" Visible="false" />
                    </div>
                </div>

                <!-- Buttons -->
                <div class="button-wrapper">
                    <div class="button-row">
                        <asp:Button ID="btnUploadChange" runat="server" Text="Upload/Change"
                            CssClass="btn-upload-change"
                            OnClick="btnUploadChange_Click"
                            ValidationGroup="vgGlobalAdmin" />
                        <asp:Button ID="btnDownload" runat="server" Text="Download"
                            CssClass="btn-primary"
                            OnClientClick="window.location.href='DownloadFile.aspx?fileName=Global Administrator Change Form 5-12-2025.docx'; return false;" />
                    </div>
                    <div class="validation-container">
                        <!-- Space reserved for button alignment -->
                    </div>
                </div>
            </div>
        </div>

        <!-- Hidden button for modal -->
        <asp:Button ID="btnHiddenModalTarget" runat="server" Style="display: none" />

        <!-- Modal Popup Extender -->
        <ajax:modalpopupextender id="mpeUpload" runat="server"
            targetcontrolid="btnHiddenModalTarget"
            popupcontrolid="pnlUploadModal"
            backgroundcssclass="modal-bg"
            dropshadow="false"
            popupdraghandlecontrolid="pnlUploadModal" />

        <!-- Upload Modal Panel - Centered -->
        <asp:Panel ID="pnlUploadModal" runat="server" CssClass="modal-panel" Style="display: none;">
            <h3>Upload Global Administrator Change Form</h3>
            <p>Please select the completed Global Administrator Change Form to upload:</p>

            <div class="file-upload-wrapper">
               <%-- <asp:FileUpload ID="fuAdminChangeForm" runat="server"
                    CssClass="file-upload-control"
                    onchange="validateFileSelection(this);" />--%>
                <mms:EncryptedFileUpload runat="server" ID="fuAdminChangeForm" aria-label="Fileupload" ViewStateMode="Enabled" CssClass="fileControl"/>
                <div class="file-info">
                    <asp:Label ID="lblFileInfo" runat="server" Text="No file chosen" CssClass="failureNotification" Style="color: Red" />
                </div>
                <%-- <div class="row">
                     <div class="col-sm-12 text-center"><asp:ValidationSummary ID="vsUpdateDocument" runat="server" DisplayMode="SingleParagraph" ValidationGroup="vgUpload" /></div>
                 </div>--%>
            </div>

            <div class="modal-buttons">
                <asp:Button ID="btnSaveUpload" runat="server" Text="Save"
                    CssClass="btn-primary"
                    OnClick="btnSaveUpload_Click"
                    ValidationGroup="vgUpload" />
                <asp:Button ID="btnCancelUpload" runat="server" Text="Cancel"
                    CssClass="btn-secondary"
                    OnClick="btnCancelUpload_Click" />
            </div>
        </asp:Panel>

        <!-- Hidden button for modal -->
        <asp:Button ID="btnDummy" runat="server" Style="display: none" />

        <!-- Modal Popup Extender -->
        <ajax:modalpopupextender id="mpeConfirmPowerAgent" runat="server"
            targetcontrolid="btnDummy"
            popupcontrolid="pnlConfirm"
            backgroundcssclass="modal-bg"
            dropshadow="false"
            popupdraghandlecontrolid="pnlConfirm" />

        <!-- Upload Modal Panel - Centered -->
        <asp:Panel ID="pnlConfirm" runat="server" CssClass="modal-panel" Style="display: none;">
            <h3>Upload Global Administrator Change Confirmation</h3>
             <div class="row" style="text-align: center">
                 <span>The power agent selected will have previous power agent access removed under additional provider administrator(s) once the administrator is transferred. The administrator functionality supersedes the previous assignments. Please confirm if you would like to proceed. </span>
             </div>
              <br />
            <div class="row">
                 <div class="modal-buttons">
                     <asp:Button ID="btnConfirm" runat="server" Text="Ok"
                       CssClass="btn-primary modal-btn" OnClick="btnConfirm_Click"
                       ValidationGroup="vgUpload" />
                   <asp:Button ID="btnCancelConfirm" runat="server" Text="Cancel"
                       CssClass="btn-secondary modal-btn" OnClick="btnCancelMpe_Click" />
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnDownload" />
        <asp:PostBackTrigger ControlID="btnSaveUpload" />
    </Triggers>
</asp:UpdatePanel>

<%--<script type="text/javascript">
    function pageLoad() {
        Sys.Application.add_load(function () {
            // Find all validators and apply error styling
            if (typeof (Page_Validators) != 'undefined') {
                for (var i = 0; i < Page_Validators.length; i++) {
                    var validator = Page_Validators[i];
                    var control = document.getElementById(validator.controltovalidate);
                    if (control && validator.style.display !== 'none') {
                        control.classList.add('input-error');
                    } else if (control) {
                        control.classList.remove('input-error');
                    }
                }
            }

            // Initialize file upload state
            var fileUpload = document.querySelector('[id$="fuAdminChangeForm"]');
            if (fileUpload) {
                validateFileSelection(fileUpload);
            }
        });
    }

    function validateFileSelection(fileInput) {
        var saveButton = document.querySelector('[id$="btnSaveUpload"]');
        var fileInfoLabel = document.querySelector('[id$="lblFileInfo"]');

        if (fileInput && saveButton && fileInfoLabel) {
            if (fileInput.files && fileInput.files.length > 0) {
                // File is selected
                saveButton.disabled = false;
                saveButton.className = 'btn-primary';

                // Update file info label with file details
                var fileName = fileInput.files[0].name;
                var fileSize = (fileInput.files[0].size / 1024).toFixed(2);
                fileInfoLabel.innerHTML = 'Selected: ' + fileName + ' (' + fileSize + ' KB)';
                fileInfoLabel.className = 'file-info'; // Remove error class
            } else {
                // No file selected
                saveButton.disabled = true;
                saveButton.className = 'btn-primary';
                fileInfoLabel.innerHTML = 'No file chosen';
                fileInfoLabel.className = 'file-info'; // Remove error class
            }
        }
    }

    // Function to show error messages in the file info label
    function showFileError(message) {
        var fileInfoLabel = document.querySelector('[id$="lblFileInfo"]');
        if (fileInfoLabel) {
            fileInfoLabel.innerHTML = message;
            fileInfoLabel.className = 'file-info error';
        }
    }

    // Reset modal when it opens
    function resetUploadModal() {
        var fileUpload = document.querySelector('[id$="fuAdminChangeForm"]');
        var saveButton = document.querySelector('[id$="btnSaveUpload"]');
        var fileInfoLabel = document.querySelector('[id$="lblFileInfo"]');

        if (fileUpload) {
            fileUpload.value = '';
        }

        if (saveButton) {
            saveButton.disabled = true;
        }

        if (fileInfoLabel) {
            fileInfoLabel.innerHTML = 'No file chosen';
            fileInfoLabel.className = 'file-info'; // Remove error class
        }
    }
</script>--%>
