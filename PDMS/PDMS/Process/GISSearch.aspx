<%@ Page Title="GIS Search" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_GISSearch" Codebehind="GISSearch.aspx.cs" %>

<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">

    <div style="display: inline;" class="page-main-header">
        <br />
        <span style="text-align: center">Provider Search (Public)</span>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">

        <style type="text/css">
            .page-main-header {
                font-family: 'Merriweather', serif !important;
                font-size: 28px !important;
            }

            .demo-container .mapTitle {
                color: #444444;
                font-family: 'Merriweather', serif !important;
                font-weight: normal;
                text-align: center;
                font-size: 20px;
            }

            #contactsContainer {
                background-color: white;
                padding: 0 0 0 20px;
            }

                #contactsContainer #offices {
                    list-style-type: none;
                    display: inline-block;
                    *display: inline;
                    zoom: 1;
                    margin: 0;
                    padding: 0;
                }

                    #contactsContainer #offices .office {
                        display: inline-block;
                        *display: inline;
                        zoom: 1;
                        width: 240px;
                        vertical-align: top;
                        margin: 20px 10px 10px 0;
                    }

            .leftCol {
                float: left;
            }

            .rightCol {
                padding-left: 55px;
                font-size: 14px;
                font-family: "Segoe UI",Segoe,"Roboto","Droid Sans","Helvetica Neue",Helvetica,Arial,sans-serif;
                text-align: left;
                line-height: 19px;
                background-image: linear-gradient(white, black);
            }

                .rightCol .country {
                    font-size: 24px;
                    font-weight: normal;
                    line-height: 18px;
                    margin-bottom: 20px;
                }

                .rightCol .city {
                    font-weight: bold;
                }

                .rightCol .email {
                    color: #0394ae;
                }

                    .rightCol .email a {
                        color: #0394ae;
                        text-decoration: none;
                    }

                        .rightCol .email a:hover {
                            text-decoration: underline;
                        }

                .rightCol .location {
                    border-top: 1px solid #c9c9c9;
                    margin-top: 10px;
                    padding-top: 10px;
                }

            .flag {
                background-image: url("images/flags.png");
                height: 40px;
                width: 40px;
                box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
                border: 1px solid white;
            }

            .flag-unitedstates {
                background-position: 0 -240px;
            }

            .flag-denmark {
                background-position: 0 -120px;
            }

            .flag-australia {
                background-position: 0 0;
            }

            .flag-bulgaria {
                background-position: 0 -40px;
            }

            .flag-india {
                background-position: 0 -160px;
            }

            .flag-unitedkingdom {
                background-position: 0 -200px;
            }

            .flag-germany {
                background-position: 0 -80px;
            }

            .RadMap .k-tooltip-content {
                font-family: "Segoe UI",Segoe,"Roboto","Droid Sans","Helvetica Neue",Helvetica,Arial,sans-serif;
                font-size: 14px;
            }

            div.k-tooltip.RadMap {
                background-image: linear-gradient(white, gray);
                border: none;
            }

                /* Change the callout's layout */
                div.k-tooltip.RadMap .k-callout {
                    border-top-color: black;
                }

            .MyMap {
                border: 1px solid #FFF;
            }

            .public-search-dropdown {
                width: 500px;
                text-align: left;
            }

            @media only screen and (max-width: 760px) {
                .public-search-dropdown {
                    width: 100% !important;
                }
            }

            .WhiteBox, .OwnerBackground {
                width: 100%;
                padding: 0px;
            }

            .rddlPopup .rddlItem {
                background-color: white;
            }

            .form-control {
                font-size: 17px;
                height: 44px;
                color: #000;
                font-weight: normal;
            }
            .formLabel150{
                float:left;
                text-align:left;
            }
        </style>

        <asp:Label runat="server" ID="Header" CssClass="formLabel150">Search Criteria</asp:Label>
        <hr />
        <%-- <cc1:GroupBox ID="gbSearch" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="98%" runat="server">   --%>
        <div style="width: 98%; padding-bottom: 100px; font-family: 'Source Sans Pro', sans-serif !important;">
            <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
            <div>
                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="ProviderSearch" ShowSummary="true" />
            </div>
            <div style="text-align: center;">
                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">

                    <div class="row">
                        <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                            <span class="formLabel150">
                                <asp:Label ID="lblProviderType" runat="server" AssociatedControlID="ddlProviderType" Text="Provider Type" />
                            </span>
                            <asp:DropDownList CssClass="form-control" runat="server" AutoPostBack="true" ID="ddlProviderType"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="btnBox">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBox" OnClick="btnSearch_Click" ToolTip="Search" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" ToolTip="Clear" />
                    </div>
                </asp:Panel>
                <h2 style="display: none;">
                    <asp:Label ID="lblgbSearch1" runat="server" Text="Search Criteria"></asp:Label>
                </h2>
            </div>
            <%-- </cc1:GroupBox>--%>
        </div>
        <br />

        <%-- <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />--%>
        <div class="demo-container size-auto">
            <telerik:radmap rendermode="Lightweight" runat="server" id="RadMap1" zoom="5" cssclass="MyMap" tooltip="Map">
                <centersettings latitude="39" longitude="-94.578331" />
                <markerdefaultssettings shape="pinTarget"></markerdefaultssettings>
                <databindings>
                    <markerbinding datatitlefield="FormattedAddress" datalocationlatitudefield="Latitude" datalocationlongitudefield="Longitude" />
                </databindings>
                <layerscollection>
                    <telerik:maplayer type="Tile" subdomains="a,b,c"
                        urltemplate="http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png"
                        attribution="&copy; <a href='http://osm.org/copyright' title='OpenStreetMap contributors' target='_blank'>OpenStreetMap contributors</a>."
                        opacity=".6">
                    </telerik:maplayer>
                </layerscollection>
            </telerik:radmap>

            <cc2:messagebox id="MessageBox2" runat="server" />

            <asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy" ToolTip="hidden" />
        </div>
    </div>
</asp:Content>

