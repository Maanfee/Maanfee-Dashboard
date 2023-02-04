
export async function DownloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    const url = URL.createObjectURL(blob);

    TriggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function TriggerFileDownload(fileName, url) {
    const anchorElement = document.createElement("a");
    anchorElement.href = url;

    if (fileName) {
        anchorElement.download = fileName;
    }

    anchorElement.click();
    anchorElement.remove();
}