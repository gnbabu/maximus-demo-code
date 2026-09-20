using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Web;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_AppSettings : System.Web.UI.UserControl
{
    protected void rgAppSettings_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        rgAppSettings.DataSource = psc.SelectAppSettingsAll();
    }
    protected void rgAppSettings_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        string appSettingsKey = valuesToUpdate["AppSettingsKey"] as string;
        string appSettingsValue = valuesToUpdate["AppSettingsValue"] as string;
        bool appSettingsReadOnly = (bool)valuesToUpdate["AppSettingsReadOnly"];
        DateTime lastUpdatedDate = DateTime.Now;
        Guid lastActivityUserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        string appSettingsNotes = valuesToUpdate["AppSettingsNotes"] as string;
        bool appSettingsEnvironSpecific = (bool)valuesToUpdate["AppSettingsEnvironSpecific"];
        bool CanOverwrite = (bool)valuesToUpdate["CanOverwrite"];

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertAppSettings(appSettingsKey, appSettingsValue, appSettingsReadOnly, lastUpdatedDate, lastActivityUserID, appSettingsNotes, appSettingsEnvironSpecific, CanOverwrite);
    }
    protected void rgAppSettings_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);

        string appSettingsKey = valuesToUpdate["AppSettingsKey"] as string;
        string appSettingsValue = valuesToUpdate["AppSettingsValue"] as string;
        bool appSettingsReadOnly = (bool)valuesToUpdate["AppSettingsReadOnly"];
        DateTime lastUpdatedDate = DateTime.Now;
        Guid lastActivityUserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        string appSettingsNotes = valuesToUpdate["AppSettingsNotes"] as string;
        bool appSettingsEnvironSpecific = (bool)valuesToUpdate["AppSettingsEnvironSpecific"];
        bool CanOverwrite = (bool)valuesToUpdate["CanOverwrite"];

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.UpdateAppSettings(appSettingsKey, appSettingsValue, appSettingsReadOnly, lastUpdatedDate, lastActivityUserID, appSettingsNotes, appSettingsEnvironSpecific, CanOverwrite);
        AppSettings.Remove(appSettingsKey);
    }

  
}