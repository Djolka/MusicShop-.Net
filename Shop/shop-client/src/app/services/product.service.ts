import { Injectable } from '@angular/core';
import { Product } from '../models/product.model';
import { Observable, catchError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { HttpErrorHandler } from './http-error-handler.model';


@Injectable({
  providedIn: 'root'
})

export class ProductService extends HttpErrorHandler{

	private products: Observable<Product[]>;
	private readonly productsUrl = 'http://localhost:3000/products/';

	constructor(private http: HttpClient, router: Router) { 
		super(router)
		this.loadProducts();
	}

	private loadProducts(): Observable<Product[]> {
		this.products = this.http.get<Product[]>(this.productsUrl+'productList/').pipe(
			catchError(super.handleError())
		)
		return this.products
	}

	public getProducts(): Observable<Product[]> {
		return this.products
	}

	public getProductById(id: string): Observable<Product> {
		return this.http.get<Product>(this.productsUrl + id).pipe(
			catchError(super.handleError())
		)
	}

	public filterProducts(body: any): Observable<Product[]> {
		return this.http.post<Product[]>(this.productsUrl + 'filter/', body)
	}
}
