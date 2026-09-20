<%@ control language="C#" autoeventwireup="true" inherits="UserControls_DashboardGroupTotal, App_Web_ejpeldiy" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="uc" %>
<%@ Register Src="DashboardItem.ascx" TagName="DashboardItem" TagPrefix="uc" %>

<style type="text/css"> 
    .headerLineText 
    { 
        float: left;
        border: none; 
        background-color:White; 
        font-weight: bold;
        padding-right: 5px;
        padding-left: 5px; 
	    margin-left: 150px;
        height:20px; 
    } 
    .headerLineContainer 
    { 
        border-bottom:solid 1px #e3e3e3; 
        height:10px;
        margin-bottom: 5px; 
    } 
</style> 

<uc:GroupBox ID="grpDashboard" Caption="Dashboard" CaptionStyle-CssClass="bodyTextBold" runat="server" CssClass="Dashboard">
    <div style="float: left; padding-left: 18%; width: 48%">
        <div class="headerLineContainer">
            <div class="headerLineText">Active</div>
        </div> 
    </div>
    <div style="clear:both;"></div>
    <asp:GridView ID="gvDashboard" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" Width="100%" 
        CssClass="gridViewSmallFont" EmptyDataText="No dashboard found.">
        <Columns>
            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="18%" />            
            <asp:TemplateField HeaderText="Maintenance" ItemStyle-Width="13%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard0" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Maintenance") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="0" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/Maintenance" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Conversion" ItemStyle-Width="12%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard1" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Conversion") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="1" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/Conversion" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Updates" ItemStyle-Width="9%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard2" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Updates") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="2" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/Updates" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Admin" ItemStyle-Width="8%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard3" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("AdminReview") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="3" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/Admin Review" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total" ItemStyle-Width="10%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard4" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Total") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="4" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/Total" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="New" ItemStyle-Width="7%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard5" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("New") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="5" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/New" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Terminated" ItemStyle-Width="12%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard6" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Terminated") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="6" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/Terminated" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Suspended" ItemStyle-Width="11%">
                <ItemTemplate>
                    <uc:DashboardItem ID="ucDashboard7" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Suspended") %>' 
                        StatusID='<%# Bind("StatusID") %>' Ordinal="7" DashboardLabel='<%# DataBinder.Eval(Container.DataItem,"Status") + "/Suspended" %>' />
                </ItemTemplate> 
            </asp:TemplateField>
        </Columns>   
        <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
        <HeaderStyle CssClass="gridViewHeader" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" /> 
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</uc:GroupBox>
