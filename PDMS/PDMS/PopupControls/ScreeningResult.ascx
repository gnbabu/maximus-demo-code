<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ScreeningResult" Codebehind="ScreeningResult.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="scs" %>
<%@ Register TagPrefix="uc" TagName="ProviderExclusionPanel" Src="~/PopupControls/ProviderExclusionPanel.ascx" %>

<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupMatchResultEventHandlers);
    });

    function setupMatchResultEventHandlers() {
        if ($("#<%= ddlMatchResults.ClientID %>") != null) {
                //PECOS - on load
                if ($("#divPecosDataFields") != null) {
                    if ($('#<%=ddlMatchResults.ClientID %> option:selected').val().toLowerCase() == "9") {
                        $("#divPecosDataFields").show();
                    }
                    else {
                        $("#divPecosDataFields").hide();
                    }
                }
            }

            $("#<%= ddlMatchResults.ClientID %>").change(function (evt) {
                //PECOS - on change 
                if ($("#divPecosDataFields") != null) {
                    if ($('#<%=ddlMatchResults.ClientID %> option:selected').val().toLowerCase() == "9") {
                        $("#divPecosDataFields").show();
                    }
                    else {
                        $("#divPecosDataFields").hide();
                    }
                }
            })
        }

        function openLink(url) {
            window.open(url, 'newWindow');
        }
</script>
<style type="text/css">
    .select {
        width:300px !important;
    }
</style>
<%--<uc:AdverseActionHeader runat="server" ID="ucAdverseActionHeader" OnCreateAdverseAction="ucAdverseActionHeader_CreateAdverseAction" />--%>
<br />
<div id="divScreeningResults" class="container-fluid">
    <div class="row"> 
        <div class="col-sm-3 text-right"><span class="formLabel">Screening Date</span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" ID="lblScreeningDate" CssClass="formFieldDisplay wd250" /></div>
        <div class="col-sm-3 text-right"><span class="formLabel">Performed By</span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblPerformedBy" /></div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel"><asp:Label ID="lblTaxIDLabel" runat="server" Text="Tax ID:"></asp:Label></span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" ID="lblTaxID" CssClass="formFieldDisplay wd250" /></div>
        <div class="col-sm-3 text-right"><span class="formLabel">NPI</span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPI" /></div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel">Organization Name</span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" ID="lblOrganizationName" CssClass="formFieldDisplay wd250" /></div>
        <div class="col-sm-3 text-right"><span class="formLabel">Individual Name</span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblIndividualName" /></div>
    </div>
    <div class="row">        
        <div class="col-sm-3 text-right" ><span class="formLabel" ><asp:Label ID="lbltxtPrimaryPracticeState" runat="server" Text="Primary Practice State" Visible="false"></asp:Label></span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" ID="lblPrimaryPracticeState" CssClass="formFieldDisplay wd250"  Visible="false"/></div>
    </div> 
    <div class="row">
        <div class="col-sm-3 text-right"><asp:Label runat="server" ID="lblResultDropDownLabel" CssClass="formLabel">Match Results</asp:Label></div>
        <div class="col-sm-6 text-left"><asp:DropDownList runat="server" ID="ddlMatchResults" CssClass="formDropDown wd250"></asp:DropDownList></div>
        <div class="col-sm-3">
            <%--<asp:LinkButton runat="server" ID="lnkExternalSearch" Text="" CssClass="formLabel300" OnClick="lnkExternalSearch_Click"  /><br />--%>
            <%--<asp:HyperLink runat="server" ID="lnkExternalSearch" Target="_blank"  ></asp:HyperLink>--%>
            <asp:PlaceHolder runat="server" ID="lnkReference"></asp:PlaceHolder>
            <br />
            <asp:PlaceHolder runat="server" ID="lnkExtraReference"></asp:PlaceHolder>
            <%--<asp:LinkButton runat="server" ID="lnkExtraExternalSearch" Text="" CssClass="formLabel300" OnClick="lnkExternalSearch_Click"  />--%>
        </div>
         
     </div>
     <div class="row">
        <div class="col-sm-3 text-right"><asp:Label runat="server" ID="lblAdverseAction" CssClass="formLabel">Screening Notes</asp:Label></div>
        <div class="col-sm-9 text-left"><asp:TextBox ID="txtAdverseAction" runat="server" MaxLength="1000" CssClass="formField" TextMode="MultiLine" Columns="2000" Rows="7" /></div>
    </div>
<br />
<br />
<div class="row btnBoxCenter">
<asp:Button runat="server" ID="btnConfirm" CssClass="buttonBoxFocus" Text="Confirm" OnClick="btnConfirm_Click" EnableViewState="true" CausesValidation="true" />

<asp:Button runat="server" ID="btnCancelScreeningResult" CssClass="buttonBox" Text="Cancel" OnClick="btnCancelScreeningResult_Click" CausesValidation="false" />
</div>
<br />
<br />
<asp:MultiView runat="server" ID="mvScreeningSpecificResults">
    <asp:View runat="server" ID="vwOIGLEIEResults">
        <div class="row">
        <asp:Panel runat="server" GroupingText="OIG Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Last Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGLastName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">First Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGFirstName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Mid Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGMiddleName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Date of Birth</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGDOB" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Business Name</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGBusinessName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">General</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGGeneral" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Specialty</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGSpecialty" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">UPIN</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGUPIN" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">NPI</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGNPI" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Address</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGAddress" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">City</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGCity" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">State</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGState" /></div>            
                <div class="col-sm-3 text-right"><span class="formLabel">Zip</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGZipCode" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Excl Type</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGExclType" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Excl Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGExclDate" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Reinstate Date</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGReinDate" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Waiver Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGWaiverDate" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Waiver State</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOIGWaiverState" /></div>
            </div>
        </asp:Panel>
            </div>
    </asp:View>
    <asp:View runat="server" ID="vwSSDMFResults">
        <div class="row">
        <asp:Panel runat="server" GroupingText="SSDMF Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">SSN</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFSSN" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">VP Code</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFVPCode" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">First Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFFirstName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Middle Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFMiddleName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Last Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFLastName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Name Suffix</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFNameSuffix" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Date of Death</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFDateOfDeath" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Date of Birth</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSDMFDateOfBirth" /></div>
            </div>
        </asp:Panel>
            </div>
    </asp:View>
    <asp:View runat="server" ID="vwSAMResults">
        <div class="row">
        <asp:Panel runat="server" GroupingText="SAM Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Business Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMBusinessName" /></div>
                <div class="col-sm-3"></div>
                <div class="col-sm-3"></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Prefix</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMNamePrefix" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel ">First</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMFirstName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Middle</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMMiddleName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Last</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMLastName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Suffix</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMNameSuffix" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Classification</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMClassification" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Address 1</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMAddress1" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Address 2</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMAddress2" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Address 3</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMAddress3" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Address 4</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMAddress4" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">City</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMCity" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">State/Province</span></div>
                <div class="col-sm-3 text-left"> <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMStateProvince" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Country</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMCountry" /></div>
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Zip Code</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMZipCode" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">DUNS</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMDuns" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Exclusion Program</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMExclusionProgram" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Excluding Agency</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMExcludingAgency" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">CT Code</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMCTCode" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Exclusion Type</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMExclusionType" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Additional Comments</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMAdditionalComments" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Active Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMActiveDate" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Termination Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMTerminationDate" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Record Status</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMRecordStatus" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Cross-Reference</span></div>
                <div class="col-sm-3 text-left"> <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMCrossReference" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">SAM Number</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAMNumber" /></div>
            </div>
        </asp:Panel>
            </div>
    </asp:View>
    <asp:View runat="server" ID="vwMCSISResults">
        <div class="row">
        <asp:Panel ID="pnlMCSIS" runat="server" GroupingText="Medicaid Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Enrollment Type</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISEnrollType" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">NPI</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISNPI" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">SSN</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISTAXID" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Last Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISLastName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">First Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISFirstName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Legal Business Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISBusinessName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">EIN</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISEIN" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Adverse Action Reason(s)</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISAdverseAction" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Effective Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblEffectiveDate" /></div>
            </div>            
            <div class="row">                
                <div class="col-sm-3 text-right"><span class="formLabel150">Terminating Program</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISTermProgram" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">Appeals Period Expired</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISAppealsProgram" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Status</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISStatus" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">CMS Published Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISCMSPublisedDate" /></div>
            </div>
             <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Correspondence Address</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISAddress" /></div>
            </div>
             <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Practice Location</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISPracticeLoc" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Active Enrollment Bar?</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISActiveEnrollmentbar" /></div>
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Enrollment Bar Expiration Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISEnrollmentExpireddate" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Medicare State(Revocation Only)</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISMedicareState" /></div>
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Enrollment ID(Revocation Only)</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISEnrollmentId" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">CMS Notes</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMCSISStatePOC" /></div>
            </div>
           

        </asp:Panel>
            </div>
    </asp:View>
    <asp:View runat="server" ID="vwNEMEPLResults">
                <div class="row">

        <asp:Panel ID="pnlNEMEPL" runat="server" GroupingText="NEMEPL Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Provider Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLProviderName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Provider Type</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLProviderType" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">NPI</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLNPI" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Effective Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLEffectiveDate" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">End Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLEndDate" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Term</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLTerm" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel150">Termination or Suspension</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLTerminationOrSuspension" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Reason for Action</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNEMEPLReasonForAction" /></div>
            </div>

        </asp:Panel>
                    </div>
    </asp:View>
    <asp:View runat="server" ID="vwNPPESResults">
                <div class="row">

        <asp:Panel ID="pnlNPPES" runat="server" GroupingText="NPPES Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel200">NPI</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESNPI" /></div>
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Entity Type</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESEntityType" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel200">Provider Organization Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESOrgName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel200">First</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESFirstName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Middle</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESMiddleName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelAuto">Last</span></div>
                <div class="col-sm-9 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESLastName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel200">Mailing Address State Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblNPPESAddressState" /></div>
            </div>
        </asp:Panel>
                    </div>
    </asp:View>
    <asp:View runat="server" ID="vwLicenseResults">
         <div class="row">
        <asp:Panel ID="pnlLicenses" runat="server" GroupingText="Recorded Licenses">
            <div class="divGrid">
                <asp:GridView runat="server" Width="98%" ID="grdLicenses" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No licenses found" OnRowDataBound="grdLicenses_RowDataBound" DataKeyNames="REG_LICENSURE_ID,LICENSE_RESTRICTION_CODE_ID">
                    <Columns>
                        <asp:BoundField DataField="LICENSE_NUMBER" HeaderText="License Number" />
                        <asp:BoundField DataField="LICENSE_STATE" HeaderText="State" />
                        <asp:Boundfield Datafield="LICENSE_STATUS" HeaderText="License Status" />
                        <asp:Boundfield Datafield="LICENSE_SUBSTATUS" HeaderText="License Sub-Status" />
                        <asp:BoundField DataField="LICENSE_TYPE_NAME" HeaderText="Type" />
                        <asp:TemplateField HeaderText="Restriction Code">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlLicenseRestrictionCode" CssClass="formDropDown">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
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
    <asp:View runat="server" ID="vwPECOSResults">
        <div id="divPecosDataFields" style="display: none;">
             <div class="row">
            <asp:Panel ID="pnlPecos" runat="server" GroupingText="PECOS Results">
                <%--
            <asp:Label runat="server" ID="lblPECOSRiskLevel" CssClass="formLabel200">Pecos Risk Level*</asp:Label>
            <asp:DropDownList runat="server" ID="ddlPECOSRiskLevel" CssClass="formDropDown" />
             <br />
            <asp:Label ID="lblPECOSState" runat="server" Text="Pecos Enrolled State*" CssClass="formLabel200" />
            <asp:DropDownList ID="ddlPECOSState" runat="server" CssClass="formDropDownMedium" />
             <br />
            <span class="pg-hint2 wdAuto"><asp:Literal ID="ltlPecosHint" runat="server" Text =" <%$ Resources:BrandingResource , SCREENING_PECOS_HELPTEXT %>" /></span>
             <br />
                --%>
                <div class="row">
                    <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lblPECOSBackgroundComplete" CssClass="formLabel300">PECOS background check conducted for each owner?</asp:Label></div>
                    <div class="col-sm-8 text-left"><asp:DropDownList runat="server" ID="ddlPECOSBackgroundComplete" CssClass="formDropDown">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                    <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                </asp:DropDownList></div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lblPECOSBackgroundDate" CssClass="formLabel300">PECOS background check complete date</asp:Label></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtPECOSBackgroundDate" runat="server" CssClass="formField formField" />
                    <cc1:CalendarExtender ID="calPECOSBackgroundDate" TargetControlID="txtPECOSBackgroundDate" runat="server" /></div>
                </div>
            </asp:Panel>
                 </div>
        </div>
    </asp:View>
    <asp:View runat="server" ID="vwSAVEResults">
         <div class="row">
        <asp:Panel ID="pnlSAVE" runat="server" GroupingText="SAVE Results">
            <div class="row">
                <div class="col-sm-4 text-right"><span class="formLabel170">Citizenship Type</span></div>
                <div class="col-sm-8 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAVECitizenshipType" /></div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right"><span class="formLabel170">Alien Number</span></div>
                <div class="col-sm-8 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAVEAlienNumber" /></div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right"><span class="formLabel170">Immigration Status</span></div>
                <div class="col-sm-8 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSAVEImmigrationStatus" /></div>
            </div>
        </asp:Panel>
             </div>
    </asp:View>
    <asp:View ID="vwDEAResults" runat="server">
         <div class="row">
        <asp:Panel ID="pnlDEAData" runat="server" GroupingText="DEA Returned Data">
            <div class="row">
                <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lblDEANumber" CssClass="formLabel170">DEA Registration Number</asp:Label></div>
                <div class="col-sm-8 text-left"><asp:TextBox ID="txtDEANumber" runat="server" CssClass="formFieldDisplay wd250" Enabled="false" /></div>
            </div>
             <div class="row">
                <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lblDEAState" CssClass="formLabel170">State</asp:Label></div>
                <div class="col-sm-8 text-left"><asp:TextBox ID="txtDEAState" runat="server" CssClass="formFieldDisplay wd250" Enabled="false" /></div>
            </div>
           <%-- <div class="row">
                <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lblDEAEffectiveDate" CssClass="formLabel170">Effective Date</asp:Label></div>
                <div class="col-sm-8 text-left"><asp:TextBox ID="txtDEAEffectiveDate" runat="server" CssClass="formFieldDisplay wd250" Enabled="false" />
            <cc1:CalendarExtender ID="calDEAEffectiveDate" TargetControlID="txtDEAEffectiveDate" runat="server" /></div>
            </div>--%>
            <div class="row">
                <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lblDEAExpireDate" CssClass="formLabel170">Expiration Date</asp:Label></div>
                <div class="col-sm-8 text-left"><asp:TextBox ID="txtDEAExpireDate" runat="server" CssClass="formFieldDisplay wd250" Enabled="false" />
                    <cc1:CalendarExtender ID="calDEAExpireDate" TargetControlID="txtDEAExpireDate" runat="server" /></div>
            </div>
             <div class="row">
                <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lblDEAStatus" CssClass="formLabel170">Status</asp:Label></div>
                <div class="col-sm-8 text-left"><asp:TextBox ID="txtDEAStatus" runat="server" CssClass="formFieldDisplay wd250" Enabled="false" /></div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlDEAnumbers" runat="server" GroupingText="DEA Numbers" Visible="false">
            <div class="divGrid">
                <asp:GridView runat="server" Width="98%" ID="grdDEANumbers" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No DEA numbers found" >
                    <Columns>
                        <asp:BoundField DataField="DEA_NUMBER" HeaderText="DEA Registration Number" />
                        <asp:BoundField DataField="DEA_EFF_DATE" HeaderText="Effective Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="DEA_END_DATE" HeaderText="Expiration Date" DataFormatString="{0:d}" />
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
    <asp:View runat="server" ID="vwBackgroundResults">
        <div id="divBackgroundResultsDataFields" class="row">
            <asp:Panel ID="pnlBackgroundResults" runat="server" GroupingText="Background Check Verification">
                <div class="row">
                    <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lbl1" CssClass="formLabel300">Background Check Verification Status</asp:Label></div>
                    <div class="col-sm-8 text-left"><asp:DropDownList runat="server" ID="ddlBackgroundVerificationStatus" CssClass="formDropDown"
                             OnSelectedIndexChanged="ddlBackgroundVerificationStatus_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><asp:Label runat="server" ID="lbl2" CssClass="formLabel300">Background Check Date</asp:Label></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtBackgroundCheckDate" runat="server" CssClass="formField formField" />
                        <cc1:CalendarExtender ID="calBackgroundCheckDate" TargetControlID="txtBackgroundCheckDate" runat="server" /></div>
                </div>
            </asp:Panel>
        </div>
    </asp:View>
    <asp:View runat="server" ID="vwSiteVisitResults">
        <div id="divSiteVisitResultsDataFields"  class="row">
            <asp:Panel ID="pnlSiteVisitResults" runat="server" GroupingText="Site Visit Verification">
                <div class="row">
                    <div class="col-sm-4 text-right"><asp:Label runat="server" ID="Label1" CssClass="formLabel300">Site Visit Verification Status</asp:Label></div>
                    <div class="col-sm-8 text-left"><asp:DropDownList runat="server" ID="ddlSiteVisitVerificationStatus" CssClass="formDropDown" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right"><asp:Label runat="server" ID="Label2" CssClass="formLabel300">Site Visit Date</asp:Label></div>
                    <div class="col-sm-8 text-left"><asp:TextBox ID="txtSiteVisitDate" runat="server" CssClass="formField formField" />
                            <cc1:CalendarExtender ID="calSiteVisitDate" TargetControlID="txtSiteVisitDate" runat="server" /></div>
                </div>
            </asp:Panel>
        </div>
    </asp:View>
    <asp:View ID="vwCDSResults" runat="server">
        <asp:Panel ID="pnlCDSnumbers" runat="server" GroupingText="CDS Numbers" >
            <div class="divGrid">
                <asp:GridView runat="server" Width="98%" ID="grdCDSNumbers" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No CDS numbers found" >
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
        </asp:Panel>
    </asp:View>
        <asp:View runat="server" ID="vwMEDResults">
            <asp:Panel ID="Panel1" runat="server" GroupingText="Medicare Returned Data">
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">NPI</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_NPI" /></div>

                    <div class="col-sm-3 text-right"><span class="formLabel">SSN</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_SSN" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">DOB</span></div>

                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_DOB" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Last Name</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_LastName" /></div>
                    <div class="col-sm-3 text-right"><span class="formLabel">First Name</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_FirstName" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Mid Initial</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_MidInitial" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Organization</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_Org" /></div>
                    <div class="col-sm-3 text-right"><span class="formLabel">Provider Type</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_ProviderType" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Address</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplayLarge" ID="lblMED_Address" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Sanction Type</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplayLarge" ID="lblMED_SanctionType" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Sanction Date</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_SanctionDate" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Reinstatement Date</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_ReinstatementDate" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Date of Death</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_DateOfDeath" /></div>
                    <div class="col-sm-3 text-right"><span class="formLabel">EIN</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_EIN" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">MEDICARE Number</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_MEDICARE" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Waiver Effective Date</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_WaiverEffectiveDate" /></div>
                    <div class="col-sm-3 text-right"><span class="formLabel">Waiver End Date</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_WaiverEndDate" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Waiver Notes</span></div>
                    <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblMED_WaiverNotes" /></div>
                </div>
        </asp:Panel>
    </asp:View>

    <asp:View runat="server" ID="vwDODDResults">
        <div class="row">
        <asp:Panel runat="server" GroupingText="DODD Returned Data">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">First Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblfirstName" /></div>
                <div class="col-sm-3 text-right"><span class="formLabel">SSN</span></div>
                <div class="col-sm-3 text-left">
                        <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblSSN" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Last Name</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblLastName" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Date of Birth</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblDOB" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Registry Date</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblRegistryDate" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel">Registry Reason</span></div>
                <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblRegistryReason" /></div>
            </div>
        </asp:Panel>
            </div>
    </asp:View>

     <asp:View runat="server" ID="vwOHExclSuspension">
     <div class="row">
       <asp:Panel runat="server" GroupingText="Ohio Medicaid Provider Exclusion and Suspension Returned Data">
         <div class="row">
             <div class="col-sm-3 text-right"><span class="formLabel">First Name</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHfname" /></div>
             <div class="col-sm-3 text-right"><span class="formLabel">Last Name</span></div>
             <div class="col-sm-3 text-left">
                     <asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHlname" /></div>
         </div>
         <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel">Middle Name</span></div>
            <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHMname" /></div>
            <div class="col-sm-3 text-right"><span class="formLabel">Tax ID</span></div>
            <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHTaxID" /></div>
         </div>
         <div class="row">
             <div class="col-sm-3 text-right"><span class="formLabel">Organization</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHOrgname" /></div>
             <div class="col-sm-3 text-right"><span class="formLabel">NPI</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHNpi" /></div>
         </div>
         <div class="row">
             <div class="col-sm-3 text-right"><span class="formLabel">Date of Birth</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHDOB" /></div>
             <div class="col-sm-3 text-right"><span class="formLabel">Medicaid ID</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHMedID" /></div>
         </div>
         <div class="row">
             <div class="col-sm-3 text-right"><span class="formLabel">Status</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHStatus" /></div>
             <div class="col-sm-3 text-right"><span class="formLabel">Action Date</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHActionDt" /></div>
         </div>
         <div class="row">
             <div class="col-sm-3 text-right"><span class="formLabel">Provider Type</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHProvType" /></div>
             <div class="col-sm-3 text-right"><span class="formLabel">Address</span></div>
             <div class="col-sm-3 text-left"><asp:Label runat="server" CssClass="formFieldDisplay wd250" ID="lblOHAddress" /></div>
         </div>
       </asp:Panel>
     </div>
 </asp:View>
<asp:View runat="server" ID="vwScreeningResultsGrid"> 
    

  <div id="ExclusionContainer" runat="server" style="padding:24px;" visible="false">
    <div class="px-container">
      <uc:ProviderExclusionPanel ID="ExclusionPanel" runat="server" />
    </div>
  </div>



    <div class="divGrid"  style="display:none;">
      <asp:GridView runat="server" Width="100%" ID="grdScreeningResults"
        AutoGenerateSelectButton="false" AutoGenerateColumns="False"
        CssClass="gridViewSmallFont"
        EmptyDataText="No Screening Match found."
        AllowPaging="false" AllowCustomPaging="false" PageSize="10"
        ShowHeaderWhenEmpty="true" OnPageIndexChanging="grdScreeningResults_PageIndexChanging">
        <Columns>            
            <asp:BoundField DataField="PROVIDER_CLASSIFICATION" HeaderText="Provider Classification" />
            <asp:BoundField DataField="ORG_NAME" HeaderText="Organization Name" />
            <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" />
            <asp:BoundField DataField="MIDDLE_NAME" HeaderText="Middle Name" />
            <asp:BoundField DataField="LAST_NAME" HeaderText="Last Name" />
            <asp:BoundField DataField="SUFFIX" HeaderText="Suffix" />
            <asp:BoundField DataField="SSN" HeaderText="SSN" />
            <asp:BoundField DataField="EIN" HeaderText="EIN" />
            <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="NPI" HeaderText="NPI" />
            <asp:BoundField DataField="DOD" HeaderText="DOD" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="EXCLUSION_AGENCY_ID" HeaderText="Exclusion Agency ID" />
            <asp:BoundField DataField="EXCLUSION_PROGRAM" HeaderText="Exclusion Program" />
            <asp:BoundField DataField="EXCLUSION_AGENCY" HeaderText="Exclusion Agency" />
            <asp:BoundField DataField="EXCLUSION_TYPE" HeaderText="Exclusion Type" />
            <asp:BoundField DataField="EXCLUSION_DATE" HeaderText="Exclusion Date" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="EXCLUSION_STATUS" HeaderText="Exclusion Status" />
            <asp:BoundField DataField="EXCLUSION_TERM_DATE" HeaderText="Exclusion Termination Date" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="REINSTATEMENT_DATE" HeaderText="Reinstatement Date" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="ADDITIONAL_DETAILS" HeaderText="Additional Details" />               
        </Columns>
        <PagerSettings Mode="NumericFirstLast" />
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    </div>
</asp:View>

</asp:MultiView>
    </div>

