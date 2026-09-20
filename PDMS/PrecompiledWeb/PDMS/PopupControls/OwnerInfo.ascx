<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OwnerInfo, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>

<style type="text/css">
    .formLabel200 {
        right: -8px;
        position: absolute;
    }
</style>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<script type="text/javascript">

    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }

    function CheckSSNLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 9)
            args.IsValid = false;
        return args.IsValid;
    }

    function ignoreValidation() {
        if (typeof Page_ClientValidate != 'undefined') {
            Page_ClientValidate('reset-validation');
            Page_BlockSubmit = false;
        }
        return true;
    };

    $(function() {
        $(".help-business-ownerinfo").mouseover(function () {
              // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpBusinessNameInfoownerinfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();
        });

        $(".help-business-ownerinfo").mouseleave(function () {
            $("#helpBusinessNameInfoownerinfo").hide();
        });
    });

</script>
<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <asp:ValidationSummary ID="vsOwnerInfo" runat="server" DisplayMode="List" ForeColor="Red" ValidationGroup="valOwnerInfo" />
        </div>
        <div>
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <ProgressTemplate>
                    <div>
                        <img src="../Images/ajax-loader.gif" alt="AJAX Loader" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <div id="divParent">
            <div class="row">
                <div class="col-sm-3  text-right" runat="server">
                    <asp:Label ID="lblOwnerType" runat="server" CssClass="formLabel200" Text="Owner Type*"></asp:Label>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlOwnerType" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="ddlOwnerType_SelectedIndexChanged"
                        AutoPostBack="True"
                        AppendDataBoundItems="True" CausesValidation="false" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerInfo"
                        ControlToValidate="ddlOwnerType" ErrorMessage="*Select an Owner Type" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                        <span id="helpBusinessNameownerinfo" class="help-business-ownerinfo" style="cursor: pointer; display: inline-block;">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
                        </span>
                        <span id="helpBusinessNameInfoownerinfo" class="infoBox" style="top: 0; right: 0;">
                            <span class="infoTitle"></span>
                            <span class="infoContent">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_Owner_Title_HELPTEXT %>"></asp:Literal>
                            </span>
                        </span>
                </div>
                <div style="display: none;">
                    <div class="pdmsLabel">
                        <asp:Label ID="lblPDMSOwnerType" runat="server" />
                    </div>
                </div>
            </div>
            <div id="divOwnTitle" style="width: 100%" runat="server">
                <div class="row" id="trOwnTitle" runat="server">
                    <div class="col-sm-3  text-right" runat="server">
                        <asp:Label ID="lblOwnerTitle" runat="server" CssClass="formLabel200" Text="Owner Title"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlOwnerTitle" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="ddlOwnerTitle_SelectedIndexChanged"
                            AutoPostBack="True"
                            AppendDataBoundItems="True" CausesValidation="false" />
                        <asp:RequiredFieldValidator runat="server" ID="RfvOwnerTitle" ValidationGroup="valOwnerTitle"
                            ControlToValidate="ddlOwnerTitle" ErrorMessage="*Select an Owner Title" Text="*" Display="Dynamic"
                            SetFocusOnError="true" InitialValue="" />
                    </div>
                    <div style="display: none;">
                        <div class="pdmsLabel">
                            <asp:Label ID="Label19" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="trOwnerTitlesHelpText" runat="server" class="row">

                <div class="col-sm-12 centerblock">
                    <i>Managing Employee.</i>
                </div>
            </div>
            <div id="divAffType" style="width: 100%" runat="server">
                <div class="row" id="trAffType" runat="server">
                    <div class="col-sm-3  text-right" runat="server">
                        <asp:Label ID="lblAffliationType" runat="server" CssClass="formLabel200" Text="Affiliation Type*"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlAffliationType" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="ddlAffliationType_SelectedIndexChanged"
                            AutoPostBack="false"
                            AppendDataBoundItems="True" CausesValidation="false" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvAffliationType" ValidationGroup="valAffType"
                            ControlToValidate="ddlAffliationType" ErrorMessage="*Select an AffliationType" Text="*" Display="Dynamic"
                            SetFocusOnError="true" Enabled="false" />

                    </div>
                    <div style="display: none;">
                        <div class="pdmsLabel">
                            <asp:Label ID="Label21" runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <div id="ParentTable" runat="server">
                <uc:Address ID="ucAddress" runat="server" ValidationGroup="valOwnerInfo"></uc:Address>
            </div>

            <div id="divDOB" runat="server">
                <div class="row" id="trDOB" runat="server">
                    <div class="col-sm-3 text-right">
                        <span class="formLabel200" style="right: 5px; position: absolute">Birth Date<asp:Label runat="server" ID="lblBirthDateRequiredMarker" Visible="false">*</asp:Label></span>
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtBirthDate"
                            runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="RFVBirthDate"
                            ControlToValidate="txtBirthDate" ErrorMessage="* Birth Date is required." Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valOwnerInfo" Enabled="false" />
                        <asp:CompareValidator ID="dateValidator" runat="server" ValidationGroup="valOwnerInfo"
                            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate"
                            ErrorMessage="Select a valid Birth Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                            SetFocusOnError="true">
                        </asp:CompareValidator>
                        <asp:CompareValidator ID="cmpValBirthDateFuture" ControlToValidate="txtBirthDate" Operator="LessThan" Type="Date"
                            runat="server" ErrorMessage="* Birth Date cannot be future date and cannot reult in an age over 100 years." Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valOwnerInfo" />
                    </div>
                </div>
            </div>
            <div style="display: none;">
                <div class="pdmsLabel">
                    <asp:Label ID="lblPDMSRequestedEffectiveDate" runat="server" />
                </div>
            </div>

            <div id="divSSN" runat="server">
                <div class="row" id="trSSN" runat="server">
                    <div class="col-sm-3 text-right">
                        <asp:Label runat="server" ID="lblTaxID" CssClass="formLabel200" Text="SSN*" />
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSSN" runat="server" CssClass="formField" onKeyUp="javascript:numericOnly(this);" MaxLength="9" />
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="valOwnerInfo"
                            ControlToValidate="txtSSN" ErrorMessage="* SSN is required" Text="*" Display="Dynamic"
                            SetFocusOnError="true" Enabled="false" />
                        <asp:CustomValidator ID="cvSSN" runat="server" SetFocusOnError="True" Display="Dynamic"
                            ControlToValidate="txtSSN" ClientValidationFunction="CheckSSNLength"
                            ErrorMessage="* SSN is invalid" Text="*" ValidationGroup="valOwnerInfo" />
                    </div>
                    <div style="display: none;">
                        <div class="pdmsLabel">
                            <asp:Label ID="lblPDMSSSN" runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <div id="divPercentageOfOwnership" runat="server">
                <div class="row" id="trPercentageOwnership" runat="server">
                    <div class="col-sm-3 text-right">
                        <span class="formLabel200">Percentage of Ownership*</span>
                    </div>
                    <div class="col-sm-9">
                        <ew:NumericBox ID="nbPercentage" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="3" CssClass="formField" />
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="valOwnerInfo"
                            ControlToValidate="nbPercentage" ErrorMessage="* Enter Percentage of Ownership" Text="*" Display="Dynamic"
                            SetFocusOnError="true" />
                        
                        <asp:CompareValidator ID="cmpValPercentage" ControlToValidate="nbPercentage" Operator="LessThanEqual" Type="Currency" ValueToCompare="100"
                            runat="server" ErrorMessage="*Percentage cannot be greater than 100" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valOwnerInfo" />
                        <%--  <asp:CompareValidator ID="cmValPercentageGreaterThanZero" ControlToValidate="nbPercentage" Operator="LessThanEqual" Type="Currency" ValueToCompare="0"
                            runat="server" ErrorMessage="Percentage cannot be less than or equal to 0" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valOwnerInfo" />--%>
                         
                        <asp:CompareValidator ID="cmValPercentageGreaterThanZero" ControlToValidate="nbPercentage" Operator="GreaterThanEqual" Type="Currency"
                            ValueToCompare="1"
                            runat="server" ErrorMessage="*Percentage must be greater than 0" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valOwnerInfo" />
                    </div>
                    <div style="display: none;">
                        <div class="pdmsLabel">
                            <asp:Label ID="lblPDMSPercentage" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="divEffectiveDates" runat="server">
                <div class="row" id="trEffectiveDate" runat="server">
                    <div class="col-sm-3  text-right" runat="server">
                        <asp:Label ID="lblEffectiveDate" runat="server" CssClass="formLabel200" Text="Owner Effective Date *"></asp:Label>
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField" Enabled="true"/><ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtEffectiveDate"
                            runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvEffectiveDate" ValidationGroup="valOwnerInfo"
                            ControlToValidate="txtEffectiveDate" ErrorMessage="* Select Owner Effective Date" Text="*" Display="Dynamic"
                            SetFocusOnError="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3  text-right" runat="server">
                        <asp:Label ID="lblEndDate" runat="server" CssClass="formLabel200" Text="Owner End Date"></asp:Label>
                    </div>

                    <div class="col-sm-9">
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEndDate"
                            runat="server" />
                    </div>
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hdnRegOwnerID" runat="server" />
<asp:HiddenField ID="hdnRegAddressID" runat="server" />
<asp:HiddenField ID="hdnOwnerType" runat="server" Value="Owner" />
