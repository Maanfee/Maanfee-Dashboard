
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

export function printwindow() {
    setTimeout(function () {
        window.print();
        window.history.back(-1);
    }, 200);
    return true;
}