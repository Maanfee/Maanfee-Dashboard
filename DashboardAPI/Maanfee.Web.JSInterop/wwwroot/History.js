/*
 * JavaScript History Library V 1.0.0
 *
 *
 *
 * Date: 02-07-20
 */

window.AppHistory = {
    Length: () => {
        return window.history.length;
    },
    Back: () => {
        return window.history.back();
    },
    Forward() {
        return window.history.forward();
    },
    Go(Index) {
        return window.history.go(Index);
    },
}

export function Length() {
    return AppHistory.Length();
}

export function Back() {
    return AppHistory.Back();
}

export function Forward() {
    return AppHistory.Forward();
}

export function Go(Index) {
    return AppHistory.Go(Index);
}
