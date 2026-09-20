using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

public partial class PopupControls_SpecialtiesTaxonomies : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void ReloadPopupEventHandler();
    public event ReloadPopupEventHandler ReloadPopupEvent;
    public bool OwnershipChangedFlag { get; set; }

    public bool IsPrimary
    {
        get
        {
            return HasPrimaryTaxonomy();
        }
    }

    public int ProviderTypeId
    {
        get
        {
            return this.WorkflowPage.ProviderTypeID;
        }
    }

    protected bool HasPrimaryTaxonomy()
    {
        bool rtn = false;

        if (Helper.HasRows(this.DataList))
        {
            DataRow[] rowsPrimary = this.DataList.Select("PRIMARY_FLAG = 1");
            if (rowsPrimary != null && rowsPrimary.Length > 0)
                rtn = true;
        }

        return rtn;
    }

    private void LoadTaxonomiesDropdown()
    {
        if (ddlSpecialty.SelectedIndex > 0 && !string.IsNullOrEmpty(ddlSpecialty.SelectedValue))
        {
            // Lookup the Provider Types by the Category that was selected
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectTaxonomyTypesBySpecProvType(Convert.ToInt32(ddlSpecialty.SelectedValue), ProviderTypeId);
            if (Helper.HasRows(ds))
            {
                Dictionary<int, string> taxonomyMap = new Dictionary<int, string>();
                ds.Tables[0].DefaultView.Sort = "TAXONOMY_CODE";

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    taxonomyMap.Add( Convert.ToInt32(row["TAXONOMY_TYPE_ID"]), row["TAXONOMY_CODE"] as string + " - " + row["TAXONOMY_NAME"] as string);
                }
                ddlTaxonomy.DataSource = taxonomyMap;
                ddlTaxonomy.DataTextField = "Value";
                ddlTaxonomy.DataValueField = "Key";
                ddlTaxonomy.DataBind();
            }
        }
        else
        {
            ddlTaxonomy.Items.Clear();
            ddlTaxonomy.DataSource = null;
            ddlTaxonomy.DataBind();
        }
    }

    private void LoadTaxonomies()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", "1");
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_TAXONOMY", parms);
        if (Helper.HasRows(ds)) grdTaxonomies.DataSource = this.DataList = ds.Tables[0];
        else grdTaxonomies.DataSource = this.DataList = null;
        grdTaxonomies.DataBind();

        btnAddTaxonomies.Visible = true;
        btnTaxonomiesHistory.Visible = (grdTaxonomies.Rows.Count > 0);
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadTaxonomies();
    }

    public override void LoadData(DataRow dr)
    {
        lblSameAsPrimary.Visible = false;

        ParentTable.Rows[0].Cells[2].Style["display"] = Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        if (ddlSpecialty.Items.Count == 0)
        {
            ds = psc.SelectGroupSpecialtiesByProviderType(ProviderTypeId);
            Helper.LoadList(ddlSpecialty, ds.Tables[0], "SPECIALTY_TYPE_NAME", "SPECIALTY_TYPE_ID", true);
        }

        hdnRegSpecialtyID.Value = hdnRegTaxonomyID.Value = string.Empty;
        if (dr != null)
        {
            hdnRegSpecialtyID.Value = Helper.GetData("REG_SPECIALTY_ID", dr);
            hdnRegTaxonomyID.Value = Helper.GetData("REG_TAXONOMY_ID", dr);
            int specialtyTypeID;

            if (!string.IsNullOrEmpty(Helper.GetData("SPECIALTY_TYPE_ID", dr)) &&
                 ddlSpecialty.Items.FindByValue(Helper.GetData("SPECIALTY_TYPE_ID", dr)) != null)
            {
                specialtyTypeID  = Helper.GetInt("SPECIALTY_TYPE_ID", dr);
                ddlSpecialty.SelectedValue = specialtyTypeID.ToString();
                ddlSpecialty.Enabled = Helper.EnableSpecialtyType(specialtyTypeID) || !IsPrimary;
            }

            LoadTaxonomiesDropdown();

            if (Helper.GetInt("TAXONOMY_TYPE_ID", dr) > 0)
            {
                if (ddlTaxonomy.Items.FindByValue(Helper.GetString("TAXONOMY_TYPE_ID", dr)) != null)
                    ddlTaxonomy.SelectedValue = Helper.GetString("TAXONOMY_TYPE_ID", dr);
            }
        }
        else
        {
            ddlSpecialty.SelectedIndex = 0;
            LoadTaxonomiesDropdown();
        }
    }

    public override bool SaveData()
    {
        try
        {
            int regTaxonomyId = 0;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            bool duplicate = psc.VerifyDuplicateTaxonomySpecialtyForReg(this.WorkflowPage.RegistrationId, int.Parse(ddlTaxonomy.SelectedValue), int.Parse(ddlSpecialty.SelectedValue));
            if (!duplicate)
            {
                // Update the Taxonomy
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("TAXONOMY_TYPE_ID", ddlTaxonomy.SelectedValue);
                parms.Add("PRIMARY_FLAG", (IsPrimary ? "1" : "0"));
                parms.Add("START_DATE", DateTime.Now.ToShortDateString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                if (!string.IsNullOrEmpty(hdnRegTaxonomyID.Value))
                {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                    parms.Add("REG_TAXONOMY_ID", hdnRegTaxonomyID.Value);
                    psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMY", parms);
                }
                else
                {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                    regTaxonomyId = psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMYCustom", parms);
                }

                // Update the Specialty
                parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("PRIMARY_FLAG", (IsPrimary ? "1" : "0"));
                parms.Add("SPECIALTY_TYPE_ID", ddlSpecialty.SelectedValue);
                parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                parms.Add("START_DATE", DateTime.Now.ToShortDateString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value))
                {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                    parms.Add("REG_SPECIALTY_ID", hdnRegSpecialtyID.Value);
                    parms.Add("REG_TAXONOMY_ID", hdnRegTaxonomyID.Value);
                    psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY", parms);
                }
                else
                {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                    parms.Add("REG_TAXONOMY_ID", regTaxonomyId.ToString());
                    psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom2", parms);
                }
                return true;
            }
            else
            {
                lblSameAsPrimary.Visible = true;
                return false;
            }
        }
        catch (Exception ex)
        {
            throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucSpecialitiesTaxonomies_SaveData - " + ex.Message));
        }
    }

    public override bool ValidateData()
    {
        return true;
    }

    protected void ddlSpecialty_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlUpdate.Update();
        LoadTaxonomies();
        pnlUpdate.Update();
        if (ReloadPopupEvent != null) ReloadPopupEvent();
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        taxonomyDetail.Visible = true;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);
    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        taxonomyDetail.Visible = true;
        this.LoadData(null);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        // TODO: EDV Here is where we should show/hide history

    }

    public override string ValidationGroup
    {
        get { return "valSpecialtiesTaxonomies"; }
    }

    public override string Title
    {
        get { return "Edit Taxonomies"; }
    }

    public override string IdText
    {
        get { return "ucTaxonomies_" + this.WorkflowPage.RegistrationId; }
    }
}