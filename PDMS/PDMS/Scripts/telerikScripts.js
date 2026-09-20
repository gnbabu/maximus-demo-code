;
(function ($, undefined) {
    clientBrushColorChange = function (sender, args) {
        var selectedColor = sender.get_selectedColor();
        var signature = $telerik.findControl(document, "RadSignature1");
        signature.set_color(selectedColor);
    };

    clientBackGroundColorChange = function (sender, args) {
        var selectedColor = sender.get_selectedColor();
        var signature = $telerik.findControl(document, "RadSignature1");
        signature.set_backgroundColor(selectedColor);
    };

    clientToolBarButtonClicked = function (sender, args) {
        var selectedItem = args.get_item();
        var signature = $telerik.findControl(document, "RadSignature1");
        if (selectedItem) {
            var selectedThickness = selectedItem.get_value();
            var strokeWidth = selectedThickness == 'thick' ? 3 : 1;
            signature.set_strokeWidth(strokeWidth);
        }
    };

})($telerik.$);
