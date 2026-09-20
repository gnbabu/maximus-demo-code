using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_Reimbursement : BaseSectionControl  
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
            return ViewState["RegVisionProiverID"] == null ? 0 : Convert.ToInt32(ViewState["REG_VISION_PROVIDER_DETAILS_ID"]);
        }
        set
        {
            ViewState["RegVisionProiverID"] = value;
        }
    }
    #endregion

    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;

    public delegate void KeepOpenEventHandler();
    public event KeepOpenEventHandler KeepOpenEvent;

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {


    }

     #region Public Methods
    public  override void LoadData(DataRow row)
    {
        // TODO: EDV This code needs fixing 
        tblreimburse.Visible = true;
        string id = hidID.Text;
        lblServiceType.Text = row["Type_Of_Service"].ToString();

        txtPayment.Text = (row["Paymment_Method"]).ToString();
        txtEffectiveDate.Text = (row["Effective_Date"]).ToString();

        hidIsEdit.Text = false.ToString();
        hidID.Text = row["REG_REIMBURSEMENT_ID"].ToString();
        
    }

    public override bool SaveData()
    {
        return SaveData(rblPhysician.SelectedValue, string.Empty);
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        reimbursementDetail.Visible = true;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);
    }

    public bool SaveData(string rblPhy, string rblSep)
    {
        // the selected value is coming as 0 instead of null
        if (!(string.Equals(rblPhy, bool.FalseString, StringComparison.CurrentCultureIgnoreCase) || string.Equals(rblPhy, bool.TrueString, StringComparison.CurrentCultureIgnoreCase)))
            rblPhy = null;

        if (!(string.Equals(rblPhy, bool.FalseString, StringComparison.CurrentCultureIgnoreCase) || string.Equals(rblPhy, bool.TrueString, StringComparison.CurrentCultureIgnoreCase)))
            rblSep = null;

        bool rtn = true;
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text); 

        Page.Validate("valReimbursement");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valReimbursement") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_REIMBURSEMENT_ID", Convert.ToString(hidID.Text));
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("Effective_Date", txtEffectiveDate.Text);
        parms.Add("PerDiem_VisitAll", ddlOption.SelectedValue);
        parms.Add("Paymment_Method", txtPayment.Text);
        parms.Add("Physician_Service", rblPhy);
        parms.Add("Separately", rblSep);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        svc.UpdateRegistrationDataTable("REIMBURSEMENT", parms);

        //Update two columns same for all, not very efficient solution, need to redesign
        Dictionary<string, string> parms1 = new Dictionary<string, string>();
       
        parms1 = new Dictionary<string, string>();
        parms1.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        // parms1.Add("REG_REIMBURSEMENT_ID", Convert.ToString(i));
        parms1.Add("Physician_Service", rblPhy);
        parms1.Add("Separately", rblSep);
        svc.UpdateRegistrationDataWithParams("updateREG_REIMBURSEMENTCustom", parms1);

        return rtn;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadReimbursementRates();
    }

    private void LoadReimbursementRates()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(Convert.ToInt32(this.WorkflowPage.RegistrationId), "REIMBURSEMENT");

        // Insert a blank row for each of the service types if they don't exist
        if (!Helper.HasRows(ds))
        {
            string[] arr1 = new string[] { "InPatient", "OutPatient", "EmergencyRoom" };
            for (int i = 0; i < arr1.Length; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("Type_Of_Service", arr1[i]);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                psc.InsertRegistrationData(Convert.ToInt32(this.WorkflowPage.RegistrationId.ToString()), "REIMBURSEMENT", parms);
            }
        }

        // Now retrieve the reimbursement records
        ds = psc.SelectRegistrationData(Convert.ToInt32(this.WorkflowPage.RegistrationId), "REIMBURSEMENT");
        DataTable dtReimbursement = Helper.HasRows(ds) ? ds.Tables[0] : null;
        grdReimbursement.DataSource = this.DataList = dtReimbursement;
        grdReimbursement.DataBind();

        if (dtReimbursement != null)
        {
            if (dtReimbursement.Rows[0]["Physician_Service"].ToString() == bool.TrueString)
                rblPhysician.SelectedIndex = 0;
            else if (dtReimbursement.Rows[0]["Physician_Service"].ToString() == bool.FalseString)
                rblPhysician.SelectedIndex = 1;
            //Commenting below lines as we no longer needed sperately radio button.
            //if (dtReimbursement.Rows[0]["Separately"].ToString() == bool.TrueString)
            //    rblSeparately.SelectedIndex = 0;
            //else if (dtReimbursement.Rows[0]["Physician_Service"].ToString() == bool.FalseString)
            //    rblSeparately.SelectedIndex = 1;
        }
    }


    public override bool ValidateData()
    {
        return true;
    }


    #endregion
    #endregion
   
    //protected void btnSave_Click(object sender, EventArgs e)
    //{

    //}
    //protected void btnCancel_Click(object sender, EventArgs e)
    //{

    //}

    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (!string.IsNullOrEmpty(ddlOption.SelectedValue) || !string.IsNullOrEmpty(txtPayment.Text) || !string.IsNullOrEmpty(txtEffectiveDate.Text))
            isRequired = true;
        return isRequired;
    }

    public override string ValidationGroup
    {
        get { return "valReimbursement"; }
    }

    public override string Title
    {
        get { return "Reimbursement Rates"; }
    }

    public override string IdText
    {
        get { return "ucReimbursement_" + this.WorkflowPage.RegistrationId; }
    }

}