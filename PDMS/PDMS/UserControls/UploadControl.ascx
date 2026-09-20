<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_UploadControl" Codebehind="UploadControl.ascx.cs" %>
<style type="text/css"> 
 
    /*.demo-container {
        display: inline-block;
        text-align: left;
    }
 
    .demo-container .RadUpload .ruUploadProgress {
        display: inline-block;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        vertical-align: top;
    }*/


</style>
<script type="text/javascript">
    function validateRadUpload(source, e) {
        e.IsValid = false;

        var upload = $find("<%= AsyncUpload1.ClientID %>");
        var inputs = upload.getUploadedFiles();
        for (var i = 0; i < inputs.length; i++) {
            //check for empty string or invalid extension 
            if (inputs[i].value == "" || !upload.isExtensionValid(inputs[i].value)) {
                return;
            }
        }
        e.IsValid = true;
    }
</script>
<%--    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />--%>
<span style="text-align:center;font-weight:bold">
    <asp:Label ID="uploadLabel" runat="server" Text=""/>
</span>
    <div>
        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" MaxFileInputsCount="1" ID="AsyncUpload1" OnFileUploaded="AsyncUpload1_FileUploaded" MultipleFileSelection="Disabled" EnableEmbeddedSkins="true" Skin="Silk" />
    </div>
   <asp:CustomValidator runat="server" ID="CustomValidator" OnServerValidate="CustomValidator_ServerValidate"
     ErrorMessage="Please upload a document">
   </asp:CustomValidator>

