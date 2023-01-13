/*
 * JavaScript Fullscreen Library V 1.0.0
 *
 *
 *
 * Date: 1401-10-22
 */

window.AppFullscreen = {
    OpenFullscreen: (Id) => {

        var elem = document.getElementById(Id);

        if (elem.requestFullscreen) {
            elem.requestFullscreen();
        } else if (elem.webkitRequestFullscreen) { /* Safari */
            elem.webkitRequestFullscreen();
        } else if (elem.msRequestFullscreen) { /* IE11 */
            elem.msRequestFullscreen();
        }
    },
    CloseFullscreen: () => {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { /* Safari */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE11 */
            document.msExitFullscreen();
        }
    },
    ToggleFullscreen() {
        if ((document.fullScreenElement && document.fullScreenElement !== null) || (!document.mozFullScreen && !document.webkitIsFullScreen)) {
            if (document.documentElement.requestFullScreen) {
                document.documentElement.requestFullScreen();
            }
            else if (document.documentElement.mozRequestFullScreen) { /* Firefox */
                document.documentElement.mozRequestFullScreen();
            }
            else if (document.documentElement.webkitRequestFullScreen) {   /* Chrome, Safari & Opera */
                document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
            }
            else if (document.msRequestFullscreen) { /* IE/Edge */
                document.documentElement.msRequestFullscreen();
            }
        }
        else {
            if (document.cancelFullScreen) {
                document.cancelFullScreen();
            }
            else if (document.mozCancelFullScreen) { /* Firefox */
                document.mozCancelFullScreen();
            }
            else if (document.webkitCancelFullScreen) {   /* Chrome, Safari and Opera */
                document.webkitCancelFullScreen();
            }
            else if (document.msExitFullscreen) { /* IE/Edge */
                document.msExitFullscreen();
            }
        }
    }
}

export function OpenFullscreen(Id) {
    return AppFullscreen.OpenFullscreen(Id);
}

export function CloseFullscreen() {
    return AppFullscreen.CloseFullscreen();
}

export function ToggleFullscreen() {
    return AppFullscreen.ToggleFullscreen();
}

