import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidateUser } from 'src/app/models/validate-user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  user:ValidateUser = {
    userName: '',
    favouriteNumber: '',
    favouriteColor: '',
    bestFriend: ''
  };

  loading=false;

  constructor(private authService:AuthService, private router:Router) {}

  validateUser(forgotPasswordForm:NgForm):void{
    if(forgotPasswordForm.valid){
      this.authService.forgotPassword(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.updatePassword(this.user.userName);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.error('Failed to validate user');
          alert(err.error.message);
        },
        complete:()=>{
          console.log('completed');
        }
      })
      this.router.navigate(['/forgotpassword']);
    }
  }

  updatePassword(username:string):void{
    this.router.navigate(['/updateuserauthpassword',username]);
  }
}
