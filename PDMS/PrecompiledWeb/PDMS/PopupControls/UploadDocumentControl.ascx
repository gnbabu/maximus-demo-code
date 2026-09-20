<%@ control language="C#" autoeventwireup="true" inherits="UserControls_UploadDocumentControl, App_Web_c4une0e1" %>

<script>
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {
        var savebutton = $("input[id$='btnSaveApplicationDisposition']");
        var nextbutton = $("input[id$='btnNextApplicationDisposition']");
        var save = (savebutton.length) ? savebutton.prop('disabled', false) : '';
        var next = (nextbutton.length) ? nextbutton.prop('disabled', false) : '';
    }
    function OnClientFilesUploaded(sender) {
        var $ = $telerik.$;
        $('#<%=BtnUpload.ClientID %>').click();
    }
    function OnClientFileUploading(sender, eventArgs) {
        var savebutton = $("input[id$='btnSaveApplicationDisposition']");
        var nextbutton = $("input[id$='btnNextApplicationDisposition']");
        var save = (savebutton.length) ? savebutton.prop('disabled', true) : '';
        var next = (nextbutton.length) ? nextbutton.prop('disabled', true) : '';

    }
</script>

<asp:UpdatePanel ID="upShowNames1" runat="server"   UpdateMode="Always">
        <ContentTemplate>


<div style="width: 85%;margin: 0 auto; display: table; ">
            <div style="border-style: solid; border-color: #000000; border-width: 0.03em;">
                <div style="background-color: #036; width: 100%;">
                    <span style="padding-left: 1px; text-align: center; font-weight: bold; color: #FFF;">
                        <asp:Label ID="uploadLabel" runat="server" Text="" />
                    </span>
                    <span style="text-align: center; font-style: italic; color: #FFF;">
                        <asp:Label ID="uploadInfo" runat="server" Text="" />
                    </span>
                </div>
                <div style="padding-top: 10px;padding-left:10px;">
               
                    <span>
                    <asp:Label ID="LblFileName" Text="" runat="server" ForeColor="Green" CssClass="formLabel" Width="40%"></asp:Label>
                    &nbsp;
                    <asp:LinkButton ID="LnkButtonDownload" runat="server" OnClick="OnFileDownload" ForeColor="Blue" Visible="false">Download</asp:LinkButton>
                   &nbsp;&nbsp;
                    <asp:LinkButton ID="LnkButtonDelete" runat="server" OnClick="OnFileRemove" ForeColor="Red" Visible="false">Remove</asp:LinkButton>
                     </span>    
                   <telerik:RadAsyncUpload   ID="RadAsyncUpload1" runat="server" MultipleFileSelection="Disabled" MaxFileInputsCount="1"  OnClientFileUploading="OnClientFileUploading"
                        OnFileUploaded="RadAsyncUpload1_FileUploaded" OnClientFileUploaded="OnClientFilesUploaded" EnableEmbeddedSkins="true" Skin="Default" InputSize="45" Localization-Select="Browse" EnableInlineProgress="true" EnableFileInputSkinning="true"   >
                    </telerik:RadAsyncUpload>
                    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                    <asp:HiddenField ID="hdndocumentId" runat="server"  />
                    <asp:HiddenField ID="hdnRowId" runat="server" />
                    <asp:HiddenField ID="hdnSectionName" runat="server" />
                </div>
                <asp:Button ID="BtnUpload" runat="server" style="display:none;"  Text="Upload"  ValidationGroup="valUpload" CausesValidation="true" />
                <asp:CustomValidator runat="server" ID="CustomValidatorUpload" OnServerValidate="CustomValidator_ServerValidate"  ValidationGroup="valUpload" ErrorMessage="Please upload a document">
                </asp:CustomValidator>
                
            </div>
        </div>
        <br /><br />
 
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BtnUpload" EventName="Click" />
    </Triggers>

            </ContentTemplate>

            </asp:UpdatePanel>
