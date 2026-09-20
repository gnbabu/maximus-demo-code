<%@ Page Title="Self Service" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_FinancialProviderInformation" Codebehind="FinancialProviderInformation.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="display: inline;">
        <h1 id="lblTitle" style="font-weight: bold;">Self Service Information</h1>

    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        :root {
            --theme_color: #112e51;
        }

        a.add-row {
            display: block;
        }

        .serform {
            position: relative
        }

            .serform .errormsg, .serform span {
                position: absolute;
                bottom: -4px;
                font-size: 14px;
                left: 0
            }

            .errormsg, .serform span {
                color: #DD2316;
            }

        .serviceonesection {
            background: #ffffff;
            /*            height: 528px
*/
        }

        .servicetwosection {
            background: #e6ebed;
            height: 528px
        }

        .serviceone, .servicetwo {
            padding: 2rem;
        }

        .serTitle {
            font-size: 2rem;
            font-weight: 600;
            padding-bottom: 2rem;
            border-bottom: 1px solid #ccc;
            display: flex;
            width: 100%;
            justify-content: space-between
        }

        .formsection {
            padding: 2rem 1rem
        }

        formsection label {
            padding-top: 2rem;
        }

        .formsection input {
            width: 100%;
            height: 52px;
            margin-bottom: 14px;
        }

        .serLinks a {
            padding: 1rem 3rem 1rem 1.5rem;
            margin: 0.2rem 1rem;
            color: #6C489C;
            /*background: var(--theme_color);*/
            border-radius: 11px;
            position: relative;
            transition: all 400ms ease-in-out;
            min-width: 180px;
            font-weight: bold;
            white-space: nowrap;
            display: table
        }

            .serLinks a:after {
                content: '';
                position: absolute;
                width: 11px;
                height: 3px;
                background: #fff;
                right: 10px;
                top: 17px;
                z-index: 999;
                transform: rotate(45deg)
            }

            .serLinks a:before {
                content: '';
                position: absolute;
                width: 11px;
                height: 3px;
                background: #fff;
                right: 10px;
                top: 24px;
                z-index: 999;
                transform: rotate(-45deg)
            }


            .serLinks a:hover {
                color: #ffffff;
                background: var(--theme_color);
                padding: 1rem 5rem 1rem 1.5rem;
                transition: all 400ms ease-in-out;
            }

        .val-title-section {
            display: flex;
            justify-content: start;
            align-items: center;
        }

            .val-title-section .errormsg {
                color: #fff;
                background-color: rgb(238 0 0 / 50%);
                padding: 0.5rem 1.2rem;
                animation: fadeInAnimation ease 2s infinite;
                margin-left: 1rem;
            }

        @keyframes fadeInAnimation {
            0% {
                opacity: 0;
            }

            100% {
                opacity: 1;
            }
        }

        .server_box {
            width: 500px !important;
            margin-top: 3rem;
            margin-bottom: 3rem;
            margin-right: auto;
        }

        #wrapper {
            /*background: linear-gradient(131deg, rgba(241,241,252,1) 0%, rgba(200,229,255,1) 100%)*/
            background-color: #f7f7f7;
        }

        .WhiteBox, .OwnerBackground {
            border-radius: 14px;
            padding: 20px 40px 40px 40px !important
        }

        .row.col-sm-12 {
            border-width: 0px !important;
            border-color: transparent !important;
        }

        .row.col-sm-13 {
            border-width: 0px !important;
            border-color: transparent !important;
        }

        .row.col-sm-15 {
            border-width: 0px !important;
            border-color: transparent !important;
        }

        .CollapsingSeparator {
            background: #7993ac !important;
            padding: 8px !important;
            margin-bottom: 4px !important
        }

            .CollapsingSeparator h1, .CollapsingSeparator h2 {
                padding: 0px !important;
                margin: 0px !important
            }

        h1 .pageHeader, h2 .pageHeader {
            color: #fff !important
        }

        .RadGrid_DCPDMS {
            border: 0px solid transparent !important
        }

        .panelHeaderStyle {
            background-color: transparent !important;
            border-style: none;
        }
    </style>
    <div class="server_box">

        <asp:Panel ID="pnlFinacialProviderInformation" runat="server">
            <div class="row MedicaidNumber " id="pnlMedicaid" runat="server">
                <div class="col-md-12 serviceonesection">
                    <div class="serviceone">
                        <div class="serTitle">
                            <div>Self Service Information</div>
                            <div>
                                <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" CssClass="errormsg" ValidationGroup="valProviderInfoHeader" />
                            </div>
                        </div>
                        <div class="formsection">
                            <label>Medicaid Number</label>
                            <div class="serform">
                                <asp:TextBox ID="txtMedicaidNumber" runat="server" CssClass="formField" MaxLength="12" aria-required="true" aria-label="Enter Medicaid Number and select the link from below" />
                                <asp:RequiredFieldValidator ID="rfvMEDBillNum" runat="server" ControlToValidate="txtMedicaidNumber" ErrorMessage="*Enter Medicaid Number" Text="Enter Medicaid Number" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                            </div>

                            <label>
                                <asp:Label ID="lblRegID" runat="server">Reg ID</asp:Label></label>
                            <div class="serform">
                                <asp:TextBox ID="txtRegID1" runat="server" aria-Label="Reg Id" CssClass="formField" MaxLength="10" AutoPostBack="true" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtRegID1"
                                    ValidationExpression="\d{0,10}" ErrorMessage="* Enter a numeric Reg ID Number with max 10 digits."
                                    Enabled="true" SetFocusOnError="true" />
                            </div>
                        </div>


                        <div class="serLinks">
                            <asp:LinkButton ID="lnkBtnRemittance" runat="server" CssClass="add-row" OnCommand="BtnAdd_Remittance" Visible="false">View Remittance Advice</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnPriorAuth" runat="server" CssClass="add-row" OnCommand="BtnAdd_PriorAuth" Visible="false">View Prior Authorizations</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnClaims" runat="server" CssClass="add-row" OnCommand="BtnAdd_Claims" Visible="false">View Claims</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnAttachments" runat="server" CssClass="add-row" OnCommand="lnkBtnAttachments_Command" Visible="false">Attachments</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnMemberElig" runat="server" CssClass="add-row" OnCommand="BtnAdd_MemberElig" Visible="false">View Member Eligibility</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnHospice" runat="server" CssClass="add-row" OnCommand="BtnAdd_Hospice" Visible="false">View Hospice Enrollment</asp:LinkButton>
                            <asp:LinkButton ID="lnkBtnProviderReport" runat="server" CssClass="add-row" OnCommand="lnkBtnProviderReport_Command" Visible="false">View Provider Reports</asp:LinkButton>
                            <asp:LinkButton ID="LnkBtnCostReports" runat="server" CssClass="add-row" OnCommand="lnkBtnProviderFinance_Click" Visible="false">View Provider Financial</asp:LinkButton>
                            <asp:LinkButton ID="lnkbtnCorrespondence" runat="server" CssClass="add-row" OnCommand="BtnAdd_Correspondence" Visible="false">View Correspondence</asp:LinkButton>
                            <asp:LinkButton ID="lnlBtnORPProviderSearch" runat="server" CssClass="add-row" OnCommand="BtnAdd_ORPProviderSearch" Visible="false">View ORP Search</asp:LinkButton>

                        </div>

                    </div>

                </div>

            </div>
        </asp:Panel>
    </div>
</asp:Content>
