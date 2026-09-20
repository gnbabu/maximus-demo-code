<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Specialties"
    EnableViewState="true" Codebehind="Specialties.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/SpecialtiesHistory.ascx" TagPrefix="sh" TagName="SpecialtiesHistory" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script src="../Scripts/customizeTelerik.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).keydown(function (e) {
        // ESCAPE key pressed
        if (e.keyCode == 27) {
            $find("mpeSplHitory").hide();
            return false;
        }

        if (e.keyCode == 13) {
            // don't let them hit 'enter'; this saves the page and could be from them being in a filter and wanting the filter to get applied; make them click the filter button instead
            return false;
        }
    });

    $(document).ready(function () {
        function getFocusable(context = 'document') {
            return Array.from(context.querySelectorAll('button, [href], input:not([type="hidden"]), textarea, select, [tabindex]:not([tabindex="-1"])')).filter(function (el) { return !el.closest('[hidden]'); });
        }
        const $dialog = document.querySelector('#divDialog1');
        const focusableItems = getFocusable($dialog);

        document.addEventListener("keydown", function (e) {
            if (e.keyCode === 9) { // Tab & Shift+Tab                
                const focusedItem = e.target;
                const focusedItemIndex = focusableItems.indexOf(focusedItem);
                if (e.shiftKey) {
                    if (!$dialog.contains(e.target) || focusedItemIndex == 0) {
                        focusableItems[focusableItems.length - 1].focus();
                        //e.preventDefault();
                    }
                } else {
                    if (!$dialog.contains(e.target) || focusedItemIndex == focusableItems.length - 1) {
                        focusableItems[0].focus();
                        //e.preventDefault();
                    }
                }
            }
        });
    });

    function specialityChange(id) {
        var selectedText = id.options[id.selectedIndex].innerHTML;
        var selectedValue = id.value;
        var storage = window.sessionStorage;
        storage.setItem("isSpecialityChanged", "True");
        /*  alert("Selected Text: " + selectedText + " Value: " + selectedValue);*/
        return true;
    }

</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ValidationGroup="valSpecialties" CssClass="failureNotification" />
            </div>
            <div style="width: 100%;">
               <div class="row">
                   <div class="col-sm-9">
                       <div class="pg-hint6">
                           <span>
                               <asp:Label ID="lblORPHelptext" CssClass="failureNotification" runat="server" Visible="false" Text="This page is read only during the ORP conversion process." />

                           </span>
                       </div>
                   </div>
               </div>
            </div>
            <div style="width: 100%;">
                <div class="row">
                    <div class="col-sm-9">
                        <div class="pg-hint4">
                            <span>Primary Specialties are not editable by provider after application submission.</span>
                        </div>
                    </div>
                </div>
            </div>             

            <div style="width: 100%;">
                <div class="row">
                    <div class="col-sm-9">
                        <div class="pg-hint4">
                            <span>
                                <asp:Label ID="lblPrimaryValidation" CssClass="failureNotification" runat="server" Visible="false" Text="A new primary must be entered to proceed." /></span>
                        </div>
                    </div>
                </div>
            </div>

            <div id="pnlSpecialties" runat="server">
                <div class="divGrid">
                    <telerik:radgrid id="grdSpecialties" role="definition" aria-label="Specialties" tabindex="0" runat="server" width="100%" aria-busy="true" onitemcommand="grd_ItemCommand" onitemcreated="grd_ItemCreated"
                        onitemdatabound="grd_ItemDataBound" skin="PDMSModern" allowsorting="True"
                        allowfilteringbycolumn="true"
                        enableariasupport="true" enableembeddedskins="false">
                        <groupingsettings casesensitive="false" />
                        <clientsettings>
                            <resizing allowcolumnresize="true" />
                            <clientevents onfiltermenushowing="filterMenuShowing" />
                        </clientsettings>
                        <filtermenu onclientshowing="MenuShowing" cssclass="gridviewFilter" />
                        <mastertableview gridlines="None" datakeynames="REG_SPECIALTY_ID"
                            autogeneratecolumns="false">
                            <norecordstemplate>
                                No records found
                            </norecordstemplate>
                            <columns>
                                <telerik:gridcalculatedcolumn headertext="Specialty" sortexpression="MMIS_SPECIALTY_TYPE_ID" datafields="MMIS_SPECIALTY_TYPE_ID, SPECIALTY_TYPE_NAME" expression='{0} + " " + {1}'>
                                </telerik:gridcalculatedcolumn>

                                <telerik:gridtemplatecolumn datafield="PRIMARY_FLAG" headertext="Primary" uniquename="Primary" sortexpression="PRIMARY_FLAG" datatype="System.Boolean">
                                    <itemtemplate>
                                        <asp:Label ID="PrimaryFlagLbl" runat="server" Text='<%# Convert.ToBoolean(Eval("PRIMARY_FLAG")) == true ? "Yes" : "No" %>'></asp:Label>
                                    </itemtemplate>
                                </telerik:gridtemplatecolumn>

                                <telerik:gridboundcolumn datafield="START_DATE" headertext="Start Date" dataformatstring="{0:MM/dd/yyyy}" sortexpression="START_DATE" />

                                <telerik:gridboundcolumn datafield="END_DATE" headertext="End Date" dataformatstring="{0:MM/dd/yyyy}" sortexpression="END_DATE" />

                                <telerik:gridboundcolumn datafield="ENROLL_STATUS_DESC" headertext="Enroll Status" uniquename="EnrollStatus">
                                    <filtertemplate>
                                        <telerik:radcombobox id="RadComboBoxEnrollStatus" aria-label="Enroll Status" enableariasupport="true" datatextfield="ENROLL_STATUS_DESC" skin="PDMSModern"
                                            datavaluefield="ENROLL_STATUS_DESC" width="100%" enableviewstate="false"
                                            runat="server" onclientselectedindexchanged="EnrollStatusIndexChanged" enableembeddedskins="false">
                                        </telerik:radcombobox>
                                        <telerik:radscriptblock id="RadScriptEnrollStatus" runat="server">
                                            <script type="text/javascript">
                                                function EnrollStatusIndexChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    var filtervalue = args.get_item().get_value();
                                                    if (filtervalue == "All") filtervalue = "";
                                                    tableView.filter("EnrollStatus", filtervalue, "EqualTo");
                                                }
                                                $(document).ready(function () {
                                                    Input = $('.rcbReadOnly').children("td").children("input");
                                                    Input.attr('aria-label', 'Select Enroll Status');
                                                });
                                            </script>
                                        </telerik:radscriptblock>
                                    </filtertemplate>
                                </telerik:gridboundcolumn>

                                <telerik:gridboundcolumn datafield="ENROLLMENT_STATUS_REASONS_DESC" headertext="Enroll Status Reason" sortexpression="ENROLLMENT_STATUS_REASONS_DESC" uniquename="EnrollStatusReason" />

                                <telerik:gridtemplatecolumn headertext="Edit" allowfiltering="false" uniquename="Edit">
                                    <itemtemplate>
                                        <asp:ImageButton ID="ImageButton1" alt="EditButton" runat="server" CommandName="EditSpecialtiesRow" CommandArgument='<%# Eval("REG_SPECIALTY_ID") %>'
                                            ImageUrl="~/Images/edit.png" ToolTip="Edit" Visible='<%# CanUserViewEdit( Convert.ToInt32(Eval("REG_SPECIALTY_ID")) ) %>' />
                                    </itemtemplate>
                                </telerik:gridtemplatecolumn>

                                <telerik:gridtemplatecolumn headertext="Delete" allowfiltering="false">
                                    <itemtemplate>
                                        <asp:ImageButton ID="ImageButton2" TabIndex="0" runat="server" CommandName="DeleteSpecialtiesRow" CommandArgument='<%# Eval("REG_SPECIALTY_ID") %>'
                                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"
                                            Visible='<%# CanUserViewDelete( Convert.ToInt32(Eval("REG_SPECIALTY_ID")) )  %>' />
                                    </itemtemplate>
                                </telerik:gridtemplatecolumn>

                            </columns>
                        </mastertableview>
                    </telerik:radgrid>
                </div>

                <div class="divHistoryAndAdd">
                    <asp:ImageButton ID="btnAddSpecialties" AlternateText="Add New" runat="server" ImageUrl="~/Images/add.png" CommandName="Specialties" OnCommand="lbtnAdd_Click" ToolTip="Add" /><br />
                    <span aria-label="Specialities History">
                        <asp:LinkButton TabIndex="0" ID="btnSpecialtiesHistory" CommandName="Specialties" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
                        <span class="glyphicon glyphicon-book" style="padding-right:7px;"></span>History 
                        </asp:LinkButton></span>
                </div>
                <br />
            </div>
            
            
            
            <div style="width: 100%; text-align: right; display: none;">
                <asp:HiddenField ID="hdnRowCount" runat="server" />
                <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
                <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
                    <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
                </div>
                <telerik:RadGrid ID="grdSpecialtiesHistory" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
                    AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
                    <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
                        <Excel Format="Biff" />
                    </ExportSettings>
                    <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
                        DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
                        <Columns>
                            <telerik:GridBoundColumn DataField="Operation"                       HeaderText="Operation"      SortExpression="Operation"          />
                            <telerik:GridBoundColumn DataField="SPECIALTY_TYPE_ID"               HeaderText="Specialty Type" SortExpression="SPECIALTY_TYPE_ID" />
                            <telerik:GridBoundColumn DataField="PRIMARY_FLAG"                    HeaderText="Primary Flag"   SortExpression="PRIMARY_FLAG" />
                            <telerik:GridBoundColumn DataField="ENROLL_STATUS_DESC"              HeaderText="ES"             SortExpression="ENROLL_STATUS_DESC" />
                            <telerik:GridBoundColumn DataField="ENROLLMENT_STATUS_REASONS_DESC"  HeaderText="ESR"            SortExpression="ENROLLMENT_STATUS_REASONS_DESC" />
                            <telerik:GridBoundColumn DataField="START_DATE"                      HeaderText="Start Date"     SortExpression="START_DATE"     DataFormatString="{0:MM/dd/yyyy}" />
                            <telerik:GridBoundColumn DataField="END_DATE"                        HeaderText="End Date"       SortExpression="END_DATE"       DataFormatString="{0:MM/dd/yyyy}" />
                            <telerik:GridBoundColumn DataField="UserName"                        HeaderText="User Name"      SortExpression="UserName" />
                            <telerik:GridBoundColumn DataField="DateOfAction"                    HeaderText="Update Date"    SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>

            <div id="specialtyDetail" visible="false" runat="server">
                <asp:Label ID="lblDuplicate" runat="server" Text="The specialty has already been added to this registration. Please select a different one" ForeColor="Red" Visible="false" />
                <asp:Label ID="lblFutureDateEnrollment" runat="server" Text="Specialty Start Date cannot be a future date. Please select today's date." ForeColor="Red" Visible="false" />
                <asp:Label ID="lblPrimaryEndDateWarning" runat="server" Text="Primary Specialty End Date cannot be changed" ForeColor="Red" Visible="false" />
                <asp:Label ID="lblOhrRiseError" runat="server" Text="OHR-OHIORISE cannot be added as Primary Specialty " ForeColor="Red" Visible="false" />
                <asp:Label ID="lblSpecialtySelectError" runat="server" Text="" ForeColor="Red" Visible="false" />
                <asp:Label ID="lblActiveSpec" runat="server" Text="There is already an ACTIVE record for this Specialty. If changes are required on this Specialty, edit the existing record " ForeColor="Red" Visible="false" />
                <asp:Label ID="lblInactiveEditWarning" runat="server" Text="Inactive specialty records cannot be changed" ForeColor="Red" Visible="false" />
                <asp:Label ID="lblEndDateWarning" runat="server" Text="Please contact the appropriate agency to request this specialty be end dated" ForeColor="Red" Visible="false" />
                <asp:Label ID="lblMarkerSpecialties" CssClass="failureNotification" runat="server" Visible="false" Text="Marker Specialties cannot be added as primary specialty." />
                <asp:Label ID="lblOnlyPastStartDate" runat="server" Text="Specialty Date cannot be moved to a later date, if this needs to be adjusted a data fix is required." ForeColor="Red" Visible="false" />
                <asp:Label ID="lblDateValidCheck" runat="server" Text="Start Date (mm/dd/yyyy) is not valid." ForeColor="Red" Visible="false" />
                <asp:Label ID="lblInvalidSpan" runat="server" Text="If trying to reactivate a provider, the previous specialties will automatically update in the workflow." ForeColor="Red" Visible="false" />
                <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="table">
                        <div class="row">
                                  <span class="col-md-3"></span>  <span class="col-md-9" style="text-align:left!important"><asp:Label ID="lblPrimaryMessage"  CssClass="failureNotification" runat="server" Text="" /></span>
                        <table id="ParentTable"  runat="server">
                            <colgroup>
                                <col width="33%" />
                                <col width="53%" align="left" />
                                <col width="20%" align="left" />
                            </colgroup>

                            <tr>
                                <td><span class="formLabel">
                                    <asp:CheckBox ID="chkIsPrimary" runat="server" Enabled="true" Checked="false" OnCheckedChanged="chkIsPrimary_CheckedChanged" AutoPostBack="true" />
                                </span></td>
                                <td align="left">
                                    <asp:Label ID="lblPrimary" runat="server" Text="Designate a Primary Specialty ." />

                                </td>
                                <td align="left">
                                    <asp:Label ID="Label3" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="left">

                                </td>
                                <td align="left">
                                    <asp:Label ID="Label6" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td><span aria-label="Specialty" class="formLabel">Specialty*</span></td>
                                <td align="left">
                                    <telerik:RadComboBox RenderMode="Classic"  BorderColor="Black" BorderWidth="1px" ID="rcbSpecialty" runat="server" CheckBoxes="false" AllowCustomText="false" ToolTip="Specialty" onchange="specialityChange(this)"
                                        OnSelectedIndexChanged="ddlSpeciality_Changed"
                                        Filter="Contains" BackColor="White" AutoPostBack="true" Skin="PDMSModern" CssClass="unsetPublicSearchRCBLength" Width="100%"
                                        aria-label="Specialty" enableariasupport="true" MarkFirstMatch="True" >
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator runat="server" ID="reqCategory" ValidationGroup="valSpecialties"
                                        ControlToValidate="rcbSpecialty" ErrorMessage="*Select a Specialty" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" InitialValue="" />
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblPDMSSpecialty" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td><span aria-label="StartDate" class="formLabel">Start Date*</span></td>
                                <td align="left">
                                    <asp:TextBox aria-label="Select StartDate" ID="txtSpecStart" runat="server" CssClass="formField formField">01/06/2001</asp:TextBox>
                                    <ajax:calendarextender id="calStart" targetcontrolid="txtSpecStart" runat="server" />
                                    <asp:RequiredFieldValidator runat="server" ID="reqStartDate" ValidationGroup="valSpecialties"
                                        ControlToValidate="txtSpecStart" ErrorMessage="*Enter a Start Date" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" InitialValue="" />
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td><span aria-label="EndDate" class="formLabel">End Date</span></td>
                                <td align="left">
                                    <asp:TextBox aria-label="Select EndDate" ID="txtSpecEnd" runat="server" CssClass="formField formField">12/31/2299</asp:TextBox>
                                    <ajax:calendarextender id="CalendarExtender1" targetcontrolid="txtSpecEnd" runat="server" />
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label2" runat="server" />
                                </td>
                            </tr>
                            <tr id="trEnrollStatus" runat="server">
                                <td><span aria-label="Enroll Status" class="formLabel">Enroll Status</span></td>
                                <td align="left">
                                    <asp:DropDownList aria-label="Select Enroll Status" ID="ddlEnrollStatus" runat="server" EnableViewState="true" OnSelectedIndexChanged="ddlEnrollStatus_Changed" AutoPostBack="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trEnrollStatusReason" runat="server">
                                <td><span aria-label="Enroll Status Reason" class="formLabel">Enroll Status Reason</span></td>
                                <td align="left">
                                    <asp:DropDownList aria-label="Select Enroll Status Reason" ID="ddlEnrollStatusReason" runat="server" EnableViewState="true" OnSelectedIndexChanged="ddlEnrollStatusReason_Changed" AutoPostBack="true"></asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="pnlUpdate">
                                        <ProgressTemplate>
                                            <div style="padding-right: 30px">
                                                <img alt="Loading" src="../Images/ajax-loader.gif" />
                                                Loading ...
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                            </div>
                        <div class="row">
                       <asp:PlaceHolder id="phAdditionalSpecialties" runat="server"></asp:PlaceHolder>
                            </div>
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:HiddenField ID="hdnRegSpecialtyID" runat="server" />
                <asp:HiddenField ID="hdnSpecialtyID" runat="server" />
                <asp:HiddenField ID="hdnEndDate" runat="server" />
                <asp:HiddenField ID="hdnEnrollStatusID" runat="server" />
                <asp:HiddenField ID="hdnSpecStart" runat="server" />
                <asp:HiddenField ID="hdnSentToSI" runat="server" />
            </div>
            <br />
            <div id="ODHUploadPanel" visible="false" runat="server">
                <asp:PlaceHolder runat="server" ID="PlaceholderUploadODH"></asp:PlaceHolder>
            </div>
            <div id="DivOHRiseSpecialties" visible="false" runat="server">
                <asp:PlaceHolder runat="server" ID="PlaceholderUploadSectionOHrise"></asp:PlaceHolder>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<div id="HRSA340Bdiv" visible="false" runat="server">
    <span class="pageHeader">HRSA 340B  </span>
    <br />
    <br />
    <div id="pnlHRSA340B" runat="server">
        <div class="divAdd">
            <asp:ImageButton ID="btnAddHRSA340B" AlternateText="AddNew" runat="server" ImageUrl="~/Images/add.png" CommandName="HRSA340B" OnCommand="lbtnAddHRSA_Click" ToolTip="Add" />
        </div>
        <div class="divGrid">
            <asp:GridView runat="server" Width="98%" ID="grdHRSA340B"
                AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
                EmptyDataText="No records found" OnRowCommand="grdHRSA_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ID_340B" HeaderText="340B ID" ItemStyle-Width="350" />
                    <asp:BoundField DataField="START_DATE" HeaderText="Effective Date " DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </div>
    <br />
    <br />
    <div id="HRSA340BDetail" visible="false" runat="server">
        <asp:UpdatePanel ID="pnlHRSA" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="HRSATable" runat="server">
                    <colgroup>
                        <col width="33%" />
                        <col width="53%" align="left" />
                        <col width="20%" align="left" />
                    </colgroup>
                    <tr>
                        <td><span class="formLabel">340B ID</span></td>
                        <td align="left">
                            <asp:TextBox ID="txtHRSA_ID" runat="server" CssClass="formField formField"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="reqHRSA_ID" ValidationGroup="valHRSA340B"
                                ControlToValidate="txtHRSA_ID" ErrorMessage="*Select a HRSA340B ID" Text="*" Display="Dynamic"
                                SetFocusOnError="true" InitialValue="" />
                        </td>
                    </tr>
                    <tr>
                        <td><span class="formLabel">Effective Date*</span></td>
                        <td align="left">
                            <asp:TextBox ID="txtHRSAEffective" runat="server" CssClass="formField formField"></asp:TextBox>
                            <ajax:calendarextender id="CalendarExtender2" targetcontrolid="txtHRSAEffective" runat="server" />
                            <asp:RequiredFieldValidator runat="server" ID="reqEffDate" ValidationGroup="valHRSA340B"
                                ControlToValidate="txtHRSAEffective" ErrorMessage="*Enter a Effective Date" Text="*" Display="Dynamic"
                                SetFocusOnError="true" InitialValue="" />
                        </td>
                    </tr>
                    <tr>
                        <td><span class="formLabel">End Date</span></td>
                        <td align="left">
                            <asp:TextBox ID="txtHRSAEnd" runat="server" CssClass="formField formField">12/31/2299</asp:TextBox>
                            <ajax:calendarextender id="CalendarExtender3" targetcontrolid="txtHRSAEnd" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <table border="0" cellpadding="0" cellspacing="5" align="center" style="padding-bottom: 10px">
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBox" CommandName="Specialties" OnCommand="btnSave_Click1" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" CommandName="Specialties" OnCommand="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdnRegHRSA340ID" runat="server" />
    </div>
</div>

<!--History Pop Modal !-->
<style type="text/css">
    
.dynamic-table {
    margin-left: 325px !important;
}

    .modalPopup {
        height: auto;
        left: 20% !important;
        top: 30% !important;
        position: fixed !important;
    }

    .history {
        text-align: justify;
        color: #000000;
        font-weight: bold;
        margin: 0;
    }
</style>
<asp:UpdatePanel ID="upSpecialtiesHistory" runat="server">
    <ContentTemplate>
        <div role="dialog" aria-live="assertive" aria-labelledby="dialog1Title" aria-hidden="true" id="divDialog1">
            <ajax:modalpopupextender id="mpe" runat="server" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseHistory" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2" behaviorid="mpeSplHitory" />
            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 60%;">
                <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
                    <div align="left">
                        &nbsp;&nbsp;
                     <h2 id="dialog1Title">
                         <asp:Label ID="lblSpHistoryTitle" runat="server" CssClass="history" ForeColor="White" Text="Title" /></h2>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
                    <div class="container-fluid" style="text-align: left; padding: 15px;">
                        <div class="row">
                            <sh:specialtieshistory id="ucSpecialtiesHistory" runat="server" />
                        </div>
                        <div class="row">
                            <div class="btnBox" style="text-align: right;">
                                <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" /><br />
                                <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="”ButtonDummy2”" />
        </div>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExportHistory" />
    </Triggers>
</asp:UpdatePanel>

<!--confirm pop-up modal -->
<asp:UpdatePanel ID="upConfirmAdd" runat="server">
    <ContentTemplate>
        <ajax:modalpopupextender id="mpeConfirmAdd" runat="server" popupcontrolid="pnlConfirmAdd" targetcontrolid="ButtonDummy1"
            backgroundcssclass="modalBackground" behaviorid="mpeConfirmAdd">
        </ajax:modalpopupextender>
        <asp:Panel ID="pnlConfirmAdd" runat="server" CssClass="modalPopup" Style="display: none; width: 25%; height: auto;">
            <asp:Panel ID="pnlConfirmTitle" CssClass="popHeader" runat="server">
                <div class="popTitle">
                    <asp:Label ID="lbl_title" runat="server" Text="Confirm" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnConfirmMsg" runat="server" Style="margin-right: 10px">
                <div style="text-align: left; padding: 15px; margin-left: 30px !important" class="container-fluid">
                    <div class="row">
                        <p style="text-align: left; background-color: white; width: 90%; margin-left: 5%;">
                            <asp:Label ID="lblConfirm" runat="server" Text="If you add this specialty, this will become your only specialty. All of your other specialties will be end dated." />
                        </p>
                    </div>
                </div>
            </asp:Panel>
            <br />
            <div class="btnBox btnBoxCenter" style="padding-top: 10px; width: 93%;">
                <asp:Button runat="server" ID="btnCancelAdd" Text="Cancel" CssClass="buttonBox" OnClick="btnCancelAdd_Click" />
                <asp:Button runat="server" ID="btnSaveAdd" Text="Save" CssClass="buttonBox" OnClick="btnSaveAdd_Click" />
            </div>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonDummy1" Style="display: none" Text="”ButtonDummy1”" />
    </ContentTemplate>
</asp:UpdatePanel>
