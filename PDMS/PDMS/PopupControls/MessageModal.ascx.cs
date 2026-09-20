using System;

public partial class UserControls_MessageModal : System.Web.UI.UserControl
{
    public delegate void ContinueEventHandler();
    public event ContinueEventHandler ContinueEvent;

    public static class MessageModalMode
    {
        public const int ContinueCancel = 1;
        public const int Continue = 2;
        public const int Ok = 3;
        public const int ContinueOk = 4;
    }

    public void Show(int mode, string title, string message)
    {
        lblTitle.Text = title;
        lblMessage.Text = message;
        switch (mode)
        {
            case MessageModalMode.ContinueCancel:
                btnContinue.Style.Add("display", "block");
                btnCancel.Text = "Cancel";
                break;
            case MessageModalMode.Continue:
                btnContinue.Style.Add("display", "none");
                btnCancel.Text = "Continue";
                break;
            case MessageModalMode.Ok:
                btnContinue.Style.Add("display", "none");
                btnCancel.Text = "OK";
                lblMessage.Style.Add("color", "red");
                break;
            case MessageModalMode.ContinueOk:
                btnContinue.Style.Add("display", "none");
                btnCancel.Text = "OK";
                lblMessage.Style.Add("color", "black");
                break;
        }
        mpe.Show();
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        if (ContinueEvent != null) ContinueEvent();
    }
}