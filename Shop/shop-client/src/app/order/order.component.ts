import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/order.service';
import { Order } from '../models/order.model';
import { UserService } from '../services/user.service';
import { Product } from '../models/product.model';

@Component({
	selector: 'app-order',
	templateUrl: './order.component.html',
	styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {

	public ordersList: Order[] = []
	public displayItems: Product[] = []
	public totalPrice: number
	public selectedOrder: Order

	constructor (private orderService: OrderService,
				 private userService: UserService) {
		this.orderService.getOrdersByCustomerID(this.userService.get_id()).subscribe(list => {
			this.ordersList = list
		})
	}

	ngOnInit(): void {
		
	}

	public showOrder(order: Order) {
		this.displayItems = []
		
		this.displayItems = order.products
		this.totalPrice = order.totalPrice
		this.selectedOrder = order;
		console.log(this.selectedOrder)
		
		window.scrollTo({ top: 0, behavior: 'smooth' });
	}
}
