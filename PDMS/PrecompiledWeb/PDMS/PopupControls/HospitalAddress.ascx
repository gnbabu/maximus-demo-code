<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospitalAddress, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/HospitalAddressHistory.ascx" TagPrefix="sh" TagName="HospitalAddressHistory" %>

<script type="text/javascript">
    function CheckPhoneLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 10)
            args.IsValid = false;
        if (test[0] === 0 || test[0] === 1 || test[3] === 0 || test[3] === 1)
            args.IsValid = false;
        return false;


    }
    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
            $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
        }

    $(document).ready(function () {
        $('#ddlLocation').change(function () {
            document.cookie = "locationTypeValue=" + $(this).val();
        });
    });
</script>
<div onmouseover="removeDisabled();">
<div id="divHospitalAddressDetails">
    <asp:ValidationSummary ID="vsHospitalAddress" runat="server" DisplayMode="List" ValidationGroup="valHospitalAddress" />
    <asp:GridView runat="server" Width="98%" ID="grdHospitalAddress" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entities found." OnRowCommand="grd_RowCommand" AllowSorting="true">
        <Columns>
            <asp:BoundField DataField="CONTACT_TYPE" HeaderText="Address Type" />
            <asp:BoundField DataField="PRACTICE_NAME" HeaderText="Organization Address Name" />
            <asp:BoundField DataField="LOCATION_TYPE_NAME" HeaderText="Location Type" />
            <asp:TemplateField ItemStyle-Width="20" ShowHeader="false">
                <ItemTemplate>
                    <asp:HiddenField ID="hdnRegAddressId" runat="server" Value='<%# Eval("REG_ADDRESS_ID")%>' />
                    <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditHospitalAddress" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>

    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddHospitalAddress" runat="server" ImageUrl="~/Images/add.png" CommandName="HospitalAddressAdd" OnCommand="lbtnAdd_Click" ToolTip="Add" />
        <br /><span aria-label="History">
            <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
            <span class="glyphicon glyphicon-book" style="padding-right:7px;"></span>History 
            </asp:LinkButton></span>

    </div>
    <asp:UpdatePanel ID="upHospitalAddressHistory" runat="server">
        <ContentTemplate>
            <div role="dialog" aria-live="assertive" aria-labelledby="dialog1Title" aria-hidden="true" id="divDialog1">
                <ajax:modalpopupextender id="mpe" runat="server" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseHistory" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2" behaviorid="mpeSplHitory" />
                <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 60%;">
                    <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
                        <div align="left">
                            &nbsp;&nbsp;<h2 id="dialog1Title"><asp:Label ID="lblSpHistoryTitle" runat="server" CssClass="history" ForeColor="White" Text="Hospital Address History" /></h2>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
                        <div class="container-fluid" style="text-align: left; padding: 15px;">
                            <div class="row">
                                <sh:hospitaladdresshistory id="ucHospitalAddressHistory" runat="server" />
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
    <div style="width: 100%; text-align: right; display: none;">
        <asp:HiddenField ID="hdnRowCount" runat="server" />
        <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
        <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
            <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
        </div>
        <telerik:RadGrid ID="grdHistory" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
            AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
            <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
                <Excel Format="Biff" />
            </ExportSettings>
            <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
                DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
                <Columns>
                    <telerik:GridBoundColumn DataField="Operation"                       HeaderText="Operation"      SortExpression="Operation"          />
                    <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
                    <telerik:GridBoundColumn DataField="PRACTICE_NAME"       HeaderText="Name"           SortExpression="PRACTICE_NAME" />
                    <telerik:GridBoundColumn DataField="LOCATION_TYPE_NAME"  HeaderText="Location Type"  SortExpression="LOCATION_TYPE_NAME" />
                    <telerik:GridBoundColumn DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
                    <telerik:GridBoundColumn DataField="EMAIL1"              HeaderText="Email Address"  SortExpression="EMAIL1" />
                    <telerik:GridBoundColumn DataField="PHONE1"              HeaderText="Phone Number"   SortExpression="PHONE1" />
                    <telerik:GridBoundColumn DataField="UserName"            HeaderText="User"           SortExpression="UserName" />
                    <telerik:GridBoundColumn DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>


    <div id="divHospitalAddress" runat="server" visible="false">

        <asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class="col-sm-3  text-right"><span class="formLabel200">Same as Practice Location</span></div>
                    <div class="col-sm-9" id="divchkchanged" runat="server">
                        <asp:CheckBox ID="prov_Same" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Same_CheckedChanged" AutoPostBack="true" Style="border: none" />
                    </div>
                </div>
                <div class="row">
                  <div class="col-sm-3  text-right"><span class="formLabel200">Override Address Validation</span></div>
                <div class="col-sm-9" id="div1" runat="server">
               <asp:CheckBox ID="prov_override" runat="server" Checked="false" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Override_CheckedChanged" AutoPostBack="true" style="border:none" />
                     </div>
                 </div>
                <div class="row" id="trLocation" runat="server">
                    <div class="col-sm-3  text-right"><span class="formLabel200">Location Type*</span></div>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="formDropDown" AutoPostBack="False" AppendDataBoundItems="True" />
                        <asp:RequiredFieldValidator runat="server" ID="rfValidatorState" ValidationGroup="valHospitalAddress"
                            ControlToValidate="ddlLocation" ErrorMessage="* Select a Location Type" Text="*" Display="Dynamic"
                            SetFocusOnError="true" InitialValue="" />
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="lblPDMSState" runat="server" />
                    </div>
                </div>
                <uc:Address ID="ucAddress" runat="server" ValidationGroup="valHospitalAddress"></uc:Address>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
<asp:TextBox ID="hidLocation" runat="server" Visible="false" />
    </div>