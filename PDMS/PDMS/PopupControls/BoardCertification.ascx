<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_BoardCertification" Codebehind="BoardCertification.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Panel ID="pnlBoardCertification" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
    <br />
    <div class="divGrid">
        <asp:GridView ID="grdBoardCertification" Width="98%" runat="server" AllowSorting="true" CssClass="gridview" EmptyDataText="No Board Certification found"
            OnRowCommand="grd_RowCommand" AutoGenerateColumns="false" HorizontalAlign="Left" >
            <Columns>

                <asp:BoundField DataField="Board_Certification_name" HeaderText="Board Certification" SortExpression="BoardCertification" />
                <asp:BoundField DataField="Board_Specialty_name" HeaderText="Board Specialty" SortExpression="BoardSpecialty" />
                <asp:BoundField DataField="Expiration_Date" HeaderText="Expiration Date" SortExpression="ExpirationDate" DataFormatString="{0:d}" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditBoardRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField>
                     <ItemTemplate>
                         <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteBoardRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete" 
                                            Visible="<%# CanUserViewDelete()  %>"/>
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
        <asp:ImageButton AlternateText="Board Certification history button" ID="btnAddBoardCertification" runat="server" ImageUrl="~/Images/add.png" CommandName="BoardCertification" OnCommand="lbtnAdd_Click" ToolTip="Add" />
    </div>
    <br />
</asp:Panel>
<div id="BoardDetail" runat="server" visible="false">
    <div style="width:750px;">
        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
        <asp:ValidationSummary ID="vsBoardCertifications" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valBoardCertifications" />
                    </div>  
        <div runat="server">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel200" id="Span1" runat="server">Are you Board Certified?</span></div>
                    <asp:RadioButtonList aria-label="Are You Board Certified?"  ID="rblBoardCertified" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblBoardCertified_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Selected="False" Text="No" Value="0"></asp:ListItem>
                        <asp:ListItem Selected="False" Text="Yes" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="valrblBoardCertify" runat="server" ControlToValidate="rblBoardCertified" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valBoardCertifications" ErrorMessage="* Board Certification is required."></asp:RequiredFieldValidator>
            </div>
            <div class="row">
                <p>If Yes, Please enter board certification information requested or confirm previously entered information is correct</p>
                <p><asp:Label ID="lblPT53HelpText" runat="server" visible="false" Text="If you are a Provider Type 53, a Board Certification is required. Enter your Board Certification information on this Board Certification page. If you currently hold a professional license, please enter your license information on the Professional License page." /></p>
            </div>
             </div>
            <div id="divBoardCertification" runat="server" visible="false">
                <div class="row" id="divIsPrimary" runat="server">
                    <div class="col-sm-3 text-right">
                        <asp:CheckBox ID="chkIsPrimaryBoard" runat="server" CssClass="mycheckBig"  /> 
                        <asp:CustomValidator ID="CustomValidator1" runat="server"  
                               Text="*" ErrorMessage="* Primary Board is Required" ValidationGroup="valBoardCertifications" 
                                onservervalidate="ValidatePrimaryBoard"></asp:CustomValidator>
                    </div>
                    <div class="col-sm-9" >
                        <span class="formLabel" id="spnPrimaryBoardCertMessage">Designate as Primary Board Certification.
                            <asp:Label ID="lblPrimaryBoardMessage" CssClass="failureNotification" runat="server"/></span>
                    </div>
                </div>

                <div class="row">
         <div class="col-sm-3 text-right"><span class="formLabel200" id="spnboardCertification" runat="server">Board Certification*</span></div>
            <div class="col-sm-9"><asp:DropDownList ID="ddlBoardCertification" aria-label="Board Certification"  runat="server" CssClass="formField wd400" OnSelectedIndexChanged="ddlBoardCertification_SelectedIndexChanged" AutoPostBack="true">  
 
</asp:DropDownList>
                    <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="ddlBoardCertification" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valBoardCertifications" ErrorMessage="* Board Certification is required."></asp:RequiredFieldValidator>
            </div>

        </div>
                <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200" id="spBoardSpecialty" runat="server">Board Specialty*</span></div>
            <div class="col-sm-9"><asp:DropDownList ID="ddlBoardSpecialty" aria-label="Board Specialty"  runat="server" CssClass="formField wd400" >  
 
</asp:DropDownList>
                <asp:TextBox ID="txtBoardSpecialty"  runat="server" CssClass="formField wd400" Visible="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valtxtBoardSpecialty" runat="server" ControlToValidate="ddlBoardSpecialty" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valBoardCertifications" ErrorMessage="* Board Specialty is required."></asp:RequiredFieldValidator>
            </div>

        </div>
                <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200">Certification Number</span></div>
            <div class="col-sm-9"><asp:TextBox ID="txtCertificationNumber" aria-label="Certification Number" runat="server" CssClass="formField wd400" >  
 
</asp:TextBox>
                
                    
            </div>

        </div>
                <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200" id="spnEffectiveDate" runat="server">Effective Date*</span></div>
            <div class="col-sm-9"><asp:TextBox ID="txtEffectiveDate" aria-label="Effective Date" runat="server" CssClass="formField wd400" />
                <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEffectiveDate" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffectiveDate" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valBoardCertifications" ErrorMessage="* Effective Date is required."></asp:RequiredFieldValidator>
                    <asp:CompareValidator id="CompareValidator2" runat="server" ValidationGroup="valBoardCertifications"   
            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEffectiveDate"  
            ErrorMessage="Select a valid Date of Effective" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
            SetFocusOnError="true" /> </div>

        </div>  
               <div class="row">
            <div class="col-sm-3 text-right"><span class="formLabel200" id="spnExpirationDate" runat="server">Expiration Date*</span></div>
            <div class="col-sm-9"><asp:TextBox ID="txtExpirationDate" aria-label="Expiration Date" runat="server" CssClass="formField wd400" />
                <ajax:CalendarExtender ID="calStart" TargetControlID="txtExpirationDate" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtExpirationDate" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valBoardCertifications" ErrorMessage="* Expiration Date is required."></asp:RequiredFieldValidator>
                    <asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="valBoardCertifications"   
            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtExpirationDate"  
            ErrorMessage="Select a valid Date of Expiration" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
            SetFocusOnError="true" /> </div>

        </div>  
            <div class="row">
            </div>
        </div>
</div>
    </div>
</div>
