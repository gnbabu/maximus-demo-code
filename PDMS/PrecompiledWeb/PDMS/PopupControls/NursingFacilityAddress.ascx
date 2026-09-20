<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_NursingFacilityAddress, App_Web_glma3lal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
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
</script>

<script type="text/javascript">
    $(document).ready(function () {
        $('#ddlLocation').change(function () {
            document.cookie = "locationTypeValue=" + $(this).val();
        });
        
        $('#<%=ucAddress.FindControl("ddlCounty").ClientID%>').change(function () {
            document.cookie = "countyValue=" + $(this).val();
            console.log("countyValue=" , $(this).val());
        });
        
        
    });
</script>
<style>
    input[type=checkbox]
    {
        height:21px;
        width:21px;
    }
</style>
<br />
<div id="divNursingFacilityAddress">
    <asp:GridView runat="server" Width="98%" ID="grdNursingFacilityAddress" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" 
        EmptyDataText="No records found." AllowSorting="true" OnRowCommand="grd_RowCommand" >
        <Columns>
            <asp:BoundField DataField="LOCATION_TYPE_NAME" HeaderText="Location Type" />
            <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Date entered/updated" />
           <%-- <asp:BoundField DataField="SATELLITE_ADDRESS1" HeaderText="Additional Practice Address" SortExpression="SATELLITE_ADDRESS1" />--%>
            <%--<asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <asp:Literal ID="litAddress" runat="server" Text='<%# FormatAddress(Eval("ADDRESS1"),Eval("ADDRESS2"),Eval("CITY"),Eval("STATE"),Eval("ZIP"),Eval("EXT_ZIP")) %>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <%--<asp:BoundField DataField="SATELLITE_PHONE_NUMBER" HeaderText="Satellite Practice Phone Number" SortExpression="SATELLITE_PHONE_NUMBER" />--%>
            <%--<asp:TemplateField HeaderText="Phone Number">
               <ItemTemplate>
                    <asp:Literal ID="litPhone" runat="server" Text='<%# FormatPhone(Eval("PHONE1")) %>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnRegAddressId" runat="server" Value='<%# Eval("REG_ADDRESS_ID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="20" ShowHeader="false">
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="NursingFacilityAddressEdit" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" AlternateText="delete" runat="server" CommandName="NursingFacilityAddressDelete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                         ImageUrl="~/Images/cancel.png" ToolTip="Delete"/>
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
        <asp:ImageButton ID="btnAdd" runat="server" AlternateText="AddNew" ImageUrl="~/Images/add.png" CommandName="AddNusrsingFacilityAddress" OnCommand="lbtnAdd_Click" ToolTip="Add" />
        <%--<asp:ImageButton ID="btnSatellitePracticeLocationsHistory" runat="server" ImageUrl="~/Images/history_icon.jpg" CommandName="SatellitePracticeLocationsHistory" OnCommand="btnHistory_Click" ToolTip="History" />--%>
    </div>
</div>
<div id="divNursingfacilityAddressDetails" runat="server" visible="false">
    <asp:ValidationSummary ID="vsNursingFacilityAddress" runat="server" DisplayMode="List" ValidationGroup="valNursingFacilityAddress" CssClass="failureNotification" />
    <asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-3  text-right"><span class="formLabel200">Same as Practice Location</span></div>
                <div class="col-sm-9" id="divchkchanged" runat="server">
                    <asp:CheckBox ID="prov_Same" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Same_CheckedChanged" AutoPostBack="true" style="border:none" />
                </div>
            </div>
               <div class="row">
                  <div class="col-sm-3  text-right"><span class="formLabel200">Override Address Validation</span></div>
                <div class="col-sm-9" id="div1" runat="server">
               <asp:CheckBox ID="prov_override" Checked="false" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Override_CheckedChanged" AutoPostBack="true" style="border:none" />
                     </div>
                 </div>
            <div class="row" id="trLocation" runat="server">
                <div class="col-sm-3  text-right"><span class="formLabel200">Location Type*</span></div>
                <div class="col-sm-9">
                    <asp:DropDownList ID="ddlLocation" runat="server" aria-label="Location Type"  CssClass="formDropDown" AutoPostBack="True" AppendDataBoundItems="True" />
                    <asp:RequiredFieldValidator runat="server" ID="rfValidatorState" ValidationGroup="valNursingFacilityAddress"
                        ControlToValidate="ddlLocation" ErrorMessage="* Select a Location Type" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblPDMSState" runat="server" />
                </div>
            </div>
            <uc:Address ID="ucAddress" runat="server" ValidationGroup="valNursingFacilityAddress"></uc:Address>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
    <asp:TextBox ID="hidID" runat="server" Visible="false" />
    <asp:TextBox ID="hidLocation" runat="server" Visible="false" />
</div>
