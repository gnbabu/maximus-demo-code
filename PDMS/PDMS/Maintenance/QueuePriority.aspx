<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Maintenance_QueuePriority" MaintainScrollPositionOnPostback="true" Codebehind="QueuePriority.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/> 
    <script type="text/javascript">
        var selectedItems = 0;
        function onClientSelectedIndexChangingHandler(sender, e)
        {
            $telerik.$(".rlbButton").addClass("rlbMoveUpDisabled rlbDisabled");
            var list = $find("<%= WorkflowTaskOrder.ClientID %>");
            var items = list.get_selectedItems();          
            if (items.length > 0) {
                $(".MoveBulk").removeClass("MoveBulk");
                $(".MoveBulk").addClass("MoveBulkHigh");
            }
            else
            {
                $(".MoveBulk").removeClass("MoveBulkHigh");
                $(".MoveBulk").addClass("MoveBulk");              
            }
        }
 </script>
    <div class="WhiteBox">
        <asp:Panel ID="pnlUserSelect" runat="server">
            <div style="width:auto;height:auto;">
                <span class="formLabelAuto"><asp:Label ID="lblSelectRole" runat="server" Text="Select Role:"></asp:Label></span>
                <asp:DropDownList ID="ddlReferenceDataSelect" runat="server" CssClass="formDropDown" AutoPostBack="true" AppendDataBoundItems="True"
            onselectedindexchanged="ddlReferenceDataSelect_SelectedIndexChanged" />
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="workflowPanel" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="data" runat="server">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadListBox ID="WorkflowTaskOrder" runat="server" AutoPostBackOnReorder="true" 
                            SelectionMode="Multiple" RenderMode="Lightweight" Height="600px" Width="550px"  
                             AllowReorder="true"
                             OnReordering="WorkflowTaskOrder_Reordering" OnClientSelectedIndexChanging="onClientSelectedIndexChangingHandler"
                             OnReordered="WorkflowTaskOrder_Reordered" EnableTheming="true" all>
                                    <ButtonSettings RenderButtonText="true" HorizontalAlign="Center" VerticalAlign="Middle"  />
                                    <ItemTemplate>
                                        <asp:Label class="formLabelleft80" ID="wfName" runat="server" Text="Workflow: "></asp:Label>
                                        <asp:Label ID="wfnameValue" runat="server" Text='<%# Bind("WorkflowName") %>' ></asp:Label>
                                        <br />
                                        <asp:Label class="formLabelleft80" ID="wfTask" runat="server" Text="Task: "></asp:Label>
                                        <asp:Label ID="wfTaskValue" runat="server" Text='<%# Bind("TaskName") %>' ></asp:Label>
                                        <asp:Label class="formLabelleft80" ID="rank" runat="server" Text="      Rank: "></asp:Label>
                                        <asp:Label ID="wfTaskRank" runat="server" Text='<%# Bind("rank") %>' ></asp:Label>
                                    </ItemTemplate>
                                </telerik:RadListBox>
                                <asp:Label ID="selectedVal" runat="server"> </asp:Label>
                                <br />
                            </td>
                            <td>
                                <asp:Button ID="MoveBulk" Text="Move to Same Priority Level" runat="server" CssClass="MoveBulk" OnClick="MoveBulk_Click" ></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="WorkflowTaskOrder" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="btnBox btnBoxCenter">
            <br />
            <asp:Button ID="btnSave" runat="server" CausesValidation="true" Text="Save" CssClass="buttonBox" OnClick="btnSave_Click"/>
            <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click"/>
        </div>
        <br />
        <br />
    </div>
</asp:Content>
