<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PendingGroupAffiliations, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:UpdatePanel ID="upRegPendingAffil" runat="server"   UpdateMode="Conditional" >
    <ContentTemplate>
   
    <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" style="padding:10px;">
        <div style="width:700px;">
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:ValidationSummary ID="vsPendingGroupAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="PendingGroupAffiliations" />
            
            <div class="wdAuto" >
           <%-- <div class="row">
               <div class="col-sm-3 text-right"><asp:Label ID="dvGrpname" runat="server" class="formLabel wd120">Group Name*</asp:Label></div> 
                <div class="col-sm-9"><asp:TextBox ID="txtGroupName" runat="server" MaxLength="35" CssClass="formField wd200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtGroupName" Enabled="true" SetFocusOnError="true" 
                            Display="Dynamic" Text="*"  ValidationGroup="PendingGroupAffiliations" ErrorMessage="* Group Name is required."></asp:RequiredFieldValidator>
                </div>

            </div>--%>
           <div class="row" >
                <div class="col-sm-3 text-right"><span class="formLabel wd120">Medicaid ID</span></div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtMedicaidID" aria-label="Medicaid ID" runat="server" MaxLength="9" CssClass="formField wd200" OnTextChanged="txtMedicaidID_TextChanged" AutoPostBack="true"/>
                     <asp:RequiredFieldValidator ID="valMedReq" runat="server" ControlToValidate="txtMedicaidID" Enabled="true" SetFocusOnError="true" 
                            Display="Dynamic" Text="*"  ValidationGroup="PendingGroupAffiliations" ErrorMessage="* Please enter the Medicaid ID for the Group"></asp:RequiredFieldValidator>
                </div>                                
            </div>
            <div class="row text-center"><asp:Label ID="lblError" runat="server" Visible="false" Text="The Medicaid ID entered does not exist in the system – please try again." CssClass="failureNotification"></asp:Label></div>
            <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel wd120">NPI</span></div>
                    <div class="col-sm-9">
                         <span aria-label="NPI" tabindex="0"><asp:TextBox ID="txtNPI" runat="server" MaxLength="10" CssClass="formField wd200"  /></span>
                        <asp:RegularExpressionValidator ID="valNPIFormat" runat="server" ControlToValidate="txtNPI" ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" 
                                ErrorMessage="* Enter a 10 digit NPI that does not begin with 0."  Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="PendingGroupAffiliations"  Display="Dynamic" />
                        <asp:RequiredFieldValidator ID="valNPIReq" runat="server" ControlToValidate="txtNPI" Enabled="true" SetFocusOnError="true" 
                                Display="Dynamic" Text="*"  ValidationGroup="PendingGroupAffiliations" ErrorMessage="* Please enter the NPI for the Group."></asp:RequiredFieldValidator>
                    </div>

            </div>
           <%-- <div class="row">
                                 <div class="col-sm-3 text-right"><span class="formLabel wd120">Tax ID</span></div>
                <div class="col-sm-9"><asp:TextBox ID="txtTaxID" runat="server" MaxLength="9" CssClass="formField wd200"  />

                        <asp:RegularExpressionValidator ID="valTaxID" runat="server" ControlToValidate="txtTaxID"  ValidationExpression="(?!0{9})(?!9{9})\d{9}" ErrorMessage="* Enter a 9 digit Tax ID."  
                            Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="PendingGroupAffiliations"  Display="Dynamic" />

                </div>

            </div>--%>


        </div>



            <asp:UpdateProgress runat="server"  ID="upSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upRegPendingAffil"    >
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
                        <table><tr>
                <td><asp:Button id="btnSave"  runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PendingGroupAffiliations" /></td>
                <td><asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                            </tr>
                            </table>
            </div> 
        </div>
        <br />
        
</asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
