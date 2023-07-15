import { showBodyMask, hideBodyMask } from "./bodyMask.js"

function downloadFile(blob, oFormElement) {
    const fileName = `BPMSoft_${oFormElement[0]?.files[0]?.name}`;
    const url = window.URL.createObjectURL(blob);
    const tempElement = document.createElement("a"); // создать ссылку с данными
    tempElement.setAttribute("href", url);
    tempElement.setAttribute("download", fileName);
    document.body.appendChild(tempElement);
    tempElement.click();
    tempElement.remove();
}

async function AJAXSubmit() {
    const oFormElement = document.getElementById("FileUpload_Form");
    const formData = new FormData(oFormElement);

    try {
        showBodyMask();
        const requestResult = await fetch(oFormElement.action, {
            method: 'POST',
            body: formData
        }).then(
            response => response.json(),
            error => console.log(error)
        );
        console.log(requestResult);

        const requestFile = await fetch(`api/File/${requestResult}`)
            .then(
                response => response.blob(),
                error => console.log(error)
            ).then(blob => {
                hideBodyMask();
                downloadFile(blob, oFormElement);
                window.location.href = '/';
            });
    } catch (error) {
        console.error('Error:', error);
    }
}

export { AJAXSubmit };