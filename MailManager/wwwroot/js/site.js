function showHidePassword(elementId) {
    var x = document.getElementById(elementId);
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function setValue(elementId, value) {
    var x = document.getElementById(elementId);
    x.value = value;
}

function toggleCheckbox(elementId, check) {
    var x = document.getElementById(elementId);
    var y = document.getElementById(elementId + "Text");
    if (check) {
        x.checked = true;
        y.value = "nur senden";
    } else {
        x.checked = false;
        y.value = "senden und empfangen";
    }
}