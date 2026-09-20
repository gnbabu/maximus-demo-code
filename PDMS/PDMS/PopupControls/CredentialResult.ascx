<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_CredentialResult" Codebehind="CredentialResult.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>

<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="scs" %>

<script type="text/javascript">


    function openLink(url) {
        window.open(url, 'newWindow');
    }
</script>
<style type="text/css">
    textarea,
    select {
        min-width: 50px !important;
    }

   .mycheckBig input {width:25px; height:25px;}

  .mycheckSmall input {width:10px; height:10px;}


</style>
<%--<uc:AdverseActionHeader runat="server" ID="ucAdverseActionHeader" OnCreateAdverseAction="ucAdverseActionHeader_CreateAdverseAction" />--%>
<br />

<div id="divCredentialingResults" class="container-fluid">
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel">Provider Name</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox runat="server" ID="txtProviderName" Enabled="false" CssClass="forformField wd250" autocomplete="off" />
        </div>
        <div class="col-sm-3 text-right"><span class="formLabel">Gender</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox runat="server" ID="txtGender" Enabled="false" CssClass="forformField wd250" autocomplete="off" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel">Provider Date Of Birth</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox runat="server" ID="txtProviderDOB" Enabled="false" CssClass="forformField wd250" autocomplete="off" />
        </div>
        <div class="col-sm-3 text-right"><span class="formLabel">Provider Type</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox runat="server" ID="txtProviderType" Enabled="false" CssClass="forformField wd250" autocomplete="off" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel">NPI</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox runat="server" ID="txtNPI" Enabled="false" CssClass="forformField wd250" autocomplete="off" />
        </div>
        <div class="col-sm-3 text-right"><span class="formLabel">Social Security/Tax ID</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox runat="server" ID="txtssn" Enabled="false" CssClass="forformField wd250" autocomplete="off" />
        </div>
    </div>
    </div>
<div class="row">
<uc2:Separator ID="ucSeperatorResult1" runat="server" />
<hr/>
    </div>
<div id="divScreeningResults" class="container-fluid">
     
    <div class="row" id ="dvBoardVerification" runat="server" visible="false">
        <div class="col-sm-1 text-right">
        <asp:CheckBox ID="chkBoardVerificationRequird" runat="server" CssClass="mycheckBig" OnCheckedChanged="chkBoardVerificationRequird_CheckedChanged" />
            </div>
        <div class="col-sm-2 text-right"><span class="formLabel">Board Verification not required for credentialing at this time <strong style="color:red">- By checking this box you are by passing the board verification requirement </strong> </span></div>
    </div>

    <div class="row" id ="dvMedicareOptOut" runat="server" visible="false">
        <div class="col-sm-1 text-right">
        <asp:CheckBox ID="chkMedicareOptOutRequird" runat="server" CssClass="mycheckBig" AutoPostBack="true" OnCheckedChanged="chkMedicareOptOutRequird_CheckedChanged" />
            </div>
        <div class="col-sm-2 text-right"><span class="formLabel"><strong style="color:red">Check here if provider is on OPT OUT list </strong></span></div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel">Verification Date</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox runat="server" ID="txtVerificationDate" CssClass="forformField wd250" autocomplete="off"  />
            <cc1:CalendarExtender ID="calVerificationDate" TargetControlID="txtVerificationDate" runat="server" />  
            <asp:RequiredFieldValidator ID="vaReqd" runat="server" ControlToValidate="txtVerificationDate"
                                                SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="DateCheck" ErrorMessage="*Required Verification Date.">
              </asp:RequiredFieldValidator>
              <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="DateCheck" ShowSummary="false" />
        </div>
        <div class="col-sm-3 text-right"><span class="formLabel">Verified By</span></div>
        <div class="col-sm-3 text-left">
            <asp:Label runat="server" ID="lblVerifiedBy" CssClass="select-control wd250"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right">
            <asp:Label runat="server" ID="lblResultDropDownLabel" CssClass="formLabel">Data Rank</asp:Label>
        </div>
        <div class="col-sm-3 text-left">
            <asp:DropDownList runat="server" ID="ddlDataRank" CssClass="select-control wd250"></asp:DropDownList>
        </div>
        <div class="col-sm-3 text-right">
            <asp:Label runat="server" ID="lblVerificationSourceUsed" CssClass="formLabel">Verification Source Used</asp:Label>
        </div>
        <div class="col-sm-3 text-left">
            <asp:DropDownList runat="server" ID="ddlVerificationSourceUsed" CssClass="select-control wd250"></asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right">
            <asp:Label runat="server" ID="lblAdverseAction" CssClass="formLabel">Credentialing Notes</asp:Label>
        </div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtAdverseAction" runat="server" MaxLength="250" CssClass="form-control wd250" TextMode="MultiLine" Columns="2000" Rows="7" />
        </div>
        <div class="col-sm-3 text-right" >
            <asp:Label runat="server" ID="lblReferenceLink" CssClass="formLabel">Reference Link</asp:Label>
        </div>
        <div class="col-sm-3 text-left">
            <asp:PlaceHolder runat="server" ID="lnkReference"></asp:PlaceHolder>
            <br />
            <%--<asp:LinkButton runat="server" ID="lnkExternalSearch" Text="" CssClass="formLabel300" OnClick="lnkExternalSearch_Click" EnableViewState="true" CausesValidation="false" /><br />--%>
            <asp:LinkButton runat="server" ID="lnkExtraExternalSearch" Text="" CssClass="formLabel300" OnClick="lnkExternalSearch_Click" EnableViewState="true" CausesValidation="false" />
        </div>
        <div class="col-sm-4">
            <asp:PlaceHolder runat="server" ID="PlaceHolder1"></asp:PlaceHolder>
            </div>
    </div>
    <div class="row" id="divAttestationDate" runat="server" visible="false">
        <div class="col-sm-3 text-right"><span class="formLabel">Attestation Date</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtAttestationDate" runat="server" CssClass="forformField wd250" />
            <cc1:CalendarExtender ID="calAttestationDate" TargetControlID="txtAttestationDate" runat="server" />
        </div>
    </div>
    <div class="row" id="divIssuingState" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">Issuing State</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtIssuingState" runat="server" CssClass="forformField wd250" />
        </div>
    </div>
    <div class="row" id="divCDSNumber" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">CDS Number</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtCDSNumber" runat="server" CssClass="forformField wd250" />
        </div>
    </div>
    <div class="row" id="divOrgEffDate" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">Original Effectiveissue Date</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtOrgEffDate" runat="server" CssClass="forformField wd250"  autocomplete="off"/>
            <cc1:CalendarExtender ID="calOrgEffDate" TargetControlID="txtOrgEffDate" runat="server" />
        </div>
    </div>
    <div class="row" id="divRenewalDate" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">Renewal Date</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtRenewalDate" runat="server" CssClass="formField wd250" autocomplete="off"/>
            <cc1:CalendarExtender ID="calRenewalDate" TargetControlID="txtRenewalDate" runat="server" />
        </div>
    </div>
    <div class="row" id="divExpDate" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">Expiration Date</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtExpDate" runat="server" CssClass="formField wd250" autocomplete="off"/>
            <cc1:CalendarExtender ID="calExpDate" TargetControlID="txtExpDate" runat="server" />
        </div>
    </div>
   <%-- <div class="row" id="divLicenseStatus" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">License Status</span></div>
        <div class="col-sm-3 text-left">
            <asp:DropDownList runat="server" ID="ddlLicenseStatus" CssClass="form-control wd250"></asp:DropDownList>
        </div>
    </div>--%>
    <div class="row" id="divBoardStatus" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">Board Status</span></div>
        <div class="col-sm-3 text-left">
            <asp:DropDownList runat="server" ID="ddlBoardStatus" CssClass="form-control wd250"></asp:DropDownList>
        </div>
    </div>
    <div class="row" id="divMaternityLicDate" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">Maternity License Date</span></div>
        <div class="col-sm-3 text-left">
           <asp:TextBox ID="txtMatLicDt" runat="server" CssClass="formField wd250" autocomplete="off"/>
            <cc1:CalendarExtender ID="CalMatLicDt" TargetControlID="txtMatLicDt" runat="server" />
        </div>
    </div>
    <div class="row" id="divSiteAccredDate" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel">Site Visit/Accreditation Date</span></div>
        <div class="col-sm-3 text-left">
            <asp:TextBox ID="txtSiteAccredDate" runat="server" CssClass="formField wd250" autocomplete="off"/>
            <cc1:CalendarExtender ID="CalSiteAccredDate" TargetControlID="txtSiteAccredDate" runat="server" />
        </div>
    </div>
    <br />
    <div class="row" id="divlblUpload" runat="server">
        <div class="col-sm-3 text-right"><span class="formLabel" style="color:red">Note:Upload Required</span></div>
    </div>
     <div class="row" id="divUploadNotRequired" runat="server" visible="false">
        <div class="col-sm-3 text-right"><span class="formLabel" runat="server" id="lblUploadNotRequird" style="color:red">Note:Upload Required if not Board Certified</span></div>
    </div>
</div>
<br />
<br />
<div class="row btnBoxCenter">
    <asp:Button runat="server" ID="btnConfirm" CssClass="buttonBoxFocus" Text="Confirm" OnClick="btnConfirm_Click" EnableViewState="true" CausesValidation="true" ValidationGroup="DateCheck" />

    <asp:Button runat="server" ID="btnCancelScreeningResult" CssClass="buttonBox" Text="Cancel" OnClick="btnCancelScreeningResult_Click" CausesValidation="false" />
</div>
<br />
<br />
<asp:MultiView runat="server" ID="mvScreeningSpecificResults">

    <asp:View runat="server" ID="vwNPPESResults">
        <div class="row">
            <asp:Panel ID="pnlNPPES" runat="server" GroupingText="NPPES Returned Data">
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel200">NPI</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESNPI" />
                    </div>
                    <div class="col-sm-3 text-right"><span class="formLabelAuto">Entity Type</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESEntityType" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel200">Provider Organization Name</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESOrgName" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel200">First</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESFirstName" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabelAuto">Middle</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESMiddleName" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabelAuto">Last</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESLastName" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel200">Mailing Address State Name</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESAddressState" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </asp:View>
    <asp:View runat="server" ID="vwMalPracticeResults1">
        <div class="row">

            <asp:Panel ID="pnlMalPractice1" runat="server" GroupingText="Malpractice Insurance Returned Data">
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Policy Number</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMalPracticeNumber" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Effective Date</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMalPracticeEffective" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Expiration Date</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMalPracticeExpiration" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Carrier Name</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMalPracticeCarrier" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Coverage Amount per Occurrence</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMalPracticeCoverage" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Coverage Amount per Aggregate</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMalPracticeAggregate" />
                    </div>
                </div>
                <div class="row">
                    <asp:PlaceHolder runat="server" ID="PlaceholderUploadMalPracticeInsurance"></asp:PlaceHolder>

                </div>
            </asp:Panel>
        </div>
    </asp:View>
    <asp:View runat="server" ID="vwWorkHistoryResults">
        <asp:Panel runat="server" ID="pnlWorkReturnData" GroupingText="5-Year Work History">
            <%--<div class="row">
                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblGap" runat="server" Text="*Gaps in Training or Work History" CssClass="formLabel wd200"></asp:Label>
                </div>
                <div class="col-sm-8">
                    <asp:RadioButtonList runat="server" ID="rblGap" AutoPostBack="true" EnableViewState="true" RepeatDirection="Horizontal">
                        <asp:ListItem Text="No" Value="0" Enabled="true"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1" Enabled="true"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>--%>

            <div class="divGrid">
                <asp:GridView runat="server" ID="grdWorkResult" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
                    EmptyDataText="No records found">
                    <Columns>
                        <asp:BoundField DataField="TITLE" HeaderText="Title" />
                        <asp:BoundField DataField="NAME" HeaderText="Name Of Organization" />
                        <asp:BoundField DataField="WORKED_FROM" HeaderText="From Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="WORKED_TO" HeaderText="To Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:TemplateField HeaderText="Contact Details">
                            <ItemTemplate>
                                <asp:Label ID="lblContact" Text='<%# GetContactDetails(Eval("CONTACT_NAME"),Eval("CONTACT_EMAIL_ADDRESS"),Eval("CONTACT_PHONE_NUMBER"))%>' runat="server" />

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>

        </asp:Panel>
    </asp:View>
    <asp:View ID="vwDEAResults" runat="server" >
        <div class="row" >
            <asp:Panel ID="pnlDEAData" runat="server" GroupingText="DEA Returned Data" Visible="false">
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Label runat="server" ID="lblDEANumber" CssClass="formLabel170">DEA Number</asp:Label>
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtDEANumber" runat="server" CssClass="formField formField" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Label runat="server" ID="lblDEAEffectiveDate" CssClass="formLabel170">Effective Date</asp:Label>
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtDEAEffectiveDate" runat="server" CssClass="formField formField" />
                        <cc1:CalendarExtender ID="calDEAEffectiveDate" TargetControlID="txtDEAEffectiveDate" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Label runat="server" ID="lblDEAExpireDate" CssClass="formLabel170">Expiration Date</asp:Label>
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtDEAExpireDate" runat="server" CssClass="formField formField" />
                        <cc1:CalendarExtender ID="calDEAExpireDate" TargetControlID="txtDEAExpireDate" runat="server" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDEAnumbers" runat="server" GroupingText="DEA Numbers">
                <div class="divGrid">
                    <asp:GridView runat="server" Width="98%" ID="grdDEANumbers" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No DEA numbers found">
                        <Columns>
                            <asp:BoundField DataField="DEA_NUMBER" HeaderText="DEA Number" />
                            <asp:BoundField DataField="DEA_STATE" HeaderText="State" />
                            <asp:BoundField DataField="DEA_EFF_DATE" HeaderText="Effective Date"  DataFormatString="{0:d}"/>
                            <asp:BoundField DataField="DEA_END_DATE" HeaderText="Expiration Date"  DataFormatString="{0:d}"/>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                <br />
            </asp:Panel>
        </div>
        <div class="row">
            <asp:PlaceHolder ID="placeHolderDEA" runat="server"></asp:PlaceHolder>
        </div>
    </asp:View>
    <asp:View runat="server" ID="vwLicenseResults">
        <div class="row">
            <asp:Panel ID="pnlLicenses" runat="server" GroupingText="Provider Licenses">
                <div class="divGrid">
                    <asp:GridView runat="server" Width="98%" ID="grdLicenses" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No licenses found" OnRowDataBound="grdLicenses_RowDataBound" DataKeyNames="REG_LICENSURE_ID,LICENSE_RESTRICTION_CODE_ID">
                        <Columns>
                            <asp:BoundField DataField="LICENSE_NUMBER" HeaderText="License Number" />
                            <asp:BoundField DataField="LICENSE_STATE" HeaderText="State" />
                            <asp:BoundField DataField="LICENSE_TYPE_NAME" HeaderText="Type" />
                             <asp:BoundField DataField="LICENSE_STATUS" HeaderText="License Status" />
                            <%--<asp:TemplateField HeaderText="License Status">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlLicenseRestrictionCode" CssClass="formDropDown">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="LICENSE_EFF_DATE" HeaderText="Begin Date" DataFormatString="{0:d}" />
                            <asp:BoundField DataField="LICENSE_END_DATE" HeaderText="Expiration Date" DataFormatString="{0:d}" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                <br />
            </asp:Panel>
        </div>
    </asp:View>
    <asp:View runat="server" ID="vwAMBSResults">
        <div class="divGrid">
            <asp:GridView runat="server" Width="98%" ID="grdSpecialties"
                AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
                EmptyDataText="No records found">
                <Columns>
                    <asp:BoundField DataField="BOARD_CERTIFICATION_NAME" HeaderText="Board Name" ItemStyle-Width="350" />
                    <asp:BoundField DataField="BOARD_SPECIALTY_NAME" HeaderText="Specialty" />
                    <asp:BoundField DataField="EFFECTIVE_DATE" HeaderText="Original Effective/Issue Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="EXPIRATION_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}" />

                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:View>
    <asp:View runat="server" ID="vwEducationResults">

        <asp:Panel runat="server" ID="pnlEducationResult" GroupingText="Education">
            <div class="divGrid">
                <asp:GridView runat="server" ID="grdEducationResult" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
                    EmptyDataText="No records found">
                    <Columns>
                        <asp:BoundField DataField="SCHOOL" HeaderText="Name of School" />
                        <asp:BoundField DataField="EDUCATION_TYPE" HeaderText="Degree/Certificate" />
                        <asp:BoundField DataField="FIELDOFSTUDY" HeaderText="Field Of Study/Specialty" />
                        <asp:BoundField DataField="START_YEAR" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="END_YEAR" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />

                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>

            </div>

            <br />
        </asp:Panel>

    </asp:View>

     <asp:View runat="server" ID="vwMalpracticeResults">

        <asp:Panel runat="server" ID="pnlMalpractice" GroupingText="Malpractice">
            <div class="divGrid">
                <asp:GridView runat="server" ID="grdInsurance" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
                    EmptyDataText="No records found">
                    <Columns>
                        <asp:BoundField DataField="CarrierName" HeaderText="Carrier Name" />
                        <asp:BoundField DataField="PolicyHolder" HeaderText="Policy Holder" />
                        <asp:BoundField DataField="POLICY_NUMBER" HeaderText="Policy Number" />
                        <asp:BoundField DataField="EFFECTIVE_DATE" HeaderText="Original Effective/Issue Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="EXPIRATION_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="CoverageAmountPerOccurance" HeaderText="Coverage Amount per Occurance" DataFormatString="${0:#,#}" />
                        <asp:BoundField DataField="CoverageAmountPerAggregate" HeaderText="Coverage Amount per Aggregate" DataFormatString="${0:#,#}" />

                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>

            </div>

            <br />
        </asp:Panel>

    </asp:View>
    <asp:View ID="vwCDSResults" runat="server">
        <asp:Panel ID="pnlCDSnumbers" runat="server" GroupingText="CDS Numbers">
            <div class="divGrid">
                <asp:GridView runat="server" Width="98%" ID="grdCDSNumbers" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No CDS numbers found">
                    <Columns>
                        <asp:BoundField DataField="STATE_CDS_NUMBER" HeaderText="CDS Number" />
                        <asp:BoundField DataField="State" HeaderText="State" />
                        <asp:BoundField DataField="DateIssued" HeaderText="Effective Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="ExpirationDate" HeaderText="Expiration Date" DataFormatString="{0:d}" />
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
            <br />
            <div class="row">
                <asp:PlaceHolder ID="placeHolderCDS" runat="server"></asp:PlaceHolder>
            </div>
            <br />
        </asp:Panel>
    </asp:View>

    <asp:View runat="server" ID="vwNPDBResults">
        <asp:Panel ID="Panel1" runat="server" GroupingText="NPDB Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Practitioner Name</span></div>
                <div class="col-sm-3 text-left">
                    <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblName" />
                </div>
                <div class="col-sm-3 text-right"><span class="formLabel">Gender</span></div>
                <div class="col-sm-3 text-left">
                    <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblGender" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Date Of Birth</span></div>

                <div class="col-sm-3 text-left">
                    <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lbl_DOB" />
                </div>
            </div>


            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Organization Name</span></div>
                <div class="col-sm-3 text-left">
                    <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lbl_Org" />
                </div>

            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Work Address</span></div>
                <div class="col-sm-9 text-left">
                    <asp:Label runat="server" CssClass="formFieldDisplayLarge" ID="lbl_Address" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">SSN</span></div>
                <div class="col-sm-3 text-left">
                    <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lbl_SSN" />
                </div>
            </div>


        </asp:Panel>
    </asp:View>

</asp:MultiView>
<asp:HiddenField ID ="hdnNPI" runat="server" />
<asp:HiddenField ID ="hdnchkBoardVerification" runat="server" />
<asp:HiddenField ID ="hdnchkMedicareOptOut" runat="server" />


