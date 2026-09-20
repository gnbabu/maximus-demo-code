using System;

public partial class UserControls_SelectedControl : System.Web.UI.UserControl
{
    public bool Selected
    {
        get
        {
            if (ViewState["Selected"] == null) ViewState["Selected"] = false;
            return Convert.ToBoolean(ViewState["Selected"]);
        }
        set 
        { 
            ViewState["Selected"] = value;
            if (value) pnlControl.CssClass = "Selected";
            else pnlControl.CssClass = "NotSelected";
        }
    }

    public string Text
    {
        get { return lblText.Text; }
        set { lblText.Text = value; }
    }

}