<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Reports_ReportCriteria, App_Web_xmsld3al" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--OHPNM-7695 Accessebility issue--%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
   <h1>Report Criteria</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
        <div class="WhiteBox">
    <asp:GridView runat="server" aria-labeledby="ServiceProviderAgreementsHeading" Width="100%" ID="grReportNameMonthly" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="ServiceProviderAgreementsHeading">Service Provider Agreements</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>

    <asp:GridView runat="server"  aria-labeledby="cpcHeading"  Width="100%" ID="grReportCPC" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
             <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="cpcHeading">CPC Reports</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")+"|"+ Eval("SideTextContent") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>

    <asp:GridView runat="server" aria-labeledby="cmcReportsHeading" Width="100%" ID="grReportCMC" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation" >
        <Columns>
           <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="cmcReportsHeading">CMC Reports</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lblCMCReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>


    <asp:GridView runat="server" aria-labeledby="monthlydatabaseChecksHeading" Width="100%" ID="grReportNameQuartely" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="monthlydatabaseChecksHeading">Monthly Database Checks</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>


    <asp:GridView runat="server" aria-labeledby="siteVisitsHeading" Width="100%" ID="grReportNameYearly" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="siteVisitsHeading">Site Visits</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    <asp:GridView runat="server" aria-labeledby="applicationFeeHeading" Width="100%" ID="grApplicationFee" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="applicationFeeHeading">Application Fee</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    <asp:GridView runat="server" aria-labeledby="statusofProviderRegHeading" Width="100%" ID="grStatusOfProviderRegistrations" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
           <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="statusofProviderRegHeading">Status Of Provider Registrations</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>

    <asp:GridView runat="server" aria-labeledby="facilitiesCHOPsAndClosuresHeading" Width="100%" ID="grFacilitiesCHOPsandClosures" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
           <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="facilitiesCHOPsAndClosuresHeading">Facilities,  CHOPs, and Closures</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>

    <asp:GridView runat="server" aria-labeledby="groupAffiliationHeading" Width="100%" ID="gvProviderListing" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
           <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="groupAffiliationHeading">Provider Listing with Group Affiliation</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />

                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
	
    <asp:GridView runat="server" aria-labeledby="credentialingReportsHeading" Width="100%" ID="gvCredentialingReports" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
             <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="credentialingReportsHeading">Credentialing Reports</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    <asp:GridView runat="server" aria-labeledby="networkAdequacyReportsHeading" Width="100%" ID="gvNetworkAdequacyReports" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
             <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="networkAdequacyReportsHeading">Network Adequacy Reports</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    <asp:GridView runat="server" aria-labeledby="odmStaffReportingHeading" Width="100%" ID="gvODMStaffReports" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
        <Columns>
             <asp:TemplateField>
                <HeaderTemplate>
                    <h2 id="odmStaffReportingHeading">ODM Staff Reporting</h2>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    <asp:GridView runat="server" aria-labeledby="complianceMonitoringHeading" Width="100%" ID="gvComplianceMonitoring" DataKeyNames="ReportName" AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found." OnRowCommand="grReportName_RowCommand" role="presentation">
    <Columns>
         <asp:TemplateField>
            <HeaderTemplate>
                <h2 id="complianceMonitoringHeading">Compliance Monitoring</h2>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="lbtnReportName" runat="server" Text='<%# Bind("DisplayReportName") %>' DataTextField="ReportName" CommandName="ReportName" HeaderText="ReportName" CommandArgument='<%#Eval("ReportName")+"|"+ Eval("DisplayReportName")  %>' />
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

</asp:Content>

