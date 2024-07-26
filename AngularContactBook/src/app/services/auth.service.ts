import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, tap } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { localStorageKeys } from './helpers/localStorageKeys';
import { LocalstorageService } from './helpers/localstorage.service';
import { User } from '../models/user.model';
import { UpdateUser } from '../models/updateUser.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl ='http://localhost:5096/api/Auth/';
private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(localStorageKeys.TockenName));
private userNameSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(localStorageKeys.userId));

  constructor(private http:HttpClient, private localStorageHelper:LocalstorageService) { }

  signup(user:any):Observable<ApiResponse<string>>{
    const body = user;
    return this.http.post<ApiResponse<string>>(this.apiUrl+"Register",body);
  }

  signin(username:string, password:string):Observable<ApiResponse<string>>{
    const body = {username,password};
    return this.http.post<ApiResponse<string>>(this.apiUrl +"Login",body).pipe(
      tap(response=>{
        if(response.success){
          this.localStorageHelper.setItem(localStorageKeys.TockenName,response.data);
          this.localStorageHelper.setItem(localStorageKeys.userId,username);
          this.authState.next(this.localStorageHelper.hasItem(localStorageKeys.TockenName));
          this.userNameSubject.next(username);
        }
      })
    );
  }

  signOut(){
    this.localStorageHelper.removeItem(localStorageKeys.TockenName);
    this.localStorageHelper.removeItem(localStorageKeys.userId);
    this.authState.next(false);
    this.userNameSubject.next(null);
  }

  isAuthenticated(){
    return this.authState.asObservable();
  }

  getUserName():Observable<string | null | undefined>{
    return this.userNameSubject.asObservable();
  }

  forgotPassword(userDetail:any):Observable<ApiResponse<string>>{
    const body = userDetail;
    return this.http.post<ApiResponse<string>>(this.apiUrl+"ValidateForgotPassword",body);
  }

  updatePassword(userPasswordDetail:any):Observable<ApiResponse<string>>{
    const body = userPasswordDetail;
    return this.http.put<ApiResponse<string>>(this.apiUrl+"AddNewPassword",body);
  }

  updateUser(updatedUserDetails:any):Observable<ApiResponse<string>>{
    const body = updatedUserDetails;
    return this.http.put<ApiResponse<string>>(this.apiUrl+"EditUser",body)
  }

  GetUserByLoginId(loginId:any | undefined):Observable<ApiResponse<UpdateUser>>{
    return this.http.get<ApiResponse<UpdateUser>>(this.apiUrl+'GetUserByUserName/'+loginId);
  }

  private profileUpdatedSource = new Subject<void>();

   // Method to emit profile update event
   emitProfileUpdated(): void {
    this.profileUpdatedSource.next();
  }

  // Method to subscribe to profile update event
  onProfileUpdated(): Observable<void> {
    return this.profileUpdatedSource.asObservable();
  }
}
