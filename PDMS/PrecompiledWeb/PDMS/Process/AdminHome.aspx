<%@ page title="Dashboard" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_AdminHome, App_Web_qtcaivva" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Dashboard.ascx" TagName="Dashboard" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/DashboardProviderSummary.ascx" TagName="DashboardProviderSummary" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/DashboardGroupTotal.ascx" TagName="DashboardGroupTotal" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
                <h1 id="lblDashboardTitle" runat="server">Home</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">


        $(document).ready(function () {
            $(".expandcollapse button").click(function (e) {

                var evtTarget;
                var ac = $(e.target);
                // var ac = $(el).find('button');
                var expanded = ac.attr("aria-expanded");
                if (expanded.toLowerCase() === "true") expanded = 'false'; else expanded = 'true';
                ac.attr("aria-expanded", expanded);

                evtTarget = $(this).attr('data-panel_id');

                var evtArgument = $(this).text().replace("+ ", "");

                if (expanded === 'true') {
                    var el = ac.closest(".accordion").find(".accordionContent div[aria-live=polite]");
                    el.prepend("<p>Loading Data...</p>");
                    __doPostBack(evtTarget, evtArgument);
                }

            });
        });
    </script>
    <div class="WhiteBox" >
    <uc:Accordion ID="AccProviderSummary" runat="Server" SelectedIndex="0" EnableViewState="false"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="AccoPaneProviderSummary" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="true" data-panel_id="<%=upSummary.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Provider Summary</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upSummary" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:DashboardProviderSummary ID="ucDashboardProviderSummary" runat="server" Caption="" StatusLabel="Provider Summary" />
                        <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>


    <uc:Accordion ID="Accordion3" runat="Server" SelectedIndex="-1"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="AccordionPane4" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent" CssClass="accordionCollapse">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=up1.UniqueID %>"><span aria-hidden="true"  class="expandSymbol"></span>Individual</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardIndividualNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Individual "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardIndividualUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Individual "/>
                        <br />
<%--                        <uc:Dashboard ID="ucDashboardIndividualUpdateOwner" runat="server" Caption="" StatusLabel="Update Ownership" />
                        <br />--%>
                        <uc:Dashboard ID="ucDashboardIndividualRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Individual "/>
                        
                   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>

        </Panes>
    </uc:Accordion>


    <uc:Accordion ID="Accordion4" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="AccordionPane5" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false"  data-panel_id="<%=up2.UniqueID %>"><span aria-hidden="true"  class="expandSymbol"></span>Groups with Members</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardGroupWithMembersNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Groups with Members "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardGroupWithMembersUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Groups with Members "/>
                        <br />
<%--                        <uc:Dashboard ID="ucDashboardGroupWithMembersUpdateOwner" runat="server" Caption="" StatusLabel="Update Ownership" />
                        <br />--%>
                        <uc:Dashboard ID="ucDashboardGroupWithMembersRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Groups with Members "/>
                    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>

    <uc:Accordion ID="Accordion5" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="AccordionPane6" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=up3.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Institutions/Facilities</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="up3" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardInstitutionsOrFacilitiesNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Institutions/Facilities "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardInstitutionsOrFacilitiesUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Institutions/Facilities "/>
                        <br />
<%--                        <uc:Dashboard ID="ucDashboardInstitutionsOrFacilitiesUpdateOwner" runat="server" Caption="" StatusLabel="Update Ownership" />
                        <br />--%>
                        <uc:Dashboard ID="ucDashboardInstitutionsOrFacilitiesRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Institutions/Facilities "/>
                    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>

    <uc:Accordion ID="Accordion6" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="AccordionPane7" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=up4.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Pharmacy</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="up4" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="UcPharmacyNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Pharmacy "/>
                        <br />
                        <uc:Dashboard ID="UcPharmacyUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Pharmacy "/>
                        <br />
<%--                        <uc:Dashboard ID="UcPharmacyUpdateOwner" runat="server" Caption="" StatusLabel="Update Ownership" />
                        <br />--%>
                        <uc:Dashboard ID="UcPharmacyRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Pharmacy "/>
                   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>


    <uc:Accordion ID="accOrgainzation" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneOrgainzation" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upOrganization.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Organization</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upOrganization" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardOrganizationNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Organization "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardOrganizationUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Organization "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardOrganizationRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Organization "/>
                    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>
    <uc:Accordion ID="accOrderingReferringPrescribing" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneOrderingReferringPrescribing" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upOrderingReferringPrescribing.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Ordering, Referring or Prescribing</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upOrderingReferringPrescribing" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardOrderingReferringPrescribingNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Ordering, Referring or Prescribing "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardOrderingReferringPrescribingUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Ordering, Referring or Prescribing "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardOrderingReferringPrescribingRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Ordering, Referring or Prescribing "/>
                    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>
    <uc:Accordion ID="accChangeofOperator" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneChangeofOperator" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upChangeofOperator.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Change of Operator</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upChangeofOperator" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardChangeOfOperatorNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Change of Operator "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardChangeOfOperatorUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Change of Operator "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardChangeOfOperatorRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Change of Operator "/>
                   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>
    <uc:Accordion ID="accManagedCare" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneManagedCare" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upManagedCare.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Managed Care Plan Single Case</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upManagedCare" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardManagedCareNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Managed Care Plan Single Case "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardManagedCareUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Managed Care Plan Single Case "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardManagedCareRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Managed Care Plan Single Case "/>
                    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>
    <uc:Accordion ID="accWaiverODM" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneStreamlined" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upWaiverODM.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Waiver ODM</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upWaiverODM" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardWaiverODMNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Waiver ODM "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardWaiverODMUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Waiver ODM "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardWaiverODMRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Waiver ODM "/>
                   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>
    <uc:Accordion ID="accWaiverODA" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneWaiverODA" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upWaiverODA.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Waiver ODA</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upWaiverODA" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardWaiverODANewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Waiver ODA "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardWaiverODAUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Waiver ODA "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardWaiverODARevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Waiver ODA "/>
                   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>
    <uc:Accordion ID="accWaiverDODD" runat="Server" SelectedIndex="-1" 
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneWaiverDODD" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upWaiverDODD.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Waiver DODD</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upWaiverDODD" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardWaiverDODDNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Waiver DODD "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardWaiverDODDUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Waiver DODD "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardWaiverDODDRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Waiver DODD "/>
                    
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>

    <uc:Accordion ID="accNonMedicaidDODD" runat="Server" SelectedIndex="-1"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneNonMedicaidDODD" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upNonMedicaidDODD.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>Non-Medicaid DODD</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upNonMedicaidDODD" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate>
                        <uc:Dashboard ID="ucDashboardNonMedicaidDODDNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="Non-Medicaid DODD "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardNonMedicaidDODDUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="Non-Medicaid DODD "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardNonMedicaidDODDRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="Non-Medicaid DODD "/>
                   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>  
        <uc:Accordion ID="accCPP" runat="Server" SelectedIndex="-1"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
        SuppressHeaderPostbacks="true" CssClass="accordion">
        <Panes>
            <uc:AccordionPane ID="paneCPC" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent">
                <Header>
                    <h2 class="expandcollapse"><button class="headingButton" aria-expanded="false" data-panel_id="<%=upCPC.UniqueID %>"><span aria-hidden="true" class="expandSymbol"></span>CPC</button></h2>
                </Header>
                <Content>
                    <asp:UpdatePanel ID="upCPC" runat="server" UpdateMode="Conditional" aria-live="polite">
                        <ContentTemplate> 
                        <uc:Dashboard ID="ucDashboardCPCNewReg" runat="server" Caption="" StatusLabel="New Enrollment" Title="CPC "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardCPCUpdateReg" runat="server" Caption="" StatusLabel="Update Enrollment" Title="CPC "/>
                        <br />
                        <uc:Dashboard ID="ucDashboardCPCRevalidation" runat="server" Caption="" StatusLabel="Revalidation" Title="CPC "/>
                   
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </uc:AccordionPane>
        </Panes>
    </uc:Accordion>   
        
       
</div>

</asp:Content>

