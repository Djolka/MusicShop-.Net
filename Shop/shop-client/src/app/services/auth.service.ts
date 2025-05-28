import { HttpClient } from "@angular/common/http";
import { EventEmitter, Injectable, Output } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs/internal/Observable";
import { User } from "../models/user.model";
import { LoginResponse } from "../models/user-login.model";

@Injectable({ providedIn: 'root' })
export class AuthService {
    @Output() log: EventEmitter<any> = new EventEmitter()
    
    private TOKEN_KEY = 'token';
    private USER_KEY = 'userId';
    private EMAIL_KEY = 'email';

    constructor(private http: HttpClient, private router: Router) { }

    // login(credentials: { email: string; password: string }): Observable<User> {
    //     return this.http.post<LoginResponse>('https://localhost:5001/users/login', credentials).pipe(
    //         tap((res) => {
    //             this.setSession(res.token, res.user);
    //         })
    //     );
    // }

    public setSession(token: string, user: User) {
        localStorage.setItem(this.TOKEN_KEY, token);
        localStorage.setItem(this.USER_KEY, user.id);
        localStorage.setItem(this.EMAIL_KEY, user.email);

        this.log.emit(true)
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

    // getUser(): User | null {
    //     const data = localStorage.getItem(this.USER_KEY);
    //     return data ? JSON.parse(data) : null;
    // }

    // isAuthenticated(): boolean {
    //     return !!this.getToken();
    // }
}
