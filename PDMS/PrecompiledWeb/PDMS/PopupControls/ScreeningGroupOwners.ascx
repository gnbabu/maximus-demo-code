<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ScreeningGroupOwners, App_Web_av5ll3zk" %>
<%@ Register Src="~/PopupControls/ScreeningHistory.ascx" TagName="ScreeningHistory" TagPrefix="uc" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script src="../Scripts/customizeTelerik.js" type="text/javascript"></script>
<style type="text/css"> 
        .selectedrow 
        { 
            background: None !important; 
            height: 22px; 
            border: solid 1px white; 
            border-top: solid 1px white; 
            border-bottom: solid 1px white; 
            padding-left: 4px; 
        } 
        .RadGrid_PDMSModern .rgSelectedRow td 
        { 
            background-color: #FFFF66 !important; 
        } 
    </style> 
 <script type="text/javascript">

     function addscope(sender, args) {
         var gridTable = sender.get_masterTableView().get_element();
         var rows = gridTable.getElementsByTagName("tr");
         for (var i = 0; i < rows.length; i++) {
             $(rows[i]).attr("scope", "col");
         }
         var cells = gridTable.getElementsByTagName("td");
         for (var i = 0; i < cells.length; i++) {
             $(cells[i]).attr("scope", "col");
         }
         var headers = gridTable.getElementsByTagName("th");
         for (var i = 0; i < headers.length; i++) {
             $(headers[i]).attr("scope", "col");
         }
     }
   </script>

<div id="divScreeningHistory" class="" style="display:inline-block;width:100%;">
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
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Name" DataField="NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Owner Type" DataField="OWNER_TYPE_NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="% of Ownership" DataField="PERCENTAGE_OF_OWNERSHIP"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Screening Start" DataField="START_DATE_TIME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Screening End" DataField="END_DATE_TIME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4" HeaderText="Screening Status" DataField="SCREENING_STATUS_NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col5" HeaderText="Result" DataField="SCREENING_RESULT_NAME"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click"  Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
</div>   

    <div class="divGrid">
        <telerik:radgrid id="gvGroupOwners" runat="server" allowpaging="True" allowsorting="false" allowfilteringbycolumn="true" skin="PDMSModern" OnNeedDataSource ="gvGroupOwners_NeedDataSource"  
            onitemcommand="gvGroupOwners_RowCommand" onitemdatabound="gvGroupOwners_RowDataBound" onpageindexchanged="gvGroupOwners_PageIndexChanging" OnSelectedIndexChanged="gvGroupOwners_SelectedIndexChanged" style="width: 100%; border-style: none" cssclass="gridViewSmallFont">
         <GroupingSettings CaseSensitive="false" />
         <mastertableview autogeneratecolumns="False" allowsorting="False" allowpaging="true" PageSize="5" TableLayout="Auto" EnableHeaderContextMenu="true"
    datakeynames="REG_OWNER_ID,SCREENING_ID,START_DATE_TIME,END_DATE_TIME,NAME,SCREENING_STATUS_ID">
         <columns>
            
            <telerik:gridboundcolumn datafield="NAME" headertext="Name" showfiltericon="true" allowfiltering="true" visible="true" />
            <telerik:gridboundcolumn datafield="OWNER_TYPE_NAME" headertext="Owner Type" showfiltericon="true" allowfiltering="true" visible="true" />
            <telerik:gridboundcolumn datafield="PERCENTAGE_OF_OWNERSHIP" headertext="% of Ownership" allowfiltering="false" visible="true" />
            <telerik:gridboundcolumn datafield="START_DATE_TIME" headertext="Screening Start" allowfiltering="false" visible="true" />
            <telerik:gridboundcolumn datafield="END_DATE_TIME" headertext="Screening End" allowfiltering="false" visible="true" />
            <telerik:gridtemplatecolumn headertext="Screening Status" allowfiltering="true" datafield="SCREENING_STATUS_NAME" DataType="System.String">
                <itemtemplate>
                    <asp:LinkButton
                        ID="btnGASelect"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# Eval("SCREENING_ID") %>'
                        CommandName="Select"
                        Text='<%# Eval("SCREENING_STATUS_NAME") %>' />
                </itemtemplate>
            </telerik:gridtemplatecolumn>
            <telerik:gridboundcolumn datafield="SCREENING_RESULT_NAME" headertext="Screening Result" allowfiltering="true" visible="true" />
            <telerik:gridboundcolumn datafield="MATCH_RESULT_NAME" headertext="Match Result" allowfiltering="true" visible="true" />
         </columns>
         <pagerstyle mode="NextPrevAndNumeric" alwaysvisible="true" />
         </mastertableview>
         <clientsettings EnableRowHoverStyle="true">
            <%--<scrolling allowscroll="True" usestaticheaders="True" scrollheight="500px" />--%>
            <selecting allowrowselect="true"></selecting>
            <ClientEvents OnFilterMenuShowing="filterMenuShowing" OnGridCreated="addscope" />
         </clientsettings>
         <SelectedItemStyle CssClass="selectedrow" /> 
         <filtermenu onclientshowing="MenuShowing" cssclass="gridviewFilter" />
        </telerik:radgrid>
    </div>
</div>