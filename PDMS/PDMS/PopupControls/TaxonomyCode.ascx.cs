using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_TaxonomyCode : System.Web.UI.UserControl
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

    private DataTable _TaxoCodes;
    private DataTable TaxoCodes
    {
        get
        {
            if (_TaxoCodes == null)
            {
                DataTable dt = svc.SelectTaxonomyTypes().Tables[0];
                if (Helper.HasRows(dt))
                {
                    _TaxoCodes = dt.Select("TAXONOMY_CODE <> '0'").CopyToDataTable();
                }
                else
                {
                    _TaxoCodes = dt;
                }
            }

            return _TaxoCodes;
        }
    }

    private DataTable dt;

    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMY");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    public void LoadData(DataRow row, bool isEdit, string existingTaxos)
    {
        prov_Code.DataValueField = "TAXONOMY_TYPE_ID";
        prov_Code.DataTextField = "TAXONOMY_CODE";

        string userId = string.IsNullOrEmpty(SessionVarRetriever.UserIdSelected) ? Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString() : SessionVarRetriever.UserIdSelected;
        string ptype = Helper.GetProviderTypeName(this.WorkflowPage.RegistrationId);
        if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupMultiSpecialty))
            prov_Code.DataSource = TaxoCodes.Select("TAXONOMY_CODE = '193200000X'").CopyToDataTable();
        else if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupSingleSpecialty))
            prov_Code.DataSource = TaxoCodes.Select("TAXONOMY_CODE = '193400000X'").CopyToDataTable();
        else
            prov_Code.DataSource = TaxoCodes.Select(existingTaxos).CopyToDataTable();

        prov_Code.DataBind();
        prov_Code.Items.Insert(0, new ListItem(string.Empty, string.Empty));

        prov_Description.DataValueField = "TAXONOMY_TYPE_ID";
        prov_Description.DataTextField = "TAXONOMY_NAME";

        if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupMultiSpecialty))
            prov_Description.DataSource = TaxoCodes.Select("TAXONOMY_CODE = '193200000X'").CopyToDataTable();
        else if (ptype.Equals(MAXIMUS.Core.Libraries.Constants.GroupProviderType.GroupSingleSpecialty))
            prov_Description.DataSource = TaxoCodes.Select("TAXONOMY_CODE = '193400000X'").CopyToDataTable();
        else
            prov_Description.DataSource = TaxoCodes.Select(existingTaxos).CopyToDataTable();

        prov_Description.DataBind();
        prov_Description.Items.Insert(0, new ListItem(string.Empty, string.Empty));

        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
        {
            prov_Description.Enabled =
            prov_Code.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = !regIsPending;
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            prov_Description.Enabled = 
            prov_Code.Enabled = 
            prov_Start.Enabled = 
            prov_End.Enabled = true;
        }
        else
        {
            prov_Description.Enabled =
            prov_Code.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = false;
        }

        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            hidID.Text = row["REG_TAXONOMY_ID"].ToString();
            prov_Code.SelectedValue = row["TAXONOMY_TYPE_ID"].ToString();
            prov_Description.SelectedValue = row["TAXONOMY_TYPE_ID"].ToString();
            prov_Start.Text =  Helper.FormatDate2(row["START_DATE"].ToString());
            prov_End.Text =  Helper.FormatDate2(row["END_DATE"].ToString());
            pdms_Code.Text = Helper.GetString("PDMS_TAXONOMY_CODE", row);
            pdms_Description.Text = Helper.GetString("PDMS_TAXONOMY_NAME", row);
        }
        else
        {
            if (row != null)
            {
                ViewState.Add("REG_TAXONOMY_ID", row["REG_TAXONOMY_ID"].ToString());
                ViewState.Add("TAXONOMY_TYPE_ID", row["TAXONOMY_TYPE_ID"].ToString());
                ViewState.Add("START_DATE", row["START_DATE"].ToString());
                ViewState.Add("END_DATE", DateTime.Now.ToShortDateString());
            }

            hidID.Text = string.Empty;
            prov_Code.SelectedIndex = 0;
            prov_Description.SelectedIndex = 0;
            prov_Start.Text = DateTime.Now.ToShortDateString();
            prov_End.Text = string.Empty;
        }
    }

    public bool SaveData()
    {
        Page.Validate("TaxonomyCode");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("TaxonomyCode") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("TAXONOMY_TYPE_ID", prov_Code.SelectedValue);
        parms.Add("PRIMARY_FLAG", "1");
        parms.Add("START_DATE", prov_Start.Text);
        parms.Add("END_DATE", prov_End.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
        if (isEdit)
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_TAXONOMY_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMY", parms);
        }
        else
        {
            if (ViewState["TAXONOMY_TYPE_ID"] != null)//If adding entry when one exists, end date the previous entry.
            {
                Dictionary<string, string> parms1 = new Dictionary<string, string>();
                parms1.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms1.Add("TAXONOMY_TYPE_ID", ViewState["TAXONOMY_TYPE_ID"].ToString());
                parms1.Add("PRIMARY_FLAG", "1");
                parms1.Add("START_DATE", ViewState["START_DATE"].ToString());
                parms1.Add("END_DATE", DateTime.Now.ToShortDateString());
                parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms1.Add("REG_TAXONOMY_ID", ViewState["REG_TAXONOMY_ID"].ToString());
                parms1.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMY", parms1);
            }

            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMY", parms);
        }

        return true;
    }

    protected void prov_Code_TextChanged(object sender, EventArgs e)
    {
        prov_Description.SelectedValue = prov_Code.SelectedValue;
        upProv.Update();
    }

    protected void prov_Description_TextChanged(object sender, EventArgs e)
    {
        prov_Code.SelectedValue = prov_Description.SelectedValue;
        upProv.Update();
    }

    protected void ValidateLC37(object sender, ServerValidateEventArgs e)
    {
        SetDt();
        if (prov_Code.SelectedItem == null || prov_Description.SelectedItem == null)
        {
            e.IsValid = false;
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void ValidateLC37_ERR22(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = true;
        //moving to page level
        //SetDt();
        //if (Helper.HasRows(dt))
        //{
        //    DataRow row = dt.Rows[0];
        //    e.IsValid = string.IsNullOrEmpty(row["END_DATE"].ToString());
        //}
    }

    protected void ValidateLC37_ERR30(object sender, ServerValidateEventArgs e)
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
            //check dtAdditional for overlap
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
                if (isEdit)
                    continue;

                if (prov_Code.SelectedValue.Equals(dt.Rows[i]["TAXONOMY_TYPE_ID"].ToString()))
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

    protected void ValidateLC38_ERR21(object sender, ServerValidateEventArgs e)
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

    protected void Validate_RequiredStart(object sender, ServerValidateEventArgs e)
    {
        if (prov_Code.SelectedIndex > 0)
        {
            e.IsValid = !string.IsNullOrEmpty(prov_Start.Text);
        }
    }
}