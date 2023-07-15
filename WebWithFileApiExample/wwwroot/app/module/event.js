import { AJAXSubmit } from "./file.js";

function subscribeEvents() {
    document.getElementById("FileUpload_FormFile")
        .addEventListener("change", updateSelection);

    document.getElementById("FileUpload_Form")
        .addEventListener("submit", AJAXSubmit);
}

function updateSelection(e) {
    if (e.target.files[0]) {
        const selectionSpan = document.getElementById("selectionSpan");
        selectionSpan.style = "background-color: #e74c3c;";
        selectionSpan.textContent = "selected";
    }
}

export { subscribeEvents };