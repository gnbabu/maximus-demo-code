using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_TaxonomyType : System.Web.UI.UserControl
{

    private DataSet taxonomyData = null;
    private DataSet Taxonomies
    {
        get
        {
            if (taxonomyData == null)
                taxonomyData = GetTaxonomyData();
            return taxonomyData;
        }
    }

    private DataSet specialtyTypes = null;
    private DataSet SpecialtyTypes
    {
        get
        {
            if (specialtyTypes == null)
                specialtyTypes = GetSpecialtyTypes();
            return specialtyTypes;
        }
    }

    private DataSet providerTypes = null;
    private DataSet ProviderTypes
    {
        get
        {
            if (providerTypes == null)
                providerTypes = GetProviderTypes();
            return providerTypes;
        }
    }

    private DataSet GetTaxonomyData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectTaxonomyTypesWithProviderSpecialty();
    }

    private DataSet GetSpecialtyTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectAllSpecialtyTypes();
    }

    private DataSet GetProviderTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectAllProviderTypes();
    }

    protected void rgTaxonomy_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        rgTaxonomy.DataSource = Taxonomies; 
    }
    protected void rgTaxonomy_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editform = (GridEditFormItem)e.Item;

            DropDownList ddlSpecialty = (DropDownList)editform.FindControl("ddSpecialtyType");
            ddlSpecialty.DataSource = SpecialtyTypes;
            ddlSpecialty.DataValueField = "SPECIALTY_TYPE_ID";
            ddlSpecialty.DataTextField = "SPECIALTY_TYPE_NAME";
            ddlSpecialty.DataBind();

            if(e.Item.DataItem is DataRowView)
                ddlSpecialty.SelectedValue = ((DataRowView)e.Item.DataItem)["SPECIALTY_TYPE_ID"].ToString();


            DropDownList ddlProviderTypes = (DropDownList)editform.FindControl("ddProviderType");
            ddlProviderTypes.DataSource = ProviderTypes;
            ddlProviderTypes.DataValueField = "PROVIDER_TYPE_ID";
            ddlProviderTypes.DataTextField = "PROVIDER_TYPE_NAME";
            ddlProviderTypes.DataBind();

            if(e.Item.DataItem is DataRowView)
                ddlProviderTypes.SelectedValue = ((DataRowView)e.Item.DataItem)["PROVIDER_TYPE_ID"].ToString();
        } 
    }


    protected void rgTaxonomy_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("SPECIALTY_TYPE_ID", ((gei).FindControl("ddSpecialtyType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddProviderType") as DropDownList).SelectedItem.Value);

        int taxonomyTypeID = int.Parse((gei).GetDataKeyValue("TAXONOMY_TYPE_ID").ToString());
        int specialtyTypeID = int.Parse(valuesToUpdate["SPECIALTY_TYPE_ID"].ToString());
        int providerTypeID = int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        string taxonomyCode = valuesToUpdate["TAXONOMY_CODE"] as string;
        string taxonomyName = valuesToUpdate["TAXONOMY_NAME"] as string;
        DateTime expirationDate = String.IsNullOrWhiteSpace(valuesToUpdate["EXPIRATION_DATE"] as string)? DateTime.MaxValue : 
        DateTime.Parse(valuesToUpdate["EXPIRATION_DATE"].ToString());
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string mmisSpecialtyTypeID = valuesToUpdate["MMIS_SPECIALTY_TYPE_ID"].ToString();
        bool npiRequired = (bool)valuesToUpdate["NPI_REQUIRED"];

        psc.UpdateTaxonomyType(taxonomyTypeID, specialtyTypeID, providerTypeID, taxonomyCode, taxonomyName, expirationDate, lastModifiedDate,
            lastModifiedUser, mmisSpecialtyTypeID, npiRequired);
    }


    protected void rgTaxonomy_InsertCommand(object sender, GridCommandEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("SPECIALTY_TYPE_ID", ((gei).FindControl("ddSpecialtyType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddProviderType") as DropDownList).SelectedItem.Value);

        int specialtyTypeID = int.Parse(valuesToUpdate["SPECIALTY_TYPE_ID"].ToString());
        int providerTypeID = int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        string taxonomyCode = valuesToUpdate["TAXONOMY_CODE"] as string;
        string taxonomyName = valuesToUpdate["TAXONOMY_NAME"] as string;
        DateTime expirationDate = String.IsNullOrWhiteSpace(valuesToUpdate["EXPIRATION_DATE"] as string) ? DateTime.MaxValue :
            DateTime.Parse(valuesToUpdate["EXPIRATION_DATE"].ToString());
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string mmisSpecialtyTypeID = valuesToUpdate["MMIS_SPECIALTY_TYPE_ID"].ToString();
        bool npiRequired = (bool)valuesToUpdate["NPI_REQUIRED"];

        psc.InsertTaxonomyType(specialtyTypeID, providerTypeID, taxonomyCode, taxonomyName, expirationDate, lastModifiedDate, lastModifiedUser,
            mmisSpecialtyTypeID, npiRequired);
    }
}