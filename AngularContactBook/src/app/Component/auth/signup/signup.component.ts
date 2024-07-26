import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {

  imageUrl: string | ArrayBuffer | null = null;
  fileToUpload: File | null = null;
  fileName='';

  user:User = {
    userId: 0,
    firstName: "",
    lastName: "",
    loginId: "",
    email: "",
    contactNumber: 0,
    password: "",
    confirmPassword: "",
    favouriteNumber: '',
    favouriteColor: '',
    bestFriend:'',
    profilePic:'',
    imageByte:'',
  };

  loading=false;

  constructor(private authService:AuthService, private router:Router) {}

  checkPasswords(form: NgForm):void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(signUpForm:NgForm):void{
    if(signUpForm.valid){
      console.log(signUpForm.value);
      this.user.profilePic=this.fileName;
      this.authService.signup(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.router.navigate(['/signupsuccess']);
          }else{
            alert(response.message);
          }
          this.loading=false;
        },
        error:(err)=>{
          console.error('Failed to register user');
          alert(err.error.message);
        },
        complete:()=>{
          console.log('completed');
        }
      })
      this.router.navigate(['/signup']);
    }
  }

  onFileSelected(event: any) {
    this.fileToUpload = event.target.files[0];
    if (this.fileToUpload) {
      const reader = new FileReader();
      reader.onload=()=>{
        this.user.imageByte = (reader.result as string).split(',')[1]; 
      }
      reader.readAsDataURL(this.fileToUpload);
      this.fileName = this.fileToUpload.name;
      this.readImage(this.fileToUpload);
    }
  }

  readImage(file: File) {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.imageUrl = e.target.result;
    };
    reader.readAsDataURL(file);
  }
}
