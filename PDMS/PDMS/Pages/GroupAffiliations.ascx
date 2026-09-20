<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_GroupAffiliations" Codebehind="GroupAffiliations.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Pages/GroupAffiliationsCtrl.ascx" TagPrefix="uc" TagName="GroupAffiliations" %>

<script type="text/javascript">
    function doClick(buttonName, e) {
        // The purpose of this function is to allow the enter key to 
        // point to the correct button to click.
        var key;

        if (window.event) key = window.event.keyCode;   // IE
        else key = e.which;                             // Firefox

        if (key == 13) {
            //Get the button the user wants to have clicked
            var btn = document.getElementById(buttonName);
            if (btn != null) { //If we find the button click it
                btn.click();
                event.keyCode = 0
            }
        }
    }

    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
</script>
<%-- Note This following script is to handle the what is this? link in group affiliations popup control. Soince it was not triggering document .ready() in the popup control, it is writtern in the containing control --%>
<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupStartDate);

    });

    function setupStartDate() {

        $("#divRequestedEffectiveDateInfo1").hide();
        $(".what-is-this-link1").mouseover(function () {
            $("#divRequestedEffectiveDateInfo1").show();
        });

        $("#divRequestedEffectiveDateInfo1").mouseleave(function () {
            $("#divRequestedEffectiveDateInfo1").hide();
        });
    }
    function exportPopup() {
        var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
        if (rowCount > 10000) {
            $("#divRowCount").dialog();
        }
        return true;
    }
</script>
<script>
    function GoToHistory(event, redirectionType) {

        event.preventDefault()
        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
        var regId = $("[id*=hdnRegId]").val();
        var redirectionUrl = urlpath + "/GroupToIndividualAffiliationHistoryDetails.aspx?regId=" + regId;
        window.location.href = redirectionUrl;
    }
</script>

<%-- NOTE: There is a bug in asp.net 4.0 that makes it so ImageButtons fail in IE10 when they are in an
    UpdatePanel. Do not uncomment this UpdatePanel without addressing this bug!  Best replacing ImageButtons
    with LinkButtons containing Images. --%>
<asp:UpdatePanel ID="updSuccess" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
		<asp:ValidationSummary ID="GroupAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="GroupAffiliations" />
        <asp:Label ID="updateMsg" runat="server" Text="Individual Provider’s enrollment profile was successfully updated." ForeColor="Red" BackColor="#ffff00" Visible="false"></asp:Label><br />
    </ContentTemplate>
</asp:UpdatePanel>
<span class="pageHeader" id="Header1" runat="server">Individual Providers Associated with Your Group</span>
<span class="pageHeader2" id="Header2" runat="server">Physician Assistants Associated with Your Practice</span>
<span class="SectionHeader" id="Header3" runat="server">PCA Aides Associated with Your Practice</span>


<asp:UpdatePanel ID="upGA_Main" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="AffiliationInfo">
            <p id="Group" runat="server">In the table below, enter or confirm each individual provider that is associated with your group.For active affiliations, click on the edit icon to make changes to the individual provider’s record.</p>
            <p id="PA" runat="server"><i>In the table below, please enter or confirm each Physician Assistant that are supervising within your Practice. Note that the Physician Assistant must be enrolled and active in DC Medicaid before being confirmed as a part of your practice.</i></p>
            <p id="PCA" runat="server"><i>In the table below, please enter or confirm each PCA Aide that are supervising within your Practice. Note that the PCA Aide must be enrolled and active in DC Medicaid before being confirmed as a part of your practice.</i></p>
            <p id="paraVerifyIndividual" runat="server" visible="true">
                <b>Note: </b>
                <span style="color: #D10000;">If the affiliation status displays as ‘Individual Enrollment Pending Approval’ or as ‘Individual Requires Revalidation’, the individual provider must create an account in PNM and complete their application for enrollment or re-validation.</span>
                <br />
                <br />
                <span style="color: #D10000;">Always verify that NPI you enter for Individuals are correct.</span>
            </p>
            <div class="divGrid">
                <div class="row">
                    <div class="col-sm-3  text-right">
                        <asp:Label ID="lblName" runat="server" CssClass="formLabel200" Text="Display Active Only"></asp:Label>
                    </div>
                    <div id="divContactName" class="col-sm-9" runat="server">
                        <asp:RadioButtonList ID="rblGridActiveStatusID" runat="server" CssClass="QstRadioList" TextAlign="Right" RepeatDirection="Horizontal" OnSelectedIndexChanged="onAffiliationViewChange" AutoPostBack="true">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0" Selected="true"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <asp:GridView runat="server" Width="100%" ID="grdGroupAffiliations"
                    AutoGenerateSelectButton="false" AutoGenerateColumns="False"
                    CssClass="gridViewSmallFont"
                    EmptyDataText="No affiliations found."
                    AllowSorting="true"
                    CurrentSortField="Name"
                    CurrentSortDirection="ASC"
                    AllowPaging="true" AllowCustomPaging="true"
                    ShowHeaderWhenEmpty="true"
                    DataKeyNames="REG_AFFILIATION_ID,SSN,NPI,RETRO_REVIEW_REQUIRED_ID"
                    OnSorting="grdGroupAffiliations_Sorting" OnRowDataBound="grdGroupAffiliations_RowDataBound"
                    OnRowCommand="grdGroupAffiliations_RowCommand" OnPageIndexChanging="grdGroupAffiliations_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnNameLink" runat="server" CommandName="LinkName" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text='<%# Eval("NAME") %>' />
                                <asp:Label ID="lblNameLink" runat="server" Text='<%# Eval("NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                        <asp:BoundField DataField="ProviderType" HeaderText="Provider Type" SortExpression="ProviderType" />
                        <asp:BoundField DataField="SPECIALTY_TYPE_NAME" HeaderText="Specialty Type" SortExpression="SPECIALTY_TYPE_NAME" />
                        <asp:BoundField DataField="START_DATE" HeaderText="Start Date" SortExpression="START_DATE" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="END_DATE" HeaderText="End Date" SortExpression="END_DATE" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="AffiliationStatus" HeaderText="Affiliation Status" SortExpression="AffiliationStatus" />
                        <asp:BoundField DataField="RevalidationDate" HeaderText="Revalidation Due Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                        <asp:BoundField DataField="AddressLocation" HeaderText="Rendering Location" SortExpression="AddressLocation" />
                       
                         <asp:TemplateField HeaderText="Directory OptOut" SortExpression="DirectoryOptOut">
                            <ItemTemplate>
                                 <asp:Label ID="DirectoryOptOut"  runat="server" CssClass='<%# bool.Parse(Eval("DirectoryOptOut").ToString()).ToString()== "True" ? "ui-icon ui-icon-check":"" %>' 
                                     Text='<%# bool.Parse(Eval("DirectoryOptOut").ToString()).ToString()== "True" ? "Yes":"" %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="GroupAffiliations" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Edit">
                                    <asp:Image ID="imgEdit"  ImageUrl="~/Images/edit.png" Alt="Edit" runat="server" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteAffiliation" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Delete Association">
                                    <asp:Image ID="imgDel" ImageUrl="~/Images/cancel.png" Alt="Delete" runat="server" BorderStyle="None" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reg ID">
                            <ItemTemplate>
                                <asp:HiddenField Value='<%#Eval("REG_ID")%>' ID="HdnIndividualRegId" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
            <div class="divHistoryAndAdd">
                <asp:ImageButton ID="btnAdd" runat="server" AlternateText="Add" ImageUrl="~/Images/add.png" CommandName="GroupAffiliations" OnCommand="btnAdd_Click" ToolTip="Add" /><br />
                <div id="hstryNewdiv" style="text-align: right; padding-top: 15px;">
                    <asp:HiddenField ID="hdnRegId" runat="server" />
                    <a href="javascript:void(0);" class="buttonBoxFocus hbtn-focus" onclick="GoToHistory(event);" title="History"><img src="../Images/HistoryIcon1.png" alt="History Icon" class="history-icon" />History</a>
                </div>
            </div>
            <div>
                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="formLabelAuto" OnClick="DisplayTenCN">Display 10 </asp:LinkButton>
                <span class="formLabelAuto">| </span>
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="formLabelAuto" OnClick="DisplayFiftyCN">Display 50 </asp:LinkButton>
                <span class="formLabelAuto">| </span>
                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="formLabelAuto" OnClick="DisplayHundredCN">Display 100 </asp:LinkButton>
                <br />
                <span class="formLabelAuto">Total Count: </span>
                <asp:Label runat="server" ID="hdnRowCount2" Text="0" CssClass="formLabelAuto" />
            </div>
        </div>
         <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" BackgroundCssClass="modalBackground" />
         <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; height: auto; width: 40%;">
             <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                 <div class="popTitle">
                     <asp:Label ID="lblTitle" CssClass="bodyTextBold groupMemberTitle" runat="server" Text="Title" />
                 </div>
             </asp:Panel>
             <asp:Panel ID="pnlMain" runat="server">
                 <asp:MultiView ID="mltPopup" runat="server">
                     <asp:View ID="vwGroupAffiliations" runat="server">
                         <div style="text-align: left; padding: 15px" class="container-fluid">
                             <div class="row">
                                 <uc:GroupAffiliations ID="ucGroupAffiliations" runat="server" />
                             </div>
                         </div>
                     </asp:View>
                 </asp:MultiView>
             </asp:Panel>
         </asp:Panel>
         <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
    </ContentTemplate>
</asp:UpdatePanel>

        <br />
        <div>
            <h1> <Legend style="border-bottom:2px solid #65659f;"><span class="pageHeader" id="Header4">Affiliate Downloads</span></Legend> </h1>
            <div style="width: 100%;">
                <asp:HiddenField ID="hdnRowCount" runat="server" />

                
                <telerik:RadGrid ID="RadGridExportSecSpec" runat="server" Visible="true" Skin="PDMSModern" EnableEmbeddedSkins="false">
                    <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                        <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Affiliate Specialties">
                            <PageFooter>
                                <RightCell Text="Page <?page-number?>" />
                            </PageFooter>
                        </Pdf>
                    </ExportSettings>
                    <MasterTableView AutoGenerateColumns="false">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="col1" HeaderText="Affiliate Name" DataField="NAME"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="col2" HeaderText="Medicaid ID" DataField="MEDICAID_ID"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="col3" HeaderText="NPI" DataField="NPI"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="col4" HeaderText="Provider Type" DataField="PROVIDER_TYPE_NAME"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="col5" HeaderText="Primary Specialty" DataField="PRIMARY_SPECIALTY_TYPE_NAME"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="col6" HeaderText="Secondary Specialties" DataField="SECONDARY_SPECIALTIES"></telerik:GridBoundColumn>                            
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <div class="row">
                    <div class="col-sm-3 text-left"><b>Full affiliate roster download: </b> </div>
                    <div class="col-sm-8 text-left">
                        <telerik:RadGrid ID="RadGridExportAffiliations" runat="server" Visible="true" Skin="PDMSModern" EnableEmbeddedSkins="false">
                            <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                                <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Affiliate Search">
                                    <PageFooter>
                                        <RightCell Text="Page <?page-number?>" />
                                    </PageFooter>
                                </Pdf>
                            </ExportSettings>
                            <MasterTableView AutoGenerateColumns="false">
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="col1" HeaderText="Name" DataField="Name"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col2" HeaderText="NPI" DataField="NPI"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col3" HeaderText="Provider Type" DataField="ProviderType"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col4" HeaderText="Specialty Type" DataField="SPECIALTY_TYPE_NAME"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col5" HeaderText="Start Date" DataField="START_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col6" HeaderText="End Date" DataField="END_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col7" HeaderText="Affiliation Status" DataField="AffiliationStatus"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col8" HeaderText="Revalidate Due Date" DataField="RevalidationDate" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col9" HeaderText="Medicaid ID" DataField="MedicaidID"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="col10" HeaderText="Rendering Location" DataField="AddressLocation"></telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="return exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="XLS" /></asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="return exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
                    </div>                                      
                </div>
                 <br /> 
                <i>This file contains only primary specialties for affiliated providers. To view secondary specialties, refer to the Affiliate specialty download. </i>
                <br /> <br />   
                <div class="row">
                    <div class="col-sm-3 text-left"><b>Affiliate Specialty download: </b> </div>
                    <div class="col-sm-8 text-left">
                        <asp:LinkButton ID="lnkExcel1" runat="server" ToolTip="Excel" OnClick="lnkExcel1_Click" OnClientClick="return exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="XLS" /></asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPDF1" runat="server" ToolTip="PDF" OnClick="lnkPDF1_Click" OnClientClick="return exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
                    </div>                                    
                </div>
                 <br /> 
                <i>The specialty download will only return all active specialties for date of download. For a more comprehensive search, use the left hand menu to navigate to Provider Specialty Search. </i>
                        <%--<div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
                    <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
                </div>--%>
            </div>
         </div>
        <br />
        <asp:Panel ID="pnlProviderSearch" runat="server" DefaultButton="btnFilterGrid">
            <h1> <Legend style="border-bottom:2px solid #65659f;"><span class="pageHeader" id="Span1">Affiliate Search</span></Legend> </h1>
            <i>Partial or Full search using Name and/or NPI. When both fields are used to search, the grid will be filtered by both Name and NPI.</i>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="Label1" runat="server" CssClass="formLabel200" Text="Display Active Only"></asp:Label>
                </div>
                <div id="div1" class="col-sm-9" runat="server">
                    <asp:RadioButtonList ID="rblAffiliationSearch" runat="server" CssClass="QstRadioList" TextAlign="Right" RepeatDirection="Horizontal" OnSelectedIndexChanged="onActiveChanged" AutoPostBack="true" >
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0" Selected="true"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="Label2" runat="server" CssClass="formLabel200" Text="Name"></asp:Label>
                </div>
                <div id="div2" class="col-sm-9" runat="server">
                    <asp:TextBox ID="txtFilterName1" runat="server" aria-label="Filtername" CssClass="formField formField" MaxLength="100" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="Label3" runat="server" CssClass="formLabel200" Text="Affiliation Status"></asp:Label>
                </div>
                <div id="div3" class="col-sm-9" runat="server">
                    <asp:DropDownList ID="ddlAffiliationStatus" aria-label="AFFILIATE STATUS" runat="server" CssClass="formDropDown">
                        <asp:ListItem Text="" Value="" Selected="True" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <asp:Label ID="Label4" runat="server" CssClass="formLabel200" Text="NPI"></asp:Label>
                </div>
                <div id="div4" class="col-sm-9" runat="server">
                    <asp:TextBox ID="txtFilterNPI" runat="server" aria-label="nPI" CssClass="formField formField" MaxLength="10" onKeyUp="javascript:numericOnly(this);" />
                </div>
            </div>
            <div class="btnBox btnBoxCenter">
                <asp:Button ID="btnFilterGrid" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnFilterGrid_Click" CausesValidation="false" Style="margin-bottom: 5px" />
                <asp:Button ID="btnClearFilter" runat="server" Text="Clear" CssClass="buttonBoxFocus" OnClick="btnClearFilter_Click" CausesValidation="false" />
            </div>
            <div style="width: 300px; margin-left: auto; margin-right: auto; display: none;">
                <p>
                    <i>
                        <asp:Literal ID="ltlMatchFound" runat="server" Text=" <%$ Resources:BrandingResource , AFFILIATION_MATCH_FOUND %>" /></i>
                </p>
            </div>
        </asp:Panel>
        <br />
        <br />
        <asp:UpdateProgress runat="server" ID="upGA_MainSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upGA_Main">
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgSaving" AlternateText="Loading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress><br />
        <h1> <Legend style="border-bottom:2px solid #65659f;"><span class="pageHeader" id="Span2">Affiliation Status Definitions</span></Legend> </h1>        
        <asp:Repeater ID="rptStatusDef" runat="server">
            <HeaderTemplate>
                <table class="legend" border="0">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <b>
                            <asp:Label runat="server" ID="lblStatusName" Text='<%# DataBinder.Eval(Container.DataItem, "[DESCRIPTION]") %>' />&nbsp;-</b>
                        <asp:Label runat="server" ID="lblStatusDesc" Text='<%# DataBinder.Eval(Container.DataItem, "[LONG_DESCRIPTION]") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
       
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
