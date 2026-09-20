using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Maintenance_ConfigurePages : System.Web.UI.Page
{
    private const int pageSize = 10;

    private int EditIndex
    {
        get { return (int)ViewState["EditIndex"]; }
        set { ViewState["EditIndex"] = value; }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.MasterPageFile = "~/MasterPage.master";
            //Page.MasterPageFile = "~/MasterPageNew.master";
            Page.Theme = "Modernization";
        }
        else
        {
            Page.MasterPageFile = "~/MasterPage.master";
            Page.Theme = "Default";
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            EditIndex = -1;

        }
        LoadData();
        if (Helper.IsModern())
        {

        }
        //UMS04.Style["disabled"] = UMS02.Text.Length + UMS08.Text.Length + txtSearchOrgName.Text.Length > 0 && cboxSearchRole.SelectedIndex > 0 ? "true" : "false";
    }





    private void LoadData()
    {
        List<string> roles = Roles.GetAllRoles().ToList();
        roles.Insert(0, "All");
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //gvUsers.DataSource = Membership.GetAllUsers();
        //gvUsers.DataBind();
        DataSet ds = new DataSet();
        ds = psc.SelectPageConfigurations();

        rgConfigurePages.DataSource = ds;
        //rgConfigurePages.DataBind();

    }







    private void SaveData()
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }

    private void LoadUser(string username)
    {
        Response.Redirect(@"~/Process/AdminUserAccounts.aspx?username=" + username);
    }



    private bool ValidateData()
    {

        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateData()) return;
        SaveData();
        LoadData();
    }











    protected void Page_PreRender(object sender, EventArgs e)
    {
        //TELERIK BUG WORKAROUND
        //Hiding table headers in IE for Data Pager and Command Item in code behind
        List<TableHeaderCell> headersToCheck = new List<TableHeaderCell>();
        foreach (Control c in this.Controls)
        {
            IEnumerable<TableHeaderCell> headersToAdd = GetControls(c).OfType<TableHeaderCell>();
            if (headersToAdd != null)
                headersToCheck.AddRange(headersToAdd);
        }
        foreach (TableHeaderCell th in headersToCheck)
        {
            if (th.Text == "Data pager" || th.Text == "Command item")
                th.Visible = false;
        }

    }

    protected void rgConfigurePages_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        ds = psc.SelectPageConfigurations();

        if (!Helper.HasRows(ds))
        {
            rgConfigurePages.DataSource = new string[] { };
        }
        else
        {
            rgConfigurePages.DataSource = ds;
        }
    }
    private IEnumerable<Control> GetControls(Control parent)
    {
        foreach (Control parentControl in parent.Controls)
        {
            yield return parentControl;
            foreach (Control childControl in GetControls(parentControl))
            {
                yield return childControl;
            }
        }
    }

    protected void rgConfigurePages_InsertCommand(object sender, GridCommandEventArgs e)
    {
        Page.Validate("ConfigurePagesValidation");

        if (Page.IsValid)
        {
            GridEditableItem editItem = e.Item as GridEditableItem;

            TextBox txtPageName = (TextBox)editItem.FindControl("txtPageName");
            TextBox txtSectionName = (TextBox)editItem.FindControl("txtSectionName");
            TextBox txtLINK_TEXT = (TextBox)editItem.FindControl("txtLINK_TEXT");
            DropDownList ddlShowAsLinkOrText = (DropDownList)editItem.FindControl("ddlShowAsLinkOrText");
            TextBox txtREFERENCE_PATH = (TextBox)editItem.FindControl("txtREFERENCE_PATH");
            TextBox txtDisplayOrder = (TextBox)editItem.FindControl("txtDisplayOrder");
            UserControls_UploadDocumentControl ucUploadDocumentControl = (UserControls_UploadDocumentControl)editItem.FindControl("ucUploadDocumentControl");
            Dictionary<string, string> parms = new Dictionary<string, string>();





            int section_display_order = 0;
            if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
            {
                section_display_order = Convert.ToInt32(txtDisplayOrder.Text);
            }
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();






            int docId = ucUploadDocumentControl.DocumentId;

            svc.InsertPage_Configuration(txtPageName.Text, txtSectionName.Text, txtLINK_TEXT.Text.Replace(Environment.NewLine, "</br>"), ddlShowAsLinkOrText.SelectedValue.ToString(), txtREFERENCE_PATH.Text, docId, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), section_display_order,"");
        }
    }
    //}

    protected void rgConfigurePages_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        Page.Validate("ConfigurePagesValidation");

        if (Page.IsValid)
        {
            GridEditableItem editItem = e.Item as GridEditableItem;

            TextBox txtPageName = (TextBox)editItem.FindControl("txtPageName");
            TextBox txtSectionName = (TextBox)editItem.FindControl("txtSectionName");
            TextBox txtLINK_TEXT = (TextBox)editItem.FindControl("txtLINK_TEXT");
            DropDownList ddlShowAsLinkOrText = (DropDownList)editItem.FindControl("ddlShowAsLinkOrText");
            TextBox txtREFERENCE_PATH = (TextBox)editItem.FindControl("txtREFERENCE_PATH");
            TextBox txtDisplayOrder = (TextBox)editItem.FindControl("txtDisplayOrder");
            UserControls_UploadDocumentControl ucUploadDocumentControl = (UserControls_UploadDocumentControl)editItem.FindControl("ucUploadDocumentControl");
            Dictionary<string, string> parms = new Dictionary<string, string>();





            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();



            int section_display_order = 0;
            if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
            {
                section_display_order = Convert.ToInt32(txtDisplayOrder.Text);
            }


            int docId = ucUploadDocumentControl.DocumentId;
            int Page_Configuration_Id = Convert.ToInt32(editItem.GetDataKeyValue("PAGE_CONFIGURATION_ID"));
            string displaytext = "";
            svc.UpdatePage_Configuration(Page_Configuration_Id, txtPageName.Text, txtSectionName.Text, txtLINK_TEXT.Text.Replace(Environment.NewLine, "</br>"), ddlShowAsLinkOrText.SelectedValue.ToString(), txtREFERENCE_PATH.Text, docId, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), section_display_order, displaytext);
        }
    }

    protected void rgConfigurePages_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            InitializeDropDowns((GridEditableItem)e.Item);
        }
    }
    private void InitializeDropDowns(GridEditableItem edit)
    {
        DropDownList ddlShowAsLinkOrText = (DropDownList)edit.FindControl("ddlShowAsLinkOrText");

        if (ddlShowAsLinkOrText.Items.Count == 0)
        {
            ddlShowAsLinkOrText.Items.Add(new ListItem("Select", ""));
            ddlShowAsLinkOrText.Items.Add(new ListItem("Link", "Link"));
            ddlShowAsLinkOrText.Items.Add(new ListItem("Text", "Text"));
            ddlShowAsLinkOrText.Items.Add(new ListItem("Reference Link", "Reference Link"));
        }


    }

    protected void rgConfigurePages_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            UserControls_UploadDocumentControl ucDoc = (UserControls_UploadDocumentControl)item.FindControl("ucUploadDocumentControl");
            if (item.ItemIndex >= 0 && !string.IsNullOrEmpty(item.OwnerTableView.DataKeyValues[item.ItemIndex]["document_id"].ToString()) && item.OwnerTableView.DataKeyValues[item.ItemIndex]["document_id"].ToString() != "0")
            {
                ucDoc.DocumentId = Convert.ToInt32(item.OwnerTableView.DataKeyValues[item.ItemIndex]["document_id"]);
                ucDoc.FileName = item.OwnerTableView.DataKeyValues[item.ItemIndex]["file_name"].ToString();
                ucDoc.Title = item.OwnerTableView.DataKeyValues[item.ItemIndex]["file_name"].ToString();
                ucDoc.Description = "";
                ucDoc.Loadfile();
            }
        }
    }

    protected void rgConfigurePages_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            GridEditableItem editItem = e.Item as GridEditableItem;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int PageConfigurationID = Convert.ToInt32(editItem.GetDataKeyValue("PAGE_CONFIGURATION_ID"));
            psc.DeletePageConfiguration(PageConfigurationID);
        }
    }
}