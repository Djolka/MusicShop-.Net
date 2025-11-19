import { EventEmitter, Injectable, Output } from '@angular/core';
import { Product } from '../models/product.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Favourite } from '../models/favourites.model';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './auth.service';
import { FavouriteDTO } from '../models/favourites-dto.model';

@Injectable({
	providedIn: 'root'
})
export class FavouritesService {

	private favListLength: number = 0
	public favSize = new BehaviorSubject<number>(this.favListLength);

	private favouritesUrl = 'https://localhost:5001/favourites/'

	constructor(private http: HttpClient, private authService: AuthService) { }

	public addToFavList(product: Product, userId: string): Observable<Favourite> {
		let body: FavouriteDTO = {
			customerId: userId,
			product: product
		}

		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});
		
		let newFavItem = this.http.post<any>(this.favouritesUrl + 'addFavourites', body, { headers });
		this.favListLength += 1
		this.favSize.next(this.favListLength)

		return newFavItem
	}

	public getFavList(userId: string): Observable<Favourite[]> {
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});

		let res = this.http.get<Favourite[]>(this.favouritesUrl + 'getFavourites/' + userId, { headers })
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
		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});

		let delFav = this.http.delete<any>(this.favouritesUrl + 'deleteFavourite/' + userId + '/' + productId, { headers })
		this.favSize.next(this.favListLength)
		return delFav
	}

	public isInFavList(product: Product, userId: string) {
		let body: FavouriteDTO = {
			customerId: userId,
			product: product
		}

		const headers = new HttpHeaders({
			'Authorization': `Bearer ${this.authService.getToken()}`
		});

		return this.http.post<any>(this.favouritesUrl + 'findFavourite/', body, { headers })
	}

	public setFavListLength(length: number) {
		this.favListLength = length;
		this.favSize.next(length);
	}

	public clear() {
		this.favSize.next(0)
	}
}
