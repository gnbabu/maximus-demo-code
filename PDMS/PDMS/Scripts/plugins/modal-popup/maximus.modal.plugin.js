(function ($) {
    $.fn.modalPlugin = function (options) {

        var settings = $.extend({
            modalId: '',
            modalWidth: '600px',   // Default modal width (can be overridden)
            onOpen: null,
            onClose: null,
            onSave: null
        }, options);

        if (!settings.modalId) {
            console.error('Modal ID is required');
            return this;
        }

        var $modal = $(settings.modalId);

        if ($modal.length === 0) {
            console.error('Modal not found:', settings.modalId);
            return this;
        }

        // Scope this modal so the plugin's theme CSS only ever
        // applies to modals the plugin itself manages, never to any
        // other Bootstrap modal already present in the host page.
        $modal.addClass('maximus-modal');

        // Apply dynamic width to the modal dialog
        $modal.find('.modal-dialog').css('max-width', settings.modalWidth); // Apply modal width

        // OPEN
        this.open = function () {
            $modal.modal('show');
            if (typeof settings.onOpen === 'function') {
                settings.onOpen($modal);
            }
        };

        // CLOSE
        this.close = function () {
            $modal.modal('hide');
            if (typeof settings.onClose === 'function') {
                settings.onClose($modal);
            }
        };

        // SAVE TRIGGER (plugin DOES NOT read data)
        this.save = function () {
            if (typeof settings.onSave === 'function') {
                settings.onSave($modal); // 🔥 pass modal reference
            }
        };

        return this;
    };

    // -------------------------
    // ✅ STATIC METHODS (alert / confirm)
    // -------------------------
    $.modalPlugin = {

        alert: function (opts) {

            let {
                title = 'Alert',
                message = '',
                okText = 'OK',
                width = '400px',
                onOk = null
            } = opts || {};

            let modalId = 'alertModal_' + Date.now();

            let html = `
                <div id="${modalId}" class="modal fade maximus-modal">
                    <div class="modal-dialog" style="max-width:${width}">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h5>${title}</h5>
                            </div>
                            <div class="modal-body">${message}</div>
                            <div class="modal-footer">
                                <button class="btn btn-primary btn-ok">${okText}</button>
                            </div>
                        </div>
                    </div>
                </div>
            `;

            $('body').append(html);

            let $modal = $('#' + modalId);

            $modal.on('click', '.btn-ok', function () {
                $modal.modal('hide');
                if (onOk) onOk();
            });

            $modal.on('hidden.bs.modal', function () {
                $modal.remove();
            });

            $modal.modal('show');
        },

        confirm: function (opts) {

            let {
                title = 'Confirm',
                message = '',
                okText = 'Yes',
                cancelText = 'No',
                width = '400px',
                onConfirm = null,
                onCancel = null
            } = opts || {};

            let modalId = 'confirmModal_' + Date.now();

            let html = `
                <div id="${modalId}" class="modal fade maximus-modal">
                    <div class="modal-dialog" style="max-width:${width}">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h5>${title}</h5>
                            </div>
                            <div class="modal-body">${message}</div>
                            <div class="modal-footer">
                                <button class="btn btn-secondary btn-cancel">${cancelText}</button>
                                <button class="btn btn-primary btn-ok">${okText}</button>
                            </div>
                        </div>
                    </div>
                </div>
            `;

            $('body').append(html);

            let $modal = $('#' + modalId);

            $modal.on('click', '.btn-ok', function () {
                $modal.modal('hide');
                if (onConfirm) onConfirm();
            });

            $modal.on('click', '.btn-cancel', function () {
                $modal.modal('hide');
                if (onCancel) onCancel();
            });

            $modal.on('hidden.bs.modal', function () {
                $modal.remove();
            });

            $modal.modal('show');
        }

    };
})(jQuery);