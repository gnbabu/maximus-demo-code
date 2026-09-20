<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_ProviderNotes" Codebehind="ProviderNotes.ascx.cs" %>

<style type="text/css">
    .formField {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 274px;
    }

    .textAlignRight {
        text-align: right;
    }

    .gridViewSelected, .gridViewSelected td, .gridViewSelected tr {
    text-align: left;
    background-color: #f9f9f9;
    }
    .txtwatermark 
    {
        color:gray;
    }
    .txtwatermark:focus 
    {
        color:black;
    }
</style>


<asp:Panel ID="pnlProvNotes" runat="server" Style="overflow-x: hidden;">
    <updatepanel id="upProvNotesPnl" runat="server">
        <contenttemplate>
            <div runat="server" style="height: auto">
                <div class="col-sm-12 row" id="divProviderNotesHeader" runat="server" style="background-color: lightblue;
                    margin-left: 15px"
                    visible="false">
                    <div class="col-sm-5">
                        <span class="ohio-field-label" style="font-size: 17px;  font-weight: bold; padding-left: 140px"><span style="color: red">*</span>Note Reference Code</span>
                    </div>
                    <div class="col-sm-5">
                        <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 0px"><span style="color: red">*</span>Note</span>
                    </div>
                </div>
                <div ID="divProviderNotesGrid" class="divProvNotesGrid" runat="server" style="padding-top: 10px">
                </div>

                <div id="providerNotesOutput" runat="server">


                </div>
                <asp:HiddenField ID="hdnSequenceidProviderNote" runat="server" />
                <asp:HiddenField ID="hdnClaim_ID" runat="server" />
                <asp:HiddenField ID="hdnClaim_Type_ID" runat="server" />
                <asp:HiddenField ID="hdnICNProviderNotes" runat="server" />
                <asp:HiddenField ID="hdnProviderNoteID" runat="server" />
                <asp:HiddenField ID="hdnProviderClaimStatus" runat="server" />


                <div id="divProvNotes" class="row" style="margin-left: 100px; padding-top: 100px"
                    runat="server">
                    <div id="divDdlReferenceCode" class="col-sm-4" runat="server">
                        <div class="row">
                            <div id="lblNoteReferenceCode" runat="server" class="col-sm-5 textAlignRight">
                                <span style="color: red;font-size:15px;">*</span><b>Note Reference Code</b>
                            </div>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlNoteRefCode" CssClass="formField" runat="server" AutoPostBack="false"
                                    AppendDataBoundItems="true" ValidationGroup="valProviderNote">
                                </asp:DropDownList>
                                 <asp:Label runat="server" ID="lblErrNoteRefCode" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div id="ProvNote" runat="server">
                    <div id="divTxtProviderNotes" class="col-sm-4" style="padding-left: 20px" runat="server">
                        <div class="row">
                            <div id="lblNote" runat="server" class="col-sm-4 textAlignRight">
                                <span style="color: red;font-size:17px;">*</span><b> Note </b>
                            </div>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtProviderNotes" MaxLength="80" CssClass="formField" runat="server" ToolTip="80 Characters Max."
                                    ValidationGroup="valProviderNote" Style="width: 500px" onKeyUp="javascript:alphanumericOnly(this);"/>
                                 <asp:Label runat="server" ID="lblNoteError" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div id="divBtnProviderNotes" class="col-sm-4" runat="server">
                        <div class="row">
                            <div class="col-sm-7">
                                 <asp:Label ID="lblGridLimitMsg" runat="server" style="padding: 2px" Visible="False" Text="*Maximum of 5 notes can be added" CssClass="failureNotification"/>&nbsp
                            </div>
                            <div class="col-sm-5">
                                 <asp:Button ID="btnProviderNoteAdd" Text="ADD" OnClientClick="return addProviderNote()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                               <br /><br />  <asp:Button ID="btnProviderNoteUpdate" Style="visibility: hidden" Text="Update" OnClientClick="return UpdateProviderNote()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                               <br /><br />  <asp:Button ID="btnProviderNoteDelete" Style="visibility: hidden" Text="Cancel" OnClientClick="return cancelProviderNote()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                            </div>
                        </div>
                    </div>
                        </div>
                </div>
                <div class="row" style="margin-left: 100px">
                    <asp:Label ID="lblErrorText" runat="server" ForeColor="Red" Text=""></asp:Label>
                </div>
            </div>
        </contenttemplate>
    </updatepanel>

</asp:Panel>

<%--Placing scripts at the end to resolve button visibility issue, since it is throwing null in console.. After button load only script should be placed.--%>


<script>
    function ProviderDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_btnProviderNoteAdd").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_btnProviderNoteAdd").prop('disabled', false);
        }, 1000);
    }

    function displaytableProviderNote() {

        var Providerclaimstatus = document.getElementById("<%=hdnProviderClaimStatus.ClientID %>").value;
        if (Providerclaimstatus == "Pending Submission") {
            var abc = localStorage.getItem("providerNotesTable");
        }
        else if (Providerclaimstatus == "Other") {
            var abc = localStorage.removeItem("providerNotesTable");
        }

        if (abc != null && abc != undefined && abc != "") {
            document.getElementById('<%= providerNotesOutput.ClientID %>').innerHTML = abc;
        }
    }
    function addProviderNote() {

        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var claimId = $("#<%= hdnClaim_ID.ClientID %>").val();
        var notes = $("#<%= txtProviderNotes.ClientID %>").first().val();
        var referenceCode = $("#<%=ddlNoteRefCode.ClientID %> option:selected").val();
        if (Claimtype == '0') referenceCode = null;
        var claimProviderNotesList;
        var validateField = "";
        validateFields();
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "AddProviderNotes?Claimtype=" + Claimtype + "&&claimId=" + claimId + "&&notes=" + notes + "&&referenceCode=" + referenceCode + "&&User=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimProviderNotesList = result;

                    var table;
                    if (Claimtype == '0') {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }
                    else {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note Reference Code</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }

                    for (var i = 0; i < claimProviderNotesList.length; i++) {

                        var sequence = i;
                        sequence++;

                        if ((sequence >= 5) && (Claimtype == "0")) {
                            document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_btnProviderNoteAdd').style.display = 'none';
                        }
                        else if (sequence >= 10) {
                            document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_btnProviderNoteAdd').style.display = 'none';
                        }

                        var notes = result[i].notes;
                        var providerNoteID = result[i].providerNoteID;
                        var Claim_ID = result[i].claimID;
                        if (Claimtype != '0') { var noteRefCode = result[i].noteRefCode; }
                        if (Claimtype == '0') {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:30px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        }
                        else {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span style='width:250px' title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        }
                    };

                    table = table + "</tbody></table>";
                    localStorage.setItem("providerNotesTable", "" + table + "");
                    document.getElementById('<%= providerNotesOutput.ClientID %>').innerHTML = table;
                    ProviderNotesClearFields();
                    if (claimProviderNotesList.length >= 10) {
                        document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "hidden";
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Value code not found.');
                }
            });
        }
        return false;

        function validateFields() {

            if (Claimtype == "1" || Claimtype == "2") {
                if ((referenceCode === null || referenceCode === "" || referenceCode === undefined)) {

                    validateField = "false";
                    $("[id*=lblErrNoteRefCode]").text('*Note Reference code is a required field');
                    return false;

                }
                else {
                    validateField = "true";
                    $("[id*=lblErrNoteRefCode]").text('');
                }
            }
            if ((notes === null || notes === "" || notes === undefined)) {

                validateField = "false";
                $("[id*=lblNoteError]").text('*Note is a required field');
                return false;

            }
            else {
                validateField = "true";
                $("[id*=lblNoteError]").text('');
            }

        }
    }

    function ProviderNotesClearFields() {
        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        if (Claimtype != "0") {
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_ddlNoteRefCode") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_ddlNoteRefCode").options[0].selected = true;
            }
        }
        $("#<%= txtProviderNotes.ClientID %>").first().val("");

    }

    function EditProviderNoteLineItem(providerNoteID, Claim_ID,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var claim_type = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var providerNoteList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetIndProviderNote?Claim_ID=" + Claim_ID + "&&providerNoteID=" + providerNoteID + "&&claim_type=" + claim_type,
            //data: '{Claim_ID: "' + Claim_ID + '", providerNoteID:"' + providerNoteID + '", claim_type:"' + claim_type + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                providerNoteList = result;
                if (providerNoteList.length != null && providerNoteList.length != undefined && providerNoteList.length != "") {
                    if (providerNoteList.length > 0) {

                        for (var i = 0; i < providerNoteList.length; i++) {
                            var sequence = i;
                            sequence++;

                            var notes = result[i].notes;
                            var providerNoteID = result[i].providerNoteID;
                            var Claim_ID = result[i].claimID;
                            if (claim_type != '0') { var noteRefCode = result[i].noteRefCode; }
                            if (claim_type != '0') {
                                if (noteRefCode == 'ALG - Allergies') $('#<%=ddlNoteRefCode.ClientID%>').val('1');
                                if (noteRefCode == 'DCP - Goals, Rehabilitation Potential, or Discharge Plans') $('#<%=ddlNoteRefCode.ClientID%>').val('2');
                                if (noteRefCode == 'DGN - Diagnosis Description') $('#<%=ddlNoteRefCode.ClientID%>').val('3');
                                if (noteRefCode == 'DME - Durable Medical Equipment (DME) and Supplies') $('#<%=ddlNoteRefCode.ClientID%>').val('4');
                                if (noteRefCode == 'NTR - Nutritional Requirements') $('#<%=ddlNoteRefCode.ClientID%>').val('5');
                                if (noteRefCode == 'ODT - Orders for Disciplines and Treatments') $('#<%=ddlNoteRefCode.ClientID%>').val('6');
                                if (noteRefCode == 'RHB - Functional Limitations, Reason Homebound, or Both') $('#<%=ddlNoteRefCode.ClientID%>').val('7');
                                if (noteRefCode == 'RLH - Reasons Patient Leaves Home') $('#<%=ddlNoteRefCode.ClientID%>').val('8');
                                if (noteRefCode == 'RNH - Times and Reasons Patient Not at Home') $('#<%=ddlNoteRefCode.ClientID%>').val('9');
                                if (noteRefCode == 'SET - Unusual Home, Social Environment, or Both') $('#<%=ddlNoteRefCode.ClientID%>').val('10');
                                if (noteRefCode == 'SFM - Safety Measures') $('#<%=ddlNoteRefCode.ClientID%>').val('11');
                                if (noteRefCode == 'SPT - Supplementary Plan of Treatment') $('#<%=ddlNoteRefCode.ClientID%>').val('12');
                                if (noteRefCode == 'MED - Medications') $('#<%=ddlNoteRefCode.ClientID%>').val('13');
                            if (noteRefCode == 'UPI - Updated Information') $('#<%=ddlNoteRefCode.ClientID%>').val('14');
                        }
                        $("[id*=txtProviderNotes]").val("" + notes + "");
                        $("[id*=hdnProviderNoteID]").val("" + providerNoteID + "");
                        document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnProviderNoteUpdate.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnProviderNoteUpdate.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnProviderNoteDelete.ClientID%>').style.visibility = "visible";
                        return false;
                    };
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
            }
        });

    }

    function UpdateProviderNote() {

        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var claimId = $("#<%= hdnClaim_ID.ClientID %>").val();
        var ProviderNoteID = $("#<%= hdnProviderNoteID.ClientID %>").val();
        var notes = $("#<%= txtProviderNotes.ClientID %>").first().val();
        var referenceCode = $("#<%=ddlNoteRefCode.ClientID %> option:selected").val();
         var claimProviderNotesList;
         var validateField = "";
        validateFields();
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
             $.ajax({
                 type: "PUT",
                 url: webApiClaims + "UpdateProviderNotes?Claimtype=" + Claimtype + "&&claimId=" + claimId + "&&ProviderNoteID=" + ProviderNoteID + "&&notes=" + notes + "&&referenceCode=" + referenceCode + "&&user=<%=HttpContext.Current.User.Identity.Name%>",
                 //data: '{Claimtype: "' + Claimtype + '" , claimId: "' + claimId + '", ProviderNoteID: "' + ProviderNoteID + '" , notes: "' + notes + '", referenceCode: "' + referenceCode + '" }',
                 headers: {
                     "Access-Control-Allow-Origin": "*",
                     "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                     "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                     "Authorization": "Bearer " + APIToken
                 },
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (result) {
                     claimProviderNotesList = result;
                     var table;
                     if (Claimtype == '0') {
                         table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                     } else { table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note Reference Code</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>"; }
                     if (claimProviderNotesList.length > 0) {
                         for (var i = 0; i < claimProviderNotesList.length; i++) {
                             var sequence = i;
                             sequence++;

                             var notes = result[i].notes;
                             var providerNoteID = result[i].providerNoteID;
                             var Claim_ID = result[i].claimID;
                             if (Claimtype != '0') { var noteRefCode = result[i].noteRefCode; }
                             if (Claimtype == '0') {
                                 table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                             }
                             else
                             {
                                 table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                             }
                           };
                         table = table + "</tbody></table>";
                         localStorage.setItem("providerNotesTable", "" + table + "");
                         document.getElementById('<%= providerNotesOutput.ClientID %>').innerHTML = table;
                         ProviderNotesClearFields();
                         document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "visible";
                         document.getElementById('<%= btnProviderNoteUpdate.ClientID%>').style.visibility = "hidden";
                         document.getElementById('<%= btnProviderNoteDelete.ClientID%>').style.visibility = "hidden";
                         if (claimProviderNotesList.length < 10) {
                             document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "visible";
                         }
                         else {
                             document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "hidden";

                         }

                     }
                 },
                 error: function (jqXHR, textStatus, errorThrown) {
                     $("[id*=lblmessage]").text('Value code not found.');
                 }
             });
        }
        return false;

        function validateFields() {

            if ((notes === null || notes === "" || notes === undefined)) {

                validateField = "false";
                $("[id*=lblNoteError]").text('*This is a required field');
                return false;

            }
            else {
                validateField = "true";
                $("[id*=lblNoteError]").text('');
            }

        }
    }


    function DeleteClaimProviderNoteLineitem(providerNoteID) {
        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var result1 = confirm("Are you sure you want to delete?");
        if (result1) {           
            var claimId = $("#<%= hdnClaim_ID.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteProviderNote?Claimtype=" + Claimtype + "&&claimId=" + claimId + "&&ProviderNoteID=" + providerNoteID,
                //data: '{Claimtype: "' + Claimtype + '" , claimId: "' + claimId + '" , ProviderNoteID: "' + providerNoteID + '" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimProviderNotesList = result;

                    var table;
                    if (Claimtype == '0') {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }
                    else
                    {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note Reference Code</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }
                    if (claimProviderNotesList.length > 0) {
                        for (var i = 0; i < claimProviderNotesList.length; i++) {
                            var sequence = i;
                            sequence++;
                            if ((sequence >= 5) && (Claimtype == "0")) {
                                document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_btnProviderNoteAdd').style.display = 'none';
                            }
                            else if (sequence >= 10) {
                                document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_btnProviderNoteAdd').style.display = 'none';
                            }
                            else {
                                document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucProviderNotes_btnProviderNoteAdd').style.display = 'inline';
                            }
                            var notes = result[i].notes;
                            var providerNoteID = result[i].providerNoteID;
                            var Claim_ID = result[i].claimID;
                            if (Claimtype != '0') { var noteRefCode = result[i].noteRefCode; }                        

                            if (Claimtype == '0') {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line'  class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:30px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                            else {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span style='width:250px' title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderNoteLineItem(\"" + providerNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderNoteLineitem(\"" + providerNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("providerNotesTable", "" + table + "");
                        document.getElementById('<%= providerNotesOutput.ClientID %>').innerHTML = table;
                        ProviderNotesClearFields();
                    }
                    else
                    {
                        document.getElementById('<%= providerNotesOutput.ClientID %>').innerHTML = "";
                    }

                    if (claimProviderNotesList.length < 10)
                    {
                        document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "visible";
                    }
                    else
                    {
                        document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "hidden";
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('');
                }
            });
        }
    }


    function cancelProviderNote() {

        ProviderNotesClearFields();
        document.getElementById('<%= btnProviderNoteAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnProviderNoteUpdate.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnProviderNoteDelete.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("providerNotesTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= providerNotesOutput.ClientID %>').innerHTML = table;

        }
        return false;
    }

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9 ]/g, '');
    }
</script>