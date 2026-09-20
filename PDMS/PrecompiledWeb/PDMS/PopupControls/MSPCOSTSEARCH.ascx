<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_MSPCOSTSEARCH, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
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
   <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="valLTCCostReport" ShowSummary="true" />

<div class="row col-sm-12" style="border: groove; margin-left: 150px">
    <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" Collapsed="true" TargetControlID="pnlMspSearch"
        ExpandControlID="pnlsepMspSearch" CollapseControlID="pnlsepMspSearch" />
    <asp:Panel runat="server" ID="pnlsepMspSearch" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'CollapsiblePanelExtender1');" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="SectHd1" Header="MSP Cost Reports Review And Submission" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlMspSearch" class="OwnerBackground">
        <div>
            <div class="row" id="divtitle" runat="server" style="background-color: cornflowerblue; padding-right: 150px">MSP Cost Reports Review And Submission</div>
            <div class="row" id="tr1" runat="server">
                <div class="col-sm-4">
                    <div class="col-sm-5">
                        <span style="text-align: right">
                            <asp:Label ID="Label1" runat="server" Text="Medicaid Provider ID" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="MEDID" runat="server" MaxLength="7" CssClass="ohio-field-input" Style="height: 30px; width: 200px" /></span>
 
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="MEDID" ValidationExpression="^[0-9]{7}$"
                ErrorMessage="* Enter 7 digits for Medicaid Provider ID." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="Medicaid" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="col-sm-5">
                        <span>
                            <asp:Label ID="Label2" runat="server" Text="CR Submission Date" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>

                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtsubmitdate" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" />
                            <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtsubmitdate" runat="server" />
                        </span>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="col-sm-5">
                        <span>
                            <asp:Label ID="Label3" runat="server" Text="Submission ID" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtSubmissionID" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" /></span>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label6" runat="server" Text="Preparer ID" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="ohio-field-input" Style="height: 30px; width: 200px" /></span>
                </div>
            </div>

            <div class="row" id="Div2" runat="server">
                <div class="col-sm-4">
                    <div class="col-sm-5">
                        <span style="text-align: right">
                            <asp:Label ID="Label7" runat="server" Text="Tracking Number" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>

                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtTrackingNumber" runat="server" MaxLength="15" CssClass="ohio-field-input" Style="height: 30px; width: 200px" /></span>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                            ControlToValidate="txtTrackingNumber" runat="server"
                            ErrorMessage="Only Numbers allowed"
                            ValidationExpression="\d+" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="col-sm-5">
                    </div>
                </div>
            </div>

            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span>
                        <asp:Label ID="Label11" runat="server" Text="FISCAL Year" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="DropDownFiscal" AutoPostBack="false" runat="server" Style="min-width: 200px; height: 30px"></asp:DropDownList></span>
                </div>
            </div>


            <div class="row" id="Div4" runat="server">
                <div class="col-sm-4">
                    <div class="col-sm-5">
                        <span style="text-align: right">
                            <asp:Label ID="Label13" runat="server" Text="Cost Report Status" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:DropDownList ID="Dropdowncostrstatus" AutoPostBack="false" runat="server" Style="min-width: 200px; height: 30px"></asp:DropDownList></span>
                    </div>
                </div>
            </div>

            <div class="col-sm-4">
                <div class="col-sm-5">
                    <span style="text-align: right">
                        <asp:Label ID="Label9" runat="server" Text="Max Record" CssClass="ohio-field" Style="font-size: 15px; text-align: right" /></span>
                </div>
                <div class="col-sm-7">
                    <span style="text-align: left;">
                         <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="False"  Style="min-width: 100px; height: 30px">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="25" Value="25" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="100" Value="100" />
                        </asp:DropDownList>
                    </span>
                </div>
            </div>

            <hr />
            <div class="row" id="Div6" runat="server">
                <div class="col-sm-12" style="margin-bottom: 20px; text-align: right">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-sucess" />
                    <asp:Button runat="server" ID="btnCancelCostReport" Text="Clear" CssClass="btn btn-danger" OnClick="btnCancelCostReport_Click" />
                </div>

            </div>
        </div>
    </asp:Panel>
</div>


<div class="row col-sm-12" style="border: groove; margin-left: 150px">
    <ajax:CollapsiblePanelExtender ID="cpGrid" runat="server" Collapsed="false" TargetControlID="pnlgrid"
        ExpandControlID="pnlsepgrid" CollapseControlID="pnlsepgrid" />
    <asp:Panel runat="server" ID="pnlsepgrid" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpGrid');" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="SectHd2" Header="MSP Cost Report Submission Detail  " />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlgrid" class="OwnerBackground">

        <asp:PlaceHolder ID="PlaceHolder2" runat="server" />
    </asp:Panel>
</div>
<div class="row col-sm-12" style="border: groove; margin-left: 150px">
    <ajax:CollapsiblePanelExtender ID="cpViewDetails" runat="server" Collapsed="true" TargetControlID="pnlViewDetails"
        ExpandControlID="pnlsepViewDetails" CollapseControlID="pnlsepViewDetails" />
    <asp:Panel runat="server" ID="pnlsepViewDetails" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this,'cpViewDetails');" ToolTip="Click to Expand/Collapse">
        <uc1:SectHd runat="server" ID="sepViewDetails" Header="+ View Details" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlViewDetails" class="OwnerBackground">
        <div class="row">
            <table runat="server" id="tblView" border="10" style="width: 110%">
                <tr>
                    <td colspan="2" style="margin-left: 25px; background-color: lightsteelblue">
                        <asp:Label runat="server" Text="MSP Cost Report Submission Detail" Style="margin-left: 10px"></asp:Label>
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
                            <asp:Label runat="server" ID="lbltext" Text="Pre Audited MSP Cost Report"></asp:Label>
                            <asp:LinkButton ID="lnkPreAud" runat="server" OnClick="OnFileDownload" Text=""></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lblfile" Text="Post Audited MSP Cost Report"></asp:Label>
                             <asp:LinkButton ID="lnkPostAud" runat="server" OnClick="OnFileDownload" Text=""></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lbltrail" Text="Attestation Of Findings Report"></asp:Label>
                              <asp:LinkButton ID="lnkAttest" runat="server" OnClick="OnFileDownload" Text=""></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lbldepre" Text="Agreed Upon Procedure Report"></asp:Label>
                              <asp:LinkButton ID="lnkAgreed" runat="server" OnClick="OnFileDownload" Text=""></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="lblhome" Text="Transportation T1 Report"></asp:Label>
                              <asp:LinkButton ID="lnkT1" runat="server" OnClick="OnFileDownload" Text=""></asp:LinkButton>
                            <br />
                            <asp:Label runat="server" ID="Label10" Text="Transportation T2 Report"></asp:Label>
                              <asp:LinkButton ID="lnkT2" runat="server" OnClick="OnFileDownload" Text=""></asp:LinkButton>
                            <br />
                        </div>

                        <div class="column" style="width: 33%">
                            <asp:Label runat="server" ID="lbldoc2" Text="Other Document"></asp:Label>
                              <asp:LinkButton ID="lnkdoc2" runat="server" OnClick="OnFileDownload" Text=""></asp:LinkButton>
                        </div>
                        <div class="column" style="width: 67%">
                            <asp:Label runat="server" Text="Other Document Description"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNote2" Text="Note"></asp:TextBox>
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
                            <%--<asp:Label runat="server" Text="Submitter Information"></asp:Label><br />
                            <asp:Label runat="server" Text="Name:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubName" Text="FirstName LastName"></asp:Label><br />
                            <asp:Label runat="server" Text="Email:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubEmail" Text="lakshman.first@ohia.gov"></asp:Label><br />
                            <asp:Label runat="server" Text="Phone:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubPhn" Text="333 444 5555"></asp:Label><br />
                            <asp:Label runat="server" Text="ID:"></asp:Label>
                            <asp:Label runat="server" ID="lblSubId" Text="333444555"></asp:Label><br />--%>

                        </div>
                        <div style="margin-left: 100px; float: left">
                           <%-- <asp:Label runat="server" Text="Provider Type:"></asp:Label>
                            <asp:Label runat="server" ID="lblProvider" Text="LTC"></asp:Label><br />--%>
                            <asp:Label runat="server" Text="Cost Report Type:"></asp:Label>
                            <asp:Label runat="server" ID="lblcost" Text="4.5 - final"></asp:Label><br />
                            <%--<asp:Label runat="server" Text="Reviewer Name:"></asp:Label>
                            <asp:Label runat="server" ID="lblrevname" Text="ABB123XY1N"></asp:Label><br />--%>
                            <asp:Label runat="server" Text="Provider Medicaid ID:"></asp:Label>
                            <asp:Label runat="server" ID="lblmedicaidID" Text="77777777"></asp:Label><br />
                            <asp:Label runat="server" Text="Tracking ID:"></asp:Label>
                            <asp:Label runat="server" ID="lblTrackID" Text="12121212121212121"></asp:Label><br />
                            <%--<asp:Label runat="server" Text="CR Document Number:"></asp:Label>
                            <asp:Label runat="server" ID="lblcrdoc" Text="ABC999XYZ"></asp:Label><br />--%>
                            <asp:Label runat="server" Text="Submission Status:"></asp:Label>
                            <asp:Label runat="server" ID="lblstatus" Text="Hold"></asp:Label><br />
                            <asp:Label runat="server" Text="CR Status Date:"></asp:Label>
                            <asp:Label runat="server" ID="lblCrDate" Text="12/05/2020"></asp:Label><br />
                            <asp:Label runat="server" Text="CR Status Time:"></asp:Label>
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
