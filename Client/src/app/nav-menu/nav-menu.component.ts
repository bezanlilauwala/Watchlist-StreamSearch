import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../login/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;

  constructor(private authService : AuthService) {}

  isLoggedIn$: Observable<boolean> = this.authService.isLoggedIn;

  ngOnInit() {
    this.isLoggedIn$ = this.authService.isLoggedIn;
  }


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logOut() {
    this.authService.logout();
  }

}
