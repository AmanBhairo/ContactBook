import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UpdateUser } from 'src/app/models/updateUser.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent {
  loginId:string|null|undefined;
  imageSrc:string= '';
  user:UpdateUser={
    userId: 0,
    firstName: '',
    lastName: '',
    loginId: '',
    contactNumber: '',
    email: '',
    profilePic: '',
    imageByte:'',
  }

  constructor(private authService:AuthService, private router:Router) {}

  ngOnInit(): void {
    this.authService.getUserName().subscribe((username:string|null|undefined)=>{
      this.loginId = username;
      this.loadUserDetail(this.loginId);
    })
  }

  loadUserDetail(loginId:string|null|undefined):void{
    this.authService.GetUserByLoginId(loginId).subscribe({
      next:(response)=>{
        if(response.success){
          this.user = response.data;
          if (this.user.profilePic) {
            this.imageSrc = 'assets/' + this.user.profilePic; 
          }
          else{
            this.imageSrc = 'assets/DefaultImage.jpg'; 
          }
        }else{
          console.error('Failed to fech user: ',response.message);
        }
      },
      error:(error)=>{
        console.error('Error fetching user: ',error);
      }
    })
  }

  updateUser(id:number):void{
    this.router.navigate(['/updateUser',id]);
  }
}
