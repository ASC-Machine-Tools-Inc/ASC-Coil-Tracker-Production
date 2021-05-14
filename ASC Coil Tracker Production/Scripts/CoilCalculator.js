function calculateFields() {
    let coil_values = {}

    // Change 0.283 to density
    coil_values.weight = ($("#widthInput").val() * 0.283 *
        ((Math.PI * (Math.pow(($("odInput") / 2), 2))) -
            (Math.PI * (Math.pow(($("idInput") / 2), 2)))));
    $("#weightInput").val(coil_values.weight);
}

function updateFields() {
    $(".qty").on("change keyup", calculateFields)
}