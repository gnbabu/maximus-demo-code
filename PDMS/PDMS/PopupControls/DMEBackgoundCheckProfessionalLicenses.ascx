<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_DMEBackgoundCheckProfessionalLicenses" Codebehind="DMEBackgoundCheckProfessionalLicenses.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
    <script type="text/javascript">
        function numericOnly(obj) {
            obj.value = obj.value.replace(/[^0-9]/g, '');
        }
        function alphabetsOnly(obj) {
            obj.value = obj.value.replace(/[^a-zA-Z]/g, '');
        }
    </script>

        <div style="width:750px;">
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:ValidationSummary ID="vsDMEBackgoundCheck" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valDMEBackgoundCheck" />
                      </div>  
            <div class="wdAuto" >
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel wd200">Name*</span></div>
                <div class="col-sm-9 text-left"><asp:TextBox ID="txtName" runat="server" MaxLength="35" CssClass="formField wd200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtName" Enabled="true" SetFocusOnError="true" 
                            Display="Dynamic" Text="*"  ValidationGroup="valDMEBackgoundCheck" ErrorMessage="* Name is required."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel wd200">Check one*</span></div>
                <div class="col-sm-9 text-left"><asp:Label ID="lblProfessional" runat="server" Text="Select whether a professional license or background check is being provided"></asp:Label>
                </div>

            </div>

        <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel wd200"></span></div>
                <div class="col-sm-9 text-left"><asp:DropDownList ID="ddlBackgroundOrProfessionalLicense" runat="server" CssClass="formField">
                <asp:ListItem Text="" Value=""></asp:ListItem>
                <asp:ListItem Text="Background Check" Value="BackgroundCheck"></asp:ListItem>
                <asp:ListItem Text="Professional License" Value="ProfessionalLicense"></asp:ListItem>
            </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBackgroundOrProfessionalLicense" Enabled="true" SetFocusOnError="true" 
                            Display="Dynamic" Text="*"  ValidationGroup="valDMEBackgoundCheck" ErrorMessage="* Background Or Professional License is required."></asp:RequiredFieldValidator> </div>

            </div>  
        </div>






        

        

