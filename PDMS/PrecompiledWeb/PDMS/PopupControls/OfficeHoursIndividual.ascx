<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OfficeHoursIndividual, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
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
            <asp:ValidationSummary ID="valsumOfficeHoursIndividual" runat="server" ValidationGroup="valOfficeHoursIndividual" />
        </div>
        <div id="ParentTable" runat="server">
            <br />
            <span class="pageHeader" style="text-decoration: underline">Office Information-Individual Providers</span>
            <br />
            <br />
            <div class="row">
                <div class="col-sm-6 pageHeader" style="font-size:20px">
                    <span class="formLabel200">
                        <asp:CheckBox ID="chkProviderDirectoryOpt" runat="server" CssClass="formFieldCheckBox" Style="border: none" />&nbsp;&nbsp;&nbsp;Provider Directory Opt-Out</span>
                </div>

            </div>
            <br />
            <span class="pageHeader" style="text-decoration: underline">Provider Information</span>
            <br />
            <br />
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Cultural Competencies</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadCulturalComp" runat="server" CheckBoxes="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
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
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Specialized Training</span>
                </div>
                <div class="col-sm-9">

                    <telerik:RadComboBox RenderMode="Lightweight" ID="RadSpecializedTraining" runat="server" CheckBoxes="true"
                        Width="450" EnableCheckAllItemsCheckBox="true" Skin="PDMSModern">
                    </telerik:RadComboBox>

                </div>
            </div>

             <div class="row" id="divTServices" runat="server" visible="false"> 
                <div class="col-sm-3">
                    <span class="formLabel200">Translation Services</span>
                </div>
                <div class="col-sm-3">

                    <asp:CheckBoxList runat="server" ID="chkTranslationServiceType" CausesValidation="true" RepeatDirection="Horizontal" >
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

            
            <%-- <div class="row">
                <div class="col-sm-2"><span class="formLabel200">Total Hours</span></div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtTotal" runat="server" CssClass="formFieldLarge" MaxLength="3" onKeyUp="javascript:numericOnly(this);" />
                </div>
            </div>--%>
            <br />
            <span class="pageHeader" style="text-decoration: underline">Patient Information</span>
            <br />
            <br />

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept new patients</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptNewPatients" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>

                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept new patients from referral only </span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptpatientsref" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3"><span class="formLabel200">Youngest patients accepted</span></div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtyoungestpatients" runat="server" CssClass="formFieldLarge" Style="width: 450px;" MaxLength="2" onkeypress="return IsNumeric(event);" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3"><span class="formLabel200">Oldest patients accepted</span></div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtoldestpatients" runat="server" CssClass="formFieldLarge" Style="width: 450px;" MaxLength="3" onkeypress="return IsNumeric(event);" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Gender of patient Accepted</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlGenderofPatients" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept newborn</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptnewborn" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-3">
                    <span class="formLabel200">Accept pregnant women</span>
                </div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlAcceptPregnanetwomen" runat="server" CssClass="formDropDown" RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"> </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>




        </div>
    </ContentTemplate>

</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
