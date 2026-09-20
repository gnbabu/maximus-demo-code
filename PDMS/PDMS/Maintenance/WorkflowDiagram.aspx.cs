using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Telerik.Web.UI;

public partial class Maintenance_WorkflowDiagram : System.Web.UI.Page
{
    public const int shapeHeight = 70;
    public const int shapeWidth = 70;
    public const int roleHeight = 100;
    public const int initialWidth = 1200;
    public const int shapePlacingWidth = 100;
    public const int shapeInitialX = 30;
    public const int shapeInitialY = 50;
    public const int workflowId = 1;
    public static Dictionary<int, DiagramShape> shapeList = new Dictionary<int, DiagramShape>();
    public static Dictionary<string, int> rolePlanes = new Dictionary<string, int>();
    public const string shapeIdPrefix = "Shape_";

    public enum TaskTypes
    {
        Queue,
        Email,
        System
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            shapeList.Clear();
            rolePlanes.Clear();

            CreateCanvas(RadDiagram1);
            //RadDiagram1.DataSource = GetShapes(RadDiagram1);
            //GetShapes(RadDiagram1);
            CreateTasks(RadDiagram1);

            //GetConnections(RadDiagram1);
            //RadDiagram1.ConnectionDataSource = GetConnections(RadDiagram1);
            RadDiagram1.DataBind();
        }
    }

    private void CreateCanvas(RadDiagram diagram)
    {
        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();

        var roleList = client.GetWorkflowRoles(workflowId);
        diagram.Height = roleHeight * (roleList.Tables[0].Rows.Count);
        diagram.Width = initialWidth;
    }

    private void AdjustCanvas(int finalWidth)
    {
        RadDiagram1.Width = finalWidth + shapePlacingWidth * 2;
    }

    private string GetGroupName(string groupName)
    {
        if (string.IsNullOrEmpty(groupName))
            return "System";

        return groupName;
    }


    private void CreateTasks(RadDiagram diagram)
    {
        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();

        var taskList = client.GetWorkflowSteps(workflowId);
        var firstTask = taskList.Tables[0].Rows[0];

        string firstTaskType = firstTask["TASK_TYPE"].ToString();
        string firstTaskName = firstTask["Task Name"].ToString();
        string firstGroupName = GetGroupName(firstTask["GROUP_NAME"].ToString());
        int firstTaskId = int.Parse(firstTask["TASK_ID"].ToString());

        var endShape = GetEnd();
        var startShape = GetStart();
        int shapeX = shapeInitialX + shapePlacingWidth;
        var firstShape = DrawTask(firstTaskType, firstTaskName, firstTaskId, firstGroupName, shapeX, shapeInitialY, taskList);

        diagram.ShapesCollection.Add(startShape);

        var firstConnector = GetConnector(startShape, firstShape, "Begin");
        diagram.ConnectionsCollection.Add(firstConnector);

        foreach (DataRow task in taskList.Tables[0].Rows)
        {
            string taskType = task["TASK_TYPE"].ToString();
            string taskName = task["Task Name"].ToString();
            int taskId = int.Parse(task["TASK_ID"].ToString());
            int actionId = int.Parse(task["ACTION_ID"].ToString());
            string actionName = task["Action"].ToString();
            int nextTaskId = int.Parse(task["NEXT_TASK_ID"].ToString());
            string groupName = GetGroupName(task["GROUP_NAME"].ToString());

            shapeX = shapeX + shapePlacingWidth;

            int shapeY = GetYFromGroup(groupName);

            var currentTaskShape = DrawTask(taskType, taskName, taskId, groupName, shapeX, shapeY, taskList);

            DiagramShape nextTaskShape = null;
            if (nextTaskId != 0)
            {
                var nextTask = taskList.Tables[0].Select("TASK_ID = " + nextTaskId)[0];
                string nextTaskType = nextTask["TASK_TYPE"].ToString();
                string nextTaskName = nextTask["Task Name"].ToString();
                string nextGroupName = GetGroupName(nextTask["GROUP_NAME"].ToString());
                int nextShapeY = GetYFromGroup(nextGroupName);
                nextTaskShape = DrawTask(nextTaskType, nextTaskName, nextTaskId, nextGroupName, shapeX, nextShapeY, taskList);
            }
            else
            {
                nextTaskShape = endShape;
            }

            var connector = GetConnector(currentTaskShape, nextTaskShape, actionName);

            //if (!shapeList.ContainsKey(taskId)) 
            //    diagram.ShapesCollection.Add(currentTaskShape);

            //if (nextTaskId != 0 && !shapeList.ContainsKey(nextTaskId))
            //    diagram.ShapesCollection.Add(nextTaskShape);

            diagram.ConnectionsCollection.Add(connector);

        }

        endShape.X = shapeX;
        endShape.Y = shapeInitialY;

        diagram.ShapesCollection.Add(endShape);
        AdjustCanvas(shapeX);
    }

    private static int GetYFromGroup(string groupName)
    {
        int maxPlaneValue = rolePlanes.Keys.Count > 0 ? rolePlanes.Values.Max() : shapeInitialY;

        if (!rolePlanes.ContainsKey(groupName))
            rolePlanes.Add(groupName, maxPlaneValue + roleHeight);

        int shapeY = rolePlanes[groupName];

        if (!rolePlanes.ContainsKey(groupName))
            rolePlanes.Add(groupName, maxPlaneValue + roleHeight);
        return shapeY;
    }

    private DiagramShape DrawTask(string taskType, string taskName, int taskId, string groupName, int shapeX, int shapeY, DataSet taskList)
    {
        if (!shapeList.ContainsKey(taskId))
        {
            DiagramShape theShape = null;
            if (taskType == TaskTypes.Queue.ToString())
            {
                theShape = GetRectagle(taskId, taskName, shapeX, shapeY);
            }
            else if (taskType == TaskTypes.Email.ToString())
            {
                theShape = GetRectagle(taskId, taskName, shapeX, shapeY);
            }
            else if (taskType == TaskTypes.System.ToString())
            {
                if (taskList.Tables[0].Select("TASK_ID = " + taskId).Length > 1)
                {
                    theShape = GetTriangle(taskId, taskName, shapeX, shapeY);
                }
                else
                {
                    theShape = GetRectagle(taskId, taskName, shapeX, shapeY);
                }
            }
            else
            {
                throw new Exception("Task type not defined");
            }
            shapeList.Add(taskId, theShape);
            RadDiagram1.ShapesCollection.Add(theShape);
            return theShape;
        }
        else
            return shapeList[taskId];

    }

    private DiagramShape GetRectagle(int taskId, string taskName, int shapeX, int shapeY)
    {
        DiagramShape shape2 = new DiagramShape();
        shape2.Id = shapeIdPrefix + taskId;
        shape2.ContentSettings.Text = taskName;
        shape2.Type = "rectangle";
        shape2.X = shapeX;
        shape2.Y = shapeY;
        shape2.Height = shapeHeight;
        shape2.Width = shapeWidth;
        shape2.FillSettings.Color = "#02b6f2";

        return shape2;
    }

    private DiagramShape GetTriangle(int taskId, string taskName, int shapeX, int shapeY)
    {
        DiagramShape shape2 = new DiagramShape();
        shape2.Id = shapeIdPrefix + taskId;
        shape2.ContentSettings.Text = taskName;
        shape2.Type = "question";
        shape2.X = shapeX;
        shape2.Y = shapeY;
        shape2.Height = shapeHeight;
        shape2.Width = shapeWidth;
        shape2.FillSettings.Color = "#ffbe33";

        return shape2;
    }

    private DiagramShape GetStart()
    {
        DiagramShape shape1 = new DiagramShape();
        shape1.Id = "start";
        shape1.ContentSettings.Text = "Start";
        shape1.Type = "start";
        shape1.X = shapeInitialX;
        shape1.Y = shapeInitialY;
        shape1.Height = shapeHeight;
        shape1.Width = shapeWidth;
        shape1.FillSettings.Color = "#cf3737";

        return shape1;
    }

    private DiagramShape GetEnd()
    {
        DiagramShape shape1 = new DiagramShape();
        shape1.Id = "end";
        shape1.ContentSettings.Text = "End";
        shape1.Type = "end";
        shape1.X = shapeInitialX;
        shape1.Y = shapeInitialY;
        shape1.Height = shapeHeight;
        shape1.Width = shapeWidth;
        shape1.FillSettings.Color = "#49a046";

        return shape1;
    }

    private DiagramConnection GetConnector(DiagramShape fromShape, DiagramShape toShape, string connectionText)
    {
        DiagramConnection connection1 = new DiagramConnection();
        connection1.FromSettings.ShapeId = fromShape.Id;
        //connection1.FromSettings.Connector = "Top";
        connection1.ToSettings.ShapeId = toShape.Id;
        //connection1.ToSettings.Connector = "Top";
        connection1.ContentSettings.Text = connectionText;
        connection1.StartCap = Telerik.Web.UI.Diagram.ConnectionStartCap.FilledCircle;
        connection1.EndCap = Telerik.Web.UI.Diagram.ConnectionEndCap.ArrowEnd;
        //connectinList.Add(connection1);

        return connection1;
    }
}