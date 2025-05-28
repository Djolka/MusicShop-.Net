import { Component, OnInit } from '@angular/core';
import { FavouritesService } from '../services/favourites.service';
import { UserService } from '../services/user.service';
import { Favourites } from '../models/favourites.model';
import Swal from 'sweetalert2';
import { AuthService } from '../services/auth.service';

@Component({
	selector: 'app-favourites',
	templateUrl: './favourites.component.html',
	styleUrls: ['./favourites.component.css']
})
export class FavouritesComponent implements OnInit {

	public favList: Favourites[] = []
	public loggedIn: boolean

	constructor(private favouritesService: FavouritesService,
		private userService: UserService,
		private authService: AuthService) {
		this.favouritesService.getFavList(this.authService.get_id()).subscribe(list => {
			this.favList = list
		})
	}

	ngOnInit(): void {
		this.authService.log.subscribe(login => {
			this.loggedIn = login
			if (this.loggedIn === false) {
				this.favList = []
			} else {

			}
		})
	}

	public removeFromFavList(productId: String) {
		this.favouritesService.removeFromFavList(productId, this.authService.get_id()).subscribe({
			next: () => {
				Swal.fire(
					'You have successfully removed item from your wishlist!',
					'',
					'success'
				)
				
				this.favouritesService.getFavList(this.authService.get_id()).subscribe(items => {
					this.favList = items;
					this.favouritesService.setFavListLength(items.length);
				});
			},
			error: (err) => {
				Swal.fire(
					'There was an error removing the item from your wishlist',
					'',
					'warning'
				)
			}
		})

	}
}
