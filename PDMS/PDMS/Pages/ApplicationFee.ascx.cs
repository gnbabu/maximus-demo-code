using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Web.UI;

public partial class Pages_ApplicationFee : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public string thisProviderFee
    {
        get
        {
            if (ViewState["AppFee"] == null) ViewState["AppFee"] = string.Empty;
            return ViewState["AppFee"].ToString();
        }
        set { ViewState["AppFee"] = value; }
    }

    public string ProviderFeeTransactionID
    {
        get
        {
            if (ViewState["AppFeeTransID"] == null) ViewState["AppFeeTransID"] = string.Empty;
            return ViewState["AppFeeTransID"].ToString();
        }
        set { ViewState["AppFeeTransID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (rblPaymentType.SelectedValue == CON.ApplicationFeePaymentTypeID.RequestWaiver)
        {
            divComments.Style["display"] = "block";
            divRequestWaiver.Style["display"] = "block";
            divPayByPaperCheck.Style["display"] = "none";
        }

        string DataSite = AppSettings.Get("ApplicationFeeDataSite");
        btnSelectPayment.Attributes.Add("data-site", DataSite);

        LoadScripts();
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
        string scriptURL = AppSettings.Get("CBOSSScriptURL");

        ScriptManager.GetCurrent(Page).Scripts.Add(new ScriptReference(scriptURL));

        if (rblPaymentType.SelectedValue == CON.ApplicationFeePaymentTypeID.RequestWaiver)
        {
            divComments.Style["display"] = "block";
            divRequestWaiver.Style["display"] = "block";
            divPayByPaperCheck.Style["display"] = "none";
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        try
        {
            LoadControlData();

            string appidentifier = AppSettings.Get("ApplicationFeeAppIdentifier");
            string s = "";
            ApplicationFee applicationFee = new ApplicationFee();
            s = applicationFee.CreateSession(appidentifier);
            btnSelectPayment.Attributes.Add("data-session", s);

            base.OnLoad(e);
        }
        catch(Exception ex)
        {

        }
    }
    public override void LoadControlData()
    {
        // TODO: EDV The if condition is not working fi this page is the 1st page in the flow. 
        // We need to chagne somethign in this scenario. WE cannot load controls during pre-render as well because Step is not set.
        //if(this.WorkflowPage.RegistrationStep == CON.SectionTypeID.ApplicationFee)
        LoadControls();
        LoadScripts();
        this.SetTakeActionVisibility(true);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = new DataSet();

        lblFeeAmount.Text = FeeAmount1.Text = thisProviderFee.ToString();
        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPLICATION_FEE");
        DataTable dt = new DataTable();
        pnlVerifyPECOS.Visible = false;             // Initialize to NOT displayed


        if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) && !Helper.IsLoggedInUserInAdminRole())
        {
            Helper.SetReadOnly(this, true);
        }

        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];

            if (string.IsNullOrWhiteSpace(Helper.GetString("IS_CURRENT_APPLICATION_FEE", dr)) || Helper.GetString("IS_CURRENT_APPLICATION_FEE", dr).ToUpper().Equals("TRUE"))
            {
                hdnRegAppFeeID.Value = Helper.GetString("REG_APPLICATION_FEE_ID", dr);

                if (!string.IsNullOrEmpty(Helper.GetString("PAYMENT_TYPE_ID", dr)))
                {
                    rblPaymentType.SelectedValue = Helper.GetString("PAYMENT_TYPE_ID", dr);
                }
                string paymentStatus = Helper.GetString("APPLICATION_FEE_STATUS_ID", dr);

                this.ProviderFeeTransactionID = Helper.GetString("TRANSACTION_ID", dr);

                if (paymentStatus == CON.ApplicationFeePaymentStatus.Paid || paymentStatus == CON.ApplicationFeePaymentStatus.Waived)
                {
                    btnSelectPayment.Disabled = true;
                    btnCompletePayment.Enabled = false;
                }
                if (Helper.GetString("PAYMENT_TYPE_ID", dr) == CON.ApplicationFeePaymentTypeID.CreditCard)
                {
                    this.SetTakeActionVisibility(paymentStatus == CON.ApplicationFeePaymentStatus.Paid);
                    //btnCompletePayment.Enabled = false;
                    //btnSelectPayment.Enabled = true;
                }

                else if (Helper.GetString("PAYMENT_TYPE_ID", dr) == CON.ApplicationFeePaymentTypeID.RequestWaiver)
                {
                    divComments.Style["display"] = "block";
                    divRequestWaiver.Style["display"] = "block";
                    ddlWaiverReason.SelectedValue = Helper.GetString("APPLICATION_FEE_WAIVER_REASON_ID", dr);
                    //OHPNM-15027 - Remove the functionality not valid for OH
                    //if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) && ddlWaiverReason.SelectedValue == CON.WaiverReason.MedicareEnrolled)
                    //{
                    //    // Must be PS user
                    //    pnlVerifyPECOS.Visible = true;
                    //    chkVerifiedPECOS.Enabled = false;
                    //    chkVerifiedPECOS.Checked = Helper.GetBool("PECOS_VERIFIED", dr);
                    //    lnkVerifyPECOS.Attributes.Add("onclick", "javascript:ChkEnabled(chkVerifiedPECOS);");
                    //    lnkVerifyPECOS.OnClientClick = string.Format("openLink('{0}');", "https://portal.cms.gov/wps/portal/unauthportal/home/");
                    //    this.SetTakeActionVisibility(false);
                    //}
                    txtComments.Text = Helper.GetString("COMMENT", dr);
                }
            }

            dt.Columns.Add("FEE_AMOUNT", typeof(string));
            dt.Columns.Add("FEE_STATUS_NAME", typeof(string));
            dt.Columns.Add("FEE_STATUS_DATE", typeof(string));
            dt.Columns.Add("APPLICATION_FEE_WAIVER_REASON_ID", typeof(string));
            dt.Columns.Add("TRANSACTION_ID", typeof(string));

            int rowCount = 0;
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                dt.Rows.Add();
                dt.Rows[rowCount]["FEE_AMOUNT"] = "$" + Helper.GetString("FEE_AMOUNT", drow);

                dt.Rows[rowCount]["FEE_STATUS_DATE"] = Helper.GetString("FEE_STATUS_DATE_TIME", drow);
                dt.Rows[rowCount]["TRANSACTION_ID"] = Helper.GetString("TRANSACTION_ID", drow);


                string Status_ID = Helper.GetString("APPLICATION_FEE_STATUS_ID", drow);

                if (Status_ID == CON.ApplicationFeePaymentStatus.Pending)
                {
                    dt.Rows[rowCount]["FEE_STATUS_NAME"] = CON.ApplicationFeePaymentStatusName.Pending;
                }
                else if (Status_ID == CON.ApplicationFeePaymentStatus.Paid)
                {
                    dt.Rows[rowCount]["FEE_STATUS_NAME"] = CON.ApplicationFeePaymentStatusName.Paid;
                }
                else if (Status_ID == CON.ApplicationFeePaymentStatus.Waived)
                {
                    dt.Rows[rowCount]["FEE_STATUS_NAME"] = CON.ApplicationFeePaymentStatusName.Waived;
                }
                lblFeeStatus.Text = dt.Rows[rowCount]["FEE_STATUS_NAME"].ToString();
                string waiverid = Helper.GetString("APPLICATION_FEE_WAIVER_REASON_ID", drow);
                if (waiverid == CON.WaiverReason.PaidinThePast5Years)
                {
                    dt.Rows[rowCount]["APPLICATION_FEE_WAIVER_REASON_ID"] = CON.WaiverReasonName.PaidinThePast5Years;
                }
                else if (waiverid == CON.WaiverReason.MedicareEnrolled)
                {
                    dt.Rows[rowCount]["APPLICATION_FEE_WAIVER_REASON_ID"] = CON.WaiverReasonName.MedicareEnrolled;
                }
                else if (waiverid == CON.WaiverReason.PaidinAnotherState)
                {
                    dt.Rows[rowCount]["APPLICATION_FEE_WAIVER_REASON_ID"] = CON.WaiverReasonName.PaidinAnotherState;
                }
                else if (waiverid == CON.WaiverReason.MedicareEnrollmentPending)
                {
                    dt.Rows[rowCount]["APPLICATION_FEE_WAIVER_REASON_ID"] = CON.WaiverReasonName.MedicareEnrollmentPending;
                }
                rowCount++;
            }

            grdFeePaymentHistory.DataSource = dt;

        }
        else
        {
            btnCompletePayment.Enabled = false;
            btnSelectPayment.Disabled = true;
            ddlWaiverReason.Enabled = false;
            grdFeePaymentHistory.DataSource = ds;
        }
        grdFeePaymentHistory.DataBind();

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }
    private void LoadControls()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        // TODO: EDV put this in cache
        DataSet ds = psc.SelectApplicationFeePaymentType();
        //btnCheckPayment.Enabled = false;

        Helper.LoadList(this.rblPaymentType, ds.Tables[0], "PAYMENT_TYPE_NAME", "PAYMENT_TYPE_ID", true);
        rblPaymentType.DataBind();
        ds = new DataSet();
        // TODO: EDV put this in cache
        ds = psc.SelectApplicationFeeWaiverReasons();

        Helper.LoadList(this.ddlWaiverReason, ds.Tables[0], "APPLICATION_FEE_WAIVER_REASON_NAME", "APPLICATION_FEE_WAIVER_REASON_ID", true);

        // TODO: EDV DO NOT CALL stored procedures from teh website directly. Change this.
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, this.WorkflowPage.ProviderTypeID, false));
        parameters.Add(SqlParms.CreateParameter("ENTITY_TYPE_ID", DbType.Int32, this.WorkflowPage.EntityTypeID, false));
        parameters.Add(SqlParms.CreateParameter("APPLICATION_TYPE_ID", DbType.Int32, this.WorkflowPage.ApplicationTypeID, false));
        DataSet dsAppFees = DataAccess.ExecuteStoredProcedure("usp_SelectProvider_Type_Fee", parameters, "RegData");
        String AppFee = string.Empty;
        if (Helper.HasRows(dsAppFees))
        {
            DataRow drAppFees = dsAppFees.Tables[0].Rows[0];
            AppFee = "$" + Helper.GetString("FEE_AMOUNT", drAppFees);
        }

        thisProviderFee = AppFee;
        txtAmount.Text = AppFee;

        txtAmount.Enabled = false;
    }
    private void LoadScripts()
    {
        int cnt = 0;
        foreach (ListItem li in rblPaymentType.Items)
        {
            switch (li.Text)
            {
                case CON.ApplicationFeePaymentType.PayByeCheck:
                    rblPaymentType.Items[cnt].Attributes.Add("onclick", "javascript:TogglePaymentType('divPayByECheck','" + divPayByECheck.ClientID + "','" + divPayByPaperCheck.ClientID + "','" + divRequestWaiver.ClientID + "','" + divComments.ClientID + "','" + tblPaperCheck.ClientID + "')");
                    break;
                case CON.ApplicationFeePaymentType.PayByPaperCheck:
                    rblPaymentType.Items[cnt].Attributes.Add("onclick", "javascript:TogglePaymentType('divPayByPaperCheck','" + divPayByECheck.ClientID + "','" + divPayByPaperCheck.ClientID + "','" + divRequestWaiver.ClientID + "','" + divComments.ClientID + "','" + tblPaperCheck.ClientID + "')");
                    break;
                case CON.ApplicationFeePaymentType.RequestWaiver:
                    rblPaymentType.Items[cnt].Attributes.Add("onclick", "javascript:TogglePaymentType('divRequestWaiver','" + divPayByECheck.ClientID + "','" + divPayByPaperCheck.ClientID + "','" + divRequestWaiver.ClientID + "','" + divComments.ClientID + "','" + tblPaperCheck.ClientID + "')");
                    break;
            }
            cnt++;
        }
    }
    public override bool ValidateData()
    {
        bool isGood = true;

        if (rblPaymentType.SelectedIndex == -1)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Payment type must be selected";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            return false;
        }

        if ((string.IsNullOrEmpty(this.ProviderFeeTransactionID)) && (rblPaymentType.SelectedValue == CON.ApplicationFeePaymentTypeID.CreditCard))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Payment must be made to proceed.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            return false;
        }

        if (rblPaymentType.SelectedValue == CON.ApplicationFeePaymentTypeID.RequestWaiver)
        {
            if (this.ddlWaiverReason.SelectedIndex < 1)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "*Select a waiver reason.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);
                return false;
            }


            if (rblPaymentType.SelectedValue == CON.ApplicationFeePaymentTypeID.RequestWaiver && (ddlWaiverReason.SelectedValue == CON.WaiverReason.PaidinAnotherState ||
                ddlWaiverReason.SelectedValue == CON.WaiverReason.PaidinThePast5Years || ddlWaiverReason.SelectedValue == CON.WaiverReason.MedicareEnrollmentPending))
            {
                if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
                {
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    DataSet ds = new DataSet();
                    ds = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep,
                    string.Empty, string.Empty, null);
                    DataSet dts = new DataSet();
                    if (this.WorkflowPage.RegistrationStep == 17)
                    {

                        DataTable appFeeTable = new DataTable();
                        var appTable = ds.Tables[0].AsEnumerable().Where(r => r.Field<string>("REG_PAGE_SECTION") != CON.sectionName);
                        if (appTable.Any())
                        {
                            appFeeTable = appTable.AsEnumerable().CopyToDataTable();
                            dts.Tables.Add(appFeeTable);
                        }
                        else
                            dts = null;
                    }
                    if (!Helper.HasRows(dts))
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "* Proof of payment must be uploaded with Enrollment Submission.";
                        val.ValidationGroup = "valProviderInfoHeader";
                        this.Page.Validators.Add(val);
                        return false;
                    }
                }
            }

            if (ddlWaiverReason.SelectedValue == CON.WaiverReason.MedicareEnrolled)
            {
                if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
                {
                    using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
                    {
                        DataSet dsPecos = psc.SelectPECOSRevalidationData(this.WorkflowPage.RegistrationId);

                        if (!Helper.HasRows(dsPecos))
                        {
                            CustomValidator val = new CustomValidator();
                            val.IsValid = false;
                            val.ErrorMessage = "* Unable to verify active status with Medicare. Please select another option.";
                            val.ValidationGroup = "valProviderInfoHeader";
                            this.Page.Validators.Add(val);
                            return false;
                        }

                    }
                }
            }
        }

        return isGood;
    }
    protected void btnCheckPayment_Click(object sender, EventArgs e)
    {
        string b2pURL = AppSettings.Get("B2P-SendPaymentInfoURL");

        string regID = this.WorkflowPage.RegistrationId.ToString();
        CartItem cartItem = new CartItem() { ProductName = "Application Fee", AccountNumber1 = regID, Amount = thisProviderFee.ToString() };
        RetrieveBill2PayPaymentVerification request = new RetrieveBill2PayPaymentVerification()
        {
            SecurityToken = AppSettings.Get("B2P-SSOXMLToken"),
            VendorReferenceCode = regID
        };

        // serialize the object to xml 
        string obj = Methods.SerializeObjectToXml(request);
        byte[] postArray = System.Text.Encoding.ASCII.GetBytes(obj);
        WebClient webClient = new WebClient();
        byte[] responseBytes = webClient.UploadData(b2pURL, postArray);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Encoding.UTF8.GetString(responseBytes));

        int statusCode = Convert.ToInt32(xmlDoc.GetElementsByTagName("StatusCode")[0].InnerText);

        if (statusCode == 0)
        {

        }

    }
    public override bool SaveData()
    {
        bool result = true;
        if (rblPaymentType.SelectedIndex != -1)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();


            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());


            switch (rblPaymentType.SelectedValue)
            {

                case CON.ApplicationFeePaymentTypeID.CreditCard:
                    parms.Add("FEE_AMOUNT", thisProviderFee.ToString().Trim('$'));
                    if (!string.IsNullOrEmpty(ProviderFeeTransactionID))
                    {
                        parms.Add("TRANSACTION_ID", this.ProviderFeeTransactionID.ToString());
                        parms.Add("APPLICATION_FEE_STATUS_ID", CON.ApplicationFeePaymentStatus.Paid);
                    }
                    break;
                case CON.ApplicationFeePaymentTypeID.RequestWaiver:
                    parms.Add("APPLICATION_FEE_WAIVER_REASON_ID", ddlWaiverReason.SelectedValue);
                    parms.Add("APPLICATION_FEE_STATUS_ID", CON.ApplicationFeePaymentStatus.Waived);
                    if (ddlWaiverReason.SelectedValue == CON.WaiverReason.MedicareEnrolled)
                    {
                        Dictionary<bool, string> dtPecos = GetPECOSfeeVerifiedData();
                        if (dtPecos.ContainsKey(true))
                        {
                            parms.Add("PECOS_VERIFIED", "1");
                            string amountWaived = dtPecos[true].ToString().Trim();
                            if (!string.IsNullOrEmpty(amountWaived))
                                parms.Add("FEE_AMOUNT", amountWaived.Trim('$'));
                        }
                    }
                    break;
            }

            //parms.Add("APPLICATION_FEE_STATUS_ID", feeStatus);
            if (txtComments.Text != "" && txtComments.Text != String.Empty && txtComments.Text != null)
            {
                parms.Add("COMMENT", txtComments.Text);
            }
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("FEE_STATUS_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("PAYMENT_TYPE_ID", rblPaymentType.SelectedValue);

            if (!string.IsNullOrEmpty(hdnRegAppFeeID.Value))
            {
                // Update
                parms.Add("REG_APPLICATION_FEE_ID", hdnRegAppFeeID.Value);
                psc.UpdateRegistrationDataTable("APPLICATION_FEECustom", parms);
            }
            else
            {
                // Insert
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("PROCESS_ID", this.WorkflowPage.WF_ProcessID.ToString());
                parms.Add("PAYMENT_SOURCE", "PNM");
                parms.Add("IS_CURRENT_APPLICATION_FEE", "1");
                psc.InsertRegistrationDataTable("APPLICATION_FEE", parms);
            }
            if (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName)) InvalidateAgreements(this, new EventArgs());
        }
        return result;
    }
    protected void btnCompletePayment_Click(object sender, EventArgs e)
    {
        string b2pURL = AppSettings.Get("B2P-SendPaymentInfoURL");

        string regID = this.WorkflowPage.RegistrationId.ToString();

        /*if (AppSettings.Get("ApplicationFeeTestingEnabled") == "true")
        {
            if(AppSettings.Get("ApplicationFeeTestingResult") == "true")
            this.ProviderFeeTransactionID = AppSettings.Get("ApplicationFeeTestingTransID");
        }*/
        bool result = SaveData();
    }

    public string ValidateDataPS()
    {
        string rtn = string.Empty;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = new DataSet();
        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPLICATION_FEE");
        DataTable dt = new DataTable();

        if (Helper.HasRows(ds))
        {
            DataRow drow = ds.Tables[0].Rows[0];
            string Status_ID = Helper.GetString("APPLICATION_FEE_STATUS_ID", drow);
            string paymentType = Helper.GetString("PAYMENT_TYPE_ID", drow);

            // Application fee must be processed for echeck
            if (paymentType == CON.ApplicationFeePaymentTypeID.CreditCard && Status_ID != CON.ApplicationFeePaymentStatus.Paid)
            {
                rtn = "The application fee payment must be processed";
            }
            if (paymentType == CON.ApplicationFeePaymentTypeID.RequestWaiver)
            {
                if (pnlVerifyPECOS.Visible)
                {
                    if (!chkVerifiedPECOS.Checked) rtn = "* Please confirm the payment information has been verified in PECOS";
                }
            }
        }

        return rtn;
    }

    public void SaveDataPS(int action)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        bool updateNeeded = false;

        if (action == CON.RegistrationProviderServicesStatusTypeId.Approved)
        {
            string paymentStatus = CON.ApplicationFeePaymentStatus.Pending;

            switch (rblPaymentType.SelectedValue)
            {

                case CON.ApplicationFeePaymentTypeID.CreditCard:
                    paymentStatus = CON.ApplicationFeePaymentStatus.Paid;
                    break;
                case CON.ApplicationFeePaymentTypeID.RequestWaiver:
                    paymentStatus = CON.ApplicationFeePaymentStatus.Waived;
                    break;
            }
            parms.Add("APPLICATION_FEE_STATUS_ID", paymentStatus);
            updateNeeded = true;
        }

        if (pnlVerifyPECOS.Visible && !string.IsNullOrEmpty(hdnRegAppFeeID.Value))
        {
            parms.Add("PECOS_VERIFIED", chkVerifiedPECOS.Checked ? "1" : "0");
            updateNeeded = true;
        }

        if (updateNeeded)
        {
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("FEE_STATUS_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("REG_APPLICATION_FEE_ID", hdnRegAppFeeID.Value);

            // Update
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            psc.UpdateRegistrationDataTable("APPLICATION_FEE", parms);
        }
    }

    protected void Verify_PECOS_Click(object sender, EventArgs e)
    {
        chkVerifiedPECOS.Enabled = true;
    }

    protected void chkVerifiedPECOS_CheckedChanged(object sender, EventArgs e)
    {
        this.SetTakeActionVisibility(chkVerifiedPECOS.Checked);
    }

    private void SetTakeActionVisibility(bool isVisible)
    {
        if (_toggleTakeActionVisibility != null)
        {
            _toggleTakeActionVisibility(new ToggleTakeActionVisibilityEventArgs(isVisible));
        }
    }

    protected void rblPaymentType_DataBound(object sender, EventArgs e)
    {

    }

    public override void LoadData(DataRow row = null)
    {

    }

    public override string ValidationGroup
    {
        get { return "valApplicationFee"; }
    }

    public override string Title
    {
        get { return "Application Fee Information"; }
    }

    public override string IdText
    {
        get { return "ucApplicationFee_" + this.WorkflowPage.RegistrationId; }
    }



    protected void btnAuthorizePayment_ServerClick(object sender, EventArgs e)
    {

        ApplicationFee af = new ApplicationFee();
        ApplicationFeeModel afm = new ApplicationFeeModel();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = new DataSet();
        ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        string businessName = string.Empty;
        DataRow drReg = ObjectControllerHelper.HasRows(ds) ? ds.Tables[0].Rows[0] : null;
        if (drReg != null)
        {
            businessName = ObjectControllerHelper.GetString("ProviderName", drReg);
        }
        afm = af.PerformWalletPayment(hdnAccountIdentifier.Value, hdnAccountNumber.Value, hdnAccountType.Value, hdnAccountFirstName.Value, hdnLastName.Value, thisProviderFee.ToString().Trim('$'), businessName, this.WorkflowPage.RegistrationId.ToString());



        this.ProviderFeeTransactionID = afm.transactionNumber;


        bool result = false;
        if (this.ProviderFeeTransactionID != "")
        {
            result = SaveData();
            LoadControlData();
        }
        else
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Transaction failed. Transaction Status " + afm.transactionStatus;
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);

        }
    }

    protected void rblPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPaymentType.SelectedValue == CON.ApplicationFeePaymentTypeID.CreditCard)
        {
            btnSelectPayment.Disabled = false;
        }
        else if (rblPaymentType.SelectedValue == CON.ApplicationFeePaymentTypeID.RequestWaiver)
        {
            ddlWaiverReason.Enabled = true;
        }
    }

    private Dictionary<bool, string> GetPECOSfeeVerifiedData()
    {

        Dictionary<bool, string> dtPecoData = new Dictionary<bool, string>();
        dtPecoData.Add(false, "0.0");
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            DataSet dsPecos = psc.SelectPECOSRevalidationData(this.WorkflowPage.RegistrationId);

            if (Helper.HasRows(dsPecos))
            {
                DataRow drow = dsPecos.Tables[0].Rows[0];
                chkVerifiedPECOS.Checked = true;
                string amountPaid = Helper.GetString("PYMT_AMT", drow);
                if (dtPecoData.ContainsKey(false))
                    dtPecoData.Remove(false);
                if (!dtPecoData.ContainsKey(true))
                    dtPecoData.Add(true, amountPaid);
            }


        }
        return dtPecoData;
    }


}





