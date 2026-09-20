<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceHLTCFProviderSearch, App_Web_av5ll3zk" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<script type="text/javascript">
    function SearchProviderinfo() {
        debugger;
        $('#gvHospiceHLTCFProviderSearch').empty();
        var npi = $('#txtNPI').val();
        var medicaidid = $('#txtMedicaidID').val();
        var lastName = $('#txtBusinessLastName').val();
        var firstName = $('#txtFirstName').val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiHospice + "Search?npi=" + npi + "&&medicaidid=" + medicaidid + "&&lastName=" + lastName + "&&firstName=" + firstName,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (result) {
                $("#gvHospiceHLTCFProviderSearch").append("<tr style='background-color:red; color:white; font-weight:bold;'><td style='text-align:left;'>NPI</td><td style='text-align:left;'>Medicaid ID</td><td style='text-align:left;'>Business/Last Name</td><td style='text-align:left;'>First Name</td></tr>");
                for (var i = 0; i < result.length; i++) {
                    if (i % 2 == 0) {
                        $("#gvHospiceHLTCFProviderSearch").append("<tr style='background-color:#F5FBEF; font-family:Verdana; font-size:10pt ;'><td style='text-align:left;'>" + result[i].NPI + "</td><td style='text-align:left;'>" + result[i].MEDICAID_ID + "</td><td style='text-align:left;'>" + result[i].LAST_NAME + "</td><td style='text-align:left;'>" + result[i].FIRST_NAME + "</td></tr>");
                    } else {
                        $("#gvHospiceHLTCFProviderSearch").append("<tr style='background-color:skyblue; font-family:Verdana; font-size:10pt ;'><td style='text-align:left;'>" + result[i].NPI + "</td><td style='text-align:left;'>" + result[i].MEDICAID_ID + "</td><td style='text-align:left;'>" + result[i].LAST_NAME + "</td><td style='text-align:left;'>" + result[i].FIRST_NAME + "</td></tr>");
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            }
        });
    }

</script>
<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>

<div class="row" style="text-align: center;">
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><b>NPI</b>
            <asp:TextBox ID="txtNPI" CssClass="ohio-field-input" runat="server" MaxLength="10">
            </asp:TextBox>
        </span>

    </div>

    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><b>Medicaid ID</b>
            <asp:TextBox ID="txtMedicaidID" CssClass="ohio-field-input" runat="server" MaxLength="7">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><b>Business/Last Name</b>
            <asp:TextBox ID="txtBusinessLastName" CssClass="ohio-field-input" runat="server" MaxLength="70">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><b>First Name</b>
            <asp:TextBox ID="txtFirstName" CssClass="ohio-field-input" runat="server" MaxLength="35">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <asp:Button ID="btnSearch" runat="server" OnClientClick="javascript:SearchProviderinfo();" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" Style="background-color: darkslateblue !important" />
        <%--  <input type="button" id="btnSearch" value="Search" class="buttonBoxFocus"  Style="background-color: darkslateblue !important" />--%>
    </div>
</div>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
<asp:GridView ID="gvHospiceHLTCFProviderSearch" runat="server" CellPadding="4" ShowHeaderWhenEmpty="True" BackColor="White" GridLines="Both" BorderColor="#CC9966" BorderStyle="None" Width="90%" BorderWidth="1px" HorizontalAlign="Center">
    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
    <RowStyle BackColor="White" ForeColor="#330099" />
    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
    <SortedAscendingCellStyle BackColor="#FEFCEB" />
    <SortedAscendingHeaderStyle BackColor="#AF0101" />
    <SortedDescendingCellStyle BackColor="#F6F0C0" />
    <SortedDescendingHeaderStyle BackColor="#7E0000" />
</asp:GridView>
<%--<mms:SortablePagingGridView
    ID="gvHospiceHLTCFProviderSearch"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnPageIndexChanging="gvHospiceHLTCFProviderSearch_PageIndexChanging"
    OnSorting="gvHospiceHLTCFProviderSearch_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="NPI" GridViewSortDirection="Ascending"
    DataKeyNames="MEDICAID_ID, LAST_NAME,FIRST_NAME">
    <Columns>

        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
        <asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
        <asp:BoundField DataField="LAST_NAME" HeaderText="Business/Last Name"   SortExpression="BusinessLastName" />
        <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" SortExpression="FirstName" />
       
    </Columns>
</mms:SortablePagingGridView>--%>