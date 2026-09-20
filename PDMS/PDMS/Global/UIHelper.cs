using System.Web.UI.WebControls;

/// <summary>
/// Summary description for UIHelper
/// </summary>
public class UIHelper
{
    public UIHelper()
    {
    }

    public static string CreateDIDDApplicationNumberWithSuffix(string applicationNo, int suffix)
    {
        string fmt = "00.##";

        string referralNo = applicationNo + (suffix != 0 ? "-" + suffix.ToString(fmt) : string.Empty);
        return referralNo;
    }


    public static void SetButtonEditability(System.Web.UI.WebControls.Button button, bool enabled)
    {
        button.Enabled = enabled;
        if (enabled)
        {
            button.CssClass = button.CssClass.Replace("button-disabled", "button");
        }
        else
        {
            button.CssClass = button.CssClass.Contains("button-disabled") ? button.CssClass : button.CssClass.Replace("button", "button-disabled");
        }

    }

    public static void SetGridEnabled(GridView grid, bool enabled)
    {
        grid.Enabled = enabled;
        if (enabled)
        {
            grid.CssClass = grid.CssClass.Replace("-disabled", "");
        }
        else
        {
            grid.CssClass = grid.CssClass.Contains("-disabled") ? grid.CssClass : string.Concat(grid.CssClass, "-disabled");
        }
    }

}