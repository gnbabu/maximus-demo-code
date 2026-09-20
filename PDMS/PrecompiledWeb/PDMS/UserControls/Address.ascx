<%@ control language="C#" autoeventwireup="true" inherits="Usercontrols_Address, App_Web_address.ascx.6bb32623" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<style type="text/css">
    .modal {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }

     
    .loading {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>

<script src="../Scripts/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript">


    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
    function ShowProgress() {
        var loadDialog = $('<div></div>')
            .html('Loading, please wait...')
            .dialog({
                autoOpen: false,
                title: 'Loading',
                resizable: true,
                show: 'fade',
                hide: 'explode',
                height: 140,
                modal: true
            });
    };

    function CheckPhoneLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 10)
            return false;
        if (test[0] === 0 || test[0] === 1 || test[3] === 0 || test[3] === 1)
            return false;

        return true;
    }

    function isNumber(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    function CheckPhoneExtLength(sender, args) {

        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;
        var len = test.length;
        if (len != 5)
            return false;
        return true;
    }

    function btnConfirmWSErrorClick() {
        $("#<%=hdnAddressConfirm.ClientID%>").val("1");
        $("#<%=divWSError.ClientID%>").dialog("close");
        $("#").prop("disabled", false);
        return false;
    }

    $(document).ready(function () {
        $('#ddlCounty').select(function () {
            var storage = window.sessionStorage;
            storage.setItem("isSpecialityChanged", "True");
            document.cookie = "countyValue=" + $(this).val();
            document.getElementById("hdnCounty").value = $(this).val();
        });
        $('#ddlCounty').change(function () {
            var storage = window.sessionStorage;
            storage.setItem("isSpecialityChanged", "True");
            document.cookie = "countyValue=" + $(this).val();
            document.getElementById("hdnCounty").value = $(this).val();

        });
        $('.phone_number').inputmask('(999) 999-9999');

    });
    function checkphonenumber() {
        $('.phone_number').inputmask('(999) 999-9999');
    }
</script>

<div style="height: auto; width: auto; overflow: hidden;">

    <div class="row" id="divAddressType" runat="server" style="width: 100%">
        <div class="col-sm-4  text-right">
            <asp:Label ID="lblAddressTypeTitle" runat="server" Text="Address Type" CssClass="formLabel200" />

        </div>
        <div class="col-sm-8">
            <fieldset>
                <legend>
                    <asp:RadioButtonList ID="rblContactType" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal" Width="350px"
                        AutoPostBack="True" OnSelectedIndexChanged="rblContactType_SelectedIndexChanged">
                        <asp:ListItem Text="Individual" Value="P" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Organization" Value="B"></asp:ListItem>
                    </asp:RadioButtonList>
                </legend>
            </fieldset>
        </div>
    </div>


    <div id="divOrgName" style="width: 100%" runat="server">
        <div class="row" id="trOrgName" runat="server">
            <div class="col-sm-4  text-right" runat="server">
                <asp:Label ID="lblOrganizationName" runat="server" CssClass="formLabel200" Text="Organization Name*"></asp:Label>
            </div>
            <div class="col-sm-8">
                <%--<asp:TextBox ID="txtOrgName" runat="server" CssClass="formField" MaxLength="100" onKeyUp="javascript:alphanumericOnly(this);"/>--%>
                <asp:TextBox ID="txtOrgName" runat="server" CssClass="formField" MaxLength="100" aria-label="Organization name" aria-required="true" />
                <asp:RequiredFieldValidator runat="server" ID="reqValidatorOrgName"
                    ControlToValidate="txtOrgName" ErrorMessage="* Enter the Organization Address Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valOrgName" />
            </div>
            <div style="display: none;">
                <asp:Label ID="Label8" runat="server" />
            </div>
        </div>
    </div>
    <div class="row" id="Div1099FormName" runat="server">
        <div class="col-sm-4  text-right" runat="server">
            <asp:Label ID="Label19" runat="server" CssClass="formLabel200" Text="Name"></asp:Label>
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txt1099FormName" runat="server" CssClass="formField" MaxLength="100" onKeyUp="javascript:alphanumericOnly(this);" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                ControlToValidate="txt1099FormName" ErrorMessage="* Enter the Name" Text="*" Display="Dynamic"
                SetFocusOnError="true" ValidationGroup="valFirstName" />
        </div>
    </div>

    <div class="row" id="divTitle" runat="server">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label5" runat="server" CssClass="formLabel200" Text="Title"></asp:Label>
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtTitle" runat="server" CssClass="formField" MaxLength="100" aria-label="title" />
            <asp:RequiredFieldValidator runat="server" ID="reqValidatorTitle"
                ControlToValidate="txtTitle" ErrorMessage="* Enter the Title" Text="*" Display="Dynamic"
                SetFocusOnError="true" ValidationGroup="valTitle" />
        </div>
    </div>
    <div id="divName" style="width: 100%" runat="server">


        <div class="row" id="divFullName" runat="server">
            <div style="display: none;">
                <asp:Label ID="Label17" runat="server" />
            </div>
            <div class="col-sm-4  text-right">
                <asp:Label ID="lblFirstName" runat="server" CssClass="formLabel200" Text="First Name*"></asp:Label>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtFirst" runat="server" CssClass="formField" MaxLength="100" aria-label="first name" aria-required="true" onKeyUp="javascript:alphanumericOnly(this);" />
                <asp:RequiredFieldValidator runat="server" ID="reqValidatorFirstName"
                    ControlToValidate="txtFirst" ErrorMessage="* Enter the First Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valFirstName" />
            </div>
            <div style="display: none;">
                <asp:Label ID="Label10" runat="server" />
            </div>

            <div class="col-sm-4  text-right">
                <asp:Label ID="lblMiddleName" runat="server" CssClass="formLabel200" Text="Middle Name"></asp:Label>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtMiddle" runat="server" CssClass="formField" MaxLength="100" aria-label="Middle" onKeyUp="javascript:alphanumericOnly(this);" />
            </div>
            <div style="display: none;">
                <asp:Label ID="Label7" runat="server" />
            </div>

            <div class="col-sm-4  text-right">
                <asp:Label ID="lblLastName" runat="server" CssClass="formLabel200" Text="Last Name*"></asp:Label>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtLast" runat="server" CssClass="formField" MaxLength="100" aria-label="last name" aria-required="true" onKeyUp="javascript:alphanumericOnly(this);" />
                <asp:RequiredFieldValidator runat="server" ID="reqValidatorLastName"
                    ControlToValidate="txtLast" ErrorMessage="* Enter the Last Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valLastName" />
            </div>
            <div style="display: none;">
                <asp:Label ID="Label9" runat="server" />
            </div>

            <div style="display: none;">
                <asp:Label ID="Label6" runat="server" />
            </div>
        </div>
    </div>

    <div id="divAddressWrapper" runat="server">
        <div id="AddressTable" style="width: 100%;">
            <%-- vishwa <div class="row" id="trAddress1HelpText" runat="server"> 
                           
                 <div class="col-sm-6 text-right">
                      <asp:Label ID="lblHelpTextPOBox" runat="server" Text="Help Text: Address 1 cannot be a PO Box" />
                 </div>
                
            </div>--%>
            <div class="row" id="trAddress1" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblAddress1" runat="server" CssClass="formLabel200" Text="Address 1*"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="formField" MaxLength="60" aria-label="Address 1" aria-required="true" />
                    <asp:RequiredFieldValidator runat="server" ID="rfValidatorAddr1"
                        ControlToValidate="txtAddress1" ErrorMessage="* Enter the Address" Text="*" Display="Dynamic"
                        SetFocusOnError="true" ValidationGroup="valAddressInfo" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSStreet1" runat="server" />
                </div>
            </div>
            <div class="row" id="trAddress2" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblAddress2" runat="server" CssClass="formLabel200" Text="Address 2"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="formField" MaxLength="60" aria-label="address2" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSStreet2" runat="server" />
                </div>
            </div>
            <div class="row" id="divfloorDept" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="Label18" runat="server" CssClass="formLabel200" Text="Suite/Dept/Floor"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtFloorDept" runat="server" CssClass="formField" MaxLength="60" aria-label="Suite/Dept/Floor" onKeyUp="javascript:alphanumericOnly(this);" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblFloorDept" runat="server" />
                </div>
            </div>
            <div class="row" id="trCity" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblCity" runat="server" CssClass="formLabel200" Text="City*"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtCity" runat="server" CssClass="formField" aria-label="city" MaxLength="30" aria-required="true" />
                    <asp:RequiredFieldValidator runat="server" ID="rfValidatorCity"
                        ControlToValidate="txtCity" ErrorMessage="* Enter the City" Text="*" Display="Dynamic"
                        SetFocusOnError="true" ValidationGroup="valAddressInfo" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSCity" runat="server" />
                </div>
            </div>
            <div class="row" id="trState" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblState" runat="server" CssClass="formLabel200" Text="State*"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown"
                        AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" aria-label="State" />
                    <asp:RequiredFieldValidator runat="server" ID="rfValidatorState" ValidationGroup="valAddressInfo"
                        ControlToValidate="ddlState" ErrorMessage="* Select a State" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                    <asp:HiddenField ID="hdnBorderStateInd" Value="No" runat="server" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSState" runat="server" />
                </div>
            </div>

            <div class="row" id="divQuadWard" runat="server" visible="false">
                <div class="col-sm-4  text-right">
                    <span class="formLabel200">Quadrant</span>
                </div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlQuadrant" runat="server" CssClass="formField">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="NW" Value="NW"></asp:ListItem>
                        <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                        <asp:ListItem Text="SW" Value="SW"></asp:ListItem>
                        <asp:ListItem Text="SE" Value="SE"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvQuadrant" ValidationGroup="valAddressInfo"
                        ControlToValidate="ddlQuadrant" ErrorMessage="* Select a Quadrant" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" Enabled="false" EnableClientScript="false" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSQuadrant" runat="server" />
                </div>

                <div class="row">
                    <div class="col-sm-4  text-right">
                        <span class="formLabel200">Ward</span>
                    </div>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlWard" runat="server" CssClass="formField">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvWard" ValidationGroup="valAddressInfo"
                            ControlToValidate="ddlWard" ErrorMessage="* Select a Ward" Text="*" Display="Dynamic"
                            SetFocusOnError="true" InitialValue="" Enabled="false" EnableClientScript="false" />
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="lblPDMSWard" runat="server" />
                    </div>
                </div>
            </div>

            <div id="trCounty" class="row" runat="server">
                <div class="col-sm-4  text-right">
                    <%-- <span class="formLabel200">County*</span>--%>
                    <asp:Label ID="lblCountyreq" runat="server" CssClass="formLabel200" Text="County"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlCounty" runat="server" CssClass="formDropDown"
                        AppendDataBoundItems="True" aria-label="County" AutoPostBack="true" />
                    <asp:RequiredFieldValidator runat="server" ID="reqvalCounty" ValidationGroup="valAddressInfo"
                        ControlToValidate="ddlCounty" ErrorMessage="* County is required" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                    <%--<asp:CompareValidator runat="server" ID="reqvalCounty" ControlToValidate="ddlCounty"
                    ValueToCompare="" Type="String" ErrorMessage="* County is required."
                    Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                    ValidationGroup="valAddressInfo" Enabled="false" />--%>
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblCounty" runat="server" />
                </div>
            </div>
            <div class="row" id="trZip1" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblZip" runat="server" CssClass="formLabel200" Text="Zip*"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <ew:numericbox id="nbZipFirst5" runat="server" decimalplaces="0" positivenumber="True" maxlength="5" cssclass="formField" aria-label="Zip" aria-required="true" />
                    <asp:RequiredFieldValidator runat="server" ID="rfValidatorZip" ValidationGroup="valAddressInfo"
                        ControlToValidate="nbZipFirst5" ErrorMessage="* Enter Zip (First 5 digits)" Text="*" Display="Dynamic"
                        SetFocusOnError="true" />
                    <asp:RegularExpressionValidator ID="rfValidatorZipFormat" runat="server" ControlToValidate="nbZipFirst5" ValidationExpression="(?!0{5})(?!9{5})\d{5}$"
                        ErrorMessage="* Enter 5 digits for the Zip (First 5)" Text="*" Display="Dynamic" ValidationGroup="valAddressInfo" Enabled="false" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSZip" runat="server" />
                </div>
            </div>
            <div class="row" id="trZip1Ext" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="LblExtZip" runat="server" CssClass="formLabel200" Text="Ext Zip" Height="39px"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <ew:numericbox id="nbZipLast4" runat="server" decimalplaces="0" positivenumber="True" maxlength="4" cssclass="formField" aria-label="Ext Zip" />
                    <asp:RegularExpressionValidator ID="rfValidatorZipExtFormat" runat="server" ControlToValidate="nbZipLast4"
                        ValidationExpression="(?!0{4})(?!9{4})\d{4}$" ErrorMessage="* Enter 4 digits for the Zip (Last 4)" Text="*"
                        Display="Dynamic" ValidationGroup="valAddressInfo" />
                    <asp:RequiredFieldValidator runat="server" ID="rfValidatorZipExt" ValidationGroup="valAddressInfo"
                        ControlToValidate="nbZipLast4" ErrorMessage="* Enter Zip Ext (Last 5 digits)" Text="*" Display="Dynamic"
                        SetFocusOnError="true" Enabled="true" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSExtZip" runat="server" />
                </div>
            </div>
            <div class="row" id="trPhone1" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblPhone1" runat="server" CssClass="formLabel200" Text="Phone Number 1*"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtPhoneNo1" runat="server" CssClass="formField phone_number" aria-label="Phone number 1" aria-required="true" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvPhone1" ValidationGroup="valAddressInfo"
                        ControlToValidate="txtPhoneNo1" ErrorMessage="* Enter Phone Number 1" Text="*" Display="Dynamic"
                        SetFocusOnError="true" />
                    <asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ControlToValidate="txtPhoneNo1" ClientValidationFunction="CheckPhoneLength"
                        ErrorMessage="Enter valid Phone Number 1" Text="*" ValidationGroup="valAddressInfo" />
                </div>
            </div>
            <div class="row" id="trPhoneExt1" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="Label13" runat="server" CssClass="formLabel200" Text="Phone Ext 1"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtPhoneExt1" runat="server" CssClass="formField" aria-label="Phone Ext 1" MaxLength="5" onkeypress="return isNumber(event)" />
                    <asp:CustomValidator ID="cvPhoneExt1" runat="server" SetFocusOnError="true" Display="Dynamic"
                        ControlToValidate="txtPhoneExt1" OnServerValidate="cvPhoneExt1_ServerValidate"
                        ErrorMessage=" Phone Extension 1 cannot be more than 5 digits" ValidationGroup="valAddressInfo" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="Label14" runat="server" />
                </div>
            </div>

            <div class="row" id="trCell1" runat="server">
                <div style="width: 25%; display: inline-block;">
                    &nbsp; &nbsp; &nbsp;
                </div>
                <div style="width: auto; display: inline-block;">
                    <asp:RadioButtonList ID="rbCell1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="Yes" />
                        <asp:ListItem Value="0" Text="No" Selected="True" />
                    </asp:RadioButtonList>
                </div>
                <div style="width: 25%; display: inline-block; font-size: 12px; text-wrap: normal;">
                    <asp:Label runat="server" ID="TextRates1">Indicate this is a cell phone if you wish to receive text message. Standard text messaging and data rates may apply</asp:Label>
                </div>
            </div>
            <div style="display: none;">
                <asp:Label ID="lblPDMSPhoneNumber" runat="server" class="formFieldDisplayAuto" />
            </div>

            <div class="row" id="trPhone2" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblPhone2" runat="server" CssClass="formLabel200" Text="Phone Number 2"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtPhoneNo2" runat="server" CssClass="formField phone_number" aria-label="Phone number 2" />
                </div>
            </div>

            <div class="row" id="trPhoneExt2" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="Label15" runat="server" CssClass="formLabel200" Text="Phone Ext 2"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtPhoneExt2" runat="server" CssClass="formField" MaxLength="5" aria-label="Phone ext 2" onkeypress="return isNumber(event)" />
                    <asp:CustomValidator ID="cvtxtPhoneExt2" runat="server" SetFocusOnError="true" Display="Dynamic"
                        ControlToValidate="txtPhoneExt2" OnServerValidate="cvtxtPhoneExt2_ServerValidate"
                        ErrorMessage=" Phone Extension 2 cannot be more than 5 digits" ValidationGroup="valAddressInfo" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="Label16" runat="server" />
                </div>
            </div>
            <div class="row" id="trEffectiveDate" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="Label20" runat="server" CssClass="formLabel200" Text="Effective Date *"></asp:Label>
                </div>
                <div>
                    &nbsp;&nbsp;&nbsp;
                    <asp:TextBox aria-label="StartDate" ID="txtEffectiveDate" runat="server" CssClass="formField"></asp:TextBox>
                    <ajax:calendarextender id="calStart" targetcontrolid="txtEffectiveDate" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="reqEffDate" ValidationGroup="valAddressInfo"
                        ControlToValidate="txtEffectiveDate" ErrorMessage="*Enter a Effective Date" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                    <asp:CustomValidator ID="cvEffectiveDate" runat="server" SetFocusOnError="true" Display="Dynamic"
                        ControlToValidate="txtEffectiveDate"
                        ErrorMessage="" ValidationGroup="valAddressInfo" />
                </div>
            </div>
            <div class="row" id="trEndDate" runat="server">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="Label21" runat="server" CssClass="formLabel200" Text="End Date"></asp:Label>
                </div>
                <div>
                    &nbsp;&nbsp;&nbsp;
                    <asp:TextBox aria-label="EndDate" ID="txtEndDate" runat="server" CssClass="formField formField">12/31/2299</asp:TextBox>
                    <ajax:calendarextender id="CalendarExtender1" targetcontrolid="txtEndDate" runat="server" />
                    <asp:CustomValidator ID="cvEndDate" runat="server" SetFocusOnError="true" Display="Dynamic"
                        ControlToValidate="txtEndDate"
                        ErrorMessage="" ValidationGroup="valAddressInfo" />
                </div>
            </div>
            <div class="row" id="trCell2" runat="server">
                <div style="width: 25%; display: inline-block;">
                    &nbsp; &nbsp; &nbsp;
                </div>
                <div style="width: auto; display: inline-block;">
                    <asp:RadioButtonList ID="rbCell2" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="Yes" />
                        <asp:ListItem Value="0" Text="No" Selected="True" />
                    </asp:RadioButtonList>
                </div>
                <div style="width: 25%; display: inline-block; font-size: 12px; text-wrap: normal;">
                    <asp:Label runat="server" ID="Label4">Indicate this is a cell phone if you wish to receive text message. Standard text messaging and data rates may apply</asp:Label>
                </div>
            </div>
            <div style="display: none;">
                <asp:Label ID="Label2" runat="server" class="formFieldDisplayAuto" />
            </div>
        </div>


        <div class="row" id="trFax1" runat="server">
            <div class="col-sm-4  text-right">
                <asp:Label ID="lblFax" runat="server" Text="Fax Number 1" class="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtFaxNo1" runat="server" CssClass="formField phone_number" aria-label="Fax number 1" />
                <asp:CustomValidator ID="CustomValidator1" runat="server" SetFocusOnError="True"
                    ControlToValidate="txtFaxNo1" ClientValidationFunction="CheckPhoneLength" Display="Dynamic"
                    ErrorMessage="Enter valid Fax Number 1" Text="*" ValidationGroup="valAddressInfo" ValidateEmptyText="False" />
            </div>
        </div>
        <div class="row" id="trFax2" runat="server">
            <div class="col-sm-4  text-right">
                <asp:Label ID="lblfaxno2" runat="server" Text="Fax Number 2" class="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtFaxNo2" runat="server" CssClass="formField phone_number" aria-label="Fax number 2" />
                <asp:CustomValidator ID="CustomValidator2" runat="server" SetFocusOnError="False"
                    ControlToValidate="txtfaxno2" ClientValidationFunction="CheckPhoneLength" Display="Dynamic"
                    ErrorMessage="Enter valid Fax Number 2" Text="" ValidationGroup="valAddressInfo" ValidateEmptyText="False" />
            </div>
        </div>

        <div class="row" id="trContactName" runat="server">
            <div class="col-sm-4  text-right" runat="server">
                <asp:Label ID="Label11" runat="server" CssClass="formLabel200" Text="Contact Name"></asp:Label>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtContact" runat="server" CssClass="formField" MaxLength="100" aria-label="Contact name" onKeyUp="javascript:alphanumericOnly(this);" />
            </div>
            <div style="display: none;">
                <asp:Label ID="Label12" runat="server" />
            </div>
        </div>

        <div class="row" id="trEmail1" runat="server">
            <div class="col-sm-4  text-right">
                <asp:Label ID="lblEmail" runat="server" Text="Email Address 1*" class="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtEmail1" CssClass="formField" runat="server" MaxLength="50" aria-label="Email Address 1" aria-required="true" />
                <asp:RequiredFieldValidator runat="server" ID="rfvEmail1"
                    ControlToValidate="txtEmail1" ErrorMessage="* Enter E-mail Address" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valAddressInfo" />
                <asp:RegularExpressionValidator ID="regEmail1" runat="server" ControlToValidate="txtEmail1" Display="Dynamic" Text="*" ValidationGroup="valAddressInfo"
                    ErrorMessage="Enter valid E-mail" SetFocusOnError="true" ValidationExpression="^[A-Z'a-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$" />
            </div>
            <div style="display: none;">
                <asp:Label ID="lblPDMSEmail" runat="server" class="formFieldDisplayAuto" />
            </div>
        </div>
        <div class="row" id="trEmail2" runat="server">
            <div class="col-sm-4  text-right">
                <asp:Label ID="Label1" runat="server" Text="Email Address 2" class="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtEmail2" CssClass="formField" runat="server" MaxLength="100" aria-label="Email 2" aria-required="true" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                    ControlToValidate="txtEmail2" ErrorMessage="* Enter E-mail Address" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valAddressInfo" ValidateEmptyText="False" />
                <asp:RegularExpressionValidator ID="regEmail2" runat="server" ControlToValidate="txtEmail2" Display="Dynamic" Text="" ValidationGroup="valAddressInfo"
                    ErrorMessage="Enter valid E-mail" SetFocusOnError="true" ValidationExpression="^[A-Z'a-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$" />

            </div>
            <div style="display: none;">
                <asp:Label ID="Label3" runat="server" class="formFieldDisplayAuto" />
            </div>
        </div>

        <div class="row" id="trOfficeMgr" runat="server">
            <div class="col-sm-4  text-right">
                <asp:Label ID="lblOfficeMgr" runat="server" class="formLabel200">Office Manager</asp:Label>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtOfficeMgr" runat="server" CssClass="formField" MaxLength="100" aria-label="Office manager" />
            </div>
            <div style="display: none;">
                <asp:Label ID="lblOfficeManager" runat="server" class="formFieldDisplayAuto" />
            </div>
        </div>


        <div runat="server" id="divConfirmAddress" style="display: none; text-align: center">
            <p style="color: darkgreen">
                According to the USPS database, the address entered is inaccurate. The following address was found:
            </p>
            <p style="color: darkgreen" id="paraUSPSAddress" runat="server"></p>
            <p style="color: darkgreen">Click on 'Accept' to accept the corrections.</p>
            <br />
            <asp:Button ID="btnConfirmAddress" CssClass="buttonBoxFocus" Text="Accept" runat="server" />
            <asp:Button ID="btnCancelAddressCorrection" CssClass="buttonBox" Text="Cancel" runat="server" />
        </div>

        <div runat="server" id="divWSError" style="display: none; text-align: left">
            <p style="color: darkgreen">
                An error occurred while validating the entered address.<br />
                You can continue to work, but any addresses will not be validated by the system.<br />
                <br />
                Error details:<br />
            </p>
            <p style="color: darkgreen" id="paraWSError" runat="server"></p>
            <asp:Button ID="btnConfirmWSError" class="buttonBox" Text="Ok" runat="server" OnClientClick="btnConfirmWSErrorClick(); return false;" />
        </div>
        <div runat="server" id="divAddressDuplicate" visible="false">
            <p style="color: red">
                This address already exists
            </p>
        </div>
    </div>
</div>

<asp:CustomValidator ID="cvAddress"
    ControlToValidate=""
    OnServerValidate="cvAddress_ServerValidate"
    Display="None"
    ErrorMessage=""
    ValidationGroup="valAddressInfo"
    runat="server" />

<script type="text/javascript">
    function maxZIndex() {
        var highest = -999;

        $("*").each(function () {
            var current = parseInt($(this).css("z-index"), 10);
            if (current && highest < current)
                highest = current;
        });

        return highest;
    }

    jQuery.expr[':'].contains = function (a, i, m) {
        return jQuery(a).text().toUpperCase()
            .indexOf(m[3].toUpperCase()) >= 0;
    };

    function sendData(streetAddress, unitAddress, floorDept, quadrant, city, state, county, zip5, zip4) {
        $("#<%= txtAddress1.ClientID %>").first().val(streetAddress);
        $("#<%= txtAddress2.ClientID %>").first().val(unitAddress);
        $("#<%= txtFloorDept.ClientID %>").first().val(floorDept);
        $("#<%= ddlQuadrant.ClientID %>").first().val(quadrant);
        $("#<%= txtCity.ClientID %>").first().val(city);
        if ($("#<%= ddlState.ClientID %>").first().val() !== state) {
            $("#<%= ddlState.ClientID %>").first().val(state);
            $("#<%= ddlState.ClientID %>").change();
        }
        $('#<%= ddlState.ClientID %> option:contains(' + state + ')').attr("selected", "selected");

       <%-- $("#<%= ddlCounty.ClientID %> option").filter(function () {
            return $(this).text().toUpperCase() === county.toUpperCase();
        }).prop('selected', true);--%>
<%--        $('#<%= ddlCounty.ClientID %> option:selected').text(county);--%>      
        $('#<%= ddlCounty.ClientID %> option:contains(' + county + ')').attr("selected", "selected");
        document.cookie = "countydisplay=" + $("#<%= ddlCounty.ClientID %>").val();
        $("#<%= nbZipFirst5.ClientID %>").first().val(zip5);
        $("#<%= nbZipLast4.ClientID %>").first().val(zip4);
    }

    function allowSave() {
        $("#<%= SaveButtonClientID %>").prop("disabled", false);
    }

    function setAddressConfirm(confirmValue) {
        var hdnAddressConfirm = document.getElementById("<% = hdnAddressConfirm.ClientID %>");
        hdnAddressConfirm.value = confirmValue;
    }
</script>
<asp:HiddenField ID="hdnContactType" runat="server" Value="" />
<asp:HiddenField ID="hdnAddressConfirm" runat="server" Value="0" />
<asp:HiddenField ID="hdnLongitude" runat="server" Value="" />
<asp:HiddenField ID="hdnLatitude" runat="server" Value="" />
<asp:HiddenField ID="hdnSaveButtonClientID" runat="server" Value="" />
<asp:HiddenField ID="hdnCounty" runat="server" Value="" />

