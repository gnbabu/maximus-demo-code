<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_Agreements" Codebehind="Agreements.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="mbox" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script src="../Scripts/telerikScripts.js"></script>

<style type="text/css">
    .notes {
        border-color: rgba(0, 0, 0, 0.08);
    }

    div.notes {
        font-style: italic;
        border-width: 1px;
        border-style: solid;
        border-top-width: 0;
        padding: 1em;
    }

    .cpHeader {
        color: #0000FF;
        background-color: White;
        font-size: 9pt;
        font-style: italic;
        cursor: pointer;
        height: 18px;
        padding: 4px;
    }

        .cpHeader a {
            color: #0000FF;
            text-decoration: underline;
        }

    .cpBody {
        background-color: #f0ffff;
        border: 1px gray;
        padding: 4px;
        padding-top: 7px;
    }

    .ajax__combobox_buttoncontainer button {
        height: 18px !important;
        width: 18px !important;
        margin-top: -5px;
    }

    .identModalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.2;
        height: auto;
    }

    .identModalPopup {
        background-color: #FFFFFF;
        border-width: 1px;
        border-style: solid;
        border-color: black;
        padding: 0px;
        width: auto;
        height: auto;
    }

    .underLine {
        display: block;
        border: 3px solid #00008b;
        width: auto;
        margin: auto;
        margin-top: 1px;
        margin-bottom: 2px;
    }

    .pg-hint2 {
        /* fixes alignment issue on password help text */
        text-align: left;
    }

    .ChkBoxClass input {
        width: 25px;
        height: 25px;
        text-align: center;
        font-size: 14px;
        margin: 5px;
    }
</style>
<script type="text/javascript">

    function ChkEnabled(docId) {
        if (docId == 'AP05') {
            window.open('../Documents/Provider%20Participation%20Agreement.pdf', 'newWindow');
        }

        //if (docId == 'AP09') {
        //    window.open('../Documents/Trading%20Partner%20Agreement.pdf', 'newWindow');
        //}

        if (docId == 'AP12') {
            window.open('../Documents/OwnershipDisclosure.pdf', 'newWindow');
        }

        if (docId == 'AP15') {
            window.open('../Documents/W9.pdf', 'newWindow');
        }

        if (docId == 'chkAP17') {
            window.open('../Documents/ACH.pdf', 'newWindow');
        }

        if (docId == 'chkAP18') {
            window.open('../Documents/US Citizenship.pdf', 'newWindow');
        }

        if (docId == 'chkAP20') {
            window.open('../Documents/AuthorizeToReleaseInfoAndAffirmation.pdf', 'newWindow');
        }
    }

    function GetRadioButtonSelectedValue(rbl, qid, qid2) {
        var horiList = rbl.rows[0].cells;
        var radioButtonList = document.getElementById(rbl);

        //var radio = rbl.getElementsByName('input');


        if (qid.id == 'ctl00_MainContent_ucAgreements_rptAgreementQuestions_ctl01_divDQ7') {
            if (horiList[1].firstChild.checked) {
                qid.style.display = 'block';
                return;
            } else {
                qid.style.display = 'none';
            }

        }

        if (qid.id == 'ctl00_MainContent_ucAgreements_rptAgreementQuestions_ctl01_divDQ10') {
            if (horiList[1].firstChild.checked &&
                qid.id == 'ctl00_MainContent_ucAgreements_rptAgreementQuestions_ctl01_divDQ10') {
                qid2.style.display = 'block';
                return;
            } else {
                qid2.style.display = 'none';
            }
        }
    }

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9 ]/g, '');
    }

    $(function () {
        $(".help-business-ownerinfo").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $(".infoBox").css({
                display: 'inline',
                position: 'relative'
            }).show();
        });

        $(".help-business-ownerinfo").mouseleave(function () {
            $(".infoBox").hide();
        });
    });

</script>

<script type="text/javascript">

    function captureSignature() {
        $("#<%= lblMessage.ClientID %>").text("");
        $("#<%= lblMessage.ClientID %>").css("color", "red");
        var sig = $find("<%= RadSignature1.ClientID %>");
        var value = sig.get_value();
        document.getElementById("<%= hdnSignature.ClientID %>").value = value;

        if (!value || value.trim() === "") {
            $("#<%= lblMessage.ClientID %>").text("Please provide a signature before continuing.");
            return false;
        }
        $("#<%= lblMessage.ClientID %>").text("");
        return true;
    }

</script>

<asp:ValidationSummary ID="valAgreements" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Agreements" />
<asp:ValidationSummary ID="ConfirmAgreements" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ConfirmAgreements" />
<asp:Label ID="lblErrorMsg" runat="server" ForeColor="red" Text=""></asp:Label>
<br />
<div id="divAgreement" runat="server">

    <uc1:separator runat="server" id="Separator11" header="State Medicaid Provider Agreement" />
    <hr class="underLine" />
    <asp:Panel runat="server" ID="pnlGeneralProvision">

        <div id="divProviderNote" style="color: #C80000; font-weight: bold; margin-top: 5px; margin-bottom: 3px;">
            Note: The Provider Agreement in the scroll box must be read and responded to in its entirety before proceeding to the next step.
        </div>

        <div runat="server" id="divGeneralProvision" style="line-height: 1.5; height: 300px; overflow-y: auto; padding: 10px;">

            <span style="font-style: italic; font-weight: bold;">All Providers must read the statements below and agree to the terms</span><br />
            <br />
            <span style="font-weight: bold; margin-bottom: 5px;">State Revised Code 2921.42 and 2921.43 Agreement</span>
            <br />
            In accordance with Chapter 102, and Sections 2921.42 and 2921.43 of the State Revised Code, Vendor or Grantee, by signature on 
            this document, certifies: (1) it has reviewed and understands Chapter 102, and Sections 2921.42 and 2921.43 of the State Revised 
            Code, (2) has reviewed and understands the State ethics and conflict of interest laws, and (3) will take no action inconsistent 
            with those laws and this order. The Vendor or Grantee understands that failure to comply with Chapter 102, and Sections 2921.42 
            and 2921.43 of the State Revised Code is, in itself, grounds for termination of this contract or grant and may result in the 
            loss of other contracts or grants with the State. 
            <br />
            <span style="font-weight: bold;">
                <br />
                False Statement Agreement
            </span>
            <br />

            Whoever knowingly and willfully makes, or causes to be made, a false statement or representation on this statement, may be
prosecuted under applicable federal or state laws. In addition, if a person knowingly and willfully fails to fully and accurately disclose
            the information requested State Department of Medicaid may deny the request to participate or, if the entity already participates,
            may terminate the agreement or contract as appropriate.

            <br />
            <asp:CheckBox ID="chkPA01" Enabled="True" runat="server" Text="I agree to Terms and Conditions" CssClass="ChkBoxClass" />
            <br />
            <span style="font-weight: bold;">
                <br />
                State Medicaid Time Limited Provider Agreements</span>
            <br />

            <span style="font-weight: bold;">
                <br />
                Credentialed Providers</span>
            <br />
            <br />
            In accordance with State Administrative Code 5160-1-42, providers who are subject to centralized credentialing are required to 
            revalidate/recredential every 36 months. Failure to recredential within 36 months will result in the termination of this 
            provider agreement<br />

            <span style="font-weight: bold;">
                <br />
                Non-Credentialed Providers</span>
            <br />
            <br />
            For providers that are not subject to centralized credentialing (as specified in rule 5160-1-42 of the Administrative Code), 
            the provider agreement will be limited to no longer than 5 years.  Failure to revalidate within 5 years will result in the 
            termination of this provider agreement.
            <br />
            <br />

            This provider agreement is a contract between the State Department of Medicaid (the Department) and the undersigned provider
of medical assistance services in which the Provider agrees to comply with the terms of this provider agreement, State statutes,
State Administrative Code rules, and Federal statutes and rules, and agrees and certifies to:

            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>1. Render medical assistance services as medically necessary for the patient and only in the amount required by the patient without
                    regard to race, color, age, gender, sexual orientation, marital status, national origin, ancestry, religion, disability or source(s)
                    of payment; submit claims only for services actually performed, and bill the Department for no more than the usual and customary
                    fee charged other patients for the same service.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>2. Ascertain and recoup any third-party resource(s) available to the recipient prior to billing the Department. The Department
                    will then pay any unpaid balance up to the lesser of the provider's billed charge or the maximum allowable reimbursement.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>3. Accept the allowable reimbursement for all covered services as payment-in-full and, except as required in paragraph 2 above,
                    will not seek reimbursement for that service from the patient, any member of the family, or any other person.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>4. Complete all required training related to Electronic Visit Verification and submit documentation of completion prior to enrollment 
                    as a Medicaid provider, and use electronic visit verification as prescribed by Chapter 5160 of the State Administrative Code. 
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>5. Maintain all records necessary and in such form so as to fully disclose the extent of services provided and significant business 
                    transactions. The provider will maintain such records for a period of six years from the date of receipt of payment based upon those 
                    records or until any initiated audit is completed, whichever is longer. 
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>6. Furnish to the Department, the secretary of the Department of Health and Human Services, or the State Medicaid Fraud Control unit or 
                    their designees any information maintained under paragraph 5  above for audit or review purposes. Audits may use statistical 
                    sampling. Failure to supply requested records within thirty days shall result in withholding of Medicaid payments and may result 
                    in termination from the Medicaid program. 
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>7. Inform the Department within thirty days of any changes in licensure, certification, or registration status; ownership; specialty;
                    additions, deletions, or replacements in group membership and hospital-based physicians; and address.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>8. Disclose ownership and control information, and disclose the identity of any person (as specified in 42 CFR, Part 455, Subpart
                    B and 1002, Subpart A, as amended, and as specified in rule 5160-1-17.3 of the Administrative Code) who has been convicted of
                    a criminal offense related to Medicare, Medicaid, or services provided under Title XX of the Social Security Act.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>9. Attest that neither the individual practitioner, nor the company, nor any owner, director, officer, employee of the company,
                    nor any independent contractor retained by the company is currently subject to sanction under Medicare, Medicaid or Title XX
                    or otherwise is prohibited from providing services to Medicare, Medicaid or Title XX beneficiaries.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>10. To follow the regulations and policies set forth in the appropriate edition of the Medicaid Handbook.</li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>11. Comply with State Revised Code 5164.46 (B) (2) Electronic claims submission process; electronic funds transfers; arrange to
                    receive Medicaid payment from the Department by means of electronic funds transfer.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>12. Provide to the Department, through the court of jurisdiction, notice of any action brought by the provider in accordance
                    with the Title 11 of the United States Code (Bankruptcy). Notice shall be mailed to: "Office of Legal Counsel, State Department
                    of Medicaid".
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>13. Comply with the advance directives requirements for hospitals, nursing facilities, providers of home health care and personal
                    care services, hospices, and HMOs specified in 42 CFR 489, Subpart I and 42 CFR 417.436(d).
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>14. Comply with Section 6032 of the Deficit Reduction Act. This requirement applies to health care entities who receive Medicaid
                    reimbursements of $5,000,000 per year or more, to establish written policies for all their own employees and contractors to provide
                    information about the False Claims Act, provide remedies for false claims, a description of false claims laws, whistleblower
                    protections and detailed provisions for detecting and preventing fraud, waste and abuse.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>15. Fully cooperate with the Department, its agents, and other state or federal agencies engaged in ensuring the integrity of
                    the State Medicaid program. Full cooperation includes, but is not limited to, making yourself and your records available upon
                    request.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>16. Limit the Department's liability under this provider agreement to actual amounts owed to Provider for the provision of Medicaid-covered 
                    services to Medicaid recipients. The Department shall not be liable for any (a) losses of revenue, profit, goodwill, or anticipated savings; 
                    (b) losses with respect to business interruption or diminished business value, (c) losses, damages, or amounts owed in connection with any 
                    third-party contract including but not limited to a provider contract with a Medicaid managed care entity, or (d) any incidental, special, 
                    indirect, exemplary, consequential, punitive, or other damages incurred by Provider as a result of, or in connection with, performance or 
                    nonperformance under this provider agreement, whether in contract, tort, strict liability, or any other legal theory, and whether or not such 
                    damages are foreseen or unforeseen. This limitation of liability will survive the termination of this provider agreement.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>17. Indemnify and hold harmless the Department, and its officers, employees, agents, contractors, and representatives from any 
                    and all liabilities, losses, damages, injunctions, injuries to persons, fines, and expenses or demands of any kind, including costs 
                    and attorneys' fees, arising from third party claims which result from or relate to the Provider's obligations under this provider 
                    agreement whether in contract, tort, strict liability, or any other legal theory. This includes but is not limited to claims against 
                    Provider by patients or other individuals where ODM is also named.
                </li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>18. This provider agreement may be canceled by either party upon 30 days written notice prior to termination date.</li>
            </ul>
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>19. I further certify that I am the individual practitioner who is applying for the provider number, or in the case of a business
                    organization, I am the officer, chief executive officer, or general partner of the business organization that is applying for
                    the provider number. I further agree to be bound by this agreement and certify that the information I have given on this application
                    is factual. As such, I have disclosed my name, social security number and date of birth on the application for enrollment, in
                    accordance with 42 CFR, Part 455, Subpart B and 1002, Subpart A, as amended, and as specified in rule 5160-1-17.3 of the Administrative
                    Code.
                </li>
            </ul>
            <asp:CheckBox ID="chkPA02" Enabled="True" runat="server" Text="I agree to Terms and Conditions" CssClass="ChkBoxClass" />
            <br />
            <asp:Label runat="server">Agreement Date: </asp:Label>
            <asp:TextBox runat="server" ID="txtDateAgreeTOC" aria-label="DateAgreeTOC" Enabled="False"></asp:TextBox>
            <br />
            <div id="divProvisionCheck" runat="server" style="line-height: 1.5;">
                <span style="font-weight: bold;">
                    <br />
                    Provision Check
                </span>
                <br />
                Certain provider agreements may be retroactive (up to 12 months) to encompass dates on which the provider furnished covered services
            to a Medicaid consumer and the service has not been billed to Medicaid.
            <br />
                A failure to check this box shall be taken by ODM to mean that you waive your rights to a retroactive period of months prior
            to the
date ODM approves your application. This agreement is limited to 5 years from the effective date.
            <br />
                <br />
                <asp:CheckBox ID="chkPA03" Enabled="True" runat="server" Text="If you meet this provision, please check this box." CssClass="ChkBoxClass" />
            </div>
            <br />
            <br />
            <div id="divLTCAgreement" runat="server" style="line-height: 1.5;">
                <uc1:separator runat="server" id="Separator2" header=" Long Term Care Facility (LTCF) Agreement" style="color: #545487; font-size: 20px; font-weight: bold; padding-bottom: 0; padding-left: 10px;" />
                <hr class="underLine" />
                <span style="font-weight: bold;">
                    <br />
                    Certification Status / Agreement Period
                </span>
                The following terms of this agreement are contingent upon continued certification by the Secretary of the U.S. Department of
                Health and Human Services, or the State Department of Health, which is the state survey agency.
                <span style="font-weight: bold;">
                    <br />
                    Department Responsibilities
                </span>
                This provider agreement is a contract between the State Department of Medicaid and the undersigned provider of Medicaid
                services. ODM shall make payments to the NF provider in accordance with Chapter 5165. of the State Revised Code (ORC) for NF services
                provided
to Medicaid recipients eligible for NF services.
Pursuant to its agreement with ODM under ORC section 5124.02, the State Department of Developmental Disabilities shall make
payments to the ICF‐IID provider in accordance with ORC Chapter 5124. for ICF‐IID services provided to Medicaid recipients eligible
for ICF‐IID services.

                <span style="font-weight: bold;">
                    <br />
                    Long Term Care Facility (LTCF) Provider Responsibilities
                </span>

                The NF or ICF‐IID Provider agrees to comply with the terms of this provider agreement, state statutes, State Administrative Code
                (OAC) rules, and Federal statutes and rules, and agrees and certifies to:
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>A. Include any part of the facility that meets standards for certification of compliance with federal and state laws and rules
                        for participation in the Medicaid program, unless otherwise specified for NFs by ORC section 5165.08 or OAC rule 5160‐3‐02.3,
                        and for ICFs‐IID by ORC section 5124.08 or OAC rule 5123-7-02, or any successor to these laws or rules.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>B. Maintain eligibility for the provider agreement as provided for NFs in section ORC 5165.06 of the and for ICFs‐IID in ORC
                        section 5124.06, including maintaining a valid license to operate if so required by law.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>C. Open its premises for inspection by ODM, the State Department of Health, and any other state or local authority having authority
                        to inspect.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>D. Accept the allowable Medicaid payment for all covered services as payment in full, as stipulated in OAC rule 5160‐1‐
        13.1 or any successor to this rule, and make no additional charge to the resident, any member of the family, or any other person. Exceptions
                        are charges for third‐party resources pursuant to OAC rule 5160‐1‐08 or any successor to this rule, or made in accordance with
                        conditions specified in OAC rules 5160‐3‐16.5 for NFs and 5123-7‐09 for ICFs‐ IID regarding personal needs allowances (PNAs)
                        or any successor to these rules.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>E. Disclose ownership and control information, and disclose the identity of any person who has been convicted of, or indicted
                        for, a criminal offense related to Medicare, Medicaid, Disability Medical Assistance, or Title XX services, and conform to any
                        other disclosure requirements specified in 42 CFR 455 Subpart B, as amended, 42 CFR 1002 Subpart A, as amended, in OAC rule 5160‐1‐17.3,
                        and as referenced in provisions of OAC rule 5160‐3‐02 for NFs and OAC rule 5123-7‐02 for ICFs‐IID, or any successor to these
                        rules.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>F. Supply ODM and other appropriate state and federal personnel, or their designees, such information as may be required to
        disclose the extent of services the Provider furnishes to residents who are, or are eligible to become Medicaid recipients; and
        make available all records relating to the delivery and costs of such services for inspection and audit by ODM.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>G. File reports as required by ODM or DODD and maintain records relating to a cost reporting period for the longer of seven
        (7) years after the cost report is filed or, if an audit report or review is issued, for six (6) years after all appeal rights
        relating to the audit report or review are exhausted.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>H. Assure that neither the company, any owner, director, officer, or employee of the company, nor any independent contractor
                        retained by the company, is currently subject to sanction under Medicare or Medicaid, or is otherwise prohibited from providing
                        services to Medicare or Medicaid beneficiaries.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>I. Comply with the “Deficit Reduction Act of 2005,” Section 6032 “Employee Education About False Claims Recovery” requirements,
                        and ORC section 5162.15, if applicable.
                    </li>
                </ul>

                <span style="font-weight: bold;">
                    <br />
                    Notification by Provider to ODM of Specific Actions and Contact Information
                </span>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>A. Bankruptcy: The Provider must submit to ODM, through the appropriate court of jurisdiction, notice of any action brought
        by the Provider in accordance with Title 7 or Title 11 of the United States Code (Bankruptcy). Notice shall be mailed to “State
        Department of Medicaid, Office of Legal Services”
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>B. Closure: The Provider may terminate this contract by providing ODM, and the residents of the LTCF and their sponsors, written
                        notice of facility closure at least ninety (90) days prior to the facility closure and relocation of residents.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>C. Change of operator: The operator under this provider agreement or owner of the LTCF must provide a written notice as specified
                        by ODM rules at least forty‐five (45) days prior to the operator or owner entering into any transaction that constitutes a change
                        of operator according to OAC rules and the ORC if the change of operator does not entail the relocation of residents. The written
                        notice must be provided at least ninety (90) days prior to the operator or owner entering into any such transaction if the change
                        of operator entails relocation of residents. Per 42 CFR 442.14, the entering operator may accept automatic assignment of this
                        agreement subject to the exiting operator’s terms and conditions or elect to submit a new application and undergo a new certification
                        survey.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>D. Voluntary withdrawal of a NF: When the facility will continue providing nursing home services but the Provider no longer
        chooses to participate in the Medicaid program, the Provider shall provide at least ninety (90) days advance written notice to
        ODM and abide by the provisions for withdrawal specified at section 1919(c)(2) of the Social Security Act.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>E. Voluntary termination of an ICF‐IID: The Provider may terminate this contract by providing ODM, and the residents of the
        ICF‐IID and their sponsors, written notice of the termination at least ninety (90) days prior to such termination.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>F. Certified bed count changes: The Provider must submit a written request to ODM to change the count of certified beds under
                        this agreement.
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>G. Provider to accept service of notices including process and to maintain valid name and addresses with ODM: The Provider
        is responsible for the timely provision and maintenance of the Provider’s name and addresses information on file with ODM. The
        Provider:
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 10px; padding: 0px;">
                    <li>(1) agrees to accept service of notices including service of process upon and
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 10px; padding: 0px;">
                    <li>(2) agrees that the Provider’s name and last known address includes, but is not limited to, the name and address of the statutory
                        agent on file with the State Secretary and any of the following as on file with ODM at the time of service: the corporation
                        name and address, the facility name and address, the mailing name and address, the Chapter 119 name and address, the franchise
                        fee name and address, or the pay‐to name and address.
                    </li>
                </ul>
                <span style="font-weight: bold;">
                    <br />
                    Notification by ODM to Provider of Specific Actions
                </span>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>A. Termination/suspension/non‐revalidation by ODM: ODM may terminate, suspend, or elect to not enter into or to not revalidate
                        this provider agreement upon thirty (30) days written notice to the Provider.
                    </li>
                </ul>
                <span style="font-weight: bold;">
                    <br />
                    Provider Ethics and Campaign Contribution Certification
                </span>
                <br />
                In accordance with ORC Chapter 102. and ORC section 3517.13, the Provider, by the signature in Section 11−PROVIDER SIGNATURE
                of this agreement, certifies:
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>A. It has reviewed and understands the State ethics and conflict of interest laws and it is currently in compliance with and
        will continue to adhere to their requirements,
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>B. Neither it nor any of its partners, administrators, executors, trustees, owners or shareholders, nor the spouse of any
        such person, has made a campaign contribution in excess of the limitations specified in ORC Section 3517.13, and
                    </li>
                </ul>
                <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                    <li>C. It will take no action inconsistent with the laws referenced in this section.
                    </li>
                </ul>
                <span style="font-weight: bold;">
                    <br />
                    Provider Signature
    <br />
                </span>
                <br />
                <asp:UpdatePanel ID="upOptionA" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkPA04" Enabled="True" runat="server" Text="Option A" CssClass="ChkBoxClass" OnCheckedChanged="chkPA04_OnCheckedChanged"
                            AutoPostBack="True" />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <span style="font-weight: bold; padding-left: 15px;">
                    <asp:Label runat="server" ID="lblOptionA" Text="I certify that I am the owner, officer, chief executive officer, general partner, or board member
    of the business organization  entering into this provider agreement to operate this facility in the Medicaid program. I agree to be bound by this agreement
    and all applicable laws. I certify the information submitted on the application and the information as it appears in this provider
    agreement is accurate and complete. I agree that our business organization will notify ODM, in writing, of any subsequent changes
    to the information contained in the application or this agreement."
                        Width="90%">
                    </asp:Label>
                </span>
                <br />
                <br />
                <asp:UpdatePanel ID="upOptionB" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkPA05" Enabled="True" runat="server" Text="Option B" CssClass="ChkBoxClass" OnCheckedChanged="chkPA05_OnCheckedChanged"
                            AutoPostBack="True" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <span style="font-weight: bold; padding-left: 15px;">
                    <asp:Label runat="server" ID="lblOptionB" Text="By my signature below, I certify that I am signing with agent authority from and on behalf of the
    owner, officer, chief executive officer, general partner, or board member of the business organization entering into this provider agreement to operate this
    facility in the Medicaid program and that I have been given the authority to bind the business organization to this agreement
    and all applicable laws. I certify, on the organization’s behalf, that the information submitted on the application and the information
    as it appears in this provider agreement is accurate and complete. Further, by my signature, I am binding the business organization
    to notify ODM, in writing, of any subsequent changes to the information contained in the application or this agreement."
                        Width="90%"></asp:Label>
                </span>

            </div>

            <br />
            <br />

            <div id="divCredentialQuestions" runat="server" style="line-height: 1.5;" visible="false">
                <uc1:separator runat="server" id="Separator1" header="Additional Credentialing Statement" style="color: #545487; font-size: 20px; font-weight: bold; padding-bottom: 0; padding-left: 10px;" />
                <hr class="underLine" />

                <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ConfirmAgreements" />
                <asp:Repeater ID="rptAgreementQuestions" runat="server" OnItemDataBound="rptAgreementQuestions_ItemDataBound">
                    <HeaderTemplate>
                        <div class="legend">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-sm-12" style="padding: 0;">
                                <asp:Label runat="server" ID="lblQuestion" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TEXT]") %>' />
                                <asp:Label runat="server" ID="lblQuestionTypeID" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TYPE_ID]") %>' Visible="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:RadioButtonList ID="rblConfirmQuestion" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                                    <asp:ListItem Selected="False" Text="No" Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Yes" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblQstComment" runat="server" Text="If, 'Yes' a comment is required." />

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:TextBox ID="txtResponseComment" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="500" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="repeaterDivider"></div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>


        </div>


        <br />


        <br />
        <br />



        <div runat="server" id="divIndividualQuestions" visible="false">
            <uc1:separator runat="server" id="Separator6" header="Individual Provider Questions" style="color: #545487; font-size: 20px; font-weight: bold; padding-bottom: 0; padding-left: 10px;" />
            <hr class="underLine" />

            <div runat="server" id="divIndividualQuestionsSection" style="line-height: 1.5; height: 300px; overflow-y: auto; padding: 15px;">

                <asp:ValidationSummary ID="ValidationSummary2" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ConfirmAgreements" />
                <asp:Repeater ID="rptIndividualQuestions" runat="server" OnItemDataBound="rptIndividualQuestions_ItemDataBound">
                    <HeaderTemplate>
                        <div class="legend">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-sm-12" style="padding: 0;">
                                <asp:Label runat="server" ID="lblIndividualQuestion" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TEXT]") %>' />
                                <asp:Label runat="server" ID="lblQuestionTypeID" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TYPE_ID]") %>' Visible="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:RadioButtonList ID="rblConfirmIndividualQuestion" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList" OnSelectedIndexChanged="rblConfirmIndividualQuestion_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="False" Text="No" Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Yes" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;<asp:Label ID="Label8" runat="server" Text="If, 'Yes' a comment is required. If necessary, please upload a document to fully disclose conviction(s)." />

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:TextBox ID="txtIndividualResponseComment" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="500" />
                                <asp:Label ID="lblError" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="repeaterDivider"></div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:HiddenField ID="hdnerror" runat="server" />
            </div>
        </div>


        <uc1:separator runat="server" id="Separator5" header="Provider Agreement Attestation" />
        <hr class="underLine" />
        <div class="row" style="padding: 10px;">

            <asp:CheckBox ID="chkPA07" AssociatedControlID="chkPA07" Enabled="true" runat="server" Text="" CssClass="ChkBoxClass" OnCheckedChanged="chkPA07_OnCheckedChanged" AutoPostBack="True" />
            <span style="font-weight: bold;">
                <asp:Label runat="server" AssociatedControlID="chkPA07" ID="lblAttest" Text="I have read the contents of this application, and the information contained herein is true, correct
                and complete. I agree to notify State Medicaid of any future changes to the information contained in this application. I understand
                that any deliberate omission, misrepresentation, or falsification of any information contained in this application or contained
                in any communication supplying information to State Medicaid may be punished by criminal, civil, or administrative penalties including,
                but not limited to, the denial or revocation of State Medicaid identification number(s), and/or the imposition of fines, civil
                damages, and/or imprisonment. My electronic signature legally and financially binds this provider to the laws, regulations, and
                program instructions of the State Medicaid program. By selecting the signature checkbox and submitting the application, I agree
                to abide by these terms.">
                </asp:Label>
            </span>

        </div>
    </asp:Panel>
    <br />

</div>
<br />

<div id="divSign" runat="server" style="line-height: 1.5;">
    <uc1:separator runat="server" id="Separator4" header="Provider Agreement Signature" style="color: #545487; font-size: 20px; font-weight: bold; padding-bottom: 0; padding-left: 10px;" />
    <hr class="underLine" />

    <br />
    <asp:Panel ID="pnlNeedsSignature" runat="server" DefaultButton="btnSaveSignature">
        <br />
        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
        <table style="padding-left: 20px" role="presentation">
            <tr>
                <td>&nbsp;</td>
                <td>
                    <ms:captchacontrol style="margin-left: auto; margin-right: auto;" id="Captcha1" runat="server"
                        captchabackgroundnoise="Low" captchalength="5" captchaheight="60" captchawidth="200"
                        captchalinenoise="None" captchamintimeout="5" captchamaxtimeout="240" fontcolor="#529E00"
                        tooltip="Captcha Control Type Text in Image" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label2" AssociatedControlID="txtCaptcha" runat="server" CssClass="formLabelAuto">Please enter the characters in the image above:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="10" CssClass="formField220" TabIndex="0" />
                </td>

            </tr>
            <tr>

                <td style="text-align: right">
                    <asp:Label ID="Label4" AssociatedControlID="txtAttester" runat="server" CssClass="formLabelAuto">Name of Person Attesting*:</asp:Label>
                </td>

                <td>
                    <asp:TextBox ID="txtAttester" runat="server" MaxLength="50" aria-label="Attester" CssClass="formField220" TabIndex="0" Visible="false" onKeyUp="javascript:alphanumericOnly(this);" />
                    <asp:DropDownList ID="ddlAttester" runat="server" OnSelectedIndexChanged="ddlAttester_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <span id="helpBusinessNameownerinfo" runat="server" class="help-business-ownerinfo" style="cursor: pointer; display: inline-block;">
                        <asp:Image ID="Image1" AlternateText="help" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
                    </span>
                    <span id="helpBusinessNameInfoownerinfo" runat="server" class="infoBox">
                        <span class="infoTitle"></span>
                        <span class="infoContent">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:BrandingResource , OTHER_HELPTEXT %>"></asp:Literal>
                        </span>
                    </span>
                </td>

            </tr>
            <tr>

                <td style="text-align: right">
                    <asp:Label ID="Label5" runat="server" CssClass="formLabelAuto">Provider Name:</asp:Label>
                </td>

                <td>
                    <asp:TextBox ID="txtProviderName" runat="server" aria-label="ProviderName" TextMode="SingleLine" CssClass="formField220" CausesValidation="false" TabIndex="0"
                        Enabled="False" />
                </td>
            </tr>
            <tr>

                <td style="text-align: right">
                    <asp:Label ID="Label6" runat="server" CssClass="formLabelAuto">User ID:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserId" runat="server" aria-label="UserId" TextMode="SingleLine" CssClass="formField220" CausesValidation="false" TabIndex="0"
                        Enabled="False" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSignHere" runat="server" CssClass="formLabelAuto">Sign here:</asp:Label>
                </td>
                <td>
                    <div class="signature-wrapper">
                        <div id="signatureWrapper" runat="server" style="position:relative;">
                            <telerik:radsignature runat="server" id="RadSignature1" onprerender="RadSignature1_PreRender"
                                  height="200px" width="600px" rounded="None">
                            </telerik:radsignature>
                        </div>
                        <asp:HiddenField ID="hdnSignature" runat="server" />
                        <asp:Button ID="btnSaveSignature" runat="server" 
                            OnClientClick="return captureSignature();" Enabled="true" 
                            CausesValidation="false" Text="Save" 
                            CssClass="buttonBox" TabIndex="0"
                            style="margin-top:10px;"
                            OnClick="btnSaveSignature_Click" />

                    </div>
                </td>

            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
    </asp:Panel>
</div>



<div id="divSubmittedAgreements" runat="server">
    <br />
    <asp:Panel ID="pnlSubmittedAgreements" runat="server">
        <uc1:separator id="Separator8" runat="server" header="Submitted Agreements" />
        <hr class="underLine" />
        <br />
        <asp:GridView ID="gvSubmittedAgreements" runat="server" AutoGenerateColumns="False"
            HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
            CssClass="gridViewSmallFont" EmptyDataText="No uploaded documents found."
            OnRowCommand="gvSubmittedAgreements_RowCommand" DataKeyNames="FILE_NAME,ONBASE_DOCUMENT_ID" AllowPaging="true" AllowCustomPaging="true" OnPageIndexChanging="gvSubmittedAgreements_PageIndexChanging" OnRowDataBound="gvSubmittedAgreements_RowDataBound">
            <Columns>
                <asp:BoundField DataField="AGREEMENT_SUBMITTED_DATE" HeaderText="Agreement Submitted Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="AGREEMENT_EFFECTIVE_DATE" HeaderText="Agreement Effective Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="UserName" HeaderText="Submitted By" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="REG_ID" HeaderText="Registration ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="FILE_NAME" HeaderText="File Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                    Visible="false" />
                <asp:BoundField DataField="ONBASE_DOCUMENT_ID" HeaderText="ONBASE_DOCUMENT_ID" Visible="false" />
                <asp:TemplateField HeaderText="Search" ItemStyle-HorizontalAlign="Center" ShowHeader="false">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgView" alt="Search Button" ImageUrl="~/Images/search.png" runat="server" ToolTip="View" CommandName="View_File" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:Panel>
</div>

<div id="divUpdateWFAgreements" runat="server">
    <br />
    <asp:Panel ID="pnlUpdateWFAgreements" runat="server">
        <uc1:separator id="Separator9" runat="server" header="Update Workflow Agreements" />
        <hr class="underLine" />
        <br />
        <asp:GridView ID="gvUpdateWFAgreements" runat="server" AutoGenerateColumns="False"
            HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
            CssClass="gridViewSmallFont" EmptyDataText="No uploaded documents found."
            OnRowCommand="gvUpdateWFAgreements_RowCommand" DataKeyNames="FILE_NAME,ONBASE_DOCUMENT_ID" AllowPaging="true" AllowCustomPaging="true" OnPageIndexChanging="gvUpdateWFAgreements_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="AGREEMENT_SUBMITTED_DATE" HeaderText="Agreement Submitted Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="UserName" HeaderText="Submitted By" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="REG_ID" HeaderText="Registration ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="FILE_NAME" HeaderText="File Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                    Visible="false" />
                <asp:BoundField DataField="ONBASE_DOCUMENT_ID" HeaderText="ONBASE_DOCUMENT_ID" Visible="false" />
                <asp:TemplateField HeaderText="Search" ItemStyle-HorizontalAlign="Center" ShowHeader="false">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgView1" alt="Search Button" ImageUrl="~/Images/search.png" runat="server" ToolTip="View" CommandName="View_File" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:Panel>
</div>

<asp:TextBox ID="hidHasSigned" runat="server" Visible="false" />
<asp:TextBox ID="TextBox1" runat="server" Visible="false" />

<mbox:messagebox id="MessageBox1" runat="server" />

<cc1:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy"
    cancelcontrolid="btnCancel" backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlModal">
</cc1:modalpopupextender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" align="center" Style="display: none">
    <asp:Panel ID="pnlHeaderMpe" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div style="text-align: left;">
            &nbsp;&nbsp;
            <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
    </asp:Panel>
    <table style="padding-bottom: 10px" role="presentation">
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBox" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="valAgreements" />
            </td>
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />