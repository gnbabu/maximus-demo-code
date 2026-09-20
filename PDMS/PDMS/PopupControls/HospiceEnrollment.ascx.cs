using Corp.Core.Libraries;
using Corp.Core.Libraries.AttachmentServiceReference;
using Corp.Core.Libraries.HospiceReference;
using Glimpse.Core.Extensions;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Telerik.Web.UI.PdfViewer;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HospiceEnrollment : System.Web.UI.UserControl
{
    public string HospiceTrackNo;
    public string ProviderMedId;
    public string MedicaidBillingNumber;
    public string Action;
    public string Status;
    public bool IsDifferentProvider;
    public bool IsProviderCoveredAllDays;
    public bool IscheckChangeofProvider;
    public string Reason_LastBefit;
    public string LastSubmissionDate;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }


    public delegate void EventHandler();
    public event EventHandler CancelHospiceClick;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.Cookies["selectedOption"] != null && Request.Cookies["selectedOption"].Value != null && Request.Cookies["selectedOption"].Value != "10007") return;


        btnHospiceSubmit.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnHospiceSubmit, null) + ";");

        if (this.WorkflowPage.HospiceRequestResponse == null)
        {
            lblMessage.Text = string.Empty;
            LoadHospiceApplicationActionType();
            this.WorkflowPage.MedicaidID = this.ProviderMedId;
            this.WorkflowPage.HospiceTrackNo = this.HospiceTrackNo;
            this.WorkflowPage.MedicaidBillingNumber = this.MedicaidBillingNumber;
            if (Action == "Search")
            {
                var isException = GetInquireHospiceResponse(this.HospiceTrackNo, (this.IscheckChangeofProvider) ? "0000000" : this.ProviderMedId);
                if (isException)
                {
                    return;
                }
                var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
                hdnHospiceStatus.Value = this.Status;
                hdLastSubmittedDate.Value = this.LastSubmissionDate;
                if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) == true && !Helper.IsUserInSubRoles(this.WorkflowPage.RegistrationId, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.HospiceMaintenanceSubRoles))
                {
                    hdHospiceMaintenanceSubRole.Value = "false";
                }
                else
                {
                    hdHospiceMaintenanceSubRole.Value = "true";
                }
                if (inquireResponse != null
                       && inquireResponse.Payload != null)
                {
                    ddlHospiceApplicationType.SelectedValue = inquireResponse.Payload.ActionType;
                }
                FillHospiceEnrollment(inquireResponse);
            }
            else
            {
                ddlHospiceApplicationType.SelectedValue = "NEWEN";
                ddlHospiceApplicationType.Enabled = false;
                string lastName, firstName;
                DateTime reciBrithDate;
                GetRecipientInformation(out lastName, out firstName, out reciBrithDate);
                this.WorkflowPage.HospiceRequestResponse = new AddUpdateHospiceRequest
                {
                    Payload = new HospiceRequestResponsePayload
                    {
                        ActionType = ddlHospiceApplicationType.SelectedValue,
                        HospiceTrackNo = 0,
                        RecipID = this.WorkflowPage.MedicaidBillingNumber,
                        ConsBirthDate = reciBrithDate,
                        ConsFirstName = firstName,
                        ConsLastName = lastName
                    }
                };
                FilterApplicationActionType();
                hdIsAllownewbenPeriod.Value = "true";
            }

        }
        else
        {
            if (this.WorkflowPage.HospiceRequestResponse.Payload != null && this.WorkflowPage.HospiceRequestResponse.Payload.HospiceTrackNo != 0)
            {
                lblHospiceTrackingNumber.Text = this.WorkflowPage.HospiceRequestResponse.Payload.HospiceTrackNo.ToString();
                lblDateofApplicationUpdate.Text = hdLastSubmittedDate.Value;
            }
        }
        if (ddlHospiceApplicationType.SelectedValue == "REVOC" || ddlHospiceApplicationType.SelectedValue == "BFTRM" || (ddlHospiceApplicationType.SelectedValue == "MAINT" && this.WorkflowPage.HospiceRequestResponse != null && this.WorkflowPage.HospiceRequestResponse.Payload != null
                && this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods != null && this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).ToList().FirstOrDefault(x => x.BenUpdateReason == "001" || x.BenUpdateReason == "002"
                || x.BenUpdateReason == "003"
                || x.BenUpdateReason == "004"
                || x.BenUpdateReason == "005"
                || x.BenUpdateReason == "006") != null))
        {
            txtDisenrollment.Enabled = true;
        }
        else
        {
            if (ddlHospiceApplicationType.SelectedValue == "MAINT" && this.WorkflowPage.HospiceRequestResponse != null && this.WorkflowPage.HospiceRequestResponse.Payload != null && this.WorkflowPage.HospiceRequestResponse.Payload.ElectionDisenrollDates != null && (this.WorkflowPage.HospiceRequestResponse.Payload.ElectionDisenrollDates.DisenrollDate != DateTime.MinValue ||
               Convert.ToDateTime(this.WorkflowPage.HospiceRequestResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy") != "01/01/0001"))
            {
                txtDisenrollment.Enabled = true;
            }
            else
            {
                txtDisenrollment.Enabled = false;
            }
        }
        this.WorkflowPage.HospiceSelectedActionType = ddlHospiceApplicationType.SelectedValue;
        if (ddlHospiceApplicationType.SelectedValue != "")
        {
            btnHospiceSubmit.Enabled = true;
        }
        else
        {
            btnHospiceSubmit.Enabled = false;
        }
    }
    public bool GetInquireHospiceResponse(string hospiceTrackNo, string providerMedId)
    {
        DataSet dshospice = new DataSet();
        bool isException = false;
        try
        {
            //string sitTransactionKey = Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            var subsriberSystem = new List<InquireRequestMessageHeaderSubscriber>();
            subsriberSystem.Add(InquireRequestMessageHeaderSubscriber.FI);
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            InquireRequestMessageHeader msgHeader = new InquireRequestMessageHeader();
            msgHeader.BusinessFlow = "InquireHospice";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = InquireRequestMessageHeaderRequestorSystem.PNM;
            msgHeader.SubscriberSystem = subsriberSystem.ToArray();
            msgHeader.ModuleTransactionId = string.Empty;
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            msgHeader.SITransactionKey = sitTransactionKey;
            InquireHospiceRequest InquireRequest = new InquireHospiceRequest
            {
                MessageHeader = msgHeader,
                Payload = new InquireRequestPayload
                {
                    HospiceTrackNo = Convert.ToInt32(hospiceTrackNo),
                    ProvMedID = providerMedId
                }
            };
            //DataSet ds = new DataSet();
            var response = HospiceServiceAgent.HospiceInquireRequest(InquireRequest, "");
            var xDoc = XDocument.Parse(response);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.XmlResolver = null;
            XmlReader xmlReader = XmlReader.Create(new StringReader(response.ToString()), settings);

            dshospice.ReadXml(xmlReader);

            this.WorkflowPage.InquireHospiceResponse = this.WorkflowPage.ConvertDatasetToInquireResponse(dshospice);
            ddlHospiceApplicationType.Enabled = true;
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

                    var response = this.WorkflowPage.ConvertDatasetToInquireResponse(dshospice);
                    if (response.ResponseHeader.ResponseType == InquireResponseResponseHeaderResponseType.Failure)
                    {
                        lblMessage.Text = response.ResponseHeader.ResponseMessage;
                    }
                }
                else
                {
                    string exception = GetExceptionMessage(ex);
                    lblMessage.Text = exception;
                }
            }
            else
            {
                string exception = GetExceptionMessage(ex);
                lblMessage.Text = exception;
            }
            isException = true;
            ddlHospiceApplicationType.Enabled = false;
        }
        catch (Exception ex)
        {
            string exception = GetExceptionMessage(ex);
            lblMessage.Text = exception;
            isException = true;
        }
        return isException;
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

    private void FillHospiceEnrollment(AddUpdateHospiceRequest inquireResponse, bool isFilterActionType = true)
    {
        if (inquireResponse != null
                        && inquireResponse.Payload != null)
        {

            if (inquireResponse.Payload.ElectionDisenrollDates != null)
            {
                txtElectionDate.Text = inquireResponse.Payload.ElectionDisenrollDates.ElectionDate.ToString("MM/dd/yyyy");
                if (inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate != DateTime.MinValue ||
                    Convert.ToDateTime(inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy") != "01/01/0001")
                {
                    txtDisenrollment.Text = Convert.ToDateTime(inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy");
                    hdnDisenrollment.Value = Convert.ToDateTime(inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy");
                }
            }

            //set HospiceTracking number flag
            inquireResponse.Payload.HospiceTrackNoSpecified =
                (Convert.ToInt32(inquireResponse.Payload.HospiceTrackNo) != 0) ? true : false;
            if (this.WorkflowPage.HospiceRequestResponse.Payload.ProvService != null)
            {
                IsDifferentProvider = this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.ToList().Where(x => x.HospiceProvID != this.ProviderMedId).Count() > 0;
            }
            if (this.WorkflowPage.HospiceRequestResponse.Payload.ProvService != null && this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods != null)
            {
                var latestben = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                var pEffDate = inquireResponse.Payload.ProvService.Where(x => x.BenPeriod == latestben.BenPeriod).OrderBy(x => x.SpanEffDate).FirstOrDefault();
                var pEndDate = inquireResponse.Payload.ProvService.Where(x => x.BenPeriod == latestben.BenPeriod).OrderByDescending(x => x.SpanEndDate).FirstOrDefault();
                if (pEffDate != null && pEndDate != null && latestben.BenPeriodEffDate == pEffDate.SpanEffDate && latestben.BenPeriodEndDate == pEndDate.SpanEndDate)
                {
                    IsProviderCoveredAllDays = true;
                }
            }
            if (IsDifferentProvider)
            {
                AddUpdateHospiceRequest prevaddUpdateHospiceRequest = new AddUpdateHospiceRequest
                {
                    Payload = new HospiceRequestResponsePayload
                    {
                        ServiceCountyState = inquireResponse.Payload.ServiceCountyState,
                        ElectionDisenrollDates = inquireResponse.Payload.ElectionDisenrollDates,
                        LongTermCareFacility = inquireResponse.Payload.LongTermCareFacility,
                        BenefitPeriods = inquireResponse.Payload.BenefitPeriods,
                        OtherPayerInfo = inquireResponse.Payload.OtherPayerInfo,
                        DiagnosisCodes = inquireResponse.Payload.DiagnosisCodes,
                        ProvService = inquireResponse.Payload.ProvService,
                        Attachments = inquireResponse.Payload.Attachments,
                        ActionType = inquireResponse.Payload.ActionType,
                        RecipID = inquireResponse.Payload.RecipID,
                        ConsBirthDate = inquireResponse.Payload.ConsBirthDate,
                        ConsFirstName = inquireResponse.Payload.ConsFirstName,
                        ConsLastName = inquireResponse.Payload.ConsLastName,
                        HospiceTrackNo = inquireResponse.Payload.HospiceTrackNo,
                        HospiceTrackNoSpecified = inquireResponse.Payload.HospiceTrackNoSpecified
                    }
                };
                this.WorkflowPage.PreviousProviderHospiceRequestResponse = prevaddUpdateHospiceRequest;
                this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState = null;
                this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility = null;
                this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes = null;
                this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo = null;
                this.WorkflowPage.HospiceRequestResponse.Payload.ProvService = null;
                this.WorkflowPage.HospiceRequestResponse.Payload.Attachments = null;
                this.WorkflowPage.HospiceRequestResponse.Payload.HospiceTrackNo = 0;
                if (inquireResponse.Payload.BenefitPeriods != null && inquireResponse.Payload.BenefitPeriods.Count() > 0)
                {
                    var latestben = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).Select(y => y.BenPeriod).FirstOrDefault();
                    var benPerioids = inquireResponse.Payload.BenefitPeriods.ToList();
                    foreach (var ben in inquireResponse.Payload.BenefitPeriods)
                    {
                        if (latestben != ben.BenPeriod)
                        {
                            benPerioids.Remove(ben);
                        }
                    }
                    this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benPerioids.ToArray();
                    if (this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Count() > 0)
                    {
                        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods[0].IsHideBenfitperiodPhy = true;
                        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods[0].IsHideBenfitperiodIDG = true;
                        var preHospiceInquire = this.WorkflowPage.PreviousProviderHospiceRequestResponse;
                        if (preHospiceInquire != null && preHospiceInquire.Payload != null && preHospiceInquire.Payload.ActionType != "CLSPR" && ddlHospiceApplicationType.SelectedValue == "CHGPR")
                        {
                            this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods[0].IsDifferentProdvider = false;
                        }
                        else if (IsProviderCoveredAllDays)
                        {
                            this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods[0].IsDifferentProdvider = true;
                        }
                    }

                }
            }
            //update isFromInquery property to true
            if (inquireResponse.Payload.ServiceCountyState != null)
            {
                inquireResponse.Payload.ServiceCountyState.ToList().ForEach(x => x.IsFromInquiry = true);
            }
            if (inquireResponse.Payload.LongTermCareFacility != null)
            {
                inquireResponse.Payload.LongTermCareFacility.ToList().ForEach(x => x.IsFromInquiry = true);
            }
            if (inquireResponse.Payload.BenefitPeriods != null)
            {
                inquireResponse.Payload.BenefitPeriods.ToList().ForEach(x => x.IsFromInquiry = true);
                var benlastReason = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).Select(y => y.BenUpdateReason).FirstOrDefault();
                Reason_LastBefit = GetHospiceReasonUpdateTypeByValue(benlastReason);
            }
            if (inquireResponse.Payload.DiagnosisCodes != null)
            {
                inquireResponse.Payload.DiagnosisCodes.ToList().ForEach(x => x.IsFromInquiry = true);
            }
            if (inquireResponse.Payload.OtherPayerInfo != null)
            {
                inquireResponse.Payload.OtherPayerInfo.ToList().ForEach(x => x.IsFromInquiry = true);
            }
            if (inquireResponse.Payload.ProvService != null)
            {
                inquireResponse.Payload.ProvService.ToList().ForEach(x => x.IsFromInquiry = true);
            }
            if (inquireResponse.Payload.Attachments != null)
            {
                inquireResponse.Payload.Attachments.ToList().ForEach(x => x.IsFromInquiry = true);
                foreach (var item in inquireResponse.Payload.Attachments.ToList())
                {
                    item.IsFromInquiry = true;
                    if (item.AttachDate.ToString("MM/dd/yyyy") != "01/01/0001")
                    {
                        item.AttachDateSpecified = true;
                    }
                }
            }
            if (inquireResponse.Payload.HospiceTrackNo != 0)
            {
                lblHospiceTrackingNumber.Text = inquireResponse.Payload.HospiceTrackNo.ToString();
                lblDateofApplicationUpdate.Text = hdLastSubmittedDate.Value;
            }

            //lblDateofApplicationUpdate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            if (isFilterActionType)
            {
                var isAllownewbenPeriod = FilterApplicationActionType();
                hdIsAllownewbenPeriod.Value = isAllownewbenPeriod.ToString();
                if (this.Status != "C")
                {
                    ddlHospiceApplicationType.SelectedValue = "";
                }
            }
        }
        else
        {
            ddlHospiceApplicationType.SelectedValue = "NEWEN";
            ddlHospiceApplicationType.Enabled = false;
            string lastName, firstName;
            DateTime reciBrithDate;
            GetRecipientInformation(out lastName, out firstName, out reciBrithDate);
            this.WorkflowPage.HospiceRequestResponse = new AddUpdateHospiceRequest
            {
                Payload = new HospiceRequestResponsePayload
                {
                    ActionType = ddlHospiceApplicationType.SelectedValue,
                    HospiceTrackNo = 0,
                    RecipID = this.WorkflowPage.MedicaidBillingNumber,
                    ConsBirthDate = reciBrithDate,
                    ConsFirstName = firstName,
                    ConsLastName = lastName
                }
            };
            FilterApplicationActionType();
            hdIsAllownewbenPeriod.Value = "true";
        }
    }
    private string GetHospiceReasonUpdateTypeByValue(string value)
    {
        var ds = GetHospiceReasonUpdateType();
        return (from q in ds.Tables[0].AsEnumerable()
                where q.Field<string>("REASON_TYPE_VALUE") == value
                select q.Field<string>("REASON_TYPE_DESC")).FirstOrDefault();
    }
    private DataSet GetHospiceReasonUpdateType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospiceReasonUpdateType();
        }
    }
    protected void ddlHospiceApplicationType_Change(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        divPortalErrors.InnerHtml = string.Empty;
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        this.HospiceTrackNo = this.WorkflowPage.HospiceTrackNo;
        this.ProviderMedId = this.WorkflowPage.MedicaidID;
        this.MedicaidBillingNumber = this.WorkflowPage.MedicaidBillingNumber;
        this.Status = hdnHospiceStatus.Value;
        this.WorkflowPage.HospiceSelectedActionType = ddlHospiceApplicationType.SelectedValue;
        if (this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods != null && this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Count() > 0)
        {
            var preHospiceInquire = this.WorkflowPage.PreviousProviderHospiceRequestResponse;
            if (preHospiceInquire != null && preHospiceInquire.Payload != null && preHospiceInquire.Payload.ProvService != null && preHospiceInquire.Payload.BenefitPeriods != null)
            {
                var latestben = preHospiceInquire.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                var pEffDate = preHospiceInquire.Payload.ProvService.Where(x => x.BenPeriod == latestben.BenPeriod).OrderBy(x => x.SpanEffDate).FirstOrDefault();
                var pEndDate = preHospiceInquire.Payload.ProvService.Where(x => x.BenPeriod == latestben.BenPeriod).OrderByDescending(x => x.SpanEndDate).FirstOrDefault();
                if (pEffDate != null && pEndDate != null && latestben.BenPeriodEffDate == pEffDate.SpanEffDate && latestben.BenPeriodEndDate == pEndDate.SpanEndDate)
                {
                    IsProviderCoveredAllDays = true;
                }
            }
            if (preHospiceInquire != null && preHospiceInquire.Payload != null && preHospiceInquire.Payload.ActionType != "CLSPR" && ddlHospiceApplicationType.SelectedValue == "CHGPR")
            {
                this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods[0].IsDifferentProdvider = false;
            }
            else if (IsProviderCoveredAllDays)
            {
                this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods[0].IsDifferentProdvider = true;
            }
        }
        this.WorkflowPage.IsHospiceTerminatedAndNewEnrollment = false;
        this.WorkflowPage.HospiceTerminatedBenefitNumber = 0;
        if (ddlHospiceApplicationType.SelectedValue == "NEWEN")
        {
            var reasonLastBefit = string.Empty;
            int latestBenfitNo = 0;
            if (inquireResponse.Payload.BenefitPeriods != null)
            {
                var benlastReason = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).Select(y => y.BenUpdateReason).FirstOrDefault();
                reasonLastBefit = GetHospiceReasonUpdateTypeByValue(benlastReason);
                latestBenfitNo = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).Select(y => y.BenPeriod).FirstOrDefault();
            }
            if ((this.Status == "P" && ((inquireResponse.Payload.ActionType == "MAINT" && (reasonLastBefit == "Individual no longer meets the enrollment criteria"
                || reasonLastBefit == "Individual is no longer terminally ill"
                || reasonLastBefit == "Individual moved out the service area"
                || reasonLastBefit == "Individual entered a non-contracted facility"
                || reasonLastBefit == "Individual revoked the Medicaid hospice benefit"
                || reasonLastBefit == "For cause")) || inquireResponse.Payload.ActionType == "REVOC" || inquireResponse.Payload.ActionType == "BFTRM")) || this.Status == "D")
            {
                this.WorkflowPage.HospiceStatus = this.Status;

                this.WorkflowPage.IsHospiceTerminatedAndNewEnrollment = true;
                this.WorkflowPage.HospiceTerminatedBenefitNumber = latestBenfitNo;
                this.WorkflowPage.HospiceRequestResponse = null;
                this.WorkflowPage.IsNewHospiceBenefitPeriod = false;
                this.WorkflowPage.HospicAttachments = null;
                this.WorkflowPage.HospicDocuments = null;
                ddlHospiceApplicationType.SelectedValue = "NEWEN";
                ddlHospiceApplicationType.Enabled = false;

                string lastName, firstName;
                DateTime reciBrithDate;
                GetRecipientInformation(out lastName, out firstName, out reciBrithDate);
                this.WorkflowPage.HospiceRequestResponse = new AddUpdateHospiceRequest
                {
                    Payload = new HospiceRequestResponsePayload
                    {
                        ActionType = ddlHospiceApplicationType.SelectedValue,
                        HospiceTrackNo = 0,
                        RecipID = this.WorkflowPage.MedicaidBillingNumber,
                        ConsBirthDate = reciBrithDate,
                        ConsFirstName = firstName,
                        ConsLastName = lastName
                    }
                };
                hdIsAllownewbenPeriod.Value = "true";
                txtDisenrollment.Text = "";
                hdnDisenrollment.Value = "";
                txtElectionDate.Text = "";
                lblHospiceTrackingNumber.Text = "";
                lblDateofApplicationUpdate.Text = "";
                txtDisenrollment.Enabled = false;
                uc3HospiceBenefitPeriod.BindGrid(this.Status);
                return;
            }
            if (this.WorkflowPage.PreviousProviderHospiceRequestResponse != null && this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload != null && this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload.ProvService != null)
            {
                if (this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload.ProvService.ToList().Where(x => x.HospiceProvID != this.ProviderMedId).Count() > 0)
                {
                    this.WorkflowPage.IsHospiceTerminatedAndNewEnrollment = true;
                    this.WorkflowPage.HospiceTerminatedBenefitNumber = latestBenfitNo;
                    this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = null;
                    ddlHospiceApplicationType.Enabled = false;
                }
            }

        }

        FillHospiceEnrollment(inquireResponse, false);
        if (ddlHospiceApplicationType.SelectedValue == "REVOC" || ddlHospiceApplicationType.SelectedValue == "BFTRM" || (ddlHospiceApplicationType.SelectedValue == "MAINT" && this.WorkflowPage.HospiceRequestResponse != null && this.WorkflowPage.HospiceRequestResponse.Payload != null
               && this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods != null && this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).ToList().FirstOrDefault(x => x.BenUpdateReason == "001" || x.BenUpdateReason == "002"
               || x.BenUpdateReason == "003"
               || x.BenUpdateReason == "004"
               || x.BenUpdateReason == "005"
               || x.BenUpdateReason == "006") != null))
        {
            txtDisenrollment.Enabled = true;
        }
        else
        {
            if (ddlHospiceApplicationType.SelectedValue == "MAINT" && inquireResponse.Payload.ElectionDisenrollDates != null && (inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate != DateTime.MinValue ||
                Convert.ToDateTime(inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy") != "01/01/0001"))
            {
                txtDisenrollment.Enabled = true;
            }
            else
            {
                txtDisenrollment.Text = "";
                txtDisenrollment.Enabled = false;
            }
        }
        if (inquireResponse.Payload.ElectionDisenrollDates != null)
        {
            if (inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate != DateTime.MinValue ||
                Convert.ToDateTime(inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy") != "01/01/0001")
            {
                txtDisenrollment.Text = Convert.ToDateTime(inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy");
                hdnDisenrollment.Value = Convert.ToDateTime(inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate).ToString("MM/dd/yyyy");
            }
        }
        this.WorkflowPage.HospiceSelectedActionType = ddlHospiceApplicationType.SelectedValue;
        if (ddlHospiceApplicationType.SelectedValue != "")
        {
            btnHospiceSubmit.Enabled = true;
        }
        else
        {
            btnHospiceSubmit.Enabled = false;
        }
        uc3HospiceBenefitPeriod.BindGrid();
    }

    private void GetRecipientInformation(out string lastName, out string firstName, out DateTime reciBrithDate)
    {
        var ri = this.WorkflowPage.RecipientInformation;
        string middleName = string.Empty;
        lastName = string.Empty;
        firstName = string.Empty;
        reciBrithDate = DateTime.MinValue;
        if (ri == null || string.IsNullOrEmpty(ri.LastName) || string.IsNullOrEmpty(ri.FirstName) || ri.DateOfBirth == null)
        {
            if (this.WorkflowPage.SearchHospiceResponse != null && this.WorkflowPage.SearchHospiceResponse.Response != null && this.WorkflowPage.HospiceTrackNo != "0" && this.WorkflowPage.HospiceTrackNo != "")
            {
                var info = this.WorkflowPage.SearchHospiceResponse.Response.Where(x => x.HospiceTrackNo == Convert.ToInt32(this.WorkflowPage.HospiceTrackNo)).FirstOrDefault();
                if (info != null)
                {
                    lastName = info.RecipName.Split(',')[0];
                    firstName = info.RecipName.Split(',')[1];
                    reciBrithDate = info.RecipBirthDate;
                }
            }

        }
        else
        {
            if (ri.MiddleName != null)
            {
                middleName = (string)ri.MiddleName;
                middleName = middleName.Length > 1 ? ", " + middleName.Substring(0, 1) : middleName;
            }
            reciBrithDate = ri.DateOfBirth != null && ri.DateOfBirth.HasValue ? ri.DateOfBirth.Value : DateTime.MinValue;
            firstName = ri.FirstName + middleName;
            lastName = ri.LastName;
        }
    }

    private string CheckBenPeriodExists(int[] source, int[] dest)
    {
        string missingbens = string.Empty;
        foreach (var a in source)
        {
            if (!dest.Any(b => b == a))
            {
                missingbens = (!string.IsNullOrEmpty(missingbens)) ? missingbens + "," + a.ToString() : a.ToString();
            }
        }

        return missingbens;
    }

    protected void btnHospiceSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //Validations 
            lblMessage.Text = string.Empty;
            divPortalErrors.InnerHtml = string.Empty;
            var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
            if (inquireResponse.Payload.BenefitPeriods != null)
            {
                var benPeriodIds = inquireResponse.Payload.BenefitPeriods.Where(y => y.IsDifferentProdvider == false).Select(x => x.BenPeriod).ToArray();
                StringBuilder strErrors = new StringBuilder();
                if (this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods != null && this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Count() > 0
                    && ddlHospiceApplicationType.SelectedValue != "CHGPR")
                {
                    //Overlap validation
                    var firstscDateshlbp = (HospiceRequestResponsePayloadBenefitPeriods)null;
                    foreach (var benId in benPeriodIds)
                    {
                        var benefitPeriods = inquireResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == benId && x.IsDifferentProdvider == false).OrderBy(y => y.BenPeriod).ToList();
                        foreach (var dates in benefitPeriods)
                        {
                            if (firstscDateshlbp != null)
                            {
                                if ((dates.BenPeriodEffDate >= firstscDateshlbp.BenPeriodEffDate && dates.BenPeriodEffDate <= firstscDateshlbp.BenPeriodEndDate) ||
                                (dates.BenPeriodEndDate >= firstscDateshlbp.BenPeriodEffDate && dates.BenPeriodEndDate <= firstscDateshlbp.BenPeriodEndDate))
                                {
                                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceBenefitPeriod\")' Style='color: red;'>Effective date or End date  should not fall in between previously entered effective end date for BenfitPeriod Line No's : {0}.</a> <br/>", benId));
                                    break;
                                }
                            }
                            firstscDateshlbp = dates;
                        }
                    }
                }

                //OHPNM-17569
                //60/90 days validations for benifits period
                foreach (var benefitPeriod in inquireResponse.Payload.BenefitPeriods)
                {
                    if (benefitPeriod.BenPeriodType == 1 || benefitPeriod.BenPeriodType == 2)
                    {
                        if (((benefitPeriod.BenPeriodEndDate - benefitPeriod.BenPeriodEffDate).TotalDays) > 90)
                        {
                            strErrors.Append(string.Format("<a onclick='SetBackgroundColor(\"gvHospiceBenefitPeriod\")' Style='color: red;'>Days count exceeded the allowed range.</a> <br/>"));
                        }
                    }

                    if (benefitPeriod.BenPeriodType == 3)
                    {
                        if (((benefitPeriod.BenPeriodEndDate - benefitPeriod.BenPeriodEffDate).TotalDays) > 60)
                        {
                            strErrors.Append(string.Format("<a onclick='SetBackgroundColor(\"gvHospiceBenefitPeriod\")' Style='color: red;'>Days count exceeded the allowed range.</a> <br/>"));
                        }

                    }
                }

                if (this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState != null && this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState.Count() > 0)
                {
                    var serviceCountyStateBenids = inquireResponse.Payload.ServiceCountyState.Select(x => x.BenPeriod).ToArray();
                    var ben = CheckBenPeriodExists(benPeriodIds, serviceCountyStateBenids);
                    if (!string.IsNullOrEmpty(ben))
                    {
                        strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvRecipentServiceLocation\")' Style='color: red;'>County and State of Recipient’s Hospice Service Location is required information panel for BenfitPeriod Line No's : {0}.</a> <br/>", ben));
                    }
                    //Overlap validation
                    var firstscDatesCounty = (HospiceRequestResponsePayloadServiceCountyState)null;
                    foreach (var benId in benPeriodIds)
                    {
                        var serviceCountyStates = inquireResponse.Payload.ServiceCountyState.Where(x => x.BenPeriod == benId).OrderBy(y => y.CountyEffDate).ToList();

                        foreach (var dates in serviceCountyStates)
                        {
                            if (firstscDatesCounty != null)
                            {
                                if ((dates.CountyEffDate >= firstscDatesCounty.CountyEffDate && dates.CountyEffDate <= firstscDatesCounty.CountyEndDate) ||
                                (dates.CountyEndDate >= firstscDatesCounty.CountyEffDate && dates.CountyEndDate <= firstscDatesCounty.CountyEndDate))
                                {
                                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvRecipentServiceLocation\")' Style='color: red;'>Effective date or End date  should not fall in between previously entered effective end date for BenfitPeriod Line No's : {0}.</a> <br/>", benId));
                                    break;
                                }
                            }
                            firstscDatesCounty = dates;
                        }
                    }

                    //days gap validation

                    foreach (var benId in benPeriodIds)
                    {
                        var serviceCountyStates = inquireResponse.Payload.ServiceCountyState.Where(x => x.BenPeriod == benId).OrderBy(y => y.CountyEffDate).ToList();
                        var firstscDates = (HospiceRequestResponsePayloadServiceCountyState)null;
                        foreach (var dates in serviceCountyStates)
                        {
                            if (firstscDates != null)
                            {
                                if ((dates.CountyEffDate - firstscDates.CountyEndDate).TotalDays > 1)
                                {
                                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvRecipentServiceLocation\")' Style='color: red;'>A county must be assigned for every day within the benefit period for BenfitPeriod Line No's : {0}.</a> <br/>", benId));
                                    break;
                                }
                            }
                            firstscDates = dates;
                        }
                    }
                }
                else
                {
                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvRecipentServiceLocation\")' Style='color: red;'>County and State of Recipient’s Hospice Service Location is required information panel.<a> <br/>"));
                }

                string attPhyMissingbens = string.Empty;
                string idgPhyMissingbens = string.Empty;
                foreach (var benperiod in this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(y => y.IsDifferentProdvider == false))
                {
                    if (benperiod.PhyNPI == null)
                    {
                        attPhyMissingbens = (!string.IsNullOrEmpty(attPhyMissingbens)) ? attPhyMissingbens + "," + benperiod.BenPeriod : benperiod.BenPeriod.ToString();
                    }
                    if (benperiod.IDGPhyNPI == null)
                    {
                        idgPhyMissingbens = (!string.IsNullOrEmpty(idgPhyMissingbens)) ? idgPhyMissingbens + "," + benperiod.BenPeriod : benperiod.BenPeriod.ToString();

                    }
                }
                if (!string.IsNullOrEmpty(attPhyMissingbens))
                {
                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceAttendingPhysician\")' Style='color: red;'>Hospice Attending Physician is required information panel for BenfitPeriod Line No's : {0}.</a> <br/>", attPhyMissingbens));
                }
                if (!string.IsNullOrEmpty(idgPhyMissingbens))
                {
                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceIDGPhysician\")' Style='color: red;'>Hospice IDG Physician is required information panel for BenfitPeriod Line No's : {0}.</a> <br/>", idgPhyMissingbens));
                }
                if (this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes != null && this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes.Count() > 0)
                {
                    var diagnosisCodesbendIds = inquireResponse.Payload.DiagnosisCodes.Select(x => x.BenPeriod).ToArray();
                    var ben = CheckBenPeriodExists(benPeriodIds, diagnosisCodesbendIds);
                    if (!string.IsNullOrEmpty(ben))
                    {
                        strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceTerminalIllnessDiagnosis\")' Style='color: red;'>Hospice Terminal Illness Diagnosis is required information panel for BenfitPeriod Line No's : {0}.</a> <br/>", ben));
                    }

                    //Overlap validation
                    var firstscDatesdc = (HospiceRequestResponsePayloadDiagnosisCodes)null;
                    foreach (var benId in benPeriodIds)
                    {
                        var diagnosisCodes = inquireResponse.Payload.DiagnosisCodes.Where(x => x.BenPeriod == benId).OrderBy(y => y.DiagEffDate).ToList();

                        foreach (var dates in diagnosisCodes)
                        {
                            if (firstscDatesdc != null)
                            {
                                if ((dates.DiagEffDate >= firstscDatesdc.DiagEffDate && dates.DiagEffDate <= firstscDatesdc.DiagEndDate) ||
                                (dates.DiagEndDate >= firstscDatesdc.DiagEffDate && dates.DiagEndDate <= firstscDatesdc.DiagEndDate))
                                {
                                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceTerminalIllnessDiagnosis\")' Style='color: red;'>Effective date or End date  should not fall in between previously entered effective end date for BenfitPeriod Line No's : {0}.</a> <br/>", benId));
                                    break;
                                }
                            }
                            firstscDatesdc = dates;
                        }
                    }

                    //days gap validation

                    foreach (var benId in benPeriodIds)
                    {
                        var diagnosisCodes = inquireResponse.Payload.DiagnosisCodes.Where(x => x.BenPeriod == benId).OrderBy(y => y.DiagEffDate).ToList();
                        var firstscDates = (HospiceRequestResponsePayloadDiagnosisCodes)null;
                        foreach (var dates in diagnosisCodes)
                        {
                            if (firstscDates != null)
                            {
                                if ((dates.DiagEffDate - firstscDates.DiagEndDate).TotalDays > 1)
                                {
                                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceTerminalIllnessDiagnosis\")' Style='color: red;'>Hospice Terminal Illness Diagnosis must be assigned for every day within the benefit period for BenfitPeriod Line No's : {0}.</a> <br/>", benId));
                                    break;
                                }
                            }
                            firstscDates = dates;
                        }
                    }
                }
                if (this.WorkflowPage.HospiceRequestResponse.Payload.ProvService != null && this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.Count() > 0)
                {
                    var provServicebendIds = inquireResponse.Payload.ProvService.Select(x => x.BenPeriod).ToArray();
                    var ben = CheckBenPeriodExists(benPeriodIds, provServicebendIds);
                    if (!string.IsNullOrEmpty(ben))
                    {
                        strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceProviderServiceSpan\")' Style='color: red;'>Hospice Provider Span is required information panel for BenfitPeriod Line No's : {0}.</a> <br/>", ben));
                    }
                    //Overlap validation
                    var firstscDatesprovService = (HospiceRequestResponsePayloadProvService)null;
                    foreach (var benId in benPeriodIds)
                    {
                        var provService = inquireResponse.Payload.ProvService.Where(x => x.BenPeriod == benId).OrderBy(y => y.BenPeriod).ToList();

                        foreach (var dates in provService)
                        {
                            if (firstscDatesprovService != null)
                            {
                                if ((dates.SpanEffDate >= firstscDatesprovService.SpanEffDate && dates.SpanEffDate <= firstscDatesprovService.SpanEndDate) ||
                                (dates.SpanEndDate >= firstscDatesprovService.SpanEffDate && dates.SpanEndDate <= firstscDatesprovService.SpanEndDate))
                                {
                                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceProviderServiceSpan\")' Style='color: red;'>Effective date or End date  should not fall in between previously entered effective end date for BenfitPeriod Line No's : {0}.</a> <br/>", benId));
                                    break;
                                }
                            }
                            firstscDatesprovService = dates;
                        }
                    }
                }
                if (this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility != null && this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility.Count() > 0)
                {
                    //Overlap validation
                    var firstscDateshltc = (HospiceRequestResponsePayloadLongTermCareFacility)null;
                    foreach (var benId in benPeriodIds)
                    {
                        var longTermCareFacility = inquireResponse.Payload.LongTermCareFacility.Where(x => x.BenPeriod == benId).OrderBy(y => y.HLTCFEffDate).ToList();
                        foreach (var dates in longTermCareFacility)
                        {
                            if (firstscDateshltc != null)
                            {
                                if ((dates.HLTCFEffDate >= firstscDateshltc.HLTCFEffDate && dates.HLTCFEffDate <= firstscDateshltc.HLTCFEndDate) ||
                                (dates.HLTCFEndDate >= firstscDateshltc.HLTCFEffDate && dates.HLTCFEndDate <= firstscDateshltc.HLTCFEndDate))
                                {
                                    strErrors.Append(string.Format("<a  onclick='SetBackgroundColor(\"gvHospiceHLTCFProviderService\")' Style='color: red;'>Effective date or End date  should not fall in between previously entered effective end date for BenfitPeriod Line No's : {0}.</a> <br/>", benId));
                                    break;
                                }
                            }
                            firstscDateshltc = dates;
                        }
                    }
                }
                // Disenrollment validations
                var latestBenperiod = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                if (latestBenperiod != null && txtDisenrollment.Text != "")
                {
                    if (Convert.ToDateTime(txtDisenrollment.Text) < latestBenperiod.BenPeriodEffDate || Convert.ToDateTime(txtDisenrollment.Text) > latestBenperiod.BenPeriodEndDate)
                    {
                        strErrors.Append("<a  onclick='SetBackgroundColor(\"txtDisenrollment\")' Style='color: red;'>Disenrollment date must be within most recent benefit period span.</a> <br/>");
                    }
                }

                if (!string.IsNullOrEmpty(strErrors.ToString()))
                {
                    divPortalErrors.InnerHtml = strErrors.ToString();
                    return;
                }
            }

            bool isAttachementFailed = false;
            MappEnrollAndDisenrollDates();
            MapProviderNamewithProviderId();
            //MapCountryNametoCountyID();
            var benlastReason = inquireResponse.Payload.BenefitPeriods != null ? inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).Select(y => y.BenUpdateReason).FirstOrDefault() : string.Empty;
            //When the reason for update is disenrollment date removal and user does not remove disenrollment date, PNM system should remove the disenrollment date
            if (benlastReason == "010")
            {
                txtDisenrollment.Text = "";
                hdnDisenrollment.Value = "";
                if (inquireResponse.Payload.ElectionDisenrollDates != null)
                {
                    inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate = DateTime.MinValue;
                    inquireResponse.Payload.ElectionDisenrollDates.DisenrollDateSpecified = false;
                }
            }
            inquireResponse.Payload.ActionType = ddlHospiceApplicationType.SelectedValue;
            if (this.WorkflowPage.PreviousProviderHospiceRequestResponse != null &&
                this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload != null &&
                this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload.ProvService != null && this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload.ProvService != null)
            {
                IsDifferentProvider = this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload.ProvService.ToList().Where(x => x.HospiceProvID != this.WorkflowPage.MedicaidID).Count() > 0;
            }

            //In a CHOP scenario where recipient has consumed all days in the last benefit period with provider 1 and did a new enrollment with provider 2 with continuation of new benefit period we should not send any data related to provider 1 benefit period.
            if (IsDifferentProvider)
            {
                var benPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods;
                if (benPeriods != null && benPeriods.Where(x => x.IsDifferentProdvider == true).FirstOrDefault() != null)
                {
                    inquireResponse.Payload.BenefitPeriods = benPeriods.Where(x => x.IsDifferentProdvider == false).ToArray();
                }
            }

            if (txtDisenrollment.Text != "")
            {
                var latestBenperiod = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                if (Convert.ToDateTime(txtDisenrollment.Text) < latestBenperiod.BenPeriodEndDate)
                {
                    latestBenperiod.BenPeriodEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                    if (inquireResponse.Payload.ServiceCountyState != null)
                    {
                        var serviceCountyStates = inquireResponse.Payload.ServiceCountyState.Where(x => x.BenPeriod == latestBenperiod.BenPeriod).ToArray();
                        var serviceCountyStatesCount = serviceCountyStates.Count();
                        var serviceCountyStatesFilter = new List<HospiceRequestResponsePayloadServiceCountyState>();
                        if (serviceCountyStatesCount > 0)
                        {
                            var latestserviceCountyState = serviceCountyStates.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                            if (Convert.ToDateTime(txtDisenrollment.Text) <= latestserviceCountyState.CountyEndDate && serviceCountyStatesCount == 1)
                            {
                                serviceCountyStates[0].CountyEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                                serviceCountyStatesFilter.Add(serviceCountyStates[0]);
                            }
                            else if (serviceCountyStatesCount > 1)
                            {
                                for (int i = 0; i <= serviceCountyStatesCount - 1; i++)
                                {
                                    if (Convert.ToDateTime(txtDisenrollment.Text) <= serviceCountyStates[i].CountyEndDate)
                                    {
                                        serviceCountyStates[i].CountyEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                                        serviceCountyStatesFilter.Add(serviceCountyStates[i]);
                                        break;
                                    }
                                    else
                                    {
                                        serviceCountyStatesFilter.Add(serviceCountyStates[i]);
                                    }
                                }
                            }
                            var otherserviceCountyStates = inquireResponse.Payload.ServiceCountyState.Where(x => x.BenPeriod != latestBenperiod.BenPeriod).ToList();
                            if (otherserviceCountyStates != null && otherserviceCountyStates.Count > 0)
                            {
                                serviceCountyStatesFilter.AddRange(otherserviceCountyStates);
                            }
                            inquireResponse.Payload.ServiceCountyState = serviceCountyStatesFilter.OrderBy(x => x.BenPeriod).ThenBy(y => y.CountyEffDate).ToArray();
                        }
                    }
                    if (inquireResponse.Payload.DiagnosisCodes != null)
                    {
                        var diagnosisCodes = inquireResponse.Payload.DiagnosisCodes.Where(x => x.BenPeriod == latestBenperiod.BenPeriod).ToArray();
                        var diagnosisCodesCount = diagnosisCodes.Count();
                        var diagnosisCodesFilter = new List<HospiceRequestResponsePayloadDiagnosisCodes>();
                        if (diagnosisCodesCount > 0)
                        {
                            var latestdiagnosisCodes = diagnosisCodes.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                            if (Convert.ToDateTime(txtDisenrollment.Text) <= latestdiagnosisCodes.DiagEndDate && diagnosisCodesCount == 1)
                            {
                                diagnosisCodes[0].DiagEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                                diagnosisCodesFilter.Add(diagnosisCodes[0]);
                            }
                            else if (diagnosisCodesCount > 1)
                            {
                                for (int i = 0; i <= diagnosisCodesCount - 1; i++)
                                {
                                    if (Convert.ToDateTime(txtDisenrollment.Text) <= diagnosisCodes[i].DiagEndDate)
                                    {
                                        diagnosisCodes[i].DiagEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                                        diagnosisCodesFilter.Add(diagnosisCodes[i]);
                                        break;
                                    }
                                    else
                                    {
                                        diagnosisCodesFilter.Add(diagnosisCodes[i]);
                                    }
                                }
                            }
                            var otherDiagnosisCodes = inquireResponse.Payload.DiagnosisCodes.Where(x => x.BenPeriod != latestBenperiod.BenPeriod).ToList();
                            if (otherDiagnosisCodes != null && otherDiagnosisCodes.Count > 0)
                            {
                                diagnosisCodesFilter.AddRange(otherDiagnosisCodes);
                            }
                            inquireResponse.Payload.DiagnosisCodes = diagnosisCodesFilter.OrderBy(x => x.BenPeriod).ThenBy(y => y.DiagEffDate).ToArray();
                        }
                    }
                    if (inquireResponse.Payload.ProvService != null)
                    {
                        var provService = inquireResponse.Payload.ProvService.Where(x => x.BenPeriod == latestBenperiod.BenPeriod).ToArray();
                        var provServiceCount = provService.Count();
                        var provServiceFilter = new List<HospiceRequestResponsePayloadProvService>();
                        if (provServiceCount > 0)
                        {
                            var latestProvService = provService.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                            if (Convert.ToDateTime(txtDisenrollment.Text) <= latestProvService.SpanEndDate && provServiceCount == 1)
                            {
                                provService[0].SpanEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                                provServiceFilter.Add(provService[0]);
                            }
                            var otherProvService = inquireResponse.Payload.ProvService.Where(x => x.BenPeriod != latestBenperiod.BenPeriod).ToList();
                            if (otherProvService != null && otherProvService.Count > 0)
                            {
                                provServiceFilter.AddRange(otherProvService);
                            }
                            inquireResponse.Payload.ProvService = provServiceFilter.OrderBy(x => x.BenPeriod).ToArray();
                        }
                    }
                    if (inquireResponse.Payload.LongTermCareFacility != null)
                    {
                        var longTermCareFacility = inquireResponse.Payload.LongTermCareFacility.Where(x => x.BenPeriod == latestBenperiod.BenPeriod).ToArray();
                        var longTermCareFacilityCount = longTermCareFacility.Count();
                        var longTermCareFacilityFilter = new List<HospiceRequestResponsePayloadLongTermCareFacility>();
                        if (longTermCareFacilityCount > 0)
                        {
                            var latestProvService = longTermCareFacility.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                            if (Convert.ToDateTime(txtDisenrollment.Text) <= latestProvService.HLTCFEndDate && longTermCareFacilityCount == 1)
                            {
                                longTermCareFacility[0].HLTCFEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                                longTermCareFacilityFilter.Add(longTermCareFacility[0]);
                            }
                            else if (longTermCareFacilityCount > 1)
                            {
                                for (int i = 0; i <= longTermCareFacilityCount - 1; i++)
                                {
                                    if (Convert.ToDateTime(txtDisenrollment.Text) <= longTermCareFacility[i].HLTCFEndDate)
                                    {
                                        longTermCareFacility[i].HLTCFEndDate = Convert.ToDateTime(txtDisenrollment.Text);
                                        longTermCareFacilityFilter.Add(longTermCareFacility[i]);
                                        break;
                                    }
                                    else
                                    {
                                        longTermCareFacilityFilter.Add(longTermCareFacility[i]);
                                    }
                                }
                            }
                            var otherLongTermCareFacility = inquireResponse.Payload.LongTermCareFacility.Where(x => x.BenPeriod != latestBenperiod.BenPeriod).ToList();
                            if (otherLongTermCareFacility != null && otherLongTermCareFacility.Count > 0)
                            {
                                longTermCareFacilityFilter.AddRange(otherLongTermCareFacility);
                            }
                            inquireResponse.Payload.LongTermCareFacility = longTermCareFacilityFilter.OrderBy(x => x.BenPeriod).ThenBy(y => y.HLTCFEffDate).ToArray();
                        }
                    }
                }
            }
            //When provider or PNM system closes the current provider service span, this end should be aligned with end date of the provider service span.
            if (inquireResponse.Payload.ActionType == "CLSPR" && this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility != null && this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility.Count() > 0)
            {
                var latestLongTermCareFacility = inquireResponse.Payload.LongTermCareFacility.OrderBy(x => x.BenPeriod).LastOrDefault();
                var latestProvService = inquireResponse.Payload.ProvService.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                latestLongTermCareFacility.HLTCFEndDate = latestProvService.SpanEndDate;
            }
            //Close Current service span if previous provder does not close
            var isOverride = ChangeofProviderMapping();
            bool isexception = false;
            if (isOverride)
            {
                var preHospiceInquire = this.WorkflowPage.PreviousProviderHospiceRequestResponse;
                if (preHospiceInquire != null && preHospiceInquire.Payload != null && preHospiceInquire.Payload.ActionType != "CLSPR" && ddlHospiceApplicationType.SelectedValue == "CHGPR")
                {
                    preHospiceInquire.Payload.ActionType = "CLSPR";
                    //When provider or PNM system closes the current provider service span, this end should be aligned with end date of the provider service span.
                    if (preHospiceInquire.Payload.ActionType == "CLSPR" && preHospiceInquire.Payload.LongTermCareFacility != null && preHospiceInquire.Payload.LongTermCareFacility.Count() > 0)
                    {
                        var latestLongTermCareFacility = preHospiceInquire.Payload.LongTermCareFacility.OrderBy(x => x.BenPeriod).LastOrDefault();
                        var latestProvService = preHospiceInquire.Payload.ProvService.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                        latestLongTermCareFacility.HLTCFEndDate = latestProvService.SpanEndDate;
                    }

                    var hospiceAddUpdateResponse = AddUpdateHospice(preHospiceInquire, out isexception);
                    if (isexception)
                    {
                        return;
                    }
                    if (hospiceAddUpdateResponse.ResponseHeader.ResponseType == AddUpdateResponseResponseHeaderResponseType.Failure)
                    {
                        if (hospiceAddUpdateResponse.Errors != null && hospiceAddUpdateResponse.Errors.Count() > 0)
                        {
                            lblMessage.Text = "Errors : Close Current service span if previous provder does not close :";
                            string strErrors = string.Join(",", hospiceAddUpdateResponse.Errors.Select(x => x.ErrorCode + "-" + x.ErrorDesc).ToList());
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "DisplayPortalErrors('" + strErrors + "')", true);
                        }
                        else
                        {
                            lblMessage.Text = "Error : Close Current service span if previous provder does not close :" + hospiceAddUpdateResponse.ResponseHeader.ResponseMessage;
                        }
                        return;
                    }
                }
            }

            ///OHPNM-17331
            if (inquireResponse.Payload.ActionType == "NEWEN" && inquireResponse.Payload.BenefitPeriods != null && inquireResponse.Payload.BenefitPeriods.Any(x => x.Status == "C"))
            {
                inquireResponse.Payload.BenefitPeriods = inquireResponse.Payload.BenefitPeriods != null ? inquireResponse.Payload.BenefitPeriods.Where(x => x.Status.Equals("C")).ToArray() : null;
                inquireResponse.Payload.ServiceCountyState = inquireResponse.Payload.ServiceCountyState != null ? inquireResponse.Payload.ServiceCountyState.Where(x => inquireResponse.Payload.BenefitPeriods[0].BenPeriod == x.BenPeriod).ToArray() : new List<HospiceRequestResponsePayloadServiceCountyState>().ToArray();
                inquireResponse.Payload.LongTermCareFacility = inquireResponse.Payload.LongTermCareFacility != null ? inquireResponse.Payload.LongTermCareFacility.Where(x => inquireResponse.Payload.BenefitPeriods[0].BenPeriod == x.BenPeriod).ToArray() : null;
                inquireResponse.Payload.DiagnosisCodes = inquireResponse.Payload.DiagnosisCodes != null ? inquireResponse.Payload.DiagnosisCodes.Where(x => inquireResponse.Payload.BenefitPeriods[0].BenPeriod == x.BenPeriod).ToArray() : null;
                inquireResponse.Payload.OtherPayerInfo = inquireResponse.Payload.OtherPayerInfo != null ? inquireResponse.Payload.OtherPayerInfo.Where(x => inquireResponse.Payload.BenefitPeriods[0].BenPeriod == x.BenPeriod).ToArray() : null;
                inquireResponse.Payload.ProvService = inquireResponse.Payload.ProvService != null ? inquireResponse.Payload.ProvService.Where(x => inquireResponse.Payload.BenefitPeriods[0].BenPeriod == x.BenPeriod).ToArray() : null;
                inquireResponse.Payload.Attachments = inquireResponse.Payload.Attachments != null ? inquireResponse.Payload.Attachments.Where(x => inquireResponse.Payload.BenefitPeriods[0].BenPeriod == x.BenPeriod).ToArray() : null;
                inquireResponse.Payload.HospiceTrackNo = 0;
                inquireResponse.Payload.HospiceTrackNoSpecified = false;
                inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate = DateTime.MinValue;
                inquireResponse.Payload.ElectionDisenrollDates.DisenrollDateSpecified = false;
            }

            //OHPNM-19069
            if (inquireResponse.Payload.BenefitPeriods == null && ddlHospiceApplicationType.SelectedValue == "CHGPR")
            {
                CreateAndReturnLogInfoThreadNumber("Benefit Periods are null for the application type Change of Provider");
                var benefitPeriods = uc3HospiceBenefitPeriod.GetBenefitPeriodsGridData();

                if (benefitPeriods != null)
                    inquireResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
            }

            var hospiceResponse = AddUpdateHospice(inquireResponse, out isexception);
            if (isexception)
            {
                CreateAndReturnLogInfoThreadNumber("An error has occurred while submitting Hospice Enrollment");
                return;
            }
            ////Once hospic application inserted or updated sucessfully we need to save data to document_index with IndexId
            ////loop through attachment Hospice AddorUpdade response and insert data into document_index table
            var hospiceattachments = this.WorkflowPage.HospicAttachments;
            var attachments = new List<SendAttachment>();
            if (hospiceResponse.ResponseHeader.ResponseType == AddUpdateResponseResponseHeaderResponseType.Success)
            {
                if (hospiceResponse != null && hospiceResponse.Response != null && hospiceResponse.Response.Count() > 0)
                {
                    var hospiceRes = hospiceResponse.Response[0];
                    if (hospiceRes.Attachments != null && hospiceRes.Attachments.Count() > 0)
                    {
                        foreach (var item in hospiceattachments)
                        {
                            int docId = item.Key;
                            var attachment = hospiceattachments[docId];
                            var attRes = hospiceRes.Attachments.ToList().Where(x => x.BenPeriod == attachment.BenPeriod && x.AttachType == attachment.DocumentType).FirstOrDefault();
                            if (attRes != null)
                            {
                                //Will Add loop here once get hospice complete wsdl file
                                RegistrationController.InsertDocumentIndex(docId, attRes.AttachType, attRes.AttachID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                                List<SendAttachmentData> identifiers = new List<SendAttachmentData>();
                                identifiers.Add(new SendAttachmentData
                                {
                                    DocXrefType = "HID",
                                    IndexId = hospiceRes.HospiceTrackNo.ToString()
                                });
                                identifiers.Add(new SendAttachmentData
                                {
                                    DocXrefType = "RID",
                                    IndexId = inquireResponse.Payload.RecipID
                                });
                                identifiers.Add(new SendAttachmentData
                                {
                                    DocXrefType = "HAD",
                                    IndexId = attRes.AttachID
                                });
                                attachment.Identifiers = identifiers.ToArray();
                                attachments.Add(attachment);
                            }
                        }

                        if (attachments.Any())
                        {
                            try
                            {
                                //Call SI document service
                                var response = SendAttachments(attachments);
                                if (response.Response != null && response.Response.Count() > 0 && (response.Response[0].ResponseType.ToLower() == "success" || response.Response[0].ResponseType.ToUpper() == "OK" || response.Response[0].ResponseType.ToLower() == "ok"))
                                {
                                    isAttachementFailed = false;
                                }
                                else
                                {
                                    isAttachementFailed = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                isAttachementFailed = true;
                                CreateAndReturnLogThreadNumber(ex);
                            }
                        }
                    }
                    if (hospiceRes != null && hospiceRes.HospiceTrackNo != 0)
                    {
                        lblHospiceTrackingNumber.Text = hospiceRes.HospiceTrackNo.ToString();
                        lblDateofApplicationUpdate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                }
                if (hospiceResponse.Errors != null && hospiceResponse.Errors.Count() > 0)
                {
                    string errorMessage = GetAddUpdateHospiseResposeErrors(hospiceResponse);
                    divPortalErrors.InnerHtml = errorMessage;
                    CreateAndReturnLogInfoThreadNumber(errorMessage);
                    return;
                }
                else
                {
                    if (isAttachementFailed)
                    {
                        lblMessage.Text = "Hospice application is successfully submitted. Attachment submission failed. Please contact Maximus Integrated Help Desk at 1-800-686-1516.";
                        CreateAndReturnLogInfoThreadNumber(lblMessage.Text);
                    }
                    else
                    {
                        lblMessage.Text = "Hospice application is successfully submitted.";
                    }
                    this.WorkflowPage.PreviousProviderHospiceRequestResponse = null;
                }
            }
            else
            {
                if (hospiceResponse.Errors != null && hospiceResponse.Errors.Count() > 0)
                {
                    string errorMessage = GetAddUpdateHospiseResposeErrors(hospiceResponse);
                    divPortalErrors.InnerHtml = errorMessage;
                    CreateAndReturnLogInfoThreadNumber(errorMessage);
                }
                else
                {
                    lblMessage.Text = hospiceResponse.ResponseHeader.ResponseMessage;
                }
            }

        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex);
            CreateAndReturnLogInfoThreadNumber("An error has occurred while submitting Hospice Enrollment");
            throw;
        }
    }

    private string GetAddUpdateHospiseResposeErrors(AddUpdateHospiceResponse hospiceResponse)
    {
        StringBuilder strErrors = new StringBuilder();

        try
        {
            foreach (var error in hospiceResponse.Errors)
            {
                strErrors.Append("<span Style='color: red;'>" + error.ErrorCode + " - " + error.ErrorDesc + "</span> <br/>");
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return strErrors.ToString();
    }

    public void btnHospiceCancel_Click(object sender, EventArgs e)
    {
        var cancelHospice = CancelHospiceClick;
        if (cancelHospice != null)
            cancelHospice();
    }
    protected Corp.Core.Libraries.HospiceReference.AddUpdateHospiceResponse AddUpdateHospice(AddUpdateHospiceRequest hospiceRes, out bool isException)
    {
        DataSet dshospice = new DataSet();
        isException = false;
        try
        {
            //string sitTransactionKey = Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            var subsriberSystem = new List<HospiceRequestResponseMessageHeaderSubscriber>();
            subsriberSystem.Add(HospiceRequestResponseMessageHeaderSubscriber.FI);
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            HospiceRequestResponseMessageHeader msgHeader = new HospiceRequestResponseMessageHeader();
            msgHeader.BusinessFlow = "AddUpdateHospice";
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = HospiceRequestResponseMessageHeaderRequestorSystem.PNM;
            msgHeader.SubscriberSystem = subsriberSystem.ToArray();
            msgHeader.ModuleTransactionId = string.Empty;
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            msgHeader.SITransactionKey = sitTransactionKey;
            AddUpdateHospiceRequest AddUpdateRequest = new AddUpdateHospiceRequest
            {
                MessageHeader = msgHeader,
                Payload = hospiceRes.Payload
            };
            var response = HospiceServiceAgent.AddUpdateHospice(AddUpdateRequest, "");
            var xDoc = XDocument.Parse(response);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.XmlResolver = null;
            XmlReader xmlReader = XmlReader.Create(new StringReader(response.ToString()), settings);
            dshospice.ReadXml(xmlReader);
            return ConvertDatasetToHospiceAddUpdateResponse(dshospice);
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

                    var response = ConvertDatasetToHospiceAddUpdateResponse(dshospice);
                    if (response.ResponseHeader.ResponseType == AddUpdateResponseResponseHeaderResponseType.Failure)
                    {
                        lblMessage.Text = response.ResponseHeader.ResponseMessage;
                    }
                }
                else
                {
                    string exception = GetExceptionMessage(ex);
                    lblMessage.Text = exception;
                }
            }
            else
            {
                string exception = GetExceptionMessage(ex);
                lblMessage.Text = exception;
            }
            isException = true;
        }
        catch (Exception ex)
        {
            string exception = GetExceptionMessage(ex);
            lblMessage.Text = exception;
            isException = true;
        }
        return null;
    }
    public Corp.Core.Libraries.HospiceReference.AddUpdateHospiceResponse ConvertDatasetToHospiceAddUpdateResponse(DataSet ds)
    {
        Corp.Core.Libraries.HospiceReference.AddUpdateHospiceResponse response = new Corp.Core.Libraries.HospiceReference.AddUpdateHospiceResponse
        {
            ResponseHeader = (ds.Tables["ResponseHeader"] != null) ? DatatableHelper.ConvertDataTableToList<AddUpdateResponseResponseHeader>
                   (ds.Tables["ResponseHeader"]).FirstOrDefault() : null,
            Errors = (ds.Tables["Errors"] != null) ? DatatableHelper.ConvertDataTableToList<AddUpdateResponseErrors>
                   (ds.Tables["Errors"]).ToArray() : null,
            Response = new List<AddUpdateResponseResponse>
            {
                new AddUpdateResponseResponse
                {
                  Attachments = (ds.Tables["Response"] != null && ds.Tables["Attachments"] != null) ? DatatableHelper.ConvertDataTableToList<AddUpdateResponseResponseAttachments>
                   (ds.Tables["Attachments"]).ToArray() : null,
                  BenefitPeriods =(ds.Tables["Response"] != null && ds.Tables["BenefitPeriods"] != null) ? DatatableHelper.ConvertDataTableToList<AddUpdateResponseResponseBenefitPeriods>
                   (ds.Tables["BenefitPeriods"]).ToArray() : null,
                  HospiceTrackNo =(ds.Tables["Response"] != null ) ?Convert.ToInt32(ds.Tables["Response"].Rows[0]["HospiceTrackNo"]) : 0
                }
            }.ToArray()
        };

        return response;
    }

    private sendAttachmentResponse SendAttachments(List<SendAttachment> attachments)
    {
        DataSet dshospice = new DataSet();
        try
        {
            List<InqMessageHeaderSubscriber> inqMessageHeaderSubscriber = new List<InqMessageHeaderSubscriber>();
            inqMessageHeaderSubscriber.Add(InqMessageHeaderSubscriber.MITS);
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            MessageHeader msgHeader = new MessageHeader();
            msgHeader.BusinessFlow = InqMessageHeaderBusinessFlow.sendAttachment;
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = InqMessageHeaderRequestorSystem.PNM;
            msgHeader.SubscriberSystem = inqMessageHeaderSubscriber.ToArray();
            msgHeader.ModuleTransactionId = string.Empty;
            msgHeader.AdditionalModuleTransactionId = string.Empty;
            msgHeader.RequestTimestamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
            msgHeader.SITransactionKey = sitTransactionKey;

            sendAttachmentRequest sendAttachment = new sendAttachmentRequest
            {
                MessageHeader = msgHeader,
                Payload = new SendAttachmentPayload
                {
                    AttachmentInfo = new SendAttachmentInformation
                    {
                        AttachmentData = attachments.ToArray(),
                        SourceId = "PNM"
                    }
                }
            };
            var response = DocumentServiceAgent.SendAttachmentRequest(sendAttachment, "");
            var xDoc = XDocument.Parse(response);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.XmlResolver = null;
            XmlReader xmlReader = XmlReader.Create(new StringReader(response.ToString()), settings);
            dshospice.ReadXml(xmlReader);
            return ConvertDatasetToSendAttachmentResponse(dshospice);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private bool ChangeofProviderMapping()
    {
        bool isOverlap = false;
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        var inquireResponsePrev = this.WorkflowPage.PreviousProviderHospiceRequestResponse;
        if ((inquireResponse != null
           && inquireResponse.Payload != null
           && inquireResponse.Payload.ProvService != null
           && inquireResponse.Payload.ProvService.Count() > 0) && (inquireResponsePrev != null
           && inquireResponsePrev.Payload != null
           && inquireResponsePrev.Payload.ProvService != null
           && inquireResponsePrev.Payload.ProvService.Count() > 0)
           )
        {
            var lastBenId = inquireResponse.Payload.ProvService.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
            var lastBenIdPrev = inquireResponsePrev.Payload.ProvService.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
            if ((lastBenId.SpanEffDate >= lastBenIdPrev.SpanEffDate && lastBenId.SpanEffDate <= lastBenIdPrev.SpanEndDate) || (lastBenId.SpanEndDate >= lastBenIdPrev.SpanEffDate && lastBenId.SpanEndDate <= lastBenIdPrev.SpanEndDate))
            {
                lastBenIdPrev.SpanEndDate = lastBenId.SpanEffDate.AddDays(-1);
                //var inquirereProv = inquireResponse.Payload.ProvService.ToList();
                //inquirereProv.AddRange(inquireResponsePrev.Payload.ProvService.ToList());
                //inquireResponse.Payload.ProvService = inquirereProv.ToArray();
                isOverlap = true;
            }
        }
        return isOverlap;
    }
    public sendAttachmentResponse ConvertDatasetToSendAttachmentResponse(DataSet ds)
    {
        sendAttachmentResponse response = new sendAttachmentResponse
        {
            Response = (ds.Tables["Response"] != null) ? DatatableHelper.ConvertDataTableToList<AttachmentResponse>
                   (ds.Tables["Response"]).ToArray() : null
        };

        return response;
    }

    private void MappEnrollAndDisenrollDates()
    {
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.ElectionDisenrollDates != null)
        {
            if (!string.IsNullOrEmpty(txtElectionDate.Text))
                inquireResponse.Payload.ElectionDisenrollDates.ElectionDate = Convert.ToDateTime(txtElectionDate.Text);

            if (!string.IsNullOrEmpty(txtDisenrollment.Text))
            {
                inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate = Convert.ToDateTime(txtDisenrollment.Text);
                inquireResponse.Payload.ElectionDisenrollDates.DisenrollDateSpecified = true;
            }
            else if (!string.IsNullOrEmpty(hdnDisenrollment.Value))
            {
                inquireResponse.Payload.ElectionDisenrollDates.DisenrollDate = Convert.ToDateTime(hdnDisenrollment.Value);
                inquireResponse.Payload.ElectionDisenrollDates.DisenrollDateSpecified = true;
            }
        }
        else
        {
            HospiceRequestResponsePayloadElectionDisenrollDates electionDisenrollDate = new HospiceRequestResponsePayloadElectionDisenrollDates();
            if (!string.IsNullOrEmpty(txtElectionDate.Text))
                electionDisenrollDate.ElectionDate = Convert.ToDateTime(txtElectionDate.Text);

            if (!string.IsNullOrEmpty(txtDisenrollment.Text))
            {
                electionDisenrollDate.DisenrollDate = Convert.ToDateTime(txtDisenrollment.Text);
                electionDisenrollDate.DisenrollDateSpecified = true;
            }
            else if (!string.IsNullOrEmpty(hdnDisenrollment.Value))
            {
                electionDisenrollDate.DisenrollDate = Convert.ToDateTime(hdnDisenrollment.Value);
                electionDisenrollDate.DisenrollDateSpecified = true;
            }
            //var electionDisenrollDates = new List<HospiceRequestResponsePayloadElectionDisenrollDates>();
            //electionDisenrollDates.Add(electionDisenrollDate);
            this.WorkflowPage.HospiceRequestResponse.Payload.ElectionDisenrollDates = electionDisenrollDate;
        }
    }
    private void MapCountryNametoCountyID()
    {
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
           && inquireResponse.Payload != null
           && inquireResponse.Payload.ServiceCountyState != null
           && inquireResponse.Payload.ServiceCountyState.Count() > 0)
        {
            foreach (var item in inquireResponse.Payload.ServiceCountyState)
            {
                var countyId = GetHospiceCountyId(item.State, item.County);
                item.County = (countyId == null) ? item.County.Substring(item.County.Length - 2) : countyId;
            }
        }
    }
    private void MapProviderNamewithProviderId()
    {
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
           && inquireResponse.Payload != null
           && inquireResponse.Payload.ProvService != null
           && inquireResponse.Payload.ProvService.Count() > 0)
        {
            foreach (var item in inquireResponse.Payload.ProvService)
            {
                item.HospiceProvID = this.WorkflowPage.MedicaidID;
            }
        }
    }

    private string GetHospiceCountyId(string state, string selectedCounty)
    {
        var ds = new DataSet();
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            ds = psc.SelectCounty(state);
            var dcountyId = (from q in ds.Tables[0].AsEnumerable()
                             where q.Field<string>("COUNTY_NAME") == selectedCounty
                             select q.Field<string>("MMIS_COUNTY_CODE")).FirstOrDefault();
            return Convert.ToString(dcountyId);

        }
    }
    private void LoadHospiceApplicationActionType()
    {
        Helper.LoadDropDown(ddlHospiceApplicationType, GetHospiceApplicationActionType().Tables[0], "ACTION_TYPE_DESC", "ACTION_TYPE_CODE", true);
    }
    private bool FilterApplicationActionType()
    {
        bool isAllownewbenPeriod = true;
        if (!string.IsNullOrEmpty(this.HospiceTrackNo) && this.HospiceTrackNo != "0")
        {
            if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "CLSPR" && this.IsDifferentProvider && !this.IsProviderCoveredAllDays)
            {
                RemoveItemsFromApplicationType("NEWEN,MAINT,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "CLSPR" && this.IsDifferentProvider && this.IsProviderCoveredAllDays)
            {
                RemoveItemsFromApplicationType("MAINT,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = true;
            }
            else if (this.Status == "P" && (ddlHospiceApplicationType.SelectedValue == "NEWEN" || ddlHospiceApplicationType.SelectedValue == "MAINT" || ddlHospiceApplicationType.SelectedValue == "CHGPR") && this.IsDifferentProvider)
            {
                if (ddlHospiceApplicationType.SelectedValue == "NEWEN" || ddlHospiceApplicationType.SelectedValue == "MAINT")
                {
                    RemoveItemsFromApplicationType("MAINT,CLSPR,REVOC,BFTRM");
                }

                if (ddlHospiceApplicationType.SelectedValue == "NEWEN")
                {
                    isAllownewbenPeriod = true;
                }
                if (ddlHospiceApplicationType.SelectedValue == "CHGPR")
                {
                    isAllownewbenPeriod = false;
                }
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "MAINT" && this.IsDifferentProvider && !this.IsProviderCoveredAllDays)
            {
                RemoveItemsFromApplicationType("NEWEN,MAINT,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "MAINT" && this.IsDifferentProvider && this.IsProviderCoveredAllDays)
            {
                RemoveItemsFromApplicationType("CHGPR,MAINT,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = true;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "MAINT" && (this.Reason_LastBefit == "Data correction"
                || this.Reason_LastBefit == "Alignment with Medicare spans"
                || this.Reason_LastBefit == "Removal of disenrollment date" || string.IsNullOrEmpty(this.Reason_LastBefit)))
            {
                RemoveItemsFromApplicationType("CHGPR,NEWEN");
                isAllownewbenPeriod = true;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "MAINT" && (this.Reason_LastBefit == "Death"))
            {
                RemoveItemsFromApplicationType("NEWEN,CHGPR,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "MAINT" && (this.Reason_LastBefit == "Data correction"
               || this.Reason_LastBefit == "Alignment with Medicare spans"
                || this.Reason_LastBefit == "Removal of disenrollment date"))
            {
                RemoveItemsFromApplicationType("NEWEN,CHGPR,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = true;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "MAINT" && (this.Reason_LastBefit == "Individual no longer meets the enrollment criteria"
                || this.Reason_LastBefit == "Individual is no longer terminally ill"
                || this.Reason_LastBefit == "Individual moved out the service area"
                || this.Reason_LastBefit == "Individual entered a non-contracted facility"
                || this.Reason_LastBefit == "Individual revoked the Medicaid hospice benefit"
                || this.Reason_LastBefit == "For cause"))
            {
                RemoveItemsFromApplicationType("CHGPR,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "CHGPR" && (string.IsNullOrEmpty(this.Reason_LastBefit)))
            {
                RemoveItemsFromApplicationType("CHGPR,NEWEN");
                isAllownewbenPeriod = true;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "NEWEN" && (string.IsNullOrEmpty(this.Reason_LastBefit)))
            {
                RemoveItemsFromApplicationType("CHGPR,NEWEN");
                isAllownewbenPeriod = true;
            }
            else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "CLSPR" && (string.IsNullOrEmpty(this.Reason_LastBefit)))
            {
                RemoveItemsFromApplicationType("NEWEN,CHGPR,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            else if (this.Status == "P" && (ddlHospiceApplicationType.SelectedValue == "REVOC" || ddlHospiceApplicationType.SelectedValue == "BFTRM"))
            {
                RemoveItemsFromApplicationType("CHGPR,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            //else if (this.Status == "P" && ddlHospiceApplicationType.SelectedValue == "MAINT" && (this.Reason_LastBefit == "Data correction"
            //   || this.Reason_LastBefit == "Alignment with Medicare spans"
            //    || this.Reason_LastBefit == "Removal of disenrollment date"))
            //{
            //    RemoveItemsFromApplicationType("NEWEN,CHGPR,CLSPR,REVOC,BFTRM");
            //}
            else if (this.Status == "C" && this.Reason_LastBefit != "Death")
            {
                RemoveItemsFromApplicationType("CHGPR,CLSPR,REVOC,BFTRM");
            }
            else if (this.Status == "P" && this.Reason_LastBefit == "Death")
            {
                RemoveItemsFromApplicationType("NEWEN,CHGPR,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            else if (this.Status == "D")
            {
                RemoveItemsFromApplicationType("MAINT,CHGPR,CLSPR,REVOC,BFTRM");
                isAllownewbenPeriod = false;
            }
            else if (this.Status == "C")
            {
                RemoveItemsFromApplicationType("NEWEN,MAINT");
                isAllownewbenPeriod = false;
            }
        }
        else
        {
            RemoveItemsFromApplicationType("MAINT,CLSPR,REVOC,BFTRM");
        }
        return isAllownewbenPeriod;
    }

    private void RemoveItemsFromApplicationType(string values)
    {
        foreach (var item in values.Split(','))
        {
            ddlHospiceApplicationType.Items.Remove(ddlHospiceApplicationType.Items.FindByValue(item));
        }
    }

    public class StatusDetails
    {
        public string ICD10Diag { get; set; }
        public string ICDVersion { get; set; }
        public string DiagDesc { get; set; }
    }
    private DataSet GetHospiceApplicationActionType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospiceApplicationActionType();
        }
    }

    public string CreateAndReturnLogThreadNumber(Exception ex, string errorKey = "", [CallerMemberName] string callingMethodName = "")
    {
        string logid = HttpContext.Current.Session["LogKey"] != null ? HttpContext.Current.Session["LogKey"].ToString() : CON.appAdminUserId;
        Logging logging = new Logging(new Guid(logid));
        string logMessage = errorKey + " " + logging.GetRecursiveException(ex);
        logging.CreateLogEntry(logMessage, this.GetType().Name + "/" + callingMethodName);
        return logging.ThreadId.ToString();
    }

    public string CreateAndReturnLogInfoThreadNumber(string logMessage = "", [CallerMemberName] string callingMethodName = "")
    {
        string logid = HttpContext.Current.Session["LogKey"] != null ? HttpContext.Current.Session["LogKey"].ToString() : CON.appAdminUserId;
        Logging logging = new Logging(new Guid(logid));
        logging.CreateLogEntry(logMessage, this.GetType().Name + "/" + callingMethodName);
        return logging.ThreadId.ToString();
    }
}
