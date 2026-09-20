using AjaxControlToolkit;
using Corp.Core.Libraries.HospiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PopupControls_HospiceProviderServiceSpan : BasePopupControl
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
            && inquireResponse.Payload.ProvService != null
            && inquireResponse.Payload.ProvService.Count() > 0)
        {
            gvHospiceProviderServiceSpan.DataSource = this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.ToList(); ;
            gvHospiceProviderServiceSpan.DataBind();
        }
        else
        {
            var sources = new List<HospiceRequestResponsePayloadProvService>
            {
               new HospiceRequestResponsePayloadProvService()
            };
            GridviewShowNoResultFound<HospiceRequestResponsePayloadProvService>(sources, gvHospiceProviderServiceSpan);
        }

        // bind country in footer row dropdownlist
        BindBenefitLinenoOnFooter();
        //gvHospiceProviderServiceSpan.FooterRow.Cells[1].Text = this.WorkflowPage.ProviderName;
    }

    private void BindBenefitSegmentIndicatorOnFooter()
    {
        DropDownList fddlBenefitSegmentIndicator = gvHospiceProviderServiceSpan.FooterRow.FindControl("fddlBenefitSegmentIndicator") as DropDownList;
        LoadHospiceBenifitSegmentIndicatorTypeDropDown(fddlBenefitSegmentIndicator);
    }
    private void BindBenefitLinenoOnFooter()
    {
        DropDownList fddlBenefitLineNo = gvHospiceProviderServiceSpan.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        LoadBenefitLinenoOnFooter(fddlBenefitLineNo);
    }
    private void LoadBenefitLinenoOnFooter(DropDownList ddlType)
    {
        //Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
        var dt = this.WorkflowPage.GetHospiceBenifitLineNo();
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.ProvService != null
            && inquireResponse.Payload.ProvService.Count() > 0 && dt != null)
        {
            var benPeriods = inquireResponse.Payload.ProvService.Select(y => y.BenPeriod).ToList();
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

    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        TextBox ftxtEndDate = gvHospiceProviderServiceSpan.FooterRow.FindControl("ftxtEndDate") as TextBox;
        TextBox ftxtEffectiveDate = gvHospiceProviderServiceSpan.FooterRow.FindControl("ftxtEffectiveDate") as TextBox;
        DropDownList fddlBenefitLineNo = gvHospiceProviderServiceSpan.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;

        HospiceRequestResponsePayloadProvService provService = new HospiceRequestResponsePayloadProvService
        {
            HospiceProvID = this.WorkflowPage.MedicaidID,
            SpanEffDate = Convert.ToDateTime(ftxtEffectiveDate.Text.Trim()),
            SpanEndDate = Convert.ToDateTime(ftxtEndDate.Text.Trim()),
            BenPeriod = Convert.ToInt32(fddlBenefitLineNo.SelectedItem.Text)
        };
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.ProvService != null)
        {
            var provServices = this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.ToList();
            provServices.Add(provService);
            this.WorkflowPage.HospiceRequestResponse.Payload.ProvService = provServices.ToArray();
        }
        else
        {
            var provServices = new List<HospiceRequestResponsePayloadProvService>();
            provServices.Add(provService);
            this.WorkflowPage.HospiceRequestResponse.Payload.ProvService = provServices.ToArray();
        }
        BindGrid();
    }
    private bool CheckDateOverlap()
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
            var lastBenId = inquireResponse.Payload.ProvService.OrderBy(x => x.BenPeriod).FirstOrDefault();
            var lastBenIdPrev = inquireResponsePrev.Payload.ProvService.OrderByDescending(x => x.BenPeriod).FirstOrDefault();
            if ((lastBenId.SpanEffDate >= lastBenIdPrev.SpanEffDate && lastBenId.SpanEffDate <= lastBenIdPrev.SpanEndDate) || (lastBenId.SpanEndDate >= lastBenIdPrev.SpanEffDate && lastBenId.SpanEndDate <= lastBenIdPrev.SpanEndDate))
            {
                isOverlap = true;
            }
        }
        return isOverlap;
    }
    protected void gvHospiceProviderServiceSpan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblProviderName = ((Label)e.Row.FindControl("lblProviderName"));
            if (lblProviderName != null && lblProviderName.Text != "")
            {
                lblProviderName.Text = GetProvideNameByProvideId(lblProviderName.Text);
            }

            Label lblBenefitLineNo = ((Label)e.Row.FindControl("lblBenefitLineNo"));
            if (lblBenefitLineNo != null && lblBenefitLineNo.Text != "0" && lblBenefitLineNo.Text != "")
            {
                var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
                Label lblBenefitPeriodType = ((Label)e.Row.FindControl("lblBenefitPeriodType"));
                Label lblBenefitPeriod = ((Label)e.Row.FindControl("lblBenefitPeriod"));
                if (!string.IsNullOrEmpty(benPeriod))
                {
                    var sigmentIndicatorText = benPeriod.Split(';');
                    lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                    lblBenefitPeriod.Text = sigmentIndicatorText[1];
                }
            }
        }
    }
    private string GetProvideNameByProvideId(string medicadeId)
    {
        string name = string.Empty;
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            DataSet ds = psc.SelectProviderByGRPMedicaidID(medicadeId);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                name = Helper.GetString("NAME", dr);
            }

            return name;
        }
    }
    private void LoadBenefitLinenoOnEdit(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    protected void gvHospiceProviderServiceSpan_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblBenefitLineNo = (Label)gvHospiceProviderServiceSpan.Rows[e.NewEditIndex].FindControl("lblBenefitLineNo");
        //main code while editing
        gvHospiceProviderServiceSpan.EditIndex = e.NewEditIndex;
        BindGrid();
        //main code while editing


        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceProviderServiceSpan.Rows[gvHospiceProviderServiceSpan.EditIndex].FindControl("eddlBenefitLineNo");
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
            Label lblBenefitPeriodType = ((Label)gvHospiceProviderServiceSpan.Rows[e.NewEditIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceProviderServiceSpan.Rows[e.NewEditIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                lblBenefitPeriod.Text = sigmentIndicatorText[1];
            }
        }
        TextBox etxtEffectiveDate = gvHospiceProviderServiceSpan.Rows[gvHospiceProviderServiceSpan.EditIndex].FindControl("etxtEffectiveDate") as TextBox;
        etxtEffectiveDate.Enabled = false;
        if (this.WorkflowPage.HospiceRequestResponse != null & this.WorkflowPage.HospiceSelectedActionType == "CHGPR")
        {
            etxtEffectiveDate.Enabled = true;
        }
        TextBox etxtEndDate = gvHospiceProviderServiceSpan.Rows[gvHospiceProviderServiceSpan.EditIndex].FindControl("etxtEndDate") as TextBox;
        etxtEndDate.Enabled = false;
        if (this.WorkflowPage.HospiceRequestResponse != null & this.WorkflowPage.HospiceSelectedActionType == "CLSPR")
        {
            etxtEndDate.Enabled = true;
        }
    }

    protected void gvHospiceProviderServiceSpan_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox etxtEndDate = gvHospiceProviderServiceSpan.Rows[e.RowIndex].FindControl("etxtEndDate") as TextBox;
        TextBox etxtEffectiveDate = gvHospiceProviderServiceSpan.Rows[e.RowIndex].FindControl("etxtEffectiveDate") as TextBox;
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceProviderServiceSpan.Rows[e.RowIndex].FindControl("hdnBenefitLineNo");
        gvHospiceProviderServiceSpan.EditIndex = -1;
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.ToList();
        benefitPeriods[e.RowIndex].SpanEffDate = Convert.ToDateTime(etxtEffectiveDate.Text.Trim());
        benefitPeriods[e.RowIndex].SpanEndDate = Convert.ToDateTime(etxtEndDate.Text.Trim());
        benefitPeriods[e.RowIndex].BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value);
        this.WorkflowPage.HospiceRequestResponse.Payload.ProvService = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceProviderServiceSpan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceProviderServiceSpan.EditIndex = -1;
        BindGrid();
    }
    protected void gvHospiceProviderServiceSpan_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.ProvService.ToList();
        benefitPeriods.Remove(benefitPeriods[e.RowIndex]);
        this.WorkflowPage.HospiceRequestResponse.Payload.ProvService = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceProviderServiceSpan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceProviderServiceSpan.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceProviderServiceSpan.EditIndex = -1;
    }

    private void LoadHospiceBenifitSegmentIndicatorTypeDropDown(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, GetHospiceBenifitSegmentIndicatorType().Tables[0], "INDICATOR_TYPE_DESC", "INDICATOR_TYPE_VALUE", true);

    }
    private DataSet GetHospiceBenifitSegmentIndicatorType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospiceBenifitSegmentIndicatorType();
        }
    }
}