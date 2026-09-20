using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_ReferenceData_DelegatesUsers : System.Web.UI.UserControl
{

    private DataSet userIDData = null;
    private DataSet UserIDData
    {
        get
        {
            if (userIDData == null)
                userIDData = GetUserIDData();
            return userIDData;
        }
    }

    private DataSet GetUserIDData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        return psc.SelectUserIDDelegatesData();
    }

    protected void rgDelegates_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        rgDelegates.DataSource = UserIDData;
    }
    protected void rgDelegates_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem itemBP = (GridDataItem)e.Item;
            if (itemBP["ACTIVE_DELEGATE"].Text == "True")
            {
                GridDataItem itemBD = e.Item as GridDataItem;
                TableCell cellDate = itemBD["ACTIVE_DELEGATE"];
                cellDate.Text = "Yes";
            }
            else
            {
                GridDataItem itemBD = e.Item as GridDataItem;
                TableCell cellDate = itemBD["ACTIVE_DELEGATE"];
                cellDate.Text = "No";
            }


        }

        if (e.Item.IsInEditMode && e.Item is GridEditableItem)
        {
            if (e.Item.ItemIndex == -1)
            {
                // insert
                GridEditableItem item = e.Item as GridEditableItem;

            }
            else
            {
                // edit
                GridEditableItem item = e.Item as GridEditableItem;
                TextBox txtUserNameTB = (TextBox)item.FindControl("txtUserName"); // for TextBox in EditItemTemplate
                txtUserNameTB.Enabled = false;

                TextBox txtEmailTB = (TextBox)item.FindControl("txtEmail"); // for TextBox in EditItemTemplate
                txtEmailTB.Enabled = false;

                TextBox txtContactNameTB = (TextBox)item.FindControl("txtContact"); // for TextBox in EditItemTemplate
                txtContactNameTB.Enabled = false;
            }

        }
    }


    protected void rgDelegates_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);
        string delegateUserName = valuesToUpdate["USERNAME"].ToString();
        ValidateData(item);
        if (Page.IsValid)
        {
            DateTime lastModifiedDate = DateTime.Now;
            Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            Guid delegateUserID = Helper.GetUserId(valuesToUpdate["USERNAME"].ToString());
            string activeDelegate = valuesToUpdate["ACTIVE_DELEGATE"].ToString();
            string phoneMatch = valuesToUpdate["PHONE_MATCH"].ToString();
            bool activeDelegate1 = bool.Parse(activeDelegate);
            bool phoneMatch1 = bool.Parse(phoneMatch);
            DateTime createdDate = DateTime.Now;
            Guid createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            string contactName = valuesToUpdate["CONTACT_NAME"].ToString();
            string Emailaddr = valuesToUpdate["EMAIL"].ToString();
            psc.InsertDelegateUsers(delegateUserID, activeDelegate1, phoneMatch1, lastModifiedUser, lastModifiedDate,
                createdBy, createdDate, contactName, Emailaddr);
        }
        else
        {            
            e.Canceled = true;
        }    
    }


    protected void rgDelegates_InsertCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
        GridEditableItem gei = (GridEditableItem)e.Item;
        e.Item.OwnerTableView.ExtractValuesFromItem(valuesToUpdate, gei);        
        string delegateUserName = valuesToUpdate["USERNAME"].ToString();
        ValidateData(item);
        if (Page.IsValid)
        {           
            DateTime lastModifiedDate = DateTime.Now;
            Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            Guid delegateUserID = Helper.GetUserId(valuesToUpdate["USERNAME"].ToString());
            string activeDelegate = valuesToUpdate["ACTIVE_DELEGATE"].ToString();
            string phoneMatch = valuesToUpdate["PHONE_MATCH"].ToString();
            bool activeDelegate1 = bool.Parse(activeDelegate);
            bool phoneMatch1 = bool.Parse(phoneMatch);
            DateTime createdDate = DateTime.Now;
            Guid createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            string contactName = valuesToUpdate["CONTACT_NAME"].ToString();
            string Emailaddr = valuesToUpdate["EMAIL"].ToString();

            psc.InsertDelegateUsers(delegateUserID, activeDelegate1, phoneMatch1, lastModifiedUser, lastModifiedDate,
                createdBy, createdDate, contactName, Emailaddr);
        }
        else
        {
            e.Canceled = true;
        }        
    }


    private void ValidateData(GridEditableItem item)
    {
        bool isValid = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        TextBox txtUserName = item.FindControl("txtUserName") as TextBox;
        if (!psc.CheckDelegateUserIDisActive(txtUserName.Text))
        {
            AddError("User ID is not Active.", ref isValid);
        }
    }

    private void AddError(string msg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "DelegateUserIDValidation";
        this.Page.Validators.Add(val);
        isGood = false;
    }
}