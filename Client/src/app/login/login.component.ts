import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from './auth.service';
import { NavMenuComponent } from '../nav-menu/nav-menu.component'
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {  
  invalidLogin: boolean = false;
  
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  login(form: NgForm) {
    this.invalidLogin = false;
    const credentials = JSON.stringify(form.value);
    this.authService.login(credentials)
    .subscribe((response: any) => {
      console.log(response);
      if (response.result === false) {
        this.invalidLogin = true;
        return;
      }
      const token = response.token;
      const username = response.username;
      const userId = response.id;

      localStorage.setItem("jwt", token);
      localStorage.setItem("username", username);
      localStorage.setItem("userId", userId);
      
      this.invalidLogin = false;
      this.authService.loggedIn.next(true);
      this.router.navigate(['/']);
    }, () => this.invalidLogin = true);
  }

}

