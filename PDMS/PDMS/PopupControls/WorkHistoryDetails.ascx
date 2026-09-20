<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_WorkHistoryDetails" Codebehind="WorkHistoryDetails.ascx.cs" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register src="~/PopupControls/WorkHistory.ascx" tagprefix="uc" tagname="WorkHistory" %>
<%@ register src="~/UserControls/Address.ascx" tagname="Address" tagprefix="uc" %>

<style>
    .table-striped > tbody > tr:nth-child(odd) > td,
    .table-striped > tbody > tr:nth-child(odd) > th {
        text-align: left;
        vertical-align: top;
        background-color: #EAEFF7;
    }

    .table-striped > tbody > tr:nth-child(even) > td,
    .table-striped > tbody > tr:nth-child(even) > th {
        background-color: #E8F0F9;
        text-align: left;
        vertical-align: top;
    }

    #dataTable_WorkHistoryDet tr td {
        vertical-align: top !important;
    }

    span.dt-column-order {
        top: 40px !important;
    }

    .datatable-min-width120 {
        min-width: 120px !important;
    }

    .datatable-min-width150 {
        min-width: 150px !important;
    }

    .datatable-min-width180 {
        min-width: 180px !important;
    }

    .datatable-min-width200 {
        min-width: 200px !important;
    }

    select {
        min-width: 75px !important;
    }

    .dt-length label {
        font-family: "Arial Narrow" !important;
        font-size: 14pt !important;
        font-weight: 100 !important;
        padding-left: 4px !important;
    }

    .dt-length {
        padding-right: 5px !important;
    }

    .dt-paging {
        text-align: right !important;
        padding-right: 5px !important;
    }

    .dt-search {
        text-align: right !important;
        padding-right: 5px !important;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        $("#dt-length-0").addClass("dt-length-select");
        $.fn.dataTable.ext.errMode = 'none';

        var isWHRedirectionNeeded = $("[id*=hdnRedirectToNewWorkHistory]").val();
     
        if (isWHRedirectionNeeded !== '' && isWHRedirectionNeeded !== undefined &&
            isWHRedirectionNeeded !== null && isWHRedirectionNeeded !== false) {
           
            $("#hstryolddiv").hide();
            $("#hstryNewdiv").show();
        }
        else {
           
            $("#hstryolddiv").show();
            $("#hstryNewdiv").hide();
        }
    });

    function IsNumeric(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    function IsNumisAlphaNumericeric(e) {
        var charCode = (e.which) ? e.which : e.keyCode;
        if (charCode == 8) return true;

        var keynum;
        var keychar;
        var charcheck = /[a-zA-Z0-9]/;
        if (window.event) // IE
        {
            keynum = e.keyCode;
        }
        else {
            if (e.which) // Netscape/Firefox/Opera
            {
                keynum = e.which;
            }
            else return true;
        }

        keychar = String.fromCharCode(keynum);
        return charcheck.test(keychar);
    };

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
    }

    function displayWorkHistoryData() {
        $("#<%= btnCancel.ClientID %>").removeAttr('disabled');
        $("#<%= btnExport.ClientID %>").removeAttr('disabled')

        $("#<%= btnClose.ClientID %>").removeAttr('disabled');
        $("#<%= btnCancel.ClientID %>").addClass('historyButtonBox');
        $("#<%= btnExport.ClientID %>").addClass('historyButtonBox');
        var webApiEnrollment = $("[id*=hdnPDMSWebAPI]").val() + "Enrollment/";
        var APIToken = $("[id*=hdnAccessToken]").val();
        var regId = $("[id*=hdnWrkHistoryRegId]").val();
        console.log(webApiEnrollment + "GetWorkHistoryData?regId=" + regId);
        document.getElementById("ctl00_upProgress").style.display = 'block';
        $.ajax({
            type: "GET",
            url: webApiEnrollment + "GetWorkHistoryData?regId=" + regId,
            async: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                document.getElementById("ctl00_upProgress").style.display = 'none';
                getWorkHistoryData(result, regId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error(textStatus + "\n" + errorThrown);

                document.getElementById("ctl00_upProgress").style.display = 'none';
                document.getElementById('output').innerHTML = 'Something went wrong. Please contact system administrator....!';
            }
        });
        return false;
    }

    function getWorkHistoryData(result, regId) {
        if (result.length > 0) {
            document.getElementById('divWorkHistoryNoData').innerHTML = '';

            $("#dataTable_WorkHistoryDet").DataTable({
                lengthMenu: [
                    [5, 10, 25, 50, 100, -1],
                    [5, 10, 25, 50, 100, 'All']
                ],
                "stripeClasses": ['gridViewRow', 'gridViewAltRow'],
                scrollY: "325px",
                "bDestroy": true,
                dom: 'Bfrtip',
                dom: "<'row'<'col-sm-4'l><'col-sm-4'><'col-sm-4'f>>" +
                    "<'row'<'col-sm-12'rt>>" +
                    "<'row'<'col-sm-4'i><'col-sm-4'><'col-sm-4'p>>",
                data: result,
                columns: [
                    { data: 'Operation', className: 'dt-left dt-top' },
                    { data: 'Current_Employer', className: 'dt-left dt-top' },
                    { data: 'PRACTICE_NAME', className: 'dt-left' },
                    {
                        data: 'Worked_From', className: 'dt-left', "sType": "date" // type of the column
                        , "render": function (data) {
                            return data ? moment(data).format('MM/DD/YYYY') : "";
                        }
                    },
                    {
                        data: 'Worked_To', className: 'dt-left', "sType": "date" // type of the column
                        , "render": function (data) {
                            return data ? moment(data).format('MM/DD/YYYY') : "";
                        }
                    },
                    { data: 'Organization_Name', className: 'dt-left' },
                    { data: 'ADDRESS1', className: 'dt-left' },
                    { data: 'ADDRESS2', className: 'dt-left' },
                    { data: 'CITY', className: 'dt-left' },
                    { data: 'STATE', className: 'dt-left' },
                    { data: 'COUNTY', className: 'dt-left' },
                    { data: 'ZIP', className: 'dt-left' },

                    { data: 'CONTACT_PHONE_NUMBER', className: 'dt-left' },
                    { data: 'CONTACT_PHONE_EXT', className: 'dt-left' },
                    { data: 'FAX1', className: 'dt-left' },
                    { data: 'FAX2', className: 'dt-left' },
                    { data: 'CONTACT_NAME', className: 'dt-left' },

                    { data: 'Contact_Email_Address1', className: 'dt-left' },
                    { data: 'Contact_Email_Address2', className: 'dt-left' },
                    { data: 'ADDITIONAL_INFO', className: 'dt-left' },
                    { data: 'REASON_FOR_DEPARTURE', className: 'dt-left' },
                    { data: 'MILTARY_RESERVE', className: 'dt-left' },

                    { data: 'LAST_MODIFIED_USERNAME', className: 'dt-left' },
                    {
                        data: 'LAST_MODIFIED_DATE_TIME', className: 'dt-left', "sType": "date" // type of the column
                        , "render": function (data) {
                            return data ? moment(data).format('MM/DD/YYYY h:mm:ss A') : "";
                        }
                    }
                ],
                "paging": true,
                "info": true,
                "language": {
                    "emptyTable": "No data available"
                }
            });
        }
        else {
            document.getElementById('divWorkHistoryData').innerHTML = '';
            document.getElementById('divWorkHistoryNoData').innerHTML = ' <h3 style="color:green"> No work history data available.</h3>';
        }
        $find("ctl00_MainContent_ucWorkHistoryDetails_" + regId + "_mpeHistory").show();
    }

    function getFormattedDate(dateVal) {
        var year = dateVal.getFullYear();
        var month = (1 + dateVal.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = dateVal.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return month + '/' + day + '/' + year;
    }
</script>

<script>
    function GoToDemoWorkHistory(event) {
        event.preventDefault()
        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
        var regId = $("[id*=hdnWrkHistoryRegId]").val();
        var redirectionUrl = urlpath + "/WorkHistoryDetails.aspx?regId=" + regId;
        console.log(redirectionUrl);
        window.location.href = redirectionUrl;
    }
</script>

<div>
    <asp:ValidationSummary ID="vsWorkHistory" runat="server" DisplayMode="List" ValidationGroup="vgWorkHistoryDetails" />
</div>
<div class="w-100">
    <div class="row">
        <div class="col-sm-9">
            <div class="pg-hint4 f-none">
                <span>Include a chronological work history for the past 5 years. </span>
            </div>
        </div>
    </div>
</div>
<div id="pnlWorkHistoryList" runat="server">
    <div class="divGrid">
        <asp:GridView runat="server" ID="grdWork" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grdWork_RowCommand">
            <Columns>
                <%--<asp:BoundField DataField="TITLE" HeaderText="Title" />--%>
                <asp:BoundField DataField="NAME" HeaderText="Practice/ Employer Name" />
                <asp:BoundField DataField="WORKED_FROM" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="WORKED_TO" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnRegAddressId" runat="server" Value='<%# Eval("REG_ADDRESS_ID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:templatefield headertext="Contact Details">
            <itemtemplate>
              <asp:label id="lblContact" text= '<%# GetContactDetails(Eval("CONTACT_NAME"),Eval("CONTACT_EMAIL_ADDRESS"),Eval("CONTACT_PHONE_NUMBER"))%>' runat="server"/> 
              
            </itemtemplate>
          </asp:templatefield>--%>
                <asp:TemplateField ItemStyle-Width="2%" HeaderText="EDIT">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditWorkDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddWorkItem" runat="server" AlternateText="ADD" ImageUrl="~/Images/add.png" CommandName="WorkHistoryAdd" OnCommand="lbtnAdd_Click" ToolTip="Add" /><br />
        <div id="hstryolddiv">
            <asp:LinkButton ID="btnWorkHistory" runat="server" CssClass="buttonBoxFocus hbtn-focus" OnClientClick="javascript:return displayWorkHistoryData();" Text="History" ToolTip="History">
              <img src="../Images/HistoryIcon1.png" alt="History Icon" class="history-icon" /> History 
            </asp:LinkButton>
        </div>
    </div>
    <div id="hstryNewdiv" style="text-align: right; padding-top: 15px;">
        <a href="javascript:void(0);" class="buttonBoxFocus hbtn-focus" onclick="GoToDemoWorkHistory(event);" title="History">
            <img src="../Images/HistoryIcon1.png" alt="History Icon" class="history-icon" />
            History
        </a>
    </div>
    <br />
</div>
<asp:UpdatePanel ID="pnlWorkItem" runat="server" UpdateMode="Conditional" Visible="false">
    <ContentTemplate>
        <div class="container">
            <%--<div class="row">
        <div class="col-sm-3 text-right">
        <asp:Label ID="lblTitle" runat="server" Text="* Title:" CssClass="formLabel wd200" ></asp:Label>
         </div>   
         <div class="col-sm-8">     
        <asp:TextBox ID="tbTitle" runat="server" CssClass="formField"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ID="rfvTitle" SetFocusOnError="true" 
            ValidationGroup="vgWorkHistoryDetails" ControlToValidate="tbTitle" ErrorMessage="*Enter a Title" Text="*" Display="Dynamic" />
         </div>
    </div>
            --%>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="lblCurrentEmployer" runat="server" Text="Current Employer" CssClass="formLabel wd200"></asp:Label>
                </div>
                <div class="col-sm-7 text-left">
                    <asp:CheckBox ID="chkIsCurrentEmployer" onchange="Page_BlockSubmit = false;" runat="server" RepeatDirection="Horizontal" CssClass="formCheckBox" AutoPostBack="true" OnCheckedChanged="chkIsCurrentEmployer_Changed"></asp:CheckBox>

                </div>
            </div>


            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="lblName" runat="server" Text="*Practice/ Employer Name:" CssClass="formLabel wd200"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="tbOrgName" runat="server" onchange="Page_BlockSubmit = false;" CssClass="formField" onKeyUp="javascript:alphanumericOnly(this);"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RfvName" SetFocusOnError="true"
                        ValidationGroup="vgWorkHistoryDetails" ControlToValidate="tbOrgName" ErrorMessage="*Enter Organization Name" Text="*" Display="Dynamic" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="lblFrom" runat="server" Text="* Start Date:" CssClass="formLabel wd200"></asp:Label>

                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="tbFrom" runat="server" CssClass="formField"></asp:TextBox>
                    <ajax:calendarextender id="calExtenderWorkFrom" targetcontrolid="tbFrom" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="RfvFromDate" SetFocusOnError="true"
                        ValidationGroup="vgWorkHistoryDetails" ControlToValidate="tbFrom" ErrorMessage="*Enter Start Date" Text="*" Display="Dynamic" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="lblTo" runat="server" Text="* End Date:" CssClass="formLabel wd200"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField"></asp:TextBox>
                    <ajax:calendarextender id="calExtenderWorkTo" targetcontrolid="txtEndDate" runat="server" />
                    <%--    <asp:RequiredFieldValidator runat="server" ID="RfvToDate" SetFocusOnError="true" 
            ValidationGroup="vgWorkHistoryDetails" ControlToValidate="txtEndDate" ErrorMessage="*Enter End Date" Text="*" Display="Dynamic" />--%>
                </div>
            </div>


            <uc:address id="ucAddress" runat="server" validationgroup="vgWorkHistoryDetails"></uc:address>

            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="lblAdditional" runat="server" Text="Additional Information:" CssClass="formLabel wd200"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="tbAdditional" runat="server" TextMode="MultiLine" CssClass="formField" Rows="3" onKeyUp="javascript:alphanumericOnly(this);" MaxLength="150"></asp:TextBox>

                </div>
            </div>
            <div class="row">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblReasonForDepart" runat="server" Text="Reason for Departure(If Applicable):" CssClass="formLabel wd250"></asp:Label>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="tbReasonForDepart" runat="server" MaxLength="25" TextMode="MultiLine" CssClass="formField" Rows="3" Style="width: 370px" onKeyUp="javascript:alphanumericOnly(this);"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-6 text-left">
                    <asp:Label ID="lblbmiltaryreserve" runat="server" Text="*Are you currently on active miltary duty or miltary reserve?" CssClass="formLabel wd200"></asp:Label>
                </div>
                <div class="col-sm-3 text-left">
                    <asp:DropDownList ID="ddlmiltaryreserve" runat="server" CssClass="hSetminwidth">

                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvMiltaryResver" SetFocusOnError="true"
                        ValidationGroup="vgWorkHistoryDetails" ControlToValidate="ddlmiltaryreserve" ErrorMessage="*" Text="*" Display="Dynamic" />
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdnRegWorkHistoryId" runat="server" />
        <asp:HiddenField ID="hdnRegGapId" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<span class="pageHeader">Gaps in Work History</span>
<br />
<br />
<div class="row">
    <div class="col-sm-9">
        <div class="pg-hint4 f-right">
            <span>Please enter and explain any time periods or gaps in work history in the past 5 years or that have occurred</span>
            <span>since graduation from professional school and are longer than three months in duration.</span>
        </div>
    </div>
</div>
<div id="pnlWorkGap" runat="server">
    <div class="divGrid">

        <asp:GridView runat="server" ID="gvGaps" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="gvGaps_RowCommand">
            <Columns>

                <asp:BoundField DataField="REG_ID" HeaderText="REG_ID" Visible="false" />
                <asp:BoundField DataField="GAP_START_DATE" HeaderText="Gap Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="GAP_END_DATE" HeaderText="Gap End Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="GAP_REASON" HeaderText="Reason" />

                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditWorkGapDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="imgAddGap" AlternateText="add" runat="server" ImageUrl="~/Images/add.png" CommandName="AddWorkGap" OnCommand="imgAddGap_Click" ToolTip="Add" />

    </div>
</div>

<asp:Panel runat="server" ID="pnlGap" Visible="false">
    <div class="row">
        <div class="col-sm-3  text-right">
            <asp:Label ID="lblGapStart" runat="server" Text="*Gap Start Date:" CssClass="formLabel wd200"></asp:Label>
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="tbGapStartDate" runat="server" CssClass="formField"></asp:TextBox>
            <ajax:calendarextender id="CalendarExtender1" targetcontrolid="tbGapStartDate" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="rfvGapStart" SetFocusOnError="true"
                ValidationGroup="Gap" ControlToValidate="tbGapStartDate" ErrorMessage="*Enter Gap Start Date" Text="*" Display="Dynamic" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3  text-right">
            <asp:Label ID="lblGapEnd" runat="server" Text="*Gap End Date:" CssClass="formLabel wd200"></asp:Label>
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="tbGapEndDate" runat="server" CssClass="formField"></asp:TextBox>
            <ajax:calendarextender id="CalendarExtender2" targetcontrolid="tbGapEndDate" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="rfvGapEnd" SetFocusOnError="true"
                ValidationGroup="Gap" ControlToValidate="tbGapEndDate" ErrorMessage="*Enter Gap End Date" Text="*" Display="Dynamic" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3  text-right">
            <asp:Label ID="lblGapText" runat="server" Text="*Reason For Gap:" CssClass="formLabel wd200"></asp:Label>
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="tbGapText" runat="server" TextMode="MultiLine" CssClass="formField" Rows="3"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="rfvGapText" SetFocusOnError="true"
                ValidationGroup="Gap" ControlToValidate="tbGapText" ErrorMessage="*Enter Gap Reason" Text="*" Display="Dynamic" />
        </div>
    </div>
</asp:Panel>
<ajax:modalpopupextender id="mpeHistory" runat="server" popupcontrolid="pnlModalWrk" targetcontrolid="ButtonDummy" cancelcontrolid="btnCancel" okcontrolid="btnClose" backgroundcssclass="modalBackground">
</ajax:modalpopupextender>
<asp:Panel ID="pnlModalWrk" runat="server" CssClass="modalPopup historyPopup" Style="display: none; width: 80%; padding: 20px; height: auto; z-index: 10000 !important;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" Style="background-color: #545487 !important;">
        <div class="popTitle">
            <asp:Label ID="lblHistory" runat="server" Text="Work History" />
            <asp:LinkButton ID="btnClose" runat="server" CssClass="closeBtn hcloseBtn" CausesValidation="false">
                <img src="../Images/close-button.png" alt="Close button Icon" class="hclose-btn" />  
            </asp:LinkButton>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlMain" runat="server">
        <%-- <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwWorkHistory" runat="server">
                <div class="row hvm-history" >
                        <uc:WorkHistory ID="ucWorkHistory" runat="server" />
                </div>
            </asp:View>
        </asp:MultiView>--%>
        <div>
            <table style="width: 100%" id="dataTable_WorkHistoryDet" class="table-responsive table-striped table-bordered" cellspacing="0">
                <thead>
                    <tr>
                        <th class="datatable_thead_td">Operation</th>
                        <th class="datatable_thead_td">Current Employer</th>
                        <th class="datatable_thead_td">
                            <div>Practice/</div>
                            <div class="datatable-min-width120">Employer Name</div>
                        </th>
                        <th class="datatable_thead_td datatable-min-width120">Start Date</th>
                        <th class="datatable_thead_td datatable-min-width120">End Date</th>
                        <th class="datatable_thead_td datatable-min-width180">Organization Name</th>
                        <th class="datatable_thead_td datatable-min-width120">Address1</th>
                        <th class="datatable_thead_td datatable-min-width120">Address2</th>
                        <th class="datatable_thead_td datatable-min-width120">City</th>
                        <th class="datatable_thead_td datatable-min-width120">State</th>
                        <th class="datatable_thead_td datatable-min-width120">County</th>
                        <th class="datatable_thead_td datatable-min-width120">Zip</th>
                        <th class="datatable_thead_td datatable-min-width180">Phone Number1</th>
                        <th class="datatable_thead_td datatable-min-width120">Phone Ext1</th>
                        <th class="datatable_thead_td datatable-min-width150">Fax Number1</th>
                        <th class="datatable_thead_td datatable-min-width150">Fax Number2</th>
                        <th class="datatable_thead_td datatable-min-width150">Contact Name</th>
                        <th class="datatable_thead_td datatable-min-width150">Email Address1</th>
                        <th class="datatable_thead_td datatable-min-width150">Email Address2</th>
                        <th class="datatable_thead_td datatable-min-width200">Additional Information</th>
                        <th class="datatable_thead_td datatable-min-width200">Reason for Departure</th>
                        <th class="datatable_thead_td">
                            <div class="datatable-min-width180">Are you currently</div>
                            <div class="datatable-min-width180">on active miltary duty</div>
                            <div class="datatable-min-width180">or miltary reserve?</div>
                        </th>
                        <th class="datatable_thead_td  datatable-min-width120">User Name</th>
                        <th class="datatable_thead_td  datatable-min-width180">Update Date</th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divWorkHistoryData" class="cwhd-table-container"></div>
        <div style="padding: 0 20px; background: #ffffff">
            <div class="row">
                <div class="col-sm-7">
                    <div id="divWorkHistoryNoData"></div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="btnBox">
        <asp:Button runat="server" ID="btnCancel" Text="OK" CssClass="historyButtonBox" CausesValidation="false" />
        <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="historyButtonBox" CausesValidation="false" OnClick="btnExcel_Click" />
    </div>
    <div class="result-Container">
        <div id="output"></div>
    </div>

</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
<asp:HiddenField ID="hidID" runat="server" Visible="false" />
<asp:HiddenField ID="hdnWorkHistoryRegAddressId" runat="server" />
<asp:HiddenField ID="hdnAccessToken" runat="server" />
<asp:HiddenField ID="hdnPDMSWebAPI" runat="server" />
<asp:HiddenField ID="hdnRedirectToNewWorkHistory" runat="server" />
<asp:HiddenField ID="hdnWrkHistoryRegId" runat="server" />
<div style="display: none;">
    <telerik:radgrid id="grdWorkHistory" width="100%" runat="server" enableariasupport="true" pagesize="10" alwaysvisible="true" virtualitemcount="0"
        allowpaging="false" allowsorting="false" allowfilteringbycolumn="false" filtertype="Classic" skin="PDMSModern">
        <exportsettings ignorepaging="true" openinnewwindow="true" exportonlydata="true">
            <excel format="Biff" />
        </exportsettings>
        <mastertableview autogeneratecolumns="False" datakeynames="">
            <columns>
                <telerik:gridboundcolumn datafield="OPERATION" headertext="Operation" visible="true" />
                <telerik:gridboundcolumn datafield="IS_CURRENT_EMPLOYER" headertext="Current Employer" visible="true" />
                <telerik:gridboundcolumn datafield="PRACTICE_NAME" headertext="Practice/Employer Name" visible="true" />
                <telerik:gridboundcolumn datafield="WORKED_FROM" headertext="Start Date" visible="true" dataformatstring="{0:MM/dd/yyyy}" />
                <telerik:gridboundcolumn datafield="WORKED_TO" headertext="End Date" visible="true" dataformatstring="{0:MM/dd/yyyy}" />
                <telerik:gridboundcolumn datafield="ADDRESS1" headertext="Address 1" visible="true" />
                <telerik:gridboundcolumn datafield="ADDRESS2" headertext="Address 2" visible="true" />
                <telerik:gridboundcolumn datafield="CITY" headertext="City" visible="true" />
                <telerik:gridboundcolumn datafield="STATE" headertext="State" visible="true" />
                <telerik:gridboundcolumn datafield="ZIP" headertext="ZIP" visible="true" />
                <telerik:gridboundcolumn datafield="CONTACT_PHONE" headertext="Phone" visible="true" />
                <telerik:gridboundcolumn datafield="CONTACT_PHONE_EXT" headertext="Phone Ext" visible="true" />
                <telerik:gridboundcolumn datafield="FAX1" headertext="Fax 1" visible="true" />
                <telerik:gridboundcolumn datafield="FAX2" headertext="Fax 2" visible="true" />
                <telerik:gridboundcolumn datafield="CONTACT_NAME" headertext="Contact" visible="true" />
                <telerik:gridboundcolumn datafield="CONTACT_EMAIL_ADDRESS" headertext="Email 1" visible="true" />
                <telerik:gridboundcolumn datafield="CONTACT_EMAIL_ADDRESS_2" headertext="Email 2" visible="true" />
                <telerik:gridboundcolumn datafield="ADDITIONAL_INFO" headertext="Additional Information" visible="true" />
                <telerik:gridboundcolumn datafield="REASON_FOR_DEPARTURE" headertext="Reason for Departure" visible="true" />
                <telerik:gridboundcolumn datafield="CONTACT_EMAIL_ADDRESS_2" headertext="Email 2" visible="true" />
                <telerik:gridboundcolumn datafield="MILITARY_RESERVE" headertext="Active Duty or Military Reserve" visible="true" />
                <telerik:gridboundcolumn datafield="username" headertext="User Name" visible="true" />
                <telerik:gridboundcolumn datafield="DateOfAction" headertext="Update Date" visible="true" dataformatstring="{0:MM/dd/yyyy hh:mm:ss}" />
            </columns>
        </mastertableview>
    </telerik:radgrid>
</div>
