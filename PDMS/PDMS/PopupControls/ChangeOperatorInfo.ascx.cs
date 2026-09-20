using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_ChangeOperatorInfo : BaseSectionControl
{
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
    private Guid UserID
    {
        get
        {
            if (ViewState["UserID"] != null)
            {
                return (Guid)ViewState["UserID"];
            }
            else
            {
                ViewState["UserID"] = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                return (Guid)ViewState["UserID"];
            }
        }
        set
        {
            ViewState["UserID"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadDropDown();
        LoadChangeOfOperator();
    }
    public override bool SaveData()
    {
        ValidateData();
        Page.Validate("valChangeOperatorInfo");
        if (Page.IsValid)
        {
            try
            {
                var purchasePrice = this.txtPurchasePrice.Text;
                var subAmount = this.txtSubLeaseAmount.Text;
                var masterAmount = this.txtTotalInitAnnualMasterAmount.Text;
                var medicaidID = this.txtExitingOperatorMedicaidIDt.Text;
                var chopTypeID = this.ddlCHOPType.SelectedValue;
                var effectiveDate = this.txtEffectiveDateCHOP.Text;

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("REG_ID_SELL", string.Empty);
                parms.Add("LAST_MODIFIED_USER", UserID.ToString());
                parms.Add("MEDICAID_ID_SELL", medicaidID);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());               
                parms.Add("CHOP_TYPE_ID", chopTypeID);
                parms.Add("AMT_MASTER_LEASE", masterAmount);
                parms.Add("AMT_SUB_LEASE", subAmount);
                parms.Add("AMT_CHOP", purchasePrice);
                parms.Add("Service_Location_Effective_Date", effectiveDate);

                if(!string.IsNullOrEmpty(hdnChopID.Value))
                {
                    parms.Add("REG_CHOP_PARENT_ID", hdnChopID.Value);
                    svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "CHOP_PARENT", parms);
                }
                else
                {
                    parms.Add("CREATED_BY_USER", UserID.ToString());
                    parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                    svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "CHOP_PARENT", parms);
                }               
               
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
        return false;
    }

    public override void LoadControlData()
    {
        LoadDropDown();
        LoadChangeOfOperator();
    }
  

    public void LoadChangeOfOperator()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CHOP_PARENT");
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
            this.LoadData(dtMisc.Rows[0], true);
        //this.LoadData(null, false);
        else
            this.LoadData(null, false);
    }
    private void LoadData(DataRow dr, bool isEdit)
    {
        int changeOfOperator = isEdit ? Helper.GetInt("REG_CHOP_PARENT_ID", dr) : 0;
       
        // InitFormData();

        if (isEdit)
        {
            if (changeOfOperator > 0)
            {
                hdnChopID.Value = changeOfOperator.ToString();
                this.txtPurchasePrice.Text = Helper.GetDecimal(dr["AMT_CHOP"].ToString()).ToString();
                this.txtSubLeaseAmount.Text = Helper.GetDecimal(dr["AMT_SUB_LEASE"].ToString()).ToString();
                this.txtTotalInitAnnualMasterAmount.Text = Helper.GetDecimal("AMT_MASTER_LEASE", dr).ToString();

                if (Helper.GetInt("CHOP_TYPE_ID", dr) > 0)
                  this.ddlCHOPType.SelectedValue = Helper.GetInt("CHOP_TYPE_ID", dr).ToString();

                this.txtExitingOperatorMedicaidIDt.Text = Helper.GetString("MEDICAID_ID_SELL", dr);
                this.txtEffectiveDateCHOP.Text = Helper.FormatDate2(dr["Service_Location_Effective_Date"].ToString());
                //core table does not have npi
            }

        }
    }
   
    private void LoadDropDown()
    {
        if (_svc == null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        DataSet dataSet = _svc.GetChopTypes();
        DataTable dt = dataSet.Tables[0];

        ddlCHOPType.DataSource = dt;
        ddlCHOPType.DataTextField = "CHOP_TYPE_DESC";
        ddlCHOPType.DataValueField = "CHOP_TYPE_ID";
        ddlCHOPType.DataBind();
    }

    public override void LoadData(DataRow dr)
    {
       
    }

    
    public override bool ValidateData()
    {
        var purchasePrice = this.txtPurchasePrice.Text;
        var subAmount = this.txtSubLeaseAmount.Text;
        var masterAmount = this.txtTotalInitAnnualMasterAmount.Text;
        var medicaidID = this.txtExitingOperatorMedicaidIDt.Text;
        var chopTypeID = this.ddlCHOPType.SelectedValue;
        var effectiveDate = this.txtEffectiveDateCHOP.Text;
        if (string.IsNullOrEmpty(purchasePrice))
        {
            AddError("Please enter the purchase price.");
            return false;
        }
        if (string.IsNullOrEmpty(subAmount))
        {
            AddError("Please enter the sub lease amount.");
            return false;
        }
        if (string.IsNullOrEmpty(masterAmount))
        {
            AddError("Please enter the total annual master amount.");
            return false;
        }

        if (string.IsNullOrEmpty(medicaidID))
        {
            AddError("Please enter exiting madicaidid.");
            return false;
        }
        if (string.IsNullOrEmpty(chopTypeID))
        {
            AddError("Please select chop type.");
            return false;
        }
        if (string.IsNullOrEmpty(effectiveDate))
        {
            AddError("Please select effective date.");
            return false;
        }

        DataSet dsMed = svc.SelectExitingProviderByMedicaidID(medicaidID);
        if (Helper.HasRows(dsMed))
        {
                DataTable dt = dsMed.Tables[0];
                if (dt.Rows[0]["ENROLLMENT_STATUS_CODE"].ToString() == CON.EnrollmentStatusTypeID.InActive.ToString())
                {
                    AddError("Medicaid is not valid.");               
                    return false;

               }
        }
        else
        {
            //If no rows means not valid medicaid
            AddError("Medicaid is not valid.");
            return false;
        }

      return true;
    }
    private void AddError(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valChangeOperatorInfo";
        this.Page.Validators.Add(val);
    }

    public override string ValidationGroup
    {
        get { return "valChangeOperatorInfo"; }
    }

    public override string Title
    {
        get { return "ChangeOperatorInfo SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucChangeOperatorInfo_" + this.WorkflowPage.RegistrationId; }
    }

}

