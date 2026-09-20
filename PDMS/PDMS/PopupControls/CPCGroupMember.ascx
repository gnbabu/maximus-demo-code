<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CPCGroupMember" Codebehind="CPCGroupMember.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<script type="text/javascript">
        function alphanumericOnly(obj) {
            obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
        } 
    

</script>
    <div id="AddressTable">
        <asp:UpdatePanel ID="upRegAffil" runat="server" UpdateMode="Conditional">

            <ContentTemplate>

                <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
                    <div>
                        <div>
                            <asp:ValidationSummary ID="vsCPCGroupMember" runat="server" DisplayMode="List" ValidationGroup="CPCGroupMember" />
                        </div>
                        
                        <div class="wdAuto">

                            <div class="row">
                                <div class="col-sm-9">
                                    <p style="color: red;">
                                        Enter Either the CPC ID or Mediaid ID for the provider you want to add to your Practice Partnership
                                 
                                    </p>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabel wd120">
                                        <asp:RadioButtonList ID="rblMedicaidId" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rblMedicaidId_SelectedIndexChanged">
                                            <asp:ListItem Text="Medicaid Id" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="CPC ID" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>*</span>

                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                                        ControlToValidate="rblMedicaidId" ErrorMessage="*  Please Select either Medicaid ID or CPC ID" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" ValidationGroup="CPCGroupMember" />

                                </div><br />
                                <div class="col-sm-9 text-left">
                                <asp:TextBox ID="txtMedicaidID" runat="server" CssClass="formField" onKeyUp="javascript:alphanumericOnly(this);"  OnTextChanged="txtMedicaidID_TextChanged" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                       <asp:HiddenField ID="hdnMemberRegId" runat="server" />
                                    <asp:RequiredFieldValidator runat="server" ID="reqPrimaryContactName"
                                        ControlToValidate="txtMedicaidID" ErrorMessage="* Enter Medicaid ID Name" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" ValidationGroup="CPCGroupMember" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd120">Start Date*</span></div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="35" CssClass="formField" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                                        ControlToValidate="txtStartDate" ErrorMessage="* Enter Start Date" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" ValidationGroup="CPCGroupMember" />
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd120">Member Name*</span></div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtMemberName" runat="server" MaxLength="35" CssClass="formField" Enabled="false"></asp:TextBox>
                          
                                </div>
                            </div>

                        </div>
                        <div>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                        <br />
                        <asp:UpdateProgress runat="server" ID="upSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upRegAffil">
                            <ProgressTemplate>
                                <div class="loading">
                                    <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                        <div id="divAffiliationSaveBox" class="btnBox text-center" runat="server">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus"
                                OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="CPCGroupMember" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
                                OnClick="btnCancel_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <br />

                    <div runat="server" id="divConfirmGroupAffiliation" style="text-align: center">
                        <p style="color: red">
                            <asp:Label runat="server" ID="lblmessage" Text=""></asp:Label>
                        </p>
                    </div>
                    <asp:HiddenField ID="hdnMemTotalAttribution" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnMemKidsAttribution" runat="server" Value="0" />
                    <asp:HiddenField ID ="hdnMemMedicaidID" runat="server" Value="0" />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

