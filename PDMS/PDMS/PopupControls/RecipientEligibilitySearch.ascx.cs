using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

public partial class PopupControls_RecipientEligibilitySearch : BaseSectionControl

{
    string displayMessage = "Service limitations have been calculated from DOS {0}. Some service limitations are not available through this search.Please check service limitations by calling provider assistance at 1-800-686-1516.";
    public bool IsFromhospice;
    #region PDMS Service Initialization
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
    public override void LoadData(DataRow dr)
    {
       // base.LoadData(dr);
    }
   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProviderInformation(this.WorkflowPage.MedicaidID);
        }
        if (IsFromhospice)
        {
            var ri = this.WorkflowPage.RecipientInformation;

            if (ri != null)
            {
                txtMedicaidBillingNumber.Text = this.WorkflowPage.MedicaidBillingNumber;
                txtSSN.Text = ri.SSN;
                txtBirthDate.Text = ri.DateOfBirth != null && ri.DateOfBirth.HasValue ? ri.DateOfBirth.Value.ToString("MM/dd/yyyy") : this.WorkflowPage.RecipientDateOfBirth;
                txtMedicaidBillingNumber.Enabled = false;
                txtBirthDate.Enabled = false;
                divSSN.Visible = false;
                divProcedureCode.Visible = false;
            }
        }
        else
        {
            txtMedicaidBillingNumber.Enabled = true;
            txtBirthDate.Enabled = true;
            divSSN.Visible = true;
            divProcedureCode.Visible = true;
        }      
        if(AppSettings.Get("ShowLevelOfCare") == "false")
        {
            gvLevelofCareDetermination.Visible = false;
        }
    }

    private void LoadProviderInformation(string medicaidNumber)
    {
        DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            this.txthddnNPI.Text = Helper.GetString("NPI", dr);
          //  this.txthddnmedicaidID.Text = Helper.GetString("medicaidID", dr);
        }
    }

    public override string ValidationGroup
    {
        //get { return "valSubmitPriorAuthorization"; }
        get { return "valProviderInfoHeader"; }
    }

    public override string Title
    {
        get { return "Recipient Eligibility Search"; }
    }

    public override string IdText
    {
        get { return "ucRecipientEligibilitySearch_" + this.WorkflowPage.RegistrationId; }
    }

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    public override void LoadControlData()
    {
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))
        {
            sepBenefitsassplan.InnerText = sepBenefitsassplan.InnerText.Replace('+', '-');
            sepEligbilitysearch.InnerText = sepEligbilitysearch.InnerText.Replace('+', '-');
            sepRecipientInfo.InnerText = sepRecipientInfo.InnerText.Replace('+', '-');
            sepManagedCarePlan.InnerText = sepManagedCarePlan.InnerText.Replace('+', '-');
            sepThirdPartyInsurance.InnerText = sepThirdPartyInsurance.InnerText.Replace('+', '-');
            sepAssociatedChildren.InnerText = sepAssociatedChildren.InnerText.Replace('+', '-');
            sepLevelofCareDetermination.InnerText = sepLevelofCareDetermination.InnerText.Replace('+', '-');
            sepLockin.InnerText = sepLockin.InnerText.Replace('+', '-'); ;
            sepLongTermCareFacilityPlacements.InnerText = sepLongTermCareFacilityPlacements.InnerText.Replace('+', '-');
            sepMedicare.InnerText = sepMedicare.InnerText.Replace('+', '-');
            sepPatientLiability.InnerText = sepPatientLiability.InnerText.Replace('+', '-');
            sepRestrictedCoverage.InnerText = sepRestrictedCoverage.InnerText.Replace('+', '-');
            sepServiceLimitation.InnerText = sepServiceLimitation.InnerText.Replace('+', '-');
            sepSpecialProgram.InnerText = sepSpecialProgram.InnerText.Replace('+', '-');
            sepThirdPartyInsurance.InnerText = sepThirdPartyInsurance.InnerText.Replace('+', '-');
        }
    }

    private void AddValidationErrorMessage(string errMsg, ref bool isValid)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        isValid = false;
    }   

    public override bool ValidateData()
    {
        bool isValid = true;
        DateTime value;
        DateTime dateOfBirth;
        DateTime.TryParse(txtBirthDate.Text, out dateOfBirth);

        if (string.IsNullOrEmpty(txtSSN.Text) &&  string.IsNullOrEmpty(txtMedicaidBillingNumber.Text))
        {
            AddValidationErrorMessage("Medicaid Billing Number or SSN is required", ref isValid);
        }
      
        else if(string.IsNullOrEmpty(txtBirthDate.Text))
        {
            AddValidationErrorMessage("*Please Enter Date Of Birth", ref isValid);
        }

        else if(string.IsNullOrEmpty(txtFromDos.Text))
        {
            AddValidationErrorMessage("Please Select From DOS", ref isValid);
        }
        else if(string.IsNullOrEmpty(txtToDos.Text))
        {
            AddValidationErrorMessage("Please Select TO DOS", ref isValid);
        }
        else if(!string.IsNullOrEmpty(txtFromDos.Text) && DateTime.TryParse(txtFromDos.Text, out value) && DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtFromDos.Text)) < 0)
        {
            AddValidationErrorMessage("* From DOS cannot be after todays date.", ref isValid);
        }
        else if ( (dateOfBirth != null) && (dateOfBirth.Year < 1900) )
        {
            AddValidationErrorMessage("* DOB Must be beyond 1900.", ref isValid);
        }
        else if(!string.IsNullOrEmpty(txtFromDos.Text) && !DateTime.TryParse(txtFromDos.Text, out value) )
        {
            AddValidationErrorMessage("* From DOS is not a date format.", ref isValid);
        }

        else if (!string.IsNullOrEmpty(txtBirthDate.Text) && DateTime.TryParse(txtBirthDate.Text, out value) && DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtBirthDate.Text)) < 0)
        {
            AddValidationErrorMessage("* DOB Does not allow future date.", ref isValid);
        }
        else if (!string.IsNullOrEmpty(txtBirthDate.Text) && !DateTime.TryParse(txtBirthDate.Text, out value))
        {
            AddValidationErrorMessage("* DOB is not a date format.", ref isValid);
        }

        else if (!String.IsNullOrEmpty(txtToDos.Text) && DateTime.TryParse(txtToDos.Text, out value) && DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtToDos.Text)) < 0)
        {
            AddValidationErrorMessage("* To DOS cannot be after todays Date", ref isValid);
        }
        else if (!String.IsNullOrEmpty(txtToDos.Text) && !DateTime.TryParse(txtToDos.Text, out value))
        {
            AddValidationErrorMessage("* To DOS is not a date format.", ref isValid);
        }      

        else if(!String.IsNullOrEmpty(txtFromDos.Text) && !String.IsNullOrEmpty(txtToDos.Text))
        {
            DateTime enteredFrom = DateTime.Parse(txtFromDos.Text);
            DateTime enteredTo = DateTime.Parse(txtToDos.Text);
            if (DateTime.Compare(enteredFrom, enteredTo) > 0)
            {
                AddValidationErrorMessage("* Choose DateFrom value prior to DateTo value", ref isValid);
            }
        }

        if (isValid)
        {
            DateTime fromDos = Convert.ToDateTime(txtFromDos.Text.Trim());
            DateTime toDos = Convert.ToDateTime(txtToDos.Text.Trim());
            DateTime date48MonthBack = DateTime.Today.AddMonths(-48);
            if (fromDos.Date < date48MonthBack)
            {
                AddValidationErrorMessage("* System Allows up to 48 months back", ref isValid);

            }
            else if (fromDos.Date > toDos.Date)
            {
                AddValidationErrorMessage("ToDOS must be greater than FromDOS", ref isValid);
            }
        }
        return isValid;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (!IsFromhospice)
        {
            txtBirthDate.Text = string.Empty;
            txtMedicaidBillingNumber.Text = string.Empty;
        }
        //Clear eligibility search section
        //txtBirthDate.Text = string.Empty;
        txtDOB.Text = string.Empty;
        txtDOD.Text = string.Empty;
        //txtMedicaidBillingNumber.Text = string.Empty;
        txtProcedureCode.Text = string.Empty;
        txtSSN.Text = string.Empty;
        txtFromDos.Text = string.Empty;
        txtToDos.Text = string.Empty;
        lblErrorMsg.Text = string.Empty;
        //Clear recipient information
        ClearControls(pnlRecipientInfo);
        //Clear all grid controls
        gvBenefitsassplan.DataSource = null;
        gvBenefitsassplan.DataBind();
        gvManagedCarePlan.DataSource = null;
        gvManagedCarePlan.DataBind();
        gvThirdPartyInsurance.DataSource = null;
        gvThirdPartyInsurance.DataBind();
        gvPatientLiability.DataSource = null;
        gvPatientLiability.DataBind();
        gvLongTermCareFacilityPlacements.DataSource = null;
        gvLongTermCareFacilityPlacements.DataBind();
        gvLockin.DataSource = null;
        gvLockin.DataBind();
        gvMedicare.DataSource = null;
        gvMedicare.DataBind();
        gvLevelofCareDetermination.DataSource = null;
        gvLevelofCareDetermination.DataBind();
        gvServiceLimitation.DataSource = null;
        gvServiceLimitation.DataBind();
        lblErrorDisplay.Text = "";
        gvRestrictedCoverage.DataSource = null;
        gvRestrictedCoverage.DataBind();
        gvAssociatedChildren.DataSource = null;
        gvAssociatedChildren.DataBind();
    }

    private void ClearControls(Control parent)
    {
        foreach (var item in parent.Controls)
        {
            if (item is TextBox)
            {
                ((TextBox)item).Text = "";
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {        
        //Validate data
        ValidateData();
        if (Page.IsValid)
        {          
            //ProvideEligVerificationDataResponse responceObj = new ProvideEligVerificationDataResponse()
            string logHeader = string.Format("Eligibility Get Transaction History for Medicaid ID - ");
            string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

            try
            {
                string procedurecode = string.Empty;
                if (!string.IsNullOrEmpty(txtProcedureCode.Text))
                {
                    procedurecode = txtProcedureCode.Text;
                }

                string medicaidID = this.WorkflowPage.MedicaidID; 
                Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());

                RecipientEligibilitySearchResponse resr = new RecipientEligibilitySearchReqRes().SearchRequest(txtSSN.Text, medicaidID, userId, txtBirthDate.Text, txtFromDos.Text, txtToDos.Text, procedurecode, txtMedicaidBillingNumber.Text, "SearchEligibility");

                if (resr != null)
                {
                    //Check if the service returned any errors
                    if (resr.ErrorDetails != null && resr.ErrorDetails.Count > 0)
                    {
                        StringBuilder errorMsg = new StringBuilder();
                        foreach (ErrorDetail ed in resr.ErrorDetails)
                        {
                            if (errorMsg.Length > 0)
                            {
                                errorMsg.Append("; ");
                                errorMsg.Append(ed.Code + " : " + ed.Description);
                            }
                            else
                                errorMsg.Append(ed.Code + " : " + ed.Description);
                        }
                        lblErrorMsg.Text = errorMsg.ToString();
                    }
                    else
                        lblErrorMsg.Text = "";
                    if (resr.RecipientInfo != null)
                    {
                        txtRecinfoMedicaidbillNumber.Text = resr.RecipientInfo.MedicaidId.ToString();
                        txtDOB.Text = resr.RecipientInfo.DateOfBirth != null ? resr.RecipientInfo.DateOfBirth.Value.ToString("MM/dd/yyyy") : null;
                        txtLast.Text = resr.RecipientInfo.LastName;
                        txtDOD.Text = resr.RecipientInfo.DateOfDeath != null ? resr.RecipientInfo.DateOfDeath.Value.ToString("MM/dd/yyyy") : null;
                        string mi = resr.RecipientInfo.MiddleName != null ? resr.RecipientInfo.MiddleName : string.Empty;
                        mi = mi.Length > 1 ? ", " + mi.Substring(0, 1) : string.Empty;
                        txtFirstName.Text = resr.RecipientInfo.FirstName + mi;
                        //OHPNM-11642 fix
                        if (!string.IsNullOrEmpty(txtSSN.Text) && txtSSN.Text == resr.RecipientInfo.SSN)
                            txtSSN2.Text = resr.RecipientInfo.SSN;
                        else
                            txtSSN2.Text = "";
                    }
                    else 
                    {
                        txtRecinfoMedicaidbillNumber.Text = "";
                        txtDOB.Text = "";
                        txtLast.Text = "";
                        txtDOD.Text = "";
                        txtFirstName.Text = "";
                        txtSSN2.Text = "";
                    }
                    if (resr.BenifitAssignmentPlans != null && resr.BenifitAssignmentPlans.Count > 0)
                    {
                        gvBenefitsassplan.DataSource = resr.BenifitAssignmentPlans;
                        gvBenefitsassplan.DataBind();
                    }
                    else
                    {
                        gvBenefitsassplan.DataSource = null;
                        gvBenefitsassplan.DataBind();
                    }
                    if (resr.ManagedCarePlans != null && resr.ManagedCarePlans.Count > 0)
                    {
                        gvManagedCarePlan.DataSource = resr.ManagedCarePlans;
                        gvManagedCarePlan.DataBind();
                    }
                    else
                    {
                        gvManagedCarePlan.DataSource = null;
                        gvManagedCarePlan.DataBind();
                    }
                    if (resr.ThirdPartyLiabilities != null && resr.ThirdPartyLiabilities.Count > 0)
                    {
                        gvThirdPartyInsurance.DataSource = resr.ThirdPartyLiabilities;
                        gvThirdPartyInsurance.DataBind();
                    }
                    else
                    {
                        gvThirdPartyInsurance.DataSource = null;
                        gvThirdPartyInsurance.DataBind();
                    }
                    if (resr.PatientLiabilities != null && resr.PatientLiabilities.Count > 0)
                    {
                        gvPatientLiability.DataSource = resr.PatientLiabilities;
                        gvPatientLiability.DataBind();
                    }
                    else
                    {
                        gvPatientLiability.DataSource = null;
                        gvPatientLiability.DataBind();
                    }
                    if (resr.LTCFPlacements != null && resr.LTCFPlacements.Count > 0)
                    {
                        gvLongTermCareFacilityPlacements.DataSource = resr.LTCFPlacements;
                        gvLongTermCareFacilityPlacements.DataBind();
                    }
                    else
                    {
                        gvLongTermCareFacilityPlacements.DataSource = null;
                        gvLongTermCareFacilityPlacements.DataBind();
                    }
                    if (resr.Lockins != null && resr.Lockins.Count > 0)
                    {
                        gvLockin.DataSource = resr.Lockins;
                        gvLockin.DataBind();
                    }
                    else
                    {
                        gvLockin.DataSource = null;
                        gvLockin.DataBind();
                    }
                    if (resr.MedicareCoverageDetails != null && resr.MedicareCoverageDetails.Count > 0)
                    {
                        gvMedicare.DataSource = resr.MedicareCoverageDetails;
                        gvMedicare.DataBind();
                    }
                    else
                    {
                        gvMedicare.DataSource = null;
                        gvMedicare.DataBind();
                    }
                    if (resr.SNIFlOCDetails != null && resr.SNIFlOCDetails.Count > 0)
                    {
                        gvLevelofCareDetermination.DataSource = resr.SNIFlOCDetails;
                        gvLevelofCareDetermination.DataBind();
                    }
                    else
                    {
                        gvLevelofCareDetermination.DataSource = null;
                        gvLevelofCareDetermination.DataBind();
                    }
                    if (resr.ServiceLimitations != null && resr.ServiceLimitations.Count > 0)
                    {
                        gvServiceLimitation.DataSource = resr.ServiceLimitations;
                        gvServiceLimitation.DataBind();
                    }
                    else
                    {
                        gvServiceLimitation.DataSource = null;
                        gvServiceLimitation.DataBind();
                    }
                    if (resr.RestrictedCoverages != null && resr.RestrictedCoverages.Count > 0)
                    {
                        gvRestrictedCoverage.DataSource = resr.RestrictedCoverages;
                        gvRestrictedCoverage.DataBind();
                    }
                    else
                    {
                        gvRestrictedCoverage.DataSource = null;
                        gvRestrictedCoverage.DataBind();
                    }
                    if (resr.Under19FamilyMembers != null && resr.Under19FamilyMembers.Count > 0)
                    {
                        gvAssociatedChildren.DataSource = resr.Under19FamilyMembers;
                        gvAssociatedChildren.DataBind();
                    }
                    else
                    {
                        gvAssociatedChildren.DataSource = null;
                        gvAssociatedChildren.DataBind();
                    }
                }
            }
            catch (FaultException ex)
            {
                lblErrorMsg.Text = "Error: An error occurred while processing the request";
                ClearData();
                Logging log = new Logging(Guid.NewGuid(), logMsg);
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
            }
            catch (Exception exx)
            {
                lblErrorMsg.Text = "Error: An error occurred while processing the request";
                ClearData();
                Logging log = new Logging(Guid.NewGuid(), logMsg);

                log.CreateLogEntry(string.Format("{0} {1}", logHeader, exx.ToString()), Logging.LogPriority.Error);
                //throw exx;
            }
        }
    }

    private void ClearData()
    {
        txtRecinfoMedicaidbillNumber.Text = "";
        txtDOB.Text = "";
        txtLast.Text = "";
        txtDOD.Text = "";
        txtFirstName.Text = "";
        txtSSN2.Text = "";

        gvBenefitsassplan.DataSource = null;
        gvBenefitsassplan.DataBind();
        
        gvManagedCarePlan.DataSource = null;
        gvManagedCarePlan.DataBind();
        
        gvThirdPartyInsurance.DataSource = null;
        gvThirdPartyInsurance.DataBind();
        
        gvPatientLiability.DataSource = null;
        gvPatientLiability.DataBind();
        
        gvLongTermCareFacilityPlacements.DataSource = null;
        gvLongTermCareFacilityPlacements.DataBind();
        
        gvLockin.DataSource = null;
        gvLockin.DataBind();
        
        gvMedicare.DataSource = null;
        gvMedicare.DataBind();
        
        gvLevelofCareDetermination.DataSource = null;
        gvLevelofCareDetermination.DataBind();
        
        gvServiceLimitation.DataSource = null;
        gvServiceLimitation.DataBind();
        
        gvRestrictedCoverage.DataSource = null;
        gvRestrictedCoverage.DataBind();
        
        gvAssociatedChildren.DataSource = null;
        gvAssociatedChildren.DataBind();
    }
   
    protected void ReportBirthDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtBirthDate.Text, true) && Helper.IsValidDate(txtBirthDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtBirthDate.Text).Subtract(Convert.ToDateTime(txtBirthDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;
    }

    protected void ReportToDos_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtToDos.Text, true) && Helper.IsValidDate(txtToDos.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtToDos.Text).Subtract(Convert.ToDateTime(txtToDos.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void ReportFromDos_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtFromDos.Text, true) && Helper.IsValidDate(txtFromDos.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtFromDos.Text).Subtract(Convert.ToDateTime(txtFromDos.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }

    public override bool SaveData()
    {
        return true;
        //throw new NotImplementedException();
    }

    protected void gvServiceLimitation_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[6].Text == "D")
            {
                e.Row.Cells[6].Text = "Day";
            }
            else if (e.Row.Cells[6].Text == "W")
            {
                e.Row.Cells[6].Text = "Calendar Week";
            }
            else if (e.Row.Cells[6].Text == "C")
            {
                e.Row.Cells[6].Text = "Calendar Month";
            }
            else if (e.Row.Cells[6].Text == "M")
            {
                e.Row.Cells[6].Text = "Month";
            }
            else if (e.Row.Cells[6].Text == "Y")
            {
                e.Row.Cells[6].Text = "Calendar Year";
            }
            else if (e.Row.Cells[6].Text == "F")
            {
                e.Row.Cells[6].Text = "Fiscal Year";
            }
        }
    }   
}