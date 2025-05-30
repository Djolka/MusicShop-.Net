import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../models/user.model';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { LoginResponse } from '../models/user-login.model';
import { AuthService } from '../services/auth.service';

@Component({
	selector: 'app-user-login',
	templateUrl: './user-login.component.html',
	styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {

	public checkoutForm: FormGroup

	constructor(private userService: UserService,
		private authService: AuthService,
		private formBuilder: FormBuilder,
		private router: Router) {
		this.checkoutForm = this.formBuilder.group({
			email: ['', [Validators.required, Validators.email]],
			password: ['', [Validators.required]]
		})
	}

	ngOnInit(): void { }

	public submitForm(data: any) {
		this.userService.getUserByEmailAndPassword(data)
			.subscribe({
				next: (data: LoginResponse) => {
					Swal.fire(
						'Welcome ' + data.user.name,
						'We are happy to see you :)!',
						'success'
					)
					this.checkoutForm.reset()
					console.log("Token: ", data.token)
					console.log("User: ", data.user)
					this.authService.setSession(data.token, data.user)
					this.router.navigate(['/'])
				},
				error: (err) => {
					Swal.fire(
						'Error',
						'Invalid email or password. Please try again.',
						'error'
					)
				},
			})
	}
}
