import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order.model';
import { UserService } from '../services/user.service';
import { Product } from '../models/product.model';
import { ProductService } from '../services/product.service';

import { firstValueFrom } from 'rxjs';

@Component({
	selector: 'app-order',
	templateUrl: './order.component.html',
	styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {

	public ordersList: Order[] = []
	public displayItems: any = []
	public totalPrice: number
	public selectedOrder: Order

	constructor(private orderService: OrderService,
		private userService: UserService,
		private productService: ProductService) {
		this.orderService.getOrdersByCustomerID(this.userService.get_id()).subscribe(list => {
			this.ordersList = list.sort((a, b) => a.date < b.date ? 1 : -1);
		})
	}

	ngOnInit(): void {

	}

	public async showOrder(order: Order) {
		this.displayItems = [];
		console.log("Order: ", order);

		for (const op of order.orderProducts) {
			const product: Product = await firstValueFrom(this.productService.getProductById(op.productId));

			if (product) {
				this.displayItems.push({
					...product,
					quantity: op.quantity
				});
			}
		}
	}
}
