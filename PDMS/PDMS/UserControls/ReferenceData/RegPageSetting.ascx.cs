using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

// TODO: EDV The ordering of columns can be better. 
public partial class UserControls_ReferenceData_RegPageSetting : System.Web.UI.UserControl
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

    private DataTable GetProviderCategoryTypes(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.","");
        return dataView.ToTable(true, "ENTITY_TYPE_ID", "ENTITY_TYPE_NAME");
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

    private DataTable GetProviderTypes(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.", "");
        return dataView.ToTable(true, "PROVIDER_TYPE_ID", "PROVIDER_TYPE_NAME");
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

    private DataTable GetApplicationTypes(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.", "");
        return dataView.ToTable(true, "APPLICATION_TYPE_ID", "APPLICATION_TYPE_NAME");
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

    private DataTable GetRegPageTypes(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.", "");
        return dataView.ToTable(true, "REG_PAGE_TYPE_ID", "REG_PAGE_NAME1");
    }

    private DataSet regSectionTypes = null;
    private DataSet RegSectionTypes
    {
        get
        {
            if (regSectionTypes == null)
                regSectionTypes = GetRegSectionTypes();
            return regSectionTypes;
        }
    }

    private DataSet GetRegSectionTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectRegSectionTypeAll();
    }

    private DataTable GetRegSectionTypes(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.", "");
        return dataView.ToTable(true, "REG_SECTION_TYPE_ID", "REG_SECTION_TYPE_NAME");
    }

    private DataTable GetVisibleOptions(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.", "");
        return dataView.ToTable(true, "IS_VISIBLE");
    }

    private DataTable GetRequiredOptions(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.", "");
        return dataView.ToTable(true, "IS_REQUIRED");
    }
    private DataTable GetReviewRequiredOptions(DataSet gridSource, string filter)
    {
        var dataView = gridSource.Tables[0].DefaultView;
        dataView.RowFilter = filter.Replace("RPS.", "");
        return dataView.ToTable(true, "IS_REVIEW_REQUIRED");
    }

    protected void rgRegPageSetting_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("ENTITY_TYPE_ID", ((gei).FindControl("ddlEntityType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddlProviderType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_PAGE_TYPE_ID", ((gei).FindControl("ddlRegPageType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_SECTION_TYPE_ID", ((gei).FindControl("ddlRegSectionType") as DropDownList).SelectedItem.Value);

        int entityTypeID = int.Parse(valuesToUpdate["ENTITY_TYPE_ID"].ToString());
        int providerTypeID = int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        string regPageName = valuesToUpdate["REG_PAGE_NAME"] as string;
        string regPageSection = valuesToUpdate["REG_PAGE_SECTION"] as string;
        bool isVisible = (bool)valuesToUpdate["IS_VISIBLE"];
        bool isEditable = (bool)valuesToUpdate["IS_EDITABLE"];
        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        string taskName = valuesToUpdate["TASK_NAME"] as string;
        bool isRequired = (bool)valuesToUpdate["IS_REQUIRED"];
        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());
        int regPageTypeID = int.Parse(valuesToUpdate["REG_PAGE_TYPE_ID"].ToString());
        int regSectionTypeID = int.Parse(valuesToUpdate["REG_SECTION_TYPE_ID"].ToString());
        bool isReviewRequired = (bool)valuesToUpdate["IS_REVIEW_REQUIRED"];

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertRegPageSetting(entityTypeID, providerTypeID, regPageName, regPageSection, isVisible, isEditable, lastModifiedDate, lastModifiedUser,
            taskName, isRequired, applicationTypeID, regPageTypeID, regSectionTypeID, isReviewRequired); 
    }

    protected void rgRegPageSetting_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("ENTITY_TYPE_ID", ((gei).FindControl("ddlEntityType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("PROVIDER_TYPE_ID", ((gei).FindControl("ddlProviderType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("APPLICATION_TYPE_ID", ((gei).FindControl("ddlApplicationType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_PAGE_TYPE_ID", ((gei).FindControl("ddlRegPageType") as DropDownList).SelectedItem.Value);
        valuesToUpdate.Add("REG_SECTION_TYPE_ID", ((gei).FindControl("ddlRegSectionType") as DropDownList).SelectedItem.Value);

        int regPageSettingID = int.Parse((gei).GetDataKeyValue("REG_PAGE_SETTING_ID").ToString());
        int entityTypeID = int.Parse(valuesToUpdate["ENTITY_TYPE_ID"].ToString());
        int providerTypeID = int.Parse(valuesToUpdate["PROVIDER_TYPE_ID"].ToString());
        string regPageName = valuesToUpdate["REG_PAGE_NAME"] as string;
        string regPageSection = valuesToUpdate["REG_PAGE_SECTION"] as string;
        bool isVisible = (bool)valuesToUpdate["IS_VISIBLE"];
        bool isEditable = (bool)valuesToUpdate["IS_EDITABLE"];
        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        string taskName = valuesToUpdate["TASK_NAME"] as string;
        bool isRequired = (bool)valuesToUpdate["IS_REQUIRED"];
        int applicationTypeID = int.Parse(valuesToUpdate["APPLICATION_TYPE_ID"].ToString());
        int regPageTypeID = int.Parse(valuesToUpdate["REG_PAGE_TYPE_ID"].ToString());
        int regSectionTypeID =0;
        if(!string.IsNullOrEmpty(valuesToUpdate["REG_SECTION_TYPE_ID"].ToString()))
        {
            regSectionTypeID = int.Parse((valuesToUpdate["REG_SECTION_TYPE_ID"]).ToString());
        }

        
        bool isReviewRequired = (bool)valuesToUpdate["IS_REVIEW_REQUIRED"];

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateRegPageSetting(regPageSettingID, entityTypeID, providerTypeID, regPageName, regPageSection, isVisible, isEditable, lastModifiedDate, lastModifiedUser,
            taskName, isRequired, applicationTypeID, regPageTypeID, regSectionTypeID, isReviewRequired); 
    }
    
    protected void rgRegPageSetting_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
        {
            GridEditFormItem editForm = (GridEditFormItem)e.Item;

            DropDownList ddlEntityType = (DropDownList)editForm.FindControl("ddlEntityType");
            ddlEntityType.DataSource = ProviderCategoryTypes;
            ddlEntityType.DataValueField = "PROVIDER_CATEGORY_TYPE_ID";
            ddlEntityType.DataTextField = "PROVIDER_CATEGORY_TYPE_NAME";
            ddlEntityType.DataBind();

            if (e.Item.DataItem is DataRowView)
                ddlEntityType.SelectedValue = ((DataRowView)e.Item.DataItem)["ENTITY_TYPE_ID"].ToString();
            
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

            DropDownList ddlRegSectionType = (DropDownList)editForm.FindControl("ddlRegSectionType");
            ddlRegSectionType.DataSource = RegSectionTypes;
            ddlRegSectionType.DataValueField = "REG_SECTION_TYPE_ID";
            ddlRegSectionType.DataTextField = "REG_SECTION_TYPE_NAME";
            ddlRegSectionType.DataBind();

            ddlRegSectionType.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            ddlRegSectionType.SelectedIndex = 0;

            if (e.Item.DataItem is DataRowView)
                ddlRegSectionType.SelectedValue = ((DataRowView)e.Item.DataItem)["REG_SECTION_TYPE_ID"].ToString();

       
        }

        if (e.Item is GridFilteringItem)
        {
            var filterItem = e.Item as GridFilteringItem;
            RadComboBox entityCombo = (RadComboBox)filterItem.FindControl("RadComboBoxEntity");
            if (entityCombo != null)
            {
                entityCombo.DataSource = GetProviderCategoryTypes((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression);
                entityCombo.DataValueField = "ENTITY_TYPE_ID";
                entityCombo.DataTextField = "ENTITY_TYPE_NAME";
                entityCombo.DataBind();

                RadComboBoxItem firstComboItem = entityCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("ENTITY_TYPE_NAME").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    entityCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    entityCombo.DefaultItem.Selected = true;
                }

            }
            RadComboBox providerTypeCombo = (RadComboBox)filterItem.FindControl("RadComboBoxProviderType");
            if (providerTypeCombo != null)
            {
                providerTypeCombo.DataSource = GetProviderTypes((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression);
                providerTypeCombo.DataValueField = "PROVIDER_TYPE_ID";
                providerTypeCombo.DataTextField = "PROVIDER_TYPE_NAME";
                providerTypeCombo.DataBind();

                RadComboBoxItem firstComboItem = providerTypeCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("PROVIDER_TYPE_NAME").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    providerTypeCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    providerTypeCombo.DefaultItem.Selected = true;
                }

            }

            RadComboBox appTypeCombo = (RadComboBox)filterItem.FindControl("RadComboBoxAppType");
            if (appTypeCombo != null)
            {
                appTypeCombo.DataSource = GetApplicationTypes((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression);
                appTypeCombo.DataValueField = "APPLICATION_TYPE_ID";
                appTypeCombo.DataTextField = "APPLICATION_TYPE_NAME";
                appTypeCombo.DataBind();

                RadComboBoxItem firstComboItem = appTypeCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("APPLICATION_TYPE_NAME").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    appTypeCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    appTypeCombo.DefaultItem.Selected = true;
                }
            }

            RadComboBox pageTypeCombo = (RadComboBox)filterItem.FindControl("RadComboBoxPageType");
            if (pageTypeCombo != null)
            {
                pageTypeCombo.DataSource = GetRegPageTypes((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression);
                pageTypeCombo.DataValueField = "REG_PAGE_TYPE_ID";
                pageTypeCombo.DataTextField = "REG_PAGE_NAME1"; // TODO: EDV There is cleanup needs to be done. WHy there are 2 columns with the same name??
                pageTypeCombo.DataBind();

                RadComboBoxItem firstComboItem = pageTypeCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("REG_PAGE_NAME").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    pageTypeCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    pageTypeCombo.DefaultItem.Selected = true;
                }

            }

            RadComboBox sectionTypeCombo = (RadComboBox)filterItem.FindControl("RadComboBoxSectionType");
            if (sectionTypeCombo != null)
            {
                sectionTypeCombo.DataSource = GetRegSectionTypes((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression); ;
                sectionTypeCombo.DataValueField = "REG_SECTION_TYPE_ID";
                sectionTypeCombo.DataTextField = "REG_SECTION_TYPE_NAME";
                sectionTypeCombo.DataBind();

                RadComboBoxItem firstComboItem = sectionTypeCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("REG_SECTION_TYPE_NAME").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    sectionTypeCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    sectionTypeCombo.DefaultItem.Selected = true;
                }
            }

            RadComboBox isVisibleCombo = (RadComboBox)filterItem.FindControl("RadComboBoxIsVisible");
            if (isVisibleCombo != null)
            {
                isVisibleCombo.DataSource = GetVisibleOptions((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression); ;
                isVisibleCombo.DataValueField = "IS_VISIBLE";
                isVisibleCombo.DataTextField = "IS_VISIBLE";
                isVisibleCombo.DataBind();

                RadComboBoxItem firstComboItem = isVisibleCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("IS_VISIBLE").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    isVisibleCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    isVisibleCombo.DefaultItem.Selected = true;
                }
            }

            RadComboBox isRequiredCombo = (RadComboBox)filterItem.FindControl("RadComboBoxIsRequired");
            if (isRequiredCombo != null)
            {
                isRequiredCombo.DataSource = GetRequiredOptions((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression); ;
                isRequiredCombo.DataValueField = "IS_REQUIRED";
                isRequiredCombo.DataTextField = "IS_REQUIRED";
                isRequiredCombo.DataBind();

                RadComboBoxItem firstComboItem = isVisibleCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("IS_REQUIRED").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    isRequiredCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    isRequiredCombo.DefaultItem.Selected = true;
                }
            }
            RadComboBox isReviewRequiredCombo = (RadComboBox)filterItem.FindControl("RadComboBoxIsReviewRequired");
            if (isReviewRequiredCombo != null)
            {
                isReviewRequiredCombo.DataSource = GetReviewRequiredOptions((DataSet)e.Item.OwnerTableView.DataSource, e.Item.OwnerTableView.FilterExpression); ;
                isReviewRequiredCombo.DataValueField = "IS_REVIEW_REQUIRED";
                isReviewRequiredCombo.DataTextField = "IS_REVIEW_REQUIRED";
                isReviewRequiredCombo.DataBind();

                RadComboBoxItem firstComboItem = isVisibleCombo.FindItemByValue(e.Item.OwnerTableView.GetColumn("IS_REVIEW_REQUIRED").CurrentFilterValue);
                if (firstComboItem != null)
                {
                    firstComboItem.Selected = true;
                }
                else
                {
                    isReviewRequiredCombo.DefaultItem = new Telerik.Web.UI.ComboBox.RadComboBoxDefaultItem();
                    isReviewRequiredCombo.DefaultItem.Selected = true;
                }
            }
            
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgRegPageSetting.DataSource = psc.SelectRegPageSettingAll(rgRegPageSetting.MasterTableView.FilterExpression);
        rgRegPageSetting.DataBind();
    }

    protected void rgRegPageSetting_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            foreach (GridColumn column in rgRegPageSetting.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            rgRegPageSetting.MasterTableView.FilterExpression = string.Empty;
            rgRegPageSetting.Rebind();
        }
    }

    protected void RadComboBoxEntity_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.ENTITY_TYPE_ID = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("ENTITY_TYPE_NAME");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();

    }
    
    protected void RadComboBoxProviderType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.PROVIDER_TYPE_ID = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("PROVIDER_TYPE_NAME");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();
    }
    
    protected void RadComboBoxPageType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.REG_PAGE_TYPE_ID = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("REG_PAGE_NAME");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();

    }
    
    protected void RadComboBoxAppType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.APPLICATION_TYPE_ID = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("APPLICATION_TYPE_NAME");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();
    }
    
    protected void RadComboBoxSectionType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.REG_SECTION_TYPE_ID = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("REG_SECTION_TYPE_NAME");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();
    }
    
    protected void RadComboBoxIsVisible_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.IS_VISIBLE = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("IS_VISIBLE");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();
    }
    
    protected void RadComboBoxIsRequired_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.IS_REQUIRED = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("IS_REQUIRED");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();
    }
    protected void RadComboBoxIsReviewRequired_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string filterExpression;
        filterExpression = "(RPS.IS_REVIEW_REQUIRED = '" + e.Value + "')";
        GridColumn column = rgRegPageSetting.MasterTableView.GetColumnSafe("IS_REVIEW_REQUIRED");
        column.CurrentFilterFunction = GridKnownFunction.EqualTo;
        column.CurrentFilterValue = e.Value;
        if (!string.IsNullOrEmpty(rgRegPageSetting.MasterTableView.FilterExpression))
            rgRegPageSetting.MasterTableView.FilterExpression += " AND ";
        rgRegPageSetting.MasterTableView.FilterExpression += filterExpression;
        rgRegPageSetting.MasterTableView.Rebind();
    }
    
}