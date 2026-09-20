using System;
using System.Web.UI.WebControls;

public partial class UserControls_exDropDownList : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public DropDownList DropDownList
    {
        get { return ddlEntry; }
        set { ddlEntry = value; }
    }

    public bool Enabled
    {
        get { return ddlEntry.Enabled; }
        set { ddlEntry.Enabled = value; }
    }

    public string CssClass
    {
        get { return ddlEntry.CssClass; }
        set { ddlEntry.CssClass = value; }
    }

    public Unit Width
    {
        get { return ddlEntry.Width; }
        set { ddlEntry.Width = value; }
    }

    public ListItemCollection Items
    {
        get { return ddlEntry.Items; }
    }

    public int SelectedIndex
    {
        get { return ddlEntry.SelectedIndex; }
        set { ddlEntry.SelectedIndex = value; }
    }

    public string SelectedValue
    {
        get { return ddlEntry.SelectedValue; }
        set { ddlEntry.SelectedValue = value; }
    }

    public string OriginalValue
    {
        get { return hdnEntry.Value; }
        set { hdnEntry.Value = value; }
    }

    // Return TRUE if the Hidden and Selected are the same
    public bool AreTheSame
    {
        get
        {
            return (OriginalValue == SelectedValue);
        }
    }

    // Return TRUE if the DropDown has a value
    public bool ValueExists
    {
        get
        {
            return !string.IsNullOrEmpty(ddlEntry.SelectedValue);
        }
    }
}