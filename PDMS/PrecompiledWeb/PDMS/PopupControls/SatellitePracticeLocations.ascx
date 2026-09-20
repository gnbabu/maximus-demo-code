<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SatellitePracticeLocations, App_Web_av5ll3zk" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register tagprefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>
<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>
<%@ register src="~/UserControls/Address.ascx" tagname="Address" tagprefix="uc" %>
<%@ register src="~/PopupControls/OfficeHoursServiceLocation.ascx" tagname="OfficeHours" tagprefix="ohsl" %>
<%@ register src="~/PopupControls/SatellitePracticeLocationsHistory.ascx" tagprefix="uc" tagname="SatelliteHistory" %>

<style type="text/css">
    .modalPopup {
        height: auto;
        left: 20% !important;
        top: 30% !important;
        position: fixed !important;
    }

    .RadCalendarPopup {
        background: #d2deef;
    }
</style>

<script type="text/javascript">
    function exportPopup() {
        
    }

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<div onmouseover="removeDisabled();">
<div class="col-sm-12 text-left">
    <span><i>*Please enter Other Service locations that bill/will bill under the same Medicaid ID</i> </span>
</div>
<div id="divSatelliteLocations">
    <telerik:radgrid id="rgSatellitePracticeLocations" rendermode="Lightweight" runat="server" enableariasupport="true" filtermenu-ariasettings-label="WorkFlow"
        allowpaging="True" allowsorting="true" allowfilteringbycolumn="true" skin="PDMSModern" OnItemCommand="RadGrid1_ItemCommand" AllowCustomPaging="true"
        OnPageIndexChanged="RadGrid1_PageIndexChanged" OnPageSizeChanged="RadGrid1_PageSizeChanged" AllowCustomSorting="true" OnSortCommand="RadGrid1_GridSortCommand">
        <mastertableview autogeneratecolumns="False" datakeynames="">
            <columns>
                <telerik:gridboundcolumn datafield="ADDN_PRACTICE_NAME" headertext="Additional Practice Name" visible="true" SortExpression="ADDN_PRACTICE_NAME" />
                <telerik:gridboundcolumn datafield="ADDN_PRACTICE_ADDR" headertext="Additional Practice Address" visible="true" SortExpression="ADDN_PRACTICE_ADDR" />
                <telerik:GridTemplateColumn HeaderText="Additional Practice Phone Number" visible="true" SortExpression="ADDN_PRACTICE_PHONE">
                <ItemTemplate>
                        <%# string.Format("{0:(###) ###-####}", Convert.ToInt64(Eval("ADDN_PRACTICE_PHONE"))) %>
                </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:griddatetimecolumn datafield="EFF_DATE" headertext="Effective Date" dataformatstring="{0:MM/dd/yyyy}" visible="true" SortExpression="EFF_DATE" />
                <telerik:griddatetimecolumn datafield="END_DATE" headertext="End Date" dataformatstring="{0:MM/dd/yyyy}" visible="true" SortExpression="END_DATE" />
                <telerik:gridtemplatecolumn visible="false">
                    <itemtemplate>
                        <asp:hiddenfield id="hdnRegAddressId" runat="server" value='<%# Eval("REG_ADDRESS_ID")%>' />
                    </itemtemplate>
                </telerik:gridtemplatecolumn>
                <telerik:gridtemplatecolumn headertext="Edit" allowfiltering="false" uniquename="Edit">
                    <itemtemplate>
                        <asp:imagebutton id="ImageButton1" alt="EditButton" runat="server" commandname="EditSatellitePracticeLocations" commandargument='<%# Eval("REG_ADDRESS_ID")%>'
                            imageurl="~/Images/edit.png" tooltip="Edit" visible="true" />
                    </itemtemplate>
                </telerik:gridtemplatecolumn>
            </columns>
            <pagerstyle mode="NextPrevAndNumeric" alwaysvisible="true" pagesizelabeltext="Page Size: " pagesizes="50,100,500" />
        </mastertableview>
        <SortingSettings EnableSkinSortStyles="false"></SortingSettings>
    </telerik:radgrid>



    <div class="divHistoryAndAdd" id="divHistoryAndAdd" runat="server">
        <asp:imagebutton id="btnAddSatellitePracticeLocations" alternatetext="Add New" runat="server" imageurl="~/Images/add.png" commandname="SatellitePracticeLocations" oncommand="lbtnAdd_Click" tooltip="Add" />
        <br />
        <asp:linkbutton id="btnSatellitePracticeLocationsHistory2" runat="server" cssclass="buttonBoxFocus" commandname="SatellitePracticeLocationsHistory" oncommand="btnHistory_Click" text="History" tooltip="History" style="color: white; text-decoration: none;">
            <span class="glyphicon glyphicon-book" style="padding-right: 7px;"></span>History 
        </asp:linkbutton>
        <asp:linkbutton id="btnSatellitePracticeLocationsHistory" runat="server" tooltip="Excel" onclick="lnkExcel_Click" onclientclick="exportPopup();" visible="true" Enabled="true">
           <img src="../Images/Excel_24x24.png" alt="XLS" /> </asp:linkbutton>&nbsp;&nbsp;
    </div>
</div>

<asp:Panel ID="upSatellitePracticeLocationsHistory" runat="server">
    <ajax:ModalPopupExtender ID="mpe" runat="server" BackgroundCssClass="modalBackground" CancelControlID="btnCloseHistory" PopupControlID="pnlModal" PopupDragHandleControlID="pnlModal" TargetControlID="ButtonDummy3" />
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 1000px;">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
            <div align="left">
                &nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" CssClass="bodyTextBold" ForeColor="White" Text="Title" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
            <div class="container-fluid" style="text-align: left; padding: 15px;">
                <div class="row">
                    <uc:SatelliteHistory ID="ucSatelliteHistory" runat="server" />
                </div>
                <div class="row">
                    <div class="btnBox" style="text-align: right;">
                        <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" />
                        <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcelHistory_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" aria-Label="Dummy Button" ID="ButtonDummy3" Style="display: none" Text=”ButtonDummy3” />
</asp:Panel>
    <div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grd" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
                <telerik:GridBoundColumn DataField="EMAIL1"              HeaderText="Email"          SortExpression="EMAIL1" />
                <telerik:GridBoundColumn DataField="PHONE1"              HeaderText="Phone"          SortExpression="PHONE1" />
                <telerik:GridBoundColumn DataField="ADR_EFFECTIVE_DATE"  HeaderText="Effective Date" SortExpression="ADR_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                <telerik:GridBoundColumn DataField="ADR_END_DATE"        HeaderText="End Date"       SortExpression="ADR_END_DATE"       DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                <telerik:GridBoundColumn DataField="UserName"            HeaderText="User Name"      SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction"       DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>

<div id="satelliteLocDetail" runat="server" visible="false">
    <asp:validationsummary id="valSumSatellitePracticeLocations" displaymode="List" runat="server" cssclass="failureNotification" validationgroup="SatellitePracticeLocations" />
    <asp:updatepanel id="upProv" runat="server" updatemode="Conditional">
        <contenttemplate>
            <div id="ParentTable" runat="server">
                <div class="row">
                    <div class="col-sm-3  text-right"><span class="formLabel200">Override Address Validation</span></div>
                    <div class="col-sm-9" id="div1" runat="server">
                        <asp:checkbox id="prov_override" runat="server" checked="false" cssclass="formFieldCheckBox" oncheckedchanged="prov_Override_CheckedChanged" autopostback="true" style="border: none" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right">
                        <asp:label id="Label14" runat="server" text="Name*" cssclass="formLabel200" />
                    </div>
                    <div class="col-sm-9">
                        <asp:textbox id="prov_Name" aria-label="Name" aria-required="true" runat="server" cssclass="formField" maxlength="100" />
                        <asp:requiredfieldvalidator runat="server" id="RequiredFieldValidator2" controltovalidate="prov_Name" errormessage="* Name is required." text="*" display="Dynamic" setfocusonerror="true" validationgroup="SatellitePracticeLocations" />
                    </div>
                </div>
            </div>
            <uc:address id="ucAddress" runat="server" validationgroup="SatellitePracticeLocations" getgeocode="false" />
            <ohsl:officehours id="OfficeHours" runat="server" validationgroup="SatellitePracticeLocations" subcontrolvalidationgroup="SatellitePracticeLocations" />
        </contenttemplate>
    </asp:updatepanel>
    <asp:textbox id="hidIsEdit" runat="server" visible="false" />
    <asp:hiddenfield id="hidID" runat="server" />
    <asp:hiddenfield id="hdnOtherServiceAddress" runat="server" />
</div>
</div>
