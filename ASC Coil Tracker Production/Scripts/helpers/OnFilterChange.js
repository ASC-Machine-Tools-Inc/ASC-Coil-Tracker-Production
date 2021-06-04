// Update view whenever length filter changed (non-depleted/all)
$('#lengthFilter').on('change', function () {
    var form = $(this).parents('form');

    form.submit();
});

// Update view whenever search field changed
$('#searchString').on('input keyup', function () {
    var form = $(this).parents('form');

    // form.submit();
    // $(this).focus().setSelectionRange(1000, 1000);
});

$(function () {
    var search = $('#searchString');
    if (search.val().length > 0) {
        search.focus();
        search.setSelectionRange(1000, 1000);
    }
})