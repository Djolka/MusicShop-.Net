import { Component } from '@angular/core';
import { Order } from '../models/order.model';
import { OrderService } from '../services/order.service';
import { User } from '../models/user.model';

@Component({
	selector: 'app-admin-orders',
	templateUrl: './admin-orders.component.html',
	styleUrls: ['./admin-orders.component.css']
})
export class AdminOrdersComponent {

	public ordersList: Order[];
	public selectedOrder: Order;
	public user: User;

	constructor(private orderService: OrderService) {
		this.orderService.getAllOrders().subscribe(ordersList => {
			this.ordersList = ordersList;
		})
	}

	public showOrder(order: Order) {
		this.selectedOrder = order;
	}
}
