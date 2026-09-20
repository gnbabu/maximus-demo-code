using Corp.Core.Libraries.HospiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PopupControls_HospiceOtherPayerSpan : BasePopupControl
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
            && inquireResponse.Payload.OtherPayerInfo != null
            && inquireResponse.Payload.OtherPayerInfo.Count() > 0)
        {
            gvHospiceOtherPayerSpan.DataSource = inquireResponse.Payload.OtherPayerInfo.ToList();
            gvHospiceOtherPayerSpan.DataBind();
        }
        else
        {
            var sources = new List<HospiceRequestResponsePayloadOtherPayerInfo>
            {
               new HospiceRequestResponsePayloadOtherPayerInfo()
            };
            GridviewShowNoResultFound<HospiceRequestResponsePayloadOtherPayerInfo>(sources, gvHospiceOtherPayerSpan);
        }
        // bind country in footer row dropdownlist
        ///BindBenefitSegmentIndicatorOnFooter();
        BindPayerTypeOnFooter();
    }

    //private void BindBenefitSegmentIndicatorOnFooter()
    //{
    //    DropDownList fddlBenefitSegmentIndicator = gvHospiceOtherPayerSpan.FooterRow.FindControl("fddlBenefitSegmentIndicator") as DropDownList;
    //    LoadHospiceBenifitSegmentIndicatorTypeDropDown(fddlBenefitSegmentIndicator);
    //}        
    private void BindPayerTypeOnFooter()
    {
        DropDownList fddlPayerType = gvHospiceOtherPayerSpan.FooterRow.FindControl("fddlPayerType") as DropDownList;
        LoadHospicePayerTypeDropDown(fddlPayerType);
    }
    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        TextBox ftxtEndDate = gvHospiceOtherPayerSpan.FooterRow.FindControl("ftxtEndDate") as TextBox;
        TextBox ftxtEffectiveDate = gvHospiceOtherPayerSpan.FooterRow.FindControl("ftxtEffectiveDate") as TextBox;
        TextBox ftxtPayerName = gvHospiceOtherPayerSpan.FooterRow.FindControl("ftxtPayerName") as TextBox;
        //DropDownList fddlBenefitSegmentIndicator = gvHospiceOtherPayerSpan.FooterRow.FindControl("fddlBenefitSegmentIndicator") as DropDownList;
        DropDownList fddlPayerType = gvHospiceOtherPayerSpan.FooterRow.FindControl("fddlPayerType") as DropDownList;

        HospiceRequestResponsePayloadOtherPayerInfo request = new HospiceRequestResponsePayloadOtherPayerInfo
        {
            PayerName = ftxtPayerName.Text.Trim(),
            PayerType = "0" + fddlPayerType.SelectedValue,
            PayerEffDate = Convert.ToDateTime(ftxtEffectiveDate.Text.Trim()),
            PayerEndDate = Convert.ToDateTime(ftxtEndDate.Text.Trim()),
            ////SegmentIndicator = fddlBenefitSegmentIndicator.SelectedItem.Text
        };
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.OtherPayerInfo != null)
        {
            var otherPayerInfo = this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo.ToList();
            otherPayerInfo.Add(request);
            this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo = otherPayerInfo.ToArray();
        }
        else
        {
            var otherPayerInfo = new List<HospiceRequestResponsePayloadOtherPayerInfo>();
            otherPayerInfo.Add(request);
            this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo = otherPayerInfo.ToArray();
        }
        BindGrid();
    }
    protected void gvHospiceOtherPayerSpan_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //Label lblBenefitSegmentIndicator = (Label)gvHospiceOtherPayerSpan.Rows[e.NewEditIndex].FindControl("lblBenefitSegmentIndicator");
        Label lblPayerType = (Label)gvHospiceOtherPayerSpan.Rows[e.NewEditIndex].FindControl("lblPayerType");
        //main code while editing
        gvHospiceOtherPayerSpan.EditIndex = e.NewEditIndex;
        BindGrid();
        //main code while editing


        //DropDownList eddlBenefitSegmentIndicator = (DropDownList)gvHospiceOtherPayerSpan.Rows[gvHospiceOtherPayerSpan.EditIndex].FindControl("eddlBenefitSegmentIndicator");
        //LoadHospiceBenifitSegmentIndicatorTypeDropDown(eddlBenefitSegmentIndicator);       

        //if (eddlBenefitSegmentIndicator.Items.FindByText(lblBenefitSegmentIndicator.Text) != null)
        //{
        //    eddlBenefitSegmentIndicator.Items.FindByText(lblBenefitSegmentIndicator.Text).Selected = true;
        //}

        DropDownList eddlPayerType = (DropDownList)gvHospiceOtherPayerSpan.Rows[gvHospiceOtherPayerSpan.EditIndex].FindControl("eddlPayerType");
        LoadHospicePayerTypeDropDown(eddlPayerType);

        if (eddlPayerType.Items.FindByText(lblPayerType.Text) != null)
        {
            eddlPayerType.Items.FindByText(lblPayerType.Text).Selected = true;
        }
    }
    protected void gvHospiceOtherPayerSpan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPayerType = ((Label)e.Row.FindControl("lblPayerType"));
            if (lblPayerType != null && lblPayerType.Text != "")
            {
                var payerType = string.Empty;
                switch (lblPayerType.Text)
                {
                    case "01":
                        payerType = "Medicare";
                        break;
                    case "02":
                        payerType = "Others";
                        break;
                    case "03":
                        payerType = "Self-Pay ";
                        break;
                    case "04":
                        payerType = "Private Insurance";
                        break;
                };
                lblPayerType.Text = payerType;
            }
        }
    }
    protected void gvHospiceOtherPayerSpan_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //int HospiceOtherPayerSpanId = Convert.ToInt32(gvHospiceOtherPayerSpan.DataKeys[e.RowIndex]["OtherPayerInfoList_Id"].ToString());
        TextBox etxtEndDate = gvHospiceOtherPayerSpan.Rows[e.RowIndex].FindControl("etxtEndDate") as TextBox;
        TextBox etxtEffectiveDate = gvHospiceOtherPayerSpan.Rows[e.RowIndex].FindControl("etxtEffectiveDate") as TextBox;
        TextBox etxtPayerName = gvHospiceOtherPayerSpan.Rows[e.RowIndex].FindControl("etxtPayerName") as TextBox;
        /// HiddenField hdnBenefitSegmentIndicator = (HiddenField)gvHospiceOtherPayerSpan.Rows[e.RowIndex].FindControl("hdnBenefitSegmentIndicator");
        HiddenField hdnPayerType = (HiddenField)gvHospiceOtherPayerSpan.Rows[e.RowIndex].FindControl("hdnPayerType");

        //Update Data in db

        gvHospiceOtherPayerSpan.EditIndex = -1;
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo.ToList();
        benefitPeriods[e.RowIndex].PayerName = etxtPayerName.Text.Trim();
        benefitPeriods[e.RowIndex].PayerType = hdnPayerType.Value;
        benefitPeriods[e.RowIndex].PayerEffDate = Convert.ToDateTime(etxtEffectiveDate.Text.Trim());
        benefitPeriods[e.RowIndex].PayerEndDate = Convert.ToDateTime(etxtEndDate.Text.Trim());
        //benefitPeriods[e.RowIndex].SegmentIndicator = hdnBenefitSegmentIndicator.Value;
        this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceOtherPayerSpan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceOtherPayerSpan.EditIndex = -1;
        BindGrid();
    }
    protected void gvHospiceOtherPayerSpan_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var benefitPeriods = this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo.ToList();
        benefitPeriods.Remove(benefitPeriods[e.RowIndex]);
        this.WorkflowPage.HospiceRequestResponse.Payload.OtherPayerInfo = benefitPeriods.ToArray();
        BindGrid();
    }
    protected void gvHospiceOtherPayerSpan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceOtherPayerSpan.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceOtherPayerSpan.EditIndex = -1;
    }

    private void LoadHospicePayerTypeDropDown(DropDownList ddlType)
    {

        Helper.LoadDropDown(ddlType, GetHospicePayerType().Tables[0], "PAYER_TYPE_DESC", "PAYER_TYPE_VALUE", true);
    }
    private DataSet GetHospicePayerType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospicePayerType();
        }
    }
    //private void LoadHospiceBenifitSegmentIndicatorTypeDropDown(DropDownList ddlType)
    //{
    //    Helper.LoadDropDown(ddlType, GetHospiceBenifitSegmentIndicatorType().Tables[0], "INDICATOR_TYPE_DESC", "INDICATOR_TYPE_DESC", true);

    //}
    //private DataSet GetHospiceBenifitSegmentIndicatorType()
    //{
    //    using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
    //    {
    //        return psc.SelectHospiceBenifitSegmentIndicatorType();
    //    }
    //}
}