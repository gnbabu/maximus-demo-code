using Corp.Core.Libraries.HospiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PopupControls_HospiceTerminalIllnessDiagnosis : BasePopupControl
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
            && inquireResponse.Payload.DiagnosisCodes != null
            && inquireResponse.Payload.DiagnosisCodes.Count() > 0)
        {
            gvHospiceTerminalIllnessDiagnosis.DataSource = this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes.ToList(); ;
            gvHospiceTerminalIllnessDiagnosis.DataBind();
        }
        else
        {
            var sources = new List<HospiceRequestResponsePayloadDiagnosisCodes>
            {
               new HospiceRequestResponsePayloadDiagnosisCodes()
            };
            GridviewShowNoResultFound<HospiceRequestResponsePayloadDiagnosisCodes>(sources, gvHospiceTerminalIllnessDiagnosis);
        }

        // bind country in footer row dropdownlist
        BindBenefitLinenoOnFooter();
        BindDummyRow();
    }
    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("ICD10Diag");
        dummy.Columns.Add("ICDVersion");
        dummy.Columns.Add("DiagDesc");
        dummy.Rows.Add();
        gvHospiceTerminalIllnessDiagnosisSearch.DataSource = dummy;
        gvHospiceTerminalIllnessDiagnosisSearch.DataBind();
    }
    private void BindBenefitLinenoOnFooter()
    {
        DropDownList fddlBenefitLineNo = gvHospiceTerminalIllnessDiagnosis.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        LoadBenefitLinenoOnFooter(fddlBenefitLineNo);
    }
    private void LoadBenefitLinenoOnFooter(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    protected void gvHospiceTerminalIllnessDiagnosis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceTerminalIllnessDiagnosis.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceTerminalIllnessDiagnosis.EditIndex = -1;
    }

    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        TextBox ftxtPrimaryTerminalDiagnosis = gvHospiceTerminalIllnessDiagnosis.FooterRow.FindControl("ftxtPrimaryTerminalDiagnosis") as TextBox;
        TextBox ftxtTerminalDiagnosis2 = gvHospiceTerminalIllnessDiagnosis.FooterRow.FindControl("ftxtTerminalDiagnosis2") as TextBox;
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceTerminalIllnessDiagnosis.FooterRow.FindControl("hdnBenefitLineNo");
        TextBox ftxtTerminalDiagnosis3 = gvHospiceTerminalIllnessDiagnosis.FooterRow.FindControl("ftxtTerminalDiagnosis3") as TextBox;
        TextBox ftxtDiagnosisEffectiveDate = gvHospiceTerminalIllnessDiagnosis.FooterRow.FindControl("ftxtDiagnosisEffectiveDate") as TextBox;
        TextBox ftxtDiagnosisEndDate = gvHospiceTerminalIllnessDiagnosis.FooterRow.FindControl("ftxtDiagnosisEndDate") as TextBox;

        HospiceRequestResponsePayloadDiagnosisCodes diagnosisCode = new HospiceRequestResponsePayloadDiagnosisCodes
        {
            PrimeTermDiag = ftxtPrimaryTerminalDiagnosis.Text.Trim(),
            TermDiag2 = (!string.IsNullOrEmpty(ftxtTerminalDiagnosis2.Text.Trim())) ? ftxtTerminalDiagnosis2.Text.Trim() : null,
            TermDiag3 = (!string.IsNullOrEmpty(ftxtTerminalDiagnosis3.Text.Trim())) ? ftxtTerminalDiagnosis3.Text.Trim() : null,
            DiagEffDate = Convert.ToDateTime(ftxtDiagnosisEffectiveDate.Text.Trim()),
            DiagEndDate = Convert.ToDateTime(ftxtDiagnosisEndDate.Text.Trim()),
            BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value)
        };
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.DiagnosisCodes != null)
        {
            var diagnosisCodes = this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes.ToList();
            diagnosisCodes.Add(diagnosisCode);
            this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes = diagnosisCodes.ToArray();
        }
        else
        {
            var diagnosisCodes = new List<HospiceRequestResponsePayloadDiagnosisCodes>();
            diagnosisCodes.Add(diagnosisCode);
            this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes = diagnosisCodes.ToArray();
        }
        BindGrid();
    }

    protected void gvHospiceTerminalIllnessDiagnosis_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceTerminalIllnessDiagnosis.EditIndex = -1;
        BindGrid();
    }

    protected void gvHospiceTerminalIllnessDiagnosis_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes.ToList();
        benefitPeriods.Remove(benefitPeriods[e.RowIndex]);
        this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes = benefitPeriods.ToArray();
        BindGrid();

    }
    private void LoadBenefitLinenoOnEdit(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    protected void gvHospiceTerminalIllnessDiagnosis_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblBenefitLineNo = (Label)gvHospiceTerminalIllnessDiagnosis.Rows[e.NewEditIndex].FindControl("lblBenefitLineNo");

        //main code while editing
        gvHospiceTerminalIllnessDiagnosis.EditIndex = e.NewEditIndex;
        BindGrid();
        //main code while editing


        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceTerminalIllnessDiagnosis.Rows[gvHospiceTerminalIllnessDiagnosis.EditIndex].FindControl("eddlBenefitLineNo");
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
            Label lblBenefitPeriodType = ((Label)gvHospiceTerminalIllnessDiagnosis.Rows[e.NewEditIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceTerminalIllnessDiagnosis.Rows[e.NewEditIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                lblBenefitPeriod.Text = sigmentIndicatorText[1];
            }
        }
    }

    protected void gvHospiceTerminalIllnessDiagnosis_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox ftxtPrimaryTerminalDiagnosis = gvHospiceTerminalIllnessDiagnosis.Rows[e.RowIndex].FindControl("etxtPrimaryTerminalDiagnosis") as TextBox;
        TextBox ftxtTerminalDiagnosis2 = gvHospiceTerminalIllnessDiagnosis.Rows[e.RowIndex].FindControl("etxtTerminalDiagnosis2") as TextBox;
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceTerminalIllnessDiagnosis.Rows[e.RowIndex].FindControl("hdnBenefitLineNo");
        TextBox ftxtTerminalDiagnosis3 = gvHospiceTerminalIllnessDiagnosis.Rows[e.RowIndex].FindControl("etxtTerminalDiagnosis3") as TextBox;
        TextBox ftxtDiagnosisEffectiveDate = gvHospiceTerminalIllnessDiagnosis.Rows[e.RowIndex].FindControl("etxtDiagnosisEffectiveDate") as TextBox;
        TextBox ftxtDiagnosisEndDate = gvHospiceTerminalIllnessDiagnosis.Rows[e.RowIndex].FindControl("etxtDiagnosisEndDate") as TextBox;
        gvHospiceTerminalIllnessDiagnosis.EditIndex = -1;

        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes.ToList();
        benefitPeriods[e.RowIndex].PrimeTermDiag = ftxtPrimaryTerminalDiagnosis.Text.Trim();
        benefitPeriods[e.RowIndex].TermDiag2 = ftxtTerminalDiagnosis2.Text.Trim();
        benefitPeriods[e.RowIndex].TermDiag3 = ftxtTerminalDiagnosis3.Text.Trim();
        benefitPeriods[e.RowIndex].DiagEffDate = Convert.ToDateTime(ftxtDiagnosisEffectiveDate.Text.Trim());
        benefitPeriods[e.RowIndex].DiagEndDate = Convert.ToDateTime(ftxtDiagnosisEndDate.Text.Trim());
        benefitPeriods[e.RowIndex].BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value);
        this.WorkflowPage.HospiceRequestResponse.Payload.DiagnosisCodes = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceTerminalIllnessDiagnosis_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                    lblBenefitPeriod.Text = sigmentIndicatorText[1];
                    hdnBFPeriodDates.Value = sigmentIndicatorText[1];
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

    //protected void lnkSearchTD_Click(Object sender,EventArgs e)
    //{
    //    using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
    //    {
    //        DiaHLTCFProviderSearchTD2.Show();
    //    }

    //}
}