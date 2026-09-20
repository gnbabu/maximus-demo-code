using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_OwnerSupplier : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") ? "block" : "none";
        hdnRegSupplierID.Value = txtCity.Text = txtAddress1.Text = txtAddress2.Text = txtName.Text = string.Empty;
        nbTaxID.Text = nbZipFirst5.Text = nbZipLast4.Text = nbNPI.Text = string.Empty;
        Helper.LoadDropDownListWithStates(ref ddlState);

        if (dr != null)
        {
            hdnRegSupplierID.Value = Helper.GetString("REG_SUPPLIER_ID", dr);
            txtAddress1.Text = Helper.GetString("ADDRESS1", dr);
            txtAddress2.Text = Helper.GetString("ADDRESS2", dr);
            nbZipFirst5.Text = Helper.GetString("ZIP", dr);
            nbZipLast4.Text = Helper.GetString("EXT_ZIP", dr);
            txtCity.Text = Helper.GetString("CITY", dr);
            ddlState.SelectedValue = Helper.GetString("STATE", dr);
            txtName.Text = Helper.GetString("NAME", dr);
            nbTaxID.Text = Helper.GetString("TAX_ID", dr);
            nbNPI.Text = Helper.GetString("NPI", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("NAME", txtName.Text);
        parms.Add("TAX_ID", nbTaxID.Text);
        parms.Add("NPI", nbNPI.Text);
        parms.Add("ADDRESS1", txtAddress1.Text);
        parms.Add("ADDRESS2", txtAddress2.Text);
        parms.Add("CITY", txtCity.Text);
        parms.Add("STATE", ddlState.SelectedValue);
        parms.Add("ZIP", nbZipFirst5.Text);
        parms.Add("EXT_ZIP", nbZipLast4.Text);
        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegSupplierID.Value))
        {
            // Update
            parms.Add("REG_SUPPLIER_ID", hdnRegSupplierID.Value);
            psc.UpdateRegistrationDataTable("SUPPLIER", parms);
        }
        else
        {
            // Insert
            psc.InsertRegistrationDataTable("SUPPLIER", parms);
        }
    }
}