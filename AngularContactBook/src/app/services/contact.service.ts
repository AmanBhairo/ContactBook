import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contacts } from '../models/contact.model';
import { AddContact } from '../models/addcontact.model';
import { UpdateContact } from '../models/updateContact.model';
import { ContactCountryState } from '../models/contact-country-state.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl ='http://localhost:5096/api/Contact/';
  constructor(private http:HttpClient) { }

  getAllContacts():Observable<ApiResponse<Contacts[]>>{
    return this.http.get<ApiResponse<Contacts[]>>(this.apiUrl+"GetAllContacts");
  }

  GetContactById(contactId:number | undefined):Observable<ApiResponse<Contacts>>{
    return this.http.get<ApiResponse<Contacts>>(this.apiUrl+'GetContactById/'+contactId);
  }

  addContact(contact:AddContact):Observable<ApiResponse<string>>{
    return this.http.post<ApiResponse<string>>(this.apiUrl+'CreateContact',contact);
  }

  UpdateContact(contact:UpdateContact):Observable<ApiResponse<string>>{
    return this.http.put<ApiResponse<string>>(this.apiUrl+'EditContact',contact);
  }

  deleteContactById(contactId:number | undefined):Observable<ApiResponse<string>>{
    return this.http.delete<ApiResponse<string>>(this.apiUrl+'DeleteContact/'+contactId);
  }
  
  GetContactWithStateCountryById(contactId:number | undefined):Observable<ApiResponse<ContactCountryState>>{
    return this.http.get<ApiResponse<ContactCountryState>>(this.apiUrl+'GetContactById/'+contactId);
  }

  //
  getAllContactsCount() : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'TotalContacts?search=no');
  }

  getAllContactsCountByLetter(letter:string) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'TotalContacts?letter='+letter);
  }

  getAllContactsCountByLetterSearch(letter:string,search:string) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'TotalContacts?letter='+letter+'&search='+search);
  }


  getAllContactsWithPagination(pageNumber: number,pageSize:number,sort:string) : Observable<ApiResponse<Contacts[]>>{
    return this.http.get<ApiResponse<Contacts[]>>(this.apiUrl+'GetAllPaginatedContacts?page='+pageNumber+'&pageSize='+pageSize+'&sort='+sort);
  }

  GetPaginatedContactsByLetter(pageNumber: number,pageSize:number,letter:string,sort:string) : Observable<ApiResponse<Contacts[]>>{
    return this.http.get<ApiResponse<Contacts[]>>(this.apiUrl+'GetPaginatedContactsByLetter?page='+pageNumber+'&pageSize='+pageSize+'&letter='+letter+'&sort='+sort+'&search=no');
  }

  GetPaginatedContactsByLetterSearch(pageNumber: number,pageSize:number,letter:string,sort:string,search:string) : Observable<ApiResponse<Contacts[]>>{
    return this.http.get<ApiResponse<Contacts[]>>(this.apiUrl+'GetPaginatedContactsByLetter?page='+pageNumber+'&pageSize='+pageSize+'&letter='+letter+'&sort='+sort+'&search='+search);
  }

  GetTotalContactsForFavourite(letter:string) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'TotalContactsForFavourite?letter='+letter);
  }

  GetPaginatedContactsForFavourite(pageNumber: number,pageSize:number,sort:string) : Observable<ApiResponse<Contacts[]>>{
    return this.http.get<ApiResponse<Contacts[]>>(this.apiUrl+'GetAllPaginatedContactsForFavourite?page='+pageNumber+'&pageSize='+pageSize+'&sort='+sort);
  }

  GetPaginatedContactsByLetterForFavourite(pageNumber: number,pageSize:number,letter:string,sort:string) : Observable<ApiResponse<Contacts[]>>{
    return this.http.get<ApiResponse<Contacts[]>>(this.apiUrl+'GetPaginatedContactsByLetterForFavourite?page='+pageNumber+'&pageSize='+pageSize+'&letter='+letter+'&sort='+sort);
  }
}
