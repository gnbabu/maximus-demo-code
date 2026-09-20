<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Reimbursement" Codebehind="Reimbursement.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

<asp:Panel ID="pnlReimbursement" runat="server" Style="display: inline-block; width: 100%;">
    <uc1:Separator ID="Separator16" runat="server" Header="Reimbursement Rates" />
    <br />
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdReimbursement" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grd_RowCommand" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="REG_REIMBURSEMENT_ID" HeaderText="" SortExpression="REG_REIMBURSEMENT_ID" HtmlEncode="False" Visible="false" />
                <asp:BoundField DataField="Type_Of_Service" HeaderText="Type Of Service" SortExpression="Type_Of_Service" HtmlEncode="False" />
                <asp:BoundField DataField="PerDiem_VisitAll" HeaderText="Per Diem or Visit All-Inclusive" SortExpression="PerDiem_VisitAll" />
                <asp:BoundField DataField="Paymment_Method" HeaderText="Payment Method" SortExpression="Paymment_Method" />
                <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditReimbursementCodeRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
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
    <div id="Div1" class="divGrid1" runat="server">
        <%--<table id="tblRe" runat="server" style="border-collapse: collapse;">
            <tr>
                <td>--%>
        <br /><br />
                    <div id="tblre1" runat="server" width="450px" border="0" class="">
                        <div class="row">
                            <div class="col-sm-3 text-right"><asp:Label ID="lblPhys" runat="server" CssClass="formLabel200" Text="Are physician services and components included in your cost" ></asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:RadioButtonList ID="rblPhysician" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="true" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="false" Text="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row" style="display:none;">
                            <div class="col-sm-3 text-right">
                            <asp:Label ID="lblSepe" runat="server" cssclass="formLabel200" Text="Separately?">
                            </asp:Label></div>
                            <div class="col-sm-9">
                                <asp:RadioButtonList ID="rblSeparately" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="true" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="false" Text="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
               <%-- </td>
            </tr>
        </table>--%>
    </div>
</asp:Panel>

<div id="reimbursementDetail" runat="server" visible="false">
    <div>
        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    </div>
    <div  style="text-align:left;margin:10px;" >
        <asp:ValidationSummary ID="vsReimbursement" runat="server" DisplayMode="List" CssClass="failureNotification" ValidationGroup="valReimbursement" />
    

    <table style="width: auto;" id="tblreimburse" runat="server" Visible="true">
        <tr>
            <td class="formLabel wd200">Type of Service?</td>
            <td style="text-align: left;">
                <asp:Label ID="lblServiceType" runat="server" />
                <%-- <asp:DropDownList ID="ddlServiceType" runat="server">
                            <asp:ListItem Text="Inpatient" Value="InPatient"></asp:ListItem>
                            <asp:ListItem Text="Outpatient" Value="OutPatient"></asp:ListItem>
                            <asp:ListItem Text="Emergency Room" Value="EmergencyRoom"></asp:ListItem>
                        </asp:DropDownList>--%>
            </td>

        </tr>
        <tr>
            <td class="formLabel wd200">Per Diem or Visit All-Inclusive</td>
            <td style="text-align: left;">
                <asp:DropDownList ID="ddlOption" runat="server">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="Per Diem" Value="Per Diem"></asp:ListItem>
                    <asp:ListItem Text="Visit All-Inclusive" Value="Visit All Inclusive"></asp:ListItem>
                </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlOption"
                    Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                    ValidationGroup="valReimbursement" ErrorMessage="* Please select Per Diem or Visit All-Inclusive."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="formLabel wd200">Other payment methods</td>
            <td style="text-align: left;">
                <asp:TextBox runat="server" ID="txtPayment"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rvPayment" runat="server" ControlToValidate="txtPayment"
                    Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                    ValidationGroup="valReimbursement" ErrorMessage="* Required."></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td class="formLabel wd200">Effective Date                                            </td>
            <td>
                <asp:TextBox ID="txtEffectiveDate" runat="server">
                </asp:TextBox>
                <ajax:CalendarExtender ID="caltxtDate" TargetControlID="txtEffectiveDate" runat="server" />

                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valReimbursement"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEffectiveDate"
                    ErrorMessage="Select a valid Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEffectiveDate"
                    Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                    ValidationGroup="valReimbursement" ErrorMessage="* Required."></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    </div>
    <asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
    <asp:TextBox ID="hidID" runat="server" Visible="false" />
</div>
