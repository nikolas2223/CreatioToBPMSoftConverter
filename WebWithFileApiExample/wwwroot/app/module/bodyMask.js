const bodyMaskBlock = document.getElementById("bodyMask");

function show() {
    bodyMaskBlock.style = "visibility: visible";
}

function hide() {
    bodyMaskBlock.style = "visibility: hidden";
}

export { show as showBodyMask, hide as hideBodyMask };