<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Licenses, App_Web_tiu3g34i" %>
<%@ register src="~/UserControls/FormField.ascx"             tagprefix="uc"     tagname="FormField"  %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit"       tagprefix="ajax" %>
<%@ register src="~/PopupControls/UploadSectionControl.ascx" tagprefix="uc"     tagname="UploadSectionControl"  %>
<%@ register src="~/PopupControls/Separator.ascx"            tagprefix="uc1"    tagname="Separator"     %>
<%@ register src="~/PopupControls/LicensesHistory.ascx"      tagprefix="uc"     tagname="LicensesHistory" %>
<%@ register src="~/PopupControls/SpecialityFocus.ascx"      tagprefix="uc"     tagname="SpecialityFocus" %>
<%@ register src="~/UserControls/Address.ascx"               tagprefix="uc"     tagname="Address"  %>

<script>
    function removeDisabled() {
        $("#<%= btnModalOk.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<div onmouseover="removeDisabled();">
<asp:panel id="pnlLicenses" runat="server" style="display: inline-block; width: 100%;">
    <p style="text-align: center; font-weight: bold">A copy of each license must be uploaded to this page.</p>
    <div class="divGrid">
        <asp:gridview runat="server" width="98%" id="grdLicenses" autogeneratecolumns="False" horizontalalign="Left" cssclass="gridview"
            emptydatatext="No licenses found" onrowcommand="grd_RowCommand" allowsorting="true" onrowdatabound="grdLicenses_RowDataBound">
            <columns>
                <asp:boundfield datafield="LICENSE_NUMBER" headertext="License Number" sortexpression="LICENSE_NUMBER" />
                <asp:boundfield datafield="LICENSE_TYPE_NAME" headertext="License Board" sortexpression="LICENSE_TYPE_NAME" />
                <asp:boundfield datafield="LICENSE_STATE" headertext="License Issuing State" sortexpression="LICENSE_STATE" />
                <asp:boundfield datafield="LICENSE_STATUS" headertext="License Status" sortexpression="LICENSE_STATUS" />
                <asp:boundfield datafield="LICENSE_SUBSTATUS" headertext="License Sub-Status" sortexpression="LICENSE_SUBSTATUS" />
                <asp:boundfield datafield="LICENSE_EFF_DATE" headertext="Effective Date" sortexpression="LICENSE_EFF_DATE" dataformatstring="{0:d}" />
                <asp:boundfield datafield="LICENSE_END_DATE" headertext="Expiration Date" sortexpression="LICENSE_END_DATE" dataformatstring="{0:d}" />
                <asp:boundfield datafield="ELICENSE_VERIFIED" headertext="Verified" sortexpression="ELICENSE_VERIFIED" visible="false" />
                <asp:templatefield headertext="Address">
                    <itemtemplate>
                        <asp:literal id="litAddress" runat="server" text='<%# FormatAddress(Eval("REG_ADDRESSID"))%>' />
                    </itemtemplate>
                </asp:templatefield>
                <asp:templatefield headertext="Endorsement">
                    <itemtemplate>
                        <asp:literal id="litEndorsement" runat="server" text='<%# FormatSpecialtyFocus(Eval("REG_LICENSURE_ID"))%>' />
                    </itemtemplate>
                </asp:templatefield>
                <asp:templatefield itemstyle-width="2%" HeaderText="<span style='display:none'>Edit</span>">
                    <itemtemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" commandname="EditLicensesCodeRow" commandargument="<%# ((GridViewRow) Container).RowIndex %>"
                            imageurl="~/Images/edit.png" tooltip="Edit" />
                    </itemtemplate>
                </asp:templatefield>
                <asp:templatefield HeaderText="<span style='display:none'>Delete</span>">
                    <itemtemplate>
                        <asp:imagebutton id="btnDelete" runat="server" commandname="DeleteLicensesCodeRow" commandargument="<%# ((GridViewRow) Container).RowIndex %>"
                            imageurl="~/Images/cancel.png" tooltip="Delete" onclientclick="return confirm('Are you sure you want to delete?');" alternatetext="Delete" />

                    </itemtemplate>
                </asp:templatefield>
            </columns>
            <pagerstyle cssclass="gridpager" horizontalalign="Right" />
            <headerstyle cssclass="gridViewHeader" width="100px" />
            <alternatingrowstyle cssclass="gridViewAltRow" />
            <rowstyle cssclass="gridViewRow" />
            <footerstyle cssclass="gridViewFooter" />
        </asp:gridview>
    </div>
    <div class="divHistoryAndAdd">
        <asp:imagebutton id="btnAddLicenses" runat="server" AlternateText="AddLicense" imageurl="~/Images/add.png" commandname="Licenses" oncommand="lbtnAdd_Click" tooltip="Add" />
        <br />
        <span aria-label="Licenses History">
            <asp:LinkButton TabIndex="0" ID="btnLicensesHistory" CommandName="Licenses" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
                <span class="glyphicon glyphicon-book" style="padding-right:7px;"></span>History 
            </asp:LinkButton></span>
    </div>
    <br />
</asp:panel>
<div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grdLicensesHistory" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="false" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="Operation"               HeaderText="Operation"              SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="LICENSE_NUMBER"          HeaderText="License Number"         SortExpression="LICENSE_NUMBER" />
                <telerik:GridBoundColumn DataField="UserName"                HeaderText="User Name"              SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="LICENSE_BOARD_NAME"      HeaderText="License Board"          SortExpression="LICENSE_BOARD_NAME" />
                <telerik:GridBoundColumn DataField="LICENSE_STATE"           HeaderText="License Issuing State"  SortExpression="LICENSE_STATE" />
                <telerik:GridBoundColumn Datafield="LICENSE_STATUS"          HeaderText="License Status"         SortExpression="LICENSE_STATUS" />
                <telerik:GridBoundColumn Datafield="LICENSE_SUBSTATUS"       HeaderText="License Sub-Status"     SortExpression="LICENSE_SUBSTATUS" />
                <telerik:GridBoundColumn Datafield="ELICENSE_VERIFIED"       HeaderText="eLicense Verified"      SortExpression="ELICENSE_VERIFIED" />
                <telerik:GridBoundColumn DataField="LICENSE_EFF_DATE"        HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}"    SortExpression="LICENSE_EFF_DATE" />
                <telerik:GridBoundColumn DataField="LICENSE_END_DATE"        HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"   SortExpression="LICENSE_END_DATE" />
                <telerik:GridBoundColumn DataField="DateOfAction"            HeaderText="Update Date"    SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>

<div id="licenseDetail" runat="server" visible="false">
    <div>
        <asp:validationsummary id="vsLicenses" runat="server" displaymode="List" validationgroup="valLicenses" />
        <asp:Label id="lblInfoEliceseVerified" runat="server" Visible="false" Text="Data cannot be populated, manual entry required."  CssClass="error-message"/>
    </div>
    <div class="row" id="trEditHelpText" runat="server">

        <p style="text-align: center; font-weight: bold">
            <asp:label runat="server" text="Results from eLicense verification are read only.After your application is submitted, the only editable field is Expiration Date." />
        </p>
    </div>
    <div id="divProfessionalLicense" style="width: 100%; margin-left: 10%;">
        <div id="ParentTable" runat="server">
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:label id="Label4" runat="server" text="State*" cssclass="formLabel" />
                </div>
                <div class="col-sm-8">
                    <asp:dropdownlist id="ddlLicenseState" runat="server" cssclass="formDropDown" appenddatabounditems="True" aria-Label="License Issuing State" aria-required="true" onselectedindexchanged="ddlBoardName_SelectedIndexChanged" autopostback="true" />
                    <asp:requiredfieldvalidator runat="server" id="RequiredFieldValidator1" validationgroup="valLicenses"
                        controltovalidate="ddlLicenseState" errormessage="*Select a State" text="*" display="Dynamic"
                        setfocusonerror="true" initialvalue="" />
                </div>
                <div style="display: none;">
                    <div class="pdmsLabel">
                        <asp:label id="pdms_State" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:label id="Label5" runat="server" text="License Board Name*" cssclass="formLabel" />
                </div>
                <div class="col-sm-8">
                    <asp:dropdownlist id="ddlBoardName" runat="server" cssclass="formDropDown" appenddatabounditems="True" aria-Label="License Board Name" aria-required="true" onselectedindexchanged="ddlBoardName_SelectedIndexChanged" autopostback="true" />
                    <asp:requiredfieldvalidator runat="server" id="RequiredFieldValidator3" validationgroup="valLicenses"
                        controltovalidate="ddlBoardName" errormessage="*Select a License Board Name" text="*" display="Dynamic"
                        setfocusonerror="true" initialvalue="" />
                </div>
                <div style="display: none;">
                    <div class="pdmsLabel">
                        <asp:label id="pdms_Type" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right"></div>
                <div class="col-sm-8"><span class="pg-hint2">If Other, enter Board Name:</span></div>

            </div>
            <div class="row">
                <div class="col-sm-4 text-right"><span class="formLabel"></span></div>
                <div class="col-sm-8">
                    <asp:textbox id="txtBoard" runat="server" cssclass="formField" aria-Label="If Other, enter Board Name"></asp:textbox>

                </div>
                <div style="display: none;">
                    <asp:label id="lblPDMSBoard" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:label id="Label1" runat="server" text="License Number*" cssclass="formLabel"/>
                </div>
                <div class="col-sm-8">
                    <asp:textbox id="prov_Number" runat="server" cssclass="formField" maxlength="50" aria-Label="License Number" aria-required="true" ontextchanged="prov_Number_TextChanged" autopostback="true"/>
                    <asp:image runat="server" id="imgcheck" alternatetext="E-License Verified" tooltip="E-License Verified" imageurl="~/Images/check.png" visible="false" />
                    <asp:requiredfieldvalidator runat="server" id="RequiredFieldValidator2" controltovalidate="prov_Number" errormessage="*Enter License Number" text="*" display="Dynamic" setfocusonerror="true" validationgroup="valLicenses" />
                    <asp:regularexpressionvalidator runat="server" id="RegularExpressionValidator" controltovalidate="prov_Number" errormessage="* Enter a License Number with max 25 characters." validationExpression=".{0,25}" display="None" setfocusonerror="true" validationgroup="valLicenses" />
                </div>
                <div style="display: none;">
                    <div class="pdmsLabel">
                        <asp:label id="pdms_Number" runat="server" />
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:label id="Label6" runat="server" text="Effective Date*" cssclass="formLabel" />
                </div>
                <div class="col-sm-8">
                    <asp:textbox id="prov_Effective" runat="server" cssclass="formField" aria-Label="Effective Date" aria-required="true" />
                    <ajax:calendarextender id="CalendarExtender1" targetcontrolid="prov_Effective" runat="server" />
                    <asp:requiredfieldvalidator runat="server" id="reqEffective" controltovalidate="prov_Effective" errormessage="*Enter an Effective Date" text="*" display="Dynamic" setfocusonerror="true" validationgroup="valLicenses" />
                    <asp:customvalidator id="CustomValidator1" validateemptytext="true" runat="server" controltovalidate="prov_Effective" onservervalidate="ValidateLCStartDate" display="Static" validationgroup="valLicenses" errormessage="*Effective date must not be a future date." text="*" />
                    <asp:comparevalidator id="CompareValidator2" runat="server" validationgroup="valLicenses"
                        type="Date" operator="DataTypeCheck" controltovalidate="prov_Effective"
                        errormessage="Select a valid Effective Date" text="*" display="Dynamic" valuetocompare="MM/dd/yyyy"
                        setfocusonerror="true" />
                </div>
                <div style="display: none;">
                    <div class="pdmsLabel">
                        <asp:label id="pdms_Effective" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:label id="Label3" runat="server" text="Expiration Date*" cssclass="formLabel" />
                </div>
                <div class="col-sm-8">
                    <asp:textbox id="prov_End" runat="server" cssclass="formField" aria-Label="Expiration Date" aria-required="true"/>
                    <ajax:calendarextender id="calEnd" targetcontrolid="prov_End" runat="server" />

                    <asp:comparevalidator id="CompareValidator1" runat="server" validationgroup="valLicenses"
                        type="Date" operator="GreaterThan" controltovalidate="prov_End" controltocompare="prov_Effective"
                        errormessage="Select a valid Expiration Date" text="*" display="Dynamic" valuetocompare="MM/dd/yyyy"
                        setfocusonerror="true" />
                    <asp:requiredfieldvalidator runat="server" id="RequiredFieldValidator4" controltovalidate="prov_End" errormessage="*Enter an Expiration Date" text="*" display="Dynamic" setfocusonerror="true" validationgroup="valLicenses" />

                </div>
                <div style="display: none;">
                    <div class="pdmsLabel">
                        <asp:label id="pdms_Expiration" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:label id="lblLicenseStatus" runat="server" text="License Status" cssclass="formLabel" />
                </div>
                <div class="col-sm-8">
                    <asp:dropdownlist id="ddlLicenseStatus" runat="server" cssclass="formDropDown" appenddatabounditems="True" aria-Label="License Status" />
                </div>
                <div style="display: none;">
                    <div class="pdmsLabel">
                        <asp:label id="Label7" runat="server" />
                    </div>
                </div>
            </div>

            <div style="width: 100%; margin-left: 9%;">
                <uc:address id="ucLicenseAddress" runat="server"></uc:address>
            </div>

            <asp:placeholder id="plcHolderFocus1" runat="server" enableviewstate="true" visible="true">
                <uc:specialityfocus id="ucSpecialtyFocus" runat="server"></uc:specialityfocus>
            </asp:placeholder>


            <div class="divHistoryAndAdd">
                <asp:imagebutton id="ImgSpecialtyFocus" runat="server" imageurl="~/Images/add_old.png" commandname="AddMoreSpecialtyFocus" oncommand="ImgButtonSpecialty_Click" tooltip="Add another Specilaty Focus" visible="false" />

            </div>

        </div>
    </div>
    <br />
    <div id="manualUpload" style="width: 80%; margin-left: 5%;">
        <uc1:separator id="ucSep1" runat="server" header="Uploaded Documents" visible="false" />

        <asp:placeholder runat="server" id="PlaceholderUploadProfessionalLicense"></asp:placeholder>
    </div>

    <asp:textbox id="hidIsEdit" runat="server" visible="false" />
    <asp:textbox id="hidID" runat="server" visible="false" />
    <asp:textbox id="hidVerified" runat="server" visible="false" />
     <asp:textbox id="hidLicenseSubStatus" runat="server" visible="false" />

</div>
<style type="text/css">
    .modalPopup
    {
        height: auto;
        left: 20% !important;
        top: 30% !important;
        position: fixed !important;
    }
</style>
<asp:panel id="upLicenseHistory" runat="server">
    <ajax:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlModal" CancelControlID="btnModalOk">
    </ajax:modalpopupextender>
    <asp:panel id="pnlModal" runat="server" cssclass="modalPopup" style="display: none; padding: 20px; width: 1000px;">
        <asp:panel id="pnlHeader" cssclass="pnlHeader" runat="server" horizontalalign="Left" style="width: 950px">
            <div align="left">
                &nbsp;&nbsp;
                <asp:label id="lblTitle" cssclass="bodyTextBold" runat="server" text="Title" forecolor="White" />
            </div>
        </asp:panel>
        <asp:panel id="pnlMain1" runat="server" style="padding: 10px; width: 950px !important; margin-left: 10px;">
            <div>
                <uc:licenseshistory id="ucLicensesHistory" runat="server" />
            </div>
        </asp:panel>
        <div class="btnBox" style="padding: 10px">
            <asp:button runat="server" id="btnModalOk" text="OK" cssclass="buttonBox" causesvalidation="false" />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
        </div>
    </asp:panel>
    <asp:button runat="server" id="ButtonDummy2" style="display: none" text="”ButtonDummy2”" />
</asp:panel>
</div>