<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_MySiteVisitQueue" Codebehind="MySiteVisitQueue.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    My Queue
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script type="text/javascript" src="//www.google.com/jsapi"></script>
    <script  type="text/javascript">
        function CheckPhoneLength(sender, args) {
            var re = /\D/g; // Remove any characters that are not numbers
            var test = args.Value.replace(re, "");
            if (test == "") return true;

            var len = test.length;
            if (len != 10)
                args.IsValid = false;
            if (test[0] == 0 || test[0] == 1 || test[3] == 0 || test[3] == 1)
                args.IsValid = false;
            return;
        }

        function CheckSSNLength(sender, args) {
            var re = /\D/g; // Remove any characters that are not numbers
            var test = args.Value.replace(re, "");
            if (test == "") return true;

            var len = test.length;
            if (len != 9)
                args.IsValid = false;
            return;
        }

        google.load("visualization", "1", { packages: ["corechart", "table"], "callback": Pardrawchart });

        function Pardrawchart() {
            $(document).ready(function () {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    url: 'MySiteVisitQueue.aspx/GetData',
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
                height: 338
                , width: 900
                , backgroundColor: '#F7F7F7'
            };


            function draw(category) {
                if (topLevel) {
                    // rename the title
                   // options.title = 'Work Items Summary';
                    // draw the chart using the aggregate data
                    chart.draw(aggregateData, options);

                    //var table = new google.visualization.Table(document.getElementById('table_div'));
                    //table.draw(aggregateData, { showRowNumber: true, width: '100%', height: '100%' });


                }
                else {
                    var view = new google.visualization.DataView(data);
                    // use columns "Name" and "Value"
                    view.setColumns([1, 2]);


                    // filter the data based on the category
                    view.setRows(data.getFilteredRows([{ column: 0, value: category }]));
                    // rename the title
                    //options.title = 'Work Item: ' + category;
                    // draw the chart using the view
                    chart.draw(view, options);

                    //var table = new google.visualization.Table(document.getElementById('table_div'));
                    //table.draw(view, { showRowNumber: true, width: '100%', height: '100%' });
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
       <style type="text/css">
            .col-sm-9 {
                width:74% !important;
            }
        </style>
    <div class="WhiteBox">
    <br />
    <asp:Panel ID="pnlUserSelect" runat="server">
        <span class="formLabelAuto">User:</span>
        <asp:DropDownList ID="ddlUserSelect" runat="server" CssClass="formDropDown" AutoPostBack="true" AppendDataBoundItems="True"
            onselectedindexchanged="ddlUserSelect_SelectedIndexChanged" />
        <br /><br />
    </asp:Panel>
    <asp:Panel ID="pnlRoleSelect" runat="server">
        <span class="formLabelAuto">Role:</span>
        <asp:DropDownList ID="ddlRoleSelect" runat="server" CssClass="formDropDown" AutoPostBack="true" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlRoleSelect_SelectedIndexChanged" />
        <br /><br />
    </asp:Panel>
    <asp:Panel ID="pnlMyQueue" runat="server">
        <table border="0" cellpadding="5" width="100%" role="presentation">
            <colgroup>
                <col width="50%" />
                <col width="50%" />
            </colgroup>
            <tr valign="top">
                <td>
                    <div class="boxPanelFull2">
                        <div class="boxPanelHeader">My Dashboard</div>
                        <div class="boxPanelData">
                            <asp:GridView runat="server" Width="100%" ID="gvMyDashboard" AutoGenerateColumns="True" 
                                HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No actions found." Title="My dashboard"
    HeaderStyle-HorizontalAlign="Left"
    RowStyle-VerticalAlign="Top" 
    RowStyle-HorizontalAlign="Left"
    AllowPaging="True" 
    PageSize="15" 
    CellPadding="3" 
    PagerSettings-Mode="NumericFirstLast">
                                
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td id="quickJump" runat="server">
                    <div  class="boxPanelFull2" >
                        <div  class="boxPanelHeader">Quick Jump</div>
                        <div class="boxPanelData">
                            <div><asp:ValidationSummary ID="vsQuickJump" runat="server" DisplayMode="List" ValidationGroup="valQuickJump" /></div>
                            <div id="Table1" runat="server">
                                <div class="row">
                                    <div class="col-sm-4"><span class="formLabel">Tax ID</span></div>
                                    <div class="col-sm-5"><ew:NumericBox ID="nbTaxID" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formField" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbTaxID" 
                                            ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4"><span class="formLabel">NPI</span></div>
                                    <div class="col-sm-5"><ew:NumericBox ID="nbNPI" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="10" CssClass="formField" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="nbNPI" 
                                            ValidationExpression="^\d{10}$" ErrorMessage="Enter a 10 digit NPI" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4"><span class="formLabel">SSN</span></div>
                                    <div class="col-sm-5"><asp:TextBox ID="txtSSN" runat="server" CssClass="formField" />
                                        <ajax:MaskedEditExtender runat="server" ID="meeSSN" AutoComplete="False" 
                                            ClearMaskOnLostFocus="False" TargetControlID="txtSSN" 
                                            MaskType="Number" Mask="999-99-9999" CultureAMPMPlaceholder="" 
                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                        <asp:CustomValidator ID="cvSSN" runat="server" SetFocusOnError="True" Display="Dynamic" 
                                            ControlToValidate="txtSSN" ClientValidationFunction="CheckSSNLength" 
                                            ErrorMessage="Enter valid SSN" Text="*" ValidationGroup="valQuickJump" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3"><span class="formLabel">&nbsp;</span></div>
                                    <div class="col-sm-9 text-right">
                                        <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="buttonBox" 
                                        ValidationGroup="valQuickJump" onclick="btnGo_Click" /></div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left"><span class="formLabel"><asp:LinkButton ID="lnkClear" runat="server" Text="Clear"
                                            onclick="lnkClear_Click" CssClass="linkNormal" /></span></div>
                                    <div class="col-sm-9 text-right"><asp:HyperLink ID="hyperAdvSearch" runat="server" Text="Advanced Search" NavigateUrl="~/Process/GroupReview.aspx" /></div>
                                </div>
                            </div>
                            <table id="DIDDSearch" runat="server">
                                <tr>
                                    <td><span class="formLabel">Tax ID</span></td>
                                    <td align="left"><ew:NumericBox ID="NumericBox1" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formField" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="nbTaxID" 
                                            ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="formLabel">Contract Number</span></td>
                                    <td align="left"><ew:NumericBox ID="NumericBox2" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="10" CssClass="formField" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="nbNPI" 
                                            ValidationExpression="^\d{10}$" ErrorMessage="Enter a 10 digit NPI" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><span class="formLabel">&nbsp;</span></td>
                                    <td align="right">
                                        <asp:Button ID="Button1" runat="server" Text="GO" CssClass="buttonBox" 
                                        ValidationGroup="valQuickJump" onclick="btnGo_Click" PostBackUrl="~/Process/Registration.aspx" /></td>
                                </tr>
                                <tr>
                                    <td><span class="formLabel"><asp:LinkButton ID="LinkButton1" runat="server" Text="Clear"
                                            onclick="lnkClear_Click" CssClass="linkNormal" /></span></td>
                                    <td align="right"><asp:HyperLink ID="HyperLink1" runat="server" Text="Advanced Search" NavigateUrl="~/Process/GroupReview.aspx" /></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td runat="server" id="tdMyDashGraph">
                    <div class="boxPanelHeader">Work Items Summary</div>
                <div id ="chart_div"></div>             
                </td>
            </tr>
        </table>

    <br /><br />
    <div class="boxPanelFull2" style="width: 100%">
        <div class="boxPanelHeader">My Actions</div>
            <div class="boxPanelData">
                <asp:GridView 
                    runat="server" 
                    Width="100%" 
                    Title="My actions"
                    ID="gvAssignedDetail"  
                    AutoGenerateColumns="False" 
                    HorizontalAlign="Center" 
                    CssClass="gridViewSmallFont" 
                    EmptyDataText="No actions found."
                     DataKeyNames="REG_ID, PROCESS_ID,STEP_ID,WORKFLOW_ID"
                    OnRowCommand="gvAssignedDetail_RowCommand"
                    OnRowCreated="gvAssignedDetail_OnRowCreated">
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderText="Task">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkReview"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    CommandName="DoWork"
                                    Text='<%# Eval("TASK_NAME") %>' 
                                    CssClass="gridLink" PostBackUrl="~/Process/Registration.aspx"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ReadOnly="true" DataField="WORKFLOW_NAME" HeaderText="Workflow" />
                        <asp:BoundField ReadOnly="true" DataField="attempt" HeaderText="Attempt" />
                        <asp:BoundField ReadOnly="true" DataField="PROVIDER_NAME" HeaderText="Name" />
                        <asp:BoundField ReadOnly="true" DataField="TAX_ID" HeaderText="Tax ID" />
                        <asp:BoundField ReadOnly="true" DataField="NPI" HeaderText="NPI" />
                        <asp:BoundField ReadOnly="true" DataField="Due_Date" HeaderText="Due By" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField ReadOnly="true" DataField="AGING" HeaderText="Aging" />
                        
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnPage" runat="server" Value='<%# Eval("CLASS_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" /> 
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
        </div>
	    <br />
	    <div style="text-align: right">
            
	    </div>
    </asp:Panel>
        </div>
</asp:Content>