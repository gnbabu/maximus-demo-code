using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_OwnerSubcontractorOwner : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        cmpValBirthDateFuture.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
    }

    public override void LoadData(DataRow dr)
    {
        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") ? "block" : "none";
        hdnRegSubcontractorOwnerID.Value = txtCity.Text = txtAddress1.Text = txtAddress2.Text = txtBirthDate.Text = txtName.Text = txtTitle.Text = string.Empty;
        nbPercent.Text = nbTaxID.Text = nbZipFirst5.Text = nbZipLast4.Text = string.Empty;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Helper.LoadDropDownListWithStates(ref ddlState);
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SUBCONTRACTOR");
        if (Helper.HasRows(ds))
        {
            Helper.LoadList(ddlSubcontractor, ds.Tables[0], "NAME", "REG_SUBCONTRACTOR_ID", true);
            ddlSubcontractor.SelectedIndex = 0;
        }

        if (dr != null)
        {
            hdnRegSubcontractorOwnerID.Value = Helper.GetString("REG_SUBCONTRACTOR_OWNER_ID", dr);
            txtAddress1.Text = Helper.GetString("ADDRESS1", dr);
            txtAddress2.Text = Helper.GetString("ADDRESS2", dr);
            ddlSubcontractor.SelectedValue = Helper.GetString("REG_SUBCONTRACTOR_ID", dr);
            txtTitle.Text = Helper.GetString("TITLE", dr);
            txtBirthDate.Text = Helper.GetDate("DOB", dr);
            nbZipFirst5.Text = Helper.GetString("ZIP", dr);
            nbZipLast4.Text = Helper.GetString("EXT_ZIP", dr);
            txtCity.Text = Helper.GetString("CITY", dr);
            ddlState.SelectedValue = Helper.GetString("STATE", dr);
            txtName.Text = Helper.GetString("NAME", dr);
            nbTaxID.Text = Helper.GetString("TAX_ID", dr);
            nbPercent.Text = Helper.GetString("PERCENT_OF_OWNERSHIP", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_SUBCONTRACTOR_ID", ddlSubcontractor.SelectedValue);
        parms.Add("NAME", txtName.Text);
        parms.Add("DOB", txtBirthDate.Text);
        parms.Add("TAX_ID", nbTaxID.Text);
        parms.Add("PERCENT_OF_OWNERSHIP", nbPercent.Text);
        parms.Add("TITLE", txtTitle.Text);
        parms.Add("ADDRESS1", txtAddress1.Text);
        parms.Add("ADDRESS2", txtAddress2.Text);
        parms.Add("CITY", txtCity.Text);
        parms.Add("STATE", ddlState.SelectedValue);
        parms.Add("ZIP", nbZipFirst5.Text);
        parms.Add("EXT_ZIP", nbZipLast4.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegSubcontractorOwnerID.Value))
        {
            // Update
            parms.Add("REG_SUBCONTRACTOR_OWNER_ID", hdnRegSubcontractorOwnerID.Value);
            psc.UpdateRegistrationDataTable("SUBCONTRACTOR_OWNER", parms);
        }
        else
        {
            // Insert
            psc.InsertRegistrationDataTable("SUBCONTRACTOR_OWNER", parms);
        }
    }
}