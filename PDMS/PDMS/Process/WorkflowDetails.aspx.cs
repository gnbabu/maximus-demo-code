using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WorkflowDetails : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (!Helper.IsLoggedInUserInAdminRole())
                    Response.Redirect("~/Default.aspx");

                this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName");
                BindDropDown();
                RefreshData();
            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.gvTask.CurrentPageIndex = 0;
        RefreshData();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
      
    }

    protected void gvTask_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvTask_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    public void RefreshData()
    {
        int totalResultCount = 0;
        DataTable dt = GetData(out totalResultCount, gvTask.PageSize);
        //hdnRowCount.Value = totalResultCount.ToString();
        gvTask.DataSource = dt;
        gvTask.VirtualItemCount = totalResultCount;
        gvTask.DataBind();
    }

    private DataTable GetData(out int totalResultCount, int pageSize)
    {
        //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //DataSet ds;
         string sortColWithDirection;
         if (this.gvTask.MasterTableView.SortExpressions.Count > 0)
             sortColWithDirection = this.gvTask.MasterTableView.SortExpressions[0].SortOrder == Telerik.Web.UI.GridSortOrder.Descending ? gvTask.MasterTableView.SortExpressions[0].FieldName + " DESC" : gvTask.MasterTableView.SortExpressions[0].FieldName;
         else
             sortColWithDirection = "";

        totalResultCount = 0;

        DataTable dt = new DataTable();
        dt.Columns.Add("Task Name");
        dt.Columns.Add("User Name");
        dt.Columns.Add("Start Date");
        dt.Columns.Add("End Date");

        dt.Rows.Add("Provider Data Entry", "Clarskville123", "6/11/2016 11:19:20 AM", "6/11/2016 2:19:20 PM");
        dt.Rows.Add("Provider Review", "sclavjo1", "6/11/2016 2:20:42 AM", "6/11/2016 2:21:23 PM");
        dt.Rows.Add("Promote to Active", "AdminCore", "6/11/2016 2:22:45 AM", "6/11/2016 2:49:20 PM");
        dt.Rows.Add("Send Active Request to MMIS", "AdminCore", "6/11/2016 2:50:20 AM", "6/11/2016 2:50:30 PM");
        dt.Rows.Add("Send Email to Provider (Welcome Letter)", "AdminCore", "6/12/2016 3:19:20 AM", "6/11/2016 3:20:20 PM");
        dt.Rows.Add("Send Group Affliations to MMIS", "AdminCore", "6/12/2016 3:21:20 AM", "6/12/2016 3:21:20 PM");
        dt.Rows.Add("Check if Billing Indicator is Set", "AdminCore", "6/12/2016 3:22:20 AM", "6/12/2016 3:23:20 PM");
        dt.Rows.Add("Account Review", "merian001", "6/12/2016 3:24:20 AM", "6/13/2016 2:55:20 PM");
        dt.Rows.Add("Send Payment Info to MMIS", "Clarskville123", "6/13/2016 02:19:20 PM", "6/13/2016 2:55:20 PM");

        return dt;
        //if (Helper.HasRows(ds))
        //{
        //    return ds.Tables[0];
        //}
       // else return new DataTable();
    }

     

    protected void gvTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName != "ReviewRow" && e.CommandName != "ExpressAdmin")
        //    return;

        //int index = Convert.ToInt32(e.CommandArgument);
        //if (e.CommandName == "ReviewRow")
        //{
        //    int regId = (int)this.gvTask.DataKeys[index].Values["REG_ID"];
        //    if (regId > 0)
        //    {
        //        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //        DataSet dsProvider = psc.SelectRegistrationData(regId, "PROVIDER");
        //        DataSet dsServiceLocation = psc.SelectRegistrationData(regId, "SERVICE_LOCATION");
        //        if (Helper.HasRows(dsProvider))
        //        { 
        //            DataRow rowProvider = dsProvider.Tables[0].Rows[0];
        //            lblProviderName.Text = Helper.GetString("NAME", rowProvider);
        //            lblAddress.Text = string.Format("{0}{1}", Helper.GetString("CONTACT_ADDRESS1", rowProvider), (Helper.GetString("CONTACT_QUADRANT", rowProvider) == string.Empty) ? "" : ", " + Helper.GetString("CONTACT_QUADRANT", rowProvider));
        //            lblCityStateZip.Text = string.Format("{0}, {1}, {2}", Helper.GetString("CONTACT_CITY", rowProvider), Helper.GetString("CONTACT_STATE", rowProvider), Helper.GetString("CONTACT_ZIP", rowProvider));
        //            lblSpecialty.Text = this.gvTask.DataKeys[index].Values["SpecialtyTypeName"].ToString();
        //            lblOfficePhone.Text = Helper.GetString("CONTACT_PHONE_NUMBER", rowProvider);
        //            long phoneNumberLong = 0;
        //            if (long.TryParse(lblOfficePhone.Text, out phoneNumberLong))
        //            {
        //                lblOfficePhone.Text = String.Format("{0:###-###-####}", phoneNumberLong);
        //            }
        //            if (Helper.GetString("CONTACT_STATE", rowProvider) == "DC")
        //            {
        //                lblWardCountyLabel.Text = "Ward";
        //                lblWardCounty.Text = Helper.GetString("CONTACT_WARD", rowProvider);
        //            }
        //            else
        //            {
        //                lblWardCountyLabel.Text = "County";
        //                lblWardCounty.Text = Helper.GetString("CONTACT_COUNTY", rowProvider);
        //            }
        //            hlGoogleMaps.NavigateUrl = string.Format("http://maps.google.com/maps?f=q&hl=en&q={0},{1}", lblAddress.Text, lblCityStateZip.Text); //1310 SOUTHERN AVENUE,WASHINGTON,DC,20032";
        //        }

        //        if (Helper.HasRows(dsServiceLocation))
        //        { 
        //            DataRow rowServiceLocation = dsServiceLocation.Tables[0].Rows[0];
        //            lblOfficeMon.Text = Helper.GetString("OFFICE_MON", rowServiceLocation);
        //            lblOfficeTue.Text = Helper.GetString("OFFICE_TUE", rowServiceLocation);
        //            lblOfficeWed.Text = Helper.GetString("OFFICE_WED", rowServiceLocation);
        //            lblOfficeThu.Text = Helper.GetString("OFFICE_THU", rowServiceLocation);
        //            lblOfficeFri.Text = Helper.GetString("OFFICE_FRI", rowServiceLocation);
        //            lblOfficeSat.Text = Helper.GetString("OFFICE_SAT", rowServiceLocation);
        //            lblOfficeSun.Text = Helper.GetString("OFFICE_SUN", rowServiceLocation);
        //        }

        //            this.mpeReviewProvider.Show();
        //    }
        //}

    }

    protected void gvTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (e.Row.Cells[5].Text == "9999999999")
        //    {
        //        e.Row.Cells[5].Text = string.Empty;
        //    }

        //    LinkButton lnkExpress = (LinkButton)e.Row.FindControl("lnkExpressAdmin");
        //    if (lnkExpress != null)
        //    {
        //        //this control is hidden on page load if user role is not allowed to perform express operations.
        //        string currentStepID = DataBinder.Eval(e.Row.DataItem, "CurrentStepID").ToString();
        //        string regUserID = DataBinder.Eval(e.Row.DataItem, "UserID").ToString();
        //        int regPgmStatusTypeID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "REG_PROGRAM_STATUS_TYPE_ID").ToString());
        //        var entitytypeIdStr = DataBinder.Eval(e.Row.DataItem, "ENTITY_TYPE_ID").ToString();
        //        int entityTypeID = string.IsNullOrEmpty(entitytypeIdStr) ? 0 : int.Parse(entitytypeIdStr);

        //        bool regOkToTerm = CanTermRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID);
        //        bool regOkToRetroDate = CanRetroDateRegistration(currentStepID, regUserID, regPgmStatusTypeID, entityTypeID);

        //        lnkExpress.Visible = regOkToTerm || regOkToRetroDate;
        //    }
        //}
    }

    private void BindDropDown()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.WF_SelectWorkflows();
        Helper.LoadList(ddlWorkflow, ds.Tables[0], "WORKFLOW_NAME", "WORKFLOW_ID", true);         
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //this.mpeReviewProvider.Hide();    
        //gvTask.SelectRow(-1);
    }
    protected void hlGoogleMaps_Click(object sender, EventArgs e)
    {

    }
}