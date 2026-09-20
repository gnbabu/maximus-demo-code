<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_BehavioralHealthInfo, App_Web_yvhxe4ml" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<div>
<asp:Panel ID="pnlBehavioralHealthInfo" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
    <uc1:Separator ID="Separator12" runat="server" Header="Behavioral Health Information" />
    <br />

    <div class="col-sm-12 text-left">
        <asp:Label ID="lblScreenText" CssClass="bodyTextBold" Font-Bold="True" runat="server" Text="Community Behavioral Health Centers that provide mental health services are certified by the Ohio Department of Mental Health and Addiction Services (ODMHAS), if the CBHC provides substance use disorder services the facility must be licensed by ODMHAS." ForeColor="Black" />
        <br />
        <br />
        <br />
    </div>

    <div class="row">

        <div class="col-sm-8 text-left">
            <asp:Label ID="lblBHCertiDate" runat="server" Text="Behavioral Health Certification Date" Font-Bold="True"></asp:Label>
            <%--<span class="formLabel wd170">Behavioral Health Certification Date </span>--%>
        </div>
        <div class="col-sm-4 text-left">
            <asp:TextBox ID="txtBHCDate" runat="server"></asp:TextBox>
            <ajax:CalendarExtender ID="CalendarExtenderTo" TargetControlID="txtBHCDate" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-8 text-left">

            <asp:Label ID="lblCertificateType" runat="server" Text="Certification Type" Font-Bold="True"></asp:Label>
            <%-- <span class="formLabel wd170">CertificationType</span>--%>
        </div>
        <div class="col-sm-4 text-left">
            <asp:DropDownList ID="ddlCertificationType" runat="server" CssClass="drpDownCertType">
            </asp:DropDownList>
        </div>
    </div>

    <asp:Repeater ID="rptBHQuestions" runat="server" OnItemDataBound="rptBHQuestions_ItemDataBound">
        <HeaderTemplate>
            <table class="legend" border="0">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>


                <td colspan="3" class="wd500" style="padding: 0;">
                    <asp:Label runat="server" ID="lblQuestion" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TEXT]") %>' /><asp:Label runat="server" ID="lblQuestionTypeID" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TYPE_ID]") %>' Visible="false" /></td>
                <td>

                    <asp:RadioButtonList ID="rblConfirmQuestion" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblConfirmQuestion_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Selected="False" Text="No" Value="0"></asp:ListItem>
                        <asp:ListItem Selected="False" Text="Yes" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:TextBox ID="txtAptTime" runat="server" Visible="false" MaxLength="20" CssClass="txtBoxBH12Stle"></asp:TextBox>


                     <asp:TextBox ID="txtBH12" runat="server" Visible="false" MaxLength="20" CssClass="txtBoxBH12Stle"></asp:TextBox>
                     <asp:Label runat="server" ID="lbBH12" Text="If yes, please provide bed capacity (# of beds) at the facility." Visible="false" CssClass="lblenew"  ></asp:Label>
                    <asp:TextBox ID="txtBH13" runat="server" Visible="false" MaxLength="20"  CssClass="txtBoxBH12Stle"></asp:TextBox>
                    <asp:Label runat="server" ID="lbBH13" Text="If yes, please provide bed capacity (# of beds) at the facility." Visible="false" CssClass="lblenew2"></asp:Label>
                    </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

</asp:Panel>
<div class="divHistoryAndAdd">
    <span aria-label="History">
        <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
        History</asp:LinkButton>
    </span>
</div>
    <div role="dialog" aria-live="assertive" aria-labelledby="dialog1Title" aria-hidden="true" id="divDialog1">
        <ajax:modalpopupextender id="mpe" runat="server" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseHistory" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2" behaviorid="mpeSplHitory" />
        <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 60%;">
            <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
                <div align="left">
                    &nbsp;&nbsp;
                    <h2 id="dialog1Title">
                        <asp:Label ID="lblSpHistoryTitle" runat="server" CssClass="history" ForeColor="White" Text="Behavioral Health History" /></h2>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
                <div class="container-fluid" style="text-align: left; padding: 15px;">
                    <div class="row">
                        <asp:GridView runat="server" Width="98%" ID="grdHistory" AutoGenerateColumns="False" HorizontalAlign="Left"  CssClass="gridview" 
                            EmptyDataText="No entries found."
                            AllowPaging="true" AllowSorting="true" PageSize="10"
                            OnPageIndexChanging="grdHistory_PageIndexChanging" OnSorting="grdHistory_Sorting">
                            <Columns>
                                <asp:BoundField DataField="Operation"           HeaderText="Operation"                    SortExpression="Operation" />
                                <asp:BoundField DataField="Question"            HeaderText="Question"                    SortExpression="Question" />
                                <asp:BoundField DataField="Answer"           HeaderText="Answer"                    SortExpression="Answer" />
                                <asp:BoundField DataField="Username"           HeaderText="Username"                    SortExpression="Username" />
                                <asp:BoundField DataField="DateOfAction"           HeaderText="DateOfAction"                    SortExpression="DateOfAction"  DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
                            </Columns>
                            <PagerStyle             CssClass="gridpager"        HorizontalAlign="Right" />
                            <HeaderStyle            CssClass="gridViewHeader"   Width="100px" />
                            <AlternatingRowStyle    CssClass="gridViewAltRow" />
                            <RowStyle               CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                    </div>
                    <div class="row">
                        <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" />
                        <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="grdHistory_Export" />
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="”ButtonDummy2”" />
    </div>
</div>
<div style="display: none;">
    <telerik:RadGrid ID="grdExportHistory" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
    AutoGenerateColumns="false" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
    <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
        <Excel Format="Biff" />
    </ExportSettings>
    <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
        DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
        <Columns>
            <telerik:GridBoundColumn DataField="Operation"           HeaderText="Operation"                    SortExpression="Operation" />
            <telerik:GridBoundColumn DataField="Question"            HeaderText="Question"                    SortExpression="Question" />
            <telerik:GridBoundColumn DataField="Answer"           HeaderText="Answer"                    SortExpression="Answer" />
            <telerik:GridBoundColumn DataField="Username"           HeaderText="Username"                    SortExpression="Username" />
            <telerik:GridBoundColumn DataField="DateOfAction"           HeaderText="DateOfAction"                    SortExpression="DateOfAction"  DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
        </Columns>
    </MasterTableView>
</telerik:RadGrid>
</div>
