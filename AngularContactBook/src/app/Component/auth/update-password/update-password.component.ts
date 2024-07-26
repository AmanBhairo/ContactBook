import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UpdatePassword } from 'src/app/models/update-password.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrls: ['./update-password.component.css']
})
export class UpdatePasswordComponent implements OnInit{
  username:string='';
  user:UpdatePassword = {
    password: '',
    confirmPassword: '',
    userName: ''
  };

  loading=false;

  constructor(private authService:AuthService, private router:Router, private route:ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.username = params['username'];
    });
  }

  checkPasswords(form: NgForm):void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(updatePasswordForm:NgForm):void{
    if(updatePasswordForm.valid){
      this.user.userName = this.username;
      this.authService.updatePassword(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.router.navigate(['/passwordupdatesuccess']);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.error('Failed to update password');
          alert(err.error.message);
        },
        complete:()=>{
          console.log('completed');
        }
      });
      this.router.navigate(['/updateuserauthpassword',this.username]);
    }
  }
}
