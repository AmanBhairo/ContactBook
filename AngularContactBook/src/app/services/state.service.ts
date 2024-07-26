import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Country } from '../models/country.model';
import { States } from '../models/state.model';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private apiUrl ='http://localhost:5096/api/State/';

  constructor(private http:HttpClient) { }

  getStatesByCountryId(contactId:number):Observable<ApiResponse<States[]>>{
    return this.http.get<ApiResponse<States[]>>(this.apiUrl+"GetStateByCountryId/"+contactId);
  }
}
