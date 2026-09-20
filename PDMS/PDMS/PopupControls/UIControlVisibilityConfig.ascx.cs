using ClosedXML.Excel;
using Corp.Core.Libraries.Helper;
using FileHelpers;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_UIControlVisibilityConfig : System.Web.UI.UserControl
{
    public delegate void RegistrationViewEventHandler(int registrationId);
    public event RegistrationViewEventHandler RegistrationViewEvent;

    #region Properties
    private bool UserCanRapidAdminRegistrations
    {
        get
        {
            return Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name);
        }
    }

    private int SelectedRegID
    {
        get
        {
            return ViewState["SelectedRegID"] == null ? 0 : Convert.ToInt32(ViewState["SelectedRegID"]);
        }
        set
        {
            ViewState["SelectedRegID"] = value;
        }
    }


    private string RegOwnerTypeId
    {
        get
        {
            return ViewState["RegOwnerTypeId"] == null ? string.Empty : ViewState["RegOwnerTypeId"].ToString();
        }
        set
        {
            ViewState["RegOwnerTypeId"] = value;
        }
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
       // hdnWebAPIURLs.Value = ConfigurationManager.AppSettings["PDMSWebAPI"].ToString();
        CredentialHelper.APIToken apiToken = ApplicationCache.RestAPIAccessToken();

        if (apiToken != null)
        {
            hdnAccessTokens.Value = apiToken.AccessToken;
        }

        if (!Page.IsPostBack)
        {
            fnClearControls();
            PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
            DataSet sectionData = client.SelectAllSections();
            Helper.LoadDropDown(ddlPageName, sectionData.Tables[0], "REG_SECTION_TYPE_NAME", "REG_SECTION_TYPE_ID", true);

            LoadDataTypes();
            PopulateAspNetControls();

            SetMultiValueControlsVisibility(false);
        }
    }
    private void GetAppilicationReviewStatus()
    {
        //PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        //DataSet dsCodeSetStatus = svc.SelectC4CodeSetStatusByName(string.Empty);
        //if (dsCodeSetStatus != null && dsCodeSetStatus.Tables.Count > 0 && Helper.HasRows((dsCodeSetStatus.Tables[0])))
        //{
        //    lblApprovalStatus.Text = dsCodeSetStatus.Tables[0].Rows[0]["ApprovalStatus"].ToString();
        //    lblApprovalDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["LoadDate"].ToString();
        //    lblReviewDate.Text = dsCodeSetStatus.Tables[0].Rows[0]["ReviewDate"].ToString();
        //}
    }

    private void LoadDataTypes()
    {

        var sqlDataTypes = new List<string>
    {
        "Name",
        "First Name",
        "Middle Name",
        "Last Name",
        "Address 1",
        "Address 2",
        "Suite/Dept/Floor",
        "City",
        "State",
        "Quadrant",
        "Ward",
        "County",
        "Zip",
        "Ext Zip",
        "Phone Number 1",
        "Phone Ext 1",
        "Phone Number 2",
        "Phone Ext 2",
        "Effective Date",
        "End Date",
        "Fax number 1",
        "Fax number 2",
        "Contact Name",
        "Email Address 1",
        "Email 2",
        "Office Manager"
    };

        ddlControlID.DataSource = sqlDataTypes;
        ddlControlID.DataBind();

        // Optional: Add a default item
        ddlControlID.Items.Insert(0, new ListItem("-- Select Data Type --", ""));

    }

    private void PopulateAspNetControls()
    {
        var aspNetControls = new List<string>
    {
        "TextBox",
        "DropDownList",
        "CheckBox",
        "RadioButton"
        
    };

    }



    protected void lnkExcel_Click(object sender, ImageClickEventArgs e)
    {
        

    }


    protected void DynamicGrid1_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        //ViewState["DynamicGridData"] = e.NewPageIndex.ToString();
        //LoadDataForDynamicGrid1();
    }

    protected void DynamicGrid1_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        //LoadDataForDynamicGrid1();
    }

    protected void DynamicGrid1_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }



    private void LoadDataForDynamicGrid1()
    {
        //int totalResultCount = 0;
        //DynamicGrid1.Visible = true;
       
        //if (totalResultCount > 0)
        //{

        //}
        //else
        //{
            
        //    DynamicGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        //}
        //DynamicGrid1.DataBind();
        //DynamicGrid1.VirtualItemCount = totalResultCount;
    }

    protected void ddlSectionTypeId_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
       

    }


    protected void actionDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    protected void ddlDataTypeId_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void ddlControlTypeId_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bool isTextBox = ddlControlTypeId.SelectedValue == string.Empty || ddlControlTypeId.SelectedItem.Text == "TextBox";
        //SetMultiValueControlsVisibility(!isTextBox);
    }

    private void SetMultiValueControlsVisibility(bool visible)
    {
        //lblMultiValueOptions.Visible = visible;
        //txtOptions.Visible = visible;
        //txtOptions.Text = string.Empty;

        //lblSelectedValues.Visible = visible;
        //txtSelectedValues.Visible = visible;
        //txtSelectedValues.Text = string.Empty;

        //btnAppendValues.Visible = visible;
        //btnClearValues.Visible = visible;
    }

    private void AddError(string errMsg, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
    }



    protected void btnCelarValues_Click(object sender, EventArgs e)
    {

        txtRegId.Text = string.Empty;
        ddlPageName.ClearSelection();
        ddlControlID.ClearSelection();
        chkIsVisible.Checked = false;
        chkIsActive.Checked = false;
        
        lblError.Visible = false;
    }

    private string getControlName(string displayname, string datatype)
    {
        displayname = displayname.Replace(" ", "_");
        switch (datatype)
        {
            case "TextBox":
                displayname = string.Concat("Txt_", displayname);
                break;
            case "DropDownList":
                displayname = string.Concat("ddl_", displayname);
                break;
            case "CheckBox":
                displayname = string.Concat("Chk_", displayname);
                break;
            case "RadioButton":
                displayname = string.Concat("Rdb_", displayname);
                break;

        }
        return displayname;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        fnClearControls();
        //mpeConfirmAdd.Show();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();


        parms = new Dictionary<string, string>();

        // Collect values from your UI controls
        parms.Add("PageName", ddlPageName.SelectedItem.Text);
        parms.Add("ControlID", ddlControlID.SelectedValue);
        parms.Add("IsVisible", chkIsVisible.Checked.ToString());
        parms.Add("reg_id", txtRegId.Text.ToString());
        parms.Add("IsActive", chkIsVisible.Checked.ToString());




        // Audit fields
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


        // Call service to insert into DYNAMIC_FIELD_CONFIGURATION
        psc.InsertUIControlVisibilityConfig(parms);

        fnClearControls();
        mpeConfirmAdd1.Show();
    }

    protected void fnClearControls()
    {

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {

    }

}
