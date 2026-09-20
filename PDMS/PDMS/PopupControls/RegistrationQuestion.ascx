<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_RegistrationQuestion" Codebehind="RegistrationQuestion.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/OwnerTransaction.ascx" TagPrefix="uc" TagName="OwnerTransaction" %>

<%@ Register Src="~/PopupControls/OwnerRelationships.ascx" TagPrefix="uc" TagName="OwnerRelationships" %>

<%@ Register Src="~/PopupControls/OwnerOtherInfo.ascx" TagPrefix="uc" TagName="OwnerOtherInfo" %>
<%@ Register Src="~/PopupControls/OwnerSubcontractor.ascx" TagPrefix="uc" TagName="OwnerSubcontractor" %>
<%@ Register Src="~/PopupControls/OwnerConviction.ascx" TagPrefix="uc" TagName="OwnerConviction" %>
<%@ Register Src="~/PopupControls/OwnerDebarred.ascx" TagPrefix="uc" TagName="OwnerDebarred" %>
<%@ Register Src="~/PopupControls/OwnerExcluded.ascx" TagPrefix="uc" TagName="OwnerExcluded" %>
<%@ Register Src="~/PopupControls/OwnerTerminated.ascx" TagPrefix="uc" TagName="OwnerTerminated" %>
<%@ Register Src="~/PopupControls/OwnerOriginal.ascx" TagPrefix="uc" TagName="OwnerOriginal" %>
<%@ Register Src="~/PopupControls/OwnerSubcontractorOwner.ascx" TagPrefix="uc" TagName="OwnerSubcontractorOwner" %>
<%@ Register Src="~/PopupControls/OwnerSupplier.ascx" TagPrefix="uc" TagName="OwnerSupplier" %>
<%@ Register Src="~/PopupControls/OwnerHistory.ascx" TagPrefix="uc" TagName="OwnerHistory" %>
<%@ Register Src="~/PopupControls/OwnerConvictionOnBehalf.ascx" TagPrefix="uc" TagName="OwnerConvictionOnBehalf" %>
<%@ Register Src="~/PopupControls/OwnerPenalty.ascx" TagPrefix="uc" TagName="OwnerPenalty" %>
<%@ Register Src="~/PopupControls/OwnerResidency.ascx" TagPrefix="uc" TagName="OwnerResidency" %>


<%--<uc: OwnerResidency runat="server" id="OwnerResidency" />--%>



<script type="text/javascript">
    function QuestionAnswerIsYes(rblId) {
        if (document.getElementById(rblId) != null) {
            var oElem = document.getElementById(rblId);
            var radio = oElem.getElementsByTagName("input");
            return radio[0].checked;
        }
        return false;
    }

    function QuestionTogglePanel(rblId, pnlId, changedId) {
        if (QuestionAnswerIsYes(rblId)) document.getElementById(pnlId).style.display = "block";
        else document.getElementById(pnlId).style.display = "none";
        var oElem = document.getElementById(changedId);
        oElem.value = "CHANGED";
    }
</script>

<asp:HiddenField ID="hdnChanged" runat="server" />

<table style="border: 1px solid gray; padding: 3px; width: 100%">
    <!-- YESNO radio button list -->
    <tr>
        <td>
            <asp:Label ID="lblQuestion" runat="server" /></td>
    </tr>
    <tr>
        <td>
            <asp:RadioButtonList ID="rblYesNo" BorderStyle="None" CellPadding="0" CellSpacing="0"
                RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList1">
                <asp:ListItem Value="1">Yes</asp:ListItem>
                <asp:ListItem Value="2">No</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="pnlQuestion" runat="server">
                <br />
                <asp:Label ID="lblInnerMessage" runat="server" />
                <asp:MultiView ID="mltQuestion" runat="server">
                    <asp:View ID="vwDefaultQuestion" runat="server"></asp:View>
                    <asp:View ID="vwOpt1" runat="server">
                        Please indicate the two related parties and describe the relationship.
            <asp:GridView runat="server" Width="98%" ID="grdOwnerRelationships" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No owner information found." OnRowCommand="grd_RowCommand" OnRowDataBound="grdOwnerRelationShips_RowDataBound">
                <Columns>
                    
                    <asp:BoundField DataField="REG_OWNER1_ID" HeaderText="Person" />
                    <asp:BoundField DataField="RELATIONSHIP_TYPE_ID" HeaderText="Relationship" />
                    <asp:BoundField DataField="REG_OWNER2_ID" HeaderText="Person 2" />
                  <%--  <asp:BoundField DataField="RELATIONSHIP_TYPE_ID2" HeaderText="Relationship 2" />--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerRelationships" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddRelationships" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerRelationships" />
                            <asp:ImageButton ID="btnHistoryRelationships" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerRelationships" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt2" runat="server">
                        Please provide the following information about the other Provider Entity the person in the Owner Information section above has an interest in.
            <asp:GridView runat="server" Width="98%" ID="grdOwnerOtherInfo" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No owner information found." OnRowCommand="grd_RowCommand"  OnRowDataBound="grdOwnerOtherInfo_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="REG_OWNER_ID" HeaderText="Person" />
                    <asp:BoundField DataField="RELATIONSHIP_TYPE_ID" HeaderText="Relationship" />
                    <asp:BoundField DataField="OWNER_OTHER_ID" HeaderText="Other Provider" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerOtherInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddOtherInfo" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerOtherInfo" />
                            <asp:ImageButton ID="btnHistoryOtherInfo" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerOtherInfo" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt3" runat="server">
                                Please indicate associations                    
                        <%--Please provide the following information about the conviction for the person or entity in the Ownership / Control Information section above.--%>
                        <asp:GridView runat="server" Width="98%" ID="grdDebarred" AutoGenerateColumns="False" HorizontalAlign="Left"
                            CssClass="gridview" EmptyDataText="No conviction found." OnRowCommand="grd_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="REG_OWNER_ID" HeaderText="Person" />
                                <asp:BoundField DataField="Explanation" HeaderText="Explanation" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerConviction" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                        <div class="divHistoryAndAdd1">
                            <%--     <asp:ImageButton ID="btnAddConviction" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerConviction" />
                <asp:ImageButton ID="btnHistoryConviction" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerConviction" />--%>
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt4" runat="server">
                            Please indicate associations
            <asp:GridView runat="server" Width="98%" ID="grdConviction" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No conviction found." OnRowCommand="grd_RowCommand" OnRowDataBound="grdOwnerConviction_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="REG_OWNER_ID" HeaderText="Person" />
                    <asp:BoundField DataField="Explanation" HeaderText="Explanation" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerConviction" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddConviction" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerConviction" />
                            <asp:ImageButton ID="btnHistoryConviction" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerConviction" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt5" runat="server">
                           Please indicate associations
            <asp:GridView runat="server" Width="98%" ID="grdConvictionOnBehalf" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No conviction found." OnRowCommand="grd_RowCommand" OnRowDataBound="grdOwnerConvictionBehalf_RowDataBound">
                <Columns>
                   <asp:BoundField DataField="NAME" HeaderText="Person" />
                    <asp:BoundField DataField="Explanation" HeaderText="Explanation" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerConvictionOnBehalf" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                              <asp:ImageButton ID="btnAddConvictionOnBehalf" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerConvictionOnBehalf" />
                            <asp:ImageButton ID="btnHistoryConvictionOnBehalf" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerConvictionOnBehalf" />
                           <%-- <asp:ImageButton ID="btnAddExcluded" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerConvictionOnBehalf" />
                            <asp:ImageButton ID="btnHistoryExcluded" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerConvictionOnBehalf" />--%>
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt6" runat="server">
                           Please indicate associations
            <asp:GridView runat="server" Width="98%" ID="grdResidency" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No Residency information found." OnRowCommand="grd_RowCommand">
                <Columns>
                    <asp:BoundField DataField="OWNER_NAME" HeaderText="Person or Entity" />
                    <%-- <asp:BoundField DataField="TERMINATION_REASON" HeaderText="Reason for Termination" />
                    <asp:BoundField DataField="TERMINATION_BEGIN_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                        HeaderText="Date of Termination" />--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerResidency" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddResidency" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerResidency" />
                            <asp:ImageButton ID="btnHistoryResidency" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerResidency" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt7" runat="server">
                              Please indicate associations
            <%--Please provide the following information about the person or entity in the Ownership / Control Information Section above that has been assessed CMPs.--%>
                        <asp:GridView runat="server" Width="98%" ID="grdPenalty" AutoGenerateColumns="False" HorizontalAlign="Left"
                            CssClass="gridview" EmptyDataText="No Medicare Sanctions found." OnRowCommand="grd_RowCommand" OnRowDataBound="grdOwnerPenalty_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="REG_OWNER_ID" HeaderText="Person" />
                                <asp:BoundField DataField="Explanation" HeaderText="Explanation" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerPenalty" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddPenalty" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerPenalty" />
                            <asp:ImageButton ID="btnHistoryPenalty" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerPenalty" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt8" runat="server">
                           Please provide the following information about the original Owner(s).
            <asp:GridView runat="server" Width="98%" ID="grdOriginalOwner" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No owners found." OnRowCommand="grd_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NAME" HeaderText="Name of Original Owner" />
                    <asp:BoundField DataField="TAX_ID" HeaderText="SSN or Tax ID of Original Owner" />
                    <asp:BoundField DataField="TRANSFER_PLACE" HeaderText="Place of Transfer" />
                    <asp:BoundField DataField="TRANSFER_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" HeaderText="Date of Transfer" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OriginalOwner" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddOriginalOwner" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OriginalOwner" />
                            <asp:ImageButton ID="btnHistoryOriginalOwner" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OriginalOwner" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt9" runat="server">
                          Please provide the following information about the Subcontractors.
            <asp:GridView runat="server" Width="98%" ID="grdSubcontractor" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No subcontractors found." OnRowCommand="grd_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NAME" HeaderText="Name of Subcontractor" />
<%--                    <asp:BoundField DataField="TAX_ID" HeaderText="Tax ID" />--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerSubcontractor" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddSubcontractor" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerSubcontractor" />
                            <asp:ImageButton ID="btnHistorySubcontractor" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerSubcontractor" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt10" runat="server">
                        Please provide the following information about the Subcontractors.
            <asp:GridView runat="server" Width="98%" ID="grdSubcontractor5Years" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No subcontractors found." OnRowCommand="grd_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NAME" HeaderText="Name of Subcontractor" />
                    <asp:BoundField DataField="CITY" HeaderText="City" />
                    <asp:BoundField DataField="STATE" HeaderText="State" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerSubcontractor5Years" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddSubcontractor5Years" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerSubcontractor5Years" />
                            <asp:ImageButton ID="btnHistorySubcontractor5Years" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerSubcontractor5Years" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt11" runat="server">
                        Please provide the following information about the Supplier.
            <asp:GridView runat="server" Width="98%" ID="grdSupplier" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No suppliers found." OnRowCommand="grd_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NAME" HeaderText="Name of Supplier" />
                    <asp:BoundField DataField="NPI" HeaderText="NPI" />
                    <asp:BoundField DataField="TAX_ID" HeaderText="Tax ID" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerSupplier" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddSupplier" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerSupplier" />
                            <asp:ImageButton ID="btnHistorySupplier" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerSupplier" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt12" runat="server">
                        Please provide the following information about the conviction for the person or entity in the Ownership / Control Information section above.
            <asp:GridView runat="server" Width="98%" ID="grdExcluded" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No conviction found." OnRowCommand="grd_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NAME" HeaderText="Person or Entity" />
                    <asp:BoundField DataField="Birth_Date" HeaderText="Birth Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="SSN" HeaderText="SSN" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerConvictionOnBehalf" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
                        <div class="divHistoryAndAdd">
                          
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt13" runat="server">

                        <asp:GridView runat="server" Width="98%" ID="grdTransaction" AutoGenerateColumns="False" HorizontalAlign="Left"
                            CssClass="gridview" EmptyDataText="No transactions found." OnRowCommand="grd_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Person_NAME" HeaderText="Person or Entity" />

                                <asp:BoundField DataField="Transaction_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" HeaderText="Date of Transaction" />
                                <asp:BoundField DataField="Transaction_Amount" HeaderText="Amount of Transaction" DataFormatString="{0:n2}" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerTransaction" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="btnAddTransaction" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerTransaction" />
                            <asp:ImageButton ID="btnHistoryTransaction" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerTransaction" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt14" runat="server">

                        <asp:GridView runat="server" Width="98%" ID="GridView1" AutoGenerateColumns="False" HorizontalAlign="Left"
                            CssClass="gridview" EmptyDataText="No transactions found." OnRowCommand="grd_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Person_NAME" HeaderText="Person or Entity" />

                                <asp:BoundField DataField="Transaction_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" HeaderText="Date of Transaction" />
                                <asp:BoundField DataField="Transaction_Amount" HeaderText="Amount of Transaction" DataFormatString="{0:n2}" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" alt="EditButton" runat="server" CommandName="OwnerTransaction" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerTransaction" />
                            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerTransaction" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt15" runat="server">

                        <asp:GridView runat="server" Width="98%" ID="GridView2" AutoGenerateColumns="False" HorizontalAlign="Left"
                            CssClass="gridview" EmptyDataText="No transactions found." OnRowCommand="grd_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Person_NAME" HeaderText="Person or Entity" />

                                <asp:BoundField DataField="Transaction_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" HeaderText="Date of Transaction" />
                                <asp:BoundField DataField="Transaction_Amount" HeaderText="Amount of Transaction" DataFormatString="{0:n2}" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton4" alt="EditButton" runat="server" CommandName="OwnerTransaction" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerTransaction" />
                            <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerTransaction" />
                        </div>
                        <br />

                    </asp:View>
                    <asp:View ID="vwOpt16" runat="server">

                        <asp:GridView runat="server" Width="98%" ID="GridView3" AutoGenerateColumns="False" HorizontalAlign="Left"
                            CssClass="gridview" EmptyDataText="No transactions found." OnRowCommand="grd_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Person_NAME" HeaderText="Person or Entity" />

                                <asp:BoundField DataField="Transaction_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" HeaderText="Date of Transaction" />
                                <asp:BoundField DataField="Transaction_Amount" HeaderText="Amount of Transaction" DataFormatString="{0:n2}" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton7" alt="EditButton" runat="server" CommandName="OwnerTransaction" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                        <div class="divHistoryAndAdd">
                            <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerTransaction" />
                            <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OwnerTransaction" />
                        </div>
                        <br />

                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </td>
    </tr>
</table>
<ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="ownerModalBackground">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="ownerModalPopup" align="center" Style="display: none; vertical-align: middle" DefaultButton="btnSave" ScrollBars="Vertical">
    <asp:Panel ID="pnlHeaderMpe" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div align="left">
            &nbsp;&nbsp;
            <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>
    <div width="100%" border="0">
        <%--<tr><td>--%>
        <asp:Panel ID="pnlMain" runat="server">
            <asp:MultiView ID="mltPopup" runat="server">
                <asp:View ID="vwDefaultPopup" runat="server"></asp:View>
                <%--<asp:View ID="vwOwnerXref" runat="server"></asp:View>--%>
                <asp:View ID="vwView1" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerRelationships ID="ucOwnerRelationships" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView2" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerOtherInfo ID="ucOwnerOtherInfo" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView4" runat="server">
                    <div style="text-align: left; padding: 15px; margin-left: 5% !important;" class="container-fluid">
                        <div class="row">
                            <uc:OwnerConviction ID="ucOwnerConviction" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView3" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerDebarred ID="ucOwnerDebarred" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView14" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerExcluded ID="ucOwnerExcluded" runat="server" />
                        </div>
                    </div>
                </asp:View>
 
                <asp:View ID="vwView6" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerResidency ID="ucOwnerResidency" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView7" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerPenalty ID="ucOwnerPenalty" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView8" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerOriginal ID="ucOwnerOriginal" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView9" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerSubcontractor ID="ucOwnerSubcontractor" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView10" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerSubcontractorOwner ID="ucOwnerSubcontractorOwner" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView11" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerSubcontractor ID="ucOwnerSubcontractor5Years" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView12" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerSupplier ID="ucOwnerSupplier" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView13" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerHistory ID="ucOwnerHistory" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView5" runat="server">
                    <div style="text-align: left; padding: 15px; margin-left: 5% !important;" class="container-fluid">
                        <div class="row">
                            <uc:OwnerConvictionOnBehalf ID="ucOwnerConvictionOnBehalf" runat="server" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwView15" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:OwnerTransaction ID="ucOwnerTransaction" runat="server" />
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </asp:Panel>

    </div>
    <div class="row text-center" style="padding-right: 10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click"
            CausesValidation="true" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
            CausesValidation="false" />
    </div>
    <br />
</asp:Panel>

<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />