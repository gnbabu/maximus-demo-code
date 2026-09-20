<%@ page title="Provider Directory" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_PublicSearch, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
                <div class="WhiteBox" >
    <cc1:GroupBox ID="gbSearch" Caption="Search Criteria" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="98%" runat="server">
        <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
        <div>
            <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="ProviderSearch" ShowSummary="true" />
        </div>
        <div style="text-align: center;">
            <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
                <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="pnlUpdate">
                    <ProgressTemplate>
                        <div style="padding-right: 30px">
                            <img src="../Images/ajax-loader.gif" alt="" />
                            <p>Loading ...</p>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table role="presentation">
                    <tr>
                        <td>
                            <%--<span class="formLabel150">Provider Type</span>--%>
                            <asp:Label ID="lblProviderType" runat="server" AssociatedControlID="lbProviderType" Text="Provider Type" CssClass="formLabel150" />
                            <asp:ListBox ID="lbProviderType" runat="server" CssClass="formDropDown" SelectionMode="Multiple" Height="100px" />
                        </td>
                        <td>
                           <%-- <span class="formLabel150">Specialty</span>--%>
                            <asp:Label ID="labelSpecialty" runat="server" AssociatedControlID="lbSpecialty" Text="Specialty" CssClass="formLabel150" />
                            <asp:ListBox ID="lbSpecialty" runat="server" CssClass="formDropDown" SelectionMode="Multiple" Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<span class="formLabel150">Last Name</span>--%>
                            <asp:Label ID="lblLastName" runat="server" AssociatedControlID="ddlLastNameSearch" Text="Last Name" CssClass="formLabel150" />
                            <asp:DropDownList ID="ddlLastNameSearch" runat="server" CssClass="formDropDown">
                                <asp:ListItem Value="equals" Text="Equal to" />
                                <asp:ListItem Value="begins" Text="Begins with" />
                                <asp:ListItem Value="contains" Text="Contains" />
                                <asp:ListItem Value="ends" Text="Ends with" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblLname" runat="server" AssociatedControlID="txtLastName" Text="- " />
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="formField300" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<span class="formLabel150">First Name</span>--%>
                            <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="ddlFirstNameSearch" Text="First Name" CssClass="formLabel150" />
                            <asp:DropDownList ID="ddlFirstNameSearch" runat="server" CssClass="formDropDown">
                                <asp:ListItem Value="equals" Text="Equal to" />
                                <asp:ListItem Value="begins" Text="Begins with" />
                                <asp:ListItem Value="contains" Text="Contains" />
                                <asp:ListItem Value="ends" Text="Ends with" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblFname" runat="server" AssociatedControlID="txtFirstName" Text="- " />
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="formField300" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <%-- <span class="formLabel150">City</span>--%>

                            <asp:Label ID="lblCity" runat="server" AssociatedControlID="ddlCitySearch" Text="City" CssClass="formLabel150" />
                            <asp:DropDownList ID="ddlCitySearch" runat="server" CssClass="formDropDown">
                                <asp:ListItem Value="equals" Text="Equal to" />
                                <asp:ListItem Value="begins" Text="Begins with" />
                                <asp:ListItem Value="contains" Text="Contains" />
                                <asp:ListItem Value="ends" Text="Ends with" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbCity" runat="server" AssociatedControlID="txtCity" Text="- " />
                            <asp:TextBox ID="txtCity" runat="server" CssClass="formField300" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<span class="formLabel150">State</span>--%>
                            <asp:Label ID="lblState" runat="server" AssociatedControlID="ddlState" Text="State" CssClass="formLabel150" />
                            <asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown" />
                        </td>
                        <td>
                            <%--<span class="formLabel150">Zip Code</span>--%>
                            <asp:Label ID="lblZip" runat="server" AssociatedControlID="txtZip" Text="Zip Code" CssClass="formLabel150" />
                            <asp:TextBox ID="txtZip" runat="server" CssClass="formField" />
                        </td>
                    </tr>
                    <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <tr style="vertical-align:top">
                                <td>
                                    <%--<span class="formLabel150">Quadrant</span>--%>
                                    <asp:Label ID="lblQuadrant" runat="server" AssociatedControlID="lbQuadrant" Text="Quadrant" CssClass="formLabel150" />
                                    <asp:ListBox ID="lbQuadrant" runat="server" CssClass="formDropDown" SelectionMode="Multiple" Height="80px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="NW" Text="NW"></asp:ListItem>
                                        <asp:ListItem Value="NE" Text="NE"></asp:ListItem>
                                        <asp:ListItem Value="SW" Text="SW"></asp:ListItem>
                                        <asp:ListItem Value="SE" Text="SE"></asp:ListItem>
                                    </asp:ListBox>
                                </td>
                                <td></td>
                            </tr>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </table>
                <div class="btnBox">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBox" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
                </div>
            </asp:Panel>
        </div>
    </cc1:GroupBox>
    <br />
    <mms:SortablePagingGridView
        ID="gvProviders"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Providers found."
        OnRowCommand="gvProviders_RowCommand"
        OnRowDataBound="gvProviders_RowDataBound"
        OnPageIndexChanging="gvProviders_PageIndexChanging"
        OnSorting="gvProviders_Sorting"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        GridViewSortColumn="OrganizationName" GridViewSortDirection="Ascending"
        DataKeyNames="REG_ID,SpecialtyTypeName">
        <Columns>
            <asp:TemplateField ShowHeader="True" HeaderText="Provider Name" SortExpression="OrganizationName">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="lnkReview"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CommandName="ReviewRow"
                        Text='<%# DataBinder.Eval(Container.DataItem, "OrganizationName") %>'
                        CssClass="gridLink" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ProviderTypeName" HeaderText="Provider Type" SortExpression="ProviderTypeName" />
            <asp:BoundField DataField="SpecialtyTypeName" HeaderText="Specialty" SortExpression="SpecialtyTypeName" />
            <asp:BoundField DataField="ContactCity" HeaderText="City" SortExpression="ContactCity" />
            <asp:BoundField DataField="ContactState" HeaderText="State" SortExpression="ContactState" />
            <asp:BoundField DataField="ContactZip" HeaderText="Zip" SortExpression="ContactZip" />
        </Columns>
    </mms:SortablePagingGridView>

<cc2:MessageBox ID="MessageBox2" runat="server" />

<ajax:ModalPopupExtender ID="mpeReviewProvider" runat="server" PopupControlID="pnlViewProvider" TargetControlID="btnDummy"
    RepositionMode="RepositionOnWindowScroll" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlViewProvider">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlViewProvider" runat="server" CssClass="modalPopup" Style="display: none; height: auto; width: auto; ">
    <asp:MultiView ID="mltViewProvider" runat="server" ActiveViewIndex="0" EnableViewState="true">
        <asp:View ID="vwProvider" runat="server">
            <asp:Panel ID="pnlAdminMaintenance" runat="server" Style="height: 130px; width: 400px;">
                <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                    <div class="popTitle">Provider Information</div>
                </asp:Panel>
                <div>
                    <asp:ValidationSummary ID="valExpressSummary" runat="server" DisplayMode="List" ValidationGroup="ExpressMaintSelection" ShowSummary="true" />
                </div>
                <br />
                <table role="presentation">
                    <tr>
                        <td class="formLabel wd150">Provider name</td>
                        <td>
                            <asp:Label ID="lblProviderName" runat="server" CssClass="wd150"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd150">Address</td>
                        <td>
                            <asp:Label ID="lblAddress" runat="server" CssClass="wd200"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd150">City, State, Zip</td>
                        <td>
                            <asp:Label ID="lblCityStateZip" runat="server" CssClass="wd150"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd150">Specialty</td>
                        <td>
                            <asp:Label ID="lblSpecialty" runat="server" CssClass="wd150"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd150">Office Hours</td>
                        <td>
                            <table role="presentation">
                                <tr>
                                    <td class="wd50">MON</td>
                                    <td><asp:Label ID="lblOfficeMon" runat="server" CssClass="wd100"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="wd50">TUE</td>
                                    <td><asp:Label ID="lblOfficeTue" runat="server" CssClass="wd100"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="wd50">WED</td>
                                    <td><asp:Label ID="lblOfficeWed" runat="server" CssClass="wd100"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="wd50">THU</td>
                                    <td><asp:Label ID="lblOfficeThu" runat="server" CssClass="wd100"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="wd50">FRI</td>
                                    <td><asp:Label ID="lblOfficeFri" runat="server" CssClass="wd100"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="wd50">SAT</td>
                                    <td><asp:Label ID="lblOfficeSat" runat="server" CssClass="wd100"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="wd50">SUN</td>
                                    <td><asp:Label ID="lblOfficeSun" runat="server" CssClass="wd100"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd150">Office Phone</td>
                        <td>
                            <asp:Label ID="lblOfficePhone" runat="server" CssClass="wd150"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd150"><asp:Label id="lblWardCountyLabel" runat="server" Text="Ward/County" /></td>
                        <td>
                            <asp:Label ID="lblWardCounty" runat="server" CssClass="wd150"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div style="width:100%;text-align:center">
                    <asp:HyperLink ID="hlGoogleMaps" runat="server" Text="Get Directions(Opens new window)" Target="_blank"></asp:HyperLink>
                </div>
                <div class="btnBox" style="padding-top: 20px; padding-right: 10px;">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
                </div>
            </asp:Panel>
        </asp:View>
    </asp:MultiView>
</asp:Panel>
<asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy"/>
                    </div>
</asp:Content>

