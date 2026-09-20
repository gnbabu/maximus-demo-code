<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ClaimDentalServicesDetails, App_Web_wbqq1lcm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Src="~/PopupControls/SubmitClaimSearchPop.ascx" TagPrefix="uc" TagName="SubmitClaimSearchPop" %>


<style>
    table.gridview td, .rgRow td, .rgAltRow td, table.gridViewSmallFont td {
        font-size: 14px;
    }

    select {
        min-width: 90%;
        height: 25px;
    }

    .gridViewFooter {
        background-color: white;
    }

    .gridViewSelected, .gridViewSelected td, .gridViewSelected tr {
        background-color: white;
    }


</style>
<script type="text/javascript">
    $(function () {
        $("[id*=ClaimDentserviceAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["ddlDocumentType"];
            return RequiredFieldsValidations(row, requiredControles);
        });
    });

    function RequiredFieldsValidations(row, requiredControles) {
        var isValid = true;
        $.each(requiredControles, function (index, Id) {
            var label = row.find("[id*=" + Id + "]").next("SPAN");
            label.hide();
            if ($.trim(row.find("[id*=" + Id + "]").val()) === "") {
                label.show();
                isValid = false;
            }
        });

        return isValid;
    }

</script>


    <ajax:ModalPopupExtender ID="mpeSubmitClaimSearchPop" runat="server" PopupControlID="pnlSubmitClaimSearchPop" TargetControlID="Button9" BackgroundCssClass="modalBackground" CancelControlID="btnClosePop"/>
    <asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
        <asp:Panel ID="pnlSubmitClaimSearchPopHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnClosePop" Text="X" style="float: right;background-image: none;border: 0px;border-radius: 0px;" CausesValidation="false" />
            <div class="popTitle">
                <asp:Label ID="lblSubmitClaimSearchPop" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
            <asp:MultiView ID="mltSubmitClaimSearchPop" runat="server">
                <asp:View ID="vwSubmitClaimSearchPop" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:SubmitClaimSearchPop runat="server" id="ucSubmitClaimSearchPop" Visible="true" EnableViewState="true" />
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />



<ajax:Accordion ID="ClaimDentalServicesDetails" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneClaimDentalServicesDetails" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblClaimDentalServices" class="expandcollapse" style="color:white;" runat="server" Text="&nbsp;DENTAL SERVICE DETAILS"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvClaimDentalServicesDetails" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvClaimDentalServicesDetails_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvClaimDentalServicesDetails_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                             <asp:BoundField DataField="DetailLineNumber" HeaderText="Detail Line" />
                            <asp:BoundField DataField="" HeaderText="Procedure Code" />
                             <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                <ItemTemplate>
                                   <asp:LinkButton ID="lnksearch" runat="server" ToolTip="Search" OnClick="lnkProcedureCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="" HeaderText="Place of Service" />
                            <%--<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                <ItemTemplate>
                                   <asp:LinkButton ID="lnksearch" runat="server" ToolTip="Search" OnClick="lnkPlaceofserviceSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField DataField="" HeaderText="*Billed Units" />
                            <asp:BoundField DataField="" HeaderText="Paid Units" />
                            <asp:BoundField DataField="" HeaderText="*Date of Service" />
                            <asp:BoundField DataField="" HeaderText="Paid Date" />
                            <asp:BoundField DataField="" HeaderText="Charges" />
                            <asp:BoundField DataField="" HeaderText="Total Charges" />
                           
                            <asp:BoundField DataField="" HeaderText="Status" />
                            <asp:BoundField DataField="" HeaderText="Total Fees" />
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"
                                        CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                <div class="row" style="text-align: center; padding-left:20px;">
              <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valProviderInfoHeader" />     
                  
                      <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">Procedure Code</span>
                            <div style="text-align: left;">
                                    <asp:TextBox ID="txtProcedureCode" runat="server" CssClass="formField"  Style="height: 30px; width: 100px; min-width:100px;"  />
                                    <asp:RequiredFieldValidator ID="rfvtxtProcedureCode" runat="server" ControlToValidate="txtProcedureCode" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                            </div>

                            <%--<asp:LinkButton ID="lnksearch" runat="server" ToolTip="Search" OnClick="lnkProcedureCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>--%>
                    </div>

                    <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                            <div style="text-align: left;">Search</div>
                    </div>

                    <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">Place Of Service</span>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtPlaceofserv" runat="server" CssClass="formField"  Style="height: 30px; width: 100px; min-width:100px;" />
                                    <asp:RequiredFieldValidator ID="rfvPlaceofserv" runat="server" ControlToValidate="txtPlaceofserv" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                            </div>
                    </div>

                    <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                            <div style="text-align: left;">
                            <asp:LinkButton ID="lnkPlaceofServiceSearchDetail" Text="Search" runat="server" ToolTip="Search" OnClick="lnkSubClaimSepopSearch_Click" CommandArgument='<%# Eval("CDE_POS") %>' Visible="true"></asp:LinkButton>                                
                            </div>
                    </div>

                    <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">Billed Units</span>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtBilledUnits" runat="server" CssClass="formField"  Style="height: 30px; width: 100px; min-width:100px;"   MaxLength="1" />
                                <asp:RequiredFieldValidator runat="server" ID="rfvBilledUnits" SetFocusOnError="true"
                                    ValidationGroup="vgBilledUnits" ControlToValidate="txtBilledUnits" ErrorMessage="*Billed unit not reported for detail N" Text="*" Display="Dynamic" InitialValue="1" />
                            </div>
                    </div>
                
                    <div class="col-lg-1">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">Date of Services</span>
                        <div style="text-align: left;">
                            <asp:TextBox ID="txtdateofservice" runat="server" CssClass="formField" Style="height: 30px; width: 100px; min-width:100px;" />
                            <ajax:CalendarExtender ID="cedateofservice" TargetControlID="txtdateofservice" runat="server" />
                            <asp:CompareValidator ID="cvdateofservice" runat="server" ValidationGroup="valOrgInfo"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtdateofservice"
                                ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="false"></asp:CompareValidator>
                        </div>
                    </div>
                   
                    <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">Charges</span>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtAuthorizedUnits" runat="server" Style="background-color: lightgrey; height: 30px; width: 100px; min-width:100px;" CssClass="formField" ReadOnly="true" />
                            </div>
  
                    </div>
                     <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">Paid Amount</span>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtPaidAmount" runat="server" Style="background-color: lightgrey; height: 30px; width: 100px; min-width:100px;" CssClass="formField" ReadOnly="true" />
                                <%-- <asp:RequiredFieldValidator ID="rfvline" runat="server" ControlToValidate="txtdentalLine" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>--%>
                            </div>
                    </div>
                    <div class="col-lg-1">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">Status</span>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtclaimsStatus" runat="server" Style="background-color: lightgrey; height: 30px; width: 100px; min-width:100px;" CssClass="formField" ReadOnly="true"/>
                                <%-- <asp:RequiredFieldValidator ID="rfvline" runat="server" ControlToValidate="txtdentalLine" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>--%>
                            </div>
                    </div>

                    <div class="col-lg-1 ">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                        <div style="text-align: left;">
                            <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="ClaimDentserviceAdd_Click" CssClass="btn btn-primary" Width="60px" CausesValidation="true" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" Visible="false" />
                        </div>
                    </div>

                     <div class="col-lg-1">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">Total Charges:</span>
                        <div style="text-align: left;">
                            <asp:TextBox ID="txttotalCharges" runat="server" Style="background-color: lightgrey; height: 30px; width: 100px; min-width:100px;" CssClass="formField" ReadOnly="true" />
                        </div>
                    </div>
                     <div class="col-sm-1">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">Total Paid</span>
                        <span style="text-align: left;">
                            <asp:TextBox ID="txttotalamountpaid" runat="server" Style="background-color: lightgrey; height: 30px; width: 100px; min-width:100px;" CssClass="formField" ReadOnly="true" />
                        </span>
                    </div>

                    </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>