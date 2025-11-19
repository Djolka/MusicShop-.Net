import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorHandler } from './http-error-handler.model';
import { Observable, catchError, of } from 'rxjs';
import { User } from '../models/user.model'
import { UpdateUserDTO } from '../models/update-user.model';
import { LoginResponse } from '../models/user-login-response.model';
import { UserLoginDTO } from '../models/user-login.model';
import { UserSignupDTO } from '../models/user-signup-model';
import { AuthService } from './auth.service';



@Injectable({
	providedIn: 'root'
})
export class UserService extends HttpErrorHandler {

	@Output() log: EventEmitter<any> = new EventEmitter()

	public userInfo: User
	private user: Observable<User>

	private usersURL = 'https://localhost:5001/users/'

	constructor(private http: HttpClient, router: Router, private authService: AuthService) {
		super(router)
	}

	public addUser(formData: UserSignupDTO): Observable<LoginResponse> { // signup
		const body = {
			...formData
		}

		return new Observable<LoginResponse>((observer) => {
			this.http.post<LoginResponse>(this.usersURL + 'signup/', body).subscribe({
				next: (res) => {
					this.userInfo = res.user;
					observer.next(res);
					observer.complete();
				},
				error: (err) => {
					observer.error(err);
				}
			});
		});
	}

	public loginUser(formData: UserLoginDTO): Observable<LoginResponse> { // login
		const body = { ...formData };

		return new Observable<LoginResponse>((observer) => {
			this.http.post<LoginResponse>(this.usersURL + 'login/', body).subscribe({
				next: (res) => {
					this.userInfo = res.user;
					observer.next(res);
					observer.complete();
				},
				error: (err) => {
					observer.error(err);
				}
			});
		});
	}

	public updateUser(formData: UpdateUserDTO, id: string): Observable<User> { // profile settings
		const body = {
			...formData
		}
		console.log(body)
		
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});

		this.user = this.http.put<User>(this.usersURL + 'update/' + id, body, { headers })
		return this.user
	}

	public getUserById(id: string): Observable<User> {
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});

		return this.http.get<User>(this.usersURL + 'user/' + id, { headers })
	}

	public getAllUsers(): Observable<User[]> {
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});
		
		return this.http.get<User[]>(this.usersURL + 'users/', { headers })
	}
 
	public logOut() {
		localStorage.clear()
		this.log.emit(false)
	}

	public changeRole(userId: string, role: string): Observable<any> {
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});
		// console.log(this.usersURL + 'setRole/' + userId + "/" + role)
		return this.http.post(this.usersURL + 'setRole/' + userId + "/" + role, {}, { headers })
	}
}
