<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_TransactionSearch, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/TransactionSearch.ascx" TagName="TransactionSearch" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/MQSearch.ascx" TagName="MQSearch" TagPrefix="uc2" %>
<%@ Register Src="~/PopupControls/TransactionMonitor.ascx" TagName="TransactionMonitor" TagPrefix="uc3" %>
<%@ Register Src="~/PopupControls/PNMDataUpdates.ascx" TagName="PNMDataUpdates" TagPrefix="uc4" %>
<%@ Register Src="~/PopupControls/DataLookup.ascx" TagName="DataLookup" TagPrefix="uc5" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content2" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <h1> Transaction Search </h1>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .RightBox {
            width: 860px;
        }

        .UserHeader {
            width: 1120px;
        }

        .rtsSelected .rtsLink {
            background-color: #65659f;
            color:white;
        }
    </style>

    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <div class="demo-container no-bg">

        <telerik:RadTabStrip RenderMode="Lightweight" runat="server" SelectedIndex="0" AutoPostBack="true" ID="RadTabStrip1" MultiPageID="RadMultiPage1" Skin="Silk">
            <Tabs>
                <telerik:RadTab TabIndex="0" Text="Transaction Search" Width="200px" PageViewID="RadPageView1"></telerik:RadTab>
                <telerik:RadTab TabIndex="1" Text="MQ Logging" Width="200px"  PageViewID="RadPageView2"></telerik:RadTab>
                <telerik:RadTab TabIndex="2" Text="Transaction Monitoring" Width="200px" PageViewID="TransactionMonitorPage"></telerik:RadTab>
                <telerik:RadTab TabIndex="3" Text="Data Fix" Width="200px" PageViewID="PNMDataUpdatesPage"></telerik:RadTab>
                <telerik:RadTab TabIndex="4" Text="Data Lookup" Width="200px" PageViewID="DataLookupPage"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage SelectedIndex="0"  runat="server" ID="RadMultiPage1"  CssClass="innerMultiPage">
            <telerik:RadPageView runat="server" ID="RadPageView1">
                <div class="ingredients qsf-ib">
                    <uc1:TransactionSearch ID="ucTransactionSearch" runat="server" />
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" ID="RadPageView2">
                <div class="ingredients qsf-ib">
                    <uc2:MQSearch ID="ucMQSearch" runat="server" />
                </div>
            </telerik:RadPageView>
            
           <telerik:RadPageView runat="server" ID="TransactionMonitorPage">
                <div class="ingredients qsf-ib">
                    <uc3:TransactionMonitor ID="ucTransactionMonitor" runat="server" />
                </div>
            </telerik:RadPageView>

             <telerik:RadPageView runat="server" ID="PNMDataUpdatesPage">
                <div class="ingredients qsf-ib">
                    <uc4:PNMDataUpdates ID="ucPNMDataUpdates" runat="server" />
                </div>
            </telerik:RadPageView>

             <telerik:RadPageView runat="server" ID="DataLookupPage">
                <div class="ingredients qsf-ib">
                    <uc5:DataLookup ID="ucDataLookup" runat="server" />
                </div>
            </telerik:RadPageView>

        </telerik:RadMultiPage>
    </div>

</asp:Content>
