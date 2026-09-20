using eWorld.UI;
using MAXIMUS.Core.Libraries;
using System;
using System.Web.UI.WebControls;

public partial class Maintenance_FormField : System.Web.UI.UserControl
{
    public Enumerations.FieldTypeEnum FieldType { get; set; }
    public string LabelText { get; set; }
    public string LinkButtonText { get; set; }
    public TextBox TextBoxControl { get { return fieldTextBox; } }
    public NumericBox NumericBoxControl { get { return fieldNumericBox; } }
    public Label Lbl_TextBoxControl { get { return lblTextBox; } }
    public TextBox CalendarControl { get { return fieldCalendar; } }
    public Label LabelControl { get { return fieldLabel; } }
    public Label StackedLabelControl { get { return fieldStackedLabel; } }
    public LinkButton LinkButtonControl { get { return fieldLinkButton; } }
    public DropDownList DropDownListControl { get { return fieldComboBox; } }
    public RadioButtonList YesNoControl { get { return fieldYesNo; } }
    public RadioButtonList RadioButtonListControl{ get { return fieldRadioButtonList; } }
    public CheckBox CheckBoxControl { get { return fieldCheckBox; } }
    public bool IsEditMode { get; set; }
    public string OptLabelTextAfter { get; set; }

    public bool ValidateTextBox { get; set; }
    public string ValidationErrorMessage { get; set; }
    public string ValidationToolTip { get; set; }

    public bool RegValidateTextBox { get; set; }
    public string RegExForTextBox { get; set; }

    public bool CustValidateTextBox { get; set; }
    public event ServerValidateEventHandler Validating;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetFieldVisible();
        }
    }

    protected void custValFieldTextBox_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        if (Validating != null)
            Validating(sender, e);
    }

    private void SetFieldVisible()
    {
        switch(FieldType)
        {
            case Enumerations.FieldTypeEnum.TextBox:
                SetupTextBox();
                break;
            case Enumerations.FieldTypeEnum.Calendar:
                SetupCalendar();
                break;
            case Enumerations.FieldTypeEnum.Label:
                SetupLabel();
                break;
            case Enumerations.FieldTypeEnum.StackedLabel:
                SetupStackedLabel();
                break;
            case Enumerations.FieldTypeEnum.LinkButton:
                SetupLinkButton();
                break;
            case Enumerations.FieldTypeEnum.ComboBox:
                SetupComboBox();
                break;
            case Enumerations.FieldTypeEnum.YesNo:
                SetupYesNo();
                break;
            case Enumerations.FieldTypeEnum.RadioButtonList:
                SetupRadioButtonList();
                break;
            case Enumerations.FieldTypeEnum.Checkbox:
                SetupCheckBox();
                break;
            case Enumerations.FieldTypeEnum.NumericBox:
                SetupNumericBox();
                break;
            default:
                divFieldNoSuchField.Style["display"] = "block";
                fieldNoSuchField.Text = FieldType + " is not a valid field type.";
                break;
        }
    }

    private void SetupTextBox()
    {
        divFieldTextBox.Style["display"] = "block";
        lblTextBox.Text = LabelText;
        if (!string.IsNullOrEmpty(OptLabelTextAfter))
        {
            lblTextBoxOptLabel.Visible = true;
            lblTextBoxOptLabel.Text = OptLabelTextAfter;
        }

        if (ValidateTextBox)
        {
            valFieldTextBox.Enabled = ValidateTextBox;
            valFieldTextBox.ErrorMessage = ValidationErrorMessage;
            valFieldTextBox.ToolTip = ValidationToolTip;
        }

        if (RegValidateTextBox)
        {
            regValFieldTextBox.Enabled = RegValidateTextBox;
            regValFieldTextBox.ErrorMessage = ValidationErrorMessage;
            regValFieldTextBox.ValidationExpression = RegExForTextBox;
            fieldTextBox.CausesValidation = true;
        }

        if (CustValidateTextBox)
        {
            custValFieldTextBox.Enabled = CustValidateTextBox;
            custValFieldTextBox.ErrorMessage = ValidationErrorMessage;
        }
    }

    private void SetupNumericBox()
    {
        divFieldNumericBox.Style["display"] = "block";
        lblNumericBox.Text = LabelText;
        if (!string.IsNullOrEmpty(OptLabelTextAfter))
        {
            lblNumbericBoxOptLabel.Visible = true;
            lblNumbericBoxOptLabel.Text = OptLabelTextAfter;
        }
    }

    private void SetupCalendar()
    {
        divFieldCalendar.Style["display"] = "block";
        lblCalendar.Text = LabelText;
    }

    private void SetupLabel()
    {
        divFieldLabel.Style["display"] = "block";
        lblLabel.Text = LabelText;
    }

    private void SetupStackedLabel()
    {
        divFieldStackedLabel.Style["display"] = "block";
        lblStackedLabel.Text = LabelText;
    }

    private void SetupLinkButton()
    {
        divFieldLinkButton.Style["display"] = "block";
        lblLinkButton.Text = LabelText;
    }

    private void SetupComboBox()
    {
        divFieldComboBox.Style["display"] = "block";
        lblComboBox.Text = LabelText;
    }

    private void SetupYesNo()
    {
        divFieldYesNo.Style["display"] = "block";
        lblYesNo.Text = LabelText;
    }

    private void SetupRadioButtonList()
    {
        divFieldRadioButtonList.Style["display"] = "block";
        lblRadioButtonList.Text = LabelText;
    }

    private void SetupCheckBox()
    {
        divFieldCheckBox.Style["display"] = "block";
        lblCheckBox.Text = LabelText;
    }
}