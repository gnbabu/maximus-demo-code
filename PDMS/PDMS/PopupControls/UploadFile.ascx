<%@ control language="C#" autoeventwireup="true" inherits="UserControls_UploadFile" Codebehind="UploadFile.ascx.cs" %>
<%@ register src="~/UserControls/FormField.ascx" tagname="FormField" tagprefix="uc" %>

<script type="text/javascript">

    function GetUploadedItem() {
        
        var fileName = document.getElementById("<%=filUploadFile.ClientID%>").value;
        document.getElementById("<%=txtName.ClientID%>").value = fileName.substring(fileName.lastIndexOf("\\") + 1);
    }
</script>
<style type="text/css">
    .fileControl {
        display: inline !important;
    }
</style>
<br />

<center>
        <p><b><asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" /></b></p>  

    <div id="divupload" runat="server">
        <div class="row" style="padding: 4px">
            <div class="col-sm-3"></div>
            <div class ="col-sm-9 text-left">
                <mms:EncryptedFileUpload runat="server" ID="filUploadFile"  Width="500px" ViewStateMode="Enabled" CssClass="fileControl" aria-label="FileUploadFile" onchange="GetUploadedItem();"/>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 text-center"><asp:ValidationSummary ID="vsUpdateDocument" runat="server" DisplayMode="SingleParagraph" ValidationGroup="valUpdateDocument" /></div>
        </div>
        <div class="row" style="padding: 4px">
            <div id="colname1" class="col-sm-3 text-right"><span class="formLabel200">Name</span></div>
            <div id="colname2" class="col-sm-9 text-left">
                <asp:TextBox ID="txtName" runat="server" CssClass="formField wd500" MaxLength="100" aria-label="FileName" ReadOnly="true"/>
            </div>
        </div>
        <div class="row" style="padding: 4px">
            <div class="col-sm-3 text-right"><span class="formLabel200">Description</span></div>
            <div class="col-sm-9 text-left">
                <asp:TextBox ID="txtDescription" runat="server" Rows="5" CssClass="formFieldMultiline wd500" aria-label="FileDescription" TextMode="MultiLine" MaxLength="500" />
            </div>
        </div>
    </div>
    <br />
    <asp:Button id="UploadButton" Text="Upload file" OnClick="UploadButton_Click" runat="server" CssClass="buttonBox"
        ValidationGroup="valUpdateDocument" />  
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
                                OnClick="btnCancel_Click" CausesValidation="false" />
    <br>
    <p><b><asp:Label ID="lblStatusMsg" runat="server" /></b></p>  
</center>
