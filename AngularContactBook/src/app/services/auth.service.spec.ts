import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { User } from '../models/user.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { UpdatePassword } from '../models/update-password.model';
import { ValidateUser } from '../models/validate-user.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock : HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[AuthService],
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //Register User
  it('should register user successfully',()=>{
    //arrange
    const registerUser:User={
      userId: 1,
      firstName: 'string',
      lastName: 'string',
      loginId: 'string',
      email: 'user@example.com',
      contactNumber: 3304071959,
      password: 'Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s!%U.+syh,0 P8sb-XmUqD',
      confirmPassword: 'string',
      favouriteNumber: '1',
      favouriteColor: 'color',
      bestFriend: 'Friend',
      profilePic: '',
      imageByte: ''
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"User register successfully.",
      data:""
    }
    //act
    service.signup(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user register',()=>{
    //arrange
    const registerUser:User={
      userId: 1,
      firstName: 'string',
      lastName: 'string',
      loginId: 'string',
      email: 'user@example.com',
      contactNumber: 3304071959,
      password: 'Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s!%U.+syh,0 P8sb-XmUqD',
      confirmPassword: 'string',
      favouriteNumber: '1',
      favouriteColor: 'color',
      bestFriend: 'Friend',
      profilePic: '',
      imageByte: ''
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.signup(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while register user',()=>{
    //arrange
    const registerUser:User={
      userId: 1,
      firstName: 'string',
      lastName: 'string',
      loginId: 'string',
      email: 'user@example.com',
      contactNumber: 3304071959,
      password: 'Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s!%U.+syh,0 P8sb-XmUqD',
      confirmPassword: 'string',
      favouriteNumber: '1',
      favouriteColor: 'color',
      bestFriend: 'Friend',
      profilePic: '',
      imageByte: ''
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.signup(registerUser).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);
  });

  //Update Password
  it('should update password successfully',()=>{
    //arrange
    const forgetPassword:UpdatePassword={
      password: "bjLRIa+\\o_$/om$Kr$D%#fY3o$Jt0(@(gtc<Gl<=7$sT4$af+='QOej2}>KR1\"%-^&",
      confirmPassword: "password",
      userName: "user"
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:'',
      data:""
    }
    //act
    service.updatePassword(forgetPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/AddNewPassword');
    expect(req.request.method).toBe('PUT');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user forget password',()=>{
    //arrange
    const forgetPassword:UpdatePassword={
      password: "bjLRIa+\\o_$/om$Kr$D%#fY3o$Jt0(@(gtc<Gl<=7$sT4$af+='QOej2}>KR1\"%-^&",
      confirmPassword: "password",
      userName: "user"
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"Something went wrong, please try after sometimes.",
      data:""
    }
    //act
    service.updatePassword(forgetPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/AddNewPassword');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while forget password',()=>{
    //arrange
    const forgetPassword:ValidateUser={
      userName: 'user',
      favouriteNumber: '1',
      favouriteColor: 'color',
      bestFriend: 'friend'
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.updatePassword(forgetPassword).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/AddNewPassword');
    expect(req.request.method).toBe('PUT');
    req.flush({},mockHttpError);

  });

  //Validate Forgot Password
  it('should validate forgot password successfully',()=>{
    //arrange
    const ValidateForgotPassword:ValidateUser={
      userName: 'user',
      favouriteNumber: '1',
      favouriteColor: 'color',
      bestFriend: 'friend'
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:'',
      data:""
    }
    //act
    service.forgotPassword(ValidateForgotPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/ValidateForgotPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user forget password',()=>{
    //arrange
    const ValidateForgotPassword:ValidateUser={
      userName: 'user',
      favouriteNumber: '1',
      favouriteColor: 'color',
      bestFriend: 'friend'
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"Something went wrong, please try after sometimes.",
      data:""
    }
    //act
    service.forgotPassword(ValidateForgotPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/ValidateForgotPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while forget password',()=>{
    //arrange
    const ValidateForgotPassword:UpdatePassword={
      password: "bjLRIa+\\o_$/om$Kr$D%#fY3o$Jt0(@(gtc<Gl<=7$sT4$af+='QOej2}>KR1\"%-^&",
      confirmPassword: "password",
      userName: "user"
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.forgotPassword(ValidateForgotPassword).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5096/api/Auth/ValidateForgotPassword');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });
});
