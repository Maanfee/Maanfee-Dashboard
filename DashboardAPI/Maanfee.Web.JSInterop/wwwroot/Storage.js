/*
 * JavaScript Local Storage Library V 1.0.0
 *
 *
 *
 * Date: 01-10-06
 */

window.AppLocalStorage = {
    Clear: () => {
        window.localStorage.clear();
    },
    Get: (KeyName) => {
        return window.localStorage[KeyName];
    },
    Key: (Index) => {
        return window.localStorage.key(Index);
    },
    Length: () => {
        return window.localStorage.length;
    },
    Set: (KeyName, Value) => {
        window.localStorage[KeyName] = Value;
    },
    Remove: (KeyName) => {
        window.localStorage.removeItem(KeyName);
    },
    Keys: () => {

        var Length = window.localStorage.length;
        let KeyNames = new Array();

        for (var Index = 0; Index < Length; Index++) {
            KeyNames.push(window.localStorage.key(Index));
        }

        if (KeyNames.length > 0) {
            return JSON.stringify(KeyNames);
        }

        return;
    }
}

export function Clear() {
    return AppLocalStorage.Clear();
}

export function Get(Key) {
    return AppLocalStorage.Get(Key);
}

export function Key(Index) {
    return AppLocalStorage.Key(Index);
}

export function Keys() {
    return AppLocalStorage.Keys();
}

export function Length() {
    return AppLocalStorage.Length();
}

export function Set(Key, Value) {
    return AppLocalStorage.Set(Key, Value);
}

export function Remove(Key) {
    return AppLocalStorage.Remove(Key);
}

	//public async Task SetAsync<T>(string key, T value)
		//{
		//	await JS.InvokeVoidAsync("AppLocalStorage.set", key, JsonConvert.SerializeObject(value));
		//}

		//public async Task<T> GetAsync<T>(string key)
		//{
		//	var value = await JS.InvokeAsync<string>("AppLocalStorage.get", key);

		//	return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		//}