using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_DataLookup : System.Web.UI.UserControl
{
    public delegate void RegistrationViewEventHandler(int registrationId);
    public event RegistrationViewEventHandler RegistrationViewEvent;

    protected void Page_PreInit(object sender, EventArgs e)
    {

    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.ddlDataLookupTableId.SelectedValue == "" ||
            ((string.IsNullOrEmpty(this.txtRegId.Text) || Convert.ToInt32(this.txtRegId.Text) < 1) && string.IsNullOrEmpty(this.txtMedicaidId.Text)))
        {
            Clear_GridSection();
            return;
        }
        RefreshData();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear_SearchFields();
        Clear_GridSection();
    }

    protected void lnkReview_Click(object sender, CommandEventArgs e)
    {
        if (("ReviewRow").Equals(e.CommandName))
        {
            if (RegistrationViewEvent != null) RegistrationViewEvent(Convert.ToInt32(e.CommandArgument.ToString()));
        }
    }

    protected void gvDataLookup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //toggle Add New button visibility based on provider type
        LinkButton btnAddNew = (LinkButton)e.Row.FindControl("btnAddNew");
        if (btnAddNew != null)
        {
            btnAddNew.Visible = false;
            if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IS_PROVIDER_TYPE_HOSPITAL")))
            {
                btnAddNew.Visible = true;
            }
        }
    }

    protected void gvDataLookup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddNewFacilityNumber")
        {
            AddFacilityNumber();
        }
        else 
        if (e.CommandName == "ProviderRecord")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int regId = (int)this.gvDataLookup.DataKeys[index].Values["REG_ID"];
            if (regId > 0)
            {
                (this.Page as RegistrationProvider).RegistrationId = regId;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                (this.Page as RegistrationProvider).CommandName = "ProviderRecord";
                if (RegistrationViewEvent != null) RegistrationViewEvent(regId);
                string logMsg = String.Format("GroupReview RegID - " + regId.ToString() + " by " + HttpContext.Current.User.Identity.Name + ", Role(s) - " + String.Join(", ", Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name)));
                Logging log = new Logging(Guid.NewGuid(), logMsg);
                log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));
            }
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        DataTable dt = GetData();
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();

        RadGridExport.MasterTableView.ExportToExcel();
    }

    private void RefreshData()
    {
        DataTable dt = GetData();
        gvDataLookup.DataSource = dt;
        gvDataLookup.VirtualItemCount = 1;
        gvDataLookup.DataBind();

    }


    private void AddFacilityNumber()
    {
        try
        {
            DataTable dt = GetData();
            DataRow dr = dt.Rows[0];

            int regId = Convert.ToInt32(dr["REG_ID"]);
            string facilityNumberType = Convert.ToString(dr["FACILITY_NO_TYPE"]);
            int regAddressId = Convert.ToInt32(dr["REG_ADDRESS_ID"]);
            DateTime lastModifiedDate = DateTime.Now;
            string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.InsertRegAddressHospitalNo(regId, facilityNumberType, regAddressId, lastModifiedDate, lastModifiedUser);
            RefreshData();
        }
        catch (Exception ex)
        {
            Logging log = new Logging(Guid.NewGuid(), ex.Message);
            log.CreateLogEntry(string.Format(ex.Message, Logging.LogPriority.Error));
            throw;
        }
    }

    private DataTable GetData()
    {
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = new DataSet();
            int regId = string.IsNullOrEmpty(this.txtRegId.Text) ? 0 : Convert.ToInt32(this.txtRegId.Text);
            string medId = string.IsNullOrEmpty(this.txtMedicaidId.Text) ? null : this.txtMedicaidId.Text;
            ds = psc.GetRegAddressHospitalNoByRegIdOrMedicaidId(regId, medId);
            if (Helper.HasRows(ds))
            {
                lnkExcel.Visible = true;
                return ds.Tables[0];
            }
            else
            {
                lnkExcel.Visible = false;
                return new DataTable();
            }
        }
        catch (Exception ex)
        {
            Logging log = new Logging(Guid.NewGuid(), ex.Message);
            log.CreateLogEntry(string.Format(ex.Message, Logging.LogPriority.Error));
            throw;
        }
    }

    private void Clear_SearchFields()
    {
        this.txtRegId.Text = "";
        this.txtMedicaidId.Text = "";
    }

    private void Clear_GridSection()
    {
        gvDataLookup.DataSource = null;
        gvDataLookup.DataBind();
        lnkExcel.Visible = false;
    }

}