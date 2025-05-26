import { Component, OnInit } from '@angular/core';
import { UserService } from './services/user.service';
import { User } from './models/user.model';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
	title = 'shop-client';
	private userId: string;
	public user: User = new User()
	constructor(private userService: UserService) { }

	ngOnInit() {
		this.userId = this.userService.get_id();
		if (this.userId) {
			this.userService.getUserById().subscribe((user) => {
				if (user) {
					this.user = user;
					this.userService.userInfo = user;
					this.userService.log.emit(true);
				} else {
					this.userService.logOut();
				}
			});
		}
	}
}
