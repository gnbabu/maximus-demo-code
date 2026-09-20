using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_CredentialQueue : RegistrationProvider
{

    private const int npiColumn = 4;
    private const int hiddenColumn = 8;
    //Below user is credential chair role and being used for demo purpose ONLY.
    //may need to replace later
    //private const string committeeChairUser="nwang";  
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
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
            //AutoAssign providers to Chair role which are unassigned.
           // AutoAssignUnassignedtoChairRole();
        }
        else
        {
            Page.Theme = "Default";
        }
    }

    //private void AutoAssignUnassignedtoChairRole()
    //{
    //    string roleName = string.Empty;
    //    int processId, stepId = 0;
    //    try
    //    {
    //        if (Helper.IsUserInRole(committeeChairUser, CON.UserRole.CredentialCommittee))
    //        {
    //            roleName = CON.UserRole.CredentialCommittee;

    //            DataSet dsUnassigned = svc.WF_SelectUnassignedSteps(roleName);

    //            if (Helper.HasRows(dsUnassigned))
    //            {
    //                foreach (DataRow dr in dsUnassigned.Tables[0].Rows)
    //                {
    //                     processId = Helper.GetInt("PROCESS_ID", dr);
    //                     stepId = svc.WF_StartStep(processId, Helper.GetUserId(committeeChairUser).ToString());
    //                }
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            SessionVarRetriever.ClearRegistrationSessionVars();

            BindCredentialProvidersGrid(GetCredentialQueueRecords());
        }
       
      
    }

    private void ShowNoShowCheckList(DataTable dtShow)
    {
        DataTable dtFilteredProviders = dtShow;
        bool isVisible = false;
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair))
        {
            if (Helper.HasRows(dtFilteredProviders))
            {
                int totalCount = dtFilteredProviders.Rows.Count;
                DataView dvPassDataList = dtFilteredProviders.DefaultView;
                //dvPassDataList.RowFilter = "DataRankID = " + Convert.ToInt32(CON.ActivityDataRankId.Pass.ToString());
                dvPassDataList.RowFilter = "credential_risk_level = 1";
                DataTable dtProviderPassAction = dvPassDataList.ToTable(); //dtFilteredProviders.Select(string.Format("DataRank = {0} ", CON.CommitteeCredentialActivityStatusId.Pass.ToString())).CopyToDataTable();
                int activityPassCount = Helper.HasRows(dtProviderPassAction) ? dtProviderPassAction.Rows.Count : 0;
                isVisible = (totalCount == activityPassCount) ? true : false;
            }           
        }
        chkAllCredential.Visible = isVisible;
    }

   
    #region Grid Functions
    private DataTable GetCredentialQueueRecords()
    {
        DataSet dsQueue = new DataSet();
        try
        {
            string roleName = Helper.GetUserRole(HttpContext.Current.User.Identity.Name);
            dsQueue = svc.WF_SelectActiveOwnerStepsCredentialProvider(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false, roleName);
           // dsQueue = svc.WF_SelectUnassignedSteps(CON.UserRole.CredentialCommittee);
        }
        catch (Exception ex)
        {
        }
        return dsQueue.Tables[0];
    }
    protected void btnCredentialSearch_Click(object sender, EventArgs e)
    {
        DataTable dtSearch = null;
        try
        {
            this.lblMessage.Visible = false;
                dtSearch = FilterSearchRecords();
                BindCredentialProvidersGrid(dtSearch);

        }
        catch (Exception ex)
        {

        }

    }
    protected void btnCredentialClear_Click(object sender, EventArgs e)
    {
        credentialSearch.ClearFormData();
        this.lblMessage.Visible = false;
        DataTable dtReturn = new DataTable();
        dtReturn = GetCredentialQueueRecords();
        BindCredentialProvidersGrid(dtReturn);
    }
    private DataTable FilterSearchRecords()
    {
        DataTable dtReturn = new DataTable();
        StringBuilder filterString = new StringBuilder();
       
        dtReturn = GetCredentialQueueRecords();

        if (credentialSearch.isSearchFieldsExists() && Helper.HasRows(dtReturn))
        {
            filterString.Clear();
            if(!string.IsNullOrEmpty(credentialSearch.ProviderName))
            {
                filterString.Append(string.Format("PROVIDER_NAME LIKE '%{0}%'", credentialSearch.ProviderName));
            }

            if (!string.IsNullOrEmpty(credentialSearch.MMISProviderTypeId))
            {
                filterString = string.IsNullOrEmpty(filterString.ToString()) ? filterString.Append(string.Format("MMIS_PROVIDER_TYPE_ID = '{0}'", credentialSearch.MMISProviderTypeId) ): filterString.Append(string.Format(" AND MMIS_PROVIDER_TYPE_ID = '{0}'", credentialSearch.MMISProviderTypeId));
            }
            if (credentialSearch.DataRank >0)
            {
                filterString = string.IsNullOrEmpty(filterString.ToString()) ? filterString.Append(string.Format("credential_risk_level = '{0}'", credentialSearch.DataRank)) : filterString.Append(string.Format(" AND credential_risk_level = '{0}'", credentialSearch.DataRank));
            }
            if (credentialSearch.WorkflowId>0)
            {
                filterString = string.IsNullOrEmpty(filterString.ToString()) ? filterString.Append(string.Format("WORKFLOW_ID = '{0}'", credentialSearch.WorkflowId)) : filterString.Append(string.Format(" AND WORKFLOW_ID = '{0}'", credentialSearch.WorkflowId));
            }
            DataTable dtFiltered = null;
            if(!string.IsNullOrEmpty(filterString.ToString()))
            {
                DataRow[] filteredRows = dtReturn.Select(filterString.ToString());
                dtFiltered = filteredRows.Length > 0 ? filteredRows.CopyToDataTable() : null;
                dtReturn = dtFiltered;
            }         
                     
            
        }
        //
        //ShowNoShowCheckList(dtReturn);

        return dtReturn;
    }
    protected void gvCredentialProviders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "GoToRegistration")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvCredentialProviders.Rows[index];
                int processId = string.IsNullOrEmpty(this.gvCredentialProviders.DataKeys[index].Values["PROCESS_ID"].ToString()) ? 0 : Convert.ToInt32(this.gvCredentialProviders.DataKeys[index].Values["PROCESS_ID"].ToString());
                int stepId = svc.WF_StartStep(processId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                int regID = string.IsNullOrEmpty(this.gvCredentialProviders.DataKeys[index].Values["REG_ID"].ToString()) ? 0 : Convert.ToInt32(this.gvCredentialProviders.DataKeys[index].Values["REG_ID"].ToString());
                HiddenField hdnPage = (HiddenField)row.FindControl("hdnPage");
                LinkButton lnkBtn = (LinkButton)row.FindControl("LnkBtnName");
                (this.Page as RegistrationProvider).RegistrationId = regID;
                (this.Page as RegistrationProvider).IsReadOnly = false;
                if (hdnPage.Value.Contains("Step="))
                {
                    string queryString = hdnPage.Value.Split('?')[1];
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                    (this.Page as RegistrationProvider).RegistrationStep = int.Parse(queryDictionary["Step"]);
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void chkAllCredential_CheckedChanged(object sender, EventArgs e)
    {

        /* JIRA 229
            string chairRole = "Chairman";
        */
        if (chkAllCredential.Checked)
        {
            //Bind Credential Mmebers.
            /* JIRA 229
            DataSet dsMembers = CredentialController.GetCredentialCommitteeMembers();
            if (Helper.HasRows(dsMembers))
            {
                var memberRows = dsMembers.Tables[0].AsEnumerable().Where(r => r.Field<string>("ROLE") != chairRole).CopyToDataTable();

                if (Helper.HasRows(memberRows))
                {
                    chklstCommitteeMember.DataSource = memberRows;
                    chklstCommitteeMember.DataTextField = "MEMBER_NAME";
                    chklstCommitteeMember.DataValueField = "MEMBER_USERNAME";
                    chklstCommitteeMember.DataBind();
                }
            }
            */
            mpeCredentialApprove.Show();
        }
    }
    private void BindCredentialProvidersGrid(DataTable dtProvidersList = null)
    {
        
        gvCredentialProviders.DataSource = dtProvidersList;
        gvCredentialProviders.DataBind();
        ShowNoShowCheckList(dtProvidersList);
    }

    protected void gvCredentialProviders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk = e.Row.FindControl("LnkBtnName") as LinkButton;
            Label lbl = e.Row.FindControl("lblName") as Label;            
            string riskLevel = (string)DataBinder.Eval(e.Row.DataItem, "CredentialRiskLevelName");
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair) && riskLevel != "Low")
            {
                lnk.Visible = false;
                lbl.Visible = true;
            }
                
        }
        if (e.Row.Cells[npiColumn].Text == "9999999999") e.Row.Cells[npiColumn].Text = string.Empty;
    }
    protected void gvCredentialProviders_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[hiddenColumn].CssClass = "hiddencol";
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        { e.Row.Cells[hiddenColumn].CssClass = "hiddencol"; }
    }
    #endregion

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        DataTable dtProviderApproved = FilterSearchRecords();
        try
        {
            if (Helper.HasRows(dtProviderApproved))
            {
                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair))
                {

                    foreach (DataRow dr in dtProviderApproved.Rows)
                    {
                    string regId = Helper.GetString("REG_ID", dr);
                    int credentialId = Helper.GetInt("credentialing_id", dr);
                    /* JIRA 229
                     List<string> committeeMembers = GetSelectedCommitteeMembers();
                    */
                    string approvalMsg = string.Format(CON.CredentialDefaultApprovalMessage, DateTime.Now.ToString());
                    int committeeResult = Convert.ToInt16(CON.CommitteeCredentialActivityStatusId.Pass.ToString());
                    //First Update Committee members result                  
                    int processId = Helper.GetInt("PROCESS_ID", dr);
                    int stepId = svc.WF_StartStep(processId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        /* JIRA 229
                        if (committeeMembers.Count > 0)
                            {
                                foreach (string member in committeeMembers)
                                {

                                svc.UpdateCredentialingCommitteeMember(credentialId, committeeResult, member, approvalMsg, DateTime.Now, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                                }
                            }
                        */
                        //second update committee chair person
                        //svc.UpdateCredentialingCommitteeMember(credentialId, committeeResult, HttpContext.Current.User.Identity.Name, approvalMsg, DateTime.Now, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        //third update credential screening result  
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", regId);
                        parms.Add("CREDENTIALING_ID", credentialId.ToString());
                        parms.Add("Committee_Result_id", CON.CredentialingResult.Pass.ToString());
                        parms.Add("COMMITTEE_DENIAL_TERM_REASON", string.Empty);
                        parms.Add("Committee_summary", string.Empty);
                        parms.Add("Committee_Date", DateTime.Now.ToString());
                        parms.Add("Committee_Discussion", string.Empty);
                        parms.Add("Last_Modified_Date_Time", DateTime.Now.ToString());
                        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                        svc.UpdateRegistrationDataTable("CREDENTIALING", parms);
                        svc.UpdateCredentialResult(Convert.ToInt32(regId), CON.CredentialingResult.Pass, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        svc.UpdateCredentialStatus(Convert.ToInt32(regId), CON.CredentilaingStatus.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                        //Move to next task after approval.
                        if (processId != 0 && !string.IsNullOrEmpty(regId))
                        {
                            svc.WF_TakeAction(processId, "Approve", string.Empty);
                            //Save credential screen to approved 
                            Registration.SetNodeStatusId(Convert.ToInt32(regId), CON.SectionTypeID.ProviderCredentialing, CON.RegistrationProviderServicesStatusTypeId.Approved);
                        }
                    }
                    lblMessage.Text = "All selected Providers have been approved.";
                    lblMessage.Visible = true;
                }
            }
            
            BindCredentialProvidersGrid(GetCredentialQueueRecords());
           
        }
        catch (Exception ex)
        {


        }
        mpeCredentialApprove.Hide();
        chkAllCredential.Checked = false;
    }

    /* JIRA 229
    private List<string> GetSelectedCommitteeMembers()
    {
        List<string> liMembers = new List<string>();
        try
        {
            if (chklstCommitteeMember != null)
            {
                foreach (ListItem Item in chklstCommitteeMember.Items)
                {
                    if (Item.Selected)
                        liMembers.Add(Item.Value.ToString().Trim());
                }

            }
        }
        catch (Exception ex)
        {
        }
        return liMembers;
    }
    */
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mpeCredentialApprove.Hide();
        chkAllCredential.Checked = false;
    }
    protected void gvCredentialProviders_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvCredentialProviders_PageIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected string GetFormattedCredentialDataRankDescription(Object ObjRank,Object objDesc)
    {
        string retDescription = string.Empty;
        DataTable dtQueue = GetCredentialQueueRecords();
        int iDataRankId=(ObjRank!=null && Helper.HasRows(dtQueue))?Convert.ToInt16(ObjRank):0;
        
        if (objDesc != null)
        {
            switch (iDataRankId)
            {
                case CON.ActivityDataRankId.pending:
                    retDescription = "<b><font color='black'>" + objDesc.ToString() + "</font></b>";
                    break;
                case CON.ActivityDataRankId.Fail:
                case CON.ActivityDataRankId.Conditional:
                case CON.ActivityDataRankId.Unconfirmed:
                    retDescription = "<b><font color='red'>" + objDesc.ToString() + "</font></b>"; 
                    break;
                case CON.ActivityDataRankId.Pass:
                    retDescription = "<b><font color='green'>" + objDesc.ToString() + "</font></b>"; 
                    break;        
                 


                default:
                    break;
            }
        }
        return retDescription;


    }
}