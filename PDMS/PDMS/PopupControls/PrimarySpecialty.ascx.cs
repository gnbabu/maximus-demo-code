using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PrimarySpecialty : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }

    private DataTable _SpecTypes;
    private DataTable SpecTypes
    {
        get
        {
            if (_SpecTypes == null)
            {
                _SpecTypes = svc.SelectSpecialtyTypes().Tables[0];
            }

            return _SpecTypes;
        }
    }
    private DataTable dt;
    
    public bool SaveData()
    {
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("Specialty") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PRIMARY_FLAG", "1");
        parms.Add("SPECIALTY_TYPE_ID", prov_Description.SelectedValue);
        parms.Add("SPECIALTY_BOARD_CERTIFIED", prov_Certified.Checked ? "Y" : "N");
        parms.Add("START_DATE", prov_Start.Text);
        parms.Add("END_DATE", prov_End.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (isEdit)
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_SPECIALTY_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY", parms);
        }
        else
        {
            if (ViewState["REG_SPECIALTY_ID"] != null)//If adding entry when one exists, end date the previous entry.
            {
                Dictionary<string, string> parms1 = new Dictionary<string, string>();
                parms1.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms1.Add("PRIMARY_FLAG", "1");
                parms1.Add("SPECIALTY_TYPE_ID", ViewState["SPECIALTY_TYPE_ID"].ToString());
                parms1.Add("SPECIALTY_BOARD_CERTIFIED", ViewState["SPECIALTY_BOARD_CERTIFIED"].ToString());
                parms1.Add("START_DATE", ViewState["START_DATE"].ToString());
                parms1.Add("END_DATE", DateTime.Now.ToShortDateString());
                parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms1.Add("REG_SPECIALTY_ID", ViewState["REG_SPECIALTY_ID"].ToString());
                parms1.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY", parms1);
            }

            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYcustom", parms);
        }

        return true;
    }

    public void LoadData(DataRow row, bool isEdit, string existingSpecialties)
    {
        #region Specialty Types
        prov_Description.DataTextField = "SPECIALTY_TYPE_NAME";
        prov_Description.DataValueField = "SPECIALTY_TYPE_ID";

        string userId = string.IsNullOrEmpty(SessionVarRetriever.UserIdSelected) ? Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString() : SessionVarRetriever.UserIdSelected;
        string ptype = Helper.GetProviderTypeName(this.WorkflowPage.RegistrationId);
        DataTable dt = new DataTable();
        if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupMultiSpecialty))
            dt = SpecTypes.Select("MMIS_SPECIALTY_TYPE_ID = '082'").CopyToDataTable();
        else if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupSingleSpecialty))
            dt = SpecTypes.Select("MMIS_SPECIALTY_TYPE_ID <> '082' AND MMIS_SPECIALTY_TYPE_ID IS NOT NULL").CopyToDataTable();
        else
            dt = SpecTypes.Select(existingSpecialties).CopyToDataTable();

        DataView dv = dt.AsDataView();
        dv.Sort = "SPECIALTY_TYPE_NAME asc";
        
        prov_Description.DataSource = dv.ToTable();
        prov_Description.DataBind();
        prov_Description.Items.Insert(0, string.Empty);
        #endregion

        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);

        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            prov_Description.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled =
            prov_Certified.Enabled = true;
        }
        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator))
        {
            prov_Description.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled =
            prov_Certified.Enabled = !regIsPending;
        }
        else
        {
            prov_Description.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled =
            prov_Certified.Enabled = false;
        }

        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            hidID.Text = row["REG_SPECIALTY_ID"].ToString();
            prov_Description.SelectedValue = row["SPECIALTY_TYPE_ID"].ToString();
            prov_Start.Text =  Helper.FormatDate2(row["START_DATE"].ToString());
            prov_End.Text =  Helper.FormatDate2(row["END_DATE"].ToString());
            prov_Certified.Checked = row["SPECIALTY_BOARD_CERTIFIED"] != null ? row["SPECIALTY_BOARD_CERTIFIED"].ToString().ToLower().Contains('y') : false;
            pdms_Description.Text = row["PDMS_SPECIALTY_TYPE_NAME"] == null ? string.Empty : row["PDMS_SPECIALTY_TYPE_NAME"].ToString();

        }
        else
        {
            if (row != null)
            {
                ViewState.Add("REG_SPECIALTY_ID", row["REG_SPECIALTY_ID"].ToString());
                ViewState.Add("SPECIALTY_TYPE_ID", row["SPECIALTY_TYPE_ID"].ToString());
                ViewState.Add("SPECIALTY_BOARD_CERTIFIED", row["SPECIALTY_BOARD_CERTIFIED"].ToString());
                ViewState.Add("START_DATE", row["START_DATE"].ToString());
                ViewState.Add("END_DATE", DateTime.Now.ToShortDateString());
            }

            hidID.Text = string.Empty;
            prov_Description.Text = string.Empty;
            prov_Start.Text = DateTime.Now.ToShortDateString();
            prov_End.Text = string.Empty;
            prov_Certified.Text = string.Empty;
        }
    }

    private string GetDescriptionText(string id)
    {
        int i;
        if(!int.TryParse(id, out i))
        {
            return string.Empty;
        }

        DataTable dt = SpecTypes.Select("SPECIALTY_TYPE_ID = " + id).CopyToDataTable();

        if (Helper.HasRows(dt))
        {
            return dt.Rows[0]["SPECIALTY_TYPE_NAME"].ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    protected void ValidateLC12_ERR22(object sender, ServerValidateEventArgs e)
    {
        SetDt();
        if (!Helper.HasRows(dt))
        {
            e.IsValid = true;
        }
        else if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            e.IsValid = true;// string.IsNullOrEmpty(row["END_DATE"].ToString());
        }
        else
        {
            e.IsValid = false;
        }
    }

    protected void ValidateLC12_ERR30(object sender, ServerValidateEventArgs e) 
    {
        SetDt();
        DateTime startA = string.IsNullOrEmpty(prov_Start.Text) ? DateTime.MinValue : Convert.ToDateTime(prov_Start.Text);
        DateTime endA = string.IsNullOrEmpty(prov_End.Text) ? DateTime.MaxValue : Convert.ToDateTime(prov_End.Text);

        if (startA > endA)
        {
            e.IsValid = false;
        }

        if (Helper.HasRows(dt))
        {
            //check dtAdditional for overlap
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
                if (isEdit)
                    continue;

                if (prov_Description.SelectedValue.Equals(dt.Rows[i]["SPECIALTY_TYPE_ID"].ToString()))
                {
                    DateTime startB = string.IsNullOrEmpty(dt.Rows[i]["START_DATE"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[i]["START_DATE"].ToString());
                    DateTime endB = string.IsNullOrEmpty(dt.Rows[i]["END_DATE"].ToString()) ? DateTime.MaxValue : Convert.ToDateTime(dt.Rows[i]["END_DATE"].ToString());

                    if (endA < startA || endB < startB)
                    {
                        e.IsValid = false;
                    }
                    else if (startA < startB)
                    {
                        e.IsValid = endA < startB;
                    }
                    else if (startA > startB)
                    {
                        e.IsValid = startA > endB;
                    }
                    else
                    {
                        e.IsValid = false;
                    }
                }
            }
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void Validate_RequireDescription(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = prov_Description.Text.Trim().Length > 0;
    }

    protected void ValidateLC13_ERR21(object sender, ServerValidateEventArgs e)
    {
        SetDt();
        DataSet reg = svc.SelectRegistration(this.WorkflowPage.RegistrationId);
        DateTime regEff = string.IsNullOrEmpty(reg.Tables[0].Rows[0]["REQUESTED_EFFECTIVE_DATE"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(reg.Tables[0].Rows[0]["REQUESTED_EFFECTIVE_DATE"].ToString());

        if (regEff == DateTime.MinValue)
        {
            e.IsValid = true;
            return;
        }

        DateTime start = string.IsNullOrEmpty(prov_Start.Text) ? DateTime.MinValue : Convert.ToDateTime(prov_Start.Text);
        DateTime end = string.IsNullOrEmpty(prov_End.Text) ? DateTime.MaxValue : Convert.ToDateTime(prov_End.Text);

        e.IsValid = (DateTime.MinValue < regEff && start <= regEff && regEff <= end) || (end == DateTime.MaxValue);
    }

    protected void LC116_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void Validate_Certified(object sender, ServerValidateEventArgs e)
    {
        if (prov_Description.SelectedIndex > 0)
        {
            e.IsValid = prov_Certified.Checked;
        }
        else
        {
            e.IsValid = false;
        }
    }
}