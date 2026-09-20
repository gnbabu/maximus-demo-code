public partial class UserControls_HeaderLine : System.Web.UI.UserControl
{
    public string Header
    {
        get { return lblHeader.Text; }
        set { lblHeader.Text = value; }
    }

    public string ToolTip
    {
        get { return lblHeader.ToolTip; }
        set { lblHeader.ToolTip = value; }
    }
}