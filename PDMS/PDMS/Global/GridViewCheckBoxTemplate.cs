using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for GridViewTemplate
/// </summary>
//A customized class for displaying the Template Column
public class GridViewCheckBoxTemplate : ITemplate
{
    //A variable to hold the type of ListItemType.
    ListItemType _templateType;

    //A variable to hold the column name.
    string _columnName;

    //Constructor where we define the template type and column name.
    public GridViewCheckBoxTemplate(ListItemType type, string colname)
    {
        //Stores the template type.
        _templateType = type;

        //Stores the column name.
        _columnName = colname;
    }

    void ITemplate.InstantiateIn(System.Web.UI.Control container)
    {
        switch (_templateType)
        {
            case ListItemType.Header:
                //Creates a new label control and add it to the container.
                Label lbl = new Label();            //Allocates the new label object.
                lbl.Text = _columnName;             //Assigns the name of the column in the lable.
                container.Controls.Add(lbl);        //Adds the newly created label control to the container.
                break;

            case ListItemType.Item:
                //Creates a new text box control and add it to the container.
                CheckBox tb1 = new CheckBox();                            //Allocates the new text box object.
                tb1.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                tb1.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
                container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
                break;

            case ListItemType.EditItem:
                //As, I am not using any EditItem, I didnot added any code here.
                break;

            case ListItemType.Footer:
                CheckBox chkColumn = new CheckBox();
                chkColumn.ID = "Chk" + _columnName;
                container.Controls.Add(chkColumn);
                break;
        }
    }

    /// <summary>
    /// This is the event, which will be raised when the binding happens.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void tb1_DataBinding(object sender, EventArgs e)
    {
        CheckBox txtdata = (CheckBox)sender;

        GridViewRow container = (GridViewRow)txtdata.NamingContainer;
        //object userId = DataBinder.Eval(container.DataItem, "USERID");
        txtdata.ID = "permCheckBox_" + _columnName;// +"_" + userId.ToString();
        object dataValue = ((System.Data.DataRowView)(container.DataItem)).Row[_columnName];
        //object dataValue = DataBinder.Eval(container.DataItem, _columnName);
        if (dataValue != DBNull.Value)
        {
            int result = 0;
            int.TryParse(dataValue.ToString(), out result);
            txtdata.Checked = result == 1 ? true : false;
        }
        else
        {
            txtdata.Checked = false;
        }
    }

    private void CheckBox_CheckedChanged(object sender, EventArgs e)
    {
        HttpContext.Current.Session["GridAgentChecked"] = true;
    }
}