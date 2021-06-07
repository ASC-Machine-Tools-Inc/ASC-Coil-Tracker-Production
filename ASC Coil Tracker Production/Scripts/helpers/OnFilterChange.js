// Update view whenever length filter changed (non-depleted/all)
$('#lengthFilter').on('change', function () {
    var form = $(this).parents('form');

    form.submit();
});

// Update view whenever search field changed
$('#searchString').on('input', function () {
    var form = $(this).parents('form');

    form.submit();
});

// Refresh view with results from search field
function refresh(result) {
    $("body").html(result);
    $('#searchString').focus();
    $('#searchString')[0].setSelectionRange(1000, 1000);
}