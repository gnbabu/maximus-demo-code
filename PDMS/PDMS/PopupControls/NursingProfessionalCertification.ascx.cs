using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_NursingProfessionalCertification : BaseSectionControl, INursingProfessionalCertificationsView 
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
    #endregion
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

    public int RegNursingProfessionalCertificationID
    {
        get
        {
            return ViewState["RegNursingProfessionalCertificationID"] == null ? 0 : Convert.ToInt32(ViewState["RegNursingProfessionalCertificationID"]);
        }
        set
        {
            ViewState["RegNursingProfessionalCertificationID"] = value;
        }
    }
    #region Parent Page Events

    public delegate void ValidationEventHandler();


    public delegate void ErrorEventHandler();


    public delegate void KeepOpenEventHandler();


    public delegate void SaveEventHandler();


    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion
    private NursingProfessionalCertificationsPresenter _presenter;

    public NursingProfessionalCertificationsPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new NursingProfessionalCertificationsPresenter(this);
            }

            return _presenter;
        }
    }

    public NursingProfessionalCertifications Model { get; set; }


    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }


    #endregion

    #region Public Methods
    public bool CanUserViewDelete()
    {

        return Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
    }
    private void LoadNursingCerts()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "Nursing_Professional_Certification");
        DataTable dtNursingCerts = Helper.HasRows(ds) ? ds.Tables[0] : null;
        grdNursingProfessionalCertification.DataSource = this.DataList = dtNursingCerts;
        grdNursingProfessionalCertification.DataBind();
        btnAddNursingProfessionalCertification.Visible = true;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadNursingCerts();
    }

    public override void LoadData(DataRow dr)
    {

        if (dr != null)
        {
            txtCertificationName.Text = Helper.GetString("Certification_ID", dr);
            txtReceivedFrom.Text = Helper.GetString("Received_From", dr);

            txtExpirationDate.Text = Helper.GetDate("Expiration", dr);
            RegNursingProfessionalCertificationID = Helper.GetInt("REG_Nursing_Professional_Certification_ID", dr);
        }
        else
        {
            txtCertificationName.Text = "";
            txtReceivedFrom.Text = "";
            txtExpirationDate.Text = "";
        }
    }

    public override bool ValidateData()
    {
        return true;
    }

    #endregion

    public void InitializeFields()
    {
        
        txtCertificationName.Text = string.Empty;
        txtReceivedFrom.Text = string.Empty;
        txtExpirationDate.Text = string.Empty;

        RegNursingProfessionalCertificationID = 0;

    }

    private void EnableAllFields(bool enable)
    {

    }


    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        nursingDetail.Visible = true;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);
    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        nursingDetail.Visible = true;
        this.LoadData(null);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        // TODO: EDV Here is where we should show/hide history
    }

    public override bool SaveData()
    {
        bool rtn = true;
        this.ValidateData();
        Page.Validate("valNursingProfessionalCertifications");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valNursingProfessionalCertifications") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }
        NursingProfessionalCertifications nursingProfessional = new NursingProfessionalCertifications();
        if (RegNursingProfessionalCertificationID != 0)
            nursingProfessional.RegNursingProfessionalCertificationID = RegNursingProfessionalCertificationID;

        nursingProfessional.RegID = this.WorkflowPage.RegistrationId;
        nursingProfessional.CertificationName = txtCertificationName.Text;
        nursingProfessional.ReceivedFrom = txtReceivedFrom.Text;

        if (!string.IsNullOrEmpty(txtExpirationDate.Text))
            nursingProfessional.Expiration = Convert.ToDateTime(txtExpirationDate.Text);
        else
            nursingProfessional.Expiration = DateTime.MinValue;

        nursingProfessional.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);


        Model = nursingProfessional;
        presenter.SaveRegNursingProfessionalCertification(nursingProfessional);

        return rtn;
    }

    private void InitFormFields()
    {


        this.txtCertificationName.Text = this.txtReceivedFrom.Text = txtExpirationDate.Text = string.Empty;


        
    }

    private void SetFormFields(DataSet affiliationData)
    {
        /*DataRow affiliationRow = affiliationData.Tables[0].Rows[0];

        //this.RegAffiliationID = Helper.GetInt("REG_AFFILIATION_ID", affiliationRow);

        this.txtGroupName.Text = Helper.GetString("FIRST_NAME", affiliationRow);
        this.txtMedicaidID.Text = Helper.GetString("LAST_NAME", affiliationRow);
        txtTaxID.Text = Helper.GetString("SSN", affiliationRow);
        txtNPI.Text = Helper.GetString("NPI", affiliationRow);*/

    }

    public void GetRegNursingProfessionalCertification(DataSet ds)
    {
        //;
    }

    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (!string.IsNullOrEmpty(txtCertificationName.Text) || !string.IsNullOrEmpty(txtReceivedFrom.Text) || !string.IsNullOrEmpty(txtExpirationDate.Text))
            isRequired = true;
        return isRequired;
    }

    public override string ValidationGroup
    {
        get { return "valNursingProfessionalCertifications"; }
    }

    public override string Title
    {
        get { return "Nursing Certifications"; }
    }

    public override string IdText
    {
        get { return "ucNursingProfessionalCertification_" + this.WorkflowPage.RegistrationId; }
    }

}