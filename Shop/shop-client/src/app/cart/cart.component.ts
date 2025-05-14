import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Product } from '../models/product.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/user.model';
import { OrderService } from '../services/order.service';
import Swal  from 'sweetalert2';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent {

	public user: User = new User()
	public items: Product[] = []
	public totalPrice:number = 0
	public phoneNumber: string
	public address: string 
	public country: string 

	constructor (private cartService: CartService,
				 private userService: UserService,
				 private orderService: OrderService,
				 private formBuilder: FormBuilder) {
		this.refreshUser()
		
		this.items = this.cartService.getitems()
		this.totalPrice = this.cartService.getTotalPrice()
	}

	refreshUser() {
		this.userService.getUserById().subscribe((user:User) => {
			this.user = user
			this.phoneNumber = user.phoneNumber
			this.address =  user.address
			this.country = user.country
		})
	}

	public removeItem(id: string) {
		this.cartService.removeItem(id)
		this.items = this.cartService.getitems()
		this.totalPrice = this.cartService.getTotalPrice()
	}

	public clearCart() {
		this.items = []
		this.totalPrice = 0
		this.cartService.clearCart()
	}

	public addOrder() {
		if(!this.phoneNumber) {
			Swal.fire(
				'Please enter your phone number',
				'',
				'warning'
			)
			return
		}
		if(!this.address) {
			Swal.fire(
				'Please enter your address',
				'',
				'info'
			)
			return
		}
		if(!this.country) {
			Swal.fire(
				'Please enter your country',
				'',
				'info'
			)
			return
		}

		this.orderService.createAnOrder(this.totalPrice, this.items, this.userService.get_id()).subscribe(order => {
			console.log(order)
		})
		this.cartService.clearCart()
		this.items = []
		this.totalPrice = 0
		Swal.fire(
			'You have successfully created an order',
			'Enjoy playing :)',
			'success'
		)
	}
}
