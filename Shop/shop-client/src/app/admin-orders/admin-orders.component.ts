import { Component } from '@angular/core';
import { Order } from '../models/order.model';
import { OrderService } from '../services/order.service';
import { User } from '../models/user.model';
import { Product } from '../models/product.model';
import { firstValueFrom } from 'rxjs';
import { ProductService } from '../services/product.service';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-admin-orders',
	templateUrl: './admin-orders.component.html',
	styleUrls: ['./admin-orders.component.css']
})
export class AdminOrdersComponent {

	public ordersList: Order[];
	public selectedOrder: Order;
	public user: User;
	public displayItems: any = []

	constructor(private orderService: OrderService,
		private productService: ProductService) {
		this.orderService.getAllOrders().subscribe(ordersList => {
			this.ordersList = ordersList;
		})
	}


	public async showOrder(order: Order) {
		this.selectedOrder = order;
		this.displayItems = [];
		this.selectedOrder = order;

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

	public approveOrder(orderId: string) {
		console.log(orderId)
		this.orderService.validateOrder(orderId).subscribe({
			next: () => {
				this.selectedOrder.isVerified = true;

				// Update item inside the list so left panel updates
				const index = this.ordersList.findIndex(o => o.id === orderId);
				if (index !== -1) {
					this.ordersList[index].isVerified = true;
				}
				Swal.fire(
					'You have successfully verified order with id: ' + orderId,
					'',
					'success'
				)
			},
			error: (err) => {
				Swal.fire(
					'Error',
					'Something went wrong, please try again.',
					'error'
				)
			}
		})
	}
}
