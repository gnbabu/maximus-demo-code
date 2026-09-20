<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmissionCR, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

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
<div>
    <asp:ValidationSummary ID="vsOrgInfo" runat="server" DisplayMode="List" ValidationGroup="valOrgInfo" />
    <asp:ValidationSummary ID="vsImmigrationInfo" runat="server" DisplayMode="List" ValidationGroup="valImmigrationInfo" />

    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="valLTCCostReport" ShowSummary="true" />

</div>
<div id="helpKFEEditInfo" class="infoBox" style="top: 0; right: 0;">
    <div class="infoTitle">Key Identifier Field Help</div>
    <div class="infoContent">
        <span class="key-field-title">Key Field</span>
        <asp:Literal ID="ltlKFEHelp" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_EDIT_HELPTEXT %>"></asp:Literal>
    </div>
</div>




<div class="row col-sm-12" style="border: groove; margin-left: 0px">
    <ajax:CollapsiblePanelExtender ID="cpeInstructions" runat="server" Collapsed="false" TargetControlID="pnlInstructions"
        ExpandControlID="pnlSepInstructions" CollapseControlID="pnlSepInstructions" />
    <asp:Panel runat="server" ID="pnlSepInstructions" class="CollapsingSeparator" style="background-color: cornflowerblue;" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepInstructions" Header="- LTC Cost Report Submission History " />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInstructions" class="OwnerBackground">
        <div class="row" id="divtitle" runat="server" style="background-color: lightsteelblue; padding-left: 10px">LTC Cost Report Submission History </div>
        <div class="row" id="tr1" runat="server">
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Provider Type " CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="ddlProvidertype" AutoPostBack="True" runat="server" Style="min-width: 200px; height: 30px" OnSelectedIndexChanged="ddlProvidertype_SelectedIndexChanged"></asp:DropDownList></span>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label2" runat="server" Text="CR Submission Date" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>

                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtsubmitdate" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                        <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtsubmitdate" runat="server" />
                        <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="Please enter a valid Submission Date!  " ControlToValidate="txtsubmitdate" Type="Date" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                       <asp:CompareValidator ID="cmpValCannotBeFutureSubmission" runat="server" ErrorMessage="Date must be less than or equal to the current date." ControlToValidate="txtsubmitdate" Operator="LessThanEqual" Type="Date" Display="Dynamic"></asp:CompareValidator>
                    </span>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label3" runat="server" Text="Submitter ID" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtSubmissionID" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" /></span>
                </div>
            </div>
        </div>

        <div class="row" id="Div1" runat="server">
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label4" runat="server" Text="Cost Report Type" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="dlCostReportType" AutoPostBack="true" runat="server" Style="min-width: 200px; height: 30px" Font-Size="Small"></asp:DropDownList></span>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label5" runat="server" Text="CR From Date Range" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                        <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="TextBox1" runat="server" />
                    </span>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Please enter a valid CR From Date!" ControlToValidate="TextBox1" Type="Date" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label6" runat="server" Text="Preparer ID" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" /></span>
                </div>
            </div>
        </div>
        <div class="row" id="Div2" runat="server">
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label7" runat="server" Text="Tracking Number" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtTrackingNumber" runat="server" MaxLength="15" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                            ControlToValidate="txtTrackingNumber" runat="server"
                            ErrorMessage="Only Numbers allowed"
                            ValidationExpression="\d+" Display="Dynamic"></asp:RegularExpressionValidator>
                    </span>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label8" runat="server" Text="CR To Date Range" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                        <ajax:CalendarExtender ID="CalendarExtenderReport" TargetControlID="TextBox3" runat="server" />
                    </span>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Please enter a valid CR To Date!  " ControlToValidate="TextBox3" Type="Date" Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="The From Date cannot be later than the To Date!  " ControlToValidate="TextBox3" Type="Date" ControlToCompare="TextBox1" Operator="GreaterThan" Display="Dynamic"></asp:CompareValidator>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                </div>
            </div>
        </div>
        <div class="row" id="Div3" runat="server">
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label10" runat="server" Text="CR Document Number " CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtCRDocumentNumber" MaxLength="11" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" /></span>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label11" runat="server" Text="FISCAL Year" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="DropDownFiscal" AutoPostBack="false" runat="server" Style="min-width: 200px; height: 30px"></asp:DropDownList></span>
                </div>
            </div>

        </div>

        <div class="row" id="Div4" runat="server">
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label13" runat="server" Text="Cost Report Status" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="Dropdowncostrstatus" AutoPostBack="false" runat="server" Style="min-width: 200px; height: 30px"></asp:DropDownList></span>
                </div>
            </div>
        </div>
        <div class="row" id="Div5" runat="server">
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label16" runat="server" Text="Medicaid Provider ID" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="7" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            ControlToValidate="txtMedicaidID" runat="server"
                            ErrorMessage="Only Numbers allowed"
                            ValidationExpression="\d+" Display="Dynamic"></asp:RegularExpressionValidator>
                    </span>
                </div>
            </div>

            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label9" runat="server" Text="Max Record" CssClass="ohio-field" Style="font-size: 12px; text-align: right; font-weight:900;" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="False" Style="min-width: 100px; height: 30px">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="25" Value="25" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="100" Value="100" />
                        </asp:DropDownList></span>
                </div>
            </div>
        </div>
        <%-- <hr/>--%>
        <div class="row" id="Div6" runat="server">
            <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-success" style="background-color:green;color:white;"/>
                <asp:Button runat="server" ID="btnCancelCostReport" Text="Clear" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelCostReport_Click" />
            </div>


        </div>


    </asp:Panel>
</div>
<div class="row col-sm-12" style="border: groove; margin-left: 0px">
    <ajax:CollapsiblePanelExtender ID="cpGrid" runat="server" Collapsed="false" TargetControlID="pnlgrid"
        ExpandControlID="pnlsepgrid" CollapseControlID="pnlsepgrid" />
    <asp:Panel runat="server" ID="pnlsepgrid" class="CollapsingSeparator" style="background-color: cornflowerblue;" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd style="background-color: red;" runat="server" ID="SectHd2" Header="- LTC Cost Report Submission History Search Results  " />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlgrid" class="OwnerBackground">

        <asp:PlaceHolder ID="PlaceHolder1" runat="server" />
    </asp:Panel>

<div class="row col-sm-12" style="border: groove; margin-left: 0px;">
    <ajax:CollapsiblePanelExtender ID="cpViewDetails" runat="server" Collapsed="true" TargetControlID="pnlViewDetails"
        ExpandControlID="pnlsepViewDetails" CollapseControlID="pnlsepViewDetails" />
    <asp:Panel runat="server" ID="pnlsepViewDetails" class="CollapsingSeparator" style="background-color: cornflowerblue;" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepViewDetails" Header="- View Details" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlViewDetails" class="OwnerBackground">
        <div class="row">
            <table runat="server" id="tblView" border="10" style="width: 110%">
                <tr>
                    <td colspan="2" style="margin-left: 25px; background-color: lightsteelblue">
                        <asp:Label runat="server" Text="LTC Cost Report Submission History Search Results" Style="margin-left: 10px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 30%; margin-left: 25px; background-color: lightblue">
                        <asp:Label runat="server" Text="Uploaded Cost Report Documents <br> (Click on the document name to view the document)" Style="margin-left: 25px"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td style="width: 50%">
                        <div class="row" style="margin-left: 10px">
                            <asp:Label runat="server" ID="lbltext" Text="Cost Report text file"></asp:Label>
                            <a id="lnktext" runat="server" onclick="OnFileDownload">(Users)</a>
                            <br />
                            <asp:Label runat="server" ID="lblfile" Text="Cost Report Backup File"></asp:Label>
                            <a id="lnkfile" runat="server" onclick="OnFileDownload">(Users)</a>
                            <br />
                            <asp:Label runat="server" ID="lbltrail" Text="Trail Balance"></asp:Label>
                            <a id="lnktrail" runat="server" onclick="OnFileDownload">(Users)</a>
                            <br />
                            <asp:Label runat="server" ID="lbldepre" Text="Depreciation and Amortization"></asp:Label>
                            <a id="lnkdepre" runat="server" onclick="OnFileDownload">(Users)</a>
                            <br />
                            <asp:Label runat="server" ID="lblhome" Text="Home Office Cost Allocation"></asp:Label>
                            <a id="lnkhome" runat="server" onclick="OnFileDownload">(Users)</a>
                            <br />
                        </div>
                        <div class="column" style="width: 33%">
                            <asp:Label runat="server" ID="lbldoc1" Text="Other Document"></asp:Label>
                            <a id="lnkdoc1" runat="server" onclick="OnFileDownload">(Users)</a>

                        </div>
                        <div class="column" style="width: 67%">
                            <asp:Label runat="server" Text="Other Document Description"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNote1" Text="Note"></asp:TextBox>
                        </div>
                        <div class="column" style="width: 33%">
                            <asp:Label runat="server" ID="lbldoc2" Text="Other Document"></asp:Label>
                            <a id="lnkdoc2" runat="server" onclick="OnFileDownload">(Users)</a>

                        </div>
                        <div class="column" style="width: 67%">
                            <asp:Label runat="server" Text="Other Document Description"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNote2" Text="Note"></asp:TextBox>
                        </div>
                        <div class="row" style="margin-left: 10px">
                            <asp:Label runat="server" ID="lblverifdoc" Text="Cost Report Verification Document"></asp:Label>
                            <a id="lnkverdoc" runat="server" onclick="OnFileDownload">(Users)</a>
                        </div>
                    </td>
                    <td style="width: 50%">
                        <div style="float: left">
                            <asp:Label runat="server" Text="Preparer Information"></asp:Label>
                            <br />
                            <asp:Label runat="server" Text="Name:"></asp:Label>
                            <asp:Label runat="server" ID="lblName" Text="FirstName LastName"></asp:Label><br />
                            <asp:Label runat="server" Text="Email:"></asp:Label>
                            <asp:Label runat="server" ID="lblEmail" Text="lakshman.first@ohia.gov"></asp:Label><br />
                            <asp:Label runat="server" Text="Phone:"></asp:Label>
                            <asp:Label runat="server" ID="lblPhone" Text="333 444 5555"></asp:Label><br />
                            <asp:Label runat="server" Text="ID:"></asp:Label>
                            <asp:Label runat="server" ID="lblId" Text="333444555"></asp:Label><br />
                            <br />
                            <br />
                            <asp:Label runat="server" Text="Submitter Information"></asp:Label><br />
                            <asp:Label runat="server" Text="Name:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubName" Text="FirstName LastName"></asp:Label><br />
                            <asp:Label runat="server" Text="Email:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubEmail" Text="lakshman.first@ohia.gov"></asp:Label><br />
                            <asp:Label runat="server" Text="Phone:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubPhn" Text="333 444 5555"></asp:Label><br />
                            <asp:Label runat="server" Text="ID:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubId" Text="333444555"></asp:Label><br />

                        </div>
                        <div style="margin-left: 100px; float: left">
                            <asp:Label runat="server" Text="Provider Type:"></asp:Label>
                            <asp:Label runat="server" ID="lblProvider" Text="LTC"></asp:Label><br />
                            <asp:Label runat="server" Text="Cost Report Type:"></asp:Label>
                            <asp:Label runat="server" ID="lblcost" Text="4.5 - final"></asp:Label><br />
                            <asp:Label runat="server" Text="Reviewer Name:"></asp:Label>
                            <asp:Label runat="server" ID="lblrevname" Text="ABB123XY1N"></asp:Label><br />
                            <asp:Label runat="server" Text="Provider ID:"></asp:Label>
                            <asp:Label runat="server" ID="lblmedicaidID" Text="77777777"></asp:Label><br />
                            <asp:Label runat="server" Text="Tracking ID:"></asp:Label>
                            <asp:Label runat="server" ID="lblTrackID" Text="12121212121212121"></asp:Label><br />
                            <asp:Label runat="server" Text="CR Document Number:"></asp:Label>
                            <asp:Label runat="server" ID="lblcrdoc" Text="ABC999XYZ"></asp:Label><br />
                            <asp:Label runat="server" Text="Cost Report Submission Status:"></asp:Label>
                            <asp:Label runat="server" ID="lblstatus" Text="Hold"></asp:Label><br />
                            <asp:Label runat="server" Text="CR Submission Date:"></asp:Label>
                            <asp:Label runat="server" ID="lblCrDate" Text="12/05/2020"></asp:Label><br />
                            <asp:Label runat="server" Text="CR Submission Time:"></asp:Label>
                            <asp:Label runat="server" ID="lblCrTime" Text="11:20:59 AM"></asp:Label><br />
                            <asp:Label runat="server" Text="Cost Report From Date:"></asp:Label>
                            <asp:Label runat="server" ID="lblReportFrom" Text="01/01/2020"></asp:Label><br />
                            <asp:Label runat="server" Text="Cost Report To Date:"></asp:Label>
                            <asp:Label runat="server" ID="lblReportTo" Text="01/01/2021"></asp:Label><br />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</div>




