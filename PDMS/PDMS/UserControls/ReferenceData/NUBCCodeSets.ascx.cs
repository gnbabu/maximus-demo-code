using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_NUBCCodeSets : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsNUBCCodeSets = svc.SelectNUBCCodeSets();
            DataTable jumpToTable = getJumpToTable();
            if (Helper.HasRows(dsNUBCCodeSets))
            {
                jumpToTable.Rows.Add(new String[] { "", "", "", "", "" });
                foreach (DataRow dr in dsNUBCCodeSets.Tables[0].Rows)
                {
                    string imgUrl = GetImageUrl(dr["TABLE_NAME"].ToString(), dr["ISAPPROVED"].ToString());
                    jumpToTable.Rows.Add(new String[] { dr["ID"].ToString(), dr["TABLE_NAME"].ToString(), dr["TABLE_NAME"].ToString(), imgUrl, imgUrl });
                }
                ddlNUBCCodeSet.DataSource = jumpToTable;
                ddlNUBCCodeSet.DataBind();
            }
        }
    }
    //protected void ddlNUBCCodeSet_Init(object sender, EventArgs e)
    //{
    //    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
    //    DataSet dsNUBCCodeSets = svc.SelectNUBCCodeSets();
    //    if (Helper.HasRows(dsNUBCCodeSets))
    //    {
    //        Helper.LoadDropDown(ddlNUBCCodeSet, dsNUBCCodeSets.Tables[0], "TABLE_NAME", "TABLE_NAME", true);
    //    }
    //}
    protected void btnReview_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlNUBCCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsNUBCCodeSets = svc.SelectNUBCCodeSetsByName(ddlNUBCCodeSet.SelectedValue);
            gvNUBCCodeSets.DataSource = dsNUBCCodeSets;
            gvNUBCCodeSets.DataBind();

            DataSet dsCodeSetStatus = svc.SelectNUBCCodeSetStatusByName(ddlNUBCCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvNUBCCodeSets.DataSource = null;
            gvNUBCCodeSets.DataBind();
        }
    }
    protected void gvNUBCCodeSets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNUBCCodeSets.PageIndex = e.NewPageIndex;

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsNUBCCodeSets = svc.SelectNUBCCodeSetsByName(ddlNUBCCodeSet.SelectedValue);
        gvNUBCCodeSets.DataSource = dsNUBCCodeSets;
        gvNUBCCodeSets.DataBind();

    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlNUBCCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveNUBCCodeSetsByName(ddlNUBCCodeSet.SelectedValue, true);

            DataSet dsCodeSetStatus = svc.SelectNUBCCodeSetStatusByName(ddlNUBCCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvNUBCCodeSets.DataSource = null;
            gvNUBCCodeSets.DataBind();
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlNUBCCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveNUBCCodeSetsByName(ddlNUBCCodeSet.SelectedValue, false);
            DataSet dsCodeSetStatus = svc.SelectNUBCCodeSetStatusByName(ddlNUBCCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvNUBCCodeSets.DataSource = null;
            gvNUBCCodeSets.DataBind();
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
            cnt = svc.SelectNUBCCodeSetChangeCount(tableName);
        }
        return cnt;
    }

    #endregion

}