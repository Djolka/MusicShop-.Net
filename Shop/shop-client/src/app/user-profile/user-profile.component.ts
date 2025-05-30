import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../models/user.model';
import Swal from 'sweetalert2';
import { Subscription } from 'rxjs';
import { countries } from "countries-list";
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
	selector: 'app-user-profile',
	templateUrl: './user-profile.component.html',
	styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {

	public user: User = new User()
	public checkoutForm: FormGroup
	subscription: Subscription

	constructor(private userService: UserService,
		private authService: AuthService,
		private formBuilder: FormBuilder,
		private router: Router) {
		this.refreshUser()
		this.checkoutForm = this.formBuilder.group({
			name: [this.user.name, [Validators.required]],
			lastName: [this.user.lastName, [Validators.required]],
			email: [this.user.email, []],
			password: [this.user.password, []],
			country: [this.user.country, []],
			phoneNumber: [this.user.phoneNumber, []],
			address: [this.user.address, []]
		})
	}

	refreshUser() {
		this.userService.getUserById(this.authService.get_id()).subscribe((user: User) => {
			console.log("Djole: ", user)
			this.user = user
		})
	}

	public submitForm(data: any) {
		data = Object.fromEntries(Object.entries(data).filter(([_, v]) => v != null))
		this.userService.updateUser(data, this.authService.get_id())
			.subscribe({
				next: () => {
					Swal.fire(
						'Successfully updated your informations',
						'Enjoy shopping!',
						'success'
					)
					this.router.navigate(['/'])
				}, 
				error: (err) => {
					Swal.fire(
						'Oops.. Something went wrong',
						'Please try again later.',
						'warning'
					)
				}
			})
	}
}
