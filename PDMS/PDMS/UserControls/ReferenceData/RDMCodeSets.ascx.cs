using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ReferenceData_RDMCodeSets : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsRDMCodeSets = svc.SelectRDMCodeSets();
            DataTable jumpToTable = getJumpToTable();
            if (Helper.HasRows(dsRDMCodeSets))
            {
                jumpToTable.Rows.Add(new String[] { "", "", "", "", "" });
                foreach (DataRow dr in dsRDMCodeSets.Tables[0].Rows)
                {
                    string imgUrl = GetImageUrl(dr["TABLE_NAME"].ToString(), dr["ISAPPROVED"].ToString());
                    jumpToTable.Rows.Add(new String[] { dr["RDM_INCOMING_CODESET_TABLE_LIST_ID"].ToString(), dr["TABLE_NAME"].ToString(), dr["TABLE_NAME"].ToString(), imgUrl, imgUrl });
                }
                ddlRDMCodeSet.DataSource = jumpToTable;
                ddlRDMCodeSet.DataBind();
                //ddlRDMCodeSet.EnableViewState = false;
            }
        }
    }
    //protected void ddlRDMCodeSet_Init(object sender, EventArgs e)
    //{
    //    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
    //    DataSet dsRDMCodeSets = svc.SelectRDMCodeSets();
    //    DataTable jumpToTable = getJumpToTable();
    //    if (Helper.HasRows(dsRDMCodeSets))
    //    {
    //        foreach (DataRow dr in dsRDMCodeSets.Tables[0].Rows)
    //        {
    //            string imgUrl = GetImageUrl(dr["TABLE_NAME"].ToString(), dr["ISAPPROVED"].ToString());
    //            jumpToTable.Rows.Add(new String[] { dr["RDM_INCOMING_CODESET_TABLE_LIST_ID"].ToString(), dr["TABLE_NAME"].ToString(), dr["TABLE_NAME"].ToString(), imgUrl, imgUrl });
    //        }
    //        ddlRDMCodeSet.DataSource = jumpToTable;
    //        ddlRDMCodeSet.DataBind();
    //        ddlRDMCodeSet.EnableViewState = false;
    //        //Helper.LoadDropDown(ddlRDMCodeSet, dsRDMCodeSets.Tables[0], "TABLE_NAME", "TABLE_NAME", true);
    //    }
    //}
    protected void btnReview_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlRDMCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet dsRDMCodeSets = svc.SelectRDMCodeSetsByName(ddlRDMCodeSet.SelectedValue);
            gvRDMCodeSets.DataSource = dsRDMCodeSets;
            gvRDMCodeSets.DataBind();

            DataSet dsCodeSetStatus = svc.SelectRDMCodeSetstatusByName(ddlRDMCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvRDMCodeSets.DataSource = null;
            gvRDMCodeSets.DataBind();
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlRDMCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveRDMCodeSetsByName(ddlRDMCodeSet.SelectedValue, true, Methods.GetUserId(HttpContext.Current.User.Identity.Name));

            DataSet dsCodeSetStatus = svc.SelectRDMCodeSetstatusByName(ddlRDMCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvRDMCodeSets.DataSource = null;
            gvRDMCodeSets.DataBind();
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlRDMCodeSet.SelectedValue))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.ApproveRDMCodeSetsByName(ddlRDMCodeSet.SelectedValue, false, Methods.GetUserId(HttpContext.Current.User.Identity.Name));
            DataSet dsCodeSetStatus = svc.SelectRDMCodeSetstatusByName(ddlRDMCodeSet.SelectedValue);
            if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
            {
                lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                lblLoadDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
                lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
            }
        }
        else
        {
            gvRDMCodeSets.DataSource = null;
            gvRDMCodeSets.DataBind();
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
            cnt = svc.SelectRDMCodeSetChangeCount(tableName);
        }
        return cnt;
    }

    #endregion
}