<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceTerminalIllnessDiagnosisSearch" Codebehind="HospiceTerminalIllnessDiagnosisSearch.ascx.cs" %>
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
        padding: 10px
    }
</style>
<script type="text/javascript">


    $(function () {
        //$("[id*=btnSearch]").click(function () {
        //    debugger;
        //var post_data = JSON.stringify({ "code": $('#txtDiagnosisCode').val(), "icdVersion": $('#ddlICDVersion').val(), "diagnosisDes": $('#txtDiagnosisCodeDescription').val()});

        var clickHandler = function (e) {
            var code = $('#<%=txtDiagnosisCode.ClientID%>').val();
            var icdVersion = $('#<%=ddlICDVersion.ClientID%>').val();
            var diagnosisDes = $('#<%=txtDiagnosisCodeDescription.ClientID%>').val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: 'GET',
                url: WebApiPA + 'GetICDDiagnosis?code=' + code + "&&icdVersion=" + icdVersion + "&&diagnosisDes=" + diagnosisDes,
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
                    $("[id*=gvHospiceTerminalIllnessDiagnosisSearch]").append("<tr><th>Diagnosis Code</th><th>ICD Version</th><th>Diagnosis Code Description</th></tr>")
                    if (r.length > 0) {
                        for (var i = 0; i < r.length; i++) {
                            $("[id*=gvHospiceTerminalIllnessDiagnosisSearch]").append("<tr><td>" + r[i].ICD10Diag + "</td><td>" + r[i].ICDVersion + "</td><td>" + r[i].DiagDesc + "</td></tr>");
                        }
                        //
                    }
                    else
                        $("[id*=gvHospiceTerminalIllnessDiagnosisSearch]").append("<tr><td>No records found</td></tr>");
                    var state = $($.parseXML(r)).find("STATE");
                    //var row = $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:last-child").clone(true);
                    //$("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr").not($("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:first-child")).remove();
                    //$.each(state, function () {
                    //    debugger;
                    //    $("td", row).eq(0).html($(this).find("ICD10Diag").text());
                    //    $("td", row).eq(1).html($(this).find("ICDVersion").text());
                    //    $("td", row).eq(2).html($(this).find("DiagDesc").text());
                    //    $("[id*=gvHospiceTerminalIllnessDiagnosisSearch]").append(row);
                    //    row = $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:last-child").clone(true);
                    //});
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
        $("[id*=btnSearch]").one('click', clickHandler);
    });
    //function OnSuccess(r) {
    //    debugger;        

    //}
</script>
<%--<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>--%>

<div class="row" style="text-align: center;">
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><span style="color: red">*</span> <b>Diagnosis Code</b>
            <asp:TextBox ID="txtDiagnosisCode" MaxLength="7" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><span style="color: red">*</span><b>ICD Version </b>
            <asp:DropDownList ID="ddlICDVersion" CssClass="ohio-field-label" runat="server">
                <asp:ListItem Value="ICD 10" Text="ICD 10"></asp:ListItem>
                <asp:ListItem Value="ICD 9" Text="ICD 9"></asp:ListItem>
            </asp:DropDownList>
        </span>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><span style="color: red">*</span> <b>Diagnosis Code Description</b>
            <asp:TextBox ID="txtDiagnosisCodeDescription" MaxLength="400" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <%--<asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus"  Style="background-color: darkslateblue !important" />--%>
        <input type="button" id="btnSearch" value="Search" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
    </div>
</div>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

<mms:SortablePagingGridView
    ID="gvHospiceTerminalIllnessDiagnosisSearch"
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

 
  
