import { Component } from '@angular/core';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
	selector: 'app-admin-users',
	templateUrl: './admin-users.component.html',
	styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent {

	public usersList: User[];
	public selectedUser: User;

	constructor (private userService: UserService) {
		this.userService.getAllUsers().subscribe(userList => {
			this.usersList = userList;
		})
	}

	public showUser(user: User) {
		this.selectedUser = user;
	}

}
