import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private apiUrl ='http://localhost:5096/api/Contact/';
  constructor(private http:HttpClient) { }

  birthdayMonthBasedReport(month : number):Observable<ApiResponse<any>>{
    return this.http.get<ApiResponse<any>>(this.apiUrl+"GetContactRecordBasedOnBirthdayMonthReport/"+month);
  }

  stateBasedReport(state : number):Observable<ApiResponse<any>>{
    return this.http.get<ApiResponse<any>>(this.apiUrl+"GetContactsByStateReport/"+state);
  }

  countryBasedReport(country : number):Observable<ApiResponse<any>>{
    return this.http.get<ApiResponse<any>>(this.apiUrl+"GetContactsCountByCountryReport/"+country);
  }

  genderBasedReport(gender : string):Observable<ApiResponse<any>>{
    return this.http.get<ApiResponse<any>>(this.apiUrl+"GetContactCountByGenderReport/"+gender);
  }
}
