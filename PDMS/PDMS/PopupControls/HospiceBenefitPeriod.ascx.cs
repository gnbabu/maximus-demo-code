using Corp.Core.Libraries.HospiceReference;
using CustomControls;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HospiceBenefitPeriod : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
        if (this.WorkflowPage.IsHospiceCheckEligibilityClick)
        {
            PopupControls_RecipientEligibilitySearch ucRecipientEligibilitySearch =
              LoadControl("~/PopupControls/RecipientEligibilitySearch.ascx") as PopupControls_RecipientEligibilitySearch;
            ucRecipientEligibilitySearch.IsFromhospice = true;
            HospicePlaceholderInitial.Controls.Add(ucRecipientEligibilitySearch);
            this.WorkflowPage.IsHospiceCheckEligibilityClick = true;

            ShowPopupCheckEligibility.Show();
        }
        hdnMedicaidID.Value = this.WorkflowPage.MedicaidID;
        hdnNPI.Value = this.WorkflowPage.NPI;
    }
    public void BindGrid(string isNew = "")
    {
        try
        {

            if (this.WorkflowPage.PreviousProviderHospiceRequestResponse != null && this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload != null && this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload.ProvService != null && this.WorkflowPage.HospiceSelectedActionType == "")
            {
                if (this.WorkflowPage.PreviousProviderHospiceRequestResponse.Payload.ProvService.ToList().Where(x => x.HospiceProvID != this.WorkflowPage.MedicaidID).Count() > 0)
                {
                    return;
                }
            }
            var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
            if (inquireResponse != null
                && inquireResponse.Payload != null
                && inquireResponse.Payload.BenefitPeriods != null
                && inquireResponse.Payload.BenefitPeriods.Count() > 0)
            {
                this.WorkflowPage.GetHospiceBenifitSegmentIndicatorType();
                gvHospiceBenefitPeriod.DataSource = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
                gvHospiceBenefitPeriod.DataBind();

                HdBenefitPeriodEffDate.Value = Helper.HtmlEncode(inquireResponse.Payload.BenefitPeriods.OrderBy(y => y.BenPeriodEffDate).Select(x => x.BenPeriodEffDate).FirstOrDefault().ToString("MM/dd/yyyy"));
                HdBenefitPeriodEndDate.Value = Helper.HtmlEncode(inquireResponse.Payload.BenefitPeriods.OrderByDescending(y => y.BenPeriodEndDate).Select(x => x.BenPeriodEndDate).FirstOrDefault().ToString("MM/dd/yyyy"));
                //Call BindSegmentIndicatorWithBefetPeriods function
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "BindSegmentIndicatorWithBefetPeriods('" + BindSegmentIndicatorwithBenefitPeriod() + "','gvHospiceAttendingPhysician,gvHospiceIDGPhysician')", true);

                var lastBenefitPeriod = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriodType).FirstOrDefault();
                int maxBenPeriodType = 0;
                if (lastBenefitPeriod != null && this.WorkflowPage.HospiceStatus == "D" && this.WorkflowPage.IsBenefitPeriodAdded == 0)
                {
                    maxBenPeriodType = lastBenefitPeriod.BenPeriodType;
                }
                else
                {
                    maxBenPeriodType = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriodType).Select(y => y.BenPeriodType).FirstOrDefault();
                    maxBenPeriodType = (maxBenPeriodType >= 3) ? 3 : maxBenPeriodType + 1;

                }
                BindBenefitSegmentIndicatorOnFooter(maxBenPeriodType);
            }
            else
            {
                var sources = new List<HospiceRequestResponsePayloadBenefitPeriods>
            {
               new HospiceRequestResponsePayloadBenefitPeriods()
            };
                GridviewShowNoResultFound<HospiceRequestResponsePayloadBenefitPeriods>(sources, gvHospiceBenefitPeriod);
                if (this.WorkflowPage.IsHospiceTerminatedAndNewEnrollment)
                {
                    int lastBenfitNo = this.WorkflowPage.HospiceTerminatedBenefitNumber;

                    if (isNew == "D" || this.WorkflowPage.HospiceStatus == "D")
                    {
                        lastBenfitNo = (lastBenfitNo >= 3) ? 3 : lastBenfitNo;
                    }
                    else
                    {
                        lastBenfitNo = (lastBenfitNo >= 3) ? 3 : lastBenfitNo + 1;
                    }
                    BindBenefitSegmentIndicatorOnFooter(lastBenfitNo);
                }
                else
                {
                    BindBenefitSegmentIndicatorOnFooter(1);
                }
            }

            if (inquireResponse != null
                && inquireResponse.Payload != null
                && inquireResponse.Payload.BenefitPeriods != null
                && inquireResponse.Payload.BenefitPeriods.Where(x => x.PhyNPI != null && x.IsHideBenfitperiodPhy == false).Count() > 0)
            {
                var dt = GetAttendingEmptyDatatable();
                foreach (var benefitPeriod in inquireResponse.Payload.BenefitPeriods.Where(x => x.PhyNPI != null && x.IsHideBenfitperiodPhy == false))
                {
                    DataRow row = dt.NewRow();
                    row["BenPeriod"] = benefitPeriod.BenPeriod;
                    row["PhyNPI"] = benefitPeriod.PhyNPI;
                    row["PhyOralCertDate"] = (benefitPeriod.PhyOralCertDate != DateTime.MinValue) ? benefitPeriod.PhyOralCertDate.ToString("MM/dd/yyyy") : "";
                    row["PhyWritCertDate"] = benefitPeriod.PhyWritCertDate.ToString("MM/dd/yyyy");
                    row["IsFromInquiry"] = benefitPeriod.IsFromInquiry;
                    dt.Rows.Add(row);
                }
                gvHospiceAttendingPhysician.DataSource = dt;
                gvHospiceAttendingPhysician.DataBind();
            }
            else
            {
                var sources = new List<HospiceRequestResponsePayloadBenefitPeriods>
            {
               new HospiceRequestResponsePayloadBenefitPeriods()
            };
                GridviewShowNoResultFound<HospiceRequestResponsePayloadBenefitPeriods>(sources, gvHospiceAttendingPhysician);
            }
            if (inquireResponse != null
                && inquireResponse.Payload != null
                && inquireResponse.Payload.BenefitPeriods != null
                && inquireResponse.Payload.BenefitPeriods.Where(x => x.IDGPhyNPI != null && x.IsHideBenfitperiodIDG == false).Count() > 0)
            {
                var dt = GetIDGEmptyDatatable();
                foreach (var benefitPeriod in inquireResponse.Payload.BenefitPeriods.Where(x => x.IDGPhyNPI != null && x.IsHideBenfitperiodIDG == false))
                {
                    DataRow row = dt.NewRow();
                    row["BenPeriod"] = benefitPeriod.BenPeriod;
                    row["IDGPhyNPI"] = benefitPeriod.IDGPhyNPI;
                    row["IDGPhyOralCertDate"] = (benefitPeriod.IDGPhyOralCertDate != DateTime.MinValue) ? benefitPeriod.IDGPhyOralCertDate.ToString("MM/dd/yyyy") : "";
                    row["IDGPhyWritCertDate"] = benefitPeriod.IDGPhyWritCertDate.ToString("MM/dd/yyyy");
                    row["IsFromInquiry"] = benefitPeriod.IsFromInquiry;
                    dt.Rows.Add(row);
                }
                gvHospiceIDGPhysician.DataSource = dt;
                gvHospiceIDGPhysician.DataBind();
            }
            else
            {
                var sources = new List<HospiceRequestResponsePayloadBenefitPeriods>
            {
               new HospiceRequestResponsePayloadBenefitPeriods()
            };
                GridviewShowNoResultFound<HospiceRequestResponsePayloadBenefitPeriods>(sources, gvHospiceIDGPhysician);
            }
            // bind country in footer row dropdownlist
            BindReasonForUpdateOnFooter();

            BindAttendingBenefitLinenoOnFooter();
            BindIDGBenefitSegmentIndicatorOnFooter();
            BindDummyRow();
            BindhospiceGrids();

        }
        catch (Exception ex) 
        {
            CreateAndReturnLogThreadNumber(ex, "An error has occurred inside BindGrid");
        }
    }

    public List<HospiceRequestResponsePayloadBenefitPeriods> GetBenefitPeriodsGridData()
    {
        return gvHospiceBenefitPeriod.DataSource as List<HospiceRequestResponsePayloadBenefitPeriods>;
    }

    public DataSet SearchProviderNPI(string npi, string medicaidID, string lastName, string firstName)
    {
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                return psc.SearchHospiceProviderNPI(npi, medicaidID, lastName, firstName);
            }
        }
        catch (Exception)
        {
            return null;
        }
    }
    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("NPI");
        dummy.Columns.Add("MEDICAID_ID");
        dummy.Columns.Add("LAST_OR_BUSINESS_NAME");
        dummy.Columns.Add("FIRST_NAME");
        dummy.Rows.Add();
        gvHospiceProviderNPISearch.DataSource = dummy;
        gvHospiceProviderNPISearch.DataBind();
    }
    #region BenfitPeriod
    private void BindReasonForUpdateOnFooter()
    {
        DropDownList fddlReasonForUpdate = gvHospiceBenefitPeriod.FooterRow.FindControl("fddlReasonForUpdate") as DropDownList;
        LoadHospiceReasonUpdateTypeDropDown(fddlReasonForUpdate);
    }

    private void BindBenefitSegmentIndicatorOnFooter(int benLineNo)
    {
        Label fddlBenefitSegmentIndicator = gvHospiceBenefitPeriod.FooterRow.FindControl("fddlBenefitSegmentIndicator") as Label;
        var sigmentIndicatorText = this.WorkflowPage.GetSegmentIndicatorByValue(benLineNo.ToString());
        fddlBenefitSegmentIndicator.Text = Helper.HtmlEncode(sigmentIndicatorText);
    }
    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            TextBox ftxtEndDate = gvHospiceBenefitPeriod.FooterRow.FindControl("ftxtEndDate") as TextBox;
            TextBox ftxtEffectiveDate = gvHospiceBenefitPeriod.FooterRow.FindControl("ftxtEffectiveDate") as TextBox;
            Label fddlBenefitSegmentIndicator = gvHospiceBenefitPeriod.FooterRow.FindControl("fddlBenefitSegmentIndicator") as Label;
            DropDownList fddlReasonForUpdate = gvHospiceBenefitPeriod.FooterRow.FindControl("fddlReasonForUpdate") as DropDownList;
            TextBox ftxtStatus = gvHospiceBenefitPeriod.FooterRow.FindControl("ftxtStatus") as TextBox;

            bool isNPISuspended = false;
            bool isNPITerminated = false;
            //DataSet dsenroll = ValidateifProviderSuspended(hdnNPI.Value, hdnMedicaidID.Value);

            DataSet dsenroll = GetServProvDateOfService(hdnNPI.Value, hdnMedicaidID.Value);
            isNPITerminated = GetProviderTerminatedSpan(dsenroll);

            if (isNPITerminated)
            {
                BFErrorMessage.InnerText = "The Hospice application is initiated by or contains a terminated provider and does not allow Submission.";
                return;
            }
            else
            {
                isNPISuspended = GetProviderSuspendedSpan(dsenroll, ftxtEffectiveDate.Text.Trim(), ftxtEndDate.Text.Trim());
                if (enableCR537 && isNPISuspended)
                {
                    BFErrorMessage.InnerText = "The date of service entered is a suspended span and does not allow Hospice submission.";
                    return;
                }
                else
                {
                    HospiceRequestResponsePayloadBenefitPeriods benefitPeriod = new HospiceRequestResponsePayloadBenefitPeriods
                    {
                        BenPeriodEffDate = Convert.ToDateTime(ftxtEffectiveDate.Text.Trim()),
                        BenPeriodEndDate = Convert.ToDateTime(ftxtEndDate.Text.Trim()),
                        BenUpdateReason = fddlReasonForUpdate.SelectedValue,
                        Status = "C",
                        BenPeriodType = this.WorkflowPage.GetSegmentIndicatorByText(fddlBenefitSegmentIndicator.Text)
                        //SubmissionDate = DateTime.Now.ToString("MM/dd/yyyy")
                    };
                    var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
                    if (inquireResponse != null
                        && inquireResponse.Payload != null
                        && inquireResponse.Payload.BenefitPeriods != null)
                    {
                        //benefitPeriod.BenPeriod = inquireResponse.Payload.BenefitPeriods.Count() + 1;
                        var lastben = inquireResponse.Payload.BenefitPeriods.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
                        benefitPeriod.BenPeriod = lastben.BenPeriod + 1;
                        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
                        benefitPeriods.Add(benefitPeriod);
                        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
                    }
                    else
                    {
                        if (this.WorkflowPage.IsHospiceTerminatedAndNewEnrollment)
                        {
                            int lastBenfitNo = this.WorkflowPage.HospiceTerminatedBenefitNumber;
                            benefitPeriod.BenPeriod = lastBenfitNo + 1;
                        }
                        else
                        {
                            benefitPeriod.BenPeriod = 1;
                        }
                        var benefitPeriods = new List<HospiceRequestResponsePayloadBenefitPeriods>();
                        benefitPeriods.Add(benefitPeriod);
                        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
                    }
                    this.WorkflowPage.IsNewHospiceBenefitPeriod = true;
                    this.WorkflowPage.IsBenefitPeriodAdded = 1;

                    BindGrid();
                    BindhospiceGrids();
                }
            }
        }
        catch (Exception ex)
        {
            BFErrorMessage.InnerText = ex.Message;
            CreateAndReturnLogThreadNumber(ex, "An error has occurred in Benefit Period fbtnAdd_Click:"+ex.Message);
        }
    }
    protected void btndummy_Click(object sender, EventArgs e)
    {

    }
    protected void btnCheckEligibility_Click(object sender, EventArgs e)
    {
        PopupControls_RecipientEligibilitySearch ucRecipientEligibilitySearch =
               LoadControl("~/PopupControls/RecipientEligibilitySearch.ascx") as PopupControls_RecipientEligibilitySearch;
        ucRecipientEligibilitySearch.IsFromhospice = true;
        HospicePlaceholderInitial.Controls.Add(ucRecipientEligibilitySearch);
        this.WorkflowPage.IsHospiceCheckEligibilityClick = true;

        ShowPopupCheckEligibility.Show();
    }
    protected void btnCloseCH_Click(object sender, EventArgs e)
    {
        this.WorkflowPage.IsHospiceCheckEligibilityClick = false;
        ShowPopupCheckEligibility.Hide();
    }
    protected void gvHospiceBenefitPeriod_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBenefitSegmentIndicator = ((Label)e.Row.FindControl("lblBenefitSegmentIndicator"));
            if (lblBenefitSegmentIndicator != null && lblBenefitSegmentIndicator.Text != "0" && lblBenefitSegmentIndicator.Text != "")
            {
                var sigmentIndicatorText = this.WorkflowPage.GetSegmentIndicatorByValue(lblBenefitSegmentIndicator.Text);
                lblBenefitSegmentIndicator.Text = Helper.HtmlEncode(sigmentIndicatorText.ToString());
            }
            Label lblReasonForUpdate = ((Label)e.Row.FindControl("lblReasonForUpdate"));
            if (lblReasonForUpdate != null && lblReasonForUpdate.Text != "")
            {
                int result;
                if (int.TryParse(lblReasonForUpdate.Text, out result))
                {
                    lblReasonForUpdate.Text = Helper.HtmlEncode(GetHospiceReasonUpdateTypeByValue(lblReasonForUpdate.Text));
                }
            }
            Label lblStatus = ((Label)e.Row.FindControl("lblStatus"));
            if (lblStatus != null && lblStatus.Text != "")
            {
                lblStatus.Text = Helper.HtmlEncode(GetStatusDescription(lblStatus.Text));
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Button btnAddnew = ((Button)e.Row.FindControl("ftnAdd"));
            if (this.WorkflowPage.IsNewHospiceBenefitPeriod && btnAddnew != null)
            {
                btnAddnew.Visible = false;
            }
        }
    }
    protected void gvHospiceBenefitPeriod_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblReasonForUpdate = (Label)gvHospiceBenefitPeriod.Rows[e.NewEditIndex].FindControl("lblReasonForUpdate");
        Label lblBenefitSegmentIndicator = (Label)gvHospiceBenefitPeriod.Rows[e.NewEditIndex].FindControl("lblBenefitSegmentIndicator");
        Label lblStatus = (Label)gvHospiceBenefitPeriod.Rows[e.NewEditIndex].FindControl("lblStatus");


        //main code while editing
        gvHospiceBenefitPeriod.EditIndex = e.NewEditIndex;
        BindGrid();
        //main code while editing

        //find the Country DropDownList of EditItemTemplate
        DropDownList eddlReasonForUpdate = (DropDownList)gvHospiceBenefitPeriod.Rows[gvHospiceBenefitPeriod.EditIndex].FindControl("eddlReasonForUpdate");
        LoadHospiceReasonUpdateTypeDropDown(eddlReasonForUpdate);

        if (eddlReasonForUpdate.Items.FindByText(lblReasonForUpdate.Text) != null)
        {
            eddlReasonForUpdate.Items.FindByText(lblReasonForUpdate.Text).Selected = true;
        }
        eddlReasonForUpdate.Enabled = false;
        if (this.WorkflowPage.HospiceSelectedActionType == "MAINT")
        {
            eddlReasonForUpdate.Enabled = true;
        }
        Label eddlBenefitSegmentIndicator = (Label)gvHospiceBenefitPeriod.Rows[gvHospiceBenefitPeriod.EditIndex].FindControl("eddlBenefitSegmentIndicator");
        eddlBenefitSegmentIndicator.Text = Helper.HtmlEncode(lblBenefitSegmentIndicator.Text);
        TextBox etxtStatus = gvHospiceBenefitPeriod.Rows[gvHospiceBenefitPeriod.EditIndex].FindControl("etxtStatus") as TextBox;
        if (lblStatus != null && lblStatus.Text != "" && etxtStatus != null)
        {
            etxtStatus.Text = lblStatus.Text;
        }
    }

    private static string GetStatusDescription(string lblStatus)
    {
        string status;
        switch (lblStatus)
        {
            case "C":
                status = "Complete";
                break;
            case "D":
                status = "Denied";
                break;
            case "P":
                status = "Processed ";
                break;
            default:
                status = "Complete ";
                break;
        };
        return status;
    }
    private DataSet GetServProvDateOfService(string npi, string medicaid)
    {
        DataSet ds = new DataSet();
        try
        {      
                ds = PriorAuthHospitalController.GetServProvDateOfService(npi, medicaid);
                return ds;            
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex, "SubmitPA-GetServiceProviderDateOfService");
            return ds;
        }
        return ds;
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
    private bool GetProviderSuspendedSpan(DataSet dsenroll,string etxtEffectiveDate,string etxtEndDate)
    {
        bool isProviderSpan = false; 
        if (dsenroll != null && dsenroll.Tables.Count > 0 && dsenroll.Tables[1].Rows.Count > 0)
        {
            if (dsenroll != null && dsenroll.Tables.Count > 0)
            {
                if (dsenroll.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < dsenroll.Tables[1].Rows.Count; i++)
                    {
                        if ((Convert.ToDateTime(etxtEffectiveDate.Trim()) >= Convert.ToDateTime(dsenroll.Tables[1].Rows[i][0].ToString())
                            && Convert.ToDateTime(etxtEffectiveDate.Trim()) <= Convert.ToDateTime(dsenroll.Tables[1].Rows[i][1].ToString()))
                            || (Convert.ToDateTime(etxtEndDate.Trim()) >= Convert.ToDateTime(dsenroll.Tables[1].Rows[i][0].ToString())
                            && Convert.ToDateTime(etxtEndDate.Trim()) <= Convert.ToDateTime(dsenroll.Tables[1].Rows[i][1].ToString())))
                        {
                            isProviderSpan = true;
                        }
                    }
                }
            }
        }
        return isProviderSpan;
    }
    private bool GetProviderTerminatedSpan(DataSet dsenroll)
    {
        bool isProviderSpan = false;
        try
        {
            if (dsenroll == null)
            {
                isProviderSpan = true;
            }
            else if (dsenroll.Tables.Count > 0 && dsenroll.Tables[0].Rows.Count == 0 && dsenroll.Tables[1].Rows.Count == 0)
            {
                isProviderSpan = true;
            }
        }
        catch (Exception ex)
        {
            isProviderSpan = false;
            CreateAndReturnLogThreadNumber(ex, "An exception has occurred in GetProviderTerminatedSpan");
        }
                   
        return isProviderSpan;
    }

    protected void gvHospiceBenefitPeriod_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox etxtEndDate = gvHospiceBenefitPeriod.Rows[e.RowIndex].FindControl("etxtEndDate") as TextBox;
        TextBox etxtEffectiveDate = gvHospiceBenefitPeriod.Rows[e.RowIndex].FindControl("etxtEffectiveDate") as TextBox;
        HiddenField hdnReasonForUpdate = (HiddenField)gvHospiceBenefitPeriod.Rows[e.RowIndex].FindControl("hdnReasonForUpdate");
        //HiddenField hdnBenefitSegmentIndicator = (HiddenField)gvHospiceBenefitPeriod.Rows[e.RowIndex].FindControl("hdnBenefitSegmentIndicator");
        TextBox etxtStatus = gvHospiceBenefitPeriod.Rows[e.RowIndex].FindControl("etxtStatus") as TextBox;

        bool isNPISuspended = false;
        bool isNPITerminated = false;
        DataSet dsenroll = GetServProvDateOfService(hdnNPI.Value, hdnMedicaidID.Value);
        isNPITerminated = GetProviderTerminatedSpan(dsenroll);
           
        if (isNPITerminated)
        {
            BFErrorMessage.InnerText = "The Hospice application is initiated by or contains a terminated provider and does not allow Submission.";
            return;
        }
        else
        {
            isNPISuspended = GetProviderSuspendedSpan(dsenroll, etxtEffectiveDate.Text.Trim(), etxtEndDate.Text.Trim());
            if (enableCR537 && isNPISuspended)
            {
                BFErrorMessage.InnerText = "The date of service entered is a suspended span and does not allow Hospice submission.";
                return;
            }
            else
            {
                //Update Data in db

                gvHospiceBenefitPeriod.EditIndex = -1;
                var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
                benefitPeriods[e.RowIndex].Status = etxtStatus.Text.Trim();
                benefitPeriods[e.RowIndex].BenPeriodEffDate = Convert.ToDateTime(etxtEffectiveDate.Text.Trim());
                benefitPeriods[e.RowIndex].BenPeriodEndDate = Convert.ToDateTime(etxtEndDate.Text.Trim());
                benefitPeriods[e.RowIndex].BenUpdateReason = hdnReasonForUpdate.Value;
                benefitPeriods[e.RowIndex].BenPeriodType = benefitPeriods[e.RowIndex].BenPeriodType;
                this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
                if (this.WorkflowPage.HospiceRequestResponse.Payload.ProvService != null)
                {
                    var provServices = this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.ToList();
                    if (provServices.Count > 0 && provServices.Where(x => x.BenPeriod == benefitPeriods[e.RowIndex].BenPeriod).FirstOrDefault() != null)
                    {
                        var providerSrvSpan = provServices.Where(x => x.BenPeriod == benefitPeriods[e.RowIndex].BenPeriod).FirstOrDefault();
                        providerSrvSpan.SpanEffDate = Convert.ToDateTime(etxtEffectiveDate.Text.Trim());
                        providerSrvSpan.SpanEndDate = Convert.ToDateTime(etxtEndDate.Text.Trim());
                        this.WorkflowPage.HospiceRequestResponse.Payload.ProvService = provServices.ToArray();
                    }
                }
                BindGrid();
                BindhospiceGrids();
            }
        }
    }
    private void BindhospiceGrids()
    {
        try
        {
            uc1HospiceRecipientServiceLocation.BindRecipientServiceLocationGrid();
            uc6HospiceOtherPayerSpan.BindGrid();
            uc8HospiceTerminalIllnessDiagnosis.BindGrid();
            uc9HospiceProviderServiceSpan.BindGrid();
            uc10HospiceHLTCFProviderService.BindGrid();
            uc11HospiceAttachment.BindGrid();
            uc7HospiceEpisodeofCare.BindHospiceEpisodeofCareGrid();

        }
        catch (Exception ex)
        {

            CreateAndReturnLogThreadNumber(ex, "An error has occurred inside BindhospiceGrids");
        }
    }
    protected void gvHospiceBenefitPeriod_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceBenefitPeriod.EditIndex = -1;
        BindGrid();
    }
    protected void gvHospiceBenefitPeriod_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
        var benlineNo = benefitPeriods[e.RowIndex].BenPeriod;
        benefitPeriods.Remove(benefitPeriods[e.RowIndex]);
        this.WorkflowPage.IsNewHospiceBenefitPeriod = false;
        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
        if (this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState != null)
        {
            var serviceCountyStates = this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState.ToList();
            if (serviceCountyStates.Count > 0 && serviceCountyStates.Where(x => x.BenPeriod == benlineNo).FirstOrDefault() != null)
            {
                serviceCountyStates.RemoveAll(x => x.BenPeriod == benlineNo);
                this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState = serviceCountyStates.ToArray();
            }
        }
        if (this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility != null)
        {
            var longTermCareFacilityStates = this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility.ToList();
            if (longTermCareFacilityStates.Count > 0 && longTermCareFacilityStates.Where(x => x.BenPeriod == benlineNo).FirstOrDefault() != null)
            {
                longTermCareFacilityStates.RemoveAll(x => x.BenPeriod == benlineNo);
                this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility = longTermCareFacilityStates.ToArray();
            }
        }
        if (this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes != null)
        {
            var diagnosisCodes = this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes.ToList();
            if (diagnosisCodes.Count > 0 && diagnosisCodes.Where(x => x.BenPeriod == benlineNo).FirstOrDefault() != null)
            {
                diagnosisCodes.RemoveAll(x => x.BenPeriod == benlineNo);
                this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes = diagnosisCodes.ToArray();
            }
        }
        if (this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo != null)
        {
            var otherPayerInfo = this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo.ToList();
            if (otherPayerInfo.Count > 0 && this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo.Where(x => x.IsFromInquiry == false).Count() > 0)
            {
                var otherPayerinfo = this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo.Where(x => x.IsFromInquiry == true);
                this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo = otherPayerinfo.ToArray();
            }
        }
        if (this.WorkflowPage.HospiceRequestResponse.Payload.ProvService != null)
        {
            var provServices = this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.ToList();
            if (provServices.Count > 0 && provServices.Where(x => x.BenPeriod == benlineNo).FirstOrDefault() != null)
            {
                provServices.RemoveAll(x => x.BenPeriod == benlineNo);
                this.WorkflowPage.HospiceRequestResponse.Payload.ProvService = provServices.ToArray();
            }
        }
        if (this.WorkflowPage.HospiceRequestResponse.Payload.Attachments != null)
        {
            var attachments = this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.ToList();
            if (attachments.Count > 0 && attachments.Where(x => x.BenPeriod == benlineNo).FirstOrDefault() != null)
            {
                attachments.RemoveAll(x => x.BenPeriod == benlineNo);
                this.WorkflowPage.HospiceRequestResponse.Payload.Attachments = attachments.ToArray();
                var dt = this.WorkflowPage.HospicDocuments;
                if (dt.Rows.Count > 0)
                {
                    var documentId = dt.AsEnumerable().Where(x => x.Field<string>("LineItem") == benlineNo.ToString()).Select(y => y.Field<string>("DocumentID")).FirstOrDefault();

                    //Delete same document Id from attachment View state 
                    if (this.WorkflowPage.HospicAttachments != null && this.WorkflowPage.HospicAttachments.ContainsKey(Convert.ToInt32(documentId)))
                    {
                        this.WorkflowPage.HospicAttachments.Remove(Convert.ToInt32(documentId));
                    }

                    //Delete from Viewstate
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var dr = dt.Rows[i];
                        if (dr["DocumentID"].ToString() == documentId)
                        {
                            dr.Delete();
                        }
                    }
                    //Delete Document from Document Table
                    ProviderController.DeleteDocument(Convert.ToInt32(documentId));
                }
            }
        }
        BindGrid();
    }
    protected void gvHospiceBenefitPeriod_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceBenefitPeriod.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceBenefitPeriod.EditIndex = -1;
    }

    private string GetHospiceReasonUpdateTypeByValue(string value)
    {
        var ds = GetHospiceReasonUpdateType();
        return (from q in ds.Tables[0].AsEnumerable()
                where q.Field<string>("REASON_TYPE_VALUE") == value
                select q.Field<string>("REASON_TYPE_DESC")).FirstOrDefault();
    }
    private void LoadHospiceReasonUpdateTypeDropDown(DropDownList ddlType)
    {

        Helper.LoadDropDown(ddlType, GetHospiceReasonUpdateType().Tables[0], "REASON_TYPE_DESC", "REASON_TYPE_VALUE", true);
    }
    private DataSet GetHospiceReasonUpdateType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospiceReasonUpdateType();
        }
    }
    private void LoadHospiceBenifitSegmentIndicatorTypeDropDown(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, GetHospiceBenifitSegmentIndicatorType(), "INDICATOR_TYPE_DESC", "INDICATOR_TYPE_VALUE", true);

    }
    private DataTable GetHospiceBenifitSegmentIndicatorType()
    {
        if (this.WorkflowPage.HospiceBenifitSegmentIndicatorType != null && this.WorkflowPage.HospiceBenifitSegmentIndicatorType.Rows.Count > 0)
        {
            return this.WorkflowPage.HospiceBenifitSegmentIndicatorType;
        }
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            this.WorkflowPage.HospiceBenifitSegmentIndicatorType = psc.SelectHospiceBenifitSegmentIndicatorType().Tables[0];
            return this.WorkflowPage.HospiceBenifitSegmentIndicatorType;
        }
    }
    private string BindSegmentIndicatorwithBenefitPeriod()
    {
        var dtBenefitSeg = this.WorkflowPage.GetHospiceBenifitSegmentIndicatorType();
        var data = string.Empty;
        if (dtBenefitSeg.Rows.Count > 0)
        {
            data = JsonConvert.SerializeObject(dtBenefitSeg);
        }

        return data;
    }
    private void BindHospiceBenifitLineNo()
    {
        var dt = this.WorkflowPage.GetHospiceBenifitLineNo();
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (dt != null && dt.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "BindBenefitPeriodLineNos('" + JsonConvert.SerializeObject(dt) + "','gvRecipentServiceLocation,gvHospiceHLTCFProviderService,gvHospiceTerminalIllnessDiagnosis,gvHospiceProviderServiceSpan,gvHospiceAttachment')", true);
            if (inquireResponse != null
             && inquireResponse.Payload != null
             && inquireResponse.Payload.Attachments != null
             && inquireResponse.Payload.Attachments.Count() > 0)
            {
                var benPeriods = inquireResponse.Payload.Attachments.Select(y => y.BenPeriod).ToList();
                var rows = dt.AsEnumerable()
                                 .Where(r => !benPeriods.Contains(r.Field<int>("LineNo")));
                if (rows.Any())
                {
                    var dtAtt = rows.CopyToDataTable();
                    if (dtAtt.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "BindBenefitPeriodLineNos('" + JsonConvert.SerializeObject(dtAtt) + "','gvHospiceAttachment')", true);
                    }
                }
            }

            if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.ProvService != null
            && inquireResponse.Payload.ProvService.Count() > 0)
            {
                var benPeriods = inquireResponse.Payload.ProvService.Select(y => y.BenPeriod).ToList();
                var rows = dt.AsEnumerable()
                                 .Where(r => !benPeriods.Contains(r.Field<int>("LineNo")));

                if (rows.Any())
                {
                    var dtProv = rows.CopyToDataTable();
                    if (dtProv.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "BindBenefitPeriodLineNos('" + JsonConvert.SerializeObject(dtProv) + "','gvHospiceProviderServiceSpan')", true);
                    }
                }
            }
        }


    }
    #endregion

    #region AttendingPhysician
    private DataTable GetAttendingEmptyDatatable()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("BenPeriod");
        dummy.Columns.Add("PhyNPI");
        dummy.Columns.Add("PhyOralCertDate");
        dummy.Columns.Add("PhyWritCertDate");
        dummy.Columns.Add("IsFromInquiry");
        return dummy;
    }
    private void BindAttendingBenefitLinenoOnFooter()
    {
        DropDownList fddlBenefitLineNo = gvHospiceAttendingPhysician.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        LoadAttendingBenefitLinenoOnFooter(fddlBenefitLineNo);
    }
    private void LoadAttendingBenefitLinenoOnFooter(DropDownList ddlType)
    {
        var dt = this.WorkflowPage.GetHospiceBenifitLineNo();
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
           && inquireResponse.Payload != null
           && inquireResponse.Payload.BenefitPeriods != null
           && inquireResponse.Payload.BenefitPeriods.Where(x => x.PhyNPI != null).Count() > 0 && dt != null)
        {
            var benPeriods = inquireResponse.Payload.BenefitPeriods.Where(x => x.PhyNPI != null && !x.IsHideBenfitperiodPhy).Select(y => y.BenPeriod).ToList();
            var rows = dt.AsEnumerable()
                             .Where(r => !benPeriods.Contains(r.Field<int>("LineNo")));

            if (rows.Any())
            {
                dt = rows.CopyToDataTable();
                Helper.LoadDropDown(ddlType, dt, "LineNo", "BenefitPeriod", true);
            }
        }
        else
        {
            Helper.LoadDropDown(ddlType, dt, "LineNo", "BenefitPeriod", true);
        }
    }
    protected void AttendingfbtnAdd_Click(object sender, EventArgs e)
    {
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceAttendingPhysician.FooterRow.FindControl("hdnBenefitLineNo");
        TextBox ftxtNPI = gvHospiceAttendingPhysician.FooterRow.FindControl("ftxtNPI") as TextBox;
        if (!ValidProviderNPI(ftxtNPI.Text))
        {
            AttendingPhyErrorMessage.InnerText = "Incorrect NPI for attending physician.";
            DropDownList ddlBenefitLineNo = (DropDownList)gvHospiceAttendingPhysician.FooterRow.FindControl("fddlBenefitLineNo");
            LoadAttendingBenefitLinenoOnFooter(ddlBenefitLineNo);


            if (ddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value) != null)
            {
                ddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value).Selected = true;
            }
            return;
        }
        TextBox ftxtOralCertificationDate = gvHospiceAttendingPhysician.FooterRow.FindControl("ftxtOralCertificationDate") as TextBox;
        DropDownList fddlBenefitLineNo = gvHospiceAttendingPhysician.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        TextBox ftxtWrittenCertificationDate = gvHospiceAttendingPhysician.FooterRow.FindControl("ftxtWrittenCertificationDate") as TextBox;

        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();

        var physican = benefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(hdnBenefitLineNo.Value)).FirstOrDefault();        
        if (physican != null)
        {
            bool isNPISuspended = false;
            bool isNPITerminated = false;
            string medicaidId = hdnAttendingMedID.Value;
            if (string.IsNullOrEmpty(medicaidId))
            {
                medicaidId = GetMedicaidIdAssociatedToNPI(ftxtNPI.Text);
            }
            DataSet dsenroll = GetServProvDateOfService(ftxtNPI.Text, medicaidId);
            isNPITerminated = GetProviderTerminatedSpan(dsenroll);
            if (isNPITerminated)
            {
                AttendingPhyErrorMessage.InnerText = "The Hospice application is initiated by or contains a terminated provider and does not allow Submission.";
                return;
            }
            else
            {
                isNPISuspended = GetProviderSuspendedSpan(dsenroll, (Convert.ToString(physican.BenPeriodEffDate)), (Convert.ToString(physican.BenPeriodEndDate)));
                if (enableCR537 && isNPISuspended)
                {
                    AttendingPhyErrorMessage.InnerText = "The date of service entered is a suspended span and does not allow Hospice submission.";
                    return;
                }
                else
                {
                    physican.PhyNPI = ftxtNPI.Text.Trim();
                    if (!string.IsNullOrEmpty(ftxtOralCertificationDate.Text))
                    {
                        physican.PhyOralCertDate = Convert.ToDateTime(ftxtOralCertificationDate.Text.Trim());
                        physican.PhyOralCertDateSpecified = true;
                    }
                    physican.PhyWritCertDate = Convert.ToDateTime(ftxtWrittenCertificationDate.Text.Trim());
                    physican.IsHideBenfitperiodPhy = false;
                }
            }
        }

        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();

        BindGrid();
    }

    private string GetMedicaidIdAssociatedToNPI(string NPI)
    {
        string medicaidId = string.Empty;
        DataSet ds = SearchProviderNPI(NPI, null, null, null);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count == 1)
            {
                medicaidId = ds.Tables[0].Rows[0].GetString("MEDICAID_ID");
            }

            if (ds.Tables[0].Rows.Count > 1)
            {
                //filter by 1 & 3
                var dataRow = ds.Tables[0].AsEnumerable().Where(row => row.Field<int>("ENROLLMENT_STATUS_CODE") == 1
                || row.Field<int>("ENROLLMENT_STATUS_CODE") == 3).Select(row => row).FirstOrDefault();

                if (dataRow != null)
                    medicaidId = dataRow.GetString("MEDICAID_ID");
            }
        }
        return medicaidId;
    }

    protected void gvHospiceAttendingPhysician_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBenefitLineNo = ((Label)e.Row.FindControl("lblBenefitLineNo"));
            if (lblBenefitLineNo != null && lblBenefitLineNo.Text != "0" && lblBenefitLineNo.Text != "")
            {
                var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
                Label lblBenefitPeriodType = ((Label)e.Row.FindControl("lblPhySegmentBenefitType"));
                Label lblBenefitPeriod = ((Label)e.Row.FindControl("lblPhyBenefitPeriod"));
                if (!string.IsNullOrEmpty(benPeriod))
                {
                    var sigmentIndicatorText = benPeriod.Split(';');
                    lblBenefitPeriodType.Text = Helper.HtmlEncode(sigmentIndicatorText[0]);
                    lblBenefitPeriod.Text = Helper.HtmlEncode(sigmentIndicatorText[1]);
                }
            }
        }
    }
    protected void gvHospiceAttendingPhysician_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblBenefitLineNo = (Label)gvHospiceAttendingPhysician.Rows[e.NewEditIndex].FindControl("lblBenefitLineNo");

        //main code while editing
        gvHospiceAttendingPhysician.EditIndex = e.NewEditIndex;
        BindGrid();
        //main code while editing


        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceAttendingPhysician.Rows[gvHospiceAttendingPhysician.EditIndex].FindControl("eddlBenefitLineNo");
        LoadBenefitLinenoOnEdit(eddlBenefitLineNo);


        if (eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text) != null)
        {
            eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text).Selected = true;
            eddlBenefitLineNo.Enabled = false;
            var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
            Label lblBenefitPeriodType = ((Label)gvHospiceAttendingPhysician.Rows[e.NewEditIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceAttendingPhysician.Rows[e.NewEditIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = Helper.HtmlEncode(sigmentIndicatorText[0]);
                lblBenefitPeriod.Text = Helper.HtmlEncode(sigmentIndicatorText[1]);
            }
        }
    }
    private void LoadBenefitLinenoOnEdit(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    protected void gvHospiceAttendingPhysician_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceAttendingPhysician.Rows[e.RowIndex].FindControl("hdnBenefitLineNo");
        TextBox etxtNPI = gvHospiceAttendingPhysician.Rows[e.RowIndex].FindControl("etxtNPI") as TextBox;
        if (!ValidProviderNPI(etxtNPI.Text))
        {
            AttendingPhyErrorMessage.InnerText = "Incorrect NPI for attending physician.";
            DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceAttendingPhysician.Rows[e.RowIndex].FindControl("eddlBenefitLineNo");
            LoadBenefitLinenoOnEdit(eddlBenefitLineNo);


            if (eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value) != null)
            {
                eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value).Selected = true;
            }
            return;
        }
        TextBox etxtOralCertificationDate = gvHospiceAttendingPhysician.Rows[e.RowIndex].FindControl("etxtOralCertificationDate") as TextBox;

        TextBox etxtWrittenCertificationDate = gvHospiceAttendingPhysician.Rows[e.RowIndex].FindControl("etxtWrittenCertificationDate") as TextBox;
        gvHospiceAttendingPhysician.EditIndex = -1;
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
        var physican = benefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(hdnBenefitLineNo.Value)).FirstOrDefault();
        if (physican != null)
        {
            bool isNPISuspended = false;
            bool isNPITerminated = false;

            string medicaidId = hdnAttendingMedID.Value;
            if (string.IsNullOrEmpty(medicaidId))
            {
                medicaidId = GetMedicaidIdAssociatedToNPI(etxtNPI.Text);
            }

            DataSet dsenroll = GetServProvDateOfService(etxtNPI.Text, medicaidId);
            isNPITerminated = GetProviderTerminatedSpan(dsenroll);
            if (isNPITerminated)
            {
                AttendingPhyErrorMessage.InnerText = "The Hospice application is initiated by or contains a terminated provider and does not allow Submission.";
                return;
            }
            else 
            {
                isNPISuspended = GetProviderSuspendedSpan(dsenroll, (Convert.ToString(physican.BenPeriodEffDate)), (Convert.ToString(physican.BenPeriodEndDate)));
                if (enableCR537 && isNPISuspended)
                {
                    AttendingPhyErrorMessage.InnerText = "The date of service entered is a suspended span and does not allow Hospice submission.";
                    return;
                }
                else
                {
                    physican.PhyNPI = etxtNPI.Text.Trim();
                    if (!string.IsNullOrEmpty(etxtOralCertificationDate.Text))
                    {
                        physican.PhyOralCertDate = Convert.ToDateTime(etxtOralCertificationDate.Text.Trim());
                        physican.PhyOralCertDateSpecified = true;
                    }
                    physican.PhyWritCertDate = Convert.ToDateTime(etxtWrittenCertificationDate.Text.Trim());
                }
            }
        }

        //benefitPeriods[e.RowIndex].SegmentIndicator = hdnBenefitSegmentIndicator.Value;
        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceAttendingPhysician_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceAttendingPhysician.EditIndex = -1;
        BindGrid();
    }
    protected void gvHospiceAttendingPhysician_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblBenefitLineNo = (Label)gvHospiceAttendingPhysician.Rows[e.RowIndex].FindControl("lblBenefitLineNo");
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
        var physican = benefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).FirstOrDefault();
        if (physican != null)
        {
            physican.PhyNPI = null;
            physican.PhyOralCertDate = DateTime.MinValue;
            physican.PhyWritCertDate = DateTime.MinValue;
        }
        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceAttendingPhysician_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceAttendingPhysician.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceAttendingPhysician.EditIndex = -1;
    }

    public bool ValidProviderNPI(string npi)
    {
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                return psc.VerifyProviderNPI(Convert.ToInt64(npi));
            }
        }
        catch (Exception ex)
        {
            CreateAndReturnLogThreadNumber(ex);
            return false;
        }
    }   
    
    #endregion

    #region IDG
    private DataTable GetIDGEmptyDatatable()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("BenPeriod");
        dummy.Columns.Add("IDGPhyNPI");
        dummy.Columns.Add("IDGPhyOralCertDate");
        dummy.Columns.Add("IDGPhyWritCertDate");
        dummy.Columns.Add("IsFromInquiry");
        return dummy;
    }
    private void BindIDGBenefitSegmentIndicatorOnFooter()
    {
        DropDownList fddlBenefitLineNo = gvHospiceIDGPhysician.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        LoadIDGHospiceBenefitLinenoDropDown(fddlBenefitLineNo);
    }
    private void LoadIDGHospiceBenefitLinenoDropDown(DropDownList ddlType)
    {
        var dt = this.WorkflowPage.GetHospiceBenifitLineNo();
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
           && inquireResponse.Payload != null
           && inquireResponse.Payload.BenefitPeriods != null
           && inquireResponse.Payload.BenefitPeriods.Where(x => x.IDGPhyNPI != null).Count() > 0 && dt != null)
        {
            var benPeriods = inquireResponse.Payload.BenefitPeriods.Where(x => x.IDGPhyNPI != null && !x.IsHideBenfitperiodIDG).Select(y => y.BenPeriod).ToList();
            var rows = dt.AsEnumerable()
                             .Where(r => !benPeriods.Contains(r.Field<int>("LineNo")));

            if (rows.Any())
            {
                dt = rows.CopyToDataTable();
                Helper.LoadDropDown(ddlType, dt, "LineNo", "BenefitPeriod", true);
            }
        }
        else
        {
            Helper.LoadDropDown(ddlType, dt, "LineNo", "BenefitPeriod", true);
        }

    }

    protected void IDGfbtnAdd_Click(object sender, EventArgs e)
    {
        TextBox ftxtNPI = gvHospiceIDGPhysician.FooterRow.FindControl("ftxtNPI") as TextBox;
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceIDGPhysician.FooterRow.FindControl("hdnBenefitLineNo");
        if (!ValidProviderNPI(ftxtNPI.Text))
        {
            IDGPhyErrorMessage.InnerText = "Incorrect NPI for Hospice IDG physician.";
            DropDownList ddlBenefitLineNo = (DropDownList)gvHospiceIDGPhysician.FooterRow.FindControl("fddlBenefitLineNo");
            LoadAttendingBenefitLinenoOnFooter(ddlBenefitLineNo);


            if (ddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value) != null)
            {
                ddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value).Selected = true;
            }
            return;
        }
        TextBox ftxtOralCertificationDate = gvHospiceIDGPhysician.FooterRow.FindControl("ftxtOralCertificationDate") as TextBox;
        DropDownList fddlBenefitSegmentIndicator = gvHospiceIDGPhysician.FooterRow.FindControl("fddlBenefitSegmentIndicator") as DropDownList;
        TextBox ftxtWrittenCertificationDate = gvHospiceIDGPhysician.FooterRow.FindControl("ftxtWrittenCertificationDate") as TextBox;

        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();

        var physican = benefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(hdnBenefitLineNo.Value)).FirstOrDefault();
        if (physican != null)
        {
            bool isNPISuspended = false;
            bool isNPITerminated = false;

            string medicaidId = hdnIDGMedID.Value;
            if (string.IsNullOrEmpty(medicaidId))
            {
                medicaidId = GetMedicaidIdAssociatedToNPI(ftxtNPI.Text);
            }

            DataSet dsenroll = GetServProvDateOfService(ftxtNPI.Text, medicaidId);
            isNPITerminated = GetProviderTerminatedSpan(dsenroll);

            if (isNPITerminated)
            {
                IDGPhyErrorMessage.InnerText = "The Hospice application is initiated by or contains a terminated provider and does not allow Submission.";
                return;
            }
            else 
            {
                isNPISuspended = GetProviderSuspendedSpan(dsenroll, (Convert.ToString(physican.BenPeriodEffDate)), (Convert.ToString(physican.BenPeriodEndDate)));
                if (enableCR537 && isNPISuspended)
                {
                    IDGPhyErrorMessage.InnerText = "The date of service entered is a suspended span and does not allow Hospice submission.";
                    return;
                }
                else
                {
                    physican.IDGPhyNPI = ftxtNPI.Text.Trim();
                    if (!string.IsNullOrEmpty(ftxtOralCertificationDate.Text))
                    {
                        physican.IDGPhyOralCertDate = Convert.ToDateTime(ftxtOralCertificationDate.Text.Trim());
                        physican.IDGPhyOralCertDateSpecified = true;
                    }
                    physican.IDGPhyWritCertDate = Convert.ToDateTime(ftxtWrittenCertificationDate.Text.Trim());
                    physican.IsHideBenfitperiodIDG = false;
                }
            }
        }

        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceIDGPhysician_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lblBenefitSegmentIndicator = ((Label)e.Row.FindControl("lblBenefitSegmentIndicator"));
            //if (lblBenefitSegmentIndicator != null && lblBenefitSegmentIndicator.Text != "0" && lblBenefitSegmentIndicator.Text != "")
            //{
            //    var sigmentIndicatorText = this.WorkflowPage.GetSegmentIndicatorByValue(lblBenefitSegmentIndicator.Text);
            //    lblBenefitSegmentIndicator.Text = sigmentIndicatorText.ToString();
            //}
            Label lblBenefitLineNo = ((Label)e.Row.FindControl("lblBenefitLineNo"));
            if (lblBenefitLineNo != null && lblBenefitLineNo.Text != "0" && lblBenefitLineNo.Text != "")
            {
                var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
                Label lblBenefitPeriodType = ((Label)e.Row.FindControl("lblIDGBenefitPeriodType"));
                Label lblBenefitPeriod = ((Label)e.Row.FindControl("lblIDGBenefitPeriod"));
                if (!string.IsNullOrEmpty(benPeriod))
                {
                    var sigmentIndicatorText = benPeriod.Split(';');
                    lblBenefitPeriodType.Text = Helper.HtmlEncode(sigmentIndicatorText[0]);
                    lblBenefitPeriod.Text = Helper.HtmlEncode(sigmentIndicatorText[1]);
                }
            }
        }
    }
    protected void gvHospiceIDGPhysician_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblBenefitLineNo = (Label)gvHospiceIDGPhysician.Rows[e.NewEditIndex].FindControl("lblBenefitLineNo");

        //main code while editing
        gvHospiceIDGPhysician.EditIndex = e.NewEditIndex;
        BindGrid();
        //main code while editing        
        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceIDGPhysician.Rows[gvHospiceIDGPhysician.EditIndex].FindControl("eddlBenefitLineNo");
        LoadBenefitLinenoOnEdit(eddlBenefitLineNo);

        if (eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text) != null)
        {
            eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text).Selected = true;
            eddlBenefitLineNo.Enabled = false;
        }

        if (eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text) != null)
        {
            eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text).Selected = true;
            var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
            Label lblBenefitPeriodType = ((Label)gvHospiceIDGPhysician.Rows[e.NewEditIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceIDGPhysician.Rows[e.NewEditIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = Helper.HtmlEncode(sigmentIndicatorText[0]);
                lblBenefitPeriod.Text = Helper.HtmlEncode(sigmentIndicatorText[1]);
            }
        }
    }
    protected void gvHospiceIDGPhysician_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int HospiceIDGPhysicianId = Convert.ToInt32(gvHospiceIDGPhysician.DataKeys[e.RowIndex]["IDGPhysicianes_Id"].ToString());
        TextBox etxtNPI = gvHospiceIDGPhysician.Rows[e.RowIndex].FindControl("etxtNPI") as TextBox;
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceIDGPhysician.Rows[e.RowIndex].FindControl("hdnBenefitLineNo");
        if (!ValidProviderNPI(etxtNPI.Text))
        {
            IDGPhyErrorMessage.InnerText = "Incorrect NPI for Hospice IDG physician.";
            DropDownList ddlBenefitLineNo = (DropDownList)gvHospiceIDGPhysician.Rows[e.RowIndex].FindControl("eddlBenefitLineNo");
            LoadBenefitLinenoOnEdit(ddlBenefitLineNo);


            if (ddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value) != null)
            {
                ddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value).Selected = true;
            }
            return;
        }
        TextBox etxtOralCertificationDate = gvHospiceIDGPhysician.Rows[e.RowIndex].FindControl("etxtOralCertificationDate") as TextBox;

        TextBox etxtWrittenCertificationDate = gvHospiceIDGPhysician.Rows[e.RowIndex].FindControl("etxtWrittenCertificationDate") as TextBox;

        gvHospiceIDGPhysician.EditIndex = -1;


        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
        var physican = benefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(hdnBenefitLineNo.Value)).FirstOrDefault();
        if (physican != null)
        {
            bool isNPISuspended = false;
            bool isNPITerminated = false;

            string medicaidId = hdnIDGMedID.Value;
            if (string.IsNullOrEmpty(medicaidId))
            {
                medicaidId = GetMedicaidIdAssociatedToNPI(etxtNPI.Text);
            }

            DataSet dsenroll = GetServProvDateOfService(etxtNPI.Text, medicaidId);
            isNPITerminated = GetProviderTerminatedSpan(dsenroll);
            if (isNPITerminated)
            {
                IDGPhyErrorMessage.InnerText = "The Hospice application is initiated by or contains a terminated provider and does not allow Submission.";
                return;
            }
            else 
            {
                isNPISuspended = GetProviderSuspendedSpan(dsenroll, (Convert.ToString(physican.BenPeriodEffDate)), (Convert.ToString(physican.BenPeriodEndDate)));
                if (enableCR537 && isNPISuspended)
                {
                    IDGPhyErrorMessage.InnerText = "The date of service entered is a suspended span and does not allow Hospice submission.";
                    return;
                }
                else
                {
                    physican.IDGPhyNPI = etxtNPI.Text.Trim();
                    if (!string.IsNullOrEmpty(etxtOralCertificationDate.Text))
                    {
                        physican.IDGPhyOralCertDate = Convert.ToDateTime(etxtOralCertificationDate.Text.Trim());
                        physican.IDGPhyOralCertDateSpecified = true;
                    }
                    physican.IDGPhyWritCertDate = Convert.ToDateTime(etxtWrittenCertificationDate.Text.Trim());
                }
            }
        }

        //benefitPeriods[e.RowIndex].SegmentIndicator = hdnBenefitSegmentIndicator.Value;
        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceIDGPhysician_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceIDGPhysician.EditIndex = -1;
        BindGrid();
    }
    protected void gvHospiceIDGPhysician_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblBenefitLineNo = (Label)gvHospiceIDGPhysician.Rows[e.RowIndex].FindControl("lblBenefitLineNo");
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.ToList();
        var physican = benefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).FirstOrDefault();
        if (physican != null)
        {
            physican.IDGPhyNPI = null;
            physican.IDGPhyOralCertDate = DateTime.MinValue;
            physican.IDGPhyWritCertDate = DateTime.MinValue;
        }
        this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceIDGPhysician_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceIDGPhysician.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceIDGPhysician.EditIndex = -1;
    }
    #endregion

}