﻿const DENSITIES = {
    STEEL: 0.283,
    ALUM: 0.098,
    COPPER: 0.324
}
Object.freeze(DENSITIES);

/* Updates the coil calculator fields based on the given values.
 * Formulas:
 * Length = (weight / (thickness * width * density)) / 12
 * Weight = width * density * pi * ((outer diameter / 2)^2 - (inner diameter / 2)^2)
 * Pounds per inches wide (PIW) = weight / width
 * Area = width * length
 */
function calculateFields() {
    let coil_values = {}

    coil_values.density = DENSITIES[$("#densitySelect").val()];
    coil_values.od = $("#odInput").val();
    coil_values.id = $("#idInput").val();
    coil_values.width = $("#widthInput").val();
    coil_values.thickness = $("#thicknessInput").val();

    // Save calculator fields
    sessionStorage.setItem("density", $("#densitySelect").val());
    sessionStorage.setItem("od", coil_values.od);
    sessionStorage.setItem("id", coil_values.id);
    sessionStorage.setItem("width", coil_values.width);
    sessionStorage.setItem("thickness", coil_values.thickness);

    coil_values.weight = Math.round(coil_values.width * coil_values.density * Math.PI *
        ((Math.pow((coil_values.od / 2), 2)) - (Math.pow((coil_values.id / 2), 2))));
    if (coil_values.weight > 0) {
        $("#weightInput").val(coil_values.weight);
    }

    coil_values.length = Math.round((coil_values.weight /
        (coil_values.thickness * coil_values.width * coil_values.density)) / 12);
    if (coil_values.length > 0) {
        $("#lengthInput").val(coil_values.length);
    }

    coil_values.piw = coil_values.weight / coil_values.width;
    if (coil_values.piw > 0) {
        $("#piwInput").val(coil_values.piw.toFixed(2));
    }

    coil_values.area = Math.round((coil_values.width / 12) * coil_values.length);
    if (coil_values.area > 0) {
        $("#areaInput").val(coil_values.area);
    }
}

$(function () {
    // On page load, grab stored calculator values.
    $("#densitySelect").val(sessionStorage.getItem("density"));
    $("#odInput").val(sessionStorage.getItem("od"));
    $("#idInput").val(sessionStorage.getItem("id"));
    $("#widthInput").val(sessionStorage.getItem("width"));
    $("#thicknessInput").val(sessionStorage.getItem("thickness"));
    calculateFields();

    $(".qty").on("change keyup", calculateFields);
})