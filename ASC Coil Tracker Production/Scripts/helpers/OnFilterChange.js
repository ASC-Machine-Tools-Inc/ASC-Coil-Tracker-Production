// Update view whenever length filter changed (non-depleted/all)
$('#lengthFilter').on('change', function () {
    var form = $(this).parents('form');

    form.submit();
});

// Update view whenever search field changed
$('#searchString').on('input keyup', function () {
    var form = $(this).parents('form');

    form.submit();
});