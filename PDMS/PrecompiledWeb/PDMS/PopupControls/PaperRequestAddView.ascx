<%@ control language="C#" autoeventwireup="true" inherits="Views_PaperRequestAddView, App_Web_guw1elnn" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<style type="text/css">
    .row {
        padding-bottom:3px;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupEventHandlers);
    });

    function setupEventHandlers() {
        $("#<%= txtConfirmTaxID.ClientID %>").bind("cut copy paste", function (e) {
            e.preventDefault();
        });

        $("#divHelpNPI").hide();

        $("#divWhatIsNPI").mouseover(function () {
            $("#divHelpNPI").show();
        });

        $("#divHelpNPI").mouseleave(function () {
            $("#divHelpNPI").hide();
        });


    }
</script>


<asp:Panel ID="pnlPaperDetails" runat="server" GroupingText="Imaged Document Information">
    <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server" ValidationGroup="AddPaperRequest" ShowSummary="true" Visible="true" Enabled="true" />
    <div class="pg-hint" style="padding-right: 4px;">* Designates a required field</div>
    <br />
    <br />
    <div style="width:70%;margin-left:auto;margin-right:auto;">
    <div class="row"> 
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblDocHandle" runat="server" AssociatedControlID="txtDocHandle" Text="Onbase Document ID*" /></span></div>
        <div class="col-md-6">
            <asp:TextBox ID="txtDocHandle" runat="server" MaxLength="9" CssClass="formField wd150"></asp:TextBox>
            <asp:RegularExpressionValidator ID="valDocIDFormat" ControlToValidate="txtDocHandle" runat="server" Text="*" ValidationGroup="AddPaperRequest" Display="Dynamic"
                ErrorMessage="* Enter a valid numeric Onbase document identifier." ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblApplicationType" runat="server" AssociatedControlID="ddlApplicationType" Text="Application Type*" /></span></div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlApplicationType" runat="server" CssClass="wd500" OnSelectedIndexChanged="ddlApplicationType_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True"/>
            <asp:CompareValidator runat="server" ID="valAppTypeReqd" ControlToValidate="ddlApplicationType" ValueToCompare="0" Type="Integer" ErrorMessage="* Application Type is required."
                Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddPaperRequest" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblDocumentType" runat="server" AssociatedControlID="ddlDocumentType" Text="Document Type*" /></span></div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="wd500" />
            <asp:CompareValidator runat="server" ID="valDocTypeReqd" ControlToValidate="ddlDocumentType" ValueToCompare="0" Type="Integer" ErrorMessage="* Document Type is required."
                Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddPaperRequest" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblRequestType" runat="server" AssociatedControlID="ddlRequestType" Text="Type of Request*" /></span></div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlRequestType" runat="server" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" />
            <asp:CompareValidator runat="server" ID="valReqTypeReqd" ControlToValidate="ddlRequestType" CssClass="wd400" ValueToCompare="0" Type="Integer" ErrorMessage="* Type of Request is required."
                Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddPaperRequest" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-3  text-right">
            <span class="formLabel200"><asp:Label ID="lblMedicaidID" runat="server" AssociatedControlID="txtMedicaidID" Text="Provider Number" /></span>
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="11" CssClass="formField wd150"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valProvNumberReqd" runat="server" ControlToValidate="txtMedicaidID"
                Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*"
                ValidationGroup="AddPaperRequest" ErrorMessage="* Provider Number is required."></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3"></div>
         <div class="col-md-9">
        <div class="pg-hint2">
            <asp:Literal ID="ltlMedicaidHint" runat="server" Text=" <%$ Resources:BrandingResource , PAPER_APPLICATION_MEDICAID_HINT %>" />
        </div>
             </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblTaxID" runat="server" AssociatedControlID="txtTaxID" Text="Tax ID" /></span></div>
        <div class="col-md-2">
            <asp:TextBox ID="txtTaxID" runat="server" CssClass="formField wd100" MaxLength="9" />
            <asp:RegularExpressionValidator ID="valTaxIDFormat" runat="server" ControlToValidate="txtTaxID"
                ValidationExpression="(?!0{9})(?!9{9})^([0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" ErrorMessage="* Enter a 9 digit Tax ID."
                Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="AddPaperRequest" Display="Dynamic" />
        </div>
        <div class="col-md-2  text-right"><span class="formLabelAuto"><asp:Label ID="lblNPI" runat="server" AssociatedControlID="txtNPI" Text="NPI*" /></span></div>
        <div class="col-md-5">
            <asp:TextBox ID="txtNPI" runat="server" MaxLength="10" CssClass="formField wd100" />
            <asp:RegularExpressionValidator ID="valNPI" runat="server" ControlToValidate="txtNPI"
                ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" ErrorMessage="* Enter a 10 digit NPI that does not begin with 0."
                Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="AddPaperRequest" Display="Dynamic" />
            <asp:RequiredFieldValidator ID="valNPIReqd" runat="server" ControlToValidate="txtNPI"
                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                ValidationGroup="AddPaperRequest" ErrorMessage="* NPI is required."></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblConfirmTaxID" runat="server" AssociatedControlID="txtConfirmTaxID" Text="Confirm Tax ID*" /></span></div>
        <div class="col-md-6">
            <asp:TextBox ID="txtConfirmTaxID" runat="server" MaxLength="9" CssClass="formField wd100" AutoCompleteType="None" ValidationGroup="AddPaperRequest" />
            <asp:RequiredFieldValidator ID="valTaxIDCReqd" runat="server" ControlToValidate="txtConfirmTaxID"
                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                ValidationGroup="AddPaperRequest" ErrorMessage="* Confirm Tax ID is required."></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valConfirmTaxIdFormat" runat="server" ControlToValidate="txtConfirmTaxID"
                ValidationExpression="(?!0{9})(?!9{9})^\d{9}$" ErrorMessage="* Enter a 9 digit Tax ID in Confirm Tax ID."
                Enabled="true" SetFocusOnError="true" Text="*"
                ValidationGroup="AddPaperRequest" Display="Dynamic" />
            <asp:CompareValidator ID="valTaxIDCompare" runat="server" ControlToCompare="txtTaxID"
                ControlToValidate="txtConfirmTaxID" Display="Dynamic" Text="*"
                ErrorMessage="* The Tax ID and Confirm Tax ID do not match.  Please verify you have entered the correct Tax ID."
                ValidationGroup="AddPaperRequest" Operator="Equal" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-3  text-right">
            <span class="formLabel200"><asp:Label ID="lblZipCode" runat="server" AssociatedControlID="txtZipCode" Text="Zip Code" /></span>
        </div>
        <div class="col-md-2">
            <asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" CssClass="formField wd100" />
            <asp:RegularExpressionValidator ID="valZipFormat" runat="server" Text="*"
                ErrorMessage="* Enter 5 digits for zip code" ControlToValidate="txtZipCode" SetFocusOnError="true"
                Display="Dynamic" ValidationExpression="(?!0{5})(?!9{5})\d{5}$" ValidationGroup="AddPaperRequest" />
        </div>
        <div class="col-md-2  text-right"><span class="formLabelAuto"><asp:Label ID="lblZipCodeExt" runat="server" AssociatedControlID="txtZipCodeExt" Text="Zip Code Extension" /></span></div>
        <div class="col-md-5">
            <asp:TextBox ID="txtZipCodeExt" runat="server" MaxLength="4" CssClass="formField wd100" />
            <asp:RegularExpressionValidator ID="valZipExtFormat" runat="server" Text="*"
                ErrorMessage="* Enter 4 digits for zip code extension" ControlToValidate="txtZipCodeExt" SetFocusOnError="true"
                Display="Dynamic" ValidationExpression="(?!9{4})\d{4}$" ValidationGroup="AddPaperRequest" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right">
            <span class="formLabel200"><asp:Label ID="lblCategory" runat="server" AssociatedControlID="ddlCategory" Text="Category" /></span>
        </div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-3  text-right">
            <span class="formLabel200"><asp:Label ID="lblProviderType" runat="server" AssociatedControlID="ddlProviderType" Text="Provider Type" /></span>
        </div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlProviderType" runat="server" AutoPostBack="true" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlProviderType_SelectedIndexChanged" />
        </div>
    </div>
    <%--        <tr>
            <td></td>
            <td class="pg-hint2">
                <asp:Literal ID="ltlPTHint" runat="server" Text=" <%$ Resources:BrandingResource , PAPER_APP_PROVIDERTYPE_HINT %>" /></td>
        </tr>--%>
    <%--                    <tr >
                        <td class="formLabel wd150">Specialty</td>
                        <td class="fieldValue"><asp:DropDownList ID="ddlSpecialty" runat="server" AutoPostBack="true" CssClass="wd400" OnSelectedIndexChanged="ddlSpecialty_SelectedIndexChanged" /></td>
                    </tr>--%>
    <div class="row">
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblTaxonomy" runat="server" AssociatedControlID="ddlTaxonomy" Text="Taxonomy" /></span></div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlTaxonomy" runat="server" AutoPostBack="false" AppendDataBoundItems="True"/>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right">
            <span class="formLabel200"><asp:Label ID="lblRequestStatus" runat="server" AssociatedControlID="ddlRequestStatus" Text="Document Status" /></span>
        </div>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlRequestStatus" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3  text-right"><span class="formLabel200"><asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" Text="Comments" /></span></div>
        <div class="col-md-6">
            <asp:TextBox ID="txtComments" runat="server" Rows="3" CssClass="formField wd500" TextMode="MultiLine" MaxLength="500" />
        </div>
    </div>
    </div>
    <div class="btnBox btnBoxCenter" style="padding-right: 10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBox buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="AddPaperRequest" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" CausesValidation="false" />

    </div>
    <br />
    <br />
    <br />

    <div id="divException" runat="server" style="min-height: 60px; padding: 4px; width: 98%;">
        <uc:Separator ID="ucSep1" runat="server" Header="Exception Log" />
        <br />

        <asp:GridView runat="server" Width="100%" ID="gvExceptions" AutoGenerateSelectButton="false" AutoGenerateColumns="False"
            CssClass="gridView" EmptyDataText="No exceptions found." AllowSorting="false" ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:BoundField DataField="CREATED_ON_DATE_TIME" HeaderText="Error Date" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField DataField="ERROR_TYPE_ID" HeaderText="Error Code" />
                <asp:BoundField DataField="ERROR_NAME" HeaderText="Description" />
                <asp:BoundField DataField="ERROR_STATUS_TYPE" HeaderText="Status" />
            </Columns>
            <HeaderStyle CssClass="gridViewHeader" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <EmptyDataRowStyle CssClass="gridViewRow" />
        </asp:GridView>
    </div>
</asp:Panel>
<ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlNoExceptions" TargetControlID="btnDummy"
    RepositionMode="RepositionOnWindowScroll" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlNoExceptions">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlNoExceptions" runat="server" CssClass="modalPopup" Style="display: none; height: 140px; width: 300px;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <div class="popTitle">Paper Request Validation Complete</div>
    </asp:Panel>
    <div style="padding-top: 10px; padding-left: 4px; padding-right: 4px;" class="center">
        <asp:Label ID="lblNewReg" runat="server" Text="No exceptions were found with the paper request.  Proceed with New Registration?"></asp:Label>
        <asp:Label ID="lblMaintReg" runat="server" Text="No exceptions were found with the paper request.  Proceed with Registration Update?"></asp:Label>
        <br />
        <br />
        <div class="btnBox">
            <asp:Button ID="btnProcessYes" runat="server" Text="Yes" CssClass="buttonBox" OnClick="btnProcessYes_Click" CausesValidation="false" />
            <asp:Button ID="btnProcessNo" runat="server" Text="No" CssClass="buttonBox" OnClick="btnProcessNo_Click" CausesValidation="false" />
        </div>
    </div>
</asp:Panel>
<asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy" />

