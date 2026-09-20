using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_Pharmacy : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

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

    public bool SaveData()
    {
        Page.Validate("Pharmacy");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("Pharmacy") && !v.IsValid)
                    return false;
                if (string.IsNullOrEmpty(txtNCPDPNumber.Text) && string.IsNullOrEmpty(txtNCPDPStartDate.Text) && string.IsNullOrEmpty(txtNCPDPEndDate.Text) && string.IsNullOrEmpty(txtRebateExemptionStartDate.Text) && string.IsNullOrEmpty(txtRebateExemptionEndDate.Text))
                {
                    return false;
                }
            }
            catch
            {
                continue;
            }
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        
        parms.Add("NCPDP_NUMBER", txtNCPDPNumber.Text);
        parms.Add("NCPDP_START_DATE", txtNCPDPStartDate.Text);
        parms.Add("NCPDP_END_DATE", txtNCPDPEndDate.Text);
        if (!string.IsNullOrEmpty(txtRebateExemptionStartDate.Text))
        {
            parms.Add("REBATE_EXEMPTION_START_DATE", txtRebateExemptionStartDate.Text);
        }
        if (!string.IsNullOrEmpty(txtRebateExemptionEndDate.Text))
        {
            parms.Add("REBATE_EXEMPTION_END_DATE", txtRebateExemptionEndDate.Text);
        }
        if (B340Participant.SelectedIndex == 0)
        {
            parms.Add("B340PARTICIPANT", "Y");
        }
        else if (B340Participant.SelectedIndex == 1)
        {
            parms.Add("B340PARTICIPANT", "N");
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
        if (isEdit)
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_MEDICARE_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "MEDICARE", parms);
        }
        else
        {
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "MEDICARE", parms);
        }

        return true;
    }

    public void LoadData(DataRow row, bool isEdit)
    {
        bool isPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            txtNCPDPNumber.Enabled =
                txtNCPDPStartDate.Enabled =
                    txtNCPDPEndDate.Enabled =
                    txtRebateExemptionStartDate.Enabled =
                        txtRebateExemptionEndDate.Enabled =
                        B340Participant.Enabled = true;
        }
        else
        {
            txtNCPDPNumber.Enabled =
                txtNCPDPStartDate.Enabled =
                    txtNCPDPEndDate.Enabled =
                    txtRebateExemptionStartDate.Enabled =
                        txtRebateExemptionEndDate.Enabled =
                        B340Participant.Enabled = false;
        }
        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            hidID.Text = row["REG_MEDICARE_ID"].ToString();
            txtNCPDPNumber.Text = row["NCPDP_NUMBER"].ToString();
            txtNCPDPStartDate.Text = Helper.FormatDate2(row["NCPDP_START_DATE"].ToString());
            txtNCPDPEndDate.Text = Helper.FormatDate2(row["NCPDP_END_DATE"].ToString());
            if (!string.IsNullOrEmpty(txtRebateExemptionStartDate.Text.Trim()))
            {
                txtRebateExemptionStartDate.Text = Helper.FormatDate2(row["REBATE_EXEMPTION_START_DATE"].ToString());
            }
            if (!string.IsNullOrEmpty(txtRebateExemptionEndDate.Text.Trim()))
            {
                txtRebateExemptionEndDate.Text = Helper.FormatDate2(row["REBATE_EXEMPTION_END_DATE"].ToString());
            }
            if (row["B340PARTICIPANT"].ToString() == "Y")
            {
                B340Participant.SelectedIndex = 0;
            }
            else if (row["B340PARTICIPANT"].ToString() == "N")
            {
                B340Participant.SelectedIndex = 1;
            }
        }
        else
        {
            hidID.Text = string.Empty;
            txtNCPDPNumber.Text = string.Empty;
            txtNCPDPStartDate.Text = string.Empty;
            txtNCPDPEndDate.Text = string.Empty;
            txtRebateExemptionStartDate.Text = string.Empty;
            txtRebateExemptionEndDate.Text = string.Empty;
            B340Participant.SelectedIndex = -1;
        }
    }

    protected void NCPDPStart_Future(object sender, ServerValidateEventArgs e)
    {
        if (string.IsNullOrEmpty(txtNCPDPStartDate.Text))
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = Convert.ToDateTime(txtNCPDPStartDate.Text) <= DateTime.Now;
        }
    }

    protected void RebateExemptionStart_Future(object sender, ServerValidateEventArgs e)
    {
        if (string.IsNullOrEmpty(txtRebateExemptionStartDate.Text))
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = Convert.ToDateTime(txtRebateExemptionStartDate.Text) <= DateTime.Now;
        }
    }

    protected void NCPDPStartRequired(object sender, ServerValidateEventArgs e)
    {
        /*if (string.IsNullOrEmpty(prov_Number.Text))
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = !string.IsNullOrEmpty(txtNCPDPStartDate.Text);
        }*/
    }

    protected void RebateExemptionStartRequired(object sender, ServerValidateEventArgs e)
    {
        /*if (string.IsNullOrEmpty(prov_Number.Text))
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = !string.IsNullOrEmpty(txtRebateExemptionStartDate.Text);
        }*/
    }


    protected void NCPDPStart_Less(object sender, ServerValidateEventArgs e)
    {
        DateTime start;
        DateTime end;

        start = DateTime.TryParse(txtNCPDPStartDate.Text, out start) ? start : DateTime.MinValue;
        end = DateTime.TryParse(txtNCPDPEndDate.Text, out end) ? end : DateTime.MaxValue;

        if (start > end)
        {
            e.IsValid = false;
            return;
        }
    }

    protected void NCPDPStart_Valid(object sender, ServerValidateEventArgs e)
    {
        DateTime start;
        DateTime minStart = new DateTime(1750,1,1);

        start = DateTime.TryParse(txtNCPDPStartDate.Text, out start) ? start : DateTime.MinValue;

        if (start < minStart)
        {
            e.IsValid = false;
            return;
        }
    }

    protected void NCPDPEnd_Valid(object sender, ServerValidateEventArgs e)
    {
        DateTime end;
        DateTime maxEnd = new DateTime(2150, 12, 31);

        end = DateTime.TryParse(txtNCPDPEndDate.Text, out end) ? end : DateTime.MaxValue;

        if (end > maxEnd)
        {
            e.IsValid = false;
            return;
        }
    }

    protected void RebateExemptionStart_Less(object sender, ServerValidateEventArgs e)
    {
        DateTime start;
        DateTime end;

        start = DateTime.TryParse(txtRebateExemptionStartDate.Text, out start) ? start : DateTime.MinValue;
        end = DateTime.TryParse(txtRebateExemptionEndDate.Text, out end) ? end : DateTime.MaxValue;

        if (start > end)
        {
            e.IsValid = false;
            return;
        }
    }

    protected void ValidateLC_Greater(object sender, ServerValidateEventArgs e)
    {
        DateTime start;
        DateTime end;

        start = DateTime.TryParse(txtNCPDPStartDate.Text, out start) ? start : DateTime.MinValue;
        end = DateTime.TryParse(txtNCPDPEndDate.Text, out end) ? end : DateTime.MaxValue;

        if (start > end)
        {
            e.IsValid = false;
            return;
        }
    }

    protected void RebateExemptionValidateLC_Greater(object sender, ServerValidateEventArgs e)
    {
        DateTime start;
        DateTime end;

        start = DateTime.TryParse(txtRebateExemptionStartDate.Text, out start) ? start : DateTime.MinValue;
        end = DateTime.TryParse(txtRebateExemptionEndDate.Text, out end) ? end : DateTime.MaxValue;

        if (start > end)
        {
            e.IsValid = false;
            return;
        }
    }

    /*protected void prov_Number_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prov_Number.Text) &&
            System.Text.RegularExpressions.Regex.IsMatch("^[a-zA-Z0-9]$", prov_Number.Text))
        {
            prov_Number.Text.Remove(prov_Number.Text.Length - 1);
        }
    }*/
}