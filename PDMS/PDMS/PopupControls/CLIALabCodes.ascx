<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CLIALabCodes" Codebehind="CLIALabCodes.ascx.cs" %>
    
        <asp:Panel runat="server" ID="pnlHCliaLab" GroupingText="CLIA Lab">
           
            <asp:GridView runat="server" ID="grdCLIALabCode" AllowPaging="True" AllowSorting="True" CssClass="gridview"
                          AutoGenerateColumns="false" CellPadding="3"  OnPageIndexChanging="grdCLIALabCode_PageIndexChanging" PageSize="10" OnRowDataBound="grdCLIALabCode_RowDataBound">

                <HeaderStyle Font-Size="10pt" />
                <PagerStyle Font-Size="10pt" />
                <RowStyle Font-Size="10pt" ForeColor="Black" />
                <Columns>
                    <asp:BoundField DataField="CLIA_Number" HeaderText="CLIA Number" Visible="false"/>
                    <asp:BoundField DataField="LabCode" HeaderText="Lab Code"  />
                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                    <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />                                   
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hdnCliaNumber" runat="server" />
        </asp:Panel>





