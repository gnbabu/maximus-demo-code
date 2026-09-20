using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_DMERegisteredAgent : BasePopupControl
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

    public int RegDMERegAgentID
    {
        get
        {
            return ViewState["RegDMERegAgentID"] == null ? 0 : Convert.ToInt32(ViewState["RegDMERegAgentID"]);
        }
        set
        {
            ViewState["RegDMERegAgentID"] = value;
        }
    }

          
    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();

    public delegate void KeepOpenEventHandler();
    public event KeepOpenEventHandler KeepOpenEvent;

    public delegate void SaveEventHandler();

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion



    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        ucAddress.SaveButtonClientID = Parent.FindControl("btnSave").ClientID;
        SetVisibleFields();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }



    public void ValidateData()
    {
         
    }



    #endregion

    #region Public Methods

    public override void LoadData(DataRow dr)
    {

        
        if (dr != null)
        {
            txtName.Text =  Helper.GetString("AGENT_NAME", dr);

            txtCompanyName.Text = Helper.GetString("COMPANY_NAME", dr);
            

            txtFaxNumber.Text = Helper.GetString("FAX", dr);

            txtPhone.Text = Helper.GetString("PHONE", dr);

            txtWebsite.Text = Helper.GetString("WEBSITE", dr);

            txtEmail.Text = Helper.GetString("EMAIL_ADDRESS", dr);

            ucAddress.StreetAddress = dr["ADDRESS1"].ToString();
            ucAddress.UnitAddress = dr["ADDRESS2"].ToString();

            ucAddress.City = dr["CITY"].ToString();
            ucAddress.State = dr["STATE"].ToString().Trim();
            if (dr["STATE"].ToString() != "")
            {
                 ucAddress.LoadState();
                string countyName = dr["COUNTY"].ToString();
                ucAddress.LoadCountiesByState(dr["STATE"].ToString().Trim());
                ucAddress.County = countyName;
            }
            ucAddress.Zip5 = dr["ZIP"].ToString();
            ucAddress.Zip4 = dr["EXT_ZIP"].ToString();
            ucAddress.FloorDepartment = dr["Suite_Department_Floor"].ToString();

            RegDMERegAgentID = Helper.GetInt("REG_DME_REGISTERED_AGENT_ID", dr);
        }



    }

    #endregion

    private void SetVisibleFields()
    {

        ucAddress.FloorDepartmetVisible = true;

    }



    public void InitializeFields()
    {
        txtName.Text = string.Empty;

        txtCompanyName.Text = string.Empty;

        txtFaxNumber.Text = string.Empty;

        txtEmail.Text = string.Empty;
        txtWebsite.Text = string.Empty;
          ucAddress.StreetAddress =
          ucAddress.UnitAddress =

          ucAddress.City =
          ucAddress.State =
          ucAddress.Zip5 =
          ucAddress.Zip4 =
          string.Empty;

        RegDMERegAgentID = 0;
        ucAddress.LoadState();

    }

 
    public bool SaveData()
    {
        bool rtn = true;
        Page.Validate("valDMERegisteredAgent");
        this.ValidateData();
        //


        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = this.Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valDMERegisteredAgent") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }
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
        if (RegDMERegAgentID != 0)
            parms.Add("REG_DME_REGISTERED_AGENT_ID", RegDMERegAgentID.ToString());

        if (txtName.Text != "" && txtName.Text != String.Empty && txtName.Text != null)
        {
            parms.Add("AGENT_NAME", txtName.Text);
        }

        parms.Add("COMPANY_NAME", txtCompanyName.Text);

        parms.Add("ADDRESS1", ucAddress.StreetAddress);
        parms.Add("ADDRESS2", ucAddress.UnitAddress);
        parms.Add("ADDRESS3", "");
        parms.Add("CITY", ucAddress.City);
        parms.Add("STATE", ucAddress.State);
        parms.Add("ZIP", ucAddress.Zip5);
        parms.Add("EXT_ZIP", ucAddress.Zip4);
        parms.Add("COUNTY", ucAddress.County);

        parms.Add("PHONE", Helper.StripNonNumerics(txtPhone.Text));
        parms.Add("Suite_Department_Floor", ucAddress.FloorDepartment);

        parms.Add("FAX", Helper.StripNonNumerics(txtFaxNumber.Text));
        parms.Add("EMAIL_ADDRESS", txtEmail.Text);

        parms.Add("WEBSITE", txtWebsite.Text);

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        if (RegDMERegAgentID > 0)
        {
            // Update

            psc.UpdateRegistrationDataTable("DME_REGISTERED_AGENT", parms);
        }
        else
        {
            // Insert
            psc.InsertRegistrationDataTable("DME_REGISTERED_AGENT", parms);
        }
            


        return rtn;
    }


 


    
}