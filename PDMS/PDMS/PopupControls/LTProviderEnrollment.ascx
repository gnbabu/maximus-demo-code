<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_LTProviderEnrollment" Codebehind="LTProviderEnrollment.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="upLTProviderEnrollment" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <asp:ValidationSummary ID="vsLTEnrollment" runat="server" DisplayMode="List" ValidationGroup="valLTCEnrollment" />
            </div>
            <div id="divPreview" runat="server">
                <p class="text-right">* Designates a required field</p>
                <div class="row">
                    <div class="col-sm-4 text-right"><span class="formLabel200">Effective Date </span></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtLTEffectiveDate" runat="server" CssClass="formField" />
                        <ajax:calendarextender id="calLTExtender" targetcontrolid="txtLTEffectiveDate" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><span class="formLabel200">Enrollment Status </span></div>
                    <div class="col-sm-8 text-left">
                        <asp:DropDownList ID="ddlEnrollStatus" runat="server" CssClass="formDropDown">
                            <asp:ListItem Text="" Value="0" />
                            <asp:ListItem Text="Active" Value="1" />
                            <asp:ListItem Text="Inactive" Value="2" />
                            <asp:ListItem Text="REPORTING ONLY" Value="3" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><span class="formLabel200">Home Number* </span></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtLTHomeNumber" runat="server" CssClass="formField" MaxLength="4"/>
                        <asp:RegularExpressionValidator ID="valNPIFormat" runat="server" ControlToValidate="txtLTHomeNumber" ValidationExpression="^\d+?$"
                            ErrorMessage="* Enter a 4 digit Home Number." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="valLTCEnrollment" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><span class="formLabel200"></span></div>
                    <div class="col-sm-8 text-left">
                        <span class="formLabel wd170"></span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><span class="formLabel200">Comments* </span></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtLTComments" runat="server" CssClass="formField" />
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ValidationGroup="valLTProviderEnrollment" ControlToValidate="txtComments" ></asp:RequiredFieldValidator>
                        --%>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
