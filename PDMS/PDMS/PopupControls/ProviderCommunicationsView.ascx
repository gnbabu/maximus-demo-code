<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ProviderCommunicationsView" Codebehind="ProviderCommunicationsView.ascx.cs" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<script type="text/javascript">
    function doClick(btnProvider, btnOwner, btnAddServices) {
        var btn = document.getElementById(btnProvider);
        if (btn != null) btn.disabled = true;
        btn = document.getElementById(btnOwner);
        if (btn != null) btn.disabled = true;
        btn = document.getElementById(btnAddServices);
        if (btn != null) btn.disabled = true;
    }

    /* Clearable Text box */
    jQuery(function ($) {

        function tog(v) { return v ? 'addClass' : 'removeClass'; }

        $(".clearable").each(function () {
            if ($(this).val() != '')
                $(this)[tog(this.value)]('x');
        });

        $(document).on('input', '.clearable', function () {
            $(this)[tog(this.value)]('x');
        }).on('mousemove', '.x', function (e) {
            $(this)[tog(this.offsetWidth - 18 < e.clientX - this.getBoundingClientRect().left)]('onX');
        }).on('click', '.onX', function () {
            $(this).removeClass('x onX').val('');
            __doPostBack($(this).Id, '');
        });

    });

    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        // re-bind your jQuery events here
        function tog(v) { return v ? 'addClass' : 'removeClass'; }

        $(".clearable").each(function () {
            if ($(this).val() != '')
                $(this)[tog(this.value)]('x');
        });
    });



    /* Clearable Text box */

</script>

<div class="WhiteBox" style="width: 100% !important; height: auto !important;">
    <div class="boxPanelHeader">Communications</div>
    <br />
    <div class="boxPanelData">
        <asp:UpdatePanel ID="upEmails" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView runat="server" Width="100%" ID="gvCommunications" AutoGenerateColumns="False"
                    OnRowDataBound="gvCommunications_RowDataBound" OnRowCommand="gvCommunications_RowCommand"
                    AllowSorting="true" OnSorting="gvCommunications_Sorting" OnRowCreated="gvCommunications_RowCreated"
                    DataKeyNames="COMMUNICATION_EVENT_ID, SUBJECT, BODY, EMAIL_TO, USER_ID, REG_ID"
                    HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No communications found."
                    ShowHeader="true" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Printed">
                            <HeaderTemplate>
                                <asp:Label ID="lblPrinted" runat="server" Text="Printed"></asp:Label><br />
                                <br />
                            </HeaderTemplate>
                            <ItemStyle CssClass="gridPrintCommunicationCell" />
                            <ItemTemplate>                                
                                <asp:Label ID="lblChkPrinted" runat="server" Text="&#10004;" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subject" SortExpression="SUBJECT">
                            <HeaderTemplate>
                                <asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFilterSubject" AutoPostBack="true" OnTextChanged="txtFilter_TextChanged" class="clearable"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnSubject" runat="server" CommandName="Communication" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NPI">
                            <HeaderTemplate>
                                <asp:Label ID="lblNPI" runat="server" Text="NPI"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFilterNPI" AutoPostBack="true" OnTextChanged="txtFilter_TextChanged" class="clearable"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnNPI" runat="server" CommandName="Communication" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Style="text-decoration: underline!important" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" SortExpression="DATE">
                            <HeaderTemplate>
                                <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label><br />
                                <br />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDate" runat="server" CommandName="Communication" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" /><br />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
                <asp:DataList ID="dlPager" CellPadding="5" RepeatDirection="Horizontal" runat="server" OnItemCommand="dlPager_ItemCommand" RepeatColumns="20">
                    <ItemStyle Wrap="true" />
                    <ItemTemplate>
                        <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>'
                            CommandName="PageNo"></asp:LinkButton>
                    </ItemTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<ajax:modalpopupextender id="mpeEmailPreview" runat="server" popupcontrolid="pnlPreview"
    targetcontrolid="ButtonDummy" backgroundcssclass="modalBackground" cancelcontrolid="btnCancel">
    </ajax:modalpopupextender>
<asp:Panel ID="pnlPreview" runat="server" CssClass="modalPopup" Style="margin-right:5% !important;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <table style="width: 100%; cursor: pointer">
            <tr>
                <td>
                    <div class="popTitle">Provider Communication</div>
                </td>
                <td style="text-align: right">
                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/cancel.png" OnClick="btnCancel_Click" /></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:UpdatePanel ID="upPreview" runat="server" UpdateMode="Conditional" style="margin-left: auto; margin-right: auto;">
        <ContentTemplate>
            <div id="divPreview" style="overflow: scroll; height: 500px; padding-right:50px;">
                <table id="tblPreview" style="padding-top: 20px; width: 100%;">
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td style="width: 20%;">
                            <asp:Label ID="lblSubject" runat="server" CssClass="formLabel300" Text="Subject" AssociatedControlID="txtSubject" /></td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="formField" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSendTo" runat="server" CssClass="formLabel300" Text="Send To" AssociatedControlID="lboxSendTo" /></td>
                        <td>
                            <asp:ListBox ID="lboxSendTo" runat="server" CssClass="formField" SelectionMode="Multiple" />
                            <asp:CustomValidator ID="cvSendTo" runat="server" ControlToValidate="lboxSendTo" OnServerValidate="SendToRequired" Display="Static"
                                ValidationGroup="Emails" ErrorMessage="* Select at least one Send To email address." Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblBody" runat="server" CssClass="formLabel300" Text="Body" AssociatedControlID="txtBody" /></td>
                        <td>
                            <asp:TextBox ID="txtBody" Text="txt" runat="server" CssClass="formField" ReadOnly="true" TextMode="MultiLine" Columns="100" Rows="20" />
                            <div runat="server" id="divBody"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAttach" runat="server" CssClass="formLabel300" Text="Attachment" /></td>
                        <td>
                            <asp:Panel ID="pnlAttachments" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="btnBox">
                    <asp:Button ID="btnResend" runat="server" Text="Resend Email" CssClass="buttonBox" OnClick="btnResend_Click" />
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" Style="margin-right:20px; margin-bottom:15px !important;"/>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" />