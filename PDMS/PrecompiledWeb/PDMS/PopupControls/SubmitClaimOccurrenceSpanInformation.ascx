<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimOccurrenceSpanInformation, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%--OccurrenceSpanInformation--%>
<ajax:Accordion ID="OccurenceSpanInfo" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneOccurenceSpanInfo" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblOccurenceSpanInfo" class="expandcollapse" runat="server" Text="- OCCURRENCE  SPAN2 INFORMATION"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvOccurenceSpanInfo" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvOccurenceSpanInfo_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvOccurenceSpanInfo_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                            <asp:BoundField DataField="" HeaderText="Item" />
                            <asp:BoundField DataField="" HeaderText="Occurrence Span Code" />
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOccurenceSpansearch" Text="Search" runat="server" ToolTip="Search"
                                        OnClick="lnkOccurenceSpanInfo_Click" OnClientClick="exportPopup();"
                                        Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="" HeaderText="From Date" />
                            <asp:BoundField DataField="" HeaderText="TO Date" />
                            <asp:BoundField DataField="" HeaderText="Occurrence Description" />
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
                <div class="row" style="text-align: center;">
                    <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valProviderInfoHeader" />

                    <div class="col-sm-6">
                        <div class="row" id="lblItem" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Item</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtOccurenceSpanItem" runat="server" Style="background-color: lightgrey; height: 30px; width: 200px" CssClass="formField" ReadOnly="true" />

                                </span>
                            </div>
                        </div>

                    </div>

                    <div class="col-sm-6">
                        <div class="row" id="lblOccurrenceSpanCode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Occurrence3 Span Code</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtOccurrenceSpanCode" runat="server" MaxLength="2" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvOccurrenceSpanCode" SetFocusOnError="true"
                                        ValidationGroup="valOwnerInfo" ControlToValidate="txtOccurrenceSpanCode" ErrorMessage="*Enter Procedure Code" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6">
                        <div class="row" id="lblFromDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">FROM Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtoccFromDate" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <ajax:CalendarExtender ID="ceFromDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtoccFromDate"
                                        PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgDate" EnabledOnClient="true" />

                                    <asp:CompareValidator ID="cvFromDate" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtoccFromDate" ValidationGroup="valFromDate"
                                        ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                                    </asp:CompareValidator>
                                    <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtoccFromDate"
                                        ErrorMessage="To Date is required" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo" />
                                  <%--  <asp:CustomValidator ID="cvFromDatea" runat="server" ControlToValidate="txtoccFromDate" ErrorMessage="Select a valid Adjuction Date."
                                        Display="Dynamic" Text="*" ValidationGroup="VldOccurenceDate" OnServerValidate="ReportOccurenceDate_ServerValidate" />--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblToDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">TO Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtoccToDate" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <ajax:CalendarExtender ID="ceToDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtoccToDate"
                                        PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgDate" EnabledOnClient="true" />

                                    <asp:CompareValidator ID="cvToDate" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtoccToDate" ValidationGroup="valOccurenceDate"
                                        ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                                    </asp:CompareValidator>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtoccToDate"
                                        ErrorMessage="To Date is required" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo" />
                                    <%--<asp:CustomValidator ID="cvToDatea" runat="server" ControlToValidate="txtoccToDate" ErrorMessage="Select a valid Adjuction Date."
                                        Display="Dynamic" Text="*" ValidationGroup="VldOccurenceDate" OnServerValidate="ReportOccurenceDate_ServerValidate" />--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblOccurrenceDesc" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Occurrence Description</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtspanOccurrenceDesc" runat="server" Rows="2" TextMode="MultiLine" MaxLength="1000" Style="background-color: lightgrey; height: 30px; width: 200px" CssClass="formField" ReadOnly="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="OccurrenceSpanInformationAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>




<%--<asp:UpdatePanel ID="upOccurenceSpanInfo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlSpanInformation" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 600px;">
                <asp:ValidationSummary ID="vsOccurenceSpanInfo" DisplayMode="List" runat="server" CssClass="failureOccurenceSpanInfo" ValidationGroup="vgOccurenceSpanInfo" />
                <div class="wdAuto">
                    <div class="row OccurrenceInformation" runat="server">
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblOccurenceSpanItems" runat="server" Text="Items" CssClass="formLabel200 " />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtOccurenceSpanItems" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblSpanOccurenceCode" runat="server" Text="Occurrence Code" CssClass="formLabel200 " />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtSpanOccurenceCode" runat="server" CssClass="formField" MaxLength="7" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvSpanOccurenceCode" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="txtSpanOccurenceCode" ErrorMessage="*Enter Occurrence Code" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblFromDate" runat="server" Text="From Date" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="formField" />
                            <ajax:CalendarExtender ID="ceFromDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtFromDate"
                                PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgFromDate" EnabledOnClient="true" />
                            <asp:CompareValidator ID="cvFd" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFromDate" ValidationGroup="VldGrpFromDate"
                                ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                            </asp:CompareValidator>
                            <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                            <asp:RequiredFieldValidator ID="RFVFromDate" runat="server" ControlToValidate="txtFromDate"
                                ErrorMessage="From date is required" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo" />
                            <asp:CustomValidator ID="cvfd2" runat="server" ControlToValidate="txtFromDate" ErrorMessage="Select a From date is required."
                                Display="Dynamic" Text="*" ValidationGroup="VldFromDate" OnServerValidate="ReportFromDate_ServerValidate" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblToDate" runat="server" Text="TO Date" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="formField" />
                            <ajax:CalendarExtender ID="ceToDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtToDate"
                                PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgToDate" EnabledOnClient="true" />
                            <asp:CompareValidator ID="cvToDate" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtToDate" ValidationGroup="VldGrpToDate"
                                ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                            </asp:CompareValidator>
                            <asp:Image ID="imgToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                ErrorMessage="From date is required" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo" />
                            <asp:CustomValidator ID="cvtodate2" runat="server" ControlToValidate="txtToDate" ErrorMessage="Select a From date is required."
                                Display="Dynamic" Text="*" ValidationGroup="VldToDate" OnServerValidate="ReportToDate_ServerValidate" />
                        </div>
                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblSpanOccurneceDesc" runat="server" Text="Occurrence Description" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-7 text-right">
                            <asp:TextBox ID="txtSpanOccurneceDesc" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />

                            <asp:RequiredFieldValidator ID="rfvSpanOccurneceDesc" runat="server" ControlToValidate="txtSpanOccurneceDesc" ErrorMessage="* Required Occurence Description" Text="*" Display="Dynamic" ValidationGroup="valOccurenceDesc"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>

                <asp:UpdateProgress runat="server" ID="upSubmitClaim" DisplayAfter="0" AssociatedUpdatePanelID="upOccurenceSpanInfo">
                    <ProgressTemplate>
                        <div class="loading">

                            <asp:Image ID="imgOccurrenceSpanSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PriorAuthNotes" /></td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>--%>