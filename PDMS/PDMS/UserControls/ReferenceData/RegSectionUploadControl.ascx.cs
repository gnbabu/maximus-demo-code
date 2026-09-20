using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_RegSectionUploadControl : System.Web.UI.UserControl
{
    private DataSet providerCategoryTypes = null;
    private DataSet ProviderCategoryTypes
    {
        get
        {
            if (providerCategoryTypes == null)
                providerCategoryTypes = GetProviderCategoryTypes();
            return providerCategoryTypes;
        }
    }

    private DataSet GetProviderCategoryTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetProviderCategories(true);
    }

    private DataSet applicationTypes = null;
    private DataSet ApplicationTypes
    {
        get
        {
            if (applicationTypes == null)
                applicationTypes = GetApplicationTypes();
            return applicationTypes;
        }
    }

    private DataSet GetApplicationTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.GetApplicationTypes();
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

    private DataSet GetProviderTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectAllProviderTypes();
    }

    private DataSet regPageTypes = null;
    private DataSet RegPageTypes
    {
        get
        {
            if (regPageTypes == null)
                regPageTypes = GetRegPageTypes();
            return regPageTypes;
        }
    }

    private DataSet GetRegPageTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectRegPageTypeAll();
    }

    private DataSet regPageSections = null;
    private DataSet RegPageSections
    {
        get
        {
            if (regPageSections == null)
                regPageSections = GetRegPageSections();
            return regPageSections;
        }
    }
    private DataSet GetRegPageSections()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectRegSectionTypeAll();
    }


    protected void rgRegSectionUploadControl_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgRegSectionUploadControl.DataSource = psc.SelectRegSectionUploadControlAll();
    }
    protected void rgRegSectionUploadControl_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddlProviderType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_CATEGORY_TYPE_ID", ((gei).FindControl("ddlProviderCategoryType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_PAGE_TYPE_ID", ((gei).FindControl("ddlRegPageType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_SECTION_TYPE_NAME", ((gei).FindControl("ddlRegPageSection") as DropDownList).SelectedItem.Value);

        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());
        int providerCategoryTypeID =  int.Parse(valuesToUpdate["PROVIDER_CATEGORY_TYPE_ID"].ToString());
        int providerTypeID =  int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        int regPageTypeID = int.Parse(valuesToUpdate["REG_PAGE_TYPE_ID"].ToString());
        string title = valuesToUpdate["TITLE"].ToString();
        string description = valuesToUpdate["DESCRIPTION"].ToString();
        bool isRequired = (bool)valuesToUpdate["IS_REQUIRED"];
        string regPageSection = valuesToUpdate["REG_PAGE_SECTION"].ToString();
        string regPageName = valuesToUpdate["REG_SECTION_TYPE_NAME"].ToString();

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertRegSectionUploadControl(applicationTypeID, providerCategoryTypeID, providerTypeID, regPageTypeID, title, description, isRequired, lastModifiedDate, lastModifiedUser,
            regPageSection, regPageName);
    }

    protected void rgRegSectionUploadControl_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddlProviderType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_CATEGORY_TYPE_ID", ((gei).FindControl("ddlProviderCategoryType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_PAGE_TYPE_ID", ((gei).FindControl("ddlRegPageType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_SECTION_TYPE_NAME", ((gei).FindControl("ddlRegPageSection") as DropDownList).SelectedItem.Value);

        int regSectionUploadControlID = int.Parse((gei).GetDataKeyValue("REG_SECTION_UPLOAD_CONTROL_ID").ToString());
        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());
        int providerCategoryTypeID = int.Parse(valuesToUpdate["PROVIDER_CATEGORY_TYPE_ID"].ToString());
        int providerTypeID = int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        int regPageTypeID = int.Parse(valuesToUpdate["REG_PAGE_TYPE_ID"].ToString());
        string title = valuesToUpdate["TITLE"].ToString();
        string description = valuesToUpdate["DESCRIPTION"].ToString();
        bool isRequired = (bool)valuesToUpdate["IS_REQUIRED"];
        string regPageSection = valuesToUpdate["REG_SECTION_TYPE_NAME"].ToString();
        string regPageName = valuesToUpdate["REG_PAGE_NAME"].ToString();

        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertRegSectionUploadControl(applicationTypeID, providerCategoryTypeID, providerTypeID, regPageTypeID, title, description, isRequired, lastModifiedDate, lastModifiedUser,
            regPageSection, regPageName);
        
    }

    protected void rgRegSectionUploadControl_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editForm = (GridEditFormItem)e.Item;

            DropDownList ddlEntityType = (DropDownList)editForm.FindControl("ddlProviderCategoryType");
            ddlEntityType.DataSource = ProviderCategoryTypes;
            ddlEntityType.DataValueField = "PROVIDER_CATEGORY_TYPE_ID";
            ddlEntityType.DataTextField = "PROVIDER_CATEGORY_TYPE_NAME";
            ddlEntityType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlEntityType.SelectedValue = ((DataRowView)e.Item.DataItem)["PROVIDER_CATEGORY_TYPE_ID"].ToString();

            DropDownList ddlProviderType = (DropDownList)editForm.FindControl("ddlProviderType");
            ddlProviderType.DataSource = ProviderTypes;
            ddlProviderType.DataValueField = "PROVIDER_TYPE_ID";
            ddlProviderType.DataTextField = "PROVIDER_TYPE_NAME";
            ddlProviderType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlProviderType.SelectedValue = ((DataRowView)e.Item.DataItem)["PROVIDER_TYPE_ID"].ToString();

            DropDownList ddlApplicationType = (DropDownList)editForm.FindControl("ddlApplicationType");
            ddlApplicationType.DataSource = ApplicationTypes;
            ddlApplicationType.DataValueField = "APPLICATION_TYPE_ID";
            ddlApplicationType.DataTextField = "APPLICATION_TYPE_NAME";
            ddlApplicationType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlApplicationType.SelectedValue = ((DataRowView)e.Item.DataItem)["APPLICATION_TYPE_ID"].ToString();


            DropDownList ddlRegPageType = (DropDownList)editForm.FindControl("ddlRegPageType");
            ddlRegPageType.DataSource = RegPageTypes;
            ddlRegPageType.DataValueField = "REG_PAGE_TYPE_ID";
            ddlRegPageType.DataTextField = "REG_PAGE_NAME";
            ddlRegPageType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlRegPageType.SelectedValue = ((DataRowView)e.Item.DataItem)["REG_PAGE_TYPE_ID"].ToString();


            DropDownList ddlRegPageSection = (DropDownList)editForm.FindControl("ddlRegPageSection");
            ddlRegPageSection.DataSource = RegPageSections;
            ddlRegPageSection.DataValueField = "REG_SECTION_TYPE_NAME";
            ddlRegPageSection.DataTextField = "SECTION_DISPLAY_NAME";
            ddlRegPageSection.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlRegPageSection.SelectedValue = ((DataRowView)e.Item.DataItem)["REG_PAGE_SECTION"].ToString();
        } 
    }
}