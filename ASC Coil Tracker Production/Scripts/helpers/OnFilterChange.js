var changed = false;

// Update view whenever length filter changed (non-depleted/all)
$('#lengthFilter').on('change', function () {
    var form = $(this).parents('form');

    form.submit();
});

// Update view whenever search field changed
$('#searchString').on('input', function () {
    changed = true;
});

// Watch for changes to search field
setInterval(function () {
    if (changed) {
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

        changed = false;
    }
}, 1);