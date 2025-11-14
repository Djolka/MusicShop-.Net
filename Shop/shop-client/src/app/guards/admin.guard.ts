// admin.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
	providedIn: 'root'
})
export class AdminGuard implements CanActivate {

	constructor(private authService: AuthService, private router: Router) { }

	canActivate(): boolean {
		const user = this.authService.get_id;
		const userRole = this.authService.getRole()
		console.log(userRole)
		if (user && userRole === 'Admin') {
			return true;
		}

		// not admin → redirect
		this.router.navigate(['/']);
		return false;
	}
}
