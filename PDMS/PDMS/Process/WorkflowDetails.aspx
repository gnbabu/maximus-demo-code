<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="WorkflowDetails" Codebehind="WorkflowDetails.aspx.cs" %>

<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
   Workflow Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    

    <style type="text/css">
         

        table {
            text-align: left;
            width: 100%;
        }

        th, td {
            padding: 2px;
            text-align: left;
        }
    </style>
 <div class="WhiteBox" >
    <cc1:GroupBox ID="gbSearch" HorizontalAlign="Center" Width="98%" runat="server">


        <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
        <div>
            <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="WorkflowDetails" ShowSummary="true" />
        </div>

       

            <asp:Panel ID="pnlFilter" runat="server" >
                <asp:UpdateProgress ID="updateProgress" runat="server" >
                    <ProgressTemplate>
                        <div style="padding-right: 30px">
                            <img src="../Images/ajax-loader.gif" />
                            Loading ...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <table  >
                    <tr>
                        <td>
                            <span class="formLabel150">Select a workflow</span>
                        </td>
                        <td>

                            <%-- <asp:DropDownList ID="ddlWorkflow" runat="server" CssClass="formDropDown"   Width="100%"/>--%>
                            <asp:DropDownList ID="ddlWorkflow" runat="server" CssClass="formDropDown"/>
                        </td>
                        <td>
                        </td>
                        <td> 
                        </td>

                    </tr>
                    <tr>

                        <td>
                            <span class="formLabel150">NPI</span>
                        </td>

                        <td>
                             <asp:Label ID="lblNPI" runat="server" CssClass="formFieldReadOnly"  Text="1083657829"/>

                        </td>
                        <td>
                            <span class="formLabel150">Tax ID</span>
                        </td>
                        <td>

                              <asp:Label ID="lblTaxID" runat="server" CssClass="formFieldReadOnly"  Text="203500913" />
                             
                        </td>

                    </tr>
                   
                    <tr>
                        <td>
                            <span class="formLabel150">Registration Id</span>
                        </td>
                        <td>

                        <asp:Label ID="lblRegID" runat="server" CssClass="formFieldReadOnly"  Text="3201"/>
                        </td>
                        <td>
                            <span class="formLabel150">Process Id</span>
                        </td>
                        <td>
                             <asp:Label ID="lblProcessID" runat="server" CssClass="formFieldReadOnly"  Text="2971"/>
                        </td>

                    </tr>
                   
                </table>
                 
            </asp:Panel>
        
    </cc1:GroupBox>
    <br />
    <telerik:RadGrid
        ID="gvTask"
        runat="server"
        AutoGenerateColumns="False"
        Width="100%"
        AllowSorting="true"
        EmptyDataText="No Task found."
        OnRowCommand="gvTask_RowCommand"
        OnRowDataBound="gvTask_RowDataBound"
        OnPageIndexChanging="gvTask_PageIndexChanging"
        OnSorting="gvTask_Sorting"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        Skin="PDMSModern"
        EnableEmbeddedSkins="false">

        <GroupingSettings CaseSensitive="false" />
        <MasterTableView AllowSorting="true" PageSize="15"
            AllowPaging="True" Width="100%" AutoGenerateColumns="true"
             TableLayout="Auto">
            
        </MasterTableView>
    </telerik:RadGrid>

    
     </div>
</asp:Content>

