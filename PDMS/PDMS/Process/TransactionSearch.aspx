<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_TransactionSearch" Codebehind="TransactionSearch.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/TransactionSearch.ascx" TagName="TransactionSearch" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/MQSearch.ascx" TagName="MQSearch" TagPrefix="uc2" %>
<%@ Register Src="~/PopupControls/TransactionMonitor.ascx" TagName="TransactionMonitor" TagPrefix="uc3" %>
<%@ Register Src="~/PopupControls/PNMDataUpdates.ascx" TagName="PNMDataUpdates" TagPrefix="uc4" %>
<%@ Register Src="~/PopupControls/DataLookup.ascx" TagName="DataLookup" TagPrefix="uc5" %>
<%@ Register Src="~/PopupControls/PNMDynamicFields.ascx" TagName="PNMDynamicFields" TagPrefix="uc6" %>
<%@ Register Src="~/PopupControls/UIControlVisibilityConfig.ascx" TagName="UIControlVisibilityConfig" TagPrefix="uc7" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content2" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="display: inline;" class="page-main-header">
        <br />
        <span style="text-align: center">Operations & Transaction Hub
        </span>

    </div>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .page-main-header {
            font-family: 'Merriweather', serif !important;
            font-size: 28px !important;
        }

        .demo-container {
            font-family: 'Source Sans Pro', sans-serif !important;
        }

        .RightBox {
            width: 860px;
        }

        .UserHeader {
            width: 1120px;
        }

        .rtsSelected .rtsLink {
            background-color: #65659f;
            color: white;
        }

        .pageHeader {
            margin-bottom: 20px !important;
        }

        .formDropDown {
            font-size: 17px;
            height: 44px;
            color: #000;
        }

        .formField {
            height: 44px;
            color: #000;
        }

        .ohio-field-input {
            height: 44px;
            color: #000;
            font-size: 17px;
            font-weight: normal;
        }

        .textEntry {
            height: 44px;
            color: #000;
            font-weight: normal;
        }

        .DropDownList {
            font-size: 17px;
            height: 44px;
            color: #000;
            font-weight: normal;
        }

        .formLabel150 {
            float: left;
            text-align: left;
        }
    </style>

    <div class="demo-container no-bg">

        <telerik:radtabstrip rendermode="Lightweight" runat="server" selectedindex="0" autopostback="true" id="RadTabStrip1" multipageid="RadMultiPage1" skin="Silk">
            <tabs>
                <telerik:radtab tabindex="0" text="Transaction Search" width="200px" pageviewid="RadPageView1"></telerik:radtab>
                <telerik:radtab tabindex="1" text="MQ Logging" width="200px" pageviewid="RadPageView2"></telerik:radtab>
                <telerik:radtab tabindex="2" text="Transaction Monitoring" width="240px" pageviewid="TransactionMonitorPage"></telerik:radtab>
                <telerik:radtab tabindex="3" text="Data Fix" width="200px" pageviewid="PNMDataUpdatesPage"></telerik:radtab>
                <telerik:radtab tabindex="4" text="Data Lookup" width="200px" pageviewid="DataLookupPage"></telerik:radtab>
                <telerik:radtab tabindex="5" text="Dynamic Field Configuration" width="270px" pageviewid="PNMDynamicFields"></telerik:radtab>
                <telerik:radtab tabindex="6" text="UI Control Visibility" width="200px" pageviewid="UIControlVisibilityConfig"></telerik:radtab>
            </tabs>
        </telerik:radtabstrip>
        <telerik:radmultipage selectedindex="0" runat="server" id="RadMultiPage1" cssclass="innerMultiPage">
            <telerik:radpageview runat="server" id="RadPageView1">
                <div class="ingredients qsf-ib">
                    <uc1:transactionsearch id="ucTransactionSearch" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="RadPageView2">
                <div class="ingredients qsf-ib">
                    <uc2:mqsearch id="ucMQSearch" runat="server" />
                </div>
            </telerik:radpageview>

            <telerik:radpageview runat="server" id="TransactionMonitorPage">
                <div class="ingredients qsf-ib">
                    <uc3:transactionmonitor id="ucTransactionMonitor" runat="server" />
                </div>
            </telerik:radpageview>

            <telerik:radpageview runat="server" id="PNMDataUpdatesPage">
                <div class="ingredients qsf-ib">
                    <uc4:pnmdataupdates id="ucPNMDataUpdates" runat="server" />
                </div>
            </telerik:radpageview>

            <telerik:radpageview runat="server" id="DataLookupPage">
                <div class="ingredients qsf-ib">
                    <uc5:datalookup id="ucDataLookup" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="PNMDynamicFields">
                <div class="ingredients qsf-ib">
                    <uc6:pnmdynamicfields id="ucPNMDynamicFields" runat="server" />
                </div>
            </telerik:radpageview>
            <telerik:radpageview runat="server" id="UIControlVisibilityConfig">
                <div class="ingredients qsf-ib">
                    <uc7:UIControlVisibilityConfig id="ucUIControlVisibilityConfig" runat="server" />
                </div>
            </telerik:radpageview>
        </telerik:radmultipage>
    </div>
</asp:Content>
