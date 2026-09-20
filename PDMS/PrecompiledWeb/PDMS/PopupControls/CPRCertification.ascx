<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CPRCertification, App_Web_tiu3g34i" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>

<div id="CPRDetail" runat="server">
    <div style="width: 750px;">
        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
        <asp:ValidationSummary ID="vsCPRCertifications" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valCPRCertifications" />
    </div>
    <asp:Panel ID="pnlCPRCertification" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
        <br />
        <div class="divGrid">
            <asp:GridView ID="grdCPRCertification" Width="98%" runat="server" AllowSorting="true" CssClass="gridview" EmptyDataText="No CPR Certification found"
                OnRowCommand="grdCPRCertification_RowCommand" AutoGenerateColumns="false" HorizontalAlign="Left" OnRowDataBound="grdCPRCertification_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="IsCPRCertified" HeaderText="CPR Certified" SortExpression="IsCPRCertified" />
                    <asp:BoundField DataField="ExpirationDate" HeaderText="CPR Expiration Date" SortExpression="ExpirationDate" DataFormatString="{0:d}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditCPRRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteCPRRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"
                                Visible="<%# CanUserViewDelete()  %>" />
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
            <asp:ImageButton ID="btnAddCPRCertification" runat="server" ImageUrl="~/Images/add.png" CommandName="CPRCertification" OnCommand="btnAdd_Click" ToolTip="Add" />
        </div>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlFieldsCPRCertification" runat="server" visible="false" Enabled="false">
        <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div runat="server">
                    <div class="row">
                        <div class="col-sm-3 text-right"><span class="formLabel200">CPR Certified?*</span></div>
                        <div class="col-sm-9">
                            <asp:RadioButtonList ID="rblCPRCertificationID" BorderStyle="None" CellPadding="0" CellSpacing="0" OnSelectedIndexChanged="OnrblCPRChanged"
                                RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList" Style="margin-left: 0;" AutoPostBack="True">
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False" Selected="true">No</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="rblCPRCertificationID" Enabled="true" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="valCPRCertifications" ErrorMessage="* Select CPR Certified?"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 text-right"><span class="formLabel200">CPR Expiration Date*</span></div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtExpirationDate" runat="server" CssClass="formField wd400" Enabled="false" ValidationGroup="valCPRCertifications" CauseValidation="true" />
                            <ajax:CalendarExtender ID="calStart" TargetControlID="txtExpirationDate" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvtxtExpirationDate" runat="server" InitialValue="" ControlToValidate="txtExpirationDate" Enabled="false" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="valCPRCertifications" ErrorMessage="* Expiration is required if CPR Certified Yes."></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvtxtExpirationDate" runat="server" ValidationGroup="valCPRCertifications"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtExpirationDate" Enabled="false"
                                ErrorMessage="Select a valid Date of Expiration" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 text-right"><span class="formLabel200">Training Organization*</span></div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtClassifications" runat="server" MaxLength="35" CssClass="formFieldLarge wd400" TextMode="MultiLine" Enabled="false"
                                ValidationGroup="valCPRCertifications" CauseValidation="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtClassifications" runat="server" ControlToValidate="txtClassifications" Enabled="false" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="valCPRCertifications" ErrorMessage="* Training Organization is required if CPR Certified Yes."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <br /><br />
                <asp:PlaceHolder runat="server" ID="PlaceholderUploadCPRCertification"></asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlFirstAidCertification" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
        <br />
        <div class="divGrid">
            <asp:GridView ID="grdFirstAidCertification" Width="98%" runat="server" AllowSorting="true" CssClass="gridview" EmptyDataText="No First-Aid Certification found"
                OnRowCommand="grdFirstAidCertification_RowCommand" AutoGenerateColumns="false" HorizontalAlign="Left" OnRowDataBound="grdFirstAidCertification_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="IsFirstAidCertified" HeaderText="First Aid Certified" SortExpression="IsFirstAidCertified" />
                    <asp:BoundField DataField="FirstAidExpirationDate" HeaderText="First Aid Expiration Date" SortExpression="FirstAidExpirationDate" DataFormatString="{0:d}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditFirstAidRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteFirstAidRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"
                                Visible="<%# CanUserViewDelete()  %>" />
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
            <asp:ImageButton ID="btnAddFirstAidCertification" runat="server" ImageUrl="~/Images/add.png" CommandName="FirstAidCertification" OnCommand="btnAdd_Click" ToolTip="Add" />
        </div>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlFieldsFirstAidCertification" runat="server" Visible="false" Enabled="false">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div runat="server">
                    <div class="row">
                        <div class="col-sm-3 text-right"><span class="formLabel200">First Aid Certified?*</span></div>
                        <div class="col-sm-9">
                            <asp:RadioButtonList ID="rblFirstAidID" BorderStyle="None" CellPadding="0" CellSpacing="0" OnSelectedIndexChanged="OnrblFirstAidChanged"
                                RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList" Style="margin-left: 0;" AutoPostBack="True">
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False" Selected="true">No</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rblFirstAidID" Enabled="true" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="valCPRCertifications" ErrorMessage="* Select First Aid Certified?"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 text-right"><span class="formLabel200">First Aid Expiration Date*</span></div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFirstAidExpDate" runat="server" CssClass="formField wd400" Enabled="false" ValidationGroup="valCPRCertifications" CauseValidation="true" />
                            <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFirstAidExpDate" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvtxtFirstAidExpDate" runat="server" ControlToValidate="txtFirstAidExpDate" Enabled="false" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="valCPRCertifications" ErrorMessage="* Expiration is required if First Aid certified Yes."></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvtxtFirstAidExpDate" runat="server" ValidationGroup="valCPRCertifications"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFirstAidExpDate" Enabled="false"
                                ErrorMessage="Select a valid Date of Expiration" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3 text-right"><span class="formLabel200">Training Organization*</span></div>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFirstAidTOrg" runat="server" MaxLength="35" CssClass="formFieldLarge wd400" TextMode="MultiLine" Enabled="false"
                                ValidationGroup="valCPRCertifications" CauseValidation="true" Rows="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtFirstAidTOrg" runat="server" ControlToValidate="txtFirstAidTOrg" Enabled="false" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="valCPRCertifications" ErrorMessage="* Training Organization is required if First Aid Certified Yes.">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <br /><br />
                <asp:PlaceHolder runat="server" ID="PlaceholderUploadFirstAidCertification"></asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
