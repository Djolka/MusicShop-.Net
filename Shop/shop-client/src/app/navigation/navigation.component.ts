import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { FavouritesService } from '../services/favourites.service';
import Swal from 'sweetalert2';
import { Product } from '../models/product.model';
import { ProductService } from '../services/product.service';
import { AuthService } from '../services/auth.service';

@Component({
	selector: 'app-navigation',
	templateUrl: './navigation.component.html',
	styleUrls: ['./navigation.component.css']
})

export class NavigationComponent implements OnInit {

	public items: Product[]
	public filteredItems: Product[]
	public loggedIn: boolean = false
	public itemsSize: number = 0
	public favSize: number = 0
	public isUserAdmin = false
	public searchFilter: string = ""

	constructor(private userService: UserService,
		private favouritesService: FavouritesService,
		private cartService: CartService,
		private productService: ProductService,
		private authService: AuthService,
		private router: Router) {
		this.productService.getProducts().subscribe(items => {
			this.items = items
		})
	}

	public logout() {
		this.authService.logout()
		this.loggedIn = false
		Swal.fire(
			'You have logged out',
			'Log in if you want to buy products or save your favourites',
			'success'
		)
		this.router.navigate(['/'])
	}

	ngOnInit(): void {
		console.log(this.isUserAdmin = this.authService.getRole() === 'Admin')
		this.isUserAdmin = this.authService.getRole() === 'Admin';

		this.authService.log.subscribe(login => {
			this.loggedIn = login
			if (this.loggedIn === false) {
				this.favouritesService.clear()
			} else {
				this.favouritesService.getFavList(this.authService.get_id()).subscribe(items => {
					this.favouritesService.setFavListLength(items.length);
				});
			}
		})

		this.authService.isAdmin.subscribe(boolIsAdmin => {
			this.isUserAdmin = boolIsAdmin
		})

		this.cartService.itemsSize.subscribe(size => {
			this.itemsSize = size
		})

		this.favouritesService.favSize.subscribe(size => {
			this.favSize = size
		})
	}

	public filterFunction() {
		if (this.searchFilter.trim() !== "") {
			this.filteredItems = this.items.filter(item => item.name.toLowerCase().includes(this.searchFilter.toLowerCase()));

			return this.filteredItems
		}
		this.filteredItems = []
		return this.filteredItems
	}

	public getToItem(id: any) {
		this.searchFilter = ""
		this.filterFunction()
		this.router.navigate(['/product', id])
	}
}
