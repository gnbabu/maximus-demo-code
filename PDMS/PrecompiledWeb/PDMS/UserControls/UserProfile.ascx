<%@ control language="C#" autoeventwireup="true" inherits="UserControls_UserProfile, App_Web_p4ixifjm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>


<style type="text/css">
 /*#ctl00_ContentPlaceHolder1_mode_divFieldComboBox>.fieldTable > tbody > tr > td:nth-child(1)
        {
            width:80%;
        }
        
        .fieldTable>tbody>tr>td:nth-child(1)
        {
            width:50%;
        }

        .formLabel
        {
            font-weight:bold;
        }
        
        #ctl00_ContentPlaceHolder1_pnlCreateUser>div>div>.formField
        {
            border:solid 1px black!important;
        }
        
        .listButtons
        {
            font-size:8pt;
            width:100px;
        }
        
        .listButtonsBelow
        {
            font-size:8pt;
            width:50px;
        }
        
        #ctl00_ContentPlaceHolder1_radioRole
        {
            width:80%;
            margin-left:auto;
            margin-right:auto;
        }
        
      
     
        .pnlWidth 
        {
            width:760px;
        }
        .createAccount
        {
            display:inline-block;
            /*width: 80%;*/
        /*}*/
        /*table.tblButtons td 
        {
            padding: 1px;
        }
        .divPadding 
        {
            padding-left:120px;
        }*/
 
        .divUserProfile {
          width:900px !important;
        max-width:950px;
        font-size:medium;
        overflow:hidden;
        height:auto;
        /*border:10px dotted black;
        background:lightblue;*/
    }
      
</style>
<div class="WhiteBox" style="text-align:center;">

<asp:Panel ID="pnlCreateUser" runat="server" >

<asp:Label runat="server" id="lblProfileTitle" Text="<h3>User Account</h3>"></asp:Label>
 
<asp:UpdatePanel ID="upUser" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                        <br /> <div class="boxContainer"><uc2:Separator ID="ucSep2" runat="server" Header="Please enter your contact information." Mode="1" /></div><br />
                          <div style="text-align: left; margin-left: 20%;" class="tablepad">
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                <span class="formLabel"><asp:Label ID="lblCname" runat="server" Text="Contact Name*" /></span>
                                 </div>
                             <div class="col-sm-8 text-left">
                                <asp:TextBox ID="txtContactName" runat="server" MaxLength="50" CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtContactName" ErrorMessage="* Contact Name is required" Text="* Contact Name is required" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" />               
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="* Contact Name: Invalid character found" ErrorMessage="* Contact Name: Invalid character found" ControlToValidate="txtContactName" SetFocusOnError="true" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z''-'\s]{1,50}$" ValidationGroup="AdminUserAccounts" />
                            </div>
                             </div>
                            <div class="row">
                                 <div class="col-sm-4  text-right">
                                <span class="formLabel"><asp:Label ID="lblTitle" runat="server" Text="Title*" /></span>
                                 </div>
                             <div class="col-sm-8 text-left">
                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator	runat="server" ID="RequiredFieldValidator6" Text="* Title is required" ControlToValidate="txtTitle" ErrorMessage="* Title is required" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="* Title: Invalid character found"     ErrorMessage="* Title: Invalid character found" ControlToValidate="txtTitle" SetFocusOnError="true" Display="Dynamic"     ValidationExpression="^[a-zA-Z''-'\s]{1,50}$" ValidationGroup="AdminUserAccounts" />
                             </div>
                             </div>
                            <div class="row">
                                <div class="col-sm-4  text-right">
                                <span class="formLabel"><asp:Label ID="lblPhNum" runat="server" Text="Phone Number*" /></span>
                                 </div>
                             <div class="col-sm-8 text-left">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="formField" />
                                <ajax:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False" ClearMaskOnLostFocus="False" TargetControlID="txtPhone" MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                        <asp:CustomValidator ID="cvPhone" runat="server" OnServerValidate="Validate_cvPhoneRequired" Display="Dynamic" ValidationGroup="AdminUserAccounts" ErrorMessage="* Phone Number is required" Text="* Phone Number is required" />
                                <asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" ControlToValidate="txtPhone" ClientValidationFunction="CheckPhoneLength" ErrorMessage="* Enter valid Phone Number" Text="* Enter valid Phone Number" ValidationGroup="AdminUserAccounts" /> 
                             </div>
                             </div>
                            <div class="row">
                                 <div class="col-sm-4  text-right">
                                <span class="formLabel"><asp:Label ID="lblext" runat="server" Text="Extension" /></span>
                                      </div>
                             <div class="col-sm-8 text-left">
                                <asp:TextBox ID="txtPhoneExt" runat="server" MaxLength="10" CssClass="formField"></asp:TextBox>
                                 </div>
                            </div>
                            <div class="row">
                               <div class="col-sm-4  text-right">
                                <span class="formLabel"><asp:Label ID="lblemail" runat="server" Text="Email Address*" /></span>
                               </div>
                             <div class="col-sm-8 text-left">

                                <asp:TextBox runat="server" ID="Email" CssClass="formField" />
                                <asp:CustomValidator ID="cvEmailRequired" runat="server" OnServerValidate="Validate_cvEmailRequired"
                                    Display="Dynamic" ValidationGroup="AdminUserAccounts" ErrorMessage="* Email is required."
                                    Text="* Email is required." />
                                <asp:CustomValidator ID="cvEmailFormat" runat="server" OnServerValidate="Validate_cvEmailFormat"
                                    Display="Dynamic" ValidationGroup="AdminUserAccounts" ErrorMessage="* Invalid email format."
                                    Text="* Invalid email format." />
                                 </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4  text-right">
                                <span class="formLabel"><asp:Label ID="lblconfirmEmail" runat="server" Text="Confirm Email" /></span>
                                </div>
                             <div class="col-sm-8 text-left">
                                <asp:TextBox runat="server" ID="ConfirmEmail" CssClass="formField" />
                                <asp:CustomValidator ID="cvCEmailMatch" runat="server" OnServerValidate="Validate_cvCEmailMatch"
                                    Display="Dynamic" ValidationGroup="AdminUserAccounts" ErrorMessage="* Email addresses must match."
                                    Text="* Email addresses must match." />
                                 </div>
                            </div>
                            <div id="divActive" runat="server">
                            <table border="0">
                                <tr>
                                    <td><span class="formLabel"><asp:Label ID="lblActive" runat="server" Text="Active" /></span></td>
                                    <td>
                                        <asp:RadioButtonList ID="rblIsApproved" BorderStyle="None" CellPadding="0" CellSpacing="0" 
                                            RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="RadioList">  
                                            <asp:ListItem Value="true">True</asp:ListItem>  
                                            <asp:ListItem Value="false">False</asp:ListItem>  
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div style="float:right;"><asp:Button ID="btnUnlockUser" runat="server" Text="Unlock User" OnClick="btnUnlockUser_Click" CssClass="buttonBox" style="margin-top: 20px;" /></div>
                                    </td>
                                </tr>
                            </table>
                                </div>
                        </div>
                        <br /> <div class="boxContainer"><uc2:Separator ID="ucSep3" runat="server" Header="Create your user id and password." Mode="1" /></div><br />
                        <div style="text-align: left; margin-left: 20%;" class="tablepad">
                        <div class="row">
                            <div class="col-sm-4 text-right">
                            <span class="formLabel"><asp:Label ID="lbluserId" runat="server" Text="User ID*" /></span>
                                 </div>
                             <div class="col-sm-8 text-left">
                            <asp:TextBox runat="server" ID="UserName" CssClass="formField" MaxLength="50" ToolTip="User Id" /> 
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="UserName" Display="Dynamic" ErrorMessage="* Username is required" SetFocusOnError="true" Text="* Username is required" ValidationGroup="AdminUserAccounts" />
                            <asp:CustomValidator ID="cvUserName" runat="server" ControlToValidate="UserName" OnServerValidate="UserName_Validating" Display="Dynamic" ValidationGroup="AdminUserAccounts" Text="* User ID:<ul style=&quot;margin-top:-5px!important&quot;><li>May contain uppercase letters</li><li>May contain lowercase letters</li><li>May contain numbers</li><li>May contain @ symbols and periods</li><li>At least on character long</li><li>At most 50 characters long</li></ul>" ErrorMessage="* User ID:<ul style=&quot;margin-top:-5px!important&quot;><li>May contain uppercase letters</li><li>May contain lowercase letters</li><li>May contain numbers</li><li>May contain @ symbols and periods</li><li>At least on character long</li><li>At most 50 characters long</li></ul>" />
                         </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4 text-right">
                               <span class="formLabel"><asp:Label ID="lblOldPwd" runat="server" Text="Old Password*" /></span>
                            </div>
                             <div class="col-sm-8 text-left">
                                <asp:TextBox runat="server" ID="OldPassword" TextMode="Password" CssClass="formField" MaxLength="50"  ToolTip="Please enter old password" /> <br />
                                <asp:RequiredFieldValidator runat="server" ID="RequiredOldPassword" ControlToValidate="OldPassword" Text="*Old Password is required" ErrorMessage="*Old Password is required" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" Display="Dynamic" />
                            
                                <asp:CustomValidator ID="cvOldPassword" runat="server" ControlToValidate="OldPassword" OnServerValidate="Password_Validating" Display="Dynamic" ValidationGroup="AdminUserAccounts" Text="* Password:<ul style=&quot;margin-top:-5px!important&quot;><li>at least 8 characters</li><li>at most 10 characters</li><li>at least one lowercase letter</li><li>at least one uppercase letter</li><li>at least one number</li><li>at least one symbol or space !@#$%^&*()-_+=;:'&quot;/[]{},.<>|`</li></ul>" ErrorMessage="* Password:<ul style=&quot;margin-top:-5px!important&quot;><li>at least 8 characters</li><li>at most 10 characters</li><li>at least one lowercase letter</li><li>at least one uppercase letter</li><li>at least one number</li><li>at least one symbol or space !@#$%^&*()-_+=;:'&quot;/[]{},.<>|`</li></ul>" />
                             </div>
                        </div>
                        <div class="row">
                              <div class="col-sm-4 text-right">
                            <span class="formLabel"><asp:Label ID="lblpwd" runat="server" Text="Password*" /></span>
                              </div>
                             <div class="col-sm-8 text-left">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="formField" MaxLength="10" ToolTip="Must contain at least one number and one uppercase and lowercase letter and one non-alphanumeric character, and at least 8 to 10 characters" /> <br />
                            <asp:Label ID="PasswordExists" runat="server" Visible="false" Text="" CssClass="failureNotification" style="color:Red" />
                            <asp:RequiredFieldValidator runat="server" ID="RequirePassword" ControlToValidate="Password" Text="* Password is required" ErrorMessage="* Password is required" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="valPasswordFormat" runat="server" ValidationGroup="AdminUserAccounts"  
                                            ControlToValidate="Password"
                                            ValidationExpression="^(?=.*[0-9])(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[!@#$%\^&*\(\)\-_+=;:'\/\[\]{},.<>|`]).{8,20}$" 
                                            ErrorMessage="<div class='pwd-val-error'>*Password is invalid. Password requirements:<ul><li>Between 8 and 20 characters</li><li>Contain at least one non-alphanumeric character</li><li>Contain at least one lowercase letter</li><li>Contain at least one uppercase letter</li></ul></div>" Text="<div class='pwd-val-error'>*Password is invalid. Password requirements:<ul><li>Between 8 and 10 characters</li><li>Contain at least one non-alphanumeric character</li><li>Contain at least one lowercase letter</li><li>Contain at least one uppercase letter</li></ul></div>" Display="Dynamic" />
                            <asp:CustomValidator ID="cvPassword" runat="server" ControlToValidate="Password" OnServerValidate="Password_Validating" Display="Dynamic" ValidationGroup="AdminUserAccounts" Text="* Password:<ul style=&quot;margin-top:-5px!important&quot;><li>at least 8 characters</li><li>at most 10 characters</li><li>at least one lowercase letter</li><li>at least one uppercase letter</li><li>at least one number</li><li>at least one symbol or space !@#$%^&*()-_+=;:'&quot;/[]{},.<>|`</li></ul>" ErrorMessage="* Password:<ul style=&quot;margin-top:-5px!important&quot;><li>at least 8 characters</li><li>at most 10 characters</li><li>at least one lowercase letter</li><li>at least one uppercase letter</li><li>at least one number</li><li>at least one symbol or space !@#$%^&*()-_+=;:'&quot;/[]{},.<>|`</li></ul>" />
                           </div>
                       </div>
                        <div class="row">
                            <div class="col-sm-4 text-right">
                              <span class="formLabel"><asp:Label ID="lblconfirmpwd" runat="server" Text="Confirm Password" /></span>
                             </div>
                             <div class="col-sm-8 text-left">
                                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" MaxLength="10" CssClass="formField" />
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="* The Password and Confirmation Password must match" SetFocusOnError="true" Text="* The Password and Confirmation Password must match" ValidationGroup="AdminUserAccounts" />
                             </div>
                        </div>
                        <div>
                             
                        </div>
                        </div>
                        <br /> <div class="boxContainer"><uc2:Separator ID="ucSep4" runat="server" Header="Answer your security question." Mode="1" /></div><br />
                        <div style="margin-left: auto; margin-right:auto;">

                            <div style="text-align: left; margin-left: 20%;" class="tablepad">
                                <div class="row">
                                     <div class="col-sm-4 text-right"><span class="formLabel"><asp:Label ID="lblPasswordQuestion1" runat="server" AssociatedControlID="ddlPasswordQuestion1" Text="Security Question*" /></span></div>
                                     <div class="col-sm-8 text-left">
                                        <asp:UpdatePanel ID="upSQ1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList runat="server" ID="ddlPasswordQuestion1" OnSelectedIndexChanged="ddlPasswordQuestion1_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="ddlPasswordQuestion1" 
                                            ValueToCompare="0" Type="String" ErrorMessage="* Security Question is required" Operator="NotEqual" 
                                            SetFocusOnError="true" Display="Dynamic" Text="* Security Question is required" ValidationGroup="AdminUserAccounts" />                                       
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-right"><span class="formLabel"><asp:Label ID="lblAnswer1" runat="server" AssociatedControlID="Answer1" Text="Answer*" /></span></div>
                                    <div class="col-sm-8 text-left">
                                        <asp:TextBox runat="server" ID="Answer1" CssClass="formField" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="Answer1" ErrorMessage="* Answer is required" SetFocusOnError="true" Text="* Answer is required" ValidationGroup="AdminUserAccounts" Display="Dynamic" />
                                    </div>
                                </div>
                               <div class="row">
                                    <div class="col-sm-4 text-right"><span class="formLabel"><asp:Label ID="lblPasswordQuestion2" runat="server" AssociatedControlID="ddlPasswordQuestion2" Text="Security Question*" /></span></div>
                                    <div class="col-sm-8 text-left">
                                        <asp:UpdatePanel ID="upSQ2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList runat="server" ID="ddlPasswordQuestion2"  OnSelectedIndexChanged="ddlPasswordQuestion2_SelectedIndexChanged" AutoPostBack="true" >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:CompareValidator runat="server" ID="CompareValidator2" ControlToValidate="ddlPasswordQuestion2" 
                                            ValueToCompare="0" Type="String" ErrorMessage="* Security Question is required" Operator="NotEqual" 
                                            SetFocusOnError="true" Display="Dynamic" Text="* Security Question is required" ValidationGroup="AdminUserAccounts" />
                                        <asp:TextBox ID="TextBox3" runat="server" Visible="false" />
                                   </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-right">
                                        <span class="formLabel"><asp:Label ID="lblAnswer2" runat="server" AssociatedControlID="Answer2" Text="Answer*" /></span>
                                    </div>
                                    <div class="col-sm-8 text-left">
                                        <asp:TextBox runat="server" ID="Answer2" CssClass="formField" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="Answer2" ErrorMessage="* Answer is required" SetFocusOnError="true" Text="* Answer is required" ValidationGroup="AdminUserAccounts"  Display="Dynamic"/>
                                    </div>
                               </div>
                            </div>
                        </div>
                        <div style="width:100%;text-align:right;display:none;"><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="buttonBox" Text="Submit" CausesValidation="false" /></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        
</asp:Panel>
 <asp:Panel runat="server" ID="pnlChangesSaved" Visible="false">
     <br /><br /><br />
    <div style="margin-left:auto; margin-right:auto;text-align:center;">
        <p>Your password has been successfully changed, please log in with your username and new password to complete the process.</p>
    </div>
    <div class="btnBox btnBoxCenter" style="padding-top: 10px; width: 93%;">  
        <asp:Button ID="btnmpeContinue" runat="server" Text="Continue" CssClass="buttonBox buttonBoxFocus" OnClick="BtnCancelBtmUser_Click" />
    </div>
</asp:Panel>

            
          <br />
<asp:Panel runat="server" ID="pnlButtons">
 <div style="float:right;">
            <table class="tblButtons" role="presentation">
                <tr>
                    <td><asp:Button ID="btnSaveBtmUser" CssClass="buttonBox buttonBoxFocus" runat="server" Text="Save" OnClick="btnSubmit_Click" /></td>                 
                    <td><asp:Button ID="BtnCancelBtmUser" CssClass="buttonBox" runat="server" Text="Cancel" OnClick="BtnCancelBtmUser_Click" CausesValidation="false" ValidationGroup="None" /></td>
                </tr>
            </table>     
    </div>
 </asp:Panel>
</div>


