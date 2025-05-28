import { Injectable } from '@angular/core';
import { Order } from '../models/order.model';
import { Product } from '../models/product.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
	providedIn: 'root'
})
export class OrderService {

	public orderList: Order[] = []
	private ordersUrl = 'https://localhost:5001/orders/'

	constructor(private http: HttpClient, private authService: AuthService) { }

	public createAnOrder(totalPrice: number, items: Product[], customerId: string): Observable<Order> { 
		const orderProducts = items.map(item => ({
			productId: item.id,
			quantity: item.quantity
		}));

		const body = {
			customerId: customerId,
			orderProducts: orderProducts,
			date: new Date(),
			totalPrice: totalPrice
		};

		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});

		return this.http.post<Order>(this.ordersUrl + 'createOrder/', body, { headers });
	}

	public getOrdersByCustomerID(id: any): Observable<Order[]>{
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});
		return this.http.get<Order[]>(this.ordersUrl + 'userOrders/'+ id, { headers })
	}

}
