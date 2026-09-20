using Corp.Core.Libraries.HospiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PopupControls_HospiceEpisodeofCare : BasePopupControl 
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        BindHospiceEpisodeofCareGrid();       
    }
    public void BindHospiceEpisodeofCareGrid()
    {
        //TODO
        var inquireResponse = this.WorkflowPage.InquireHospiceResponse;
        if (inquireResponse != null && inquireResponse.EpisodeOfCare != null)
        {
            gvHospiceEpisodeofCare.DataSource = this.WorkflowPage.InquireHospiceResponse.EpisodeOfCare.ToList();
            gvHospiceEpisodeofCare.DataBind();
        }
        else
        {
            var sources = new List<InquireResponseEpisodeOfCare>
            {
               new InquireResponseEpisodeOfCare()
            };
            GridviewShowNoResultFound<InquireResponseEpisodeOfCare>(sources, gvHospiceEpisodeofCare);
        } 
    }
    protected void gvHospiceEpisodeofCare_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceEpisodeofCare.PageIndex = e.NewPageIndex;
        BindHospiceEpisodeofCareGrid();
        gvHospiceEpisodeofCare.EditIndex = -1;
    }
    private void BindBenefitSegmentIndicatorOnFooter()
    {
        DropDownList fddlBenefitSegmentIndicator = gvHospiceEpisodeofCare .FooterRow.FindControl("fddlBenefitSegmentIndicator") as DropDownList;
        LoadHospiceBenifitSegmentIndicatorTypeDropDown(fddlBenefitSegmentIndicator);
    }
    private void LoadHospiceBenifitSegmentIndicatorTypeDropDown(DropDownList ddlType)
    {

        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitSegmentIndicatorType(), "INDICATOR_TYPE_DESC", "INDICATOR_TYPE_DESC", true);

    }
}