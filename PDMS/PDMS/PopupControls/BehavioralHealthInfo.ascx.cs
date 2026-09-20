using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

public partial class PopupControls_BehavioralHealthInfo : BaseSectionControl
{
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


    private bool _ExportHistory
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
    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "Index"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (_svc == null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        DataSet dataSet = _svc.GetCertificationTypes();
        DataTable dt = dataSet.Tables[0];

        ddlCertificationType.DataSource = dt;
        ddlCertificationType.DataTextField = "CERTIFICATION_TYPE_NAME";
        ddlCertificationType.DataValueField = "CERTIFICATION_TYPE_VALUE";
        ddlCertificationType.DataBind();

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = _svc.SelectRegistrationDataWithParams("usp_SelectREG_BHHISTORY", parms);
        if (ds.Tables.Count > 0)
        {
            grdHistory.DataSource = ds.Tables[0];
            grdHistory.DataBind();

            grdExportHistory.DataSource = ds.Tables[0];
            grdExportHistory.DataBind();
            if (_ExportHistory)
            {
                _ExportHistory = false;
                grdExportHistory.MasterTableView.ExportToExcel();
            }
        }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }


    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        if (_ExportHistory == true)
        {
            _ExportHistory = false;
            grdExportHistory.MasterTableView.ExportToExcel();
            mpe.Hide();
        }
        LoadBehaviouralHealthInfo();
    }

    public override void LoadData(System.Data.DataRow dr = null)
    {
        throw new NotImplementedException();
    }

    public override bool SaveData()
    {
        bool saved = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();

        try
        {
            var certificationType = ddlCertificationType.SelectedValue;
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("Certification_Type_Id", certificationType.ToString());
            parms.Add("Certification_Date", Convert.ToDateTime(txtBHCDate.Text.Trim()).ToString());
            parms.Add("CREATED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "BehavioralHealthInfo", parms);
            parms = new Dictionary<string, string>();
            foreach (RepeaterItem item in this.rptBHQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Label qTypeID = item.FindControl("lblQuestionTypeID") as Label;
                    RadioButtonList chkResponse = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                    TextBox txtComment = (TextBox)item.FindControl("txtAptTime");
                    TextBox txtBH12 = (TextBox)item.FindControl("txtBH12");
                    TextBox txtBH13 = (TextBox)item.FindControl("txtBH13");

                    if (qTypeID != null && chkResponse != null && txtComment != null)
                    {
                        //parms.Add("QUESTION_TYPE_ID", qTypeID.Text);
                        //parms.Add("REPONSE", chkResponse.SelectedValue);
                        //parms.Add("RESPONSE_COMMENT", txtComment.Text.Trim());
                        //parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        //parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        if (!string.IsNullOrEmpty(txtComment.Text) && qTypeID.Text == "BH11")
                        {
                            chkResponse.SelectedValue = "1";
                        }
                        else if (qTypeID.Text == "BH11")
                        {
                            chkResponse.SelectedValue = "0";
                        }
                        var comments = "";
                        if (qTypeID.Text == "BH12")
                        {
                            comments = txtBH12.Text.Trim();
                        }
                        else if (qTypeID.Text == "BH13")
                        {
                            comments = txtBH13.Text.Trim();
                        }
                        else
                        {
                            comments = txtComment.Text.Trim();
                        }
                        psc.SaveRegistrationQuestion(this.WorkflowPage.RegistrationId, qTypeID.Text, Convert.ToInt16(chkResponse.SelectedValue),
                        MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                        comments);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            saved = false;
        }

        return saved;
    }

    private void LoadBehaviouralHealthInfo()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectQuestionTypesByIDBeginsWith("BH");  //the question type id in this table i=serves as unique identifier and type of question.
        this.rptBHQuestions.DataSource = ds;
        this.rptBHQuestions.DataBind();

        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "QUESTION");
        DataTable dtQuestion = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;

        //Set Answers
        if (Helper.HasRows(dtQuestion))
        {
            this.SetRegBehavioralHealthInfoAnswers(dtQuestion);
        }
        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BehavioralHealthInfo");
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables["RegistrationData"].Rows[0];
            var behaviouralId = Helper.GetInt("REG_BehavioralHealthInfo_ID", dr);

            if (behaviouralId > 0)
            {
                this.txtBHCDate.Text = Helper.FormatDate2(dr["Certification_Date"].ToString());
                this.ddlCertificationType.SelectedValue = Helper.GetInt("Certification_Type_Id", dr).ToString();
            }
        }
    }

    private void SetRegBehavioralHealthInfoAnswers(DataTable dtQuestion)
    {
        foreach (RepeaterItem item in this.rptBHQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList checkBox = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");
                TextBox txtComment = (TextBox)item.FindControl("txtAptTime");
                TextBox txtBH12 = (TextBox)item.FindControl("txtBH12");
                TextBox txtBH13 = (TextBox)item.FindControl("txtBH13");

                if (lblTypeId != null)
                {
                    if (dtQuestion.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).Length > 0)
                    {
                        DataTable dtTemp = dtQuestion.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).CopyToDataTable();
                        bool yesNo = Helper.GetBool("RESPONSE", dtTemp.Rows[0]);

                        checkBox.SelectedIndex = yesNo ? 1 : 0;
                        if (lblTypeId.Text == "BH12")
                        {
                            txtBH12.Text = Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0]);
                        }
                        else if (lblTypeId.Text == "BH13")
                        {
                            txtBH13.Text = Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0]);
                        }
                        else
                        {
                            txtComment.Text = Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0]);
                        }

                    }
                }
            }
        }
    }

    protected void rptBHQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item)
        {
            Label lblQuestionTypeID = (Label)e.Item.FindControl("lblQuestionTypeID");
            Label lblQstComment = (Label)e.Item.FindControl("lblQstComment");
            RadioButtonList Options = e.Item.FindControl("rblConfirmQuestion") as RadioButtonList;
            TextBox txtAptTime = (TextBox)e.Item.FindControl("txtAptTime");
            TextBox txtBH13 = (TextBox)e.Item.FindControl("txtBH13");
            if (lblQuestionTypeID.Text == "BH11")
            {
                txtAptTime.Visible = true;
                Options.Visible = false;
            }
            else
            {
                Options.Visible = true;
                txtAptTime.Visible = false;
            }
            if (lblQuestionTypeID.Text == "BH13")
            {
                // To show BH13 text box and lable
                Options.Visible = true;
                txtBH13.Visible = true;
                var lbBh13 = (Label)e.Item.FindControl("lbBH13");
                lbBh13.Visible = true;

                // To show BH12 text box and lable
                Options = rptBHQuestions.Items[11].FindControl("rblConfirmQuestion") as RadioButtonList;
                var txtBH12 = (TextBox)rptBHQuestions.Items[11].FindControl("txtBH12");
                var lbBh12 = (Label)rptBHQuestions.Items[11].FindControl("lbBH12");
                Options.Visible = true;
                txtBH12.Visible = true;
                lbBh12.Visible = true;
            }
        }
    }

    public override bool ValidateData()
    {
        bool isGood = true;

        if (pnlBehavioralHealthInfo.Visible)
        {
            foreach (RepeaterItem item in this.rptBHQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                    Label lblQuestionTypeID = (Label)item.FindControl("lblQuestionTypeID");
                    if (rblConfirmQuestion.SelectedIndex == -1 && lblQuestionTypeID.Text != "BH11")
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "* A Yes or No answer is required for all the questions.";
                        val.ValidationGroup = "valProviderInfoHeader";
                        this.Page.Validators.Add(val);
                        isGood = false;
                        break;
                    }
                    else if (lblQuestionTypeID.Text == "BH11")
                    {
                        TextBox txtAptTime = (TextBox)item.FindControl("txtAptTime");
                        if (string.IsNullOrEmpty(txtAptTime.Text))
                        {
                            CustomValidator val = new CustomValidator();
                            val.IsValid = false;
                            val.ErrorMessage = "* Average Waiting time is required.";
                            val.ValidationGroup = "valProviderInfoHeader";
                            this.Page.Validators.Add(val);
                            isGood = false;
                            break;
                        }
                    }
                    else if (lblQuestionTypeID.Text == "BH12")
                    {
                        if (rblConfirmQuestion.SelectedIndex == 1)
                        {
                            TextBox txtBH12 = (TextBox)item.FindControl("txtBH12");
                            if (string.IsNullOrEmpty(txtBH12.Text))
                            {
                                CustomValidator val = new CustomValidator();
                                val.IsValid = false;
                                val.ErrorMessage = "*Bed capacity (# of beds) at the facility.";
                                val.ValidationGroup = "valProviderInfoHeader";
                                this.Page.Validators.Add(val);
                                isGood = false;
                                break;
                            }
                        }

                    }
                    else if (lblQuestionTypeID.Text == "BH13")
                    {
                        if (rblConfirmQuestion.SelectedIndex == 1)
                        {
                            TextBox txtBH13 = (TextBox)item.FindControl("txtBH13");
                            if (string.IsNullOrEmpty(txtBH13.Text))
                            {
                                CustomValidator val = new CustomValidator();
                                val.IsValid = false;
                                val.ErrorMessage = "*Bed capacity (# of beds) at the facility.";
                                val.ValidationGroup = "valProviderInfoHeader";
                                this.Page.Validators.Add(val);
                                isGood = false;
                                break;
                            }
                        }

                    }

                }
            }
        }

        return isGood;
    }

    public override string Title
    {
        get { return "Behavioral Health Information"; }
    }

    public override string IdText
    {
        get { return "ucBehavioralHealthInfo_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valBehavioralHealthInfo"; }
    }


    protected void rblConfirmQuestion_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in this.rptBHQuestions.Items)
        {
            RadioButtonList chkResponse = (RadioButtonList)item.FindControl("rblConfirmQuestion");
            if (chkResponse.SelectedItem != null && chkResponse.ClientID.Contains("ctl13_rblConfirmQuestion"))
            {
                TextBox txtBH13 = (TextBox)item.FindControl("txtBH13");
                if (chkResponse.SelectedItem.Value == "0")
                {
                    txtBH13.Text = string.Empty;
                    txtBH13.ReadOnly = true;
                }
                else
                {
                    txtBH13.ReadOnly = false;
                }
            }
            else if (chkResponse.SelectedItem != null && chkResponse.ClientID.Contains("ctl12_rblConfirmQuestion"))
            {
                TextBox txtBH12 = (TextBox)item.FindControl("txtBH12");
                if (chkResponse.SelectedItem.Value == "0")
                {
                    txtBH12.Text = string.Empty;
                    txtBH12.ReadOnly = true;
                }
                else
                {
                    txtBH12.ReadOnly = false;
                }
            }
        }
    }

    protected void grdHistory_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (_SortField.Equals(e.SortExpression))
        {
            _SortField = _SortField + " DESC";
        }
        else
        {
            _SortField = e.SortExpression;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_BHHISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdHistory.DataSource = ds.Tables[0];
            grdHistory.DataBind();
            mpe.Show();
        }
    }

    protected void grdHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_BHHISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grdHistory.DataSource = ds.Tables[0];
            grdHistory.DataBind();
            mpe.Show();
        }
    }

    protected void grdHistory_Export(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        mpe.Show();
    }
}