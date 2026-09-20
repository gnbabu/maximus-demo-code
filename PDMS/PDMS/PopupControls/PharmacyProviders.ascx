<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PharmacyProviders" Codebehind="PharmacyProviders.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
    }

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }

    function showHideHistory() {
        if (document.getElementById('<%= pHistory.ClientID %>').style.display == 'block') {
            document.getElementById('<%= pHistory.ClientID %>').style.display = 'none';
        }
        else {
            document.getElementById('<%= pHistory.ClientID %>').style.display = 'block';
        }
    }
</script>
<div onmouseover="removeDisabled();">
<asp:panel id="upHistory" runat="server">
    <ajax:modalpopupextender id="mpeHistory" runat="server" popupcontrolid="pHistory" targetcontrolid="ButtonDummy3"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pHistory" CancelControlID="btnCloseHistory">
    </ajax:modalpopupextender>
    <asp:panel id="pHistory" runat="server" cssclass="modalPopup" style="padding: 20px; position: relative; min-width: 1400px; top: 275px !important;">
        <div>
             <asp:panel id="pnlHistoryDetails" runat="server">
                <div>
                    <asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found."
                        AllowPaging="true" AllowSorting="True" PageSize="10"
                        OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
                        <Columns>
                            <asp:BoundField DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                            <asp:BoundField DataField="PHARMACY_NAME"           HeaderText="Name"          SortExpression="PHARMACY_NAME" />
                            <asp:BoundField DataField="LICENSE_NUMBER"           HeaderText="License Number"          SortExpression="LICENSE_NUMBER" />
                            <asp:BoundField DataField="LICENSE_STATE"           HeaderText="License State"          SortExpression="LICENSE_STATE" />
                            <asp:BoundField DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                            <asp:BoundField DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
            </asp:panel>
            <asp:Button id="btnCloseHistory"  runat="server" Text="OK" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false"  />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkHistoryExcel_Click" />
        </div>
        <asp:button runat="server" id="ButtonDummy3" style="display: none" text="”ButtonDummy3”" />
    </asp:panel>
</asp:panel>
<div class="divHistoryAndAdd" style="vertical-align: middle;">
    <asp:LinkButton ID="btnHistory" runat="server" OnClientClick="javascript:showHideHistory(); return false;" AlternateText="History" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" ToolTip="History" Text="History" CausesValidation="false" Style="vertical-align: middle; color: white; text-decoration: none; padding: 4px;" />
</div>

        <div>
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:ValidationSummary ID="vsPharmacyProvider" runat="server" DisplayMode="List" ValidationGroup="valPharmacyProvider" />
              </div>
 <div class="row" id="divPharmacy">
                <div class="col-sm-8"><span class="formLabel200">Do you want to add pharmacy information? </span></div>
                <div class="col-sm-4 text-left">
                    <asp:RadioButtonList ID="rblAddPharmacy" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList" OnSelectedIndexChanged="rblAddPharmacy_SelectedIndexChanged" AutoPostback="true">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>

                </div>
            </div>
              <div style="width:auto;" id="divPharmacyProviders" runat="server" visible="false">
                <div class="row">
                     <div class="col-sm-3 text-right"><asp:label ID="lblPharmacyName" runat="server" Text ="Name of Pharmacy"  class="formLabel wd200"/></div>
                  <div class="col-sm-9 text-left">
                      <asp:TextBox ID="txtPharmacyName" runat="server" onKeyUp="javascript:alphanumericOnly(this);" CssClass="formField"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rfvPharmacyName" runat="server" ControlToValidate="txtPharmacyName" ErrorMessage="Name of Pharmacy is required." Text="*" ValidationGroup="valPharmacyProvider"></asp:RequiredFieldValidator>
                     </div>
            
                </div>

                <div class="row">
                     <div class="col-sm-3 text-right"><asp:label ID="lblChiefPharmacist" runat="server" Text ="Chief Pharmacist"  class="formLabel wd200"/></div>
                  <div class="col-sm-9 text-left">
                      <asp:TextBox ID="txtChiefPharmacist" runat="server" onKeyUp="javascript:alphanumericOnly(this);" CssClass="formField"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rfvChiefPharmacist" ControlToValidate="txtChiefPharmacist" runat="server" ErrorMessage="Chief Pharmacist is required." Text="*" ValidationGroup="valPharmacyProvider"></asp:RequiredFieldValidator>
                     </div>
      
                </div>

                  <div class="row">
                       <div class="col-sm-3 text-right"><asp:label ID="lblOccupancyPermitNumber" runat="server" Text ="Occupancy Permit Number"  class="formLabel wd200"/></div>
                  <div class="col-sm-9 text-left">
                      <asp:TextBox ID="txtOccupancyPermitNumber" runat="server" onKeyUp="javascript:alphanumericOnly(this);"  CssClass="formField"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rfvlblOccupancyPermitNumber" ControlToValidate="txtOccupancyPermitNumber" runat="server" ErrorMessage="Occupancy Permit Number is required." Text="*"  ValidationGroup="valPharmacyProvider"></asp:RequiredFieldValidator>
                     </div>
                      </div>
        </div>
<asp:PlaceHolder runat="server" 
               ID="PlaceholderPharmacyProvider"></asp:PlaceHolder>

<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
    <div style="width: 100%; text-align: right; display: none;">
        <telerik:RadGrid ID="grdHistoryExport" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
            AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
            <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
                <Excel Format="Biff" />
            </ExportSettings>
            <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
                DataKeyNames="DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
                <Columns>
                    <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                    <telerik:GridBoundColumn DataField="PHARMACY_NAME"           HeaderText="Name"          SortExpression="PHARMACY_NAME" />
                    <telerik:GridBoundColumn DataField="LICENSE_NUMBER"           HeaderText="License Number"          SortExpression="LICENSE_NUMBER" />
                    <telerik:GridBoundColumn DataField="LICENSE_STATE"           HeaderText="License State"          SortExpression="LICENSE_STATE" />
                    <telerik:GridBoundColumn DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                    <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>

</div>