using AjaxControlToolkit;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_ACHHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private void LoadGrid(string tableName, GridView grd)
    {
		PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, tableName + "_History");
        if (Helper.HasRows(ds)) grd.DataSource = ds.Tables[0];
        else grd.DataSource = null;
        grd.DataBind();
    }
    
	// Opt: 1 = Banking Info, 2 = Eft Contact, 3 = Vendor Info
	public void LoadData(int opt)
    {
        opt -= 1;
        mltHistory.ActiveViewIndex = opt;
        string tableName = string.Empty;
        switch (opt)
        {
            case 0:
				LoadGrid("ACH_REQUEST", grdBankingInfo);
				LoadGrid("ACH_CONTACT", grdEftContact);
				/*LoadGrid_ACH_REQUEST("ACH_REQUEST");
				LoadGrid_ACH_CONTACT("ACH_CONTACT");*/
				break;
            case 1:
                LoadGrid("ACH_CONTACT", grdEftContact);
                break;
            case 2:
                LoadGrid("ACH_EDISON", grdVendorInfo);
                break;
        }
    }

    protected void grdEftContact_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idx = 1;
            e.Row.Cells[idx].Text = Helper.FormatPhone(e.Row.Cells[idx].Text);
        }
    }

    protected void grdBankingInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idx = 2;
            string accountNumber = e.Row.Cells[idx].Text;
            if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) && !Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name))
            {
                // Mask it if not in FA1 or FA2
                accountNumber = Helper.MaskValue(accountNumber, 4);
            }
            e.Row.Cells[idx].Text = accountNumber;
        }
    }

    protected void grdBankingInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
		grdBankingInfo.PageIndex = e.NewPageIndex;
		LoadGrid("ACH_REQUEST", grdBankingInfo);
		ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
		mpe.Show();
	}

	protected void grdEftContact_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		grdEftContact.PageIndex = e.NewPageIndex;
		LoadGrid("ACH_CONTACT", grdEftContact);
		ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
		mpe.Show();
	}
}