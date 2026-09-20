<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Correspondence, App_Web_av5ll3zk" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register src="~/PopupControls/Separator.ascx" tagname="SectHd" tagprefix="uc1" %>
<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>
<%@ register assembly="SCS.WebControls.GroupBox" namespace="SCS.WebControls" tagprefix="cc1" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>
<%@ register src="~/PopupControls/MessageBox.ascx" tagname="MessageBox" tagprefix="cc2" %>


<script type="text/javascript">
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day earlier than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
    function pageLoad() {
        $('[data-toggle="popover"]').popover()
    }
    function SelectAll(id) {
        var frm = document.forms[0];
        for (i = 0; i < frm.elements.length; i++) {
            var thisName = frm.elements[i].name;
            if (frm.elements[i].type == "checkbox" && thisName.indexOf("chkSelect") != -1) {
                frm.elements[i].checked = document.getElementById(id).checked;
            }
        }
    }

    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }

    function correspondenceTypeChange() {
        document.getElementById("<%= txtDateAvailableFrom.ClientID%>").value = ''
        document.getElementById("<%= txtDateAvailableTo.ClientID%>").value = ''
        $("#<%= gvCorrespondencesearch.ClientID%>").remove();
        $("#<%= gvConvertedCorrespondencesearch.ClientID%>").remove();

        var endDate = new Date();
        $("#<%= calExtDateAvailableFrom.ClientID%>").datepicker("destroy");
        $("#<%= calExtDateAvailableTo.ClientID%>").datepicker("destroy");

        $("#<%= calExtDateAvailableFrom.ClientID%>").datepicker("refresh");
        $("#<%= calExtDateAvailableTo.ClientID%>").datepicker("refresh");

        $("#<%= calExtDateAvailableFrom.ClientID%>").datepicker('option', 'maxDate', endDate);
        $("#<%= calExtDateAvailableTo.ClientID%>").datepicker('option', 'maxDate', endDate);

    }

    function printPartOfPage(elementId) {
        var printContent = document.getElementById(elementId);
        var windowUrl = '';
        var uniqueName = new Date();
        var windowName = '';
        var printWindow = window.open('left=50000,top=50000,width=0,height=0');
        printWindow.document.write(printContent.innerHTML);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }
    function btnSearch_Click() {
        var selectedOption = document.getElementById("ddlCorrespondenceType").value;
        if (selectedOption === "") {
            document.getElementById("requiredMessage").style.display = "block";
            return false; // Prevent form submission
        }
    }


</script>
<style type="text/css">
    .headRowSortable {
        background: url('../App_Themes/Default/Grid/SortAsc.gif') left center no-repeat;
        background-size: contain;
        text-decoration: none;
        vertical-align: central;
    }
</style>




<div class="row col-sm-13" style="border: groove; margin-left: 10px">

    <ajax:collapsiblepanelextender id="cpeCorrespondence" runat="server" collapsed="false" targetcontrolid="pnlCorrespondence"
        expandcontrolid="pnlsepCorrespondence" collapsecontrolid="pnlSepCorrespondence" />
    <asp:panel runat="server" id="pnlSepCorrespondence" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" style="background-color: cornflowerblue;" tooltip="Click to Expand/Collapse">
        <h1><span id="sepCorrespondence" runat="server" class="pageHeader">- *  SEARCH CORRESPONDENCE</span></h1>
    </asp:panel>
    <span id="requiredMessage" role="alert" aria-live="assertive" style="color: #CC0505; font-size: 14pt !important; font-weight: 100 !important">An asterisk * indicates a required field</span>
    <%--    <asp:Panel runat="server" ID="pnlInstructions" class="OwnerBackground">--%>
    <asp:panel id="pnlCorrespondence" runat="server" style="min-height: 340px; min-width: 300px; height: 238px; width: auto; max-width: 1200px;">
        <div class="row">
            <asp:validationsummary id="valSummary" runat="server" displaymode="List" style="margin-left: 20px;" validationgroup="valCorrespondence" showsummary="true" cssclass="failureNotification" />

            <div id="CorrespondenceType" class="col-sm-5">
                <span class="ohio-field-label" style="margin-top: 8px;"><span style="color: red">*</span>Correspondence TYPE
                          <%--  <asp:Label ID="lblAttDocType" runat="server" Text="Document Type" CssClass="formLabel200" />--%>
                    <asp:dropdownlist id="ddlCorrespondenceType" aria-label="*Correspondence TYPE" cssclass="formField" style="margin-top: 8px;" enableviewstate="true" runat="server" onchange="correspondenceTypeChange()"
                        appenddatabounditems="True">
                    </asp:dropdownlist>

                    <span style="color: red; display: none">Correspondence Type is required</span>
                </span>

            </div>
            <div class="col-sm-3">
                <asp:label id="Label2" cssclass="ohio-field" associatedcontrolid="txtDateAvailableFrom" runat="server">
                    <span class="ohio-field-label">Date Available From: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right"
                        aria-hidden="true"></span>
                    </span>
                    <span class="input-group">
                        <ajax:calendarextender
                            id="calExtDateAvailableFrom"
                            runat="server"
                            format="MM/dd/yyyy"
                            targetcontrolid="txtDateAvailableFrom"
                            popupposition="BottomLeft"
                            cssclass=""
                            popupbuttonid=""
                            enabledonclient="true" />
                        <asp:textbox id="txtDateAvailableFrom" cssclass="ohio-field-input" runat="server" autocomplete="off" />
                        <asp:comparevalidator
                            id="CompareValidator1"
                            runat="server"
                            type="Date"
                            operator="DataTypeCheck"
                            controltovalidate="txtDateAvailableFrom"
                            errormessage="Select a valid PNM Date Available From"
                            display="Dynamic"
                            valuetocompare="MM/dd/yyyy"
                            validationgroup="valCorrespondence"
                            style="position: absolute"
                            setfocusonerror="true">
                        </asp:comparevalidator>
                        <span class="input-group-addon">
                            <span class="fa-date" aria-hidden="true"></span>
                        </span>
                    </span>
                </asp:label>
            </div>
            <div class="col-sm-3">
                <asp:label id="lblDateAvailableTo" cssclass="ohio-field" associatedcontrolid="txtDateAvailableTo" runat="server">
                    <span class="ohio-field-label">Date Available To: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right"
                        aria-hidden="true"></span>
                    </span>
                    <span class="input-group">
                        <ajax:calendarextender
                            id="calExtDateAvailableTo"
                            runat="server"
                            format="MM/dd/yyyy"
                            targetcontrolid="txtDateAvailableTo"
                            popupposition="BottomLeft"
                            cssclass=""
                            popupbuttonid=""
                            enabledonclient="true" />
                        <asp:textbox id="txtDateAvailableTo" cssclass="ohio-field-input" runat="server" autocomplete="off" />
                        <asp:comparevalidator
                            id="cvDateAvailableTo"
                            runat="server"
                            type="Date"
                            operator="DataTypeCheck"
                            controltovalidate="txtDateAvailableTo"
                            errormessage="Select a valid PNM Date Available To"
                            display="Dynamic"
                            valuetocompare="MM/dd/yyyy"
                            style="position: absolute"
                            validationgroup="valCorrespondence"
                            setfocusonerror="true">
                        </asp:comparevalidator>
                        <span class="input-group-addon">
                            <span class="fa-date" aria-hidden="true"></span>
                        </span>
                    </span>
                    <asp:comparevalidator
                        id="cvDateRange"
                        runat="server"
                        type="Date"
                        operator="LessThanEqual"
                        forecolor="red"
                        controltovalidate="txtDateAvailableFrom"
                        controltocompare="txtDateAvailableTo"
                        errormessage="Date Available To should always be greater than Date Available From"
                        display="Dynamic"
                        style="position: absolute"
                        validationgroup="valCorrespondence"
                        setfocusonerror="true">
                    </asp:comparevalidator>
                </asp:label>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-sm-11">
                <div style="float: right; margin-right: 45px;">
                    <asp:updatepanel id="UpdatePanel1" runat="server" updatemode="Conditional">
                        <contenttemplate>
                            <asp:button id="btnSearch" runat="server" causesvalidation="true" text="Search" cssclass="buttonBoxFocus" onclick="btnSearch_Click" validationgroup="valProviderHeader" onclientclick="showProgress()" />
                            <asp:button id="btnClear" runat="server" causesvalidation="false" text="Clear" cssclass="buttonBox" onclick="btnClear_Click" onclientclick="showProgress()" />

                        </contenttemplate>
                        <triggers>
                            <asp:postbacktrigger controlid="btnSearch" />
                            <asp:postbacktrigger controlid="btnClear" />
                        </triggers>
                    </asp:updatepanel>
                </div>
            </div>
            <div class="col-sm-1">
            </div>
        </div>


        <asp:updateprogress id="UpdateProgress" runat="server" displayafter="1">
            <progresstemplate>
                <div style="padding-right: 30px">
                    <img src="../Images/ajax-loader.gif" alt="" />
                    Loading ...
                </div>
            </progresstemplate>
        </asp:updateprogress>
    </asp:panel>
    <ajax:collapsiblepanelextender id="cpeCorrespondencetype" runat="server" collapsed="false" targetcontrolid="pnlCorrespondencetype" expandcontrolid="pnlSepCorrespondencetype" collapsecontrolid="pnlSepCorrespondencetype" />
    <asp:panel runat="server" id="pnlsepCorrespondencetype" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" style="background-color: cornflowerblue;" tooltip="Click to Expand/Collapse">
        <h2><span id="sepCorrespondencetype" runat="server" class="pageHeader">- CORRESPONDENCE SEARCH RESULT</span></h2>
    </asp:panel>
    <asp:panel runat="server" id="pnlCorrespondencetype">
        <div style="width: 100%; text-align: right">
            <asp:hiddenfield id="hdnRowCount" runat="server" />
            <telerik:radgrid id="RadGridExport" runat="server" visible="true">
                <exportsettings ignorepaging="true" openinnewwindow="true">
                    <pdf pageheight="8.5in" pagewidth="11in" pagetitle="CORRESPONDENCE SEARCH RESULT">
                        <pagefooter>
                            <rightcell text="Page <?page-number?>" />
                        </pagefooter>
                    </pdf>
                </exportsettings>
                <mastertableview autogeneratecolumns="false">
                    <columns>
                        <telerik:gridboundcolumn uniquename="col1" headertext="Correspondence Subject" datafield="SUBJECT"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col2" headertext="Correspondence Type" datafield="CORRESPODENCE_TYPE"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col3" headertext="Prior Authorization Number" datafield="PA_NUMBER" Visible="false"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col3" headertext="Hospice Tracking Number" datafield="HTN_NUMBER" Visible="false"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col4" headertext="Recipient ID" datafield="RECIPIENT_ID" Visible="false"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col5" headertext="Date Sent" datafield="DATE_SENT"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col6" headertext="Dave Viewed" datafield="DATE_VIEWED"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col7" headertext="Printed"></telerik:gridboundcolumn>
                    </columns>
                </mastertableview>
            </telerik:radgrid>


            <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
                <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10,000 results from the search will be exported.</p>
            </div>
        </div>
        <asp:gridview
            id="gvCorrespondencesearch"
            runat="server"
            autogeneratecolumns="False"
            cssclass="gridViewSmallFont" width="100%"
            allowsorting="true"
            emptydatatext="No Email found."
            onrowdatabound="gvCorrespondencesearch_RowDataBound"
            onrowcommand="gvCorrespondencesearch_RowCommand"
            onpageindexchanging="gvCorrespondencesearch_PageIndexChanging"
            onsorting="gvCorrespondencesearch_Sorting"
            currentsortfield="DATE_SENT"
            currentsortdirection="DESC"
            rowstyle-verticalalign="Top"
            allowpaging="True"
            pagesize="10"
            datakeynames="COMMUNICATION_EVENT_ID, SUBJECT,CORRESPODENCE_TYPE, PA_NUMBER, HTN_NUMBER, RECIPIENT_ID, DATE_SENT,DATE_VIEWED, BODY, EMAIL_TO, DOCUMENT_ID, ELIGIBILITY, DOCUMENT_TYPE, DOCUMENT_ID_PDF,  ONBASE_DOCUMENT_ID,FILE_NAME_PDF, FILE_NAME, DOCUMENT_ATTACHMENT_XREF_ID">
            <columns>
                <asp:templatefield headertext="Correspondence Subject" sortexpression="SUBJECT">
                    <itemtemplate>
                        <asp:LinkButton
                            ID="LnkButtonEnrollment"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# Eval("DOCUMENT_ID").ToString() == "" ? true : false %>'
                            CommandName="OpenEnrollment"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadpdf1"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# Eval("document_type").ToString() == "pdf" ? true : false %>'
                            CommandName="OnFileDownloadPDF"
                            OnClientClick="openInNewTab();"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadpdfReal"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# Eval("document_type").ToString() == "zip" ? true : false %>'
                            CommandName="OnFileDownloadPDF_Zip"
                            OnClientClick="openInNewTab();"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadpdf"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# (Eval("document_type").ToString() == "zip" && Eval("DOCUMENT_ID_PDF").ToString() != "0") ? true : false %>'
                            CommandName="OnFileDownloadPDF"
                            OnClientClick="openInNewTab();"
                            Text='pdf'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadNonPdf"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# (Eval("DOCUMENT_ID").ToString() != "" && Eval("document_type").ToString() != "zip" && Eval("document_type").ToString() != "pdf") ? true : false %>'
                            CommandName="OnFileDownload"
                            OnClientClick="openInNewTab();"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                    </itemtemplate>
                </asp:templatefield>
                <asp:boundfield datafield="CORRESPODENCE_TYPE" headertext="Correspondence Type" sortexpression="CORRESPODENCE_TYPE" />
                <asp:boundfield datafield="PA_NUMBER" headertext="Prior Authorization Number" sortexpression="PA_NUMBER" Visible="false" />
                <asp:boundfield datafield="HTN_NUMBER" headertext="Hospice Tracking Number" sortexpression="HTN_NUMBER" Visible="false" />
                <asp:boundfield datafield="RECIPIENT_ID" headertext="Recipient ID" sortexpression="RECIPIENT_ID" Visible="false" />
                <asp:boundfield datafield="DATE_SENT" headertext="Date Sent" sortexpression="DATE_SENT" />
                <asp:boundfield datafield="DATE_VIEWED" headertext="Date Viewed" sortexpression="DATE_VIEWED" />



            </columns>
        </asp:gridview>
    </asp:panel>


    <ajax:collapsiblepanelextender id="cpeConvertedCorrespondence" runat="server" collapsed="false" targetcontrolid="pnlConvertedCorrespondence" expandcontrolid="pnlSepConvertedCorrespondence" collapsecontrolid="pnlSepConvertedCorrespondence" />
    <asp:panel runat="server" id="pnlsepConvertedCorrespondence" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" style="background-color: cornflowerblue;" tooltip="Click to Expand/Collapse">
        <h2><span id="sepConvertedCorrespondence" runat="server" class="pageHeader">- CONVERTED CORRESPONDENCE</span></h2>
    </asp:panel>
    <asp:panel runat="server" id="pnlConvertedCorrespondence">
        <div style="width: 100%; text-align: right">
            <asp:hiddenfield id="hdnROwncont2" runat="server" />
            <telerik:radgrid id="RadGridExport1" runat="server" visible="true">
                <exportsettings ignorepaging="true" openinnewwindow="true">
                    <pdf pageheight="8.5in" pagewidth="11in" pagetitle="Converted Correspondence">
                        <pagefooter>
                            <rightcell text="Page <?page-number?>" />
                        </pagefooter>
                    </pdf>
                </exportsettings>
                <mastertableview autogeneratecolumns="false">
                    <columns>
                        <telerik:gridboundcolumn uniquename="col1" headertext="Correspondence Subject" datafield="DOCUMENT_SERVICE_NAME"></telerik:gridboundcolumn>

                    </columns>
                </mastertableview>
            </telerik:radgrid>


            <div id="divRowCount2" title="High Row Count" style="display: none; text-align: center">
                <p style="color: darkgreen" id="p1" runat="server">Only the first 10,000 results from the search will be exported.</p>
            </div>
        </div>
        <div id="LoadingPanel" style="display: none; color: red; text-align: center;">Converted Correspondence...</div>
        <mms:sortablepaginggridview
            id="gvConvertedCorrespondencesearch"
            runat="server"
            autogeneratecolumns="False"
            cssclass="gridViewSmallFont" width="100%"
            allowsorting="true"
            emptydatatext="No Converted Correspondence found."
            onrowcommand="gvConvertedCorrespondencesearch_RowCommand"
            onpageindexchanging="gvConvertedCorrespondencesearch_PageIndexChanging"
            onsorting="gvConvertedCorrespondencesearch_Sorting"
            currentsortfield="DOCUMENT_TYPE_DESC"
            currentsortdirection="DESC"
            rowstyle-verticalalign="Top"
            allowpaging="True"
            pagesize="10"
            datakeynames="DOCUMENT_TYPE_DESC,BODY,EMAIL_TO,DOCUMENT_ID,ELIGIBILITY,DATE_VIEWED">
            <columns>
                <asp:templatefield headertext="Correspondence Subject" sortexpression="DOCUMENT_TYPE_DESC">
                    <itemtemplate>
                        <asp:linkbutton id="lbtnSubject" runat="server" commandname="subject" commandargument="<%# ((GridViewRow) Container).RowIndex %>" text='<%# Eval("DOCUMENT_TYPE_DESC") %>'></asp:linkbutton>
                    </itemtemplate>
                </asp:templatefield>

            </columns>
        </mms:sortablepaginggridview>
    </asp:panel>


    <cc2:messagebox id="MessageBox2" runat="server" />

</div>
<ajax:modalpopupextender id="mpeEmailPreview" runat="server" popupcontrolid="pnlPreview"
    targetcontrolid="ButtonDummy" backgroundcssclass="modalBackground" cancelcontrolid="btnCancel">
</ajax:modalpopupextender>
<asp:panel id="pnlPreview" runat="server" cssclass="modalPopup" style="margin-right: 5% !important;">
    <asp:panel id="pnlHeader" cssclass="popHeader" runat="server">
        <table style="width: 100%; cursor: pointer">
            <tr>
                <td>
                    <div class="popTitle">Provider Communication</div>
                </td>
                <td style="text-align: right">
                    <asp:imagebutton id="imgClose" runat="server" imageurl="~/Images/cancel.png" onclick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:panel>

    <asp:updatepanel id="upPreview" runat="server" updatemode="Conditional" style="margin-left: auto; margin-right: auto;">
        <contenttemplate>
            <div id="divPreview" style="overflow: scroll; height: 500px; padding-right: 50px;">
                <table id="tblPreview" style="padding-top: 20px; width: 100%;">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            <%-- <asp:Label ID="lblSubject" runat="server" CssClass="formLabel300" Text="Subject" AssociatedControlID="txtSubject" /></td>--%>
                        <td>
                            <asp:textbox id="txtSubject" runat="server" cssclass="formField" readonly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--                 <asp:Label ID="lblSendTo" runat="server" CssClass="formLabel300" Text="Send To" AssociatedControlID="lboxSendTo" /></td>
                            --%>
                        <td>
                            <asp:listbox id="lboxSendTo" runat="server" cssclass="formField" selectionmode="Multiple" />
                            <asp:customvalidator id="cvSendTo" runat="server" controltovalidate="lboxSendTo" onservervalidate="SendToRequired" display="Static"
                                validationgroup="Emails" errormessage="* Select at least one Send To email address." text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--    <asp:Label ID="lblBody" runat="server" CssClass="formLabel300" Text="Body" AssociatedControlID="txtBody" /></td>--%>
                        <td>
                            <asp:textbox id="txtBody" text="txt" runat="server" cssclass="formField" readonly="true" textmode="MultiLine" columns="100" rows="20" />
                            <div runat="server" id="divBody"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:label id="lblAttach" runat="server" cssclass="formLabel300" text="Attachment" />
                        </td>
                        <td>
                            <asp:panel id="pnlAttachments" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="btnBox">
                <asp:button id="btnPrint" runat="server" text="Print" cssclass="buttonBoxFocus" onclientclick="javascript:printPartOfPage('divPreview');" />
                <asp:button id="btnCancel" runat="server" causesvalidation="false" text="Close" cssclass="buttonBox" onclick="btnCancel_Click" style="margin-right: 20px; margin-bottom: 15px !important;" />
            </div>
        </contenttemplate>
    </asp:updatepanel>
</asp:panel>
<asp:button runat="server" id="ButtonDummy" style="display: none" />

<%--  GridViewSortColumn="" GridViewSortDirection="Descending"--%>