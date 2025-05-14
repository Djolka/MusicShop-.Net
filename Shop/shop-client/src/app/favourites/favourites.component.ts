import { Component, OnInit } from '@angular/core';
import { FavouritesService } from '../services/favourites.service';
import { UserService } from '../services/user.service';
import { Favourites } from '../models/favourites.model';
import Swal  from 'sweetalert2';

@Component({
	selector: 'app-favourites',
	templateUrl: './favourites.component.html',
	styleUrls: ['./favourites.component.css']
})
export class FavouritesComponent implements OnInit {

	public favList: Favourites[] = []
	public loggedIn: boolean

	constructor(private favouritesService: FavouritesService,
				private userService: UserService) {
		this.favouritesService.getFavList(this.userService.get_id()).subscribe(list => {
			this.favList = list
		})
	}

	ngOnInit(): void {
		this.userService.log.subscribe(login => {
			this.loggedIn = login
			if(this.loggedIn === false) {
				this.favList = []
			}else {
				
			}
		})
	}

	public removeFromFavList(productId: String) {
		this.favouritesService.removeFromFavList(productId, this.userService.get_id()).subscribe(item => {
			Swal.fire(
				'You have successfully removed item from your wishlist!',
				'',
				'success'
			)
		})
		this.favouritesService.getFavList(this.userService.get_id()).subscribe(items => {
			this.favList = items
		})
	}
}
