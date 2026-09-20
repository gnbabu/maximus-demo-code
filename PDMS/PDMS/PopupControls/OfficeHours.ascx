<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OfficeHours" Codebehind="OfficeHours.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
     function alphanumericOnly(obj) {
            obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
        } 
    function IsNumeric(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
    function findTotal() {
        var arr = $('[id^=OFFICE]')
        //document.getElementsByName('qty');
        var tot = 0;
        for (var i = 0; i < arr.length; i++) {
            if (parseInt(arr[i].value))
                tot += parseInt(arr[i].value);
        }
        //document.getElementById('total').value = tot;
        alert(tot);
    }

</script>
<style type="text/css">
    .formLabel200 {
        width: auto !important;
        white-space: normal !important;
        text-align: left !important;
    }
</style>
<asp:UpdatePanel ID="upOffice" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <asp:ValidationSummary ID="valsumOfficeHours" runat="server" ValidationGroup="valOfficeHours" />
        </div>
        <div id="ParentTable" runat="server">


            <br />
            <span class="pageHeader" style="text-decoration: underline">Office Information-Group Providers</span>
            <br />
            <br />
            <div class="row">
                <div class="col-sm-6 pageHeader" style="font-size:20px">
                    <span class="formLabel200">
                        <asp:CheckBox ID="chkProviderDirectoryOpt" runat="server" CssClass="formFieldCheckBox" Style="border: none" />&nbsp;&nbsp;&nbsp;Provider Directory Opt-Out</span>
                </div>

            </div>
            <br />
            <span class="pageHeader" style="text-decoration: underline">Hours of Operation</span>
             <asp:Label ID="officeHelp" runat="server" Text="*Hours providers available for appointments" ForeColor="Red" ></asp:Label>

            <br />
            <br />
            <div class="row">
                <div class="col-sm-2 "><span class="formLabel200">Monday</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtMon" runat="server" CssClass="formFieldLarge" onkeypress="return alphanumericOnly(event);" MaxLength="8" />
                    <%--<asp:RegularExpressionValidator ID="valTaxIdFormat" runat="server" ControlToValidate="txtMon"
                                                ValidationExpression="^[\d -]+$" ErrorMessage="* Please enter hours in this format e.g. 8-5. Numbers and dash only "  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="valOfficeHours"  Display="Dynamic" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Tuesday</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtTue" runat="server" CssClass="formFieldLarge" onkeypress="return alphanumericOnly(event);" MaxLength="8" />
                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTue"
                                                ValidationExpression="^[\d -]+$" ErrorMessage="* Please enter hours in this format e.g. 8-5. Numbers and dash only "  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="valOfficeHours"  Display="Dynamic" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Wednesday</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtWed" runat="server" CssClass="formFieldLarge" onkeypress="return alphanumericOnly(event);" MaxLength="8" />
                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtWed"
                                                ValidationExpression="^[\d -]+$" ErrorMessage="* Please enter hours in this format e.g. 8-5. Numbers and dash only "  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="valOfficeHours"  Display="Dynamic" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Thursday</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtThu" runat="server" CssClass="formFieldLarge" onkeypress="return alphanumericOnly(event);" MaxLength="8" />
                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtThu"
                                                ValidationExpression="^[\d -]+$" ErrorMessage="* Please enter hours in this format e.g. 8-5. Numbers and dash only "  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="valOfficeHours"  Display="Dynamic" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Friday</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtFri" runat="server" CssClass="formFieldLarge" onkeypress="return alphanumericOnly(event);" MaxLength="8" />
                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtFri"
                                                ValidationExpression="^[\d -]+$" ErrorMessage="* Please enter hours in this format e.g. 8-5. Numbers and dash only "  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="valOfficeHours"  Display="Dynamic" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Saturday</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtSat" runat="server" CssClass="formFieldLarge" onkeypress="return alphanumericOnly(event);" MaxLength="8" />
                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtSat"
                                                ValidationExpression="^[\d -]+$" ErrorMessage="* Please enter hours in this format e.g. 8-5. Numbers and dash only "  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="valOfficeHours"  Display="Dynamic" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Sunday</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtSun" runat="server" CssClass="formFieldLarge" onkeypress="return alphanumericOnly(event);" MaxLength="8" />
                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtSun"
                                                ValidationExpression="^[\d -]+$" ErrorMessage="* Please enter hours in this format e.g. 8-5. Numbers and dash only "  
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="valOfficeHours"  Display="Dynamic" />--%>
                </div>
            </div>
            <%-- <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Total Hours</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtTotal" runat="server" CssClass="formFieldLarge" MaxLength="3" onKeyUp="javascript:numericOnly(this);" />
                </div>
            </div>--%>
            <br />
            <span class="pageHeader" style="text-decoration: underline">Office Information</span>
            <br />
            <br />

            <div class="row">
                <div class="col-sm-3"><span class="formLabel200">Website</span></div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="formFieldLarge" Style="width: 450px;" MaxLength="100" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">24-hour telephone coverage</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTel" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Public transportation access</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTrans" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Electronic billing</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlEbilling" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">TDD/TDY</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTDD" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">ASL Offered</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlASL" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>



            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">ADA Accommodations</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadADAAccommodation" runat="server" CheckBoxes="true" 
                        Width="450" EnableCheckAllItemsCheckBox="true" DataTextField="Description" Skin="PDMSModern"
                        DataValueField="ADAAccomomodationId">
                    </telerik:RadComboBox>

                </div>
            </div>
             <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Languages Spoken</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadLanguagesSpoken" runat="server" CheckBoxes="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>

                </div>
            </div>
            <div class="row" id="divTServices" runat="server"> 
                <div class="col-sm-3">
                    <span class="formLabel200">Translation Services</span>
                </div>
                <div class="col-sm-3">
                    <asp:CheckBoxList runat="server" ID="chkTranslationServiceType" CausesValidation="true" RepeatDirection="vertical" >
                       <asp:ListItem Text="Language Line" Value="1"></asp:ListItem>
                       <asp:ListItem Text="Translation" Value="2"></asp:ListItem> 
                    </asp:CheckBoxList>
                </div>
            </div>


            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Telehealth Offered</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlTelehealth" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Unknown" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">CHIP</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlCHIP" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Unknown" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accepts New Medicaid Patients</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlNewMedicaid" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Unknown" Value="" />
                        <asp:ListItem Text="Yes" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
       
        </div>
    </ContentTemplate>

</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
