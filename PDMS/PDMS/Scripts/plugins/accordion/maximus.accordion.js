(function ($) {

    var methods = {

        init: function (options) {

            var settings = $.extend({
                allowMultiple: false,
                defaultOpen: null,
                animationSpeed: 200,
                onOpen: null,
                onClose: null
            }, options);

            return this.each(function () {

                var $accordion = $(this);

                $accordion.data("settings", settings);

                // hide all panels initially to prevent flicker
                $accordion.find(".maximus-accordion-content").hide();

                // Click handler
                $accordion.on("click", ".maximus-accordion-header", function () {

                    var index = $(this)
                        .closest(".maximus-accordion-item")
                        .index();

                    methods.toggle.call($accordion, index);

                });

                // Open default section if configured
                if (settings.defaultOpen !== null) {
                    methods.open.call($accordion, settings.defaultOpen);
                }

            });

        },

        open: function (index) {

            var $accordion = $(this);
            var settings = $accordion.data("settings");

            var $item = $accordion.find(".maximus-accordion-item").eq(index);
            var $content = $item.find(".maximus-accordion-content");

            if (!settings.allowMultiple) {
                methods.closeAll.call($accordion);
            }

            if (!$item.hasClass("active")) {

                $item.addClass("active");

                $content
                    .stop(true, true)
                    .slideDown(settings.animationSpeed, "swing");

                if (settings.onOpen)
                    settings.onOpen($item);

            }

        },

        close: function (index) {

            var $accordion = $(this);
            var settings = $accordion.data("settings");

            var $item = $accordion.find(".maximus-accordion-item").eq(index);
            var $content = $item.find(".maximus-accordion-content");

            if ($item.hasClass("active")) {

                $item.removeClass("active");

                $content
                    .stop(true, true)
                    .slideUp(settings.animationSpeed, "swing");

                if (settings.onClose)
                    settings.onClose($item);

            }

        },

        toggle: function (index) {

            var $accordion = $(this);
            var $item = $accordion.find(".maximus-accordion-item").eq(index);

            if ($item.hasClass("active"))
                methods.close.call($accordion, index);
            else
                methods.open.call($accordion, index);

        },

        closeAll: function () {

            var $accordion = $(this);
            var settings = $accordion.data("settings");

            $accordion.find(".maximus-accordion-item")
                .removeClass("active")
                .find(".maximus-accordion-content")
                .stop(true, true)
                .slideUp(settings.animationSpeed);

        }

    };

    $.fn.maximusAccordion = function (method) {

        if (methods[method])
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));

        else if (typeof method === "object" || !method)
            return methods.init.apply(this, arguments);

    };

}(jQuery));