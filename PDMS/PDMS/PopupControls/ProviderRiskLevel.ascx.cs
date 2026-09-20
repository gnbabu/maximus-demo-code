using Corp.Core.Libraries.Helper;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ProviderRiskLevel : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public bool isSaved = false;
    int riskLevelID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectProviderRiskLevels();
            if (Helper.HasRows(ds))
            {
                ddlRiskLevel.DataTextField = "PROVIDER_RISK_LEVEL_NAME";
                ddlRiskLevel.DataValueField = "PROVIDER_RISK_LEVEL_ID";
                ddlRiskLevel.DataSource = ds.Tables[0];
                ddlRiskLevel.DataBind();
                if (ddlRiskLevel.Items[0].Text == "Undefined")
                {
                    ddlRiskLevel.Items[0].Text = "";
                }
            }
            ds = psc.SelectBumpUpReasons();
            if (Helper.HasRows(ds))
            {
                Helper.LoadList(this.ddlBumpUpReason, ds.Tables[0], "BUMP_UP_REASON_DESC", "BUMP_UP_REASON_ID", true);

            }
        }
    }
    public void LoadControls()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        
        if (Helper.HasRows(ds))
        {
            DataTable dtProvider = ds.Tables[0];
            riskLevelID = Helper.GetInt("PROVIDER_RISK_LEVEL_ID", dtProvider.Rows[0]);
        }
        lblCurrentProviderRiskLevel.Text = "";
        foreach (ListItem item in ddlRiskLevel.Items)
        {
            if (item.Value == riskLevelID.ToString())
            {
                lblCurrentProviderRiskLevel.Text = item.Text;
            }
        }
    }

    protected void ddl_validate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(WorkflowPage.RegistrationId);
        DataRow dr = ds.Tables[0].Rows[0];
        int RegistrationProgramStatusTypeID = Helper.GetInt("RegProgramStatusTypeID", dr);
        int ProviderRisklevelID = Helper.GetInt("PROVIDER_RISK_LEVEL_ID", dr);

        riskLevelID = ProviderRisklevelID;
        if (riskLevelID < Convert.ToInt32(ddlRiskLevel.SelectedValue) && ddlBumpUpReason.SelectedIndex < 1)
        {
            args.IsValid = false;
            ddl_validate.ErrorMessage = "Bump Up reason is required.";
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        int regid = WorkflowPage.RegistrationId;
        //parms.Add("REG_ID", regid.ToString());
        //parms.Add("PROVIDER_RISK_LEVEL_ID", ddlRiskLevel.SelectedValue);
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(regid);
        DataRow dr = ds.Tables[0].Rows[0];
        int RegistrationProgramStatusTypeID = Helper.GetInt("RegProgramStatusTypeID", dr);
        int ProviderRisklevelID = Helper.GetInt("PROVIDER_RISK_LEVEL_ID", dr);

        riskLevelID = ProviderRisklevelID;
        
        if (riskLevelID > Convert.ToInt32(ddlRiskLevel.SelectedValue) && string.IsNullOrEmpty(txtComments.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Comments is required.";
            val.ValidationGroup = "valProviderRiskLevel";
            this.Page.Validators.Add(val);
        }
        
        
            this.Page.Validate("valProviderRiskLevel");
            if (Page.IsValid)
            {
                //if (RegistrationProgramStatusTypeID != CON.RegistrationProgramStatusTypeId.Maintenance)
                //psc.UpdateRegistrationDataTable("PROVIDERCustom", parms);

                int RegPageTypeId = Registration.GetRegPageTypeId(WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                {
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                }

            // jira 2194
            //if (!string.IsNullOrEmpty(txtComments.Text))
            //    {
            //    psc.InsertRegProviderNote(regid, RegPageTypeId, 0, CON.ProviderNoteTypeId.Internal, txtComments.Text.Trim(),
            //                    DateTime.Now, WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            //}
            //psc.InsertRegProviderNote(regid, RegPageTypeId, 0, CON.ProviderNoteTypeId.Internal, "Provider Risk level changed from " + lblCurrentProviderRiskLevel.Text + " to " + ddlRiskLevel.SelectedItem.Text,



              //ProviderFeedHelper.InsertProviderFeedNotes(regID, 0, string.Empty, " Sent Email - " + subject, null, "Job", "Disenrolled", ProcessID); DateTime.Now, WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


            ProviderFeedHelper.InsertProviderFeedNotes(regid, 0, HttpContext.Current.User.Identity.Name, " Set Provider Risk Level - " + txtComments.Text, null, null, null, WorkflowPage.WF_ProcessID);

            if (Convert.ToInt32(ddlRiskLevel.SelectedItem.Value) != ProviderRisklevelID)
            {
                //get registration
                string revalDueWindow = Helper.GetAppSettingFromDB("RevalidationDueWindow");
                    int RevalidationDueWindow = Convert.ToInt32(revalDueWindow) * -1;
                    string dtTerminationDate = ObjectControllerHelper.GetString("TerminationDate", dr);
                    DateTime? dtRevalidationDate = ObjectControllerHelper.GetDateTime("RevalidationDate", dr);

                    bool revalidationNeeded = string.IsNullOrEmpty(dtTerminationDate) && ((dtRevalidationDate.HasValue && dtRevalidationDate.Value.AddDays(RevalidationDueWindow) <= DateTime.Today) || (dtRevalidationDate == null && RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion));

                    if (revalidationNeeded)
                    {
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", regid.ToString());
                        parms.Add("PROVIDER_RISK_LEVEL_ID", ddlRiskLevel.SelectedValue);
                        parms.Add("BUMP_UP_REASON_ID", ddlBumpUpReason.SelectedValue);
                        if (Convert.ToInt32(ddlRiskLevel.SelectedValue) > riskLevelID)
                        {
                            parms.Add("RISK_LEVEL_STATUS_ID", CON.RiskLevelStatus.Increased.ToString());
                        }
                        else
                        {
                            parms.Add("RISK_LEVEL_STATUS_ID", CON.RiskLevelStatus.Decreased.ToString());
                        }
                        psc.UpdateRegistrationDataTable("PROVIDERCustom", parms);
                        string pdmsurl = AppSettings.Get("PDMS-URL");
                        Guid requestGuid = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                        psc.NotifyRiskLevelBumpUp(string.Empty, string.Empty, pdmsurl, this.WorkflowPage.RegistrationId, requestGuid);
                    }
                    if (Helper.HasRows(ds))
                    {
                        int newRegID = 0;
                        newRegID = regid;

                        //if in maintenance
                        //TODO: add to version tables before creating process
                        //INSERT INTO VERSION Tables....
                        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                        DataSet ds1 = svc.InsertIntoVersionTables(regid, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                        DataRow dr1;
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            dr1 = ds1.Tables[0].Rows[0];
                            if (!string.IsNullOrEmpty(Methods.GetStringValue(dr1["ErrorMessage"])))
                            {
                                
                                
                                    CustomValidator val = new CustomValidator();
                                    val.IsValid = false;
                                    val.ErrorMessage = Methods.GetStringValue(dr1["ErrorMessage"]);
                                    val.ValidationGroup = "valProviderRiskLevel";
                                    this.Page.Validators.Add(val);
                                CustomValidator val1 = new CustomValidator();
                                val1.IsValid = false;
                                val1.ErrorMessage = "CopyToVersionTables returned 0 rows.";
                                val1.ValidationGroup = "valProviderRiskLevel";
                                this.Page.Validators.Add(val1);

                                


                                this.Page.Validate("valProviderRiskLevel");
                                return;
                            }
                        }
                        /*very important above TODO*/
                        int EntryTaskID = CON.RiskLevelEntryTaskID;
                        //Spawn new workflow
                        Workflow.Process pr = new Workflow.Process(19, null, Guid.NewGuid(), EntryTaskID);
                            if (pr == null)
                            {

                                return;
                            }

                            // Save the Registration ID as a process parameter of the Workflow
                            psc.WF_SaveProcessParameter(pr.ProcessID, "REGISTRATION_ID", newRegID.ToString());

                            psc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", newRegID.ToString() }, { "WORKFLOW_EVENT_TYPE_ID", "4" } });
                            parms = new Dictionary<string, string>();
                            parms.Add("REG_ID", newRegID.ToString());
                            parms.Add("PROVIDER_RISK_LEVEL_ID", ddlRiskLevel.SelectedValue);
                            parms.Add("BUMP_UP_REASON_ID", ddlBumpUpReason.SelectedValue);
                            if (Convert.ToInt32(ddlRiskLevel.SelectedValue) > riskLevelID)
                            {
                                parms.Add("RISK_LEVEL_STATUS_ID", CON.RiskLevelStatus.Increased.ToString());
                            }
                            else
                            {
                                parms.Add("RISK_LEVEL_STATUS_ID", CON.RiskLevelStatus.Decreased.ToString());
                            }
                            psc.UpdateRegistrationDataTable("PROVIDERCustom", parms);
                            parms = new Dictionary<string, string>();
                            parms.Add("REG_ID", newRegID.ToString());
                            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                            parms.Add("BUMP_UP_START_DATE_TIME", DateTime.Now.ToString());
                            int REGEventInfoID = 0;
                            DataSet ds2 = psc.SelectRegistrationData(newRegID, "EVENT_INFO");
                            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                            {
                                REGEventInfoID = Convert.ToInt32(ds2.Tables[0].Rows[0]["REG_EVENT_INFO_ID"]);
                                parms.Add("REG_EVENT_INFO_ID", REGEventInfoID.ToString());
                                psc.UpdateRegistrationDataTable("EVENT_INFO", parms);
                            }
                            else
                            {
                                psc.InsertRegistrationDataTable("EVENT_INFO", parms);
                            }
                            this.Page.Response.Redirect("~/Process/Registration.aspx");
                    }
            }
            isSaved = true;
            }
            else
            {
                return;
            }
        
    }

    protected void ddlBumpUpReason_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}