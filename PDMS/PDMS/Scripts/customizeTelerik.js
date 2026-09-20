var column = null;
function MenuShowing(sender, args) {
    if (column == null) return;
    var menu = sender; var items = menu.get_items();
    if (column.get_dataType() == "System.String") {
        var i = 0;
        while (i < items.get_count()) {
            if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '', 'NotIsEmpty': '', 'IsEmpty': '', 'NotEqualTo': '', 'EqualTo': '' })) {
                var item = items.getItem(i);
                if (item != null)
                    item.set_visible(false);
            }
            else {
                var item = items.getItem(i);
                if (item != null)
                    item.set_visible(true);
            } i++;
        }
    }
    if (column.get_dataType() == "System.Int64") {
        var j = 0; while (j < items.get_count()) {
            if (!(items.getItem(j).get_value() in { 'NoFilter': '', 'GreaterThan': '', 'LessThan': '', 'NotEqualTo': '', 'EqualTo': '' })) {
                var item = items.getItem(j); if (item != null)
                    item.set_visible(false);
            }
            else { var item = items.getItem(j); if (item != null) item.set_visible(true); } j++;
        }
    }
    column = null;
    menu.repaint();
}
function filterMenuShowing(sender, eventArgs) {
    column = eventArgs.get_column();
}