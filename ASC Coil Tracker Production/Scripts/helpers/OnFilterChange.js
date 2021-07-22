var timeout = null;

// Update view whenever length filter changed (non-depleted/all)
$('#lengthFilter').on('change', function () {
    var form = $(this).parents('form');

    form.submit();
});

/* Disable for now: Buggy with constant updating, might fix with caching index.
// Update view whenever search field changed and user done typing.
$('#searchString').on('input', function () {
    clearTimeout(timeout);

    // Clear timeout if already set, preventing update.
    timeout = setTimeout(function () {
        var form = $('#searchString').parents('form');

        $.ajax({
            type: "GET",
            data: form.serialize(),
            success: function (data) {
                $("body").html(data);
                $('#searchString').focus();
                $('#searchString')[0].setSelectionRange(1000, 1000);
            }
        });
    }, 250);
    // Need a longer timeout since querying production database is slower
    // than locally. Whoops!
});
*/