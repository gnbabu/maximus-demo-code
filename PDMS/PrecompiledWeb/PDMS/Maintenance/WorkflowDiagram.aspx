<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Maintenance_WorkflowDiagram, App_Web_nqybrob4" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <script language="javascript" type="text/javascript">
            (function (global, undefined) {
                var diagram;

                function pageLoad() {
                    capAllConnections(diagram.connections);
                    cleanUpShapesContent(diagram.shapes);
                }

                function diagram_load(sender) {
                    diagram = sender.get_kendoWidget();
                    diagram.saveAsPDF();
                }

                function visualizeShape(options) {
                    var ns = kendo.dataviz.diagram,
                        diagramCanvas = getDiagramCanvasOnPage(),
                        lineHeight = 16,
                        type = options.type,
                        shapeGroup = new ns.Group({ autoSize: true }),
                        textGroup = new ns.Group(),
                        textLines = [];

                    if (options.type != "text" && options.content && options.content.text) {
                        text = options.content.text.split("\\n");

                        var textHeight = options.height - (text.length - 1) * lineHeight;

                        for (var i = 0; i < text.length; i++) {
                            var y = (i * lineHeight);

                            textLines.push(new ns.TextBlock({
                                autoSize: false,
                                text: text[i],
                                x: 0,
                                y: y,
                                width: options.width,
                                height: textHeight + 2 * y,
                                color: options.stroke.color,
                                fontSize: 6,
                                fontFamily: "Segoe UI"
                            }));
                        }
                        options.content.text = "";
                    }

                    if (type == "rectangle") {
                        var rectangle = new ns.Rectangle(options);
                        appendToGroupWithoutOffset(rectangle, shapeGroup);
                    }
                    else if (type == "question") {
                        options.data = "M70,0 L140,70 L70,140 L0,70 z";
                        var path = new ns.Path(options);
                        appendToGroupWithoutOffset(path, shapeGroup);
                    }
                    else if (type == "start" || type == "end") {
                        var circle = new ns.Circle(options);
                        appendToGroupWithoutOffset(circle, shapeGroup);
                    }

                    if (options.type != "text") {
                        diagramCanvas.append(shapeGroup);
                        var lineHeight_x2 = 2 * lineHeight,
                            box = shapeGroup.drawingElement.bbox(),
                            largestTextContainerHeight = box.size.height + lineHeight * (textLines.length - 1);

                        for (var j = textLines.length - 1, textEdge = largestTextContainerHeight; j >= 0; j--, textEdge -= lineHeight_x2) {
                            var textLine = textLines[j];
                            shapeGroup.append(textLine);
                            var containerRect = new kendo.dataviz.diagram.Rect(box.origin.x, box.origin.y, box.size.width, textEdge);
                            alignTextShape(containerRect, textLine);
                        }
                        diagramCanvas.remove(shapeGroup);
                    }

                    return shapeGroup;
                }

                function appendToGroupWithoutOffset(shape, group) {
                    shape.position(0, 0);
                    group.append(shape);
                }

                function alignTextShape(containerRect, textLine) {
                    var aligner = new kendo.dataviz.diagram.RectAlign(containerRect);
                    var contentBounds = textLine.drawingElement.bbox(null);

                    var contentRect = new kendo.dataviz.diagram.Rect(0, 0, contentBounds.width(), contentBounds.height());
                    var alignedBounds = aligner.align(contentRect, "center middle");

                    textLine.position(alignedBounds.topLeft());
                }

                function createCustomMarker() {
                    return new kendo.dataviz.diagram.ArrowMarker({
                        path: "M 0 0 L 8 4 L 0 8 L 2 4 z",
                        fill: "#6c6c6c",
                        stroke: {
                            color: "#6c6c6c",
                            width: 0.5
                        },
                        id: "custom",
                        orientation: "auto",
                        width: 10,
                        height: 10,
                        anchor: new kendo.dataviz.diagram.Point(7, 4)
                    });
                }

                function capAllConnections(connections) {
                    Array.forEach(connections, function (connection) {
                        var marker = createCustomMarker();
                        connection.path._markers.end = marker;
                        connection.path.drawingContainer().append(marker.drawingElement);
                        connection.path._redrawMarkers(true, connection.path.options);
                    });
                }
                function cleanUpShapesContent(shapes) {
                    Array.forEach(shapes, function (shape) {
                        if (!/Yes|No/.test(shape.content())) {
                            shape.visual.remove(shape._contentVisual);
                        }
                    });
                }

                function getDiagramCanvasOnPage() {
                    return $telerik.$(".k-diagram").getKendoDiagram().canvas;
                }

                global.pageLoad = pageLoad;
                global.diagram_load = diagram_load;
                global.visualizeShape = visualizeShape;
            })(window);

</script>
    <div class="WhiteBox">

            <telerik:RadDiagram ID="theDiagram" runat="server" Width="1200" Height="700" Visible="false" Editable="true" >
                            <ClientEvents OnLoad="diagram_load" />
            <ShapeDefaultsSettings Visual="visualizeShape">
                <StrokeSettings Color="#fff" />
            </ShapeDefaultsSettings>
            <ShapesCollection>
                <telerik:DiagramShape Id="start" Type="start" X="30" Y="70">
                    <FillSettings Color="#cf3737" />
                    <ContentSettings Text="I have\na problem" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q1" Type="question" X="170" Y="50" Width="140" Height="140">
                    <FillSettings Color="#ffbe33" />
                    <ContentSettings Text="Can I solve it\nalone" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q1Yes" Type="text" X="260" Y="40">
                    <ContentSettings Text="Yes" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q1No" Type="text" X="330" Y="100">
                    <ContentSettings Text="No" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q2" Type="question" Width="140" Height="140" X="350" Y="50">
                    <FillSettings Color="#ffbe33" />
                    <ContentSettings Text="I found\na solution in\nthe forums" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q2Yes" Type="text" X="440" Y="40">
                    <ContentSettings Text="Yes" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q2No" Type="text" X="510" Y="100">
                    <ContentSettings Text="No" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q3" Type="question" Width="140" Height="140" X="530" Y="50">
                    <FillSettings Color="#ffbe33" />
                    <ContentSettings Text="The latest\nproduct version\nsolves the\nproblem" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q3Yes" Type="text" X="620" Y="40">
                    <ContentSettings Text="Yes" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q3No" Type="text" X="690" Y="100">
                    <ContentSettings Text="No" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q4" Type="question" Width="140" Height="140" X="710" Y="50">
                    <FillSettings Color="#ffbe33" />
                    <ContentSettings Text="The problem\nis reproducible\nin the QSF" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q4Yes" Type="text" X="800" Y="208">
                    <ContentSettings Text="Yes" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q4No" Type="text" X="870" Y="100">
                    <ContentSettings Text="No" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="action1" Type="rectangle" X="730" Y="230">
                    <ContentSettings Text="Open a\nsupport ticket\nand send the\ndemo link" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="action2" Type="rectangle" X="890" Y="70">
                    <ContentSettings Text="Prepare\na sample\nproject" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="action3" Type="rectangle" X="890" Y="230">
                    <ContentSettings Text="Open a\nsupport ticket\nand send your\nproject" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="action4" Type="rectangle" X="810" Y="390">
                    <ContentSettings Text="Describe\ndetails" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="action5" Type="rectangle" X="810" Y="530">
                    <ContentSettings Text="Submit the\nticket" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q5" Type="question" Width="140" Height="140" X="1030" Y="440">
                    <FillSettings Color="#ffbe33" />
                    <ContentSettings Text="The support\nofficer's response\nwas helpful" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q5Yes" Type="text" X="1120" Y="300">
                    <ContentSettings Text="Yes" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="q5No" Type="text" X="1000" Y="450">
                    <ContentSettings Text="No" />
                    <StrokeSettings Color="#000" />
                </telerik:DiagramShape>
                <telerik:DiagramShape Id="end" Type="end" X="1050" Y="70">
                    <FillSettings Color="#49a046" />
                    <ContentSettings Text="Problem\nsolved" />
                </telerik:DiagramShape>
            </ShapesCollection>
            <ConnectionsCollection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="start" />
                    <ToSettings ShapeId="q1" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q1" Connector="Right" />
                    <ToSettings ShapeId="q2" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q1" Connector="Top" />
                    <ToSettings ShapeId="end" Connector="Top" />
                    <PointsCollection>
                        <telerik:DiagramConnectionPoint X="240" Y="20" />
                        <telerik:DiagramConnectionPoint X="1100" Y="20" />
                    </PointsCollection>
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q2" Connector="Right" />
                    <ToSettings ShapeId="q3" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q2" Connector="Top" />
                    <ToSettings ShapeId="end" Connector="Top" />
                    <PointsCollection>
                        <telerik:DiagramConnectionPoint X="420" Y="20" />
                        <telerik:DiagramConnectionPoint X="1100" Y="20" />
                    </PointsCollection>
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q3" Connector="Right" />
                    <ToSettings ShapeId="q4" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q3" Connector="Top" />
                    <ToSettings ShapeId="end" Connector="Top" />
                    <PointsCollection>
                        <telerik:DiagramConnectionPoint X="600" Y="20" />
                        <telerik:DiagramConnectionPoint X="1100" Y="20" />
                    </PointsCollection>
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q4" />
                    <ToSettings ShapeId="action1" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q4" Connector="Right" />
                    <ToSettings ShapeId="action2" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="action2" />
                    <ToSettings ShapeId="action3" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="action1" />
                    <ToSettings ShapeId="action4" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="action3" />
                    <ToSettings ShapeId="action4" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="action4" />
                    <ToSettings ShapeId="action5" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="action5" />
                    <ToSettings ShapeId="q5" Connector="Bottom" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q5" Connector="Top" />
                    <ToSettings ShapeId="action4" />
                </telerik:DiagramConnection>
                <telerik:DiagramConnection>
                    <FromSettings ShapeId="q5" />
                    <ToSettings ShapeId="end" />
                </telerik:DiagramConnection>
            </ConnectionsCollection>
</telerik:RadDiagram>

    <telerik:RadDiagram ID="RadDiagram1" runat="server" Width="1200" Height="700" Editable="true" Pannable="true" >
    <PdfSettings FileName="diagram.pdf" Title="Diagram Exported to PDF" />
                            <ClientEvents OnLoad="diagram_load" />
            <ShapeDefaultsSettings Visual="visualizeShape" >
                <ContentSettings FontSize="6"  />
                <StrokeSettings Color="#000" />
            </ShapeDefaultsSettings>
</telerik:RadDiagram>


</div>
</asp:Content>
