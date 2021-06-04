document.addEventListener('keydown', function (event) {
    if (event.keyCode === 13 && event.target.nodeName === 'INPUT') {
        var form = event.target.form;
        var index = Array.prototype.indexOf.call(form, event.target);
        form.elements[index + 1].focus();
        event.preventDefault();
    }
});