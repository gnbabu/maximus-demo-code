using ClosedXML.Excel;
using Corp.Core.Libraries.Helper;
using FileHelpers;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PNMDynamicFields : System.Web.UI.UserControl
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
        if (!Page.IsPostBack)
        {
            fnClearControls();
            PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
            DataSet sectionData = client.SelectAllSections();
            Helper.LoadDropDown(ddlSectionTypeId, sectionData.Tables[0], "REG_SECTION_TYPE_NAME", "REG_SECTION_TYPE_ID", true);
            DataSet ds = client.SelectProviderTypesPublicSearch();
            Helper.LoadDropDown(ddlProvTypeId, ds.Tables[0], "PROVIDER_TYPE_NAME", "MMIS_PROVIDER_TYPE_ID", true);
            LoadDataTypes();
            PopulateAspNetControls();
            GetAppilicationReviewStatus();

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
        "INT",
        "VARCHAR",
        "TEXT",
        "DATE",
        "DATETIME",
        "BIT"
    };

        ddlDataTypeId.DataSource = sqlDataTypes;
        ddlDataTypeId.DataBind();

        // Optional: Add a default item
        ddlDataTypeId.Items.Insert(0, new ListItem("-- Select Data Type --", ""));

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

        ddlControlTypeId.DataSource = aspNetControls;
        ddlControlTypeId.DataBind();

        // Optional: Add a default item
        ddlControlTypeId.Items.Insert(0, new ListItem("-- Select Control Types--", ""));
    }



    protected void lnkExcel_Click(object sender, ImageClickEventArgs e)
    {


    }


    protected void DynamicGrid1_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        ViewState["DynamicGridData"] = e.NewPageIndex.ToString();
        LoadDataForDynamicGrid1();
    }

    protected void DynamicGrid1_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        LoadDataForDynamicGrid1();
    }

    protected void DynamicGrid1_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }



    private void LoadDataForDynamicGrid1()
    {
        int totalResultCount = 0;
        DynamicGrid1.Visible = true;

        if (totalResultCount > 0)
        {

        }
        else
        {

            DynamicGrid1.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        }
        DynamicGrid1.DataBind();
        DynamicGrid1.VirtualItemCount = totalResultCount;
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
        bool isTextBox = ddlControlTypeId.SelectedValue == string.Empty || ddlControlTypeId.SelectedItem.Text == "TextBox";
        bool isRadioButton = ddlControlTypeId.SelectedValue == string.Empty || ddlControlTypeId.SelectedItem.Text == "RadioButton";
        SetMultiValueControlsVisibility(!isTextBox);

        if (isRadioButton)
        {
            DataSet dataSet = LookupTableController.SelectDynamicFieldChildControls();

            ddlChildControlIDs.DataSource = dataSet.Tables[0];
            ddlChildControlIDs.DataTextField = "control_type_id";
            ddlChildControlIDs.DataValueField = "DYNAMIC_FIELD_CONFIGURATION_ID";
            ddlChildControlIDs.DataBind();

            ddlChildControlIDs.Items.Insert(0, new ListItem("-- Select Child Control --", ""));
        }

    }

    private void SetMultiValueControlsVisibility(bool visible)
    {
        lblMultiValueOptions.Visible = visible;
        txtOptions.Visible = visible;
        txtOptions.Text = string.Empty;

        lblSelectedValues.Visible = visible;
        txtSelectedValues.Visible = visible;
        txtSelectedValues.Text = string.Empty;

        btnAppendValues.Visible = visible;
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

    protected void btnAppendValues_Click(object sender, EventArgs e)
    {
        string newText = txtOptions.Text.Trim();

        if (string.IsNullOrEmpty(newText))
            return;

        // Build formatted value using string.Format
        string formattedValue = !string.IsNullOrEmpty(ddlChildControlIDs.SelectedValue) ? string.Format("{0}:{1}", newText, ddlChildControlIDs.SelectedValue) : newText;


        // Append with newline if existing text is present
        if (!string.IsNullOrEmpty(txtSelectedValues.Text))
        {
            txtSelectedValues.Text += Environment.NewLine + formattedValue;
        }
        else
        {
            txtSelectedValues.Text = formattedValue;
        }

        // Clear input box
        txtOptions.Text = string.Empty;
    }



    protected void btnCelarValues_Click(object sender, EventArgs e)
    {

        // Clear textboxes
        txtFieldName.Text = string.Empty;
        txtOptions.Text = string.Empty;
        txtSelectedValues.Text = string.Empty;

        // Reset dropdownlists to first/default item
        if (ddlProvTypeId.Items.Count > 0)
            ddlProvTypeId.SelectedIndex = 0;
        if (ddlSectionTypeId.Items.Count > 0)
            ddlSectionTypeId.SelectedIndex = 0;
        if (ddlDataTypeId.Items.Count > 0)
            ddlDataTypeId.SelectedIndex = 0;
        if (ddlControlTypeId.Items.Count > 0)
            ddlControlTypeId.SelectedIndex = 0;

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

    protected void btnClear_Click(object sender, EventArgs e)
    {
        fnClearControls();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError.Visible = false;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();

        parms.Add("REG_ID", txtRegID.Text.Trim());
        parms.Add("FIELD_NAME", txtFieldName.Text.Trim());
        parms.Add("PROVIDER_TYPE_ID", ddlProvTypeId.SelectedValue);
        parms.Add("SECTION_TYPE_ID", ddlSectionTypeId.SelectedValue);
        DataSet ds = psc.SelectDynamicFieldDisplayName(parms);
        if (Helper.HasRows(ds) && ds.Tables[0].Rows[0][0].ToString() == "1")
        {
            lblError.Text = "The field name '" + txtFieldName.Text.Trim() + "' already exists. Please choose a different name.";
            lblError.Visible = true;
            //// Create a new CustomValidator
            //CustomValidator customValidator = new CustomValidator
            //{
            //    IsValid = false,
            //    ErrorMessage = "The field name '" + txtFieldName.Text.Trim() + "' already exists. Please choose a different name.",
            //    ValidationGroup = "DynamicControlFix"
            //};

            //// Add it to the page's validators collection
            //Page.Validators.Add(customValidator);

            return;
        }


        var values = txtSelectedValues.Text
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        var jsonObject = new
        {
            Specialties = values
        };

        var serializer = new JavaScriptSerializer();
        string JSONSelectValues = serializer.Serialize(jsonObject);
        string controlid = getControlName(txtFieldName.Text.Trim(), ddlControlTypeId.SelectedValue);
        // Create an object to hold form data
        var formData = new
        {
            FieldName = txtFieldName.Text.Trim(),
            ControlId = controlid,
            SectionType = ddlSectionTypeId.SelectedValue,
            ControlType = ddlControlTypeId.SelectedValue,
            SelectValues = JSONSelectValues,
            MMISProviderTypeID = ddlProvTypeId.SelectedValue,
            DataType = ddlDataTypeId.SelectedValue,
            Options = txtOptions.Text.Trim()
        };

        // Convert to JSON
        string jsonResult = JsonConvert.SerializeObject(formData, Formatting.Indented);

        parms = new Dictionary<string, string>();

        // Collect values from your UI controls
        parms.Add("mmis_Provider_type_id", ddlProvTypeId.SelectedValue);
        parms.Add("reg_section_type_id", ddlSectionTypeId.SelectedValue);
        parms.Add("control_type_id", ddlControlTypeId.SelectedValue);
        parms.Add("control_id", controlid);
        parms.Add("data_type", ddlDataTypeId.SelectedValue);
        parms.Add("control_level_id", ckbParentChildControl.Checked ? "1" : "0");

        // Multi-value JSON (from Select Values textbox)
        parms.Add("multivalue_json", txtSelectedValues.Text.Trim());

        // Complete JSON (from all fields combined)
        var completeJson = new
        {
            FieldName = txtFieldName.Text.Trim(),
            ControlId = controlid,
            SectionType = ddlSectionTypeId.SelectedValue,
            ControlType = ddlControlTypeId.SelectedValue,
            SelectValues = txtSelectedValues.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries),
            MMISProviderTypeID = ddlProvTypeId.SelectedValue,
            DataType = ddlDataTypeId.SelectedValue,
            Options = txtOptions.Text.Trim()
        };

        parms.Add("complete_json", Newtonsoft.Json.JsonConvert.SerializeObject(completeJson));

        // Audit fields
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("IsActive", "0"); // Assuming active by default

        // Call service to insert into DYNAMIC_FIELD_CONFIGURATION
        psc.InsertDynamicFieldConfiguration(parms);

        fnClearControls();
        mpeConfirmAdd.Show();
    }

    protected void fnClearControls()
    {
        txtFieldName.Text = string.Empty;
        ddlSectionTypeId.SelectedIndex = -1;
        ddlControlTypeId.SelectedIndex = -1;
        ddlProvTypeId.SelectedIndex = -1;
        ddlDataTypeId.SelectedIndex = -1;
        txtOptions.Text = string.Empty;
        txtSelectedValues.Text = string.Empty;

        lblError.Text = "";
        lblError.Visible = false;
        SetMultiValueControlsVisibility(false);
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        txtSelectedValues.Text = string.Empty;
        txtOptions.Text = string.Empty;

        lblError.Text = "";
        lblError.Visible = false;

        mpeConfirmAdd.Hide();
    }

}
