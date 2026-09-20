using AjaxControlToolkit;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_LicensesHistory : BaseSectionControl
{
    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "Index"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
   
    public void LoadData()
    {
        try
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "LICENSESHistory");
            if (Helper.HasRows(ds))
            {
                grd.DataSource = ds.Tables[0];
                grd.DataBind();
            }

        }
        catch (Exception ex)
        {
            AddError(string.Format("Exception : {0}", ex.Message));
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (_SortField.Equals(e.SortExpression))
        {
            _SortField = _SortField + " DESC";
        }
        else
        {
            _SortField = e.SortExpression;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "LICENSESHistory");
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
        Label historyLabel = (Label)this.Parent.FindControl("lblTitle");
        historyLabel.Text = "License History";
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "LICENSESHistory");
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.DataSource = ds.Tables[0];
            grd.PageIndex = e.NewPageIndex;
            grd.DataBind();
        }
        ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpe");
        mpe.Show();
        Label historyLabel = (Label)this.Parent.FindControl("lblTitle");
        historyLabel.Text = "License History";
    }



    protected string FormatAddress(object regAddressId)
    {
        string address = string.Empty;
        try
        {
            if (regAddressId != null && Convert.ToInt32(regAddressId) > 0)
            {
                DataRow drAddress = GetLicenseAddress(this.WorkflowPage.RegistrationId, Convert.ToInt32(regAddressId));

                address = Helper.GetFormattedAddress(Helper.GetString("ADDRESS1", drAddress), Helper.GetString("ADDRESS2", drAddress), Helper.GetString("CITY", drAddress), Helper.GetString("STATE", drAddress), Helper.GetString("ZIP", drAddress), string.Empty,string.Empty);
                address = string.IsNullOrEmpty(Helper.GetString("COUNTY", drAddress)) ? address : address + "<BR/>" + Helper.GetString("COUNTYNAME", drAddress);
            }
        }
        catch (Exception)
        {

        }
        return address;
    }
    protected string FormatSpecialtyFocus(object regLicensureId)
    {
        string specialtyFocus = string.Empty;
        try
        {
            if (regLicensureId != null && Convert.ToInt32(regLicensureId) > 0)
            {
                DataTable dtSpecialty = GetLicenseSpecialtyFocusInformation(Convert.ToInt32(regLicensureId));

                specialtyFocus = Helper.GetFomattedLicenseSpecialtyFocusInfo(dtSpecialty);
            }
        }
        catch (Exception)
        {

        }
        return specialtyFocus;
    }
    private DataRow GetLicenseAddress(int regID, int regAddressId)
    {
        DataRow drAddress = null;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectProviderAddressInfo(regID, CON.AddressType.ProfessionalLicenseAddress);
        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        {
            drAddress = ds.Tables[0].Select("REG_ADDRESS_ID=" + regAddressId)[0];

        }

        return drAddress;
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "LicensesHistoryValidationGP";
        this.Page.Validators.Add(val);
    }

    private DataTable GetLicenseSpecialtyFocusInformation(int regLicensureID)
    {
        //code optimization needed.
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        DataSet ds = new DataSet();
        parameters.Add("REG_LICENSURE_ID", regLicensureID.ToString());
        ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ENDORSEMENT_SPECIALTY_FOCUS", parameters);

        return ds.Tables[0];
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadData();
    }

    public override void LoadData(DataRow row)
    {

    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "LicensesHistory"; }
    }

    public override string Title
    {
        get { return "Licenses History"; }
    }

    public override string IdText
    {
        get { return "ucLicensesHistory_" + this.WorkflowPage.RegistrationId; }
    }
}