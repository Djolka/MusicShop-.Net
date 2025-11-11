import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../models/user.model';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { AuthService } from '../services/auth.service';
import { LoginResponse } from '../models/user-login-response.model';
import { UserSignupDTO } from '../models/user-signup-model';


@Component({
	selector: 'app-user-signup',
	templateUrl: './user-signup.component.html',
	styleUrls: ['./user-signup.component.css']
})
export class UserSignupComponent implements OnInit{
	
	public user: User
	public checkoutForm: FormGroup
	public loggedIn: boolean = false

	constructor (private userService: UserService,
				 private authService: AuthService,
				 private formBuilder: FormBuilder,
				 private router: Router) {
		this.checkoutForm = this.formBuilder.group({
			name: ['', [Validators.required]],
			lastName: ['', [Validators.required]],
			email: ['', [Validators.required, Validators.email]],
			password: ['', [Validators.required]]
		})
		this.user = new User()
	}

	ngOnInit(): void {}

	public submitForm(data: UserSignupDTO) {
		this.userService.addUser(data)
		  	.subscribe({
				next: (data: LoginResponse) => {
					Swal.fire(
						'Welcome ' + data.user.name,
						'We are happy to see you :)!',
						'success'
					  )
					this.checkoutForm.reset()
					this.authService.setSession(data.token, data.user)
					this.loggedIn = true
					this.router.navigate(['/'])
				},
				error: (err) => {
					Swal.fire(
						'Email already in use',
						'Please try again with new email address',
						'warning'
					)
				},
		  	})
	}
}
