/*
 * JavaScript Screen Library V 1.1.0
 *
 *
 *
 * Date: 02-01-31
 */

window.AppScreen = {
    Width: () => {
        return screen.width;
    },
    Height: () => {
        return screen.height;
    },
    AvailableWidth() {
        return screen.availWidth;
    },
    AvailableHeight() {
        return screen.availHeight;
    },
    ColorDepth() {
        return screen.colorDepth;
    },
    PixelDepth() {
        return screen.pixelDepth;
    }
}

export function Width() {
    return AppScreen.Width();
}

export function Height() {
    return AppScreen.Height();
}

export function AvailableWidth() {
    return AppScreen.AvailableWidth();
}

export function AvailableHeight() {
    return AppScreen.AvailableHeight();
}

export function ColorDepth() {
    return AppScreen.ColorDepth();
}

export function PixelDepth() {
    return AppScreen.PixelDepth();
}