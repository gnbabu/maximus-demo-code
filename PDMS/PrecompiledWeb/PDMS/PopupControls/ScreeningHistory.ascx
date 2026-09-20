<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ScreeningHistory, App_Web_c4une0e1" %>

<div id="divScreeningHistory" style="display:inline-block;width:100%;">
    <br />
    <table style="width:100%;" role="presentation">
        <tr>
            <td style="width: 70%"></td>
            <td style="width: 30%;" align="right">
                
                <asp:CheckBox ID ="chkShowFullHistory" runat="server" CssClass="formCheckBox" Text="Show Monthly Database Checks Results" AutoPostBack="True" OnCheckedChanged="chkShowFullHistory_CheckedChanged"  />
                        
            </td>
        </tr>
    </table>
    <br />
    <div style="width: 100%; text-align: right">
    <telerik:RadGrid ID="RadGridExport" runat="server" Visible="true">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
            <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Provider Screening Details">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false">
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" HeaderText="Screening Type" DataField="WORKFLOW_LONG_NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Screening Start" DataField="START_DATE_TIME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Screening End" DataField="END_DATE_TIME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4" HeaderText="Status" DataField="SCREENING_STATUS_NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col5" HeaderText="Result" DataField="SCREENING_RESULT_NAME"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click"  Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" Visible="false"><img src="../Images/PDF_24x24.png" alt="Download PDF" /></asp:LinkButton>
</div>
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdScreeningHistory" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No screening history found." DataKeyNames="SCREENING_ID" 
            OnSelectedIndexChanged="grdScreeningHistory_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="WORKFLOW_LONG_NAME" HeaderText="Screening Type" />
                <asp:BoundField DataField="START_DATE_TIME" HeaderText="Screening Start" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField DataField="END_DATE_TIME" HeaderText="Screening End" DataFormatString="{0:MM/dd/yy}" />
                <%--<asp:BoundField DataField="SCREENING_STATUS_NAME" HeaderText="Status" />--%>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:LinkButton ToolTip="Select" ID="btnSelect" runat="server" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' 
                            Text='<%# Eval("SCREENING_STATUS_NAME") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SCREENING_RESULT_NAME" HeaderText="Result" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
</div>