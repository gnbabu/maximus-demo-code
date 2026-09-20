using Corp.Core.Libraries.HospiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class PopupControls_HospiceRecipientServiceLocation : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BindRecipientServiceLocationGrid();
    }
    public void BindRecipientServiceLocationGrid()
    {
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.ServiceCountyState != null
            && inquireResponse.Payload.ServiceCountyState.Count() > 0)
        {
            gvRecipentServiceLocation.DataSource = inquireResponse.Payload.ServiceCountyState;
            gvRecipentServiceLocation.DataBind();
        }
        else
        {
            var sources = new List<HospiceRequestResponsePayloadServiceCountyState>
            {
               new HospiceRequestResponsePayloadServiceCountyState()
            };
            GridviewShowNoResultFound<HospiceRequestResponsePayloadServiceCountyState>(sources, gvRecipentServiceLocation);
        }
        BindStateOnFooter();
        BindCountyOnFooter();
        BindBenefitLinenoOnFooter();
    }
    private void BindCountyOnFooter()
    {
        DropDownList fddlCounty = gvRecipentServiceLocation.FooterRow.FindControl("fddlCountyofService") as DropDownList;
        DropDownList fddlStateOfService = gvRecipentServiceLocation.FooterRow.FindControl("fddlStateOfService") as DropDownList;
        LoadHospiceCountyDropDown(fddlCounty, fddlStateOfService.SelectedValue);
    }
    private void BindStateOnFooter()
    {
        DropDownList fddlStateOfService = gvRecipentServiceLocation.FooterRow.FindControl("fddlStateOfService") as DropDownList;
        LoadHospiceStateDropDown(fddlStateOfService);
    }
    private void BindBenefitLinenoOnFooter()
    {
        DropDownList fddlBenefitLineNo = gvRecipentServiceLocation.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        LoadBenefitLinenoOnFooter(fddlBenefitLineNo);
    }
    private void LoadBenefitLinenoOnFooter(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    private void LoadBenefitLinenoOnEdit(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        TextBox ftxtEndDate = gvRecipentServiceLocation.FooterRow.FindControl("ftxtEndDate") as TextBox;
        TextBox ftxtEffectiveDate = gvRecipentServiceLocation.FooterRow.FindControl("ftxtEffectiveDate") as TextBox;
        HiddenField hdnBenefitLineNo = (HiddenField)gvRecipentServiceLocation.FooterRow.FindControl("hdnBenefitLineNo");
        DropDownList fddlCountyofService = gvRecipentServiceLocation.FooterRow.FindControl("fddlCountyofService") as DropDownList;
        DropDownList fddlStateOfService = gvRecipentServiceLocation.FooterRow.FindControl("fddlStateOfService") as DropDownList;
        HiddenField hdncountyOfService = (HiddenField)gvRecipentServiceLocation.FooterRow.FindControl("hdncountyOfServiceF");
        HospiceRequestResponsePayloadServiceCountyState serviceCountyState = new HospiceRequestResponsePayloadServiceCountyState
        {
            County = hdncountyOfService.Value,
            CountyEffDate = Convert.ToDateTime(ftxtEffectiveDate.Text.Trim()),
            CountyEndDate = Convert.ToDateTime(ftxtEndDate.Text.Trim()),
            State = fddlStateOfService.SelectedValue,
            BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value)
        };
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.ServiceCountyState != null)
        {
            var serviceCountyStates = this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState.ToList();
            serviceCountyStates.Add(serviceCountyState);
            this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState = serviceCountyStates.ToArray();
        }
        else
        {
            var serviceCountyStates = new List<HospiceRequestResponsePayloadServiceCountyState>();
            serviceCountyStates.Add(serviceCountyState);
            this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState = serviceCountyStates.ToArray();
        }
        BindRecipientServiceLocationGrid();
    }
    protected void gvRecipentServiceLocation_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblCountry = (Label)gvRecipentServiceLocation.Rows[e.NewEditIndex].FindControl("lblCountyofService");
        Label lblState = (Label)gvRecipentServiceLocation.Rows[e.NewEditIndex].FindControl("lblStateOfService");
        Label lblBenefitLineNo = (Label)gvRecipentServiceLocation.Rows[e.NewEditIndex].FindControl("lblBenefitLineNo");

        gvRecipentServiceLocation.EditIndex = e.NewEditIndex;
        BindRecipientServiceLocationGrid();

        ////find the Country DropDownList of EditItemTemplate
        DropDownList eddlStateOfService = gvRecipentServiceLocation.Rows[gvRecipentServiceLocation.EditIndex].FindControl("eddlStateOfService") as DropDownList;

        //// assigning the Country DataTable to DropDownList
        LoadHospiceStateDropDown(eddlStateOfService);

        if (eddlStateOfService.Items.FindByValue(lblState.Text) != null)
        {
            eddlStateOfService.Items.FindByValue(lblState.Text).Selected = true;
        }

        DropDownList eddlCountyofService = gvRecipentServiceLocation.Rows[gvRecipentServiceLocation.EditIndex].FindControl("eddlCountyofService") as DropDownList;
        LoadHospiceCountyDropDown(eddlCountyofService, lblState.Text);

        if (eddlCountyofService.Items.FindByText(lblCountry.Text.Trim()) != null)
        {
            eddlCountyofService.Items.FindByText(lblCountry.Text.Trim()).Selected = true;
        }

        DropDownList eddlBenefitLineNo = (DropDownList)gvRecipentServiceLocation.Rows[gvRecipentServiceLocation.EditIndex].FindControl("eddlBenefitLineNo");
        LoadBenefitLinenoOnEdit(eddlBenefitLineNo);


        if (eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text) != null)
        {
            eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text).Selected = true;
            eddlBenefitLineNo.Enabled = false;
            var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                          + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                          + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
            Label lblBenefitPeriodType = ((Label)gvRecipentServiceLocation.Rows[e.NewEditIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvRecipentServiceLocation.Rows[e.NewEditIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = Helper.HtmlEncode(sigmentIndicatorText[0]);
                lblBenefitPeriod.Text = Helper.HtmlEncode(sigmentIndicatorText[1]);
            }
        }

    }
    protected void gvRecipentServiceLocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        //int RecipentServiceLocationId = Convert.ToInt32(gvRecipentServiceLocation.DataKeys[e.RowIndex]["ServiceCountyStates_id"].ToString());
        HiddenField hdnstateOfService = (HiddenField)gvRecipentServiceLocation.Rows[e.RowIndex].FindControl("hdnstateOfService");
        HiddenField hdncountyOfService = (HiddenField)gvRecipentServiceLocation.Rows[e.RowIndex].FindControl("hdncountyOfService");
        HiddenField hdnBenefitLineNo = (HiddenField)gvRecipentServiceLocation.Rows[e.RowIndex].FindControl("hdnBenefitLineNo");
        TextBox etxtEndDate = gvRecipentServiceLocation.Rows[e.RowIndex].FindControl("etxtEndDate") as TextBox;
        TextBox etxtEffectiveDate = gvRecipentServiceLocation.Rows[e.RowIndex].FindControl("etxtEffectiveDate") as TextBox;

        var serviceCountyStates = this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState.ToList();
        serviceCountyStates[e.RowIndex].County = hdncountyOfService.Value;
        serviceCountyStates[e.RowIndex].CountyEffDate = Convert.ToDateTime(etxtEffectiveDate.Text.Trim());
        serviceCountyStates[e.RowIndex].CountyEndDate = Convert.ToDateTime(etxtEndDate.Text.Trim());
        serviceCountyStates[e.RowIndex].State = hdnstateOfService.Value;
        serviceCountyStates[e.RowIndex].BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value);
        this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState = serviceCountyStates.ToArray();
        gvRecipentServiceLocation.EditIndex = -1;
        BindRecipientServiceLocationGrid();
    }
    protected void gvRecipentServiceLocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvRecipentServiceLocation.EditIndex = -1;
        BindRecipientServiceLocationGrid();
    }

    protected void gvRecipentServiceLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var serviceCountyStates = this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState.ToList();
        serviceCountyStates.Remove(serviceCountyStates[e.RowIndex]);
        this.WorkflowPage.HospiceRequestResponse.Payload.ServiceCountyState = serviceCountyStates.ToArray();
        BindRecipientServiceLocationGrid();
    }
    protected void gvRecipentServiceLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRecipentServiceLocation.PageIndex = e.NewPageIndex;
        BindRecipientServiceLocationGrid();
        gvRecipentServiceLocation.EditIndex = -1;
    }
    protected void gvRecipentServiceLocation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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
                    lblBenefitPeriodType.Text = Helper.HtmlEncode(sigmentIndicatorText[0]);
                    lblBenefitPeriod.Text = Helper.HtmlEncode(sigmentIndicatorText[1]);
                    hdnBFPeriodDates.Value = Helper.HtmlEncode(sigmentIndicatorText[1]);
                }

            }
        }
    }
    private DataSet GetHospiceBenifitSegmentIndicatorType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospiceBenifitSegmentIndicatorType();
        }
    }
    private void LoadHospiceCountyDropDown(DropDownList ddlType, string state)
    {
        Helper.LoadDropDown(ddlType, GetHospiceCounty(state).Tables[0], "COUNTY_NAME", "COUNTY_NAME", true);
    }
    private void LoadHospiceStateDropDown(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, GetHospiceState().Tables[0], "STATE_NAME", "STATE_ABBREV", true);
    }
    private DataSet GetHospiceCounty(string state)
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectCounty(state);
        }
    }
    private DataSet GetHospiceState()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectState();
        }
    }

}