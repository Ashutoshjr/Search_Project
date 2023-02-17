import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  searchDataResult: DataFile | undefined;

  searchData: string | undefined

  buildings: Buildings[] = []

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {

    //http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result =>
    //{

    //  this.forecasts = result;


    //},
    //  error => console.error(error));


    http.get<DataFile>(baseUrl + 'weatherforecast').subscribe(result =>
    {
      this.searchDataResult = result;
      this.buildings = result.buildings;
      console.log('search data', this.searchDataResult.buildings)

    }, error => console.error(error));


  }

 


  onSubmit(form:NgForm) {

    this.searchData = form.value;

  }


}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}


interface DataFile {

  buildings: Buildings[],
  groups: Groups[],
  locks: Locks[],
  media:Media[]


}

interface Buildings {
  id: string;
  shortCut: string;
  name: string;
  description: string;
}

interface Groups {
  id: string;
  name: string;
  description: string;
}


interface Locks {
  id: string;
  buildingId: string;
  type: string;
  name: string;
  description: string;
  serialNumber: string;
  floor: string;
  roomNumber: string;

}


interface Media {
  id: string;
  groupId: string;
  type: string;
  owner: string;
  description: string;
  serialNumber: string;

}
