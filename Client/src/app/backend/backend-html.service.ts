import { HttpUrlEncodingCodec } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BackendHTMLService {
  dev: boolean = false;
  test: boolean = true;
  prod: boolean = false;
  home: boolean = false;
  
  url: string = "";
  constructor() { 
    if (this.dev) {
      this.url = "https://localhost:44317/";
    }
    else if (this.test) {
      this.url = "http://54.67.35.92:5200/";
    }
    else if (this.prod) {
      this.url = "http://dev1.us-west-1.elasticbeanstalk.com/"
    }
    else if (this.home) {
      this.url = "https:/75.82.10.29:44317";
    }
  }
}
