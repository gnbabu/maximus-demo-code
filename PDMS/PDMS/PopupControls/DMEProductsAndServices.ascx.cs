using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_DMEProductsAndServices : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }
    #endregion

    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }

    public int RegDMEProductsAndServicesInfoID
    {
        get
        {
            return ViewState["RegDMEProductsAndServicesInfoID"] == null ? 0 : Convert.ToInt32(ViewState["RegDMEProductsAndServicesInfoID"]);
        }
        set
        {
            ViewState["RegDMEProductsAndServicesInfoID"] = value;
        }
    }

    public int RegDMEProductsAndServicesID
    {
        get
        {
            return ViewState["RegDMEProductsAndServicesID"] == null ? 0 : Convert.ToInt32(ViewState["RegDMEProductsAndServicesID"]);
        }
        set
        {
            ViewState["RegDMEProductsAndServicesID"] = value;
        }
    }
    public int RegDMEProductsAndServicesSubID
    {
        get
        {
            return ViewState["RegDMEProductsAndServicesSubID"] == null ? 0 : Convert.ToInt32(ViewState["RegDMEProductsAndServicesSubID"]);
        }
        set
        {
            ViewState["RegDMEProductsAndServicesSubID"] = value;
        }
    }

    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();

    public delegate void KeepOpenEventHandler();
    public event KeepOpenEventHandler KeepOpenEvent;

    public delegate void SaveEventHandler();

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.DME) // TODO: EDV Check this is correct
                LoadProductsServiceCategory();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    public bool ValidateData()
    {
        bool isValid = false;
        int flag = 0;
        const string validationGroup = "valDMEProductsAndServices";
        if (chkQualifications.Visible)
        {

            for (int i = 0; i < chkQualifications.Items.Count; i++)
            {

                if (chkQualifications.Items[i].Selected)
                    flag++;
            }
            if (flag == 0)
            {
                isValid = AddError("Please select at least one qualification.", validationGroup);
            }
        }
        flag = 0;
        if (chkSubCategory.Visible)
        {
            for (int i = 0; i < chkSubCategory.Items.Count; i++)
            {

                if (chkSubCategory.Items[i].Selected)
                    flag++;
            }
            if (flag == 0)
            {
                isValid = AddError("Please select at least one sub category.", validationGroup);
            }
        }
        if (string.IsNullOrWhiteSpace(txtQualifications.Text) && txtQualifications.Visible)
        {
            isValid = AddError("Please enter a qualification.", validationGroup);
        }

        return isValid;
    }



    #endregion

    #region Private Methods

    private bool AddError(string errorMessage, string validationGroup)
    {
        var validator = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = errorMessage,
            ValidationGroup = validationGroup
        };
        this.Page.Validators.Add(validator);
        return false;
    }

    #endregion

    #region Public Methods

    public void LoadData(int categoryTypeID)
    {
        if (categoryTypeID > 0)
        {
            LoadProductsServiceCategory();
            ddlProductsServices.SelectedValue = categoryTypeID.ToString();

            LoadProductsSubServiceCategory(categoryTypeID, true, true);
            RegDMEProductsAndServicesID = categoryTypeID;
        }
    }

    #endregion

    public void InitializeFields()
    {
        LoadProductsServiceCategory();
        chkSubCategory.Visible = false;
        chkQualifications.Visible = false;
        txtQualifications.Visible = false;
        lblQualifications.Visible = lblQualificationscomment.Visible = lblSubCategory.Visible = false;


        RegDMEProductsAndServicesInfoID = 0;
        RegDMEProductsAndServicesID = 0;
        RegDMEProductsAndServicesSubID = 0;

    }

    private void LoadProductsServiceCategory()
    {
        // TODO: EDV Put this in cache
        DataSet ds = svc.SelectProductServiceCategoryType();
        Helper.LoadDropDown(ddlProductsServices, ds.Tables[0], "DME_PRODUCT_SERVICE_CATEGORY_TYPE_NAME", "DME_PRODUCT_SERVICE_CATEGORY_TYPE_ID", true);
    }


    private void LoadProductsSubServiceCategory(int categoryTypeID, bool isChkSubCategorySelected = false, bool isQualification = false)
    {
        DataSet ds = svc.SelectProductServiceSubCategoryType(categoryTypeID);
        chkSubCategory.Items.Clear();
        chkQualifications.Items.Clear();
        chkQualifications.Visible = false;
        txtQualifications.Visible = false;
        lblQualifications.Visible = lblQualificationscomment.Visible = false;
        lblSubCategory.Visible = false;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_DME_PRODUCT_CATEGORY_TYPE_ID", categoryTypeID.ToString());
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

        DataSet ds2 = svc.SelectRegistrationDataWithParams("usp_SelectREG_DME_PRODUCT_CATEGORY_INFO_ByID", parms);

        if (Helper.HasRows(ds))
        {
            chkSubCategory.DataSource = ds;
            chkSubCategory.DataTextField = "DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_NAME";
            chkSubCategory.DataValueField = "DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_ID";
            chkSubCategory.DataBind();
            chkSubCategory.Visible = true;
            lblSubCategory.Visible = true;

            if (Helper.HasRows(ds2))
            {
                if (isChkSubCategorySelected)
                {
                    for (int i = 0; i < chkSubCategory.Items.Count; i++)
                    {
                        if (ds2.Tables[0].Select("DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_ID = '" + chkSubCategory.Items[i].Value + "'").Length > 0)
                        {
                            chkSubCategory.Items[i].Selected = true;
                        }
                    }
                }
                RegDMEProductsAndServicesInfoID = Helper.GetInt("REG_DME_PRODUCT_CATEGORY_INFO_ID", ds2.Tables[0].Rows[0]);
            }
        }
        else
        {
            chkSubCategory.Visible = false;
        }

        DataSet ds1 = svc.SelectDMEQualificationType(Convert.ToInt32(ddlProductsServices.SelectedValue));
        if (Helper.HasRows(ds1))
        {
            chkQualifications.DataSource = ds1;
            chkQualifications.DataTextField = "DME_QUALIFICATION_TYPE_NAME";
            chkQualifications.DataValueField = "DME_QUALIFICATION_TYPE_ID";
            chkQualifications.DataBind();
            chkQualifications.Visible = true;
            lblQualifications.Visible = true;

            if (Helper.HasRows(ds2))
            {
                for (int i = 0; i < chkQualifications.Items.Count; i++)
                {
                    if (ds2.Tables[0].Select("DME_QUALIFICATION_TYPE_ID = '" + chkQualifications.Items[i].Value + "'").Length > 0)
                        chkQualifications.Items[i].Selected = true;
                }
            }
            RegDMEProductsAndServicesInfoID = Helper.GetInt("REG_DME_PRODUCT_CATEGORY_INFO_ID", ds2.Tables[0].Rows[0]);

        }
        else
        {
            txtQualifications.Visible = true;
            rfvtxtQualifications.Visible = true;
            lblQualifications.Visible = true;
            lblQualificationscomment.Visible = true;
            txtQualifications.Text = "";
            if (Helper.HasRows(ds2) && isQualification)
            {
                txtQualifications.Text = Helper.GetString("DME_QUALIFICATION_TYPE", ds2.Tables[0].Rows[0]);
                RegDMEProductsAndServicesInfoID = Helper.GetInt("REG_DME_PRODUCT_CATEGORY_INFO_ID", ds2.Tables[0].Rows[0]);
            }
        }
    }

    private void updateProductCategoryInfo(string RegId, string CategoryTypeId, string QualificationType, string RegDMEProductsAndServicesInfoID)

    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", RegId);
        parms.Add("REG_DME_PRODUCT_CATEGORY_INFO_ID", RegDMEProductsAndServicesInfoID);
        parms.Add("DME_PRODUCT_SERVICE_CATEGORY_TYPE_ID", CategoryTypeId);
        parms.Add("DME_QUALIFICATION_TYPE", QualificationType);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        psc.UpdateRegistrationDataTable("DME_PRODUCT_CATEGORY_INFO", parms);

    }


    private void InsertProductCategoryInfo(string RegId, string CategoryTypeId, string CategorySubTypeId, string QualificationTypeId,
        string QualificationType)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        parms.Add("REG_ID", RegId);
        parms.Add("DME_PRODUCT_SERVICE_CATEGORY_TYPE_ID", CategoryTypeId);
        if (CategorySubTypeId != "")
            parms.Add("DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_ID", CategorySubTypeId);

        if (QualificationTypeId != "")
            parms.Add("DME_QUALIFICATION_TYPE_ID", QualificationTypeId);

        if (QualificationType != "")
            parms.Add("DME_QUALIFICATION_TYPE", QualificationType);

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());

        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        psc.InsertRegistrationDataTable("DME_PRODUCT_CATEGORY_INFO", parms);

    }



    public bool SaveData()
    {
        bool rtn = true;
        Page.Validate("valDMEProductsAndServices");
        this.ValidateData();

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return false;
        }

        string RegId = this.WorkflowPage.RegistrationId.ToString();
        string RegDMEProductsAndServicesCatTypeID = "";

        if (ddlProductsServices.SelectedItem.Text != "")
        {
            RegDMEProductsAndServicesCatTypeID = ddlProductsServices.SelectedValue.ToString();
        }

        //if product service category is changed in edit mode then delete the existing one
        if (RegDMEProductsAndServicesInfoID != 0)
        {
            if (RegDMEProductsAndServicesID != Convert.ToInt32(ddlProductsServices.SelectedValue))
            {
                //delete product service category
                svc.DeleteRegDMEProductServiceCategoryByRegID(Convert.ToInt32(RegId), RegDMEProductsAndServicesID);
            }
            if (txtQualifications.Visible)
            {
                if (RegDMEProductsAndServicesID == Convert.ToInt32(RegDMEProductsAndServicesCatTypeID))
                { //update

                    this.updateProductCategoryInfo(RegId, RegDMEProductsAndServicesCatTypeID, txtQualifications.Text.ToString(), RegDMEProductsAndServicesInfoID.ToString());
                }
                else
                {
                    this.InsertProductCategoryInfo(RegId, RegDMEProductsAndServicesCatTypeID, "", "", txtQualifications.Text.ToString());
                }
            }
        }
        else
        {

            if (txtQualifications.Visible)
            {
                this.InsertProductCategoryInfo(RegId, RegDMEProductsAndServicesCatTypeID, "", "", txtQualifications.Text.ToString());
            }
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();

        parms.Add("REG_ID", RegId);
        parms.Add("REG_DME_PRODUCT_CATEGORY_TYPE_ID", RegDMEProductsAndServicesCatTypeID);
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_DME_PRODUCT_CATEGORY_INFO_ByID", parms);

        DataTable tblExistingDME_PRODUCT_CATEGORY_INFO = null;
        tblExistingDME_PRODUCT_CATEGORY_INFO = ds.Tables[0];

        if (chkSubCategory.Visible)
        {
            for (int i = 0; i < chkSubCategory.Items.Count; i++)
            {
                DataRow[] dr = tblExistingDME_PRODUCT_CATEGORY_INFO.Select("DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_ID = '" + chkSubCategory.Items[i].Value + "'");

                if (chkSubCategory.Items[i].Text != "" && chkSubCategory.Visible && chkSubCategory.Items[i].Selected && dr.Length == 0)
                {
                    this.InsertProductCategoryInfo(RegId, RegDMEProductsAndServicesCatTypeID, chkSubCategory.Items[i].Value.ToString(), "", "");
                }
                else if (chkSubCategory.Items[i].Selected == false && dr.Length != 0)
                { //delete
                    psc.DeleteRegDMEProductServiceSubCategoryByRegID(Convert.ToInt32(RegId), RegDMEProductsAndServicesID, Convert.ToInt32(chkSubCategory.Items[i].Value));
                }
            }
        }

        if (chkQualifications.Visible)
        {
            for (int i = 0; i < chkQualifications.Items.Count; i++)
            {

                DataRow[] dr1 = ds.Tables[0].Select("DME_QUALIFICATION_TYPE_ID = '" + chkQualifications.Items[i].Value + "'");

                if (chkQualifications.Items[i].Text != "" && chkQualifications.Items[i].Selected && dr1.Length == 0)
                {
                    this.InsertProductCategoryInfo(RegId, RegDMEProductsAndServicesCatTypeID, "", chkQualifications.Items[i].Value.ToString(), "");
                }
                else if (chkQualifications.Items[i].Selected == false && dr1.Length != 0)
                {  // delete   
                    psc.DeleteRegDMEProductServiceQualificationByRegID(Convert.ToInt32(RegId), RegDMEProductsAndServicesID, Convert.ToInt32(chkQualifications.Items[i].Value));
                }
            }
        }

        return rtn;
    }


    protected void ddlProductsServices_SelectedIndexChanged(object sender, EventArgs e)
    {

        Button btn = (Button)this.Parent.FindControl("btnSave");
        if (btn != null)
        {
            // Set the disable after click action. 
            btn.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btn, null) + ";");
        }

        chkSubCategory.Items.Clear();
        chkQualifications.Items.Clear();

        if (ddlProductsServices.SelectedValue != "")
            LoadProductsSubServiceCategory(Convert.ToInt32(ddlProductsServices.SelectedValue), false, false);

        if (btn != null)
        {
            btn.Attributes.Add("onclick", " this.disabled = false; " + this.Page.ClientScript.GetPostBackEventReference(btn, null) + ";");
        }
    }
}