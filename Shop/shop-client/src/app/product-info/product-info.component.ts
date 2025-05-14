import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { Product } from '../models/product.model';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { CartService } from '../services/cart.service';
import { FavouritesService } from '../services/favourites.service';
import { UserService } from '../services/user.service';
import Swal  from 'sweetalert2';

@Component({
	selector: 'app-product-info',
	templateUrl: './product-info.component.html',
	styleUrls: ['./product-info.component.css']
})
export class ProductInfoComponent implements OnInit{
	
	public product: Product = new Product()
	public quantity: number = 1
	public isInFav: boolean
	public loggedIn: boolean
	private pId: string

  	constructor (private productService: ProductService,
				 private cartService: CartService,
				 private favouritesService: FavouritesService,
				 private userService: UserService,
				 private route: ActivatedRoute) {
		this.route.paramMap.subscribe(params => { 
			this.pId = params.get('id')

			this.productService
				.getProductById(this.pId)
				.subscribe((product: Product) => this.product = product) 
		})
	}

	ngOnInit(): void {
		this.isInFavlist(this.pId);
		this.loggedIn = this.userService.get_id() !== null
		this.userService.log.subscribe(login => {
			this.loggedIn = login
		})
	}

	public decQuantity() {
		this.quantity -= 1
	}

	public incQuantity() {
		this.quantity += 1
	}

	public addToCart() {
		this.product.quantity = this.quantity
		this.quantity = 1
		this.cartService.addToCart(this.product)
	}

	public addToFavList() {
		this.favouritesService.addToFavList(this.product._id, this.userService.get_id()).subscribe(item => {
			Swal.fire(
				'You have successfully added item to your wishlist!',
				'',
				'success'
			)
		})
		this.isInFav = true
	}

	public removeFromFavList() {
		this.favouritesService.removeFromFavList(this.product._id, this.userService.get_id()).subscribe(item => {
			Swal.fire(
				'You have successfully removed item from your wishlist!',
				'',
				'success'
			)
		})
		this.isInFav = false
	}

	public isInFavlist(productId: String) {
		this.favouritesService.isInFavList(productId, this.userService.get_id()).subscribe(item => {
			this.isInFav = item.found
		})
	}
}
