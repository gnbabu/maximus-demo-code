<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_UploadLTCCostReports" Codebehind="UploadLTCCostReports.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>
<script type="text/javascript">
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
    .viewdetailpanel
    {

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

    .auto-style2 {
        width: 647px;
    }

    .auto-style3 {
        width: 41px;
    }

    .auto-style4 {
        height: 58px;
    }

    .auto-style5 {
        color: #000;
        font-size: 8pt;
        font-weight: normal;
        text-align: left;
        height: 58px;
    }

    .auto-style6 {
        width: 663px;
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
  <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="valLTCCostReport" ShowSummary="true" />
<%--Save cost report section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px" id="divSaveCostReport" runat="server">

    <ajax:CollapsiblePanelExtender ID="cpeInstructions" runat="server" Collapsed="false" TargetControlID="pnlInstructions"
        ExpandControlID="pnlSepInstructions" CollapseControlID="pnlSepInstructions" />
    <asp:Panel runat="server" ID="pnlSepInstructions" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpeInstructions');"  Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepInstructions" Header="- Upload Cost Reports" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInstructions" class="OwnerBackground">
        <div class="row" id="divtitle" runat="server" style="background-color: cornflowerblue; padding-left: 10px">Upload Cost Reports</div>
       
        <div class="row" id="trFacProgType" runat="server" style="background-color: lightblue; padding-top: 10px">
            <div class="col-sm-2">
                <span>
                    <asp:Label ID="Label1" runat="server" Text="Facility Program Type" CssClass="ohio-field" Style="font-size: 15px; text-align: right" Font-Bold="True" /></span>
            </div>
            <div class="col-sm-8">
                <span style="text-align: left;">
                    <asp:DropDownList ID="dlFacProgType" AutoPostBack="false" runat="server" Style="min-width: 450px; height: 30px"></asp:DropDownList></span>
            </div>
        </div>
        <div class="row col-sm-12" style="margin-bottom: 10px">Instructions : If uploading attachments, they will need to uploaded along with Cost Report using this panel at same time.</div>
        <div class="row" id="tr1" runat="server">
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label2" runat="server" Text="Cost Report Type" CssClass="ohio-field" Style="font-size: 15px; text-align: right" Font-Bold="True" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="dlCostReportType" AutoPostBack="false" runat="server" Style="min-width: 200px; height: 30px"></asp:DropDownList></span>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row" id="trStartDate" runat="server">
                    <div class="col-sm-5">
                        <span class="bodyTextBold" style="font-size: 15px; text-align: right"><strong>*Cost Report From Date</strong></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtCstRptFromDate" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                            <ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtCstRptFromDate" runat="server" />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valOrgInfo"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtCstRptFromDate"
                                ErrorMessage="Select a valid Cost Report From Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true"> </asp:CompareValidator>
                        </span>
                        <p>
                            <b>
                                <asp:Label ID="lblCRFromdate" runat="server" /></b>
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row" id="trEndDate" runat="server">
                    <div class="col-sm-5">
                        <span class="bodyTextBold" style="font-size: 15px; text-align: right"><strong>*Cost Report To Date</strong></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtCstRptToDate" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                            <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtCstRptToDate" runat="server" />
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="valOrgInfo"
                                Type="Date" Operator="GreaterThan" ControlToValidate="txtCstRptToDate" ControlToCompare="txtCstRptFromDate"
                                ErrorMessage="Select a valid Cost Report To Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true"> </asp:CompareValidator>
                        </span>
                        <p>
                            <b>
                                <asp:Label ID="lblCRTodate" runat="server" /></b>
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" id="Div1" runat="server">
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="*Cost Report Text File (Pnnnnn.txt)" CssClass="bodyTextBold" Style="font-size: 15px; text-align: right" Font-Bold="True" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">

                        <mms:EncryptedFileUpload runat="server" ID="upCostReportTextFile" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" />

                    </span>
                    <p>
                        <b>
                            <asp:Label ID="lblCostReportTextFile" runat="server" /></b>
                    </p>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label4" runat="server" Text="*Cost Report Backup File (Pnnnnn.acrbak)" CssClass="bodyTextBold" Style="font-size: 15px; text-align: right" Font-Bold="True" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">

                        <mms:EncryptedFileUpload runat="server" ID="upCostReportBackupFile" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" />

                    </span>
                    <p>
                        <b>
                            <asp:Label ID="lblCostReportBackupFile" runat="server" /></b>
                    </p>
                </div>
            </div>
            <div class="row col-sm-12" id="divtitle2" runat="server" style="background-color: lightgray; padding-left: 50px"><strong>Supporting Documentation</strong> </div>
            <div class="row col-sm-12" style="margin-bottom: 10px"><strong>Instructions : </strong>Only click on browser box for supporting documentation categories you need. If you have multiple file for catgories, this files can be zipped into one compressed file for submission.</div>
            <div class="row" id="Div2" runat="server">
                <div class="col-sm-6">
                    <div class="col-sm-5">
                        <span style="text-align: right">
                            <asp:Label ID="Label5" runat="server" Text="*Trial Balance (doc, pdf, xlsx, zip)" CssClass="bodyTextBold" Style="font-size: 15px; text-align: right" Font-Bold="True" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <mms:EncryptedFileUpload runat="server" ID="upTrailBalance" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" /></span>
                        <p>
                            <b>
                                <asp:Label ID="lblTrailBalance" runat="server" /></b>
                        </p>
                    </div>
                    <%--<div class="col-sm-7">
            <span style="text-align: left;">
                <asp:DropDownList ID="DropDownList4" AutoPostBack="false" runat="server" style="min-width: 200px;height:30px"></asp:DropDownList></span>
            </div>--%>
                </div>
                <div class="col-sm-6">
                    <div class="col-sm-5">
                        <span style="text-align: right">
                            <asp:Label ID="Label6" runat="server" Text="*Depreciation and Amortization (doc, pdf, xlsx, zip)" CssClass="bodyTextBold" Style="font-size: 15px; text-align: right" Font-Bold="True" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <mms:EncryptedFileUpload runat="server" ID="upDepAndAmo" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" /></span>
                        <p>
                            <b>
                                <asp:Label ID="lblDepAndAmo" runat="server" /></b>
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
                            <asp:Label ID="lblQuality" runat="server" Text="Quality Document (doc, pdf, xlsx, zip)" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <mms:EncryptedFileUpload runat="server" ID="upQualityDocument" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" /></span>
                        <p>
                            <b>
                                <asp:Label ID="lblQualityDocument" runat="server" /></b>
                        </p>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="col-sm-5">
                        <span style="text-align: right">
                            <asp:Label ID="lblQualityDesc" runat="server" Text="Quality Document Description" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtQualityDocument" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 250px" /></span>
                    </div>
                </div>
            </div>
            <div class="row" id="Div5" runat="server">
                <div class="col-sm-6">
                    <div class="col-sm-5">
                        <span style="text-align: right">
                            <asp:Label ID="Label11" runat="server" Text="Home Office Cost Allocation (doc, pdf, xlsx, zip)" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <mms:EncryptedFileUpload runat="server" ID="upHomeOfficeCostAllocation" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" /></span>
                        <p>
                            <b>
                                <asp:Label ID="lblHomeOfficeCostAllocation" runat="server" /></b>
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
                            <asp:TextBox ID="txtTrackingId" runat="server" CssClass="ohio-field-input" Enabled="false" Width="151px" /></span>
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

        </div>

    </asp:Panel>
</div>
<%--Save cost report section--%>
<%--Cost report grid section--%>
<div class="row col-sm-12" style="border: groove; margin-left: 10px">
    <ajax:CollapsiblePanelExtender ID="cpGrid" runat="server" Collapsed="false" TargetControlID="pnlgrid"
        ExpandControlID="pnlsepgrid" CollapseControlID="pnlsepgrid" />
    <asp:Panel runat="server" ID="pnlsepgrid" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpGrid');"  Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="SectHd2" Header="- LTC Cost Reviews and Submission " />
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
    <asp:Panel runat="server" ID="pnlsepViewDetails" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpViewDetails');"  Style="background-color: #2297bc"  ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepViewDetails" Header="+ View Details" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlViewDetails" class="OwnerBackground">
        <div class="row">
            <table runat="server" id="tblView" border="10" class="viewdetailpanel" style="width: 110%">
                <tr>
                    <td colspan="2" style="margin-left: 25px; background-color: lightsteelblue">
                        <asp:Label runat="server" Text="LTC Cost Report Submission Details" Style="margin-left: 10px" Font-Bold="True" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 30%; margin-left: 25px; background-color: lightblue">
                        <asp:Label runat="server" Text="&lt;Strong&gt;Uploaded Cost Report Documents&lt;/Strong&gt; &lt;br&gt; (Click on the document name to view the document)" Style="margin-left: 25px" Font-Bold="False"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td style="width: 50%">
                        <div class="row" style="margin-left: 10px">
                            <asp:Label runat="server" ID="lbltext" Text="Cost Report text file" Font-Bold="True"></asp:Label>
                           
                            <asp:LinkButton ID="lnktext" runat="server" Text="(Users)" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lblfile" Text="Cost Report Backup File" Font-Bold="True"></asp:Label>
                             <asp:LinkButton ID="lnkfile" runat="server" Text="(Users)" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lbltrail" Text="Trail Balance" Font-Bold="True"></asp:Label>
                             <asp:LinkButton ID="lnktrail" runat="server" Text="(Users)" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lbldepre" Text="Depreciation and Amortization" Font-Bold="True"></asp:Label>
                             <asp:LinkButton ID="lnkdepre" runat="server" Text="(Users)" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lblhome" Text="Home Office Cost Allocation" Font-Bold="True"></asp:Label>
                             <asp:LinkButton ID="lnkhome" runat="server" Text="(Users)" OnClick="OnFileDownload"></asp:LinkButton>
                            <br />
                        </div>
                        <div class="column" style="width: 33%">
                            <asp:Label runat="server" ID="lbldoc1" Text="Other Document" Font-Bold="True"></asp:Label>
                              <asp:LinkButton ID="lnkdoc1" runat="server" Text="(Users)" OnClick="OnFileDownload"></asp:LinkButton>
                        </div>
                        <div class="column" style="width: 67%">
                            <asp:Label runat="server" Text="Other Document Description" Font-Bold="True"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNote1" Text="Note"></asp:TextBox>
                        </div>
                        <div class="column" style="width: 33%">
                            <asp:Label runat="server" ID="lbldoc2" Text="Other Document" Font-Bold="True"></asp:Label>
                          
                              <asp:LinkButton ID="lnkdoc2" runat="server" Text="(Users)" OnClick="OnFileDownload"></asp:LinkButton>

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
                            <asp:Label runat="server" ID="lblName" Text="FirstName LastName"></asp:Label><br />
                            <asp:Label runat="server" Text="Email:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblEmail" Text="lakshman.first@ohia.gov"></asp:Label><br />
                            <asp:Label runat="server" Text="Phone:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblPhone" Text="333 444 5555"></asp:Label><br />
                            <asp:Label runat="server" Text="ID:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblId" Text="333444555"></asp:Label><br />
                        </div>
                        <div style="margin-left: 100px; float: left">
                            <asp:Label runat="server" Text="Cost Report Type:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblcost" Text="4.5 - final"></asp:Label><br />
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
                            <asp:Label runat="server" Text="Cost Report From Date:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblReportFrom" Text="01/01/2020"></asp:Label><br />
                            <asp:Label runat="server" Text="Cost Report From To:" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="lblReportTo" Text="01/01/2021"></asp:Label><br />
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
    <asp:Panel runat="server" ID="pnlsepSubmitterApproval" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpSubmitterApproval');"  Style="background-color: #2297bc"  ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepSubmitterApproval" Header="+ Submitter's Approval" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSubmitterApproval" class="OwnerBackground">
        <div class="row">
            <span style="text-align: left">
                <asp:CheckBox ID="chkSubmitCr" runat="server" OnCheckedChanged="chkSubmitCr_CheckedChanged" AutoPostBack="true" Text="By checking this box you are submitting the cost report and agreeing to the statement below. An automated email will be sent to the preparer of this cost report and provider admin regarding the submission of this cost report." Font-Bold="True" />
            <br />
            </span>
        </div>
        <span style="text-align: left">
        <br />
        MISREPRESENTATION OR FALSIFICATION OF ANY INFORMATION CONTAINED IN THIS COST REPORT, OR CONCEALMENT OF A MATERIAL FACT, MAY BE PROSECUTED UNDER FEDERAL AND STATE LAWS AND PUNISHED BY FINE AND/OR IMPRISONMENT.<br />
        <br />
        &nbsp;I hereby certify that I am the owner, officer or authorized representative of the provider filing this cost report and have been granted the authority by the provider business organization to bind the provider business organization to this cost report and all applicable laws. I further certify, on the provider’s behalf, that I have read and understood above statement regarding misrepresentation and falsification and that I have examined the accompanying cost report and supporting schedules and attachments prepared for
            <asp:Literal ID="spProvName" Text="Provider Name" runat="server" />
            <asp:Literal ID="spMedicaid" Text="Medicaid Id" runat="server" />
            for the cost report period beginning
            <asp:Literal ID="spCRFrom" Text="Cost Report From Date" runat="server" />
            and ending
            <asp:Literal ID="spCRTo" Text="Cost Report To Date" runat="server" />
            and that to the best of my knowledge and belief, it is a true, accurate and complete statement from the books and records of the provider in accordance with applicable instructions, except as noted.

        </span>

        <div class="row" id="Div10" runat="server">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                <asp:Button runat="server" Text="Confirm" ID="btnconfirmsubmitCr" Enabled="false" OnClick="btnConfirmApproval_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" Height="40px" Width="120px" />
                <asp:Button runat="server" ID="Button2" Text="Cancel" OnClick="btnCancelApproval_Click" BackColor="#FF5050" Font-Bold="True" ForeColor="White" Height="40px" Width="120px" />
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
    <asp:Panel runat="server" ID="pnlsepSubmitterRejection" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpSubmitterRejection');"  Style="background-color: #2297bc"  ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepSubmitterRejection" Header="+ Submitter's Rejection" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSubmitterRejection" class="OwnerBackground">
        <div class="row">
            <span style="text-align: left">
                <asp:CheckBox ID="chkRejectCr" OnCheckedChanged="chkRejectCr_CheckedChanged" AutoPostBack="true" runat="server" Text="By checking this box you are rejecting the selected cost report and supporting documentation. An automated email regarding the rejection of this cost report will be sent to preparer of this cost report and provider admin with your comments." />
            </span>
        </div>
        <div class="row">
            <span style="text-align: left">
                <asp:Label ID="Label33" runat="server" Text="*Comment" CssClass="ohio-field" Style="font-size: 20px; text-align: Left" Font-Bold="True" />
                 <asp:TextBox ID="txtComments" runat="server" Width="2501px" Height="102px" TextMode="MultiLine"></asp:TextBox>
                <asp:Label Text="*Comments are mandatory" runat="server" ID="lblComments" Visible="false"></asp:Label>
            </span>
           
        </div>
        
        <div class="row" id="Div11" runat="server">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                <asp:Button runat="server" Text="Confirm" ID="btnConfirmRejection" Enabled="false" OnClick="btnConfirmRejection_Click" BackColor="#00CC00" Font-Bold="True" ForeColor="White" Height="40px" Width="120px" />
                <asp:Button runat="server" ID="Button4" Text="Cancel" OnClick="btnCancelRejection_Click" BackColor="#FF5050" Font-Bold="True" ForeColor="White" Height="40px" Width="120px" />
            </div>
        </div>
    </asp:Panel>

</div>
<%--Cost report rejection section--%>

















  
   



 