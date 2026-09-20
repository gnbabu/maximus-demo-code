<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ACHHistory" Codebehind="ACHHistory.ascx.cs" %>

<br />
<asp:UpdatePanel ID="upACHHistory" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mltHistory" runat="server">
    <asp:View ID="vwBankingInfo" runat="server">

        <asp:GridView runat="server" Width="98%" ID="grdBankingInfo" 
            AutoGenerateColumns="False" HorizontalAlign="Left" onrowdatabound="grdBankingInfo_RowDataBound" 
            CssClass="gridview" EmptyDataText="No banking information found." AllowPaging="True" 
            PageSize="3" OnPageIndexChanging="grdBankingInfo_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="BANK_NAME" HeaderText="Bank Name" />
                <asp:BoundField DataField="ACCOUNT_NUMBER" HeaderText="Account Number" />
                <asp:BoundField DataField="ACH_ACCOUNT_TYPE" HeaderText="Account Type" />
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


        
        <asp:GridView runat="server" Width="98%" ID="grdEftContact" style="margin-top:20px"
            AutoGenerateColumns="False" HorizontalAlign="Left" onrowdatabound="grdEftContact_RowDataBound" 
            CssClass="gridview" EmptyDataText="No EFT contact found." AllowPaging="True" PageSize="3" OnPageIndexChanging="grdEftContact_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="EFT Contact Name">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("FIRST_NAME")+ " " + Eval("MIDDLE_NAME") + " " + Eval("LAST_NAME")%>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PHONE_NUMBER" HeaderText="Phone Number" />
                <asp:BoundField DataField="PHONE_EXTENSION" HeaderText="Ext" />
                <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="E-mail Address" />
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
    <asp:View ID="vwVendorInfo" runat="server">
        <asp:GridView runat="server" Width="98%" ID="grdVendorInfo" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No vendor information found.">
            <Columns>
                <asp:BoundField DataField="VENDOR_NUMBER" HeaderText="Vendor Number" />
                <asp:BoundField DataField="LOCATION_CODE" HeaderText="Location Code" />
                <asp:BoundField DataField="SEQUENCE_NUMBER" HeaderText="Sequence Number" />
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
</ContentTemplate>
</asp:UpdatePanel>