
export function addClass(size, islandscape) {

    document.body.classList.add(size);

    if (islandscape == "true") {
        document.body.classList.add("landscape");
    }

    return true;
}

export function removeClass(size, islandscape) {

    document.body.classList.remove(size);

    if (islandscape == "true") {
        document.body.classList.remove("landscape");
    }

    return true;
}

export function printwindow(IsBackward) {
    setTimeout(function () {
        window.print();
        if (IsBackward) {
            window.history.back(-1)
        };
        window.close();
    }, 200);
    return true;
}

export function PrintComponent(id, title) {
    let iframe = document.createElement("iframe");
    let content = document.getElementById("printable-content-" + id).innerHTML;

    iframe.style.display = "none";
    document.body.appendChild(iframe);

    let doc = iframe.contentWindow.document;

    doc.open();
    doc.write(`<!DOCTYPE html>
                <html lang="en">
                <head>
                    <title>${title}</title>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                </head>
                <body>
                    ${content}
                </body>
                </html>`);
    doc.close();

    setTimeout(function () {
        iframe.contentWindow.focus();
        iframe.contentWindow.print();
        document.body.removeChild(iframe);
    }, 200);
}