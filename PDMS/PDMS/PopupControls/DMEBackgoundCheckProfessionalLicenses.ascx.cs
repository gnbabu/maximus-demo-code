using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class PopupControls_DMEBackgoundCheckProfessionalLicenses : BasePopupControl
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

    public int RegDMEBackgoundCheckProfessionalLicensesID
    {
        get
        {
            return ViewState["RegDMEBackgoundCheckProfessionalLicensesID"] == null ? 0 : Convert.ToInt32(ViewState["RegDMEBackgoundCheckProfessionalLicensesID"]);
        }
        set
        {
            ViewState["RegDMEBackgoundCheckProfessionalLicensesID"] = value;
        }
    }
    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();

    public delegate void KeepOpenEventHandler();

    public delegate void SaveEventHandler();

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion



    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }



    private void ValidateData()
    {

    }



    #endregion

    #region Public Methods

    public override void LoadData(DataRow dr)
    {


        if (dr != null)
        {
            txtName.Text = Helper.GetString("Name", dr);
            if(Helper.GetString("IsBackgroundCheck", dr) == "True")
            ddlBackgroundOrProfessionalLicense.SelectedValue = "BackgroundCheck";
            if (Helper.GetString("IsProfessionalLicense", dr) == "True")
                ddlBackgroundOrProfessionalLicense.SelectedValue = "ProfessionalLicense";


            RegDMEBackgoundCheckProfessionalLicensesID = Helper.GetInt("REG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO_ID", dr);
        }
        


    }

    #endregion





    public void InitializeFields()
    {
        
        txtName.Text = string.Empty;
        ddlBackgroundOrProfessionalLicense.SelectedValue = string.Empty;
        

        RegDMEBackgoundCheckProfessionalLicensesID = 0;

    }

    private void EnableAllFields(bool enable)
    {

    }






    public bool SaveData()
    {
        bool rtn = true;
        this.ValidateData();
        Page.Validate("valDMEBackgoundCheck");

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return false;
        }
        


        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        

        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (RegDMEBackgoundCheckProfessionalLicensesID != 0)
            parms.Add("REG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO_ID", RegDMEBackgoundCheckProfessionalLicensesID.ToString());

        if(ddlBackgroundOrProfessionalLicense.SelectedValue == "BackgroundCheck")
        {
            parms.Add("IsBackgroundCheck", "1");
            parms.Add("IsProfessionalLicense", "0");

        }
        else if (ddlBackgroundOrProfessionalLicense.SelectedValue == "ProfessionalLicense")
        {
            parms.Add("IsBackgroundCheck", "0");
            parms.Add("IsProfessionalLicense", "1");
        }

        
        if (txtName.Text != "" && txtName.Text != String.Empty && txtName.Text != null)
        {
            parms.Add("Name", txtName.Text);
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (RegDMEBackgoundCheckProfessionalLicensesID > 0)
        {
            // Update

            psc.UpdateRegistrationDataTable("DME_BACKGROUND_CHK_PROFESSIONAL_INFO", parms);
        }
        else
        {
            // Insert
            psc.InsertRegistrationDataTable("DME_BACKGROUND_CHK_PROFESSIONAL_INFO", parms);
        }

        return rtn;
    }



    private void InitFormFields()
    {


        this.txtName.Text = string.Empty;
        ddlBackgroundOrProfessionalLicense.SelectedIndex = -1;


        
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




}