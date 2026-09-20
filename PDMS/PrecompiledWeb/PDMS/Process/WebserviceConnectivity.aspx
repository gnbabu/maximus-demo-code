<%@ page title="WebService Connectivity" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_WebserviceConnectivity, App_Web_unbhbgmw" enableEventValidation="false" stylesheettheme="Default" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>
<%@ register src="~/PopupControls/TestELicenseVerification.ascx" tagname="TestELicenseVerification" tagprefix="ucTEV" %>
<%@ register src="~/PopupControls/RevalidationTesting.ascx" tagname="RevalidationTesting" tagprefix="ucRT" %>
<%@ register src="~/PopupControls/PriorAuthTesting.ascx" tagname="PriorAuthTesting" tagprefix="ucPAT" %>
<%@ register src="~/PopupControls/ProviderFinancialTesting.ascx" tagname="ProviderFinancialTesting" tagprefix="ucPFT" %>
<%@ register src="~/PopupControls/HospiceTesting.ascx" tagname="HospiceTesting" tagprefix="ucHST" %>
<%@ register src="~/PopupControls/ClaimsTesting.ascx" tagname="ClaimsTesting" tagprefix="ucCLT" %>
<%@ register src="~/PopupControls/MemberEligibilityTesting.ascx" tagname="MemberEligibilityTesting" tagprefix="ucMET" %>
<%@ register src="~/PopupControls/UploadAttachmentTesting.ascx" tagname="UploadAttachmentTesting" tagprefix="uc" %>
<%@ register src="~/PopupControls/AMATesting.ascx" tagname="AMATesting" tagprefix="ucAMA" %>
<%@ register src="~/PopupControls/VerifySecrets.ascx" tagname="VerifySecretsTesting" tagprefix="ucVST" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function pageLoad() {
            $('[data-toggle="popover"]').popover()
        }
    </script>

    <style type="text/css">
        .RightBox {
            width: 860px;
        }

        .UserHeader {
            width: 1120px;
        }

        .rtsSelected .rtsLink {
            background-color: cornflowerblue;
            font-weight: bold;
            color: #000000;
        }
    </style>


    <%--Quick note: 
    Separated the every tab, added external user controls for both Revalidation Testing and Test eLicense Verification forms to register in the .aspx
    the other two forms such as Search Transaction and Incident Compliance are interlink one to another so leave it to be in the same .aspx to avoid the confusion
    --%>
    <telerik:radskinmanager id="RadSkinManager1" runat="server" />
    <div class="demo-container no-bg">
        <telerik:radtabstrip rendermode="Lightweight" runat="server" id="RadTabStrip1" multipageid="RadMultiPage1" selectedindex="0" skin="Silk">
            <tabs>
                <telerik:radtab text="Search Transaction" width="250px" pageviewid="PageView1">
                </telerik:radtab>
                <telerik:radtab text="Incident Compliance" width="250px" pageviewid="RadPageView2">
                </telerik:radtab>
                <telerik:radtab text="Revalidation Testing" width="250px" pageviewid="RadPageView1">
                </telerik:radtab>
                <telerik:radtab text="Test eLicense Verification" width="250px" pageviewid="RadPageView3">
                </telerik:radtab>
                <telerik:radtab text="Prior-Auth Transactions" width="250px" pageviewid="RadPageView4">
                </telerik:radtab>
                <telerik:radtab text="Provider Financial Services" width="250px" pageviewid="RadPageView7">
                </telerik:radtab>
                <telerik:radtab text="Claims Services" width="250px" pageviewid="RadPageView8">
                </telerik:radtab>
                <telerik:radtab text="Hospice Services" width="250px" pageviewid="RadPageView9">
                </telerik:radtab>
                <telerik:radtab text="Member Eligibility Services" width="250px" pageviewid="RadPageView10">
                </telerik:radtab>
                <telerik:radtab text="Upload Attachment Status" width="250px" pageviewid="RadPageView6">
                </telerik:radtab>
                <telerik:radtab text="AMA Testing" width="250px" pageviewid="RadPageView11">
                </telerik:radtab>
                <telerik:radtab text="Verify Secrets" width="250px" pageviewid="RadPageView12">
                </telerik:radtab>
            </tabs>
        </telerik:radtabstrip>
        <telerik:radmultipage runat="server" id="RadMultiPage1" selectedindex="0" cssclass="innerMultiPage">
            <telerik:radpageview runat="server" id="PageView1">
                <br />
                <div style="border-top: 2px solid black; margin-bottom: -10px">
                    <br />
                </div>
                <div class="ingredients qsf-ib">
                    <asp:Panel ID="Panel2" runat="server">
                        <div id="div1" runat="server" class="test-left" style="text-align: left;">
                            <span class="pdsSectionHeader" id="Span1" runat="server">
                                <h1>Search Transaction</h1>
                            </span>
                            <hr />
                        </div>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                    <asp:Label ID="lblRegID" CssClass="ohio-field" AssociatedControlID="txtRegID" runat="server">
                                        <span class="ohio-field-label">Registration ID: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="enter the regID for with transaction was created" aria-hidden="true">
                                            <asp:Image ID="imgInfoIcon16" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                        </span>
                                        </span>
                                        <asp:TextBox ID="txtRegID" CssClass="ohio-field-input" runat="server" />
                                    </asp:Label>
                                </div>
                                <div class="col-sm-6 col-md-8 col-lg-9 outerName">
                                    <asp:Label ID="lblTxnResult" CssClass="ohio-field" AssociatedControlID="txtTxnResult" runat="server">
                                        <span class="ohio-field-label">TransactionID Result: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Result for the particular registration ID" aria-hidden="true">
                                            <asp:Image ID="imgInfoIcon13" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                        </span>
                                        </span>
                                        <asp:TextBox ID="txtTxnResult" CssClass="ohio-field-input" runat="server" Height="37px" ReadOnly="true" TextMode="MultiLine" />
                                    </asp:Label>
                                </div>
                            </div>
                            <div class="btnBox btnBoxCenter">
                                <asp:UpdatePanel ID="udtpnlGetTransaction" runat="server" UpdateMode="Conditional">
                                    <contenttemplate>
                                        <div class="row">
                                            <asp:Button ID="btnGetTransaction" Width="200px" runat="server" Text="Search Transaction ID" CssClass="buttonBoxFocus" OnClick="btnGetTransaction_Click" />
                                        </div>
                                    </contenttemplate>
                                    <triggers>
                                        <asp:PostBackTrigger ControlID="btnGetTransaction" />
                                    </triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlFinacialProviderInformation" runat="server">
                        <div id="divHeader_1" runat="server" class="test-left" style="text-align: left;">
                            <span class="pdsSectionHeader" id="pdsSectionHeader_1" runat="server">
                                <h2>Request Response</h2>
                            </span>
                            <hr />
                        </div>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-sm-9 col-md-9 col-lg-9">
                                    <div class="container-fluid">
                                        <div class="row">
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="lblTransactionID" CssClass="ohio-field" AssociatedControlID="txtTransactionID" runat="server">
                                                    <span class="ohio-field-label">TransactionID: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Please enter the Transaction ID, use the above fields to fetch your transaction id" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon15" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="txtTransactionID" CssClass="ohio-field-input" runat="server" />
                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="lblCertificate" CssClass="ohio-field" AssociatedControlID="tbcert" runat="server">
                                                    <span class="ohio-field-label">Certificate Name: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Not being used" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon1" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="tbcert" CssClass="ohio-field-input" runat="server" />
                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="lblUser" CssClass="ohio-field" AssociatedControlID="txtUser" runat="server">
                                                    <span class="ohio-field-label">UserName: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Not being used" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="txtUser" CssClass="ohio-field-input" runat="server" />
                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="lblPassword" CssClass="ohio-field" AssociatedControlID="txtPassword" runat="server">
                                                    <span class="ohio-field-label">Password: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Not being used" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon2" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="txtPassword" CssClass="ohio-field-input" runat="server" />

                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="lblSubscriberCliams" CssClass="ohio-field" AssociatedControlID="tbSubscriber" runat="server">
                                                    <span class="ohio-field-label">Subscriber System: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Please subscriber system as FI/EDI" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon3" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="tbSubscriber" CssClass="ohio-field-input" runat="server" />
                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="lbltxnid" CssClass="ohio-field" AssociatedControlID="tbSubscriberId" runat="server">
                                                    <span class="ohio-field-label">Subscriber ID: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Please subscriber Id" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon4" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="tbSubscriberId" CssClass="ohio-field-input" runat="server" />
                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="Label13" CssClass="ohio-field" AssociatedControlID="txtAckTargetSys" runat="server">
                                                    <span class="ohio-field-label">Ack Req Target System: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Not being used" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon5" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="txtAckTargetSys" CssClass="ohio-field-input" runat="server" />
                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                                <asp:Label ID="lblWebServiceType" class="ohio-select" AssociatedControlID="ddlWebServiceType" runat="server">
                                                    <span tabindex="0" class="ohio-select-label">WebService Type <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_5' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_SEARCH_POPUP_PROVIDER_APPTYPE %>' />" aria-hidden="true">
                                                        <asp:Image ID="Image5" Height="13" AlternateText="INFO" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span></span>
                                                    <asp:DropDownList ID="ddlWebServiceType" height="38px" CssClass="ohio-field-input" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlWebServiceType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                                                <asp:Label ID="lblGenXML" CssClass="ohio-field" AssociatedControlID="txtGenXML" runat="server">
                                                    <span class="ohio-field-label">Generated XML: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Generated XML for that transaction ID" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon6" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="txtGenXML" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                                                </asp:Label>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                                                <asp:Label ID="lblResponse" CssClass="ohio-field" AssociatedControlID="txtResponse" runat="server">
                                                    <span class="ohio-field-label">SI Response: <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Here you wil see Response From SI when Make Request is clicked" aria-hidden="true">
                                                        <asp:Image ID="imgInfoIcon7" AlternateText="InfoLink" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" />
                                                    </span>
                                                    </span>
                                                    <asp:TextBox ID="txtResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                                                </asp:Label>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <div class="container-fluid" style="height: 400px; overflow-y: scroll;">
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateXML" Width="300px" runat="server" Text="Generate ProvManagement Enroll XML" CssClass="buttonBoxFocus" OnClick="btnGenerateXML_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateUpdateXML" Width="300px" runat="server" Text="Generate ProvManagement Update XML" CssClass="buttonBoxFocus" OnClick="btnGenerateUpdateXML_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGeneratePartialPSMXML" Width="300px" runat="server" Text="Generate PartialManagement PSM XML" CssClass="buttonBoxFocus" OnClick="btnGeneratePartialPSMXML_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGeneratePartialPCWXML" Width="300px" runat="server" Text="Generate PartialManagement PCW XML" CssClass="buttonBoxFocus" OnClick="btnGeneratePartialPCWXML_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnEnrollRequestApi" Width="300px" runat="server" Text="Make Enroll ProviderManagement Request" Enabled="true" CssClass="buttonBoxFocus" OnClick="btnEnrollRequest_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnUpdateRequestApi" Width="300px" runat="server" Text="Make Update ProviderManagement Request" Enabled="true" CssClass="buttonBoxFocus" OnClick="btnUpdateRequest_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateClaimsSearchXML" Width="300px" runat="server" Text="Generate Claims Search XML" CssClass="buttonBoxFocus" OnClick="btnGenerateClaimsSearchXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateClaimsAddUpdateXML" Width="300px" runat="server" Text="Generate Claims  Dental Add Update XML" CssClass="buttonBoxFocus" OnClick="btnGenerateClaimsAddUpdateXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateClaimsInquiryXML" Width="300px" runat="server" Text="Generate Claims Dental Inquiry XML" CssClass="buttonBoxFocus" OnClick="btnGenerateClaimsInquiryXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateClaimsProfessionalAddUpdateXML" Width="300px" runat="server" Text="Generate Claims  Professional Add Update XML" CssClass="buttonBoxFocus" OnClick="btnGenerateClaimsAddUpdateProsessionalXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateClaimsProfessionalInquiryXML" Width="300px" runat="server" Text="Generate Claims Professional Inquiry XML" CssClass="buttonBoxFocus" OnClick="btnGenerateClaimsInquiryProsessionalXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateClaimsInstitutionalInquiryXML" Width="300px" runat="server" Text="Generate Claims Institutional Inquiry XML" CssClass="buttonBoxFocus" OnClick="btnGenerateClaimsInquiryInstXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateClaimsInstitutionalAddUpdateXML" Width="300px" runat="server" Text="Generate Claims  Institutional Add Update XML" CssClass="buttonBoxFocus" OnClick="btnGenerateClaimsAddUpdateInstXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnCreateAuthXML" Width="300px" runat="server" Text="Generate Create Auth XML" CssClass="buttonBoxFocus" OnClick="btnGenerateCreateAuthXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnInquireAuthXML" Width="300px" runat="server" Text="Generate Inquire Auth XML" CssClass="buttonBoxFocus" OnClick="btnGenerateInquireAuthXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnSearchAuthXML" Width="300px" runat="server" Text="Generate Search Auth XML" CssClass="buttonBoxFocus" OnClick="btnGenerateSearchAuthXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnUpdateAuthXML" Width="300px" runat="server" Text="Generate Update Auth XML" CssClass="buttonBoxFocus" OnClick="btnGenerateUpdateAuthXML_Click" />

                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnmakeAttachmentRequest" Width="300px" runat="server" Text="Make Attachment Request" CssClass="buttonBoxFocus" OnClick="btnmakeAttachmentRequest_Click" />
                                            &nbsp;
                              
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateAttachmentXML" Width="300px" runat="server" Text="Generate Attachment XML" CssClass="buttonBoxFocus" OnClick="btnGenerateAttachmentXML_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnSearchAuth" Width="300px" runat="server" Text="Make Search PriorAuth Request" CssClass="buttonBoxFocus" OnClick="btnSearchAuth_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnInquireAuth" Width="300px" runat="server" Text="Make Inquire PriorAuth Request" CssClass="buttonBoxFocus" OnClick="btnInquireAuth_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnCreateAuth" Width="300px" runat="server" Text="Make AddUpdate PriorAuth Request" CssClass="buttonBox" OnClick="btnAddUpdateAuth_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenXMLPriorAuthSer" Width="300px" runat="server" Text="Generate Search PriorAuth XML" CssClass="buttonBox" OnClick="btnGenXMLPriorAuthSer_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenXMLPriorAuthInq" Width="300px" runat="server" Text="Generate Inquire PriorAuth XML" CssClass="buttonBox" OnClick="btnGenXMLPriorAuthInq_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenXMLPriorAuthAU" Width="300px" runat="server" Text="Generate AddUpdate PriorAuth XML" CssClass="buttonBox" OnClick="btnGenXMLPriorAuthAU_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnClaimsAddUpdateInstituational" Width="300px" runat="server" Text="Make Claims Instituational AddUpdate Request" CssClass="buttonBoxFocus" OnClick="btnClaimsInstituationalAddUpdate_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnClaimsInquireInstituational" Width="300px" runat="server" Text="Make Claims Instituational Inquire Request" CssClass="buttonBoxFocus" OnClick="btnClaimsInstituationalInquire_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnClaimsAddUpdateProfessional" Width="300px" runat="server" Text="Make Claims Professional AddUpdate Request" CssClass="buttonBoxFocus" OnClick="btnClaimsProfessionalAddUpdate_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnClaimsInquireProfessional" Width="300px" runat="server" Text="Make Claims Professional Inquire Request" CssClass="buttonBoxFocus" OnClick="btnClaimsProfessionalInquire_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnClaimsAddUpdate" Width="300px" runat="server" Text="Make Claims Dental AddUpdate Request" CssClass="buttonBoxFocus" OnClick="btnClaimsAddUpdate_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnClaimsInquire" Width="300px" runat="server" Text="Make Claims Dental Inquire Request" CssClass="buttonBoxFocus" OnClick="btnClaimsInquire_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnClaimsSearch" Width="300px" runat="server" Text="Make Claims Search Request" CssClass="buttonBoxFocus" OnClick="btnClaimsSearch_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnMemberEligibilitySearch" Width="300px" runat="server" Text="Member Eligibility Search Request" CssClass="buttonBoxFocus" OnClick="btnMemberEligibilitySearch_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnDocument" Width="300px" runat="server" Text="Make Document Request" CssClass="buttonBox" OnClick="btnDocument_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnHospiceAddUpdate" Width="300px" runat="server" Text="Make HospiceAddUpdate Request" CssClass="buttonBox" OnClick="btnHospiceAddUpdate_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnHospiceInquire" Width="300px" runat="server" Text="Make HospiceInquire Request" CssClass="buttonBox" OnClick="btnHospiceInquire_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnHospiceSearch" Width="300px" runat="server" Text="Make HospiceSearch Request" CssClass="buttonBox" OnClick="btnHospiceSearch_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnRequestAck" Width="300px" runat="server" Text="Make Acknowledgement Request" CssClass="buttonBox" OnClick="btnRequestAck_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateAckReq" Width="300px" runat="server" Text="Generate Acknowledgement Request" CssClass="buttonBox" OnClick="btnGenerateAckReq_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateDocument" Width="300px" runat="server" Text="Generate Document XML" CssClass="buttonBoxFocus" OnClick="btnGenerateDocumentXml_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateHospiceAddUpdateXml" Width="300px" runat="server" Text="Generate Hospice AddUpdate XML" CssClass="buttonBoxFocus" OnClick="btnGenerateHospiceAddUpdateXml_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateHospiceInquireRequestXML" Width="300px" runat="server" Text="Generate Hospice Inquire XML" CssClass="buttonBoxFocus" OnClick="btnGenerateHospiceInquireRequestXML_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnGenerateHospiceXML" Width="300px" runat="server" Text="Generate Hospice XML" CssClass="buttonBoxFocus" OnClick="btnGenerateHospiceXML_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnProcess" Width="300px" runat="server" Text="Process" CssClass="buttonBoxFocus" OnClick="btnProcess_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="btnLTCHomeProviderFileClean" Width="300px" runat="server" Text="LTCHomeProvider File CleanUp" CssClass="buttonBoxFocus" OnClick="btnLTCHomeProviderFileClean_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="Button1" Width="300px" runat="server" Text="LTC Home File CleanUp" CssClass="buttonBoxFocus" OnClick="btnLTCHomeFileClean_Click" />
                                        </div>
                                        <div class="row btnBox btnBoxCenter">
                                            <asp:Button ID="Button2" Width="300px" runat="server" Text="LTC Provider File CleanUp" CssClass="buttonBoxFocus" OnClick="btnLTCProviderFileClean_Click" />
                                        </div>

                                        <div class="btnBox btnBoxCenter">
                                            <br />
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <triggers>
                                                    <asp:PostBackTrigger ControlID="btnGenXMLPriorAuthAU" />
                                                    <asp:PostBackTrigger ControlID="btnGenXMLPriorAuthInq" />
                                                    <asp:PostBackTrigger ControlID="btnGenXMLPriorAuthSer" />
                                                    <asp:PostBackTrigger ControlID="btnGenerateAttachmentXML" />
                                                    <asp:PostBackTrigger ControlID="btnmakeAttachmentRequest" />
                                                    <asp:PostBackTrigger ControlID="btnCreateAuth" />
                                                    <asp:PostBackTrigger ControlID="btnInquireAuth" />
                                                    <asp:PostBackTrigger ControlID="btnSearchAuth" />
                                                    <asp:PostBackTrigger ControlID="btnClaimsSearch" />
                                                    <asp:PostBackTrigger ControlID="btnClaimsInquire" />
                                                    <asp:PostBackTrigger ControlID="btnClaimsAddUpdate" />
                                                    <asp:PostBackTrigger ControlID="btnGenerateHospiceXML" />
                                                    <asp:PostBackTrigger ControlID="btnHospiceSearch" />
                                                    <asp:PostBackTrigger ControlID="btnHospiceInquire" />
                                                    <asp:PostBackTrigger ControlID="btnRequestAck" />
                                                    <asp:PostBackTrigger ControlID="btnGenerateAckReq" />
                                                    <asp:PostBackTrigger ControlID="btnGenerateHospiceInquireRequestXML" />
                                                    <asp:PostBackTrigger ControlID="btnHospiceAddUpdate" />
                                                    <asp:PostBackTrigger ControlID="btnDocument" />
                                                    <asp:PostBackTrigger ControlID="btnGenerateHospiceAddUpdateXml" />
                                                    <asp:PostBackTrigger ControlID="btnGenerateDocument" />
                                                    <asp:PostBackTrigger ControlID="btnClaimsInquireProfessional" />
                                                    <asp:PostBackTrigger ControlID="btnClaimsAddUpdateProfessional" />
                                                    <asp:PostBackTrigger ControlID="btnClaimsInquireInstituational" />
                                                    <asp:PostBackTrigger ControlID="btnClaimsAddUpdateInstituational" />
                                                    <asp:PostBackTrigger ControlID="btnMemberEligibilitySearch" />
                                                    <asp:PostBackTrigger ControlID="btnProcess" />
                                                </triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="divRRSearchHeader" class="RetrieveReportsSearchHeader" runat="server" visible="false">TRANSACTION QUEUE Results</div>
                    <mms:sortablepaginggridview
                        id="gvQueryTransactionQueue"
                        runat="server"
                        autogeneratecolumns="False"
                        cssclass="gridViewSmallFont" width="100%"
                        allowsorting="true"
                        emptydatatext="No Records Found"
                        role="presentation"
                        rowstyle-verticalalign="Top"
                        allowpaging="True"
                        pagesize="15"
                        gridviewsortcolumn="" gridviewsortdirection="Ascending"
                        datakeynames="TQID">
                        <columns>
                            <asp:BoundField DataField="TQID" HeaderText="TRANSACTION_QUEUE_ID" />
                            <asp:BoundField DataField="TQREG_ID" HeaderText="REG_ID" />
                            <asp:BoundField DataField="SI_RESPONSE_TYPE" HeaderText="SI_RESPONSE_TYPE" />
                            <asp:BoundField DataField="SI_RESPONSE_MESSAGE" HeaderText="SI_RESPONSE_MESSAGE" />
                            <asp:BoundField DataField="SI_TRANSACTION_KEY" HeaderText="SI_TRANSACTION_KEY" />
                            <asp:BoundField DataField="RESPONSE_DETAILS" HeaderText="RESPONSE_DETAILS" />
                            <asp:BoundField DataField="TD_CREATE_DATE" HeaderText="CREATE_DATE" />
                        </columns>
                    </mms:sortablepaginggridview>
                    <asp:HiddenField ID="hdnRowCount" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView1">
                <br />
                <div class="ingredients qsf-ib">
                    <ucrt:revalidationtesting id="RevalidationTesting1" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView2">
                <br />
                <div style="border-top: 2px solid black; margin-bottom: -10px">
                    <br />
                </div>
                <div class="ingredients qsf-ib">
                    <asp:Panel ID="pnlIMS" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;" GroupingText="Incident Compliance">
                        <div>
                            <div class="row">
                                <div class="col-sm-2 text-right">
                                    <span class="formLabel150">
                                        <asp:Label ID="Label1" runat="server" Text="Medicaid ID"></asp:Label>
                                    </span>
                                </div>
                                <div class="col-sm-4 text-left">
                                    <asp:TextBox ID="txtMedID" runat="server" aria-Label="Medicaid ID"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 text-right">
                                    <span class="formLabel150">
                                        <asp:Label ID="lbl2" runat="server" Text="IMSAssociateID"></asp:Label>
                                    </span>
                                </div>
                                <div class="col-sm-4 text-left">
                                    <asp:TextBox ID="txtAssociateID" runat="server" aria-Label="Associate ID"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 text-right">
                                    <span class="formLabel150">
                                        <asp:Label ID="Label2" runat="server" Text="Case Number"></asp:Label>
                                    </span>
                                </div>
                                <div class="col-sm-4 text-left">
                                    <asp:TextBox ID="txtCasenum" runat="server" aria-Label="Case Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 text-right">
                                    <span class="formLabel150">
                                        <asp:Label ID="lblDate" runat="server" Text="Start Date"></asp:Label>
                                    </span>
                                </div>
                                <div class="col-sm-4 text-left">
                                    <asp:TextBox ID="txtDate" runat="server" aria-Label="Date"></asp:TextBox>
                                    <cc1:calendarextender id="calDate" targetcontrolid="txtDate" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 text-right">
                                    <span class="formLabel150">
                                        <asp:Label ID="Label4" runat="server" Text="End Date"></asp:Label>
                                    </span>
                                </div>
                                <div class="col-sm-4 text-left">
                                    <asp:TextBox ID="txtEndDate" runat="server" aria-Label="Date"></asp:TextBox>
                                    <cc1:calendarextender id="CalendarExtender1" targetcontrolid="txtEndDate" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-2 text-right">
                                </div>
                                <div class="col-sm-4 text-left">
                                    <asp:Button ID="btnIMS" runat="server" Text="Generate IMS Request" CssClass="buttonBox" OnClick="btnIMS_Click" />
                                    <asp:Button ID="btnIMSResp" runat="server" Text="Get IMS Response" CssClass="buttonBox" OnClick="btnIMSResp_Click" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView3">
                <br />
                <div class="ingredients qsf-ib">
                    <uctev:testelicenseverification id="ucTestELicenseVerification" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView4">
                <div class="ingredients qsf-ib">
                    <ucpat:priorauthtesting id="PriorAuthTestingID" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView7">
                <div class="ingredients qsf-ib">
                    <ucpft:providerfinancialtesting id="ProviderFinancialTestingID" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView9">
                <div class="ingredients qsf-ib">
                    <uchst:hospicetesting id="HospiceTesting" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView8">
                <div class="ingredients qsf-ib">
                    <ucclt:claimstesting id="ClaimsTesting" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView10">
                <div class="ingredients qsf-ib">
                    <ucmet:membereligibilitytesting id="MemberEligibilityTesting" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView6">
                <div class="ingredients qsf-ib">
                    <uc:uploadattachmenttesting id="UploadAttachmentTestingID" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView11">
                <div class="ingredients qsf-ib">
                    <ucama:amatesting id="AMATesting" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView12">
                <div class="ingredients qsf-ib">
                    <ucvst:verifysecretstesting id="VerifySecretsTesting" runat="server" />
                </div>
            </telerik:radpageview>
        </telerik:radmultipage>
    </div>
</asp:Content>
