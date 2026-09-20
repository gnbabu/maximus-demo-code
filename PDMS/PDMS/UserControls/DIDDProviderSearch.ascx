<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_DIDDProviderSearch" Codebehind="DIDDProviderSearch.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc3" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="DIDDReferral.ascx" TagName="DIDDReferral" TagPrefix="uc" %>

<cc1:GroupBox ID="GroupBox1" Caption="Search Criteria" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="98%"
    runat="server">
    <div style="text-align: center;">
        <div style="display: inline-block">
            <asp:Panel ID="pnlEntry" runat="server" DefaultButton="btnSearch">
                <div><asp:ValidationSummary ID="valSummary" runat="server" ShowSummary="true" DisplayMode="List" ValidationGroup="ReferralSearch" /></div>
                <table class="wdAuto">
                    <tr>
                        <td class="formLabel">Name</td>
                        <td><asp:TextBox ID="txtName" runat="server" CssClass="formField" MaxLength="30" /></td>
                        <td class="formLabel150">Tax ID</td>
                        <td><ew:NumericBox ID="nbTaxID" runat="server" CssClass="formField" MaxLength="30" /></td>
                    </tr>
                      <tr>
                        <td class="formLabel">Organization ID</td>
                        <td><asp:TextBox ID="txtOrgID" runat="server" CssClass="formField" MaxLength="8" /> </td>
                        <td class="formLabel150">Referred By</td>
                        <td><asp:TextBox ID="txtReferredBy" runat="server" CssClass="formField" MaxLength="100" /> </td>
                    </tr>
                    <tr>
                        <td class="formLabel">Email Address</td>
                        <td><asp:TextBox ID="txtEmailAddress" runat="server" CssClass="formField" MaxLength="100" /></td>
                        <td class="formLabel150">Referral No</td>
                        <td><asp:TextBox ID="txtApplicationNumber" runat="server" CssClass="formField" MaxLength="30" /> </td>
                    </tr>
                    <tr>
                        <asp:CustomValidator ID="valOneSearchValueReqd" runat="server" OnServerValidate="Validate_OneSearchValueEntered" 
                            Display="None" ValidationGroup="ReferralSearch" ErrorMessage="* A value in at least one of the search fields is required." Enabled="true"  />
                    </tr>
                </table>
                 <div class="btnBox">
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBox"  OnClick="btnSearch_Click" ValidationGroup="ReferralSearch"  />
                            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" 
                                CssClass="buttonBox" OnClick="btnClear_Click" />
                   </div>
            </asp:Panel>
        </div>
    </div>
</cc1:GroupBox>
<br />

<asp:Panel ID="pnlResultHeader" runat="server" CssClass="ResultHeader">
    <asp:Label ID="lblResultHeader" runat="server" />
</asp:Panel>
<mms:SortablePagingGridView 
    ID="gvDIDDProviders" 
    runat="server" 
    AutoGenerateColumns="False" 
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true" 
    EmptyDataText="No Providers found." 
    OnRowDataBound="gvDIDDProviders_RowDataBound" OnRowCommand="gvDIDDProviders_RowCommand"
    OnPageIndexChanging="gvDIDDProviders_PageIndexChanging"
    OnSorting="gvDIDDProviders_Sorting"
    RowStyle-VerticalAlign="Top" 
    AllowPaging="True" 
    PageSize="15" 
    GridViewSortColumn="OrganizationName" GridViewSortDirection="Ascending"   
    DataKeyNames="RegID">
    <Columns>
        <asp:TemplateField ShowHeader="False" HeaderText="" SortExpression="">
            <ItemTemplate>
                <asp:LinkButton 
                    ID="lnkReferral" ToolTip="Review the Referral" 
                    runat="server" 
                    CausesValidation="false" 
                    CommandArgument='<%# Bind("DIDD_REFERRAL_ID") %>' 
                    CommandName="ReferralRow" 
                    Text="Referral" 
                    CssClass="gridLink" />
            </ItemTemplate> 
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False" HeaderText="" SortExpression="">
            <ItemTemplate>
                <asp:LinkButton 
                    ID="lnkReview" ToolTip="Review the Registration"
                    runat="server" 
                    CausesValidation="false" 
                    CommandArgument='<%# Bind("RegID") %>' 
                    CommandName="ReviewRow" 
                    Text="Review" 
                    CssClass="gridLink" />
            </ItemTemplate> 
        </asp:TemplateField>
        <asp:BoundField DataField="OrganizationName" HeaderText="Name" SortExpression="OrganizationName" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="TaxId" HeaderText="Tax ID" SortExpression="TaxId" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="ApplicationNo" HeaderText="Referral No" SortExpression="ApplicationNo" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="MedicaidID" HeaderText="OrganizationID" SortExpression="MedicaidID" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="Servicing_Zip" HeaderText="Zip" SortExpression="ServicingZip" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="PDMSStatus" HeaderText="Review Status" SortExpression="PDMSStatus" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="PDMSStatusDate" HeaderText="Status Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="PDMSStatusDate" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="ReferredBy" HeaderText="Referred By" SortExpression="ReferredBy" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" SortExpression="AssignedTo" HeaderStyle-HorizontalAlign="Left" />
        <asp:BoundField DataField="Waivers" HeaderText="Program" SortExpression="Waivers" HeaderStyle-HorizontalAlign="Left" />
    </Columns>   
</mms:SortablePagingGridView>

<!-- ModalPopupExtender -->
<cc3:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="btnDummy"
     BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
</cc3:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" align="center" style="display:none; height:auto; width:auto; min-height:300px; min-width:400px;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" >
        <div class="popTitle"><asp:Literal ID="ltlAppNbr" runat="server" Text =" <%$ Resources:BrandingResource , WAIVER_SERVICES_NAME %>" /> Referral
        </div>
    </asp:Panel>  
        <uc:DIDDReferral ID="ucDIDDReferral" runat="server" />
</asp:Panel>
<asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy" />
