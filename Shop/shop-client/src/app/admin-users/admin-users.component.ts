import { Component } from '@angular/core';
import { User } from '../models/user.model';
import { UserService } from '../services/user.service';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order.model';
import { FavouritesService } from '../services/favourites.service';
import { Favourite } from '../models/favourites.model';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-admin-users',
	templateUrl: './admin-users.component.html',
	styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent {

	public usersList: User[];
	public userOrdersList: Order[] = [];
	public userFavouritesList: Favourite[] = [];
	public selectedUser: User;
	public selectedButton: string | null = null;
	public selectedUserId: string | null = null;

	constructor(private userService: UserService,
		private orderService: OrderService,
		private favouritesService: FavouritesService) {
		this.userService.getAllUsers().subscribe(userList => {
			this.usersList = userList;
		})
	}

	public highlightUser(user: User, selectedButton: string) {
		this.selectedUser = user;
		this.selectedUserId = user.id;
		this.selectedButton = selectedButton;
	}

	public ordersByUser(user: User) {
		this.orderService.getOrdersByCustomerID(user.id).subscribe({
			next: (orders) => {
				this.userOrdersList = orders;
			},
			error: (err) => {
				this.userOrdersList = [];
			}
		})
	}

	public favouritesByUser(user: User) {
		this.favouritesService.getFavList(user.id).subscribe({
			next: (favourites) => {
				this.userFavouritesList = favourites;
				console.log(this.userFavouritesList.length)
			},
			error: (err) => {
				this.userFavouritesList = [];
			}
		})
	}

	public onOrdersClick(user: User) {
		this.highlightUser(user, 'orders');
		this.ordersByUser(user);
	}

	public onFavouritesClick(user: User) {
		this.highlightUser(user, 'favourites');
		this.favouritesByUser(user);
	}

	public onChangeRoleClick(user: User) {
		this.highlightUser(user, 'changeRole');
	}

	public saveUserRole(selectedUser: User, role: string) {
		this.userService.changeRole(selectedUser.id, role).subscribe({
			next: (msg) => {
				selectedUser.role = role;
				Swal.fire(
					'You have successfully update role for user ' + selectedUser.name + " " + selectedUser.lastName,
					'',
					'success'
				)
			},
			error: (err) => {
				Swal.fire(
					'Error',
					'Something went wrong, please try again.',
					'error'
				)
			}
		})
	}
}
