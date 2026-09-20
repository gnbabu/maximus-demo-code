<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PharmacyPharmacist" Codebehind="PharmacyPharmacist.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9 ]/g, '');
    }
</script>

<div class="divGrid" style="margin-top: 15px;">
    <telerik:RadGrid RenderMode="Lightweight" ID="rgPharmacist" runat="server" Width="100%" OnItemCommand="rgPharmacist_ItemCommand" AllowAutomaticInserts="false" AutoGenerateColumns="false" EnableAjaxSkinRendering="false" EnableEmbeddedBaseStylesheet="false">
        <HeaderStyle CssClass="gridViewHeader" Font-Bold="true" />
        <PagerStyle Mode="NumericPages" CssClass="gridViewPager" />
        <ItemStyle CssClass="gridViewRow" />
        <AlternatingItemStyle CssClass="gridViewAltRow" />
        <MasterTableView AllowSorting="true" PageSize="15" AllowPaging="True" Width="100%" AutoGenerateColumns="false" Visible="true">
            <Columns>
                <telerik:GridBoundColumn DataField="PHARMACIST_NAME" HeaderText="Pharmacist Name" UniqueName="PHARMACIST_NAME">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="LICENSE_NUMBER" HeaderText="License Number" UniqueName="LICENSE_NUMBER">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="LICENSE_STATE" HeaderText="License State" UniqueName="LICENSE_STATE">
                </telerik:GridBoundColumn>
                  <telerik:GridTemplateColumn ItemStyle-Width="2%">
                     <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditPharmacistRow" CommandArgument="<%# Container.RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeletePharmacistRow" CommandArgument="<%# Container.RowIndex %>" ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete" Visible="<%# CanUserViewDelete()  %>"/>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
    <div id="divAddPharmacists" runat="server" class="divHistoryAndAdd">
        <div style="margin-bottom: 5px; margin-left: 10px; float: left;">
            <b>
                <asp:Label ID="lblAddPharmacist" runat="server">Add Pharmacist</asp:Label></b>
        </div>
        <asp:ImageButton ID="btnAddPharPharmacists" runat="server" ImageUrl="~/Images/add.png" CommandName="Pharmacist" OnCommand="lbtnAdd_Click" ToolTip="Add" />
    </div>

<div id="pharmacistDetail" runat="server" visible="false">
    <div>
        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
        <asp:ValidationSummary ID="vsPharmacist" runat="server" DisplayMode="List" ValidationGroup="valPharmacist" />
        <asp:ValidationSummary ID="MiscValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valPharmacist" />
    </div>
    <div style="width:auto;">
        <div class="row">
            <div class="col-sm-3 text-right"><asp:label ID="lblPharmacistName" runat="server" Text ="Pharmacist Name"  class="formLabel wd200"/></div>
            <div class="col-sm-9 text-left">
                <asp:TextBox ID="txtPHARMACIST_NAME" runat="server" CssClass="formField"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rvtxtPHARMACIST_NAME" runat="server" ControlToValidate="txtPHARMACIST_NAME" Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="valPharmacist" ErrorMessage="* Pharmacist name is required."></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right">
                <asp:Label ID="lblLicenseNumber" runat="server" Text="License Number" class="formLabel wd200" />
            </div>
            <div class="col-sm-9 text-left">
                <asp:TextBox ID="txtLICENSENUMBER" runat="server" onKeyUp="javascript:alphanumericOnly(this);" MaxLength="10" CssClass="formField" />
                <asp:RequiredFieldValidator ID="rfvLICENSENUMBER" runat="server" ControlToValidate="txtLICENSENUMBER" Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="valPharmacist" ErrorMessage="* Please enter License Number"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cv1" runat="server" ControlToValidate="txtLICENSENUMBER" OnServerValidate="cv1_ServerValidate" Display="Static" ValidationGroup="valPharmacist" ErrorMessage="* License Number can't be more than 10 characters." Text="*" />
                <asp:RegularExpressionValidator ID="retxtLICENSENUMBER" runat="server" ControlToValidate="txtLICENSENUMBER" ValidationExpression="^[a-zA-Z0-9]+$" ErrorMessage="* Enter only alphabets or numbers for License Number." Text="*" Display="Dynamic" ValidationGroup="valPharmacist" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right"><asp:label ID="lblLicenseState" runat="server" Text ="License State"  class="formLabel wd200"/></div>
            <div class="col-sm-9 text-left">
                <asp:DropDownList ID="ddlState" runat="server" AppendDataBoundItems="true" DataTextField="StateName" DataValueField="StateId"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlState" Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="valPharmacist" ErrorMessage="* State is required."></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="PlaceholderUploadPharmacist"></asp:PlaceHolder>

    <asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
    <asp:TextBox ID="hidID" runat="server" Visible="false" />
</div>