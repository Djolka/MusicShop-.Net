import { EventEmitter, Injectable, Output } from '@angular/core';
import { Product } from '../models/product.model';
import { HttpClient } from '@angular/common/http';
import { Favourites } from '../models/favourites.model';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class FavouritesService {

	@Output() favSize: EventEmitter<number> = new EventEmitter()
	private favListLength: number = 0

	private favouritesUrl = 'http://localhost:3000/favourites/'

	constructor(private http: HttpClient) {}

	public addToFavList(productId: String, userId: string): Observable<Favourites> {
		let body = {
			customerId: userId,
			product: productId
		}
		let newFavItem = this.http.post<any>(this.favouritesUrl + 'addFavourites', body)
		this.favListLength += 1
		this.favSize.emit(this.favListLength)

		return newFavItem
	}

	public getFavList(userId: string): Observable<Favourites[]>{
		let res = this.http.get<Favourites[]>(this.favouritesUrl + 'getFavourites/' + userId)
		res.subscribe(items => {
			this.favListLength = items.length
			this.favSize.emit(this.favListLength)
		})
		return res
	}
	
	public removeFromFavList(productId: String, userId: string) {
		let delFav = this.http.delete<any>(this.favouritesUrl + 'deleteFavourite/' + userId + '/' + productId)
		this.favListLength -= 1
		this.favSize.emit(this.favListLength)
		return delFav
	}

	public isInFavList(productId: String, userId: string) {
		let body = {
			customerId: userId,
			product: productId
		}
		return this.http.post<any>(this.favouritesUrl + 'findFavourite/', body)
	}

	public clear() {
		this.favSize.emit(0)
	}
}
