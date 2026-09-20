using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_DIDDProviderSearch : System.Web.UI.UserControl
{
    public delegate void RegistrationViewEventHandler(int registrationId);
    public event RegistrationViewEventHandler RegistrationViewEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardRegistrationIds))
        {
            GetDashboardData();
        }
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardReferralIds))
        {
            GetDashboardData();
        }
        if (SessionVarRetriever.DashBoardTableId > 0)
        {
            RefreshData();
        }
        this.ucDIDDReferral.KeepOpenEvent += new UserControls_DIDDReferral.KeyOpenEventHandler(Control_KeepOpen);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) txtName.Focus();
    }


    protected void Validate_OneSearchValueEntered(object source, ServerValidateEventArgs args)
    {
        if (!HasSearchValues())
            args.IsValid = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        SessionVarRetriever.DashBoardReferralIds = "";

        this.gvDIDDProviders.CurrentPageIndex = 0;
        RefreshData();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //txtName.Text = txtApplicationNumber.Text = txtEmailAddress.Text = txtMedicaidID.Text = 
        //    nbNPI.Text = nbTaxID.Text = string.Empty;
        txtName.Text = txtOrgID.Text = txtEmailAddress.Text = txtApplicationNumber.Text = nbTaxID.Text = string.Empty;
        gvDIDDProviders.DataSource = null;
        gvDIDDProviders.DataBind();
        txtName.Focus();
    }

    protected void gvDIDDProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var contorl = e.Row.FindControl("lnkReview");
            DataRowView row = (DataRowView)e.Row.DataItem;

            //string referralNo = UIHelper.CreateDIDDApplicationNumberWithSuffix(row["ApplicationNo"] as string, Convert.ToInt32(row["REFERRAL_SUFFIX"]));

            if (Convert.ToInt32(row["RegID"]) > 0 && string.IsNullOrEmpty(SessionVarRetriever.DashBoardReferralIds))
                contorl.Visible = true;
            else
            {
                contorl.Visible = false;
                //e.Row.Cells[9].Text = string.Empty;
            }
        }
    }

    private void Control_KeepOpen()
    {
        this.mpe.Show();
    }

    private void LoadMPE(int refId)
    {
        ucDIDDReferral.LoadData(refId);
    }

    private bool HasSearchValues()
    {
        bool hasValue = false;
        if (!string.IsNullOrWhiteSpace(this.txtName.Text.Trim()))
            hasValue = true;
        else if (!string.IsNullOrWhiteSpace(this.nbTaxID.Text.Trim()))
            hasValue = true;
        else if (!string.IsNullOrWhiteSpace(this.txtApplicationNumber.Text.Trim()))
            hasValue = true;
        else if (!string.IsNullOrWhiteSpace(this.txtEmailAddress.Text.Trim()))
            hasValue = true;
        else if (!string.IsNullOrWhiteSpace(this.txtOrgID.Text.Trim()))
            hasValue = true;
        else if (!string.IsNullOrWhiteSpace(this.txtReferredBy.Text.Trim()))
            hasValue = true;
        
        return hasValue;
    }
    protected void gvDIDDProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ReviewRow")
        {
            int regId = int.Parse(e.CommandArgument.ToString());
            if (regId > 0)
            {
                if (RegistrationViewEvent != null) RegistrationViewEvent(regId);
            }
        }

        if (e.CommandName == "ReferralRow")
        {
            int refId = int.Parse(e.CommandArgument.ToString());
            if (refId > 0)
            {
                LoadMPE(refId);
                mpe.Show();
            }
        }
    }

    protected void gvDIDDProviders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }

    protected void gvDIDDProviders_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    private void RefreshData()
    {
        int totalResultCount = 0;
        DataTable dt = new DataTable(); 
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardRegistrationIds))
        {
            GetDashboardData();
        }
        else if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardReferralIds))
        {
            GetDashboardData();
        }
        else
        {
            dt = GetSearchedData(out totalResultCount);
            gvDIDDProviders.DataSource = dt;
            gvDIDDProviders.VirtualItemCount = totalResultCount;
            gvDIDDProviders.DataBind();
        }
    }

    private DataTable GetSearchedData(out int totalResultCount)
    {
        DataSet ds;
        string sortColWithDirection = this.gvDIDDProviders.GridViewSortDirection == SortDirection.Descending ? gvDIDDProviders.GridViewSortColumn + " DESC" : gvDIDDProviders.GridViewSortColumn;
        totalResultCount = 0;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (SessionVarRetriever.DashBoardTableId > 0)
        {
            if(SessionVarRetriever.DashBoardStatusID == 1091)
                ds = psc.SearchDIDDReferralByReferralIDs(SessionVarRetriever.DashBoardReferralIds, gvDIDDProviders.PageSize, gvDIDDProviders.CurrentRowIndex, SessionVarRetriever.DashBoardTableId, SessionVarRetriever.DashBoardStatusID, SessionVarRetriever.DashBoardOrdinal, out totalResultCount);
            else
                ds = psc.SearchDIDDReferralByRegIDs(SessionVarRetriever.DashBoardRegistrationIds, gvDIDDProviders.PageSize, gvDIDDProviders.CurrentRowIndex, SessionVarRetriever.DashBoardTableId, SessionVarRetriever.DashBoardStatusID, SessionVarRetriever.DashBoardOrdinal, SessionVarRetriever.DashBoardIsAssigned, out totalResultCount);
        }
        else
        {
            ds = psc.SearchDIDDReferral(txtName.Text.Trim(), txtApplicationNumber.Text.Trim(), nbTaxID.Text.Trim(), txtOrgID.Text.Trim(), "",
                txtEmailAddress.Text.Trim(), txtReferredBy.Text.Trim(), sortColWithDirection, gvDIDDProviders.PageSize, gvDIDDProviders.CurrentRowIndex, true, out totalResultCount);
        }
        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    private void GetDashboardData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int totalResultCount = 0;
        //gvDIDDProviders.DataSource = psc.SearchDIDDReferral(txtName.Text, txtApplicationNumber.Text, nbTaxID.Text, txtMedicaidID.Text, nbNPI.Text, 
        //    txtEmailAddress.Text);
        if (!string.IsNullOrEmpty(SessionVarRetriever.DashBoardReferralIds))
        {
            gvDIDDProviders.DataSource = psc.SearchDIDDReferralByReferralIDs(SessionVarRetriever.DashBoardReferralIds, gvDIDDProviders.PageSize, gvDIDDProviders.CurrentRowIndex,0,0,0,  out totalResultCount);
            gvDIDDProviders.VirtualItemCount = totalResultCount;
            gvDIDDProviders.DataBind();
        }
        else
        {
            gvDIDDProviders.DataSource = psc.SearchDIDDReferralByRegIDs(SessionVarRetriever.DashBoardRegistrationIds, gvDIDDProviders.PageSize, gvDIDDProviders.CurrentRowIndex,0,0,0,SessionVarRetriever.DashBoardIsAssigned,  out totalResultCount);
            gvDIDDProviders.VirtualItemCount = totalResultCount;
            gvDIDDProviders.DataBind();
        }
    }
}