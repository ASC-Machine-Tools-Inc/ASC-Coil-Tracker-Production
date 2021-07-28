// Constants for the notes table column length
const TAG_OPENING_LENGTH = 18;
const TAG_CLOSING_LENGTH = 5;
const MAX_NOTE_LENGTH = 15;

function setHtml() {
    // Get CSS
    // Manually adding them for now, since jQuery selector can't search head
    let css = "<head> <link href='/Content/bootstrap.min.css' rel='stylesheet'>" +
        "<link href='/Content/Site.css' rel='stylesheet'>" +
        "<link href='/Content/print.css' rel='stylesheet'> </head>";

    // Get HTML
    let html = "<body><div id='print-table'>" + $("#coilTable")[0].innerHTML + "</div></body>";

    // Add table-sm class to the table.
    html = html.replace("table m-0", "table table-sm m-0");

    // Truncate notes if they're too long.
    let notesRegex = /<td class="clamp">((.|\n)*?)<\/td>/g;
    html = html.replace(notesRegex, function (note) {
        // Strip the tags and whitespace from the note.
        let strippedNote = note.substring(TAG_OPENING_LENGTH,
            note.length - TAG_CLOSING_LENGTH).trim();

        if (strippedNote.length > MAX_NOTE_LENGTH) {
            // Truncate and re-add the tags.
            strippedNote = strippedNote.substring(0, 25).trim();
            note = "<td class='clamp'>" + strippedNote + "...</td>";
        }

        return note;
    });

    // Load into hidden input value.
    $("#tableHtml").val("<html>" + css + html + "</html>");
}