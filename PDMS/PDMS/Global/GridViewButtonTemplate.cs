using System;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for GridViewButtonTemplate
/// </summary>
public class GridViewButtonTemplate : ITemplate
{
    //A variable to hold the type of ListItemType.
    ListItemType _templateType;

    //A variable to hold the column name.
    string _columnName;

    //A variable to hold the column name.
    EventHandler _eventHandlerForButtonClick;

    //Constructor where we define the template type and column name.
    public GridViewButtonTemplate(ListItemType type, string colname, EventHandler buttonClickEvent)
    {
        //Stores the template type.
        _templateType = type;

        //Stores the column name.
        _columnName = colname;
        _eventHandlerForButtonClick = buttonClickEvent;

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
                Button tb1 = new Button();                            //Allocates the new text box object.
                tb1.CssClass = "buttonBox";
                tb1.DataBinding += new EventHandler(tb1_DataBinding);   //Attaches the data binding event.
                tb1.Click += _eventHandlerForButtonClick;
                container.Controls.Add(tb1);                            //Adds the newly created textbox to the container.
                break;

            case ListItemType.EditItem:
                //As, I am not using any EditItem, I didnot added any code here.
                break;

            case ListItemType.Footer:
                Button chkColumn = new Button();
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
        Button txtdata = (Button)sender;

        GridViewRow container = (GridViewRow)txtdata.NamingContainer;
        //object userId = DataBinder.Eval(container.DataItem, "USERID");
        txtdata.ID = "permCheckBox_" + _columnName;// +"_" + userId.ToString();
        object dataValue = ((System.Data.DataRowView)(container.DataItem)).Row[_columnName];
        txtdata.Text = "De-activate";
        txtdata.CommandName = "Deactivate";
        txtdata.CommandArgument = _columnName;
        if (dataValue.ToString() == "0")
        {
            txtdata.Enabled = false;
        }
    }
}