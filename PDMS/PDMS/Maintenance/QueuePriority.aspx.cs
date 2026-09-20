using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_QueuePriority : System.Web.UI.Page
{

    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectWorkflowRoles();
            var test = ds.Tables[0].AsEnumerable().ToList().ConvertAll(x => new ListItem(x[0].ToString(), x[0].ToString()));
            ddlReferenceDataSelect.Items.AddRange(test.ToArray());
            GetWOrkflowTasksForRole(ddlReferenceDataSelect.SelectedValue);
        }

     }

    private void GetWOrkflowTasksForRole(string role)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectTasksByRole(role);
        WorkflowTaskOrder.DataSource = ds.Tables[0];
        //WorkflowTaskOrder.DataTextField = "WFTASK";
        WorkflowTaskOrder.DataKeyField = "TASK_ID";
        WorkflowTaskOrder.DataValueField = "rank";
        WorkflowTaskOrder.DataBind();
    }

    protected void ddlReferenceDataSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetWOrkflowTasksForRole(ddlReferenceDataSelect.SelectedValue);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        GetWOrkflowTasksForRole(ddlReferenceDataSelect.SelectedValue);
    }
    protected void WorkflowTaskOrder_Reordering(object sender, Telerik.Web.UI.RadListBoxReorderingEventArgs e)
    {
        RadListBox listBox = (RadListBox)sender;
        var movedItem = (RadListBoxItem)e.Items[0];
      //  var otherItem = (RadListBoxItem)listBox.Items[e.Items[0].Index +  e.Offset];

        int oldRanking = Convert.ToInt32(movedItem.Value);
        int newRanking = oldRanking == 1 && e.Offset < 0 ? 1 : oldRanking + e.Offset;
        // Swap ranks of the items and update the database
        
        psc.UpdateTaskRankForRole(ddlReferenceDataSelect.SelectedValue, int.Parse(movedItem.DataKey.ToString()), newRanking);

        Session["RankingDataKey"] = e.Items[0].DataKey;
        Session["QueueChangeOffSet"] = e.Offset;
        //for (int itemIdx = 0; itemIdx < e.Items.Count; itemIdx++)
        //{
        //    psc.UpdateTaskRankForRole(ddlReferenceDataSelect.SelectedValue, int.Parse(e.Items[itemIdx].DataKey.ToString()), int.Parse(e.Items[itemIdx +e.Offset].Index.ToString()));
        //}

        //psc.UpdateTaskRankForRole(ddlReferenceDataSelect.SelectedValue, int.Parse(otherItem.DataKey.ToString()), int.Parse(e.Items[0].Index.ToString()));
    }
    protected void WorkflowTaskOrder_Reordered(object sender, Telerik.Web.UI.RadListBoxEventArgs e)
    {
        GetWOrkflowTasksForRole(ddlReferenceDataSelect.SelectedValue);
        RadListBox listBox = (RadListBox)sender;
        int rankingData = 0;
        if (Session["RankingDataKey"] != null)
        {
            rankingData = Convert.ToInt32(Session["RankingDataKey"]);
        }
        RadListBoxItem tempItem = WorkflowTaskOrder.FindItem(i => Convert.ToInt32(i.DataKey) == rankingData);
        if (tempItem != null)
        {
            WorkflowTaskOrder.Items[tempItem.Index].Selected = true;
        }

    }

    protected void MoveBulk_Click(object sender, EventArgs e)
    {
        int selectItems = WorkflowTaskOrder.GetSelectedIndices().Length;
           if (selectItems > 0)
        {
            var lowestRank = 0;
            foreach (int index in WorkflowTaskOrder.GetSelectedIndices())
            {
                //all selected items take Rank of whatever is lowest rank in the items selected
                //read the first selected in RadListbox
                lowestRank = int.Parse(WorkflowTaskOrder.Items[index].Value.ToString());
                break;
            }
            //update the rank of existing items with rank below
            //psc.UpdateTaskRankForRole(ddlReferenceDataSelect.SelectedValue, 0, 2);
            foreach (int index in WorkflowTaskOrder.GetSelectedIndices())
            {
                //all selected items take Rank of whatever is lowest rank in the items selected
                var movedItem = WorkflowTaskOrder.Items[index];
                psc.UpdateTaskRankForRole(ddlReferenceDataSelect.SelectedValue, int.Parse(movedItem.DataKey.ToString()), lowestRank);
            }
            //reload the dropdown with new ranking
            GetWOrkflowTasksForRole(ddlReferenceDataSelect.SelectedValue);
        }
    }
}