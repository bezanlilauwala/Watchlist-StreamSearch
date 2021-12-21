import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BackendHTMLService } from '../backend/backend-html.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  constructor(http: HttpClient, backend: BackendHTMLService) {
    let token = localStorage.getItem("jwt");
    http.get<WeatherForecast[]>(backend.url + "WeatherForecast", {
      headers: {
        "Content-Type": "application/json"
      }
    }).subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
