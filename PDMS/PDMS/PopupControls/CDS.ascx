<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CDS" Codebehind="CDS.ascx.cs" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<div>
    <asp:ValidationSummary ID="vsCDSNumber" DisplayMode="List" runat="server" CssClass="failureNotification val-summary" Visible="true" Enabled="true" ValidationGroup="valStateCDSNumber" ShowSummary="true" />
</div>
</br>
<div id="cdsGrid" runat="server">
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdCDS" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grd_RowCommand" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="STATE_CDS_Number" HeaderText="CDS Number" SortExpression="STATE_CDS_Number" />
                <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                <asp:BoundField DataField="DateIssued" HeaderText="Date Issued" SortExpression="DateIssued" DataFormatString="{0:d}" />
                <asp:BoundField DataField="ExpirationDate" HeaderText="Expiration Date" SortExpression="ExpirationDate" DataFormatString="{0:d}" />
                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="2%" >
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditCDSCodeRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField>
                  <ItemTemplate>
                       <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteCDSCodeRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                          ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"   />                        
                         
                  </ItemTemplate>
               </asp:TemplateField>--%>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddCDS" runat="server" AlternateText="CDS" ImageUrl="~/Images/add.png" CommandName="CDS" OnCommand="lbtnAdd_Click" ToolTip="Add" /><br />
    </div>
    <br />
</div>

<div id="cdsDetail" runat="server" visible="false">
  <div id="Table3" runat="server">
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel200">CDS Number</span></div>
        <div class="col-sm-9">
            <asp:TextBox ID="txtSTATE_CDS_Number" runat="server" Text="" MaxLength="20" aria-label="CDS Number" CssClass="formField"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rvStateCDSNumber" runat="server" ControlToValidate="txtSTATE_CDS_Number"
                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                ValidationGroup="valStateCDSNumber" ErrorMessage="* State CDS Number is required."></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="reStateCDSNumberValidator" runat="server" ControlToValidate="txtSTATE_CDS_Number"
                ValidationExpression="^[a-zA-Z0-9]+$" ErrorMessage="* Enter only alphabets or numbers for State CDS Number." Text="*" Display="Dynamic"
                ValidationGroup="valStateCDSNumber" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel200">State</span></div>
        <div class="col-sm-9">
            <asp:DropDownList ID="ddlState" runat="server" AppendDataBoundItems="true" aria-label="State" DataTextField="StateName"  DataValueField="StateId" CssClass="formDropDown"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlState"
                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                ValidationGroup="valStateCDSNumber" ErrorMessage="* State is required."></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel200">Date Issued</span></div>
        <div class="col-sm-9">
            <asp:TextBox ID="txtDateIssued" runat="server" aria-label="Date Issued" Text="" CssClass="formField"></asp:TextBox>
            <ajax:calendarextender id="caltxtDateIssued" targetcontrolid="txtDateIssued" runat="server" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valStateCDSNumber"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateIssued"
                ErrorMessage="Select a valid Date Issed" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDateIssued"
                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                ValidationGroup="valStateCDSNumber" ErrorMessage="* Issue date is required."></asp:RequiredFieldValidator>
            </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel200">Expiration Date</span></div>
        <div class="col-sm-9">
            <asp:TextBox ID="txtExpirationDate" aria-label="Expiration Date" runat="server" Text="" CssClass="formField"></asp:TextBox>
            <ajax:calendarextender id="CalendarExtender2" targetcontrolid="txtExpirationDate" runat="server" />

            <asp:CompareValidator ID="CompareValidator3" runat="server" ValidationGroup="valStateCDSNumber"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtExpirationDate"
                ErrorMessage="Select a valid Date of Expiration" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtExpirationDate"
                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                ValidationGroup="valStateCDSNumber" ErrorMessage="* Expiration date is required."></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToCompare="txtDateIssued" ControlToValidate="txtExpirationDate"
                Display="Static" ErrorMessage="Expiration Date must not be earlier than Issue Date" Text="*" Operator="GreaterThanEqual" SetFocusOnError="True" Type="Date" ValidationGroup="valStateCDSNumber" />
<%--                        <asp:CompareValidator ID="CompareEndTodayValidator" runat="server" ValidationGroup="valStateCDSNumber" 
                Type="Date" Operator="GreaterThan" ControlToCompare="txtExpirationDate" 
                ErrorMessage="Date must greater than current date." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" 
                ControlToValidate="txtExpirationDate"></asp:CompareValidator>--%>


        </div>
    </div>
  </div>
    <br />
 <uc1:Separator ID="ucSep1" runat="server" Header="Uploaded Documents" />
    <br />
<asp:PlaceHolder runat="server" 
               ID="PlaceholderUploadCDS"></asp:PlaceHolder>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
</div>