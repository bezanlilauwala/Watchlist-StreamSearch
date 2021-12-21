import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BackendHTMLService } from '../backend/backend-html.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private backend: BackendHTMLService) { }

  register(username: string, password: string, email: string) {
    let query: string = "?Username=" + username + "&Password=" + password + "&Email=" + email;
    return this.http.post(this.backend.url + "api/Auth/Create" + query, "", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

}