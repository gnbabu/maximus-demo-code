<%@ control language="C#" autoeventwireup="true" inherits="Pages_HouseholdMembers, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/HouseholdMember.ascx" TagName="MemberDetail" TagPrefix="uc" %>

<script type="text/javascript">
    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }
</script>

    Click on the section header to expand or collapse the panel.<br /><br />
    <ajax:CollapsiblePanelExtender ID="cpeInstructions" runat="server" Collapsed="false" TargetControlID="pnlInstructions" 
        ExpandControlID="pnlSepInstructions" CollapseControlID="pnlSepInstructions" />
    <asp:Panel runat="server" ID="pnlSepInstructions" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
    <uc1:SectHd runat="server" ID="sepInstructions" Header="- Instructions" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlInstructions" class="panelContent">
        I understand that the District of Columbia Department of Health and Human Services requires the following background information on me.<br />
        History may be requested from law enforcement or criminal justice agencies, including but not limited to:
        <ul>
            <li>District of Columbia Adult/ Child Abuse and Neglect Central Registry/er</li>
            <li>Law Enforcement Records</li>
            <li>The District of Columbia Sex Offender's Registry</li>
            <li>The District of Columbia Department of Motor Vehicles District of Columbia Driver License Information System</li>
            <li>License Information System</li>
            <li>GSA website (opens new window) <a href="http://epls.gov" target="_blank">http://epls.gov</a> for debarment actions by federal agencies and exclusion actions from Medicare, Medicaid 
                or other federal programs through the Office of Inspector General at <a href="www.oig.hhs.gov/fraud/exclusions.asp" target="_blank">www.oig.hhs.gov/fraud/exclusions.asp</a>.</li>
        </ul>
        Based on the services you are providing, you will be providing services in your home.  The Department requires background information on 
        all members of that household including full names, previous names, birthdates and Social Security numbers on all persons living in that residence and any criminal 
        background information. This information is required in determining your approval as a service provider.<br /><br />
        Please complete this information in the section below.
    </asp:Panel>

    <br /><uc1:SectHd runat="server" ID="sepMembers" Header="Household Members" /><br />
    <div class="wdAll">
        <asp:GridView runat="server" Width="100%" ID="gvMembers" AutoGenerateColumns="False" HorizontalAlign="Left" ShowHeaderWhenEmpty="true" 
             DataKeyNames="REG_HOUSEHOLD_MEMBER_ID" CssClass="gridview" EmptyDataText="No household members found." 
            OnRowCommand="gvMembers_RowCommand" OnRowDataBound="gvMembers_RowDataBound" >
            <Columns>
                <asp:BoundField DataField="NAME" HeaderText="Name" ItemStyle-Width="40%" />
                <asp:BoundField DataField="BIRTH_DATE" HeaderText="Date of Birth" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" ItemStyle-Width="20%" />
                <asp:BoundField DataField="RELATIONSHIP" HeaderText="Household Status" ItemStyle-Width="25%" />
                <asp:BoundField DataField="SEX" HeaderText="Sex" ItemStyle-Width="10%" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditMember"   CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  
                            ToolTip="Edit Member Details" Width="20" ><asp:Image ID="imgEdit" ImageUrl="~/Images/edit.png" runat="server" /></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteMember"   CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  
                            ToolTip="Delete Member" Width="20" ><asp:Image ID="imgDelete" ImageUrl="~/Images/cancel.png" runat="server" /></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader"  />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div style="clear:both;padding-bottom:10px;text-align:right; width:100%;">
        <asp:LinkButton ID="lnkAdd" runat="server"  OnClick="lnkAdd_Click" ToolTip="Add Member" ><asp:Image ID="imgAdd" ImageUrl="~/Images/add.png" runat="server" /></asp:LinkButton>
<%--        <asp:LinkButton ID="lnkHistory" runat="server"  OnClick="lnkHistory_Click" ToolTip="Member History" ><asp:Image ID="imgHistory" ImageUrl="~/Images/history_icon.jpg" runat="server" /></asp:LinkButton>--%>
    </div>

<ajax:ModalPopupExtender ID="mpeDetail" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" BackgroundCssClass="modalBackground" >
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" style="display: none; height: auto; width: 650px;" >
    <asp:Panel ID="pnlHeaderMpe" CssClass="popHeader" runat="server" >
        <div class="popTitle">
            <asp:Label ID="lblTitle"  runat="server" Text="Edit Member Detail"  />
        </div>
    </asp:Panel>  
     <asp:Panel ID="pnlMain" runat="server" style="margin:10px" >
            <uc:MemberDetail ID="ucMemberDetail" runat="server" />
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />



