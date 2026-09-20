using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_RegPageSettingAction : System.Web.UI.UserControl
{
    protected void rgRegPageSettingAction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgRegPageSettingAction.DataSource = psc.SelectRegPageSettingActionAll();
    }
    protected void rgRegPageSettingAction_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        string regPageName = valuesToUpdate["REG_PAGE_NAME"] as string;
        string roleName = valuesToUpdate["ROLE_NAME"] as string;
        bool takeActionAllowed = (bool)valuesToUpdate["TAKE_ACTION_ALLOWED"];
        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertRegPageSettingAction(regPageName, roleName, takeActionAllowed, lastModifiedDate, lastModifiedUser);
    }
    protected void rgRegPageSettingAction_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        int regPageSettingActionID = int.Parse((gei).GetDataKeyValue("REG_PAGE_SETTING_ACTION_ID").ToString());
        string regPageName = valuesToUpdate["REG_PAGE_NAME"] as string;
        string roleName = valuesToUpdate["ROLE_NAME"] as string;
        bool takeActionAllowed = (bool)valuesToUpdate["TAKE_ACTION_ALLOWED"];
        DateTime lastModifiedDate = DateTime.Now;
        Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateRegPageSettingAction(regPageSettingActionID,regPageName, roleName, takeActionAllowed, lastModifiedDate, lastModifiedUser);
    }
}