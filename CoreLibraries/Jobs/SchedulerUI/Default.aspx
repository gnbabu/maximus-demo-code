<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SchedulerUI._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Modify this template to jump-start your ASP.NET application.</h2>
            </hgroup>
            <p>
                To learn more about ASP.NET, visit <a href="http://asp.net" title="ASP.NET Website">http://asp.net</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET.
                If you have any questions about ASP.NET visit
                <a href="http://forums.asp.net/18.aspx" title="ASP.NET Forum">our forums</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <telerik:RadGrid  ID="RadGrid1" runat="server" OnDetailTableDataBind="RadGrid1_DetailTableDataBind" AutoGenerateColumns="false"  OnPreRender="RadGrid1_PreRender" OnItemCommand="RadGrid1_ItemCommand">
       <MasterTableView>
           <DetailTables>
               <telerik:GridTableView Name="TriggerTable" >
                   <Columns>
                            <telerik:GridBoundColumn SortExpression="TriggerType" HeaderText="Trigger Type" DataField="TriggerType" />
                            <telerik:GridBoundColumn SortExpression="TriggerState" HeaderText="Trigger State" DataField="TriggerState" />
                            <telerik:GridBoundColumn SortExpression="Priority" HeaderText="Priority" DataField="Priority" />
                            <telerik:GridBoundColumn SortExpression="NextFire" HeaderText="NextFire" DataField="NextFire" />
                            <telerik:GridBoundColumn SortExpression="LastFire" HeaderText="LastFire" DataField="LastFire" />
                            <telerik:GridBoundColumn SortExpression="CronExpr" HeaderText="Cron Expression" DataField="CronExpr" />
                   </Columns>
               </telerik:GridTableView>
           </DetailTables>
                        <Columns>
                            <telerik:GridBoundColumn SortExpression="Name" HeaderText="Name" DataField="Name" />
                            <telerik:GridBoundColumn SortExpression="Group" HeaderText="Group" DataField="Group" />
                            <telerik:GridBoundColumn SortExpression="Description" HeaderText="Description" DataField="Description" />
                            <telerik:GridBoundColumn SortExpression="AssemblyPath" HeaderText="Assembly Path" DataField="AssemblyPath" />
                            <telerik:GridBoundColumn SortExpression="ClassName" HeaderText="Class Name" DataField="ClassName" />
                            <telerik:GridBoundColumn SortExpression="AssemblyName" HeaderText="Assembly Name" DataField="AssemblyName" />
                            <telerik:GridBoundColumn SortExpression="EmailErrors" HeaderText="Email Errors" DataField="EmailErrors" />
                            <telerik:GridBoundColumn SortExpression="EmailEndofExecution" HeaderText="Email Endof Execution" DataField="EmailEndofExecution" />
                            <telerik:GridButtonColumn CommandName="RunNow" Text="Run Now" UniqueName="RunNow" HeaderText="Run Now" />
                        </Columns>
       </MasterTableView>
    </telerik:RadGrid>

</asp:Content>