(function ($) {

    $.fn.maximusDatePicker = function (options) {

        const settings = $.extend({
            dateFormat: 'MM/DD/YYYY',
            minDate: null,
            maxDate: null,
            disableDates: [],
            onDateSelect: null,
            disableWeekends: false
        }, options);

        let activeCalendar = null;

        function buildCalendar(input, date) {

            const cal = $(`
        <div class="maximus-calendar">
            <div class="maximus-calendar-header">
                <span class="material-icons prev">chevron_left</span>

                <div class="maximus-month-year-picker">
                    <select class="maximus-month-select"></select>
                    <select class="maximus-year-select"></select>
                </div>

                <span class="material-icons next">chevron_right</span>
            </div>

            <div class="maximus-calendar-days"></div>
        </div>
    `);

            function render(d) {

                // 🔥 Month dropdown
                let months = moment.months();
                let monthOptions = months.map((m, i) =>
                    `<option value="${i}" ${i === d.month() ? 'selected' : ''}>${m}</option>`
                ).join('');

                // 🔥 Year dropdown
                let currentYear = moment().year();
                let yearOptions = '';

                for (let y = currentYear - 50; y <= currentYear + 50; y++) {
                    yearOptions += `<option value="${y}" ${y === d.year() ? 'selected' : ''}>${y}</option>`;
                }

                cal.find('.maximus-month-select').html(monthOptions);
                cal.find('.maximus-year-select').html(yearOptions);

                // 🔥 Days render (same as yours)
                let start = d.clone().startOf('month');
                let end = d.clone().endOf('month');

                let html = '';
                let startDay = start.day();

                for (let i = 0; i < startDay; i++) {
                    html += `<div class="maximus-day empty"></div>`;
                }

                for (let i = 1; i <= end.date(); i++) {

                    let current = d.clone().date(i);
                    let formatted = current.format('YYYY-MM-DD');

                    let today = moment().format('YYYY-MM-DD');

                    let selectedDate = input.val()
                        ? moment(input.val(), settings.dateFormat).format('YYYY-MM-DD')
                        : null;

                    let disabled = false;

                    // ✅ Min Date
                    if (settings.minDate && current.isBefore(settings.minDate, 'day')) {
                        disabled = true;
                    }

                    // ✅ Max Date
                    if (settings.maxDate && current.isAfter(settings.maxDate, 'day')) {
                        disabled = true;
                    }

                    // ✅ Weekends
                    if (settings.disableWeekends && (current.day() === 0 || current.day() === 6)) {
                        disabled = true;
                    }

                    // ✅ Specific Dates
                    if (Array.isArray(settings.disableDates) && settings.disableDates.includes(formatted)) {
                        disabled = true;
                    }

                    html += `
                    <div class="maximus-day 
                        ${disabled ? 'disabled' : ''} 
                        ${formatted === today ? 'today' : ''} 
                        ${formatted === selectedDate ? 'active' : ''}" 
                        data-date="${formatted}">
                        ${i}
                    </div>`;
                }

                cal.find('.maximus-calendar-days').html(html);
            }

            render(date);

            // 🔥 PREV
            cal.on('click', '.prev', function (e) {
                e.stopPropagation();
                date.subtract(1, 'month');
                render(date);
            });

            // 🔥 NEXT
            cal.on('click', '.next', function (e) {
                e.stopPropagation();
                date.add(1, 'month');
                render(date);
            });

            // 🔥 MONTH CHANGE
            cal.on('change', '.maximus-month-select', function (e) {
                e.stopPropagation();
                date.month(parseInt($(this).val()));
                render(date);
            });

            // 🔥 YEAR CHANGE
            cal.on('change', '.maximus-year-select', function (e) {
                e.stopPropagation();
                date.year(parseInt($(this).val()));
                render(date);
            });

            // 🔥 DATE SELECT
            cal.on('click', '.maximus-day:not(.disabled)', function (e) {
                e.stopPropagation();

                let selected = $(this).data('date');
                input.val(moment(selected).format(settings.dateFormat));

                if (settings.onDateSelect) {
                    settings.onDateSelect(selected);
                }

                closeCalendar();
            });

            // prevent close
            cal.on('click', function (e) {
                e.stopPropagation();
            });

            return cal;
        }

        function openCalendar(input) {

            closeCalendar();

            let date = input.val()
                ? moment(input.val(), settings.dateFormat)
                : moment();

            const cal = buildCalendar(input, date);
            activeCalendar = cal;

            // ✅ append inside wrapper
            $('body').append(cal);

            // ✅ position inside wrapper
            const rect = input[0].getBoundingClientRect();

            cal.css({
                position: 'fixed',
                top: rect.bottom + 6,
                left: rect.left,
                width: input.outerWidth(),
                zIndex: 999999
            });

            setTimeout(() => {
                $(document).on('click.maximus', function () {
                    closeCalendar();
                });
            }, 0);
        }

        function closeCalendar() {
            $('.maximus-calendar').remove();   // 🔥 remove all calendars
            activeCalendar = null;
            $(document).off('click.maximus');
        }

        $(window).on('scroll resize', function () {
            if (activeCalendar && activeCalendar.length) {
                const rect = input[0].getBoundingClientRect();

                activeCalendar.css({
                    top: rect.bottom + 6,
                    left: rect.left
                });
            }
        });


        return this.each(function () {

            const input = $(this);
            const icon = input.siblings('.maximus-icon');

            input.on('click', function (e) {
                e.stopPropagation();

                // 🔥 prevent reopening if already open
                if ($(this).siblings('.maximus-calendar').length) return;

                openCalendar(input);
            });

            icon.on('click', function (e) {
                e.stopPropagation();

                if (input.siblings('.maximus-calendar').length) return;

                openCalendar(input);
            });
        });
    };

})(jQuery);