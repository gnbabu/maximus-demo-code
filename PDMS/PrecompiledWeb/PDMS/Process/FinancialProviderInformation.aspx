<%@ page title="Self Service" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_FinancialProviderInformation, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="display: inline;">
        <h1 id="lblTitle">Self Service Information</h1>

    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        a.add-row {
            display: block;
        }
        .errormsg{
            color: #DD2316;
        }
    </style>
    <script type="text/javascript">
        function OnClientLoad() {           
            var currentItem = document.getElementById("<%=hdnLblM.ClientID %>").value;            
            if (currentItem == "true") {
                $('#ctl00_MainContent_lblMedicaidNumber').css('display', 'none');
            }           
          
        }
        window.onload = function () {
            OnClientLoad();
        };
    </script>
    <asp:HiddenField ID="hdnLblM" runat ="server"></asp:HiddenField>
    <asp:Panel ID="pnlFinacialProviderInformation" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
        <div class="row MedicaidNumber" id="pnlMedicaid" runat="server">
            <table role="presentation">
                <tr>
                    <td>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <h2 class="formLabel200" id="lblMedicaidNumber" runat="server" style="margin-top: 05px; font-size: 20px">Medicaid Number*</h2>
                        </div>
                    </td>
                    <td>
                        <div class="col-sm-9 text-left">
                            <span style="text-align: left;">
                                <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" CssClass="errormsg" ValidationGroup="valProviderInfoHeader" />
                                <asp:TextBox ID="txtMedicaidNumber" runat="server" CssClass="formField" MaxLength="12" aria-required="true" aria-label="Enter Medicaid Number and select the link from below"/>
                                <asp:RequiredFieldValidator ID="rfvMEDBillNum" runat="server" ControlToValidate="txtMedicaidNumber" ErrorMessage="*Enter Medicaid Number" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                            </span>
                            <br />
                            <br />
                        </div>
                    </td>

                    <td>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label class="formLabel200" id="lblMedicaidNumber2" runat="server" style="margin-top: 05px; font-size: 20px">Medicaid Number</asp:Label>
                        </div>
                    </td>

                    <td>
                        <div class="col-sm-9 text-left">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtMedicaidNumber2" runat="server" aria-label="Medicaid Number" CssClass="formField" AutoPostBack="true" />
                            </span>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>

                    <td>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label class="formLabel200" id="lblRegID" runat="server" style="margin-top: 05px; font-size: 20px">Reg ID</asp:Label>
                        </div>
                    </td>

                    <td>
                        <div class="col-sm-9 text-left">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtRegID1" runat="server" aria-Label="Reg Id" CssClass="formField" MaxLength="10" AutoPostBack="true"  />
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <div class="col-sm-9 text-left">
                            <span style="text-align: left;">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtRegID1"
                                    ValidationExpression="\d{0,10}" ErrorMessage="* Enter a numeric Reg ID Number with max 10 digits."
                                     Enabled="true" SetFocusOnError="true" />
                            </span>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td>
                        <div style="margin-left: 25px">
                            <h3 style="font-size: 14pt !important;"><b>Select the appropriate link</b></h3>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <div style="margin-left: 25px">
                            <h4><span style="color: darkblue"><b>Self Service</b></span></h4>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td>
                        <div class="row" style="padding-left: 40px;">
                            <asp:LinkButton ID="lnkBtnRemittance" runat="server" CssClass="add-row" OnCommand="BtnAdd_Remittance" Visible="false">View Remittance Advice</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnFinancial" runat="server" CssClass="add-row" OnCommand="BtnAdd_Financial" Visible="false">View Provider Financials</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnPriorAuth" runat="server" CssClass="add-row" OnCommand="BtnAdd_PriorAuth" Visible="false">View Prior Authorizations</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnClaims" runat="server" CssClass="add-row" OnCommand="BtnAdd_Claims" Visible="false">View Claims</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnHospice" runat="server" CssClass="add-row" OnCommand="BtnAdd_Hospice" Visible="false">View Hospice</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnMemberElig" runat="server" CssClass="add-row" OnCommand="BtnAdd_MemberElig" Visible="false">View Member Eligibility</asp:LinkButton>
                            <asp:LinkButton ID="lnlBtnORPProviderSearch" runat="server" CssClass="add-row" OnCommand="BtnAdd_ORPProviderSearch" Visible="false">Ordering, Referring, & Prescribing Search</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtnCostReports" runat="server" CssClass="add-row" OnCommand="BtnAdd_NavigateToMAS" Visible="false">Cost Reports and Rate Settings</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnProviderReport" runat="server" CssClass="add-row" OnCommand="lnkBtnProviderReport_Command" Visible="false">View Provider Reports</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnAttachments" runat="server" CssClass="add-row" OnCommand="lnkBtnAttachments_Command" Visible="false">Attachments</asp:LinkButton>
                        </div>
                    </td>
                    <td></td>
                    <td>
                        <div class="row" style="padding-left: 40px;">

                            <asp:LinkButton ID="lnkbtnCorrespondence" runat="server" CssClass="add-row" OnCommand="BtnAdd_Correspondence" Visible="false">View Correspondence</asp:LinkButton>

                        </div>
                    </td>
                </tr>
            </table>

        </div>
    </asp:Panel>
</asp:Content>
