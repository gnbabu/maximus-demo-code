<%@ Control Language="C#" AutoEventWireup="true" Inherits="Views_ProviderOperatorView" Codebehind="ProviderOperatorView.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script type="text/javascript" src="//www.google.com/jsapi"></script>
<script type="text/javascript">
    
    google.load("visualization", "1", { packages: ["corechart", "table"], "callback": Pardrawchart });

    function Pardrawchart() {
        $(document).ready(function () {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'ProviderOperatorHome.aspx/GetData',
                // data: "{'charttype':" + (charttype) + "}",
                data: '{}',
                success:
                    function (response) {
                        drawChart(response.d);
                    }
            });
        })
    }

        var firstClick = 0; var chk = "";

        var secondClick = 0;

        $(window).load(function () {
            Pardrawchart();
 
        })
    


        function drawChart(dataValues) {


            var lookup = {};
            var items = dataValues;
            var result = [];

            for (var item, i = 0; item = items[i++];) {
                var Category = item.Category;

                if (!(Category in lookup)) {
                    lookup[Category] = 1;
                    result.push(Category);
                }
            }




            //Streamlined
            var name = "";
            var i;
            var checked = "checked"
            for (i = 0; i < result.length; i++) {


                var element = document.getElementsByName(result[i]);



                if (typeof element !== "undefined" && element == '') {
                    if (element.checked) {
                        checked = "checked"
                    }
                    else {
                        checked = ""
                    }

                }

                chk += '<input id="' + result[i] + '" type="checkbox" name="' + result[i] + '" value="' + result[i] + '" ' + checked + ' onclick="this.form.submit()";>';
                chk += '<label for="' + result[i] + '"> ' + result[i] + '</label>';
                $('#someId').html(chk);


            }




            if (typeof (name) !== "undefined") {
                var updateData = [];
                for (var i = 0; i < dataValues.length; i++) {
                    var category = dataValues[i].Category;
                    if (category != name) {
                        updateData.push(dataValues[i]);
                    }
                }
            }
            dataValues = updateData;

            var data = new google.visualization.DataTable();


            // debugger;
            data.addColumn('string', 'Work Items');
            data.addColumn('string', 'Status');
            data.addColumn('number', 'Count');
            for (var i = 0; i < dataValues.length; i++) {
                data.addRow([dataValues[i].Category, dataValues[i].ColumnName, dataValues[i].Value]);
            }

            var aggregateData = google.visualization.data.group(data, [0], [{
                type: 'number',
                label: 'Count',
                column: 2,

                aggregation: google.visualization.data.sum
            }]);

            var topLevel = true;
            

            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));



            var options = {
                height: 338,
                width: 787
            };


            function draw(category) {
                if (topLevel) {
                    // rename the title
                    options.title = 'Work Items';
                    // draw the chart using the aggregate data


                    chart.draw(aggregateData, options);

                    var table = new google.visualization.Table(document.getElementById('table_div'));
                    //table.draw(aggregateData, { showRowNumber: true, width: '100%', height: '100%' });


                }
                else {
                    var view = new google.visualization.DataView(data);
                    // use columns "Name" and "Value"
                    view.setColumns([1, 2]);


                    // filter the data based on the category
                    view.setRows(data.getFilteredRows([{ column: 0, value: category }]));
                    // rename the title
                    options.title = 'Work Item: ' + category;
                    // draw the chart using the view
                    chart.draw(view, options);

                    var table = new google.visualization.Table(document.getElementById('table_div'));
                    table.draw(view, { showRowNumber: true, width: '100%', height: '100%' });
                }
            }

            google.visualization.events.addListener(chart, 'select', function () {

                var date = new Date();
                var millis = date.getTime();




                if (topLevel) {
                    var selection = chart.getSelection();
                    // drill down if the selection isn't empty
                    if (selection.length) {
                        var category = aggregateData.getValue(selection[0].row, 0);
                        topLevel = false;
                        draw(category);
                    }
                }
                else {
                    // go back to the top
                    topLevel = true;
                    draw();
                }
            });
            draw();
        }
</script>
<div>
    <div class="gridCaption">My Dashboard</div>
    <table role="presentation"style="width:100%;">
        <tr>
            <td>
                <asp:GridView runat="server" Width="100%" ID="gvMyDashboard"
                    AutoGenerateSelectButton="false"
                    AutoGenerateColumns="False"
                    CssClass="gridViewSmallFont"
                    EmptyDataText="No providers found."
                    ShowHeaderWhenEmpty="true"
                    AllowPaging="false" AllowSorting="false"
                    OnRowCommand="gvMyDashboard_RowCommand"
                    OnSelectedIndexChanged="gvMyDashboard_SelectedIndexChanged"
                    DataKeyNames="PAPER_REQUEST_TYPE_ID">
                    <Columns>

                        <asp:BoundField DataField="PAPER_REQUEST_TYPE_NAME" HeaderText="WorkItem" />
                        <asp:TemplateField HeaderText="Assigned">
                            <ItemTemplate>
                                <asp:LinkButton ToolTip="Status" ID="btnAssigned" runat="server" CommandName="SelectRow"
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("ASSIGNED") %>' />
                                <asp:Label ID="lblAssigned" runat="server" Text='<%# Eval("ASSIGNED") %>' Visible="false"></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:BoundField DataField="UNASSIGNED" HeaderText="UnAssigned" />
                    </Columns>
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <SelectedRowStyle CssClass="gridViewSelected" />
                    <EditRowStyle CssClass="gridViewSelected" />
                    <EmptyDataRowStyle CssClass="gridViewRow" />
                </asp:GridView>
            </td>
            <td runat="server" id="tdMyDashGraph">
                <div id ="chart_div"></div>             
            </td>
            <td runat="server" id="tdMyDashGraphTable">
                <div id="table_div"> </div>
            </td>
        </tr>
    </table>

    <br />
    <asp:Label ID="lblInformationalMessage" runat="server" Text=""></asp:Label>
    <div class="gridCaption">My Providers</div>
    <mms:SortablePagingGridView runat="server" Width="100%" ID="gvMyProviders"
        AutoGenerateSelectButton="false" AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" PageSize="15"
        EmptyDataText="No providers found."
        GridViewSortColumn="Aging" GridViewSortDirection="Ascending" AllowPaging="true"
        ShowHeaderWhenEmpty="true"
        OnRowDataBound="gvMyProviders_RowDataBound"
        OnPageIndexChanging="gvMyProviders_PageIndexChanging"
        OnSorting="gvMyProviders_Sorting"
        OnRowCommand="gvMyProviders_RowCommand"
        OnSelectedIndexChanged="gvMyProviders_SelectedIndexChanged"
        DataKeyNames="PaperRequestQueueID,UserID, RegID">
        <Columns>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:LinkButton ToolTip="Status" ID="btnManage" runat="server" CommandName="SelectRow"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("CurrentStatusType") %>' />
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("CurrentStatusType") %>' Visible="false"></asp:Label>
                </ItemTemplate>

            </asp:TemplateField>
            <asp:BoundField DataField="ProviderName" HeaderText="Name" SortExpression="ProviderName" />
            <asp:BoundField DataField="PaperRequestTypeName" HeaderText="Request Type" SortExpression="PaperRequestTypeName" />
            <asp:BoundField DataField="TaxID" HeaderText="Tax ID" SortExpression="TaxID" />
            <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
            <asp:BoundField DataField="SpecialtyTypeName" HeaderText="Specialty" SortExpression="SpecialtyTypeName" />
            <asp:BoundField DataField="PracticeLocationZip" HeaderText="Location" SortExpression="PracticeLocationZip" />
            <asp:BoundField DataField="Aging" HeaderText="Aging" SortExpression="Aging" />
        </Columns>
    </mms:SortablePagingGridView>
    <br />
    <div class="btnBox">
        <asp:Button ID="btnNext" runat="server" Text="Get Next In Queue" CssClass="buttonBox wd200" ToolTip="Get Next Unassigned Paper Request" OnClick="btnNext_Click" />
    </div>
    <br />
    <br />
    <br />
</div>
