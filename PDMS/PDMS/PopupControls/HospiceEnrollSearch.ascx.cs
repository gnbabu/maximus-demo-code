using Corp.Core.Libraries;
using Corp.Core.Libraries.HospiceReference;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Net;
using System.Web.Security;

public partial class PopupControls_HospiceEnrollSearch : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        cvttxtDateofBirth.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        // OHPNM-4851
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true && Helper.IsUserInSubRoles(this.WorkflowPage.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.HospiceReadOnlySubRoles)
            && !Helper.IsUserInSubRoles(this.WorkflowPage.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.HospiceMaintenanceSubRoles))
        {
            btnAdd.Visible = false;
        }
        if (this.WorkflowPage.HospiceTrackNo != "0" && this.WorkflowPage.HospiceTrackNo != "")
        {
            this.WorkflowPage.RecipientInformation = null;
        }
        if (!pnlHospiceSearch.Visible && pnlHospiceEnrollment.Visible)
        {
            //HospicePlaceholderInitial.Controls.Clear();
            PopupControls_HospiceEnrollment ucHospiceEnrollmentControl =
                            LoadControl("~/PopupControls/HospiceEnrollment.ascx") as PopupControls_HospiceEnrollment;
            ucHospiceEnrollmentControl.ProviderMedId = this.WorkflowPage.MedicaidID;
            ucHospiceEnrollmentControl.HospiceTrackNo = txtHTrackingNumber.Text;
            ucHospiceEnrollmentControl.IscheckChangeofProvider = chlChangeofProvider.Checked;
            ucHospiceEnrollmentControl.CancelHospiceClick += new PopupControls_HospiceEnrollment.EventHandler(UcHospiceEnrollmentControl_CancelHospiceClick);
            HospicePlaceholderInitial.Controls.Add(ucHospiceEnrollmentControl);
            this.WorkflowPage.IsHospiceEnrollmentClick = false;
        }
    }

    public void UcHospiceEnrollmentControl_CancelHospiceClick()
    {
        HospicePlaceholderInitial.Controls.Clear();
        pnlHospiceSearch.Visible = true;
        if (!string.IsNullOrEmpty(txtMBillingNumber.Text) && txtMBillingNumber.Text.Length == 12)
            btnAdd.Enabled = true;
        else
            btnAdd.Enabled = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lblMessage.InnerText = string.Empty;
        if (Page.IsValid)
        {
            // Clear these out prior to doing the Search
            this.lblMessages.Text = string.Empty;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            this.gvHospice.PageIndex = 0;
            lnkExcel.Visible = lnkPDF.Visible = false;
            gvHospice.DataSource = null;
            gvHospice.DataBind();
            BindData(txtHTrackingNumber.Text.Trim(), this.WorkflowPage.MedicaidID, txtMBillingNumber.Text.Trim(), true);
            lnkExcel.Visible = lnkPDF.Visible = gvHospice.Rows.Count > 0;
        }
        this.WorkflowPage.IsHospiceEnrollmentClick = false;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
         lnkExcel.Visible = lnkPDF.Visible = false;
        gvHospice.DataSource = null;
        gvHospice.DataBind();
        txtMBillingNumber.Text = string.Empty;
        txtHTrackingNumber.Text = string.Empty;
        lblMessage.InnerText = string.Empty;
        chlChangeofProvider.Checked = false;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.WorkflowPage.HospiceRequestResponse = null;
        this.WorkflowPage.IsNewHospiceBenefitPeriod = false;
        this.WorkflowPage.HospicAttachments = null;
        this.WorkflowPage.HospicDocuments = null;
        lblMessage.InnerText = string.Empty;
        lnkExcel.Visible = lnkPDF.Visible = false;
        gvHospice.DataSource = null;
        gvHospice.DataBind();
        try
        {
            //Get Recipient Information
            var recipientInfo = GetRecipientInformation(txtMBillingNumber.Text, txtDateofBirth.Text);
            if (recipientInfo == null)
            {
                lblMessage.InnerText = "Recipient is not found";
                txtDateofBirth.Text = string.Empty;
                return;
            }
            else if (recipientInfo != null && recipientInfo.ResponseHeaderDetails != null && recipientInfo.ResponseHeaderDetails.ResponseType.ToUpper().Contains("FAILURE"))
            {
                lblMessage.InnerText = "Exception from Recipient Eligibility Service :" + recipientInfo.ResponseHeaderDetails.ResponseMessage;
                txtDateofBirth.Text = string.Empty;
                return;
            }
            else if (recipientInfo != null && recipientInfo.ErrorDetails != null && recipientInfo.ErrorDetails.Count() > 0)
            {
                var errors = string.Empty;
                foreach (var error in recipientInfo.ErrorDetails)
                {
                    errors += error.Description;
                }
                lblMessage.InnerText = "Exception from Recipient Eligibility Service :" + errors;
                txtDateofBirth.Text = string.Empty;
                return;
            }
            else
            {
                this.WorkflowPage.RecipientInformation = recipientInfo.RecipientInfo;
            }
        }
        catch (Exception ex)
        {
            string exception = GetExceptionMessage(ex);
            lblMessage.InnerText = "Exception in Recipient Eligibility Service :" + exception;
            txtDateofBirth.Text = string.Empty;
            return;
        }
        bool hasEnrollment = false;
        string status = string.Empty;
        string lastSubmitedDate = string.Empty;
        try
        {
            //Search service here
           var isException =  SearchHospice(txtHTrackingNumber.Text.Trim(), "0000000", txtMBillingNumber.Text.Trim(), Convert.ToInt32(ddlPageSize.SelectedValue));
            if (isException) return;
            var response = this.WorkflowPage.SearchHospiceResponse;
            if (response != null && response.Errors != null && response.Errors.Count() > 0)
            {
                var errors = string.Empty;
                foreach (var error in response.Errors)
                {
                    if (error.ErrorCode != "B1001")
                    {
                        errors += error.ErrorDesc;
                    }
                }
                if (!string.IsNullOrEmpty(errors))
                {
                    lblMessage.InnerText = errors;
                    return;
                }
            }
            else if (response != null && response.Response != null && response.Response.Count() > 0)
            {
                if (response.Response.Count() == 1)
                {
                    hdHospiceTrackNo.Value = response.Response[0].HospiceTrackNo.ToString();
                    status = response.Response[0].Status.ToString();
                    lastSubmitedDate = response.Response[0].SubmissionDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    var searchRes = response.Response.OrderByDescending(o => o.SubmissionDate).First();
                    hdHospiceTrackNo.Value = searchRes.HospiceTrackNo.ToString();
                    status = searchRes.Status.ToString();
                    lastSubmitedDate = response.Response[0].SubmissionDate.ToString("MM/dd/yyyy");
                }
                hasEnrollment = true;
            }
        }
        catch (Exception ex)
        {
            string exception = GetExceptionMessage(ex);
            lblMessage.InnerText = "Exception in Search Hospice Service :" + exception;
            return;
        }
        //get latest enrollment request tracking# if search returns result
        //change action to 'Search' else take it as 'Add'
        if (!hasEnrollment)
            hdHospiceTrackNo.Value = txtHTrackingNumber.Text;
        hdMedicaidBillingNumber.Value = txtMBillingNumber.Text;
        PopupControls_HospiceEnrollment ucHospiceEnrollmentControl =
                LoadControl("~/PopupControls/HospiceEnrollment.ascx") as PopupControls_HospiceEnrollment;
        ucHospiceEnrollmentControl.ProviderMedId = this.WorkflowPage.MedicaidID;
        ucHospiceEnrollmentControl.MedicaidBillingNumber = txtMBillingNumber.Text;
        ucHospiceEnrollmentControl.Status = status;
        ucHospiceEnrollmentControl.LastSubmissionDate = lastSubmitedDate;
        ucHospiceEnrollmentControl.IscheckChangeofProvider = chlChangeofProvider.Checked;
        if (hasEnrollment) 
            ucHospiceEnrollmentControl.IscheckChangeofProvider = true;
        this.WorkflowPage.RecipientDateOfBirth = txtDateofBirth.Text;
        pnlHospiceSearch.Visible = false;
        if (hasEnrollment)
        {
            ucHospiceEnrollmentControl.Action = "Search";
            ucHospiceEnrollmentControl.HospiceTrackNo = hdHospiceTrackNo.Value;
        }
        else
        {
            ucHospiceEnrollmentControl.Action = "Add";
            ucHospiceEnrollmentControl.HospiceTrackNo = txtHTrackingNumber.Text;
        }
        HospicePlaceholderInitial.Controls.Add(ucHospiceEnrollmentControl);
        if (ucHospiceEnrollmentControl.Reason_LastBefit == "Death" && ucHospiceEnrollmentControl.IsDifferentProvider)
        {
            lblMessage.InnerText = "The individual recipient is deceased while enrolled with different provider, so new application cannot be added";
            UcHospiceEnrollmentControl_CancelHospiceClick();
        }
        else
        {
            pnlHospiceEnrollment.Visible = true;
            lblMessage.InnerText = string.Empty;
            this.WorkflowPage.IsHospiceEnrollmentClick = false;
            txtDateofBirth.Text = string.Empty;
        }
      
    }

    private static string GetExceptionMessage(Exception ex)
    {
        string exception = ex.Message;

        if (ex.InnerException != null && string.IsNullOrEmpty(ex.InnerException.Message))
        {
            exception += "Inner Exception :" + ex.InnerException.Message;
        }

        return exception;
    }

    private RecipientEligibilitySearchResponse GetRecipientInformation(string medicaidbillingnumber, string dateofBirth)
    {
        RecipientEligibilitySearchReqRes recipientEligibility = new RecipientEligibilitySearchReqRes();
        string medicaidID = this.WorkflowPage.MedicaidID;
        Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());
        var resr = recipientEligibility.GetRecipientEligibilitySearchResponse(medicaidbillingnumber, medicaidID, userId, dateofBirth, "HospiceEligibility");
            return resr;
    }
    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        BindData(txtHTrackingNumber.Text.Trim(), this.WorkflowPage.MedicaidID, txtMBillingNumber.Text.Trim());
        RadGridExport.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        BindData(txtHTrackingNumber.Text.Trim(), this.WorkflowPage.MedicaidID, txtMBillingNumber.Text.Trim());
        RadGridExport.MasterTableView.ExportToExcel();

    }
    protected void gvHospice_Sorting(object sender, GridViewSortEventArgs e)
    {
        BindData(txtHTrackingNumber.Text.Trim(), this.WorkflowPage.MedicaidID, txtMBillingNumber.Text.Trim());
    }

    protected void gvHospice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospice.PageIndex = e.NewPageIndex;
        BindData(txtHTrackingNumber.Text.Trim(), this.WorkflowPage.MedicaidID, txtMBillingNumber.Text.Trim());
    }
    protected void gvHospice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        { 
        if (e.CommandName.Equals("Sort"))
        {
            return;
        }
        this.WorkflowPage.HospiceRequestResponse = null;
        this.WorkflowPage.IsNewHospiceBenefitPeriod = false;
        this.WorkflowPage.HospicAttachments = null;
        this.WorkflowPage.HospicDocuments = null;
        var searchResponse = this.WorkflowPage.SearchHospiceResponse;
        int index = Convert.ToInt32(e.CommandArgument);
        string hospiceTrackNo = Convert.ToString(this.gvHospice.DataKeys[index].Values["HospiceTrackNo"].ToString());
        string medicaidBillingNumber = Convert.ToString(this.gvHospice.DataKeys[index].Values["RecipID"].ToString());
        string status = Convert.ToString(this.gvHospice.DataKeys[index].Values["Status"].ToString());
        string birthDate = Convert.ToString(searchResponse.Response[index].RecipBirthDate);
        string lastSubmitedDate = searchResponse.Response[index].SubmissionDate.ToString("MM/dd/yyyy");     
        try
        {
            //Get Recipient Information
           
            var t = txtMBillingNumber.Text;
            var recipientInfo = GetRecipientInformation(medicaidBillingNumber, birthDate);
            if (recipientInfo == null)
            {
                lblMessage.InnerText = "Recipient is not found";
                return;
            }
            else if (recipientInfo != null && recipientInfo.ResponseHeaderDetails != null && recipientInfo.ResponseHeaderDetails.ResponseType.ToUpper().Contains("FAILURE"))
            {
                lblMessage.InnerText = "Exception from Recipient Eligibility Service :" + recipientInfo.ResponseHeaderDetails.ResponseMessage;
                return;
            }
            else if (recipientInfo != null && recipientInfo.ErrorDetails != null && recipientInfo.ErrorDetails.Count() > 0)
            {
                var errors = string.Empty;
                foreach (var error in recipientInfo.ErrorDetails)
                {
                    errors += error.Description;
                }
                lblMessage.InnerText = "Exception from Recipient Eligibility Service :" + errors;
                return;
            }
            else
            {
                this.WorkflowPage.RecipientInformation = recipientInfo.RecipientInfo;
            }
        }
        catch (Exception ex)
        {
            string exception = GetExceptionMessage(ex);
            lblMessage.InnerText = "Exception in Recipient Eligibility Service :" + exception;
            return;
        }

            if (e.CommandName.Equals("ShowHosLinkage"))
            {
                this.WorkflowPage.HospiceRequestResponse = null;
                this.WorkflowPage.IsNewHospiceBenefitPeriod = false;
                this.WorkflowPage.HospicAttachments = null;
                this.WorkflowPage.HospicDocuments = null;
                this.WorkflowPage.HospiceStatus = status;
                hdHospiceTrackNo.Value = hospiceTrackNo;
                hdMedicaidBillingNumber.Value = medicaidBillingNumber;
                pnlHospiceSearch.Visible = false;
                PopupControls_HospiceEnrollment ucHospiceEnrollmentControl =
                        LoadControl("~/PopupControls/HospiceEnrollment.ascx") as PopupControls_HospiceEnrollment;
                ucHospiceEnrollmentControl.ProviderMedId = this.WorkflowPage.MedicaidID;
                ucHospiceEnrollmentControl.HospiceTrackNo = hospiceTrackNo;
                ucHospiceEnrollmentControl.MedicaidBillingNumber = medicaidBillingNumber;
                ucHospiceEnrollmentControl.Action = "Search";
                ucHospiceEnrollmentControl.Status = status;
                ucHospiceEnrollmentControl.LastSubmissionDate = lastSubmitedDate;
                ucHospiceEnrollmentControl.IscheckChangeofProvider = chlChangeofProvider.Checked;
                HospicePlaceholderInitial.Controls.Add(ucHospiceEnrollmentControl);
                if (ucHospiceEnrollmentControl.Reason_LastBefit == "Death" && ucHospiceEnrollmentControl.IsDifferentProvider)
                {
                    lblMessage.InnerText = "The individual recipient is deceased while enrolled with different provider, so new application cannot be added";
                    UcHospiceEnrollmentControl_CancelHospiceClick();
                }
                else
                {
                    pnlHospiceEnrollment.Visible = true;
                    this.WorkflowPage.IsHospiceEnrollmentClick = false;
                }
               
            }
        }
    }
    public void BindData(string hospiceTrackNo, string providerMedicaidId, string ReciptID, bool fromSearch=false)
    {
        if (chlChangeofProvider.Checked)
        {
            providerMedicaidId = "0000000";
            hospiceTrackNo = "";
        }
        try
        {
            SearchHospiceResponse response;
            if (fromSearch)
            {
               var isException=  SearchHospice(hospiceTrackNo, providerMedicaidId, ReciptID, Convert.ToInt32(ddlPageSize.SelectedValue));
                if (isException) return;
                response = this.WorkflowPage.SearchHospiceResponse;
                Session["HospiceResponse"] = response;
            }
            else
            {
                if (Session["HospiceResponse"] != null)
                    response = (SearchHospiceResponse)Session["HospiceResponse"];
                else
                {
                    var isException = SearchHospice(hospiceTrackNo, providerMedicaidId, ReciptID, Convert.ToInt32(ddlPageSize.SelectedValue));
                    if (isException) return;
                    response = this.WorkflowPage.SearchHospiceResponse;
                }
            }
            if (response == null)
            {
                lblMessage.InnerText = "No Records Found";
            }
            else if (response != null && response.Errors != null && response.Errors.Count() > 0)
            {
                var errors = string.Empty;
                foreach (var error in response.Errors)
                {
                    errors += error.ErrorDesc;
                }
                lblMessage.InnerText = errors;
            }
            else if (response.ResponseHeader.ResponseType == SearchResponsesResponseHeaderResponseType.Failure)
            {
                lblMessage.InnerText = response.ResponseHeader.ResponseMessage;
            }
            else if (response != null && response.Response != null)
            {
                gvHospice.DataSource = response.Response.ToList();
                gvHospice.VirtualItemCount = response.Response.Count();
                gvHospice.DataBind();
            }
        }
        catch (Exception ex)
        {
            string exception = GetExceptionMessage(ex);
            lblMessage.InnerText = exception;
        }
    }
    private bool SearchHospice(string hospiceTrackNo, string providerMedicaidId, string ReciptID, int recCount)
    {
        DataSet dshospice = new DataSet();
        var isException = false;
        try
        {
            // string sitTransactionKey = Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            var subsriberSystem = new List<SearchRequestMessageHeaderSubscriber>();
            subsriberSystem.Add(SearchRequestMessageHeaderSubscriber.FI);
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            SearchRequestMessageHeader msgHeader = new SearchRequestMessageHeader();
            msgHeader.BusinessFlow = "SearchHospice";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = SearchRequestMessageHeaderRequestorSystem.PNM;
            msgHeader.SubscriberSystem = subsriberSystem.ToArray();
            msgHeader.ModuleTransactionId = string.Empty;
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            msgHeader.SITransactionKey = sitTransactionKey;
            SearchHospiceRequest searchRequest = new SearchHospiceRequest
            {
                MessageHeader = msgHeader,
                Payload = new SearchRequestPayload
                {
                    HospiceTrackNo = (hospiceTrackNo != "") ? Convert.ToInt32(hospiceTrackNo) : 0,
                    HospiceTrackNoSpecified = (hospiceTrackNo != "" && Convert.ToInt32(hospiceTrackNo) != 0) ? true : false,
                    RecipID = ReciptID,
                    ProvMedID = providerMedicaidId,
                    Count = recCount,
                    CountSpecified = recCount > 0
                }
            };
            var response = HospiceServiceAgent.SearchHospiceResponse(searchRequest, "");
            
            var xDoc = XDocument.Parse(response);
            //var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals("SearchResponses"));
            TextReader tr = new StringReader(xDoc.ToString());
            dshospice.ReadXml(tr);

            this.WorkflowPage.SearchHospiceResponse = ConvertDatasetToSearchHospiceResponse(dshospice);
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                var httpresponse = ex.Response as HttpWebResponse;
                if (httpresponse != null)
                {
                    //http status code avaliable
                    var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    var xDoc = XDocument.Parse(respBody);
                    TextReader tr = new StringReader(xDoc.ToString());
                    dshospice.ReadXml(tr);

                    var response = ConvertDatasetToSearchHospiceResponse(dshospice);
                    if (response.ResponseHeader.ResponseType == SearchResponsesResponseHeaderResponseType.Failure)
                    {
                        lblMessage.InnerText = response.ResponseHeader.ResponseMessage;
                    }
                }
                else
                {
                    string exception = GetExceptionMessage(ex);
                    lblMessage.InnerText = exception;
                }
            }
            else
            {
                string exception = GetExceptionMessage(ex);
                lblMessage.InnerText = exception;
            }
            isException = true;
        }
        catch (Exception ex)
        {
            string exception = GetExceptionMessage(ex);
            lblMessage.InnerText = exception;
            isException = true;
        }
        return isException;
    }
    public SearchHospiceResponse ConvertDatasetToSearchHospiceResponse(DataSet ds)
    {
        SearchHospiceResponse response = new SearchHospiceResponse
        {
            //MessageHeader = (ds.Tables["MessageHeader"] != null) ? DatatableHelper.ConvertDataTableToList<SearchResponsesMessageHeader>
            ///(ds.Tables["MessageHeader"]).FirstOrDefault() : null,
            Response = (ds.Tables["Response"] != null) ? DatatableHelper.ConvertDataTableToList<SearchResponsesResponse>
                (ds.Tables["Response"]).ToArray() : null,
            Errors = (ds.Tables["Errors"] != null) ? DatatableHelper.ConvertDataTableToList<SearchResponsesErrors>
                (ds.Tables["Errors"]).ToArray() : null,
            ResponseHeader = (ds.Tables["ResponseHeader"] != null) ? DatatableHelper.ConvertDataTableToList<SearchResponsesResponseHeader>
                (ds.Tables["ResponseHeader"]).ToArray().FirstOrDefault() : null
        };

        return response;
    }
    public override bool SaveData()
    {
        return true;
    }

    public override void LoadControlData()
    {

    }

    public override void LoadData(DataRow dr)
    {
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "valProviderFinancial"; }
    }

    public override string Title
    {
        get { return "HOSPICE ENROLLMENT SEARCH"; }
    }

    //public WorkflowPage WorkflowPage
    //{
    //    get { return (WorkflowPage)this.Page; }
    //}

    public override string IdText
    {
        get { return "ucProviderFinancial_" + this.WorkflowPage.RegistrationId; }
    }

    protected void PageSize_Changed(object sender, EventArgs e)
    {
        gvHospice.PageIndex = 0;
        gvHospice.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        if (!string.IsNullOrEmpty(txtMBillingNumber.Text))
            BindData(txtHTrackingNumber.Text.Trim(), this.WorkflowPage.MedicaidID, txtMBillingNumber.Text.Trim());
    }
}