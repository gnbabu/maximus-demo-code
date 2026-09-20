<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SpecialtiesTaxonomies, App_Web_c4une0e1" enableviewstate="true" %>

<asp:Panel ID="pnlTaxonomies" runat="server" Style="display: inline-block; width: 100%;">
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdTaxonomies"
            AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grd_RowCommand">
            <Columns>
                <asp:BoundField DataField="TAXONOMY_CODE" HeaderText="Primary Taxonomy" />
                <asp:BoundField DataField="TAXONOMY_NAME" HeaderText="Taxonomy Description" />
                <asp:TemplateField HeaderText="Primary">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# (Convert.ToBoolean(Eval("PRIMARY_FLAG")) == true) ? "Yes" : "No" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditTaxonomiesRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
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
        <asp:ImageButton ID="btnAddTaxonomies" runat="server" ImageUrl="~/Images/add.png" CommandName="Taxonomies" OnCommand="lbtnAdd_Click" ToolTip="Add" /><br />
        <asp:Button ID="btnTaxonomiesHistory" CommandName="Taxonomies" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" />
        <div id="Div2" class="help-kfe" style="cursor: pointer; display: inline-block;" runat="server">
            <div class="help-parent-name" style="display: none;">Primary Taxonomy</div>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Top" />
        </div>
    </div>
    <br />
</asp:Panel>

<div id="taxonomyDetail" runat="server" visible="false">
    <div><asp:ValidationSummary ID="vsSpecialtiesTaxonomies" runat="server" DisplayMode="List" ValidationGroup="valSpecialtiesTaxonomies" /></div>
    <asp:Label ID="lblSameAsPrimary" runat="server" Text="The taxonomy/specialty combination is same as primary. Please select a different one" ForeColor="Red" Visible="false" />
    <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="ParentTable" runat="server">
                <colgroup>
                    <col width="33%" />
                    <col width="53%" align="left" />
                    <col width="20%" align="left" />
                </colgroup>
    <%--            <tr>
                    <td>&nbsp;</td>
                    <td align="left"><h3>Provider</h3></td>
                    <td align="left"><h3>PDMS</h3></td>
                </tr>--%>
                <tr>
                    <td><span class="formLabel">Specialty*</span></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlSpecialty" AutoPostBack="true" AppendDataBoundItems="True" runat="server" EnableViewState="true" 
                            OnSelectedIndexChanged="ddlSpecialty_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="reqCategory" ValidationGroup="valSpecialtiesTaxonomies"
                            ControlToValidate="ddlSpecialty" ErrorMessage="*Select a Specialty" Text="*" Display="Dynamic" 
                            SetFocusOnError="true" InitialValue="" />               
                    </td>
                    <td align="left"><asp:Label ID="lblPDMSSpecialty" runat="server" /></td>
                </tr>
                <tr align="left">
                    <td><span class="formLabel">Taxonomy*</span></td>
                    <td align="left">
                        <asp:DropDownList ID="ddlTaxonomy" runat="server"  />
                            <asp:RequiredFieldValidator runat="server" ID="reqType" ValidationGroup="valSpecialtiesTaxonomies"
                                ControlToValidate="ddlTaxonomy" ErrorMessage="*Select a Taxonomy" Text="*" Display="Dynamic" 
                                SetFocusOnError="true" InitialValue="" />               
                    </td>
                    <td align="left"><asp:Label ID="lblPDMSTaxonomy" runat="server" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="left">
                        <asp:UpdateProgress id="updateProgress" runat="server" AssociatedUpdatePanelID="pnlUpdate">
                            <ProgressTemplate>
                                <div style="padding-right:30px">
                                    <img src="../Images/ajax-loader.gif" /> Loading ...
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hdnRegSpecialtyID" runat="server" />
    <asp:HiddenField ID="hdnRegTaxonomyID" runat="server" />
</div>
