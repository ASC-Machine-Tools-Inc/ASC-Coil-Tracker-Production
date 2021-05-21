$('#lengthFilter').on('change', function () {
    var form = $(this).parents('form');

    form.submit();
});