<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ClaimsProviderBillingNotes, App_Web_rqhgepvh" %>

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
                <div class="col-sm-12 row" id="divProviderBillingNotesHeader" runat="server" style="background-color: lightblue;
                    margin-left: 15px"
                    visible="false">
                    <div class="col-sm-5">
                        <span class="ohio-field-label" style="font-size: 17px;  font-weight: bold; padding-left: 140px"><span style="color: red">*</span>Note Reference Code</span>
                    </div>
                    <div class="col-sm-5">
                        <span class="ohio-field-label" style="font-size: 17px; text-align: left; font-weight: bold; padding-left: 0px"><span style="color: red">*</span>Note</span>
                    </div>
                </div>                

                <div id="providerBillingNotesOutput" runat="server">


                </div>               
              

                <asp:HiddenField ID="hdnSequenceidProviderBillingNote" runat="server" />
                <asp:HiddenField ID="hdnClaim_ID" runat="server" />
                <asp:HiddenField ID="hdnClaim_Type_ID" runat="server" />
                <asp:HiddenField ID="hdnICNProviderBillingNotes" runat="server" />
                <asp:HiddenField ID="hdnProviderBillingNoteID" runat="server" />
                <asp:HiddenField ID="hdnProviderClaimStatus" runat="server" />


                <div id="divProvNotes" class="row" style="margin-left: 100px; padding-top: 100px"
                    runat="server">
                    <div id="divDdlReferenceCode" class="col-sm-4" runat="server">
                        <div class="row">
                            <div id="lblNoteReferenceCode" runat="server" class="col-sm-5 textAlignRight">
                                <span style="color: red;font-size:15px;">*</span><b>Note Reference Code</b>
                            </div>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlBillingNoteRefCode" CssClass="formField" runat="server" AutoPostBack="false"
                                    AppendDataBoundItems="true" ValidationGroup="valProviderBillingNote">
                                </asp:DropDownList>
                                 <asp:Label runat="server" ID="lblErrNoteRefCode" ForeColor="Red"></asp:Label>                             
                            </div>
                        </div>
                    </div>
                    <div id="ProvNote" runat="server">
                    <div id="divTxtProviderBillingNotes" class="col-sm-4" style="padding-left: 20px" runat="server">
                        <div class="row">
                            <div id="lblNote" runat="server" class="col-sm-4 textAlignRight">
                                <span style="color: red;font-size:17px;">*</span><b> Note </b>
                            </div>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtProviderBillingNotes" MaxLength="80" CssClass="formField" runat="server" ToolTip="80 Characters Max."
                                    ValidationGroup="valProviderBillingNote" Style="width: 500px" onKeyUp="javascript:alphanumericOnly(this);"/>
                             
                                 <asp:Label runat="server" ID="lblNoteError" ForeColor="Red"></asp:Label>
                            
                            </div>
                        </div>
                    </div>

                    <div id="divBtnProviderBillingNotes" class="col-sm-4" runat="server">
                        <div class="row">
                            <div class="col-sm-7">
                                 <asp:Label ID="lblGridLimitMsg" runat="server" style="padding: 2px" Visible="False" Text="*Maximum of 5 notes can be added" CssClass="failureNotification"/>&nbsp
                            </div>
                            <div class="col-sm-5">

                                 <asp:Button ID="btnProviderBillingNoteAdd" Text="ADD" OnClientClick="return addProviderBillingNote()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />                                
                               <br /><br />  <asp:Button ID="btnProviderBillingNoteUpdate" Style="visibility: hidden" Text="Update" OnClientClick="return UpdateProviderBillingNote()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                               <br /><br />  <asp:Button ID="btnProviderBillingNoteDelete" Style="visibility: hidden" Text="Cancel" OnClientClick="return cancelProviderBillingNote()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
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

<script>
    function ProviderDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucProviderBillingNotes_btnProviderBillingNoteAdd").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucProviderBillingNotes_btnProviderBillingNoteAdd").prop('disabled', false);
        }, 1000);
    }

    function displaytableProviderBillingNote() {

        var Providerclaimstatus = document.getElementById("<%=hdnProviderClaimStatus.ClientID %>").value;
        if (Providerclaimstatus == "Pending Submission") {
            var abc = localStorage.getItem("providerBillingNotesTable");
        }
        else if (Providerclaimstatus == "Other") {
            var abc = localStorage.removeItem("providerBillingNotesTable");
        }

        if (abc != null && abc != undefined && abc != "") {
            document.getElementById('<%= providerBillingNotesOutput.ClientID %>').innerHTML = abc;
        }
    }
    function addProviderBillingNote() {

        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var claimId = $("#<%= hdnClaim_ID.ClientID %>").val();
        var notes = $("#<%= txtProviderBillingNotes.ClientID %>").first().val();
        var referenceCode = $("#<%=ddlBillingNoteRefCode.ClientID %> option:selected").val();
        if (Claimtype == '0') referenceCode = null;
        var claimProviderBillingNotesList;
        var validateField = "";
        validateFields();
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "AddProviderBillingNotes?Claimtype=" + Claimtype + "&&claimId=" + claimId + "&&notes=" + notes + "&&referenceCode=" + referenceCode + "&&User=<%=HttpContext.Current.User.Identity.Name%>",                
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimProviderBillingNotesList = result;

                    var table;
                   // if (Claimtype == '0') {
                    //    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    //}
                    //else {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note Reference Code</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    //}

                    for (var i = 0; i < claimProviderBillingNotesList.length; i++) {

                        var sequence = i;
                        sequence++;

                        var notes = result[i].notes;
                        var ProviderNoteID = result[i].providerNoteID;
                        var Claim_ID = result[i].claimID;
                        if (Claimtype != '0') { var noteRefCode = result[i].noteRefCode; }
                       // if (Claimtype == '0') {
                         //   table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderBillingNoteLineItem(\"" + ProviderNoteID + "\",\"" + Claim_ID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:30px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        //}
                        //else {
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span style='width:250px' title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick ='return EditProviderBillingNoteLineItem(\"" + ProviderNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        //}
                    };

                    table = table + "</tbody></table>";
                    localStorage.setItem("providerBillingNotesTable", "" + table + "");
                    document.getElementById('<%= providerBillingNotesOutput.ClientID %>').innerHTML = table;
                    ProviderBillingNotesClearFields();
                    
                    if ((claimProviderBillingNotesList.length < 2)) {
                        document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "hidden";
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

    function ProviderBillingNotesClearFields() {
        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        if (Claimtype != "0") {
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucProviderBillingNotes_ddlBillingNoteRefCode") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucProviderBillingNotes_ddlBillingNoteRefCode").options[0].selected = true;
            }
        }
        $("#<%= txtProviderBillingNotes.ClientID %>").first().val("");

    }

    function EditProviderBillingNoteLineItem(ProviderNoteID, Claim_ID, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var claim_type = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var ProviderBillingNoteList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetIndProviderNote?Claim_ID=" + Claim_ID + "&&providerNoteID=" + ProviderNoteID + "&&claim_type=" + claim_type,
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
                ProviderBillingNoteList = result;
                if (ProviderBillingNoteList.length != null && ProviderBillingNoteList.length != undefined && ProviderBillingNoteList.length != "") {
                    if (ProviderBillingNoteList.length > 0) {
                        
                        for (var i = 0; i < ProviderBillingNoteList.length; i++) {
                            var sequence = i;
                            sequence++;

                            var notes = result[i].notes;
                            var ProviderNoteID = result[i].providerNoteID;
                            var Claim_ID = result[i].claimID;
                            if (claim_type != '0') {
                                var noteRefCode = result[i].noteRefCode;
                                if (noteRefCode == 'ALG - Allergies') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('1');
                                if (noteRefCode == 'DCP - Goals, Rehabilitation Potential, or Discharge Plans') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('2');
                                if (noteRefCode == 'DGN - Diagnosis Description') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('3');
                                if (noteRefCode == 'DME - Durable Medical Equipment (DME) and Supplies') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('4');
                                if (noteRefCode == 'NTR - Nutritional Requirements') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('5');
                                if (noteRefCode == 'ODT - Orders for Disciplines and Treatments') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('6');
                                if (noteRefCode == 'RHB - Functional Limitations, Reason Homebound, or Both') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('7');
                                if (noteRefCode == 'RLH - Reasons Patient Leaves Home') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('8');
                                if (noteRefCode == 'RNH - Times and Reasons Patient Not at Home') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('9');
                                if (noteRefCode == 'SET - Unusual Home, Social Environment, or Both') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('10');
                                if (noteRefCode == 'SFM - Safety Measures') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('11');
                                if (noteRefCode == 'SPT - Supplementary Plan of Treatment') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('12');
                                if (noteRefCode == 'MED - Medications') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('13');
                                if (noteRefCode == 'UPI - Updated Information') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('14');
                                if (noteRefCode == 'ADD - Additional Information') $('#<%=ddlBillingNoteRefCode.ClientID%>').val('21');
                        }
                        $("[id*=txtProviderBillingNotes]").val("" + notes + "");
                        $("[id*=hdnProviderBillingNoteID]").val("" + ProviderNoteID + "");
                        document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnProviderBillingNoteUpdate.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnProviderBillingNoteUpdate.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnProviderBillingNoteDelete.ClientID%>').style.visibility = "visible";
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

    function UpdateProviderBillingNote() {

        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var claimId = $("#<%= hdnClaim_ID.ClientID %>").val();
        var ProviderNoteID = $("#<%= hdnProviderBillingNoteID.ClientID %>").val();
        var notes = $("#<%= txtProviderBillingNotes.ClientID %>").first().val();
        var referenceCode = $("#<%=ddlBillingNoteRefCode.ClientID %> option:selected").val();
         var claimProviderBillingNotesList;
         var validateField = "";
        validateFields();
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
             $.ajax({
                 type: "PUT",
                 url: webApiClaims + "UpdateProviderBillingNotes?Claimtype=" + Claimtype + "&&claimId=" + claimId + "&&ProviderNoteID=" + ProviderNoteID + "&&notes=" + notes + "&&referenceCode=" + referenceCode + "&&userid=<%=HttpContext.Current.User.Identity.Name%>", 
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
                     claimProviderBillingNotesList = result;
                     var table;
                     if (Claimtype == '0') {
                         table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                     } else { table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note Reference Code</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>"; }
                     if (claimProviderBillingNotesList.length > 0) {
                         for (var i = 0; i < claimProviderBillingNotesList.length; i++) {
                             var sequence = i;
                             sequence++;
                             var notes = result[i].notes;
                             var ProviderNoteID = result[i].providerNoteID;
                             var Claim_ID = result[i].claimID;
                             if (Claimtype != '0') { var noteRefCode = result[i].noteRefCode; }
                             if (Claimtype == '0') {
                                 table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick ='return EditProviderBillingNoteLineItem(\"" + ProviderNoteID + "\",\"" + Claim_ID + "\",this);' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                             }
                             else
                             {
                                 table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderBillingNoteLineItem(\"" + ProviderNoteID + "\",\"" + Claim_ID + "\",this);' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                             }
                           };
                         table = table + "</tbody></table>";
                         localStorage.setItem("providerBillingNotesTable", "" + table + "");
                         document.getElementById('<%= providerBillingNotesOutput.ClientID %>').innerHTML = table;
                         ProviderBillingNotesClearFields();
                         document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "visible";
                         document.getElementById('<%= btnProviderBillingNoteUpdate.ClientID%>').style.visibility = "hidden";
                         document.getElementById('<%= btnProviderBillingNoteDelete.ClientID%>').style.visibility = "hidden";
                         //alert(claimProviderBillingNotesList.length);
                         if (claimProviderBillingNotesList.length < 2) {
                             document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "hidden";
                         }
                         else {
                             document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "visible";

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


    function DeleteClaimProviderBillingNoteLineitem(ProviderNoteID) {        
        var Claimtype = $("#<%= hdnClaim_Type_ID.ClientID %>").val();
        var result1 = confirm("Are you sure you want to delete?");
        if (result1) {           
            var claimId = $("#<%= hdnClaim_ID.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteProviderBillingNote?Claimtype=" + Claimtype + "&&claimId=" + claimId + "&&ProviderNoteID=" + ProviderNoteID,
                //data: '{Claimtype: "' + Claimtype + '" , claimId: "' + claimId + '" , ProviderNoteID: "' + ProviderNoteID + '" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimProviderBillingNotesList = result;

                    var table;
                    if (Claimtype == '0') {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }
                    else
                    {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>*Note Reference Code</th><th style='width:10px; scope='col'>*Note</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    }
                    if (claimProviderBillingNotesList.length > 0) {
                        for (var i = 0; i < claimProviderBillingNotesList.length; i++) {
                            var sequence = i;
                            sequence++;

                            var notes = result[i].notes;
                            var ProviderNoteID = result[i].providerNoteID;
                            var Claim_ID = result[i].claimID;
                            if (Claimtype != '0') { var noteRefCode = result[i].noteRefCode; }                        

                            if (Claimtype == '0') {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line'  class='tNumber'>" + sequence + "</span></td><td><span title='Line' style='width:200px' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderBillingNoteLineItem(\"" + ProviderNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:30px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                            else {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + noteRefCode + "</span></td><td><span style='width:250px' title='Line' class='tNumber'>" + notes + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditProviderBillingNoteLineItem(\"" + ProviderNoteID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimProviderBillingNoteLineitem(\"" + ProviderNoteID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("providerBillingNotesTable", "" + table + "");
                        document.getElementById('<%= providerBillingNotesOutput.ClientID %>').innerHTML = table;
                        ProviderBillingNotesClearFields();
                    }
                    else
                    {
                        document.getElementById('<%= providerBillingNotesOutput.ClientID %>').innerHTML = "";
                    }

                    if (claimProviderBillingNotesList.length < 2)
                    {
                        document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "visible";
                    }
                    else
                    {
                        document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "hidden";
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('');
                }
            });
        }
    }


    function cancelProviderBillingNote() {

        ProviderBillingNotesClearFields();
        document.getElementById('<%= btnProviderBillingNoteAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnProviderBillingNoteUpdate.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnProviderBillingNoteDelete.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("providerBillingNotesTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%=  providerBillingNotesOutput.ClientID %>').innerHTML = table;
        }
        return false;
    }

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9 ]/g, '');
    }
</script>