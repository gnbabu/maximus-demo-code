using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_ACHReliaCardAuthorization : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void SaveData()
    {
       
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("LAST_NAME", txtLastName.Text);
        parms.Add("FIRST_NAME", txtFirstName.Text);
        parms.Add("MIDDLE_INITIAL", txtMiddleInitial.Text);
        parms.Add("STREET", txtStreet.Text);
        parms.Add("CITY", txtCity.Text);
        parms.Add("STATE", ddlState.SelectedValue);
        parms.Add("ZIPCODE", txtZipCode.Text);
        
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegACHReliaCardID.Value))
        {
            // Update
            parms.Add("REG_ACH_RELIACARD_ID", hdnRegACHReliaCardID.Value);
            psc.UpdateRegistrationDataTable("ACH_RELIACARD", parms);
        }
        else
        {
            // Insert
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationDataTable("ACH_RELIACARD", parms);
        }
    }


    public override void LoadData(DataRow dr)
    {
        InitiFields();
        hdnRegACHReliaCardID.Value = String.Empty;
        Helper.LoadDropDownListWithStates(ref ddlState);
        if (dr != null)
        {
            hdnRegACHReliaCardID.Value = Helper.GetString("REG_ACH_RELIACARD_ID", dr);
            txtLastName.Text = Helper.GetString("LAST_NAME", dr);
            txtFirstName.Text = Helper.GetString("FIRST_NAME", dr);
            txtMiddleInitial.Text = Helper.GetString("MIDDLE_INITIAL", dr);
            txtStreet.Text = Helper.GetString("STREET", dr);
            txtCity.Text = Helper.GetString("CITY", dr);
            ddlState.SelectedValue = Helper.GetString("STATE", dr);
            txtZipCode.Text = Helper.GetString("ZIPCODE", dr);
        }
        
    }

    private void InitiFields()
    {
        this.txtCity.Text = string.Empty;
        this.txtFirstName.Text = string.Empty;
        this.txtLastName.Text = string.Empty;
        this.txtMiddleInitial.Text = string.Empty;
        this.txtStreet.Text = string.Empty;
        this.txtZipCode.Text = string.Empty;
        hdnRegACHReliaCardID.Value = string.Empty;
    }

   
}