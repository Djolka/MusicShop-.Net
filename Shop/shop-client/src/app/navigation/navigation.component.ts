import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { FavouritesService } from '../services/favourites.service';
import Swal  from 'sweetalert2';
import { Product } from '../models/product.model';
import { ProductService } from '../services/product.service';

@Component({
	selector: 'app-navigation',
	templateUrl: './navigation.component.html',
	styleUrls: ['./navigation.component.css']
})

export class NavigationComponent implements OnInit{

	public items: Product[]
	public filteredItems: Product[]
	public loggedIn: boolean = false
	public itemsSize: number = 0
	public favSize: number = 0
	public searchFilter: string = ""

	constructor (private userService: UserService,
				 private favouritesService: FavouritesService,
				 private cartService: CartService,
				 private productService: ProductService,		 
				 private router: Router) {
		this.productService.getProducts().subscribe(items => {
			this.items = items
		})
		if(this.userService.get_id() === undefined) {
			this.loggedIn = true
		}
	}

	public logout() {
		this.loggedIn = false
		this.userService.logOut()
		Swal.fire(
			'You have logged out',
			'Log in if you want to buy products or save your favourites',
			'success'
		)
		this.router.navigate(['/'])
	}

	ngOnInit(): void {
		this.userService.log.subscribe(login => {
			this.loggedIn = login
			if(this.loggedIn === false) {
				this.favouritesService.clear()
			} else {
				this.favouritesService.getFavList(this.userService.get_id()).subscribe(items => {
					this.favSize = items.length
				})
			}
		})
		this.cartService.itemsSize.subscribe(size => {
			this.itemsSize = size
		})
		
		this.favouritesService.favSize.subscribe(size => {
			this.favSize = size
		}) 
	}

	public filterFunction() {
		if(this.searchFilter.trim() !== ""){
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
