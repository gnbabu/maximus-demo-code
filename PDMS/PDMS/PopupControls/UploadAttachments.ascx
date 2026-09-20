<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_UploadAttachments" Codebehind="UploadAttachments.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="jk" Namespace="JK.BootstrapControls" Assembly="PDMS" %>
<%@ Register Src="~/PopupControls/MaliciousAttachments.ascx" TagName="MaliciousAttachments"  TagPrefix="uc" %>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>

<%--<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>--%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<script type="text/javascript">

    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            sender._selectedDate = null;
            sender._textbox.set_Value(null)
        }
    }
    function onlyNumbers(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    function uploadAttachment() {
        var isValid = validateForm();

        if (isValid) {
            isValid = validateFileUpload();
        }
        if (isValid) {

            $(".addAttachment").prop("disabled", true);
            let timestamp = new Date().toLocaleTimeString('it-IT').replace(':', '').replace(':', '');

            $('#hdnTime').val(timestamp);
            let file = document.getElementById('<%=UploadAttachments.ClientID%>').files[0];
            let fileName = '<%=AttachmentFileName%>' + $("#hdnTime").val() + '--<%=AttachmentFileNameSuffix%>--' + $("#tradingPatnerID").val() + '.' + file.name.split('.').pop();
            let payLoad = JSON.stringify({ "fileName": fileName, "contentType": file.type });
            let send = document.getElementById('<%=btnSend.ClientID%>');
            $(".divUploading").show();
            $("#send").prop("disabled", false);

            // Generate presigned url
            var script = document.createElement("script");
            script.type = "text/javascript";

            script.src = '<%="../Process/StandaloneUploadAttachments.aspx" %>' + '?payLoadData=' + payLoad + '&callback=addAttachment'  ;
            document.getElementsByTagName("head")[0].appendChild(script);
        }
        return isValid;
    };

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
                __doPostBack("btnAdd", "addAttachment");
            },
            error: function (error) {
                var span = $('<span />').css('color', '#CC0505').html('Error in uploading file');
                $('.divUploading').html(span);
            },
        });
    }

    $(document).on("click", ".sendAttachments", function () {
        function sendAttachments() {
            let send = document.getElementById('<%=btnSend.ClientID%>');
            $("#send").prop("disabled", true);
        }
        sendAttachments();
    });

    function validateForm() {
        var isValid = true;

        $("#spanAttachment").hide();
        $("#spantxtRecipientID").hide();
        $("#spantxtRecipientIDMinimum").hide();
        $("#spanddlDocumentType").hide();
        $("#spantxtICN").hide();
        $("#spantxtPANumber").hide();
        $("#spanddlDestinationPayerID").hide();
        $("#spanddlTransactionTypeID").hide();
        $("#spantxtComments").hide();

        if ($(".ddlPrimaryDestinationPayer :selected").val() == "") {
            $("#spanddlPrimaryDestinationPayer").show();
            isValid = false;
        }

        if ($(".ddlDestinationPayerID :selected").val() == "") {
            $("#spanddlDestinationPayerID").show();
            isValid = false;
        }

        if (document.getElementById('<%=UploadAttachments.ClientID%>').files[0] == undefined) {
            $("#spanAttachment").show();
            isValid = false;
        }

        if ($(".txtRecipientID").val() == "") {
            $("#spantxtRecipientID").show();
            isValid = false;
        }
        else {
            if ($(".txtRecipientID").val().length < 12) {
                $("#spantxtRecipientIDMinimum").show();
                isValid = false;
            }
        }

        if ($(".ddlPriorAuthDocType :selected").val() == "") {
            $("#spanddlDocumentType").show();
            isValid = false;
        }
        
        if ($(".txtComments").val() == "") {

            $("#spantxtComments").show();
            isValid = false;
        }

        if ($(".ddlTransactionTypeID :selected").val() !== "" && $(".ddlTransactionTypeID :selected").text() == "Prior Auth" && document.getElementById('<%=txtPANumber.ClientID%>').value == "") {
            $("#spantxtPANumber").show();
            isValid = false;
        }

        if ($(".ddlTransactionTypeID :selected").val() !== "" && $(".ddlTransactionTypeID :selected").text() !== "Prior Auth" && document.getElementById('<%=txtICN.ClientID%>').value == "") {
            $("#spantxtICN").show();
            isValid = false;
        }

        if (isValid) {
            isValid = validateFileUpload();
        }

        return isValid;
    }

    function validateFileUpload() {
        $("#spanAttachment").hide();
        file = document.getElementById('<%=UploadAttachments.ClientID%>').files[0];
        if (file.size > 10485760) {
            $("#spanAttachment").show();
            $("#spanAttachment").text("File size exceeds max 10MB limit");
            return false;
        }
        if (file.size < 1) {
            $("#spanAttachment").show();
            $("#spanAttachment").text("File size cannot be 0kb");
            return false;
        }

        var fileExtension = file.name.substr(file.name.lastIndexOf("."));

        // restricted file types.
        var allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".pdf", ".zip", ".csv", ".xls", ".xlsx", ".ppt", ".doc", ".docx", ".txt", ".pptx"];
        if (!allowedExtensions.includes(fileExtension.toLowerCase())) {
            $("#spanAttachment").show();
            $("#spanAttachment").text("Not a valid file type");
            return false;
        }
        return true;
    }
</script>
<style type="text/css">
    .modal-content {
        -webkit-box-shadow: 0 5px 15px rgba(0,0,0,.5);
        box-shadow: 0 5px 15px rgba(0,0,0,.5);
        height: 589px;
        width: max-content;
    }

    .modal-body {
        height: 385px;
        width: 1349px;
        position: relative;
        padding: 15px;
    }

    .pH2 {
        padding-left: 10px;
        color: white;
    }

    .modal-bodyother {
        height: 185px;
        width: 1337px;
        position: relative;
        padding: 15px;
    }

    .modal-bodydiag {
        height: 204px;
        width: 1334px;
        position: relative;
        padding: 15px;
    }

    .formField {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 264px;
    }

    .Assignment {
        margin-bottom: 0px;
    }

    select {
        min-width: 273px;
        height: 41px;
        border: 1px solid #ccc;
    }


    .formLabel200 {
        width: 178px;
        /*width: 162px;*/
    }

    .col-lg-6 {
        /* width: 50%; */
    }

    .col-sm-3 {
        width: 23%;
    }

    .col-sm-8 {
        width: 60.666667%;
    }

    #ctl00_MainContent_UploadDocuments_pnlAttachment {
        background-color: #eaeff7;
        width: 100%;
        max-width: 100% !important;
        padding-bottom: 10px
    }

        #ctl00_MainContent_UploadDocuments_pnlAttachment .gridView, #ctl00_MainContent_UploadDocuments_pnlAttachment .gridView td, #ctl00_MainContent_UploadDocuments_pnlAttachment .gridView tr {
            border-width: 0px !important
        }

            #ctl00_MainContent_UploadDocuments_pnlAttachment .gridView td {
                padding-left: 8px !important
            }

    /*AJAX CALENDAR*/
    .QstLTCCalendarCSS .ajax__calendar_container {
        background-color: #DEF1F4;
        border: solid 1px #77D5F7;
        width: 200px !important;
        z-index: 1000 !important;
        top: -18px;
        margin-top: -30px;
    }

    .file-attachments select, .file-attachments input {
        min-width: 100% !important;
        width: 100% !important
    }

    .QstLTCCalendarCSS .ajax__calendar_header {
        background-color: #ffffff;
        margin-bottom: 4px;
    }

    .QstLTCCalendarCSS .ajax__calendar_title,
    .QstLTCCalendarCSS .ajax__calendar_next,
    .QstLTCCalendarCSS .ajax__calendar_prev {
        color: #004080;
        padding-top: 3px;
    }
</style>
    <asp:HiddenField ID="hdnTime" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="tradingPatnerID" ClientIDMode="Static" runat="server" />

    <div class="row">
      <div class="col-sm-6 col-md-4 col-lg-4 text-right">
        <asp:Label ID="lblTransactionTypeID" class="ohio-select" Text="Assignment" AssociatedControlID="ddlTransactionTypeID" runat="server">
             <div class="row">
              <div class="col-sm-4">
                <span class="ohio-field-label" ><span style="color: #CC0505">*</span>Transaction Type
                      <span data-toggle="popover" data-trigger="hover" data-placement="right" aria-hidden="true"></span>
                </span>
               </div>
                <div aria-hidden="true" class="col-sm-8">
                    <asp:DropDownList ID="ddlTransactionTypeID" CssClass="formField ddlTransactionTypeID" EnableViewState="true" runat="server" AutoPostBack="true"
                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTransactionTypeID_SelectedIndexChanged" ValidationGroup="valUploadAttachements">
                    </asp:DropDownList>
                        <span style="color: #CC0505; display: none" id="spanddlTransactionTypeID">
                            Transaction type is required
                        </span>
                  </div>
               </div>    
            </asp:Label>
       </div>
      <div class="col-sm-6 col-md-4 col-lg-4 text-right" >
        <asp:Label ID="lblPrimaryDestinationPayer" class="ohio-select" Text="Assignment" AssociatedControlID="ddlPrimaryDestinationPayer" runat="server">
            <div class="row">
              <div class="col-sm-4">
                   <span class="ohio-field-label"><span style="color: #e50000">*</span>
                    Destination Payer Name
                        <span data-toggle="popover" data-trigger="hover" data-placement="right" aria-hidden="true"></span>
                </span>
              </div>
            <div  class="col-sm-4">
                    <asp:DropDownList ID="ddlPrimaryDestinationPayer" CssClass="formField ddlPrimaryDestinationPayer" ValidationGroup="valUploadAttachements"  EnableViewState="true" runat="server" AutoPostBack="true"
                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlPrimaryDestinationPayer_SelectedIndexChanged"  >
                    </asp:DropDownList>
                       <span style="color: #CC0505; display: none" role="alert" aria-live="assertive" id="spanddlPrimaryDestinationPayer">
                            Destination Payer Name is required
                        </span>
                </div>
               
           </div>
        </asp:Label>
    </div>
      <div class="col-sm-6 col-md-4 col-lg-4 text-right" >
        <asp:Label ID="lblDestinationPayerID" class="ohio-select" Text="Assignment" AssociatedControlID="ddlDestinationPayerID" runat="server">
            <div class="row">
              <div class="col-sm-4">
                   <span class="ohio-field-label"><span style="color: #e50000">*</span>
                    Destination Payer ID
                        <span data-toggle="popover" data-trigger="hover" data-placement="right" aria-hidden="true"></span>
                </span>
              </div>
           
                <div aria-hidden="true" class="col-sm-8">
                    <asp:DropDownList ID="ddlDestinationPayerID" CssClass="formField ddlDestinationPayerID" ValidationGroup="valUploadAttachements" EnableViewState="true" runat="server"
                        AppendDataBoundItems="True">
                    </asp:DropDownList>
                       <span style="color: #CC0505; display: none" id="spanddlDestinationPayerID">
                            Destination payer is required
                        </span>
                </div>
           </div>
        </asp:Label>
    </div>
      
 </div>
<script type="text/javascript">

 function CollapseExpandPriorAuthSearch() {
     var button = document.getElementById("btattachment").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("btattachment").addClass("panelHeaderStyle");
        } else {
            button = "true"
            $("btattachment").addClass("panelHeaderStyle");
        }
     document.getElementById("btattachment").setAttribute("aria-expanded", button);
    };
</script>
<style>
    .panelHeaderStyle {
        background-color: #2297bc;
        border-style: none;
    }
</style>

<ajax:CollapsiblePanelExtender ID="cpeAttachment" runat="server" Collapsed="false" TargetControlID="pnlAttachment"
    ExpandControlID="pnlSepAttachment" CollapseControlID="pnlSepAttachment"
    ExpandedText="-" CollapsedSize="0" ScrollContents="true" CollapsedText="+" ExpandDirection="Vertical"
         SuppressPostBack="true" TextLabelID="lblsepContact"/>
<asp:Panel runat="server" ID="pnlSepAttachment" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc;padding:8px;display:flex;flex-direction:row;justify-content:space-between" ToolTip="Click to Expand/Collapse" CssClass="OwnerAttachment">
<h1><span Style="color:white;font-weight:bold;" ><button type="button" class="panelHeaderStyle" tabindex="0" id="btattachment" aria-expanded="true" onclick="CollapseExpandPriorAuthSearch()">ATTACHMENT</button></span></h1>
                <asp:Label runat="server" ID="lblsepContact" style="color:white; margin-right:30px;margin-top:7px" />
     <div style="display:flex;flex-direction:row;justify-content:space-between;display:none;margin-right:30px;margin-top:7px" class="divUploading">
            <span style="color:white"" class="uploading">Uploading please wait...</span>
            <span style="margin-right:40px;margin-left:10px;margin-bottom:10px" class="spinner"></span>
     </div>
</asp:Panel>
<asp:Panel ID="pnlAttachment" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto;">
    <ContentTemplate>
       <div>
        <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" OnRowCreated="gvAttachment_RowCreated" OnRowCommand="gvAttachment_RowCommand"
            HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
            EmptyDataText=""
            GridLines="Horizontal"
            DataKeyNames="outbound_document_uploads_id">
            
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:ImageButton ID="AttachmentIcon" runat="server" ImageUrl="~/Images/file-icon.jpg" Height="20px" Width="20px" 
                          CommandName="DownloadDocument" CommandArgument='<%# Eval("DocumentName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ICN" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblICN" runat="server" CssClass="tNumber" ToolTip="ICN"
                            Text='<%# Eval("Claim_number") %>'
                            Visible="true">
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="PA NUMBER" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                    <ItemTemplate>
                        <asp:Label ID="lblPANUMBER" runat="server" CssClass="tNumber" ToolTip="PA NUMBER"
                            Text='<%# Eval("PA_NUMBER") %>'
                            Visible="true">
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Recipient ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                    <ItemTemplate>
                        <asp:Label ID="lblreceiverID" runat="server" CssClass="tNumber" ToolTip="Receiver ID"
                            Text='<%# Eval("Member_ID") %>'
                            Visible="true">
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Document Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                    <ItemTemplate>
                        <asp:Label ID="lblDocType" Text='<%# Eval("DOCUMENT_TYPE_DESC") %>' runat="server" CssClass="tNumber" ToolTip="document Type" Visible="true">
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Document ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                    <ItemTemplate>
                        <asp:Label ID="lblDocId" runat="server" CssClass="tNumber" ToolTip="Document ID"
                            Text='<%# Eval("OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER") %>'
                            Visible="true">
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnAttachmentDelete" CommandName="DeleteDocument" CommandArgument='<%# Eval("OutBound_Document_Uploads_ID") %>' Text="Delete"
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
    <span style="color:#CC0505 ; font-size: 14pt !important; font-weight:100 !important";>An asterisk * indicates a required field</span>
    <div Style="padding:0 15px;">
        <asp:Panel ID="pnlFileUpload" runat="server" Style="min-height: 120px; min-width: 150px; height: auto; width: auto;">
        <div id="divFileUpload" class="row file-attachments" style="text-align: center;">
            <div class="col-lg-3 col-sm-3 col-md-3" style="padding-left:0px; padding-right:0px;">
                        <div class="col-lg-3 col-sm-3 col-md-3 text-left"> 
                            <span class="ohio-field-label"><span style="color: #CC0505">*</span><b> Upload attachment: </b></span>
                        </div>
                        <div class="col-lg-9 col-sm-9 col-md-9 text-left">
                              <mms:EncryptedFileUpload runat="server" ID="UploadAttachments" aria-label="Upload attachment" ViewStateMode="Enabled" CssClass="fileControl uploadAttachment"  />
                             <asp:TextBox ID="txtAttachmentName" Visible="false" runat="server" CssClass="formField wd500" MaxLength="100" ReadOnly="true" />
                        <span style="color: #CC0505; display: none" role="alert" aria-live="assertive" id="spanAttachment">
                        <br />
                         Document is required</span>
                        </div>
                             
                        </div>
            <div class="col-lg-2 col-sm-2 col-md-2" id="divICN" runat="server" visible="false">

                  <div class="row">
                        <div class="col-lg-3 col-sm-3 col-md-3 text-right" style="padding-left:0px; padding-right:0px;"> <span class="ohio-field-label"><span style="color: #CC0505">*</span><b>ICN: </b></span></div>
                    
                        <div class="col-lg-9 col-sm-9 col-md-9">
                             <asp:TextBox ID="txtICN" runat="server" CssClass="formField txtICN" Width="100%" ValidationGroup="valUploadAttachements">
                            </asp:TextBox>
                              <span style="color: #CC0505; display: none" id="spantxtICN">
                                <br />
                                ICN is required</span>
                            </div>
                      </div>              
            </div>

    
            <div class="col-lg-2 col-sm-2 col-md-2" id="divPANumber" runat="server" visible="false">
                 <div class="row">
                        <div class="col-lg-3 col-sm-3 col-md-3 text-left" style="padding-left:0px; padding-right:0px;"> <span class="ohio-field-label"><span style="color: #CC0505">*</span><b>PA Number: </b></span>
                        </div>
                      <div class="col-lg-9 col-sm-9 col-md-9">
                            <asp:TextBox ID="txtPANumber" ValidationGroup="valUploadAttachements" runat="server" CssClass="formField txtPANumber" Width="260px">
                    </asp:TextBox>
                    <span style="color: #CC0505; display: none" id="spantxtPANumber">
                        <br />
                        PA Number is required</span>
                        </div>
                </div>          
            </div>
            <div class="col-lg-3 col-sm-3 col-md-3">
                 <div class="row">
                        <div class="col-lg-3 col-sm-3 col-md-3 text-left" style="padding-left:0px; padding-right:0px;">  <span class="ohio-field-label"><span style="color: #CC0505">*</span><b>Recipient ID: </b></span>
                        </div>
                        <div class="col-lg-9 col-sm-9 col-md-9">
                            
                            <asp:TextBox ID="txtRecipientID" aria-label="Recipient ID" MaxLength="12" runat="server" CssClass="formField txtRecipientID" Width="100%">
                            </asp:TextBox>
                            <span style="color: #CC0505; display: none" id="spantxtRecipientIDMinimum">
                                <br />
                                Minimum 12 characters required</span>
                            <span style="color: #CC0505; display: none" role="alert" aria-live="assertive" id="spantxtRecipientID">
                                <br />
                                Recipient ID is required</span>
                            </div>
                    </div>                      
            </div>
            <div class="col-lg-3 col-sm-3 col-md-3">
                 <div class="row">
                        <div class="col-lg-4 col-sm-4 col-md-4 text-left" style="padding-left:0px; padding-right:0px;">  <span class="ohio-field-label"><span style="color: #CC0505">*</span><b>Document Type: </b></span>
                        </div>
                        <div class="col-lg-8 col-sm-8 col-md-8">
                        <asp:DropDownList ID="ddlPriorAuthDocType" aria-label="Document Type" runat="server" CssClass="formField ddlPriorAuthDocType" Width="100%" ValidationGroup="valUploadAttachements"></asp:DropDownList>
                        <span style="color: #CC0505; display: none" id="spanddlDocumentType">
                        <br />
                            Document type is required
                        </span>
                       </div>
                    </div>                    

            </div>
            

        </div>
            <div class="row">
                <div class="col-lg-9 col-sm-9 col-md-9">
     <div class="row">
            <div class="col-lg-1 col-sm-1 col-md-1 text-left" style="padding-left:0px; padding-right:0px;" id ="divcomments" runat="server">  <span class="ohio-field-label"><span style="color: #CC0505">*</span><b>Comments: </b></span>
            </div>
            <div class="col-lg-11 col-sm-11 col-md-11">
            <asp:TextBox ID="txtComments" aria-label="Comments" runat="server" style="width: 420px !important; height : 100px !important" CssClass="formField txtComments" Width="100%" TextMode="MultiLine" MaxLength="2000" Rows="5" Columns="50" ValidationGroup="valUploadAttachements" ></asp:TextBox>
            <span style="color: #CC0505; display: none" role="alert" aria-live="assertive"  id="spantxtComments">
            <br />
                Comments is required
            </span>
           </div>
        </div>                    

</div>
<div class="col-lg-3 col-sm-3 col-md-3 text-Center">
    <div class="col-lg-12 col-sm-12 col-md-12" style="padding-left:0px; padding-right:0px;padding-top:35px;">
        <input id="btnAddAttachment" type="button" value="Add" class="btn btn-primary addAttachment" style="padding-left:4px" Font-Bold="True" onclick="return uploadAttachment();" />
    </div>
</div>
            </div>
            <div class="row" style="align-content:flex-end; text-align:center;">
<div class="col-lg-1 col-sm-1 col-md-1">
            <asp:Button ID="btnSend" Text="Send" runat="server" OnClick="btnSend_Click" Visible="false"
        CssClass="btn btn-primary sendAttachments" style="padding-left:4px" Font-Bold="True" />
</div>
    <div class="col-lg-1 col-sm-1 col-md-1">
    <asp:Button ID="btnClear" Text="Cancel" runat="server" OnClick="btnClear_Click" Visible="false"
CssClass="btn btn-primary sendAttachments" style="padding-left:4px" Font-Bold="True" />
</div>
</div>
</div>
    </asp:Panel>
        

        <p>
           <b>
             <asp:Label ID="lblAttachmentStatusMsg" Visible="false" runat="server" /></b>
        </p>
        <p>            
            <asp:Label ID="lblAttachmentErrorMsg" Visible="false" runat="server"/>
            
        </p>
        <br />
    </asp:Panel>

<%--Malicious Attachments--%>
<ajax:CollapsiblePanelExtender ID="CollapsibleMaliciousAttachments" runat="server" Collapsed="true"
    TargetControlID="pnlMaliciousAttachments"
    ExpandControlID="pnlSepMaliciousAttachmentsInfo" CollapseControlID="pnlSepMaliciousAttachmentsInfo" />
<asp:Panel runat="server" ID="pnlSepMaliciousAttachmentsInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
    Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerEOBInfo">
    <span id="Span7" runat="server" class="pageHeader pH2">+MALICIOUS ATTACHMENTS
    </span>
</asp:Panel>
<asp:Panel ID="pnlMaliciousAttachments" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 2200px;">

    <uc:MaliciousAttachments ID="MaliciousAttachments" runat="server" />

</asp:Panel>