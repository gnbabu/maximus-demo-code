using Corp.Core.Libraries;
using Corp.Core.Libraries.Interface;
using Corp.Core.Libraries.Proxy;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Data.Extensions;
using Telerik.Web.UI;
using Telerik.Web.UI.Widgets;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_Agreements : BaseSectionControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public int EntityTypeId
    {
        get
        {
            if (ViewState["EntityTypeId"] == null) ViewState["EntityTypeId"] = 0;
            return Convert.ToInt32(ViewState["EntityTypeId"]);
        }
        set { ViewState["EntityTypeId"] = value; }
    }

    public int ProviderTypeId
    {
        get
        {
            if (ViewState["ProviderTypeId"] == null) ViewState["ProviderTypeId"] = 0;
            return Convert.ToInt32(ViewState["ProviderTypeId"]);
        }
        set { ViewState["ProviderTypeId"] = value; }
    }


    public int DIDDReferralId
    {
        get
        {
            if (ViewState["DIDDReferralId"] == null) ViewState["DIDDReferralId"] = 0;
            return Convert.ToInt32(ViewState["DIDDReferralId"]);
        }
        set { ViewState["DIDDReferralId"] = value; }
    }

    public int SpecialtyTypeID
    {
        get
        {
            if (ViewState["SpecialtyTypeID"] == null) ViewState["SpecialtyTypeID"] = 0;
            return Convert.ToInt32(ViewState["SpecialtyTypeID"]);
        }
        set { ViewState["SpecialtyTypeID"] = value; }
    }

    private bool _isLTCVisible = false;
    private DataTable _dtYesNoQuestions = new DataTable();
    private bool _isAgreementEnabled = true;
    private bool alreadySigned = false;

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
    #region Response
    DataTable _dtResponse;
    public DataTable dtResponse
    {
        get
        {
            if (_dtResponse == null)
            {
                DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "QUESTION");
                _dtResponse = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            }

            return _dtResponse;
        }
    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
           "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        }
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //RadSkinManager.GetCurrent(Page).Skin = "Silk";
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Agreements";
    }
    public override void LoadControlData()
    {
        DataSet dsAgreementsInitials = svc.SelectAgreementInitials(this.WorkflowPage.RegistrationId);

        DataRow pa07Row = null;
        String Initials = string.Empty;
        if (Helper.HasRows(dsAgreementsInitials))
        {
            pa07Row = dsAgreementsInitials.Tables[0].AsEnumerable()
                                     .Where(r => r.Field<String>("QUESTION_TYPE_ID") == "PA07").FirstOrDefault();
            if (pa07Row != null && !string.IsNullOrEmpty(pa07Row["INITIALS"].ToString()))
            {
                alreadySigned = true;
            }
            if (!string.IsNullOrEmpty(pa07Row["INITIALS"].ToString()))
            {
                txtAttester.Text = pa07Row["INITIALS"].ToString();
                Initials = pa07Row["INITIALS"].ToString();
            }
            if (pa07Row != null && pa07Row["RESPONSE"].ToString() == "True" && !string.IsNullOrEmpty(pa07Row["USERNAME"].ToString()))
            {
                txtUserId.Text = pa07Row["USERNAME"].ToString();
            }
            else
                txtUserId.Text = HttpContext.Current.User.Identity.Name;
        }
        else
            txtUserId.Text = HttpContext.Current.User.Identity.Name;
        hidHasSigned.Text = string.Empty;
        divSign.Visible = true;
        if (!Page.IsPostBack)
        {
            txtAttester.Text = string.Empty;
        }
        EnableDisableCaptcha();
        if (SessionVarRetriever.IsOhID && alreadySigned)
        {
            Captcha1.Enabled = false;
            txtCaptcha.Enabled = false;
            //OHPNM-14169 & OHPNM-13994 : Unable to Sign Agreements Page to Attest 
            //txtAttester.Enabled = (WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg);
            txtAttester.Text = pa07Row["INITIALS"].ToString();
            btnSaveSignature.Visible = false;
            MakeSignatureReadOnly();
        }


        if (pa07Row != null && pa07Row["REG_SIGNATURE"] != DBNull.Value)
        {
            string base64Signature = pa07Row["REG_SIGNATURE"].ToString();

            if (!string.IsNullOrEmpty(base64Signature))
            {
                // Add "data:" prefix if missing
                if (!base64Signature.StartsWith("data:"))
                {
                    base64Signature = "data:" + base64Signature;
                }

                RadSignature1.Value = base64Signature;
                MakeSignatureReadOnly();

            }
        }


        DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
        if (provRow != null)
        {
            txtProviderName.Text = provRow["NAME"].ToString();
        }

        txtDateAgreeTOC.Text = !string.IsNullOrEmpty(provRow["SUBMIT_DATE_TIME"].ToString()) ? DateTime.Parse(provRow["SUBMIT_DATE_TIME"].ToString()).ToString("MM/dd/yyyy") : DateTime.Today.Date.ToShortDateString();

        bool userIsProvider = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
        var provType = Registration.GetMMIStProviderType(this.WorkflowPage.RegistrationId);

        _isLTCVisible = provType == CON.AgreementsProviderType.NursingFacility || provType == CON.AgreementsProviderType.IntermediateCareFacility;
        divLTCAgreement.Visible = _isLTCVisible;

        // OHPNM-16669 SAM530 Remove provision check language in agreement​ during revalidation
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !this.WorkflowPage.IsReapplication && !this.WorkflowPage.IsReactivation)
            divProvisionCheck.Visible = false;
        else
            divProvisionCheck.Visible = true;

        bool agreementsIsValidated = Registration.AgreementsIsValidated(this.WorkflowPage.RegistrationId);

        hidHasSigned.Text = (agreementsIsValidated && userIsProvider).ToString();



        LoadAgreementQuestions();
        LoadCheckBoxAnswers();

        LoadIndividualQuestion();
        LoadYesNoIndividualProviders();

        if (this.WorkflowPage.EntityTypeID != MAXIMUS.Core.Libraries.Constants.ProviderCategoryTypeID.Individual)
        {
            LoadAttester(Initials);
        }
        else
        {
            ddlAttester.Visible = false;
            txtAttester.Visible = true;

        }
        if (gvUpdateWFAgreements.Rows.Count == 0 && gvSubmittedAgreements.Rows.Count == 0)
        {
            this.LoadUploadedAgreements();
        }

        SetEditability();
        //if (Registration.IsILProvider(this.WorkflowPage.ProviderTypeID))
        //if (this.WorkflowPage.IsCredentialingProvider)
        // OHPNM-2363 - added last check to make sure this panel only shows for an individual
        DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        DataRow dr = null;
        dr = Helper.HasRows(ds) ? ds.Tables[0].Rows[0] : null;
        if (Helper.HasRows(ds))
        {
            if (ds.Tables[0].Rows[0]["IsCredentialingProvider"].ToString() == "1" && (this.WorkflowPage.ApplicationTypeID == CON.ApplicationType.Standard) && (this.WorkflowPage.EntityTypeID == CON.ProviderCategoryTypeID.Individual))
            {
                divCredentialQuestions.Visible = true;
                LoadYesNoAnswers();
            }
        }

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
        if (this.WorkflowPage.EntityTypeID == CON.ProviderCategoryTypeID.Individual
            && (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg
            || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
            && this.WorkflowPage.ApplicationTypeID != CON.ApplicationType.CPC)
        {
            divIndividualQuestions.Visible = true;
        }

    }

    private void MakeSignatureReadOnly()
    {
        // Apply style to wrapper div to block all interactions
        signatureWrapper.Attributes["style"] = "position:relative; pointer-events:none;";
    }




    private void LoadAttester(string Initials)
    {
        ddlAttester.Visible = true;
        helpBusinessNameownerinfo.Visible = false;
        helpBusinessNameInfoownerinfo.Visible = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_OWNER", parms);

        if (Helper.HasRows(ds))
        {
            Dictionary<string, string> item = new Dictionary<string, string>();
            ds.Tables[0].DefaultView.Sort = "PDMS_NAME";
            item.Add("0", "");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["PDMS_OWNER_TYPE_ID"]) == 15 || Convert.ToInt32(row["PDMS_OWNER_TYPE_ID"]) == 18)
                {
                    string name = string.Concat(Convert.ToString(row["FIRST_NAME"]), " ", Convert.ToString(row["MIDDLE_INITIAL"]), " ", Convert.ToString(row["LAST_NAME"]));
                    if (!item.ContainsKey(name))
                        item.Add(name, name);
                }

            }
            //OHPNM-13994 : PROD - Application doesn't allow the attestation signature to be corrected.
            //if (item.Count == 1 || alreadySigned)
            if (item.Count == 1)
            {
                txtAttester.Visible = true;
                ddlAttester.Visible = false;
            }
            else
            {
                txtAttester.Visible = false;
                ddlAttester.Visible = true;
                item.Add("Other", "Other");
                this.ddlAttester.DataSource = item;
                this.ddlAttester.DataTextField = "Value";
                this.ddlAttester.DataValueField = "Key";
                this.ddlAttester.DataBind();
                //OHPNM-14935-Changing Name of Person Attesting in Agreements page properly
                //this.ddlAttester.SelectedIndex = 0;
                if (!string.IsNullOrEmpty(Initials) && this.ddlAttester.Items.FindByValue(Initials) == null)
                {
                    this.ddlAttester.SelectedIndex = this.ddlAttester.Items.Count - 1;
                    txtAttester.Visible = true;
                    txtAttester.Text = Initials;
                }
                else
                    this.ddlAttester.SelectedIndex = !string.IsNullOrEmpty(Initials) && this.ddlAttester.Items.FindByValue(Initials) != null ?
                        this.ddlAttester.Items.IndexOf(this.ddlAttester.Items.FindByValue(Initials)) : 0;
                ddlAttester.Items.RemoveAt(0);

            }


        }
        else
        {
            txtAttester.Visible = true;
            ddlAttester.Visible = false;
        }
    }

    private void SetEditability()
    {
        chkPA01.Enabled = chkPA02.Enabled = chkPA03.Enabled = chkPA04.Enabled =
            chkPA05.Enabled = chkPA05.Enabled = chkPA07.Enabled = _isAgreementEnabled;



        foreach (RepeaterItem item in this.rptAgreementQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList checkBox = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                checkBox.Enabled = _isAgreementEnabled;
            }
        }

        foreach (RepeaterItem item in this.rptIndividualQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList checkBox = (RadioButtonList)item.FindControl("rblConfirmIndividualQuestion");
                checkBox.Enabled = _isAgreementEnabled;
            }
        }
    }

    #region Load
    private bool LoadCheckBoxAnswers()
    {
        bool loaded = false;

        if (dtResponse != null)
        {
            foreach (DataRow row in dtResponse.Rows)
            {
                switch (Helper.GetString("QUESTION_TYPE_ID", row))
                {
                    case "PA01": chkPA01.Text = Helper.GetString("QUESTION_TEXT", row); chkPA01.Checked = Convert.ToBoolean(Helper.GetString("RESPONSE", row)); loaded = true; break;
                    case "PA02": chkPA02.Text = Helper.GetString("QUESTION_TEXT", row); chkPA02.Checked = Convert.ToBoolean(Helper.GetString("RESPONSE", row)); loaded = true; break;
                    case "PA03": chkPA03.Text = Helper.GetString("QUESTION_TEXT", row); chkPA03.Checked = Convert.ToBoolean(Helper.GetString("RESPONSE", row)); loaded = true; break;
                    case "PA04": lblOptionA.Text = Helper.GetString("QUESTION_TEXT", row); chkPA04.Checked = Convert.ToBoolean(Helper.GetString("RESPONSE", row)); loaded = true; break;
                    case "PA05": lblOptionB.Text = Helper.GetString("QUESTION_TEXT", row); chkPA05.Checked = Convert.ToBoolean(Helper.GetString("RESPONSE", row)); loaded = true; break;
                    case "PA07": lblAttest.Text = Helper.GetString("QUESTION_TEXT", row); chkPA07.Checked = Convert.ToBoolean(Helper.GetString("RESPONSE", row)); loaded = true; break;
                    default:
                        break;
                }
            }
            if ((this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg
                || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CPCReattest) && !isAgreementsCompleted())
            {
                chkPA01.Checked = chkPA02.Checked = chkPA03.Checked = chkPA04.Checked = chkPA05.Checked = chkPA07.Checked = false;
            }
        }

        return loaded;
    }
    private void LoadIndividualQuestion()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet dsIndividualQuestions = psc.SelectQuestionTypesByIDBeginsWith("IP");
        DataTable dtIndividualQuestionsYesNo = dsIndividualQuestions.Tables[0].Clone();
        dsIndividualQuestions.Tables[0].AsEnumerable().Where(row => Convert.ToInt32(row.ItemArray[3].ToString()) == 1).ToList().ForEach(row => dtIndividualQuestionsYesNo.ImportRow(row));

        this.rptIndividualQuestions.DataSource = dtIndividualQuestionsYesNo;
        this.rptIndividualQuestions.DataBind();
    }

    private void LoadAgreementQuestions()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet dsPA = psc.SelectQuestionTypesByIDBeginsWith("PA");
        DataTable dtYesNo = dsPA.Tables[0].Clone();
        dsPA.Tables[0].AsEnumerable().Where(row => Convert.ToInt32(row.ItemArray[3].ToString()) == 1).ToList().ForEach(row => dtYesNo.ImportRow(row));
        this.rptAgreementQuestions.DataSource = dtYesNo;
        this.rptAgreementQuestions.DataBind();
        foreach (DataRow row in dsPA.Tables[0].Rows)
        {
            switch (Helper.GetString("QUESTION_TYPE_ID", row))
            {
                case "PA01": chkPA01.Text = Helper.GetString("QUESTION_TEXT", row); break;
                case "PA02": chkPA02.Text = Helper.GetString("QUESTION_TEXT", row); break;
                case "PA03": chkPA03.Text = Helper.GetString("QUESTION_TEXT", row); break;
                case "PA04": lblOptionA.Text = Helper.GetString("QUESTION_TEXT", row); break;
                case "PA05": lblOptionB.Text = Helper.GetString("QUESTION_TEXT", row); break;
                case "PA07": lblAttest.Text = Helper.GetString("QUESTION_TEXT", row); break;
                default:
                    break;
            }
        }
    }

    private void LoadYesNoAnswers()
    {
        int regProviderStatusTypeID = 0;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SECTION_STATUS");
        if (Helper.HasRows(ds))
        {
            DataTable dt = ds.Tables[0];
            if (dt.Select("SectionName = 'Agreements'").Length > 0)
            {
                DataRow row = (dt.Select("SectionName = 'Agreements'")).FirstOrDefault();
                if (row != null)
                {
                    try
                    {
                        regProviderStatusTypeID = Helper.GetInt("REG_PROVIDER_STATUS_TYPE_ID", row);
                    }
                    catch
                    {

                    }

                }

            }
        }
        if (_dtResponse != null && Helper.HasRows(_dtResponse))
        {
            foreach (RepeaterItem item in this.rptAgreementQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList checkBox = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                    Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");
                    TextBox txtComment = (TextBox)item.FindControl("txtResponseComment");

                    if (lblTypeId != null)
                    {
                        if (_dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).Any())
                        {
                            DataTable dtTemp = _dtResponse
                                .Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).CopyToDataTable();
                            bool yesNo = Helper.GetBool("RESPONSE", dtTemp.Rows[0]);

                            //OHPNM-14257
                            checkBox.SelectedIndex = yesNo ? 1 : 0;
                            if (!string.IsNullOrEmpty(Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0])) && yesNo)
                            {
                                txtComment.Text = Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0]);
                            }
                        }
                    }
                }
            }
        }
    }
    private void LoadYesNoIndividualProviders()
    {
        if (_dtResponse != null && Helper.HasRows(_dtResponse))
        {
            foreach (RepeaterItem item in this.rptIndividualQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList checkBox = (RadioButtonList)item.FindControl("rblConfirmIndividualQuestion");
                    Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");
                    TextBox txtComment = (TextBox)item.FindControl("txtIndividualResponseComment");

                    if (lblTypeId != null)
                    {
                        if (_dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).Any())
                        {
                            DataTable dtTemp = _dtResponse
                                .Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).CopyToDataTable();
                            bool yesNo = Helper.GetBool("RESPONSE", dtTemp.Rows[0]);
                            checkBox.SelectedIndex = yesNo ? 1 : 0;
                            if (!string.IsNullOrEmpty(Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0])) && yesNo)
                            {
                                txtComment.Text = Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0]);
                            }
                        }
                    }
                }
            }
        }
    }


    protected void rptAgreementQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblQuestionTypeID = (Label)e.Item.FindControl("lblQuestionTypeID");
            Label lblQstComment = (Label)e.Item.FindControl("lblQstComment");

            if (lblQuestionTypeID != null && lblQstComment != null)
            {
                lblQstComment.Text = "If 'Yes' a comment is required.";
            }
        }
    }

    protected void rptIndividualQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblQuestionTypeID = (Label)e.Item.FindControl("lblQuestionTypeID");
            Label lblQstComment = (Label)e.Item.FindControl("lblQstComment");

            if (lblQuestionTypeID != null && lblQstComment != null)
            {
                lblQstComment.Text = "If 'Yes' a comment is required.";
            }
        }
    }

    private void EnableDisableCaptcha()
    {
        Captcha1.Visible = Convert.ToBoolean(AppSettings.Get("MSCaptcha"));
        txtCaptcha.Visible = Convert.ToBoolean(AppSettings.Get("MSCaptcha"));
        Label2.Visible = Convert.ToBoolean(AppSettings.Get("MSCaptcha"));
    }

    #endregion Load


    protected void Validate_RequiresAll(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = RequiredAgreementsChecked() && IsCaptchaValid();
    }

    private string RequireCommentByTypeId(string typeID)
    {
        switch (typeID)
        {
            case "PA01":
            case "PA02":
            case "PA03":
            case "PA04":
            case "PA05":
            case "PA06": return "0";
            default: return "1";
        }

    }

    protected bool ValidateCommentEntered()
    {
        bool isValid = true;
        if (divCredentialQuestions.Visible)
        {
            foreach (RepeaterItem item in this.rptAgreementQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                    Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");

                    if (rblConfirmQuestion.SelectedIndex != -1 && rblConfirmQuestion.SelectedValue == RequireCommentByTypeId(lblTypeId.Text))
                    {
                        TextBox txtResponseComment = (TextBox)item.FindControl("txtResponseComment");
                        if (string.IsNullOrEmpty(txtResponseComment.Text.Trim()))
                        {
                            AddError("* A comment is required for each Yes answer.");
                            isValid = false;
                            break;
                        }
                    }
                }
            }
        }
        return isValid;
    }


    protected bool ValidateQuestionsAnswered()
    {
        bool isValid = true;
        if (divCredentialQuestions.Visible)
        {
            foreach (RepeaterItem item in this.rptAgreementQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");

                    if (rblConfirmQuestion.SelectedIndex == -1)
                    {
                        AddError("* A Yes or No answer is required for all the questions.");
                        isValid = false;
                        break;
                    }
                }
            }
        }
        return isValid;
    }
    protected bool ValidateIndividualQuestionsAnswered()
    {
        bool isValid = true;
        if (divIndividualQuestions.Visible)
        {
            foreach (RepeaterItem item in this.rptIndividualQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmIndividualQuestion");

                    if (rblConfirmQuestion.SelectedIndex == -1)
                    {
                        AddError("* A Yes or No answer is required for all the questions.");
                        isValid = false;
                        break;
                    }
                }
            }
        }
        return isValid;
    }


    private void LoadUploadedAgreements()
    {
        int pageSize = 5;
        if (Session["grdAgreementsPageSize"] != null)
        {
            pageSize = Convert.ToInt32(Session["grdAgreementsPageSize"]);
        }

        int startRowCnt_sub = gvSubmittedAgreements.PageIndex;
        int startRowCnt_upd = gvUpdateWFAgreements.PageIndex;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //DataSet ds = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.RegistrationPageType.Contracts,
        //    MAXIMUS.Core.Libraries.Constants.ApplicationPDF, string.Empty, null);
        DataSet ds = psc.SelectSubmittedAgreementPDFs(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.RegistrationPageType.Contracts,
            MAXIMUS.Core.Libraries.Constants.ApplicationPDF, pageSize, startRowCnt_sub * pageSize, startRowCnt_upd * pageSize);


        if (ds != null && ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0))
        {
            //previous page index
            var previousPageIndexSA = gvSubmittedAgreements.PageIndex;
            this.gvSubmittedAgreements.DataSource = ds.Tables[0];
            this.gvSubmittedAgreements.VirtualItemCount = Convert.ToInt32(ds.Tables[2].Rows[0]["SUBMITTED_AGREEMENT_TOTAL_COUNT"]);
            this.gvSubmittedAgreements.PageSize = pageSize;

            this.gvSubmittedAgreements.PageIndex = previousPageIndexSA;

            this.gvSubmittedAgreements.DataBind();


            var previousPageIndexUA = gvUpdateWFAgreements.PageIndex;
            this.gvUpdateWFAgreements.DataSource = ds.Tables[1];
            this.gvUpdateWFAgreements.VirtualItemCount = Convert.ToInt32(ds.Tables[2].Rows[0]["UPDATED_AGREEMENT_TOTAL_COUNT"]);
            this.gvUpdateWFAgreements.PageSize = pageSize;

            this.gvUpdateWFAgreements.PageIndex = previousPageIndexUA;

            this.gvUpdateWFAgreements.DataBind();
        }
        else
        {
            gvSubmittedAgreements.DataSource = null;
            gvUpdateWFAgreements.DataSource = null;
        }

        gvSubmittedAgreements.DataBind();
        gvUpdateWFAgreements.DataBind();
    }
    protected void gvSubmittedAgreements_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSubmittedAgreements.PageIndex = e.NewPageIndex;
        LoadUploadedAgreements();
    }
    protected void gvUpdateWFAgreements_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUpdateWFAgreements.PageIndex = e.NewPageIndex;
        LoadUploadedAgreements();
    }

    private bool HasSigned()
    {
        bool hasSigned;
        if (bool.TryParse(hidHasSigned.Text, out hasSigned))
        {
            return hasSigned;
        }
        else
        {
            return false;
        }
    }

    protected void lbtnAgreement_Click(object sender, EventArgs e)
    {
        if (!HasSigned() && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            //chkAgreement.Enabled = true;

        }
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
    }

    public bool SaveAgreementInitials()
    {
        var txtInitials = string.Empty;


        for (int i = 1; i <= 12; i++)
        {
            TextBox txtInitials2 = (TextBox)this.FindControl("PA" + i.ToString());
            svc.SaveRegistrationAgreementInitials(this.WorkflowPage.RegistrationId, "PA" + i.ToString(), (txtInitials2.Text.Trim().Length > 0 ? 1 : 0),
            MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, txtInitials2.Text.Trim());
        }

        return true;
    }

    public override bool SaveData()
    {
        if (Registration.PreviewingRegistrationSection())
        {
            return false;
        }

        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, "PA01", chkPA01.Checked ? 1 : 0,
            CON.RegistrationModifiedStatusType.Changed,
            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, "PA02", chkPA02.Checked ? 1 : 0,
            CON.RegistrationModifiedStatusType.Changed,
            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, "PA03", chkPA03.Checked ? 1 : 0,
            CON.RegistrationModifiedStatusType.Changed,
            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


        if (divLTCAgreement.Visible)
        {
            SaveLTCOptions();
        }

        SaveCredentialQuestions();
        SaveAgreementIntitials();
        if (divIndividualQuestions.Visible == true)
        {
            bool checkIndividualQuestions = SaveIndividualQuestions();
            if (checkIndividualQuestions == false)
            {
                return false;
            }
        }
        UpdateDateSigned(this.WorkflowPage.RegistrationId.ToString(), DateTime.Now);
        return true;
    }
    private void UpdateDateSigned(string RegID, DateTime Datesigned)
    {
        svc.UpdateDateSigned(RegID, Datesigned, Helper.GetUserId(HttpContext.Current.User.Identity.Name));

    }
    private void SaveCredentialQuestions()
    {
        foreach (RepeaterItem item in this.rptAgreementQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");

                if (rblConfirmQuestion.SelectedIndex != -1)
                {
                    TextBox txtResponseComment = (TextBox)item.FindControl("txtResponseComment");
                    svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, lblTypeId.Text, rblConfirmQuestion.SelectedValue == "0" ? 0 : 1,
                        CON.RegistrationModifiedStatusType.Changed,
                        Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), txtResponseComment.Text.Trim());
                }
            }
        }
    }
    private bool SaveIndividualQuestions()
    {
        bool isvalid = false;
        foreach (RepeaterItem item in this.rptIndividualQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmIndividualQuestion");
                Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");

                if (rblConfirmQuestion.SelectedIndex != -1)
                {
                    TextBox txtResponseComment = (TextBox)item.FindControl("txtIndividualResponseComment");
                    if (rblConfirmQuestion.SelectedValue == "0")
                    {
                        txtResponseComment.Text = "";
                        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, lblTypeId.Text, rblConfirmQuestion.SelectedValue == "0" ? 0 : 1,
                            CON.RegistrationModifiedStatusType.Changed,
                            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), txtResponseComment.Text.Trim());
                    }
                    if (rblConfirmQuestion.SelectedValue == "1" && (hdnerror.Value == "True") && (string.IsNullOrEmpty(txtResponseComment.Text)))
                    {
                        lblErrorMsg.Text = "Comments are required";
                        isvalid = false;
                        return isvalid;
                    }
                    else
                    {
                        lblErrorMsg.Text = "";

                        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, lblTypeId.Text, rblConfirmQuestion.SelectedValue == "0" ? 0 : 1,
                            CON.RegistrationModifiedStatusType.Changed,
                            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), txtResponseComment.Text.Trim());
                        isvalid = true;
                    }
                }
            }
        }
        hdnerror.Value = isvalid ? "" : hdnerror.Value;
        return isvalid;
    }

    public override bool ValidateData()
    {
        bool isGood = true;
        isGood = RequiredAgreementsChecked();

        if (!isGood)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Agreement must be acknowledged by checking the corresponding checkboxes.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
        }

        if ((txtAttester.Visible && string.IsNullOrEmpty(txtAttester.Text)) ||
        (ddlAttester.Visible && this.ddlAttester.SelectedValue == "0"))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            isGood &= false;
            val.ErrorMessage = "*Must enter name of person attesting.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
        }

        if (divSign.Visible)
        {
            if (!IsCaptchaValid())
            {
                isGood &= false;
            }
        }


        isGood &= ValidateQuestionsAnswered();
        isGood &= ValidateIndividualQuestionsAnswered();
        isGood &= ValidateCommentEntered();


        return isGood;
    }

    private bool IsCaptchaValid()
    {
        string validateCaptcha = AppSettings.Get("MSCaptcha");
        if (validateCaptcha == "true")
        {
            Captcha1.ValidateCaptcha(txtCaptcha.Text);

            if (Captcha1.UserValidated)
            {
                hidHasSigned.Text = "true";
                return true;
            }
            else
            {
                AddError("* CAPTCHA must be validated.");
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    private bool RequiredAgreementsChecked()
    {
        bool allChecked = true;


        if (divLTCAgreement.Visible && (!chkPA04.Checked && !chkPA05.Checked))
        {
            return false;
        }

        // Individual Provider Questions - JIRA 3635 
        if (divIndividualQuestionsSection.Visible && !ValidateIndividualQuestionsAnswered())
        {
            return false;
        }

        allChecked = chkPA01.Checked && chkPA02.Checked && chkPA07.Checked;

        return allChecked;
    }

    private bool AgreementsChecked()
    {
        bool allChecked = true;


        if (divLTCAgreement.Visible && (!chkPA04.Checked && !chkPA05.Checked))
        {
            return false;
        }

        // Individual Provider Questions - JIRA 3635 
        if (divIndividualQuestionsSection.Visible && !ValidateIndividualQuestionsAnswered())
        {
            return false;
        }

        allChecked = chkPA01.Checked && chkPA02.Checked;

        return allChecked;
    }
    private bool DocIsUploaded()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.Agreements, string.Empty,
            CON.RegistrationPageType.Certification.ToString(), null);
        return Helper.HasRows(ds);
    }

    private void DownloadFile(string fileName, bool isDiddReferral)
    {
        DownloadRequest downloadRequest = new DownloadRequest();
        downloadRequest.FileName = fileName;
        downloadRequest.IsDiddReferral = isDiddReferral;
        FileTransferServiceClient client = new FileTransferServiceClient();

        using (var fileStream = client.DownloadFile(downloadRequest).FileByteStream)
        {
            SendBinaryResponseToClient(fileStream, "attachment;filename=" + fileName, "application/pdf");
        }
    }

    private void SendBinaryResponseToClient(Stream response, string contentHeader, string contentType)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();

        Response.Buffer = true;
        Response.ContentType = contentType;
        Response.AddHeader("Content-Disposition", contentHeader);
        response.CopyTo(Response.OutputStream);

        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        // Technically, I should be doing Response.End(), but due to a bug in ASP.NET 
        // we tried doing complete request http://support.microsoft.com/kb/312629/en-us
        // but complete request is putting all the page up there. So, we are swallowing the
        // thread abort exception here.
        // Response.End();
        try
        {
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }
        catch (Exception ex)
        {
            CoreException.ThrowException(ex);
        }
        finally
        {
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    protected void gvSubmittedAgreements_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string fileName = string.Empty;
            fileName = DataBinder.Eval(e.Row.DataItem, "FILE_NAME").ToString();
            ImageButton img = (ImageButton)e.Row.FindControl("imgView");

            if (img != null)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    img.Visible = false;
                }
                else
                {
                    img.Visible = true;
                }
            }

        }
    }
    protected void gvSubmittedAgreements_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View_File"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string fileName = string.Empty;
            fileName = gvSubmittedAgreements.DataKeys[index].Values["FILE_NAME"].ToString();
            int OnBaseId = 0;
            if (!String.IsNullOrEmpty(gvSubmittedAgreements.DataKeys[index].Values["ONBASE_DOCUMENT_ID"].ToString()))
                OnBaseId = int.Parse(gvSubmittedAgreements.DataKeys[index].Values["ONBASE_DOCUMENT_ID"].ToString());

            switch (e.CommandName)
            {
                case "View_File":
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (OnBaseId > 0)
                        {
                            this.DownloadFileFromOnbase(OnBaseId, fileName);
                        }
                        else
                            this.DownloadFile(fileName, false);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    protected void gvUpdateWFAgreements_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View_File"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string fileName = string.Empty;
            fileName = gvUpdateWFAgreements.DataKeys[index].Values["FILE_NAME"].ToString();
            int OnBaseId = 0;
            if (!String.IsNullOrEmpty(gvUpdateWFAgreements.DataKeys[index].Values["ONBASE_DOCUMENT_ID"].ToString()))
                OnBaseId = int.Parse(gvUpdateWFAgreements.DataKeys[index].Values["ONBASE_DOCUMENT_ID"].ToString());

            switch (e.CommandName)
            {
                case "View_File":
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (OnBaseId > 0)
                        {
                            this.DownloadFileFromOnbase(OnBaseId, fileName);
                        }
                        else
                            this.DownloadFile(fileName, false);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private string SelectAppSetting(string appSettingKey)
    {
        return AppSettings.Get(appSettingKey, string.Empty);
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool isGood = true;
        if (isGood)
        {
            btnSave.Enabled = true;
            mpe.Show();
            return;
        }
    }

    public bool SaveAgreementIntitials()
    {
        string signature = hdnSignature.Value;

        if (this.ddlAttester.Visible == true)
        {
            if (this.ddlAttester.SelectedValue == "Other")
            {
                RegistrationController.SaveRegistrationAgreementInitials(WorkflowPage.RegistrationId, "PA07", chkPA07.Checked ? 1 : 0,
                   CON.RegistrationModifiedStatusType.Changed, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, txtAttester.Text, signature);
            }
            else
            {
                RegistrationController.SaveRegistrationAgreementInitials(WorkflowPage.RegistrationId, "PA07", chkPA07.Checked ? 1 : 0,
               CON.RegistrationModifiedStatusType.Changed, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, ddlAttester.Text.Trim(), signature);
            }
        }
        else if (this.txtAttester.Visible == true)
        {
            RegistrationController.SaveRegistrationAgreementInitials(WorkflowPage.RegistrationId, "PA07", chkPA07.Checked ? 1 : 0,
               CON.RegistrationModifiedStatusType.Changed, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, txtAttester.Text, signature);
        }

        return true;
    }

    private bool isAgreementsCompleted()
    {
        return Registration.AgreementsIsValidated(this.WorkflowPage.RegistrationId);
    }

    public override void LoadData(DataRow row = null)
    {

    }

    protected void RadSignature1_PreRender(object sender, EventArgs e)
    {
        // Example: Set default signature value (Base64 image)
        if (!Page.IsPostBack)
        {
            // You can set a default signature image here if needed
            // Example: Load from file or database
            string defaultSignatureBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA...";
            RadSignature1.Value = defaultSignatureBase64;
        }

        // Example: Set additional properties dynamically
        RadSignature1.ForeColor = System.Drawing.Color.Blue;

    }

    protected void btnSaveSignature_Click(object sender, EventArgs e)
    {

        if (this.ddlAttester.SelectedValue == "Other")
        {
            txtAttester.Visible = true;
            ddlAttester.Visible = false;
        }

        bool isSuccess = SaveAgreementIntitials();
        if (isSuccess)
        {
            lblMessage.ForeColor = Color.Green;
            lblMessage.Text = "Signature captured successfully.";
        }
        LoadControlData();

        // Validate all checkboxes and CAPTCHA have been selected.
        // Save data

    }

    private string ExtractImage()
    {
        string imageData = string.Empty;
        string base64Signature = RadSignature1.Value;

        if (!string.IsNullOrEmpty(base64Signature))
        {
            byte[] imageBytes = Convert.FromBase64String(base64Signature.Split(',')[1]);
            imageData = "data:image/png;base64," + base64Signature;
        }

        return imageData;
    }

    public override string ValidationGroup
    {
        get { return "valAgreements"; }
    }

    public override string Title
    {
        get { return "Agreements"; }
    }

    public override string IdText
    {
        get { return "ucAgreements_" + this.WorkflowPage.RegistrationId; }
    }

    protected void chkPA04_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkPA04.Checked)
        {
            chkPA05.Checked = false;
        }

        SaveLTCOptions();
    }

    protected void chkPA05_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkPA05.Checked)
        {
            chkPA04.Checked = false;
        }

        SaveLTCOptions();
    }
    protected void chkPA07_OnCheckedChanged(object sender, EventArgs e)
    {
        bool isGood = true;
        isGood = AgreementsChecked();
        if (!isGood)
        {

            chkPA07.Checked = false;
        }

        SaveLTCOptions();
    }

    protected void SaveLTCOptions()
    {
        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, "PA04", chkPA04.Checked ? 1 : 0,
            CON.RegistrationModifiedStatusType.Changed,
            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, "PA05", chkPA05.Checked ? 1 : 0,
            CON.RegistrationModifiedStatusType.Changed,
            Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, "PA07", chkPA07.Checked ? 1 : 0,
           CON.RegistrationModifiedStatusType.Changed,
           Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    }

    protected void ddlAttester_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAttester.SelectedValue == "Other")
        {
            txtAttester.Visible = true;
            ddlAttester.Visible = true;
            helpBusinessNameownerinfo.Visible = true;
            helpBusinessNameInfoownerinfo.Visible = true;
            Page.Title = Resources.BrandingResource.OTHER_HELPTEXT;
        }
        else
        {
            helpBusinessNameownerinfo.Visible = false;
            helpBusinessNameInfoownerinfo.Visible = false;
            txtAttester.Visible = false;
        }
    }

    protected void rblConfirmIndividualQuestion_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool isValid = false;
        foreach (RepeaterItem item in this.rptIndividualQuestions.Items)
        {
            RadioButtonList chkResponse = (RadioButtonList)item.FindControl("rblConfirmIndividualQuestion");
            if (chkResponse.SelectedItem != null)
            {
                RequiredFieldValidator rfvIndividualResponseComment = (RequiredFieldValidator)item.FindControl("rfvIndividualResponseComment");
                TextBox txtIndividualComment = (TextBox)item.FindControl("txtIndividualResponseComment");
                Label lblError = (Label)item.FindControl("lblError");
                if (chkResponse.SelectedItem.Value == "0")
                {
                    lblError.Visible = false;
                    isValid = false;
                }
                else
                {
                    lblError.Visible = true;
                    isValid = true;
                    hdnerror.Value = isValid.ToString();
                }
            }
        }
    }
    private void DownloadFileFromOnbase(int onBaseDocID, string fileName)
    {
        try
        {

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFilebyOnBaseDocID(onBaseDocID.ToString());

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();

        }
        catch (ThreadAbortException)
        { }
        catch (SqlException ex)
        {
            // OHPNM-4137
            if (ex.Message.Contains("Subquery returned more than 1 value"))
            {
                lblErrorMsg.Text = "test data in the database where duplicate filenames exist is causing an error; 'Remove' your file and upload it with a different name";
            }
            else
            {
                lblErrorMsg.Text = "a database error has occurred during the download operation";
            }
        }
        catch (Exception ex)
        {
#if DEBUG
                lblErrorMsg.Text =  ex.Message;
#endif

            lblErrorMsg.Text = "an error has occurred during the download operation";
        }

    }
}