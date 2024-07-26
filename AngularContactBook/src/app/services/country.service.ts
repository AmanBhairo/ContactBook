import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';
import { Country } from '../models/country.model';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private apiUrl ='http://localhost:5096/api/Country/';

  constructor(private http:HttpClient) { }

  getAllCountries():Observable<ApiResponse<Country[]>>{
    return this.http.get<ApiResponse<Country[]>>(this.apiUrl+"GetAllCountry");
  }
  
}
