using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_CMSCodeSets : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsCMSCodeSets = svc.SelectCMSCodeSets();
            DataTable jumpToTable = getJumpToTable();
            if (Helper.HasRows(dsCMSCodeSets))
            {
                jumpToTable.Rows.Add(new String[] { "", "", "", "", "" });
                foreach (DataRow dr in dsCMSCodeSets.Tables[0].Rows)
                {
                    string imgUrl = GetImageUrl(dr["TABLE_NAME"].ToString(), dr["ISAPPROVED"].ToString());
                    jumpToTable.Rows.Add(new String[] { dr["ID"].ToString(), dr["TABLE_NAME"].ToString(), dr["TABLE_NAME"].ToString(), imgUrl, imgUrl });
                }
                ddlCMSCodeSet.DataSource = jumpToTable;
                ddlCMSCodeSet.DataBind();
            }
        }
    }
    //protected void ddlCMSCodeSet_Init(object sender, EventArgs e)
    //{
    //    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
    //    DataSet dsCMSCodeSets = svc.SelectCMSCodeSets();
    //    if (Helper.HasRows(dsCMSCodeSets))
    //    {
    //        Helper.LoadDropDown(ddlCMSCodeSet, dsCMSCodeSets.Tables[0], "DISPLAY_NAME", "TABLE_NAME", true);
    //    }
    //}
    protected void btnReview_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCMSCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsCMSCodeSets = svc.SelectCMSCodeSetsByName(ddlCMSCodeSet.SelectedValue);
            gvCMSCodeSets.DataSource = dsCMSCodeSets;
            gvCMSCodeSets.DataBind();

            DataSet dsCodeSetStatus = svc.SelectCMSCodeSetStatusByName(ddlCMSCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvCMSCodeSets.DataSource = null;
            gvCMSCodeSets.DataBind();
        }
    }
    protected void gvCMSCodeSets_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCMSCodeSets.PageIndex = e.NewPageIndex;

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsCMSCodeSets = svc.SelectCMSCodeSetsByName(ddlCMSCodeSet.SelectedValue);
        gvCMSCodeSets.DataSource = dsCMSCodeSets;
        gvCMSCodeSets.DataBind();

    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCMSCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveCMSCodeSetsByName(ddlCMSCodeSet.SelectedValue, true);

            DataSet dsCodeSetStatus = svc.SelectCMSCodeSetStatusByName(ddlCMSCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvCMSCodeSets.DataSource = null;
            gvCMSCodeSets.DataBind();
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlCMSCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveCMSCodeSetsByName(ddlCMSCodeSet.SelectedValue, false);
            DataSet dsCodeSetStatus = svc.SelectCMSCodeSetStatusByName(ddlCMSCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvCMSCodeSets.DataSource = null;
            gvCMSCodeSets.DataBind();
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
            cnt = svc.SelectCMSCodeSetChangeCount(tableName);
        }
        return cnt;
    }

    #endregion

}