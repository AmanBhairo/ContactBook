import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { States } from 'src/app/models/state.model';
import { UpdateContact } from 'src/app/models/updateContact.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-update-contact',
  templateUrl: './update-contact.component.html',
  styleUrls: ['./update-contact.component.css']
})
export class UpdateContactComponent {
  @ViewChild('imageInput') imageInput!: ElementRef;
  contactId:number | undefined;
  countries : Country[]=[];
  states : States[]=[];

  imageUrl: string | ArrayBuffer | null = null;
  fileToUpload: File | null = null;
  fileName='';
  tempProfilePic:string='';
  
  

  contact:UpdateContact={
    contactId:0,
    countryId: 0,
    stateId: null,
    firstName: '',
    lastName: '',
    contactNumber: '',
    email: '',
    contactDescription: '',
    profilePic: '',
    gender: '',
    address: '',
    favourite: false,
    imageByte:'',
  }

  imageByte2: string ='';
  displayImageUrl:string | ArrayBuffer |null =null;
  loading: boolean =false;

  constructor(
    private contactServicec:ContactService,
    private countryService:CountryService,
    private stateServie:StateService, 
    private router:Router,
    private route:ActivatedRoute,
    private http: HttpClient) {}

  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.contactId = params['id'];
      this.loadContactDetail(this.contactId); 
      this.loadCountries();
      this.loadStates(this.contact.countryId);
    })
    
  }

  onFileSelected(event: any) {
    this.fileToUpload = event.target.files[0];
    if (this.fileToUpload) {
      const reader = new FileReader();
      reader.onload=()=>{
        this.contact.imageByte = (reader.result as string).split(',')[1]; 
        this.imageByte2 = this.contact.imageByte;
      }
      reader.readAsDataURL(this.fileToUpload);
      this.contact.profilePic = this.fileToUpload.name;
      this.tempProfilePic = this.contact.profilePic;
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

  loadContactDetail(contactId:number | undefined):void{
    this.contactServicec.GetContactById(contactId).subscribe({
      next:(response)=>{
        if(response.success){
          this.contact = response.data;
          this.loadStates(this.contact.countryId);
          this.imageUrl = 'data:image/jpeg;base64,' + this.contact.imageByte;
          this.imageByte2 = this.contact.imageByte;
          this.contact.profilePic = response.data.profilePic;
        }else{
          console.error('Failed to fech contacts:',response.message);
        }
      },
      error:(error)=>{
        console.error('Error fetching contacts: ',error);
      }
    })
  }

  loadCountries():void{
    this.loading = true;
    this.countryService.getAllCountries().subscribe({
      next:(response: ApiResponse<Country[]>)=>{
        if(response.success){
          this.countries = response.data;
        }else{
          console.error('Failed to fetch countries',response.message);
        }
        this.loading = false;
      },
      error:(error)=>{
        console.error('Error fetching countries: ',error);
        this.loading = false;
      },
      complete:()=>{
        this.loading = false;
        console.log("Completed");
      }
    });
  }

  loadStates(countryId:number):void{
    this.loading = true;
    this.stateServie.getStatesByCountryId(countryId).subscribe({
      next:(response: ApiResponse<States[]>)=>{
        if(response.success){
          this.states = response.data;
        }else{
          console.error('Failed to fetch states',response.message);
        }
        this.loading = false;
      },
      error:(error)=>{
        console.error('Error fetching states:',error);
        this.loading = false;
      }
    });
  }

  onCountryChange() {
    if (this.contact.countryId) {
      this.contact.stateId = null;
      this.states = [];
      this.loadStates(this.contact.countryId);
    } 
  }

  removeImage(event: any){
    if(event.target.checked){
      this.imageUrl=null;
      this.contact.imageByte='';
      this.tempProfilePic = this.contact.profilePic;
      this.contact.profilePic='';
      this.imageInput.nativeElement.value = '';
    }
    else{
      this.contact.imageByte=this.imageByte2;
      this.contact.profilePic = this.tempProfilePic;
      this.imageUrl='data:image/jpeg;base64,' + this.contact.imageByte;
    }
  }

  

  onSubmit(updateContactTFForm:NgForm):void{
    if(updateContactTFForm.valid){
      this.loading = true;
      console.log(updateContactTFForm.value);

      // if (this.fileToUpload) {
      //   const formData = new FormData();
      //   formData.append('file', this.fileToUpload, this.fileToUpload.name);
  
      //   this.http.post<any>('assets/', formData).subscribe(
      //     (response) => {
      //       console.log('Image uploaded successfully:', response);
      //       // Handle success response here
      //     },
      //     (error) => {
      //       console.error('Error occurred while uploading image:', error);
      //       // Handle error response here
      //     }
      //   );
      // } else {
      //   console.error('No image selected!');
      // }

      // let updateContact :UpdateContact ={
      //   contactId:updateContactTFForm.controls['contactId'].value,
      //   countryId: updateContactTFForm.controls['countryId'].value,
      //   stateId: updateContactTFForm.controls['stateId'].value,
      //   firstName: updateContactTFForm.controls['firstName'].value,
      //   lastName: updateContactTFForm.controls['lastName'].value,
      //   contactNumber: updateContactTFForm.controls['contactNumber'].value,
      //   email: updateContactTFForm.controls['email'].value,
      //   contactDescription: updateContactTFForm.controls['contactDescription'].value,
      //   profilePic: this.fileName,
      //   gender: updateContactTFForm.controls['gender'].value,
      //   address: updateContactTFForm.controls['address'].value,
      //   favourite: updateContactTFForm.controls['favourite'].value,
      //   imageByte: this.contact.imageByte,
      // };

      this.contactServicec.UpdateContact(this.contact).subscribe({
        next:(response)=>{
          if(response.success){
            console.log(updateContactTFForm.value);
            this.router.navigate(['/filteredContact']);
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
    }}
}
