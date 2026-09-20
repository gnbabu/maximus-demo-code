using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ReSendEmailNotices : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                BindDropDown();
                RefreshData();
            }
            catch (Exception ex)
            {
                AddError("Error Displaying records: " + ex.Message);
            }
        }
    }

    protected void ddlNoticeTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        

    }

    private DataTable GetData(out int totalResultCount, int pageSize, int startRowIndex)
    {
        DataSet ds;
        totalResultCount = 0;

        // If list of IDs has been passed in, display in search results list.
        ds = LookupTableController.GetReSendNoticesToGenerateData(pageSize, startRowIndex, true, out totalResultCount);

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    public void RefreshData()
    {
        int totalResultCount = 0;
        int startRowIndex = 0;
        DataTable dt = GetData(out totalResultCount, gvQueryResendSelectedNotices.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvQueryResendSelectedNotices.EmptyDataText = "No Data found.";
        }
        gvQueryResendSelectedNotices.DataSource = dt;
        gvQueryResendSelectedNotices.VirtualItemCount = totalResultCount;
        gvQueryResendSelectedNotices.DataBind();

    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "ReSendNoticesVG";
        this.Page.Validators.Add(val);
    }

    protected void gvQueryResendSelectedNotices_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "DeleteResendSelectedNoticesRow")
        {
            try
            {
                string id = gvQueryResendSelectedNotices.DataKeys[index].Value.ToString();


                int result;

                if (int.TryParse(id, out result))
                {
                    LookupTableController.DeleteReSendNotices(result);
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                AddError("Error deleting records: " + ex.Message);
            }
        }
    }

    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtRegIdsToReSend.Text))
            {
                AddError("Please enter IDs for deletion");
            }
            else
            {
                LookupTableController.DeleteAllReSendNotices(txtRegIdsToReSend.Text);
                RefreshData();
            }
        }
        catch (Exception ex)
        {
            AddError("Error deleting all records: " + ex.Message);
        }
    }

    protected void btnResendSelectedNotices_Click(object sender, EventArgs e)
    {
        try
        {
            if (!ddlNoticeTypesID.SelectedValue.Equals(""))
            {
                string checkboxValue = Request.Form["ckbSendPaperNoticeID"];
                bool isChecked = !string.IsNullOrEmpty(checkboxValue);

                LookupTableController.InsertReSendNotices(txtRegIdsToReSend.Text, ddlNoticeTypesID.SelectedValue.ToString(), isChecked);
                RefreshData();

                txtRegIdsToReSend.Text = string.Empty;
                ddlNoticeTypesID.SelectedIndex = -1;
            }
            else
            {
                AddError("Error Notice Type must be selected");
            }
        }
        catch (Exception ex)
        {
            AddError("Error inserting records: " + ex.Message);
        }
    }

    private void BindDropDown()
    {
        DataSet ds = LookupTableController.GetReSendNoticesDropDownValues();
        Helper.LoadDropDown(ddlNoticeTypesID, ds.Tables[0], "NOTICE_TYPE_NAME", "NOTICE_TYPE_ID", true);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtRegIdsToReSend.Text = string.Empty;
        ddlNoticeTypesID.SelectedIndex = -1;
    }

    protected void gvQueryResendSelectedNotices_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvQueryResendSelectedNotices.PageIndex = e.NewPageIndex;
        gvQueryResendSelectedNotices.VirtualItemCount = 1000;
        gvQueryResendSelectedNotices.DataBind();
    }

    protected void gvQueryResendSelectedNotices_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        int totalResultCount = 0;
        int startRowIndex = gvQueryResendSelectedNotices.PageSize * e.NewPageIndex;
        DataTable dt = GetData(out totalResultCount, gvQueryResendSelectedNotices.PageSize, startRowIndex);
        hdnRowCount.Value = totalResultCount.ToString();
        if (totalResultCount == 0)
        {
            gvQueryResendSelectedNotices.EmptyDataText = "No Data found.";
            gvQueryResendSelectedNotices.DataSource = null;
            gvQueryResendSelectedNotices.DataBind();
        }
        gvQueryResendSelectedNotices.PageIndex = e.NewPageIndex;
        gvQueryResendSelectedNotices.DataSource = dt;
        gvQueryResendSelectedNotices.VirtualItemCount = totalResultCount;
        gvQueryResendSelectedNotices.DataBind();
    }

}