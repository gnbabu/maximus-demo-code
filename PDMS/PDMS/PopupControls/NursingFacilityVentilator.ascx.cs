using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_NursingFacilityVentilator : BaseSectionControl
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
    private static int NV_Quest_Count = 0, NW_Quest_Count = 0, cnt_qV = 0, cnt_qW = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Nursing Facility Ventilator";
    }

    public override void LoadControlData()
    {
        divVentilatorQuestions.Visible = false;
        divWeaningQuestions.Visible = false;

        RadioButtonList rbl = (RadioButtonList)this.FindControl("rblnewnfv");
        if (dtResponse != null && Helper.HasRows(dtResponse) && rbl.SelectedIndex == -1)
        {
            if (dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", "NVW0")).Any())
            {
                DataTable dtTemp = dtResponse
                    .Select(string.Format("QUESTION_TYPE_ID = '{0}'", "NVW0")).CopyToDataTable();
                bool yesNo = Helper.GetBool("RESPONSE", dtTemp.Rows[0]);
                rbl.SelectedIndex = yesNo ? 1 : 0;
            }
        }

        if (rbl.SelectedValue.Equals("1"))
        {
            divVentilatorQuestions.Visible = true;
            divWeaningQuestions.Visible = true;
            LoadVentilatorQuestions();
            LoadWeaningQuestions();
            LoadAnswers();
        }
        bool isEditable = false;
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            isEditable = true;
            rbl.Enabled = isEditable;
        }

        else
        {
            isEditable = false;
            rbl.Enabled = isEditable;
        }

        if (divVentilatorQuestions.Visible)
        {
            foreach (RepeaterItem item in this.rptVentilatorQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblV = (RadioButtonList)item.FindControl("rblConfirmQuestionV");
                    rblV.Enabled = isEditable;
                }
            }
        }

        if (divWeaningQuestions.Visible)
        {
            foreach (RepeaterItem item in this.rptWeaningQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblW = (RadioButtonList)item.FindControl("rblConfirmQuestionW");
                    rblW.Enabled = isEditable;
                }
            }
        }
    }

    protected void rblnewnfv_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblnewnfv.SelectedValue.Equals("1"))
        {
            divVentilatorQuestions.Visible = true;
            divWeaningQuestions.Visible = true;
            LoadVentilatorQuestions();
            LoadWeaningQuestions();
            LoadAnswers();
        }
        else
        {
            divVentilatorQuestions.Visible = false;
            divWeaningQuestions.Visible = false;
        }
    }

    private void LoadVentilatorQuestions()
    {
        divVentilatorQuestions.Visible = true;
        DataSet dsNV = svc.SelectQuestionTypesByIDBeginsWith("NV0");
        this.rptVentilatorQuestions.DataSource = dsNV;
        this.rptVentilatorQuestions.DataBind();
        NV_Quest_Count = dsNV.Tables[0].Rows.Count;
    }

    protected void rptVentilatorQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblQuestionTypeIDV = (Label)e.Item.FindControl("lblQuestionTypeID");
        }
    }

    private void LoadWeaningQuestions()
    {
        divWeaningQuestions.Visible = true;
        DataSet dsNW = svc.SelectQuestionTypesByIDBeginsWith("NW");
        this.rptWeaningQuestions.DataSource = dsNW;
        this.rptWeaningQuestions.DataBind();
        NW_Quest_Count = dsNW.Tables[0].Rows.Count;
    }

    protected void rptWeaningQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblQuestionTypeIDW = (Label)e.Item.FindControl("lblQuestionTypeID");
        }
    }

    private void LoadAnswers()
    {
        if (_dtResponse != null && Helper.HasRows(_dtResponse) && _dtResponse.Rows.Count > 1)
        {
            foreach (RepeaterItem item in this.rptVentilatorQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rbl = (RadioButtonList)item.FindControl("rblConfirmQuestionV");
                    Label lblTypeId = (Label)item.FindControl("lblQuestionTypeIDV");

                    if (lblTypeId != null)
                    {
                        if (_dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).Any())
                        {
                            DataTable dtTemp = _dtResponse
                                .Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).CopyToDataTable();
                            bool yesNo = Helper.GetBool("RESPONSE", dtTemp.Rows[0]);
                            rbl.SelectedIndex = yesNo ? 1 : 0;
                        }
                    }
                }
            }
            foreach (RepeaterItem item in this.rptWeaningQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rbl1 = (RadioButtonList)item.FindControl("rblConfirmQuestionW");
                    Label lblTypeId1 = (Label)item.FindControl("lblQuestionTypeIDW");

                    if (lblTypeId1 != null)
                    {
                        if (_dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId1.Text)).Any())
                        {
                            DataTable dtTemp = _dtResponse
                                .Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId1.Text)).CopyToDataTable();
                            bool yesNo1 = Helper.GetBool("RESPONSE", dtTemp.Rows[0]);
                            rbl1.SelectedIndex = yesNo1 ? 1 : 0;
                        }
                    }
                }
            }
        }
    }

    public override bool ValidateData()
    {
        bool isValid = true;
        if (divVentilatorQuestions.Visible)
        {
            foreach (RepeaterItem item in this.rptVentilatorQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblConfirmQuestionV = (RadioButtonList)item.FindControl("rblConfirmQuestionV");

                    if (rblConfirmQuestionV.SelectedIndex == -1)
                    {
                        AddError("* A Yes or No answer is required for all the Ventilator questions.");
                        isValid = false;
                        break;
                    }
                }
            }
        }
        if (divWeaningQuestions.Visible)
        {
            foreach (RepeaterItem item in this.rptWeaningQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblConfirmQuestionW = (RadioButtonList)item.FindControl("rblConfirmQuestionW");

                    if (rblConfirmQuestionW.SelectedIndex == -1)
                    {
                        AddError("* A Yes or No answer is required for all the Weaning questions.");
                        isValid = false;
                        break;
                    }
                }
            }
        }
        return isValid;
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
    }

    public override bool SaveData()
    {
        bool isValid = true;
        bool ShowPopup = false;

        if (_dtResponse != null && Helper.HasRows(_dtResponse) && _dtResponse.Rows.Count > 1)
        {
            foreach (RepeaterItem item in this.rptVentilatorQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rbl = (RadioButtonList)item.FindControl("rblConfirmQuestionV");
                    Label lblTypeId = (Label)item.FindControl("lblQuestionTypeIDV");

                    if (lblTypeId != null)
                    {
                        if (_dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).Any())
                        {
                            DataTable dtTemp = _dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).CopyToDataTable();
                            int yesNo = Helper.GetInt("RESPONSE", dtTemp.Rows[0]);
                            if (rbl.SelectedValue != yesNo.ToString())
                                ShowPopup = true;
                        }
                    }
                }
            }
            foreach (RepeaterItem item in this.rptWeaningQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rbl1 = (RadioButtonList)item.FindControl("rblConfirmQuestionW");
                    Label lblTypeId1 = (Label)item.FindControl("lblQuestionTypeIDW");

                    if (lblTypeId1 != null)
                    {
                        if (_dtResponse.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId1.Text)).Any())
                        {
                            DataTable dtTemp = _dtResponse
                                .Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId1.Text)).CopyToDataTable();
                            int yesNo1 = Helper.GetInt("RESPONSE", dtTemp.Rows[0]);
                            if (rbl1.SelectedValue != yesNo1.ToString()) ShowPopup = true;
                        }
                    }
                }
            }
        }

        RadioButtonList rblnewnfv = (RadioButtonList)this.FindControl("rblnewnfv");
        if (string.IsNullOrEmpty(lblWarningMsg.Text) || ShowPopup)
        {
            cnt_qV = 0; cnt_qW = 0; lblWarningMsg.Text = string.Empty;                      
            if (rblnewnfv.SelectedIndex != -1)
            {
                svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, "NVW0", rblnewnfv.SelectedValue == "0" ? 0 : 1,
                               CON.RegistrationModifiedStatusType.Changed,
                               Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), string.Empty);
            }

            if (divVentilatorQuestions.Visible)
            {
                foreach (RepeaterItem item in this.rptVentilatorQuestions.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        RadioButtonList rblConfirmQuestionV = (RadioButtonList)item.FindControl("rblConfirmQuestionV");
                        Label lblTypeIdV = (Label)item.FindControl("lblQuestionTypeIDV");

                        if (rblConfirmQuestionV.SelectedIndex != -1)
                        {
                            svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, lblTypeIdV.Text, rblConfirmQuestionV.SelectedValue == "0" ? 0 : 1,
                                CON.RegistrationModifiedStatusType.Changed,
                                Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), string.Empty);
                        }

                        if (rblConfirmQuestionV.SelectedValue.Equals("1"))
                        {
                            cnt_qV = cnt_qV + 1;
                        }
                    }
                }

                if (cnt_qV == NV_Quest_Count)
                {
                    bool duplicate = svc.VerifyDuplicateSpecialtyForReg(this.WorkflowPage.RegistrationId, CON.SpecialtyTypeID.NF_VENT, 0, DateTime.Now);
                    if (!duplicate)
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("PRIMARY_FLAG", "0");
                        parms.Add("SPECIALTY_TYPE_ID", CON.SpecialtyTypeID.NF_VENT.ToString());
                        parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                        parms.Add("SPECIALTY_BOARD_STATE", "OH");
                        parms.Add("SPECIALTY_BOARD_NAME", string.Empty);
                        parms.Add("START_DATE", DateTime.Now.ToString());
                        parms.Add("END_DATE", string.Empty);
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                        parms.Add("ENROLL_STATUS_ID", CON.EnrollStatus.INACTIVE.ToString());
                        parms.Add("ENROLLMENT_STATUS_REASONS_ID", CON.EnrollStatusReasonID.InActive.ToString());
                        svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom2", parms);
                    }
                    lblWarningMsg.Text += "Your facility is approved for the Nursing Facility Chronic Ventilator Program effective " + DateTime.Now.ToString() + "<br /><br />";
                }
                if (cnt_qV != NV_Quest_Count)
                {
                    lblWarningMsg.Text += "Your facility does not meet the criteria for participating in the Nursing Facility Chronic Ventilator Program." + "<br /><br />";
                    bool duplicate = svc.VerifyDuplicateSpecialtyForReg(this.WorkflowPage.RegistrationId, CON.SpecialtyTypeID.NF_VENT, 0, DateTime.Now);
                    if (duplicate)
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("SPECIALTY_TYPE_ID", CON.SpecialtyTypeID.NF_VENT.ToString());
                        svc.DeleteRegistrationDataWithParams("SPECIALTYCustom2", parms);
                    }
                }
            }

            if (divWeaningQuestions.Visible)
            {
                foreach (RepeaterItem item in this.rptWeaningQuestions.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        RadioButtonList rblConfirmQuestionW = (RadioButtonList)item.FindControl("rblConfirmQuestionW");
                        Label lblTypeIdW = (Label)item.FindControl("lblQuestionTypeIDW");

                        if (rblConfirmQuestionW.SelectedIndex != -1)
                        {
                            svc.SaveRegistrationQuestion(WorkflowPage.RegistrationId, lblTypeIdW.Text, rblConfirmQuestionW.SelectedValue == "0" ? 0 : 1,
                                CON.RegistrationModifiedStatusType.Changed,
                                Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), string.Empty);
                        }

                        if (rblConfirmQuestionW.SelectedValue.Equals("1"))
                        {
                            cnt_qW = cnt_qW + 1;
                        }
                    }
                }
                if (cnt_qW == NW_Quest_Count)
                {
                    bool duplicate = svc.VerifyDuplicateSpecialtyForReg(this.WorkflowPage.RegistrationId, CON.SpecialtyTypeID.NF_WEAN, 0, DateTime.Now);
                    if (!duplicate)
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("PRIMARY_FLAG", "0");
                        parms.Add("SPECIALTY_TYPE_ID", CON.SpecialtyTypeID.NF_WEAN.ToString());
                        parms.Add("SPECIALTY_BOARD_CERTIFIED", "Y");
                        parms.Add("SPECIALTY_BOARD_STATE", "OH");
                        parms.Add("SPECIALTY_BOARD_NAME", string.Empty);
                        parms.Add("START_DATE", DateTime.Now.ToString());
                        parms.Add("END_DATE", string.Empty);
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                        parms.Add("ENROLL_STATUS_ID", CON.EnrollStatus.INACTIVE.ToString());
                        parms.Add("ENROLLMENT_STATUS_REASONS_ID", CON.EnrollStatusReasonID.InActive.ToString());
                        svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTYCustom2", parms);
                    }
                    lblWarningMsg.Text += "Your facility is approved for the Nursing Facility Weaning Program effective " + DateTime.Now.ToString() + "<br />";
                }
                if (cnt_qW != NW_Quest_Count)
                {
                    lblWarningMsg.Text += "Your facility does not meet the criteria for participating in the Nursing Facility Weaning Program." + "<br />";
                    bool duplicate = svc.VerifyDuplicateSpecialtyForReg(this.WorkflowPage.RegistrationId, CON.SpecialtyTypeID.NF_WEAN, 0, DateTime.Now);
                    if (duplicate)
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms = new Dictionary<string, string>();
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("SPECIALTY_TYPE_ID", CON.SpecialtyTypeID.NF_WEAN.ToString());
                        svc.DeleteRegistrationDataWithParams("SPECIALTYCustom2", parms);
                    }
                }
            }
            if (!string.IsNullOrEmpty(lblWarningMsg.Text))
            {
                mpeWarningMsgNF.Show();
                isValid = false;
            }
            else
            {
                // OHPNM-9565               
                if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && rblnewnfv.SelectedIndex == -1)
                {
                    // this page is required, but atleast one item should be selected on the radion button list to continue
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
        }
        else
        {
            // OHPNM-9565          
            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && rblnewnfv.SelectedIndex == -1)
            {
                // this page is required, but atleast one item should be selected on the radion button list to continue
                isValid = false;
            }
            else
            {
                isValid = true;
            }
        }
        return isValid;
    }
    //protected void btnWarningMsgOK_Click(object sender, EventArgs e)
    //{
    //    mpeWarningMsg.Hide();
    //}

    public override string ValidationGroup
    {
        get { return "valNursingFacilityVentilator"; }
    }

    public override string Title
    {
        get { return "Nursing Facility Ventilator"; }
    }

    public override string IdText
    {
        get { return "ucNursingFacilityVentilator_" + this.WorkflowPage.RegistrationId; }
    }

    public override void LoadData(DataRow row = null)
    {

    }

}