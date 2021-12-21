import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { BackendHTMLService } from '../backend/backend-html.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private router: Router, private backend: BackendHTMLService) { }

  loggedIn = new BehaviorSubject<boolean>(false);

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  login(credentials: string) {
    return this.http.post(this.backend.url + "api/Auth/Login", credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  logout() {
    this.loggedIn.next(false);
    localStorage.removeItem('jwt');
    localStorage.removeItem('username');
    localStorage.removeItem('userId');
    this.router.navigate(['/login']);
  }

}
