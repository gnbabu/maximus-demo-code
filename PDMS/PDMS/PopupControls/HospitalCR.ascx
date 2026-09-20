<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospitalCR" Codebehind="HospitalCR.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>

<script type="text/javascript">
    $(document).ready(function () {
        var max_fields = 11;
        var wrapper = $(".input_fields_wrap");
        var add_button = $(".add_field_button");

        var x = 1;
        $(add_button).click(function (e) {
            e.preventDefault();
            if (x < max_fields) {
                x++;
                $(wrapper).append('<div><input type="file" name="attachment[]"/><a href="#" class="remove_field">Delete</a></div>');
            }
        });

        $(wrapper).on("click", ".remove_field", function (e) {
            e.preventDefault();
            $(this).parent('div').remove();
            x--;
        })
    });
</script>
<style type="text/css">
    .radioButtonList {
        margin-left: 0px !important;
        margin-right: 0px !important;
    }

    table, th, td {
        border: 1px solid black;
    }

    td, th {
        padding: 5px;
    }

    input, select, textarea {
        max-width: 280px;
        margin-left: 10px;
    }

    .column {
        float: left;
        width: 55%;
        padding: 10px;
        height: 50px;
    }
</style>

<asp:HiddenField ID="RegIdTxt" runat="server" />
<div class="row col-sm-12" style="border: groove; margin-left: 10px">
    <ajax:CollapsiblePanelExtender ID="cpeInstructions" runat="server" Collapsed="false" TargetControlID="pnlInstructions"
        ExpandControlID="pnlSepInstructions" CollapseControlID="pnlSepInstructions" />
    <asp:Panel runat="server" ID="pnlSepInstructions" class="CollapsingSeparator"  onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepInstructions" Header="- Upload Cost Report" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInstructions" class="OwnerBackground">
        <div class="row" style="background-color: cornflowerblue; padding-left: 10px">Upload Cost Report</div>
        <div class="row" style="background-color: lightblue; padding-top: 10px">
            <div class="col-sm-2">
                <span>
                    <asp:Label ID="Label1" runat="server" Text="Facility Program Type:" CssClass="ohio-field" Style="font-size: 15px; text-align: right" />
                </span>
            </div>
            <div class="col-sm-4">
                <span>
                    <asp:DropDownList ID="dlFacProgType" AutoPostBack="false" runat="server" Style="min-width: 450px; height: 30px"></asp:DropDownList>
                </span>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-6">
                    <span>
                        <asp:Label ID="Label3" runat="server" Text="Medicaid Hospital Cost Report (Excel)" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upMedicaidHospitalCostReport" ViewStateMode="Enabled" CssClass="fileControl" />
                    </span>
                    <p>
                        <asp:Label ID="lblMedicaidHospitalCostReport" runat="server" CssClass="bodyTextBold" />
                    </p>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label2" runat="server" Text="Tracking ID" CssClass="ohio-field" Style="font-size: 15px; margin-left: 48px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtMedicaidHospitalCostReportId" runat="server" CssClass="ohio-field-input" ReadOnly="true" Enabled="false" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-6">
                    <span>
                        <asp:Label ID="Label4" runat="server" Text="Medicare Cost Report (EC)" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upMedicareCostReportEC" ViewStateMode="Enabled" CssClass="fileControl" />
                    </span>
                    <p>
                        <asp:Label ID="LblMedicareCostReportEC" runat="server" CssClass="bodyTextBold" />
                    </p>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label6" runat="server" Text="Tracking ID" CssClass="ohio-field" Style="font-size: 15px; margin-left: 48px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtMedicareCostReportECId" runat="server" CssClass="ohio-field-input" ReadOnly="true" Enabled="false" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-6">
                    <span>
                        <asp:Label ID="Label7" runat="server" Text="Medicare Cost Report-(PI)" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upMedicareCostReportPI" ViewStateMode="Enabled" CssClass="fileControl" />
                    </span>
                    <p>
                        <asp:Label ID="LblMedicareCostReportPI" runat="server" />
                    </p>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label9" runat="server" Text="Tracking ID" CssClass="ohio-field" Style="font-size: 15px; margin-left: 48px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtMedicareCostReportPIId" runat="server" CssClass="ohio-field-input" ReadOnly="true" Enabled="false" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-6">
                    <span>
                        <asp:Label ID="Label10" runat="server" Text="Medicare Certification Statement" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upCertificationStatement" ViewStateMode="Enabled" CssClass="fileControl" ReadOnly="true" Enabled="false" />
                    </span>
                    <p>
                        <asp:Label ID="LblCertificationStatement" runat="server" />
                    </p>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label12" runat="server" Text="Tracking ID" CssClass="ohio-field" Style="font-size: 15px; margin-left: 48px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtCertificationStatementId" runat="server" CssClass="ohio-field-input" ReadOnly="true" Enabled="false" Style="/* color: gray; */
    background-color: aliceblue;" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-6">
                    <span>
                        <asp:Label ID="Label13" runat="server" Text="IRIS Report" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upIRISReport" ViewStateMode="Enabled" CssClass="fileControl" ReadOnly="true" Enabled="false" />
                    </span>
                    <p>
                        <asp:Label ID="lblIRISReport" runat="server" />
                    </p>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label15" runat="server" Text="Tracking ID" CssClass="ohio-field" Style="font-size: 15px; margin-left: 48px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="TextBox9" runat="server" CssClass="ohio-field-input" ReadOnly="true" Style="/* color: gray; */
    background-color: aliceblue;"
                        Enabled="false" />
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-6">
                    <span>
                        <asp:Label ID="Label19" runat="server" Text="Other Document" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upOtherDOC" ViewStateMode="Enabled" CssClass="fileControl" />
                    </span>
                    <p>
                        <asp:Label ID="LblOtherDOC" runat="server" />
                    </p>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label21" runat="server" Text="Tracking ID" CssClass="ohio-field" Style="font-size: 15px; margin-left: 48px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtOtherDOCId" runat="server" ReadOnly="true" CssClass="ohio-field-input" BackColor="White" />
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label22" runat="server" Text="Other Document Description" CssClass="ohio-field" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtOtherDescr" runat="server" CssClass="ohio-field-input" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="col-sm-5">
                    <asp:PlaceHolder ID="dynamicCtrl" runat="server"></asp:PlaceHolder>
                </div>

                <div class="col-sm-7">
                    <span style="text-align: right;">
                        <div class="input_fields_wrap">
                            <a href="#" backcolor="#3366FF" class="add_field_button">Add Document</a>
                        </div>
                </div>

            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-sm-4">
                <div class="col-sm-6">
                    <span>
                        <asp:Label ID="Label23" runat="server" Text="Settelement Type" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="dlSettelementType" AutoPostBack="false" runat="server" CssClass="ohio-field" Style="max-width: 200px; min-width: 200px;"></asp:DropDownList>
                    </span>
                    <p>
                        <asp:Label ID="Label24" runat="server" />
                    </p>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label25" runat="server" Text="Settelement Amount" CssClass="bodyTextBold" Font-Bold="True" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtSettelmentAmt" runat="server" CssClass="ohio-field-input" />
                </div>
            </div>
            <div class="col-sm-4" visible="false" runat="server">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label26" runat="server" Text="Tracking Id" CssClass="ohio-field" Style="font-size: 15px;" />
                    </span>
                </div>
                <div class="col-sm-6">
                    <asp:TextBox ID="txtTrackingId" runat="server" CssClass="ohio-field-input" ReadOnly="true" />
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
            <asp:Label  runat="server"  ID="docValidatedLabel" ForeColor="Red"></asp:Label>
            </div>
        <div class="row">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                <asp:HiddenField ID="postBack" runat="server" Value="false" />
                <asp:Button runat="server" Text="Upload" ID="btnSaveCostReport" CssClass="btn btn-success" OnClick="btnSaveCostReport_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" Height="40px" Width="120px" />
                <asp:Button runat="server" ID="btnCancelCostReport" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancelCostReport_Click" BackColor="#FF3300" Font-Bold="True" ForeColor="White" Height="40px" Width="120px"/>
            </div>
        </div>
        </span>
    </asp:Panel>
</div>
<%--Cost report submit section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px">
    <ajax:CollapsiblePanelExtender ID="cpSubmitterApproval" runat="server" Collapsed="true" TargetControlID="pnlSubmitterApproval"
        ExpandControlID="pnlsepSubmitterApproval" CollapseControlID="pnlsepSubmitterApproval" />
    <asp:Panel runat="server" ID="pnlsepSubmitterApproval" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepSubmitterApproval" Header="- Submitter's Certfication" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSubmitterApproval" class="OwnerBackground">
        <div class="row">
            <span style="text-align: left">
                <asp:CheckBox ID="chkSubmitCr" runat="server" OnCheckedChanged="chkSubmitCr_CheckedChanged" AutoPostBack="true" Text="By checking this box you are submitting the cost report and agreeing to the statement below. An automated email will be sent to the preparer of this cost report and provider admin regarding the submission of this cost report." />
            </span>
        </div>
        <span style="text-align: left">INTENTIONAL MISREPRESENTATION OR FALSIFICATION OF ANY INFORMATION CONTAINED HEREIN MAY BE PUNISHABLE BY FINE AND/OR IMPRISONMENT UNDER FEDERAL AND/OR SATE LAW. CERTIFICATION BY OFFICER OF THE PROVIDER I HEREBY CERTIFY THAT:
        <br />
        1) I have examined this statement, the accompanying supporting schedules, the allocation of expenses and services, and the attached worksheets for the cost report year () and that to the best of my knowledge and belief they are true and accurate statements prepared from the books and records of the Provider in accordance with applicable instructions.<br /> &nbsp;2) The expenditures included in this statement are based on the actual cost of recorded expenditures.
        <br />
        3) All cost included herein comply with certified public expenditure (CPE) requirements and all local, state and federal requirements (including that the funds were not federal funds in origin, or are federal funds authorized by federal law to be used to match other federal funds and that the claimed expenditure were not used to meet matching requirements under other federally funded programs).
        <br />
        4) Federal funds are being claimed on this report in accordance with the cost report instructions provided by the ODE effective for the above reporting period.
        <br />
        5) I am the officer authorized by the referenced provider to submit this form and I have made a good faith effort to assure that all information reported is true and accurate.
        <br />
        6) I understand that this information will be used as a basis for claims for federal funds, and possibly state funds, and the falsification and concealment of a material fact may be prosecuted under federal or state civil or criminal law.

        </span>

        <div class="row" id="Div10" runat="server">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                <asp:Button runat="server" Text="Confirm" ID="btnconfirmsubmitCr" Enabled="false" OnClick="btnConfirmApproval_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" Height="40px" Width="120px" />
                <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="buttonBox" TabIndex="3" OnClick="bntCancelSig_Click"  BackColor="#FF3300" />
            </div>
        </div>
    </asp:Panel>
</div>
<%--Capctha--%>
<div id="divSign" runat="server" style="line-height: 1.5;" visible="false">
    <div class="row col-sm-12" style="border: groove; margin-left: 10px">
        <ajax:CollapsiblePanelExtender ID="cpCredentail" runat="server" Collapsed="true" TargetControlID="pnlNeedsSignature"
            ExpandControlID="pnlsepSubmitterApproval" CollapseControlID="pnlsepSubmitterApproval" />
        <asp:Panel runat="server" ID="pnlsepcredentials" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
            <uc1:SectHd runat="server" ID="sepCredentials" Header="- Credential Validation " />
        </asp:Panel>

        <asp:Panel ID="pnlNeedsSignature" runat="server" DefaultButton="btnSaveSignature">
            <br />
            <center>
            <table style="padding-left: 20px">
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <ms:CaptchaControl Style="margin-left: auto; margin-right: auto;" ID="Captcha1" runat="server"
                            CaptchaBackgroundNoise="Low" CaptchaLength="5" CaptchaHeight="60" CaptchaWidth="200"
                            CaptchaLineNoise="None" CaptchaMinTimeout="5" CaptchaMaxTimeout="240" FontColor="#529E00"
                            ToolTip="Captcha Control Type Text in Image" />
                    </td>
                   <%-- <td>&nbsp;</td>--%>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label AssociatedControlID="txtCaptcha" runat="server" CssClass="formLabelAuto">Please enter the characters in the image above:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="10" CssClass="formField220" TabIndex="1" />
                    </td>

                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label AssociatedControlID="txtPassword" runat="server" CssClass="formLabelAuto">Enter password:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="formField220" CausesValidation="true" TabIndex="2" Text="" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="pg-hint2">The password requested is your user login password.</td>
                </tr>
                <tr>

                    <td style="text-align: right">
                        <asp:Label ID="Label16" runat="server" CssClass="formLabelAuto">Submitter Name:</asp:Label>
                    </td>

                    <td>
                        <asp:TextBox ID="txtSubName" runat="server" TextMode="SingleLine" CssClass="formField220" CausesValidation="false" TabIndex="2" />
                    </td>
                </tr>
                <tr>

                    <td style="text-align: right">
                        <asp:Label ID="Label17" runat="server" CssClass="formLabelAuto">Submitter User ID:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubUserId" runat="server" TextMode="SingleLine" CssClass="formField220" CausesValidation="false" TabIndex="2" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSaveSignature" runat="server" CausesValidation="false" Text="Confirm" CssClass="buttonBox" TabIndex="3"
                            OnClick="btnSaveSignature_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" />
                         <asp:Button ID="btncancelSig" runat="server" Text="Cancel" CssClass="buttonBox" TabIndex="3" OnClick="bntCancelSig_Click" BackColor="#FF5050" Font-Bold="True" ForeColor="White" />
                    </td>
                </tr>
            </table>
                </center>
        </asp:Panel>
    </div>
</div>
