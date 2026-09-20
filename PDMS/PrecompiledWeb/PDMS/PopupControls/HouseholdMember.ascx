<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HouseholdMember, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

    <div style="padding:10px;">
<%-- <asp:UpdatePanel ID="upHHMain" runat="server"  ChildrenAsTriggers="false"  UpdateMode="Conditional" >
    <ContentTemplate>
            <asp:UpdateProgress runat="server"  ID="upSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upHHMain"    >
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
     <asp:Panel ID="pnlMain" runat="server" GroupingText="Household Member">
        <asp:ValidationSummary ID="vsHouseholdMember" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="HouseholdMember" />
        <table style="width:auto;">
            <tr>
                <td class="formLabel wd150">Name*</td>
                <td class="fieldValue wd400" ><asp:TextBox ID="txtName" runat="server" MaxLength="255" CssClass="formField wd250"></asp:TextBox><span class="field-hint">&nbsp;(First, Middle, Last)</span>
                        <asp:RequiredFieldValidator ID="valNameReqd" runat="server" ControlToValidate="txtName" Enabled="true" SetFocusOnError="true" 
                            Display="Dynamic" Text="*"  ValidationGroup="HouseholdMember" ErrorMessage="* Name is required."></asp:RequiredFieldValidator>
                        
                </td>
            </tr>
            <tr>
                <td class="formLabel wd150">Household Status*</td>
                <td class="fieldValue wd400"><asp:TextBox ID="txtRelationship" runat="server" MaxLength="50" CssClass="formField wd170"></asp:TextBox><span class="field-hint">&nbsp;(Husband, Son, etc.)</span>
                        <asp:RequiredFieldValidator ID="valRelationshipReqd" runat="server" ControlToValidate="txtRelationship" Enabled="true" SetFocusOnError="true" 
                            Display="Dynamic" Text="*"  ValidationGroup="HouseholdMember" ErrorMessage="* Household status is required."></asp:RequiredFieldValidator>
                        
                </td>
            </tr>
            <tr>
                <td class="formLabel wd150">Birth Date*</td>
                <td class="fieldValue">
                    <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField  wd120" /><ajax:CalendarExtender ID="calBirthDate" TargetControlID="txtBirthdate" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="valBirthDateReqd" ControlToValidate="txtBirthDate" ErrorMessage="* Birth Date is required." Text="*" Display="Dynamic" 
                        SetFocusOnError="true" ValidationGroup="HouseholdMember" />
                    <asp:CompareValidator ID="cvEndDate" runat="server" ValidationGroup="HouseholdMember"   
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate"   Enabled="true"
                        ErrorMessage="* A valid Birth Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true" />
                    <asp:CustomValidator ID="valStartCustom" runat="server" OnServerValidate="Validate_FutureBirthDate" ControlToValidate="txtBirthDate" Display="Static" ValidationGroup="HouseholdMember" 
                        ErrorMessage="* Birth Date can not be a future date and cannot result in an age over 100 years." Text="*" />
                </td>
            </tr>
            <tr>
                <td class="formLabel wd150">SSN*</td>
                <td class="fieldValue">
                    <asp:TextBox ID="txtTaxID" runat="server"  MaxLength="9" CssClass="formField wd120"  />
                    <asp:RequiredFieldValidator ID="valTaxIDReqd" runat="server" ControlToValidate="txtTaxID"  
                        Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="HouseholdMember" ErrorMessage="* SSN is required."></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valTaxIdFormat" runat="server" ControlToValidate="txtTaxID"
                        ValidationExpression="^\d{9}$" ErrorMessage="* Enter a 9 digit SSN."   
                        Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="HouseholdMember"  Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td class="formLabel wd150">Sex</td>
                <td class="fieldValue"><asp:DropDownList ID="ddlSex" runat="server">
                        <asp:ListItem Selected="True" Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                        <asp:ListItem Text="Male" Value="M"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="formLabel wd150">Previous Last Names</td>
                <td class="fieldValue wd400"><asp:TextBox ID="txtPreviousNames" runat="server" MaxLength="255" CssClass="formField wd400"></asp:TextBox><br /><span class="field-hint">(List All Previous Married, Maiden or Other Legal Names)</span>
                </td>
            </tr>
        </table>
        <div class="btnBox">
            <asp:Button ID="btnSave"  runat="server" Text="Save" CssClass="buttonBox" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="HouseholdMember" />
            <asp:Button ID="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
        </div> 
    </asp:Panel>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>

<%-- <asp:UpdatePanel ID="upHHChild" runat="server"  ChildrenAsTriggers="false"  UpdateMode="Conditional" >
    <ContentTemplate>
            <asp:UpdateProgress runat="server"  ID="uprogChild" DisplayAfter="0" AssociatedUpdatePanelID="upHHChild"    >
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="imgSaving2" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
    <br /><div class="boxContainer"><span class="boxLabel">Member Address History</span></div><br />
     <div  class="wdAll" >
            <asp:GridView runat="server" Width="100%" ID="gvAddressHist" AutoGenerateColumns="False" HorizontalAlign="Left" ShowHeaderWhenEmpty="true" 
                 DataKeyNames="REG_HOUSEHOLD_MEMBER_ADDRESS_HISTORY_ID"
                CssClass="gridview" EmptyDataText="No previous addresses found." OnRowCommand="gvAddressHist_RowCommand" >
                <Columns>
                    <asp:BoundField DataField="COUNTY" HeaderText="County" ItemStyle-Width="23%" />
                    <asp:BoundField DataField="CITY" HeaderText="City"  ItemStyle-Width="24%"/>
                    <asp:BoundField DataField="STATE" HeaderText="State" ItemStyle-Width="23%" />
                    <asp:BoundField DataField="FROM_DATE" HeaderText="From Date"  DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"  ItemStyle-Width="15%"/>
                    <asp:BoundField DataField="TO_DATE" HeaderText="To Date"  DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"  ItemStyle-Width="15%"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditAddress"   CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  
                                ToolTip="Edit Address" Width="20"><asp:Image ID="imgEdit" ImageUrl="~/Images/edit.png" runat="server" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteAddress"   CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  
                                ToolTip="Delete Address" Width="20"><asp:Image ID="imgDelete" ImageUrl="~/Images/cancel.png" runat="server" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
         <br />
            <div class="grid-hint">List each residence in the last 10 years</div>
            <div style="padding-bottom:10px;text-align:right; width:100%;">
                <asp:LinkButton ID="lnkAddAddrHist" runat="server"  OnClick="lnkAddAddrHist_Click" ToolTip="Add Address History" ><asp:Image ID="imgAddrAdd" ImageUrl="~/Images/add.png" runat="server" /></asp:LinkButton>
            </div>
        </div>
    
     <br /><div class="boxContainer"><span class="boxLabel">Member Criminal History</span></div><br />
     <div class="wdAll" >
            <asp:GridView runat="server" Width="100%" ID="gvCriminalHist" AutoGenerateColumns="False" HorizontalAlign="Left" ShowHeaderWhenEmpty="true" 
                 DataKeyNames="REG_HOUSEHOLD_MEMBER_CRIMINAL_HISTORY_ID" OnRowDataBound="gvCriminalHist_RowDataBound"
                CssClass="gridview" EmptyDataText="No criminal history found." OnRowCommand="gvCriminialHist_RowCommand" >
                <Columns>
                    <asp:BoundField DataField="CRIMINAL_HISTORY" HeaderText="History" ItemStyle-Width="98%" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditCrimHist"   CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  
                                ToolTip="Edit Criminal History" Width="20" ><asp:Image ID="imgEdit" ImageUrl="~/Images/edit.png" runat="server" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteCrimHist"   CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  
                                ToolTip="Delete Criminal History" Width="20" ><asp:Image ID="imgDelete" ImageUrl="~/Images/cancel.png" runat="server" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
         <br />
            <div class="grid-hint">List details including dates and disposition, i.e., Parole, Probation, Fine, Time Served, etc.</div>
            <div style="padding-bottom:10px;text-align:right; width:100%;">
                <asp:LinkButton ID="lnkAddCHist" runat="server"  OnClick="lnkAddCHist_Click" ToolTip="Add Criminal History" ><asp:Image ID="imgCHAdd" ImageUrl="~/Images/add.png" runat="server" /></asp:LinkButton>
            </div>
    </div>
        <div style="text-align:right; width:100%;">
              <asp:Button ID="btnClose"  runat="server" Text="Close" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
</div>

<ajax:ModalPopupExtender ID="mpeMbrChild" runat="server" PopupControlID="pnlMbrChildModal" TargetControlID="btnChildDummy"
   BackgroundCssClass="modalBackground" CancelControlID="btnPopAddrCancel" >
</ajax:ModalPopupExtender>
    <asp:Panel ID="pnlMbrChildModal" runat="server" CssClass="modalPopup" style="display: none; height: auto; width: 440px;" >
    <asp:MultiView ID="mltMbrChild" runat="server" ActiveViewIndex="0">
        <asp:View ID="vwMbrAddrHist" runat="server">
                <asp:Panel ID="pnlPopAddrHeader" CssClass="popHeader" runat="server" >
                    <div class="popTitle">
                        <asp:Label ID="lblPopAddrTitle"  runat="server" Text="Edit Member Address"  />
                    </div>
                </asp:Panel>  
                <div><asp:ValidationSummary ID="vsMemberAddress" runat="server" DisplayMode="List" ValidationGroup="MemberAddress" ShowSummary="true" /></div>
                <br />
                <table style="width:auto;">
                    <tr>
                        <td class="formLabel wd100">County*</td>
                        <td class="fieldValue wd170" ><asp:TextBox ID="txtCounty" runat="server" MaxLength="50" CssClass="formField wd170"></asp:TextBox> <asp:RequiredFieldValidator ID="valCountyReqd" runat="server" ControlToValidate="txtCounty" Enabled="true" SetFocusOnError="true" 
                                    Display="Dynamic" Text="*"  ValidationGroup="MemberAddress" ErrorMessage="* County is required."></asp:RequiredFieldValidator>
                        
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd100">City*</td>
                        <td class="fieldValue wd170"><asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="formField wd170"></asp:TextBox> <asp:RequiredFieldValidator ID="valCityReqd" runat="server" ControlToValidate="txtCity" Enabled="true" SetFocusOnError="true" 
                                    Display="Dynamic" Text="*"  ValidationGroup="MemberAddress" ErrorMessage="* City is required."></asp:RequiredFieldValidator>
                        
                        </td>
                     </tr>
                    <tr>
                       <td class="formLabel wd100">State*</td>
                        <td class="fieldValue wd170"><asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown" />
                            <asp:RequiredFieldValidator runat="server" ID="valStateReqd" ValidationGroup="MemberAddress"
                                ControlToValidate="ddlState" ErrorMessage="* State is required." Text="*" Display="Dynamic"  SetFocusOnError="true" InitialValue="" />               
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd100">From Date*</td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="formField  wd120" /><ajax:CalendarExtender ID="ceFromDate" TargetControlID="txtFromDate" runat="server" />
                            <asp:RequiredFieldValidator runat="server" ID="valFromDateReqd" ControlToValidate="txtFromDate" ErrorMessage="* From Date is required." Text="*" Display="Dynamic" 
                                SetFocusOnError="true" ValidationGroup="MemberAddress" />
                            <asp:CompareValidator ID="valFromDateFormat" runat="server" ValidationGroup="MemberAddress"   
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFromDate"   Enabled="true"
                                ErrorMessage="* A valid From Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true" />
                            <asp:CustomValidator ID="valFromDateCustom" runat="server" OnServerValidate="Validate_FutureFromDate" ControlToValidate="txtFromDate" Display="Static" 
                                ValidationGroup="MemberAddress"  ErrorMessage="* From Date can not be a future date." Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel wd100">To Date</td>
                        <td class="fieldValue">
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="formField  wd120" /><ajax:CalendarExtender ID="ceToDate" TargetControlID="txtToDate" runat="server" />
                            <asp:CompareValidator ID="valToDateFormat" runat="server" ValidationGroup="MemberAddress"   
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtToDate"   Enabled="true"
                                ErrorMessage="* A valid To Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true" />
                        </td>
                    </tr>
                </table>
                <div class="btnBox" style="padding-top: 20px; padding-right:10px;">
                    <asp:Button ID="btnPopAddrSave" runat="server" Text="Save" CssClass="buttonBox" OnClick="btnPopAddrSave_Click"  CausesValidation="true" ValidationGroup="MemberAddress" />
                    <asp:Button ID="btnPopAddrCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnPopCancel_Click" CausesValidation="false" />
                </div>
        </asp:View>

        <asp:View ID="vwMbrCriminalHist" runat="server">
                <asp:Panel ID="pnlPopCHHeader" CssClass="popHeader" runat="server" >
                    <div class="popTitle">
                        <asp:Label ID="lblPopCHTitle"  runat="server" Text="Edit Member Criminal History"  />
                    </div>
                </asp:Panel>  
                <div><asp:ValidationSummary ID="vsMemberCH" runat="server" DisplayMode="List" ValidationGroup="MemberCriminalHist" ShowSummary="true" /></div>
                <br />
                    <table>
                        <tr>
                            <td class="formLabel wd50">Offense</td>
                            <td><asp:TextBox ID="txtOffense" runat="server" Rows="6" CssClass="formField wd350" TextMode="MultiLine" MaxLength="500" />
                                <asp:RequiredFieldValidator ID="valOffenseReqd" runat="server" SetFocusOnError="true" ValidationGroup="MemberCriminalHist" Text="*"
                                    ControlToValidate="txtOffense" ErrorMessage="* Offense details required." Display="Dynamic"  Enabled="true" />
                            </td>
                        </tr>
                        <tr><td></td><td><span class="field-hint">List details including dates and disposition, i.e., Parole, Probation, Fine, Time Served, etc.</span></td></tr>
                    </table>
                    <div class="btnBox" style="padding-top: 20px; padding-right:10px;">
                        <asp:Button ID="btnPopCHSave" runat="server" Text="Save" CssClass="buttonBox" OnClick="btnPopCHSave_Click" CausesValidation="true" ValidationGroup="MemberCriminalHist" />
                        <asp:Button ID="btnPopCHCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnPopCancel_Click" CausesValidation="false" />
                    </div>        

        </asp:View>
    </asp:MultiView>
</asp:Panel>
<asp:Button runat="server" ID="btnChildDummy" Style="display: none" Text="btnChildDummy" />

<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
