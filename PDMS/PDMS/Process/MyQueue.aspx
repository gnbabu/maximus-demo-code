<%@ Page Title="My Queue" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_MyQueue" Codebehind="MyQueue.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="display: inline;" class="page-main-header">
        <br />
        <span style="text-align: center">My Queue
        </span>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="WhiteBox">
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript"></script>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
        <script type="text/javascript" src="//www.google.com/jsapi"></script>
        <script type="text/javascript">
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
                        url: 'MyQueue.aspx/GetData',
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
                        options.title = 'Work Item: ' + category;
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
            .page-main-header {
                font-family: 'Merriweather', serif !important;
                font-size: 28px !important;
            }

            .WhiteBox {
                font-family: 'Source Sans Pro', sans-serif !important;
            }

            .col-sm-7 {
                width: 52% !important;
            }

            .pnlMyqueue {
                border: 0;
                padding: 5px;
                width: 100%;
            }

            .gvmydasboard {
                width: 100%;
                align-items: center;
            }

            .trest {
                vertical-align: top;
            }

            .form-control {
                margin-top: 10px;
                font-size: 17px;
                height: 44px;
                color: #000;
            }
        </style>
        <br />

        <asp:Panel ID="pnlUserSelect" runat="server">
            <div class="row">
                <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                    <asp:Label ID="lbluser" runat="server" Text="User:" CssClass="ohio-field" />
                    <asp:DropDownList ID="ddlUserSelect" runat="server" CssClass="form-control" AutoPostBack="true" AppendDataBoundItems="True" aria-label="User Select"
                        onselectedindexchanged="ddlUserSelect_SelectedIndexChanged" />
                </div>
            </div>

        </asp:Panel>
        <asp:Panel ID="pnlRoleSelect" runat="server">
            <div class="row">
                <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                    <asp:Label ID="lblRoleSelect" runat="server" AssociatedControlID="ddlRoleSelect" CssClass="ohio-field" Text="Role:" />

                    <asp:DropDownList ID="ddlRoleSelect" runat="server" CssClass="form-control" AutoPostBack="true" AppendDataBoundItems="True" aria-label="role select" OnSelectedIndexChanged="ddlRoleSelect_SelectedIndexChanged" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMyQueue" runat="server">
            <h2 class="boxPanelHeader">My Dashboard</h2>
            <div class="boxPanelData">
                <asp:GridView runat="server" ID="gvMyDashboard" AutoGenerateColumns="False" CssClass="gridview"
                    style="width: 100%; align-content: center" EmptyDataText="No actions found." title="My Dashboard">
                    <columns>
                        <asp:BoundField DataField="WORKFLOW_NAME" HeaderText="Work Item" />
                        <asp:TemplateField HeaderText="Assigned">
                            <itemtemplate>
                                <asp:Label ID="lnkAssigned" runat="server"
                                    Text='<%# DataBinder.Eval(Container.DataItem, "ASSIGNED") %>' />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UNASSIGNED" HeaderText="UnAssigned" />
                    </columns>
                    <pagerstyle cssclass="gridpager" horizontalalign="Right" />
                    <headerstyle cssclass="gridViewHeader" width="100px" />
                    <alternatingrowstyle cssclass="gridViewAltRow" />
                    <rowstyle cssclass="gridViewRow" />
                    <footerstyle cssclass="gridViewFooter" />
                </asp:GridView>
            </div>

            <div id="quickJump" runat="server" class="boxPanelFull2">
                <div class="boxPanelHeader">Quick Jump</div>
                <div class="boxPanelData">
                    <div>
                        <asp:ValidationSummary ID="vsQuickJump" runat="server" DisplayMode="List" ValidationGroup="valQuickJump" />
                    </div>
                    <div id="Table1" runat="server">
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel150">Tax ID</span></div>
                            <div class="col-sm-4 text-left">
                                <ew:numericbox id="nbTaxID" runat="server" decimalplaces="0" positivenumber="True" maxlength="9" cssclass="formField300" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbTaxID"
                                    ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel150">NPI</span></div>
                            <div class="col-sm-4 text-left">
                                <ew:numericbox id="nbNPI" runat="server" decimalplaces="0" positivenumber="True" maxlength="10" cssclass="formField300" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="nbNPI"
                                    ValidationExpression="^\d{10}$" ErrorMessage="Enter a 10 digit NPI" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel150">SSN</span></div>
                            <div class="col-sm-4 text-left">
                                <asp:TextBox ID="txtSSN" runat="server" CssClass="formField300" />
                                <ajax:maskededitextender runat="server" id="meeSSN" autocomplete="False"
                                    clearmaskonlostfocus="False" targetcontrolid="txtSSN"
                                    masktype="Number" mask="999-99-9999" cultureampmplaceholder=""
                                    culturecurrencysymbolplaceholder="" culturedateformat=""
                                    culturedateplaceholder="" culturedecimalplaceholder=""
                                    culturethousandsplaceholder="" culturetimeplaceholder="" enabled="True" />
                                <asp:CustomValidator ID="cvSSN" runat="server" SetFocusOnError="True" Display="Dynamic"
                                    ControlToValidate="txtSSN" ClientValidationFunction="CheckSSNLength"
                                    ErrorMessage="Enter valid SSN" Text="*" ValidationGroup="valQuickJump" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3"><span class="formLabel">&nbsp;</span></div>
                            <div class="col-sm-7 text-right">
                                <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="buttonBox"
                                    ValidationGroup="valQuickJump" onclick="btnGo_Click" PostBackUrl="~/Process/Registration.aspx" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-left">
                                <span class="formLabel">
                                    <asp:LinkButton ID="lnkClear" runat="server" Text="Clear"
                                        onclick="lnkClear_Click" CssClass="linkNormal" />
                                </span>
                            </div>
                            <div class="col-sm-7 text-right">
                                <asp:HyperLink ID="hyperAdvSearch" runat="server" Text="Advanced Search" NavigateUrl="~/Process/GroupReview.aspx" />
                            </div>
                        </div>
                        <table id="DIDDSearch" runat="server">
                            <tr>
                                <th><span class="formLabel">Tax ID</span></th>
                                <th align="left">
                                    <ew:numericbox id="NumericBox1" runat="server" decimalplaces="0" positivenumber="True" maxlength="9" cssclass="formField" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="nbTaxID"
                                        ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                                </th>
                            </tr>
                            <tr>
                                <th><span class="formLabel">Contract Number</span></th>
                                <th align="left">
                                    <ew:numericbox id="NumericBox2" runat="server" decimalplaces="0" positivenumber="True" maxlength="10" cssclass="formField" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="nbNPI"
                                        ValidationExpression="^\d{10}$" ErrorMessage="Enter a 10 digit NPI" Text="*" Display="Dynamic" ValidationGroup="valQuickJump" />
                                </th>
                            </tr>
                            <tr>
                                <th><span class="formLabel">&nbsp;</span></th>
                                <th align="right">
                                    <asp:Button ID="Button1" runat="server" Text="GO" CssClass="buttonBox"
                                        ValidationGroup="valQuickJump" onclick="btnGo_Click" PostBackUrl="~/Process/Registration.aspx" />
                                </th>
                            </tr>
                            <tr>
                                <th><span class="formLabel">
                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Clear"
                                        onclick="lnkClear_Click" CssClass="linkNormal" />
                                </span></th>
                                <th align="right">
                                    <asp:HyperLink ID="HyperLink1" runat="server" Text="Advanced Search" NavigateUrl="~/Process/GroupReview.aspx" />
                                </th>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <br />
            <div runat="server" class="row" id="tdMyDashGraph">

                <div class="row">
                    <h2 class="boxPanelHeader">Work Items Summary</h2>
                </div>
                <div id="chart_div" tabindex="0"></div>
            </div>
            <div class="row">
                <h2 class="boxPanelHeader">Work Items</h2>
            </div>
            <div style="text-align: right">
                <asp:Button id="btnGetNext" runat="server" Text="Get Next In Queue"
                    CssClass="buttonBoxFocus" ToolTip="Get the next item from the Queue"
                    onclick="btnGetNext_Click" PostBackUrl="~/Process/Registration.aspx" />
            </div>
            <div class="boxPanelData">
                <asp:GridView
                    runat="server"
                    style="width: 100%;"
                    title="Work item"
                    ID="gvAssignedDetail"
                    AutoGenerateColumns="False"
                    HorizontalAlign="Center"
                    CssClass="gridViewSmallFont"
                    EmptyDataText="No actions found."
                    DataKeyNames="REG_ID, PROCESS_ID,STEP_ID,TASK_ID,WORKFLOW_ID"
                    OnRowCommand="gvAssignedDetail_RowCommand" OnRowDataBound="gvAssignedDetail_RowDataBound" OnRowCreated="gvAssignedDetail_OnRowCreated">
                    <columns>
                        <asp:TemplateField ShowHeader="False" HeaderText="Task">
                            <itemtemplate>
                                <asp:LinkButton
                                    ID="lnkReview"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                    CommandName="DoWork"
                                    Text='<%# Eval("TASK_NAME") %>'
                                    CssClass="gridLink" PostBackUrl="~/Process/Registration.aspx" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:BoundField ReadOnly="true" DataField="WORKFLOW_NAME" HeaderText="Workflow">
                            <itemstyle width="120px" />
                        </asp:BoundField>
                        <asp:BoundField ReadOnly="true" DataField="PROVIDER_NAME" HeaderText="Name" />
                        <asp:BoundField ReadOnly="true" DataField="REG_ID" HeaderText="Reg ID" />
                        <asp:BoundField ReadOnly="true" DataField="NPI" HeaderText="NPI" />
                        <asp:BoundField ReadOnly="true" DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" />
                        <asp:BoundField ReadOnly="true" DataField="START_DATE_TIME" HeaderText="Date Assigned" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField ReadOnly="true" DataField="AGING" HeaderText="Aging" />
                        <asp:BoundField ReadOnly="true" DataField="PROVIDER_RISK_LEVEL_NAME" HeaderText=" Risk Level" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="">
                            <itemtemplate>
                                <asp:HiddenField ID="hdnPage" runat="server" Value='<%# Eval("CLASS_NAME") %>' />
                            </itemtemplate>
                        </asp:TemplateField>
                    </columns>
                    <pagerstyle cssclass="gridpager" horizontalalign="Right" />
                    <headerstyle cssclass="gridViewHeader" width="100px" />
                    <alternatingrowstyle cssclass="gridViewAltRow" />
                    <rowstyle cssclass="gridViewRow" />
                    <footerstyle cssclass="gridViewFooter" />
                </asp:GridView>
            </div>
            <br />
            <%--<div style="text-align: right">
            <asp:Button id="btnGetNext" runat="server" Text="Get Next In Queue" 
                CssClass="buttonBox" ToolTip="Get the next item from the Queue" 
                onclick="btnGetNext_Click" PostBackUrl="~/Process/Registration.aspx" />
	    </div>--%>
        </asp:Panel>
    </div>
</asp:Content>

