import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UpdateUser } from 'src/app/models/updateUser.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css']
})
export class UpdateUserComponent {
  @ViewChild('imageInput') imageInput!: ElementRef;
  userId:number | undefined;
  loginId:string|null|undefined;

  imageUrl: string | ArrayBuffer | null = null;
  fileToUpload: File | null = null;
  fileName='';
  tempProfilePic:string='';

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

  imageByte2: string ='';
  displayImageUrl:string | ArrayBuffer |null =null;
  loading: boolean =false;

  constructor(private authService:AuthService, private router:Router, private route:ActivatedRoute,) {}

  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.userId = params['id'];
    })
    this.authService.getUserName().subscribe((username:string|null|undefined)=>{
      this.loginId = username;
      this.loadUserDetail(this.loginId);
    })
  }

  onFileSelected(event: any) {
    this.fileToUpload = event.target.files[0];
    if (this.fileToUpload) {
      const reader = new FileReader();
      reader.onload=()=>{
        this.user.imageByte = (reader.result as string).split(',')[1]; 
        this.imageByte2 = this.user.imageByte;
      }
      reader.readAsDataURL(this.fileToUpload);
      this.user.profilePic = this.fileToUpload.name;
      this.tempProfilePic = this.user.profilePic;
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

  removeImage(event: any){
    if(event.target.checked){
      this.imageUrl=null;
      this.user.imageByte='';
      this.tempProfilePic = this.user.profilePic;
      this.user.profilePic='';
      this.imageInput.nativeElement.value = '';
    }
    else{
      this.user.imageByte=this.imageByte2;
      this.user.profilePic = this.tempProfilePic;
      this.imageUrl='data:image/jpeg;base64,' + this.user.imageByte;
    }
  }

  loadUserDetail(loginId:string|null|undefined):void{
    this.authService.GetUserByLoginId(loginId).subscribe({
      next:(response)=>{
        if(response.success){
          this.user = response.data;
          this.imageUrl = 'data:image/jpeg;base64,' + this.user.imageByte;
          this.imageByte2 = this.user.imageByte;
          this.user.profilePic = response.data.profilePic;
        }else{
          console.error('Failed to fech users: ',response.message);
        }
      },
      error:(error)=>{
        console.error('Error fetching users: ',error);
      }
    })
  }

  onSubmit(userProfileUpdateTFForm:NgForm):void{
    if(userProfileUpdateTFForm.valid){
      this.loading = true;
      console.log(userProfileUpdateTFForm.value);
    }

      this.authService.updateUser(this.user).subscribe({
        next:(response)=>{
          if(response.success){
            this.authService.emitProfileUpdated(); // Emit profile updated event
            console.log(userProfileUpdateTFForm.value);
            this.router.navigate(['/searchContact']);
            alert('User profile updated successfully.');
          }else{
            alert(response.message);
          }
          this.loading = false;
        },
        error:(err)=>{
          console.log(err)
          console.log(err.error.message);
          this.loading = false;
          alert(err.error.message);
        },
        complete:()=>{
          this.loading = false;
          console.log("completed");
        }
      });
    }
}
