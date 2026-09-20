<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SpecialtiesHistory, App_Web_rqhgepvh" %>
<br />

<asp:Panel ID="pnlSpecialtiesHistory" runat="server">
    <asp:GridView runat="server" Width="98%" ID="grdSpecialtiesHistory" AutoGenerateColumns="False" HorizontalAlign="Left"   AllowPaging="True" AllowSorting="True"       
            PageSize="10"  CssClass="gridview" OnPageIndexChanging="grdSpecialtiesHistory_PageIndexChanging" EmptyDataText="No entries found." OnSorting="grdSpecialtiesHistory_Sorting">
        <Columns>
            <%-- <asp:BoundField DataField="Index" HeaderText="Index" />--%>
            <asp:BoundField DataField="Operation"                       HeaderText="Operation"      SortExpression="Operation"          />
            <asp:BoundField DataField="SPECIALTY_TYPE_ID"               HeaderText="Specialty Type" SortExpression="SPECIALTY_TYPE_ID" />
            <asp:BoundField DataField="PRIMARY_FLAG"                    HeaderText="Primary Flag"   SortExpression="PRIMARY_FLAG" />
            <asp:BoundField DataField="ENROLL_STATUS_DESC"              HeaderText="ES"             SortExpression="ENROLL_STATUS_DESC" />
            <asp:BoundField DataField="ENROLLMENT_STATUS_REASONS_DESC"  HeaderText="ESR"            SortExpression="ENROLLMENT_STATUS_REASONS_DESC" />
            <asp:BoundField DataField="START_DATE"                      HeaderText="Start Date"     SortExpression="START_DATE"     DataFormatString="{0:MM/dd/yyyy}" />
            <asp:BoundField DataField="END_DATE"                        HeaderText="End Date"       SortExpression="END_DATE"       DataFormatString="{0:MM/dd/yyyy}" />
            <asp:BoundField DataField="UserName"                        HeaderText="User Name"      SortExpression="UserName" />
            <asp:BoundField DataField="DateOfAction"                    HeaderText="Update Date"    SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
        </Columns>
        <PagerStyle             CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle            CssClass="gridViewHeader" />
        <AlternatingRowStyle    CssClass="gridViewAltRow" />
        <RowStyle               CssClass="gridViewRow" />
        <FooterStyle            CssClass="gridViewFooter" />
    </asp:GridView>
</asp:Panel>
