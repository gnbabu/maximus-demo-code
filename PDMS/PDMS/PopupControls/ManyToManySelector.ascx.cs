using System;
using System.Web.UI.WebControls;

public partial class UserControls_ManyToManySelector : System.Web.UI.UserControl
{
    public delegate void delgOnItemSelected(ListItem newItem);
    public delegate void delgOnItemRemoved(ListItem removedItem);

    private delgOnItemSelected itemSelectedCallback;
    private delgOnItemRemoved itemRemovedCallback;

    public delgOnItemSelected OnItemSelected
    {
        set
        {
            itemSelectedCallback = value;
        }
    }

    public delgOnItemRemoved OnItemRemoved
    {
        set
        {
            itemRemovedCallback = value;
        }
    }
    
    public ListBox SourceListBox
    {
        get { return lbSource; }
    }

    public ListBox SelectedListBox
    {
        get { return lbSelected; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public ListItem SelectItemByText(string selectionText)
    {
        ListItem selectedItem = null;
        ListItem[] liList = new ListItem[SourceListBox.Items.Count];
        SourceListBox.Items.CopyTo(liList, 0);
        foreach (ListItem li in liList)
        {
            if (li.Value == selectionText)
            {
                SourceListBox.Items.Remove(li);
                SelectedListBox.Items.Add(li);
                selectedItem = li;
                if (itemSelectedCallback != null)
                {
                    itemSelectedCallback.Invoke(li);
                }
            }
        }
        return selectedItem;
    }

    public void UnSelectItemByText(string selectionText)
    {
        ListItem[] liList = new ListItem[SelectedListBox.Items.Count];
        SelectedListBox.Items.CopyTo(liList, 0);
        foreach (ListItem li in liList)
        {
            if (li.Text == selectionText)
            {
                SourceListBox.Items.Add(li);
                SelectedListBox.Items.Remove(li);
                if (itemSelectedCallback != null)
                {
                    itemSelectedCallback.Invoke(li);
                }
            }
        }
    }
    protected void btnSourceAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbSource.Items)
        {
            li.Selected = true;
        }
    }
    protected void btnSourceNone_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbSource.Items)
        {
            li.Selected = false;
        }
    }
    protected void btnSelectedAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbSelected.Items)
        {
            li.Selected = true;
        }
    }
    protected void btnSelectedNone_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbSelected.Items)
        {
            li.Selected = false;
        }
    }
    protected void btnAddToSelected_Click(object sender, EventArgs e)
    {
        ListItem[] liList = new ListItem[SourceListBox.Items.Count];
        SourceListBox.Items.CopyTo(liList, 0);
        foreach (ListItem li in liList)
        {
            if (li.Selected)
            {
                lbSelected.Items.Add(li);
                lbSource.Items.Remove(li);
                if (itemSelectedCallback != null)
                {
                    itemSelectedCallback.Invoke(li);
                }
            }
        }
    }
    protected void btnRemoveFromSelected_Click(object sender, EventArgs e)
    {
        ListItem[] liList = new ListItem[SelectedListBox.Items.Count];
        SelectedListBox.Items.CopyTo(liList, 0);
        foreach (ListItem li in liList)
        {
            if (li.Selected)
            {
                lbSelected.Items.Remove(li);
                lbSource.Items.Add(li);
                if (itemRemovedCallback != null)
                {
                    itemRemovedCallback.Invoke(li);
                }
            }
        }
    }
}