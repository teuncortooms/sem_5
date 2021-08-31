import { Component, OnInit } from '@angular/core';
import { WeatherForecast } from '../../models/WeatherForecast';
import { WeatherService } from '../../services/weather/weather.service';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  public forecasts?: WeatherForecast[];

  constructor(private service: WeatherService) { }

  ngOnInit(): void {
    this.getForecasts();
  }

  getForecasts(): void {
    this.service.getForecasts().subscribe(forecasts => this.forecasts = forecasts);
  }

}
