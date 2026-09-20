<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_MSPCR, App_Web_guw1elnn" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>
<script runat="server">

    protected void btncancelSig_Click(object sender, EventArgs e)
    {

    }
</script>

<script type="text/javascript">
	$(document).ready(function() {
  var max_fields = 6;
  var wrapper = $(".input_fields_wrap");
  var add_button = $(".add_field_button");

  var x = 1;
  $(add_button).click(function(e) {
    e.preventDefault();
    if (x < max_fields) {
      x++;
      $(wrapper).append('<div><input type="file" name="attachment[]"/><a href="#" class="remove_field linkDeleteButton">Delete</a></div>');
    }
  });

  $(wrapper).on("click", ".remove_field", function(e) {
    e.preventDefault();
    $(this).parent('div').remove();
    x--;
  })
});
    function CollapseExpand(obj, pnlName) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
        var collPanel = $find(pnlName);
        if (collPanel.get_Collapsed())
            collPanel.set_Collapsed(false);
        else
            collPanel.set_Collapsed(true);

    }
</script>
<style type="text/css">
    .radioButtonList {
        margin-left: 0px !important;
        margin-right: 0px !important;
    }

    /*table, th, td {
        border: 1px solid black;
    }*/

    td, th {
        padding: 5px;
    }
    .viewdetailpanel{

       border: 1px solid black;
    }
     table.viewdetailpanel  tr  td{
        border: 1px solid black;
    }

     .credentialPanel {
        border:none !important;
        margin-left:30px;
    }
    table.credentialPanel  tr  td{
        border:none !important;
    }

    input, select, textarea {
        max-width: 100%;
        margin-left: 10px;
    }

    .column {
        float: left;
        width: 55%;
        padding: 10px;
        height: 50px;
    }
    .linkButton{
        text-decoration:none !important; 
        border: 1px solid #2297bc;
        padding:5px;
        background: lightblue;
        border-radius: 10px;
    }
     .linkDeleteButton{
        text-decoration:none !important; 
        border: 1px solid #2297bc;
        padding:5px;
        border-radius: 10px;
        background: #f0b3a0
    }
</style>


<div class="row">
    <div class="col-sm-2  text-right">
        <asp:Label ID="lblMedicaid" runat="server" CssClass="formLabel400" Text="Provider Medicaid ID:" Visible="false"></asp:Label>

    </div>
    <div class="col-sm-2  text-left">
        <asp:Label ID="lblMedicaidTxt" runat="server" CssClass="formLabel400" Text="" Visible="false"></asp:Label>
    </div>
    <div class="col-sm-2  text-right">
        <asp:Label ID="lblNPI" runat="server" CssClass="formLabel400" Text="Provider NPI:" Visible="false"></asp:Label>
    </div>

    <div class="col-sm-2  text-left">
        <asp:Label ID="lblNPITxt" runat="server" CssClass="formLabel400" Text="" Visible="false"></asp:Label>
    </div>
    <div class="col-sm-2  text-right">
        <asp:Label ID="lblProName" runat="server" CssClass="formLabel400" Text="Provider Name:" Visible="false"></asp:Label>
    </div>
    <div class="col-sm-2  text-left">
        <asp:Label ID="lblProNameTxt" runat="server" CssClass="formLabel400" Text="" Visible="false"></asp:Label>
    </div>
</div>

<asp:HiddenField ID="RegIdTxt" runat="server" />
<asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="valMSPCostReport" ShowSummary="true" />
<%--Save cost report section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px" id="divSaveCostReport" runat="server">
    <ajax:CollapsiblePanelExtender ID="cpeInstructions" runat="server" Collapsed="false" TargetControlID="pnlInstructions"
        ExpandControlID="pnlSepInstructions" CollapseControlID="pnlSepInstructions" />
    <asp:Panel runat="server" ID="pnlSepInstructions" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpeInstructions');" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepInstructions" Header="- Upload Cost Report" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInstructions" class="OwnerBackground">
        <div class="row" id="divtitle" runat="server" style="background-color: cornflowerblue; padding-left: 10px">Upload Cost Report</div>
        <div class="row" id="trFacProgType" runat="server" style="background-color: lightblue; padding-top: 10px">
            <div class="col-sm-2">
                <span>
                    <asp:Label ID="Label1" runat="server" Text="Facility Program Type" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
            </div>
            <div class="col-sm-8">
                <span style="text-align: left;">
                    <asp:DropDownList ID="dlFacProgType" AutoPostBack="false" runat="server" Style="min-width: 450px; height: 30px"></asp:DropDownList></span>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="row" id="trStartDate" runat="server">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Cost Report Fiscal Year</span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtCstRptFiscalYear" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" Enabled="false" />

                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-4">
            <div class="row" id="trEndDate" runat="server">
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Cost Report Due Date</span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtCstRptDueDate" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" Enabled="false" />
                        <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtCstRptDueDate" runat="server" />
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="valOrgInfo"
                            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtCstRptDueDate"
                            ErrorMessage="Select a valid Cost Report To Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                            SetFocusOnError="true"> </asp:CompareValidator>
                    </span>
                </div>
            </div>
        </div>
        <div class="row" id="Div1" runat="server">
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="margin-left: 60px;">
                        <asp:Label ID="Label11" runat="server" Text=" * " CssClass="ohio-field" Style="font-size: 20px; margin-left:10px;color:red; text-align: right; width:auto !important;" Font-Bold="True" />
                         <asp:Label ID="Label3" runat="server" Text="Pre-Audited MSP Cost Report" CssClass="ohio-field" Style="font-size: 15px;width:auto !important; text-align: right" Font-Bold="True" />
                    </span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">

                        <mms:EncryptedFileUpload runat="server" ID="upPreAuditedMSPCostReport" Width="400px" ViewStateMode="Enabled"  CssClass="fileControl" />

                    </span>
                    <p>
                        <b>
                            <asp:Label ID="lblPreAuditedMSPCostReport" runat="server" /></b>
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style= "margin-left:60px;">
                        <asp:Label ID="Label14" runat="server" Text=" * " CssClass="ohio-field" Style="font-size: 20px; margin-left:10px;color:red; text-align: right; width:auto !important" Font-Bold="True" />
                        <asp:Label ID="Label4" runat="server" Text="Attestation of Findings Report" CssClass="ohio-field" Style="font-size: 15px; width:auto !important; text-align: right" Font-Bold="True" />
                       <%-- <asp:Label ID="Label14" runat="server" Text=" *" CssClass="ohio-field" Style="font-size: 20px; margin-left:10px;color:red; text-align: right" Font-Bold="True" />--%>
                    </span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">

                        <mms:EncryptedFileUpload runat="server" ID="upAttestationofFindingsReport" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" />

                    </span>
                    <p>
                        <b>
                            <asp:Label ID="lblAttestationofFindingsReport" runat="server" /></b>
                    </p>
                </div>
            </div>
        </div>
        <div class="row" id="Div2" runat="server">
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style= "margin-left:60px;">
                        <asp:Label ID="Label18" runat="server" Text=" * " CssClass="ohio-field" Style="font-size: 20px; margin-left:10px;color:red; text-align: right;width:auto !important;" Font-Bold="True" />
                        <asp:Label ID="Label5" runat="server" CssClass="ohio-field" Style="font-size: 15px;width:auto !important; text-align: right" Text="Post Audited MSP Cost Report" Font-Bold="True" />
                    </span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload ID="upPostAuditedMSPCostReport" runat="server" CssClass="fileControl" ViewStateMode="Enabled" Width="400px" />
                    </span>
                    <p>
                        <b>
                            <asp:Label ID="lblPostAuditedMSPCostReport" runat="server" />
                        </b>
                    </p>
                </div>
                <%--<div class="col-sm-7">
            <span style="text-align: left;">
                <asp:DropDownList ID="DropDownList4" AutoPostBack="false" runat="server" style="min-width: 200px;height:30px"></asp:DropDownList></span>
            </div>--%>
            </div>
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style= "margin-left:45px;">
                        <asp:Label ID="Label19" runat="server" Text=" * " CssClass="ohio-field" Style="font-size: 20px; margin-left:10px;color:red; text-align: right;width:auto !important;" Font-Bold="True" />
                        <asp:Label ID="Label6" runat="server" CssClass="ohio-field" Style="font-size: 15px;width:auto !important; text-align: right" Text="Agreed Upon Procedures Report" Font-Bold="True" />
                    </span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload ID="upAgreedUponProc" runat="server" CssClass="fileControl" ViewStateMode="Enabled" Width="400px" />
                    </span>
                    <p>
                        <b>
                            <asp:Label ID="lblAgreedUponProc" runat="server" />
                        </b>
                    </p>
                </div>
            </div>
        </div>
        <div class="row" id="Div3" runat="server">
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label7" runat="server" Text="Other Document (doc, pdf, xlsx, zip)" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upOtherDocument" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" /></span>
                    <p>
                        <b>
                            <asp:Label ID="lblOtherDocument" runat="server" /></b>
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label8" runat="server" Text="Other Document Description" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtOtherDocumentDescription" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 250px" /></span>
                </div>
            </div>
        </div>
        <div class="row" id="Div4" runat="server">
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label9" runat="server" Text="Transportation T1 Report" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upTransportationT1Report" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" /></span>
                    <p>
                        <b>
                            <asp:Label ID="lblTransportationT1Report" runat="server" /></b>
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="col-sm-5">
                     <asp:PlaceHolder ID="dynamicCtrl" runat="server"></asp:PlaceHolder>
                </div>

                <div class="col-sm-7">
                    <span style="text-align: right;">
						<div class="input_fields_wrap">
  <a href="#" BackColor="#3366FF" class="add_field_button linkButton"> Add Document</a>
</div>

                        
                </div>

            </div>
        </div>
        <div class="row" id="Div5" runat="server">
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Transportation T2 Report" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <mms:EncryptedFileUpload runat="server" ID="upTransportationT2Report" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" /></span>
                    <p>
                        <b>
                            <asp:Label ID="lblTransportationT2Report" runat="server" /></b>
                    </p>
                </div>

            </div>
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label12" runat="server" Text="Tracking ID" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtTrackingId" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 120px" Enabled="false" /></span>
                </div>
            </div>
        </div>
        <hr />
        <div class="row" id="Div6" runat="server">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                <asp:Button runat="server" Text="Save" ID="btnSaveCostReport" OnClick="btnSaveCostReport_Click" BackColor="#0099FF" />
                <asp:Button runat="server" ID="btnCancelCostReport" Text="Clear" OnClick="btnCancelCostReport_Click" BackColor="#FF3300" />
            </div>
        </div>
        </span>
    </asp:Panel>
</div>
<%--Save cost report section--%>

<%--Cost report grid section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px">
    <ajax:CollapsiblePanelExtender ID="cpGrid" runat="server" Collapsed="false" TargetControlID="pnlgrid"
        ExpandControlID="pnlsepgrid" CollapseControlID="pnlsepgrid" />
    <asp:Panel runat="server" ID="pnlsepgrid" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpGrid');" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="SectHd2" Header="- MSP Cost Reviews and Submission " />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlgrid" class="OwnerBackground">

        <asp:PlaceHolder ID="PlaceHolder1" runat="server" />
    </asp:Panel>


</div>
<%--Cost report grid section--%>

<%--Cost report view details section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px">
    <ajax:CollapsiblePanelExtender ID="cpViewDetails" runat="server" Collapsed="true" TargetControlID="pnlViewDetails"
        ExpandControlID="pnlsepViewDetails" CollapseControlID="pnlsepViewDetails" />
    <asp:Panel runat="server" ID="pnlsepViewDetails" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpViewDetails');" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepViewDetails" Header="+ View Details" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlViewDetails" class="OwnerBackground">
        <div class="row">
            <table runat="server" id="tblView" class="viewdetailpanel" border="10" style="width: 110%">
                <tr>
                    <td colspan="2" style="margin-left: 25px; background-color: lightsteelblue">
                        <asp:Label runat="server" Text="MSP Cost Report Submission Details" Style="margin-left: 10px" Font-Bold="True" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 30%; margin-left: 25px; background-color: lightblue">
                        <asp:Label runat="server" Text="<strong>Uploaded Cost Report Documents</strong> <br> (Click on the document name to view the document)" Style="margin-left: 25px"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td style="width: 50%">
                        <div class="row" style="margin-left: 10px">
                            <asp:Label runat="server" ID="lbltext" Text="Pre Audited MSP Cost Report" Font-Bold="True"></asp:Label>
                            <asp:LinkButton ID="lnkPreAud" runat="server" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lblfile" Text="Post Audited MSP Cost Report" Font-Bold="True"></asp:Label>
                            <asp:LinkButton ID="lnkPostAud" runat="server" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lbltrail" Text="Attestation Of Findings Report" Font-Bold="True"></asp:Label>
                            <asp:LinkButton ID="lnkAttest" runat="server" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lbldepre" Text="Agreed Upon Procedure Report" Font-Bold="True"></asp:Label>
                            <asp:LinkButton ID="lnkAgreed" runat="server" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lblhome" Text="Transportation T1 Report" Font-Bold="True"></asp:Label>
                            <asp:LinkButton ID="lnkT1" runat="server" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="Label10" Text="Transportation T2 Report" Font-Bold="True"></asp:Label>
                            <asp:LinkButton ID="lnkT2" runat="server" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                        </div>

                        <div class="column" style="width: 33%">
                            <asp:Label runat="server" ID="lbldoc2" Text="Other Document" Font-Bold="True"></asp:Label>
                            <asp:LinkButton ID="lnkdoc2" runat="server" OnClick="OnFileDownload"></asp:LinkButton>
                        </div>
                        <div class="column" style="width: 67%">
                            <asp:Label runat="server" Text="Other Document Description" Font-Bold="True"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNote2" Text="Note"></asp:TextBox>
                        </div>
                    </td>
                    <td style="width: 50%">
                        <div style="float: left">
                            <asp:Label runat="server" Text="Preparer Information" Font-Bold="True"></asp:Label>
                            <br />
                            <asp:Label runat="server" Text="Name:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblName" Text="FirstName LastName" Font-Bold="False"></asp:Label><br />
                            <asp:Label runat="server" Text="Email:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblEmail" Text="lakshman.first@ohia.gov" Font-Bold="False"></asp:Label><br />
                            <asp:Label runat="server" Text="Phone:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblPhone" Text="333 444 5555"></asp:Label><br />
                            <asp:Label runat="server" Text="ID:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblId" Text="333444555"></asp:Label><br />
                        </div>
                        <div style="margin-left: 100px; float: left">
                            <asp:Label runat="server" Text="Cost Report Type:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblcost" Text="4.5 - final" ></asp:Label><br />
                            <asp:Label runat="server" Text="Provider Medicaid ID:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblmedicaidID" Text="77777777"></asp:Label><br />
                            <asp:Label runat="server" Text="Tracking ID:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblTrackID" Text="12121212121212121"></asp:Label><br />
                            <asp:Label runat="server" Text="Submission Status:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblstatus" Text="Hold"></asp:Label><br />
                            <asp:Label runat="server" Text="CR Status Date:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblCrDate" Text="12/05/2020"></asp:Label><br />
                            <asp:Label runat="server" Text="CR Status Time:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblCrTime" Text="11:20:59 AM"></asp:Label><br />
                            <asp:Label runat="server" Text="Cost Report Fiscal Year:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblReportFrom" Text="01/01/2020-01/01/2021"></asp:Label><br />

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</div>
<%--Cost report view details section--%>

<%--Cost report submit section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px" id="divSubmitCostReport" runat="server">
    <ajax:CollapsiblePanelExtender ID="cpSubmitterApproval" runat="server" Collapsed="true" TargetControlID="pnlSubmitterApproval"
        ExpandControlID="pnlsepSubmitterApproval" CollapseControlID="pnlsepSubmitterApproval" />
    <asp:Panel runat="server" ID="pnlsepSubmitterApproval" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpSubmitterApproval');" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepSubmitterApproval" Header="+ Submitter's Approval" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSubmitterApproval" class="OwnerBackground">
        <div class="row">
            <span style="text-align: left">
                <asp:CheckBox ID="chkSubmitCr" runat="server" OnCheckedChanged="chkSubmitCr_CheckedChanged" AutoPostBack="true" Text="By checking this box you are submitting the cost report and agreeing to the statement below. An automated email will be sent to the preparer of this cost report and provider admin regarding the submission of this cost report." />
            <br />
            <br />
            </span>
        </div>
        <span style="text-align: left">INTENTIONAL MISREPRESENTATION OR FALSIFICATION OF ANY INFORMATION CONTAINED HEREIN MAY BE PUNISHABLE BY FINE AND/OR IMPRISONMENT UNDER FEDERAL AND/OR SATE LAW.
        <br />
        CERTIFICATION BY OFFICER OF THE PROVIDER<br /> &nbsp;I HEREBY CERTIFY THAT:<br /> 1) I have examined this statement, the accompanying supporting schedules, the allocation of expenses and services, and the attached worksheets for the cost report year () and that to the best of my knowledge and belief they are true and accurate statements prepared from the books and records of the Provider in accordance with applicable instructions.<br /> 2) The expenditures included in this statement are based on the actual cost of recorded expenditures.
        <br />
        3) All cost included herein comply with certified public expenditure (CPE) requirements and all local, state and federal requirements (including that the funds were not federal funds in origin, or are federal funds authorized by federal law to be used to match other federal funds and that the claimed expenditure were not used to meet matching requirements under other federally funded programs).
        <br />
        4) Federal funds are being claimed on this report in accordance with the cost report instructions provided by the ODE effective for the above reporting period.<br /> 5) I am the officer authorized by the referenced provider to submit this form and I have made a good faith effort to assure that all information reported is true and accurate.
        <br />
        6) I understand that this information will be used as a basis for claims for federal funds, and possibly state funds, and the falsification and concealment of a material fact may be prosecuted under federal or state civil or criminal law.

        </span>

        <div class="row" id="Div10" runat="server">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
             <asp:Button runat="server" Text="Confirm" ID="btnconfirmsubmitCr" Enabled="false" OnClick="btnConfirmApproval_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" Height="40px" Width="120px"  />
                <asp:Button runat="server" ID="Button2" Text="Cancel" OnClick="btnCancelApproval_Click"  BackColor="#FF5050" Font-Bold="True" ForeColor="White" Height="40px" Width="120px"  />
            </div>
        </div>
    </asp:Panel>
</div>
<%--Cost report submit section--%>

<div id="divSign" runat="server" style="line-height: 1.5;" visible="false">
    <div class="row col-sm-12" style="border: groove; margin-left: 10px">
        <ajax:CollapsiblePanelExtender ID="cpCredentail" runat="server" Collapsed="true" TargetControlID="pnlNeedsSignature"
            ExpandControlID="pnlsepcredentials" CollapseControlID="pnlsepcredentials" />
        <asp:Panel runat="server" ID="pnlsepcredentials" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpCredentail');" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
            <uc1:SectHd runat="server" ID="sepCredentials" Header="+ Credential Validation " />
        </asp:Panel>

        <asp:Panel ID="pnlNeedsSignature" runat="server" DefaultButton="btnSaveSignature">
            <br />
             
            <table class="credentialPanel">
               
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <div style="border: 1px solid #25a0da; width:280px; ">
                        <ms:CaptchaControl Style="margin-left: auto; margin-right: auto;" ID="Captcha1" runat="server"
                            CaptchaBackgroundNoise="Low" CaptchaLength="5" CaptchaHeight="60" CaptchaWidth="200"
                            CaptchaLineNoise="None" CaptchaMinTimeout="5" CaptchaMaxTimeout="240" FontColor="#529E00"
                            ToolTip="Captcha Control Type Text in Image" />
                            </div>
                    </td>
                 </tr>
                <tr>
                    <td style="text-align: right">
                        <span style="color:red;font-weight:900;font-size:35px;">*</span>&nbsp;<asp:Label ID="Label13" AssociatedControlID="txtCaptcha" runat="server" CssClass="formLabelAuto">Please enter the characters in the image above</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="10" CssClass="formField220" TabIndex="1" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtCaptcha" Text="You must enter the characters in the captch!" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                    </td>

                </tr>
                <tr>
                    <td style="text-align: right">
                        <span style="color:red;font-weight:900;font-size:35px;">*</span>&nbsp;<asp:Label ID="Label15" AssociatedControlID="txtPassword" runat="server" CssClass="formLabelAuto">Enter password</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="formField220" TabIndex="2" Text="" />
                        
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtPassword" Text="Password is required!" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="pg-hint2">The password requested is your user login password.</td>
                </tr>
                <tr>

                    <td style="text-align: right">
                        <asp:Label ID="Label16" runat="server" CssClass="formLabelAuto">Submitter&#39;s Name</asp:Label>
                    </td>

                    <td>
                        <asp:TextBox ID="txtSubName" runat="server" TextMode="SingleLine" CssClass="formField220" CausesValidation="false" TabIndex="2" />
                    </td>
                </tr>
                <tr>

                    <td style="text-align: right">
                        <asp:Label ID="Label17" runat="server" CssClass="formLabelAuto">Submitter&#39;s User ID</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubUserId" runat="server" TextMode="SingleLine" CssClass="formField220" CausesValidation="false" TabIndex="2" />
                        <asp:RequiredFieldValidator ID="valSubUserId" runat="server"
                                        ControlToValidate="txtSubUserId" Text="Submitter's User ID is required!" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSaveSignature" runat="server" CausesValidation="false" Text="Confirm" CssClass="buttonBox" TabIndex="3"
                            OnClick="btnSaveSignature_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" />
                        <asp:Button ID="btncancelSig" runat="server" Text="Cancel" CssClass="buttonBox" TabIndex="3" OnClick="bntCancelSig_Click" BackColor="#FF5050" Font-Bold="True" ForeColor="White" CausesValidation="False"  />
                    </td>
                </tr>
            </table>
                  
        </asp:Panel>
    </div>
</div>

<%--Cost report rejection section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px" id="divRejectCostReport" runat="server">
    <ajax:CollapsiblePanelExtender ID="cpSubmitterRejection" runat="server" Collapsed="true" TargetControlID="pnlSubmitterRejection"
        ExpandControlID="pnlsepSubmitterRejection" CollapseControlID="pnlsepSubmitterRejection" />
    <asp:Panel runat="server" ID="pnlsepSubmitterRejection" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpSubmitterRejection');" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepSubmitterRejection" Header="- Submitter's Rejection" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSubmitterRejection" class="OwnerBackground">
        <div class="row">
            <span style="text-align: left">
                <asp:CheckBox  ID="chkRejectCr" OnCheckedChanged="chkRejectCr_CheckedChanged" AutoPostBack="true" runat="server" Text="By checking this box you are rejecting the selected cost report and supporting documentation. An automated email regarding the rejection of this cost report will be sent to preparer of this cost report and provider admin with your comments." />
            </span>
        </div>
        <div class="row">
            <span style="text-align: left">
                <asp:Label ID="Label33" runat="server" Text="Comment" CssClass="ohio-field" Style="font-size: 20px; text-align: Left" Font-Bold="True" />
                <asp:TextBox ID="txtRejectionCmnts" runat="server" Width="2224px"></asp:TextBox>
            </span>
        </div>
        <div class="row" id="Div11" runat="server">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                <asp:Button runat="server" Text="Confirm" ID="btnConfirmRejection" Enabled="false" OnClick="btnConfirmRejection_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" Height="40px" Width="120px" />
                <asp:Button runat="server" ID="Button4" Text="Cancel" OnClick="btnCancelRejection_Click" BackColor="#FF5050" Font-Bold="True" ForeColor="White" Height="40px" Width="120px"/>
            </div>
        </div>
    </asp:Panel>

</div>
<%--Cost report rejection section--%>


















  
   



 