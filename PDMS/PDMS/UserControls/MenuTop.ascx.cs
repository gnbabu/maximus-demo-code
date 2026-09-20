using System;
using System.Web.UI.WebControls;

public partial class UIControls_MenuTop : System.Web.UI.UserControl
{
    private void RemoveMenuItem(MenuItemCollection col, string value)
    {
        foreach (MenuItem itm in col)
        {
            if (itm.ChildItems.Count > 0) RemoveMenuItem(itm.ChildItems, value);
            if (itm.Value == value)
            {
                col.Remove(itm);
                return;
            }
        }
    }

    // Remove the "PDMS to MMIS" menu option if necessary
    protected void mnuTop_PreRender(object sender, EventArgs e)
    {
        Menu mnu = (Menu)sender;
        if (mnu == null) return;
    }
}
