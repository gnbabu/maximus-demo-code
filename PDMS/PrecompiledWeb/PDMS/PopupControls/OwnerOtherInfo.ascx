<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OwnerOtherInfo, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:UpdateProgress runat="server" ID="upProgress" DisplayAfter="0">
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />Loading...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div>
            <asp:ValidationSummary ID="vsOwnerRelationships" runat="server" DisplayMode="List" ValidationGroup="valOwnerOtherInfo" />
        </div>
        <table id="Table1" runat="server" border="0" cellpadding="0" cellspacing="0" align="center" style="width: 800px !important">
            <tr>
                <td style="text-align: right !important; font-weight: bold">Person 1*</td>
                <td>
                    <asp:DropDownList ID="ddlOwner1" runat="server" CssClass="formDropDownMedium" OnSelectedIndexChanged="ddlOwner1_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerOtherInfo"
                        ControlToValidate="ddlOwner1" ErrorMessage="*Select Owner 1" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right !important; font-weight: bold">Relationship 1*</td>
                <td>
                    <asp:DropDownList ID="ddlRelationship" AutoCompleteMode="SuggestAppend" AutoPostBack="false" runat="server" CssClass="formDropDownMedium" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="valOwnerOtherInfo"
                        ControlToValidate="ddlRelationship" ErrorMessage="*Select Relationship" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right !important; font-weight: bold">Other Provider</td>
                <td>
                    <asp:DropDownList ID="ddlOwner2" runat="server" CssClass="formDropDownMedium" AutoPostBack="true" OnSelectedIndexChanged="ddlOwner2_SelectedIndexChanged" AppendDataBoundItems="True" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="valOwnerOtherInfo"
                        ControlToValidate="ddlOwner2" ErrorMessage="*Select Other Provider" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" />
                </td>
            </tr>
          
        </table>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlOwner1" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>

<asp:HiddenField ID="hdnRegOtherOwnerID" runat="server" />