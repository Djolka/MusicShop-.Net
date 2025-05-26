import { EventEmitter, Injectable, Output } from '@angular/core';
import { Product } from '../models/product.model';
import { HttpClient } from '@angular/common/http';
import { Favourites } from '../models/favourites.model';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class FavouritesService {

	private favListLength: number = 0
	public favSize = new BehaviorSubject<number>(this.favListLength);

	private favouritesUrl = 'http://localhost:3000/favourites/'

	constructor(private http: HttpClient) { }

	public addToFavList(product: Product, userId: string): Observable<Favourites> {
		let body = {
			customerId: userId,
			product: product
		}
		let newFavItem = this.http.post<any>(this.favouritesUrl + 'addFavourites', body)
		this.favListLength += 1
		this.favSize.next(this.favListLength)

		return newFavItem
	}

	public getFavList(userId: string): Observable<Favourites[]> {
		let res = this.http.get<Favourites[]>(this.favouritesUrl + 'getFavourites/' + userId)
		res.subscribe({
			next: (items) => {
				this.favListLength = items.length
				this.favSize.next(this.favListLength)
				
			}, 
			error: (err) => {
				console.log(err)
			}
		})
		return res
	}

	public removeFromFavList(productId: String, userId: string) {
		let delFav = this.http.delete<any>(this.favouritesUrl + 'deleteFavourite/' + userId + '/' + productId)
		this.favSize.next(this.favListLength)
		return delFav
	}

	public isInFavList(product: Product, userId: string) {
		let body = {
			customerId: userId,
			product: product
		}
		return this.http.post<any>(this.favouritesUrl + 'findFavourite/', body)
	}

	public setFavListLength(length: number) {
		this.favListLength = length;
		this.favSize.next(length);
	}

	public clear() {
		this.favSize.next(0)
	}
}
