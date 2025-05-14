import { Injectable } from '@angular/core';
import { Order } from '../models/order.model';
import { Product } from '../models/product.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class OrderService {

	public orderList: Order[] = []
	private ordersUrl = 'http://localhost:3000/orders/'

	constructor(private http: HttpClient) { }

	public createAnOrder(totalPrice: number, items: Product[], customerId: string): Observable<Order> { 
		let body = {
			customerId: customerId,
			products: items,
			date: new Date(),
			totalPrice: totalPrice
		}
		return this.http.post<Order>(this.ordersUrl + 'createOrder/', body)
	}

	public getOrdersByCustomerID(id: any): Observable<Order[]>{
		return this.http.get<Order[]>(this.ordersUrl + 'userOrders/'+ id)
	}

}
