<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimOtherPayerPaidAmoutnAdjustment, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>

<asp:UpdatePanel ID="upOtherPayerPaidAmoutnAdjustment" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlpnlPage" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 600px;" id="divpaid" runat="server">
                <asp:ValidationSummary ID="vsOtherPayerPaidAmoutnAdjustment" DisplayMode="List" runat="server"
                    CssClass="failureNotification" ValidationGroup="vgOtherPayerPaidAmoutnAdjustment" />
                <div class="wdAuto">
                    <div class="row OtherPayerPaidAmoutnAdjustment" runat="server">
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblDetails" runat="server" Text="*Details" CssClass="formLabel200 " />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:DropDownList ID="ddlDetails" CssClass="formField" EnableViewState="true" runat="server"
                                AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDetails_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvDetails" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="ddlDetails" ErrorMessage="*Missing detail number for other payer paid amount detail panel"
                                Text="*" Display="Dynamic" InitialValue="0" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblProcedureCode" runat="server" Text="Procedure Code" CssClass="formLabel200 " />

                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtProcedureCode" runat="server" Style="background-color: lightgrey;"
                                CssClass="formField" ReadOnly="true" MaxLength="11" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvProcedureCode" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="txtProcedureCode" ErrorMessage="*Procedure Code"
                                Text="*" Display="Dynamic" InitialValue="0" />
                        </div>
                        <%-- <asp:LinkButton ID="lnkNDC" runat="server" ToolTip="Search" OnClick="lnkNDCSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;--%>


                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblHealthPlan" runat="server" Text="*Health Plan ID" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:DropDownList ID="ddlHealthPlan" CssClass="formField" EnableViewState="true"
                                runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlHealthPlan_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvHealthPlan" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="ddlHealthPlan" ErrorMessage="*Missing Health Plan ID for detail N in detail claim adjustment section"
                                Text="*" Display="Dynamic" InitialValue="0" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblAmountPaid" Text="*Amount Paid" runat="server" class="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtAmountPaid" runat="server" CssClass="formField" MaxLength="15" />
                            <asp:RequiredFieldValidator runat="server" ID="rfPaidAmount1"
                                ControlToValidate="txtAmountPaid" ErrorMessage="*Enter Paid Amount" Text="*"
                                Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valAmountPaid" />
                            <asp:RegularExpressionValidator ID="revAmountPaid" runat="server" ControlToValidate="txtAmountPaid"
                                ValidationExpression="^-?(0|[1-9]\d*)(\.\d+)?$" ErrorMessage="*Enter Paid Amount"
                                Text="*" Display="Dynamic"
                                ValidationGroup="valAmountPaid" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblPaidDate" runat="server" Text="Paid Date" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtPaidDate" runat="server" CssClass="formField" />
                            <ajax:CalendarExtender ID="cePaidDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtPaidDate"
                                PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgPaidDate"
                                EnabledOnClient="true" />

                            <asp:CompareValidator ID="cvPaidDate" runat="server" Type="Date" Operator="DataTypeCheck"
                                ControlToValidate="txtPaidDate" ValidationGroup="valPaidDate"
                                ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true"> 
                            </asp:CompareValidator>
                            <asp:Image ID="imgPaidDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp"
                                Height="16px" AlternateText="Calendar Icon" />
                            <asp:RequiredFieldValidator ID="rfvPaidDate" runat="server" ControlToValidate="txtPaidDate"
                                ErrorMessage="To Date is required" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo" />
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtPaidDate"
                                ErrorMessage="Select a valid Adjuction Date."
                                Display="Dynamic" Text="*" ValidationGroup="VldPaidDate" OnServerValidate="ReportPaidDate_ServerValidate" />
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblAmountDue" Text="Amount Due" runat="server" class="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtAmountDue" runat="server" Style="background-color: lightgrey;"
                                CssClass="formField" ReadOnly="true" MaxLength="15" />
                            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                                ControlToValidate="txtAmountDue" ErrorMessage="*Enter Amount Due" Text="*" Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valAmountDue" />--%>
                            <asp:RegularExpressionValidator ID="revAmountDue" runat="server" ControlToValidate="txtAmountDue"
                                ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter AmountDuet"
                                Text="*" Display="Dynamic"
                                ValidationGroup="valAmountDue" />
                        </div>
                        <div id="DivOtherPayerPaidAmoutnAdjustment" style="text-align: center; overflow: scroll;">
                            <wc:GroupBox ID="gbOtherPayerPaidAmoutnAdjustment" Caption="" CaptionStyle-CssClass="bodyTextBold"
                                HorizontalAlign="Center" Width="98%"
                                runat="server">
                                <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                                    <asp:Label ID="lblDetails1" runat="server" Text="*Details" CssClass="formLabel200 " />
                                </div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlDetails1" CssClass="formField" EnableViewState="true" runat="server"
                                        AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDetails1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" SetFocusOnError="true"
                                        ValidationGroup="valOwnerInfo" ControlToValidate="ddlDetails1" ErrorMessage="*Missing detail number for other payer paid amount detail panel"
                                        Text="*" Display="Dynamic" InitialValue="0" />
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                                    <asp:Label ID="lblAdjustmentGroup" runat="server" Text="*Adjustment Group" CssClass="formLabel200 " />
                                </div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlAdjustmentGroup" CssClass="formField" EnableViewState="true"
                                        runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlAdjustmentGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvAdjustmentGroup" SetFocusOnError="true"
                                        ValidationGroup="valOwnerInfo" ControlToValidate="ddlAdjustmentGroup" ErrorMessage="*Adjustment Group selection required for detail N"
                                        Text="*" Display="Dynamic" InitialValue="0" />
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                                    <asp:Label ID="lblReasonCode" runat="server" Text="*Reason Code" CssClass="formLabel200 " />
                                </div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtReasonCode" runat="server" CssClass="formField" MaxLength="11" />
                                    <asp:RequiredFieldValidator ID="tfvReasonCode" runat="server" ControlToValidate="txtReasonCode"
                                        ErrorMessage="*Reason code is required for detail N" Text="*" Display="Dynamic"
                                        ValidationGroup="valtxtReasonCode"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                                    <asp:Label ID="lblAmount" Text="*Amount" runat="server" class="formLabel200" />
                                </div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="formField" MaxLength="15" />
                                    <asp:RequiredFieldValidator runat="server" ID="tfvtxtAmount"
                                        ControlToValidate="txtAmount" ErrorMessage="*Enter  Amount" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" ValidationGroup="valAmount" />
                                    <asp:RegularExpressionValidator ID="revtxtAmount" runat="server" ControlToValidate="txtAmount"
                                        ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter  Amount"
                                        Text="*" Display="Dynamic"
                                        ValidationGroup="valAmount" />
                                </div>

                            </wc:GroupBox>
                        </div>

                    </div>
                </div>
                <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0" AssociatedUpdatePanelID="upNDCDetails">
                    <ProgressTemplate>
                        <div class="loading">

                            <asp:Image ID="imgNDCDetails" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="Button3" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click"
                                CausesValidation="true" ValidationGroup="NDCDetails" /></td>
                        <td>
                            <asp:Button ID="Button4" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click"
                                CausesValidation="false" /></td>
                    </tr>
                </table>
            </div>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
