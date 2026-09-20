<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CLIACertificationsInfo, App_Web_rqhgepvh" %>

        <asp:Panel runat="server" ID="pnlHCliaCertGroup" GroupingText="CLIA Certification Data">
           
            <asp:GridView runat="server" ID="grdCLIACert" AllowPaging="True" AllowSorting="True"
                          AutoGenerateColumns="false" CellPadding="3"  OnPageIndexChanging="grdCLIACert_PageIndexChanging" PageSize="10"  CssClass="gridview">

                <HeaderStyle Font-Size="10pt" />
                <PagerStyle Font-Size="10pt" />
                <RowStyle Font-Size="10pt" ForeColor="Black" />
                <Columns>
                    <asp:BoundField DataField="CLIA_Number" HeaderText="CLIA Number" Visible="false"/>
                    <asp:BoundField DataField="Certificate_Number" HeaderText="Certificate Number"  />
                    <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date"  DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                    <asp:BoundField DataField="Expiration_Date" HeaderText="End Date"  DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                    <asp:BoundField DataField="Certificate_Type" HeaderText="Certification Type"  />
                    <asp:BoundField DataField="Facility_Type" HeaderText="Lab Type"  />                    
                </Columns>
            </asp:GridView>
             <asp:HiddenField ID="hdnCliaNo" runat="server" />
        </asp:Panel>




