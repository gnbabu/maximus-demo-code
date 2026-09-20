using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

public partial class UserControls_FormFieldEntry : System.Web.UI.UserControl
{
    public delegate void SelectedIndexChangedHandler(string value);
    public event SelectedIndexChangedHandler SelectedIndexChanged;

    public enum FieldControlType
    {
        Label = 0,
        TextBox = 1,
        DropDownList = 2,
        NumericBox = 3,
        Date = 4,
        LicenseNo = 5,
        StateZip = 6,
        PhoneNo = 7
    }

    public FieldControlType FieldType
    {
        get 
        {
            switch (mltFormFieldEntry.ActiveViewIndex)
            {
                case 1:
                    return FieldControlType.TextBox;
                case 2:
                    return FieldControlType.DropDownList;
                case 3:
                    return FieldControlType.NumericBox;
                case 4:
                    return FieldControlType.Date;
                case 5:
                    return FieldControlType.LicenseNo;
                case 6:
                    return FieldControlType.StateZip;
                case 7:
                    return FieldControlType.PhoneNo;
                default:
                    return FieldControlType.Label;
            }
        }
        set 
        {
            mltFormFieldEntry.ActiveViewIndex = Convert.ToInt32(value);
        }
    }

    public string CssClassEdit { get; set; }
    public string CssClassDisplay
    {
        get { return lblDisplay.CssClass; }
        set { lblDisplay.CssClass = value; }
    }
    public int MaxLength { get; set; }
    public int MinLength { get; set; }
    public bool Required { get; set; }
    public int RequiredLength { get; set; } 
    public string ToolTip { get; set; }
    public string ValidationGroup { get; set; }

    public void LoadDropDown(DataTable dt, string dataText, string dataValue)
    {
        Helper.LoadList(ddlEdit, dt, dataText, dataValue, true);
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg + " - " + Helper.PascalCaseParse(this.ID.Substring(3));
        val.Display = ValidatorDisplay.Dynamic;
        val.Text = val.ErrorMessage;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public void SetDropDownValue(string value)
    {
        if (ddlEdit.Items.FindByValue(value) != null)
        {
            ddlEdit.SelectedValue = value;
            lblDisplay.Text = ddlEdit.SelectedItem.Text;
        }
    }

    public void LoadData(string value)
    {
        switch (mltFormFieldEntry.ActiveViewIndex)
        {
            case 1:
                txtEdit.Text = value;
                break;
            case 2:
                SetDropDownValue(value);
                break;
            case 3:
                nbEdit.Text = value;
                break;
            case 4:
                txtEditDate.Text = value;
                break;
            case 5:
                ClearData();
                if (value.Length >= 4)
                {
                    ddlLicenseNo.SelectedValue = ddlLicenseNo.Items.FindByText(value.Substring(0, 2)).Value;
                    txtLicenseNo.Text = value.Substring(3);
                }
                break;
            case 6:
                ClearData();
                ddlState.SelectedValue = ddlState.Items.FindByText(value.Substring(0, 2)).Value;
                nbZip.Text = value.Substring(3);
                break;
            case 7:                
                txtPhoneNo.Text = value;
                break;
            default:
                lblDisplay.Text = value;
                break;
        }
    }

    public bool IsModified()
    {
        bool rtn = false;
        switch (mltFormFieldEntry.ActiveViewIndex)
        {
            case 1:
                rtn = (lblDisplay.Text != txtEdit.Text);
                break;
            case 2:
                rtn = (lblDisplay.Text != ddlEdit.SelectedItem.Text);
                break;
            case 3:
                rtn = (lblDisplay.Text != nbEdit.Text);
                break;
            case 4:
                rtn = (lblDisplay.Text != txtEditDate.Text);
                break;
            case 5:
                rtn = (lblDisplay.Text != ddlLicenseNo.SelectedItem.Text + "-" + txtLicenseNo.Text);
                break;
            case 6:
                rtn = (lblDisplay.Text != ddlState.SelectedItem.Text + "_" + nbZip.Text);
                break;
            case 7:
                rtn = (lblDisplay.Text != txtPhoneNo.Text);
                break;
        }
        return rtn;
    }

    public string GetData()
    {
        switch (mltFormFieldEntry.ActiveViewIndex)
        {
            case 1:
                return txtEdit.Text;
            case 2:
                return ddlEdit.SelectedValue;
            case 3:
                return nbEdit.Text;
            case 4:
                return txtEditDate.Text;
            case 5:
                if (ddlLicenseNo.SelectedIndex > 0 && !string.IsNullOrEmpty(txtLicenseNo.Text))
                    return ddlLicenseNo.SelectedItem.Text + "-" + txtLicenseNo.Text;
                else return string.Empty;
            case 6:
                return ddlState.SelectedItem.Text + "_" + nbZip.Text;
            case 7:
                return txtPhoneNo.Text;
            default:
                return lblDisplay.Text;
        }
    }

    public string GetDataDropDownText()
    {
        if (mltFormFieldEntry.ActiveViewIndex != 2) return string.Empty;
        return ddlEdit.SelectedItem.Text;
    }

    public string GetModifiedData()
    {
        if (!IsModified()) return null;
        return GetData();
    }

    public string GetModifiedData(bool getData)
    {
        if (!getData) return null;
        return GetData();
    }

    public string GetModifiedPart(bool digitsOnly, int pos, int len)
    {
        if (!IsModified()) return null;
        string rtn = null;
        try
        {
            string data = GetData();
            if (digitsOnly) data = Regex.Replace(data, "\\D", string.Empty);
            rtn = data.Substring(pos, len);
        }
        catch { }
        return rtn;
    }

    public string GetModifiedPart(int pos, int len)
    {
        return GetModifiedPart(false, pos, len);
    }

    public void ClearData()
    {
        switch (mltFormFieldEntry.ActiveViewIndex)
        {
            case 1:
                txtEdit.Text = string.Empty;
                break;
            case 2:
                ddlEdit.SelectedIndex = -1;
                break;
            case 3:
                nbEdit.Text = string.Empty;
                break;
            case 4:
                txtEditDate.Text = string.Empty;
                break;
            case 5:
                if (ddlLicenseNo.Items.Count == 0) Helper.LoadDropDownListWithStates(ref ddlLicenseNo, true);
                ddlLicenseNo.SelectedIndex = 0;
                txtLicenseNo.Text = string.Empty;
                break;
            case 6:
                if (ddlState.Items.Count == 0) Helper.LoadDropDownListWithStates(ref ddlState, true);
                ddlState.SelectedIndex = 0;
                nbZip.Text = string.Empty;
                break;
            case 7:
                txtPhoneNo.Text = string.Empty;
                break;
            default:
                lblDisplay.Text = string.Empty;
                break;
        }
    }

    public bool ValidateData()
    {
        bool isGood = true;
        if (Required)
        {
            switch (mltFormFieldEntry.ActiveViewIndex)
            {
                case 1:
                    if (string.IsNullOrEmpty(txtEdit.Text.Trim())) AddError("* Enter free form text value", ref isGood);
                    else if (MinLength > 0)
                    {
                        if (txtEdit.Text.Length < MinLength) AddError("* Enter at least " + MinLength.ToString() + " character(s)", ref isGood);
                    }
                    break;
                case 2:
                    if (ddlEdit.SelectedIndex <= 0) AddError("* Select a value from the Dropdown List", ref isGood);
                    break;
                case 3:
                    if (string.IsNullOrEmpty(nbEdit.Text.Trim())) AddError("* Enter numeric value", ref isGood);
                    else if (MinLength > 0)
                    {
                        if (nbEdit.Text.Length < MinLength) AddError("* Enter at least " + MinLength.ToString() + " number(s)", ref isGood);
                    }
                    break;
                case 4:
                    if (string.IsNullOrEmpty(txtEditDate.Text.Trim())) AddError("* Enter a date", ref isGood);
                    break;
                case 5:
                    if (ddlLicenseNo.SelectedIndex <= 0) AddError("* Select a State", ref isGood);
                    if (string.IsNullOrEmpty(txtLicenseNo.Text.Trim())) AddError("* Enter a License No", ref isGood);
                    else if (MinLength > 0)
                    {
                        if (txtLicenseNo.Text.Length < MinLength) AddError("* Enter at least " + MinLength.ToString() + " character(s) for License No", ref isGood);
                    }
                    break;
                case 6:
                    if (ddlState.SelectedIndex <= 0) AddError("* Select a State", ref isGood);
                    if (string.IsNullOrEmpty(nbZip.Text.Trim())) AddError("* Enter a Zip Code", ref isGood);
                    else if (nbZip.Text.Length < 5) AddError("* Enter at least 5 numbers for Zip Code", ref isGood);
                    break;
                case 7:
                    if (string.IsNullOrEmpty(Regex.Replace(txtPhoneNo.Text, "\\D", string.Empty))) 
                        AddError("* Enter Phone Number", ref isGood);
                    break;
            }
        }
        if (RequiredLength > 0)
        {
            switch (mltFormFieldEntry.ActiveViewIndex)
            {
                case 1:
                    if (!string.IsNullOrEmpty(txtEdit.Text) && txtEdit.Text.Length != RequiredLength) AddError("* Enter " + 
                        RequiredLength.ToString() + " characters", ref isGood);
                    break;
                case 3:
                    if (!string.IsNullOrEmpty(nbEdit.Text) && nbEdit.Text.Length != RequiredLength) 
                        AddError("* Enter " + RequiredLength.ToString() + " digits", ref isGood);
                    break;
                case 5:
                    if (!string.IsNullOrEmpty(txtLicenseNo.Text))
                    {
                        if (ddlLicenseNo.SelectedIndex <= 0) AddError("* Select a State", ref isGood);
                        else if (txtLicenseNo.Text.Length != RequiredLength) AddError("* Enter " + RequiredLength.ToString() + " characters", ref isGood);
                    }
                    break;
            }
        }

        // If phone number verify length
        if (mltFormFieldEntry.ActiveViewIndex == 7)
        {
            string phoneNo = Regex.Replace(txtPhoneNo.Text, "\\D", string.Empty);
            if (!string.IsNullOrEmpty(phoneNo))
            {
                if (phoneNo.Length != 10) AddError("* Enter a valid 10-digit Phone Number", ref isGood);
            }
        }
        return isGood;
    }


    //Z        <asp:CustomValidator ID="cvPhone" runat="server" OnServerValidate="Validate_cvPhoneRequired" Display="Static" ValidationGroup="RegisterUserValidationGroup" ErrorMessage="* Phone Number is required" Text="*" />


    public void SwitchToEdit(FieldControlType type, bool setMultiIndexOnly)
    {
        mltFormFieldEntry.ActiveViewIndex = Convert.ToInt32(type);
        if (setMultiIndexOnly) return;
        switch (Convert.ToInt32(type))
        {
            case 1:
                txtEdit.Text = lblDisplay.Text;
                if (!string.IsNullOrEmpty(CssClassEdit)) txtEdit.CssClass = CssClassEdit;
                if (!string.IsNullOrEmpty(ToolTip)) txtEdit.ToolTip = ToolTip;
                if (MaxLength > 0) txtEdit.MaxLength = MaxLength;
                break;
            case 2:
                ddlEdit.SelectedIndex = -1;
                if (ddlEdit.Items.FindByText(lblDisplay.Text) != null)
                    ddlEdit.SelectedValue = ddlEdit.Items.FindByText(lblDisplay.Text).Value;
                if (!string.IsNullOrEmpty(CssClassEdit)) ddlEdit.CssClass = CssClassEdit;
                if (!string.IsNullOrEmpty(ToolTip)) ddlEdit.ToolTip = ToolTip;
                break;
            case 3:
                nbEdit.Text = lblDisplay.Text;
                if (!string.IsNullOrEmpty(CssClassEdit)) nbEdit.CssClass = CssClassEdit;
                if (!string.IsNullOrEmpty(ToolTip)) nbEdit.ToolTip = ToolTip;
                if (MaxLength > 0) nbEdit.MaxLength = MaxLength;
                break;
            case 4:
                txtEditDate.Text = lblDisplay.Text;
                if (!string.IsNullOrEmpty(CssClassEdit)) txtEditDate.CssClass = CssClassEdit;
                if (!string.IsNullOrEmpty(ToolTip)) txtEditDate.ToolTip = ToolTip;
                if (MaxLength > 0) txtEditDate.MaxLength = MaxLength;
                break;
            case 5:
                ClearData();
                if (lblDisplay.Text.Length >= 4)
                {
                    if (ddlLicenseNo.Items.FindByText(lblDisplay.Text.Substring(0, 2)) != null)
                        ddlLicenseNo.SelectedValue = ddlLicenseNo.Items.FindByText(lblDisplay.Text.Substring(0, 2)).Value;
                    txtLicenseNo.Text = lblDisplay.Text.Substring(3);
                }
                if (!string.IsNullOrEmpty(CssClassEdit)) txtLicenseNo.CssClass = CssClassEdit;
                if (!string.IsNullOrEmpty(ToolTip)) txtLicenseNo.ToolTip = ToolTip;
                if (MaxLength > 0) txtLicenseNo.MaxLength = MaxLength;
                break;
            case 6:
                ClearData();
                if (lblDisplay.Text.Length >= 4)
                {
                    if (ddlState.Items.FindByText(lblDisplay.Text.Substring(0, 2)) != null)
                        ddlState.SelectedValue = ddlState.Items.FindByText(lblDisplay.Text.Substring(0, 2)).Value;
                    nbZip.Text = lblDisplay.Text.Substring(3);
                }
                if (!string.IsNullOrEmpty(CssClassEdit)) nbZip.CssClass = CssClassEdit;
                if (!string.IsNullOrEmpty(ToolTip)) nbZip.ToolTip = ToolTip;
                if (MaxLength > 0) nbZip.MaxLength = MaxLength;
                break;
            case 7:
                txtPhoneNo.Text = lblDisplay.Text;
                if (!string.IsNullOrEmpty(CssClassEdit)) txtPhoneNo.CssClass = CssClassEdit;
                if (!string.IsNullOrEmpty(ToolTip)) txtPhoneNo.ToolTip = ToolTip;
                if (MaxLength > 0) txtPhoneNo.MaxLength = MaxLength;
                break;
        }
    }

    public void SwitchToEdit(FieldControlType type)
    {
        SwitchToEdit(type, false);
    }

    public void SwitchToDisplay()
    {
        mltFormFieldEntry.ActiveViewIndex = 0;                  // Set to Label
    }

    protected void ddlEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SelectedIndexChanged != null) SelectedIndexChanged(ddlEdit.SelectedValue);
    }
}