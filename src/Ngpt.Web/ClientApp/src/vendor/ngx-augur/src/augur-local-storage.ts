import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class AugurLocalStorageManager {
    store<T>(obj: T, key: string) {
        localStorage.setItem(key, JSON.stringify(obj));
    }

    load<T>(key: string, defaultVal: T = null): T {
        var objStr = localStorage.getItem(key);
        if (objStr) {
            return JSON.parse(objStr);
        }
        else {
            return defaultVal;
        }
    }

    remove(key: string) {
        localStorage.removeItem(key);
    }

    clear() {
        localStorage.clear();
    }
}
