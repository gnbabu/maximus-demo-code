<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_UploadFilePT" Codebehind="UploadFilePT.ascx.cs" %>
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
    <p><b>
        <asp:label id="lblErrorMsg" runat="server" forecolor="Red" />
    </b></p>

    <div id="divupload" runat="server">
        <div class="row" style="padding: 4px">
            <div class="col-sm-3"></div>
            <div class="col-sm-9 text-left">
                <mms:encryptedfileupload runat="server" id="filUploadFile" width="500px" viewstatemode="Enabled" cssclass="fileControl" aria-label="FileUploadFile" onchange="GetUploadedItem();" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 text-center">
                <asp:validationsummary id="vsUpdateDocument" runat="server" displaymode="SingleParagraph" validationgroup="valUpdateDocument" />
            </div>
        </div>
        <div class="row" style="padding: 4px">
            <div id="colname1" class="col-sm-3 text-right"><span class="formLabel200">Name</span></div>
            <div id="colname2" class="col-sm-9 text-left">
                <asp:textbox id="txtName" runat="server" cssclass="formField wd500" maxlength="100" aria-label="FileName" readonly="true" />
            </div>
        </div>
        <div class="row" style="padding: 4px">
            <div class="col-sm-3 text-right"><span class="formLabel200">Description</span></div>
            <div class="col-sm-9 text-left">
                <asp:textbox id="txtDescription" runat="server" rows="5" cssclass="formFieldMultiline wd500" aria-label="FileDescription" textmode="MultiLine" maxlength="500" />
            </div>
        </div>
    </div>
    <br />
    <asp:button id="UploadButton" text="Upload file" onclick="UploadButton_Click" runat="server" cssclass="buttonBox"
        validationgroup="valUpdateDocument" />
    <asp:button id="btnCancel" runat="server" text="Cancel" cssclass="buttonBox"
        onclick="btnCancel_Click" causesvalidation="false" />
    <br>
    <p><b>
        <asp:label id="lblStatusMsg" runat="server" />
    </b></p>
</center>
