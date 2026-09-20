using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_VisionProviders : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region svc
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

    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }

    public int RegVisionProiverID
    {
        get
        {
            return ViewState["RegVisionProiverID"] == null ? 0 : Convert.ToInt32(ViewState["RegVisionProiverID"]);
        }
        set
        {
            ViewState["RegVisionProiverID"] = value;
        }
    }
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void cvPrescribe_ServerValidate(object source, ServerValidateEventArgs args)
    {

        for (int i = 0; i < ckPrescribe.Items.Count; i++)
        {
            if (ckPrescribe.Items[i].Selected)
            {
                args.IsValid = true;
            }
        }
        args.IsValid = false;
    }
    #region Public Methods
    public override void LoadData(DataRow row)
    {
        bool isEdit = false;

        if (row == null)
            isEdit = false;
        else
            isEdit = true;


        DataSet ds = svc.SelectVisionProvidersByID(Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString()));

        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (ds.Tables[0].Rows.Count >= 1)
            {
                 DataRow datarow = ds.Tables[0].Rows[0];
                 if (Convert.ToBoolean(datarow["OnSiteLab"]))
                 rblOnSiteLab.SelectedValue = "True";
                 if (Convert.ToBoolean(datarow["TopicalPA"]))
                 ckPrescribe.Items.FindByValue("Topical").Selected = true;
                 if (Convert.ToBoolean(datarow["TherapeuticPA"]))
                 ckPrescribe.Items.FindByValue("Therapeutic").Selected = true;
                 if (Convert.ToBoolean(datarow["DignosticPA"]))
                 ckPrescribe.Items.FindByValue("Dignostic").Selected = true;

                 RegVisionProiverID = Helper.GetInt("REG_VISION_PROVIDER_DETAILS_ID", dr);
              }
            }
            hidIsEdit.Text = isEdit.ToString();
        
        
    }

    public override bool HasInputValue()
    {
        bool IsRequired = false;
        IEnumerable<string> allChecked = (from item in ckPrescribe.Items.Cast<ListItem>()
                                          where item.Selected
                                          select item.Value);
        if ((allChecked != null && allChecked.ToList<string>().Count > 0) || !string.IsNullOrEmpty(rblOnSiteLab.SelectedValue))
            IsRequired = true;
        return IsRequired;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadVisionCerts();
    }

    private void LoadVisionCerts()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //DataTable dtVisionCert = new DataTable();
        //dtVisionCert.Columns.Add("CERT_TYPE_DETAILS", typeof(String));
        //dtVisionCert.Columns.Add("ONSITELAB", typeof(String));
        bool isEdit = false;
        DataSet ds = psc.SelectVisionProvidersByID(this.WorkflowPage.RegistrationId);
        //if (ds.Tables[0].Rows.Count > 0)
        //{

        //    //concatenate the LicenseType
        //    String paType = "";
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        bool topical = (bool)ds.Tables[0].Rows[0]["TopicalPA"] ? true : false;
        //        bool therapeutic = (bool)ds.Tables[0].Rows[0]["TherapeuticPA"] ? true : false;
        //        bool dignostics = (bool)ds.Tables[0].Rows[0]["DignosticPA"] ? true : false;
        //        string onSitelab = (bool)ds.Tables[0].Rows[0]["OnSiteLab"] ? "Yes" : "No";
        //        if (topical)
        //            paType += "Topical Ocular Dignostic Pharmaceutical Agents" + "<br/>";
        //        if (therapeutic)
        //            paType += "Therapeutic Pharmaceutical Agents" + "<br/>";
        //        if (dignostics)
        //            paType += "Dignostic Pharmaceutical Agents" + "<br/>";

        //        dtVisionCert.Rows.Add(paType, onSitelab);
        //    }
        //}

        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (ds.Tables[0].Rows.Count >= 1)
            {
                DataRow datarow = ds.Tables[0].Rows[0];
                if (Convert.ToBoolean(datarow["OnSiteLab"]))
                    rblOnSiteLab.SelectedValue = "True";
                if (Convert.ToBoolean(datarow["TopicalPA"]))
                    ckPrescribe.Items.FindByValue("Topical").Selected = true;
                if (Convert.ToBoolean(datarow["TherapeuticPA"]))
                    ckPrescribe.Items.FindByValue("Therapeutic").Selected = true;
                if (Convert.ToBoolean(datarow["DignosticPA"]))
                    ckPrescribe.Items.FindByValue("Dignostic").Selected = true;

                RegVisionProiverID = Helper.GetInt("REG_VISION_PROVIDER_DETAILS_ID", dr);
                isEdit = true;
            }
        }
        else
            isEdit = false;

        hidIsEdit.Text = isEdit.ToString();

        //dtVisionCert = (Helper.HasRows(dtVisionCert)) ? dtVisionCert : null;
        //this.DataList = dtVisionCert;
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        bool rtn = true;
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text); 
        
        IEnumerable<string> allChecked1 = (from item in ckPrescribe.Items.Cast<ListItem>()
                                          where item.Selected
                                          select item.Value);
        Page.Validate("vgVisionProviders");
        if (allChecked1 == null || allChecked1.ToList<string>().Count == 0)
        {
            cvPrescribe.IsValid = false;

        }
        
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("vgVisionProviders") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        if (RegVisionProiverID > 0)
        {
            parms.Add("REG_VISION_PROVIDER_DETAILS_ID", RegVisionProiverID.ToString());
        }
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
  

        IEnumerable<string> allChecked = (from item in ckPrescribe.Items.Cast<ListItem>()
                                       where item.Selected
                                       select item.Value);

        bool topical = allChecked.Any(s => s == "Topical");
        if (topical)
        parms.Add("TopicalPA", "True");
        else
        parms.Add("TopicalPA", "False");

        bool Therapeutic = allChecked.Any(s => s == "Therapeutic");
        if (Therapeutic)
            parms.Add("TherapeuticPA", "True");
        else
            parms.Add("TherapeuticPA", "False");

        bool Dignostic = allChecked.Any(s => s == "Dignostic");
        if (Dignostic)
            parms.Add("DignosticPA", "True");
        else
            parms.Add("DignosticPA", "False");
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
     
        parms.Add("OnSiteLab", rblOnSiteLab.SelectedValue);

        if(!isEdit)
        svc.InsertDentalRegistrationData("VISION_PROVIDER_DETAILS", parms);
        else
        svc.UpdateRegistrationDataTable("VISION_PROVIDER_DETAILS", parms);


        //presenter.SaveRegVisionProviders(vProviders, false);

        return rtn;
    }
    public void GetVisionProvidersInfo(int visionProvidersID)
    {
      
    }



    #endregion
    #endregion
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    public override string ValidationGroup
    {
        get { return "vgVisionProviders"; }
    }

    public override string Title
    {
        get { return "Vision Certifications"; }
    }

    public override string IdText
    {
        get { return "ucVisionProviders_" + this.WorkflowPage.RegistrationId; }
    }

}