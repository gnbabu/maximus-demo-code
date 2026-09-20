//using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Specialties : BaseSectionControl
{
    private PDMSService.PDMSServiceClient _svc;
    private const string _SectionName = "Specialties";
    private const string defaultEndDate = "12/31/2299";
    private bool _DisplayConfirmAdd = false;
    private bool isPrimaryPastEndDate = false;
    public DataSet dsSpecialties = new DataSet();
    public DataSet speclist = new DataSet();
    private Logging log = null;
    public Guid ThreadId { get; set; }
    private bool isConvertFrmORPWF = false;
    private bool isStreamlinedApp = false;
    public PopupControls_Specialties()
    {
        ThreadId = Guid.NewGuid();
    }

    public DataSet SpecialitiesInfo
    {
        get
        {
            if (!Helper.HasRows(speclist))
            {
                if (Helper.HasRows(Loadspeclist()))
                {
                    return speclist;
                }
            }
            return speclist;
        }
    }

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
            if (ViewState["IsPrimary"] == null) ViewState["IsPrimary"] = false;
            return Convert.ToBoolean(ViewState["IsPrimary"]);
        }
        set { ViewState["IsPrimary"] = value; }
    }

    public bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
    }

    public DataTable DataList1
    {
        get
        {
            string id = this.IdText + "_DataList1";
            if (ViewState[id] == null) return null;
            return (DataTable)ViewState[id];
        }
        set
        {
            string id = this.IdText + "_DataList1";
            ViewState[id] = value;
        }
    }

    //Datatable that stores all the specialities for a given provider type
    public DataTable DataListProviderTypeSpecialties
    {
        get
        {
            string id = this.IdText + "_DataListProviderTypeSpecialties";
            if (ViewState[id] == null) return null;
            return (DataTable)ViewState[id];
        }
        set
        {
            string id = this.IdText + "_DataListProviderTypeSpecialties";
            ViewState[id] = value;
        }
    }

    private DataSet enrollStatusTypes = null;
    private DataSet EnrollStatusTypes
    {
        get
        {
            if (enrollStatusTypes == null)
                enrollStatusTypes = GetEnrollmentStatusType();
            return enrollStatusTypes;
        }
    }

    public int ProviderTypeId
    {
        get
        {
            return this.WorkflowPage.ProviderTypeID;
        }
    }

    public bool HasPrimary
    {
        get
        {
            return HasPrimarySpecialty();
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //if (ViewState["SPTyp"] != null)
        //{
        //dsSPTyp = (DataSet)ViewState["SPTyp"];
        BuildDynamicControls();
        //}
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Specialties";
    }

    public override void LoadControlData()
    {
        txtSpecStart.Text = DateTime.Today.ToShortDateString();

        // OHPNM-18353 - Set Specialties page read only on Convert to ORP WF, Specialty dates will be applied in PromoteToActive step
        PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
        DataSet ds1 = psc1.WF_SelectProcessParameters(this.WorkflowPage.WF_ProcessID);

        if (Helper.HasRows(ds1))
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            isConvertFrmORPWF = string.IsNullOrEmpty(dr[CON.ProcessParameter.IsConvertFrmORPWF].ToString()) ? false : Convert.ToBoolean(dr[CON.ProcessParameter.IsConvertFrmORPWF]);
            isStreamlinedApp = string.IsNullOrEmpty(dr[CON.ProcessParameter.IsStreamlinedApp].ToString()) ? false : Convert.ToBoolean(dr[CON.ProcessParameter.IsStreamlinedApp]);
            isStreamlinedApp = (isStreamlinedApp && !IsInternalUser() && this.WorkflowPage.WF_StepID > 0 && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.NewReg);
        }
        if ((isConvertFrmORPWF && this.WorkflowPage.WF_StepID > 0)
             || (isStreamlinedApp))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
            btnAddSpecialties.Visible = false;
            lblORPHelptext.Visible = isConvertFrmORPWF;
        }

        LoadSpecialties();
        CanUserViewHRSA();

        if (Helper.HasRows(dsSpecialties))
        {
            isPrimaryPastEndDate = dsSpecialties.Tables[0].AsEnumerable()
                                     .Where(r => r.Field<DateTime?>("END_DATE") <= DateTime.Now &&
                                     r.Field<bool>("PRIMARY_FLAG") == true).Any();
            bool adding386Primary = dsSpecialties.Tables[0].AsEnumerable()
                                     .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == "386" &&
                                     r.Field<bool>("PRIMARY_FLAG") == true).Any();
            bool adding190Primary = dsSpecialties.Tables[0].AsEnumerable()
                                    .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == CON.MMISSpecialtyType.OHRISE_MCOProviderOnly &&
                                    r.Field<bool>("PRIMARY_FLAG") == true).Any();

            if (adding386Primary)
            {
                isPrimaryPastEndDate = false;
            }
            if (adding190Primary)
            {
                isPrimaryPastEndDate = false;
            }
            if (isPrimaryPastEndDate)
            {
                lblPrimaryValidation.Visible = true;
                lblPrimaryValidation.Text = GetGlobalResourceObject("BrandingResource", "IsPrimaryPastEndDate").ToString();
            }
        }
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTYHistory", parms);
            if (Helper.HasRows(ds))
            {
                grdSpecialtiesHistory.DataSource = ds.Tables[0];
                grdSpecialtiesHistory.DataBind();
                grdSpecialtiesHistory.MasterTableView.ExportToExcel();
            }
        }

    }

    public bool CanUserViewDelete(int regSpecialtyId)
    {
        DataRow dr = findDataRowByRegSpecialtyId(regSpecialtyId);

        int modifiedStatusTypeId = dr["MODIFIED_STATUS_TYPE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MODIFIED_STATUS_TYPE_ID"]);

        if (isConvertFrmORPWF || isStreamlinedApp)
            return false;

        bool sentToSIflag = dr["SENT_TO_SI"] == DBNull.Value ? false : Convert.ToBoolean(dr["SENT_TO_SI"]);

        return !sentToSIflag;
    }
    public bool CanUserViewEdit(int regSpecialtyId)
    {
        DataRow dr = findDataRowByRegSpecialtyId(regSpecialtyId);

        string primaryFlag = dr["PRIMARY_FLAG"].ToString();
        bool sentToSIflag = dr["SENT_TO_SI"] == DBNull.Value ? false : Convert.ToBoolean(dr["SENT_TO_SI"]);
        string enrollStatus = dr["ENROLL_STATUS_DESC"].ToString();
        //ohpnm-16598
        string mmispecidnotshow = Helper.GetAppSettingFromDB("SpecialtyToNotMakeActive");
        string mmispecid = dr["MMIS_SPECIALTY_TYPE_ID"].ToString();
        bool isSpecid = mmispecidnotshow.Contains(mmispecid);

        if (isConvertFrmORPWF || isStreamlinedApp)
        {
            return false;
        }
        else
        {
            if (!IsInternalUser() && (enrollStatus.Trim() == "INACTIVE") && sentToSIflag) //OHPNM-4561 OHPNM-5035
                return false;
            else if (Convert.ToBoolean(primaryFlag) && sentToSIflag)
                return false;
            if (!IsInternalUser() && isSpecid)
                return false;
        }
        return true;
    }

    public bool IsInternalUser()
    {
        return Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
    }

    public void CanUserViewHRSA()
    {
        if (((Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name)) && (
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.Hospital) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.Psychiatric_Hospital) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.Rural_Health_Clinic) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.FEDERALLY_QUALIFIED_HEALTH_CENTER) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.Professional_Medical_Group) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.CLINIC) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.PHARMACY) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.OHIO_DEPARTMENT_OF_MENTAL_HEALTH_PROVIDER) ||
            (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.OMHAS_CERTIFIED_LICENSED_TREATMENT_PROGRAM))))
        {
            HRSA340Bdiv.Visible = true;
            LoadHRSA340B();
        }

    }
    private void LoadSpecialties()
    {
        if (!pnlSpecialties.Visible) return;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", "0");
        DataSet ds = speclist = dsSpecialties = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTY", parms);
        if (Helper.HasRows(ds))
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.MATERNALANDINFANTSUPPORT)
                {
                    var start_date_existing = Convert.ToDateTime(ds.Tables[0].Rows[i]["START_DATE"]);
                    if (start_date_existing.Day != 1 && start_date_existing.Month != 1)
                    {
                        DateTime start_date = new DateTime(start_date_existing.Year, 01, 01);
                        ds.Tables[0].Rows[i]["START_DATE"] = Convert.ToDateTime(start_date).ToString("MM/dd/yyyy");
                    }
                    break;
                }
            }
            grdSpecialties.DataSource = this.DataList = ds.Tables[0];
        }
        else grdSpecialties.DataSource = this.DataList = null;
        grdSpecialties.DataBind();
        //btnAddSpecialties.Visible = (grdSpecialties.Rows.Count == 0);

        btnSpecialtiesHistory.Visible = (this.DataList != null && this.DataList.Rows != null && this.DataList.Rows.Count > 0);

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
            lblPrimaryValidation.Visible = true;
            lblPrimaryValidation.Text = GetGlobalResourceObject("BrandingResource", "InMaintainance").ToString();

            //Enable History popup OK button
            Helper.SetReadOnly(pnlMain, false);
        }

        DisplayODHUpload();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }
    private void LoadHRSA340B()
    {
        if (!pnlHRSA340B.Visible) return;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_HRSA340B", parms);
        if (Helper.HasRows(ds)) grdHRSA340B.DataSource = this.DataList1 = ds.Tables[0];
        else grdHRSA340B.DataSource = this.DataList1 = null;
        grdHRSA340B.DataBind();
    }
    protected bool HasPrimarySpecialty()
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

    public void BindSpeciality(bool isPrimary = false, bool isChkPrimaryChanged = false)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        bool isInternalUser = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
        if (rcbSpecialty.Items.Count == 0 || isChkPrimaryChanged)

        {
            ds = psc.SelectGroupSpecialtiesByProviderTypeRole(ProviderTypeId, isInternalUser, this.WorkflowPage.RegistrationId);

            if (ds == null || ds.Tables == null)
            {
                return;
            }

            ds.Tables[0].Columns.Add("IdWithName", typeof(string), "MMIS_SPECIALTY_TYPE_ID + ' - ' + SPECIALTY_TYPE_NAME");
            DataTable specialties = ds.Tables[0];

            EnumerableRowCollection<DataRow> query = null;

            if (isPrimary)
            {
                //filter to include only rows for those with "PT_PS_ALLOWED_PORTAL" of P and B
                if (this.DataList != null && this.DataList.Rows != null && this.DataList.Rows.Count == 0)
                {
                    if (isStreamlinedApp)
                    {
                        query = from specialty in specialties.AsEnumerable()
                                where (specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "P" ||
                                       specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "B")
                                      && specialty.Field<string>("PT_PS_ALLOWED_STREAMLINE_APP") == "Y"
                                select specialty;
                    }
                    else
                    {
                        query = from specialty in specialties.AsEnumerable()
                                where specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "P" |
                                      specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "B"
                                select specialty;
                    }
                }
                else
                {
                    if (isStreamlinedApp)
                    {
                        query = from specialty in specialties.AsEnumerable()
                                where (specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "P" ||
                                       specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "B")
                                      && specialty.Field<string>("PT_PS_ALLOWED_STREAMLINE_APP") == "Y"
                                select specialty;
                    }
                    else
                    {
                        query = from specialty in specialties.AsEnumerable()
                                where specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "P" |
                                      specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "B"
                                select specialty;
                    }
                }
            }
            else
            {
                if (isStreamlinedApp && !isInternalUser)
                {
                    query = from specialty in specialties.AsEnumerable()
                            where (specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "S" ||
                                   specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "B")
                                  && specialty.Field<string>("PT_PS_ALLOWED_STREAMLINE_APP") == "Y"
                            select specialty;
                }
                else
                {

                    //filter to include only rows for those with "PT_PS_ALLOWED_PORTAL" of S and B
                    query = from specialty in specialties.AsEnumerable()
                            where specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "S" |
                                  specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "B"
                            select specialty;
                }
            }

            DataTable validSpecialites = new DataTable();

            if (query.Any())
            {
                validSpecialites = query.CopyToDataTable<DataRow>();
            }

            string providerTypeID = svc.GetProviderTypeIdByRegId(this.WorkflowPage.RegistrationId);

            if (providerTypeID == CON.MMISProviderType.CLINICAL_COUNSELING)
            {
                string pSpecialtyTypeID = string.Empty;
                DataTable dt = (DataTable)grdSpecialties.DataSource;
                if (Helper.HasRows(dt))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string isPriumary = Helper.GetData("PRIMARY_FLAG", row);
                        if (isPriumary == "True")
                        {
                            //pSpecialtyTypeID = Helper.GetData("SPECIALTY_TYPE_ID", row);
                            pSpecialtyTypeID = Helper.GetData("MMIS_SPECIALTY_TYPE_ID", row);

                            break;
                        }
                    }

                }
                if (pSpecialtyTypeID != "474")
                {
                    query = from specialty in specialties.AsEnumerable()
                            where (specialty.Field<string>("MMIS_SPECIALTY_TYPE_ID") != "BHR" && specialty.Field<string>("MMIS_SPECIALTY_TYPE_ID") != "TSS")
                            select specialty;

                    if (query.Any())
                    {
                        validSpecialites = query.CopyToDataTable<DataRow>();
                    }
                }
            }

            if (Helper.HasRows(validSpecialites))
            {
                rcbSpecialty.Items.Clear();
                foreach (DataRow dr in validSpecialites.Rows)
                {
                    rcbSpecialty.Items.Add(new RadComboBoxItem(dr["IdWithName"].ToString(), dr["SPECIALTY_TYPE_ID"].ToString()));
                }
            }
            this.DataListProviderTypeSpecialties = validSpecialites;
        }
    }

    public void BindEnrollStatus()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;

        if (ddlEnrollStatus.Items.Count == 0)
        {
            ds = psc.GetEnrollmentStatusType();

            if (ds == null || ds.Tables == null)
            {
                return;
            }

            DataTable enrollStatusList = ds.Tables[0];

            Helper.LoadList(ddlEnrollStatus, enrollStatusList, "Enroll_Status_Desc", "ENROLL_STATUS_ID", true);
        }
    }

    public void BindEnrollStatusReason()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;

        if (ddlEnrollStatusReason.Items.Count == 0)
        {
            ds = psc.SelectEnrollmentStatusReasons();

            if (ds == null || ds.Tables == null)
            {
                return;
            }

            DataTable enrollStatusReasonList = ds.Tables[0];

            Helper.LoadList(ddlEnrollStatusReason, enrollStatusReasonList, "Enrollment_Status_Reasons_Desc", "Enrollment_Status_Reasons_ID", true);
        }
    }

    public override void LoadData(DataRow dr)
    {
        ParentTable.Rows[0].Cells[2].Style["display"] = Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        BindSpeciality(chkIsPrimary.Checked);
        lblDuplicate.Visible = false;
        lblFutureDateEnrollment.Visible = false;
        lblActiveSpec.Visible = false;
        hdnRegSpecialtyID.Value = string.Empty;
        hdnSpecialtyID.Value = string.Empty;
        hdnSpecStart.Value = string.Empty;
        bool isInternalUser = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
        BindEnrollStatus();
        BindEnrollStatusReason();

        trEnrollStatus.Visible = IsInternalUser();
        trEnrollStatusReason.Visible = IsInternalUser();

        if (dr != null)
        {
            hdnRegSpecialtyID.Value = Helper.GetData("REG_SPECIALTY_ID", dr);
            int specialtyTypeID = 0;

            if (!string.IsNullOrEmpty(Helper.GetData("SPECIALTY_TYPE_ID", dr)))
            {
                specialtyTypeID = Helper.GetInt("SPECIALTY_TYPE_ID", dr);
                foreach (RadComboBoxItem li in rcbSpecialty.Items)
                {
                    if (specialtyTypeID.ToString().Equals(li.Value))
                    {
                        rcbSpecialty.SelectedValue = specialtyTypeID.ToString();
                        hdnSpecialtyID.Value = specialtyTypeID.ToString();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(Helper.GetData("ENROLL_STATUS_ID", dr)) &&
                ddlEnrollStatus.Items.FindByValue(Helper.GetData("ENROLL_STATUS_ID", dr)) != null)
            {
                var enrollStatusID = Convert.ToString(Helper.GetInt("ENROLL_STATUS_ID", dr));
                ddlEnrollStatus.SelectedValue = enrollStatusID;
            }

            if (!string.IsNullOrEmpty(Helper.GetData("ENROLLMENT_STATUS_REASONS_ID", dr)) &&
                ddlEnrollStatusReason.Items.FindByValue(Helper.GetData("ENROLLMENT_STATUS_REASONS_ID", dr)) != null)
            {
                var enrollStatusReasonsID = Convert.ToString(Helper.GetInt("ENROLLMENT_STATUS_REASONS_ID", dr));
                ddlEnrollStatusReason.SelectedValue = enrollStatusReasonsID;
            }

            // Get the board state and name

            if (!string.IsNullOrEmpty(Helper.GetData("START_DATE", dr)))
            {
                txtSpecStart.Text = Helper.GetDate("START_DATE", dr).ToString();
                hdnSpecStart.Value = Helper.GetDate("START_DATE", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("END_DATE", dr)))
            {
                txtSpecEnd.Text = Helper.GetDate("END_DATE", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("PRIMARY_FLAG", dr)))
            {
                chkIsPrimary.Checked = Helper.GetBool("PRIMARY_FLAG", dr);
            }



            DataSet ds = psc.SelectGroupSpecialtiesByProviderTypeRole(ProviderTypeId, isInternalUser, this.WorkflowPage.RegistrationId);
            bool canbePrimarySpec = ds.Tables[0].AsEnumerable()
                                    .Where(r => r.Field<int>("SPECIALTY_TYPE_ID") == specialtyTypeID &&
                                                (r.Field<string>("PT_PS_ALLOWED_PORTAL") == "P" || r.Field<string>("PT_PS_ALLOWED_PORTAL") == "B"))
                                    .Any();

            chkIsPrimary.Enabled = canbePrimarySpec;

            specialtyDetail.Visible = true;
        }
        else
        {
            if (rcbSpecialty.Items.Count > 0)
            {
                rcbSpecialty.SelectedIndex = 0;
            }
            chkIsPrimary.Enabled = true;

        }

        if (isStreamlinedApp && !isInternalUser)
        {
            chkIsPrimary.Checked = true;
            chkIsPrimary.Enabled = false;
        }


    }

    public void LoadHRSAData(DataRow dr)
    {
        //HRSATable.Rows[0].Cells[2].Style["display"] = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name) ? "block" : "none";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (dr != null)
        {
            if (!string.IsNullOrEmpty(Helper.GetData("ID_340B", dr)))
            {
                txtHRSA_ID.Text = Helper.GetDate("ID_340B", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("START_DATE", dr)))
            {
                txtHRSAEffective.Text = Helper.GetDate("START_DATE", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("END_DATE", dr)))
            {
                txtHRSAEnd.Text = Helper.GetDate("END_DATE", dr).ToString();
            }
        }


    }
    protected void grdHRSA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        HRSA340BDetail.Visible = true;
        DataRow dr = this.DataList1.Rows[index];

        if (Helper.HasRows(this.DataList1))
            this.LoadHRSAData(dr);
        else
            this.LoadHRSAData(null);

    }

    protected void grd_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        // disable Enroll Status Reason column if this isn't an internal user
        if (!IsInternalUser())
        {
            grdSpecialties.MasterTableView.GetColumn("EnrollStatusReason").Display = false;
        }
    }

    protected void grd_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (Convert.ToString(e.CommandArgument) != "System.Web.UI.Pair" && !(e.Item is GridHeaderItem) && !(e.Item is GridFilteringItem))
        {
            int regSpecialtyId = Convert.ToInt32(e.CommandArgument);
            DataRow dr = findDataRowByRegSpecialtyId(regSpecialtyId);

            string logMsg = String.Format("For Specialty grid current selected row value is " + regSpecialtyId.ToString() + " grd_RowCommand method of Specialty has called", this.WorkflowPage.RegistrationId);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));

            specialtyDetail.Visible = false;

            if (Helper.IsUserInSpecialityInternalRoles(HttpContext.Current.User.Identity.Name))
                ddlEnrollStatus.Enabled = true;
            else
                ddlEnrollStatus.Enabled = false;

            if (e.CommandName == "DeleteSpecialtiesRow")
            {
                bool IsDeleted = DeleteSpecialty(dr);

                if (IsDeleted)
                {
                    PlaceholderUploadSectionOHrise.Controls.Clear();
                    LoadSpecialties();
                    log.CreateLogEntry(string.Format("Specialty has been deleted", logMsg, Logging.LogPriority.Information));
                }
            }
            else
            {
                if (Helper.HasRows(this.DataList))
                {
                    this.LoadData(dr);
                    hdnEndDate.Value = defaultEndDate;
                    var endDate = Helper.GetData("END_DATE", dr);
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        hdnEndDate.Value = Convert.ToDateTime(endDate).ToString("MMddyyyy");
                    }
                    hdnEnrollStatusID.Value = Helper.GetData("ENROLL_STATUS_ID", dr);
                    hdnSpecialtyID.Value = Helper.GetData("SPECIALTY_TYPE_ID", dr);
                    string sentToSIflag = hdnSentToSI.Value = Helper.GetData("SENT_TO_SI", dr);
                    if (!string.IsNullOrEmpty(sentToSIflag) && Convert.ToBoolean(sentToSIflag))
                    {
                        rcbSpecialty.Enabled = false;
                    }

                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    parms.Add("REG_SPECIALTY_ID", Helper.GetData("REG_SPECIALTY_ID", dr));
                    DataSet ds = speclist = dsSpecialties = psc.SelectRegistrationDataWithParams("usp_GetRegSpecialtyAdditionalFields", parms);

                    BuildDynamicControls(ds);
                }
                else
                    this.LoadData(null);
            }
            DisplayODHUpload();
        }
    }

    private DataRow findDataRowByRegSpecialtyId(int regSpecialtyId)
    {
        for (int i = 0; i < this.DataList.Rows.Count; i++)
        {
            int rowRegSpecialtyId = Convert.ToInt32(this.DataList.Rows[i]["REG_SPECIALTY_ID"]);
            if (rowRegSpecialtyId == regSpecialtyId)
            {
                return this.DataList.Rows[i];
            }
        }

        return null; // didn't find it, return null
    }
    public class DynamicFieldConfig
    {
        public string FieldName { get; set; }
        public string SectionType { get; set; }
        public string ControlType { get; set; }
        public string ControlId { get; set; }
        public string[] SelectValues { get; set; }
        public string MMISProviderTypeID { get; set; }
        public string DataType { get; set; }
        public string Options { get; set; }
    }
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        if (Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).Contains("ProviderAdministrator") || Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).Contains("ProviderAgent"))
        {
            calStart.Enabled = false;
            txtSpecStart.ReadOnly = true;
        }
        specialtyDetail.Visible = true;

        if (Helper.IsUserInSpecialityInternalRoles(HttpContext.Current.User.Identity.Name))
            ddlEnrollStatus.Enabled = true;
        else
            ddlEnrollStatus.Enabled = false;

        if (HasPrimary)
        {
            //Removed the logic for isPrimaryPastEndDate and making primary check box to false and from now always on adding a specialty primary check box will be enabled
            //chkIsPrimary.Checked = true;
            chkIsPrimary.Visible = true;
            lblPrimary.Visible = true;
            lblPrimaryMessage.Visible = true;
            lblPrimaryMessage.Text = GetGlobalResourceObject("BrandingResource", "PrimarySpecialty").ToString();


        }
        else
        {
            chkIsPrimary.Checked = true;
            lblPrimaryMessage.Visible = true;
            lblPrimaryMessage.Text = GetGlobalResourceObject("BrandingResource", "PrimarySpecialty").ToString();

        }

        BuildDynamicControls();

        this.LoadData(null);
    }
    private void BuildDynamicControls(DataSet ds = null)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
        string provider_type_id = provRow != null ? Helper.GetString("MMIS_Provider_Type_ID", provRow) : string.Empty;

        Dictionary<string, string> parms = new Dictionary<string, string>
    {
        { "mmis_Provider_type_id", provider_type_id },
        { "reg_section_type_id", MAXIMUS.Core.Libraries.Constants.SectionTypeID.Specialties.ToString() }
    };

        DataSet dsSPTyp = psc.SelectDynamicFieldConfiguration(parms);

        if (dsSPTyp != null && dsSPTyp.Tables.Count > 0)
        {
            DataTable dt = dsSPTyp.Tables[0];

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(dt.Rows[i]["control_level_id"]) == 1)
                {
                    dt.Rows.RemoveAt(i);
                }
            }
        }



        phAdditionalSpecialties.Controls.Clear();

        Table tbl = new Table();
        tbl.CssClass = "ble no-row-border";
        tbl.CssClass += " dynamic-table";


        if (Helper.HasRows(dsSPTyp))
        {
            TableRow row = new TableRow();
            TableCell lblCell = new TableCell();
            lblCell.HorizontalAlign = HorizontalAlign.Right;
            lblCell.CssClass = "align-middle pe-10"; // Bootstrap padding-end
            row.Cells.Add(lblCell);

            lblCell = new TableCell();
            lblCell.HorizontalAlign = HorizontalAlign.Left;
            lblCell.CssClass = "align-middle pe-10"; // Bootstrap padding-end
            Label lbl = new Label
            {
                Text = "Additional Fields",
                CssClass = "form-label fw-bold"
            };
            lbl.Style.Add("font-weight", "bold");

            lblCell.Controls.Add(lbl);
            row.Cells.Add(lblCell);
            tbl.Rows.Add(row);

            foreach (DataRow rowData in dsSPTyp.Tables[0].Rows)
            {
                string json = rowData["complete_json"].ToString();
                var config = JsonConvert.DeserializeObject<DynamicFieldConfig>(json);

                row = new TableRow();

                // Label cell (right aligned)
                lblCell = new TableCell();
                lblCell.HorizontalAlign = HorizontalAlign.Right;
                lblCell.CssClass = "align-middle pe-10"; // Bootstrap padding-end
                lbl = new Label
                {
                    Text = config.FieldName,
                    CssClass = "form-label fw-bold"
                };
                lblCell.Controls.Add(lbl);
                row.Cells.Add(lblCell);

                // Control cell (left aligned)
                TableCell ctrlCell = new TableCell();
                ctrlCell.HorizontalAlign = HorizontalAlign.Left;
                ctrlCell.CssClass = "align-middle";

                Control inputControl = null;

                switch (config.ControlType.ToLower())
                {
                    case "textbox":
                        TextBox txt = new TextBox
                        {
                            ID = !string.IsNullOrEmpty(config.ControlId) ? config.ControlId : "txt_" + config.FieldName,
                            CssClass = "form-control",
                            TextMode = config.DataType.Equals("DATE", StringComparison.OrdinalIgnoreCase) ? TextBoxMode.SingleLine : TextBoxMode.MultiLine,
                            Text = GetDynamicControlValuesByKey(ds, config.FieldName)
                        };
                        inputControl = txt;
                        break;

                    case "dropdownlist":
                        DropDownList ddl = new DropDownList
                        {
                            ID = !string.IsNullOrEmpty(config.ControlId) ? config.ControlId : "ddl_" + config.FieldName,
                            CssClass = "form-select"
                        };
                        if (config.SelectValues != null)
                        {
                            foreach (var val in config.SelectValues)
                            {
                                ddl.Items.Add(new ListItem(val));
                            }
                            ddl.SelectedValue = GetDynamicControlValuesByKey(ds, config.FieldName);
                        }
                        inputControl = ddl;
                        break;

                    case "checkbox":
                        CheckBox chk = new CheckBox
                        {
                            ID = !string.IsNullOrEmpty(config.ControlId) ? config.ControlId : "chk_" + config.FieldName,
                            Text = config.Options
                        };
                        inputControl = chk;
                        break;

                    case "radiobutton":
                        RadioButtonList rbl = new RadioButtonList
                        {
                            ID = !string.IsNullOrEmpty(config.ControlId) ? config.ControlId : "rbl_" + config.FieldName,
                            RepeatDirection = RepeatDirection.Horizontal,
                            RepeatLayout = RepeatLayout.Flow
                        };
                        if (config.SelectValues != null)
                        {
                            foreach (var val in config.SelectValues)
                            {

                                string text = default(string); ;
                                string childControlId = default(string);

                                if (val.Contains(":"))
                                {
                                    var parts = val.Split(':');
                                    text = parts[0];
                                    childControlId = parts.Length > 1 ? parts[1] : parts[0];
                                }

                                if (!string.IsNullOrEmpty(childControlId))
                                {
                                    rbl.AutoPostBack = true;

                                }

                                var item = new ListItem(!string.IsNullOrEmpty(text) ? text : val);

                                if (!string.IsNullOrEmpty(childControlId))
                                {
                                    item.Value = childControlId;
                                }
                                else
                                {
                                    item.Value = "";
                                }
                                item.Attributes["style"] = "margin-right:15px; padding:5px; font-size:14px; color:#333;";
                                rbl.Items.Add(item);

                            }
                            //rbl.SelectedValue = GetDynamicControlValuesByKey(ds, config.FieldName);
                            rbl.SelectedIndexChanged += Rbl_SelectedIndexChanged;
                        }
                        inputControl = rbl;
                        break;
                }

                if (inputControl != null)
                    ctrlCell.Controls.Add(inputControl);

                row.Cells.Add(ctrlCell);
                tbl.Rows.Add(row);
            }
        }

        Panel responsivePanel = new Panel();
        responsivePanel.CssClass = "table-responsive";
        responsivePanel.Controls.Add(tbl);
        phAdditionalSpecialties.Controls.Add(responsivePanel);


        phAdditionalSpecialties.Controls.Add(responsivePanel);
    }

    protected void Rbl_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Your logic here
        RadioButtonList rbl = (RadioButtonList)sender;
        string selectedValue = rbl.SelectedValue;

        Panel responsivePanel = phAdditionalSpecialties.FindControl("pnlDynamicControls") as Panel;
        if (responsivePanel != null)
        {
            phAdditionalSpecialties.Controls.Remove(responsivePanel);
        }

        if (!string.IsNullOrEmpty(selectedValue))
        {
            DataSet childControlData = LookupTableController.SelectDynamicFieldChildControls(Convert.ToInt32(selectedValue));
            Table tbl = new Table();
            tbl.CssClass = "ble no-row-border";
            tbl.CssClass += " dynamic-table";
            if (Helper.HasRows(childControlData))
            {
                TableRow row = new TableRow();
                TableCell lblCell = new TableCell();
                lblCell.HorizontalAlign = HorizontalAlign.Right;
                lblCell.CssClass = "align-middle pe-10"; // Bootstrap padding-end
                row.Cells.Add(lblCell);

                lblCell = new TableCell();
                lblCell.HorizontalAlign = HorizontalAlign.Left;
                lblCell.CssClass = "align-middle pe-10"; // Bootstrap padding-end
                Label lbl = new Label
                {
                    Text = "Additional Fields",
                    CssClass = "form-label fw-bold"
                };
                lbl.Style.Add("font-weight", "bold");

                lblCell.Controls.Add(lbl);
                row.Cells.Add(lblCell);
                tbl.Rows.Add(row);
                foreach (DataRow rowData in childControlData.Tables[0].Rows)
                {
                    string json = rowData["complete_json"].ToString();
                    var config = JsonConvert.DeserializeObject<DynamicFieldConfig>(json);

                    row = new TableRow();

                    // Label cell (right aligned)
                    lblCell = new TableCell();
                    lblCell.HorizontalAlign = HorizontalAlign.Right;
                    lblCell.CssClass = "align-middle pe-10"; // Bootstrap padding-end
                    lbl = new Label
                    {
                        Text = config.FieldName,
                        CssClass = "form-label fw-bold"
                    };
                    lblCell.Controls.Add(lbl);
                    row.Cells.Add(lblCell);

                    // Control cell (left aligned)
                    TableCell ctrlCell = new TableCell();
                    ctrlCell.HorizontalAlign = HorizontalAlign.Left;
                    ctrlCell.CssClass = "align-middle";

                    Control inputControl = null;

                    switch (config.ControlType.ToLower())
                    {
                        case "textbox":
                            TextBox txt = new TextBox
                            {
                                ID = !string.IsNullOrEmpty(config.ControlId) ? config.ControlId : "txt_" + config.FieldName,
                                CssClass = "form-control",
                                TextMode = config.DataType.Equals("DATE", StringComparison.OrdinalIgnoreCase) ? TextBoxMode.SingleLine : TextBoxMode.MultiLine,
                                Text = GetDynamicControlValuesByKey(childControlData, config.FieldName)
                            };
                            inputControl = txt;
                            break;
                    }

                    if (inputControl != null)
                        ctrlCell.Controls.Add(inputControl);

                    row.Cells.Add(ctrlCell);
                    tbl.Rows.Add(row);
                }
            }

            responsivePanel = new Panel();
            responsivePanel.ID = "pnlDynamicControls";
            responsivePanel.CssClass = "table-responsive";
            responsivePanel.Controls.Add(tbl);
            phAdditionalSpecialties.Controls.Add(responsivePanel);

            phAdditionalSpecialties.Controls.Add(responsivePanel);
        }

    }


    private string GetDynamicControlValuesByKey(DataSet ds, string key)
    {
        string value = string.Empty;
        if (Helper.HasRows(ds))
        {
            if (ds.Tables[0].Columns.Contains("field_name"))
            {
                var datarow = ds.Tables[0].Select(string.Format("field_name = '{0}'", key)).FirstOrDefault();
                value = Helper.GetData("selected_value", datarow);
            }
        }

        return value;
    }
    public void SaveAdditionalSpecialtyFields(int regSpecialtyId, int regid)
    {
        DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
        string provider_type_id = Helper.GetString("MMIS_Provider_Type_ID", provRow);

        List<DynamicFieldConfig> configs = new List<DynamicFieldConfig>();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("mmis_Provider_type_id", provider_type_id);
        parms.Add("reg_section_type_id", MAXIMUS.Core.Libraries.Constants.SectionTypeID.Specialties.ToString());
        DataSet dsSPTyp = psc.SelectDynamicFieldConfiguration(parms);
        // Reconstruct configs from your dataset
        foreach (DataRow row in dsSPTyp.Tables[0].Rows)
        {
            string json = row["complete_json"].ToString();
            var config = JsonConvert.DeserializeObject<DynamicFieldConfig>(json);
            configs.Add(config);
        }


        foreach (var config in configs)
        {
            string controlId = string.Empty;
            string selectedValue = string.Empty;

            if (config.ControlType.Equals("TextBox", StringComparison.OrdinalIgnoreCase))
            {
                controlId = "txt_" + config.FieldName;
                var txt = phAdditionalSpecialties.FindControl(controlId) as TextBox;
                if (txt != null)
                    selectedValue = txt.Text.Trim();
            }
            else if (config.ControlType.Equals("DropDownList", StringComparison.OrdinalIgnoreCase))
            {
                controlId = "ddl_" + config.FieldName;
                var ddl = phAdditionalSpecialties.FindControl(controlId) as DropDownList;
                if (ddl != null)
                    selectedValue = ddl.SelectedValue;
            }
            else if (config.ControlType.Equals("CheckBox", StringComparison.OrdinalIgnoreCase))
            {
                controlId = "chk_" + config.FieldName;
                var chk = phAdditionalSpecialties.FindControl(controlId) as CheckBox;
                if (chk != null)
                    selectedValue = chk.Checked ? "true" : "false";
            }
            else if (config.ControlType.Equals("RadioButton", StringComparison.OrdinalIgnoreCase))
            {
                controlId = "rbl_" + config.FieldName;
                var rbl = phAdditionalSpecialties.FindControl(controlId) as RadioButtonList;
                if (rbl != null)
                    selectedValue = rbl.SelectedValue;
            }

            parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("REG_specialty_ID", regSpecialtyId.ToString());
            parms.Add("FIELD_NAME", config.FieldName.ToString());
            parms.Add("REG_SECTION_TYPE_ID", config.SectionType.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            parms.Add("SELECTED_VALUE", selectedValue);

            // Call the PDMS service method
            psc.InsertRegistrationDataTable("SPECIALTY_ADDITIONAL_FIELDS", parms);

            // Save to DB

        }
    }

    protected void lbtnAddHRSA_Click(object sender, CommandEventArgs e)
    {
        lblOhrRiseError.Visible = false;
        HRSA340BDetail.Visible = true;
        this.LoadHRSAData(null);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Specialties":
                lblSpHistoryTitle.Text = "Specialties History";
                ucSpecialtiesHistory.LoadData(0);
                mpe.Show();
                break;
            default:
                break;
        }
    }

    public bool SaveHRSAData()
    {
        //Added If condition to check data entry screen visible before validating, to make sure Next click button works as expected.
        if (HRSA340BDetail.Visible)
        {
            Page.Validate("valHRSA340B");

            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("valHRSA340B") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }


            if (ValidateHRSAData())
            {
                try
                {

                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    int regspecialtyid = 0;
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("MMIS_SPECIALTY_TYPE_ID", CON.MMISSpecialtyType.Pharmacy_340B));
                    DataSet dsSpecialtyTypeId = DataAccess.ExecuteStoredProcedure("usp_SelectSpecialtyTypeIdByMMISSpecialtyTypeId", param, "SPECIALTY_TYPE");
                    int SpecialtyTypeID = Convert.ToInt16(dsSpecialtyTypeId.Tables[0].Rows[0]["SPECIALTY_TYPE_ID"]);
                    bool duplicate = psc.VerifyDuplicateSpecialtyForReg(this.WorkflowPage.RegistrationId, SpecialtyTypeID, regspecialtyid, DateTime.Now);
                    if (!duplicate)
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("PRIMARY_FLAG", "0");
                        parms.Add("SPECIALTY_TYPE_ID", Convert.ToString(SpecialtyTypeID));
                        parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                        parms.Add("START_DATE", DateTime.Now.ToString());
                        if (txtSpecEnd.Text.Trim() == "")
                            parms.Add("END_DATE", null);
                        else
                            parms.Add("END_DATE", Convert.ToDateTime(txtHRSAEnd.Text).ToString());
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value))
                        {
                            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                            parms.Add("REG_SPECIALTY_ID", hdnRegSpecialtyID.Value);
                            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY", parms);
                        }
                        else
                        {
                            parms.Add("ENROLL_STATUS_ID", CON.EnrollStatus.INACTIVE.ToString());
                            parms.Add("ENROLLMENT_STATUS_REASONS_ID", CON.EnrollStatusReasonID.InActive.ToString());
                            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Inserted.ToString());
                            psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom2", parms);
                        }
                    }

                    Dictionary<string, string> HRSAparms = new Dictionary<string, string>();
                    HRSAparms = new Dictionary<string, string>();
                    HRSAparms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    HRSAparms.Add("ID_340B", Convert.ToString(txtHRSA_ID.Text));
                    HRSAparms.Add("START_DATE", Convert.ToDateTime(txtHRSAEffective.Text).ToString());
                    if (txtHRSAEnd.Text.Trim() == "")
                        HRSAparms.Add("END_DATE", null);
                    else
                        HRSAparms.Add("END_DATE", Convert.ToDateTime(txtHRSAEnd.Text).ToString());
                    HRSAparms.Add("LAST_MODIFIED_DATE_TIME", Convert.ToString(DateTime.Now));
                    HRSAparms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "HRSA340B", HRSAparms);

                    return true;

                }
                catch (Exception ex)
                {
                    throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucSpecialties_SaveData - " + ex.Message));
                }
            }

            return false;
        }
        else
        {
            //if no details screen visible then no need to save.
            return true;
        }
    }
    protected void btnSave_Click1(object sender, CommandEventArgs e)
    {
        this.SaveHRSAData();
        this.LoadControlData();
        ClearHRSADataEntryFields();
    }
    protected void btnCancel_Click(object sender, CommandEventArgs e)
    {
        this.txtHRSA_ID.Text = string.Empty;
        this.txtHRSAEffective.Text = string.Empty;

    }
    protected void chkIsPrimary_CheckedChanged(object sender, EventArgs e)
    {
        bool isChkPrimaryChanged = true;
        BindSpeciality(chkIsPrimary.Checked, isChkPrimaryChanged);
        rcbSpecialty.SelectedValue = !string.IsNullOrEmpty(hdnSpecialtyID.Value) ? hdnSpecialtyID.Value : string.Empty;
    }

    protected void ddlSpeciality_Changed(object sender, EventArgs e)
    {
        var hdn = hdnRegSpecialtyID.Value;
        DataTable dt = this.DataListProviderTypeSpecialties;
        string providerTypeID = svc.GetProviderTypeIdByRegId(this.WorkflowPage.RegistrationId);
        //OHPNM-8614
        lblMarkerSpecialties.Visible = false;
        bool IsMarkerSpecialtiesExist = false;
        string[] specialty = CON.MarkerSpecialtiesMMISTypeId.Split(',');
        bool isInternalUser = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
        string Specialty = rcbSpecialty.SelectedItem.Text;
        string SpecialtyMMISTypeId = Specialty.Split('-')[0].Trim();
        foreach (string Spl in specialty)
        {
            if (Spl == SpecialtyMMISTypeId) IsMarkerSpecialtiesExist = true;
        }

        HiddenField hdnUnsavedData = (HiddenField)Parent.FindControl("HasUnsavedData");
        if (hdnUnsavedData != null)
        {
            hdnUnsavedData.Value = "TrueSpeciality";
        }

        if (rcbSpecialty.SelectedIndex > 0)
        {

            if (chkIsPrimary.Checked == true
                && (rcbSpecialty.SelectedItem.Text.Split('-')[0].ToString().Trim() == "OHR")
                && providerTypeID != CON.MMISProviderType.MANAGED_CARE_ORGANIZATION_PANEL_PROVIDER_ONLY
                && providerTypeID != CON.MMISProviderType.Non_Agency_Personal_Care_Aide
                && providerTypeID != CON.MMISProviderType.WAIVERED_SERVICES_ORGANIZATION)
            {
                lblOhrRiseError.Visible = true;
                ClearPanelFields();
                return;

            }
            else if ((rcbSpecialty.SelectedItem.Text.Split('-')[0].ToString().Trim() == "ORR") && chkIsPrimary.Checked == true)
            {
                lblSpecialtySelectError.Text = "ORR - OHIORISE WAIVER OUT OF HOME RESPITE cannot be added as Primary Specialty";
                lblSpecialtySelectError.Visible = true;
                ClearPanelFields();
                return;
            }
            else if ((rcbSpecialty.SelectedItem.Text.Split('-')[0].ToString().Trim() == "ORC") && providerTypeID == "01" && chkIsPrimary.Checked == true)
            {
                lblSpecialtySelectError.Text = "ORC - CANS ASSESSOR cannot be selected as Primary Specialty";
                lblSpecialtySelectError.Visible = true;
                ClearPanelFields();
                return;
            }
            else if ((IsMarkerSpecialtiesExist) && chkIsPrimary.Checked == true && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist) && (providerTypeID == "54" || providerTypeID == "20" || providerTypeID == "60"))
            {
                lblMarkerSpecialties.Visible = true;
                return;
            }
            else
            {
                lblOhrRiseError.Visible = false;
                lblSpecialtySelectError.Text = string.Empty;
                lblSpecialtySelectError.Visible = false;
            }
        }

        if (dt == null)
        {
            return;
        }


        if (string.IsNullOrEmpty(hdnRegSpecialtyID.Value))
        {
            if (rcbSpecialty.SelectedIndex > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    if (Helper.GetData("SPECIALTY_TYPE_ID", dr) == rcbSpecialty.SelectedValue)
                    {
                        //enable only the checkbox if PT_PS_ALLOWED_PORTAL is set to "B"(Both)
                        if (Helper.GetData("PT_PS_ALLOWED_PORTAL", dr) == "B")
                        {
                            chkIsPrimary.Enabled = true;
                        }
                        if (Helper.GetData("MMIS_SPECIALTY_TYPE_ID", dr) == CON.MMISSpecialtyType.OHRISE_CANS)
                        {
                            ODHUploadPanel.Visible = true;
                        }
                        else
                        {
                            ODHUploadPanel.Visible = false;
                        }

                        break;
                    }


                }
            }
        }

        if (chkIsPrimary.Checked && this.rcbSpecialty.SelectedValue != null && this.rcbSpecialty.SelectedItem.Text.ToUpper().Contains("PRE-NATAL POSTPARTUM NURSE HOME VISITOR"))
        {
            txtSpecStart.Text = DateTime.Today.ToShortDateString();
            txtSpecStart.Enabled = false;
            txtSpecEnd.Text = "12/31/2299";
            txtSpecEnd.Enabled = false;
        }
        else
        {
            txtSpecStart.Enabled = true;
            txtSpecEnd.Enabled = true;
        }

        if (isStreamlinedApp && !isInternalUser)
        {
            chkIsPrimary.Checked = true;
            chkIsPrimary.Enabled = false;
        }

    }

    protected void ddlEnrollStatus_Changed(object sender, EventArgs e)
    {

    }

    protected void ddlEnrollStatusReason_Changed(object sender, EventArgs e)
    {

    }

    public override bool SaveData()
    {
        // OHPNM-18353 - Convert From ORP WF as the page is read only nothing to save.
        if (isConvertFrmORPWF || isStreamlinedApp)
        {
            return true;
        }
        if (!Helper.IsValidDate(this.txtSpecStart.Text.Trim(), false))
        {
            lblDateValidCheck.Visible = true;
            return false;
        }

        bool IsMarkerSpecialtiesExist = false;
        string[] specialty = CON.MarkerSpecialtiesMMISTypeId.Split(',');
        string providerTypeID = svc.GetProviderTypeIdByRegId(this.WorkflowPage.RegistrationId);

        string logMsg = String.Format("SaveData method of Specialty has called,ProviderTypeID is " + providerTypeID, this.WorkflowPage.RegistrationId);
        Logging log = new Logging(this.ThreadId, logMsg);
        log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));

        if (rcbSpecialty.SelectedItem != null)
        {
            string Specialty = rcbSpecialty.SelectedItem.Text;
            string SpecialtyMMISTypeId = Specialty.Split('-')[0].Trim();
            foreach (string Spl in specialty)
            {
                if (Spl == SpecialtyMMISTypeId) IsMarkerSpecialtiesExist = true;
            }

        }
        //Added If condition to check data entry screen visible before validating, to make sure Next click button works as expected.
        if (ODHUploadPanel.Visible)
        {
            foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadODH.Controls)
            {
                svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, 0);
            }
        }

        if (DivOHRiseSpecialties.Visible)
        {
            foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadSectionOHrise.Controls)
            {
                svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, 0);
            }
        }


        if (specialtyDetail.Visible)
        {
            Page.Validate(ValidationGroup);

            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals(ValidationGroup) && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }

            string[] MMISProviderTypesForIndividualNurse = new string[] { "65", "71", "72" };

            if (_DisplayConfirmAdd && !MMISProviderTypesForIndividualNurse.Contains(this.WorkflowPage.MMISProviderTypeID))
            {
                if (this.WorkflowPage.MedicaidID == null || this.WorkflowPage.MedicaidID.Length == 0)
                {
                    lblConfirm.Text = "If you add this specialty, this will be your only specialty.";
                }
                else
                {
                    lblConfirm.Text = "If you add this specialty, this will become your only specialty. All of your other specialties will be end dated.";
                }
                mpeConfirmAdd.Show();
                return false;
            }
            if ((IsMarkerSpecialtiesExist) && chkIsPrimary.Checked == true && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.EnrollmentSpecialist) && (providerTypeID == "54" || providerTypeID == "20" || providerTypeID == "60"))
            {
                lblMarkerSpecialties.Visible = true;
                return false;
            }
            return AddSpecialty();
        }
        else
        {
            // OHPNM-2229
            //if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && this.DataList != null & this.DataList.Rows != null && this.DataList.Rows.Count == 0)
            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdSpecialties.Items.Count == 0)
            {
                // this page is required, but there aren't any items, so don't let them 'save' the data on the screen
                return false;
            }

            //if no details screen visible then no need to save.
            return true;
        }
    }

    private void AddError(string errMsg, ref bool isGood, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }
    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (!string.IsNullOrEmpty(this.txtSpecStart.Text) || !string.IsNullOrEmpty(this.txtSpecEnd.Text) || !string.IsNullOrEmpty(rcbSpecialty.SelectedValue))
            isRequired = true;
        return isRequired;
    }

    public bool ValidateHRSAData()
    {
        bool isValid = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        //Do validation only when its a data entry
        if (HRSA340BDetail.Visible)
        {
            if (!Helper.IsValidDate(this.txtHRSAEffective.Text.Trim(), true))
            {
                AddError("A valid Effective Date (mm/dd/yyyy) is required.", ref isValid, "valHRSA340B");
                isValid = false;
            }

            if (!Helper.IsValidDate(this.txtHRSAEnd.Text.Trim(), false))
            {
                AddError("End Date (mm/dd/yyyy) is not valid.", ref isValid, "valHRSA340B");
                isValid = false;
            }

            if (txtHRSAEnd.Text != "")
            {
                if (!IsInternalUser() && Convert.ToDateTime(txtHRSAEffective.Text) >= Convert.ToDateTime(txtHRSAEnd.Text))
                {
                    AddError("* End date need to be greater than start date.", ref isValid, "valHRSA340B");
                    isValid = false;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int statusCode = Helper.GetInt("RegistrationProgramStatusTypeID", ds.Tables[0].Rows[0]);
                    if (statusCode != 6)
                    {
                        int result = DateTime.Compare(Convert.ToDateTime(txtHRSAEnd.Text), DateTime.Now);

                        if (!IsInternalUser() && result < 0) // expiration is before today's date
                        {
                            AddError("* End date need to be greater than today's date.", ref isValid, "valHRSA340B");
                            isValid = false;
                        }
                    }
                }
            }
        }

        return isValid;
    }
    public override bool ValidateData()
    {
        bool isValid = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        bool isInternalUser = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
        //Do validation only when its a data entry
        if (specialtyDetail.Visible)
        {
            string ptspAllowed = string.Empty;
            string mmisSpecialtyTypeID = string.Empty;
            DataRow drSPID = null;
            DataSet dsSPTyp = psc.SelectGroupSpecialtiesByProviderTypeRole(ProviderTypeId, isInternalUser, this.WorkflowPage.RegistrationId);
            if (Helper.HasRows(dsSPTyp) && !string.IsNullOrEmpty(rcbSpecialty.SelectedValue))
            {
                drSPID = dsSPTyp.Tables[0].Select(String.Format("SPECIALTY_TYPE_ID = '{0}'", rcbSpecialty.SelectedValue)).FirstOrDefault();
                ptspAllowed = Helper.GetString("PT_PS_ALLOWED_PORTAL", drSPID);
                // See if we need to display the confirmation dialog 
                _DisplayConfirmAdd = Helper.GetString("MMIS_SPECIALTY_TYPE_ID", drSPID).Equals("386") ? true : false;

                mmisSpecialtyTypeID = Helper.GetString("MMIS_SPECIALTY_TYPE_ID", drSPID);
                DateTime specEffectiveDt = Helper.GetDateTime("SPECIALTY_EFFECTIVE_DATE", drSPID);

                // OHPNM-15507 SAM536 Start dates for 45S and 45V cannot be prior to 7/1/2025
                if ((mmisSpecialtyTypeID == "45V" || mmisSpecialtyTypeID == "45S")
                    && Convert.ToDateTime(txtSpecStart.Text) < Convert.ToDateTime(specEffectiveDt))
                {
                    AddError(String.Format("* Specialty Start date for specialty type {0} cannot be prior to 07/01/2025", mmisSpecialtyTypeID), ref isValid, ValidationGroup);
                    return isValid;
                }
            }
            int regspecialtyid = 0;
            if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value.ToString()))
            {
                regspecialtyid = Convert.ToInt32(hdnRegSpecialtyID.Value.ToString());
            }
            var primaryStatus = svc.ValidateIsPrimarySpeciality(this.WorkflowPage.RegistrationId, regspecialtyid);
            //var primaryCount = (DataList!=null && DataList.Rows.Count>0)? (from pc in DataList.AsEnumerable()
            //                    where  pc.Field<Boolean>("PRIMARY_FLAG")== true
            //                    select pc).Count():0;
            DataTable dt = (DataTable)grdSpecialties.DataSource;
            if ((this.rcbSpecialty.SelectedItem.Text.ToUpper().Contains("847 - IHBT")) && !Helper.HasRows(dt))
            {
                AddError("* Cannot added IHBT as primary", ref isValid, ValidationGroup);
                return isValid;
            }
            if (primaryStatus == false && !chkIsPrimary.Checked)
            {
                AddError("* Please select a Specialty as Primary.", ref isValid, ValidationGroup);
                return isValid;
            }
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                var changeEffectiveDate = Helper.GetDate("ChangeEffectiveDate", ds.Tables[0].Rows[0]);

                if (!string.IsNullOrWhiteSpace(changeEffectiveDate)) //OHPNM-12485 - Check if speciality date is greater than eefective date otherwise show error
                {
                    if (Convert.ToDateTime(txtSpecStart.Text) < Convert.ToDateTime(changeEffectiveDate))
                    {
                        AddError("Specialty start date cannot be prior to effective date", ref isValid, ValidationGroup);
                        isValid = false;
                    }
                }
                else if (Methods.ToNullableDateTime(this.txtSpecStart.Text.Trim()) == null) //In case effective date not available then do a min date check on start date to avoid sql date exceptions
                {
                    AddError("Specialty start date cannot be prior to effective date", ref isValid, ValidationGroup);
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(this.txtSpecEnd.Text.Trim()))
            {
                DateTime dtEndDate;
                bool validateEndDate = DateTime.TryParseExact(this.txtSpecEnd.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEndDate);
                if (!validateEndDate)
                    this.txtSpecEnd.Text = "12/31/2299";
            }

            if (!Helper.IsValidDate(this.txtSpecEnd.Text.Trim(), false))
            {
                AddError("End Date (mm/dd/yyyy) is not valid.", ref isValid, ValidationGroup);
                isValid = false;
            }

            if (txtSpecEnd.Text != "")
            {
                if (Convert.ToDateTime(txtSpecStart.Text) >= Convert.ToDateTime(txtSpecEnd.Text))//!IsInternalUser() && -- before it was for only innternaluser now the code is for everyone 
                {
                    AddError("* End date need to be greater than start date.", ref isValid, ValidationGroup);
                    isValid = false;
                }
                else
                {
                    if (chkIsPrimary.Checked && this.rcbSpecialty.SelectedValue != null && this.rcbSpecialty.SelectedItem.Text.ToUpper().Contains("PRE-NATAL POSTPARTUM NURSE HOME VISITOR")
                        && (this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.Nurse_Midwife_Individual ||
                            this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.Clinical_Nurse_Specialist_Individual ||
                            this.WorkflowPage.MMISProviderTypeID == CON.MMISProviderType.Nurse_Practitioner_Individual))
                    {
                        AddError("The selected speciality cannot be marked as Primary.", ref isValid, ValidationGroup);
                        isValid = false;
                    }
                    else if (chkIsPrimary.Checked)
                    {
                        var specEndDate = Convert.ToDateTime(txtSpecEnd.Text.Trim());
                        var tomorrowDate = DateTime.Today.AddDays(1);
                        if (specEndDate.Date == tomorrowDate)
                        {
                            AddError("End date should not be tomorrow's date.", ref isValid, ValidationGroup);
                            isValid = false;
                        }
                    }
                }

                // OHPNM-15852 allow historic specialties to be entered, remove validation
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    int statusCode = Helper.GetInt("RegistrationProgramStatusTypeID", ds.Tables[0].Rows[0]);
                //    if (statusCode != 6)
                //    {
                //        int result = DateTime.Compare(Convert.ToDateTime(txtSpecEnd.Text), DateTime.Now);
                //        if (!IsInternalUser() && result < 0) // expiration is before today's date
                //        {
                //            AddError("* End date need to be greater than today's date.", ref isValid, ValidationGroup);
                //            isValid = false;
                //        }
                //    }
                //}
            }

            if (chkIsPrimary.Checked && ptspAllowed.Equals("S"))
            {
                AddError("* This specialty cannot be saved as the primary, please select a different primary specialty.", ref isValid, ValidationGroup);
            }

            //OHPNM-16094:Updating All PT 19 ES to"Reporting Only and ESRC to Non-Participating RPT Only And updating specialty190 need to be the primary in all instances on this PT type
            if (rcbSpecialty.SelectedItem != null)
            {
                string Specialty = rcbSpecialty.SelectedItem.Text;
                string SpecialtyMMISTypeId = Specialty.Split('-')[0].Trim();
                if (this.WorkflowPage.MMISProviderTypeID == "19" && ((SpecialtyMMISTypeId == "190" && chkIsPrimary.Checked == false) || (SpecialtyMMISTypeId != "190" && chkIsPrimary.Checked == true)))
                {
                    lblSpecialtySelectError.Text = "Primary Specialty for Managed Care Organization Provider should be 190(MCO Provider Only)";
                    lblSpecialtySelectError.Visible = true;
                    return false;

                }
            }

            // if adding check to see if we need to display the ODH panel
            DisplayODHUpload();
        }

        if (ODHUploadPanel.Visible)
        {
            // Check that a file has been uploaded for ODH Nurse Home Visit Documentation of Certification
            foreach (Control ctrl in PlaceholderUploadODH.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                uploadControl.IsRequired = true;

                isValid &= uploadControl.ValidateData("vsFormODH");
            }
        }

        if (DivOHRiseSpecialties.Visible)
        {
            // Check that a file has been uploaded for ODH Nurse Home Visit Documentation of Certification
            foreach (Control ctrl in PlaceholderUploadSectionOHrise.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                uploadControl.IsRequired = uploadControl.IsRequired;

                isValid &= uploadControl.ValidateData("vsFormOHRise");
            }
        }

        return isValid;
    }

    public override string ValidationGroup
    {
        get { return "valSpecialties"; }
    }

    public override string Title
    {
        get { return "Edit Specialty"; }

    }

    public override string IdText
    {
        get { return "ucSpecialties_" + this.WorkflowPage.RegistrationId; }
    }

    private bool DeleteSpecialty(DataRow dr)
    {
        bool isValid = true;

        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.Specialties)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This specialty cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_SPECIALTY_ID = Helper.GetInt("REG_SPECIALTY_ID", dr);
            int SPECIALTY_ID = Helper.GetInt("SPECIALTY_ID", dr);
            bool IsPrimary = Helper.GetBool("PRIMARY_FLAG", dr);

            string logMsg = String.Format("DeleteSpecialty method has been called,IsPrimary ? " + IsPrimary + " and Specialty ID is " + SPECIALTY_ID, this.WorkflowPage.RegistrationId);
            Logging log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));

            if (SPECIALTY_ID == 0 && !IsPrimary)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("SPECIALTY", "REG_SPECIALTY_ID", REG_SPECIALTY_ID);
                log.CreateLogEntry(string.Format("Specialty has been deleted, SPECIALTY_ID is 0 and it is not primary", logMsg, Logging.LogPriority.Information));
            }
            else
            {
                if (IsPrimary)
                    AddError("* The Primary Specialty can not be deleted.", ref isValid, ValidationGroup);
                else
                    AddError("* Specialty cannot be deleted.", ref isValid, ValidationGroup);

                return isValid;
            }
        }
        return isValid;
    }
    private void ClearHRSADataEntryFields()
    {
        this.txtHRSA_ID.Text = string.Empty;
        this.txtHRSAEffective.Text = string.Empty;
        HRSA340BDetail.Visible = false;
        pnlHRSA.Update();

    }
    private void ClearPanelFields()
    {
        rcbSpecialty.SelectedIndex = 0;
        txtSpecStart.Text = string.Empty;
        txtSpecEnd.Text = string.Empty;
        ddlEnrollStatus.SelectedIndex = 0;
        ddlEnrollStatusReason.SelectedIndex = 0;
    }
    private void DisplayODHUpload()
    {
        bool showODH = false;
        bool showOHrise = false;
        int regspecialtyid = 0;
        if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value.ToString()))
        {
            regspecialtyid = Convert.ToInt32(hdnRegSpecialtyID.Value.ToString());
        }
        if (DataList != null && DataList.Rows.Count > 0)
        {
            string whereStmt = "(MMIS_SPECIALTY_TYPE_ID = '386')";
            if (regspecialtyid > 0)
            {
                whereStmt += " and REG_SPECIALTY_ID <> " + regspecialtyid.ToString();
            }
            DataRow[] rowsPrimary = this.DataList.Select(whereStmt);
            if (rowsPrimary != null && rowsPrimary.Length > 0) showODH = true;
        }
        if (DataList != null && DataList.Rows.Count > 0)
        {
            foreach (DataRow dr in DataList.Rows)
            {
                if (dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.OHRISE_FMS ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.OHRISE_MRSS ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.OHRISE_IHBT ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.FAMILY_CONNECT ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.OHRISE_CANS ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.OHRISE_BHR ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.OHRISE_TSS ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.LACTATION_CONSULTANT_SERVICES ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.PEDIATRIC_RECOVERY_SPECIALTY ||
                    dr["MMIS_SPECIALTY_TYPE_ID"].ToString() == CON.MMISSpecialtyType.STRUCTIRED_FAMILY_CAREGIVER_SERVICE)
                {
                    showOHrise = true; break;
                }
            }
        }

        ODHUploadPanel.Visible = showODH;
        //SAM769 When specialties page is read only in these wfs do not show add new button
        if (!(isConvertFrmORPWF || isStreamlinedApp))
            btnAddSpecialties.Visible = !showODH;

        if (showODH)
        {
            LoadPlaceHolder();
        }
        DivOHRiseSpecialties.Visible = showOHrise;
        if (showOHrise)
        {
            LoadPlaceHolder(true, false, _SectionName, showOHrise);
        }
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                btnAddSpecialties.Visible = false;
            }
        }
    }

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


    public void LoadPlaceHolder(bool isEdit = true, bool loadViewState = false, string pageSection = _SectionName, bool loadOHrise = false)
    {
        Upload upload = new Upload();
        PlaceholderUploadODH.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, 0, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, 0, pageSection, isEdit);
        //Filter out null REG_SECTION_UPLOAD_CONTROL_SPECIALTY_ID records
        DataRow[] filteredRows = ds.Tables[0].Select("REG_SECTION_UPLOAD_CONTROL_SPECIALTY_ID IS NOT NULL");

        if (filteredRows.Length > 0)
        {
            foreach (DataRow dr in filteredRows)//ds.Tables[i].Rows)
            {
                //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                if ((dr.Table.Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = _SectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                if (loadOHrise)
                {
                    if (PlaceholderUploadSectionOHrise.Controls.Count > 0)
                    {

                        if (!PlaceholderUploadSectionOHrise.Controls.OfType<Control>().Any(x => x.ID == ucUploadSectionControl.ID))
                        {
                            PlaceholderUploadSectionOHrise.Controls.Add(ucUploadSectionControl);
                        }
                    }
                    else
                    {
                        PlaceholderUploadSectionOHrise.Controls.Add(ucUploadSectionControl);
                    }
                }
                else
                {
                    PlaceholderUploadODH.Controls.Add(ucUploadSectionControl);

                }

            }

        }

    }
    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        mpeConfirmAdd.Hide();
    }

    protected void btnSaveAdd_Click(object sender, EventArgs e)
    {

        chkIsPrimary.Checked = true;
        _DisplayConfirmAdd = false;

        string logMsg = String.Format("btnSaveAdd_Click method of Specialty has called", this.WorkflowPage.RegistrationId);
        Logging log = new Logging(this.ThreadId, logMsg);
        log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));

        if (AddSpecialty())
        {
            if (this.WorkflowPage.MedicaidID == null || this.WorkflowPage.MedicaidID.Length == 0)
            {
                DeleteAllExistingSpeciaties();
                log.CreateLogEntry(string.Format("All the specialties have been deleted,MedicaidID is null or empty", logMsg, Logging.LogPriority.Information));
            }
            else
            {
                SetEndDateOnExistingSpecialties();
            }
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DateTime startDate = Convert.ToDateTime(txtSpecStart.Text);
            psc.CreateContractForSpecialty386(this.WorkflowPage.RegistrationId, startDate, Helper.GetUserId(HttpContext.Current.User.Identity.Name));

            mpeConfirmAdd.Hide();
            specialtyDetail.Visible = false;
            // Clear the hidden value so the display doesn't ignore the row if it's being updated
            hdnRegSpecialtyID.Value = "";
            this.LoadControlData();
        }

    }

    private bool AddSpecialty()
    {
        Label lblError = (Label)Page.Master.FindControl("lblspecilityErrormsg");
        if (lblError != null) lblError.Text = string.Empty;
        if (ValidateData())
        {
            try
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                int regspecialtyid = 0;
                if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value.ToString()))
                {
                    regspecialtyid = Convert.ToInt32(hdnRegSpecialtyID.Value.ToString());
                }
                lblDuplicate.Visible = false;
                lblFutureDateEnrollment.Visible = false;
                lblActiveSpec.Visible = false;
                bool hasActiveEnrollment = false;
                lblOnlyPastStartDate.Visible = false;

                if (txtSpecStart.Text != "" && hdnSpecStart.Value != "" && regspecialtyid > 0 && !string.IsNullOrEmpty(hdnSentToSI.Value.ToString()) && hdnSentToSI.Value.ToLower() == "true")
                {
                    if (IsInternalUser() && Convert.ToDateTime(txtSpecStart.Text) > Convert.ToDateTime(hdnSpecStart.Value))
                    {
                        lblOnlyPastStartDate.Visible = true;
                        return false;
                    }
                }

                //OHPNM-8443-Check if provide has one active enrollment in case new enrollment is for future date
                if (Convert.ToDateTime(txtSpecStart.Text) > DateTime.Now.Date)
                {
                    hasActiveEnrollment = psc.VerifyFutureDatedSpecialtyEnrollmentForReg(this.WorkflowPage.RegistrationId, int.Parse(rcbSpecialty.SelectedValue), regspecialtyid, Convert.ToDateTime(txtSpecStart.Text));
                }
                if (hasActiveEnrollment)
                {
                    lblFutureDateEnrollment.Visible = true;
                    return false;
                }

                DataSet dsInvalid = psc.VerifyIfCreatesInvalidSpecialtySpanForReg(this.WorkflowPage.RegistrationId, int.Parse(rcbSpecialty.SelectedValue), regspecialtyid, Convert.ToDateTime(txtSpecStart.Text), Convert.ToDateTime(txtSpecEnd.Text));
                bool ifCreatsInvalidSpan = false;
                int enrollStatus = 0;
                if (dsInvalid.Tables[0].Rows.Count > 0)
                {
                    if (int.Parse(dsInvalid.Tables[0].Rows[0]["RecordCount"].ToString()) > 0)
                        ifCreatsInvalidSpan = true;
                    enrollStatus = int.Parse(dsInvalid.Tables[0].Rows[0]["ENROLLMENT_STATUS_CODE"].ToString());
                }

                if (ifCreatsInvalidSpan)
                {
                    if (enrollStatus == CON.EnrollStatus.INACTIVE)
                    {
                        lblInvalidSpan.Visible = true;
                        lblDuplicate.Visible = false;
                    }
                    else
                    {
                        lblDuplicate.Visible = true;
                        lblInvalidSpan.Visible = false;
                    }
                    return false;
                }

                bool duplicate = psc.VerifyDuplicateSpecialtyForReg(this.WorkflowPage.RegistrationId, int.Parse(rcbSpecialty.SelectedValue), regspecialtyid, Convert.ToDateTime(txtSpecStart.Text));
                if (duplicate && string.IsNullOrWhiteSpace(hdnRegSpecialtyID.Value))
                {
                    bool activeSpec = psc.VerifyActiveSpecialtyForRegBySpecialtyTypeID(this.WorkflowPage.RegistrationId, int.Parse(rcbSpecialty.SelectedValue), Convert.ToDateTime(txtSpecStart.Text));
                    if (activeSpec)
                    {
                        lblActiveSpec.Visible = true;
                        return false;
                    }
                    else
                    {
                        duplicate = false;
                    }
                }
                if (!duplicate)
                {

                    // Update the Specialty
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    //parms.Add("PRIMARY_FLAG", (IsPrimary ? "1" : "0"));
                    parms.Add("PRIMARY_FLAG", (chkIsPrimary.Checked) ? "1" : "0");
                    parms.Add("SPECIALTY_TYPE_ID", rcbSpecialty.SelectedValue);
                    parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                    parms.Add("START_DATE", Convert.ToDateTime(txtSpecStart.Text).ToString());
                    if (txtSpecEnd.Text.Trim() == "")
                        parms.Add("END_DATE", Convert.ToDateTime(defaultEndDate).ToString());
                    else
                        parms.Add("END_DATE", Convert.ToDateTime(txtSpecEnd.Text).ToString());
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value))
                    {
                        string mmis_SpecialtyTypeID = "";
                        bool isInternalUser = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
                        DataSet dsSPTyp = psc.SelectGroupSpecialtiesByProviderTypeRole(ProviderTypeId, isInternalUser, this.WorkflowPage.RegistrationId);
                        if (Helper.HasRows(dsSPTyp) && !string.IsNullOrEmpty(rcbSpecialty.SelectedValue))
                        {
                            var drSPID = dsSPTyp.Tables[0].Select(String.Format("SPECIALTY_TYPE_ID = '{0}'", rcbSpecialty.SelectedValue)).FirstOrDefault();
                            mmis_SpecialtyTypeID = Helper.GetString("MMIS_SPECIALTY_TYPE_ID", drSPID);
                        }

                        if (!IsInternalUser())
                        {
                            if (chkIsPrimary.Checked && hdnEndDate.Value != Convert.ToDateTime(txtSpecEnd.Text).ToString("MMddyyyy"))
                            {
                                lblPrimaryEndDateWarning.Visible = true;
                                return false;
                            }
                            if (Convert.ToInt32(hdnEnrollStatusID.Value) == CON.EnrollStatus.INACTIVE)
                            {
                                lblInactiveEditWarning.Visible = true;
                                return false;
                            }
                            else if (mmis_SpecialtyTypeID == "480" || mmis_SpecialtyTypeID == "490" || mmis_SpecialtyTypeID == "740")
                            {
                                lblEndDateWarning.Visible = true;
                                return false;
                            }
                        }
                        else //OHPNM-4561 OHPNM-5035
                        {
                            parms.Add("ENROLL_STATUS_ID", ddlEnrollStatus.SelectedValue);
                            parms.Add("ENROLLMENT_STATUS_REASONS_ID", ddlEnrollStatusReason.SelectedValue);
                        }

                        parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                        parms.Add("REG_SPECIALTY_ID", hdnRegSpecialtyID.Value);

                        psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom", parms);
                        SaveAdditionalSpecialtyFields(int.Parse(hdnRegSpecialtyID.Value), this.WorkflowPage.RegistrationId);
                    }
                    else
                    {
                        // SAM505
                        psc.SetAddBCITextRTPEmailFlag(this.WorkflowPage.RegistrationId, int.Parse(rcbSpecialty.SelectedValue));

                        parms.Add("ENROLL_STATUS_ID", !string.IsNullOrWhiteSpace(ddlEnrollStatus.SelectedValue) ? ddlEnrollStatus.SelectedValue : CON.EnrollStatus.INACTIVE.ToString());
                        parms.Add("ENROLLMENT_STATUS_REASONS_ID", !string.IsNullOrEmpty(ddlEnrollStatusReason.SelectedValue) ? ddlEnrollStatusReason.SelectedValue : CON.EnrollStatusReasonID.InActive.ToString());
                        parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Inserted.ToString());
                        int regspecid = psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom2", parms);

                        SaveAdditionalSpecialtyFields(regspecid, this.WorkflowPage.RegistrationId);
                    }
                    return true;
                }
                else
                {
                    lblDuplicate.Visible = true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucSpecialties_SaveData - " + ex.Message));
            }
        }

        return false;
    }

    private void SetEndDateOnExistingSpecialties()
    {

        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            DataTable dt = (DataTable)grdSpecialties.DataSource;
            DateTime endDate = Convert.ToDateTime(txtSpecStart.Text).AddDays(-1);
            foreach (DataRow row in dt.Rows)
            {
                DateTime rowEndDate = (DateTime)row["END_DATE"];

                if (rowEndDate > endDate)
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    parms.Add("PRIMARY_FLAG", Helper.GetData("PRIMARY_FLAG", row));
                    parms.Add("SPECIALTY_TYPE_ID", Helper.GetData("SPECIALTY_TYPE_ID", row));
                    parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                    parms.Add("START_DATE", Helper.GetData("START_DATE", row));

                    parms.Add("END_DATE", endDate.ToString());
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                    parms.Add("REG_SPECIALTY_ID", Helper.GetData("REG_SPECIALTY_ID", row));
                    psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom", parms);
                }


            }

        }
        catch (Exception ex)
        {
            throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucSpecialties_SaveData - " + ex.Message));
        }
        return;

    }

    private void DeleteAllExistingSpeciaties()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataTable dt = (DataTable)grdSpecialties.DataSource;

        string logMsg = String.Format("DeleteAllExistingSpeciaties method has been called to Delete all Existing Speciaties", this.WorkflowPage.RegistrationId);
        Logging log = new Logging(this.ThreadId, logMsg);
        log.CreateLogEntry(string.Format(logMsg, Logging.LogPriority.Information));

        // If they are updating an existing record then don't delete it 
        int regspecialtyid = 0;
        if (!string.IsNullOrEmpty(hdnRegSpecialtyID.Value.ToString()))
        {
            regspecialtyid = Convert.ToInt32(hdnRegSpecialtyID.Value.ToString());
        }
        if (dt != null)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (regspecialtyid == 0 || regspecialtyid != Helper.GetInt("REG_SPECIALTY_ID", row))
                {
                    psc.DeleteRegistrationData("SPECIALTY", "REG_SPECIALTY_ID", Helper.GetInt("REG_SPECIALTY_ID", row));
                }
                log.CreateLogEntry(string.Format("Deleted all existing Specialties", logMsg, Logging.LogPriority.Information));
            }
        }

    }

    public DataSet Loadspeclist()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", "0");
        DataSet ds = speclist = dsSpecialties = psc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTY", parms);
        return ds;
    }

    protected void grd_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridFilteringItem)
        {
            GridFilteringItem eItem = (GridFilteringItem)e.Item;

            Telerik.Web.UI.RadComboBox combo = (Telerik.Web.UI.RadComboBox)eItem.FindControl("RadComboBoxEnrollStatus");
            combo.DataSource = null;
            combo.Items.Clear();
            DataTable dt = EnrollStatusTypes.Tables[0].DefaultView.ToTable(true, new String[] { "ENROLL_STATUS_DESC" });
            DataRow dr = dt.NewRow();
            dr["ENROLL_STATUS_DESC"] = "All";
            dt.Rows.InsertAt(dr, 0);
            combo.DataSource = dt;
            combo.DataBind();

            combo.SelectedValue = ((GridItem)e.Item).OwnerTableView.GetColumn("EnrollStatus").CurrentFilterValue;
        }
    }

    DataSet GetEnrollmentStatusType()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetEnrollmentStatusType();
    }
}