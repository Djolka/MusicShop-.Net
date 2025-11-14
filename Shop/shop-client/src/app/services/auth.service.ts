import { HttpClient } from "@angular/common/http";
import { EventEmitter, Injectable, Output } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "../models/user.model";

@Injectable({ providedIn: 'root' })
export class AuthService {
    @Output() log: EventEmitter<any> = new EventEmitter()
    @Output() isAdmin: EventEmitter<any> = new EventEmitter()
    
    private TOKEN_KEY = 'token';
    private USER_KEY = 'userId';
    private EMAIL_KEY = 'email';
    private ROLE_KEY = 'role';

    constructor(private http: HttpClient, private router: Router) { }

    public setSession(token: string, user: User) {
        localStorage.setItem(this.TOKEN_KEY, token);
        localStorage.setItem(this.USER_KEY, user.id);
        localStorage.setItem(this.EMAIL_KEY, user.email);
        localStorage.setItem(this.ROLE_KEY, user.role);

        this.log.emit(true)
        this.isAdmin.emit(this.getRole() === "Admin")
    }

    public get_id() {
		return localStorage.getItem(this.USER_KEY)
	}

    logout() {
        localStorage.clear()
		this.log.emit(false)
    }

    getToken(): string | null {
        return localStorage.getItem(this.TOKEN_KEY);
    }

    getRole(): string {
        return localStorage.getItem(this.ROLE_KEY);
    }
}
