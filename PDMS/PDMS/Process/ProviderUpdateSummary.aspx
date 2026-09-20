<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_ProviderUpdateSummary" MasterPageFile="~/MasterWorkflowPage.master" Title="Provider Management Details" Codebehind="ProviderUpdateSummary.aspx.cs" %>
<%@ MasterType TypeName="MasterWorkflowPage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/SectionUpdateList.ascx" TagName="SectionUpdateList" TagPrefix="cc5" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <span class="pageHeader">&nbsp;&nbsp;&nbsp;Provider Update - Lets keep your information current !</span><br />
    <div class="WhiteBox">
                <div id="updateSections" runat="server">
                     
                    <p> Please click Update button to update your provider information. Once you have completed all your
                    updates, you will be able to submit your changes from this screen.</p>
                    <svg width="85%" height="20px">
                                              <g fill="none" style="stroke:rgb(91,155,213);stroke-width:2">
                        <line x1="1" x2="100%" y1="10" y2="10" stroke-width="2"/>
                        </g>
                    </svg>
                    
                    <cc5:SectionUpdateList runat="server" id="mostCommonUpd" Name="Most Common Updates" IconSrc="~/Images/Affiliations_Summary.png"></cc5:SectionUpdateList>            

                    <div>
                        <asp:Repeater ID="rptUpdateSections" runat="server" OnItemDataBound="rptUpdateSections_ItemDataBound">
                            <ItemTemplate>
                                <svg width="85%" height="20px">
                                                          <g fill="none" style="stroke:rgb(91,155,213);stroke-width:2">
                                    <line x1="1" x2="100%" y1="10" y2="10" stroke-width="2"/>
                                    </g>
                                </svg>
                                <cc5:SectionUpdateList runat="server" id="sectionItem" Name="Most Common Updates" IconSrc="~/Images/MostCommon.png"></cc5:SectionUpdateList>            
                            </ItemTemplate>
                        </asp:Repeater>
                        <svg width="85%" height="20px">
                            <g fill="none" stroke="black" stroke-width="1">
                                <line x1="1" x2="100%" y1="10" y2="10" stroke-width="2" stroke-dasharray="1, 2"/>
                            </g>
                        </svg>

                    </div>

                </div>

        </div>
</asp:Content>
