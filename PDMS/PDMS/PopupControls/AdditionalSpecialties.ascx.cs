using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_AdditionalSpecialties : BasePopupControl
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

    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ADDTL_SPECIALTY");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    public bool SaveData()
    {
        Page.Validate("AdditionalSpecialties");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v!= null && v.ValidationGroup.Equals("AdditionalSpecialties") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PRIMARY_FLAG", "0");
        parms.Add("SPECIALTY_TYPE_ID", prov_Description.SelectedValue);
        parms.Add("SPECIALTY_BOARD_CERTIFIED", prov_Certified.Checked ? "Y" : "N");
        parms.Add("START_DATE", DateTime.Now.ToShortDateString());
        parms.Add("END_DATE", prov_End.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
        if (isEdit)
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_SPECIALTY_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY", parms);
        }
        else
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY", parms);
        }
        return true;
    }

    public void LoadData(DataRow row, bool isEdit, string existingSpecialties)
    {
        #region Specialty Types
        prov_Description.DataTextField = "SPECIALTY_TYPE_NAME";
        prov_Description.DataValueField = "SPECIALTY_TYPE_ID";

        string ptype = svc.GetProviderTypeByRegId(this.WorkflowPage.RegistrationId);
        if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupMultiSpecialty))
            prov_Description.DataSource = SpecTypes.Select("MMIS_SPECIALTY_TYPE_ID = '082'").CopyToDataTable();
        else if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupSingleSpecialty))
            prov_Description.DataSource = SpecTypes.Select("MMIS_SPECIALTY_TYPE_ID <> '082' AND MMIS_SPECIALTY_TYPE_ID IS NOT NULL").CopyToDataTable();
        else
            prov_Description.DataSource = SpecTypes.Select(existingSpecialties).CopyToDataTable();
        prov_Description.DataBind();
        prov_Description.Items.Insert(0, string.Empty);
        #endregion

        prov_Start.Text = DateTime.Now.ToShortDateString();

        bool isPend = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
        {
            prov_Description.Enabled =
            prov_Certified.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = !isPend;
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            prov_Description.Enabled =
            prov_Certified.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = true;
        }
        else
        {
            prov_Description.Enabled =
            prov_Certified.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = false;
        }


        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            // Load Provider values
            hidID.Text = row["REG_SPECIALTY_ID"].ToString();
            prov_Description.SelectedValue = row["SPECIALTY_TYPE_ID"].ToString();
            prov_Start.Text = Helper.FormatDate2(row["START_DATE"].ToString());
            prov_End.Text = Helper.FormatDate2(row["END_DATE"].ToString());
            prov_Certified.Checked = row["SPECIALTY_BOARD_CERTIFIED"] != null ? row["SPECIALTY_BOARD_CERTIFIED"].ToString().ToLower().Contains('y') : false;
            pdms_Description.Text = row["PDMS_SPECIALTY_TYPE_NAME"] == null ? string.Empty : row["PDMS_SPECIALTY_TYPE_NAME"].ToString();
        }
        else
        {
            hidID.Text = string.Empty;
            prov_Start.Text = string.Empty;
            prov_End.Text = string.Empty;
            prov_Certified.Checked = false;
        }
    }

    protected void ValidateLC23_ERR23(object sender, ServerValidateEventArgs e)
    {
        SetDt();

        DateTime startA = string.IsNullOrEmpty(prov_Start.Text) ? DateTime.MinValue : Convert.ToDateTime(prov_Start.Text);
        DateTime endA = string.IsNullOrEmpty(prov_End.Text) ? DateTime.MaxValue : Convert.ToDateTime(prov_End.Text);
        
        if (startA > endA)
        {
            e.IsValid = false;
            return;
        }

        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count - 1; i++)
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

    protected void ValidateLC24(object sender, ServerValidateEventArgs e)
    {
        SetDt();
        //if (!string.IsNullOrEmpty(prov_Description.SelectedValue.Trim()))
        //{
            e.IsValid = !string.IsNullOrEmpty(prov_Start.Text);
        //}
        //else
        //{
        //    e.IsValid = true;
        //}
    }

    protected void ValidateLC26(object sender, ServerValidateEventArgs e)
    {
        SetDt();
        if (!string.IsNullOrEmpty(prov_Description.SelectedValue.Trim()))
        {
            e.IsValid = prov_Certified.Checked;
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void Validate_DescriptionRequired(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = true; //LC23 Mandatory says no (8/2/2013) !string.IsNullOrEmpty(prov_Description.Text);
    }

    private string GetDescriptionText(string id)
    {
        int i;
        if (!int.TryParse(id, out i))
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
}