<%@ page title="GIS Search" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_GISSearch, App_Web_14kfymdt" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <h1>  Provider Search (Public) </h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">

        <style type="text/css">
            .demo-container .mapTitle {
                color: #444444;
                font-family: 'Segoe UI',Segoe,'Roboto','Droid Sans','Helvetica Neue',Helvetica,Arial,sans-serif;
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
            .public-search-dropdown{
                width:500px;
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
        }
        </style>

       <asp:Label runat="server" ID="Header"><h2>Search Criteria</h2></asp:Label>
         <hr />
       <%-- <cc1:GroupBox ID="gbSearch" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="98%" runat="server">   --%>
          <div style="width:98%;padding-bottom:100px">
            <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
            <div>
                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="ProviderSearch" ShowSummary="true" />
            </div>
            <div style="text-align: center;">
                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
                    <div class ="container">
                        <div style="text-align:left">
                                <span class="formLabel150">
                                    <asp:Label ID="lblProviderType" runat="server" AssociatedControlID="ddlProviderType" Text="Provider Type" />
                                </span>
                               <%-- <telerik:RadDropDownList ID="ddlProviderType" runat="server" Skin="PDMSModern" RenderMode="Lightweight" BackColor="White" AutoPostBack ="true" OnItemSelected="RddOnProviderTypeSelected_ItemSelected"
                                    CssClass="public-search-dropdown" DropDownHeight="250px" >
                                 </telerik:RadDropDownList>--%>
                             <asp:dropdownlist runat="server" autopostback="true" id="ddlProviderType">  </asp:dropdownlist>
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
            <telerik:RadMap RenderMode="Lightweight" runat="server" ID="RadMap1" Zoom="5" CssClass="MyMap" ToolTip="Map">
                <CenterSettings Latitude="39" Longitude="-94.578331" />
                <MarkerDefaultsSettings Shape="pinTarget"></MarkerDefaultsSettings>
                <DataBindings>
                    <MarkerBinding DataTitleField="FormattedAddress" DataLocationLatitudeField="Latitude" DataLocationLongitudeField="Longitude" />
                </DataBindings>
                <LayersCollection>
                    <telerik:MapLayer Type="Tile" Subdomains="a,b,c"
                        UrlTemplate="http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png"
                        Attribution="&copy; <a href='http://osm.org/copyright' title='OpenStreetMap contributors' target='_blank'>OpenStreetMap contributors</a>."
                        Opacity=".6">
                    </telerik:MapLayer>
                </LayersCollection>
            </telerik:RadMap>

            <cc2:MessageBox ID="MessageBox2" runat="server" />

            <asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy" ToolTip="hidden" />
        </div>
    </div>
</asp:Content>

