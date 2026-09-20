<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerResidency" Codebehind="OwnerResidency.ascx.cs" %>
<style type="text/css"> 

        
.formLabel200 {
    right:-8px;
    position:absolute;
}
    </style> 

<div>
    <asp:ValidationSummary ID="vsOwnerOtherInfo" runat="server" DisplayMode="List" ValidationGroup="valOwnerResidency" />
</div>
<div id="ParentTable">
    <div class="row">
        <div class="col-sm-3 text-right" style="right:10px"><span class="formLabel200">Person 1*</span></div>
        <div class="col-sm-6">
            <asp:DropDownList ID="ddlOwner" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerResidency"
                ControlToValidate="ddlOwner" ErrorMessage="*Select an Owner" Text="*" Display="Dynamic"
                SetFocusOnError="true" InitialValue="" />
        </div>
        <div style="display: none;">
            <div class="pdmsLabel">
                <asp:Label ID="lblPDMSPersonEntityNo" runat="server" /></div>
        </div>
       
    </div>
</div>

<asp:HiddenField ID="hdnRegOwnerResidentFlag" runat="server" />
