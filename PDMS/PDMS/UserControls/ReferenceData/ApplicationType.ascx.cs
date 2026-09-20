using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_ApplicationType : System.Web.UI.UserControl
{

    protected void rgApplicationType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgApplicationType.DataSource = psc.SelectAllApplicationTypes();
      
    }
    protected void rgApplicationType_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("IS_USED_IN_MMIS", ((gei).FindControl("chkIsUsedInMMIS") as CheckBox).Checked);
        valuesToUpdate.Add("IsVisible", ((gei).FindControl("chkIsVisible") as CheckBox).Checked);

        string providerCategoryTypeName = valuesToUpdate["APPLICATION_TYPE_NAME"] as string;
        string applicationTypeDescription = valuesToUpdate["APPLICATION_TYPE_DESC"] as string;
        bool isUsedInMMIS= (bool)valuesToUpdate["IS_USED_IN_MMIS"];     
        string mmisApplicationTypeID = valuesToUpdate["MMIS_APPLICATION_TYPE_ID"] as string;
        bool isVisible = (bool)valuesToUpdate["IsVisible"];
        
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        psc.InsertApplicationType(providerCategoryTypeName, applicationTypeDescription, isUsedInMMIS, mmisApplicationTypeID,
            lastModifiedDate, lastModifiedUser, isVisible);
    }
    protected void rgApplicationType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        valuesToUpdate.Add("IS_USED_IN_MMIS", ((gei).FindControl("chkIsUsedInMMIS") as CheckBox).Checked);
        valuesToUpdate.Add("IsVisible", ((gei).FindControl("chkIsVisible") as CheckBox).Checked);


        int applicationTypeID = int.Parse((gei).GetDataKeyValue("APPLICATION_TYPE_ID").ToString());
        string applicationTypeName = valuesToUpdate["APPLICATION_TYPE_NAME"] as string;
        string applicationTypeDescription = valuesToUpdate["APPLICATION_TYPE_DESC"] as string;
        bool isUsedInMMIS= (bool)valuesToUpdate["IS_USED_IN_MMIS"];     
        string mmisApplicationTypeID = valuesToUpdate["MMIS_APPLICATION_TYPE_ID"] as string;
        bool isVisible = (bool)valuesToUpdate["IsVisible"];
        
        DateTime lastModifiedDate = DateTime.Now;
        string lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        psc.UpdateApplicationType(applicationTypeID, applicationTypeName, applicationTypeDescription, isUsedInMMIS, mmisApplicationTypeID,
            lastModifiedDate, lastModifiedUser, isVisible); 
    }
    protected void rgApplicationType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //if (e.CommandName == RadGrid.EditCommandName)
        //{
        //    if (((DataRowView)e.Item.DataItem)["IS_USED_IN_MMIS"] == null)
        //    {
        //        e.Canceled = true;
        //        System.Collections.Specialized.ListDictionary newValues = new System.Collections.Specialized.ListDictionary();
        //        newValues["IS_USED_IN_MMIS"] = false;
        //        e.Item.OwnerTableView.InsertItem(newValues);
        //    }
        //}
    }
}