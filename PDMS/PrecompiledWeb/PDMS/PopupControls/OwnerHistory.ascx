<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OwnerHistory, App_Web_l5y5araq" %>

<br />
<asp:MultiView ID="mltHistory" runat="server">
    <asp:View ID="vwOpt0" runat="server" />
    <asp:View ID="vwOpt1" runat="server" >
                <asp:GridView runat="server" Width="98%" ID="grdOwnerRelationships" AutoGenerateColumns="False" HorizontalAlign="Left" 
                CssClass="gridview" EmptyDataText="No owner information found.">
                <Columns>
                    <asp:BoundField DataField="OWNER1" HeaderText="Person" />
                    <asp:BoundField DataField="RELATIONSHIP_NAME" HeaderText="Relationship" />
                    <asp:BoundField DataField="OWNER2" HeaderText="Person" />
                    <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                    <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                        HeaderText="Last Modified Date" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
     </asp:View>
    <asp:View ID="vwOtherInfo" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdOwnerOtherInfo" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No owner information found.">
            <Columns>
                <asp:BoundField DataField="OWNER_NAME" HeaderText="Person or Entity" />
                <asp:BoundField DataField="NAME" HeaderText="Name of Other Provider Entity" />
                <asp:BoundField DataField="TAX_ID" HeaderText="Tax ID" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwConviction" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdConviction" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No conviction found.">
            <Columns>
                <asp:BoundField DataField="OWNER_NAME" HeaderText="Person or Entity" />
                <asp:BoundField DataField="COURT_RECORD_NAME" HeaderText="Name on Court Records" />
                <asp:BoundField DataField="TAX_ID" HeaderText="SSN/TIN" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwDebarred" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdDebarred" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No debarred owner/control information found.">
            <Columns>
                <asp:BoundField DataField="OWNER_NAME" HeaderText="Person or Entity" />
                <asp:BoundField DataField="DEBARMENT_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"  
                    AccessibleHeaderText="When were you Debarred" HeaderText="When were you Debarred" />
                <asp:BoundField DataField="DEBARMENT_DURATION" HeaderText="Length of Debarment" />
                <asp:BoundField DataField="DEBARMENT_REASON" HeaderText="Reason for Debarment" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwExcluded" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdExcluded" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No information found.">
            <Columns>
                <asp:BoundField DataField="OWNER_NAME" HeaderText="Person or Entity" />
                <asp:BoundField DataField="EXCLUSION_BEGIN_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" 
                    HeaderText="Beginning date of Exclusion or Termination" />
                <asp:BoundField DataField="EXCLUSION_END_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" 
                    HeaderText="End date of Exclusion or Termination" />
                <asp:BoundField DataField="EXCLUSION_REASON" HeaderText="Reason for Exclusion or Termination" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwTerminated" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdTerminated" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No termination information found.">
            <Columns>
                <asp:BoundField DataField="OWNER_NAME" HeaderText="Person or Entity" />
                <asp:BoundField DataField="TERMINATION_REASON" HeaderText="Reason for Termination" />
                <asp:BoundField DataField="TERMINATION_BEGIN_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" 
                    HeaderText="Date of Termination" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwPenalty" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdPenalty" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No penalties found.">
            <Columns>
                <asp:BoundField DataField="OWNER_NAME" HeaderText="Person or Entity" />
                <asp:BoundField DataField="STATE_NAME" HeaderText="State where practicing when CMP assessed" />
                <asp:BoundField DataField="CMP_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" HeaderText="Date of CMP" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwOriginalOwner" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdOriginalOwner" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No owners found.">
            <Columns>
                <asp:BoundField DataField="NAME" HeaderText="Name of Original Owner" />
                <asp:BoundField DataField="TAX_ID" HeaderText="SSN or Tax ID of Original Owner" />
                <asp:BoundField DataField="TRANSFER_PLACE" HeaderText="Place of Transfer" />
                <asp:BoundField DataField="TRANSFER_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" HeaderText="Date of Transfer" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwSubcontractor" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdSubcontractor" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No subcontractors found.">
            <Columns>
                <asp:BoundField DataField="NAME" HeaderText="Name of Subcontractor" />
                <asp:BoundField DataField="TAX_ID" HeaderText="Tax ID" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwSubcontractorOwner" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdSubcontractoOwner" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No subcontractor information found.">
            <Columns>
                <asp:BoundField DataField="NAME" HeaderText="Name of Individual or Entity" />
                <asp:BoundField DataField="PERCENT_OF_OWNERSHIP" HeaderText="Percent of Ownership" />
                <asp:BoundField DataField="TITLE" HeaderText="Title" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwSubcontractor5Years" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdSubcontractor5Years" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No subcontractors found.">
            <Columns>
                <asp:BoundField DataField="NAME" HeaderText="Name of Subcontractor" />
                <asp:BoundField DataField="CITY" HeaderText="City" />
                <asp:BoundField DataField="STATE" HeaderText="State" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
    <asp:View ID="vwSupplier" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdSupplier" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No suppliers found.">
            <Columns>
                <asp:BoundField DataField="NAME" HeaderText="Name of Supplier" />
                <asp:BoundField DataField="NPI" HeaderText="NPI" />
                <asp:BoundField DataField="TAX_ID" HeaderText="Tax ID" />
                <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
                <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                    HeaderText="Last Modified Date" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:View>
</asp:MultiView>