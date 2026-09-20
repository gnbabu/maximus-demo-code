using Corp.Core.Libraries;
using Corp.Core.Libraries.PriorAuthServiceReference;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claims = Models.Data.Claims;
using CON = MAXIMUS.Core.Libraries.Constants;
using Dental = Corp.Core.Libraries.FI.ClaimsDentalService;
using Inst = Corp.Core.Libraries.FI.ClaimsInstitutionalService;
using Pro = Corp.Core.Libraries.FI.ClaimsProfessionalService;





//using Corp.Core.Libraries;

public partial class Process_StandaloneUploadAttachments : WorkflowPage
{
    public static string ClaimID = string.Empty;
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
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public bool OwnershipChangedFlag { get; set; }
    private DataTable DataList;
    private int Step;

    public int EntityTypeId
    {
        get
        {
            return this.EntityTypeID;
        }
        set { this.EntityTypeID = value; }
    }

    public string _icn;
    public string Icn
    {
        get
        {
            return this._icn;
        }
        set { this._icn = value; }
    }
    public string _panumber;
    public string PANumber
    {
        get
        {
            return this._panumber;
        }
        set { this._panumber = value; }
    }
    public int ProviderTypeId
    {
        get
        {
            return this.ProviderTypeID;
        }
        set { this.ProviderTypeID = value; }
    }
    public int _transactionTypeID;
    public int TransactionTypeID
    {
        get
        {
            return this._transactionTypeID;
        }
        set { this._transactionTypeID = value; }
    }
    public string _memberid;
    public string MemberID
    {
        get
        {
            return this._memberid;
        }
        set { this._memberid = value; }
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }
    protected override void Page_PreRender(object sender, EventArgs e)
    {
        //TODO: Is this popup functionality for any specific role?
        if (HasUnsavedDataPA.Value == "True")
        {
            HasUnsavedDataPA.Value = "";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        CredentialHelper.APIToken APIToken = ApplicationCache.RestAPIAccessToken();
        HiddenField hdnAPIToken = (HiddenField)Page.Master.FindControl("hdnAccessToken");
        HiddenField hdnAPIRefreshToken = (HiddenField)Page.Master.FindControl("hdnRefreshToken");
        hdnAPIToken.Value = APIToken.AccessToken;
        hdnAPIRefreshToken.Value = APIToken.RefreshToken;


        MaintainScrollPositionOnPostBack = true;
        string MedicaidNumber = string.Empty;
        string regID = string.Empty;

        /*if (Request.QueryString.AllKeys.Contains("MedicaidNumber"))
            MedicaidNumber = Request.QueryString["MedicaidNumber"];

        if (Request.QueryString.AllKeys.Contains("RegID"))
            regID = Request.QueryString["RegID"];

        if (Request.QueryString.AllKeys.Contains("StepID"))
            Step = Convert.ToInt32(Request.QueryString["StepID"]);*/

        if (Session["RegId"] != null)
        {
            this.RegistrationId = Convert.ToInt32(Session["RegId"]);
        }
        if (Session["MedID"] != null)
        {
            if (string.IsNullOrEmpty(this.WorkflowPage.MedicaidID) && !string.IsNullOrEmpty(Convert.ToString(Session["MedID"])))
            {
                this.WorkflowPage.MedicaidID = Convert.ToString(Session["MedID"]);
            }
        }


        //if (!IsPostBack)
        //{
            if (PreviousPage != null)
            {
                this.RegistrationId = PreviousPage.RegistrationId;
                this.RegistrationStep = PreviousPage.RegistrationStep;
                this.CommandName = PreviousPage.CommandName;
                this.RegistrationIdSelected = PreviousPage.IsReadOnly ? PreviousPage.RegistrationId : 0;
                this.MedicaidID = PreviousPage.MedicaidID;
            }
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString.AllKeys.Contains("PANumber"))
                {
                    //PA = Convert.ToString(Request["PA"].ToString());
                    this.PANumber = Helper.Decrypt(HttpUtility.UrlDecode(Convert.ToString(Request.QueryString["PANumber"])).TrimStart());
                }
                if (Request.QueryString.AllKeys.Contains("icn"))
                {
                    //PA = Convert.ToString(Request["PA"].ToString());
                    this.Icn = Helper.Decrypt(HttpUtility.UrlDecode(Convert.ToString(Request.QueryString["icn"])).TrimStart());
                }
                if (Request.QueryString.AllKeys.Contains("MemberID"))
                {
                    this.MemberID = Helper.Decrypt(HttpUtility.UrlDecode(Convert.ToString(Request.QueryString["MemberID"]).TrimStart()));
                    //PayorId = Convert.ToString(Request["PayorId"]);
                }
                if (Request.QueryString.AllKeys.Contains("TransactionTypeID"))
                {
                    //PA = Convert.ToString(Request["PA"].ToString());
                    if (Request.QueryString["TransactionTypeID"].ToString() == "DENTAL")
                    {
                        TransactionTypeID = 1;
                    }
                    else if (Request.QueryString["TransactionTypeID"].ToString() == "INSTITUTIONAL")
                    {
                        TransactionTypeID = 3;
                    }
                    else if (Request.QueryString["TransactionTypeID"].ToString() == "PROFESSIONAL")
                    {
                        TransactionTypeID = 2;
                    }
                    else
                        TransactionTypeID = Convert.ToInt32((Request.QueryString["TransactionTypeID"]));
                }
                //this.TransactionTypeID = TransactionTypeID;
                if (Request.QueryString.AllKeys.Contains("RegId"))
                    this.RegistrationId = int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["RegId"].ToString(), true));
                if (Request.QueryString.AllKeys.Contains("Step"))
                    this.RegistrationStep = int.Parse(Request["Step"]);
                //UploadAttachments.LoadControlData(this.Icn, this.PANumber, this.MemberID, this.TransactionTypeID);

            }
            this.FillRegistrationData();
        //}
        LoadProviderInformation(MedicaidNumber, regID);

        if (!string.IsNullOrEmpty(this.Icn) || !string.IsNullOrEmpty(this.PANumber))
        {
            UploadAttachments.LoadControlData(this.Icn, this.PANumber, this.MemberID, this.TransactionTypeID);
        }

        if (!IsPostBack && Request.QueryString["payloadData"] == null)
        {
            DeleteAttachmentsForProvider(MedicaidNumber);

            if (Request.Cookies["selectedOption"] != null)
            {
                Request.Cookies["selectedOption"].Expires = DateTime.Now.AddDays(-30);
                Request.Cookies["selectedOption"].Value = null;
            }
        }
        this.ucRegProgressBar.RefreshEvent += UcRegProgressBar_RefreshEvent;
        this.MedicaidID = MedicaidNumber;
        ucRegProgressBar.SelectStep();
    }



    private void LoadProviderInformation(string medicaidNumber, string regID = "")
    {
        if (!string.IsNullOrEmpty(medicaidNumber))
        {
            DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                /*this.lblProMedicaidID2.Text = Helper.GetString("MEDICAID_ID", dr);
                this.lblPRONPI2.Text = Helper.GetString("NPI", dr);
                this.lblProviderName2.Text = Helper.GetString("NAME", dr);*/
                this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
                this.WorkflowPage.MedicaidID = Helper.GetString("MEDICAID_ID", dr);
                this.WorkflowPage.NPI = Helper.GetString("NPI", dr);
            }
        }
        else if (!string.IsNullOrWhiteSpace(regID))
        {
            DataSet ds = svc.SelectProviderByRegID(regID);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                /*this.lblProMedicaidID2.Text = Helper.GetString("MEDICAID_ID", dr);
                this.lblPRONPI2.Text = Helper.GetString("NPI", dr);
                this.lblProviderName2.Text = Helper.GetString("NAME", dr);*/
                this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
                this.WorkflowPage.MedicaidID = Helper.GetString("MEDICAID_ID", dr);
                this.WorkflowPage.NPI = Helper.GetString("NPI", dr);
            }
        }
        else
        {
            string regId = "";
            if (Session["RegId"] != null)
                regId = Session["RegId"].ToString();

            DataSet ds = svc.SelectProviderByRegID(regId);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                /*this.lblProMedicaidID2.Text = Helper.GetString("MEDICAID_ID", dr);
                this.lblPRONPI2.Text = Helper.GetString("NPI", dr);
                this.lblProviderName2.Text = Helper.GetString("NAME", dr);*/
                this.RegIdTxt.Value = Helper.GetString("REG_ID", dr);
                this.WorkflowPage.MedicaidID = Helper.GetString("MEDICAID_ID", dr);
                this.WorkflowPage.NPI = Helper.GetString("NPI", dr);
            }
        }
        //this.WorkflowPage.ProviderName = this.lblProviderName2.Text;

        if (!string.IsNullOrEmpty(this.RegIdTxt.Value))
            this.RegistrationId = Convert.ToInt32(this.RegIdTxt.Value);
    }

    private void DeleteAttachmentsForProvider(string medicaidNumber)
    {
        svc.DeleteAttachmentsForMedicaidID(medicaidNumber);

    }
    private string GetMMISProviderTypeID(int providerTypeId)
    {
        string rtn = "";
        DataSet ds = svc.GetProviderTypeById(providerTypeId);
        if (Methods.HasRows(ds))
        {
            rtn = Methods.GetStringValue(ds.Tables[0].Rows[0], "MMIS_PROVIDER_TYPE_ID");
        }
        return rtn;
    }
    private void UcRegProgressBar_RefreshEvent(int step)
    {
        RefreshPage(step);
    }

    private void RefreshPage(int step)
    {
        Step = step;
        string controlPath = string.Empty;
        var mmisProviderTypeId = "";
        DataSet ds = svc.SelectProviderByGRPMedicaidID(MedicaidID);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            var provTypeId = Helper.GetString("PROVIDER_TYPE_ID", dr);
            mmisProviderTypeId = GetMMISProviderTypeID(Convert.ToInt32(provTypeId));
        }
        this.WorkflowPage.IsHospiceEnrollmentClick = false;
        //OHPNM-6291
        this.WorkflowPage.RegistrationStep = step;
        ucRegProgressBar.SelectStep();


        switch (Step)
        {

            case CON.SectionTypeID.UploadAttachments:


                divUploadAttachments.Visible = true;
                break;
            default:
                break;
        }
    }



    private void CreatePlaceholderControlPath(string controlPath)
    {
        BaseSectionControl ctrl = LoadControl(controlPath) as BaseSectionControl;
        AddSectionToPlaceHolder(ctrl);
    }

    private void AddSectionToPlaceHolder(Control controlTOAdd)
    {
        // sectionPH.Controls.Clear();
        //  sectionPH.Controls.Add(controlTOAdd);
    }

    private void DisableControls()
    {
        // pnlRecipientEligibility.Enabled = false;
        pnlBillingAndotherservice.Enabled = false;
    }

    private void EnableControls()
    {
        //  pnlRecipientEligibility.Enabled = true;
        pnlBillingAndotherservice.Enabled = true;
    }
    private void LoadData(int step)
    {

    }


    public class destinationPayerID
    {
        public destinationPayerID(string payer_code, string payer_desc)
        {
            this.Payer_code = payer_code;
            this.Payer_desc = payer_desc;
        }
        private string _payercode;
        private string _payerdesc;
        public string Payer_code
        {
            get { return _payercode; }
            set { _payercode = value; }
        }
        public string Payer_desc
        {
            get { return _payerdesc; }
            set { _payerdesc = value; }
        }
    }

    public string GetPreSignedUrl(string fileName, string contentType)
    {
        DateTime expiryTime = DateTime.Now.AddMinutes(20);
        ProcessAttachments processAttachments = new ProcessAttachments();
        return processAttachments.generatePreSignedUrl(fileName, contentType, expiryTime);
    }

    private static DataTable GetData(string npi, string medicaidid, string lastName, string firstName)
    {
        DataTable dtNPI = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                var dsNPI = psc.SearchProviderNPI(npi, medicaidid, lastName, firstName);
                if (dsNPI != null)
                {
                    dtNPI = dsNPI.Tables[0];
                }
            }
        }
        catch
        {
            return dtNPI;
        }
        return dtNPI;
    }


    public static DataTable GetNPIForAdditional(string ProviderNPI)
    {
        DataTable dt = null;
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, ProviderNPI, true));

            var ds = DataAccess.ExecuteStoredProcedure("usp_Search_NPI", parameters, "Search_NPI");

            if (ds != null)
            {
                dt = ds.Tables[0];
            }
        }
        return dt;
    }


    public class NPIData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string MedicaidId { get; set; }
        public string Errormessage { get; set; }
    }
    public static List<NPIData> GetDataforNPI(string ProviderNPI)
    {
        string errormsg = string.Empty;
        string ProviderName = string.Empty;
        List<NPIData> lstdata = new List<NPIData>();
        NPIData NPIdata = new NPIData();
        if (!string.IsNullOrEmpty(ProviderNPI))
        {
            if (ProviderNPI.Length == 10)
            {
                var isValid = Claims.ValidProviderNPI(ProviderNPI.Trim());
                if (!isValid)
                {
                    errormsg = string.Concat(ProviderNPI, "NPI is not found in the system");
                    //if (_lblMedicaidId != null)
                    //{
                    //    _lblMedicaidId = "";
                    //}                  
                    NPIdata.Errormessage = errormsg;
                    lstdata.Add(NPIdata);
                    //return;
                }
                else
                {
                    DataTable dt = GetData(ProviderNPI, "", "", "");
                    if (dt.Rows.Count > 0)
                    {

                        dt = dt.Select("NPI <> ''").CopyToDataTable();
                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            //if (_lblMedicaidId != null)
                            //{
                            //    _lblMedicaidId = row["MEDICAID_ID"].ToString();
                            //}                           
                            ProviderName = row["FIRST_NAME"].ToString() + " " + row["LAST_OR_BUSINESS_NAME"].ToString();
                            NPIdata.FirstName = row["FIRST_NAME"].ToString();
                            NPIdata.LastName = row["LAST_OR_BUSINESS_NAME"].ToString();
                            lstdata.Add(NPIdata);
                        }

                    }
                }
            }
            else
            {
                errormsg = "10-digit number is required";
                NPIdata.Errormessage = errormsg;
                lstdata.Add(NPIdata);
            }
        }

        return lstdata;
    }



}


