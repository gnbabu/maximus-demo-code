using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_WPCCodeSets : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsRDMCodeSets = svc.SelectWPCCodeSets();
            DataTable jumpToTable = getJumpToTable();
            if (Helper.HasRows(dsRDMCodeSets))
            {
                jumpToTable.Rows.Add(new String[] { "", "", "", "", "" });
                foreach (DataRow dr in dsRDMCodeSets.Tables[0].Rows)
                {
                    string imgUrl = GetImageUrl(dr["TABLE_NAME"].ToString(), dr["ISAPPROVED"].ToString());
                    jumpToTable.Rows.Add(new String[] { dr["ID"].ToString(), dr["TABLE_NAME"].ToString(), dr["TABLE_NAME"].ToString(), imgUrl, imgUrl });
                }
                ddlWPCCodeSet.DataSource = jumpToTable;
                ddlWPCCodeSet.DataBind();
            }
        }
    }

    //protected void ddlWPCCodeSet_Init(object sender, EventArgs e)
    //{
    //    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
    //    DataSet dsWPCCodeSets = svc.SelectWPCCodeSets();
    //    if (Helper.HasRows(dsWPCCodeSets))
    //    {
    //        Helper.LoadDropDown(ddlWPCCodeSet, dsWPCCodeSets.Tables[0], "TABLE_NAME", "TABLE_NAME", true);
    //    }
    //}
    protected void btnReview_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlWPCCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsWPCCodeSets = svc.SelectWPCCodeSetsByName(ddlWPCCodeSet.SelectedValue);
            gvWPCCodeSets.DataSource = dsWPCCodeSets;
            gvWPCCodeSets.DataBind();

            DataSet dsCodeSetStatus = svc.SelectWPCCodeSetStatusByName(ddlWPCCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvWPCCodeSets.DataSource = null;
            gvWPCCodeSets.DataBind();
        }
    }

    protected void gvWPCCodeSets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvWPCCodeSets.PageIndex = e.NewPageIndex;

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsWPCCodeSets = svc.SelectWPCCodeSetsByName(ddlWPCCodeSet.SelectedValue);
        gvWPCCodeSets.DataSource = dsWPCCodeSets;
        gvWPCCodeSets.DataBind();
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlWPCCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveWPCCodeSetsByName(ddlWPCCodeSet.SelectedValue, true);

            DataSet dsCodeSetStatus = svc.SelectWPCCodeSetStatusByName(ddlWPCCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvWPCCodeSets.DataSource = null;
            gvWPCCodeSets.DataBind();
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlWPCCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveWPCCodeSetsByName(ddlWPCCodeSet.SelectedValue, false);
            DataSet dsCodeSetStatus = svc.SelectWPCCodeSetStatusByName(ddlWPCCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvWPCCodeSets.DataSource = null;
            gvWPCCodeSets.DataBind();
        }
    }

    #region "Dropdown list methods"
    protected void RadJumpTo_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        //set the Text and Value property of every item
        //here you can set any other properties like Enabled, ToolTip, Visible, etc.
        string jumpto_text = ((DataRowView)e.Item.DataItem)["Text"].ToString();
        e.Item.Text = jumpto_text.IndexOf('<') > 0 ? jumpto_text.Substring(0, jumpto_text.IndexOf('<')) : jumpto_text;
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Value"].ToString();
    }


    private DataTable getJumpToTable()
    {
        DataTable jumpToTable = new DataTable();
        jumpToTable.Columns.Add("ID");
        jumpToTable.Columns.Add("Value");
        jumpToTable.Columns.Add("Text");
        jumpToTable.Columns.Add("Icon");
        jumpToTable.Columns.Add("Status");


        return jumpToTable;
    }

    private string GetImageUrl(string tableName, string statusId)
    {
        string rtn = "~/Images/blank.png";
        var changeCnt = GetCodeSetChangeCount(tableName);
        if (changeCnt == 0)
            rtn = "~/Images/StepCheck.png";
        else
            rtn = "~/Images/InProcess.png";

        return rtn;
    }
    protected void RadJumpTo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Value))
        {
            //Do nothing here
        }
    }

    private int GetCodeSetChangeCount(string tableName)
    {
        int cnt = 0;
        if (!string.IsNullOrEmpty(tableName))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            cnt = svc.SelectWPCCodeSetChangeCount(tableName);
        }
        return cnt;
    }

    #endregion

}