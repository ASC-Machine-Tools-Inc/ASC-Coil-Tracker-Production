const MAX_NOTE_LENGTH = 15;

function setHtml() {
    // Get CSS
    // Manually adding them for now, since jQuery selector can't search head
    let css = "<head> <link href='/Content/bootstrap.min.css' rel='stylesheet'>" +
        "<link href='/Content/Site.css' rel='stylesheet'>" +
        "<link href='/Content/print.css' rel='stylesheet'> </head>";

    // Get HTML
    let html = "<body><div id='print-table'>" + $("#coilTable")[0].outerHTML + "</div></body>";

    // Add table-sm class to the table.
    html = html.replace("table m-0", "table table-sm m-0");

    // Truncate notes if they're too long.
    /*
    html = html.match(/<td class="clamp">(.*?)<\/td>/g).map(function (note) {
        if (note.length > MAX_NOTE_LENGTH) {
            return note.substring(0, MAX_NOTE_LENGTH - 3) + "...";
        } else {
            return note;
        }
    });
    */

    console.log(html);

    // Load into hidden input value.
    $("#tableHtml").val("<html>" + css + html + "</html>");
}