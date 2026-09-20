using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CPCAcknowledgement = Models.Data.CPCAcknowledgement;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CMCAttestation : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Private variables
    private CPCAcknowledgement _acknowledgement = new CPCAcknowledgement();

    #endregion

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


    #endregion

    public string SaveButtonClientID
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadControlData();
    }
 
    private void SetFieldsAccessibility(bool isVisible)
    {
        foreach (ListItem item in chbxlAttestation.Items)
        {
            item.Enabled = isVisible;
        }

        //btnSave.Visible = isVisible;
    }

    public override bool HasInputValue()
    {
        bool rtn = false;


        return rtn;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (_acknowledgement == null)
        {
            _acknowledgement = new CPCAcknowledgement();
        }
        base.OnLoad(e);
    }

    public override void LoadControlData()
    {
        LoadPageControls();
    }

    private void LoadPageControls()
    {
        //If CheckboxList is not already loaded. then Load the CheckBoxList
        if (chbxlAttestation.Items.Count <= 0)
        {
            DataSet ds;
            DataTable dtAttestationControls;

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            //if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCEnroll ||
            //   this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCUpdate ||
            //   this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCReattest)
           // {
                ds = psc.GetCMCProviderAttestationControls(this.WorkflowPage.RegistrationId); // Call CMC SP
           // }
           // else
            //{
            //    ds = psc.GetCPCProviderAttestationControls(this.WorkflowPage.RegistrationId);
            //}

            dtAttestationControls = Helper.HasRows(ds) ? ds.Tables["Table"] : null;
            this.DataList = dtAttestationControls;
            foreach (DataRow row in dtAttestationControls.Rows)
            {
                if (Helper.HasRows(this.DataList))
                {
                    this.LoadData(row);
                }
                else
                {
                    this.LoadData(null);
                }
            }
        }

        // OHPNM-17107 - While reattesting provider has to select check boxes again.
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CMCReAttest &&
                        !Registration.CMCSectionIsValidated(this.WorkflowPage.RegistrationId, CON.SectionTypeID.CMCAttestation))
        {
            foreach (ListItem item in chbxlAttestation.Items)
            {
                item.Selected = false;
            }
        }
    }

    public override void LoadData(DataRow row)
    {
        //ListItem item = new ListItem(Helper.GetString("ATTESTATION_INFO_TYPE_DESC", row));
        //item.Attributes.Add("ID", Helper.GetString("ATTESTATION_INFO_TYPE_ID", row));
        //item.Attributes.Add("onclick", "GetSelectedItem();");

        ListItem item = new ListItem();
        item.Value = Helper.GetString("ATTESTATION_INFO_TYPE_ID", row);
        item.Text = Helper.GetString("ATTESTATION_INFO_TYPE_DESC", row);
        item.Selected = Helper.GetBool("Selected", row);
        chbxlAttestation.Items.Add(item);
        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        SetFieldsAccessibility(regIsPending);
    }

    public override bool SaveData()
    {
        bool isChecked = false;
        Page.Validate("CMCAcknowledgement");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("CMCAcknowledgement") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        foreach(ListItem item in chbxlAttestation.Items)
        {
            if (item.Selected == true)
            {
                isChecked = true;
            }
            else
            {
                isChecked = false;
                break;
            }
        }

        if (isChecked)
        {
            ErrorLabel.Visible = false;
            SaveFormData();
            SetFieldsAccessibility(false);
            return true;
        }
        else
        {
            ErrorLabel.Visible = true;
            SetFieldsAccessibility(true);
            return false;
        }            
    }

    private void SaveFormData()
    {
        var ArrSelectedCheckboxes = hidSelection.Text.Split('|');

        DataTable dt = new DataTable("CMCAttestation");
        DataRow dr;
        dt.Columns.Add("ATTESTATION_INFO_TYPE_ID", typeof(int));
        dt.Columns.Add("ATTESTATION_INFO_RESPONSE", typeof(string));
        //foreach (string str in ArrSelectedCheckboxes)
        //{
        //    if (!string.IsNullOrEmpty(str.Trim()) && str.Contains("="))
        //    {
        //        dr = dt.NewRow();
        //        dr["ATTESTATION_INFO_TYPE_ID"] = str.Trim().Split('=')[0];
        //        dr["ATTESTATION_INFO_RESPONSE"] = str.Trim().Split('=')[1];

        //        dt.Rows.Add(dr);
        //    }

        //}
        foreach (ListItem item in chbxlAttestation.Items)
        {
            if (item.Selected)
            {
                dr = dt.NewRow();
                dr["ATTESTATION_INFO_TYPE_ID"] = item.Value;
                dr["ATTESTATION_INFO_RESPONSE"] = item.Selected ? "true" : "false";

                dt.Rows.Add(dr);
            }
        }
        
        if (dt != null && dt.Rows.Count > 0)
        {
            _acknowledgement.RegID = this.WorkflowPage.RegistrationId;

            var parms = _acknowledgement.CreateParameterList();

            parms.Add("AttestationData", dt);

            _acknowledgement.SaveData(parms);

        }
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    SaveFormData();
    //}
    public override bool ValidateData()
    {
        return true;
    }


    public override string ValidationGroup
    {
        get { return "CMCAcknowledgement"; }
    }

    public override string Title
    {
        get { return "Attestation and Acknowledgement"; }
    }

    public override string IdText
    {
        get { return "ucCMCAttestation_" + this.WorkflowPage.RegistrationId; }
    }


}