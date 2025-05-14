import { EventEmitter, Injectable, Output } from '@angular/core';
import { Product } from '../models/product.model';
import { HttpErrorHandler } from './http-error-handler.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Injectable({
	providedIn: 'root'
})
export class CartService extends HttpErrorHandler {

	private items: Product[] = []
	@Output() itemsSize: EventEmitter<number> = new EventEmitter()
	private readonly ordersUrl = 'http://localhost:3000/cart/'

	constructor(private http: HttpClient, router: Router) {
		super(router)
	}

	public addToCart(product: Product) {
		let foundSameItem = this.items.find(item => item._id === product._id)
		if(foundSameItem === undefined){
			this.items.push(product)
		} else {
			foundSameItem.quantity += product.quantity
		}
		this.itemsSize.emit(this.items.length)
		Swal.fire(
			'You have successfully added the product to the cart!',
			'',
			'success'
		)
	}

	public getitems(): Product[] {
		return this.items
	}

	public itemsLength(): number { 
		return this.items.length
	}

	public getTotalPrice(): number {
		let sum = 0
		this.items.map(({price, quantity}) => ({price, quantity}))
				.forEach(pair => {
					sum += pair.price*pair.quantity
				})
	
		return sum
	}

	public removeItem(id:string):void {
		this.items = this.items.filter((item) => item._id !== id)
		this.itemsSize.emit(this.items.length)
	}

	public clearCart(): Product[] {
		this.items = []
		this.itemsSize.emit(this.items.length)
		return this.items
	}
}
