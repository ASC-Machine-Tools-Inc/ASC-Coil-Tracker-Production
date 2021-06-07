$("#swapRightBtn").click(function () {
    $("#widthInput").val($("#WIDTH").val());
    $("#thicknessInput").val($("#THICK").val());
});

$("#swapLeftBtn").click(function () {
    $("#WIDTH").val($("#widthInput").val());
    $("#THICK").val($("#thicknessInput").val());
    $("#WEIGHT").val($("#weightInput").val());
    $("#LENGTH").val($("#lengthInput").val());
});