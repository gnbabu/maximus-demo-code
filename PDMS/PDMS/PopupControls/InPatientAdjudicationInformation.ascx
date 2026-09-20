<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_InPatientAdjudicationInformation" Codebehind="InPatientAdjudicationInformation.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<script>
    function onlyDotsAndNumbers(txt, event) {        
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            else
                return false;;
        }
        if (txt.value.indexOf(".") > 0) {
            var txtlen = txt.value.length;
            var dotpos = txt.value.indexOf(".");
            if ((txtlen - dotpos) > 2)
                return false;
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        else
            return true;
    }



</script>
<asp:UpdatePanel ID="updatePanelInPatientAdjudication" runat="server">
    <ContentTemplate>
        <ajax:CollapsiblePanelExtender ID="cpeInPatientAdjudication" runat="server" Collapsed="true" TargetControlID="pnlInPatientAdjudication" ExpandControlID="pnlsepInPatientAdjudication"
            CollapseControlID="pnlsepInPatientAdjudication" />
        <asp:Panel runat="server" ID="pnlsepInPatientAdjudication" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerToothQuadrantInfo">
            <span id="sepInPatientAdjudication" runat="server" class="pageHeader pH2">+ INPATIENT ADJUDICATION INFORMATION </span>
        </asp:Panel>
        <asp:Panel ID="pnlInPatientAdjudication" runat="server" Style="min-height: 140px; min-width: 150px; height: auto; width: auto; max-width: 98%;">
            <div><asp:Label ID="lblErrorMsg5" runat="server" ForeColor="Red" Style="margin-left: 20px;"></asp:Label></div>

            <div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="lblCoveredDays" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Covered Days or Visits Count (MIA01):</span>
                            </div>
                            <div class="col-sm-7">
                                
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtCoveredDays" runat="server" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="15" onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>

                                </span>
                            </div>
                        </div>
                    </div>
                      <div class="col-sm-6">
                        <div class="row" id="lblClaimRemarkCode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim Remark Code (MOA05):</span>
                            </div>
                            <div class="col-sm-7">
                                
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBoxClaimRemarkCode05" runat="server" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="5"></asp:TextBox>

                                </span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtBoxClaimRemarkCode05" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="lblClaimDRGAmount" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim DRG Amount (MIA04):</span>
                            </div>
                            <div class="col-sm-7">

                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBoxClaimDRGAmount" runat="server" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="18"  onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>

                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div2" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim Remark Code (MOA20):</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBoxClaimRemarkCode20" runat="server" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="5" ></asp:TextBox>

                                </span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBoxClaimRemarkCode20" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>

                </div>

            </div>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hdnClaimId" runat="server" />
<asp:HiddenField ID="hdnClaimType" runat="server" />
<asp:HiddenField ID="hdnInPatientAdjudification" runat="server" />
