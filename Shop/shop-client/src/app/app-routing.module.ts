import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserLoginComponent } from './user-login/user-login.component';
import { UserSignupComponent } from './user-signup/user-signup.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { CarouselComponent } from './carousel/carousel.component';
import { HomeComponent } from './home/home.component';
import { ProductInfoComponent } from './product-info/product-info.component';
import { CartComponent } from './cart/cart.component';
import { FavouritesComponent } from './favourites/favourites.component';
import { OrderComponent } from './order/order.component';


const routes: Routes = [
	{path: '', component: HomeComponent},
	{path: 'orders', component: OrderComponent},
	{path: 'favourites', component: FavouritesComponent},
	{path: 'product/:id', component: ProductInfoComponent},
	{path: 'login', component: UserLoginComponent},
	{path: 'signup', component: UserSignupComponent},
	{path: 'profile', component: UserProfileComponent},
	{path: 'cart', component: CartComponent}
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})

export class AppRoutingModule { }
