import { ChangeDetectorRef, Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  isAuthenticated: boolean = false;
  userName:string|null|undefined;
  imageByte : string='';
  imageSrc = 'assets/DefaultImage.jpg';

  private profileUpdatedSubscription: Subscription | undefined;

  constructor(private authService:AuthService,private cdr:ChangeDetectorRef) {}
  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated = authState;
      if(this.isAuthenticated)
        {
        this.authService.getUserName().subscribe((username:string|null|undefined)=>{
          this.userName = username;
          if(this.userName)
            {
                this.loadUserProfileImage()
            }
          this.cdr.detectChanges();
        });

      }
      this.cdr.detectChanges();//Manually trigger change detection
    });
    
    this.loadUserProfileImage();
    

    // Subscribe to profile updated event
    this.profileUpdatedSubscription = this.authService.onProfileUpdated().subscribe(() => {
      this.loadUserProfileImage();
    });
  }

  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks
    if (this.profileUpdatedSubscription) {
      this.profileUpdatedSubscription.unsubscribe();
    }
  }



  loadUserProfileImage(): void {
    if (this.userName) {
      this.authService.GetUserByLoginId(this.userName).subscribe({
        next: (response) => {
          if (response.success) {
            this.imageByte = response.data.imageByte;
          } else {
            console.error('Failed to fetch User profile image: ', response.message);
          }
        },
        error: (error) => {
          console.error('Error fetching user profile image: ', error);
        }
      });
    }
  }

  signOut(){
    this.authService.signOut();
    this.imageByte = '';
    this.imageSrc = 'assets/DefaultImage.jpg';
  }
}
