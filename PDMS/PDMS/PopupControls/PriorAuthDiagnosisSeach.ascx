<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PriorAuthDiagnosisSeach" Codebehind="PriorAuthDiagnosisSeach.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<style>
    table.gridViewSmallFont th {
    background-color: #545487;
    text-align: left;
    color: white;
    border: 1px solid #fff !important;
    padding: 10px;
</style>
<script type="text/javascript">


    $(function () {
       

        var clickHandler = function (e) {
            var code = $('#<%=txtDiagnosisCode.ClientID%>').val();
            var icdVersion = $('#<%=ddlICDVersion.ClientID%>').val();
            var diagnosisDes = $('#<%=txtDiagnosisCodeDesc.ClientID%>').val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: 'GET',
                url: webApiPA + 'GetICDDiagnosis?code=' + code + "&&icdVersion=" +icdVersion + "&&diagnosisDes" + diagnosisDes,
                //data: JSON.stringify(data),
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: 'application/json;charset=utf-8',
                dataType: 'json',
                success: function (r) {
                    debugger;                    
                     $("[id*=gvPriorAuthSearchDiagnosis]").append("<tr><th>Diagnosis Code</th><th>ICD Version</th><th>Diagnosis Code Description</th></tr>") 
                    for (var i = 0; i < r.length; i++) {
 $("[id*=gvPriorAuthSearchDiagnosis]").append("<tr><td>" + r[i].ICD10Diag + "</td><td>" + r[i].ICDVersion + "</td><td>" + r[i].DiagDesc + "</td></tr>");
}
                  
                },
                failure: function (r) {
                    debugger;
                    alert(r);
                },
                error: function (response) {
                    debugger;
                    alert(r);
                }
            });
            e.stopImmediatePropagation();
            return false;
        }
        $("[id*=lnkSearchDiagnosis]").one('click', clickHandler);
    });
    //function OnSuccess(r) {
    //    debugger;        

    //}
</script>
<ajax:Accordion ID="PriorAuthSearchDiagnosis" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthSearchDiagnosis" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblPriorAuthDiagnosisSearch" class="expandcollapse" runat="server" Text="- Diagnosis Search"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                      <div class="row" style="text-align: center;">
                  <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red"></span> <b>Diagnosis Code </b>
                           <asp:TextBox ID="txtDiagnosisCode" runat="server" CssClass="formField" />
                            <asp:LinkButton ID="lnkSearchDiagnosis" runat="server" ToolTip="Search" CommandName="DiagnosisSearch" OnClick="lnkDiagnosisSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red">*</span><b>ICD Version</b>
                              <asp:DropDownList ID="ddlICDVersion" CssClass="formField" runat="server">
                        <%-- <asp:DropDownList ID="ddlICDVersion" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlICDVersion_SelectedIndexChanged">--%>
                             <asp:ListItem Value="ICD 10" Text="ICD 10"></asp:ListItem>
                                <asp:ListItem Value="ICD 9" Text="ICD 9"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="reqICDVersion" SetFocusOnError="true"
                                ValidationGroup="vgCarePlan" ControlToValidate="ddlICDVersion" ErrorMessage="*ICD Version" Text="*" Display="Dynamic" InitialValue="0" />
                            <span style="color:red; display:none"><br />ICD Version is required</span>
                        </span>
                    </div>
                    
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><b>Diagnosis Code Description </b>
                            <asp:TextBox ID="txtDiagnosisCodeDesc" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000"  />
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="DiagnosissearchAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <%--                        <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />--%>
                    </div>
                </div>
                   <%-- <asp:GridView ID="gvPriorAuthSearchDiagnosis" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthSearchDiagnosis_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvPriorAuthSearchDiagnosis_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                          
                            <asp:BoundField DataField="DiagnosisCode" HeaderText="Diagnosis Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="ICDType" HeaderText="ICD Version" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="DiagnosisTypeCode" HeaderText="Diagnosis Code Description" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                          <%--  <asp:BoundField DataField="" HeaderText="Search" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />--%>
                          <%--  <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"
                                        CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>--%>
                </div>
              
            </Content>

        </ajax:AccordionPane>
    </Panes>
   
</ajax:Accordion>
 <%--<div class="col-sm-6 col-md-4 col-lg-3 ">
      
        <input type="button" id="btnSearch" value="Search" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
    </div>
</div>--%>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

<mms:SortablePagingGridView
    ID="gvPriorAuthSearchDiagnosis"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
     ShowHeaderWhenEmpty="true" 
    EmptyDataText=" "   
    RowStyle-VerticalAlign="Top"
    AlternatingRowStyle-BackColor="White" GridLines="Horizontal"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="ICD10Diag" GridViewSortDirection="Ascending">
    <Columns>

        <asp:BoundField DataField="ICD10Diag" HeaderText="Diagnosis Code" SortExpression="ICD10Diag" />
        <asp:BoundField DataField="ICDVersion" HeaderText="ICD Version" SortExpression="ICDVersion" />
        <asp:BoundField DataField="DiagDesc" HeaderText="Diagnosis Code Description" SortExpression="DiagDesc" />

    </Columns>
</mms:SortablePagingGridView>





<%--<asp:UpdatePanel ID="upPriorAuthDiagnosisSearch" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 1200px;">
                <asp:ValidationSummary ID="vsPriorAuthDiagnosisSearch" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="PriorAuthDiagnosisSearch" />
                <div class="wdAuto">

                    <div class="row Diagnosis" runat="server">
                        
                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblDiagnosisCode" runat="server" Text="Diagnosis Code" CssClass="formLabel200" />
                            <asp:TextBox ID="txtDiagnosisCode" runat="server" CssClass="formField" />
                            <asp:LinkButton ID="lnkDiagnosisSearch" runat="server" ToolTip="Search" OnClick="lnkDiagnosisSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
                        </div>
                         <div class="col-sm-3  text-left">
                            <asp:Label ID="lblICDVersion" runat="server" Text="ICD Version" CssClass="formLabel200" />
                            <asp:DropDownList ID="ddlICDVersion" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlICDVersion_SelectedIndexChanged">
                              
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvICDVersion" SetFocusOnError="true"
                                ValidationGroup="vgICDVersion" ControlToValidate="ddlICDVersion" ErrorMessage="*" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>

                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblDiagnosisCodeDesc" runat="server" Text="Diagnosis Code Description" CssClass="formLabel200" />
                            <asp:TextBox ID="txtDiagnosisCodeDesc" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />
                        </div>
                    </div>

                </div>
                <asp:UpdateProgress runat="server" ID="upPriorAuth" DisplayAfter="0" AssociatedUpdatePanelID="upPriorAuthDiagnosisSearch">
                    <ProgressTemplate>
                        <div class="loading">
                            <asp:Image ID="imgPriorAuthDiagnosisSearch" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                           <asp:LinkButton ID="lnkDiagnosisSearch" runat="server" ToolTip="Search" OnClick="lnkDiagnosisSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                    </tr>
                </table>
            </div>

        </asp:Panel>
        
    </ContentTemplate>

</asp:UpdatePanel>--%>
