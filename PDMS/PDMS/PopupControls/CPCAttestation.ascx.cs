using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CPCAcknowledgement = Models.Data.CPCAcknowledgement;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CPCAttestation : BaseSectionControl
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
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.GetCPCProviderAttestationControls(this.WorkflowPage.RegistrationId);
            DataTable dtAttestationControls = Helper.HasRows(ds) ? ds.Tables["Table"] : null;
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
        // OHPNM-13913 - While reattesting provider has to select check boxes again.
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CPCReattest &&
                        //!IsSectionComplete(this.WorkflowPage.RegistrationId, "CPCAttestation"))
                        !Registration.SectionIsValidated(this.WorkflowPage.RegistrationId, CON.SectionTypeID.CPCAttestation))
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
        Page.Validate("CPCAcknowledgement");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("CPCAcknowledgement") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        SaveFormData();

        SetFieldsAccessibility(false);
        return true;
    }


    private void SaveFormData()
    {
        var ArrSelectedCheckboxes = hidSelection.Text.Split('|');

        DataTable dt = new DataTable("CPCAttestation");
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
        bool isChecked = true;
        foreach (ListItem item in chbxlAttestation.Items)
        {
            if (!item.Selected)
            {
                isChecked = false;
            }
        }
        if (!isChecked)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Please select all check attestation.";
            val.ValidationGroup = "CPCAcknowledgement";
            this.Page.Validators.Add(val);
           
        }
        return isChecked;
    }


    public override string ValidationGroup
    {
        get { return "CPCAcknowledgement"; }
    }

    public override string Title
    {
        get { return "Attestation and Acknowledgement"; }
    }

    public override string IdText
    {
        get { return "ucCPCAttestation_" + this.WorkflowPage.RegistrationId; }
    }


}