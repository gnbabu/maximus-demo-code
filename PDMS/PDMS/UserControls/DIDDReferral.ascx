<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_DIDDReferral" Codebehind="DIDDReferral.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>

        <asp:Panel ID="pnlMain" runat="server" Style="margin-right:10px; width: 800px;" DefaultButton="btnCancel">
                <br />
            <table style="width: auto;">
                <tr>
                    <asp:MultiView ID="mltName" runat="server">
                        <asp:View ID="vwProvider" runat="server">
                            <td class="formLabel">Provider Name</td>
                            <td colspan="3" class="wd400"><asp:Label ID="lblProviderName" runat="server" CssClass="formFieldDisplay wd400" /></td>
                        </asp:View>
                        <asp:View ID="vwGroup" runat="server">
                            <td class="formLabel">Group/Entity Name</td>
                            <td colspan="3" class="wd400"><asp:Label ID="lblGroupEntityName" runat="server" CssClass="formFieldDisplay wd400" /></td>
                        </asp:View>
                    </asp:MultiView>
                </tr>
                <tr>
                    <td class="formLabel">Tax ID</td>
                    <td><asp:Label ID="lblTaxID" runat="server" CssClass="formFieldDisplay" /></td>
                    <td class="formLabel">Zip Code</td>
                    <td><asp:Label ID="lblZip" runat="server" CssClass="formFieldDisplay" /></td>  
                </tr>
                <tr>
                    <td class="formLabel"><asp:Literal ID="ltlAppNbr" runat="server" Text =" <%$ Resources:BrandingResource , WAIVER_SERVICES_APPLICATION_NUMBER %>" /></td>
                    <td><asp:Label ID="lblApplicationNo" runat="server" CssClass="formFieldDisplay" /></td>
                    <td class="formLabel">Effective Date</td>
                    <td><asp:Label ID="lblEffectiveDate" runat="server" CssClass="formFieldDisplay" /></td>  
                </tr>
                <tr>
                    <td class="formLabel">Provider Email</td>
                    <td colspan="3" class="wd400"><asp:Label ID="lblEmail" runat="server" CssClass="formFieldDisplay wd400" /></td>  
                </tr>
    
            </table>

            <div style="padding-left: 15px; padding-right:5px;">
                <asp:GridView runat="server" ID="grdServices" AutoGenerateColumns="False" 
                        DataKeyNames="DIDD_REFERRAL_SERVICE_ID, CONTRACT_FROMDATE"
                        CssClass="gridViewSmallFont" EmptyDataText="No services found." Width="100%" 
                        OnRowCommand="grdServices_RowCommand" OnSelectedIndexChanged="grdServices_SelectedIndexChanged">       
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderText="" >
                            <ItemTemplate>
                               <asp:LinkButton 
                                    ID="lnkSelect" ToolTip="Select to edit the service dates."
                                    runat="server" 
                                    CausesValidation="false" 
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' 
                                    CommandName="EditDates" 
                                    Text="Edit Dates" 
                                    ItemStyle-Width="80"
                                    CssClass="gridLink" />
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:BoundField DataField="WAIVER_TYPE_CODE" HeaderText="Service Type Code" ItemStyle-Width="100" />
                        <asp:BoundField DataField="NAME" HeaderText="Service Name"/>
                        <asp:BoundField DataField="CONTRACT_FROMDATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="CONTRACT_FROMDATE" ItemStyle-Width="80" />
                        <asp:BoundField DataField="CONTRACT_TODATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="CONTRACT_TODATE" ItemStyle-Width="80"  />
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
                    <div class="btnBox">
                        <asp:Label ID="lblAmendExtend" runat="server" Text="<%$Resources:BrandingResource , WAIVER_SERVICES_SEARCH_INVALID_SELECTION %>"  CssClass="error-message" /> <br />
                        <asp:Button id="btnAmendExtendContract"  runat="server" Text=" <%$Resources:BrandingResource , WAIVER_SERVICES_BUTTON_AMEND_CONTRACT %>" 
                            CssClass="buttonBox" CausesValidation="false" OnClick="btnAmendExtendContract_Click" />
                        <asp:Button id="btnCancel"  runat="server" Text="Close" CssClass="buttonBox" CausesValidation="false" />
                    </div> 
            </div>

        <cc1:GroupBox ID="gbEditDates" Caption="Edit Dates" CssClass="detailSection" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="400"  runat="server">        
            <div><asp:ValidationSummary ID="valSummary" runat="server" ShowSummary="true" DisplayMode="List" ValidationGroup="EditServiceDate" /></div>
            <table >
                    <tr>
                        <td class="formLabel wd130">Start Date*</td>
                        <td class="fieldValue wd200">
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="ceStartDate" TargetControlID="txtStartDate" runat="server" />
                                    <asp:RequiredFieldValidator ID="valStartDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="EditServiceDate" Text="*"
                                        ControlToValidate="txtStartDate" ErrorMessage="* Start Date is required." Display="Dynamic"  Enabled="true" />
                                    <asp:CompareValidator id="cvStartDate" runat="server" ValidationGroup="EditServiceDate"   
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate"   Enabled="true"
                                        ErrorMessage="* A valid Start Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="true"> 
                                    </asp:CompareValidator>
                                <asp:CustomValidator ID="valNewStartDateCustom" runat="server" OnServerValidate="Validate_NewStartPriorToOldStart" Text="*"
                                    Display="None" ValidationGroup="EditServiceDate" ErrorMessage="* The new start date must be prior to the current start date." Enabled="true"  />
                        </td>
                    </tr>                
                    <tr>
                        <td class="formLabel wd130">End Date</td>
                        <td class="fieldValue wd200">
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="ceEndDate" TargetControlID="txtEndDate" runat="server" />
                                <asp:CustomValidator ID="cvEndDate" runat="server" OnServerValidate="Validate_EndDateAfterStartDate" Text="*"
                                    Display="None" ValidationGroup="EditServiceDate" ErrorMessage="* The end date must be after the start date." Enabled="true"  />
                        </td>
                    </tr>                
                </table>
                    <div class="btnBox">
                        <asp:Button id="btnSaveService"  runat="server" Text="Save"  CssClass="buttonBox" CausesValidation="true" ValidationGroup="EditServiceDate" OnClick="btnSaveService_Click" />
                        <asp:Button id="btnCancelService"  runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" OnClick="btnCancelService_Click" />
                    </div> 
            </cc1:GroupBox>
                        <br /><br /><br />
        </asp:Panel>
