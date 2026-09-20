<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_ClaimAuthAttachment" Codebehind="ClaimAuthAttachment.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<style>
    table.gridview td, .rgRow td, .rgAltRow td, table.gridViewSmallFont td {
        font-size: 14px;
    }

    select {
        min-width: 90%;
        height: 25px;
    }

    .gridViewFooter {
        background-color: white;
    }

    .gridViewSelected, .gridViewSelected td, .gridViewSelected tr {
        background-color: white;
    }
</style>
<script type="text/javascript">

    function validateCSS(obj) {
        let file = document.getElementById('<%=UploadAttachments.ClientID%>').files[0];
        let filesize = file.size/1000000;
        document.getElementById("<%=filesize.ClientID%>").value = filesize;
       
        "<%Session["LoadDocument"] = "False"; %>";
        "<%Session["SelectedPage"] = "SubmitClaim"; %>";
      
        document.getElementById(obj.id).style.backgroundColor = "white";
        var cleartxt = $('#' + obj.id).val().replace(/[^\p{L}\p{N}\p{P}\p{Z}^$\n]/gu, '');
        $('#' + obj.id).val(cleartxt.trim());
        if (cleartxt.trim() != "") {
            document.getElementById(obj.id).style.backgroundColor = "white";
        }
    }


    function GetClaimUploadedItem() {
 
        var fileName = document.getElementById("<%=UploadAttachments.ClientID%>").value;
        document.getElementById("<%=txtClaimAttachmentName.ClientID%>").value = fileName.substring(fileName.lastIndexOf("\\") + 1);
       
    }
    function loadAddFiles() {
       
       
    }

    $(document).on("click", ".addAttachment", function () {

            var isValid = validateFileUpload();

        if (isValid) {
            
            uploadAttachment();
        }
        else return false;

        function uploadAttachment() {

            let timestamp = new Date().toLocaleTimeString('it-IT').replace(':', '').replace(':', '');
            $('#hdnTime').val(timestamp);
            var tradingPartnerIDval = "<%= Session["DestinationPayerIDVal"]%>";
          
             let file = document.getElementById('<%=UploadAttachments.ClientID%>').files[0];
            
            let fileName = '<%=AttachmentFileName%>' + $("#hdnTime").val() + '--<%=AttachmentFileNameSuffix%>--' + tradingPartnerIDval + '.' + file.name.split('.').pop();
           
                let payLoad = JSON.stringify({ "fileName": fileName, "contentType": file.type });

                $(".divUploading").show();

                if (file.size > 10485760) {
                    alert('File size exceeds max 10MB limit');
                }
                else {
                   
                    var script = document.createElement("script");
                    script.type = "text/javascript";
                    script.src = '<%="../Process/SubmitClaim.aspx" %>' + '?payLoadData1=' + payLoad + '&callback=addAttachment' + '&MedicaidNumber=' + <%=MedicaidNumber%> ;
                    document.getElementsByTagName("head")[0].appendChild(script);
                }
          
            return isValid;
        }
    });

    function addAttachment(response) {
      
      
            let file = document.getElementById('<%=UploadAttachments.ClientID%>').files[0];
            var contentType = file.type;

            $.ajax({
                type: 'PUT',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With"
                },
                url: response.preSignedUrl,
                contentType: contentType,
                processData: false,
                async: true,
                data: file,
                crossDomain: true,
                success: function (data, status, xhr) {
                    console.log("Upload success");
                    __doPostBack("btnPriorAdd", "addAttachment");
                },
                error: function (error) {
                   
                    $(".divUploading").hide();
                    alert('File not uploaded' + error);
                },

            });
      
    }

    function validateFileUpload() {
        $("#spanAttachment").hide();
        file = document.getElementById('<%=UploadAttachments.ClientID%>').files[0];
        if (file != undefined) {
            var lblfrstname = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblfrstmi').text();
            if (lblfrstname == null || lblfrstname == undefined || lblfrstname == "") {
                $("#spanAttachment").show();
                $("#spanAttachment").text("Please enter recipient information to add attachment");
                return false;
            }
            if (file.size > 10485760) {
                $("#spanAttachment").show();
                $("#spanAttachment").text("File size exceeds max 10MB limit");
                return false;
            }
           
            var fileExtension = file.name.substr(file.name.lastIndexOf("."));
            //Restricted file types.
            var allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".pdf", ".zip", ".csv", ".xls", ".xlsx", ".ppt", ".pptx", ".doc", ".docx", ".xlsm", ".mdi", ".jpe", ".tif", ".pi", ".ec", ".msg",".acrbak"];

            if (!allowedExtensions.includes(fileExtension.toLowerCase())) {
                $("#spanAttachment").show();
                $("#spanAttachment").text("Not a valid file type");
                return false;
            }
            return true;
        } else {
            $("#spanAttachment").show();
            return false;
        }
    }

   
</script>



<div class="row" style="text-align: center; width: 97%; margin-left: 1%">

      <asp:GridView ID="gvClaimAuthAttachment" runat="server" AutoGenerateColumns="False" 
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" OnRowCommand="gvAttachment_RowCommand"
                        EmptyDataText="" OnPageIndexChanging="gvClaimAuthAttachment_PageIndexChanging"
                        GridLines="Horizontal" DataKeyNames="OutBound_Document_Uploads_ID">
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="AttachmentIcon" runat="server" ImageUrl="~/Images/file-icon.jpg" Height="20px" Width="20px"
                                        CommandName="DownloadDocument" CommandArgument='<%# Eval("DocumentName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Line Item">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Document_ID" HeaderText="Document ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="Document_Service_Name" HeaderText="Document Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                           
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnAttachmentDelete" CommandArgument='<%# Eval("OutBound_Document_Uploads_ID") %>' Text="Delete"
                                        OnCommand="btnAttachmentDelete_Command" OnClientClick='return confirm("Are you sure you want to delete this record?");'
                                        runat="server" CssClass="btn btn-danger" Font-Bold="True" CausesValidation="True" Width="90px" Sytle="margin-left:10px" />
                                </ItemTemplate>
                            </asp:TemplateField>


                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                    </asp:GridView>

    
</div>
<asp:Panel runat="server" ID="pnlAttachAdd">
    <div runat="server" id="Attach" style="padding-bottom:20px">
        <div style="display:flex;flex-direction:row;justify-content:space-between;display:none;padding-top:10px" class="divUploading">
            <span style="padding-left:20px;" class="uploading">Uploading please wait...</span>
            <span style="margin-right:10px;margin-left:10px;margin-bottom:10px" class="spinner"></span>
        </div>
        <br />
        <div class="row" style="padding-left: 40px;">
            <div class="col-sm-5">
                <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; font-weight: bold;"*>Upload attachment:</span>
            </div>
            <div class="col-sm-4">
                <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; font-weight: bold;"*>Document
                    Type:</span>
            </div>
            <div class="col-sm-3">
            </div>
        </div>
        <div class="row" style="padding-left: 40px;">
            <asp:Label runat="server" ID="lblclaimAttachError" CssClass="failureNotification"></asp:Label>
            <div class="col-sm-5 ">
                <div class="row">
                    <mms:EncryptedFileUpload runat="server" ID="UploadAttachments" ViewStateMode="Enabled" CssClass="fileControl" OnDataBinding="UploadAttachments_DataBinding" onchange="javascript:validateCSS(this);" />
                     
                        
                    <asp:TextBox ID="txtClaimAttachmentName" Visible="false" runat="server" CssClass="formField wd500"
                        MaxLength="100" ReadOnly="true" />
                     <span style="color: red; display: none" id="spanAttachment">
                          <br />
                                Document is required</span>
                      
                        <asp:Label ID="lblInstUploadErrMsg" runat="server" ForeColor="Red" />
                   
                </div>
            </div>

            <div class="col-sm-4 ">
                <div class="row" style="padding-left: 40px;">
                    <asp:DropDownList ID="ddlDocumentTypeclaims" CssClass="formField ddlPriorDentalAuthDocType" EnableViewState="true"
                        runat="server" 
                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDocumentTypeclaims_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" Display="Dynamic" ValidationGroup="valClaimUpload"
                        ErrorMessage="* Required Field" Text="*" ForeColor="Red"
                        SetFocusOnError="true" ControlToValidate="ddlDocumentTypeclaims">
                    </asp:RequiredFieldValidator>
                      <asp:Label ID="lblInstDocTypeErrMsg" Visible="false" runat="server" ForeColor="Red" />
                </div>
                
            </div>
            <div class="col-sm-3">
                <div class="row">
                    
                    <asp:Button ID="btnAddAttachment" Text="Add" runat="server" ValidationGroup="valClaimUpload"
                            CssClass="btn btn-primary buttonBoxFocus addAttachment" Font-Bold="True"/>

                </div>

                 
            </div>

        </div>
    </div>
    <div>
            
    </div>
      <p>
                    <b>
                        <asp:Label ID="lblAttachmentStatusMsg" Visible="false" runat="server" /></b>
                </p>
                <p>
                    <b>
                        <asp:Label ID="lblAttachmentErrorMsg" Visible="false" runat="server" ForeColor="Red" /></b>
                </p>
</asp:Panel>

<asp:HiddenField ID="hdnAttachClaimID" runat="server" />
<asp:HiddenField ID="filesize" runat="server" />
<asp:HiddenField ID="hdnAttachClaimType" runat="server" />
<asp:HiddenField ID="hdnFile" runat="server" />
<asp:HiddenField ID="hdnTime" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="txtDestinationpayerID" runat="server" />






