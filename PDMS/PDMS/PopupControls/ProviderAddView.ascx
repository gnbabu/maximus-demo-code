<%@ Control Language="C#" AutoEventWireup="true" Inherits="Views_ProviderAddView" Codebehind="ProviderAddView.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<style type="text/css">
    .auto-style1 {
        height: 20px;
    }
    .auto-style2 {
        height: 33px;
    }
</style>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupEventHandlers);
    });

    function setupEventHandlers() { 
        $("#divRequestedEffectiveDateInfo").hide();

        $(".what-is-this-link").mouseover(function () {
                $("#divRequestedEffectiveDateInfo").show();
        });
       
        $("#divRequestedEffectiveDateInfo").mouseleave(function () {
            $("#divRequestedEffectiveDateInfo").hide();
        });
    }


</script>

<asp:UpdatePanel ID="upAddNew" runat="server"  UpdateMode="Conditional" >
    <ContentTemplate>
    <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" style="padding-left:4px;padding-right:4px; padding-bottom:4px;padding-top:0px;">

        <ajax:ModalPopupExtender ID="mpeKFEVerify" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" 
            CancelControlID="btnKFECancel" BackgroundCssClass="modalBackground" Drag="false" >
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" style="display:none; top:0px; padding:0px;width:360px;">
            <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                <div class="popTitle"><asp:Label ID="lblConfirmUpdate" runat="server" Text="Confirm Update" /></div>
            </asp:Panel>
            <div  style="padding:10px;">
                <div class="center"><asp:Literal id= "ltlKFEConfirm" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_EDIT_DELETE_CONFIRMATION_MESSAGE %>" /></div>
                <div class="btnBox">
                    <asp:Button runat="server" ID="btnKFEYes" Text="Yes" CssClass="buttonBox" OnClick="btnKFEYes_Click" CausesValidation="false" />
                    <asp:Button runat="server" ID="btnKFECancel" Text="Cancel" CssClass="buttonBox" OnClick="btnKFECancel_Click" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonDummy" Style="display: none" value="dummy" />

        <div>
             <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
           <asp:ValidationSummary ID="vsNewProvider" CssClass="failureNotification" DisplayMode="List" runat="server"  ValidationGroup="AddNewProvider" ShowSummary="true"  />
        </div>
            <div class="pg-hint"><asp:Label ID="lblhint" runat="server" Text="* Designates a required field" /></div><br /><br />
            <div id="div10DayMessage" style="width:100%; margin-left:20px;">
               <%-- <p style="color:red; text-align:left; width:90%; font-size:11px;">Please note that you have 10 days to complete your application. After 10 days, your information will be deleted and you will
                have to re-start the process from the beginning of the application.</p>--%>
                <p style="color:red; text-align:left; width:90%; font-size:11px;">
                    <asp:Label ID="lblElapsedTimeMsg" runat="server" Text="" />              
                </p>
            </div>
            <div style="width:auto;" >
                <div id="trApplicationType" runat="server" class="row">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblAppType" runat="server" Text="Application Type" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:DropDownList ID="ddlApplicationType" runat="server" Enabled="false" CssClass="formDropDown"></asp:DropDownList>
                        <asp:LinkButton ID="lnkConversionConvertToFeeForService" runat="server" Text="Convert to Fee-for-Service" CommandName="ConvertToFeeForService" OnClick="lnkConversionConvertToFeeForService_Click" Visible="false"/>
                        <%--<asp:TextBox ID="txtApplicationType" runat="server" Enabled="false"></asp:TextBox>--%>
                    </div>                    
                </div>
                <div class="row" id="trEntityType" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblEntityType" runat="server" Text="Entity Type*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:RadioButtonList ID="rblEntityType" runat="server" RepeatDirection="Horizontal"  style="padding:0; margin:0;" AutoPostBack="true" OnSelectedIndexChanged="rblEntityType_SelectedIndexChanged" CssClass="QstRadioList">
                            <asp:ListItem Selected="False" Text="Individual" Value="Individual"></asp:ListItem>
                            <asp:ListItem Selected="True" Text="Organization" Value="Organization"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="row" id="trCategory" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblCategory" runat="server" Text="Category*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:DropDownList ID="ddlCategory" runat="server" 
                                 OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"  AutoPostBack="true" AppendDataBoundItems="True" CssClass="formDropDown" />            
                        <asp:CompareValidator runat="server" ID="valCatCmp" ControlToValidate="ddlCategory"  ValueToCompare="0" Type="Integer" ErrorMessage="* Category is required." 
                            Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" />
                    </div>
                </div>
                <div class="row" id="trProviderType"  runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblProviderType" runat="server" Text="Provider Type*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:DropDownList ID="ddlProviderType" runat="server" AutoPostBack="true" AppendDataBoundItems="True" 
                                 OnSelectedIndexChanged="ddlProviderType_SelectedIndexChanged" CssClass="formDropDown" />
                            <asp:CompareValidator runat="server" ID="valTypeCmp" ControlToValidate="ddlProviderType" 
                                ValueToCompare="0" Type="Integer" ErrorMessage="* Provider Type is required." 
                                Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"  
                                ValidationGroup="AddNewProvider" />
								<asp:HiddenField ID="hidDCOnly" runat="server" Value="0" />													   
                    </div>
                </div>
                <div class="row" id="trSpecialty" runat="server" style="display:none">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblspecialty" runat="server" Text="Specialty*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:DropDownList ID="ddlSpecialty" runat="server" AutoPostBack="true" AppendDataBoundItems="True" 
                              CssClass="formDropDown"  OnSelectedIndexChanged="ddlSpecialty_SelectedIndexChanged" />
                        <%--<asp:CompareValidator runat="server" ID="valSpecCmp" ControlToValidate="ddlSpecialty" ValueToCompare="0" Type="Integer" 
                            ErrorMessage="* Specialty is required." Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"  ValidationGroup="AddNewProvider" Enabled="false" />--%>
                    </div>
                </div>
                <div class="row" id="trTaxonomy" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblTaxonomy" runat="server" Text="Taxonomy*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:DropDownList ID="ddlTaxonomy" runat="server" AutoPostBack="false" CssClass="formDropDown" />
                            <%--<asp:RequiredFieldValidator ID="rfvldddlTaxonomy" runat="server" ControlToValidate="ddlTaxonomy" Enabled="true" SetFocusOnError="true"
                                        Display="Dynamic" Text="*" ValidationGroup="GroupAffiliations" ErrorMessage="* Taxonomy is required"></asp:RequiredFieldValidator>--%>
                            <asp:CompareValidator runat="server" ID="valTaxonmyCmp" ControlToValidate="ddlTaxonomy" ValueToCompare="0" Type="Integer" 
                                 Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"  ValidationGroup="AddNewProvider" />
                            
                    </div>
               </div>
                <div class="row" id="trPracticeType" runat="server" visible ="false">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblTypeOfPractice" runat="server" Text="Type of Practice*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:DropDownList ID="ddlPracticeType" runat="server" AutoPostBack="false"  ViewStateMode="Enabled" Enabled ="false" CssClass="formDropDown" />
                            <asp:CompareValidator runat="server" ID="cvPracticeType" ControlToValidate="ddlPracticeType"  
                                ValueToCompare="0" Type="Integer" ErrorMessage="* Practice Type is required."  Enabled="false"
                                Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"  
                                ValidationGroup="AddNewProvider" />
                    </div>
                </div>
                 
                <div id="trOrgName" runat="server"  class="row group-provider">
                   <div class="col-sm-4 text-right"><asp:Label ID="lblNameOfBussiness" runat="server" Text="Name of Business Entity*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtProviderName" runat="server" MaxLength="100" CssClass="formField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valNameReqd" runat="server" ControlToValidate="txtProviderName" Enabled="true" SetFocusOnError="true" 
                                Display="Dynamic" Text="*"  ValidationGroup="AddNewProvider" ErrorMessage="* Business Name is required."></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdnName" runat="server" />
                    </div>
                </div>
                <div id="trOrgNameHint" runat="server" class="row group-provider">
                    <%--<td></div>--%>
                    <div class="col-sm-9 pg-hint2" >
                        <asp:Label ID="lblbusiness" runat="server" Text="Business Name as it appears on your IRS Assignment letter" />
                    </div>
                </div>
                <div id="trFN" runat="server" class="row indiv-provider">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblFname" runat="server" Text="First Name*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtFirstName" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtFirstName" Enabled="false" SetFocusOnError="true" 
                                Display="Dynamic" Text="*"  ValidationGroup="AddNewProvider" ErrorMessage="* First Name is required."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div id="trMI" runat="server"  class="row indiv-provider">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblMiddleInitial" runat="server" Text="Middle Initial" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtMI" runat="server" MaxLength="10" CssClass="formField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revMI" runat="server" ControlToValidate="txtMI"
                                    ValidationExpression=".*[a-zA-Z]+.*" ErrorMessage="* Enter valid Middle Initial."
                                    Enabled="true" SetFocusOnError="true" Text="*"
                                    ValidationGroup="AddNewProvider" Display="Dynamic" />
                    </div>
                </div>
                <div id="trLN" runat="server"  class="row indiv-provider">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblLname" runat="server" Text="Last Name*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtLastName" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valLNReqd" runat="server" ControlToValidate="txtLastName" Enabled="false" SetFocusOnError="true" 
                                Display="Dynamic" Text="*"  ValidationGroup="AddNewProvider" ErrorMessage="* Last Name is required."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div id="trTaxIDType" runat="server" class="row indiv-provider">
                    <div class="col-sm-4 text-right"><asp:Label ID="lbltaxidtype" runat="server" Text="Tax ID Type*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:RadioButtonList ID="rblTaxIDType" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList" >
                            <asp:ListItem Selected="False" Text="EIN" Value="16"></asp:ListItem>
                            <asp:ListItem Selected="False" Text="SSN" Value="15"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="valTaxTypeReqd" runat="server" ControlToValidate="rblTaxIDType"  Enabled="true" SetFocusOnError="true" 
                            Display="None" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Tax ID Type is required."></asp:RequiredFieldValidator>
                             <asp:CustomValidator ID="cvTaxType" runat="server" OnServerValidate="Validate_TaxIDType" ControlToValidate="rblTaxIDType"
                                Display="Dynamic" ValidationGroup="AddNewProvider" ErrorMessage="* When enrolling or re-enrolling as Individual provider, you must provide your SSN. Please contact the MAXIMUS help desk." SetFocusOnError="true" Enabled="true" Text="*" />

                    </div>
                 

                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblTaxId" runat="server" Text="Tax ID*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtTaxID" runat="server" CssClass="formField" MaxLength="9" Enabled="false"/>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTaxID"
                                ValidationExpression="^([0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" ErrorMessage="* Enter a 9 digit Tax ID."  
                                Enabled="true" SetFocusOnError="true" Text="*"
                                ValidationGroup="AddNewProvider"  Display="Dynamic" />
                    </div>
                </div>
                <div class="row" id="trOldNPI" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblOldNPI" runat="server" Text="Old NPI" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtOldNPI" runat="server" CssClass="formField" Enabled ="false"></asp:TextBox>
                    </div>
                </div>
                  <div class="row" id="trOldNPIStartDate" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblNPIStartDate" runat="server" Text="Old NPI Start Date" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtOldNPIStartDate" runat="server" CssClass="formField" Enabled ="false"></asp:TextBox>
                    </div>
                </div>
                  <div class="row" id="trOldNPIEndDate" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblNPIenddate" runat="server" Text="Old NPI End Date*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">  <asp:TextBox ID="txtOldNPIEndDate" runat="server" CssClass="formField"/>
                                <ajax:CalendarExtender ID="ceOldNPIEndDate" TargetControlID="txtOldNPIEndDate" runat="server" />
                                <asp:RequiredFieldValidator ID="valOldNPIEndDate" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtOldNPIEndDate" ErrorMessage="* Old NPI End Date is required." Display="Dynamic"  Enabled="true" />
                                <asp:CompareValidator id="cvOldNPIEndDate" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtOldNPIEndDate"   Enabled="true"
                                    ErrorMessage="* A valid Old NPI End Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                                 <asp:CompareValidator id="cvOldNPIEndDateGreater" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="GreaterThan" ControlToValidate= "txtOldNPIEndDate"
                                    ControlToCompare="txtOldNPIStartDate"   Enabled="true"
                                    ErrorMessage="* Old NPI end date must be after old NPI start date." 
                                    Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                                  <asp:CompareValidator id="cvOldNPIEndDateLessThan" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="LessThan" ControlToValidate= "txtOldNPIEndDate"
                                    ControlToCompare="txtNPIStartDate"   Enabled="true"
                                    ErrorMessage="* Old NPI end must be before NPI start date." 
                                    Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="false"> 
                                </asp:CompareValidator>
                    </div>
                </div>
                <div class="row" id="trNPI" runat="server">
                    <%--<td  class="formLabel wd170">NPI(if applicable)</div>--%>
					<div class="col-sm-4 text-right"><asp:Label ID="lblNpiApplicable" runat="server" Text="NPI(if applicable)" CssClass="formLabel wd200" /></div> 
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtNPI" runat="server" MaxLength="10" CssClass="formField" OnTextChanged="txtNPI_TextChanged" AutoPostBack="true" />
                            <asp:RegularExpressionValidator ID="valNPI" runat="server" ControlToValidate="txtNPI"
                                ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" ErrorMessage="* Enter a 10 digit NPI that does not begin with 0."  
                                Enabled="true" SetFocusOnError="true" Text="*"
                                ValidationGroup="AddNewProvider"  Display="Dynamic" />
						<asp:RequiredFieldValidator ID="valNPIRequired" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtNPI" Display="Dynamic"  Enabled="true" />																																
                        <asp:CustomValidator ID="cvNPIUnchanged" runat="server" ControlToValidate="txtNPI" OnServerValidate="Validate_NPIEdit" 
                            Display="Dynamic" ValidationGroup="AddNewProvider" ErrorMessage="* New NPI must be different than old NPI." Text="*" />
                        <div id="divEditNPI" runat="server" class="bodyTextSmall what-is-this-link" style="color:blue; cursor: pointer; display:inline-block; text-decoration:underline">
                            <asp:LinkButton ID="lnkEditNPI" runat="server"  Text="Edit NPI" CommandName="Edit NPI" OnClick="lnkEditNPI_Click" />
                         </div>
                         <div id="divEditNPIEndDate" runat="server" class="bodyTextSmall what-is-this-link" style="color:blue; cursor: pointer; display:inline-block; text-decoration:underline">
                            <asp:LinkButton ID="lnkEditEndDate" runat="server"  Text="Edit End Date" CommandName="Edit End Date" OnClick="lnkEditEndDate_Click" />
                        </div>
                         <asp:HiddenField ID="hdnNPI" runat="server" />

                    </div>
                </div>
                <div class="row" id="trNPIStartDate" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblStrtDate" runat="server" Text="NPI Start Date*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtNPIStartDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceNPIStartDate" TargetControlID="txtNPIStartDate" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtNPIStartDate" ErrorMessage="* NPI Start Date is required." Display="Dynamic"  Enabled="true" />
                                <asp:CompareValidator id="valNPIStartDate" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNPIStartDate"   Enabled="true"
                                    ErrorMessage="* A valid NPI Start Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                        </div>
                    </div>
                  <div class="row" id="trNPIEndDate" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblEndDate" runat="server" Text="NPI End Date" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtNPIEndDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceNPIEndDate" TargetControlID="txtNPIEndDate" runat="server" />
                                <asp:CompareValidator id="valNPIEndDate" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNPIEndDate"   Enabled="true"
                                    ErrorMessage="* A valid NPI End Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                                  <asp:CompareValidator id="cvNPIEndDate" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="GreaterThan" ControlToValidate= "txtNPIEndDate"
                                    ControlToCompare="txtNPIStartDate"   Enabled="true"
                                    ErrorMessage="* NPI end date must be after NPI start date." 
                                    Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                        </div>
                      </div>
                <div class="row" id="trRED" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblRequestedDate" runat="server" Text="Requested Effective Date*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceEffectiveDate" TargetControlID="txtEffectiveDate" runat="server" />
                                <asp:RequiredFieldValidator ID="valEffectiveDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtEffectiveDate" ErrorMessage="* Requested Effective Date is required." Display="Dynamic"  Enabled="true" />
                                <asp:CompareValidator id="cvEffectiveDate" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEffectiveDate"   Enabled="true"
                                    ErrorMessage="* A valid Requested Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                        <div id="divWhatIsReqEffectiveDate" class="bodyTextSmall what-is-this-link" style="color:blue; cursor: pointer; display:inline-block; text-decoration:underline" runat="server">What is this?</div>
                        <div id="divRequestedEffectiveDateInfo" class="infoBox" style="left:120px; top:100px;">
                            <div class="infoTitle"><asp:Label ID="lblrequestedeffdate" runat="server" Text="Requested Effective Date" /></div>
                            <div class="infoContent">
                                <asp:Literal ID="ltlRequestedEffectiveDateInfo" runat="server" Text="<%$ Resources:BrandingResource , REQUESTED_EFFECTIVE_DATE_HELPTEXT %>" ></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>  
            </div>
            <div style="width:auto;" >
                <div id="trGender" runat="server" class="row indiv-provider">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblgender" runat="server" Text="Gender*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left">  
                        <asp:RadioButtonList ID="rblGender" runat="server"  RepeatDirection="Horizontal" CssClass="QstRadioList" OnSelectedIndexChanged="rblGender_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Selected="False" Text="Female" Value="F"></asp:ListItem>
                            <asp:ListItem Selected="False" Text="Male" Value="M"></asp:ListItem>
                            <asp:ListItem Selected="False" Text="Unknown" Value="N"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="valGenderReqd" runat="server" ControlToValidate="rblGender"  Enabled="false" SetFocusOnError="true" 
                            Display="None" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Gender is required."></asp:RequiredFieldValidator></div>
                </div>
                <div id="trDOB" runat="server" class="row indiv-provider">
                    <div class="col-sm-4 text-right"><span class="formLabel wd200">Date of Birth*</span></div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField"  />
                                <ajax:CalendarExtender ID="ceDOB" TargetControlID="txtBirthDate" runat="server"  />
                                <asp:RequiredFieldValidator ID="valDOBReqd" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtBirthDate" ErrorMessage="* Date of Birth is required." Display="Dynamic"  Enabled="false" />
                                <asp:CompareValidator id="cvDOBFormat" runat="server" ValidationGroup="AddNewProvider"   
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate"   Enabled="false"
                                    ErrorMessage="* A valid Date of Birth is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>                    
                    </div>
                </div>
                <div class="row" id="trZipCode" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblZipCode" runat="server" Text="Zip Code*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" CssClass="formField"  />
                            <asp:RequiredFieldValidator ID="valZipReqd" runat="server" ControlToValidate="txtZipCode"  
                                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                                ValidationGroup="AddNewProvider" ></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valZipFormat" runat="server" Text="*"
                                ErrorMessage="* Enter 5 digits for zip code" ControlToValidate="txtZipCode" SetFocusOnError="true"
                                Display="Dynamic" ValidationExpression="(?!0{5})(?!9{5})\d{5}$" ValidationGroup="AddNewProvider" />
                    </div>
                </div>
                <div class="row" id="trZipExt" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblZipExt" runat="server" Text="Zip Code Extension*" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtZipCodeExt" runat="server" MaxLength="4" CssClass="formField" OnTextChanged="txtZipCodeExt_TextChanged" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="valZipExtRqd" runat="server" ControlToValidate="txtZipCodeExt"  
                                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                                ValidationGroup="AddNewProvider" ErrorMessage="Zip Code Extension is required."></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valZipExtFormat" runat="server" Text="*"
                                ErrorMessage="* Enter 4 digits for zip code extension" ControlToValidate="txtZipCodeExt" SetFocusOnError="true"
                                Display="Dynamic" ValidationExpression="(?!0{4})(?!9{4})\d{4}$" ValidationGroup="AddNewProvider" />
                    </div>
                </div>
                <div class="row" id="trReferral" runat="server">
                    <div class="col-sm-4 text-right"><asp:Literal id= "lbl4ReferralNumber" runat="server" Text="<%$ Resources:BrandingResource , REFERRAL_NUMBER_LABEL %>"/>*</div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtApplicationNumber" runat="server" MaxLength="20" CssClass="formField " />
                            <asp:RequiredFieldValidator ID="valAppNbrRqd" runat="server" ControlToValidate="txtApplicationNumber"  
                                Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Referral Number is required."></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="valApplicationNbr" runat="server" OnServerValidate="Validate_AppNumber" ControlToValidate="txtApplicationNumber"
                                Display="Dynamic" ValidationGroup="AddNewProvider" ErrorMessage="* Referral Number not found" SetFocusOnError="true" Enabled="false" Text="*" />
                    </div>
                </div>
                  <div class="row" id="trMedicaid" runat="server">
                    <div class="col-sm-4 text-right"><asp:Label ID="lblMedicaidId" runat="server" Text="Medicaid ID" CssClass="formLabel wd200" /></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="80" CssClass="formField " /></div>
                </div>
            </div>
            <div id="divMultipleMedicaidIds" runat="server" style="display:none;">
                 <div class="grid-hint"><asp:Label ID="lblhintMultipleMedicaid" runat="server" Text="Multiple Medicaid IDs were found. Please select the medicaid id to use for this registration." /></div>
               <div class="fieldTable" style="width:auto;">
                    <div class="row">
                        <div class="col-sm-4 text-right"><asp:Label ID="lblMedicaidBilling" runat="server" Text="Medicaid/Billing Numbers" CssClass="formLabel wd200" /></div>
                        <div class="col-sm-8 text-left"><asp:RadioButtonList ID="rblMedaidIds" runat="server" CssClass="formLabel300 rbl_Vertical formField"  
                            RepeatDirection="Vertical" TextAlign="Right" />
                            <asp:CompareValidator runat="server" ID="cvMedicaidIDs" ControlToValidate="rblMedaidIds"  ValueToCompare="0" Type="Integer" 
                                ErrorMessage="* Multiple medicaid ids were found, selection of one is required." Enabled="false"
                            Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:Panel id="divTaxonomy" runat="server">
                <%--<div style="color: red; text-align: left; width: 90%; font-size: 12px;margin-left:20px;">Select Taxonomy code from the list.</div>--%>
                <div class="fieldTable" style="width:auto;">
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel wd200">Taxonomy*</span></div>
                        <div class="col-sm-8 text-left">
                            <asp:DropDownList ID="ddlTaxonomyNPPES" runat="server" AutoPostBack="true" CssClass="formDropDown" OnSelectedIndexChanged="ddlTaxonomy_SelectedIndexChanged"/>
                            <%--<asp:RequiredFieldValidator ID="rfvldTaxonomyNPPES" runat="server" ControlToValidate="ddlTaxonomyNPPES"  
                                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                                ValidationGroup="AddNewProvider" ErrorMessage="Taxonomy is required."></asp:RequiredFieldValidator>--%>
                          </div>
                    </div>
                </div>
            </asp:Panel>
           <asp:UpdateProgress runat="server"  ID="upAddProgress" DisplayAfter="0"  >
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif"  alt="Loading.." />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div id="divKeyFieldEditInfo" runat="server" class="center" style="padding:10px;width:500px;">
               <div class="pg-hint2">*** <asp:Literal id= "ltlKFE" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_EDIT_PAGE_INFO %>" /> ***</div>
            </div>
            <div class="btnBox">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBox buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="AddNewProvider" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>
    </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

