import { Component, OnInit } from '@angular/core';
import { UserService } from './services/user.service';
import { User } from './models/user.model';
import { AuthService } from './services/auth.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
	title = 'shop-client';
	private userId: string;
	public user: User = new User()
	constructor(private userService: UserService, private authService: AuthService) { }

	ngOnInit() {
		this.userId = this.authService.get_id();
		if (this.userId) {
			this.userService.getUserById(this.userId).subscribe((user) => {
				console.log("Test: ", user)
				if (user) {
					this.user = user;
					this.userService.userInfo = user;
					this.authService.log.emit(true);
				} else {
					this.authService.logout();
				}
			});
		}
	}
}
