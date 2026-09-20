<%@ control language="C#" autoeventwireup="true" inherits="Pages_SubstituteW9Form, App_Web_guw1elnn" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Pages/ContactEntry.ascx" TagName="ContactEntry" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/W9Address.ascx" TagPrefix="uc" TagName="W9Address" %>
<%@ Register Src="~/PopupControls/W9AddressHistroy.ascx" TagPrefix="uc" TagName="W9AddressHistory" %>

<style type="text/css">
    .divHistoryAndAdd {
        width: 100%;
        text-align: right;
    }

    .divGrid {
        width: 100%;
    }

    .gridview {
        float: right;
    }

    .formFieldReadOnly100 {
        width: auto!important;
    }

    .gridViewHeader > th > a {
        color: White!important;
    }

    .rbl_Vertical > tbody > tr > td:nth-child(1) {
        width: 75px;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_pageLoaded(setupHelpPopEventHandlers);
    });

    function setupHelpPopEventHandlers() {
        //$(".help-ssn").mouseover(function () {
        //    // .position() uses position relative to the offset parent, 
        //    var pos = $(this).position();

        //    // .outerWidth() takes into account border and padding.
        //    var width = $(this).outerWidth();

        //    //show the menu directly over the placeholder
        //    $("#helpSSNInfo").css({
        //        position: "absolute",
        //        top: pos.top + "px",
        //        left: (pos.left + width) + "px"
        //    }).show();

        //});

        //$("#helpSSNInfo").mouseleave(function () {
        //    $("#helpSSNInfo").hide();
        //});

        $(".help-ein").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpEINInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });

        $("#helpEINInfo").mouseleave(function () {
            $("#helpEINInfo").hide();
        });

    }
</script>
<div>
    <asp:ValidationSummary ID="vsProviderPaymentInfo" runat="server" DisplayMode="List" ValidationGroup="valProviderPaymentInfo" />

</div>




<br />
<div class="container-fluid">
    <div class="row">
        <div class="col-sm-12">
            <b>Information from the Identification page displayed below.</b>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <i>Corrections to this information must be made in the Organization/Individual Identification and Primary Contact sections of the Identification page.</i><br />
        </div>
    </div>


    <br />
    <br />
    <div class="row">
        <div class="col-sm-3 text-right">
            <span class="formLabelAuto ">Legal Business Name</span>
        </div>
        <div class="col-sm-9" style="text-align: left;">
            <asp:Label ID="RS02" runat="server" CssClass="formFieldDisplay wdAuto" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right">
            <span class="formLabelAuto">SSN</span>
        </div>
        <div class="col-sm-9" style="vertical-align: top; text-align: left;">
            <asp:TextBox ID="txtSSN" runat="server" CssClass="formField300" />
        </div>
        </div>
        <div class="row">

        <div class="col-sm-3  text-right"><span class="formLabelAuto">EIN</span></div>
        <div class="col-sm-9" style="vertical-align: top; text-align: left;">
            <asp:TextBox ID="txtEIN" runat="server" CssClass="formField300" />

            <div id="helpEIN" class="help-ein" style="display:inline;cursor: pointer; text-align: left;">
                <asp:Image ID="imgHelpEIN" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
            </div>
            <div id="helpEINInfo" class="infoBox" style="top: 0; right: 0; width: 440px">
                <div class="infoContent">
                    <asp:Literal ID="ltlhelpEINHelp" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_W9_EIN_HELPTEXT %>"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    <div class="row" id="pnlFiscalYearEndHosp" runat="server">
        <div class="col-sm-3 text-right">

            <span class="formLabelAuto">Fiscal Year End</span>
        </div>
        <div style="vertical-align: top; text-align: left;" class="col-sm-9">
            <asp:TextBox ID="txtFiscalYearEnd" runat="server" CssClass="formField300" />
            <cc1:CalendarExtender ID="calReqEffectiveDate" TargetControlID="txtFiscalYearEnd" runat="server" />
            <asp:RequiredFieldValidator ID="rfvReqEffectiveDate" runat="server" SetFocusOnError="true" ValidationGroup="valProviderInfoHeader" Text="*"
                ControlToValidate="txtFiscalYearEnd" ErrorMessage="Enter Fiscal Year End" Display="Dynamic" />
        </div>
    </div>

    <div class="row">
        <div class="col-sm-8">
    <br />
    <div class="pg-hint2">**Please visit (opens new window)<a href="http://www.irs.gov" title="http://www.irs.gov" target="_blank">http://www.irs.gov</a> to obtain a copy of the W9 with instructions.</div>
        </div>
    </div>

    <!--commented based requirement changes
    <b>Type of Ownership</b><br />
    <table>
    <tr><td>
        <asp:Label ID="lblGovernment" runat ="server" Text="Government"/>
        </td>
        <td>
            <asp:DropDownList ID="ddlGovernment" runat="server" />
        </td>
    </tr>
        <tr><td>
        <asp:Label ID="lblProfit" runat ="server" Text="Profit"/>
        </td>
        <td>
            <asp:DropDownList ID="ddlProfit" runat="server" />
        </td>
    </tr>-->

    <!--<tr><td>
        <asp:Label ID="lblTypeOfOwnership" runat ="server" Text="Type Of Ownership"/>
        </td>
        <td>
            <asp:DropDownList ID="ddlTypeOfOwnership" runat="server" />
        </td>
    </tr></table>
    </div>
    <div class="boxPanelFull">
        <table>
        
        <tr><td>
        <asp:Label ID="lblRegisteredBusiness" runat ="server" Text="Registered to do Business in District of Columbia"/>
        </td>
        <td>
            <asp:RadioButtonList ID="rblRegisteredBusiness" runat="server"  TextAlign="Right" RepeatDirection="Horizontal">
            <asp:ListItem Text="Yes" Value="Y"/>
                    <asp:ListItem Text="No" Value="N"/>
            </asp:RadioButtonList>
        </td>
    </tr>
    </table>
</div>-->
    <br />
    <asp:Panel ID="pnlTaxClassification" runat="server">
        <div class="row">


            <div class="col-sm-8">
                <div class="pageHeader">Tax Classification</div>
            </div>
        </div>
        <br />
        <div class="row">


            <div class="col-sm-8">
                <b>Select the most appropriate category below:</b><br />
                <asp:RadioButtonList ID="radioCategory" runat="server" CssClass="radioButtonList" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-sm-2 text-right"><span class="formLabelAuto">State Registered:</span></div>
            <div class="col-sm-10">
                <asp:DropDownList ID="ddlStateRegistered" runat="server"></asp:DropDownList>
            </div>
        </div>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlPracticeType" runat="server">
        
        <div class="row">
            <div class="col-sm-8">
                <div class="pageHeader">Practice Type</div>
            </div>
            <div class="col-sm-8">
                <asp:RadioButtonList ID="radioPracticeType" runat="server" CssClass="radioButtonList" />
            </div>
        </div>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlW9Address" runat="server">
        <div class="pageHeader">W9 Address</div>
        <br />
        <div class="divGrid">
            <asp:GridView runat="server" Width="98%" ID="grdW9Address" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
                EmptyDataText="No W9 Address found." OnRowCommand="grd_RowCommand" AllowPaging="false" PageSize="1">
                <Columns>
                    <asp:BoundField DataField="W9_ADDRESS1" HeaderText="Address 1" />
                    <asp:BoundField DataField="W9_ADDRESS2" HeaderText="Address 2" />
                    <asp:BoundField DataField="W9_CITY" HeaderText="City" />
                    <asp:BoundField DataField="W9_STATE" HeaderText="State" />
                    <asp:BoundField DataField="W9_ZIP" HeaderText="Zip" />
                    <asp:BoundField DataField="W9_EXT_ZIP" HeaderText="Zip Ext" />
                    <asp:TemplateField ItemStyle-Width="20" ShowHeader="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="W9Address" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
        <div class="divHistoryAndAdd">
            <asp:ImageButton ID="btnW9AddressHistory" runat="server" ImageUrl="~/Images/history_icon.jpg" CommandName="W9AddressHistory" OnCommand="btnHistory_Click" ToolTip="History" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlProfitStatus" runat="server">
        <div class="pageHeader">Profit Status</div>
        <br />
        <div class="row">
            <div class="col-sm-8">
                <b>Select the most appropriate category below:</b>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-8">

                <asp:RadioButtonList ID="rblProfitStatus" runat="server" CssClass="radioButtonList" />
            </div>
        </div>
        <br />
    </asp:Panel>
</div>
<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground" >
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="ownerModalPopup" Style="display: none; vertical-align: middle;" ScrollBars="Vertical" align="center">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <div class="popTitle">
            <asp:Label ID="lblTitle" runat="server" Text="Title" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwW9Address" runat="server">
                <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:W9Address ID="ucW9Address" runat="server" />
                     </div>
              </div>
            </asp:View>
            <asp:View ID="vwW9AddressHistory" runat="server">
                <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:W9AddressHistory ID="ucW9AddressHistory" runat="server" />
                     </div>
              </div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <div class="row text-center" style="padding-right: 10px;">
         <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" />
         <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
                CausesValidation="false" />       
    </div>
    <br />
</asp:Panel>

<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
<uc1:MessageBox ID="MessageBox2" runat="server" />
<%--<asp:HiddenField ID="hdnRegProviderPaymentInfoID" runat="server" />--%>
