import { Injectable } from "@angular/core";

// auth.service.ts
@Injectable({ providedIn: 'root' })
export class AuthService {
    private _isLoggedIn = false;
    private _userId: any = null;

    constructor() { }

    public checkLocalStorage() {
        const userId = localStorage.getItem('userId');
        if (userId) {
            this._userId = userId;
            this._isLoggedIn = true;
        } else {
            this._isLoggedIn = false;
            this._userId = null;
        }
    }

    public isLoggedIn(): boolean {
        return this._isLoggedIn;
    }

    public getUser() {
        return this._userId;
    }
}
