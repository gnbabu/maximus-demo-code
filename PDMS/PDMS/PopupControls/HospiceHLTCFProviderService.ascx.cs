using Corp.Core.Libraries.HospiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PopupControls_HospiceHLTCFProviderService : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
    }
    public void BindGrid()
    {
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.LongTermCareFacility != null
            && inquireResponse.Payload.LongTermCareFacility.Count() > 0)
        {
            gvHospiceHLTCFProviderService.DataSource = this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility.ToList();
            gvHospiceHLTCFProviderService.DataBind();
        }
        else
        {
            var sources = new List<HospiceRequestResponsePayloadLongTermCareFacility>
            {
               new HospiceRequestResponsePayloadLongTermCareFacility()
            };
            GridviewShowNoResultFound<HospiceRequestResponsePayloadLongTermCareFacility>(sources, gvHospiceHLTCFProviderService);
        }
        BindBenefitLinenoOnFooter();
        BindDummyRow();
    }
    private void BindBenefitLinenoOnFooter()
    {
        DropDownList fddlBenefitLineNo = gvHospiceHLTCFProviderService.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        LoadBenefitLinenoOnFooter(fddlBenefitLineNo);
    }
    private void LoadBenefitLinenoOnFooter(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("NPI");
        dummy.Columns.Add("MEDICAID_ID");
        dummy.Columns.Add("LAST_OR_BUSINESS_NAME");
        dummy.Columns.Add("FIRST_NAME");
        dummy.Rows.Add();
        gvHospiceHLTCFProviderSearch.DataSource = dummy;
        gvHospiceHLTCFProviderSearch.DataBind();
    }
    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        TextBox ftxtEndDate = gvHospiceHLTCFProviderService.FooterRow.FindControl("ftxtEndDate") as TextBox;
        TextBox ftxtEffectiveDate = gvHospiceHLTCFProviderService.FooterRow.FindControl("ftxtEffectiveDate") as TextBox;
        TextBox ftxtProviderMedicaidID = gvHospiceHLTCFProviderService.FooterRow.FindControl("ftxtProviderMedicaidID") as TextBox;
        HiddenField hdnftxtProviderMedicaidID = (HiddenField)gvHospiceHLTCFProviderService.FooterRow.FindControl("hdnftxtProviderMedicaidID");
        TextBox ftxtProviderNPI = gvHospiceHLTCFProviderService.FooterRow.FindControl("ftxtProviderNPI") as TextBox;
        DropDownList fddlBenefitLineNo = gvHospiceHLTCFProviderService.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        HospiceRequestResponsePayloadLongTermCareFacility request = new HospiceRequestResponsePayloadLongTermCareFacility
        {
            //HLTCFProvMedID = ftxtProviderMedicaidID.Text.Trim(),
            HLTCFProvMedID = hdnftxtProviderMedicaidID.Value.Trim(),
            HLTCFEffDate = Convert.ToDateTime(ftxtEffectiveDate.Text.Trim()),
            HLTCFEndDate = Convert.ToDateTime(ftxtEndDate.Text.Trim()),
            BenPeriod = Convert.ToInt32(fddlBenefitLineNo.SelectedItem.Text)
        };
        if (!ValidateFields(ftxtProviderNPI.Text, request.BenPeriod, request.HLTCFEffDate, request.HLTCFEndDate, hdnftxtProviderMedicaidID.Value.Trim()))
        {
            if (request.HLTCFProvMedID != null && request.HLTCFProvMedID != "")
            {
                var provInfo = GetProvideNameByProvideId(request.HLTCFProvMedID);
                TextBox ftxtProviderName = ((TextBox)gvHospiceHLTCFProviderService.FooterRow.FindControl("ftxtProviderName"));
                ftxtProviderMedicaidID.Text = request.HLTCFProvMedID;
                ftxtProviderName.Text = provInfo.Item1;
                ftxtProviderNPI.Text = provInfo.Item2;
            }

            if (request.BenPeriod > 0)
            {
                var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == request.BenPeriod).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
                Label flblSegmentBenefitType = ((Label)gvHospiceHLTCFProviderService.FooterRow.FindControl("flblSegmentBenefitType"));
                Label flblDateBenefitPeriod = ((Label)gvHospiceHLTCFProviderService.FooterRow.FindControl("flblDateBenefitPeriod"));
                if (!string.IsNullOrEmpty(benPeriod))
                {
                    var sigmentIndicatorText = benPeriod.Split(';');
                    flblSegmentBenefitType.Text = sigmentIndicatorText[0];
                    flblDateBenefitPeriod.Text = sigmentIndicatorText[1];
                }
            }
            return;
        }
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.LongTermCareFacility != null)
        {
            var longTermCareFacilities = this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility.ToList();
            longTermCareFacilities.Add(request);
            this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility = longTermCareFacilities.ToArray();
        }
        else
        {
            var longTermCareFacilities = new List<HospiceRequestResponsePayloadLongTermCareFacility>();
            longTermCareFacilities.Add(request);
            this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility = longTermCareFacilities.ToArray();
        }
        BindGrid();
    }
    private bool ValidateFields(string NPI, int benPeriodLineNo, DateTime effDate, DateTime endDate, string medicaidID = null)
    {
        if (string.IsNullOrEmpty(medicaidID))
        {
            medicaidID = null;
        }

        if (!ValidProviderNPI(NPI))
        {
            HltcPhyErrorMessage.InnerText = "Incorrect NPI for HLTCF Provider Service.";
            return false;
        }
        //OHPNM-17401
        if (!ValidHLTCProviderNPI(NPI, medicaidID))
        {
            HltcPhyErrorMessage.InnerText = "Provider is not an HLTCF provider";
            return false;
        }
        if (effDate > endDate)
        {
            HltcPhyErrorMessage.InnerText = "Effective date is greater than end date.";
            return false;
        }
        if (benPeriodLineNo > 0)
        {
            var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == benPeriodLineNo).FirstOrDefault();

            if (benPeriod.BenPeriodEffDate > effDate || effDate > benPeriod.BenPeriodEndDate)
            {
                HltcPhyErrorMessage.InnerText = "Effective date is not within the time span of the associated benefit period.";
                return false;
            }
            if (benPeriod.BenPeriodEffDate > endDate || endDate > benPeriod.BenPeriodEndDate)
            {
                HltcPhyErrorMessage.InnerText = "End date is not within the time span of the associated benefit period.";
                return false;
            }
        }
        return true;
    }
    protected void gvHospiceHLTCFProviderService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblHLTCProviderName = ((Label)e.Row.FindControl("lblHLTCProviderName"));
            Label lblProviderNPI = ((Label)e.Row.FindControl("lblProviderNPI"));
            Label lblProviderMedicaidID = ((Label)e.Row.FindControl("lblProviderMedicaidID"));
            if (lblProviderMedicaidID != null && lblProviderMedicaidID.Text != "")
            {
                var provInfo = GetProvideNameByProvideId(lblProviderMedicaidID.Text);
                lblHLTCProviderName.Text = provInfo.Item1;
                lblProviderNPI.Text = provInfo.Item2;
            }

            Label lblBenefitLineNo = ((Label)e.Row.FindControl("lblBenefitLineNo"));
            if (lblBenefitLineNo != null && lblBenefitLineNo.Text != "0" && lblBenefitLineNo.Text != "")
            {
                var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
                Label lblBenefitPeriodType = ((Label)e.Row.FindControl("lblBenefitPeriodType"));
                Label lblBenefitPeriod = ((Label)e.Row.FindControl("lblBenefitPeriod"));
                HiddenField hdnBFPeriodDates = ((HiddenField)e.Row.FindControl("hdnBFPeriodDates"));
                if (!string.IsNullOrEmpty(benPeriod))
                {
                    var sigmentIndicatorText = benPeriod.Split(';');
                    lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                    lblBenefitPeriod.Text = sigmentIndicatorText[1];
                    hdnBFPeriodDates.Value = sigmentIndicatorText[1];
                }
            }
        }
    }

    private Tuple<string, string> GetProvideNameByProvideId(string medicadeId)
    {
        string name = string.Empty;
        string npi = string.Empty;
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            DataSet ds = psc.SelectProviderByGRPMedicaidID(medicadeId);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                name = Helper.GetString("NAME", dr);
                npi = Helper.GetString("NPI", dr);
            }

            return Tuple.Create(name, npi);
        }
    }
    private void LoadBenefitLinenoOnEdit(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    protected void gvHospiceHLTCFProviderService_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblBenefitLineNo = (Label)gvHospiceHLTCFProviderService.Rows[e.NewEditIndex].FindControl("lblBenefitLineNo");
        gvHospiceHLTCFProviderService.EditIndex = e.NewEditIndex;
        BindGrid();
        TextBox etxtProviderMedicaidID = (TextBox)gvHospiceHLTCFProviderService.Rows[gvHospiceHLTCFProviderService.EditIndex].FindControl("etxtProviderMedicaidID");
        TextBox etxtProviderName = (TextBox)gvHospiceHLTCFProviderService.Rows[gvHospiceHLTCFProviderService.EditIndex].FindControl("etxtProviderName");
        TextBox etxtProviderNPI = (TextBox)gvHospiceHLTCFProviderService.Rows[gvHospiceHLTCFProviderService.EditIndex].FindControl("etxtProviderNPI");
        HiddenField hdnetxtProviderMedicaidID = (HiddenField)gvHospiceHLTCFProviderService.Rows[gvHospiceHLTCFProviderService.EditIndex].FindControl("hdnetxtProviderMedicaidID");
        if (etxtProviderMedicaidID != null && etxtProviderMedicaidID.Text != "")
        {
            var provInfo = GetProvideNameByProvideId(etxtProviderMedicaidID.Text);
            etxtProviderName.Text = provInfo.Item1;
            etxtProviderNPI.Text = provInfo.Item2;
            hdnetxtProviderMedicaidID.Value = etxtProviderMedicaidID.Text;
        }
        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceHLTCFProviderService.Rows[gvHospiceHLTCFProviderService.EditIndex].FindControl("eddlBenefitLineNo");
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
            Label lblBenefitPeriodType = ((Label)gvHospiceHLTCFProviderService.Rows[e.NewEditIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceHLTCFProviderService.Rows[e.NewEditIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                lblBenefitPeriod.Text = sigmentIndicatorText[1];
            }
        }
    }
    protected void gvHospiceHLTCFProviderService_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int HospiceHLTCFProviderServiceId = Convert.ToInt32(gvHospiceHLTCFProviderService.DataKeys[e.RowIndex]["LongTermCareFacilities_id"].ToString());
        TextBox etxtEndDate = gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("etxtEndDate") as TextBox;
        TextBox etxtEffectiveDate = gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("etxtEffectiveDate") as TextBox;
        TextBox etxtProviderMedicaidID = gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("etxtProviderMedicaidID") as TextBox;
        HiddenField hdnetxtProviderMedicaidID = (HiddenField)gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("hdnetxtProviderMedicaidID");
        TextBox etxtProviderNPI = gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("etxtProviderNPI") as TextBox;
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("hdnBenefitLineNo");

        TextBox etxtProviderName = (TextBox)gvHospiceHLTCFProviderService.Rows[gvHospiceHLTCFProviderService.EditIndex].FindControl("etxtProviderName");

        if (hdnetxtProviderMedicaidID != null && hdnetxtProviderMedicaidID.Value != "")
        {
            var provInfo = GetProvideNameByProvideId(hdnetxtProviderMedicaidID.Value);
            etxtProviderName.Text = provInfo.Item1;
            etxtProviderNPI.Text = provInfo.Item2;
        }
        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceHLTCFProviderService.Rows[gvHospiceHLTCFProviderService.EditIndex].FindControl("eddlBenefitLineNo");
        LoadBenefitLinenoOnEdit(eddlBenefitLineNo);

        if (eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value) != null)
        {
            eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value).Selected = true;
            eddlBenefitLineNo.Enabled = false;
        }
        if (eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value) != null)
        {
            eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value).Selected = true;
            var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(hdnBenefitLineNo.Value)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
            Label lblBenefitPeriodType = ((Label)gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceHLTCFProviderService.Rows[e.RowIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                lblBenefitPeriod.Text = sigmentIndicatorText[1];
            }
        }

        if (!ValidateFields(etxtProviderNPI.Text, Convert.ToInt32(hdnBenefitLineNo.Value), Convert.ToDateTime(etxtEffectiveDate.Text.Trim()), Convert.ToDateTime(etxtEndDate.Text.Trim())))
        {
            return;
        }

        //Update Data in db

        gvHospiceHLTCFProviderService.EditIndex = -1;
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility.ToList();
        //benefitPeriods[e.RowIndex].HLTCFProvMedID = etxtProviderMedicaidID.Text.Trim();
        benefitPeriods[e.RowIndex].HLTCFProvMedID = hdnetxtProviderMedicaidID.Value.Trim();
        benefitPeriods[e.RowIndex].HLTCFEffDate = Convert.ToDateTime(etxtEffectiveDate.Text.Trim());
        benefitPeriods[e.RowIndex].HLTCFEndDate = Convert.ToDateTime(etxtEndDate.Text.Trim());
        benefitPeriods[e.RowIndex].BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value);
        this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceHLTCFProviderService_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceHLTCFProviderService.EditIndex = -1;
        BindGrid();
    }
    protected void gvHospiceHLTCFProviderService_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility.ToList();
        benefitPeriods.Remove(benefitPeriods[e.RowIndex]);
        this.WorkflowPage.HospiceRequestResponse.Payload.LongTermCareFacility = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceHLTCFProviderService_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceHLTCFProviderService.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceHLTCFProviderService.EditIndex = -1;
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
        catch (Exception)
        {
            return false;
        }
    }

    public bool ValidHLTCProviderNPI(string npi, string medicaidID = null)
    {
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                DataSet ds = psc.SearchProviderNPI(npi, medicaidID, null, null);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string mmisproviderType = Convert.ToString(ds.Tables[0].Rows[0]["MMIS_PROVIDER_TYPE_ID"]);
                        if (mmisproviderType.Equals("85") || mmisproviderType.Equals("86") || mmisproviderType.Equals("88") || mmisproviderType.Equals("89") )
                        {
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            return false;
        }
        return false;
    }
}