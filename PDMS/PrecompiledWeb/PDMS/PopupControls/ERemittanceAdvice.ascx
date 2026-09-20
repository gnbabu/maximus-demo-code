<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ERemittanceAdvice, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%--<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>--%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>

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
    function exportPopup() {
        var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
        if (rowCount > 10000) {
            $("#divRowCount").dialog();
        }
    }

 //   $(document).ready(function(){
	//	$('#btnclear').click(function(){				
	//		if(confirm("Want to clear?")){
	//			/*Clear all input type="text" box*/
	//			$('#ranum input[type="text"]').val('');
	//			/*Clear textarea using id */
	//			$('#ranum #nbRANumber').val('');
	//		}					
	//	});
	//});

    function alphanumericOnly(obj) {
            obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
    function numericOnly(obj) {
            obj.value = obj.value.replace(/[^0-9- ]/g, '');
    }
    function CollapseExpandRemittanceSearch() {
        var button = document.getElementById("remSearch").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("remSearch").addClass("panelHeaderStyle");
        } else {
            button = "true"
            $("remSearch").addClass("panelHeaderStyle");
        }
        document.getElementById("remSearch").setAttribute("aria-expanded", button);
    };
    function CollapseExpandRemittanceSearchResult() {
        var button = document.getElementById("remSearchResult").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("remSearchResult").addClass("panelHeaderStyle");
        } else {
            button = "true"
            $("remSearchResult").addClass("panelHeaderStyle");
        }
        document.getElementById("remSearchResult").setAttribute("aria-expanded", button);
    };

    function LoadGrid() {
        $("#divsearchEliginilityScroll").find("li a").attr("tabindex", "-1");
        $("#remSearch").attr("tabindex", "-1");
        $("#ctl00_MainContent_ERemittanceAdvice_ddlPrimaryDestinationPayer").attr("tabindex", "-1");
        $("#ctl00_MainContent_ERemittanceAdvice_txtRANumber").attr("tabindex", "-1");
        $("#ctl00_MainContent_ERemittanceAdvice_txtDateAvailableFrom").attr("tabindex", "-1");
        $("#ctl00_MainContent_ERemittanceAdvice_txtDateAvailableTo").attr("tabindex", "-1");
        $("#ctl00_MainContent_ERemittanceAdvice_btnSearch").attr("tabindex", "-1");
        $("#ctl00_MainContent_ERemittanceAdvice_btnClear").attr("tabindex", "-1");
        $("#ctl00_MainContent_ERemittanceAdvice_ddlPageSize").attr("tabindex", "-1");
    }

    $(document).ready(function () {
        $('.setTab').keydown(function (e) {
            if (e.keyCode == 9) {
                $("#divsearchEliginilityScroll").find("li a").attr("tabindex", "0");
                $("#remSearch").attr("tabindex", "0");
                $("#ctl00_MainContent_ERemittanceAdvice_ddlPrimaryDestinationPayer").attr("tabindex", "0");
                $("#ctl00_MainContent_ERemittanceAdvice_txtRANumber").attr("tabindex", "0");
                $("#ctl00_MainContent_ERemittanceAdvice_txtDateAvailableFrom").attr("tabindex", "0");
                $("#ctl00_MainContent_ERemittanceAdvice_txtDateAvailableTo").attr("tabindex", "0");
                $("#ctl00_MainContent_ERemittanceAdvice_btnSearch").attr("tabindex", "0");
                $("#ctl00_MainContent_ERemittanceAdvice_btnClear").attr("tabindex", "0");
                $("#ctl00_MainContent_ERemittanceAdvice_ddlPageSize").attr("tabindex", "0");
            }
        });
    });
    function btnSearch_Click() {
        var selectedOption = document.getElementById("ddlPrimaryDestinationPayer").value;
        if (selectedOption === "") {
            document.getElementById("errorMessagesearchRA").style.display = "block";
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

.ajax__calendar_container { z-index : 1000 ; }

.layoutTable {
    display: table;
}
.layoutRow {
    display: table-row;
   vertical-align: middle;
}
.layoutCell, .layoutTableHead {
   display: table-cell;
   width: auto;
   padding: 3px 10px;
}

.errorclass{
    color:#D33421;
}

.panelHeaderStyle{
    background-color: #2297bc;
    border-style:none;
}
</style>




<div>
    <ajax:CollapsiblePanelExtender ID="cpeInstructions" runat="server" Collapsed="false" TargetControlID="pnlInstructions" ExpandControlID="pnlSepInstructions" CollapseControlID="pnlSepInstructions" />
    <asp:Panel runat="server" ID="pnlSepInstructions" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"  Style="background-color: #2297bc;" ToolTip="Click to Expand/Collapse" CssClass="OwnerNDCDetails">
        <h1><span id="sepInstructions" runat="server" class="pageHeader pH2" Style="color:white"><button type="button" class="panelHeaderStyle" tabindex="0" id="remSearch" aria-expanded="true" onclick="CollapseExpandRemittanceSearch()">* REMITTANCE ADVICE SEARCH</button></span></h1>
    </asp:Panel>
    <asp:Panel ID="pnlInstructions" runat="server">
        <div>
            <div>
                 <span style="color:#D33421 ; font-size: 14pt !important; font-weight:100 !important" ;="">An asterisk * indicates a required field</span>
                 <span id="errorMessagesearchRA" role="alert" aria-live="assertive"><asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="valERemittanceAdvice" ShowSummary="true"  CssClass="failureNotification" class="errorclass" /></span>
                <div style="padding: 3px 10px;">                   
                    <asp:Label ID="payerLabel" AssociatedControlID="ddlPrimaryDestinationPayer" runat="server"> <span class="errorclass">* </span>Payer</asp:Label>
                    <div>
                        <asp:DropDownList id="ddlPrimaryDestinationPayer" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="layoutTable" role="presentation">
            <div class="layoutRow">
                <div class="layoutCell" style="width: 175px;">
                    <asp:Label ID="Label1" AssociatedControlID="txtRANumber" runat="server">RA Number</asp:Label>
                    <asp:TextBox ID="txtRANumber" CssClass="ohio-field-input" runat="server" Width="175" MaxLength="50"  onKeyUp="javascript:alphanumericOnly(this)"  />
             
                </div>
                <div class="layoutCell" style="width: 175px;">
                    <asp:Label ID="Label3" AssociatedControlID="txtICN" runat="server">ICN</asp:Label>
                    <asp:TextBox ID="txtICN" CssClass="ohio-field-input" runat="server" Width="175" MaxLength="50"  onKeyUp="javascript:alphanumericOnly(this)"  />
				</div>
                <div class="layoutCell" style="width: 175px;">
                    <asp:Label ID="Label2" AssociatedControlID="txtDateAvailableFrom" runat="server">Report Run Date From:</asp:Label>
                    <span class="input-group">
                        <ajax:CalendarExtender ID="calExDateAvaliableFrom" runat="server"
                            Format="MM/dd/yyyy"
                            TargetControlID="txtDateAvailableFrom"
                            PopupPosition="BottomLeft"
                            CssClass=""
                            PopupButtonID=""
                            EnabledOnClient="true" />
                        <asp:TextBox ID="txtDateAvailableFrom" CssClass="ohio-field-input" runat="server" Width="150" aria-label="Report Run Date From: MM/DD/YYYY" TabIndex="0" />
                        <asp:CompareValidator
                            ID="CompareValidator1"
                            runat="server"
                            Type="Date"
                            Operator="DataTypeCheck"
                            ControlToValidate="txtDateAvailableFrom"
                            ErrorMessage="Select a valid PNM Date Available From"
                            Display="Dynamic"
                            ValueToCompare="MM/dd/yyyy"
                            ValidationGroup="valERemittanceAdvice"
                            Style="position: absolute"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                        <span class="input-group-addon">
                            <span class="fa-date" aria-hidden="true" onclick="document.getElementById('ctl00_MainContent_ERemittanceAdvice_txtDateAvailableFrom').focus();"></span>
                        </span>
                    </span>
                </div>
                <div class="layoutCell" style="width: 175px;">
                    <asp:Label ID="lblDateAvailableTo" AssociatedControlID="txtDateAvailableTo" runat="server">To Date</asp:Label>
                    <span class="input-group">
                        <ajax:CalendarExtender
                            ID="CalendarExtender2"
                            runat="server"
                            Format="MM/dd/yyyy"
                            TargetControlID="txtDateAvailableTo"
                            PopupPosition="BottomLeft"
                            CssClass=""
                            PopupButtonID=""
                            EnabledOnClient="true" />
                        <asp:TextBox ID="txtDateAvailableTo" CssClass="ohio-field-input" runat="server" Width="150" />
                        <asp:CompareValidator
                            ID="cvDateAvailableTo"
                            runat="server"
                            Type="Date"
                            Operator="DataTypeCheck"
                            ControlToValidate="txtDateAvailableTo"
                            ErrorMessage="Select a valid PNM Date Available To"
                            Display="Dynamic"
                            ValueToCompare="MM/dd/yyyy"
                            Style="position: absolute"
                            ValidationGroup="valERemittanceAdvice"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                        <span class="input-group-addon">
                            <span class="fa-date" aria-hidden="true" onclick="document.getElementById('ctl00_MainContent_ERemittanceAdvice_txtDateAvailableTo').focus();"></span>
                        </span>
                    </span>
                </div>
                <div class="layoutCell" style="text-align: center;">
                    <div style="width: 250px; text-align: center;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="valProviderHeader"  />
                                <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSearch" />
                                <asp:PostBackTrigger ControlID="btnClear" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div>
                            <asp:Label runat="server" AssociatedControlID="ddlPageSize" ID="lblPageSize">Max Records</asp:Label>
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed" CssClass="unsetPublicSearchDDLLength">
                                <asp:ListItem Text="10" Value="10" />
                                <asp:ListItem Text="25" Value="25" />
                                <asp:ListItem Text="50" Value="50" />
                                <asp:ListItem Text="100" Value="100" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div> 
        <br/><br/><br/><br/>
        <div id="spacingSpacer" style="height: 100px;">&nbsp;</div>
        <asp:UpdateProgress ID="UpdateProgress" runat="server" DisplayAfter="1">
            <ProgressTemplate>
                <div style="padding-right: 30px">
                    <img src="../Images/ajax-loader.gif" alt="" />
                    Loading ... </div></ProgressTemplate></asp:UpdateProgress><%--</div>--%></asp:Panel>
   
    <ajax:CollapsiblePanelExtender ID="cpeOwnInfo" runat="server" Collapsed="false" TargetControlID="divERemittanceAdvicehHeader" ExpandControlID="pnlSepOwnInfo" CollapseControlID="pnlSepOwnInfo" />
    <asp:Panel runat="server" ID="pnlSepOwnInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc;" ToolTip="Click to Expand/Collapse">
        <h2><span id="sepOwnInfo" runat="server" class="pageHeader pH2" Style="color:white"><button type="button" class="panelHeaderStyle" tabindex="0" id="remSearchResult" aria-expanded="true" onclick="CollapseExpandRemittanceSearchResult()"> REMITTANCE ADVICE SEARCH RESULT</button></span></h2> </asp:Panel>
    <asp:Panel runat="server" ID="divERemittanceAdvicehHeader"  Style="height: auto; width: auto; max-width: 100%;">
        
            
                 <asp:GridView
                    ID="gvRemittanceAdvicesearch"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="gridViewSmallFont" Width="100%"
                    AllowSorting="true"
                    EmptyDataText="No Reports found."
                    OnPageIndexChanging="gvRemittanceAdvicesearch_PageIndexChanging"
                    OnRowCommand="gvRemittanceAdvicesearch_RowCommand"
                    OnSorting="gvRemittanceAdvicesearch_Sorting"
                    CurrentSortField="DOCUMENT_RECEIVED_DATE"
                    CurrentSortDirection="DESC"                    
                     OnRowDataBound="gvRemittanceAdvicesearch_RowDataBound"
                    RowStyle-VerticalAlign="Top"
                    AllowPaging="True"
                    PageSize="10"
                    DataKeyNames="DOCUMENT_ATTACHMENT_XREF_ID, FileName, DOCUMENT_ID,document_type, DOCUMENT_ID_PDF, FileNamePDF, UUID, remittance_advice_id"
                    caption="<span style='display:none'> REMITTANCE ADVICE SEARCH RESULT</span>">
                    <Columns>
                        <asp:BoundField DataField="INDEXID" HeaderText="RA Number" />
                        <asp:BoundField DataField="DOCUMENT_RECEIVED_DATE"  HeaderStyle-CssClass="setTab" HeaderText="Report Run Date" SortExpression="DOCUMENT_RECEIVED_DATE" />
                        <asp:TemplateField ShowHeader="False" HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="LnkButtonDownloadOTH"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    CommandName="OnFileDownload"
                                    Text="Download Report"
                                    CssClass="gridLink" />
                                <asp:LinkButton
                                    ID="LnkButtonDownloadpdf"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    Visible='<%# (Eval("document_type").ToString() == "txt" && Eval("DOCUMENT_ID_PDF").ToString() != "") ? true : false %>'
                                    CommandName="OnFileDownloadPDF"
                                    Text="pdf"
                                    CssClass="gridLink" />
                                <asp:LinkButton
                                    ID="LinkButton1"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    Visible='<%# (Eval("document_type").ToString() == "txt" && Eval("DOCUMENT_ID_PDF").ToString() == "") ? true : false %>'
                                    CommandName="OnFileDownloadPDF_Migra"
                                    Text="pdf"
                                    CssClass="gridLink" />
                                <asp:LinkButton
                                    ID="LnkButtonDownloadpdfReal"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    Visible='<%# Eval("document_type").ToString() == "zip" ? true : false %>'
                                    CommandName="OnFileDownloadPDF_Zip"
                                    Text="pdf"
                                    CssClass="gridLink" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
           
    </asp:Panel>
    <div style="width: 100%; text-align: right">
            <asp:HiddenField ID="hdnRowCount" runat="server" />
            <telerik:RadGrid ID="RadGridExport" runat="server" Visible="false">
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                    <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="ERemittance Advise Search Results">
                        <PageFooter>
                            <RightCell Text="Page <?page-number?>" />
                        </PageFooter>
                    </Pdf>
                </ExportSettings>
                <MasterTableView AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="col1" HeaderText="File Name" DataField="FileName"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="col2" HeaderText="Document Type" DataField="DocumentType"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="col3" HeaderText="Date Available" DataField="DateAvailable"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="col4" HeaderText="Date Downloaded" DataField="DateDownloaded"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:Label ID="lnkRowCount" Visible="false" runat="server" Style="float: left" CssClass="formLabelAuto" />
            <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false">
                <img src="../Images/Excel_24x24.png" alt="XLS" style="display: none;" />
            </asp:LinkButton>&nbsp;&nbsp; <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false">
                <img src="../Images/PDF_24x24.png" alt="PDF" style="display: none;" />
            </asp:LinkButton></div>
    <cc2:MessageBox ID="MessageBox2" runat="server" />
</div>