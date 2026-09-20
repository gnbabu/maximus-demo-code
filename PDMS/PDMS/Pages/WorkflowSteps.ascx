<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_WorkflowSteps" Codebehind="WorkflowSteps.ascx.cs" %>

                                    <table role="presentation">
                                        <tr>
                                            <td><span class="formLabel" style="width:auto">Select a workflow:</span></td>
                                    <td style="text-align:left" colspan="3">
                                        <asp:DropDownList ID="ddlProcessIdList" runat="server" AutoPostBack="true" aria-label="Select a workflow" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlProcessIdList_SelectedIndexChanged"></asp:DropDownList>
                                        </tr>
                                        <tr>
                                             <td><span class="formLabel" style="width:auto">NPI:</span></td>
                                            <td style="text-align:left"><asp:Label ID="lblNPI" runat="server" /></td>
                                            <td><span class="formLabel" style="width:auto">Tax ID:</span></td>
                                            <td style="text-align:left"> <asp:Label ID="lblTaxId" runat="server" /> </td>
                                        </tr>
                                        <tr>
                                          <td><span class="formLabel" style="width:auto">Registration Id:</span></td>
                                            <td style="text-align:left"><asp:Label ID="lblRegId" runat="server" /></td>
                                            <td><span class="formLabel" style="width:auto">Process Id:</span></td>
                                            <td style="text-align:left"><asp:Label ID="lblProcessID" runat="server" /></td>
                                        </tr>
                                        <tr>
                                          <td><span class="formLabel" style="width:auto">Enrollment Action:</span></td>
                                            <td style="text-align:left"><asp:Label ID="lblEnrollmentAction" runat="server" /></td>
                                            <td><span class="formLabel" style="width:auto">DODD Contract Number:</span></td>
                                            <td style="text-align:left"><asp:Label ID="lblDODDContractNo" runat="server" /></td>
                                        </tr>
                                         <tr>
                                            <td><span class="formLabel" style="width:auto">Waiver Service Update Type:</span></td>
                                    <td style="text-align:left"> <asp:Label ID="lblWaiverSvcUpd" runat="server" /> </td>
                                            <td><span class="formLabel" style="width:auto">DODD App ID:</span></td>
                                    <td style="text-align:left"><asp:Label ID="lblDODDAppID" runat="server" /></td>
                                        </tr>
                                        <%--    <td><span class="formLabel" style="width:auto">Workflow Enddate:</span></td>
                                    <td style="text-align:left"> <asp:Label ID="lblWorkflowEndDate" runat="server" /> </td>
                                            <td><span class="formLabel" style="width:auto">Workflow Owner:</span></td>
                                    <td style="text-align:left"><asp:Label ID="lblWorkflowOwner" runat="server" /></td>
                                        </tr>--%>
                                        </table>

<br />
<asp:GridView runat="server" Width="100%" ID="grdHistory" Caption="Workflow" AutoGenerateColumns="false" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No records found." >
    <Columns>
        <asp:BoundField DataField="Task Name" HeaderText="Task Name" />
        <asp:BoundField DataField="User Name" HeaderText="User Name" />
        <asp:BoundField DataField="Start Date" HeaderText="Start Date" />
        <asp:BoundField DataField="End Date" HeaderText="End Date" />       
    </Columns>
    <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" /> 
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>


<table border="0" style="padding-left: 10px; width: 100%;">
    <tr><td><br />
<br />
<asp:GridView runat="server" ID="grdApplications" AutoGenerateColumns="False" HorizontalAlign="Left"
                        CssClass="gridview" EmptyDataText="No records found." DataKeyNames="PNMApplicationID">
<Columns>
    <asp:BoundField DataField="PNMApplicationID" HeaderText="PNM Application Id" />
    <asp:BoundField DataField="PNMApplicationStatus" HeaderText="PNM Application Status" />
    <asp:BoundField DataField="OtherAgencyAppID" HeaderText="Other Agency Application Id" />
    <asp:BoundField DataField="OtherAgencyAppType" HeaderText="Other Agency Application Type" />
    <asp:BoundField DataField="OtherAgencyAppStatus" HeaderText="Other Agency Application Status" />
    <asp:BoundField DataField="OtherAgencyStatusDt" HeaderText="Other Agency Status Date" DataFormatString="{0:MM/dd/yy}" />
    <asp:BoundField DataField="OtherAgencyAppLglStatus" HeaderText ="DODD Legal Status" />
</Columns>
<PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
<HeaderStyle CssClass="gridViewHeader" Width="100px" />
<AlternatingRowStyle CssClass="gridViewAltRow" />
<RowStyle CssClass="gridViewRow" />
<FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
        </td></tr>
    </table>

<table border="0" style="padding-left: 10px; width: 100%;">
    <tr><td><br />
<br />
<asp:GridView runat="server" ID="grdCPC" AutoGenerateColumns="False" HorizontalAlign="Left"
                        CssClass="gridview" EmptyDataText="No CPC records found." DataKeyNames="REG_ID">
<Columns>    
    <asp:BoundField DataField="CPC_PRIME_MED_ID" HeaderText="CPC Primary Medicaid ID" />
    <asp:BoundField DataField="CPC_IND_MED_ID" HeaderText="CPC ID Individual" />
    <asp:BoundField DataField="CPC_CON_MED_ID" HeaderText="CPC ID Convener" />
    <asp:BoundField DataField="CPC_PRAC_TYPE" HeaderText="CPC Practice Type" />
    <asp:BoundField DataField="SHOW_CREATE_CPC_IND_LINK" HeaderText="Create CPC Individual Link" />
    <asp:BoundField DataField="SHOW_CREATE_CPC_PP_LINK" HeaderText="Create a Practice Partnership Link" />
    <asp:BoundField DataField="SHOW_REATTEST_CPC_LINK" HeaderText="Re-attest CPC Individual or Practice Partnership Link" />
    <asp:BoundField DataField="SHOW_UPDATE_CPC_REG_LINK" HeaderText="Begin CPC Enrollment Update Link" />
    <asp:BoundField DataField="SHOW_UPDATE_CPC_CONTACT_LINK" HeaderText="Update CPC Contact Link" />
    <asp:BoundField DataField="SHOW_CPC_CONTINUE_APP_LINK" HeaderText="Continue CPC Application Link" />
</Columns>
<PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
<HeaderStyle CssClass="gridViewHeader" Width="100px" />
<AlternatingRowStyle CssClass="gridViewAltRow" />
<RowStyle CssClass="gridViewRow" />
<FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
        </td></tr>
    </table>

<table border="0" style="padding-left: 10px; width: 100%;">
    <tr><td><br />
<br />
<asp:GridView runat="server" Caption="CMC"  ID="grdCMC" AutoGenerateColumns="False" HorizontalAlign="Left"
                        CssClass="gridview" EmptyDataText="No CMC records found." DataKeyNames="REG_ID">
<Columns>       
    <asp:BoundField DataField="SHOW_ENROLL_CMC_REG_LINK" HeaderText="Initiate CMC Enrollment Link" />
    <asp:BoundField DataField="SHOW_REATTEST_CMC_REG_LINK" HeaderText="Re-attest CMC Provider Link" />
    <asp:BoundField DataField="SHOW_UPDATE_CMC_CONTACT_LINK" HeaderText="Update CMC Contact Link" />
    <asp:BoundField DataField="SHOW_CMC_CONTINUE_APP_LINK" HeaderText="Continue CMC Application Link" />
</Columns>
<PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
<HeaderStyle CssClass="gridViewHeader" Width="100px" />
<AlternatingRowStyle CssClass="gridViewAltRow" />
<RowStyle CssClass="gridViewRow" />
<FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
        </td></tr>
    </table>